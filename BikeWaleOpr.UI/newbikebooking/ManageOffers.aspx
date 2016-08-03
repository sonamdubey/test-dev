<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.BikeBooking.ManageOffers" Async="true" Trace="false" EnableEventValidation="false" %>

<%@ Register TagPrefix="dt" TagName="DateControl" Src="/Controls/DateControl.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="/src/jquery-1.6.min.js" type="text/javascript"></script>
    <script src="/src/AjaxFunctions.js" type="text/javascript"></script>
    <script src="/src/graybox.js"></script>
    <script language="javascript" src="/src/calender.js"></script>
    <link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
    <style type="text/css">
        .errMessage {
            color: #FF4A4A;
        }

        .delete, .update {
            cursor: pointer;
        }

        .expired {
            background-color: #FF4A4A;
        }

        .yellow {
            background-color: #ffff48;
        }
    </style>
    <title></title>
</head>
<body>
    <h1>Bike Offers</h1>
    <form id="AddOffer" runat="server">
        <span id="spnError" class="error" runat="server"></span>
        <div class="" style="width: 1000px;">
            <table width="100%" border="0" cellpadding="2" cellspacing="0">
                <tr>
                    <td>
                        <fieldset class="margin-left10">
                            <legend>Add New Offer</legend>
                            <div>
                                <table>
                                    <tr>
                                        <td colspan="2" class="floatLeft">Select City <span class="errMessage">*</span>
                                        </td>
                                        <td colspan="2" class="floatLeft margin-left10"><asp:DropDownList ID="drpCity" runat="server" Width="100%" />
                                        </td>
                                        <td colspan="2" class="floatLeft margin-left10"><span id="spntxtCity" class="errorMessage"></span>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="floatLeft" >Select Make <span class="errMessage">*</span>
                                        </td>
                                        <td class="floatLeft margin-left10">
                                            <asp:DropDownList ID="DropDownMake" runat="server" Width="100%" />
                                        </td>
                                        <td class="floatLeft margin-left10">
                                            <span id="spntxtMake" class="errorMessage"></span>
                                        </td>
                                        <td class="floatLeft">Select Model <span class="errMessage">*</span>
                                        </td>
                                        <td class="floatLeft margin-left10"><asp:DropDownList ID="DropDownModels" multiple="multiple" runat="server" Width="100%" />
                                        </td>
                                        <td class="floatLeft margin-left10">
                                            <span id="spntxtModel" class="errorMessage"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="floatLeft">Offer Type <span class="errMessage">*</span>
                                        </td>
                                        <td class="floatLeft margin-left10">
                                            <asp:DropDownList ID="drpOffers" runat="server" Width="100%" />
                                        </td>
                                        <td class="floatLeft margin-left10">
                                            <span id="spntxtofferType" class="errorMessage"></span>
                                        </td>
                                        <td class="floatLeft">Offer Text <span class="errMessage">*</span>
                                        </td>
                                        <td class="floatLeft margin-left10">
                                            <asp:TextBox ID="offerText" runat="server" Width="95%" />
                                        </td>
                                        <td class="floatLeft margin-left10">
                                            <span class="errorMessage" id="spntxtofferText"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="floatLeft">Offer Value :
                                        </td>
                                        <td colspan="2" class="floatLeft margin-left10">
                                            <asp:TextBox ID="offerValue" runat="server" Width="95%" />
                                        </td>
                                        <td colspan="2" class="floatLeft margin-left10">
                                            <span class="errorMessage" id="spnofferValue"></span>
                                        </td>
                                    </tr>
                                    <tr class="floatLeft margin-left">
                                        <td colspan="2">Offer Valid Till  <span class="errMessage">*</span></td>
                                        <td colspan="2">
                                            <dt:DateControl ID="dtDate" FutureTolerance="2" runat="server" />
                                            <asp:DropDownList ID="ddlHours" runat="server" /><asp:DropDownList ID="ddlMins" runat="server" />
                                        </td>
                                        <td colspan="2">
                                            <span id="spnoffervalidTill" class="errMessage"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="floatLeft">Is Price Impact</td>
                                        <td colspan="2" class="floatLeft margin-left10"><asp:CheckBox ID="chkIsPriceImpact" runat="server" /></td>
                                       <td colspan="2" class="floatLeft margin-left10">
                                           
                                        </td>
                                    </tr>
                                    </tr>
                                  
                                   <tr>
                                       
                                       <td colspan="2" class="floatLeft">Terms and Conditions 
                                        </td>
                                        <td colspan="2" class="floatLeft margin-left10">
                                            <asp:TextBox ID="txtAreaTerms" maxlength="4096" TextMode="Multiline" height="150px" wrap="true" runat="server" Width="200%" />
                                        </td>
                                   </tr>
                

                                    <tr>
                                        <td colspan="6" class="margin10">
                                            <asp:Button ID="btnAdd" Text="Add Offer" OnClientClick="return btnAdd_Click();" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                    <td>
                        <table><tr><td>
                            <%-- Start Pivotal Tracker # : 95410582 Author : Sumit Kate --%>
                        <fieldset class="margin-left10" >
                            <legend>Copy Offers to other Cities</legend>
                            <table><tr><td>Select State : <span class="errMessage">*</span></td>
                            <td><asp:DropDownList ID="ddlState" runat="server" /></td><td>Select cities : <span class="errMessage">*</span></td><td>
                            <select id="lstCity" multiple="multiple" style="height: auto"></select></td></tr>
                            <tr><td><asp:Label runat="server" ID="lblTransferStatus" class="errorMessage" Text="Offer/s copied successfully."></asp:Label></td>
                                </tr>                            
                            <tr><td><asp:Button ID="btnCopyOffers" Text="Copy Offers" runat="server" OnClientClick="return confirmCopy();"></asp:Button></td>
                                    </tr></table>
                        </fieldset>
                        <%-- End Pivotal Tracker # : 95410582 Author : Sumit Kate --%></td></tr></table>
                    </td>
                </tr>
            </table>
            <div class="hide" id="updHtml" style="float: left;">
                <div>Offer Type <span class="errMessage ">*</span><asp:DropDownList ID="ddlUpdOffers" runat="server" /></div>
                <div class="margin-top10"><span id="spnTxtUpdOfferType" class="errorMessage"></span></div>
                <div class="margin-top10">Offer Text <span class="errMessage">*</span><asp:TextBox ID="txtUpdOffer" runat="server" /></div>
                <div><span id="spnTxtUpdOfferText" class="errorMessage"></span></div>
                <div class="margin-top10">Offer Value :<asp:TextBox ID="txtUpdOfferValue" runat="server" /></div>
                <div><span id="spnTxtUpdOfferValue" class="errorMessage"></span></div>
                <div class="margin-top10">
                    Offer Valid Till  <span class="errMessage">*</span><dt:DateControl ID="updDtDate" FutureTolerance="2" runat="server" />
                    <div><span id="spnTxtUpdOfferValidity" class="errorMessage"></span></div>
                </div>
                <div class="margin-top10">Is Price Impact Value : <input type="checkbox" runat="server" id="chkPopup" /></div>
                <div class="margin-top10">Terms and Conditions : </div>
                <textarea class="margin-top10" style="height:150px; width:100%"  runat="server" id="txtTerms" ></textarea>
                <div class="margin-top10">
                    <asp:Button ID="btnUpdate" Text="Update" runat="server" />
                </div>
            </div>
            <%--<asp:HiddenField id="hdn_cityId" runat="server"/>
            <asp:HiddenField id="hdn_offerType" runat="server"/>--%>
            <asp:HiddenField ID="hdn_modelId" runat="server" />
            <%--<asp:HiddenField id="hdn_dtDate" runat="server"/>
            <asp:HiddenField id="hdn_dtMonth" runat="server"/>
            <asp:HiddenField id="hdn_ddlHours" runat="server"/>
            <asp:HiddenField id="hdn_ddlMins" runat="server"/>--%>
            <asp:HiddenField ID="hdnCities" runat="server" />
            <asp:HiddenField ID="hdnOffersIds" runat="server" />
            <div id="AddedOffers">
                <div class="errorMessage">
                    <asp:Label runat="server" ID="lblSaved"></asp:Label>
                </div>
                <asp:Repeater ID="offer_table" runat="server">
                    <HeaderTemplate>
                        <h1>Added Offer(s) :</h1>
                        <br />
                        <input class="margin-bottom10 margin-left10" type="button" value="Delete Offers" id="dltOffers"/>
                        <table border="1" style="border-collapse: collapse;" cellpadding="5" class="margin-left10">
                            <tr style="background-color: #D4CFCF;">
                                <th>
                                    <div>Select All</div>
                                    <div>
                                        <input type="checkbox" runat="server" id="chkAll" />
                                    </div>
                                </th>
                                <th>Id</th>
                                <th>Make</th>
                                <th>Model</th>
                                <th>City</th>
                                <th>Offer Type</th>
                                <th>Offer Text</th>
                                <th>Offer Value</th>
                                <th>Offer Valid Till [mm/dd/yyyy]</th>
                                <th>Is Price Impact</th>
                                <th>Edit</th>
                                <th>Delete Offer</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="row_<%# DataBinder.Eval(Container.DataItem,"OfferId") %>" class='<%# DataBinder.Eval(Container.DataItem, "OfferValidTill") %> < <%# System.DateTime.Now%> ? "expired" : ""'>
                            <td>
                                <input type="checkbox" class="checkboxAll" id="chkOffer_<%# DataBinder.Eval(Container.DataItem,"OfferId") %>" offerId="<%# DataBinder.Eval(Container.DataItem,"OfferId") %>" /></td>
                            <td><%# DataBinder.Eval(Container.DataItem,"OfferId") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem,"objMake.MakeName") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem,"objModel.ModelName") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem,"objCity.CityName") %></td>
                            <td id="offerType_<%# DataBinder.Eval(Container.DataItem,"OfferId") %>" offerid="<%#DataBinder.Eval(Container.DataItem,"offerTypeId") %>"><%# DataBinder.Eval(Container.DataItem,"OfferType") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "OfferText") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "OfferValue") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "OfferValidTill") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "isPriceImpact") %></td>
                            <td class="hide"><%# DataBinder.Eval(Container.DataItem, "Terms") %></td>
                            <td class="update"><a id="update_<%#Eval("OfferId")%>" onclick="javascript:LinkUpdateClick(<%#Eval("OfferId")%>)">Edit</a></td>
                            <td class="delete" style="text-align: center"><a id="delete_<%#Eval("OfferId")%>" onclick="javascript:btnDelete_Click(<%#Eval("OfferId")%>)">Delete</a></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
    </form>
    <br />
    <script>

        <%--Start Pivotal Tracker # : 95410582
            Created by Sumit Kate on 12/09/2015
            Purpose : To ask for Transfer Price Sheet confimation before submitting the form.
        --%>
        function confirmCopy() {
            var acknowledge = false;
            var stateId = $("#ddlState").val();
            var strOfferId = "";
            var strCityId = "";
            var $hdnOffersIds = $("#hdnOffersIds");
            var $hdnCities = $("#hdnCities");
            $hdnOffersIds.val('');
            $(".checkboxAll:checked").each(function () { strOfferId += $(this).attr("id").split("_")[1] + ","; });
            $("#lstCity option:selected").each(function () { strCityId += this.value + ","; });

            strOfferId = strOfferId.substring(0, strOfferId.length - 1);
            strCityId = strCityId.substring(0, strCityId.length - 1);

            if ((stateId > 0) && (strOfferId != '') && (strCityId != '')) {
                $hdnOffersIds.val(strOfferId);
                $hdnCities.val(strCityId);
                acknowledge = confirm("Do you want to copy the selected cities offers to selected cities.");
                if (acknowledge) {
                    return acknowledge;
                }
            }
            else {
                alert("Please select State, Cities and Offers.");
            }
            return acknowledge;
        }

        $("#offer_table_chkAll").click(function () {
            if ($(this).is(":checked")) {
                $('.checkboxAll').each(function () { this.checked = true; });
            }
            else {
                $('.checkboxAll').each(function () { this.checked = false; });
            }
        });
        $("#ddlState").change(function () { loadStateCities(); });
        <%--
    Created by Sumit Kate on 12/09/2015
    Purpose : To Load all cities of the state from CarWale DB using AjaxPro API call
    --%>
        function loadStateCities() {
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

    <%--End Pivotal Tracker # : 95410582 Author : Sumit Kate--%>

        $(document).ready(function () {
            if ($("#DropDownMake").val() > 0)
            {
                GetModels($("#DropDownMake"));
            }

            $("#DropDownMake").change(function () {
                $("#lblSaved").text("");
                $("#DropDownModels").val("0").attr("disabled", "disabled");
                GetModels(this);
            });

            if ($("#ddlState").val() > 0) {
                loadStateCities();
            }
        });

        function GetModels(e) {
            var makeId = $(e).val();
            var reqType = 'NEW';
            if (makeId > 0) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"makeId":"' + makeId + '" , "requestType":"' + reqType + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        bindDropDownList(resObj, $("#DropDownModels"), "", "--Select Model--");
                    }
                });
            } else {
                $("#DropDownModels").val("0").attr("disabled", "disabled");
            }
        }

        function btnDelete_Click(offerId) {
            var host = '<%= ConfigurationManager.AppSettings["BwOprHostUrlForJs"] %>';
            var acknowledge = confirm("Are you sure you want to delete this record");
            if (acknowledge) {
                $.ajax({
                    type: "GET",
                    url: host + "/api/Dealers/DeleteDealerOffer/?offerId=" + offerId,
                    success: function (response) {
                            window.location.href = window.location.href;
                    }
                });
            }
        }

        function btnAdd_Click() {
            $("#spntxtCity").text("");
            $("#spntxtModel").text("");
            $("#spntxtMake").text("");
            $("#spntxtofferType").text("");
            $("#spntxtofferText").text("");
            $("#spnoffervalidTill").text("");
            $("#spnofferValue").text("");

            var date = new Date();
            var month = date.getMonth();
            var day = date.getDate();

            var currentDate = new Date(date.getFullYear(), (month < 10 ? '0' : '') + month, (day < 10 ? '0' : '') + day);
            var enteredDay = $("#dtDate_cmbDay").val();
            var enteredmonth = $("#dtDate_cmbMonth").val();
            var enteredYear = $("#dtDate_txtYear").val();

            var completeDate = new Date(enteredYear, (enteredmonth < 10 ? '0' : '') + enteredmonth - 1, (enteredDay < 10 ? '0' : '') + enteredDay);

            var isError = false;

            if (isNaN($("#offerValue").val())) {
                $("#spnofferValue").text("Please Enter Numeric Values");
                isError = true;
            }
            if (completeDate < currentDate) {
                $("#spnoffervalidTill").text("Offers can't be Added for Past Dates");
                isError = true;
            }
            if ($("#drpCity option:selected").val() <= '0') {
                $("#spntxtCity").text("Please Select City");
                isError = true;
            }
            if ($("#DropDownMake option:selected").val() <= '0') {
                $("#spntxtMake").text("Please Select Make");
                isError = true;
            }
            if ($("#DropDownModels option:selected").length <= 0) {
                $("#spntxtModel").text("Please Select Model");
                isError = true;
            }
            if ($("#drpOffers option:selected").val() <= 0) {
                $("#spntxtofferType").text("Please Select Offer type");
                isError = true;
            }
            if (jQuery.trim($("#offerText").val()).length == 0) {
                $("#spntxtofferText").text("Offer Text Required");
                isError = true;
            }

            if (!isError)
            {
                var strModelId = '';
                $('#DropDownModels option:selected').each(function () {
                    strModelId += this.value + ',';
                });

                strModelId = strModelId.substring(0, strModelId.length-1);

                $("#hdn_modelId").val(strModelId);
            }

            return !isError;
        }

        function LinkUpdateClick(offerIdVal) {
            $("#lblSaved").text("");
            var html = $("#updHtml").html();
            var title = "<h3>" + $("#row_" + offerIdVal).find('td').eq(2).text() + " " + $("#row_" + offerIdVal).find('td').eq(3).text() + "</h3>";
            html = title + html;
            var host = '<%= ConfigurationManager.AppSettings["BwOprHostUrlForJs"]%>';
            var offerId = offerIdVal;
            var url = "";
            var applyIframe = true;
            var caption = "Edit Bike Offer";
            var GB_Html = html;
            
            var selectedMonth = $("#row_" + offerId).find('td').eq(8).text().split("/")[0];
            var selectedDay = $("#row_" + offerId).find('td').eq(8).text().split("/")[1];
            var selectedYear = $("#row_" + offerId).find('td').eq(8).text().split("/")[2].split(" ")[0];
            GB_show(caption, url, 200, 500, applyIframe, GB_Html);

            $("#ddlUpdOffers").val($("#row_" + offerId).find('td').eq(5).attr("offerid"));
            $("#txtUpdOffer").val($("#row_" + offerId).find('td').eq(6).text());
            $("#txtUpdOfferValue").val($("#row_" + offerId).find('td').eq(7).text());
            $("#updDtDate_cmbDay").val(selectedDay);
            $("#updDtDate_cmbMonth").val(selectedMonth);
            $("#updDtDate_txtYear").val(selectedYear);
            $("#txtTerms").val($("#row_" + offerId).find('td').eq(10).text());
            if ($("#row_" + offerId).find('td').eq(9).html() == 'True')
                $("#chkPopup").prop('checked', true);

            $("#btnUpdate").click(function () {
                $("#spnTxtUpdOfferType").text("");
                $("#spnTxtUpdOfferText").text("");
                $("#spnTxtUpdOfferValidity").text("");
                $("#spnTxtUpdOfferValue").text("");

                var isError = false;
                var enteredDay = $("#updDtDate_cmbDay").val();
                var enteredmonth = $("#updDtDate_cmbMonth").val();
                var enteredYear = $("#updDtDate_txtYear").val();
                
                if ($("#ddlUpdOffers option:selected").val() <= '0') {
                    $("#spnTxtUpdOfferType").text("Please Select Offer type");
                    isError = true;
                }
                if (isNaN($("#txtUpdOfferValue").val())) {
                    $("#spnTxtUpdOfferValue").text("Please Enter Only Numeric Values");
                    isError = true;
                }
                if (jQuery.trim($("#txtUpdOffer").val()).length == 0) {
                    $("#spnTxtUpdOfferText").text("Offer Text can't be empty");
                    isError = true;
                }
                var isPrcImpct = false;
                if ($("#chkPopup").prop("checked"))
                {
                    isPrcImpct = true;
                }
                if (!isError) {
                    $.ajax({
                        type: "POST",
                        url: host + "/api/Dealers/UpdateDealerBikeOffers/?offerId=" + offerId + "&userId=" + <%=userId%> + "&offerCategoryId=" + $("#ddlUpdOffers option:selected").val() + "&offertext=" + encodeURIComponent($("#txtUpdOffer").val()) + "&offerValue=" + $("#txtUpdOfferValue").val() + "&offerValidTill=" + enteredYear + "-" + enteredmonth + "-" + enteredDay + "&isPriceImpact=" + isPrcImpct + "&terms=" + encodeURIComponent($("#txtTerms").val()) ,
                        success: function () {
                            $("#gb-content").html("Offers updated Successfully, Please Close this Box");
                        }
                    });

                    $("#gb-close").click(function () {
                        window.location.href = window.location.href;
                    });
                }
                return !isError;
            });
        }

        $("#dltOffers").click(function () {
            var isSuccess = false;
            var offerIds = '';
            $('.checkboxAll').each(function () {
                if ($(this).is(":checked")) {
                    offerIds += $(this).attr('offerId') + ',';
                }
            });


            if (offerIds.length > 1) {
                offerIds = offerIds.substring(0, offerIds.length - 1);
                isSuccess = true;
            }
            
            if (isSuccess)
                btnDelete_Click(offerIds);
            else
                alert("please select offers to delete.");
        });
    </script>
</body>
</html>
