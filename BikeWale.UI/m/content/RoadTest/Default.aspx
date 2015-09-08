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
%>
<!-- #include file="/includes/headermobile.aspx" -->
<script type="text/javascript" src="http://st2.aeplcdn.com/bikewale/src/BikeWaleCommon.js?v=3.2"></script>
<form runat="server">
<div class="padding5">
    <div id="br-cr"><a href="/m/" class="normal">Home</a> &rsaquo; <span class="lightgray">Road Test</span></div>
    <div class="box1 new-line5 hide" onClick="BoxClicked(this);" type="expando">
	    <div class="heading">Search By : </div>
	    <div class="plus"></div>
    </div>
    <div class="box-bot" style="padding:10px;display:none;">
	    <div><select id="ddlMakes" runat="server"></select>
	    </div>
	    <div class="new-line10">
            <img id="imgLoaderModels" src="http://img.carwale.com/bikewaleimg/images/circleloader.gif" width="16" height="16" style="position:relative;top:3px;display:none;" /> 
            <select id="ddlModels"  runat="server">
                <option value="0">--Select Model--</option>
            </select>
	    </div>
	    <div class="new-line10">
		    <div onClick="SearchClicked();" style='display:none;width:60px;height:30px;background-image: url("/images/nav.png");background-position: 0px 0px;border-radius:7px;-moz-border-radius:7px;-webkit-border-radius:7px;float:left;'>
			    <div style="margin:auto;width:50px;height:30px;background-image: url('/images/icons-sheet.png');background-position: 12px -672px;"></div>
		    </div>
		    <div style="width:60px;float:left;">
			  <%--  <input type="button" id="btnSubmit" class="linkButtonBig"  Text="&nbsp;&nbsp;&nbsp;&nbsp;GO&nbsp;&nbsp;&nbsp;&nbsp;" />--%>
                  <a id="filterButton" href="#" class="normal" style="color:#fff !important;"><span class="linkButton">Go</span></a>
		    </div>
		    <div id="errSearch" class="error" style="padding:8px;width:160px;float:left;display:none;">Select make</div>
		    <div style="clear:both;"></div>
	    </div>
	    <div class="new-line10"><a class="normal" href="/m/road-tests/">Show all road tests&nbsp;&nbsp;<span class="arr-small">»</span></a></div>
    </div>
    <div class="pgsubhead">Latest bike Road Tests</div>
    <div id="divListing">
        <asp:Repeater id="rptRoadTest" runat="server">
            <itemtemplate>
                <a class="normal" href='/m/road-tests/<%#DataBinder.Eval(Container.DataItem, "ArticleUrl").ToString()%>-<%# DataBinder.Eval(Container.DataItem, "BasicId") %>.html' >
		            <div class="box1 new-line15" >
                        <table cellspacing="0" cellpadding="0" style="width:100%;overflow:visible;">
					        <tr>
						        <td style="width:100px;vertical-align:top;"><img style="width:100%;max-width:100%;height:auto;" alt='Road Test: <%# DataBinder.Eval(Container.DataItem, "Title") %>' title="Road Test: <%# DataBinder.Eval(Container.DataItem, "Title") %>" src='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>'></td>
						        <td valign="top" style="padding-left:10px;">
			                        <div class="sub-heading">
				                        Road Test: <%# DataBinder.Eval(Container.DataItem, "Title") %>&nbsp;&nbsp;<span class="arr-small">&raquo;</span>
			                        </div>
			                        <div class="lightgray new-line" style="font-size:13px; margin-bottom:10px;">
				                        by <%# DataBinder.Eval(Container.DataItem, "AuthorName") %>
			                        </div>
                                    <div style="border:1px solid #b3b4c6;background-color:#ffffff;width:100px;position:absolute;right:-1px;bottom:-10px;padding:2px 2px;font-size:13px;" class="lightgray">
                                        <%# CommonOpn.GetDisplayDate(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString()) %>
                                    </div>
                                </td>
                            </tr>
                        </table>
		            </div>
	            </a>
            </itemtemplate>
        </asp:Repeater>                
    </div>  
    <Pager:Pager ID="listPager" runat="server" />  
</div>
</form>
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