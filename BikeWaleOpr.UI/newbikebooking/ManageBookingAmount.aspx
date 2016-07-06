<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.BikeBooking.ManageBookingAmount" Debug="true" EnableEventValidation="false" Async="true" Trace="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Booking Amount</title>
    <link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
    <script type="text/javascript" src="http://st.carwale.com/jquery-1.7.2.min.js?v=1.0" ></script>    
    <script src="/src/AjaxFunctions.js" type="text/javascript"></script>
    <script src="/src/graybox.js"></script>
    <style>
        .yellow{background-color:#ffff48}
    </style>
</head>
<body>
    <form runat="server">
        <h1 class="margin-left10">Manage Booking Amount :</h1>
        <fieldset class="margin-left10 margin-top10 padding10">
	        <legend >Add New Booking Amount</legend>            
            <div>
                Select Make<span class="errorMessage">*</span> : 
                <asp:DropDownList ID="ddlMake" runat="server" CssClass="margin-right5"/>
                <span id="spntxtMake" class="errorMessage margin-right5"></span>
                Select Model<span class="errorMessage">*</span> :
                <asp:DropDownList ID="ddlModel" runat="server" CssClass="margin-right5">
                <asp:ListItem value="0">--Select Model--</asp:ListItem>
                </asp:DropDownList>
                <span id="spntxtModel" class="errorMessage margin-right5"></span>
                Select Version : 
                <asp:DropDownList ID="ddlVersions" runat="server">
                    <asp:ListItem value="0">--Select Versions-- </asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="margin-top10">
                Enter Booking Amount<span class="errorMessage">*</span> :<asp:TextBox runat="server" ID="txtAddBkgAmount" />
                <span id="spntxtValidAmount" class="errorMessage"></span>
            </div>
            <div class="margin-top10">
                <asp:Button ID="btnAddAmount" runat="server" Text="Save" />
            </div>            
        </fieldset>
        <div class="hide" id="updHtml" style="float:left;">
            New Booking Amount<span class="errorMessage">*</span> :<asp:TextBox ID="txtEditAmount" runat="server"/>&nbsp;                            
                            <asp:Button ID="btnEdit" Text="Update" runat="server" />
                            <span class="errorMessage" id="spnEditText"></span>
        </div>
        <asp:HiddenField id="hdn_ddlMake" runat="server"/>
        <asp:HiddenField id="hdn_ddlModel" runat="server"/>
        <asp:HiddenField id="hdn_ddlVersions" runat="server"/>
        <asp:HiddenField id="hdn_BkgAmount" runat="server"/>

        <div id="dataTable">        
            <asp:Label runat="server" ID="lblMessage" class="errorMessage" ></asp:Label>
            <asp:Repeater id="rptAddedBkgAmount" runat="server">
                <HeaderTemplate>
                 <h3 class="margin-left10 margin-bottom10;">Added Booking Amount :</h3>
                 <table border="1" style="border-collapse:collapse;" cellpadding ="5" class="margin-left10 lstTable" >
                    <tr>
                        <th>Id</th>
                        <th>Make</th>
                        <th>Model</th>
                        <th>Version</th>
                        <th>Booking Amount</th>
                        <th>Edit Amount</th>
                        <th>Delete</th>
                    </tr>
                 </HeaderTemplate>
                 <ItemTemplate> 
                    <tr id="row_<%# DataBinder.Eval(Container.DataItem,"objBookingAmountEntityBase.id") %>">
                        <td><%# DataBinder.Eval(Container.DataItem,"objBookingAmountEntityBase.id") %></td>                                        
                        <td class="make_<%#Eval("objBookingAmountEntityBase.id")%>"><%# DataBinder.Eval(Container.DataItem,"objMake.MakeName") %></td>                                            
                        <td class="model_<%#Eval("objBookingAmountEntityBase.id")%>"><%# DataBinder.Eval(Container.DataItem,"objModel.ModelName") %></td> 
                        <td class="version_<%#Eval("objBookingAmountEntityBase.id")%>"><%# DataBinder.Eval(Container.DataItem,"objVersion.VersionName") %></td>                                  
                        <td class="amt_<%#Eval("objBookingAmountEntityBase.id")%>" style="text-align:center"><%# DataBinder.Eval(Container.DataItem,"objBookingAmountEntityBase.amount") %></td>
                        <td class="edit" style="text-align:center"><a onclick="javascript:EditClick(<%#Eval("objBookingAmountEntityBase.id")%>)" class="pointer">Edit</a></td>
                        <td class="delete" style="text-align:center"><a onclick="javascript:DeleteClick(<%#Eval("objBookingAmountEntityBase.id")%>)" class="pointer">Delete</a></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#ddlMake").val() > 0) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"requestType":"' + 'NEW' + '","makeId":"' + $("#ddlMake").val() + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        bindDropDownList(resObj, $("#ddlModel"), "", "--Select Models--");
                    }
                });
            }
            $("#ddlMake").change(function () {
                $("#hdn_ddlMake").val($(this).val());
                $("#hdn_ddlModel").val(0);
                $("#hdn_ddlVersions").val(0);
                $("#ddlModel").val("0").attr("disabled", "disabled");
                GetModels(this);
            });
            $("#ddlModel").change(function () {
                $("#hdn_ddlModel").val($(this).val());
                $("#hdn_ddlVersions").val(0);
                $("#ddlVersions").val("0").attr("disabled", "disabled");
                GetVersions(this);
            });
            $("#ddlVersions").change(function () {
                $("#hdn_ddlMake").val($(this).val());
                $("#hdn_ddlVersions").val($(this).val());
            });
            $("#btnAddAmount").click(function () {
                $("#spntxtMake").text("");
                $("#spntxtModel").text("");
                $("#spntxtValidAmount").text("");
                var validNum = /^\d{0,9}(\.\d{0,9}){0,9}$/;
                var amt = $("#txtAddBkgAmount").val().trim();
                $("#txtAddBkgAmount").focus();
                var isValid = true;
                if ($("#ddlMake option:selected").val() <= '0') {
                    $("#spntxtMake").text("Please Select Make");
                    isValid = false;
                }
                else if ($("#ddlModel option:selected").val() <= '0') {
                    $("#spntxtModel").text("Please Select Model");
                    isValid = false;
                }
                else if (amt.trim() == "") {
                    isValid = false;
                    $("#spntxtValidAmount").text("Please Enter Amount");
                }
                else if (!validNum.test(amt)) {
                    $("#spntxtValidAmount").text("Please Enter only numeric values");
                    isValid = false;
                }
                return isValid;
            });
        });
        function GetModels(e) {
            var makeId = $(e).val();
            var requestType = 'NEW';
            if (makeId > 0) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"requestType":"' + requestType + '","makeId":"' + makeId + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        bindDropDownList(resObj, $("#ddlModel"), "", "--Select Models--");
                    }
                });
            } else {
                $("#ddlModel").val("0").attr("disabled", "disabled");
            }
        }

        function GetVersions(e) {
            var modelId = $(e).val();

            if (modelId > 0) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"modelId":"' + modelId + '","requestType":"NEW"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetVersions"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        bindDropDownList(resObj, $("#ddlVersions"), "", "--Select Version--");
                    }
                });
            } else
                $("#ddlModel").val("0").attr("disabled", "disabled");
        }

        var BwOprHostUrl = '<%=ConfigurationManager.AppSettings["BwOprHostUrlForJs"]%>';

        function DeleteClick(bookingId) {
            $("#row_"+bookingId).addClass("yellow");
            var acknowledge = confirm("Are you sure you want to delete this record");
            alert(acknowledge);
            if (acknowledge)
            {
                $.ajax({
                    type: "GET",
                    url: BwOprHostUrl + "/api/Dealers/DeleteBookingAmount/?bookingId=" + bookingId,
                    success: function (response) {
                       window.location.href = window.location.href;
                    }
                });
            }
            $("#row_" + bookingId).removeClass("yellow");
        }

        function EditClick(bookingId) {

            var html = $("#updHtml").html();
            var title = "<h3>" + $(".make_" + bookingId).text() + " " + $(".model_" + bookingId).text() + " " + $(".version_" + bookingId).text() + "</h3>";
            html = title + html;

            var bkgId = bookingId;
            var url = "";
            var applyIframe = true;
            var caption = "Edit Booking Amount";
            var GB_Html = html;

            GB_show(caption, url, 200, 500, applyIframe, GB_Html);
            $("#txtEditAmount").val($('.amt_' + bookingId).text());
            $("#txtEditAmount").focus();

            $("#btnEdit").click(function () {
                $("#spnEditText").text("");
                var validNum = /^\d{0,9}(\.\d{0,9}){0,9}$/;
                var newAmount = $("#txtEditAmount").val().trim();
                var isError = false;

                if (newAmount.trim() == "") {
                    isError = true;
                    $("#spnEditText").text("Booking amount can't be empty");
                    return !isError;
                }
                else if (!validNum.test(newAmount)) {
                    isError = true;
                    $("#spnEditText").text("Please Enter only Numeric values");
                    return !isError;
                }
                if (newAmount != "" && newAmount != null) {
                    //var objBooking = { Id: bookingId, Amount: parseInt(newAmount), IsActive: true };
                    //var jsonStr = JSON.stringify(objBooking);
                    $.ajax({
                        type: "GET",
                        //contentType: "application/json; charset=utf-8",
                       // data: "'"+jsonStr+"'",
                        url: BwOprHostUrl + "/api/Dealers/UpdateBookingAmount/?bookingId=" + bkgId + '&amount=' + parseInt(newAmount),
                        success: function (response) {
                            $("#gb-content").html("Booking amount updated Successfully, Please Close this Box");
                        }
                    });

                }

                $("#gb-close").click(function () {
                    window.location.href = window.location.href;
                });
            })
        }
    </script>
        </form>
</body>
</html>
