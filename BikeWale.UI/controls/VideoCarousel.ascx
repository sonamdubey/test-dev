<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Controls.VideoCarousel" %>
 <div class="grid_12 column">
            <div class="content-block-white margin-top10">
                <h1 class="content-block-video">Videos</h1><div class="clear"></div>
                <div class="content-block white-shadow">
                    <div class="list_carousel" style="border:1px solid yellow;">
                        <div class="right-float arrow-position">
                            <a class="prev disabled" id="ucHome_prev" href="#"></a>
                            <a class="next" href="#" id="ucHome_next"></a>
                        </div><div class="clear"></div>
                        <div id="uc-container" class="carousel_wrapper">
                            <ul id="featuredVideos">
                           <%--     <asp:Repeater ID="rptFeatured" runat="server">
                                    <ItemTemplate>--%>
                                        <li>
                                            <div class="pic-place">
                                                <a href="#" target="_blank" rel="noopener">
                                	                <div class="video-sprite play-btn"></div>
                                	                <img src="https://img.youtube.com/vi/jcEaopeBfUU/mqdefault.jpg" alt="" title=""  width="200" height="120"/>
                                                </a>
                                            </div>
                                            <div class="content-place">
                            	                <p><a href="#" target="_blank" rel="noopener"><strong>Video Title</strong></a></p>
                                                <div>
                                	                <span class="video-sprite review-icon-light margin-right5 float-left"></span><span>34,789 Views</span>
                                                </div>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="pic-place">
                                                <a href="#" target="_blank" rel="noopener">
                                	                <div class="video-sprite play-btn"></div>
                                	                <img src="https://img.youtube.com/vi/cCS7WWY5hlk/mqdefault.jpg" alt="" title=""  width="200" height="120"/>
                                                </a>
                                            </div>
                                            <div class="content-place">
                            	                <p><a href="#" target="_blank" rel="noopener"><strong>Video Title</strong></a></p>
                                                <div>
                                	                <span class="video-sprite review-icon-light margin-right5"></span> <span>34,789 Views</span>
                                                </div>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="pic-place">
                                                <a href="#" target="_blank" rel="noopener">
                                	                <div class="video-sprite play-btn"></div>
                                	                <img src="https://img.youtube.com/vi/cCS7WWY5hlk/mqdefault.jpg" alt="" title=""  width="200" height="120"/>
                                                </a>
                                            </div>
                                            <div class="content-place">
                            	                <p><a href="#" target="_blank" rel="noopener"><strong>Video Title</strong></a></p>
                                                <div>
                                	                <span class="video-sprite review-icon-light margin-right5"></span><span>34,789 Views</span>
                                                </div>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="pic-place">
                                                <a href="#" target="_blank" rel="noopener">
                                	                <div class="video-sprite play-btn"></div>
                                	                <img src="https://img.youtube.com/vi/cCS7WWY5hlk/mqdefault.jpg" alt="" title=""  width="200" height="120"/>
                                                </a>
                                            </div>
                                            <div class="content-place">
                            	                <p><a href="#" target="_blank" rel="noopener"><strong>Video Title</strong></a></p>
                                                <div>
                                	                <span class="video-sprite review-icon-light margin-right5"></span> <span>34,789 Views</span>
                                                </div>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="pic-place">
                                                <a href="#" target="_blank" rel="noopener">
                                	                <div class="video-sprite play-btn"></div>
                                	                <img src="https://img.youtube.com/vi/jcEaopeBfUU/mqdefault.jpg" alt="" title=""  width="200" height="120"/>
                                                </a>
                                            </div>
                                            <div class="content-place">
                            	                <p><a href="#" target="_blank" rel="noopener"><strong>Video Title</strong></a></p>
                                                <div>
                                	                <span class="video-sprite review-icon-light margin-right5"></span><span>34,789 Views</span>
                                                </div>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="pic-place">
                                                <a href="#" target="_blank" rel="noopener">
                                	                <div class="video-sprite play-btn"></div>
                                	                <img src="https://img.youtube.com/vi/jcEaopeBfUU/mqdefault.jpg" alt="" title=""  width="200" height="120"/>
                                                </a>
                                            </div>
                                            <div class="content-place">
                            	                <p><a href="#" target="_blank" rel="noopener"><strong>Video Title</strong></a></p>
                                                <div>
                                	                <span class="video-sprite review-icon-light margin-right5"></span><span>34,789 Views</span>
                                                </div>
                                            </div>
                                        </li>

                                <%--     </ItemTemplate>
                                </asp:Repeater>--%>
                            </ul>
                     </div>
                    </div>
                </div>
            </div>
        </div>