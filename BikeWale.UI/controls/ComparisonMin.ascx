<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.controls.ComparisonMin" %>

    <div class="grid-6 margin-top20 margin-bottom20">
        <div class="border-solid-right">
            <h3 class="font16 text-center padding-bottom15">
                <a href="<%= FormatComparisonUrl(TopRecord.MakeMaskingName1,TopRecord.ModelMaskingName1,TopRecord.MakeMaskingName2,TopRecord.ModelMaskingName2, TopRecord.VersionId1.ToString(), TopRecord.VersionId2.ToString())%>">
                    <%= FormatBikeCompareAnchorText(TopRecord.Bike1,TopRecord.Bike2) %>
                </a>
            </h3>
            <div class="bike-preview margin-bottom10">
                <a href="<%= FormatComparisonUrl(TopRecord.MakeMaskingName1,TopRecord.ModelMaskingName1,TopRecord.MakeMaskingName2,TopRecord.ModelMaskingName2, TopRecord.VersionId1.ToString(), TopRecord.VersionId2.ToString())%>">
                    <img class="lazy" src="http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif" data-original="<%= TopCompareImage %>" title="<%= FormatBikeCompareAnchorText(TopRecord.Bike1,TopRecord.Bike2) %>" alt="<%= FormatBikeCompareAnchorText(TopRecord.Bike1,TopRecord.Bike2) %>">
                </a>
            </div>
            <div>
                <div class="grid-6 alpha border-solid-right">
                    <div class="content-inner-block-5 text-center">
                        <div class="font18 margin-bottom5">
                            <span class="bwsprite inr-lg-thin"></span> <%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(TopRecord.Price1)) %>
                        </div>
                        <div>
                            <% if (Convert.ToDouble(TopRecord.Review1) > 0)
                               {%>
                            <p class="margin-bottom10 ">
                                <%= Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(TopRecord.Review1)) %>
                            </p>
                            <%} else { %>
                            <p class="margin-bottom10 font14 ">
                                Not rated yet
                            </p>
                            <%} %>
                            <p class="font14"><a href="<%= Bike1ReviewLink %>" class="margin-left5"><%= Bike1ReviewText %></a></p>
                        </div>
                    </div>
                </div>
                <div class="grid-6 omega">
                    <div class="content-inner-block-5 text-center">
                        <div class="font18 margin-bottom5">
                            <span class="bwsprite inr-lg-thin"></span> <%= Bikewale.Utility.Format.FormatPrice(TopRecord.Price2.ToString()) %>
                        </div>
                        <div>
                            <% if (Convert.ToDouble(TopRecord.Review2) > 0)
                               {%>
                            <p class="margin-bottom5 ">
                                <%= Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(TopRecord.Review2)) %>
                            </p>
                            <%} else { %>
                            <p class="margin-bottom5 font14 ">
                                Not rated yet
                            </p>
                            <%} %>
                            <p class="font14"><a href="<%= Bike2ReviewLink %>" class="margin-left5"><%= Bike2ReviewText %></a></p>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </div>
    </div>
    <div class="grid-6 margin-top20 margin-bottom20">
        <div class="compare-list-home">
            <ul>
                <asp:Repeater runat="server" ID="rptCompareBike">
                    <ItemTemplate>
                        <li>
                            <p class="font16 text-center padding-bottom15">
                                <a href="<%# FormatComparisonUrl(DataBinder.Eval(Container.DataItem,"MakeMaskingName1").ToString(),DataBinder.Eval(Container.DataItem,"ModelMaskingName1").ToString(),DataBinder.Eval(Container.DataItem,"MakeMaskingName2").ToString(),DataBinder.Eval(Container.DataItem,"ModelMaskingName2").ToString(), DataBinder.Eval(Container.DataItem,"VersionId1").ToString(), DataBinder.Eval(Container.DataItem,"VersionId2").ToString()) %>">
                                    <%# FormatBikeCompareAnchorText(DataBinder.Eval(Container.DataItem,"Bike1").ToString(),DataBinder.Eval(Container.DataItem,"Bike2").ToString()) %>
                                </a>
                            </p>
                            <div>
                                <span class="margin-right50">
                                    <span class="bwsprite inr-md-light"></span> <span class="font16 text-xt-light-grey"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Price1").ToString()) %></span>
                                </span>
                                <span class="bwsprite inr-md-light"></span> <span class="font16 text-xt-light-grey"><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem,"Price2").ToString()) %></span>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <div class="text-center margin-top20">
                <a href="/comparebikes/" class="btn btn-orange">View more comparisons</a>
            </div>
        </div>
    </div>
    <div class="clear"></div>
