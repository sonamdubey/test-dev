<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Content.ViewBikeCare" %>
<%@ Register Src="~/m/controls/MUpcomingBikesMin.ascx" TagPrefix="BW" TagName="MUpcomingBikesMin"  %>
<%@ Register Src="~/m/controls/MPopularBikesMin.ascx" TagPrefix="BW" TagName="MPopularBikesMin"  %>
<%@ Register Src="~/m/controls/ModelGallery.ascx" TagPrefix="BW" TagName="ModelGallery"  %>
<%	
    keywords = pageKeywords;
    title = pageTitle;
    description  = pageDescription;
    AdPath = "/1017752/Bikewale_Mobile_NewBikes";
    AdId = "1398766302464";
  
%>
<!-- #include file="/includes/headermobile.aspx" -->
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/m/src/loadPhotos.js?v=1.0"></script>

<style>
	.imgWidth{width:100%;max-width:100%;height:auto;}
	.ulist {margin:0px;padding:0px 0px 0px 15px;}
	.ulist li {margin-bottom:10px;}
	.over-flow {overflow:hidden;}
    .socialplugins li{float:left;width:84px;}
</style>
<div class="padding5">
        <h1><%= pageTitle %></h1>
        <div class="new-line5 f-12"><%=Bikewale.Utility.FormatDate.GetFormatDate(displayDate, "MMMM dd, yyyy hh:mm tt") %> | By <%=author %> <%= (bikeTested != null && !String.IsNullOrEmpty(bikeTested.ToString())) ? String.Format("| {0}",bikeTested) : "" %></div>
        <div class="new-line5">
            <ul class="socialplugins  new-line10">
                <li><fb:like href="<%= canonicalUrl%>" send="false" layout="button_count"  show_faces="false"></fb:like></li>
                <li><a href="https://twitter.com/share" class="twitter-share-button" data-url="<%= canonicalUrl %>" data-via='<%= title %>' data-lang="en">Tweet</a></li>
                <li><div class="g-plusone" data-size="medium" data-href="<%= canonicalUrl %>"></div></li>
            </ul>  
            <div class="clear"></div> 
        </div>
     <% if (objTipsAndAdvice != null)
        {%>
        <div class="box1 new-line5" style="position:relative;">
          <% foreach (var page in objTipsAndAdvice.PageList) {%>
                            <div class="margin-top10 margin-bottom10">
                                <h3 class="ui-bar ui-bar-a" role="heading"><%=page.PageName %></h3>
                                <div id='<%=page.pageId %>' class="margin-top-10 article-content">
                                    <%=page.Content %>
                                </div>
                            </div>
            <% } %>
					  
        </div>
    <%} %>
    <% if (objTipsAndAdvice != null)
        {%>
            <div class="box1 margin-top-10">
	            <div id="divPhotos" style="position:relative;top:-5px;">
                    <h3 class="ui-bar ui-bar-a" role="heading">Photos</h3>
		       <% foreach (var img in objImg){ %>
				            <div style="width:50%;float:left;margin-top:10px;">
					            <div style="margin:auto;width:80px;border:1px solid #DBDCDE;padding:3px;" onClick="ShowLargePhotos(this);" class="thumbDiv">
						            <img style="width:100%;max-width:100%;height:auto;" title="<%=img.ImageName%>" alt="<%=img.ImageName%>" src='<%=Bikewale.Utility.Image.GetPathToShowImages(img.OriginalImgPath,img.HostUrl,Bikewale.Utility.ImageSize._640x348) %>'>
					            </div>
				            </div>
			          <% } %>	
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
    <%} %>
</div>
<BW:MPopularBikesMin runat="server" ID="ctrlPopularBikes" />
<BW:MUpcomingBikesMin runat="server" ID="ctrlUpcomingBikes" />
<BW:ModelGallery runat="server" ID="photoGallery" />
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
<div class="back-to-top" id="back-to-top"></div>
<!-- #include file="/includes/footermobile.aspx" -->
<script type="text/javascript">
    ga_pg_id = "13";
</script>
