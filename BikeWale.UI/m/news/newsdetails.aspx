<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Content.newsdetails" Trace="false" Async="true" %>
<% 
    title = newsTitle + " - BikeWale News";
    description = "BikeWale coverage on " + newsTitle + ". Get the latest reviews and photos for " + newsTitle + " on BikeWale coverage.";
    canonical = "http://www.bikewale.com/news/" + pageUrl;
    fbTitle = newsTitle;
    fbImage = GetMainImagePath();
    AdPath = "/1017752/Bikewale_Mobile_NewBikes";
    AdId = "1398766302464";
    menu = "6";
%>
<!-- #include file="/includes/headermobile.aspx" -->
<style>
	.imgWidth{width:100%;max-width:100%;height:auto;}
	.ulist {margin:0px;padding:0px 0px 0px 15px;}
	.ulist li {margin-bottom:10px;}
	.over-flow {overflow:hidden;}   
    .socialplugins li{float:left;width:84px;}
</style>

<div class="padding10">
    <div id="br-cr">
        <a href="/m/" class="normal">Home</a> &rsaquo; 
        <a href="/m/news/" class="normal">News</a> &rsaquo; 
        <span class="lightgray"><%= newsTitle %></span>
    </div>
    <div class="pgsubhead"><%= newsTitle %></div>
    <div class="new-line5 lightgray f-12" style="font-size:13px;"><%= Bikewale.Utility.FormatDate.GetDaysAgo(displayDate) %>  | By <%= author  %></div>    
    <div class="new-line5">
        <ul class="socialplugins  new-line10">
            <li><fb:like href="http://www.bikewale.com/news/<%= pageUrl%>" send="false" layout="button_count"  show_faces="false"></fb:like></li>
            <li><a href="https://twitter.com/share" class="twitter-share-button" data-url="http://www.bikewale.com/news/<%= pageUrl %>" data-via='<%= title %>' data-lang="en">Tweet</a></li>
            <li><div class="g-plusone" data-size="medium" data-href="http://www.bikewale.com/news/<%= pageUrl %>"></div></li>
        </ul>  
        <div class="clear"></div> 
    </div>
    <div class="box1 new-line5">           
        <div id="divDesc" class="new-line10 article-content" style="font-size:14px; line-height:18px;">
            <%if(!String.IsNullOrEmpty(GetMainImagePath())) %>
                <img alt='<%= newsTitle%>' title='<%= newsTitle%>' src='<%= GetMainImagePath() %>'>
            <%= String.IsNullOrEmpty(newsContent) ? "" : newsContent %>
        </div>
    </div>
    
    <%if( !String.IsNullOrEmpty(nextPageUrl)) {%>
        <a href="/m/news/<%= nextPageUrl%>" class="normal">
            <div class="box1 new-line5">
                <div class="rightArrowContainer">
                    <div style="text-align:right;">
                        <div style="font-weight:bold;">Next Article</div>
                        <div class="new-line longText"><%=nextPageTitle %></div>  
                    </div>
                </div>
            </div>
        </a>
    <%} %>

    <%if( !String.IsNullOrEmpty(prevPageUrl)) {%>
        <a href="/m/news/<%= prevPageUrl %>" class="normal">
            <div class="box1 new-line5">
                <div class="leftArrowContainer">
                    <div>
                        <div style="font-weight:bold;">Previous Article</div>
                        <div class="new-line longText"><%=prevPageTitle %></div>  
                    </div>
                </div>
            </div>
        </a>
    <%} %>
</div>

<div class="back-to-top" id="back-to-top"><a><span></span></a></div>
<!-- #include file="/includes/footermobile.aspx" -->
