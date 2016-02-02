<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageDealerCampaigns.aspx.cs" Inherits="BikewaleOpr.newbikebooking.ManageDealerCampaigns" %>
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
    <div id="mfgCampaigns">
        <h3> Manage Manufacturer's Campaigns</h3>
        <hr />
        <input name="actionCamp" class="rdbtn" type="radio" value="1" data-bind="checked: selectedAction" /> Add Campaign
        <input name="actionCamp" class="rdbtn" type="radio" value="2" data-bind="checked: selectedAction" /> Edit Campaign
        <hr />
        <div class="margin-top10" data-bind="visible: parseInt(selectedAction(), 16) > 0">
             <asp:DropDownList ID="ddlMake" runat="server"><asp:ListItem Value="0" Text="--Selected Make--" ></asp:ListItem></asp:DropDownList>
            <select id="ddlModel" multiple><option value="0">--Selected Model--</option></select>
             <select id="ddlManufacturers" data-bind="options: Manufacturers, optionsValue: 'Id', optionsText: 'Organization', value: selectedManufacturer, optionsCaption: 'Choose manufacturer...'" ></select>
            <input type="button" id="selectAction" data-bind="click: selectAction " value="Go" style="padding:10px; margin-left:20px; cursor:pointer;"/>
           
        </div> 
        
        <div id="addCamp" data-bind="with: Camp,visible: ActionState() == 1" ><hr /> 
            <h3> Campaign Description</h3>
            <table border="1" cellspacing="0" cellpadding="5">
                <tbody>
                    <tr>
                        <th>Date</th>
                        <th>Campaign Description</th>
                        <th>Make</th>
                        <th>Model</th>
                        <th>Action</th>
                    </tr>
                    <tr >
                        <td data-bind="text: LDate"></td>
                        <td><input type="text" style="padding:5px;min-width:250px" id="campDesc" data-bind="value: Description" placeholder="campaign description"/></td>
                        <td><span data-bind="text: $root.selectedMake()"></span></td>
                        <td><span data-bind="text: $root.selectedModels()"></span></td>
                        <td><input data-bind="click: $root.AddCampaign" type="button" value="Add" style="padding:10px; margin-left:20px; cursor:pointer;"/></td>
                    </tr>
                </tbody>
            </table>

        </div> 
                    <div class="margin-top10" id="addStatus" data-bind="text: ErrorMsg"></div>  
        
        <div id="editCamp" data-bind="visible: ActionState() == 2"> <hr /> 
            <input type="button" class="margin-bottom10" value="Stop Selected Campaigns" data-bind="click: $root.stopCampaign" style="padding:10px; cursor:pointer;"/>
             <table border="1" cellspacing="0" cellpadding="5">
                <tbody>
                    <tr>
                        <th>Select</th>
                        <th>Campaign Description</th>
                        <th>Make</th>
                        <th>Model</th>
                        <th>Campaign Launch Date</th>
                    </tr>
                    <!-- ko foreach : ManufacturerCamps -->
                    <tr >
                        <th><input type="checkbox" name="campaign" class="chkcampaign" data-bind="attr: { CampId: CampaignId }"/><input type="hidden" data-bind="value: DealerId" /></th>
                        <th data-bind="text : Description"></th>
                        <th data-bind="text : MakeName"></th>
                        <th data-bind="text: ModelName"></th>
                         <th data-bind="text: EntryDate"></th>
                    </tr>
                    <!-- /ko -->
                </tbody>
            </table>   
           
        </div>     
    </div>
    </div>

