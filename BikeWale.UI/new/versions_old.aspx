<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.Versions_old" Trace="false" Debug="false" Async="true" %>

<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="tips" TagName="TipsAdvicesMin" Src="~/controls/TipsAdvicesMin.ascx" %>
<%@ Register TagPrefix="news" TagName="NewsMin" Src="~/controls/NewsMin.ascx" %>
<%@ Register TagPrefix="uc" TagName="UserReviewsMin" Src="~/controls/UserReviewsMin.ascx" %>
<%@ Register TagPrefix="BikeWale" TagName="FeaturedBikes" Src="~/controls/FeaturedBike.ascx" %>
<%@ Register TagPrefix="RT" TagName="RoadTest" Src="/controls/RoadTestControl.ascx" %>
<%@ Register TagPrefix="BikeWale" TagName="BikeRatings" Src="~/controls/BikeRatings.ascx" %>
<%@ Register TagPrefix="BV" TagName="BikeVideos" Src="~/controls/BikeVideos.ascx" %>
<%@ Register TagPrefix="SB" TagName="SimilarBike" Src="~/controls/SimilarBikes.ascx" %>
<%@ Register TagPrefix="BikeBooking" TagName="BikeBookingMin" Src="~/controls/BikeBookingMinWidget.ascx"  %>
<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>

<%
    title = makeName + " " + modelName + " Price in India, Review, Mileage & Photos - Bikewale";
    description = makeName + " " + modelName + " Price in India - Rs." + formattedPrice + ". Check out " + makeName + " " + modelName + " on road price, reviews, mileage, versions, news & photos at Bikewale.";
    ShowTargeting = "1";
    TargetedModel = modelOnly;
    canonical = "http://www.bikewale.com/" + MakeMaskingName + "-bikes/" + ModelMaskingName + "/";
    fbTitle = title;
    fbImage = fbLogoUrl;
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
    alternate="http://www.bikewale.com/m/"+ MakeMaskingName + "-bikes/" + ModelMaskingName + "/";
%>
<!-- #include file="/includes/headNew.aspx" -->
<script src="/src/graybox.js"></script>
<style>
    .tab-spec {
        margin-top: 10px;
    }

        .tab-spec ul {
            list-style: none;
            width: 620px;
            overflow: hidden;
        }

        .tab-spec li {
            float: left;
            padding: 5px 14px;
            border-left: 1px solid #999;
            background-color: #e5e4e4;
        }

            .tab-spec li a {
                font-weight: bold;
                font-size: 14px;
                text-decoration: none;
                *position: relative;
                *height: 25px;
                *display: inline-block;
                *top: -10px;
                cursor: pointer;
            }

    .divUsedBikes a {
        text-decoration: none;
        font-size: 14px;
    }

        .divUsedBikes a:hover {
            text-decoration: underline;
        }

    #emiLoan {
        font-weight: normal;
        text-decoration: underline;
        cursor: pointer;
    }
     #emiLoanNotRated {
        font-weight: normal;
        text-decoration: underline;
        cursor: pointer;
    }
    
    .inner-content {
        border: 1px solid #eaeaea;
        padding: 10px;
        margin-bottom: 20px;
    }

    .blue {
        color: #0056cc;
        text-decoration: none;
        cursor: pointer;
    }
</style>




