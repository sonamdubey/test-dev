<%@ Page Language="C#" Inherits="BikeWaleOpr.Content.ViewReview" AutoEventWireup="false" Trace="false" Debug="false" ValidateRequest="false" %>
<%@ Register Src="/controls/RichTextEditor.ascx" TagPrefix="ucRTE" TagName="RichTextEditor" %>
<form runat="server">
<link href="/css/Common.css?V1.1" rel="stylesheet" />
<script src="/src/jquery-1.6.min.js" type="text/javascript"></script>
<script src="/src/AjaxFunctions.js" type="text/javascript"></script>
<div>
    <div id="errMsg" runat="server" class="margin-top10"></div>
    <div id="divShowReview" runat="server">
        <table class="lstTable" border="1" style="width: 100%;" cellpadding="5">
            <tr>
                <th style="width:300px;">Exterior/Style</th>
                <td><asp:TextBox ID="txtExterior" runat="server" MaxLength="1" Columns="1" /><asp:label id="exterior" runat="server"></asp:label></td>
            </tr>
            <tr>
                <th>Comfort & Space</th>
                <td><asp:TextBox ID="txtComfort" runat="server" MaxLength="1" Columns="1" /><asp:label id="Comfort" runat="server"></asp:label></td>
            </tr>
            <tr>
                <th>Performance(Engine, gearbox & overall)</th>
                <td><asp:TextBox ID="txtPerformance" runat="server" MaxLength="1" Columns="1" /><asp:label id="Performance" runat="server"></asp:label></td>
            </tr>
            <tr>
                <th>Fuel Economy (km/l)</th>
                <td><asp:TextBox ID="txtFuel" runat="server" MaxLength="1" Columns="1" /><asp:label id="Fuel" runat="server"></asp:label></td>
            </tr>
            <tr>
                <th>Value for money/Features</th>
                <td><asp:TextBox ID="txtValue" runat="server" MaxLength="1" Columns="1" /><asp:label id="Value" runat="server"></asp:label></td>
            </tr>                
            <tr>
                <th>Title</th>
                <td><asp:TextBox ID="txtTitle" runat="server" style="width:100%" /></td>
            </tr>
            <tr>
                <th>Pros</th>
                <td><asp:TextBox ID="txtPros" runat="server" style="width:100%" /></td>
            </tr>
            <tr>
                <th>Cons</th>
                <td><asp:TextBox ID="txtCons" runat="server" style="width:100%" /></td>
            </tr>
            <tr>
                <th>Detailed Review</th>
                <td><ucRTE:RichTextEditor runat="server" ID="rteDetail"  Rows="15" Cols="20" title="Maximum 8000 characters (approx. 2000 words). Minimum 50 words" /></td>
            </tr>
            <tr>
                <th>Newly Purchased</th>
                <td><asp:label id="Purchased" runat="server" class="margin-left5"></asp:label></td>
            </tr>
            <tr>
                <th>Familiarity with the bike</th>
                <td><asp:TextBox ID="txtFamiliarity" runat="server" MaxLength="1" Columns="1" /><asp:label id="Familiarity" runat="server"></asp:label></td>
            </tr>
            <tr>
                <th>Fuel Economy(mileage)</th>
                <td><asp:TextBox ID="txtMileage" runat="server" MaxLength="3" Columns="3" /><asp:label id="mileage" runat="server"></asp:label></td>
            </tr>        
            <tr>
                <td colspan="2"><asp:Button ID="btnUpdateReview" runat="server" Text="Update Review" class="margin-top10 margin-bottom10" />
                <input type="button" id="btn_approve" reviewId="<%=reviewId %>" onclick="approve_click(event)" value="Approve"  <%= isVerified ? "disabled" : "" %> >           
                <input type="button" id="btn_discard" reviewId="<%=reviewId %>" onclick="discard_click(event)" value="Discard" <%= isDiscarded ? "disabled" : "" %>>
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            function approve_click(e) {
                var Id = $('#btn_approve').attr('reviewId');
                var r = confirm("Approve this Review");

                if (r) {
                    $.ajax({
                        type: "POST",
                        url: "/ajaxpro/BikeWaleOpr.ReviewDetail,BikewaleOpr.ashx",
                        data: '{"Id":"' + Id + '"}',
                        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SetIsReviewed"); },
                        
                        success: function (response) {
                            alert("Review Approved Successfully");
                            $("#" + Id).remove();
                            window.close();
                            window.opener.location.reload();
                        }
                    });
                }
                
            }

            function discard_click(e) {
                var Id = $('#btn_discard').attr('reviewId');
                var r = confirm("Discard this Review");

                if (r) {
                    $.ajax({
                        type: "POST",
                        url: "/ajaxpro/BikeWaleOpr.ReviewDetail,BikewaleOpr.ashx",
                        data: '{"Id":"' + Id + '"}',
                        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SetIsDiscarded"); },
                        success: function (response) {
                            alert("Review Discarded Successfully");
                            $("#" + Id).remove();
                            window.opener.location.reload();
                            window.close();
                        }
                    });
                }
            }
        </script>
    </div>
</div>
</form>
       