<%@ Page Language="C#" Trace="false" Inherits="Carwale.UI.Editorial.UserWrittenReviews" EnableViewState="false" EnableEventValidation="false" %>

<%@ Register TagPrefix="ur" TagName="UserReviews" Src="/Controls/CarwaleReviews.ascx" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>

    <%
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,

        PageId = 43;
        Title = "User Reviews on Cars in India";
        Description = "Know what users are saying about the car you aspire to buy. Read first hand user feedback on cars in India. Write your own review or write comments on others' reviews to let people know about your experience.";
        Keywords = "car user reviews, car users reviews, customer car reviews, customer car feedback, car reviews, car owner feedback, owner car reviews, owner report, owner comments";
        Revisit = "15";
        DocumentState = "Static";
        canonical = "https://www.carwale.com/userreviews/";
        altUrl = "https://www.carwale.com/m/userreviews/";
        AdId = "1396440332273";
        AdPath = "/1017752/ReviewsNews_";
        DeeplinkAlternatives = Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString();
    %>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <script type='text/javascript'>
        googletag.cmd.push(function () {
            googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath %>300x250_BTF', [300, 250], 'div-gpt-ad-<%= AdId %>-1').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
            <% if (Ad643 == true)
               { %>googletag.defineSlot('/7590/CarWale_NewCar/NewCar_Make_Page/NewCar_Model_Page/NewCar_Model_643x65', [643, 65], 'div-gpt-ad-1383197943786-0').addService(googletag.pubads()); <% } %>
            googletag.pubads().setTargeting("<%= targetKey %>", "<%= targetValue %>");
            googletag.pubads().setTargeting("City", "<%= Carwale.UI.Common.CookiesCustomers.MasterCity.ToString() %>");
            googletag.pubads().setTargeting('UserModelHistory', '<%= CookiesCustomers.UserModelHistory.Replace('~', ',')%>');
            //googletag.pubads().enableSyncRendering();
            googletag.pubads().collapseEmptyDivs();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });
    </script>
   
</head>


