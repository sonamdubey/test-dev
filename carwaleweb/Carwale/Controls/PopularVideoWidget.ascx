<%@ Control Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Controls.PopularVideoWidget" %>
<%@ Import Namespace="Carwale.Entity.CMS" %>
<div class="content-box-shadow">
    <% if(videoFlag != 1){ %>
    <div class="content-inner-block-10">
        <h2 class="font18 text-bold">Popular Videos</h2>
    	<asp:Repeater ID="rptVideoData" runat="server">
                <ItemTemplate>
                    <div class="video-placer">
                        <p style="font-size:14px;"><strong><a href="/<%# ((VideosEntity)Container.DataItem).MakeName %>-cars/<%# ((VideosEntity)Container.DataItem).MaskingName%>/videos/<%# ((VideosEntity)Container.DataItem).SubCatName %>-<%# ((VideosEntity)Container.DataItem).BasicId %>/" target="_blank"><%# ((VideosEntity)Container.DataItem).Title %></a></strong></p>
                            
        	            <iframe width="290" height="250" src="<%# ((VideosEntity)Container.DataItem).VideoSrc %>" frameborder="0" allowfullscreen></iframe>
        	            <p><%# ((VideosEntity)Container.DataItem).Description %>...</p>
                        <div class="video-social-icons">
                            <div>       
                                 <span class="news-sprite v-fb-like-icon"></span> <%# ((VideosEntity)Container.DataItem).Likes %> Likes     
                            </div>
                            <div class="v-comment">
                           
                            </div>
                            <div class="v-views">
                                <span class="news-sprite v-views-icon"></span> <%# ((VideosEntity)Container.DataItem).Views %> Views
                                    
                            </div>
                        </div>
                </div>
                </ItemTemplate>
        </asp:Repeater>
    </div>
        <%}
       else if (rptVideoNewsList.Items.Count > 0)
       { %>
    <div class="content-inner-block-10">
        <h2 class="font18 text-bold">Popular Videos</h2>
            <div>
    	        <asp:Repeater ID="rptVideoNewsList" runat="server">
                 <ItemTemplate>
                     <div class="video-placer">
                         <p class="font14"><strong><a href="/<%# ((Video)Container.DataItem).MakeName %>-cars/<%# ((Video)Container.DataItem).MaskingName%>/videos/<%# ((Video)Container.DataItem).SubCatName %>-<%# ((Video)Container.DataItem).BasicId %>/" target="_blank"><%# ((Video)Container.DataItem).VideoTitle %></a></strong></p>
                            
        	                <iframe width="288" height="250" src="<%# ((Video)Container.DataItem).VideoUrl %>" frameborder="0" allowfullscreen></iframe>
        	                <p><%# ((Video)Container.DataItem).Description %>...</p>
                            <div class="video-social-icons">
                                <div>       
                                        <span class="news-sprite v-fb-like-icon"></span> <%# ((Video)Container.DataItem).Likes %> Likes     
                                </div>
                                <div class="v-comment">
                           
                                </div>
                                <div class="v-views">
                                    <span class="news-sprite v-views-icon"></span> <%# ((Video)Container.DataItem).Views %> Views
                                    
                                </div>
                            </div>
                    </div>
                  </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <%} %>
</div>