<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb" itemscope itemtype="http://data-vocabulary.org/Breadcrumb">
            <li>You are here: </li>
            <li><a itemprop='url' href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href='/<%=MakeMaskingName %>-bikes/'><span itemprop='title'><%= make%>  </span></a></li>
            <li class="fwd-arrow">&rsaquo;</li>
         <%--   <%if (modelCount > 1) {%>
            <li><a itemprop='url' href="/<%=MakeMaskingName %>-bikes/<%=seriesMaskingName%>-series/"><span itemprop="title"><%= seriesName %></span></a></li>
            <%} %>
            <li class='<%= modelCount <= 1 ? "hide" : "fwd-arrow"  %>'>&rsaquo;</li>--%>
            <li class="current"><span itemprop="title"><strong><%=modelOnly%></strong></span></li>
        </ul>
        <div class="clear"></div>
    </div>
    <div class="grid_8 margin-top15">
        <%if (mmv.IsFuturistic != true)
          {
              if (reviewCount > 0){%>
        <div itemtype="http://data-vocabulary.org/Review-aggregate" itemscope>
            <h1 itemprop="itemreviewed" id="heading"><%=model%>
                <%-- <span id="discontinued" runat="server" style="font-size:16px; color:red;">(Discontinued)</span>
                <span id="upcoming" runat="server" style="font-size:16px; color:#ff6a00;">(Upcoming)</span>--%>
                <span id="reviewedModelStatus" runat="server" style="font-size: 16px; color: #ff6a00;"></span>
            </h1>
            <div class="grid_3 alpha margin-top15">
                <% if (isPhotoAvailable) { %>
                <a href="/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName %>/photos/" title="<%= MakeMaskingName + " " + ModelMaskingName%>">
                    <% } %>
                    <%--<img itemprop="photo" src="<%=MakeModelVersion.GetModelImage(hostURL,imageUrl) %>" class="padding-5 border-light" title="<%=model%>" alt="<%=model%>" />--%>
                    <img itemprop="photo" src="<%=MakeModelVersion.GetModelImage(hostURL,imageUrl,Bikewale.Utility.ImageSize._227x128) %>" class="padding-5 border-light" title="<%=model%>" alt="<%=model%>" />
                    <% if (isPhotoAvailable) { %>
                </a>
                <% } %>
            </div>
          
            <div class="grid_5 omega margin-top15" id="div_BikeRatings" runat="server">

                <% if (mmv.IsNew == false && mmv.IsUsed == true)
                   { %>
                <div class="padding5 text-highlight">Last Recorded Price Rs. <%=estimatedPrice %><span class="margin-left5">(Ex-Showroom)</span></div>
                <% } %>
                <% else
                   { %><div class="padding5 text-highlight">
                    <h2>Starts At Rs. <%=estimatedPrice %><span class="margin-left5">(Ex-Showroom Price)</span></h2>
                    &nbsp;&nbsp;<a id="emiLoan" class="reviewLink <%=String.IsNullOrEmpty(mmv.MinPrice)?"hide":"" %>">Get EMI Assistance </a></div>
                <% } %>
                <div class="action-btn padding5 <%= String.IsNullOrEmpty(mmv.MinPrice) || (mmv.IsNew==false && mmv.IsUsed==true) ? "hide" : "" %>">
                    <a href="/pricequote/default.aspx?model=<%=modelId %>" pageCatId="3" class="fillPopupData" modelId="<%=modelId%>">Check On Road Price</a>
                </div>
                <BikeWale:BikeRatings runat="server" ID="ctrl_BikeRatings" />

            </div>
            <div class="clear"></div>
            <div class="grid_5 omega">
                <ul class="social">
                    <li>
                        <fb:like href="http://www.bikewale.com/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/" send="false" layout="button_count" width="80" show_faces="false"></fb:like>
                    </li>
                    <li><a href="https://twitter.com/share" class="twitter-share-button" data-url="http://www.bikewale.com/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/" data-via='<%= title %>' data-lang="en">Tweet</a></li>
                    <li>
                        <div class="g-plusone" data-size="medium" data-href="http://www.bikewale.com/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/"></div>
                    </li>
                </ul>
            </div>
            <div class='divUsedBikes <%= string.IsNullOrEmpty(usedBikeCount) ? "hide" : "grid_3 alpha margin-top20" %>'><a href="/used/bikes-in-india/#make=<%=makeId %>"><%=usedBikeCount %> used <%=makeName %> Bikes for sale</a></div>
            <div class="clear"></div>           
        
        <%}else{%>              
         <div>
            <h1><%=model%>
                <span id="modelStatus" runat="server" style="font-size: 16px; color: #ff6a00;"></span>
            </h1>
            <div class="grid_3 alpha margin-top15">
                <% if (isPhotoAvailable) { %>
                <a href="/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName %>/photos/" title="<%= MakeMaskingName + " " + ModelMaskingName%>">
                    <% } %>
                    <%--<img  src="<%=MakeModelVersion.GetModelImage(hostURL,imageUrl) %>" class="padding-5 border-light" title="<%=model%>" alt="<%=model%>" />--%>
                    <img  src="<%=MakeModelVersion.GetModelImage(hostURL,imageUrl,Bikewale.Utility.ImageSize._227x128) %>" class="padding-5 border-light" title="<%=model%>" alt="<%=model%>" />
                    <% if (isPhotoAvailable) { %>
                </a>
                <% } %>
            </div>
          
            <div class="grid_5 omega margin-top15" id="div_BikeRatingsNotAvail" runat="server">
                <% if (mmv.IsNew == false && mmv.IsUsed == true)
                   { %>
                <div class="padding5 text-highlight">Last Recorded Price Rs. <%=estimatedPrice %><span class="margin-left5">(Ex-Showroom)</span></div>
                <% } %>
                <% else
                   { %><div class="padding5 text-highlight">
                    <h2>Starts At Rs. <%=estimatedPrice %><span class="margin-left5">(Ex-Showroom Price)</span></h2>
                    &nbsp;&nbsp;<a id="emiLoanNotRated" class="reviewLink <%=String.IsNullOrEmpty(mmv.MinPrice)?"hide":"" %>">Get EMI Assistance </a></div>
                <% } %>
                <div class="action-btn padding5 <%= String.IsNullOrEmpty(mmv.MinPrice) || (mmv.IsNew==false && mmv.IsUsed==true) ? "hide" : "" %>">
                    <a href="/pricequote/default.aspx?model=<%=modelId %>" class="fillPopupData" pageCatId="3" modelId="<%=modelId %>">Check On Road Price</a>
                </div>
                <div>
                    <span class='reviewText'>No review available for this model.</span><br><a rel='nofollow' class='reviewLink' href='<%= reviewLink %>'>Be the first one to write  a review</a>
                </div>
            </div>
            <div class="clear"></div>
            <div class="grid_5 omega">
                <ul class="social">
                    <li>
                        <fb:like href="http://www.bikewale.com/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/" send="false" layout="button_count" width="80" show_faces="false"></fb:like>
                    </li>
                    <li><a href="https://twitter.com/share" class="twitter-share-button" data-url="http://www.bikewale.com/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/" data-via='<%= title %>' data-lang="en">Tweet</a></li>
                    <li>
                        <div class="g-plusone" data-size="medium" data-href="http://www.bikewale.com/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/"></div>
                    </li>
                </ul>
            </div>             
            <div class='divUsedBikes <%= string.IsNullOrEmpty(usedBikeCount) ? "hide" : "grid_3 alpha margin-top20" %>'><a href="/used/bikes-in-india/#make=<%=makeId %>"><%=usedBikeCount %> used <%=makeName %> Bikes for sale</a></div>
            <div class="clear"></div>
             <% } %>
             <% if (mmv.IsNew && mmv.IsUsed) { %>
            <div>
                <BikeBooking:BikeBookingMin ID="ctrlBikeBooking" runat="server" />
            </div>
             <% } %>
            <div class="clear"></div>
        </div>
            <%
                }else{ %>
          <div>
            <h1><%=model%><span style="font-size: 16px; color: #ff6a00;"> (Upcoming)</span></h1>
            <div class="grid_3 alpha margin-top15">
                <% if (isPhotoAvailable) { %>
                <a href="/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName %>/photos/" title="<%= MakeMaskingName + " " + ModelMaskingName%>">
                    <% } %>
                    <%--<img itemprop="photo" src="<%=MakeModelVersion.GetModelImage(hostURL,imageUrl) %>" class="padding-5 border-light" title="<%=model%>" alt="<%=model%>" />--%>
                    <img itemprop="photo" src="<%=MakeModelVersion.GetModelImage(hostURL,imageUrl,Bikewale.Utility.ImageSize._227x128) %>" class="padding-5 border-light" title="<%=model%>" alt="<%=model%>" />
                    <% if (isPhotoAvailable) { %>
                </a>
                <% } %>
            </div>
           <div id="divFuturistic" runat="server" class="grid_5 omega margin-top15">
                <div><span>Expected Launch: </span><b><%= expectedLaunch %></b></div>
                <div class="margin-top5"><span>Expected Price: </span><b>Rs. <%= estimatedPrice %></b></div>
                <div class="margin-top5"><%=model%> is not launched in India yet. Information on this page is tentative.</div>
                <% if (isPhotoAvailable)
                   { %>
                <div class="margin-top10"><a href="/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/photos/">View <%=model %> photos</a></div>
                <% } %>
            </div>

            <div class="clear"></div>
            <div class="grid_5 omega">
                <ul class="social">
                    <li>
                        <fb:like href="http://www.bikewale.com/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/" send="false" layout="button_count" width="80" show_faces="false"></fb:like>
                    </li>
                    <li><a href="https://twitter.com/share" class="twitter-share-button" data-url="http://www.bikewale.com/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/" data-via='<%= title %>' data-lang="en">Tweet</a></li>
                    <li>
                        <div class="g-plusone" data-size="medium" data-href="http://www.bikewale.com/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/"></div>
                    </li>
                </ul>
            </div>
            <div class='divUsedBikes <%= string.IsNullOrEmpty(usedBikeCount) ? "hide" : "grid_3 alpha margin-top20" %>'><a href="/used/bikes-in-india/#make=<%=makeId %>"><%=usedBikeCount %> used <%=makeName %> Bikes for sale</a></div>
            <div class="clear"></div>
        </div>
        <%} %>
            <div id="div_description" runat="server" class="margin-top20">               
                <div id="smallDescription" class="margin-top15"><%=smallDescription %></div>
                <span class="readmore pointer <%=String.IsNullOrEmpty(smallDescription) ? "hide" : "show" %>"><a id="showFullRev">..Read Full Review</a></span>
                <div id="largeDescription" class="hide margin-top15"><%=largeDescription %></div>
                <span class="readmore pointer <%=String.IsNullOrEmpty(largeDescription) ? "hide" : "show" %>"><a id="hideFullRev" class="hide">..Hide Full Review</a></span>
            </div>
            <div class="clear"></div>

        <div class="clear"></div>        
        <div class="grid_8 alpha tab-spec" id="specTabs">
            <%if (mmv.IsFuturistic != true)
              {%>
            <ul id="tabMenu">
                <%if (!String.IsNullOrEmpty(versionId))
                  {%>
                <li style="border-left: 0px;"><a id="price" href="#versions-and-price">Price</a></li>
                <%} %>
                <%if (versionCount > 0) %>
                <li><a id="specs" href="#specification-and-features">Specs & Features</a></li>
                <%if (isPhotoAvailable)
                  { %>
                <li><a href="/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName %>/photos/" target="_blank">Photos</a></li>
                <%} %>
                <%if (ucUserReviewsMin.RecordCount > 0)
                  {%>
                <li><a href="#reviews">Reviews</a></li>
                <%} %>
                <%if (ucRoadTestMin.RecordCount > 0)
                  { %>
                <li><a id="roadtest" href="#roadtests">Road Test</a></li>
                <%} %>
                <%if (ucBikeVideos.RecordCount > 0)
                  { %>
                <li><a id="video" href="#videos">Videos</a></li>
                <%} %>
                <%if (newsMin.RecordCount > 0)
                  { %>
                <li><a id="new" href="#news">News</a></li>
                <%} %>
            </ul>
            <%} %>
        </div>
        <div class="clear"></div>
        <div id="versions-and-price" class='<%=String.IsNullOrEmpty(versionId) ? "hide" : "" %>'>
            <div id="divVersions" runat="server" class="margin-top10">
                <table id="tblVersions" class="tbl-std" cellpadding="0" cellspacing="0" border="0" style="margin: 5px 0;">
                    <tr>
                        <th>Versions</th>
                        <th width="210"><%=mmv.IsNew==false && mmv.IsUsed==true ? "Last Recorded Price" : "Ex-Showroom Price"%> (<%= Bikewale.Common.Configuration.GetDefaultCityName %>
                        )
                    <th>&nbsp;</th>
                    </tr>
                    <tbody>
                        <asp:repeater id="rptVersions" runat="server">
	                    <itemtemplate>
		                    <tr id='<%# DataBinder.Eval( Container.DataItem, "ID" ) %>'>
			                    <td>
                                    <span>
                                         <strong> <%# DataBinder.Eval(Container.DataItem,"Version") %></strong>
                                    </span>
				                    <p class="margin-top5">
					                    <%# GetBikeSpecsMin(DataBinder.Eval(Container.DataItem,"Displacement").ToString(), DataBinder.Eval(Container.DataItem,"BikeFuelType").ToString(), DataBinder.Eval(Container.DataItem,"BikeTransmission").ToString(), DataBinder.Eval(Container.DataItem,"FuelEfficiencyOverall").ToString() )%>
				                    </p>						
			                    </td>			
			                    <td><span class="text-highlight"><%# GetMinPrice(DataBinder.Eval(Container.DataItem,"VersionPrice").ToString()) %></span><p class="<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"VersionPrice").ToString())||(mmv.IsNew==false && mmv.IsUsed==true) ? "hide" : "" %>"><a class="margin-top5 fillPopupData" pageCatId="3" modelId="<%=modelId %>" href="/pricequote/default.aspx?version=<%# DataBinder.Eval( Container.DataItem, "ID" ) %>"  >Check On-Road Price</a></p></td>
		                    </tr>		
		                 </itemtemplate>
                    </asp:repeater>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="clear"></div>

        <div id="specification-and-features" class='<%= versionCount == 0 ? "hide" : "" %>'>
            <h2 class='grid_8 alpha margin-top10 <%=isFuturistic == true ? "hide" : "" %>'><%=makeName + " "+ modelName + " - " %><span class='<%= versionCount == 1 ? "hide" : "" %>'><select class='pointer' id='ddlVersion' style="border: 0px; font-weight: bolder; width: auto; color: #000;" runat='server' /></span><span class='<%= Count == 1? "" : "hide" %>'><%= version %></span> Specifications</h2>
            <div id="divSpecs" runat="server">
                <div id="loader"></div>
                <div id="versionDetails"></div>
            </div>
        </div>

        <div class="clear"></div>
        <div id="videos">
            <div class="margin-top20" id="divVideos">
                <BV:BikeVideos ID="ucBikeVideos" runat="server"></BV:BikeVideos>
            </div>
        </div>
        <%if (!mmv.IsFuturistic) {%>
        <div id="roadtests">
            <div class="margin-top20" id="divRoadTest">
                <RT:RoadTest ID="ucRoadTestMin" runat="server" TopRecords="4" ControlWidth="grid_2" HeaderText="Road Tests"></RT:RoadTest>
            </div>
        </div>
        <%} %>
        <div class="clear"></div>
        <div id="reviews">
            <div class="margin-top10" id="divReviews">
                <uc:UserReviewsMin ID="ucUserReviewsMin" runat="server" TopRecords="5"></uc:UserReviewsMin>
            </div>
        </div>
    </div>
    <!--    Left Container ends here -->
    <div class="grid_4">
        <div class="margin-top15 margin-bottom20">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>
        <% if (!mmv.IsFuturistic)
           {%>
        <div id="sililarBikes">
            <SB:SimilarBike ID="ctrl_similarBikes" runat="server" />
        </div>
        <%} %>
        <div id="news">
            <div id="divNews">
                <news:NewsMin ID="newsMin" runat="server"></news:NewsMin>
            </div>
        </div>
        <div class="margin-top15">
            <tips:TipsAdvicesMin ID="tipsAdvices" runat="server" />
        </div>
        <div class="margin-top15">
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
        </div>
    </div>
    <!--    Right Container ends here -->
