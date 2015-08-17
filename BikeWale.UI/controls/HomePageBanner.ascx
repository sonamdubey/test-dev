<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.HomePageBanner"%>
<%@ Import Namespace="Bikewale.Common" %>
<div id="divBannerData">
    <asp:Repeater ID="rptFullImage" runat="server">
        <HeaderTemplate>
            <div class="left-float">
                <ul class="bike-img-list" style="height: 280px;">
        </HeaderTemplate>
        <ItemTemplate>            
            <li name="banner-<%# DataBinder.Eval( Container.DataItem, "BasicId" ) %>" <%# Category =="0" ? "id=\"banner-full-feature-"+DataBinder.Eval( Container.DataItem, "BasicId" )+"\"":"id=\"banner-full-"+DataBinder.Eval( Container.DataItem, "BasicId" )+"\"" %> <%# Container.ItemIndex == 0 ? "":"class='hide'" %> >
                <a href="<%# GetFeaturedArticlesLink(DataBinder.Eval(Container.DataItem, "ArticleUrl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem, "BasicId")), Convert.ToUInt16(DataBinder.Eval(Container.DataItem, "CategoryId"))) %>">
                    <%--<img src="<%# ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "LargePicUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString()) %>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString() %>" alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString() %>" hspace="0" vspace="0" border="0" height="273" width="503"/>--%>
                    <img src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._566x318) %>" title="<%# DataBinder.Eval(Container.DataItem, "Title").ToString() %>" alt="<%# DataBinder.Eval(Container.DataItem, "Title").ToString() %>" hspace="0" vspace="0" border="0" height="273" width="503"/>
                    <div class="banner-title" style="position:relative; z-index:1001; top:-37px; padding-left:10px; background: none repeat scroll 0 0 #000000; opacity: 0.5;filter: alpha(opacity = 50); width:493px; height:30px; padding-top:5px; color:#fff; font-size:16px; font-weight:bold;"><%# DataBinder.Eval( Container.DataItem, "Title" ) %></div>
                </a>
            </li>
        </ItemTemplate>
        <FooterTemplate>
                </ul>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Repeater ID="rptThumbBanner" runat="server">
        <HeaderTemplate>
            <div class="right-float">
                <ul class="gallery-thumb-container">
        </HeaderTemplate>
        <ItemTemplate>            
            <li <%# Category =="0" ? "id=\"banner-thumb-feature-"+DataBinder.Eval( Container.DataItem, "BasicId" )+"\"":"id=\"banner-thumb-"+DataBinder.Eval( Container.DataItem, "BasicId" )+"\"" %> onclick="javascript:SelectBannerImage(this);"  class="selected">
                <%--<img title="<%# DataBinder.Eval( Container.DataItem, "Title" ) %>" alt="<%# DataBinder.Eval( Container.DataItem, "Title" ) %>" src="<%# ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "LargePicUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString()) %>" hspace="0" vspace="0" border="0"  height="59" width="88" />                --%>
                <img title="<%# DataBinder.Eval( Container.DataItem, "Title" ) %>" alt="<%# DataBinder.Eval( Container.DataItem, "Title" ) %>" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._210x118) %>" hspace="0" vspace="0" border="0"  height="59" width="88" />
            </li>
        </ItemTemplate>
        <FooterTemplate>
                </ul>
            </div>
            <div class="clear"></div>
        </FooterTemplate>
    </asp:Repeater>        
</div>