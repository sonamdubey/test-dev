<%@ Page Language="C#" Inherits="Bikewale.Used.UploadBasic" Trace="false" Debug="false" AutoEventWireUp="false"  ValidateRequest="false" %>
<%@ Import Namespace="Bikewale.Common" %>
<% title = "Basic File Uploader"; %>
<%
    isAd300x250Shown = false;
    isAd300x250BTFShown = false;
        
 %>
<!-- #include file="/includes/headSell.aspx" -->
<script type="text/javascript" src="<%= staticUrl  %>/src/classified/sellbike.js?14sept2015"></script>
<script language="javascript">
	var inquiryId = '<%= inquiryId %>';
	var requestCount = 0;
	var responseCount = 0;
	nextStepUrl = "/used/sell/confirmation.aspx";
</script>
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/" itemprop="url">
                    <span itemprop="title">Home</span>
                </a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/used/" itemprop="url">
                    <span itemprop="title">Used Bikes</span>
                </a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>Upload Images</strong></li>
        </ul><div class="clear"></div>
    </div>
    <div class="grid_8 margin-top10">
        <h1>Upload Bike Images</h1>
	    <p class="desc-para">BikeWale stats show that listings with images are more likely to fetch more number of genuine buyers. So, go ahead and upload your bike images!</p>
        <div id="div_NotAuthorised" runat="server" class="min-height margin-top15"></div>
        <div id="div_Photos" runat="server">
            <p class="desc-para">We accept <strong>jpeg</strong>, <strong>png</strong>, <strong>gif</strong> formats only. Image size up to <strong>4MB</strong> permissible. By uploading images you agree to BikeWale photos <a href="#">Terms & Conditions</a></p>				
	        <div id="divAlertMsg" class="error padding5" runat="server"></div>                         
	        <input type="file" id="fileInput2" name="fileInput2" runat="server"/>
	        <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="buttons text_white"></asp:Button>				
	    
            <%--<p class="desc-para">No, I don't want to upload photos now. <a href="/used/bikedetails.aspx?bike=S<%= inquiryId %>">Take me to my listing</a></p>--%>
            <p class="desc-para">No, I don't want to upload images now. <a href="/mybikewale/mylisting.aspx">Take me to my listing</a></p>

            <div class="<%= objPhotos.ClassifiedImageCount == 0 ? " hide" : " show" %>">						
		        <div class="mid-box"><h2 class="hd2"><span id="spnPhotoCount"><%= objPhotos.ClassifiedImageCount %></span> Images available with this listing</h2></div>
           
		        <asp:Repeater ID="rptImageList" runat="server">
			        <itemtemplate>														
				        <div id='<%# DataBinder.Eval(Container.DataItem,"Id")%>' class="img-preview">
					        <table width="100%" border="0">
						        <tr>
                                    <td width="100">
                                        <div style="float:left;" id='dtlstPhotos_<%# DataBinder.Eval(Container.DataItem,"ID")%>' class='<%# DataBinder.Eval(Container.DataItem, "StatusId").ToString()=="1" ? "hide" : "show" %>'> 
                                            <img class='img-border' id="imgUpload" src="<%# (ImagingFunctions.GetPathToShowImages("/144X81/"+DataBinder.Eval( Container.DataItem, "OriginalImagePath").ToString(), DataBinder.Eval( Container.DataItem, "HostURL").ToString() )).Replace("http://http://","https://") %>" />
                                        </div>
                                        <div style="float:left;width:60px;" id ='dtlstPhotosPending_<%# DataBinder.Eval(Container.DataItem,"ID")%>' class='pending <%# DataBinder.Eval(Container.DataItem, "StatusId").ToString()=="1"? "show" : "hide" %>' pending="<%# DataBinder.Eval(Container.DataItem, "StatusId").ToString()=="1"? "true" : "false" %>">
                                            <p style="color:#555555;font-weight:bold;">
                                            <img align="center" src='https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/search-loading.gif'/>
                                            </p>
                                        </div>   
                                    </td>
							        <td width="250"><textarea id='desc<%# DataBinder.Eval(Container.DataItem,"Id")%>' onfocus="javascript:clearTextArea(this)" onblur="javascript:msgTextArea(this)" rows="3" cols="30" class="text-grey"><%#String.IsNullOrEmpty(DataBinder.Eval( Container.DataItem, "Description").ToString()) ? "Describe this image here" : DataBinder.Eval( Container.DataItem, "Description").ToString()%></textarea></td>
							        <td width="200"><input type="radio" id='rdo<%# DataBinder.Eval(Container.DataItem,"Id")%>' name="mianimg" <%# GetIsMainImageChecked(DataBinder.Eval(Container.DataItem,"IsMain").ToString()) %>  onclick="javascript:makeMainImg('<%# DataBinder.Eval(Container.DataItem,"Id")%>')" /><label for="rdo<%# DataBinder.Eval(Container.DataItem,"Id")%>" title="This will be the main photo displayed in search results." class="pointer front-img">Make Profile Image</label></td>
							        <td><a href="#" id='remove<%# DataBinder.Eval(Container.DataItem,"Id")%>' onclick="javascript:deleteImg('<%# DataBinder.Eval(Container.DataItem,"Id")%>')" class="pointer" title="This will remove photo from bike profile page">Remove Photo</a></td>
                                 </tr>
					        </table>
				        </div>						
			        </itemtemplate>
		        </asp:Repeater>
		        <div id="done" class="mid-box" align="right"><a class="buttons" onclick="javascript:mDone();">I'm Done</a></div>			
	        </div>
        </div>
    </div><!-- grid_8 -->
