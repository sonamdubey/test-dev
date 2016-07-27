<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.RoadTest"  Async="true" Trace="false"%>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="Pager" TagName="Pager" Src="/m/controls/ListPagerControl.ascx" %>
<% 
    title = "Road tests, First drives of New Bikes in India";
    description = "Road testing a bike is the only way to know true capabilities of a bike. Read our road tests to know how bikes perform on various aspects.";
    keywords = "road test, road tests, roadtests, roadtest, bike reviews, expert bike reviews, detailed bike reviews, test-drives, comprehensive bike tests, bike preview, first drives";
    canonical = "http://www.bikewale.com" + "/road-tests/";
    relPrevPageUrl = String.IsNullOrEmpty(prevPageUrl) ? "" : "http://www.bikewale.com" + prevPageUrl;
    relNextPageUrl = String.IsNullOrEmpty(nextPageUrl) ? "" : "http://www.bikewale.com" + nextPageUrl;
    AdPath = "/1017752/Bikewale_Mobile_NewBikes";
    AdId = "1398766302464";
    menu = "7";
    Ad_320x50 = true;
    Ad_Bot_320x50 = true;
%>
<!-- #include file="/includes/headermobile.aspx" -->
<style type="text/css">
    #divListing .box1 { padding-top:20px; }
    .article-wrapper { display:table; margin-bottom:10px; }
    .article-image-wrapper { width:120px; }
    .article-image-wrapper, .article-desc-wrapper { display:table-cell; vertical-align:top; }
    .article-stats-wrapper { min-width:115px; padding-right:10px; }
    .calender-grey-icon, .author-grey-icon { width:14px; height:15px; position:relative; top:-1px; margin-right:6px; }
    .calender-grey-icon { background-position:-40px -460px; }
    .author-grey-icon { background-position:-64px -460px; }
</style>
<script type="text/javascript" src="http://st2.aeplcdn.com/bikewale/src/BikeWaleCommon.js?v=3.2"></script>
<div class="padding5">
    <div id="br-cr">
        <span itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
        <a href="/m/" class="normal" itemprop="url"><span itemprop="title">Home</span></a> </span>
        &rsaquo; <span class="lightgray">Road Test</span></div>
    <h1>Latest bike Road Tests</h1>
    <div id="divListing">
        <asp:Repeater id="rptRoadTest" runat="server">
            <itemtemplate>
                <a class="normal" href='/m/road-tests/<%#DataBinder.Eval(Container.DataItem, "ArticleUrl").ToString()%>-<%# DataBinder.Eval(Container.DataItem, "BasicId") %>.html' >
                    <div class="box1 new-line15" >
                        <div class="article-wrapper">
                            <div class="article-image-wrapper">
                                <img alt='Road Test: <%# DataBinder.Eval(Container.DataItem, "Title") %>' title="Road Test: <%# DataBinder.Eval(Container.DataItem, "Title") %>" src='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>' width="100%" border="0">
                            </div>
                            <div class="padding-left10 article-desc-wrapper">
                                <div class="font14 text-bold text-black">
                                    Road Test: <%# DataBinder.Eval(Container.DataItem, "Title") %>
                                </div>
                            </div>
                        </div>
                        <div class="article-stats-wrapper font12 leftfloat text-light-grey">
                            <span class="bwmsprite calender-grey-icon inline-block"></span><span class="inline-block">date</span>
                        </div>
                        <div class="article-stats-wrapper font12 leftfloat text-light-grey">
                            <span class="bwmsprite author-grey-icon inline-block"></span><span class="inline-block"><%# DataBinder.Eval(Container.DataItem, "AuthorName") %></span>
                        </div>
                        <div class="clear"></div>
                        <%--<div style="border:1px solid #b3b4c6;background-color:#ffffff;width:100px;position:absolute;right:-1px;bottom:-10px;padding:2px 2px;font-size:13px;" class="lightgray">
                            <%# CommonOpn.GetDisplayDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString()) %>
                        </div>--%>
                    </div>
                </a>
            </itemtemplate>
        </asp:Repeater>                
    </div>  
    <Pager:Pager ID="listPager" runat="server" />  
</div>
<script language="javascript" type="text/javascript">
    function BoxClicked(box)
    {	
        var nextHidden = $(box).next().is(":hidden").toString();
        if (nextHidden == "true")
        {
            $(box).next().show();
            $(box).find(":nth-child(2)").attr("class", "minus");
            $(box).addClass("bot-rad-0");
        }
        else
        {
            $(box).next().hide();
            $(box).find(":nth-child(2)").attr("class", "plus");	
            $(box).removeClass("bot-rad-0");
        }
        
        $("#ddlMakes, #ddlModels").each(function(){
            $(this).width(parseInt($(this).parent().width())-5);
        });
    }

    $("#ddlMakes").change(function () {
        make = $(this).val();
        if (make <= 0) {
            $("#ddlModels").html("<option value='0'>--Select Model--</option>");
            $("#ddlModels").selectmenu("refresh", true);
            $("#ddlModels").attr("disabled", true);
        }
        else {
            $("#imgLoaderModels").show();
            fillModel();
        }
    });

    $(document).ready(function () {
        $("#ddlModels").attr("disabled", true);
        if ($("#ddlMakes").val().split('_')[0] > 0) {
            $("#imgLoaderModels").show();
            fillModel();
        }
    });


    function fillModel() {
        $("#ddlModels").html("<option value='0'>--Select Model--</option>");
        $("#ddlModels").selectmenu("refresh", true);

        if ($("#ddlMakes").val() > "0") {
            var MakeId = $("#ddlMakes").val().split('_')[0];
            //$("#ddlModels").empty().append("<option value=\"\">Loading...</option>");
            var reqType = "ROADTEST";
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"requestType":"' + reqType + '" , "makeId":"' + MakeId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModelsWithMappingName"); },
                success: function (response) {
                    $("#imgLoaderModels").hide();
                    var responseJSON = eval('(' + response + ')');
                    if (responseJSON.value != "") {
                        var resObj = eval('(' + responseJSON.value + ')');
                        var dependentCmbs = new Array;
                        bindDropDownList(resObj, $("#ddlModels"), "", dependentCmbs, "--Select Model--");
                    }
                    else
                        $("#ddlModels").empty().append("<option value=\"\">--Select Model--</option>");
                }
            });
            $("#ddlModels").attr("disabled", false);
            $("#imgLoaderModels").hide();
        }
    }

    $("#filterButton").click(function () {
        var makeId = $("#ddlMakes").val().split('_')[0];
        var makeName = $("#ddlMakes").val().split('_')[1];
        var modelId = $("#ddlModels").val().split('_')[0];
        var modelName = $("#ddlModels").val().split('_')[1];
        
        if (modelId > 0)
        {
            window.location = "/m/" + makeName + "-bikes/" + modelName + "/road-tests/";
        }
        else if (makeId > 0)
        {
            window.location = "/m/" + makeName + "-bikes/road-tests/";
        }
        else 
            alert("Please Select Make.");
       
    });

</script>
<!-- #include file="/includes/footermobile.aspx" -->
<script type="text/javascript">
    ga_pg_id = "12";
</script>