<%@ Page Language="C#" Inherits="Bikewale.Videos.Default" AutoEventWireup="false" EnableViewState="false" Trace="false" %>  
<%@ Import Namespace="Bikewale.Utility.StringExtention" %>
<%@ Register TagPrefix="BikeWale" TagName="video" Src="/UI/controls/VideoCarousel.ascx" %>  
<%@ Register Src="~/UI/controls/VideoByCategory.ascx" TagName="ByCategory" TagPrefix="BW" %>
<%@ Register Src="~/UI/controls/ExpertReviewsVideos.ascx" TagName="ExpertReview" TagPrefix="BW" %>
<%@ Register TagPrefix="BW" TagName="PopularBikesByBodyStyle" Src="~/UI/controls/PopularBikesByBodyStyle.ascx" %>


<!DOCTYPE html>
<html>
<head>
	<%  
		title = "Bike Videos, Expert Video Reviews with Road Test & Bike Comparison - BikeWale";
		description ="Check latest bike and scooter videos, watch BikeWale expert's take on latest bikes and scooters - features, performance, price, fuel economy, handling and more.";
		keywords = "bike videos, video reviews, expert video reviews, road test videos, bike comparison videos";
		canonical = "https://www.bikewale.com/bike-videos/";
		alternate =  "https://www.bikewale.com/m/bike-videos/";

		isAd970x90Shown = false;
		isAd300x250Shown = false;
		isAd300x250BTFShown = false;
		isAd970x90BottomShown = false;
	%>
	<!-- #include file="/UI/includes/headscript.aspx" -->
	<style type="text/css">
		#videoJumbotron .grid-8{padding:20px 0 20px 20px}#videoJumbotron .grid-4{padding:20px 10px}.main-video-container{width:100%;height:350px;display:block;position:relative;background:url(https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif) center center no-repeat;text-align:center;overflow:hidden}.sidebar-video-image,.sidebar-video-title{display:inline-block;vertical-align:middle}.main-video-container img{width:100%;position:relative;top:-59px}.main-video-container span{position:absolute;left:0;bottom:0;text-align:left;font-size:22px;width:624px;color:#fff;padding:20px;background:linear-gradient(to bottom,rgba(0,0,0,0),rgba(5,0,0,.7))}.main-video-container span:hover{text-decoration:underline}#videoJumbotron ul{border-left:1px solid #e2e2e2;padding-left:14px}#videoJumbotron li{width:280px;height:auto;margin-top:20px;padding-top:20px;border-top:1px solid #e2e2e2}#videoJumbotron li:first-child{margin-top:0;padding-top:0;border-top:none}.sidebar-video-image{width:100px;height:55px;overflow:hidden;margin-right:15px;background:url(https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif) center center no-repeat;text-align:center}.sidebar-video-title{width:160px;height:auto}.sidebar-video-title:hover{color:#4d5057}.sidebar-video-image img{width:100%;position:relative;top:-10px}.powerdrift-banner{background:url(https://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/d/d-powerdrift-banner.jpg) center no-repeat;height:129px}.powerdrift-banner h3{line-height:1.7}.margin-top35{margin-top:35px}.powerdrift-subscribe{padding:15px 15px 10px;margin-top:25px;margin-right:25px;background:#fff}.firstride-jcarousel li{height:312px;border:1px solid #e2e2e2;padding:20px}.videocarousel-image-wrapper{width:271px;height:153px;margin-bottom:15px;overflow:hidden;text-align:center}.reviews-image-wrapper a,.videocarousel-image-wrapper a{width:100%;height:100%;display:block;overflow:hidden;background:url(https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif) center center no-repeat}.reviews-image-wrapper img,.videocarousel-image-wrapper img{width:100%}.reviews-image-wrapper img{position:relative;top:-43px}.border-light-right{border-right:1px solid #e2e2e2}.more-videos-link{width:200px;display:block;margin:5px auto 25px}.reviews-image-wrapper{width:458px;height:258px;margin-bottom:15px;border:1px solid #e2e2e2;overflow:hidden;text-align:center}.firstride-jcarousel .jcarousel-control-next{background-position:-66px -81px}.firstride-jcarousel .jcarousel-control-prev{background-position:-34px -81px}.firstride-jcarousel .jcarousel-control-next:hover{background-position:-66px -136px}.firstride-jcarousel .jcarousel-control-prev:hover{background-position:-34px -136px}.firstride-jcarousel .jcarousel-control-next.inactive{background-position:-66px -25px}.firstride-jcarousel .jcarousel-control-prev.inactive{background-position:-34px -25px}.brand-type-container li{display:inline-block;vertical-align:top;width:180px;height:85px;margin:0 5px 30px;text-align:center;font-size:18px;-moz-border-radius:2px;-webkit-border-radius:2px;-o-border-radius:2px;-ms-border-radius:2px;border-radius:2px}.brand-1,.brand-10,.brand-11,.brand-12,.brand-13,.brand-14,.brand-15,.brand-16,.brand-17,.brand-18,.brand-19,.brand-2,.brand-20,.brand-22,.brand-23,.brand-24,.brand-3,.brand-34,.brand-37,.brand-38,.brand-39,.brand-4,.brand-40,.brand-41,.brand-42,.brand-5,.brand-6,.brand-7,.brand-71,.brand-8,.brand-81,.brand-9,.brand-23,.brand-73,.brand-type,.brand-74,.brand-75,.brand-76,.brand-77,.brand-79{height:50px}.brand-type{width:180px;display:block;margin:0 auto}.brand-type-title{margin-top:10px;display:block}.brand-type-container a{text-decoration:none;color:#82888b;display:inline-block}.brand-type-container li:hover span.brand-type-title{color:#4d5057;font-weight:700}.brand-bottom-border{overflow:hidden}.brandlogosprite{background:url(https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/brand-type-sprite.png?07Jun20181528360754292) no-repeat;display:inline-block}.brand-2{width:87px;background-position:0 0}.brand-7{width:56px;background-position:-96px 0}.brand-1{width:88px;background-position:-162px 0}.brand-8{width:100px;background-position:-260px 0}.brand-12{width:67px;background-position:-370px 0}.brand-40{width:125px;background-position:-447px 0}.brand-34{width:122px;background-position:-582px 0}.brand-22{width:121px;background-position:-714px 0}.brand-3{width:44px;background-position:-845px 0}.brand-17{width:86px;background-position:-899px 0}.brand-15{width:118px;background-position:-995px 0}.brand-4{width:43px;background-position:-1123px 0}.brand-9{width:99px;background-position:-1176px 0}.brand-16{width:117px;background-position:-1285px 0}.brand-5{width:59px;background-position:-1412px 0}.brand-19{width:122px;background-position:-1481px 0}.brand-13{width:122px;background-position:-1613px 0}.brand-6{width:63px;background-position:-1745px 0}.brand-10{width:102px;background-position:-1818px 0}.brand-14{width:127px;background-position:-1930px 0}.brand-39{width:89px;background-position:-2067px 0}.brand-20{width:82px;background-position:-2166px 0}.brand-11{width:121px;background-position:-2258px 0}.brand-41{width:63px;background-position:-2389px 0}.brand-42{width:64px;background-position:-2461px 0}.brand-81{width:71px;background-position:-2535px 0}.brand-71{width:39px;background-position:-2616px 0}.brand-73{width: 60px;background-position:-3217px 0;}.brand-23{width:125px;background-position:-3026px 0;}.brand-74{width: 110px;background-position:-3281px 0;}.brand-75{width: 122px;background-position:-3400px 0;}.brand-76{width:124px;background-position:-3530px 0;}.brand-77{width:122px;background-position:-3663px 0;}.brand-79{width:120px;background-position:-3795px 0;}@media only screen and (max-width:1024px){.brand-type,.brand-type-container li{width:170px}}
