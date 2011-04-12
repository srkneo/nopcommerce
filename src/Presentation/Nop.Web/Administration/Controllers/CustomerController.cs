﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Nop.Admin.Models;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Tax;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.ExportImport;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Security.Permissions;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;

namespace Nop.Admin.Controllers
{
    [AdminAuthorize]
    public class CustomerController : BaseNopController
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly TaxSettings _taxSettings;

        #endregion Fields

        #region Constructors

        public CustomerController(ICustomerService customerService, IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService, DateTimeSettings dateTimeSettings, 
            TaxSettings taxSettings)
        {
            this._customerService = customerService;
            this._dateTimeHelper = dateTimeHelper;
            this._localizationService = localizationService;
            this._dateTimeSettings = dateTimeSettings;
            this._taxSettings = taxSettings;
        }

        #endregion Constructors

        #region Utities

        [NonAction]
        private string GetCustomerRolesNames(IList<CustomerRole> customerRoles, string separator = ",")
        {
            var sb = new StringBuilder();
            for (int i = 0; i < customerRoles.Count; i++)
            {
                sb.Append(customerRoles[i].Name);
                if (i != customerRoles.Count - 1)
                {
                    sb.Append(separator);
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// Gets VAT Number status name
        /// </summary>
        /// <param name="status">VAT Number status</param>
        /// <returns>VAT Number status name</returns>
        [NonAction]
        private string GetVatNumberStatusName(VatNumberStatus status)
        {
            return _localizationService.GetResource(string.Format("Admin.Common.VatNumberStatus.{0}", status.ToString()));
        }

        #endregion

        #region Methods

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            //TODO add filtering by customer role, registration date
            var customers = _customerService.GetAllCustomers(null,null, 0, 10);
            var gridModel = new GridModel<CustomerModel>
            {
                Data = customers.Select(x =>
                {
                    var model = x.ToModel();
                    model.FullName = string.Format("{0} {1}", x.GetAttribute<string>(SystemCustomerAttributeNames.FirstName), x.GetAttribute<string>(SystemCustomerAttributeNames.LastName));
                    model.CustomerRoleNames = GetCustomerRolesNames(x.CustomerRoles.ToList());
                    model.CreatedOnStr = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc).ToString();
                    return model;
                }),
                Total = customers.TotalCount
            };
            return View(gridModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command)
        {
            var customers = _customerService.GetAllCustomers(null, null, command.Page - 1, command.PageSize);
            var gridModel = new GridModel<CustomerModel>
            {
                Data = customers.Select(x =>
                {
                    var model = x.ToModel();
                    model.FullName = string.Format("{0} {1}", x.GetAttribute<string>(SystemCustomerAttributeNames.FirstName), x.GetAttribute<string>(SystemCustomerAttributeNames.LastName));
                    model.CustomerRoleNames = GetCustomerRolesNames(x.CustomerRoles.ToList());
                    model.CreatedOnStr = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc).ToString();
                    return model;
                }),
                Total = customers.TotalCount
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            var model = new CustomerModel();
            model.AllowCustomersToSetTimeZone = _dateTimeSettings.AllowCustomersToSetTimeZone;
            foreach (var tzi in _dateTimeHelper.GetSystemTimeZones())
                model.AvailableTimeZones.Add(new SelectListItem() { Text = tzi.DisplayName, Value = tzi.Id, Selected = (tzi.Id == _dateTimeHelper.DefaultStoreTimeZone.Id) });
            model.DisplayVatNumber = false;
            model.VatNumberStatusNote = GetVatNumberStatusName(VatNumberStatus.Empty);

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CustomerModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customer = model.ToEntity();
            customer.CustomerGuid = Guid.NewGuid();
            customer.CreatedOnUtc = DateTime.UtcNow;
            _customerService.InsertCustomer(customer);
            _customerService.SaveCustomerAttribute(customer, SystemCustomerAttributeNames.Gender, model.Gender);
            _customerService.SaveCustomerAttribute(customer, SystemCustomerAttributeNames.FirstName, model.FirstName);
            _customerService.SaveCustomerAttribute(customer, SystemCustomerAttributeNames.LastName, model.LastName);
            _customerService.SaveCustomerAttribute(customer, SystemCustomerAttributeNames.DateOfBirth, model.DateOfBirth);
            
            return RedirectToAction("Edit", new { id = customer.Id });
        }

        public ActionResult Edit(int id)
        {
            var customer = _customerService.GetCustomerById(id);
            if (customer == null) 
                throw new ArgumentException("No customer found with the specified id", "id");

            var model = customer.ToModel();
            model.AllowCustomersToSetTimeZone = _dateTimeSettings.AllowCustomersToSetTimeZone;
            foreach (var tzi in _dateTimeHelper.GetSystemTimeZones())
                model.AvailableTimeZones.Add(new SelectListItem() { Text = tzi.DisplayName, Value = tzi.Id, Selected = (tzi.Id == customer.TimeZoneId) });
            model.DisplayVatNumber = _taxSettings.EuVatEnabled;
            model.VatNumberStatusNote = GetVatNumberStatusName(customer.VatNumberStatus);
            model.FirstName = customer.GetAttribute<string>(SystemCustomerAttributeNames.FirstName);
            model.LastName = customer.GetAttribute<string>(SystemCustomerAttributeNames.LastName);
            model.Gender = customer.GetAttribute<string>(SystemCustomerAttributeNames.Gender);
            model.DateOfBirth = customer.GetAttribute<DateTime?>(SystemCustomerAttributeNames.DateOfBirth);
            model.CreatedOnStr = _dateTimeHelper.ConvertToUserTime(customer.CreatedOnUtc, DateTimeKind.Utc).ToString();

            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(CustomerModel model, bool continueEditing)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var customer = _customerService.GetCustomerById(model.Id);
            if (customer == null) 
                throw new ArgumentException("No customer found with the specified id", "id");

            customer = model.ToEntity(customer);

            //UNDONE set VAT number after country is saved
            //TODO get country from default address (can this be "hacked" during checkout? for example, selecting a new address with new country)
            //if (_taxSettings.EuVatEnabled)
            //{
            //    string prevVatNumber = customer.VatNumber;
            //    customer.VatNumber = txtVatNumber.Text;
            //    //set VAT number status
            //    if (!txtVatNumber.Text.Trim().Equals(prevVatNumber))
            //        customer.VatNumberStatus = _taxService.GetVatNumberStatus(_countryService.GetCountryById(_countryId), customer.VatNumber);
            //}

            _customerService.UpdateCustomer(customer);
            _customerService.SaveCustomerAttribute(customer, SystemCustomerAttributeNames.Gender, model.Gender);
            _customerService.SaveCustomerAttribute(customer, SystemCustomerAttributeNames.FirstName, model.FirstName);
            _customerService.SaveCustomerAttribute(customer, SystemCustomerAttributeNames.LastName, model.LastName);
            _customerService.SaveCustomerAttribute(customer, SystemCustomerAttributeNames.DateOfBirth, model.DateOfBirth);


            return continueEditing ? RedirectToAction("Edit", customer.Id) : RedirectToAction("List");
        }
        
        [HttpPost, ActionName("Edit")]
        [FormValueRequired("markVatNumberAsValid")]
        public ActionResult MarkVatNumberAsValid(CustomerModel model)
        {
            var customer = _customerService.GetCustomerById(model.Id);
            if (customer == null)
                throw new ArgumentException("No customer found with the specified id", "id");

            customer.VatNumberStatus = VatNumberStatus.Valid;
            _customerService.UpdateCustomer(customer);

            return RedirectToAction("Edit", customer.Id);
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("markVatNumberAsInvalid")]
        public ActionResult MarkVatNumberAsInvalid(CustomerModel model)
        {
            var customer = _customerService.GetCustomerById(model.Id);
            if (customer == null)
                throw new ArgumentException("No customer found with the specified id", "id");

            customer.VatNumberStatus = VatNumberStatus.Invalid;
            _customerService.UpdateCustomer(customer);

            return RedirectToAction("Edit", customer.Id);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var customer = _customerService.GetCustomerById(id);
            if (customer == null) 
                throw new ArgumentException("No customer found with the specified id", "id");

            _customerService.DeleteCustomer(customer);
            return RedirectToAction("List");
        }
        
        #endregion
    }
}