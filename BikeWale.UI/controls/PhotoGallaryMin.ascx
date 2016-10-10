﻿<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.PhotoGallaryMin" %>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="LD" TagName="LocateDealer" Src="/controls/locatedealer.ascx" %>
<h1 style="padding-bottom:10px;"><%=objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName %> Photos</h1>
<div id="imageContent">            
    <div class="clear"></div>
    <div id="gallery" class="ad-gallery" style="float:left; padding-top:10px;">  
        <div id="galleryHolder"  title="<%= objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + selectedImageCategory %> Photos" >              
            <div class="ad-image-wrapper" >
                <div class="ad-image" >
                    <img border="0" id="first" src="<%=selectedImagePath %>" itemprop="contentURL" />
                    <p class="ad-image-description" style="width: 600px;">
                        <strong class="ad-description-title" itemprop="description">
                            <%= objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + selectedImageCategory %> Photos
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
                    <asp:Repeater ID="rptPhotos" runat="server" EnableViewState="false">
	                    <itemtemplate>
                                <li>
                                    <a original-href="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem,"HostUrl").ToString(),Bikewale.Utility.ImageSize._640x348) %>">
                                   <img height="70" id='<%# Container.ItemIndex %>'
                                            src='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImgPath").ToString(),DataBinder.Eval(Container.DataItem,"HostUrl").ToString(),Bikewale.Utility.ImageSize._144x81) %>' 
                                            border="0" alt='<%# string.Format("{0} {1} - {2}",DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") , DataBinder.Eval(Container.DataItem,"ModelBase.ModelName"),DataBinder.Eval(Container.DataItem,"ImageCategory")) %>'
                                            title='<%# string.Format("{0} {1} - {2}",DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") , DataBinder.Eval(Container.DataItem,"ModelBase.ModelName"),DataBinder.Eval(Container.DataItem,"ImageCategory")) %>' 
                                            desc='<%# DataBinder.Eval(Container.DataItem, "Caption").ToString() %>'
                                            imgCnt='<%=FetchedCount %>' artTitle='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "ImageTitle").ToString().Replace("'","&rsquo;")) %>' />
                                          
			                        </a>
                                </li>                                
	                    </itemtemplate>
                    </asp:Repeater>  
                             
                    <div id="noImageAv" runat="server">
                        <li>
                            <a href='http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/no-img-big.png'>
				                <img src='http://imgd2.aeplcdn.com/0x0/bw/static/design15/old-images/d/no-img-thumb.png' border="0" style="height:70px;" title='No Images Available' alt='No Images Available' />
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
     <%--   <div style="height:165px;"><span>In this photo: </span><br /><a style="color:#fff;" href="/<%=objModelEntity.MakeBase.MaskingName %>-bikes/<%=objModelEntity.MaskingName %>/"><%=objModelEntity.MakeBase.MakeName + " " +objModelEntity.ModelName %></a>
            <div id="artDesc"></div>
        </div>--%>
        <div class="light-grey-bg content-block border-radius5 padding-bottom20 margin-top15">
                <BP:InstantBikePrice runat="server" ID="InstantBikePrice" />
            </div>
            <div class="light-grey-bg content-block border-radius5 margin-top10 padding-bottom20">
                <LD:LocateDealer runat="server" id="LocateDealer" />
            </div> 
    </div>
    <div class="clear"></div>
</div>
