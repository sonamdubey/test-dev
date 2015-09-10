<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.BrowseUserReviews" %>
<h2>Consumer Bike Reviews</h2>
<ul class="ul-hrz-left margin-top10">  
    <li><asp:DropDownList ID="drpRevMake" runat="server" CssClass="brand">
            <asp:ListItem Selected="true" Text="--Select Make--" Value="0"></asp:ListItem>
        </asp:DropDownList>
    </li>
    <li><asp:DropDownList ID="drpRevModel" runat="server" CssClass="brand"  Enabled="false">
        <asp:ListItem Selected="true" Text="--Select Model--" Value="0"></asp:ListItem>
        </asp:DropDownList>
    </li>
    <li><div class="action-btn margin-left10 btn-xs"><a onclick="viewReviews();">Read Reviews</a></div></li>
    <li class="padding-top10 padding-left5">or browse <a href="/user-reviews/">Bike Reviews</a></li>
</ul><div class="clear"></div>
<input type="hidden" id="hdn_drpModel" runat="server" />
<script type="text/javascript">
    var makeId = '<%=drpRevMake_Id%>';
    var makeObj = $("#" + makeId);
    var modelId = '<%=drpRevModel_Id%>';

    makeObj.change(function () {        
        if (makeObj.val() == 0) {
            $("#" + modelId).val("0").attr("disabled", true);
        }
        else
        {
            make_Change();
        }       
    });

    function make_Change()
    {
        showLoading(modelId);
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
            data: '{"requestType":"USERREVIEW", "makeId":"' + makeObj.val().split('_')[0] + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModelsWithMappingName"); },
            success: function (response) {
                var responseJSON = eval('(' + response + ')');
                var resObj = eval('(' + responseJSON.value + ')');
                var dependentCmbs = new Array;
                bindDropDownList(resObj, document.getElementById(modelId), "<%=hdn_drpModel_Id%>", dependentCmbs, "--Select Model--");
            }
        });
    }

    function viewReviews() {
        var makeName = "", modelName = "";
        if (makeObj.val() < 1) {
            alert("Please select make for reviews");
            return;
        }
        if ($("#" + modelId).val() < 1) {
            alert("Please select model for reviews");
            return;
        }
        makeName = $("#" + makeId + " option:selected").val().split('_')[1];     
        modelName = $("#" + modelId + " option:selected").val().split('_')[1];     
        location.href = "/" + makeName + "-bikes/" + modelName + "/user-reviews/";
    }

    // replace multiple occurrence of commas
    var urlCharRepArray = new Array('.', '-', ' ', '(', ')');
    function formatURL(str) {
        str = str.toLowerCase();
        for (i = 0; i < urlCharRepArray.length; i++) {
            var occurrence = str.indexOf(urlCharRepArray[i]);

            while (occurrence != -1) {
                str = str.replace(urlCharRepArray[i], "");
                occurrence = str.indexOf(urlCharRepArray[i]);
            }
        }
        return str;
    }

    $(document).ready(function () {
        if (makeObj.val() != 0)
        {
            make_Change();
        }
    });
</script>