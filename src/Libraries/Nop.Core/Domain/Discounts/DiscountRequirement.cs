﻿
using System.Collections.Generic;
using Nop.Core.Domain.Customers;

namespace Nop.Core.Domain.Discounts
{
    /// <summary>
    /// Represents a discount requirement
    /// </summary>
    public partial class DiscountRequirement : BaseEntity
    {
        /// <summary>
        /// Gets or sets the discount identifier
        /// </summary>
        public int DiscountId { get; set; }
        
        /// <summary>
        /// Gets or sets the discount requirement rule system name
        /// </summary>
        public string DiscountRequirementRuleSystemName { get; set; }

        /// <summary>
        /// Gets or sets the the discount requirement spent amount - customer had spent/purchased x.xx amount (used when requirement is set to "Customer had spent/purchased x.xx amount")
        /// </summary>
        public decimal SpentAmount { get; set; }

        /// <summary>
        /// Gets or sets the discount requirement - customer's billing country is... (used when requirement is set to "Billing country is")
        /// </summary>
        public int BillingCountryId { get; set; }

        /// <summary>
        /// Gets or sets the discount requirement - customer's shipping country is... (used when requirement is set to "Shipping country is")
        /// </summary>
        public int ShippingCountryId { get; set; }
        
        /// <summary>
        /// Gets or sets the discount
        /// </summary>
        public virtual Discount Discount { get; set; }
        
        /// <summary>
        /// Gets or sets the customer roles
        /// </summary>
        public virtual ICollection<CustomerRole> RestrictedToCustomerRoles { get; set; }

    }
}
