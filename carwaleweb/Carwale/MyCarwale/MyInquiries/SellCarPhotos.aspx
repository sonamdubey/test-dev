<%@ Page Language="C#" Inherits="Carwale.UI.MyCarwale.MyInquiries.SellCarPhotos" Trace="false" Debug="false" AutoEventWireUp="false" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Import Namespace="Carwale.Utility" %>
<!doctype html>
<html>
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 72;
	Title 			= "Manage Car Photos";
	Description 	= "";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
    AdId            = "1337162297840";
    AdPath          = "/7590/CarWale_MyCarWale/CarWale_MyCarWale_Misc/CarWale_MyCarWale_Misc_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<style type="text/css">	
	.img-preview{margin:10px 5px 0 0; background-color:#f7f7f7; border:1px solid #EBEBEB; padding:5px;}
	.icons-sell{background:url(https://img.carwale.com/sell/iconsheet.gif) no-repeat; display:inline-block;}	
	.delete-photo{background-position:-60px -9px; width:20px; height:20px;}
    .hide{display:none;}
	.show{display:block;}
</style>
<script  type="text/javascript"  src="/static/src/bt.js" ></script>
<script  type="text/javascript"  src="/static/src/process.js" ></script>
<script  type= "text/javascript"   src="/static/src/sellcar.js" ></script>
<script  type= "text/javascript"   src="/static/src/ajaxfunctionsrq.js" ></script>
<script type="text/javascript">
	var inquiryId = '<%= inquiryId %>';	
	var requestCount = 0;
	var responseCount = 0;
	nextStepUrl = "/mycarwale/myinquiries/confirmmessage.aspx?t=p";
</script>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <input type="hidden" id="hdnIsPageFromCache" runat="server" />
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
                        <ul class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/MyCarwale/default.aspx">My CarWale</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="default.aspx">My Inquiry Details</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="MySellInquiry.aspx">Car Sell Inquiries</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Manage Car Photos</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black special-skin-text">Manage Car Photos</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-12">
                      <div class="content-box-shadow content-inner-block-10">
                           <div class="blue-block moz-round clear-margin margin-top10 margin-bottom10">
		                    <a title="Add photos to your listing" href="addcarphotos.aspx?car=<%= profileId %>" class="btn btn-orange mid-box"><span class="price">+</span> Add <%= rptImageList.Items.Count > 0 ? "More" : "" %> Photos</a>
	                    </div>
                      </div>
                 </div>
                 <div class="clear"></div>
                 <div class="grid-12 margin-top20">
                      <div class="content-box-shadow content-inner-block-10">	                   	
	                    <div class="gray-block2 moz-round<%= rptImageList.Items.Count > 0 ? "" : " hide" %>">
		                    <h2 class="hd2"><div class="hd2 leftfloat" id="divImageCount"><%= rptImageList.Items.Count %></div>&nbsp; Photos available with this listing</h2>
		                    <div class="mid-box">				
			                    <asp:Repeater ID="rptImageList" runat="server">
				                    <itemtemplate>
					                    <div id="<%# DataBinder.Eval(Container.DataItem,"Id")%>" class="img-preview">
						                    <table width="100%" border="0">
							                    <tr>
								                    <td width="100">
                                                        <div id='dtlstPhotos_<%# DataBinder.Eval(Container.DataItem,"Id")%>' class ='<%# DataBinder.Eval(Container.DataItem, "StatusId").ToString()=="1" ? "hide" : "show" %>' >
                                                            <img class='img-border' src="<%# ImageSizes.CreateImageUrl(DataBinder.Eval(Container.DataItem,"HostUrl").ToString(),ImageSizes._110X61, DataBinder.Eval(Container.DataItem,"OriginalImgPath").ToString())%>" />
                                                        </div>
                                                        <div id='dtlstPhotosPending_<%# DataBinder.Eval(Container.DataItem,"Id")%>'  
                                                            class='pending <%# DataBinder.Eval(Container.DataItem, "StatusId").ToString()=="1"? "show" : "hide" %>' 
                                                            pending="<%# DataBinder.Eval(Container.DataItem, "StatusId").ToString()=="1"? "true" : "false" %>">
                                                            <p style="color:#555555;font-weight:bold;">
                                                            Processing...
                                                            <img  align="center" src='https://imgd.aeplcdn.com/0x0/statics/loader.gif'/>
                                                            </p>
                                                        </div>
								                    </td>
								                    <td width="200"><input type="radio" id="rdo<%# DataBinder.Eval(Container.DataItem,"Id")%>" name="mianimg" <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsMain")) == true ? "checked=\"checked\"" : ""%> onclick="javascript:makeMainImg('<%# DataBinder.Eval(Container.DataItem,"Id")%>    ')" />Make Profile Image[<a title="This will be the main photo displayed in search results." class="front-img">?</a>]</td>
								                    <td><a id="remove<%# DataBinder.Eval(Container.DataItem,"Id")%>" onclick="javascript:deleteImg('<%# DataBinder.Eval(Container.DataItem,"Id")%>')" class="icons-sell delete-photo"></a></td>								
							                    </tr>
						                    </table>
					                    </div>
				                    </itemtemplate>
			                    </asp:Repeater>
		                    </div>
		                    <div id="done" class="mid-box margin-top20" align="right"><a class="btn btn-orange" onclick="javascript:mDone();">I'm Done</a></div>
	                    </div>
	                    <div class="mid-box"><br /></div>
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
            $(".front-img").bt({fill: '#FCF5A9',strokeWidth: 1,strokeStyle: '#D3D3D3',spikeLength:20,shadow: true,positions:['right']});
            var imgCategory = '<%= imgCategory%>';
        </script>
</form>
</body>
</html>
