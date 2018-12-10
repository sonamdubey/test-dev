<%@ Page Language="C#" trace="false" Inherits="Carwale.UI.Used.Dealers.DealerShowroom" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 81;
	Title 			= "Used Car Dealers Showroom";
	Description 	= "Dealer Showroom at CarWale";
	Keywords		= "mumbai dealers, car dealers india, used cars";
	Revisit 		= "15";
	DocumentState 	= "Static";
    canonical       = "https://www.carwale.com/dealer/dealershowroom.aspx";
    AdId            = "1396440544094";
    AdPath          = "/1017752/UsedCar_";
    //Ad300           = true;
%>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
<!-- #include file="/includes/global/head-script.aspx" -->
<%--<link href="<%=Carwale.Utility.CWConfiguration._staticHostUrl %>/css/cw-misk.css?20160419032055" rel="stylesheet" />--%>
<style>
	.dlCityList {width:100%; padding-right:20px;}
    .dlCityList tr {border-bottom:1px solid #f2f2f2;}
    .dlCityList tr:last-child {border:none;}
    .dlCityList td {padding:10px 0px;}
    #dealerList h1 {border:none;margin:0!important;}
    .slot-cont li {list-style-type: none;background: transparent url(https://imgd.aeplcdn.com/0x0/cw-common/ul-arrow.gif) no-repeat 2px;border: 0;margin: 0;padding: 2px 0 5px 9px;}
</style>
<script type='text/javascript'>
    googletag.cmd.push(function () {
        googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
        googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());

        //googletag.pubads().enableSyncRendering();
        googletag.pubads().setTargeting('<%=targetKey%>', '<%=targetValue%>');
        googletag.pubads().collapseEmptyDivs();
        googletag.pubads().enableSingleRequest();
        googletag.enableServices();
    });
</script>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <!-- #include file="/includes/header.aspx" -->
    <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
    <div class="clear"></div>
  <section  class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
    <section>
        <div class="container">
            <div class="grid-12">
                <div id="youHere" class="margin-top10 margin-bottom10">
                    <a class=" margin-right10" href="/">Home</a><span class="fa fa-angle-right margin-right10"></span>
                    <a class=" margin-right10" href="/used/">Used Cars</a><span class="fa fa-angle-right margin-right10"></span>
                    <strong>Car Dealer Showroom</strong>
                </div>
            <div id="dealerList">
                <h1 class="font30 text-black special-skin-text">Used Car Dealers in India</h1>
                <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
            </div>
        </div>
        </div>
    </section>
    <div class="clear"></div>
    <section class="padding-bottom20">
        <div class="container">
            <div class="grid-8">
                <div class="left_grid">
                    <div id="content">
		                <div class="content-box-shadow content-inner-block-10 rounded-corner2" style="overflow:auto;">
                            <div>
                                <asp:DataList ID="dlCityList" class="dlCityList" runat="server" RepeatColumns="2" RepeatLayout="Table" RepeatDirection="Vertical" HeaderStyle-VerticalAlign="Top">  
                                    <ItemTemplate>   
                                          <a href="/used/dealers-in-<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem, "City").ToString()) %>/" style="color:#3a8ae0;"><%# DataBinder.Eval(Container.DataItem, "City")%> Used Car Dealers</a>
                                    </ItemTemplate>   
                                </asp:DataList>
                           </div>         
                        </div>
                    </div>
                </div>
            </div>
            <div class="grid-4">
                <div class="right-grid">
                    <div class="margin-bottom10">
                        <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 20, 20, false, 0) %>
                    </div>
                    <!--Popular Cars in India Start-->
                    <div class="content-box-shadow content-inner-block-5 rounded-corner2">
                        <h3 class="margin-bottom5">Popular Cars in India</h3>
                        <div>
                            <div class="slot-cont">
                                <ul>
                                    <li><a title='Hyundai i10 in India' href='/used/hyundai-i10-cars/'>Hyundai i10</a></li>
                                    <li><a title='Hyundai Santro Xing in India' href='/used/hyundai-santroxing-cars/'>Hyundai Santro Xing</a></li>
                                    <li><a title='Maruti Suzuki Alto [2000-2012] in India' href='/used/marutisuzuki-alto20002012-cars/'>Maruti Suzuki Alto [2000-2012]</a></li>
                                    <li><a title='Maruti Suzuki Swift in India' href='/used/marutisuzuki-swift-cars/'>Maruti Suzuki Swift</a></li>
                                    <li><a title='Toyota Innova in India' href='/used/toyota-innova-cars/'>Toyota Innova</a></li>
                                    <li><a title='Hyundai Verna in India' href='/used/hyundai-verna-cars/'>Hyundai Verna</a></li>
                                    <li><a title='Maruti Suzuki 800 in India' href='/used/marutisuzuki-800-cars/'>Maruti Suzuki 800</a></li>
                                    <li><a title='Hyundai i20 in India' href='/used/hyundai-i20-cars/'>Hyundai i20</a></li>
                                    <li><a title='Maruti Suzuki Swift [2005-2011] in India' href='/used/marutisuzuki-swift20052011-cars/'>Maruti Suzuki Swift [2005-2011]</a></li>
                                    <li><a title='Maruti Suzuki Wagon R [1999-2010] in India' href='/used/marutisuzuki-wagonr19992010-cars/'>Maruti Suzuki Wagon R [1999-2010]</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <!--Popular Cars in India End-->
                </div> 
            </div>
            <div class="clear"></div>
        </div>
    </section>
  </section>
    <div class="clear"></div>
    
    <!-- #include file="/includes/footer.aspx" -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
</body>
</html>