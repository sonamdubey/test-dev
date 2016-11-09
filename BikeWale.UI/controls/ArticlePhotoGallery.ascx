<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArticlePhotoGallery.ascx.cs" Inherits="Bikewale.Controls.ArticlePhotoGallery" %>
<div id="ctrlNewsPhotoGallery_taggedPhotogallery">
    <h2 class="border-solid-top padding-top10">Gallery</h2>
    <div id="gallery">
        <div class="jcarousel-wrapper article-jcarousel-wrapper">
            <div class="jcarousel article-jcarousel">
                <asp:Repeater ID="rptPhotos" runat="server">
                    <headertemplate>
                        <ul id="image-gallery">                
                    </headertemplate>
                        <itemtemplate>
                            <li>
                                <div class="article-img-container">
                                    <span>
                                        <img border="0" class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString(),Bikewale.Utility.ImageSize._640x348)%>" alt="<%#Eval("AltImageName") %>" title="<%#Eval("ImageTitle") %>" src="">
                                    </span>
                                </div>
                            </li>
                        </itemtemplate>
                    <footertemplate>
                        </ul>
                    </footertemplate>
                </asp:Repeater>
            </div>
            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive"></a></span>
            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
            <div class="clear"></div>  
        </div>
    </div>
</div>