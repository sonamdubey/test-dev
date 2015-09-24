<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.VideosControl" %>
<div class="bw-tabs-data hide" id="ctrlVideos"><!-- Videos data code starts here-->
    <asp:Repeater ID="rptVideos" runat="server">
        <ItemTemplate>
            <div class="padding-bottom30">
                <div class="grid-4 alpha">
                    <div class="yt-iframe-preview">
                        <iframe frameborder="0" allowtransparency="true" src="<%# DataBinder.Eval(Container.DataItem,"VideoUrl").ToString() %>"></iframe>
                    </div>
                </div>
                <div class="grid-8 omega">
                    <h2 class="margin-bottom10 font20"><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %><%--<a href="#" class="text-black"><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString() %></a>--%></h2>
                    <p class="margin-bottom10 text-light-grey font14">Updated on <span><%# Bikewale.Utility.FormatDate.GetDaysAgo(DataBinder.Eval(Container.DataItem, "DisplayDate").ToString()) %></span></p>
                    <div class="margin-bottom15 text-light-grey"><span class="bwsprite review-sm-lgt-grey"></span> Views <span><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Views").ToString()) %></span></div>
                    <div class="text-light-grey"><span class="fa fa-thumbs-o-up text-light-grey margin-right5"></span> Likes <span><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Likes").ToString()) %></span></div>
                </div>
                <div class="clear"></div>
            </div>
        </ItemTemplate>
    </asp:Repeater>                        
    <%--<div class="padding-bottom50 text-center">
        <a href="#" class="font16">View more videos</a>
    </div>--%>
</div><!-- Ends here-->