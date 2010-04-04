﻿//------------------------------------------------------------------------------
// The contents of this file are subject to the nopCommerce Public License Version 1.0 ("License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at  http://www.nopCommerce.com/License.aspx. 
// 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. 
// See the License for the specific language governing rights and limitations under the License.
// 
// The Original Code is nopCommerce.
// The Initial Developer of the Original Code is NopSolutions.
// All Rights Reserved.
// 
// Contributor(s): _______. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Measures;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class MeasuresControl : BaseNopAdministrationUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindDimensions();
                BindWeights();
            }
        }

        private void BindDimensions()
        {
            MeasureDimensionCollection dimensions = MeasureManager.GetAllMeasureDimensions();
            gvDimensions.DataSource = dimensions;
            gvDimensions.DataBind();
        }

        private void BindWeights()
        {
            MeasureWeightCollection weights = MeasureManager.GetAllMeasureWeights();
            gvWeights.DataSource = weights;
            gvWeights.DataBind();
        }

        protected void btnAddDimension_Click(object sender, EventArgs e)
        {
            Response.Redirect("MeasureDimensionAdd.aspx");
        }

        protected void btnAddWeight_Click(object sender, EventArgs e)
        {
            Response.Redirect("MeasureWeightAdd.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //dimensions
                foreach (GridViewRow row in gvDimensions.Rows)
                {
                    RadioButton rdbIsPrimaryDimension = (RadioButton)row.FindControl("rdbIsPrimaryDimension");
                    HiddenField hfMeasureDimensionID = (HiddenField)row.FindControl("hfMeasureDimensionID");
                    int measureDimensionID = int.Parse(hfMeasureDimensionID.Value);
                    if (rdbIsPrimaryDimension.Checked)
                        MeasureManager.BaseDimensionIn = MeasureManager.GetMeasureDimensionByID(measureDimensionID);
                }

                //weights
                foreach (GridViewRow row in gvWeights.Rows)
                {
                    RadioButton rdbIsPrimaryWeight = (RadioButton)row.FindControl("rdbIsPrimaryWeight");
                    HiddenField hfMeasureWeightID = (HiddenField)row.FindControl("hfMeasureWeightID");
                    int measureWeightID = int.Parse(hfMeasureWeightID.Value);
                    if (rdbIsPrimaryWeight.Checked)
                        MeasureManager.BaseWeightIn = MeasureManager.GetMeasureWeightByID(measureWeightID);
                }

                BindDimensions();
                BindWeights();
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }
    }
}