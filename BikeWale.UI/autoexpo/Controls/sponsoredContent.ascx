<%@ Control Language="C#" AutoEventWireUp="false" Inherits="AutoExpo.sponsoredContent" %>
<span style="float:right;font-size:8px; margin-right:5px;">Sponsored Content</span>
<div style="clear:both;"></div>
<div style="float:left;">
    <%= SMainImageSet == "True" ? "<a class='cbBox' href='" + Bikewale.Common.ImagingFunctions.GetImagePath("/ec/", SHostURL.ToString()) + SBasicId + "/img/m/" + SBasicId + "_l.jpg'><img class='alignleft size-thumbnail img-border-news' style='margin:0 10px 0 10px;' src='" + Bikewale.Common.ImagingFunctions.GetImagePath("/ec/", SHostURL.ToString()) + SBasicId + "/img/m/" + SBasicId + ".jpg?v=1.4' align='right' border='0' /></a>" : ""%>
</div>
<div style="float:left; width:170px;">
    <a href="/autoexpo/2014/<%= SBasicId + "-" + SUrl %>.html"><h2 class="sph2"><%= SNewsTitle %></h2></a>     
</div>
<div class="spDetails" ><%= SDetails %></div>
<div style="float:right;margin-right:5px;"><a class="spKnow" href="http://www.chevrolet.co.in/experience/auto-expo/introduction.html" target="_blank">Chevrolet at Auto Expo</a></div>
<div class="spDetails"><a href="/autoexpo/2014/<%= SBasicId + "-" + SUrl %>.html" >Know more...</a></div>
<div style="clear:both;"></div>

