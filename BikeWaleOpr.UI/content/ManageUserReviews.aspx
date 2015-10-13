<%@ Page Language="C#" Inherits="BikeWaleOpr.Content.ManageUserReviews" AutoEventWireup="false" Trace="false" Debug="false" EnableEventValidation="false" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<div class="urh">
		You are here &raquo; <a href="/content/default.aspx">Contents</a> &raquo; Manage User Reviews
</div>
<div>
    <!-- #Include file="contentsMenu.aspx" -->
</div>
<div class="left">
    <h3>Manage Reviews</h3>
    <div class="margin-top10">
        <fieldset>
            <legend>Select Criteria</legend>
            Make : <asp:DropdownList ID="ddlMakes" runat="server" />
            Model : <asp:DropdownList ID="ddlModels" runat="server" disabled="disabled" />
            <asp:HiddenField ID="hdnSelectedModel" runat="server" />
            <asp:RadioButton ID="rdoPending" runat="server" GroupName="reviewType" Text="Pending" />
            <asp:RadioButton ID="rdoApproved" runat="server" GroupName="reviewType" Text="Approved"/>
            <asp:RadioButton ID="rdoDiscarded" runat="server" GroupName="reviewType" Text="Discarded" />
            <asp:Button ID="btnShowReviews" runat="server" Text="Show Reviews" />
        </fieldset>
    </div>
    <div id="errMsg" runat="server" class="margin-top10"></div>
    <asp:Repeater id="rptReviews" runat="server">       
        <HeaderTemplate>
            <table border="1" style="border-collapse:collapse;" cellpadding ="5" class="margin-top10">
                <tr style="background-color:#D4CFCF;">
                    <th>Review ID</th>
                    <th>View/Edit</th>
                    <th>Bike</th>
                    <th>WrittenBy</th>
                    <th>Views</th>
                    <th>Entry Date</th>
                    <th>LastUpdatedOn</th>
                    <th>Approve</th>
                    <th>Discard</th>                    
                </tr>
        </HeaderTemplate>
        <ItemTemplate> 
                <tr id="<%# DataBinder.Eval(Container.DataItem,"ID")%>">
                    <td><%# DataBinder.Eval(Container.DataItem,"ID") %></td>                    
                    <td><a class="pointer" onclick="window.open('viewreview.aspx?id=<%# DataBinder.Eval(Container.DataItem,"ID")%>','mywin','scrollbars=yes,left=200,top=50,width=940,height=600')">View</a></td>                    
                    <td><%# DataBinder.Eval(Container.DataItem,"Bike") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem,"WrittenBy") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem,"Viewed") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem,"EntryDateTime") %></td>
                    <td><%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"LastUpdatedOn").ToString()) ? DataBinder.Eval(Container.DataItem,"LastUpdatedOn") : "N/A" %></td>
                    <td>
                        <input type="button" id="<%# DataBinder.Eval(Container.DataItem,"ID")%>A" value="Approve" onclick ="approve_click(event);" class="<%# DataBinder.Eval(Container.DataItem,"IsVerified").ToString() == "True" ? "hide" : "show" %>"/>
                    </td>
                    <td><input type="button" id="<%# DataBinder.Eval(Container.DataItem,"ID")%>D" value="Discard" onclick ="discard_click(event);" class="<%# DataBinder.Eval(Container.DataItem,"IsDiscarded").ToString() == "True" ? "hide" : "show" %>"/></td>                    
                </tr>
        </ItemTemplate>
        <FooterTemplate>
             </table>
        </FooterTemplate>
    </asp:Repeater>
</div>

<script type = "text/javascript">
    $(document).ready(function () {

        $("#ddlMakes").change(function () {
            getModels(this);
            $("#hdnSelectedModel").val(0);
        });

        $("#ddlModels").change(function () {
            $("#hdnSelectedModel").val($(this).val());
        });

        if ($("#ddlMakes").val() > 0)
        {            
            getModels($("#ddlMakes"));

            var selectedModelId = $("#hdnSelectedModel").val();
            
            if (selectedModelId != "" && selectedModelId > 0)
            {                
                $("#ddlModels").val(selectedModelId);
            }
        }        
    });

    function getModels(e)
    {        
        var makeId = $(e).val();
        var reqType='ALL';
        if (makeId > 0) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"makeId":"' + makeId + '","requestType":"' + reqType + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    bindDropDownList(resObj, $("#ddlModels"), "", "--Select Models--");
                }
            });
        } else {
            $("#ddlModels").val("0").attr("disabled", "disabled");
        }
    }

    function approve_click(e) {

        var evt = e || window.event;
        var current = evt.target || evt.srcElement;
        var Id = current.id;
        var r = confirm("Approve this Review");
        var ID = Id.substring(0, Id.length - 1);
        if (r == true) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.ReviewDetail,BikewaleOpr.ashx",
                data: '{"Id":"' + Id + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SetIsReviewed"); },
                success: function (response) {
                    $("#" + ID).remove();
                    alert("Review Approved Successfully");
                }
            });
        }
    }
    function discard_click(e) {
        var evt = e || window.event;
        var current = evt.target || evt.srcElement;
        var Id = current.id;
        var r = confirm("Confirm Discard");
        var ID = Id.substring(0, Id.length - 1);
        if (r == true) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.ReviewDetail,BikewaleOpr.ashx",
                data: '{"Id":"' + Id + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SetIsDiscarded"); },
                success: function (response) {
                    $("#" + ID).remove();
                    alert("Review Discarded Successfully");
                }
            });
        }
    }
</script>
<!-- #Include file="/includes/footerNew.aspx" -->