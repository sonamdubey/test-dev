<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.BikeBooking.Default" Trace="false" Debug="false" Async="true" EnableEventValidation="false" EnableViewState="true" %>

<!-- #Include file="/includes/headerNew.aspx" -->
<script src="/src/jquery-1.6.min.js" type="text/javascript"></script>
<script src="/src/AjaxFunctions.js" type="text/javascript"></script>
<link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
<style type="text/css">
    input[type='text'] {
        background-color: #fff;
    }
</style>
<div>
    You are here &raquo; New Bike Booking
</div>
<form runat="server">
    <div style="border: 1px solid black; margin-left: 180px;" class="padding10">
        <table width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr id="city">
                Dealer's City : <font color="red">* &nbsp</font>
                <asp:dropdownlist id="drpCity" enabled="True" cssclass="drpClass" runat="server">
					<asp:ListItem Text="--Select City--" Value="-1"/>
					</asp:dropdownlist>
                <span style="font-weight: bold; color: red;" id="spndrpCity" class="error" />&nbsp&nbsp
            </tr>
            <tr>
                Dealer Name : <font color="red">* &nbsp</font>
                <asp:dropdownlist id="drpDealer" enabled="True" cssclass="drpClass" runat="server">
				<asp:ListItem Text="--Select Dealer--" Value="-1" />
				</asp:dropdownlist>
            </tr>
            <br></br>
            <tr>
                <td>
                    <input type="button" value="Manage Offers" id="btnManageoffer" />&nbsp;&nbsp;
                    <input runat="server" type="submit" value="Manage Prices And Availability" id="btnManagePrice" />&nbsp;&nbsp;
                    <input type="button" value="Manage Facilities" id="btnManagefacilities" />&nbsp;&nbsp;
                    <input type="button" value="Manage Dealer Area Mapping" id="btnMapDealer" />&nbsp;&nbsp;
                    <input type="button" value="Manage Emi" id="btnEmi" />&nbsp;&nbsp;
                    <%--<input type ="button" value="Manage Bike Avalability" id="btngoAvailable"/>--%>&nbsp;&nbsp;
                    <input type="button" value="Manage Dealer Disclaimer" id="btnDisclaimer" />&nbsp;&nbsp;
                    <input type="button" value="Manage Booking Amount" id="btnBkgAmount" />&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div id="selectCityPriceHead" runat="server" class="hide">
        <hr />
        <table width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr>
                <td width="50%">
                    <fieldset style="height: 180px">
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
                            <asp:listbox id="drpPriceHead" runat="server" selectionmode="Multiple" style="height: 134px; vertical-align: text-top;">
                                <asp:ListItem Value="0" Text="--Select--" />
                            </asp:listbox>
                            <asp:button id="btnAddCat" text="Add Cateogry to Price Sheet" runat="server"></asp:button>
                        </div>                        
                    </fieldset>
                </td>
                <td><%-- Start Pivotal Tracker # : 95144444 & 96417936 Author : Sumit Kate --%>
                    <fieldset style="height: 180px">
                        <legend>Copy Price Sheet to Other cities</legend>
                        Select State :<span style="color:red">* </span>
                        <asp:dropdownlist id="ddlState" runat="server" />
                        Select cities :<span style="color:red">* </span>
                        <select id="lstCity" multiple="multiple" style="height: 134px; vertical-align: text-top;" ></select>
                        <asp:button id="btnTransferPriceSheet" text="Copy Price Sheet" runat="server" onclientclick="return ConfirmCopy();"></asp:button>
                        <asp:label runat="server" id="lblTransferStatus" class="errorMessage" text="Price Sheet copied successfully."></asp:label>
                    </fieldset>
                    <%-- End Pivotal Tracker # : 95144444 & 96417936 Author : Sumit Kate --%></td>
            </tr>
        </table>
    </div>
    <div id="btnSaveDelete" class="hide">
        <asp:button id="btnUpdate" text="Update" style="background-color: red; color: white;" runat="server" class="margin-right10" onclientclick="return ValidatePrice();"></asp:button>
        <asp:button id="btnDelete" text="Delete" style="background-color: red; color: white;" runat="server" onclientclick="return ConfirmDelete();"></asp:button>
    </div>
    <div><span class="errorMessage margin-top10" id="spnError"></span></div>
    <div id="bindModels">
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
                        </tr>
                </HeaderTemplate>
                <AlternatingItemTemplate>
                        <tr id='<%#DataBinder.Eval(Container.DataItem, "VersionId")%>' style="background-color:#bfbfc6" >
                        <%--<td>
                            <asp:Label style="display:none;" id="lblVersionId" Text='<%# DataBinder.Eval( Container.DataItem, "VersionId" ) %>' runat="server"></asp:Label>
                            <input type="checkbox" versionid='<%#Eval("VersionId") %>' class="checkbox" runat="server" id="chkUpdate" />
                        </td>--%>
                        <td style="text-align: center;">
                            <asp:Label style="display:none;" id="lblVersionId" Text='<%# DataBinder.Eval( Container.DataItem, "VersionId" ) %>' runat="server"></asp:Label>
                            <input type="checkbox" versionid='<%#Eval("VersionId") %>' id="chkUpdate" class="checkboxAll itsGrey" runat="server"/>
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
                        <%--<td class="metAvailable"><%# String.IsNullOrEmpty(Eval("NumOfDays").ToString())?"NA":Eval("NumOfDays") %></td>--%>
                        <td>
                            <span class="spnDays"><%# String.IsNullOrEmpty(Eval("NumOfDays").ToString())?"NA":Eval("NumOfDays") %></span>
                            <asp:Textbox class="metAvailableBlack hide" id="lblAvailableDays" runat="server" style="width:60px;" Text='<%# String.IsNullOrEmpty(Eval("NumOfDays").ToString())?"NA":Eval("NumOfDays") %>'></asp:Textbox>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
                <ItemTemplate>
                    <tr id='<%#DataBinder.Eval(Container.DataItem, "VersionId")%>'>
                        <%--<td>
                            <asp:Label style="display:none;" id="lblVersionId" Text='<%# DataBinder.Eval( Container.DataItem, "VersionId" ) %>' runat="server"></asp:Label>
                            <input type="checkbox" versionid='<%#Eval("VersionId") %>' class="checkbox" runat="server" id="chkUpdate" />
                        </td>--%>
                        <td style="text-align: center;">
                            <asp:Label style="display:none;" id="lblVersionId" Text='<%# DataBinder.Eval( Container.DataItem, "VersionId" ) %>' runat="server"></asp:Label>
                            <input type="checkbox" versionid='<%#Eval("VersionId") %>'  id="chkUpdate" class="checkboxAll" runat="server" />
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
    </FooterTemplate>
        </div>
     <%--   <asp:HiddenField  ID="hdnCityId" runat="server" />
        <asp:HiddenField  ID="hdnMakeId" runat="server" />
        <asp:HiddenField  ID="hdnDealerId" runat="server" />--%>
    <%--<input type="hidden"   ID="hdnCityId" runat="server">--%>
    <input type="hidden" id="hdnMakeId" runat="server">
    <input type="hidden" id="hdnDealerId" runat="server">
    <input type="hidden" id="hdnCities" runat="server">
