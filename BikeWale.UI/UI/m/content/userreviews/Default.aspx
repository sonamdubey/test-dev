<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Content.Default" Trace="false" %>

<%     
    title = "User Reviews on Bikes in India";
    description = "Know what users are saying about the bike you aspire to buy. Read first hand user feedback on bikes in India. Write your own review or write comments on others' reviews to let people know about your experience.";
    keywords = "bike user reviews, bike users reviews, customer bike reviews, customer bike feedback, bike reviews, bike owner feedback, owner bike reviews, owner report, owner comments";
    canonical = "https://www.bikewale.com/user-reviews/";
    AdPath = "/1017752/Bikewale_Mobile_NewBikes";
    AdId = "1398766302464";
    menu = "9";
%>
<!-- #include file="/UI/includes/headermobile.aspx" -->
<link rel="stylesheet" type="text/css" href="/UI/m/css/user-review/landing.css" />
<script type="text/javascript" src="https://stb.aeplcdn.com/bikewale/src/BikeWaleCommon.js?v=3.2"></script>

<section>
    <% 
        string q = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64(string.Format("csrc={0}",(int)Bikewale.Entities.UserReviews.UserReviewPageSourceEnum.Mobile_UserReview_Landing));
        string hrefStr = string.Format("/m/bike-review-contest/?q={0}", q);
         %>
	<a href="<%= hrefStr %>" class="contest-slug-sm slug-teal-target">
		<span class="trophy-white"></span>
		<p class="contest-slug__label">Write a review and win Amazon vouchers worth &#x20B9;<%= Bikewale.Utility.Format.FormatPrice(Bikewale.Utility.BWConfiguration.Instance.ContestPriceMoney)%></p>
		<span class="bwmsprite arrow-white-right"></span>
	</a>
</section>

