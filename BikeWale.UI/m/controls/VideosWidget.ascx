<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.VideosWidget" %>
<div class="bw-tabs-data hide" id="ctrlVideos">
    <div class="jcarousel-wrapper">
        <div class="jcarousel">
            <ul>
                <asp:Repeater ID="rptVideos" runat="server">
                <ItemTemplate>
                <li>
                    <div class="front">
                        <div class="contentWrapper">
                            <div class="yt-iframe-preview">
                                <iframe frameborder="0" allowtransparency="true" src="<%# DataBinder.Eval(Container.DataItem,"VideoUrl").ToString() %>"></iframe>
                            </div>
                            <div class="bikeDescWrapper">
                                <div class="bikeTitle margin-bottom20">
                                    <h3><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %></h3>
                                </div>
                                <div class="margin-bottom15 text-light-grey">
                                    <span class="bwmsprite review-sm-lgt-grey"></span> Views <span><%# DataBinder.Eval(Container.DataItem,"Views").ToString() %></span>
                                </div>
                                <div class="text-light-grey">
                                    <span class="fa fa-thumbs-o-up text-light-grey margin-right5"></span> Likes <span><%# DataBinder.Eval(Container.DataItem,"Likes").ToString() %></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </li>
                </ItemTemplate>
                </asp:Repeater>                   
            </ul>
        </div>
        <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
        <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
        <p class="text-center jcarousel-pagination margin-bottom30"></p>
    </div>
    <%--<div class="text-center margin-bottom40 clear">
        <a class="font16" href="javascript:void(0)">View more videos</a>
    </div>--%>
    </div>