</div>
<div id="back-to-top" class="back-to-top"><a><span></span></a></div>

<script type="text/javascript">
    $(document).ready(function () {
        var speed = 300;
        //var versionId = $(this).val();
        //input parameter : id of element, scroll up speed 
        ScrollToTop("back-to-top", speed);

        // $("#defaultVersion").addClass("hide");
        if ('<%= isFuturistic %>' == 'False' && '<%= versionIdSpecs%>' != "" && '<%= versionIdSpecs%>' != null && !isNaN('<%= versionIdSpecs%>')) {
            $("#divSpecs").load("/new/versionspecs.aspx?version=" + '<%= versionIdSpecs%>' + "&count=" + '<%= Count%>');
        }
        else {
            $("#divSpecs").text("Specification for this version is not available.");
        }

        $(".chkCompare").click(function () {
            if ($(this).is(":checked") && $("input.chkCompare:checked").length <= 4) {
            } else if ($(this).is(":checked") && $("input.chkCompare:checked").length > 4) {
                alert("You can select upto 4 bikes for comparison");
                $(this).attr("checked", false);
            }
        });

        $("#showFullRev").click(function () {
            $("#smallDescription").addClass("hide");
            $(this).addClass("hide");
            $("#largeDescription").removeClass("hide").fadeIn();
            $("#hideFullRev").removeClass("hide");
        });

        $("#hideFullRev").click(function () {
            $("#smallDescription").removeClass("hide").fadeIn();
            $("#showFullRev").removeClass("hide");
            $("#largeDescription").addClass("hide");
            $(this).addClass("hide");
        });

        $("#ddlVersion").change(function () {
            var versionId = $(this).val();
            //alert(versionId);
            var versionName = $(this).find(":selected").html();
            $("#versionName").text(versionName);
            $("#versionDetails").html("<img src='http://img.aeplcdn.com/loader.gif' border='0'/>Loading Results...");
            setTimeout(2000);
            if (versionId != "" && !isNaN(versionId) && versionId != null) {
                $("#divSpecs").load("/new/versionspecs.aspx?version=" + versionId + "&count=" + '<%= Count%>');
            }
            else
                $("#divSpecs").text("Specification for this version is not available.");
        });

        $("#emiLoan").click(function () {
            var comment = "";
            var caption = "Free Bike Loan Assistance";
            var modelId = '<%=modelId %>';
            var url = "/new/emiloan.aspx?modelid=" + modelId;
            var applyIframe = true;
            var GB_Html = "";
            GB_show(caption, url, 280, 540, applyIframe, GB_Html);
        });

        $("#emiLoanNotRated").click(function () {
            var comment = "";
            var caption = "Free Bike Loan Assistance";
            var modelId = '<%=modelId %>';
            var url = "/new/emiloan.aspx?modelid=" + modelId;
            var applyIframe = true;
            var GB_Html = "";
            GB_show(caption, url, 280, 540, applyIframe, GB_Html);
        });
    });
</script>
<!--    Left Container ends here -->
    <PW:PopupWidget runat="server" ID="PopupWidget" />
<!-- #include file="/includes/footerInner.aspx" -->

      
       
