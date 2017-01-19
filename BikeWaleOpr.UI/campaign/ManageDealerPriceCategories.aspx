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
                <%if(priceCatList!=null){ %>
                <%foreach (var category in priceCatList)
                  { %>
                <tr class="text-align-center">
                    <td><%=category.PriceCategoryId %></td>
                    <td><%=category.PriceCategoryName %></td>
                </tr>
                <%} %>
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
        <div class="margin-top10 font13">Category Name : </div>
        <div class="margin-top10 form-control-box">
            <asp:textbox runat="server" class="font13" id="txtPriceCat" maxlength="100" height="20px" runat="server" />
            <span class="bwsprite error-icon hide"></span>
            <div class="bw-blackbg-tooltip hide">Please enter a category</div>
        </div>
        <div class="margin-top20 margin-bottom10 ">
            <asp:button id="btnAddCat" text="Add Category" runat="server" />
        </div>
        </fieldset>
</div>
<script>
    var txt;
    var txtPriceCat=$("#txtPriceCat");
   $(document).ready(function () {
        $('#txtPriceCat').val("");
        var msg = '<%= msg %>';       
        if (msg != "") { showToast(msg); }
    });
    $('#btnAddCat').click(function (e) {
        txt = $('#txtPriceCat').val().trim();
        if (txt == null || txt.length < 1) {
            showHideMatchError(txtPriceCat,true);
            return false;
        }
    });
    txtPriceCat.focus(function () {
      showHideMatchError(txtPriceCat, false);
    });
    </script>
    <!-- #Include file="/includes/footerNew.aspx" -->