</style>
  
</head>
<body class="bg-light-grey header-fixed-inner page-type-landing">
	<form id="form1" runat="server">
		<!-- #include file="/UI/includes/headBW.aspx" -->
		<section>
			<div class="container">
				<div class="grid-12">
					<div class="breadcrumb margin-top15 margin-bottom10">
						<ul>
							<li><a href="/"><span>Home</span></a></li>
							<li><span class="bwsprite fa-angle-right margin-right10"></span>Videos</li>
						</ul>
					</div>
					<h1 class="font26 margin-bottom5">Videos</h1>
				</div>
				<div class="clear"></div>
			</div>
		</section>


		<section>
			<div id="videoJumbotron" class="container">
				<div class="grid-12">
					<div class="content-box-shadow">
						<div class="grid-8">
							<a href="<%= Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(ctrlVideosLandingFirst.VideoTitleUrl,ctrlVideosLandingFirst.BasicId.ToString()) %>" class="main-video-container">
								<img class="lazy" data-original="<%= String.Format("https://img.youtube.com/vi/{0}/sddefault.jpg",ctrlVideosLandingFirst.VideoId)  %>" alt="<%= ctrlVideosLandingFirst.VideoTitle  %>" title="<%= ctrlVideosLandingFirst.VideoTitle  %>" src="<%= String.Format("https://img.youtube.com/vi/{0}/sddefault.jpg",ctrlVideosLandingFirst.VideoId)  %>" border="0" />
								<span><%= ctrlVideosLandingFirst.VideoTitle  %></span>
							</a>
						</div>
						<div class="grid-4">
							<ul>
								<asp:Repeater ID="rptLandingVideos" runat="server">
									<ItemTemplate>

										<li>
											<a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" class="sidebar-video-image">
												<img class="lazy" data-original="<%# String.Format("https://img.youtube.com/vi/{0}/default.jpg",DataBinder.Eval(Container.DataItem,"VideoId"))  %>" alt="<%# DataBinder.Eval(Container.DataItem,"VideoTitle") %>" title="<%# DataBinder.Eval(Container.DataItem,"VideoTitle") %>" src="<%# String.Format("https://img.youtube.com/vi/{0}/default.jpg",DataBinder.Eval(Container.DataItem,"VideoId"))  %>" border="0" /></a>
											<a href="<%# Bikewale.Utility.UrlFormatter.VideoDetailPageUrl(DataBinder.Eval(Container.DataItem,"VideoTitleUrl").ToString(),DataBinder.Eval(Container.DataItem,"BasicId").ToString()) %>" title="<%# DataBinder.Eval(Container.DataItem,"VideoTitle") %>" class="sidebar-video-title font14 text-light-grey"><%# DataBinder.Eval(Container.DataItem,"VideoTitle").ToString().Truncate(35) %></a>
										</li>

									</ItemTemplate>
								</asp:Repeater>
							</ul>
						</div>
						<div class="clear"></div>
					</div>
				</div>
				<div class="clear"></div>
			</div>
		</section>

		<section>
			<div class="container margin-top20 powerdrift-banner">
				<div class="grid-12">
					<div class="leftfloat margin-left25 margin-top35">
						<h3 class="text-white">Reviews, Specials, Underground, Launch Alerts &<br />
							a whole lot more...</h3>
					</div>
					<div class="rightfloat powerdrift-subscribe">
						<script src="https://apis.google.com/js/platform.js"></script>
						<div class="g-ytsubscribe" data-channel="powerdriftofficial" data-layout="full" data-count="hidden"></div>
					</div>
				</div>
				<div class="clear"></div>
			</div>
		</section>

		<% if (ctrlExpertReview.FetchedRecordsCount > 0)
		   {%>
		<BW:ExpertReview runat="server" ID="ctrlExpertReview" />
		<% } %>


		<% if (ctrlFirstRide.FetchedRecordsCount > 0)
		   {%>
		<BW:ByCategory runat="server" ID="ctrlFirstRide" />
		<% } %>   

		<% if (ctrlLaunchAlert.FetchedRecordsCount > 0) {%>
		<BW:ByCategory runat="server" ID="ctrlLaunchAlert" />
		<% } %> 

		<% if (ctrlFirstLook.FetchedRecordsCount > 0)
		   {%>
		<BW:ByCategory runat="server" ID="ctrlFirstLook" />
		<% } %>
		   <% if (objVideo != null && objVideo.TopMakeList!=null)
              { %>
		<section>
			<div class="container section-bottom-margin">
				<div class="grid-12">
                     <h2 class="text-bold text-center margin-top40 margin-bottom20 font28">Browse videos by brands</h2>
					<div class="content-box-shadow padding-top20 collapsible-brand-content">
						<div id="brand-type-container" class="brand-type-container">
							<ul class="text-center">
							  <% foreach (var make in objVideo.TopMakeList)
								 { %>
										<li>
											<a href="/<%=make.MaskingName %>-bikes/videos/" title="<%=make.MakeName %> bikes videos">
												<span class="brand-type">
													<span class="lazy brandlogosprite brand-<%=make.MakeId%>"></span>
												</span>
												<span class="brand-type-title"><%=make.MakeName %></span>
											</a>
										</li>
								  <%} %>
							</ul>
							<% if (objVideo.OtherMakeList!=null&&objVideo.OtherMakeList.Count() >0)
                               { %>
                            <ul class="brand-style-moreBtn padding-top25 brandTypeMore hide margin-left5">
							 <% foreach (var make in objVideo.OtherMakeList)
                                { %>
									   <li>
											<a href="/<%=make.MaskingName %>-bikes/videos/" title="<%=make.MakeName %> bikes videos">
												<span class="brand-type">
													<span class="lazy brandlogosprite brand-<%=make.MakeId%>"></span>
												</span>
												<span class="brand-type-title"><%=make.MakeName %></span>
											</a>
										</li>
							   <%} %>
							</ul>
						</div>
                       
						<div class="view-all-btn-container padding-bottom25">
							<a href="javascript:void(0)" class="view-brandType btn view-all-target-btn rotate-arrow" rel="nofollow"><span class="btn-label">View more brands</span><span class="bwsprite teal-right"></span></a>
						</div>
						<%} %>
					</div>
				</div>
				<div class="clear"></div>
			</div>
		</section>
		<%} %>

		<% if (ctrlPDBlockbuster.FetchedRecordsCount > 0)
		   {%>
		<BW:ByCategory runat="server" ID="ctrlPDBlockbuster" />
		<% } %>


		<% if (ctrlMotorSports.FetchedRecordsCount > 0)
		   {%>
		<BW:ByCategory runat="server" ID="ctrlMotorSports" />
		<% } %>          


		<% if (ctrlPDSpecials.FetchedRecordsCount > 0)
		   {%>
		<BW:ByCategory runat="server" ID="ctrlPDSpecials" />
		<% } %>
		

		<% if (ctrlTopMusic.FetchedRecordsCount > 0)
		   {%>
		<BW:ByCategory runat="server" ID="ctrlTopMusic" />
		<% } %>

		 <% if (ctrlMiscellaneous.FetchedRecordsCount > 0) {%>
		<BW:ByCategory runat="server" ID="ctrlMiscellaneous" />
		<% } %> 
        
		<div class="margin-bottom25"></div>

		<script type="text/javascript">
			$(document).ready(function () { $("img.lazy").lazyload(); });
		</script>
		<!-- #include file="/UI/includes/footerBW.aspx" -->
		<!-- #include file="/UI/includes/footerscript.aspx" -->
	</form>
</body>
</html>
