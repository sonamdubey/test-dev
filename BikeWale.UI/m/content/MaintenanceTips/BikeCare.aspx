<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.m.content.MaintenanceTips.BikeCare" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="Pager" TagName="Pager" Src="/m/controls/ListPagerControl.ascx" %>
 <h1>Bike Care</h1>
<h2>BikeWale brings you maintenance tips from experts to rescue you from common problems</h2>
	<div id="divListing">
		<asp:Repeater id="rptRoadTest" runat="server">
			<itemtemplate>
				<a class="normal" href='<%# string.Format("/m{0}", Bikewale.Utility.UrlFormatter.GetArticleUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"BasicId")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ArticleUrl")),(Bikewale.Entities.CMS.EnumCMSContentType.RoadTest).ToString())) %>' >
					<div class="box1 new-line15" >
						<%# Regex.Match(Convert.ToString(DataBinder.Eval(Container.DataItem,"AuthorName")), @"\b(sponsored)\b",RegexOptions.IgnoreCase).Success ? "<div class=\"sponsored-tag-wrapper position-rel\"><span>Sponsored</span><span class=\"sponsored-left-tag\"></span></div>" : "" %>
						<div class="article-wrapper">
							<div class="article-image-wrapper">
                                <img alt='Expert Review: <%# DataBinder.Eval(Container.DataItem, "Title") %>' title="Expert reviews: <%# DataBinder.Eval(Container.DataItem, "Title") %>" src='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImgUrl").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>' width="100%" border="0">
							</div>
							<div class="padding-left10 article-desc-wrapper">
								<h2 class="font14 text-bold text-black">
                                    Expert Review: <%# DataBinder.Eval(Container.DataItem, "Title") %>
								</h2>
							</div>
						</div>
						<div class="article-stats-wrapper font12 leftfloat text-light-grey">
							<span class="bwmsprite calender-grey-icon inline-block"></span><span class="inline-block"><%# Bikewale.Utility.FormatDate.GetFormatDate(DataBinder.Eval(Container.DataItem,"DisplayDate").ToString(),"MMMM dd, yyyy") %></span>
						</div>
						<div class="article-stats-wrapper font12 leftfloat text-light-grey">
							<span class="bwmsprite author-grey-icon inline-block"></span><span class="inline-block"><%# DataBinder.Eval(Container.DataItem, "AuthorName") %></span>
						</div>
						<div class="clear"></div>
					</div>
				</a>
			</itemtemplate>
		</asp:Repeater>                
	</div>  
	<Pager:Pager ID="listPager" runat="server" />  
</div>