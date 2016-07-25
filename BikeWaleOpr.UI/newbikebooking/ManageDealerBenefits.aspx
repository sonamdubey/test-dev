<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.NewBikeBooking.ManageDealerBenefits" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="/src/jquery-1.6.min.js" type="text/javascript"></script>
    <script src="/src/AjaxFunctions.js" type="text/javascript"></script>
<%--    <script src="/src/graybox.js"></script>--%>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>
    <link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
    <script type="text/javascript">
        var host = '<%= ConfigurationManager.AppSettings["BwOprHostUrlForJs"]%>';
    </script>
    <style type="text/css">
        .errMessage, .errorMessage {
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
        .greenMessage {
            color:#6B8E23;
        }
    </style>
    <title></title>
</head>
<body>
    <h1>Add benefits/ USP</h1>
    <br />
    <form id="addbenefits" runat="server">
        
        <fieldset class="margin-left10">
            <legend>Add a New Benefit</legend>
            <div id="box" class="box">
                <table>
                    <tr><td colspan="6" class="margin10">
                       <span id="spnError" class="error" runat="server"></span>
                        </td></tr>
                    <tr>
                        <td colspan="2" class="floatLeft">Select Benefit Category ID<span class="errMessage">*</span>
                        </td>
                        <td colspan="2" class="floatLeft margin-left10">
                            <asp:DropDownList ID="ddlBenefitCat" runat="server" Width="100%" />
                        </td>
                        <td colspan="2" class="floatLeft margin-left10">
                            <span id="ddlBenefitCatError" class="errorMessage"></span>
                        </td>
                        
                    </tr>
                    <tr>
                        <td colspan="2" class="floatLeft">Add Benefit Text <span class="errMessage">*</span>
                        </td>
                        <td colspan="2" class="floatLeft margin-left10">
                            <asp:TextBox ID="benefitText" MaxLength="200" runat="server" Width="95%" />
                        </td>
                        <td colspan="2"  class="floatLeft margin-left10">
                            <span class="errorMessage" id="benefitTextError"></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" class="margin10">
                            <asp:Button ID="btnAdd" Text="Save benefit" OnClientClick="return btnAdd_Click();" runat="server" />
                            <asp:Button ID="btnReset" Text="Reset" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
        <asp:Label class="margin-bottom10 margin-left10 greenMessage" ID="greenMessage" runat="server" />
        <asp:Repeater ID="rptBenefits" runat="server">
                    <HeaderTemplate>
                        <h1>Added Benefit(s) :</h1>
                        <br />
                        <input class="margin-bottom10 margin-left10" type="button" value="Delete Benefits" id="dltBenefits"/>
                        <table border="1" style="border-collapse: collapse;" cellpadding="5" class="margin-left10">
                            <tr style="background-color: #D4CFCF;">
                                <th>
                                    <div>Select All</div>
                                    <div>
                                        <input type="checkbox" runat="server" id="chkAll" />
                                    </div>
                                </th>
                                <%--<th>Id</th>--%>
								<th>Benefit Text</th>
                                <th>Category</th>
                                <th>Edit</th>
                                <th>Delete Benefit</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                    <tr id="row_<%# DataBinder.Eval(Container.DataItem,"BenefitId") %>" catId="<%# DataBinder.Eval(Container.DataItem,"CatId") %>" >
                           <td align="center"> <input type="checkbox" class="checkboxAll" id="chkOffer_<%# DataBinder.Eval(Container.DataItem,"BenefitId") %>" benefitId="<%# DataBinder.Eval(Container.DataItem,"BenefitId") %>" /></td>
                            <%--<td><%# DataBinder.Eval(Container.DataItem, "BenefitId") %></td>--%>
                            <td><%# DataBinder.Eval(Container.DataItem, "BenefitText") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "CategoryText") %></td>
                            <td class="update"><a id="update_<%#Eval("BenefitId")%>" onclick="javascript:LinkUpdateClick(<%#Eval("BenefitId")%>)">Edit</a></td>
                            <td class="delete" style="text-align: center"><a id="delete_<%#Eval("BenefitId")%>" onclick="javascript:btnDelete_Click(<%#Eval("BenefitId")%>)">Delete</a></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
        <div id="popup" style="display: none">
            <table>
                    <tr><td colspan="6" class="margin10">
                       <span id="spnEditError" class="error" runat="server"></span>
                        </td></tr>
                    <tr>
                        <td colspan="2" class="floatLeft">Select Benefit Category ID<span class="errMessage">*</span>
                        </td>
                        <td colspan="2" class="floatLeft margin-left10">
                            <asp:DropDownList ID="ddlEditBenefit" runat="server" Width="100%" />
                        </td>
                        <td colspan="2" class="floatLeft margin-left10">
                            <span id="ddlEditBenefitError" class="errorMessage"></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="floatLeft">Add Benefit Text <span class="errMessage">*</span>
                        </td>
                        <td colspan="2" class="floatLeft margin-left10">
                            <asp:TextBox ID="txtEditBenefit" MaxLength="200" runat="server" Width="95%" />
                        </td>
                        <td colspan="2"  class="floatLeft margin-left10">
                            <span class="errorMessage" id="txtEditBenefitError"></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" class="margin10">
                            <asp:HiddenField ID="hdnBenefitId" Value="0" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" class="margin10">
                            <asp:Button ID="btnEditSubmit" Text="Save benefit" OnClientClick="return btnEditSubmit_Click();" runat="server" />
                        </td>
                    </tr>
                </table>
        </div>
    </form>
    <script>
        function btnAdd_Click() {
            $("#spnError").text("");
            $("#benefitTextError").text("");
            $("#ddlBenefitCatError").text("");
            var errorMsg = "";
            var isError = false;
            if ($('#benefitText').val().length == 0){
                $('#benefitTextError').text('Please enter Name');
                isError = true;
            }
            if($('#ddlBenefitCat').val() == "0"){
                $('#ddlBenefitCatError').text('Please select category');
                isError = true;
            }
            $('#hdnBenefitId').val('0');
            return !isError;
        }

        function btnEditSubmit_Click() {
            $("#txtEditBenefitError").text("");
            $("#ddlEditBenefitError").text("");
            var errorMsg = "";
            var isError = false;
            if ($('#txtEditBenefit').val().length == 0) {
                $('#txtEditBenefitError').text('Please enter Name');
                isError = true;
            }
            if ($('#ddlEditBenefit').val() == "0") {
                $('#ddlEditBenefitError').text('Please select category');
                isError = true;
            }
            if (!isError) {
                var url = "/api/Dealers/SaveDealerBenefit/?dealerId=" + <%=_dealerId %> +"&cityId=" + <%=_cityId%> + "&catId=" + $('#ddlEditBenefit').val() + "&benefitText=" + encodeURIComponent($('#txtEditBenefit').val()) + "&userId=" + <%=_currentUserID %> +"&benefitId=" + $('#hdnBenefitId').val(); 
                $.ajax({
                    type: "POST",
                    url: host + url,
                    success: function (response) {
                        window.location.href = window.location.href;
                    },
                    error: function (error) {
                        alert('error: ' + eval(error));
                    }
                });
            }
            //return !isError;
        }

        function btnDelete_Click(id) {
            var acknowledge = confirm("Are you sure you want to delete this record");
            if (acknowledge) {
                $.ajax({
                    type: "GET",
                    url: host + "/api/Dealers/DeleteDealerBenefits/?benefitIds=" + id,
                    success: function (response) {
                        window.location.href = window.location.href;
                    }
                });
            }
        }

        $("#dltBenefits").click(function () {
            var isSuccess = false;
            var benefitIds = '';
            $('.checkboxAll').each(function () {
                if ($(this).is(":checked")) {
                    benefitIds += $(this).attr('BenefitId') + ',';
                }
            });
            if (benefitIds.length > 1) {
                benefitIds = benefitIds.substring(0, benefitIds.length - 1);
                isSuccess = true;
            }

            if (isSuccess)
                btnDelete_Click(benefitIds);
            else
                alert("please select offers to delete.");
        });

        $("#rptBenefits_chkAll").click(function () {
            if ($(this).is(":checked")) {
                $('.checkboxAll').each(function () { this.checked = true; });
            }
            else {
                $('.checkboxAll').each(function () { this.checked = false; });
            }
        });

        function LinkUpdateClick(benefitId) {
            $('#greenMessage').html('');
            $("#txtEditBenefit").val($("#row_" + benefitId).find('td').eq(1).text());
            $('#ddlEditBenefit').val($("#row_" + benefitId).attr('catId'));
            $('#hdnBenefitId').val(benefitId);
            $("#popup").dialog({
                title: "Edit benefit",
                height: 180,
                width:500,
                buttons: {
                    Close: function () {
                        $(this).dialog('close');
                    }
                },
                modal: true
            });
        };
    </script>
</body>
</html>
