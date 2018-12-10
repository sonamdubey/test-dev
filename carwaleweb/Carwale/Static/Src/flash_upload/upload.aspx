<%@ Page Language="C#" Inherits="Uploadify.Upload_" AutoEventWireup="false" Trace="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<style>
	/* --Uploadify -- */
	.uploadifyQueueItem {
		font: 11px Verdana, Geneva, sans-serif;
		border: 1px solid #E5E5E5;
		background-color: #E7F8FF;
		margin-top: 5px;
		padding: 10px;
		width: 350px;
	}
	.uploadifyError {
		border: 2px solid #FBCBBC !important;
		background-color: #FDE5DD !important;
	}
	.uploadifyQueueItem .cancel {
		float: right;
	}
	.uploadifyProgress {
		background-color:#fff;
		margin-top: 10px;
		width: 100%;
	}
	.uploadifyProgressBar {
		background-color:#336699;
		width: 1px;
		height: 3px;
	}
	#fileQueue .uploadifyQueueItem {
		font: 11px Verdana, Geneva, sans-serif;
		border: none;
		border-bottom: 1px solid #E5E5E5;
		background-color: #FFFFFF;
		padding: 5%;
		width: 90%;
	}
	#fileQueue .uploadifyError {
		background-color: #FDE5DD !important;
	}
	#fileQueue .uploadifyQueueItem .cancel {
		float: right;
	}
</style>
<script  language="javascript"  src="/static/src/flash_upload/swfobject.js"  type="text/javascript"></script>
<script  language="javascript"  src="/static/src/flash_upload/jquery.uploadify.v2.1.0.min.js"  type="text/javascript"></script>
<title>Uploadfy</title>
</head>
<body>	
	<input id="fileInput2" name="fileInput2" type="file" /> 
	<a href="javascript:$('#fileInput2').uploadifyUpload();">Upload Files</a> | <a href="javascript:$('#fileInput2').uploadifyClearQueue();">Clear Queue</a></div>
	<script language="javascript">// <![CDATA[
		$(document).ready(function(){
			$("#fileInput2").uploadify({
				'uploader'       : '/src/flash_upload/uploadify.swf',
				'script'         : '/src/flash_upload/upload.ashx',
				'cancelImg'      : '/src/flash_upload/cancel.gif',
				'height'		:	'34', //height of your browse button file
  				'width'			:	'145', //width of your browse button file
				'buttonImg'		: '/src/flash_upload/browse.gif',				
				'folder'         : '/src/flash_upload/_uploadImgs',
  				'fileDesc'  : '',
  				'fileExt'		: '*.jpg;*.gif,*.png',
				'multi'          : true,
				'scriptData' : {'size':'500x400|70x70|50x50'},				
				onComplete: function(event, queueID, fileObj, response, data){
                   alert(response);
                }

			});
		});// ]]>
	</script>
</body>
</html>
