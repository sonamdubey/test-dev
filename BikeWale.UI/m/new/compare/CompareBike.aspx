<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.CompareBike" EnableViewState="false" %>

<%@ Register TagPrefix="CB" TagName="CompareBike" Src="/m/controls/CompareBikeMin.ascx" %>
<%

    if (pageMetas != null)
    {
        title = pageMetas.Title;
        keywords = pageMetas.Keywords;
        description = pageMetas.Description;
        canonical = pageMetas.CanonicalUrl;
        AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        AdId = "1398766302464";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
    }
    
%>

<!-- #include file="/includes/headscript_mobile_min.aspx" -->
<link rel="stylesheet" type="text/css" href="/m/css/compare/landing.css" />
<script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
</script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div class="container box-shadow bg-white card-bottom-margin bw-tabs-panel">
                <h1 class="box-shadow padding-15-20 margin-bottom3 text-bold">Compare Bikes</h1>
                <div class="comparison-main-card">

                    <div class="bike-details-block " >
                        <div class="compare-box-placeholder">
                            <div class="bike-icon-wrapper"><span class="grey-bike"></span>
                                <p class="font14 text-light-grey">Tap to select bike 1</p>
                            </div>
                        </div>
                    </div>

                    <div class="bike-details-block " >
                         <div class="compare-box-placeholder">
                            <div class="bike-icon-wrapper"><span class="grey-bike"></span>
                                <p class="font14 text-light-grey">Tap to select bike 2</p>
                            </div>
                        </div>
                    </div>

                    <div class="padding-bottom15 text-center">
                        <a href="javascript:void(0)" class="btn btn-white btn-size-1"  rel="nofollow" data-bind="click : compareBikes">Compare Now</a>
                    </div>

                    <div class="clear"></div>
                </div>
            </div>

                        <% if (objMakes != null)
               { %>
            <!-- select bike starts here -->
            <div id="select-bike-cover-popup" class="cover-window-popup">
                <div class="ui-corner-top">
                    <div id="close-bike-popup" class="cover-popup-back cur-pointer leftfloat" data-bind="click: closeBikePopup">
                        <span class="bwmsprite fa-angle-left"></span>
                    </div>
                    <div class="cover-popup-header leftfloat">Select bikes</div>
                    <div class="clear"></div>
                </div>
                <div class="bike-banner"></div>
                <div id="select-make-wrapper" class="cover-popup-body">
                    <div class="cover-popup-body-head">
                        <p class="no-back-btn-label head-label inline-block">Select Make</p>
                    </div>
                    <ul class="cover-popup-list with-arrow">
                        <% foreach (var make in objMakes)
                           { %>
                        <li data-bind="click: makeChanged"><span data-masking="<%= make.MaskingName %>" data-id="<%= make.MakeId %>"><%= make.MakeName %></span></li>
                        <% } %>
                    </ul>
                </div>

                <div id="select-model-wrapper" class="cover-popup-body">
                    <div class="cover-popup-body-head">
                        <div data-bind="click: modelBackBtn" class="body-popup-back cur-pointer inline-block">
                            <span class="bwmsprite back-long-arrow-left"></span>
                        </div>
                        <p class="head-label inline-block">Select Model</p>
                    </div>
                    <ul class="cover-popup-list with-arrow" data-bind="foreach: modelArray">
                        <li data-bind="click: $parent.modelChanged">
                            <span data-bind="text: modelName, attr: { 'data-id': modelId, 'data-masking': maskingName }"></span>
                        </li>
                    </ul>
                </div>

                <div id="select-version-wrapper" class="cover-popup-body">
                    <div class="cover-popup-body-head">
                        <div data-bind="click: versionBackBtn" class="body-popup-back cur-pointer inline-block">
                            <span id="arrow-version-back" class="bwmsprite back-long-arrow-left"></span>
                        </div>
                        <p class="head-label inline-block">Select Version</p>
                    </div>
                    <ul class="cover-popup-list" data-bind="foreach: versionArray">
                        <li data-bind="click: $parent.versionChanged">
                            <span data-bind="text: versionName, attr: { 'data-id': versionId }"></span>
                        </li>
                    </ul>
                </div>

                <div class="cover-popup-loader-body">
                    <div class="cover-popup-loader"></div>
                    <div class="cover-popup-loader-text font14">Loading...</div>
                </div>
            </div>
            <!-- select bike ends here -->
            <% } %>

        </section>

        <section>
            <div class="container margin-bottom20">
                <div class="grid-12 alpha omega">
                    <CB:CompareBike ID="ctrlCompareBikes" runat="server"></CB:CompareBike>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
         <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : ""%>/m/src/compare/landing.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,600,700' rel='stylesheet' type='text/css' />
    </form>
