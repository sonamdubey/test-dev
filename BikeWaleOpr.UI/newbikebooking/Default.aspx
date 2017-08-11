﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.BikeBooking.Default" Trace="false" Debug="false" Async="true" EnableEventValidation="false" EnableViewState="true" %>

<!-- #Include file="/includes/headerNew.aspx" -->

<link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />

<style type="text/css">
    input[type='text'] {background-color: #fff;}
</style>
<div>

</div>
<div class="left">
    <h1>Manage New Bike Dealers</h1>
    <div>
        <div style="border: 1px solid #777;" class="padding10" id="dealersmakecity">
            <div class="margin-right10 verical-middle form-control-box">
                Dealer's City : <span class="errMessage">* &nbsp</span>
                <asp:dropdownlist id="drpCity" enabled="True" cssclass="drpClass chosen-select" runat="server">
                    <asp:ListItem Text="--Select City--" Value="-1" />
                    </asp:dropdownlist>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select City</div>
            </div>
            <div class="margin-right10 verical-middle form-control-box position-rel">
                Bike Make : <span class="errMessage">* &nbsp</span>
                <select data-placeholder="--Select Make--" id="ddlMakes" class="drpClass chosen-select" data-bind="options: listMakes, value: selectedMake, optionsText: 'makeName', optionsValue: 'makeId', optionsCaption: '--Select Make--', chosen: { width: '150px', search_contains: true }">
                </select>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select Make</div>
            </div>
            <div class="margin-right10 verical-middle form-control-box position-rel">
                Dealer Name : <span class="errMessage">* &nbsp</span>
                <select data-placeholder="--Select Dealer--" id="drpDealer" class="drpClass chosen-select" data-bind="options: listDealers, value: selectedDealer, optionsText: 'dealerName', optionsValue: 'dealerId', optionsCaption: '--Select Dealers--', chosen: { width: '200px', search_contains: true }">
                </select>
                <span class="bwsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please Select Dealer</div>
            </div>
        </div>

        <table width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr>
                <td style="padding-top:10px;">
                    <input type="button" value="Manage Offers" id="btnManageoffer" />&nbsp;&nbsp;
                    <input type="submit" value="Manage Prices And Availability" id="btnManagePrice" runat="server"/>&nbsp;&nbsp;
                    <input type="button" value="Manage Facilities" id="btnManagefacilities" />&nbsp;&nbsp;
                    <input type="button" value="Manage Emi" id="btnEmi" />&nbsp;&nbsp;                    
                     <input type="button" value="Manage Dealer Disclaimer" id="btnDisclaimer" />&nbsp;&nbsp;
                    <input type="button" value="Manage Booking Amount" id="btnBkgAmount" />&nbsp;&nbsp;
                    <input type="button" value="Manage Benefits/ USP" id="btnManageBenefits" />&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div id="selectCityPriceHead" runat="server" class="hide">
        <hr />
        <table width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr>
                <td width="33%">
                    <fieldset style="height: 250px">
                        <legend>Add / Show Price</legend>
                        <div>
                            Select City of Pricing :
                            <asp:dropdownlist id="drpAllCity" runat="server">
                                <asp:ListItem Value="0" Text="--Select--" />
                            </asp:dropdownlist>
                            <asp:button id="addShowPrice" text="Show Price" runat="server"></asp:button>
                        </div>
                        <div class="margin-top10">
                            Select Pricing Heads :
                            <asp:listbox id="drpPriceHead" runat="server" selectionmode="Multiple" style="height: 100px; vertical-align: text-top; position: relative">
                                <asp:ListItem Value="0" Text="--Select--" />
                            </asp:listbox>
                            <asp:button class="margin-top10" id="btnAddCat" text="Add Cateogry to Price Sheet" runat="server"></asp:button>
                        </div>
                    </fieldset>
                </td>
                <td width="33%"><%-- Start Pivotal Tracker # : 95144444 & 96417936 Author : Sumit Kate --%>
                    <fieldset style="height: 250px;">
                        <legend>Copy Price Sheet to Other cities</legend>
                        Select State :<span style="color: red">* </span>
                        <asp:dropdownlist id="ddlState" runat="server" />
                        Select cities :<span style="color: red">* </span>
                        <select id="lstCity" multiple="multiple" style="height: 134px; vertical-align: text-top;"></select>
                        <asp:button id="btnTransferPriceSheet" text="Copy Price Sheet" runat="server" onclientclick="return ConfirmCopy();"></asp:button>
                        <asp:label runat="server" id="lblTransferStatus" class="errorMessage" text="Price Sheet copied successfully."></asp:label>
                    </fieldset>
                    <%-- End Pivotal Tracker # : 95144444 & 96417936 Author : Sumit Kate --%>
                </td>
                <%-- Start Pivotal Tracker # : 104505670 Author : Sadhana Upadhyay --%>
                <td width="33%">
                    <fieldset style="height: 250px">
                        <legend>Copy Price Sheet to Other dealer</legend>
                        Select City :<span style="color: red">* </span>
                        <asp:dropdownlist id="ddlDealerCity" runat="server" />
                        Select Dealer :<span style="color: red">* </span>
                        <select id="lstDealer" multiple="multiple" style="height: 134px; vertical-align: text-top;"></select>
                        <asp:button id="btnUpdateDealerPrice" text="Copy Dealer Price Sheet" runat="server" onclientclick="return ConfirmDealerCopy();"></asp:button>
                        <asp:label runat="server" id="lblDealerPriceStatus" class="errorMessage" text="Price Sheet copied successfully."></asp:label>
                    </fieldset>
                </td>
                <%-- End Pivotal Tracker # : 104505670 Author : Sadhana Upadhyay --%>
            </tr>
        </table>
    </div>
    <div id="btnSaveDelete" class="hide">
        <asp:button id="btnUpdate" text="Update" style="background-color: red; color: white;" runat="server" class="margin-right10" onclientclick="return ValidatePrice();"></asp:button>
        <asp:button id="btnDelete" text="Delete" style="background-color: red; color: white;" runat="server" onclientclick="return ConfirmDelete();"></asp:button>
    </div>
    <div><span class="errorMessage margin-top10" id="spnError"></span></div>
    <div id="bindModels">
        <asp:HiddenField ID="alertText" runat="server"></asp:HiddenField>
        <asp:label runat="server" id="lblSaved" class="errorMessage"></asp:label>
        <asp:repeater id="rptModels" runat="server">
                <HeaderTemplate>
                    <table class="lstTable" border="1">
                        <tr>
                            <th><div>Select All</div><div><input type="checkbox" runat="server" id="hd_chk"/></div></th>
                            <th>Make</th>
                            <th>Model</th>
                            <th>Version</th>
                            <asp:Repeater Id="rptHeader" runat="server" DataSource="<%# GetPQCommonAttrs() %>">
                                <ItemTemplate>
                                    <th> <%# Eval("ItemName") %></th>            
                                </ItemTemplate>
                            </asp:Repeater>
                            <th>Availability(days)</th>
                            <th>Availability By Color</th>
                        </tr>
                </HeaderTemplate>
                <AlternatingItemTemplate>
                        <tr id='<%#DataBinder.Eval(Container.DataItem, "VersionId")%>' style="background-color:#bfbfc6" >
                        <td style="text-align: center;">
                            <asp:Label style="display:none;" id="lblVersionId" Text='<%# DataBinder.Eval( Container.DataItem, "VersionId" ) %>' runat="server"></asp:Label>
                            <input type="checkbox" versionid='<%#Eval("VersionId") %>' id="chkUpdate" class="checkboxAll itsGrey" runat="server"/>
                        <asp:HiddenField ID="txtModel" Value='<%# Eval("Model") %>' runat="server" ></asp:HiddenField>
                        <asp:HiddenField ID="txtModelId" Value='<%# Eval("BikeModelId") %>' runat="server" ></asp:HiddenField>                       
                        </td>
                        <td><%# Eval("Make") %></td>
                        <td><%# Eval("Model") %></td>
                        <td><%# Eval("VersionName") %></td>   
                                         
                        <asp:Repeater ID="rptValues" DataSource="<%# GetPQCommonAttrs() %>"  runat="server" >
                            <ItemTemplate>
                                <td style="width:90px; text-align:center">
                                    <asp:Label style="display:none;" id="lblCategoryId" Text='<%# DataBinder.Eval( Container.DataItem, "ItemCategoryId" ) %>' runat="server"></asp:Label>
                                    <span class="spnPrices"><%# GetItemValue(DataBinder.Eval(((RepeaterItem)Container.Parent.Parent).DataItem,"VersionId").ToString(), Eval("ItemCategoryId").ToString()) %></span>
                                    <asp:Textbox ID="txtValue" class="hide" style="width:60px;" MaxLength="9"  Text='<%# GetItemValue(DataBinder.Eval(((RepeaterItem)Container.Parent.Parent).DataItem,"VersionId").ToString(), Eval("ItemCategoryId").ToString()) %>' runat="server" categoryid='<%# DataBinder.Eval( Container.DataItem, "ItemCategoryId" ) %>'></asp:Textbox>
                                    <span class="spnValueError"></span>
                                </td>
                            </ItemTemplate>
                        </asp:Repeater>
                        <td>
                            <span class="spnDays"><%# String.IsNullOrEmpty(Eval("NumOfDays").ToString())?"NA":Eval("NumOfDays") %></span>
                            <asp:Textbox class="metAvailableBlack hide" id="lblAvailableDays" runat="server" style="width:60px;" Text='<%# String.IsNullOrEmpty(Eval("NumOfDays").ToString())?"NA":Eval("NumOfDays") %>'></asp:Textbox>
                        </td>
                        <td>
                            <a class="availabilityByColor text-blue" NoOfDays="<%# String.IsNullOrEmpty(Eval("NumOfDays").ToString())?"NA":Eval("NumOfDays") %>" href="Javascript:void(0)" >View Colors</a> 
                        </td>
                    </tr>
                </AlternatingItemTemplate>
                <ItemTemplate>
                    <tr id='<%#DataBinder.Eval(Container.DataItem, "VersionId")%>'>
                            <td style="text-align: center;">
                            <asp:Label style="display:none;" id="lblVersionId" Text='<%# DataBinder.Eval( Container.DataItem, "VersionId" ) %>' runat="server"></asp:Label>
                            <input type="checkbox" versionid='<%#Eval("VersionId") %>'  id="chkUpdate" class="checkboxAll" runat="server" />
                            <asp:HiddenField ID="txtModel" Value='<%# Eval("Model") %>' runat="server" ></asp:HiddenField>
                            <asp:HiddenField ID="txtModelId" Value='<%# Eval("BikeModelId") %>' runat="server" ></asp:HiddenField>                           
                        </td>
                        </td>
                        <td><%# Eval("Make") %></td>
                        <td><%# Eval("Model") %></td>
                        <td><%# Eval("VersionName") %></td>
                        
                        <asp:Repeater ID="rptValues" DataSource="<%# GetPQCommonAttrs() %>"  runat="server" >
                            <ItemTemplate>
                                <td style="width:90px;text-align:center">
                                    <asp:Label style="display:none;" id="lblCategoryId" Text='<%# DataBinder.Eval( Container.DataItem, "ItemCategoryId" ) %>' runat="server"></asp:Label>
                                    <span class="spnPrices"><%# GetItemValue(DataBinder.Eval(((RepeaterItem)Container.Parent.Parent).DataItem,"VersionId").ToString(), Eval("ItemCategoryId").ToString()) %></span>
                                    <asp:Textbox ID="txtValue" class="hide" style="width:60px;" MaxLength="9" Text='<%# GetItemValue(DataBinder.Eval(((RepeaterItem)Container.Parent.Parent).DataItem,"VersionId").ToString(), Eval("ItemCategoryId").ToString()) %>' runat="server" categoryid='<%# DataBinder.Eval( Container.DataItem, "ItemCategoryId" ) %>' ></asp:Textbox>
                                </td>
                            </ItemTemplate>
                        </asp:Repeater>
                        <td>
                            <span class="spnDays"><%# String.IsNullOrEmpty(Eval("NumOfDays").ToString())?"NA":Eval("NumOfDays") %></span>
                            <asp:Textbox class="metAvailableWhite hide"  id="lblAvailableDays" runat="server" style="width:60px;" Text='<%# String.IsNullOrEmpty(Eval("NumOfDays").ToString())?"NA":Eval("NumOfDays") %>'></asp:Textbox>
                        </td>
                        <td>
                            <a class="availabilityByColor text-blue" NoOfDays="<%# String.IsNullOrEmpty(Eval("NumOfDays").ToString())?"NA":Eval("NumOfDays") %>" href="Javascript:void(0)" >View Colors</a> 
                        </td>
                    </tr>
                </ItemTemplate>
                    <FooterTemplate>
                        </table>
                </FooterTemplate>      
            </asp:repeater>
    </div>
    <%-- category Items Binding--%>
    <div>
        <asp:repeater id="rptcatItem" runat="server">
                <HeaderTemplate>
                    <div style="background-color: #f4f3f3; padding: 10px; margin-top: 10px;min-height:110px;"  class="padding10">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="floatLeft padding5" style="width:140px;">
                        <input type="checkbox" runat="server" categoryid='<%# Eval("CategoryId")%>' id="chkCat"/>
                        <label for="chk_<%# DataBinder.Eval(Container.DataItem, "CategoryId") %>"><%# DataBinder.Eval(Container.DataItem, "CategoryName") %></label>                        
                    </div>
                </ItemTemplate>
            </asp:repeater>
        <footertemplate>
    </div>

    <input type="hidden" id="hdnMakeId" runat="server">
    <input type="hidden" id="hdnDealerId" runat="server">
    <input type="hidden" id="hdnCities" runat="server">
    <input type="hidden" id="hdnDealerList" runat="server" />
    <input type="hidden" id="hdnDealerCity" runat="server" />
    <input type="hidden" id="ispostback" value="<%=Page.IsPostBack.ToString()%>" />
</div>


<script type="text/javascript" src="https://stb.aeplcdn.com/bikewale/src/common/chosen.jquery.min.js?v15416"></script>
<script type="text/javascript">
    var ddlCities = $('#<%=drpCity.ClientID%>');
    var ddlMakes = $('#ddlMakes');
    var ddlDealers = $('#drpDealer');

    ko.bindingHandlers.chosen = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var $element = $(element);
            var options = ko.unwrap(valueAccessor());
            if (typeof options === 'object')
                $element.chosen(options);

            ['options', 'selectedOptions', 'value'].forEach(function (propName) {
                if (allBindings.has(propName)) {
                    var prop = allBindings.get(propName);
                    if (ko.isObservable(prop)) {
                        prop.subscribe(function () {
                            $element.trigger('chosen:updated');
                        });
                    }
                }
            });
        }
    };

    function isPostBack() {
        return $('#ispostback').val() == 'True';
    }

    function dealerModel() {
        var self = this;
        self.listMakes = ko.observableArray();
        self.listDealers = ko.observableArray();
        self.selectedCity = ko.observable();
        self.selectedMake = ko.observable();
        self.selectedDealer = ko.observable();
        self.makeApiUrl = "/api/makes/city/";

        self.ClearMakes = function () {
            self.listMakes([]);
            ddlMakes.trigger('chosen:updated');
        }

        self.ClearDealers = function () {
            self.listDealers([]);
            ddlDealers.trigger('chosen:updated');
        }

        ddlCities.change(function (item, event) {
            self.selectedCity(ddlCities.val());
            showHideMatchError(ddlCities, false);
            showHideMatchError(ddlMakes, false);
            showHideMatchError(ddlDealers, false);
            self.ClearMakes();
            self.ClearDealers();
            self.cityChanged();
        });
        ddlMakes.change(function () {
            self.selectedMake(ddlMakes.val());
            showHideMatchError(ddlMakes, false);
            showHideMatchError(ddlDealers, false);
            self.ClearDealers();
            self.makeChanged();
        });
        ddlDealers.change(function () {
            if (ddlDealers.val()) {
                self.selectedDealer(ddlDealers.val());
                showHideMatchError(ddlDealers, false);
                $('#hdnDealerId').val(self.selectedDealer());
            }
        });

        self.cityChanged = function () {
            try {
                if (self.selectedCity() != null && self.selectedCity() > 0) {
                    $("#hdnCityId").val(self.selectedCity());
                    $.ajax({
                        type: "GET",
                        url: self.makeApiUrl + self.selectedCity() + "/",
                        datatype: "json",
                        success: function (response) {
                            var makes = ko.toJS(response);
                            self.listMakes(makes);
                            if (isPostBack()) {
                                self.selectedMake($('#hdnMakeId').val());
                             }
                            else if (self.listMakes().length == 1) {
                                self.selectedMake(self.listMakes()[0].makeId);
                            }
                            ddlMakes.trigger('chosen:updated');
                            self.makeChanged();
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            showToast("AJAX request failed status : " + xhr.status + " and err : " + thrownError);
                            self.ClearMakes();
                        },
                        complete: function (xhr) {
                            if (xhr.status != 200) {
                                showToast("Something went wrong .Please try again !!");
                            }
                        }
                    });
                }
                else {
                    self.ClearMakes();
                }
            }
            catch (e) {
                showToast("Error occured : " + e.message);
                self.ClearMakes();
            }
        }

        self.makeChanged = function () {
            try {
                if (self.selectedMake() != null && self.selectedMake() > 0) {
                    $.ajax({
                        type: "GET",
                        url: "/api/dealers/make/" + self.selectedMake() + "/city/" + self.selectedCity() + "/",
                        datatype: "json",
                        success: function (response) {
                            var dealers = ko.toJS(response);
                            self.listDealers(dealers);
                            ddlDealers.trigger('chosen:updated');
                            if (isPostBack()) {
                                self.selectedDealer($('#hdnDealerId').val());
                                ddlDealers.trigger('chosen:updated');
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            showToast("AJAX request failed status : " + xhr.status + " and err : " + thrownError);
                            self.ClearDealers();
                        },
                        complete: function (xhr) {
                            if (xhr.status != 200) {
                                showToast("Something went wrong .Please try again !!");
                            }
                        }
                    });
                }
                else {

                    self.ClearDealers();
                }
            }

            catch (e) {
                showToast("Error occured : " + e.message);
                self.ClearDealers();
            }
        }


        self.validateInputs = function (cityid, makeid, dealerId) {
            var isValid = true;
            if (navigator.onLine) {
                if (!parseInt(cityid)) {
                    showHideMatchError(ddlCities, true);
                    isValid = false;
                }
                if (!parseInt(makeid)) {
                    showHideMatchError(ddlMakes, true);
                    isValid = false;
                }
                if (!parseInt(dealerId)) {
                    showHideMatchError(ddlDealers, true);
                    isValid = false;
                }
            }
            else {
                showToast("Oops you're offline!!! Please check the network connection.");
                isValid = false;
            }
            return isValid;
        }


    }

    var dVm = new dealerModel();
    ko.applyBindings(dVm, document.getElementById("dealersmakecity"));
    ddlCities.chosen();
    ddlMakes.chosen()
    ddlDealers.chosen();

    function ConfirmDelete() {
        var exists = false;
        var acknowledge;
        $('.checkboxAll').each(function () {
            if (this.checked) {
                exists = true;
                $(this).parent().parent().css('background-color', '#FFFF7F')
            }
        })
        if (exists) {
            acknowledge = confirm("Are you sure you want to delete selected record(s)");
            if (acknowledge) {
                return acknowledge;
            }
        }
        return acknowledge;
    }
    function ValidatePrice() {
        $("#lblSaved").text("");
        var isError = true;
        $('#spnError').text("");
        $('.checkboxAll').each(function () {
            if ((this).checked) {
                var parentInputRow = $(this).parent().parent().find("input[type='text']");
                for (var i = 1; i < parentInputRow.length; i++) {
                    if (isNaN(parentInputRow[i].value) || parentInputRow[i].value.trim() == "") {
                        $('#spnError').text("Please Enter Numeric Values in Selected Text-Box Field(s)");
                        isError = false;
                        }
                }
            }
        });
        return isError;
    }
    <%--Start Pivotal Tracker # : 95144444 & 96417936
    Author : Sumit Kate
    Created by Sumit Kate on 11/09/2015
    Purpose : To ask for Transfer Price Sheet confimation before submitting the form.
    --%>
    function ConfirmCopy() {
        var acknowledge = false;
        var strCityId = "";
        var stateId = $("#ddlState").val();
        $("#lstCity option:selected").each(function () { strCityId += this.value + ","; });
        strCityId = strCityId.substring(0, strCityId.length - 1);
        if ((strCityId.length > 0) && (stateId != "") && (stateId > 0)) {
            $("#hdnCities").val(strCityId);
            acknowledge = confirm("Do you want to copy the current city's price sheet to selected cities.");
            if (acknowledge) {
                return acknowledge;
            }
        }
        else {
            alert("Please select state and cities.");
        }
        return acknowledge;
    }

    //Created By : Sadhana Upadhyay on 5 Oct 2015
    //To validate dealer price sheet coty form
    function ConfirmDealerCopy() {
        var acknowledge = false;
        var strDealerId = '';
        var cityId = $('#ddlDealerCity').val();
        $('#lstDealer option:selected').each(function () {
            strDealerId += this.value + ',';
        });

        strDealerId = strDealerId.substring(0, strDealerId.length - 1);
        if (strDealerId.length > 0 && cityId != '' && cityId > 0) {

            $('#hdnDealerList').val(strDealerId);
            acknowledge = confirm("Do you want to copy the current city's price sheet to selected cities.");
            if (acknowledge)
                return acknowledge;
        } else {
            alert('Please select city and dealers.');
        }
        return acknowledge;
    }

    <%--
    Created by Sumit Kate on 11/09/2015
    Purpose : To Load all cities of the state from CarWale DB using AjaxPro API call
    --%>
    function LoadStateCities() {
        var stateId = $("#ddlState").val();
        if (stateId > 0) {
            var requestType = "ALL";
            $("#hdnCities").val("");
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"requestType":"' + requestType + '", "stateId":"' + stateId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetCWCities"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    bindDropDownList(resObj, $("#lstCity"), null, null);
                    $("#lstCity option[value=0]").remove();
                }
            });
        } else {
            $("#lstCity").empty();
            $("#lstCity").val("0").attr("disabled", true);
        }
    }
    <%--
    Created by Sumit Kate on 11/09/2015
    Purpose : Change event handler function assignment for States dropdown list
    --%>
    $("#ddlState").change(LoadStateCities);

    <%--End Pivotal Tracker # : 95144444 & 96417936 Author : Sumit Kate--%>
    $(document).ready(function () {
        var alertBox = $("#alertText");
        if (alertBox.val()) {
            alert(alertBox.val());
            alertBox.val("");
        }

        if ($("#ddlState").val() > 0) {
            LoadStateCities();
        }

        $("#drpDealer").change(function () {
            $("#drpAllCity").val("-1");
        })

        $('#rptModels_hd_chk').click(function () {
            if (this.checked) {
                $("#btnSaveDelete").removeClass("hide");
            }
            else $("#btnSaveDelete").addClass("hide");
        });

        $('.checkboxAll').click(function () {
            $("#lblSaved").text("");
            var parentRow = $(this).parent().parent();
            if ($("[type='checkbox']:checked").length == 1) {

                if ($("#rptModels_hd_chk").is(':checked')) {
                    $('#rptModels_hd_chk').prop('checked', false);
                    $("#btnSaveDelete").addClass("hide");
                }
            }
            if ($("[type='checkbox']:checked").length >= 1) {
                $("#btnSaveDelete").removeClass("hide");
            }
            else
                $("#btnSaveDelete").addClass("hide");

            var chkSiblings = $(this).parent().siblings();
            if (this.checked) {
                chkSiblings.find("input:text").show();
                chkSiblings.find("span.spnPrices").hide();
                chkSiblings.find("span.spnDays").hide();
            }
            else {
                chkSiblings.find("input:text").hide();
                chkSiblings.find("span.spnPrices").show();
                chkSiblings.find("span.spnDays").show();
            }
        });

        $('#rptModels_hd_chk').click(function (event) {
            //on click 
            var chkSiblings;
            if (this.checked) {
                // check select status
                $('.checkboxAll').each(function () { //loop through each checkbox
                    this.checked = true;  //select all checkboxes with class "checkboxAll"      
                    chkSiblings = $(this).parent().siblings();
                    chkSiblings.find("input:text").show();
                    chkSiblings.find("span.spnPrices").hide();
                    chkSiblings.find("span.spnDays").hide();
                });

            } else {
                $('.checkboxAll').each(function () { //loop through each checkbox
                    this.checked = false; //deselect all checkboxes with class "checkboxAll" 
                    chkSiblings = $(this).parent().siblings();
                    chkSiblings.find("input:text").hide();
                    chkSiblings.find("span.spnPrices").show();
                    chkSiblings.find("span.spnDays").show();
                });
            }

        });
        var BwOprHostUrl = '<%= BwOprHostUrl %>';
        var ddlDealer = $("#drpDealer");
        var selectString = "--Select Dealer--";

        $('#drpPriceHead option:selected').click(function () {

            $(this).attr("selected", "selected");
        });

        $('#ddlDealerCity').change(function () {
            var cityId = $(this).val();
            $('#hdnDealerCity').val(cityId);
            var ddlDealerList = $('#lstDealer');

            if (cityId > 0) {
                $.ajax({
                    type: "GET",
                    url: BwOprHostUrl + "/api/Dealers/GetAllDealers/?cityId=" + cityId,
                    success: function (response) {
                        ddlDealerList.empty().append("<option value=\"0\">" + selectString + "</option>").removeAttr("disabled");
                        for (var i = 0; i < response.length; i++) {
                            ddlDealerList.append("<option value=" + response[i].Value + " makeId=" + response[i].MakeId + ">" + response[i].Text + "</option>");
                        }
                    }
                });
            }
            else {
                ddlDealerList.empty().append("<option value=\"0\">" + selectString + "</option>").removeAttr("enabled");
            }
        });

        $("#drpAllCity").change(function () {
            $("#hdnCityId").val($(this).val());
        })

        $("#btnManageoffer").click(function () {
            $("#bindModels").addClass("hide");
            $("#selectCityPriceHead").addClass("hide");
            var dealerId = dVm.selectedDealer();
            if (dealerId > 0) {
                window.open('/newbikebooking/ManageOffers.aspx?dealerId=' + dealerId + '', 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
            }
            else
                alert("Please select dealer");
        });

        $("#btnManageBenefits").click(function () {
            $("#bindModels").addClass("hide");
            $("#selectCityPriceHead").addClass("hide");
            var dealerId = dVm.selectedDealer();
            var cityId = dVm.selectedCity();
            if (dealerId > 0 && cityId > 0) {
                window.open('/newbikebooking/ManageDealerBenefits.aspx?dealerId=' + dealerId + '&cityId=' + cityId, 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
            }
            else
                alert("Please select dealer");
        });

       $("#btnManagePrice").click(function () {
            if (dVm.validateInputs(dVm.selectedCity(), dVm.selectedMake(), dVm.selectedDealer())) {
                showHideMatchError(ddlCities, false);
                showHideMatchError(ddlMakes, false);
                showHideMatchError(ddlDealers, false);
                $("#hdnDealerId").val(dVm.selectedDealer());
                $("#hdnMakeId").val(dVm.selectedMake());
                $("#selectCityPriceHead").removeClass("hide");
            }
            else {
                return false;
            }

        });

        if (isPostBack()) {
            dVm.selectedCity(ddlCities.select().val());
            dVm.cityChanged();
         }


        //code my
        $("#btnManagefacilities").click(function () {
            $("#bindModels").addClass("hide");
            $("#selectCityPriceHead").addClass("hide");
            var dealerId = dVm.selectedDealer();
            if (dealerId > 0) {
                window.open('/dealers/' + dealerId + '/facilities/', '_blank');
            }
            else
                alert("Please select dealer");
        });

        $("#btnMapDealer").click(function () {
            $("#bindModels").addClass("hide");
            $("#selectCityPriceHead").addClass("hide");
            var dealerId = dVm.selectedDealer();
            if (dealerId > 0) {
                window.open('/newbikebooking/ManageDealerAreaMapping.aspx?dealerid=' + dealerId + '', 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
            }
            else
                alert("Please select dealer");
        });

        $("#btngoAvailable").click(function () {
            var dealerId = dVm.selectedDealer();
            if (dealerId > 0) {
                window.open('/newbikebooking/ManageBikeAvailability.aspx?dealerId=' + dealerId + '', 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
            }
            else
                alert("Please select dealer");

        });

        $("#btnEmi").click(function () {
            $("#bindModels").addClass("hide");
            $("#selectCityPriceHead").addClass("hide");
            var dealerId = dVm.selectedDealer();
            if (dealerId > 0) {
                window.open('/newbikebooking/ManageDealerLoanAmounts.aspx?dealerId=' + dealerId + '', 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
            }
            else
                alert("Please select dealer");

        });

        $("#btnDisclaimer").click(function () {
            $("#bindModels").addClass("hide");
            $("#selectCityPriceHead").addClass("hide");
            var dealerId = dVm.selectedDealer();
            if (dealerId > 0) {
                window.open('/newbikebooking/ManageDealerDisclaimer.aspx?dealerId=' + dealerId + '', 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
            }
            else
                alert("Please select dealer");
        });

        $("#btnBkgAmount").click(function () {
            $("#bindModels").addClass("hide");
            $("#selectCityPriceHead").addClass("hide");
            var dealerId = dVm.selectedDealer();
            if (dealerId > 0) {
                window.open('/newbikebooking/ManageBookingAmount.aspx?dealerId=' + dealerId + '', 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
            }
            else
                alert("Please select dealer");
        });

        //manage availability by color and cost script goes here
        $("a.availabilityByColor").click(function () {
            var versionId = $(this).parent().parent().attr('id');
            var versionAvailDays = $(this).attr("NoOfDays");
            var dealerId = $("#hdnDealerId").val();
            if (dealerId > 0) {
                window.open('/newbikebooking/ManageBikeAvailabilityByColorAndCost.aspx?dealerId=' + dealerId + '&versionId=' + versionId + '&versionAvailDays=' + versionAvailDays, 'mywin', 'scrollbars=yes,left=25%,top=25%,width=600,height=400');
            }
            else
                alert("Please select dealer");
        });
        //ends here
    });

    function showHideMatchError(element, error) {
        if (error) {
            element.parent().find('.error-icon').removeClass('hide');
            element.parent().find('.bw-blackbg-tooltip').removeClass('hide');
            element.addClass('border-red')
        }
        else {
            element.parent().find('.error-icon').addClass('hide');
            element.parent().find('.bw-blackbg-tooltip').addClass('hide');
            element.removeClass('border-red');
        }
    }
</script>

<!-- #Include file="/includes/footerNew.aspx" -->
