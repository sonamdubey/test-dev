<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.ManufactureCampaign.ManageDealersCampaigns" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<div>
<!-- #Include file="/content/DealerMenu.aspx" -->
</div>
<div class="left min-height600">
    <div id="mfgCampaigns">
        <h3> Manage Manufacturer's Campaigns</h3>      
        <div class="margin-top10">
            <span>Select Manufacture : </span><span class="errorMessage">*</span>
            <asp:DropDownList id="ddlManufacturers" runat="server" class="margin-left10" />
            <input type="button" data-bind="click: SearchCampaigns" id="SearchCampaigns" value="Search Campaigns" class="margin-left20"/>                
            <input type="button" data-bind="click: redirect" value="Add Campaigns" class="margin-left20"/>
        </div> 
    </div>    
    <div id="msgBox" class="errorMessage margin-top10"></div>
    <div id="tblCampaigns">
        <table class="margin-top10 margin-bottom10" rules="all" cellspacing="0" cellpadding="5" style="border-width: 1px; border-style: solid; width: 80%; border-collapse: collapse;" id="DealerCampaignsList">
            <tr class="dtHeader">
                <td>Sr No.</td>
                <td>Campaign Id</td>
                <td>Campaign Description</td>
                <td>Campaign  Status</td>
                <td>Edit Rules</td>
                <td>Edit Campaign</td>
                <td>Start/Stop Campaign</td>
            </tr>
            <tbody data-bind="template: { name: 'DealerCampaignList', foreach: ManufacturersCampaign }"></tbody>
        </table>
    </div>
</div>  
<script type="text/html" id="DealerCampaignList">
    <tr class="dtItem text-align-center">
        <td data-bind="text: $index() + 1"></td>
        <td data-bind="text: id"></td>
        <td data-bind="text: description"></td>
        <td data-bind="text: isactive ? 'Active' : 'In-Acitve'"></td>
        <td><a  data-bind="attr: { href: '/ManufactureCamapign/ManufacturerCampaignRules.aspx?campaignid=' + id + '&dealerid=' + dealerid }" target="_blank"><img src="http://opr.carwale.com/images/edit.jpg" alt="Edit"/></a></td>
        <td>
            <a  data-bind="attr: { href: '/ManufactureCamapign/ManageDealer.aspx?campaignid=' + id + '&dealerid=' + dealerid + '&manufactureName='+encodeURIComponent(manufactureName)}"  target="_blank"><img src="http://opr.carwale.com/images/edit.jpg" alt="Edit"/></a>
        </td>            
        <td><input  type="button"  data-bind="event: { click: function (data, event) { $parent.ChangeStatus(data, event); } }, value: (isactive ? 'Stop' : 'Start')" class="margin-left20"/></td>
             
    </tr>
</script>    
 
<script>
    var ddlManufacturers = $("#ddlManufacturers");
    var BwOprHostUrl = '<%=ConfigurationManager.AppSettings["BwOprHostUrlForJs"]%>';
    $('#tblCampaigns').hide();
    var msgBox = $("#msgBox");
    var mfgCamp = function () {
        var self = this;
        self.selectedManufacturer = ko.observable();
        self.Manufacturers = ko.observableArray([]);
        self.ManufacturersCampaign = ko.observableArray([]);
        
        self.redirect = function () {
           dealerId = ddlManufacturers.val();
           manufactureName = ddlManufacturers.find("option:selected").text();
            if (!isNaN(dealerId) && dealerId > 0) {
                var url = BwOprHostUrl + '/campaign/ManageDealer.aspx?manufactureName=' + encodeURIComponent(manufactureName) + '&dealerid=' + dealerId;
                window.location.href = url;
            }
            else {
                alert("Please select manufacturer");
            }
        }
        self.SearchCampaigns = function () {
            dealerId = ddlManufacturers.val();
            manufactureName = ddlManufacturers.find("option:selected").text();
        if (!isNaN(dealerId) && dealerId > 0) {
                var element = document.getElementById('DealerCampaignsList');
                $.ajax({
                    type: "GET",
                    url: BwOprHostUrl + "/api/campaigns/manufacturer/search/dealerId/" + dealerId,
                    datatype: "json",
                    success: function (response) {
                        ko.cleanNode(element);

                        if (response.length > 0) {
                            ko.applyBindings(ko.mapping.fromJS(response, {}, self), element);
                            self.ManufacturersCampaign(response);
                            $('#tblCampaigns').show();
                        }
                        else {
                            $('#tblCampaigns').hide();
                            msgBox.text("Campaigns for " + manufactureName + " are not available ");
                        }
                    },
                    complete: function (xhr) {
                        if (xhr.status != 200) {
                            alert("Something went wrong .Please try again !!");
                        }
                       
                    }
                });
            } else {
                alert("Please select manufacturer");
            }

        }

        self.ChangeStatus = function (data, event) {
            var r = confirm("Arey you Sure you want to " + (data.isactive ? 'In-Active' : 'Activate')+" Manufacture Campaign");
            if (r == true) {
                if (data.isactive == 1)
                    data.isactive = false;
                else
                    data.isactive = true;
                $.ajax({
                    type: "POST",
                    url: BwOprHostUrl + "/api/campaigns/manufacturer/updatecampaignstatus/campaignId/" + data.id + '/status/' + data.isactive,
                    datatype: "json",
                    success: function (response) {
                        self.SearchCampaigns();
                    },
                    complete: function (xhr) {
                        if (xhr.status != 200) {
                            alert("Something went wrong .Please try again !!")
                        }
                        else {
                            msgBox.text("Campaign for " + manufactureName + " has been Updated");
                        }
                    }
                });
            }

        };
 }
    var viewModel = new mfgCamp();
    ko.applyBindings(viewModel, $("#mfgCampaigns")[0]);
</script>
<!-- #Include file="/includes/footerNew.aspx" -->
