<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.NewBikeBooking.ManageDealersCampaigns" %>
<!-- #Include file="/includes/headerNew.aspx" -->
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
            <span>Select Manufacture : <font color="red">* &nbsp</font>
           <asp:DropDownList id="ddlManufacturers" runat="server">                    
                </asp:DropDownList>
            <input type="button" data-bind="click: SearchCampaigns" id="SearchCampaigns" value="Search Campaigns" style="padding:10px; margin-left:20px; cursor:pointer;"/>
                
           <input type="button" data-bind="click: redirect" value="Add Campaigns"  style="padding:10px; margin-left:20px; cursor:pointer;"/>
        </div> 
        </div>
<div style="margin-left:200px">
        <h3 id="selDealerHeading"></h3>
    </div>
 <div id="mfgtable">
        <table class="margin-top10 margin-bottom10" rules="all" cellspacing="0" cellpadding="5" style="border-width: 1px; border-style: solid; width: 80%; border-collapse: collapse;" id="DealerCampaignsList">
                <tr class="dtHeader">
                    <td>Sr No.</td>
                    <td>Campaign Id</td>
                    <td>Campaign Description</td>
                    <td>Campaign  Active</td>
                     <td>Rules</td>
                     <td>Edit Campaign</td>
                    <td>Action</td>

                </tr>
        <tbody data-bind="template: { name: 'DealerCampaignList', foreach: ManufacturersCampaign }">
            </tbody>
                    </table>
       </div>    
          <script type="text/html" id="DealerCampaignList">
        <tr class="dtItem">
            <td data-bind="text: $index() + 1"></td>
            <td data-bind="text: id"></td>
            <td data-bind="text: description"></td>
            <td data-bind="text: isactive"></td>
               <td >
                <a  data-bind="attr: { href: '/campaign/DealersRules.aspx?campaignid=' + id + '&dealerid=' + dealerid }" target="_blank">Edit Rules</a>
            </td>
            <td >
             
                <a  data-bind="attr: { href: '/campaign/ManageDealer.aspx?campaignid=' + id + '&dealerid=' + dealerid + '&dealerHeading='+encodeURIComponent(dealerHeading)}"  target="_blank"><img src="http://opr.carwale.com/images/edit.jpg" alt="Edit"/></a>
            </td>
            
            <td><input  type="button"  data-bind="event: { click: function (data, event) { $parent.ChangeStatus(data, event); } }, value: (isactive ? 'Stop' : 'Start'), style: { color: isactive > 0 ? 'red' : 'green' }"  style="padding:10px; margin-left:20px; cursor:pointer;"/></td>
             
        </tr>
    </script>    
 
<script>
    var ddlManufacturers = $("#ddlManufacturers");
    var BwOprHostUrl = '<%=ConfigurationManager.AppSettings["BwOprHostUrlForJs"]%>';
    $('#mfgtable').hide();
    var mfgCamp = function () {
        var self = this;
        self.selectedManufacturer = ko.observable();
        self.Manufacturers = ko.observableArray([]);
        self.ManufacturersCampaign = ko.observableArray([]);
        var msg = $("#selDealerHeading");
       
        self.redirect = function () {
            dId = $("#ddlManufacturers option:selected");
            dealerId = dId.val();
            dealerHeading = dId.text();
            if (!isNaN(dealerId) && dealerId > 0) {
                var url = BwOprHostUrl + '/campaign/ManageDealer.aspx?dealerHeading=' + encodeURIComponent(dealerHeading) + '&dealerid=' + dealerId;
                window.location.href = url;
            }
            else {
                alert("Please select Dealer");
            }
        }
        self.SearchCampaigns = function () {
            debugger
            dId = $("#ddlManufacturers option:selected");
            dealerId = dId.val();
            dealerHeading = dId.text();
            if (!isNaN(dealerId) && dealerId > 0) {
                var element = document.getElementById('DealerCampaignsList');
                $.ajax({
                    type: "POST",
                    url: BwOprHostUrl + "/api/ManufacturerCampaign/GetManufactureCampaigns/?dealerId=" + dealerId,
                    datatype: "json",
                    success: function (response) {
                        ko.cleanNode(element);

                        if (response.length > 0) {
                            ko.applyBindings(ko.mapping.fromJS(response, {}, self), element);
                            self.ManufacturersCampaign(response);
                            $('#mfgtable').show();
                        }
                        else {
                            $('#mfgtable').hide();
                            msg.text("Campaigns for " + dealerHeading + " not available ");
                        }
                    },
                    complete: function (xhr) {
                        if (xhr.status != 200) {
                            alert("Something went wrong .Please try again !!");
                        }
                    }
                });
            } else {
                alert("Please select Dealer");
            }

        }

        self.ChangeStatus = function (data, event) {
            if (data.isactive == 1)
                data.isactive = 0;
            else
                data.isactive = 1;
         $.ajax({
                type: "POST",
                url: BwOprHostUrl + "/api/ManufacturerCampaign/GetstatuschangeCampaigns/?id=" + data.id + '&isactive=' + data.isactive,
                datatype: "json",
                success: function (response) {
                    self.SearchCampaigns();
                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        alert("Something went wrong .Please try again !!")
                    }
                }
            });

        };
 }
    var viewModel = new mfgCamp();
    ko.applyBindings(viewModel, $("#mfgCampaigns")[0]);

</script>

<!-- #Include file="/includes/footerNew.aspx" -->
