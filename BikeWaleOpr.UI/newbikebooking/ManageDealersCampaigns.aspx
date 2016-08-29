<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageDealerCampaigns.aspx.cs" Inherits="BikewaleOpr.NewBikeBooking.ManageDealerCampaigns" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<%--<script type="text/javascript" src="/src/common/common.js?V1.1"></script>--%>
<script type="text/ecmascript" src="/src/AjaxFunctions.js"></script>
<script src="/src/knockout.js" type="text/javascript"></script>
<style>
    .dtItem {
        border-bottom: 1px solid #808080;
    }

    select {
        padding: 10px;
        cursor: pointer;
        vertical-align: top;
    }

    .footer {
        margin-top: 20px;
    }

    .top_info_left {
        text-transform: capitalize;
    }

    .dtItem {
        font-size: larger;
    }
</style>
    <div>
    <!-- #Include file="/content/DealerMenu.aspx" -->
    </div>
    <div id="mfgCampaigns" style="padding-left:20px;">
        <h3> Manage Manufacturer's Campaigns</h3>
      
        <div class="margin-top10">
            <span>Manufacture Campaigns : <font color="red">* &nbsp</font>
            <select id="ddlManufacturers" data-bind="options: Manufacturers, optionsValue: 'Id', optionsText: 'Organization', value: selectedManufacturer, optionsCaption: 'Choose manufacturer...'" ></select>
            <input type="button" id="SearchCampaigns" value="SearchCampaigns" style="padding:10px; margin-left:20px; cursor:pointer;"/>
            <input type="button" id="AddCampaigns"  value="AddCampaigns" style="padding:10px; margin-left:20px; cursor:pointer;"/>
        </div> 
        </div>
 <div id="t1">
        <table class="margin-top10 margin-bottom10" rules="all" cellspacing="0" cellpadding="5" style="border-width: 1px; border-style: solid; width: 100%; border-collapse: collapse;" id="DealerCampaignsList">
                <tr class="dtHeader">
                    <td>Sr No.</td>
                    <td>campaign Id</td>
                    <td>campaign description</td>
                    <td>campaign  active</td>
                     <td>Rules</td>
                     <td>Edit Campaign</td>

                </tr>
         
             </thead>
            <tbody data-bind="template: { name: 'DealerCampaignList', foreach: Table }">
            </tbody>
                    </table>
       </div>    
          <script type="text/html" id="DealerCampaignList">
        <%--<tr class="dtItem" data-bind="style: { 'background-color': (ContractStatus == 1) ? '#32cd32' : '#fffacd' }">--%>
        <tr class="dtItem">
            <td data-bind="text: $index() + 1"></td>
            <td data-bind="text: dealerId"></td>
            <td data-bind="text: description"></td>
            <td data-bind="text: isactive"></td>
            
        </tr>
    </script>    
 
<script>
  var  ddlManufacturers = $("#ddlManufacturers");

    var d = new Date().toJSON().slice(0, 10);

 

    var mfgCamp = function () {
        debugger;
        var self = this;
        $('#t1').hide();
        self.selectedManufacturer = ko.observable();
        self.Manufacturers = ko.observableArray([]);
      

        var DealerViewModel = function (model) {
            ko.mapping.fromJS(model, {}, this);
        };

        function startLoading(ele) {
            try {
                var _self = $(ele).find(".progress-bar").css({ 'width': '0' }).show();
                _self.animate({ width: '100%' }, 5000);
            }
            catch (e) { return };
        }
        $("#SearchCampaigns").click(function () {
            debugger;

            dId = $("#ddlManufacturers option:selected");
            dealerId = dId.val();
            dealerHeading = dId.text();

            if (!isNaN(dealerId) && dealerId != "") {
                var element = document.getElementById('DealerCampaignsList');
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"dealerId":"' + dealerId + '"}',
                    beforeSend: function (xhr) {
                        startLoading($("#mfgCampaigns"));
                        xhr.setRequestHeader("X-AjaxPro-Method", "GetManufactureCampaigns");
                    },
                    datatype: "json",
                    success: function (response) {
                        debugger;
                        ko.cleanNode(element);
                        var responseJSON = eval('(' + response + ')');
                        if (responseJSON.value != "") {
                            response = eval('(' + responseJSON.value + ')');
                            if (response != null && response.Table != null) {
                               
                                ko.applyBindings(new DealerViewModel(response), element);
                                $('#t1').show();
                            }
                            else {

                                msg.text("Campaigns for " + dealerHeading + " not available ");
                            }
                        }
                        else {

                            msg.text("Campaigns for " + dealerHeading + " not available ");
                        }

                    },
                    complete: function (xhr) {
                        if (xhr.status != 200) {
                            alert("Something went wrong .Please try again !!")
                        }
                    }
                });
            } else {
                alert("Please select Dealer");
            }

        });
        }


        var viewModel = new mfgCamp();
        ko.applyBindings(viewModel, $("#mfgCampaigns")[0]);
        function fillManuFacturers() {
            //modelIds = str;
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikewaleOpr.Common.AjaxManufacturerCampaign,BikewaleOpr.ashx",
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetDealerAsManuFacturer"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    //bindDropDownList(resObj, ddlManufacturers, "", "--Select Manufacturer--");
                    viewModel.Manufacturers(ko.toJS(resObj));
                },
                complete: function (xhr) {
                    if (xhr.status == 404 || xhr.status == 204) {
                        alert("Error Occurred");
                    }
                }
            });
        }
    
    fillManuFacturers();
</script>

<!-- #Include file="/includes/footerNew.aspx" -->