<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server" action="default.aspx">
        <!-- #include file="/includes/header.aspx" -->
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
        <div class="clear"></div>
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul class="special-skin-text" itemscope itemtype="http://schema.org/BreadcrumbList">
                            <li itemprop="itemListElement" itemscope itemtype="http://schema.org/ListItem"><a href="/" itemprop="item" title="Carwale"><span itemprop="name">Home</span></a><meta itemprop="position" content="1" /></li>
                            <li itemprop="itemListElement" itemscope itemtype="http://schema.org/ListItem"><span class="fa fa-angle-right margin-right10"></span><span itemprop="name">Reviews & News</span><meta itemprop="position" content="2" /></li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black special-skin-text">User Reviews</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-8">
                    <div class="content-box-shadow margin-bottom20">
                        <div class="grid-6">
                            <h2 class="margin-top10 margin-bottom15">Most Read</h2>
                            <div>
                                <ur:UserReviews ID="urUserReviews" ReviewCount="5" ShowComment="false" RetriveBy="MostRead" runat="server" />
                            </div>
                        </div>
                        <div class="grid-6">
                            <h2 class="margin-top10 margin-bottom15">Most Helpful</h2>
                            <div>
                                <ur:UserReviews ID="urUserReviewsMostHelpful" ReviewCount="5" ShowComment="false" RetriveBy="MostHelpful" runat="server" />
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="margin-top25"></div>
                        <div class="grid-6">
                            <h2 class="margin-bottom15">Most Recent</h2>
                            <div class="margin-bottom15">
                                <ur:UserReviews ID="urUserReviewsMostRead" ReviewCount="5" ShowComment="false" RetriveBy="MostRecent" runat="server" />
                            </div>
                        </div>
                        <div class="grid-6">
                            <h2 class="margin-bottom15">Most Rated</h2>
                            <div class="margin-bottom15">
                                <ur:UserReviews ID="urUserReviewsMostRated" ReviewCount="5" ShowComment="false" RetriveBy="MostRated" runat="server" />
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="grid-4">
                        <div class="content-box-shadow content-inner-block-10 margin-bottom20">
                            <h2 class="margin-bottom10">Write Your Own Review</h2>
                            <div id="write-review" class="margin-bottom25">
                                <div class="form-control-box leftfloat margin-right10" style="width: 90px;">
                                    <span class="select-box fa fa-angle-down"></span>
                                    <asp:DropDownList ID="drpMake" data-bind="value: Make" CssClass="form-control" runat="server" />
                                </div>
                                <div class="form-control-box leftfloat margin-right10" style="width: 90px;">
                                    <span class="select-box fa fa-angle-down"></span>
                                    <asp:DropDownList ID="drpModel" data-bind="foreach: Models" CssClass="form-control" runat="server">
                                        <asp:ListItem data-bind="value: ModelId,text:ModelName, attr: { 'mask': MaskingName }"></asp:ListItem>
                                    </asp:DropDownList>
                                    <input type="hidden" id="hdn_drpModel" runat="server" />
                                </div>
                                <div class="leftfloat" style="width: 80px;">
                                    <asp:Button ID="btnWrite" CssClass="btn btn-orange btn-xs" runat="server" Text="Next" />
                                </div>
                                <div class="clear"></div>
                                <span id="spnModel" class="text-red"></span>
                                <div class="clear"></div>
                            </div>
                        </div>
                        <div class="clear"></div>
                    <div class="mid-box">
                            <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 20, 20, false, 0) %>
                        </div>
                        <div class="content-box-shadow content-inner-block-10 margin-bottom20">
                            <h2 class="margin-bottom10">Most Reviewed Cars</h2>
                            <ul>
                                <asp:Repeater ID="rptMostReviewed" runat="server">
                                    <ItemTemplate>
                                        <li><a href="/<%# Carwale.UI.Common.UrlRewrite.FormatSpecial( DataBinder.Eval(Container.DataItem, "CarMake").ToString() )%>-cars/<%# DataBinder.Eval(Container.DataItem, "MaskingName").ToString() %>/userreviews/">
                                            <%# DataBinder.Eval(Container.DataItem, "ModelName")%> 
                                        </a>(<%# DataBinder.Eval(Container.DataItem, "TotalReviews")%> reviews)
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                        <div class="mid-box">
                            <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 0, 20, false, 1) %>
                        </div>

                </div>
                <div class="clear"></div>
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-10">
                        <h2 class="margin-top15">Browse By Car Makes</h2>
                        <div>
                            <asp:Repeater ID="rptMakes" runat="server">
                                <ItemTemplate>
                                    <h2><span><%# DataBinder.Eval(Container.DataItem, "MakeName")%></span></h2>
                                    <asp:DataList ID="dtlstPhotos" runat="server"
                                        RepeatColumns="6"
                                        DataSource='<%# GetDataSource( DataBinder.Eval(Container.DataItem, "MakeId").ToString() ).Tables[0] %>'
                                        RepeatDirection="Horizontal" CellPadding="3" CellSpacing="7"
                                        RepeatLayout="Table">
                                        <ItemStyle HorizontalAlign="left"></ItemStyle>
                                        <ItemTemplate>
                                            <a href="/<%# Carwale.UI.Common.UrlRewrite.FormatSpecial( DataBinder.Eval(Container.DataItem, "CarMake").ToString() )%>-cars/<%# DataBinder.Eval(Container.DataItem, "MaskingName").ToString() %>/userreviews/">
                                                <%# DataBinder.Eval(Container.DataItem, "ModelName")%> 
							            (<%# DataBinder.Eval(Container.DataItem, "TotalReviews")%>)
                                            </a>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->

        <script type="text/javascript">
            Common.showCityPopup = false;
            var carDataDroplist = eval('(' + genericMakeModelKVM + ')');
            carDataDroplist.Make(0);

            $(document).ready(function () {
                ko.applyBindings(carDataDroplist, $('#write-review')[0]);
                $('#drpModel').attr('disabled', true);
                $('#drpMake').change(function () {
                    bindModelsList("all", $('#drpMake option:selected').val(), carDataDroplist, '#drpModel', "--Select--");
                });
            });
            //document.getElementById("drpMake").onchange = drpMake_Change;
            document.getElementById("btnWrite").onclick = btnWrite_Click;
            function btnWrite_Click() {
                if (document.getElementById("drpModel").selectedIndex == 0) {
                    document.getElementById("spnModel").innerHTML = "Please select model to continue!";
                    return false;
                }
                else
                    document.getElementById("spnModel").innerHTML = "";
            }
        </script>
    </form>
</body>
</html>

