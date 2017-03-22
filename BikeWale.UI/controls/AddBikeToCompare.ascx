<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.AddBikeToCompare" %>
<style>
	.qr-ver{list-style:none;}
	.qr-ver li{padding:2px 0;}
</style>

<ul id="ulAddBike" runat="server" class="addAnotherBikeUL qr-ver ul-normal center-align">
	<li><select id="drpMake" runat="server" /></li>
	<li><select id="drpModel" runat="server"><option selected="selected" Value="-1">--Select Model--</option></select></li>
	<li><select id="drpVersion" runat="server"><option selected="selected" Value="-1">--Select Version--</option></select></li>
	<li><input class="action-btn text_white" type="button" id="btnAddBike" runat="server" value="Add a Bike"/>
	</li>
</ul>

<script type="text/javascript">

    $(window).on("load", function () {
        if ($("#addAnotherBike").hasClass("fivecolum")) {
            $(".addAnotherBikeUL li select").css({ "width": "130px", "font-size": "12px" });
            $(".addAnotherBikeUL li input").addClass("btn-xs");
        };
    });

    var drpMake = "#<%=drpMake.ClientID.ToString()%>";
    var drpModel = "#<%=drpModel.ClientID.ToString()%>";
    var drpVersion = "#<%=drpVersion.ClientID.ToString()%>";
    var btnAddBike = "#<%= btnAddBike.ClientID.ToString()%>";
    var versionId = '<%=versionId%>';

    $(drpModel).attr("disabled", "disabled");
    $(drpVersion).attr("disabled", "disabled");

    $(drpMake).change(function () {
        makeId = $(drpMake).val().split('_')[0];
        var requestType = "All";
        if (makeId > 0) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxCompareBikes,Bikewale.ashx",
                data: '{"makeId":"' + makeId + '", "compareBikes":"new"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    bindDropDownList(resObj, drpModel, "", "", "--Select Model--");
                }
            });
        } else {
            $(drpModel).val("0").attr("disabled", true);
            $(drpVersion).val("0").attr("disabled", true);
        }
    });

    $(drpModel).change(function () {
        makeId = $(drpMake).val().split('_')[0];
        modelId = $(drpModel).val().split('_')[0];
        var requestType = "All";
        if (modelId > 0) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxCompareBikes,Bikewale.ashx",
                data: '{"modelId":"' + modelId + '", "compareBikes":"new"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetVersions"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    bindDropDownList(resObj, drpVersion, "", "", "--Select Version--");
                }
            });
        } else {
            $(drpVersion).val("0").attr("disabled", true);
        }
    });

    $(btnAddBike).click(function () {
        var isError = false;

        if ($(drpMake).val() <= 0 || $(drpModel).val() <= 0 || $(drpVersion).val() <= 0) {
            alert("Please Select Bike Version for Comparison.");
            isError = true;
        }

        if (isError == false) {
            var isDuplicate = false;
            var verIdlist = new Array();
            verIdlist = versionId.split(',');
            for (var i = 0; i < verIdlist.length; i++) {
                if ($(drpVersion).val() == verIdlist[i]) {
                    alert("Please choose different bikes for comparison.");
                    isDuplicate = true;
                }
            }
            if (isDuplicate == false) {
                AddBike();
            }
        }

    });

    function AddBike() {
        var queryStr = "?";
        var count = 1;
        var verIdCount;
        var url = window.location.pathname;
        var isFeatured = '<%= IsFeatured%>';

        if (isFeatured == 'True') {
            versionId = versionId.substring(0, versionId.lastIndexOf(','));
        }
        var basicUrl = (window.location.pathname).split('/')[2];
        var curVersions = versions;
        var makeModelList = new Array();
        var VersionIdList = new Array();
        var newBasicUrl = "";
        var newQueryStr = "";
        var Count = 0;
        makeModelList = basicUrl.split("-vs-");
        VersionIdList = curVersions.split(',');
        var versionToAdd = $('#addBike_drpModel').val().split('_')[0];
        VersionIdList.push(versionToAdd);
        VersionIdList = VersionIdList.sort(sortNumber);
        var indx = VersionIdList.indexOf(versionToAdd);
        makeModelList.splice(indx, 0, $(drpMake).val().split('_')[1] + "-" + $(drpModel).val().split('_')[1]);
        for (var i = 0; i < makeModelList.length; i++) {
                newBasicUrl += makeModelList[i] + "-vs-";
            }
        newBasicUrl = newBasicUrl.substring(0, newBasicUrl.lastIndexOf("-vs-"));
        var verIdlist = new Array();
        verIdlist = versionId.split(',');

        for (var i = 0; i < verIdlist.length; i++) {
            queryStr += "bike" + count + "=" + verIdlist[i] + "&";
            count++;
        }
        queryStr += "bike" + count + "=" + $(drpVersion).val();
        window.location = '/comparebikes/' +newBasicUrl + queryStr + "&source=" + '<%= (int)Bikewale.Entities.Compare.CompareSources.Desktop_CompareBike_UserSelection %>';
    }

    function sortNumber(a, b) {
        return a - b;
    }
</script>