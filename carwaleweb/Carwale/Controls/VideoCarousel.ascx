<%@ Control Language="C#" AutoEventWireup="false" Inherits="VideoCarousel" %>
<%@ Import Namespace="Carwale.UI.Common"%>
    <div class="list_carousel">
        <div class="rightfloat margin-right10 margin-bottom10 arrow-position">
            <a class="prev disabled" id="featuredVideos_prev" href="#"></a>
            <a class="next margin-left5" id="featuredVideos_next" href="#"></a>
        </div>
        <div id="uc-container" class="carousel_wrapper">
            <ul id="featuredVideos">
                <asp:Repeater ID="rptFeatured" runat="server">
                    <ItemTemplate>
                        <li>
                            <div class="pic-place">
                                <a href="/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"MakeName").ToString()) %>-cars/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"MaskingName").ToString()) %>/videos/<%# FormatSubCat(DataBinder.Eval(Container.DataItem,"SubCatName").ToString()) %>-<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>/" target="_blank">
                                	<div class="video-sprite play-btn"></div>
                                    <img class="lazy" width="210" height="112" src="https://imgd.aeplcdn.com/0x0/statics/grey.gif" data-original="<%# DataBinder.Eval(Container.DataItem,"ImagePath").ToString()!=""? DataBinder.Eval(Container.DataItem,"ImgHost").ToString() + Carwale.Utility.ImageSizes._210X118 + DataBinder.Eval(Container.DataItem,"ImagePath").ToString() :"https://img.youtube.com/vi/"+DataBinder.Eval(Container.DataItem,"VideoId").ToString()+"/mqdefault.jpg" %>"
                                        border="0" title="<%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %>" alt="<%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %>" border="0" />
                                </a>
                            </div>
                            <div class="content-place">
                            	<p><a href="/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"MakeName").ToString()) %>-cars/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem,"MaskingName").ToString()) %>/videos/<%# FormatSubCat(DataBinder.Eval(Container.DataItem,"SubCatName").ToString()) %>-<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>/" target="_blank"><strong><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %></strong></a></p>                                           
                                <%if(PageId==1) {%>
                                <div class="margin-top15">
                                	<span class="cw-sprite v-review-icon margin-right5"></span> <%# Convert.ToInt32( DataBinder.Eval(Container.DataItem,"Views").ToString()).ToString("#,##0") %> Views
                                </div>
                                <div class="margin-top10">
                                	<span class="cw-sprite v-like-icon margin-right10"></span><%# Convert.ToInt32( DataBinder.Eval(Container.DataItem,"Likes").ToString()) %> Likes
                                </div><%} else {%>
                                <div>
                                	<span class="video-sprite review-icon-light margin-right5"></span> <%# Convert.ToInt32( DataBinder.Eval(Container.DataItem,"Views").ToString()).ToString("#,##0") %> Views
                                </div> 
                                <%} %>
                            </div>
                        </li>
                        </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
    </div>