<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.PhotoGallaryMin" %>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/UI/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="LD" TagName="LocateDealer" Src="/UI/controls/locatedealer.ascx" %>
<h1 style="padding-bottom:10px;"><%=BikeName %> Images</h1>
<div id="imageContent">            
    <div class="clear"></div>
    <div id="gallery" class="ad-gallery" style="float:left; padding-top:10px;">  
        <div id="galleryHolder" >              
            <div class="ad-image-wrapper" >
                <div class="ad-image" >
                    <img border="0" title="<%= BikeName + selectedImageCategoryName %> Images" src="<%=selectedImagePath %>" itemprop="contentURL" />
                    <p class="ad-image-description" style="width: 600px;">
                        <strong class="ad-description-title" itemprop="description">
                            <%= BikeName + selectedImageCategory %> Images
                        </strong></p>
                    <meta itemprop="representativeOfPage" content="true">
                </div>
            </div>
            <div id="descriptions">
                <div class="ad-controls"></div>
            </div>
        </div>
        <div class="ad-nav">
            <div class="ad-thumbs">
                <ul id="galleryList" class="ad-thumb-list">
                <% 
                    foreach(Bikewale.Entities.CMS.Photos.ColorImageBaseEntity img in objImageList){ %>
                    <li>
                        <a original-href="<%= Bikewale.Utility.Image.GetPathToShowImages(img.OriginalImgPath,img.HostUrl,Bikewale.Utility.ImageSize._640x348) %>">
                            <img height="70" <%--id='<%=index %>--%>'
                                src='<%= Bikewale.Utility.Image.GetPathToShowImages(img.OriginalImgPath,img.HostUrl,Bikewale.Utility.ImageSize._144x81) %>'
                                border="0" alt='<%= string.Format("{0} - {1}",BikeName, img.ImageCategory) %>'
                                title='<%= string.Format("{0} - {1}",BikeName, img.ImageCategory) %>'
                                desc='<%= img.ImageTitle %>'
                                imgcnt='<%=FetchedCount %>' arttitle='<%= Server.HtmlEncode(img.ImageTitle.Replace("'","&rsquo;")) %>' />

                        </a>
                    </li>
                    <% } %>
                    <div id="noImageAv" visible="false" runat="server">
                        <li>
                            <a href='https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/no-img-big.png'>
				                <img src='https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/no-img-thumb.png' border="0" style="height:70px;" title='No Images Available' alt='No Images Available' />
			                </a>
                        </li>                          
                    </div>      
                </ul>
            </div>
            <div class="ad-forward"><a href="#"></a></div>
            <div class="ad-back"><a href="#"></a></div>
        </div>                
    </div>            
    <div id="imageInfo">
        <div class="light-grey-bg content-block border-radius5 padding-bottom20 margin-top15">
                <BP:InstantBikePrice runat="server" ID="InstantBikePrice" />
            </div>
            <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20">
                <LD:LocateDealer runat="server" id="LocateDealer" />
            </div> 
    </div>
    <div class="clear"></div>
</div>
