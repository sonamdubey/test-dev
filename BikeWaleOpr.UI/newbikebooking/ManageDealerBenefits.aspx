<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ManageDealerBenefits.aspx.cs" Inherits="BikewaleOpr.newbikebooking.ManageDealerBenefits" %>

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
    <h1>Add benefits/ USP</h1>
    <h3>Dealer Name:</h3>
    <form id="addbenefits" runat="server">
        <fieldset class="margin-left10">
            <legend>Add a New Benefit</legend>
            <div>
                <table>
                    <tr>
                        <td class="floatLeft">Select Benefit Category ID<span class="errMessage">*</span>
                        </td>
                        <td class="floatLeft margin-left10">
                            <asp:DropDownList ID="ddlBenefitCat" runat="server" Width="100%" />
                        </td>
                        <td class="floatLeft margin-left10">
                            <span id="spntxtofferType" class="errorMessage"></span>
                        </td>
                        <td class="floatLeft">Add Benefit Text <span class="errMessage">*</span>
                        </td>
                        <td class="floatLeft margin-left10">
                            <asp:TextBox ID="offerText" MaxLength="200" runat="server" Width="95%" />
                        </td>
                        <td class="floatLeft margin-left10">
                            <span class="errorMessage" id="spntxtofferText"></span>
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

        <asp:Repeater ID="rptBenefits" runat="server">
                    <HeaderTemplate>
                        <h1>Added Benefits(s) :</h1>
                        <br />
                        <input class="margin-bottom10 margin-left10" type="button" value="Delete Offers" id="dltBenefits"/>
                        <table border="1" style="border-collapse: collapse;" cellpadding="5" class="margin-left10">
                            <tr style="background-color: #D4CFCF;">
                                <th>
                                    <div>Select All</div>
                                    <div>
                                        <input type="checkbox" runat="server" id="chkAll" />
                                    </div>
                                </th>
                                <th>Id</th>
								<th>Benefit Text</th>
                                <th>Edit</th>
                                <th>Delete Benefit</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="row_<%# DataBinder.Eval(Container.DataItem,"Id") %>">
                            <td><%# DataBinder.Eval(Container.DataItem, "Id") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "BenefitCatId") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "BenefitText") %></td>
                            <td class="update"><a id="update_<%#Eval("OfferId")%>" onclick="javascript:LinkUpdateClick(<%#Eval("Id")%>)">Edit</a></td>
                            <td class="delete" style="text-align: center"><a id="delete_<%#Eval("OfferId")%>" onclick="javascript:btnDelete_Click(<%#Eval("OfferId")%>)">Delete</a></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
    </form>
</body>
</html>