</body>
</html>

    <%--
    <div data-role="popup" id="popupDialog" data-overlay-theme="a" data-theme="c" data-dismissible="false"  class="ui-corner-all">
        <div data-role="header" data-theme="a" class="ui-corner-top" style="background-color:#000;">
            <h1 class="ui-title" style="color:#fff;" role="heading" aria-level="1">Error !!</h1>
        </div>
        <div data-role="content" data-theme="d" class="ui-corner-bottom ui-content" style="background-color:#fff;">
            <span id="spnError" style="font-size:14px;line-height:20px;" class="error" ></span>
            <a href="#" data-role="button" data-rel="back" data-theme="c" data-mini="true">OK</a>
        </div>
    </div>
    
    </div>

    <div class="container margin-bottom20">
        <div class="grid-12 alpha omega">
            <CB:CompareBike ID="ctrlCompareBikes" runat="server" ></CB:CompareBike>
        </div>
        <div class="clear"></div>
    </div>


<script type="text/javascript">
    
    var formatedMake1 = null, formatedMake2 = null, formatedModel1 = null, formatedModel2 = null;

    $(document).ready(function () {
        var bikeName = "",
            divMakeddl1 = "#divMakeddl1",
            divMakeddl2 = "#divMakeddl2";
 
    });

    function OpenPopup(divMakeddl) {
        var compareBox = $(divMakeddl).closest('.compare-box');

        $("#divParentPageContainer").hide();
        $("#divForPopup").attr("style", "z-index:10;width:100%;height:100%;position:absolute;");        
        $(compareBox).next().show();
        $(".divMake").show();
        $("#divForPopup").html($(compareBox).next().html());
        $(".divModel, .divVersion").hide();
    }

    function ShowModel(a) {
        bikeName = $(a).text();
        makeId = $(a).attr("id");
        var type = $(a).attr("type").toString();
        $("#imgLoaderModel").show();
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxCompareBikes,Bikewale.ashx",
            data: '{"makeId":"' + makeId + '", "compareBikes":"new", "type" : "' + type + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetNewBikeModels"); },
            success: function (response) {
                $("#imgLoaderModel").hide();
                var response = eval('(' + response + ')');
                if (response != "") {
                    if (type == "1") {
                        makeName1 = $(a).attr("MaskingName");
                        $("#divForPopup ul").attr("type", "ddlModel1").html(response.value);
                        $("#divForPopup ul").attr("type", "ddlModel1").listview().listview('refresh');
                    }
                    else if (type == "2") {
                        makeName2 = $(a).attr("MaskingName");
                        $("#divForPopup ul").attr("type", "ddlModel2").html(response.value);
                        $("#divForPopup ul").attr("type", "ddlModel2").listview().listview('refresh');
                    }
                }
                    $(".divModel").show();
                    window.scrollTo(0, 1);
                    $(".divMake").hide();
                    $(".divVersion").hide();
                //}
            }
        });
    }

    function ShowVersion(a) {
        var retVal = "";
        bikeName += " " + $(a).text();
        modelId = $(a).attr("id").split('_')[0];
        var type = $(a).attr("type");
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
            data: '{"modelId":"' + modelId + '", "requestType":"compare"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetVersions"); },
            success: function (response) {
                var jsonString = eval('(' + response + ')');
                var resObj = eval('(' + jsonString.value + ')');
                if (resObj.Table.length > 0);
                {
                    for (var i = 0; i < resObj.Table.length; i++) {
                        if (type == "1")
                            retVal += "<li data-hosturl='" + resObj.Table[i].HostURL + "' data-price='" + resObj.Table[i].VersionPrice + "' data-imagepath='" + resObj.Table[i].OriginalImagePath + "' ><a onclick=\"ShowBikeName(this);\" id ='" + resObj.Table[i].Value + "' type='1'>" + resObj.Table[i].Text + "</a></li>";
                        else if (type == "2")
                            retVal += "<li data-hosturl='" + resObj.Table[i].HostURL + "' data-price='" + resObj.Table[i].VersionPrice + "' data-imagepath='" + resObj.Table[i].OriginalImagePath + "' ><a onclick=\"ShowBikeName(this);\" id ='" + resObj.Table[i].Value + "' type='2'>" + resObj.Table[i].Text + "</a></li>";
                    }
                }
                if (retVal != "") {
                    if (type == "1") {
                        modelName1 = $(a).attr('id').split('_')[1];
                        $("#divForPopup ul").attr("type", "ddlVersion1").html(retVal);
                        $("#divForPopup ul").attr("type", "ddlVersion1").listview().listview('refresh');
                    }
                    else if (type == "2") {
                        modelName2 = $(a).attr('id').split('_')[1];
                        $("#divForPopup ul").attr("type", "ddlVersion2").html(retVal);
                        $("#divForPopup ul").attr("type", "ddlVersion2").listview().listview('refresh');
                    }
                    $(".divVersion").show();
                    window.scrollTo(0, 1);
                    $(".divMake").hide();
                    $(".divModel").hide();
                }
            }
        });
    }

    function ShowBikeName(a) {
        var type = "";

        $(".divMake").hide();
        $(".divModel").hide();
        $(".divVersion").hide();
        $("#divForPopup").hide();
        $("#divParentPageContainer").show();
        bikeName += " " + $(a).text();

        var ele = $(a).parent();
        type = $(a).attr("type");
        bikeprice = ele.attr("data-price");
        bikeimage = "https://imgd3.aeplcdn.com/210x118/bikewaleimg/images/noimage.png";
        bikeimghost = ele.attr("data-hosturl");
        bikeimgpath = ele.attr("data-imagepath");
        
        if (bikeprice != null && bikeprice != '' && bikeprice != "0")
        {
            bikeprice = formatPrice(bikeprice);
            bikeprice = '<span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold font16">' + bikeprice + '</span>';
        }            
        else bikeprice = "Price not available";

        if (bikeimghost != "" || bikeimgpath!="")
        {
            bikeimage = bikeimghost + "/210x118/" + bikeimgpath;
        }

        var element = $('<div class="selected-bike position-rel"><span class="bwmsprite cancel-select cross-sm-dark-grey cur-pointer position-abt pos-top5 pos-right5"></span><img src="'+ bikeimage +'" border="0" /><p class="selected-bike-label text-bold text-truncate font12 margin-bottom5">' + bikeName + '</p><p class="text-truncate text-light-grey font11">Ex-showroom, Mumbai</p><p class="text-default">'+bikeprice+'</p></div>');
        
        if (type == "1") {
            formatedMake1 = formatURL(makeName1);
            formatedModel1 = modelName1;
            $("#hdnVersionId1").val($(a).attr("id"));

            $(divMakeddl1).find('.compare-box-placeholder').hide();
            $(divMakeddl1).append(element);
        }
        else if (type == "2") {
            formatedMake2 = formatURL(makeName2);
            formatedModel2 = modelName2;
            $("#hdnVersionId2").val($(a).attr("id"));

            $(divMakeddl2).find('.compare-box-placeholder').hide();
            $(divMakeddl2).append(element);
        }
        $('#divListContainer, #divListContainer2').hide();
    }

    function VerifyVersion() {
        var isError = false;
        if (formatedMake1 && formatedMake2 && formatedModel1 && formatedModel2) {
            var ver1 = $("#hdnVersionId1").val();
            var ver2 = $("#hdnVersionId2").val();
            if (ver1 == "-1" && ver2 == "-1") {
                $("#spnError").html("Please select bikes to compare <br>");
                $("#popupDialog").popup("open");
                isError = true;
            }
            else if (ver1 == ver2) {
                $("#spnError").html("Please choose different bikes for comparison <br>");
                $("#popupDialog").popup("open");
                isError = true;
            }
        }
        else {
            $("#spnError").html("Please select bikes to compare <br>");
            $("#popupDialog").popup("open");
            isError = true;
        }

        if (isError)
            return false;
        else
            GenerateURL();
    }

    function GenerateURL() {
        var makeArr = new Array;
        var modelArr = new Array;
        var versionArr = new Array;
        if ($("#hdnVersionId1").val() > 0) {
            makeArr.push(formatedMake1);
            modelArr.push(formatedModel1);
            versionArr.push($("#hdnVersionId1").val());
        }
        if ($("#hdnVersionId2").val() > 0) {
            makeArr.push(formatedMake2);
            modelArr.push(formatedModel2);
            versionArr.push($("#hdnVersionId2").val());
        }

        var url = '';
        var qs = '';
        for (var i = 0; i < makeArr.length; i++) {
            if (i != makeArr.length - 1) {
                url += makeArr[i] + '-' + modelArr[i] + '-vs-';
                qs += 'bike' + (i + 1).toString() + '=' + versionArr[i] + '&';
            }
            else {
                url += makeArr[i] + '-' + modelArr[i];
                qs += 'bike' + (i + 1).toString() + '=' + versionArr[i];
            }
        }
        $("#hdnMake1").val(formatedMake1);
        $("#hdnMake2").val(formatedMake2);
        $("#hdnModel1").val(formatedModel1);
        $("#hdnModel2").val(formatedModel2);
        $("#hdnBikeName1").val($("#divMakeddl1 p").html());
        $("#hdnBikeName2").val($("#divMakeddl2 p").html());
        location.href = "/m/comparebikes/" + url + '/?' + qs + "&source=" + <%= (int)Bikewale.Entities.Compare.CompareSources.Mobile_CompareBike_UserSelection %>;
    }

    function formatURL(str) {
        str = str.toLowerCase();
        str = str.replace(/[^0-9a-zA-Z]/g, '');
        return str;
    }

    function CloseWindow() {
        $(".divMake").hide();
        $(".divModel").hide();
        $(".divVersion").hide();
        $("#divForPopup").hide();
        $("#divParentPageContainer").show();
        $('#divListContainer, #divListContainer2').hide();
    }

    $('.compare-box').on('click', '.cancel-select', function (event) {
        var selectedBike = $(this).closest('.selected-bike'),
            compareBox = $(this).closest('.compare-box');

        selectedBike.siblings('.compare-box-placeholder').show();
        selectedBike.remove();
        if ($(compareBox).attr('id') == 'divMakeddl1') {
            formatedMake1 = null;
            formatedModel1 = null;
        }
        else {
            formatedMake2 = null;
            formatedModel2 = null;
        }
    });

    function formatPrice(price) {
        price = price.toString();
        var lastThree = price.substring(price.length - 3);
        var otherNumbers = price.substring(0, price.length - 3);
        if (otherNumbers != '')
            lastThree = ',' + lastThree;
        var price = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
        return price;
    }

</script>
<!-- #include file="/includes/footermobile.aspx" -->--%>
