using System;
using NopSolutions.NopCommerce.BusinessLogic.Profile;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class DatePicker : BaseNopAdministrationUserControl
    {
        #region Properties
        public bool ShowTime
        {
            get
            {
                return Convert.ToBoolean(ViewState["ShowTime"]);
            }
            set
            {
                ViewState["ShowTime"] = value;
            }
        }

        public DateTime? SelectedDate
        {
            get
            {
                DateTime inptDate;
                if(!DateTime.TryParse(txtDateTime.Text, out inptDate))
                {
                    return null;
                }
                return inptDate;
            }
            set
            {
                ajaxCalendar.SelectedDate = value;
            }
        }

        public string Format
        {
            get
            {
                return ajaxCalendar.Format;
            }
            set
            {
                ajaxCalendar.Format = value;
            }
        }
        #endregion
    }
}