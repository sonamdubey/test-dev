<%@ Page Language="C#" Inherits="Carwale.UI.Used.ReportListing" AutoEventWireup="false" Trace="false" %>
<div id="reportListingfrm">                
    <form runat="server">   
        <table border="0" cellpadding="5" cellspacing="0" style="height:280px;">
            <tr>
                <td>Reason</td>
                <td width="10">&nbsp;</td>
                <td>
                    <div class="form-control-box">
                        <span class="select-box fa fa-angle-down"></span>                
                        <asp:DropDownList class="form-control" ID="ddlReason" runat="server"></asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>Description</td>
                <td width="10">&nbsp;</td>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="text form-control" TextMode="multiline" Rows="5" Columns="40" MaxLength="6000" PlaceHolder="Please tell us a little about your experience with the seller/car" Text="Please tell us a little about your experience with the seller/car"></asp:TextBox>
                    <span id="spnDesc" class="note">Characters Left : 6000</span>
                </td>
            </tr>
            <tr>
                <td>Email</td>
                <td width="10">&nbsp;</td>
                <td>
                    <asp:TextBox ID="txtEmail" CssClass="text form-control" runat="server" Columns="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td width="10">&nbsp;</td>
                <td>
                    <a ID="btnReport" class="btn btn-orange">Report</a><div id="process_img" class="process-inline"></div>
                </td>
            </tr>
            <tr>
                <td colspan="3"><span id="spnError" class="error"></span></td>
            </tr>
        </table> 
    </form>
</div>
<div id="showTY"></div>
