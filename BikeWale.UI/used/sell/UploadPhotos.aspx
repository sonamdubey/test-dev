<%@ Page Language="C#" Inherits="Bikewale.Used.UploadPhotos" AutoEventWireUp="false" Trace="false" Debug="false"%>
<%@ Import Namespace="Bikewale.Common" %>
<%
    title = "Upload actual bike images - BikeWale";
    description = "Upload your bike's image";
     isAd970x90BottomShown = false;
    is300x250Shown = false;
                     is300x250BTFShown = false;
%>
<!-- #include file="/includes/headUsed.aspx" -->
<script language="javascript" src="/src/flash_upload/swfobject.js?v=1.0" type="text/javascript"></script>
<script language="javascript" src="/src/flash_upload/jquery.uploadify.v2.1.0.js?v=1.0" type="text/javascript"></script>
<div class="grid_8">	
	<div class="sell_block moz-round">
		<h2 class="hd2-red">Actual Bike Photos</h2>
		<p>BikeWale stats show that the listings with images are likely to fetch more number of genuine leads. So, go ahead and upload your bike images.</p>
		<div class="mid-box">
			<div id="utility">				
				<div>In case currently you don't have bike images, you can skip for now and come back later to upload.</div>		
				<div class="mid-box">
					<div class="lt-upload"><input id="fileInput2" name="fileInput2" type="file"/></div>
					<div class="rt-upload text-grey2">We accept jpeg, png, gif formats only. Image size up to <strong>4MB</strong> is permissible.</div>
				</div>
				<div class="clear"></div>								
			</div>			
			<div id="basic-uploader" class="text-grey2" style="margin-top:30px;">Problem uploading images? Use <a href="UploadBasic.aspx">Basic File Uploader</a></div>
			<div id="skip" class="mid-box" align="right"><a title="Skip this step for now" href="Confirmation.aspx">Skip this step for now</a></div>					
		</div>
	</div>	
</div>
<script language="javascript">	
	var imgUrl = '<%= ImagingFunctions.GetImagePath("/bikewaleimg/used/ucp/") %>';
	var inquiryId = '<%= inquiryId %>';
	var requestCount = 0;
	var responseCount = 0;
	
	$(document).ready(function(){
		$("#fileInput2").uploadify({
			'uploader'      : '/src/flash_upload/uploadify.swf',
			'script'        : '/used/sell/UploadHandler.ashx',
			'cancelImg'     : '/src/flash_upload/trash.gif',
			'height'		: '34',
			'width'			: '145',
			'buttonImg'		: '/src/flash_upload/upload-photos.gif',
			'folder'        : '/ucp',
			'fileExt'       : '*.jpg;*.gif,*.png',
            'fileDesc'      : 'Image Files',
			'multi'			: true,
			'method'		:'post',
			'sizeLimit'		: '4194304',
			'scriptData' 	: {'size':'640x428|300x225|80x60','inquiryId':inquiryId,'isDealer':'0'},
			onAllComplete:function(a,b){							
				if(b.filesUploaded != 0){
					window.location.href = "/used/sell/uploadpreview.aspx";
				}
			},
			onClearQueue:function(){
				$("#done").show();
			},
			onSelectOnce:function(){
				$("#upload-frame, #upload-ctrl").show();
				$("#uploadTrigger").click(function(){
					$('#fileInput2').uploadifyUpload();
				});
				$("#clearQueue").click(function(){
					$('#fileInput2').uploadifyClearQueue();
				});			
			},
			onError: function (a, b, c, d) {  				
				if (d.type ==="File Size")  
					alert(c.name +": Image size up to 4MB permissible");  
				else  
					alert("error "+d.type+": "+d.text);  
			}
		});
	});		
</script>
<!-- #include file="/includes/footer.aspx" -->