</form>

<script type="text/javascript">
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
        //$('#btnUpdate').click(function () {
        $("#lblSaved").text("");
        var isError = true;
        $('#spnError').text("");
        $('.checkboxAll').each(function () {
            if ((this).checked) {
                var parentInputRow = $(this).parent().parent().find('input');
                for (var i = 1; i < parentInputRow.length; i++) {
                    //alert(isNaN(parentInputRow[i].value) || parentInputRow[i].value.trim() == "");
                    if (isNaN(parentInputRow[i].value) || parentInputRow[i].value.trim() == "") {
                        $('#spnError').text("Please Enter Numeric Values in Selected Text-Box Field(s)");
                        // alert("inside as");
                        //return false;

                        isError = false;
                        //return false;
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
            //alert($(this).parent().val());
            //parentRow.hasClass("metBlack");
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

            //if ($(this).checked)

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
                //$("#btnSaveDelete").removeClass("hide");
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
        var ABApiHostUrl = '<%= cwHostUrl%>';
        var ddlDealer = $("#drpDealer");
        var selectString = "--Select Dealer--";
        var onInitCity = $("#drpCity option:selected").val();
        if (onInitCity > 0) {
            $.ajax({
                type: "GET",
                url: ABApiHostUrl + "/api/Dealers/GetAllDealers/?cityId=" + onInitCity,
                success: function (response) {
                    ddlDealer.empty().append("<option value=\"0\">" + selectString + "</option>").removeAttr("disabled");
                    for (var i = 0; i < response.length; i++) {
                        ddlDealer.append("<option value=" + response[i].Value + " makeId=" + response[i].MakeId + ">" + response[i].Text + "</option>");
                    }
                }
            });
        }


        $('#drpPriceHead option:selected').click(function () {

            $(this).attr("selected", "selected");
        });
        $("#drpCity").change(function () {
            var cityId = $(this).val();
            $("#hdnCityId").val(cityId);
            //alert("cityId : " + $("#hdnCityId").val());

            if (cityId > 0) {
                $.ajax({
                    type: "GET",
                    url: ABApiHostUrl + "/api/Dealers/GetAllDealers/?cityId=" + cityId,
                    success: function (response) {
                        ddlDealer.empty().append("<option value=\"0\">" + selectString + "</option>").removeAttr("disabled");
                        for (var i = 0; i < response.length; i++) {
                            ddlDealer.append("<option value=" + response[i].Value + " makeId=" + response[i].MakeId + ">" + response[i].Text + "</option>");
                        }
                    }
                });
            }
            else {
                ddlDealer.empty().append("<option value=\"0\">" + selectString + "</option>").removeAttr("enabled");
            }
        });

        $("#drpAllCity").change(function () {
            $("#hdnCityId").val($(this).val());
        })
        $("#btnManageoffer").click(function () {
            $("#bindModels").addClass("hide");
            $("#selectCityPriceHead").addClass("hide");
            var dealerId = $("#drpDealer").val();
            if (dealerId > 0) {
                window.open('/newbikebooking/ManageOffers.aspx?dealerId=' + dealerId + '', 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
            }
            else
                alert("Please select dealer");
        });
        $("#btnManagePrice").click(function () {

            var dealerId = $("#drpDealer").val();
            if (dealerId > 0) {
                var makeId = $("#drpDealer option:selected").attr("makeid");
                $("#hdnDealerId").val(dealerId);
                $("#hdnMakeId").val(makeId);
                $("#selectCityPriceHead").removeClass("hide");
            }
            //alert("dealerId : " + dealerId + "MakeId : " + makeId);
            if (dealerId <= 0) {
                alert("Please select dealer");
            }
        });
        $("#btnManagefacilities").click(function () {
            $("#bindModels").addClass("hide");
            $("#selectCityPriceHead").addClass("hide");
            var dealerId = $("#drpDealer").val();
            if (dealerId > 0) {
                window.open('/newbikebooking/ManageDealerFacilities.aspx?dealerId=' + dealerId + '', 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
            }
            else
                alert("Please select dealer");
        });
        $("#btnMapDealer").click(function () {
            $("#bindModels").addClass("hide");
            $("#selectCityPriceHead").addClass("hide");
            var dealerId = $("#drpDealer").val();
            if (dealerId > 0) {
                window.open('/newbikebooking/ManageDealerAreaMapping.aspx?dealerid=' + dealerId + '', 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
            }
            else
                alert("Please select dealer");
        });
        $("#btngoAvailable").click(function () {
            var dealerId = $("#drpDealer").val();
            if (dealerId > 0) {
                window.open('/newbikebooking/ManageBikeAvailability.aspx?dealerId=' + dealerId + '', 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
            }
            else
                alert("Please select dealer");

        });

        $("#btnEmi").click(function () {
            $("#bindModels").addClass("hide");
            $("#selectCityPriceHead").addClass("hide");
            var dealerId = $("#drpDealer").val();
            if (dealerId > 0) {
                window.open('/newbikebooking/ManageDealerLoanAmounts.aspx?dealerId=' + dealerId + '', 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
            }
            else
                alert("Please select dealer");

        });
        $("#btnDisclaimer").click(function () {
            $("#bindModels").addClass("hide");
            $("#selectCityPriceHead").addClass("hide");
            var dealerId = $("#drpDealer").val();
            if (dealerId > 0) {
                window.open('/newbikebooking/ManageDealerDisclaimer.aspx?dealerId=' + dealerId + '', 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
            }
            else
                alert("Please select dealer");
        });
        $("#btnBkgAmount").click(function () {
            $("#bindModels").addClass("hide");
            $("#selectCityPriceHead").addClass("hide");
            var dealerId = $("#drpDealer").val();
            if (dealerId > 0) {
                window.open('/newbikebooking/ManageBookingAmount.aspx?dealerId=' + dealerId + '', 'mywin', 'scrollbars=yes,left=0,top=0,width=1350,height=600');
            }
            else
                alert("Please select dealer");
        });
    });
</script>
<!-- #Include file="/includes/footerNew.aspx" -->