</div>
<script type="text/javascript">
    var pendingList = new Array();
    var refreshTime = 2000;
    var isValidImg = true;
    $(document).ready(function () {

        GetPendingList();
        setInterval(UpdatePendingImages, refreshTime);
    });

    $('#btnUpload').click(function (){
        if(isValidImg)
            return isValidImg;
        else 
        {
            alert("Image max size should be 4 MB");
            return false;
        }
    })
    $('#fileInput2').change(function(e){
        isValidImg = true;
        $("#divAlertMsg").text("");
        $("#divAlertMsg").visible = false;     
        var f = this.files[0];

        if( f.size > 4194304 || f.fileSize > 4194304  )
        {        
            $("#divAlertMsg").visible = true;
            $("#divAlertMsg").text("Maximum image size exceeded.");    
            isValidImg = false;
            return false;
        }       
        else 
            return true;
    })
    /*
    for binding images 
    */
    function CheckImageStatus(imageList) {
        $.ajax({
            type: "POST", url: "/ajaxpro/Bikewale.Ajax.AjaxSellBike,Bikewale.ashx",
            data: '{"imageList":"' + imageList + '"}',

            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "FetchProcessedImagesList"); },
            success: function (response) {
                var ret_response = eval('(' + response + ')');
                var obj_response = eval('(' + ret_response.value + ')');

                if (obj_response.Table.length > 0) {
                    for (var i = 0; i < obj_response.Table.length; i++) {

                        imageList = imageList.replace(obj_response.Table[i].Id + ',', '');
                        var imgUrl = obj_response.Table[i].HostUrl +"/144X81/"+ obj_response.Table[i].DirectoryPath + obj_response.Table[i].OriginalImagePath;
                        if (pendingList.indexOf(obj_response.Table[i].Id) > -1)
                            pendingList.splice(pendingList.indexOf(obj_response.Table[i].Id), 1);

                        $('#dtlstPhotosPending_' + obj_response.Table[i].Id).removeClass('show').addClass('hide');
                        $('#dtlstPhotosPending_' + obj_response.Table[i].Id).attr("pending", "false");
                        $('#dtlstPhotos_' + obj_response.Table[i].Id).attr('class', 'show');
                        $('#dtlstPhotos_' + obj_response.Table[i].Id).find('img').attr('src', imgUrl);
                    }
                }
            }
        })
    }

    function GetPendingList() {
        $(".pending").each(function () {
            if ($(this).attr("pending") == "true") {
                pendingList.push(this.id.replace("dtlstPhotosPending_", ""));
            }
        });
    }
    function UpdatePendingImages() {
        var list = "";
        if (pendingList.length > 0) {
            for (var i = 0; i < pendingList.length; i++) {
                list += pendingList[i] + ",";
            }
            CheckImageStatus(list);
        }
    }
</script>
<!-- #include file="/includes/footerInner.aspx" -->