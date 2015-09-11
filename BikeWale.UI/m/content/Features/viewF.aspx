<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Content.viewF" Async="true" Trace="false" %>
<%	
    title = pageTitle + " - Bikewale ";
    keywords = "features, stories, travelogues, specials, drives";
    description = "";
    canonical = "http://www.bikewale.com" + baseUrl;
    AdPath = "/1017752/Bikewale_Mobile_NewBikes";
    AdId = "1398766302464";
    menu = "8";
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
            <a href="/m/features/" class="normal">Features</a> &rsaquo;
            <span class="lightgray"><%= pageTitle %></span>
        </div>
    
        <h1><%= pageTitle %></h1>
        <div class="new-line5 f-12"><%=displayDate %> | By <%=author %></div>
        <div class="new-line5">
            <ul class="socialplugins  new-line10">
                <li><fb:like href="http://www.bikewale.com<%= url%>" send="false" layout="button_count"  show_faces="false"></fb:like></li>
                <li><a href="https://twitter.com/share" class="twitter-share-button" data-url="http://www.bikewale.com<%= url %>" data-via='<%= title %>' data-lang="en">Tweet</a></li>
                <li><div class="g-plusone" data-size="medium" data-href="http://www.bikewale.com<%= url %>"></div></li>
            </ul>  
            <div class="clear"></div> 
       <%-- </div>
        <div class="new-line10 margin-bottom10 box1">
            <div class="text-bold">
                Read Pages :
            </div>	   
			<div style="padding:5px 0;">   
				<asp:Repeater ID="rptPages" runat="server">
                    <headertemplate>
                        <ul data-role="listview" data-inset="true">                        
                    </headertemplate>
					<itemtemplate>
                        <li>
                            <a href="#<%#Eval("pageId") %>"><%#Eval("PageName") %></a>
                        </li>
						
					</itemtemplate>
                    <footertemplate>
                        <li>
                            <a href="#divPhotos">Photos</a>
                        </li>
                        </ul>
                    </footertemplate>				
				</asp:Repeater>
			</div>	
        </div>--%>
        <div class="box1 new-line5" style="position:relative;">
	        <div id="divPageContent">
                 <div class="margin-top10">
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
            </div>
        </div>
        <div class="box1 margin-top-10">
            <div id="divPhotos" style="position:relative;top:-5px;">
		        <asp:Repeater id="rptPhotos" runat="server">
                    <headertemplate>
                        <div><h3 class="ui-bar ui-bar-a" role="heading">Photos</h3>
                    </headertemplate>
			        <itemtemplate>
				        <div style="width:50%;float:left;margin-top:10px;">
					        <div style="margin:auto;width:80px;border:1px solid #DBDCDE;padding:3px;" onClick="ShowLargePhotos(this);" class="thumbDiv">
						        <%--<img style="width:100%;max-width:100%;height:auto;" title="<%#DataBinder.Eval(Container.DataItem, "ImageName").ToString()%>" alt="<%#DataBinder.Eval(Container.DataItem, "ImageName").ToString()%>" src='<%# GetImageUrl(DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),DataBinder.Eval(Container.DataItem, "ImagePathLarge").ToString()) %>'>--%>
                                <img style="width:100%;max-width:100%;height:auto;" title="<%#DataBinder.Eval(Container.DataItem, "ImageName").ToString()%>" alt="<%#DataBinder.Eval(Container.DataItem, "ImageName").ToString()%>" src='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._640x348) %>'>
					        </div>
				        </div>
			        </itemtemplate>
                    <footertemplate>
                         </div>
                    </footertemplate>
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
        <%--<div class="box1 new-line5" style="overflow:hidden">
             <ul class="socialplugins  new-line10">
                <li><fb:like href="http://www.bikewale.com<%= baseUrl%>" send="false" layout="button_count"  show_faces="false"></fb:like></li>
                <li><a href="https://twitter.com/share" class="twitter-share-button" data-url="http://www.bikewale.com/<%= baseUrl %>/" data-via='<%= title %>' data-lang="en">Tweet</a></li>
                <li><div class="g-plusone" data-size="medium" data-href="http://www.bikewale.com/<%= baseUrl %>/"></div></li>
            </ul>
            <div class="clear"></div>
        </div>
        <div id="divPageNav" class="new-line10">
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
        $("#ddlPages").change(function ()
        {
            pageId = $(this).val();
            //alert(pageUrl + 'p' + pageId + '/');
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