<div class="padding5">
    <div id="br-cr">
        <span itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
            <a href="/m/new-bikes-in-india/" itemprop="url" class="normal"><span itemprop="title">New Bikes</span></a></span> &rsaquo; 
            <span class="lightgray">User Reviews</span>
    </div>
    <h1>Read user reviews</h1>
    <div class="box1 new-line5">
        <div class="new-line5">
            <asp:dropdownlist id="ddlMake" runat="server" class="textAlignLeft" data-mini="true"><asp:ListItem Text="--Select Make--" Value="0" /></asp:dropdownlist>
        </div>
        <div id="divModel" class="new-line15">
            <img id="imgLoaderMake" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif" width="16" height="16" style="position: relative; top: 3px; display: none;" />
            <select data-mini="true" id="ddlModel">
                <option value="0">--Select Model--</option>
            </select>
        </div>
        <div class="new-line15">
            <input type="button" value="Go" data-theme="b" id="btnSubmit" data-mini="true" data-role="button" data-inline="true" class="ui-corner-all">
        </div>
    </div>

    <div id="rate-bike-landing">
    <h2 class="margin-top20 margin-bottom10">Rate your bike</h2>
    <div class="box1 new-line5">
        <div class="circle-placeholder text-center">
            <span class="bwmsprite write-icon margin-top25"></span>
        </div>
        <div class="text-light-grey font16 padding-top15">
            Rate your bike and help others with your experience. Share your ratings and review now!
        </div>
        <div class="new-line15">
            <a id="rateBike" class="btn btn-teal padding-left15 padding-right15" data-bind="click: function (d, e) { openBikeSelection(); }">Select your bike to rate</a>
        </div>

        <% if (objMakes != null)
           { %>
        <!-- select bike starts here -->
        <div id="select-bike-cover-popup" data-bind="with: bikeSelection" class="cover-window-popup">
            <div class="ui-corner-top">
                <div id="close-bike-popup" class="cover-popup-back cur-pointer leftfloat" data-bind="click: closeBikePopup">
                    <span class="bwmsprite fa-angle-left"></span>
                </div>
                <div class="cover-popup-header leftfloat">Select bikes</div>
                <div class="clear"></div>
            </div>
            <div class="bike-banner"></div>
            <div id="select-make-wrapper" class="cover-popup-body" data-bind="visible: currentStep() == 1">
                <div class="cover-popup-body-head">
                    <p class="no-back-btn-label head-label inline-block">Select Make</p>
                </div>
                <ul class="cover-popup-list with-arrow">
                    <% foreach (var make in objMakes)
                       { %>
                    <li data-bind="click: makeChanged"><span data-masking="<%= make.MaskingName %>" data-id="<%= make.MakeId %>"><%= make.MakeName %></span></li>
                    <% } %>
                </ul>
            </div>

            <div id="select-model-wrapper" class="cover-popup-body" data-bind="visible: currentStep() == 2">
                <div class="cover-popup-body-head">
                    <div data-bind="click: modelBackBtn" class="body-popup-back cur-pointer inline-block">
                        <span class="bwmsprite back-long-arrow-left"></span>
                    </div>
                    <p class="head-label inline-block">Select Model</p>
                </div>
                <ul class="cover-popup-list with-arrow" data-bind="foreach: modelArray">
                    <li data-bind="click: $parent.modelChanged">
                        <span data-bind="text: modelName, attr: { 'data-id': modelId }"></span>
                    </li>
                </ul>
            </div>
            <div class="cover-popup-loader-body" data-bind="visible: IsLoading()">
                    <div class="cover-popup-loader"></div>
                    <div class="cover-popup-loader-text font14" data-bind="text: LoadingText()"></div>
                </div>
        </div>
        <!-- select bike ends here -->
        <% } %>
    </div>
        </div>


    <div>
        <h2 class="margin-top30 margin-bottom20">Most Reviewed Bikes</h2>
    </div>
    <div id="mostReviewed" class="box new-line5" style="padding: 0px 5px;">
        <asp:repeater id="rptMostReviewed" runat="server">
                <itemtemplate>
                    <a href='/m/<%#DataBinder.Eval(Container.DataItem,"MakeEntity.MaskingName") %>-bikes/<%#DataBinder.Eval(Container.DataItem,"ModelEntity.MaskingName") %>/reviews/' style="text-decoration:none;" class="normal">                           
                        <div class="container">
                            <div class="sub-heading">
		                        <b><%# DataBinder.Eval(Container.DataItem, "ModelEntity.ModelName")%> </b> (<%#DataBinder.Eval(Container.DataItem, "ReviewsCount ")%>)&nbsp;&nbsp;<span class="arr-small">&raquo;</span>
	                        </div>    
                        </div>
                    </a>
                </itemtemplate>
            </asp:repeater>
    </div>
    <div>
        <h2 class="margin-top30 margin-bottom20">Most Read Bikes</h2>
    </div>
    <div id="divMostRead" class="box new-line5" style="padding: 0px 5px;">
        <asp:repeater id="rptMostRead" runat="server">
		    <itemtemplate>
                <a href='/m/<%#DataBinder.Eval(Container.DataItem,"TaggedBike.MakeEntity.MaskingName")%>-bikes/<%#DataBinder.Eval(Container.DataItem,"TaggedBike.ModelEntity.MaskingName") %>/user-reviews/<%#DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewId") %>.html' style="text-decoration:none;" class="normal">    
		            <div class="container">
                        <div class="sub-heading">
				            <b><%#DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewTitle ") %></b> &nbsp;&nbsp;<span class="arr-small">&raquo;</span>
			            </div>    
                        <div class="darkgray new-line5 f-12">
                            By <%#DataBinder.Eval(Container.DataItem,"ReviewEntity.WrittenBy") %>
                        </div>
                        <div class="darkgray new-line5" style="font-size:0px;">
                           <%# Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble( DataBinder.Eval(Container.DataItem,"ReviewRating.OverAllRating"))) %>
                            <span class="f-12"> &nbsp;&nbsp;on <%#DataBinder.Eval(Container.DataItem,"TaggedBike.ModelEntity.ModelName") %>  &nbsp;[<%# Convert.ToDateTime (DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewDate")).ToString("dd-MMM-yyyy")%>]</span>                   
                        </div>
		            </div>
                </a>              
	        </itemtemplate>
	    </asp:repeater>
    </div>
    <div>
        <h2 class="margin-top30 margin-bottom20">Most Helpful</h2>
    </div>
    <div id="mostHelpful" class="box new-line5" style="padding: 0px 5px;">
        <asp:repeater id="rptMostHelpful" runat="server">
                <itemtemplate>
                  <a href='/m/<%#DataBinder.Eval(Container.DataItem,"TaggedBike.MakeEntity.MaskingName")%>-bikes/<%#DataBinder.Eval(Container.DataItem,"TaggedBike.ModelEntity.MaskingName") %>/user-reviews/<%#DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewId") %>.html' style="text-decoration:none;" class="normal">    
		                <div class="container">
                            <div class="sub-heading">
				                <b><%#DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewTitle ") %></b> &nbsp;&nbsp;<span class="arr-small">&raquo;</span>
			                </div>    
                            <div class="darkgray new-line5 f-12">
                                By <%#DataBinder.Eval(Container.DataItem,"ReviewEntity.WrittenBy") %>
                            </div>
                            <div class="darkgray new-line5" style="font-size:0px;">
                               <%# Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble( DataBinder.Eval(Container.DataItem,"ReviewRating.OverAllRating"))) %> 
                                <span class="f-12"> &nbsp;&nbsp;on <%#DataBinder.Eval(Container.DataItem,"TaggedBike.ModelEntity.ModelName") %> &nbsp; [<%# Convert.ToDateTime (DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewDate")).ToString("dd-MMM-yyyy")%>]</span>                   
                            </div>
		                </div>
                    </a>             
                </itemtemplate>
            </asp:repeater>
    </div>
    <div>
        <h2 class="margin-top30 margin-bottom20">Most Recent</h2>
    </div>
    <div id="mostRecent" class="box new-line5" style="padding: 0px 5px;">
        <asp:repeater id="rptMostRecent" runat="server">
                <itemtemplate>
                    <a href='/m/<%#DataBinder.Eval(Container.DataItem,"TaggedBike.MakeEntity.MaskingName")%>-bikes/<%#DataBinder.Eval(Container.DataItem,"TaggedBike.ModelEntity.MaskingName") %>/user-reviews/<%#DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewId") %>.html' style="text-decoration:none;" class="normal">    
		                <div class="container">
                            <div class="sub-heading">
				                <b><%#DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewTitle ") %></b> &nbsp;&nbsp;<span class="arr-small">&raquo;</span>
			                </div>    
                            <div class="darkgray new-line5 f-12">
                                By <%#DataBinder.Eval(Container.DataItem,"ReviewEntity.WrittenBy") %>
                            </div>
                            <div class="darkgray new-line5" style="font-size:0px;">
                               <%# Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble( DataBinder.Eval(Container.DataItem,"ReviewRating.OverAllRating"))) %> 
                                <span class="f-12"> &nbsp;&nbsp;on <%#DataBinder.Eval(Container.DataItem,"TaggedBike.ModelEntity.ModelName") %> &nbsp; [<%# Convert.ToDateTime (DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewDate")).ToString("dd-MMM-yyyy")%>]</span>                   
                            </div>
		                </div>
                    </a>           
                </itemtemplate>
            </asp:repeater>
    </div>
    <div>
        <h2 class="margin-top30 margin-bottom20">Most Rated</h2>
    </div>
    <div id="mostRated" class="box new-line5" style="padding: 0px 5px;">
        <asp:repeater id="rptMostRated" runat="server">
                <itemtemplate>
                     <a href='/m/<%#DataBinder.Eval(Container.DataItem,"TaggedBike.MakeEntity.MaskingName")%>-bikes/<%#DataBinder.Eval(Container.DataItem,"TaggedBike.ModelEntity.MaskingName") %>/user-reviews/<%#DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewId") %>.html' style="text-decoration:none;" class="normal">    
		                <div class="container">
                            <div class="sub-heading">
				                <b><%#DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewTitle ") %></b> &nbsp;&nbsp;<span class="arr-small">&raquo;</span>
			                </div>    
                            <div class="darkgray new-line5 f-12">
                                By <%#DataBinder.Eval(Container.DataItem,"ReviewEntity.WrittenBy") %>
                            </div>
                            <div class="darkgray new-line5" style="font-size:0px;">
                               <%# Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble( DataBinder.Eval(Container.DataItem,"ReviewRating.OverAllRating"))) %> 
                                <span class="f-12"> &nbsp;&nbsp;on <%#DataBinder.Eval(Container.DataItem,"TaggedBike.ModelEntity.ModelName") %>  [<%# Convert.ToDateTime (DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewDate")).ToString("dd-MMM-yyyy")%>]</span>                   
                            </div>
		                </div>
                    </a>           
                </itemtemplate>
            </asp:repeater>
    </div>
</div>
<div data-role="popup" id="popupDialog" data-overlay-theme="a" data-theme="c" data-dismissible="false" class="ui-corner-all">
    <div data-role="header" data-theme="a" class="ui-corner-top" style="background-color: #000">
        <h1 style="color: #fff;">Error !!</h1>
    </div>
    <div data-role="content" data-theme="d" class="ui-corner-bottom ui-content" style="background-color: #ffffff;">
        <span id="spnError" style="font-size: 14px; line-height: 20px;" class="error"></span>
        <a href="#" data-role="button" data-rel="back" data-theme="c" data-mini="true">OK</a>
    </div>
</div>
<script type="text/javascript" src="<%= staticUrl %>/UI/m/src/user-review/landing.js?<%= staticFileVersion %>"></script>
<script type="text/javascript">
    var returnUrl = '<%=TripleDES.EncryptTripleDES(string.Format("returnUrl=/user-reviews/&sourceid={0}",(int)Bikewale.Entities.UserReviews.UserReviewPageSourceEnum.Mobile_UserReview_Landing))%>';
</script>
<!-- #include file="/UI/includes/footermobile.aspx" -->
