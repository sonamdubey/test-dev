﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.campaign.SearchDealerCampaigns" Async="true" AsyncTimeout="60" %>

<!-- #Include file="/includes/headerNew.aspx" -->
<style type="text/css">
    .greenMessage {
        color: #6B8E23;
        font-size: 11px;
    }

    .redmsg {
        color: #FFCECE;
    }

    .errMessage {
        color: #FF4A4A;
    }

    .valign {
        vertical-align: top;
    }

    .progress-bar {
        width: 0;
        display: none;
        height: 2px;
        background: #16A085;
        bottom: 0px;
        left: 0;
        border-radius: 2px;
    }

    .position-abt {
        position: absolute;
    }

    .position-rel {
        position: relative;
    }

    .active-contract {
        background-color: #ccc;
    }

    .unstarted-contract {
        background-color: #ccc;
    }
</style>
<div class="left">
    <h1>Manage Dealer Campaigns</h1>
    <div id="inputSection" class="position-rel margin-top10">
        <div style="border: 1px solid #777;" class="padding10">
            <div class="margin-right10 verical-middle form-control-box">
                Dealer's City : 
                <div class="materialize-select">
                    <asp:dropdownlist id="drpCity" enabled="True" cssclass="drpClass" runat="server">
                    <asp:ListItem Text="--Select City--" Value="-1"/>
                    </asp:dropdownlist>
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please Select City</div>
                    <span class="caret">▼</span>
                </div>
            </div>
            <div class="margin-right10 verical-middle form-control-box position-rel" id="dvMakes">
                Bike Make : 
                <div class="materialize-select">
                    <select id="ddlMakes" class="drpClass">
                        <option value="">--Select Make--</option>
                    </select>
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please Select Make</div>
                    <span class="caret">▼</span>
                </div>
            </div>
            <div class="margin-right10 verical-middle form-control-box position-rel" id="dvDealers">
                Dealer Name : 
                <div class="materialize-select">
                    <select id="drpDealer" class="drpClass">
                        <option value="">--Select Dealer--</option>
                    </select>
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please Select Dealer</div>
                    <span class="caret">▼</span>
                </div>
            </div>
            <div class="margin-right10 verical-middle">
                <input class="verical-middle" type="checkbox" id="chkActiveCampaign" checked />
                <label for="chkActiveCampaign" class="verical-middle">Show only active campaigns</label>
            </div>
            <div class="verical-middle">
                <input id="btnGetCampaigns" type="button" value="Get Campaigns" />
            </div>
        </div>
        <span class="position-abt progress-bar" style="width: 100%; overflow: hidden;"></span>
    </div>

    <div style="margin-left: 200px">
        <h4 id="selDealerHeading"></h4>
    </div>
    <div style="display: none; overflow-x: auto; overflow-y: hidden" id="DealerCampaignsList">

        <table class="margin-top10 margin-bottom10" rules="all" cellspacing="0" cellpadding="5" style="border-width: 1px; border-style: solid; width: 100%; border-collapse: collapse;">
            <thead>
                <tr class="dtHeader">
                    <td>Sr No.</td>
                    <td>Contract Id</td>
                    <td>Package Name</td>
                    <td>Contract Start Date</td>
                    <td>Contract End Date</td>
                    <td>Campaign Name</td>
                    <td>Campaign EmailId</td>
                    <td>Campaign Serving Status</td>
                    <td>LeadServingRadius (kms)</td>
                    <td>Masking Number</td>
                    <td>Contract Status</td>                    
                    <td>Edit Campaign</td>
                    <td>Campaign Models</td>
                    <td>Campaign Areas</td>
                </tr>
            </thead>
            <tbody data-bind="template: { name: 'DealerCampaignList', foreach: Table }">
            </tbody>
        </table>
    </div>
    <script type="text/html" id="DealerCampaignList">
        <tr class="dtItem text-align-center" data-bind="style: { 'background-color': ColorCode }">
            <td data-bind="text: $index() + 1"></td>
            <td data-bind="text: ContractId"></td>
            <td data-bind="text: PackageName"></td>
            <td data-bind="text: StartDate"></td>
            <td data-bind="text: EndDate"></td>
            <td data-bind="text: CampaignName"></td>
            <td data-bind="text: EmailId"></td>
            <td data-bind="text: CampaignServingStatus"></td>
            <td data-bind="text: ServingRadius"></td>
            <td data-bind="text: MaskingNumber"></td>
            <td data-bind="text: Status"></td>            
            <td>
                <a data-bind="attr: { href: '/campaign/ManageDealers.aspx?dealername=' + DealerName() + '&contractid=' + ContractId() + '&campaignid=' + CampaignId() + '&dealerid=' + DealerId() }" target="_blank" rel="noopener">
                    <img src="https://opr.carwale.com/images/edit.jpg" alt="Edit" />
                </a>
            </td>
            <td >
                <a data-bind="attr: { href: '/campaign/DealersRules.aspx?campaignid=' + CampaignId() + '&dealerid=' + DealerId() + '&dealerName=' + DealerName() }, text: (NoOfRules() > 0) ? 'Yes' : 'No'" target="_blank" rel="noopener"></a>
            </td>
            <td>
                <a  data-bind="attr: { href: '/dealercampaign/servingareas/dealerid/' + DealerId() + '/campaignid/' + CampaignId() + '/' }" target="_blank" rel="noopener">
                    <img src="https://opr.carwale.com/images/edit.jpg" alt="Edit"/>
                </a>
            </td>
        </tr>
    </script>
