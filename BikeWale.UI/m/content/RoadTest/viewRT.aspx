<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.viewRT" Async="true" Trace="false" %>
<%	
    keywords     = modelName + " ,road test, road tests, roadtests, roadtest, bike reviews, expert bike reviews, detailed bike reviews, test-drives, comprehensive bike tests, bike preview, first drives";
    title        = pageTitle + " - BikeWale.";
    description  = "BikeWale tests " + modelName + ", Read the complete road test report to know how it performed.";
    canonical    = canonicalUrl;
    AdPath = "/1017752/Bikewale_Mobile_NewBikes";
    AdId = "1398766302464";
    menu = "7";
%>
<!-- #include file="/includes/headermobile.aspx" -->
<script type="text/javascript" src="/m/src/loadPhotos.js?v=1.0"></script>

<style>
	.imgWidth{width:100%;max-width:100%;height:auto;}
	.ulist {margin:0px;padding:0px 0px 0px 15px;}
	.ulist li {margin-bottom:10px;}
	.over-flow {overflow:hidden;}
    .socialplugins li{float:left;width:84px;}
</style>
<div class="padding5">
        <div id="br-cr">
            <a href="/m/" class="normal">Home</a> &rsaquo; 
            <a href="/m/road-tests/" class="normal">Road Tests</a> &rsaquo;
            <span class="lightgray"><%= pageTitle %></span>
        </div>
    
        <h1><%= pageTitle %></h1>
        <div class="new-line5 f-12"><%=displayDate %> | By <%=author %>| <%=_bikeTested %></div>
        <div class="new-line5">
            <ul class="socialplugins  new-line10">
                <li><fb:like href="<%= canonicalUrl%>" send="false" layout="button_count"  show_faces="false"></fb:like></li>
                <li><a href="https://twitter.com/share" class="twitter-share-button" data-url="<%= canonicalUrl %>" data-via='<%= title %>' data-lang="en">Tweet</a></li>
                <li><div class="g-plusone" data-size="medium" data-href="<%= canonicalUrl %>"></div></li>
            </ul>  
            <div class="clear"></div> 
        </div>
        <%--<div class="new-line10 margin-bottom10">
            <select id="ddlPages" name="ddlPages" runat="server"></select>
            <div style="padding:5px 0;">			   
				<asp:Repeater ID="rptPages" runat="server">
                    <headertemplate>
                        <ul data-role="listview" data-theme="d" data-inset="true">
                            <li style="border:none;" ><a>Read Pages : </a></li>
                    </headertemplate>
					<itemtemplate>
                        <li>
                            <a href="#<%#Eval("pageId") %>"><%#Eval("PageName") %></a>
                        </li>

					</itemtemplate>
                    <footertemplate>
                        </ul>
                    </footertemplate>
				</asp:Repeater>
            </div>
        </div>--%>
        <div class="box1 new-line5" style="position:relative;">
             <asp:Repeater ID="rptPageContent" runat="server">
					    <itemtemplate>
                            <div class="margin-top10 margin-bottom10">
                                <h3 class="ui-bar ui-bar-a" role="heading"><%#Eval("PageName") %></h3>
                                <div id='<%#Eval("pageId") %>' class="margin-top-10 article-content">
                                    <%#Eval("content") %>
                                </div>
                            </div>
					    </itemtemplate>             
				</asp:Repeater>
        </div>
            <div class="box1 margin-top-10">
	            <div id="divPhotos" style="position:relative;top:-5px;">
                    <h3 class="ui-bar ui-bar-a" role="heading">Photos</h3>
		            <asp:Repeater id="rptPhotos" runat="server">
			            <itemtemplate>
				            <div style="width:50%;float:left;margin-top:10px;">
					            <div style="margin:auto;width:80px;border:1px solid #DBDCDE;padding:3px;" onClick="ShowLargePhotos(this);" class="thumbDiv">
						            <img style="width:100%;max-width:100%;height:auto;" title="<%#DataBinder.Eval(Container.DataItem, "ImageName").ToString()%>" alt="<%#DataBinder.Eval(Container.DataItem, "ImageName").ToString()%>" src='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._640x348) %>'>
					            </div>
				            </div>
			            </itemtemplate>
		            </asp:Repeater>	
		            <div style="clear:both;"></div>	
	            </div>
	            <div id="divPhotosOverlay"></div>
	            <div id="divLargeImgContainer" style="display:none;">
		            <div id="divLargeImg" style="margin-top:10px;"></div>
		            <div>
			            <div class="prev" onClick="PrevClicked();"><span class="arr-big">«<span style="position:relative;left:-3px;">«</span><span style="position:relative;left:-6px;">«</span><span style="position:relative;left:-9px;">«</span></span></div>
			            <div class="next" onClick="NextClicked();"><span class="arr-big"><span style="position:relative;right:-9px;">»</span><span style="position:relative;right:-6px;">»</span><span style="position:relative;right:-3px;">»</span>»</span></div>
			            <div style="clear:both;"></div>
		            </div>
		            <div>
			            <table style="width:100%;" cellpadding="0" cellspacing="0" border="0">
				            <tr><td align="center"><span onClick="ShowThumbnails();">View Thumbnails&nbsp;&nbsp;<span class="arr-small">&raquo;</span></span></td></tr>
			            </table>
		            </div>
	            </div>
        </div>
       <%-- <div id="divPageNav" class="new-line10">
            <% if( !String.IsNullOrEmpty(prevPageUrl)) {%>
	        <div class="prev"><a class="normal ui-link" href="<%=prevPageUrl %>"><span class="arr-big">&laquo;</span>&nbsp;Prev</a></div>
            <%} %>
            <% if( !String.IsNullOrEmpty(nextPageUrl)) {%>
	        <div class="next"><a class="normal ui-link" href="<%=nextPageUrl %>">Next<span class="arr-big">&nbsp;&raquo;</span></a></div>	
            <%} %>
	        <div style="clear:both;"></div>
        </div>--%>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        var pageId = 1;
        var pageUrl = '<%= baseUrl%>';
        $("#ddlPages").change(function () {
            pageId = $(this).val();
            window.location.href = pageUrl + 'p' + pageId + '/';

        });
    });

    function LoadLargePhoto() {
        var imgLarge = $(document.createElement("img"));
        imgLarge.attr("src", fullUrl)
        imgLarge.attr("style", "height: auto !important;max-width: 100% !important;width: 100%;");
        imgLarge.bind("load", function () {
            setTimeout(function () {
                $("#divLargeImg").html("");
                imgLarge.appendTo("#divLargeImg");
                $("#divPhotosOverlay").hide();
                $("#divPhotos").hide();
                $("#divLargeImgContainer").show();
                $("#divLargeImgContainer .prev").show();
                $("#divLargeImgContainer .next").show();
                if (currentIndex == 0)
                    $("#divLargeImgContainer .prev").hide();

                if (currentIndex == (parseInt(totalThumbDivs) - 1))
                    $("#divLargeImgContainer .next").hide();
            }, 2000);
        });
    }

    function ShowThumbnails() {
        $("#divPhotosOverlay").hide();
        $("#divLargeImgContainer").hide();
        $("#divPhotos").show();
    }

</script>
<!-- #include file="/includes/footermobile.aspx" -->