<script>

    var ddlModel = $("#ddlModel"), ddlMake = $("#ddlMake"), btnGetPriceQuote = $("#btnGetPriceQuote"), ddlManufacturers = $("#ddlManufacturers");
    ddlMake.change(function () {
        var makeId = $(this).val();
        $("#selMake").text($("#ddlMake option:selected").text());
        fillmodels(makeId);
    });

    $("input.rdbtn").click(function () {
        if ($(this).val() == "1") {
            $("#ddlModel").show();
            $("#ddlMake").show();
            $("#editCamp").hide();
        }
        else if ($(this).val() == "2") {
            $("#ddlModel").hide();
            $("#ddlMake").hide();
            $("#addCamp").hide();
        }
    });

    var d = new Date().toJSON().slice(0, 10);

    var campaign = function () {
        self.LDate = ko.observable(d);
        self.Description = ko.observable();
        self.Make = ko.observable();
        self.DealerId = ko.observable();
        self.CampId = ko.observable();
        self.Models = ko.observableArray([]);
        self.ErrorMsg = ko.observable();
    }

    var mfgCamp = function () {
        var self = this;
        self.selectedMake = ko.observable();
        self.selectedModels = ko.observableArray([]);
        self.selectedModelIds = ko.observableArray([]);
        self.selectedManufacturer = ko.observable();
        self.selectedAction = ko.observable();
        self.ActionState = ko.observable(0);
        self.Manufacturers = ko.observableArray([]);
        self.Camp = ko.observable(new campaign());
        self.ManufacturerCamps = ko.observableArray([]);

        self.selectAction = function () {
            if (self.selectedAction()) {
                if (self.selectedAction() == "1") {
                    $("#ddlModel").show();
                    $("#ddlMake").show();
                    self.ActionState(1);
                }
                else if (self.selectedAction() == "2") {
                    $("#ddlModel").hide();
                    $("#ddlMake").hide();
                    self.ManufacturerCamps([]);
                    self.Camp().ErrorMsg = "";
                    self.viewAllCampaign();
                    self.ActionState(2);
                }
            }
            else
                self.ActionState(0);
            return;
        }

        self.AddCampaign = function (data, event) {
            self.Camp().Description = $("#campDesc").val();
            if (validateDetails())
            {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikewaleOpr.Common.AjaxManufacturerCampaign,BikewaleOpr.ashx",
                    data: ko.toJSON({ "dealerId": self.selectedManufacturer(), "modelIds": self.selectedModelIds(), "description": $("#campDesc").val() }),
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SaveManufacturerCampaign"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        if(resObj)
                        {
                            $("#addStatus").text("Campaign Sucessfully Added");
                            self.ActionState(2);
                            self.selectedAction("2");
                            $("#ddlModel").hide();
                            $("#ddlMake").hide();
                            self.viewAllCampaign();
                            
                        }else{
                            $("#addStatus").text("Some error occured !!");
                            self.Camp().ErrorMsg = "";

                        }
                    },
                    complete: function (xhr) {
                        if (xhr.status == 404 || xhr.status == 204) {
                            alert("Error Occurred");
                            $("#addStatus").text("Some error occured !!");
                        }
                    }
                });
            }           
            return;
        }

        self.stopCampaign = function () {
            var strCampIds = "";
            $('input.chkcampaign:checked').each(function () {
                if ($(this).val() != "0") {
                    strCampIds += $(this).attr("CampId") + ",";
                }
            });
            strCampIds = strCampIds.substr(0, strCampIds.length - 1);
            if (self.selectedManufacturer() > 0)
            {
                if (strCampIds.length > 0) {

                    $.ajax({
                        type: "POST",
                        url: "/ajaxpro/BikewaleOpr.Common.AjaxManufacturerCampaign,BikewaleOpr.ashx",
                        data: ko.toJSON({ "dealerId": self.selectedManufacturer(), "campaignIds": strCampIds }),
                        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SetManufacturerCampaignInActive"); },
                        success: function (response) {
                            var responseJSON = eval('(' + response + ')');
                            var resObj = eval('(' + responseJSON.value + ')');
                            if (resObj) {
                                $("#addStatus").text("Campaign Sucessfully Removed");
                                self.ActionState(0);
                                self.viewAllCampaign();
                            } else {
                                $("#addStatus").text("Some error occured !!");
                                self.Camp().ErrorMsg = "";
                            }
                        },
                        complete: function (xhr) {
                            if (xhr.status == 404 || xhr.status == 204) {
                                alert("Error Occurred");
                                $("#addStatus").text("Some error occured !!");
                            }
                        }
                    });
                }
                else {
                    alert("No Campaign Selected");
                }
            }
            else {
                alert("No Manufacturer Selected");
            }
            
        }

        self.viewAllCampaign = function () {

            if (self.selectedManufacturer() > 0)
            {
               $.ajax({
                type: "POST",
                url: "/ajaxpro/BikewaleOpr.Common.AjaxManufacturerCampaign,BikewaleOpr.ashx",
                data: ko.toJSON({ "dealerId": self.selectedManufacturer()}),
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetManufacturerCampaigns"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    viewModel.ManufacturerCamps(ko.toJS(resObj));
                },
                complete: function (xhr) {
                    if (xhr.status == 404 || xhr.status == 204) {
                        alert("Error Occurred");
                    }
                }
               });
            }
            else {
                alert("Select Manufacturer !");
                return false;
            }
            

        };
    }

    var viewModel = new mfgCamp();
    ko.applyBindings(viewModel, $("#mfgCampaigns")[0]);

    
    ddlModel.change(function () {
        var str = "", strIds = "";
        viewModel.selectedMake($("#ddlMake option:selected").text());
        $("#ddlModel option:selected").each(function () {
            if ($(this).val() != "0") {
                str += $(this).text() + ",";
                strIds += $(this).val() + ",";
            }
        });
        str = str.substr(0, str.length - 1);
        strIds = strIds.substr(0, strIds.length - 1);
        $("#selModels").text(str);
        viewModel.selectedModels(str);
        viewModel.selectedModelIds(strIds);

    });

    function validateDetails()
    {
        isValid =false;
        if(viewModel.selectedManufacturer()> 0 ) isValid = true;
        else{
            viewModel.Camp().ErrorMsg += "Manufacturer Not Selected";
        }

        if(viewModel.Camp().Description.trim().length > 5 ) isValid = true;
        else viewModel.Camp().ErrorMsg += "Please Enter valid description more than 10 characters";

        if(viewModel.selectedModels().trim().length > 0 ) isValid = true;
        else viewModel.Camp().ErrorMsg += "No Models Selected !!";

        return isValid;

    }

    function fillmodels(makeid) {
        var requestType = "PRICEQUOTE";
        if (makeid > 0) {
            ddlModel.val("0").attr("disabled", true);
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"makeId":"' + makeid + '","requestType":"' + requestType + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    bindDropDownList(resObj, ddlModel, "", "--Select Model--");
                },
                complete: function (xhr) {
                    if (xhr.status == 404 || xhr.status == 204) {
                        alert("Error Occurred");
                    }
                }
            });
        }
        else {
            ddlModel.val("0").attr("disabled", true);
        }
    }

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
