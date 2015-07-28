<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.PhotoGallaryMin" %>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="LD" TagName="LocateDealer" Src="/controls/locatedealer.ascx" %>
<%--<h1 style="padding-bottom:10px;"><%=objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName %> Photos</h1>--%>
<div id="imageContent">            
    <div class="clear"></div>
    <div id="gallery" class="ad-gallery" style="float:left; padding-top:10px;">  
        <div id="galleryHolder">              
            <div class="ad-image-wrapper">
                <div class="ad-image">
                    <img border="0" src="<%=selectedImagePath %>" 
                        title="<%= objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + selectedImageCategory %> Photos" 
                        itemprop="contentURL" />
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
                                    <a original-href="<%# Bikewale.Common.ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"ImagePathLarge").ToString(),DataBinder.Eval(Container.DataItem,"HostUrl").ToString()) %>" 
                                        href="/<%# Bikewale.Common.UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"MakeBase.MakeName").ToString()) + "-bikes/" + DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName") + "/photos/" + (DataBinder.Eval(Container.DataItem,"MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem,"ModelBase.ModelName").ToString() + " " + DataBinder.Eval(Container.DataItem,"ImageId")).Replace(" ", "-") %>.html" >
				                            <img height="70" 
                                            src='<%# Bikewale.Common.ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"ImagePathThumbnail").ToString(),DataBinder.Eval(Container.DataItem,"HostUrl").ToString()) %>' 
                                            border="0" alt='<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelBase.ModelName") + " - " + DataBinder.Eval(Container.DataItem,"ImageCategory") %>'
                                            title='<%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelBase.ModelName") + " - " + DataBinder.Eval(Container.DataItem,"ImageCategory") %> ' 
                                            desc='<%# DataBinder.Eval(Container.DataItem, "Caption").ToString() %>' <%--artID='<%# DataBinder.Eval(Container.DataItem, "BasicId").ToString() %>'--%> 
                                            imgCnt='<%=recordCount %>' artTitle='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "ImageTitle").ToString().Replace("'","&rsquo;")) %>' 
                                           <%-- artUrl='<%# DataBinder.Eval(Container.DataItem, "ArticleUrl").ToString() %>'--%>/>
			                        </a>
                                </li>                                
	                    </itemtemplate>
                    </asp:Repeater>  
                             
                    <div id="noImageAv" runat="server">
                        <li>
                            <a href='http://img.carwale.com/adgallery/no-img-big.png'>
				                <img src='http://img.carwale.com/adgallery/no-img-thumb.png' border="0" style="height:70px;" title='No Images Available' alt='No Images Available' />
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
