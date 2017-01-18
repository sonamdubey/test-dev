<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.Campaign.ManageDealerPriceCategories" EnableViewState="false" %>

<%@ Import Namespace="BikeWaleOpr.Common" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<!-- #Include file="/content/DealerMenu.aspx" -->
<div class="left min-height600">
    <div id="tblPriceCat">
        <table class="margin-top10 margin-bottom10" rules="all" cellspacing="0" cellpadding="5" style="border-width: 1px; border-style: solid; border-collapse: collapse;" id="PriceCatList">
            <tr class="dtHeader">
                <td>Category Id</td>
                <td>Price Category</td>
            </tr>
            <tbody>
                <%foreach (var category in priceCatList)
                  { %>
                <tr class="text-align-center">
                    <td><%=category.PriceCategoryId %></td>
                    <td><%=category.PriceCategoryName %></td>
                </tr>
                <%} %>
            </tbody>
        </table>
    </div>

</div>

<div class="margin-top10" style="position: fixed; right: 80px;">
    <fieldset>
        <legend>
            <h3>Enter a new category</h3>
        </legend>
        <div class="margin-top20">
            Category Name :
            <asp:textbox runat="server" id="txtPriceCat" maxlength="100" runat="server" />
        </div>
        <div class="margin-top20 text-align-center">
            <asp:button id="btnAddCat" text="Add Category" runat="server" />
        </div>
            <div class="margin-top20 margin-bottom10 errorMessage text-align-center">
            <asp:label id="errAddCat" runat="server"></asp:label>
        </div>
    </fieldset>
</div>
<script>
    var txt;
   $(document).ready(function () {
        $('#txtPriceCat').val("");
        var msg = '<%= msg %>';       
        if (msg != "") { showToast(msg); }
    });
    $('#btnAddCat').click(function (e) {
        txt = $('#txtPriceCat').val().trim();
        if (txt == null || txt.length < 1) {
            $('#errAddCat').html("Please add a category");
            return false;
        }
    });
    </script>
    <!-- #Include file="/includes/footerNew.aspx" -->

