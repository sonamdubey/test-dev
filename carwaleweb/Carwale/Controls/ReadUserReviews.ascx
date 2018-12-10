<%@ Control Language="C#" Inherits="Carwale.UI.Controls.ReadUserReviews" AutoEventWireup="false" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<style>
    .ur-hor {
        list-style: none;
    }

        .ur-hor li {
            float: left;
            padding-right: 5px;
        }

            .ur-hor li select {
                width: 150px;
            }

    .ur-ver {
        list-style: none;
    }

        .ur-ver li {
            padding: 2px 0;
        }

            .ur-ver li select {
                width: 100%;
            }
            .ul-arrow li{padding:3px 0px;}
.ul-arrow li a{background:url(https://imgd.aeplcdn.com/0x0/cw-common/ul-arrow.gif) no-repeat center left; margin-left:5px; padding-left:10px;}
</style>
<ul id="ulUserReview" runat="server" class="ur-ver">
    <li>
        <div class="form-control-box margin-bottom5">
            <span class="select-box fa fa-angle-down"></span>
            <asp:DropDownList ID="drpRevMake" CssClass="form-control" data-bind="value: Make" runat="server" />

        </div>
    </li>
    <li>
        <div class="form-control-box margin-bottom15">
        <span class="select-box fa fa-angle-down"></span>
            <asp:DropDownList ID="drpRevModel" CssClass="form-control" data-bind="foreach: Models" runat="server">
            <asp:ListItem data-bind="value: ModelId,text:ModelName, attr: { 'mask': MaskingName }"></asp:ListItem>
        </asp:DropDownList>
        </div>
    </li>
</ul>
<div class="clear"></div>
<a class="btn btn-orange btn-xs" onclick="viewReviews();">Read Reviews</a> or browse <a data-cwtccat="ReviewLandingLinkage" data-cwtcact="UserReviewDetailDesktopLinkClick" data-cwtclbl="source=1" href="/userreviews/">Car Reviews</a>
<input type="hidden" id="hdn_drpModel" runat="server" />
<script language="javascript" type="text/javascript">
    var makeId = '<%=drpRevMake_Id%>';
    var makeObj = $("#" + makeId);
    var modelId = '<%=drpRevModel_Id%>';

	var ReadUserReviewsKOVM;


	var link = "";
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
	    makeName = $("#" + makeId + " option:selected").text();
	    modelName = $("#" + modelId + " option:selected").attr('mask');
	    location.href = "/research/" + formatURL(makeName) + "-cars/" + modelName + "/userreviews/";
	}

	// replace multiple occurrence of commas
	var urlCharRepArray = new Array('.', '-', ' ', '(', ')', '[', ']', '&', ':', '#', '?', '/', '\\', '-', '%', ',', '!', '+', '"', '$', '_', '*');
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
	    ReadUserReviewsKOVM = eval('(' + genericMakeModelKVM + ')');
	    ko.applyBindings(ReadUserReviewsKOVM, $("#" + modelId).parent().parent()[0]);
	    ReadUserReviewsKOVM.Models([{ "ModelId": -1, "ModelName": "--Select Model--", "MaskingName": "" }]);
	    $('#' + modelId).attr('disabled', true);
	    $("#" + makeId).change(function () {
	        bindModelsList("new", $("#" + makeId + " option:selected").val(), ReadUserReviewsKOVM, '#' + modelId, "--Select Model--");
	    });
	});
</script>