</div>
<script type="text/javascript" src="https://stb.aeplcdn.com/bikewale/src/common/chosen.jquery.min.js?v15416"></script>
<script>
    var BwOprHostUrl = '<%= BwOprHostUrl%>';
    var ddlDealer = $("#drpDealer");
    var ddlMakes = $("#ddlMakes");
    var ddlCity = $("#drpCity");
    var chkActiveCampaign = $("#chkActiveCampaign");
    var msg = $("#selDealerHeading");
    var onInitCity = $("#drpCity option:selected").val();
    if (onInitCity > 0) {
        PopulateDealerMakes(onInitCity);
    }

    function formatDate(strDate) {
        var dt = Date.parse(strDate);
        return (dt.getDate() + "/" + (dt.getMonth() + 1 + "/") + dt.getFullYear());
    }

    function loadMakes() {
        ddlMakes.empty().append("<option value=\"0\">Loading makes...</option>");
        ddlMakes.trigger('chosen:updated');
    }

    function loadDealers() {
        ddlDealer.empty().append("<option value=\"0\">Loading dealers...</option>");
        ddlDealer.trigger('chosen:updated');
    }

    function ClearMakes() {
        ddlMakes.empty().append("<option value=\"0\">--Select Make--</option>").removeAttr("enabled");
        ddlMakes.trigger('chosen:updated');
    }

    function ClearDealers() {
        ddlDealer.empty().append("<option value=\"0\">--Select Dealer--</option>").removeAttr("enabled");
        ddlDealer.trigger('chosen:updated');
    }

    function PopulateDealers(cityId, makeId) {
        try {
            var onlyActiveCampaign = $(chkActiveCampaign).prop("checked");
            if (parseInt(cityId) && parseInt(makeId)) {
                showHideMatchError(ddlCity, false);
                showHideMatchError(ddlMakes, false);
                $.ajax({
                    type: "GET",
                    url: "/api/dealers/city/" + cityId + "/make/" + makeId + "/" + "?activecontract=" + onlyActiveCampaign,
                    datatype: "json",
                    beforeSend: function (xhr) {
                        loadDealers();
                    },
                    success: function (response) {
                        ddlDealer.empty().append("<option value=\"0\">--Select Dealer--</option>").removeAttr("disabled");
                        for (var i = 0; i < response.length; i++) {
                            ddlDealer.append("<option value=" + response[i].Id + ">" + response[i].Name + "</option>");
                        }
                        ddlDealer.trigger('chosen:updated');
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        showToast("AJAX request failed status : " + xhr.status + " and err : " + thrownError);
                        ClearDealers();
                    },
                    complete: function (xhr) {
                        if (xhr.status != 200) {
                            showToast("Something went wrong .Please try again !!");
                        }
                    }
                });
            }
            else {
                ClearDealers();
            }
        } catch (e) {
            showToast("Error occured : " + e.message);
            ClearDealers();
        }
    }

    function PopulateDealerMakes(cityId) {
        try {
            if (parseInt(cityId)) {
                showHideMatchError(ddlCity, false);
                $.ajax({
                    type: "GET",
                    url: "/api/dealermakes/city/" + cityId + "/",
                    datatype: "json",
                    beforeSend: function (xhr) {
                        loadMakes();
                    },
                    success: function (response) {
                        ddlMakes.empty().append("<option value=\"0\">--Select Make--</option>").removeAttr("disabled");
                        for (var i = 0; i < response.length; i++) {
                            ddlMakes.append("<option value=" + response[i].makeId + " makeId=" + response[i].makeId + ">" + response[i].makeName + "</option>");
                        }
                        ddlMakes.trigger('chosen:updated');
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        showToast("AJAX request failed status : " + xhr.status + " and err : " + thrownError);
                        ClearMakes();
                    },
                    complete: function (xhr) {
                        if (xhr.status != 200) {
                            showToast("Something went wrong .Please try again !!");
                        }
                    }
                });
            }            
        } catch (e) {
            showToast("Error occured : " + e.message);
            ClearMakes();
        }
    }

    function GetDealerCampaigns(cityid, makeid, dealerId) {
        if (validateInputs(cityid, makeid, dealerId)) {
            var onlyActiveCampaign = $(chkActiveCampaign).prop("checked");
            try {
                if (onlyActiveCampaign != 'undefined') {
                    var dealerName = $("#drpDealer option:selected").text();
                    var element = document.getElementById('DealerCampaignsList');
                    $.ajax({
                        type: "GET",
                        url: "/api/dealercampaigns/dealer/" + dealerId + "/?activecontract=" + onlyActiveCampaign + "&cityId=" +cityid +"&makeId=" + makeid,
                        beforeSend: function (xhr) {
                            startLoading($("#inputSection"));
                        },
                        datatype: "json",
                        success: function (response) {
                            ko.cleanNode(element);
                            if (response) {
                                var resp = new Object();
                                resp.Table = response;                                
                                ko.applyBindings(new DealerViewModel(resp), element);
                                $('#DealerCampaignsList').show();
                            }
                            else {
                                $('#DealerCampaignsList').hide();
                                showToast("Campaigns for " + dealerHeading + " not available ");
                            }

                        },
                        complete: function (xhr) {
                            if (xhr.status != 200) {
                                showToast("Something went wrong .Please try again !!");
                            }
                            stopLoading($("#inputSection"));
                        }
                    });

                }
                else {
                    showToast("Something went wrong .Please try again !!");
                }
            } catch (e) {
                showToast("Error occured : " + e.message);
            }
        }
    }

    function validateInputs(cityid, makeid, dealerId) {
        var isValid = true;
        if (navigator.onLine) {
            /*if (!parseInt(cityid)) {
                showHideMatchError(ddlCity, true);
                isValid = false;
            }
            if (!parseInt(makeid)) {
                showHideMatchError(ddlMakes, true);
                isValid = false;
            }
            if (!parseInt(dealerId)) {
                showHideMatchError(ddlDealer, true);
                isValid = false;
            }*/
        }
        else {
            showToast("Oops you're offline!!! Please check the network connetion.");
            isValid = false;
        }
        return isValid;
    }

    ddlCity.change(function () {
        var cityId = $(this).val();
        $("#hdnCityId").val(cityId);
        ClearMakes();
        ClearDealers();
        PopulateDealerMakes(cityId);
    });

    ddlDealer.change(function () { showHideMatchError(ddlDealer, !(parseInt($(this).val()) > 0)); });

    ddlMakes.change(function () {
        ClearDealers();
        PopulateDealers(ddlCity.val(), $(this).val());
    });
    chkActiveCampaign.change(function () {
        PopulateDealers(ddlCity.val(), ddlMakes.val());
    });

    $("#btnGetCampaigns").click(function () {
        var dealerId = ddlDealer.val() ? ddlDealer.val() : 0;
        var cityId = ddlCity.val() ? ddlCity.val() : 0;
        var makeId = ddlMakes.val() ? ddlMakes.val() : 0;
        showHideMatchError(ddlCity, false);
        showHideMatchError(ddlMakes, false);
        showHideMatchError(ddlDealer, false);
        GetDealerCampaigns(cityId, makeId, dealerId);
    });


    var DealerViewModel = function (model) {
        var self = this;
        self.models = model;
        $(self.models.Table).each(function () {
            switch (parseInt(this.ContractStatus)) {
                case 1:
                    this.ColorCode = '#dff0d8';
                    break;
                case 2:
                    this.ColorCode = '#d9edf7';
                    break;
                case 3:
                    this.ColorCode = '#d9534f';
                    break;
                case 4:
                    this.ColorCode = '#fcf8e3';
                    break;
                case 5:
                    this.ColorCode = '#ff6666';
                    break;
                default:
                    this.ColorCode = '#cfcfcf';
                    break;
            }
        });
        ko.mapping.fromJS(self.models, {}, this);
    };

    ddlCity.chosen({ width: "150px", no_results_text: "No matches found!!", search_contains: true });
    ddlMakes.chosen({ width: "150px", no_results_text: "No matches found!!", search_contains: true });
    ddlDealer.chosen({ width: "200px", no_results_text: "No matches found!!", search_contains: true });
    function startLoading(ele) {
        try {
            var _self = $(ele).find(".progress-bar").css({ 'width': '0' }).show();
            _self.animate({ width: '100%' }, 5000);
        }
        catch (e) { return };
    }

    function stopLoading(ele) {
        try {
            var _self = $(ele).find(".progress-bar");
            _self.stop(true, true).css({ 'width': '100%' }).fadeOut(1000);
        }
        catch (e) { return };
    }



</script>
<!-- #Include file="/includes/footerNew.aspx" -->
