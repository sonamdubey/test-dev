<%@ Page Language="C#" Inherits="Carwale.UI.MyCarwale.MyInquiries.BasicUploader" Trace="false" Debug="false" AutoEventWireUp="false" %>
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
	Title 			= "Basic Uploader";
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
    .mid-box{margin-top:20px!important;}
    .redirect-lt {float:left;}
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
                            <div id="divErrorMsg" class="alert"></div> 
                            <div id="divAlertMsg" class="alert" runat="server" visible="false"></div> 
	                        <div class="blue-block">		
		                        <div class="mid-box">
			                        <div id="utility">
				                        <div class="mid-box">
					                        <p class="margin-bottom25">CarWale research shows that listings with photographs are 32% more likely to get a response from buyers. So, upload photos of your car and reach more buyers.</p>				
                                            <label for="imageInput" class="btn btn-orange">Upload</label>
					                        <input id="imageInput" name="fileInput2" type="file" class="hide"/>
                                            <img  id="uploadLoading" src='https://imgd.aeplcdn.com/0x0/statics/loader.gif' class="hide"/>
					                        <p class="margin-top10">We accept images in jpeg and png formats of size up to <strong>8MB</strong>. Capture images in <strong>landscape</strong> mode for better Ad quality.</p>
				                        </div>				
				                        <div class="clear"></div>
			                        </div>
		                        </div>
	                        </div>
	                        <div class="gray-block2<%= rptImageList.Items.Count > 0 ? "" : " hide" %>">
		                        <h2 class="hd2 margin-top10"><div class="hd2 redirect-lt" id="divImageCount"><%= rptImageList.Items.Count %></div>&nbsp;Photos available with this listing</h2>
		                        <asp:Repeater ID="rptImageList" runat="server">
			                        <itemtemplate>														
				                        <div id="<%# DataBinder.Eval(Container.DataItem,"Id")%>" class="img-preview">
					                        <table width="100%" border="0">
						                        <tr>
							                        <td width="100">
                                                        <div id='dtlstPhotos_<%# DataBinder.Eval(Container.DataItem,"Id")%>' class ='<%# DataBinder.Eval(Container.DataItem, "StatusId").ToString()=="1" ? "hide" : "show" %>' >
                                                            <img class='img-border' src="<%# ImageSizes.CreateImageUrl(DataBinder.Eval(Container.DataItem,"HostUrl").ToString(),ImageSizes._110X61,DataBinder.Eval(Container.DataItem,"OriginalImgPath").ToString())%>" />
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
		                        <div id="done" class="mid-box" align="right"><a class="btn btn-orange" onclick="javascript:mDone();">I'm Done</a></div>
	                        </div>			
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
        <script type="text/javascript" src="/static/js/imageUpload.js"></script>
    <script type="text/javascript">
        $(".front-img").bt({fill: '#FCF5A9',strokeWidth: 1,strokeStyle: '#D3D3D3',spikeLength:20,shadow: true,positions:['right']});
        var errorDivObj = $('#divErrorMsg');
        var uploadLoading = $('#uploadLoading');
        var sizeLimitBytes = 8 * 1024 * 1024;//8MB limit
        errorDivObj.text("");
        uploadLoading.hide();
        $(document).ready(function(){
            $('#imageInput').on('change', beginUpload);
        });

        function beginUpload(){
            var imageFiles = document.getElementById('imageInput').files;
            if(imageFiles.length === 0){
                errorDivObj.text('No files currently selected for upload');
            }
            else{
                for (var i = 0; i < imageFiles.length; i++) {
                    if(isFileUploadValid(imageFiles[i])){
                        var imgUploadUtility = new ImageUploadUtility();
                        imgUploadUtility.imageType = "7";
                        uploadLoading.show();
                        imgUploadUtility.upload(imageFiles[i])
                            .then(function (resp){
                                window.location.reload(true);
                            }).catch(function(er){
                                uploadLoading.hide();
                                errorDivObj.text("Something went wrong. Please try again");
                            });
                    }
                }

            }
        }

        function isFileUploadValid(file){
            if (!validFileType(file)) {
                errorDivObj.text("You are trying to upload invalid file. We accept only jpg and png file formats.");
                return false;
            }
            else if(!validFileSize(file)){
                errorDivObj.text("The file you are trying to upload is too large. Please try uploading a file of smaller size.");
                return false;
            }
            else{
                return true;
            }
        }

        function validFileType(file) {
            var fileTypes = [
              'image/jpeg',
              'image/png'
            ];
            for(var i = 0; i < fileTypes.length; i++) {
                if(file.type === fileTypes[i]) {
                    return true;
                }
            }
            return false;
        }

        function validFileSize(file, maxSizeBytes) {
            return file.size <= (maxSizeBytes || sizeLimitBytes)
        }

</script>
        
</form>
</body>
</html>
