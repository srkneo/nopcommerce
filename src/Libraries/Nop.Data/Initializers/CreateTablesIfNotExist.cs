﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;

namespace Nop.Data.Initializers
{
    public class CreateTablesIfNotExist<TContext> : IDatabaseInitializer<TContext> where TContext : DbContext
    {
        private readonly string[] _tablesToValidate;
        private readonly string[] _customCommands;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="tablesToValidate">A list of existing table names to validate; null to don't validate table names</param>
        /// <param name="customCommands">A list of custom commands to execute</param>
        public CreateTablesIfNotExist(string[] tablesToValidate, string [] customCommands)
            : base()
        {
            this._tablesToValidate = tablesToValidate;
            this._customCommands = customCommands;
        }
        public void InitializeDatabase(TContext context)
        {
            bool dbExists;
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                dbExists = context.Database.Exists();
            }
            if (dbExists)
            {
                bool createTables = false;
                if (_tablesToValidate != null && _tablesToValidate.Length > 0)
                {
                    //we have some table names to validate
                    var existingTableNames = new List<string>(context.Database.SqlQuery<string>("SELECT table_name FROM INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE'"));
                    createTables = existingTableNames.Intersect(_tablesToValidate, StringComparer.InvariantCultureIgnoreCase).Count() == 0;
                }
                else
                {
                    //check whether tables are already created
                    int numberOfTables = 0;
                    foreach (var t1 in context.Database.SqlQuery<int>("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE' "))
                        numberOfTables = t1;

                    createTables = numberOfTables == 0;
                }

                if (createTables)
                {
                    //create all tables
                    var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();

                    //Need to fix some of the script for MySql
                    if (context.Database.Connection.GetType() == typeof(MySql.Data.MySqlClient.MySqlConnection))
                    {
                        //MySql doesn't support varbinary(MAX) so it generates the script with varbinary only without
                        //a size specified, so change to longblob.
                        dbCreationScript = dbCreationScript.Replace("`PictureBinary` varbinary,", "`PictureBinary` LONGBLOB,");
                        dbCreationScript = dbCreationScript.Replace("`DownloadBinary` varbinary,", "`DownloadBinary` LONGBLOB,");

                        //Order is a keyword so need to put in quotes
                        dbCreationScript = dbCreationScript.Replace("REFERENCES Order (Id);", "REFERENCES `Order` (Id);");

                        //Some of the constraint names are too long, so shorten them
                        dbCreationScript = dbCreationScript.Replace("ProductReview_TypeConstraint_From_CustomerContent_To_ProductReview", "ProductReview_CustomerContent_ProductReview");
                        dbCreationScript = dbCreationScript.Replace("PollVotingRecord_TypeConstraint_From_CustomerContent_To_PollVotingRecord", "PollVotingRecord_CustomerContent_PollVotingRecord");
                        dbCreationScript = dbCreationScript.Replace("ProductReviewHelpfulness_TypeConstraint_From_CustomerContent_To_ProductReviewHelpfulness", "ProductReviewHelpfulnes_CustomerContent_ProductReviewHelpfulnes");
                    }

                    context.Database.ExecuteSqlCommand(dbCreationScript);

                    //Seed(context);
                    context.SaveChanges();

                    if (_customCommands != null && _customCommands.Length > 0)
                    {
                        foreach (var command in _customCommands)
                            context.Database.ExecuteSqlCommand(command);
                    }
                }
            }
            else
            {
                throw new ApplicationException("No database instance");
            }
        }
    }
}
