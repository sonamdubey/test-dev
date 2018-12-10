<%@ Page Language="C#" Inherits="Carwale.UI.MyCarwale.MyInquiries.AddCarPhotos" Trace="false" Debug="false" AutoEventWireUp="false" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<!doctype html>
<html>
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 72;
	Title 			= "Add Car Photos";
	Description 	= "";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
	AdId            = "1337162297840";
	AdPath          = "/7590/CarWale_MyCarWale/CarWale_MyCarWale_Misc/CarWale_MyCarWale_Misc_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<style>
	/* --Uploadify -- */
	.uploadifyQueueItem {width:380px; font-size:11px;}
	.uploadifyError {border: 2px solid #FBCBBC !important;background-color: #FDE5DD !important;}	
	.uploadifyProgress {background-color:#F7F7F7;width: 100%; height:15px; opacity: .70;filter:Alpha(Opacity=70);top:-13px; margin:0;}
	.uploadifyProgressBar {width:1px; height:15px; border-right:2px solid #225E9A; background-color:#D2D2D2; margin:0; text-align:center;}
	.uploadifyQueue{height:200px; overflow:auto; width:400px; border:2px solid #EBEBEB; padding:5px;}
	.uplodifyHeader{width:414px; font-size:11px; background-color:#EBEBEB;}		
	#fileQueue .uploadifyQueueItem {font-size:11px; border: none;border-bottom: 1px solid #E5E5E5;background-color: #FFFFFF;padding: 5%;width: 90%;}
	#fileQueue .uploadifyError {background-color: #FDE5DD !important;}
	#fileQueue .uploadifyQueueItem .cancel {float: right;}
	#upload-frame{display:none; margin-top:20px;}
	#utility .lt-upload{width:115px; float:left;}
	#utility .rt-upload{}
	.img-items{font-size:11px;}	
	#upload-ctrl{margin:5px 0; width:413px;}
	.text-grey2{color:#999999 !important;}
	.mid-box{margin-top:20px!important;}
</style>
<script  language="javascript"  src="/static/src/swfobject.js"  type="text/javascript"></script>
<script  language="javascript"  src="/static/src/jquery.uploadify.v2.1.0.js"  type="text/javascript"></script>
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
							<li><span class="fa fa-angle-right margin-right10"></span>Upload Car Photos</li>
						</ul>
						<div class="clear"></div>
					</div>
					<h1 class="font30 text-black special-skin-text">Upload Car Photos</h1>
					<div class="border-solid-bottom margin-top10 margin-bottom15"></div>
				</div>
				<div class="clear"></div>
				<div class="grid-12">
					<div class="content-box-shadow content-inner-block-10">
						<div class="">	
							<div class="gray-block moz-round clear-margin">		
								<p>CarWale research shows that listings with photographs are 32% more likely to get a response from buyers. So, upload photos of your car and reach more buyers.</p>
								<div class="mid-box">
									<div id="utility">				
										<div>If you do not have any photos currently, you can skip for now and upload them later.</div>				
										<div class="mid-box">
											<div class="lt-upload"><input id="fileInput2" name="fileInput2" type="file"/></div>
											<div class="rt-upload">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;We accept images in jpeg, png, gif formats of size up to <strong>4MB</strong>. Capture images in <strong>landscape</strong> mode for better Ad quality.</div>
										</div>
										<div class="clear"></div>								
									</div>			
									<div id="basic-uploader" class="text-grey2" style="margin-top:30px;">Facing problem in uploading photos? Use <a href="basicuploader.aspx?car=<%=profileId%>">Basic File Uploader</a></div>								
								</div>
							</div>
							<div class="mid-box"></div>
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
		<script language="javascript">
			var inquiryId = '<%= inquiryId %>';

			$(document).ready(function () {
				$("#fileInput2").uploadify({
					'uploader': '/static/src/uploadify.swf?x=2',
					'script': '/used/sellcar/UploadHandler.ashx',
					'cancelImg': '/static/src/trash.gif?x=2',
					'height': '35',
					'width': '145',
					'buttonImg': '/static/src/upload-photos.gif?x=2',
					'folder': '/ucp',
					'fileDesc': '',
					'fileExt': '*.jpg;*.gif;*.png;*.jpeg;',
					'fileDesc': 'Image Files',
					'multi': true,
					'sizeLimit': '4194303',
					'scriptData': { 'size': '640x428|300x225|80x60', 'inquiryId': inquiryId, 'isDealer': '0' },
					onAllComplete: function () {
						window.location.href = "/mycarwale/myinquiries/sellcarphotos.aspx?car=" + '<%= profileId %>';
			},
			onClearQueue: function () {
				$("#utility").hide();
				$("#done").show();
			},
			onSelectOnce: function () {
				$("#upload-frame, #upload-ctrl").show();
				$("#uploadTrigger").click(function () {
					$('#fileInput2').uploadifyUpload();
				});
				$("#clearQueue").click(function () {
					$('#fileInput2').uploadifyClearQueue();
				});
			},
			onError: function (a, b, c, d) {
				if (d.type === "File Size")
					alert(c.name + ": Image size up to 4MB permissible");
				else
					alert("error " + d.type + ": " + d.text);
			}
		});
	});
</script>
</form>
</body>
</html>
