<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.DefaultUR" Trace="false" EnableEventValidation="false" %>

<%@ Register TagPrefix="ur" TagName="UserReviews" Src="/Controls/BikeReviews.ascx" %>
<%     
    title = "User Reviews on Bikes in India";
    description = "Know what users are saying about the bike you aspire to buy. Read first hand user feedback on bikes in India. Write your own review or write comments on others' reviews to let people know about your experience.";
    keywords = "bike user reviews, bike users reviews, customer bike reviews, customer bike feedback, bike reviews, bike owner feedback, owner bike reviews, owner report, owner comments";
    canonical = "https://www.bikewale.com/user-reviews/";
    alternate = "https://www.bikewale.com/m/user-reviews/";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";

    //modified by SajalGupta for unfilled impression of ads on 04 Aug 2016.
    isAd300x250Shown = false;
    isAd300x250_BTFShown = false;
%>
<!-- #include file="/includes/headnew.aspx" -->
<style type="text/css">
    .ul-2col li {
        float: left;
        width: 200px;
    }
</style>
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/" itemprop="url">
                    <span itemprop="title">Home</span></a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/new-bikes-in-india/" itemprop="url"><span itemprop="title">New</span></a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>User Reviews</strong></li>
        </ul>
        <div class="clear"></div>
    </div>
    <div class="grid_12">
        <h1 class="margin-top10">User Reviews</h1>
    </div>
    <div class="grid_5 margin-top10">
        <div class="grey-bg content-block">
            <h2>Browse By Make</h2>
            <div class="margin-top15">
                <asp:repeater id="rptMakes" runat="server">
				    <itemtemplate>
					    <h3><%# DataBinder.Eval(Container.DataItem, "MakeName")%></h3>
                        <asp:DataList ID="dtlstPhotos" runat="server"
						    RepeatColumns="2"
						    DataSource='<%# GetDataSource( DataBinder.Eval(Container.DataItem, "MakeId").ToString() ).Tables[0] %>'
						    RepeatDirection="Horizontal" CssClass="tbl-std"
						    RepeatLayout="Table">
						    <itemstyle Width="50%" HorizontalAlign="left"></itemstyle>
						    <itemtemplate>
							    <a href="/<%# DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString() %>-bikes/<%#  DataBinder.Eval(Container.DataItem, "ModelMaskingName").ToString() %>/reviews/">
								    <%# DataBinder.Eval(Container.DataItem, "ModelName")%> 
								    (<%# DataBinder.Eval(Container.DataItem, "TotalReviews")%>)
							    </a>
						    </itemtemplate>
					    </asp:DataList>                  
				    </itemtemplate>
			    </asp:repeater>
            </div>
        </div>
    </div>
    <div class="grid_7">
        <!-- div right container -->
        <div class="grey-bg content-block margin-top10">
            <h2>Write your own review</h2>
            <div class="margin-top10">
                Make<font color="red">*</font>
                <asp:dropdownlist id="drpMake" runat="server" tabindex="1" />
                &nbsp; Model<font color="red">*</font>
                <asp:dropdownlist id="drpModel" runat="server" tabindex="2">
				    <asp:ListItem Text="--Select--" Value="0" />
			    </asp:dropdownlist>
                <input type="hidden" id="hdn_drpModel" runat="server" />
                <asp:button id="btnWrite" cssclass="buttons text_white" runat="server" text="Next" tabindex="3" />
                <br>
                <span id="spnModel" class="error"></span>
            </div>
        </div>
        <div class="clear"></div>
        <div class="margin-top15">
            <h3>Most Reviewed Bikes</h3>
            <ul class="ul-2col std-ul-list">
                <asp:repeater id="rptMostReviewed" runat="server">
					<itemtemplate>
						<li>
                            <a href="/<%# DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString() %>-bikes/<%# DataBinder.Eval(Container.DataItem, "ModelMaskingName").ToString() %>/reviews/">
								<%# DataBinder.Eval(Container.DataItem, "ModelName")%> 
							</a> (<%# DataBinder.Eval(Container.DataItem, "TotalReviews")%> reviews)
						</li>
					</itemtemplate>
				</asp:repeater>
            </ul>
        </div>
        <div class="clear"></div>
        <br />
        <div class="padding-top20 featured-bike-tabs">
            <ul class="featured-bike-tabs-inner padding-top20">
                <li id="tab_mostread" class="fbike-active-tab">Most Read</li>
                <li id="tab_mosthelpful">Most Helpful</li>
                <li id="tab_mostrecent">Most Recent</li>
                <li id="tab_mostrated">Most Rated</li>
            </ul>
        </div>
        <div class="clear"></div>
        <div id="mostread" class="tab_inner_container">
            <ur:UserReviews ID="urUserReviews" ReviewCount="5" ShowComment="false" RetriveBy="MostRead" runat="server" />
        </div>
        <div id="mosthelpful" class="hide tab_inner_container">
            <ur:UserReviews ID="urUserReviewsMostHelpful" ReviewCount="5" ShowComment="false" RetriveBy="MostHelpful" runat="server" />
        </div>
        <div id="mostrecent" class="hide tab_inner_container">
            <ur:UserReviews ID="urUserReviewsMostRead" ReviewCount="5" ShowComment="false" RetriveBy="MostRecent" runat="server" />
        </div>
        <div id="mostrated" class="hide tab_inner_container">
            <ur:UserReviews ID="urUserReviewsMostRated" ReviewCount="5" ShowComment="false" RetriveBy="MostRated" runat="server" />
        </div>
    </div>

</div>
<script type="text/javascript">

    $("#drpMake").change(function () {
        var makeId = $("#drpMake").val();
        var reqType = 'USED';

        if (makeId > 0) {

            showLoading('drpModel');
            $.ajax({
                type: "POST", url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"requestType":"' + reqType + '" , "makeId":"' + makeId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                success: function (response) {
                    var jsonString = eval('(' + response + ')');
                    var resObj = eval('(' + jsonString.value + ')');
                    bindDropDownList(resObj, $("#drpModel"), 'hdn_drpModel', '', '--Select--');
                }
            });
        }
        else {
            $("#drpModel").val("0");
            $("#drpModel").prop("disabled", true)
        }
    });

    $("#btnWrite").click(function () {
        if ($("#drpModel").val() == "0") {
            $("#spnModel").html("Please select model to continue!");
            return false;
        }
        else
            $("#spnModel").html("");
    });

    $("#tab_mostread,#tab_mosthelpful,#tab_mostrecent,#tab_mostrated").click(function () {
        $("#tab_mostread,#tab_mosthelpful,#tab_mostrecent,#tab_mostrated").removeClass("fbike-active-tab");
        $(this).addClass("fbike-active-tab");
        $("#mostread,#mosthelpful,#mostrecent,#mostrated").addClass("hide");
        $("#" + $(this).attr('id').split('_')[1]).removeClass("hide");
    });

</script>
<!-- #include file="/includes/footerinner.aspx" -->
