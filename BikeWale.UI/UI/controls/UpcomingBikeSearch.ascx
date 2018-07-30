<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UpcomingBikeSearch" %>
<div class="grey-bg margin-top15">
    <div class="content-block">
        <div class="grid_4 alpha">
            Filter by:&nbsp;<asp:DropDownList ID="drpMake" runat="server" />&nbsp;	
	        <a class="buttons" id="btnMakeModelSearch" onclick="javascript:MakeModelSearch_click()">Go</a> 
        </div>
        <div class="grid_3 omega">
            Sort by:
            <select id="drpUCSortList" runat="server">
                <option value="-1">Select</option>
                <option value="1">Price: Low to High</option>
                <option value="2">Price: High to Low</option>
                <option value="3">Launch Date: Sooner</option>
                <option value="4">Launch Date: Later</option>
            </select>
        </div><div class="clear"></div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $("#UpcomingBikeSearch_drpUCSortList").change(function () {
            var sortVal = $("#UpcomingBikeSearch_drpUCSortList").val();
            if (sortVal != "-1") {
                var url = '<%=Request.ServerVariables["HTTP_X_ORIGINAL_URL"].ToString() %>';

                if (url.indexOf('/page') > -1 || url.indexOf('/sort') > -1) {
                    if (url.indexOf('/page') > -1) {
                        url = url.substring(0, url.indexOf("/page")) + "/sort/" + sortVal + "/";
                    }
                    if (url.indexOf('/sort') > -1) {
                        url = url.substring(0, url.indexOf("/sort") + 5) + "/" + sortVal + "/";
                    }
                } else {
                    url += "sort/" + sortVal + "/";
                }
                window.location.href = url;
            }
        });
    });
    function MakeModelSearch_click() {
        var makeValueArray = $("#UpcomingBikeSearch_drpMake option:selected").val();
        //alert(makeValueArray);
        var makeId = makeValueArray.split('_')[0];
        if (makeId != "0") {
            var makeName = makeValueArray.split('_')[1],base = '/',loc = base + makeName + "-bikes/";
            loc += 'upcoming/';
            window.location.href = loc;
        } else {
            window.location.href = "/upcoming-bikes/";
        }        
    }
</script>
