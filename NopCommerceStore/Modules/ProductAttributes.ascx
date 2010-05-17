<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.ProductAttributesControl" Codebehind="ProductAttributes.ascx.cs" %>

<% if(SettingManager.GetSettingValueBoolean("ProductAttribute.EnableDynamicPriceUpdate")) { %>
<script type="text/javascript">
    // Adjustment table
    var adjustmentTable = new Array();
    // Adjustment table initialize
    <asp:Literal runat="server" ID="lblAdjustmentTableScripts" />
    // Adjustment function
    function adjustPrice() {
        var sum = 0;
        for(var i in adjustmentTable) {
            var ctrl = $('#' + i);
            if((ctrl.is(':radio') && ctrl.is(':checked')) || (ctrl.is(':checkbox') && ctrl.is(':checked'))) {
                sum += adjustmentTable[i];
            }
            else if(ctrl.is('select')) {
                var idx = $('#' + i + " option").index($('#' + i + " option:selected"));
                if(idx != -1) {
                    sum += adjustmentTable[i][idx];
                }
            }
        }
        var res = (priceValForDynUpd + sum).toFixed(2);
        $(".price-val-for-dyn-upd").text(res);
    }
    // Attributes handlers
    $(document).ready(function() {
        adjustPrice();
        <asp:Literal runat="server" ID="lblAttributeScripts" />
    });
</script>
<%} %>
<asp:PlaceHolder runat="server" ID="phAttributes" />
