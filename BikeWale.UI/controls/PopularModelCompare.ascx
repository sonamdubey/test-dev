<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.PopularModelCompare" %>

<div id="modelComparisonContent" class="bw-model-tabs-data padding-top20 padding-bottom20 font14">
    <h2 class="padding-left20 padding-right20 margin-bottom15">Popular comparisons for  <%=ModelName%> </h2>
    <div class="jcarousel-wrapper inner-content-carousel margin-bottom20">
        <div class="jcarousel">
            <ul class="model-comparison-list">
                <asp:Repeater ID="rptPopularCompareBikes" runat="server">
                    <ItemTemplate>
                        <li>
                            <a href="/<%# Bikewale.Utility.UrlFormatter.CreateCompareUrl(DataBinder.Eval(Container.DataItem,"MakeMasking1").ToString(),DataBinder.Eval(Container.DataItem,"ModelMasking1").ToString(),DataBinder.Eval(Container.DataItem,"MakeMasking2").ToString(),DataBinder.Eval(Container.DataItem,"ModelMasking2").ToString(),DataBinder.Eval(Container.DataItem,"VersionId1").ToString(),DataBinder.Eval(Container.DataItem,"VersionId2").ToString()) %>"  title="<%#Bikewale.Utility.UrlFormatter.CreatePopularCompare( DataBinder.Eval(Container.DataItem, "Model1").ToString(), DataBinder.Eval(Container.DataItem, "Model2").ToString())  %>">
                                <h3 class="text-black text-center"><%#Bikewale.Utility.UrlFormatter.CreatePopularCompare( DataBinder.Eval(Container.DataItem, "Model1").ToString(), DataBinder.Eval(Container.DataItem, "Model2").ToString())  %></h3>
                                <div class="grid-6 alpha omega border-light-right">
                                    <div class="imageWrapper margin-bottom10">
                                        <div class="comparison-image">
                                            <img src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath1").ToString(),DataBinder.Eval(Container.DataItem,"HostURL1").ToString(),Bikewale.Utility.ImageSize._174x98) %>" />
                                            
                                        </div>
                                    </div>
                                    <p class="text-light-grey margin-bottom5">Ex-showroom, <%# DataBinder.Eval(Container.DataItem, "City1").ToString() %> </p>
                                    <span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-default text-bold"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem, "Price1").ToString())%></span>
                                </div>
                                <div class="grid-6 padding-left30 omega">
                                    <div class="imageWrapper margin-bottom10">
                                        <div class="comparison-image">
                                           <img src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath2").ToString(),DataBinder.Eval(Container.DataItem,"HostURL2").ToString(),Bikewale.Utility.ImageSize._174x98) %>" />
                                        </div>
                                    </div>
                                    <p class="text-light-grey margin-bottom5">Ex-showroom, <%# DataBinder.Eval(Container.DataItem, "City2").ToString() %></p>
                                    <span class="bwsprite inr-md-lg"></span>&nbsp;<span class="font22 text-default text-bold"><%#Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem, "Price2").ToString())%></span>
                                </div>
                                <div class="clear"></div>
                                <div class="margin-top20 text-center">
                                    <span class="btn btn-white btn-size-1">Compare now</span>
                                </div>
                            </a>
                        </li>

                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>

        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
    </div>

    <div class="margin-left20">
        <a href="/comparebikes/">View more comparisons<span class="bwsprite blue-right-arrow-icon"></span></a>
    </div>
</div>
