<%@ Page AutoEventWireUp="false" Language="C#" Inherits="BikeWaleOpr.EditCms.CreateAlbum" debug="false" Trace="false" EnableEventValidation="false" %>
<%@ Register TagPrefix="Uc" TagName="DispBasicInfo" src="/editcms/DisplayBasicInfo.ascx" %>
<%@ Register TagPrefix="Ec" TagName="EditCmsCommon" src="/editcms/EditCmsCommon.ascx" %>
<%@ Import Namespace="BikeWaleOpr" %>
<!-- #include file="/includes/headerNew.aspx" -->
<script type="text/javascript" src="/src/graybox.js"></script>
<!--[if IE]><script language="javascript" src="/src/common/excanvas.js"></script><![endif]-->
<style>
	.errMessage {color:#FF4A4A;}
	.note {font-size:9px; color:#6F6F6F; font-style:italic;}
    .hide { display:none; }
</style>
<div class="urh">
	<a href="/default.aspx">Bikewale operations</a> &raquo; <a href="/editcms/default.aspx">Editorial Home</a> &raquo; Manage Articles
</div>
<div style="clear:both;">
		<asp:Label ID="lblEditImageId" runat="server" Text="-1" Visible="false" />
		<div>
			<Ec:EditCmsCommon ID="EditCmsCommon" runat="server" />
		</div>
		<div>
		  <div style="width:525px;float:left;">
				<table cellpadding="2" cellspacing="3">
					<tr>
					  <td width="72">
						Photo
						<% if(!isEditImage) { %> 
						<span style="color:red;">*</span>
						<% } %>
					  </td>
					  <td width="196">
						<input name="file" type="file" id="inpPhoto" runat="server" /> <span class="note">Max file size is 2MB.</span>
						<span class="errMessage" id="spnPhoto"></span>
					  </td>
					  <% if(isEditImage) { %>
					  <td width="36" rowspan="5"><img id="imgEdit" src='<%= EditImagePath %>' /></td>
					  <% } else { %>
					  <td width="1" rowspan="5">&nbsp;</td>
					  <% } %>
					  <td width="218"> Gallery Shown? : <%= Gallery %> <br />
						<asp:Button ID="btnGallery" runat="server"></asp:Button><br />
					  	<i><span id="lblGallery" runat="server"></span></i>
					  </td>
					</tr>
					<tr>
					  <td width="72">Caption </td>
					  <td>
						<asp:TextBox ID="txtCaption" runat="server" />
					  </td>					   
					</tr>
					<tr>
					  <td width="72">Category <span style="color:red;">*</span></td>
					  <td>
						<asp:DropDownList ID="ddlCategory" runat="server" />
						<span class="errMessage" id="spnCategory"></span>
					  </td>					  
					</tr>
                    <tr>
                        <td width="72">Dimensions </td>
                        <td>
                            <asp:DropDownList ID="ddlDimensions" runat="server">
                                <asp:ListItem Value="-1" Text="--Select Dimension--"></asp:ListItem>
                                <asp:ListItem Value="330|166" Text="Exterior Dimension: 330 x 166"></asp:ListItem>
                                <asp:ListItem Value="285|166" Text="Boot Dimension: 285 x 166"></asp:ListItem>
                                <asp:ListItem Value="620|400" Text="Overview: 620 x 400"></asp:ListItem>
                                <asp:ListItem Value="200|100" Text="Verdict: 200 x 100"></asp:ListItem>
                                <asp:ListItem Value="300|180" Text="Exterior & Interior: 300 x 180"></asp:ListItem>
                                <asp:ListItem Value="500|270" Text="Home Page Image: 500 x 270"></asp:ListItem>
                            </asp:DropDownList>                            
                        </td>
                        <td>
                            <div id="div_hp_photo" style="margin-top:15px; border:1px solid #DBDBCE; background-color:#EEEEEE; padding:3px;" class="hide">
                                <input name="file" type="file" id="fl_HomePgPhoto" runat="server" />
                                <div class="note">Select 500 x 270 size image for home page.</div>
                                <div class="note">Max file size is 2MB.</div>
                                <span class="errMessage" id="spnHPImage"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td width="85">Make & Model</td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlMake" runat="server" style="width:150px;"/>&nbsp;&nbsp;<asp:DropDownList ID="ddlModel" runat="server" style="width:150px;"></asp:DropDownList>
                            <input type="hidden" id="hdn_drpModel" runat="server" />
				            <input type="hidden" id="hdn_selModel" runat="server" value="-1" />
				            <input type="hidden" id="hdn_selModelName" runat="server" value="" />	
                        </td>
                    </tr>
                    <tr>
                        <td>Image Name <span style="color:red;">*</span></td>
                        <td colspan="3">
                            <input type="text" id="txtImageName" runat="server" style="width:200px;" /> <span class="note">Image Names should not exceed 100 Characters and should be separated by Spaces or hyphen(-). No other special characters allowed.</span>
                        </td>
                    </tr>
					<tr>
					  <td width="72"></td>
					  <td>
						<asp:CheckBox ID="chkIsMainImg" runat="server" Text="Is this main image?" />
						<asp:Label ID="lblMainImgSet" runat="server" />
					  </td>					 
					</tr>
                    <tr>
					  <td width="72"></td>
					  <td colspan="3">
						<asp:CheckBox ID="chkGenWaterMark"  runat="server" Text="Generate Watermark? (Check to generate)" Checked="true"/>						
					  </td>					 
					</tr>
					<tr>
					  <td width="72"></td>
					  <td>
						<asp:Button ID="btnSave" runat="server" Text="Save" />
						<asp:Button ID="btnUpdate" runat="server" Text="Update" style="display:none;" />
						<input type="button" id="btnCancel" value="Cancel" style="display:none;" onclick="CancelEdit();" />
					</td>
					</tr>
					<tr>
						<td></td>
						<td><asp:Label ID="lblMessage" runat="server" CssClass="errMessage" /></td>
					</tr>
				</table>
			</div>
			<div style="display:none;">
				<a href="addpages.aspx?bid=<%=Request.QueryString["bid"]%>&new=<%=Request.QueryString["new"]%>"><asp:Button ID="btnContinue" runat="server" Text="Continue" style="display:none;"/></a>
			</div>
			<div style="width:400px;float:right;display:none;">
				<Uc:DispBasicInfo ID="BasicInfo" runat="server" />
			</div>
			<div style="clear:both;"></div>
		</div>		
		<div id="divMainImage" runat="server" style="margin-left:14px;padding:5px;border:1px solid #DBDBCE;width:180px; display:block;">

            <div id ="divMainImageRepTable" pending="<%= statusId =="1"? "true" : "false" %>">
                <% if(imageName != string.Empty){ %>
                <img id='divMainImage_<%= mainImgId%>'   aria-label="Main Image" src="<%= statusId =="1"? "http://img.aeplcdn.com/loader.gif" :  ImagingOperations.GetPathToShowImages(imagePathThumbnail, hostUrl)%>"/>
                <div style='margin:5px 5px;'>Main Image&nbsp;&nbsp;&nbsp;&nbsp;<a id='A1' image-id= <%= mainImgId%> class='pointer editTagging' style='text-decoration:underline' >Edit Tagging</a></div>
                <% } else { %>
                     No Main Image
                 <% } %>
            </div>

                
		</div>		
        

		<div style="z-index:1;">
			<asp:DataList ID="dlstPhoto" DataKeyField="ID" RepeatDirection="Horizontal" runat="server" RepeatColumns="4" ItemStyle-BorderColor="#DBDBCE" ItemStyle-BorderWidth="1" ItemStyle-Width="180px" CellSpacing="14" >
				<itemtemplate>
                    <div class="padding5">
					    <div style="text-align:center;vertical-align:text-top;"><b><%# DataBinder.Eval( Container.DataItem, "Sequence" ).ToString() %></b></div>
                        <div>
                        <div style="float:left;width:180px;" id='divImg_<%# DataBinder.Eval( Container.DataItem, "Id" ).ToString()%>' class='<%# DataBinder.Eval(Container.DataItem, "StatusId").ToString()=="1" ? "hide" : "show" %>'> 
                            <img class='img-border' id="img1" src='<%# "http://" + DataBinder.Eval(Container.DataItem, "HostURL").ToString() + DataBinder.Eval( Container.DataItem, "ImagePathThumbNail" ) %>' style='margin:auto; display:block;' />
                            <div style="margin:5px 0;"><b>Caption : </b> <%# String.IsNullOrEmpty(DataBinder.Eval( Container.DataItem, "Caption" ).ToString()) ? "N/A" : DataBinder.Eval( Container.DataItem, "Caption" ).ToString() %></div>
                        </div>
                        <div style="float:left;width:180px;" id ='divPendingImg_<%# DataBinder.Eval( Container.DataItem, "Id" ).ToString()%>' class='pending <%# DataBinder.Eval(Container.DataItem, "StatusId").ToString()=="1"? "show" : "hide" %>' pending="<%# DataBinder.Eval(Container.DataItem, "StatusId").ToString()=="1"? "true" : "false" %>">
                            <p style="color:#555555;font-weight:bold;">
                            Processing...
                                <img  align="center" src='http://img.aeplcdn.com/loader.gif'/>
                            </p>
                        </div>
                        </div>
                        <div>
					        <a style="display:none;" href="CreateAlbum.aspx?bid=<%=Request.QueryString["bid"]%>&EditImageId=<%# DataBinder.Eval( Container.DataItem, "Id" ).ToString() %>">Edit</a>
					        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="delete">Delete</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
					        <a class="move" style="text-decoration:underline;" att='<%# DataBinder.Eval( Container.DataItem, "Id" ).ToString() %>'>Shift To</a>&nbsp;&nbsp;&nbsp;&nbsp;
					        <a class="pointer editTagging" style="text-decoration:underline" image-id='<%# DataBinder.Eval( Container.DataItem, "Id" ).ToString() %>' make-id="<%# DataBinder.Eval( Container.DataItem, "MakeId" ).ToString() %>" model-id="<%# DataBinder.Eval( Container.DataItem, "ModelId" ).ToString() %>">Edit Tagging</a>
                        </div>
					   <%-- <div class="margin-top10"><img src='<%# "http://" + DataBinder.Eval(Container.DataItem, "HostURL").ToString() + DataBinder.Eval( Container.DataItem, "ImagePathThumbNail" ) %>' style="margin:auto; display:block;"/></div>--%>
					    
                    </div>
				</itemtemplate>
			</asp:DataList>	
		</div>
		<div style="min-height:200px;">&nbsp;</div>
		<div id="divPosition" style="position:absolute;border:1px solid;display:none;background-color:#EEEEEE;height:50px;width:100px;">
			<br />
			<asp:TextBox ID="txtPosition" runat="server" style="width:25px;margin-left:8px;"></asp:TextBox>&nbsp;
			<asp:Button ID="btnPosition" runat="server"  Text="Go"></asp:Button>
			<input type="hidden" id="hdnImageId" runat="server" />
		</div>
	

<script type="text/javascript">	  
    var pendingList = new Array();
    var updateCounter = 0;
    var refreshTime = 2000;
    var imageIdList = "";

    $(document).ready(function () {

        GetPendingList();

        setInterval(UpdatePendingMainImage, refreshTime)
        setInterval(UpdatePendingImages, refreshTime)

        $("#ddlMake").change(function () {
            ddlMake_Change($(this));
        });
        $("#ddlModel").change(function () {
            ddlModel_Change($(this));
        });
        $("#ddlCategory").change(function () {
            CreateImageName();
        });
        $("a.move").click(function (e) {
            $("#divPosition").show();
            $("#hdnImageId").val($(this).attr("att"));
            $("#divPosition").css("left", e.pageX);
            $("#divPosition").css("top", e.pageY);
        });

        $("#divPosition").mouseleave(function (e) {
            $("#txtPosition").val("");
            $("#divPosition").hide();
            $("#hdnImageId").val("");
        });
        $("#txtPosition").keydown(function (event) {
            // Allow only backspace and delete
            if (event.keyCode == 46 || event.keyCode == 8) {
                // let it happen, don't do anything
            } else {
                // Ensure that it is a number and stop the keypress
                if (event.keyCode < 48 || event.keyCode > 57) {
                    event.preventDefault();
                }
            }
        });

        $("#ddlDimensions").change(function () {
            if ($(this).val() == "500|270") {
                $("#div_hp_photo").show();
                $("#chkIsMainImg").attr("checked", true);
                //alert($("#chkIsMainImg").is(":checked"));
            } else {
                $("#div_hp_photo").hide();
                $("#chkIsMainImg").attr("checked", false);
            }
        });

        $("#chkIsMainImg").change(function () {
            if ($(this).is(":checked")) {
                $("#div_hp_photo").show();
                $("#ddlDimensions").val("500|270");
            }
            else {
                $("#div_hp_photo").hide();
                $("#ddlDimensions").val("-1");
            }
        });
        

        $("a.editTagging").click(function () {
            var comment = "";
            var caption = "Update Image Tagging";
            var id = $(this).attr("image-id");
            var url = "/editcms/manageimagetagging.aspx?id=" + id;
            var applyIframe = true;
            var GB_Html = "";            
            GB_show(caption, url, 220, 340, applyIframe, GB_Html);
        });
    });
        function ddlMake_Change(ddl) {
            var make = ddl.val();
            AjaxFunctions.GetModels(make, modelCallback);
            CreateImageName();
        }

        function ddlModel_Change(ddl) {
            var model = ddl.val();
            $("#hdn_selModel").val(model);
            $("#hdn_selModelName").val(ddl.find("option[value='" + model + "']").html());
            CreateImageName();
        }

        function CreateImageName() {
            var makeText = "";
            var modelText = "";
            var categoryText = "";

            if ($("#ddlMake").val() != "0" && $("#ddlMake").val() != "-1") {
                makeText = $("#ddlMake option:selected").text().replace(' ', '-') + "-";

                if ($("#ddlModel").val() != "0" && $("#ddlModel").val() != "-1") {
                    modelText = $("#ddlModel option:selected").text().replace(' ', '-') + "-";
                }
            }
            if ($("#ddlCategory").val() != "0" && $("#ddlCategory").val() != "-1") {
                categoryText = $("#ddlCategory option:selected").text().replace(' ', '-');
            }

            $("#txtImageName").val(makeText + modelText + categoryText);
        }

        function AppendCategory() {
            var category = $("#ddlCategory").val();
            if (category == 0) {
                alert("Please select image category");
            } else {
                $("#txtImageName").val($("#txtImageName").val() + '-' + $("#ddlCategory").find("option[value='" + category + "']").html().replace(' ', '-'));
            }
        }

        function modelCallback(response) {
            var dependentCmbs = new Array();
            dependentCmbs[0] = "";
            FillCombo_Callback(response, document.getElementById("ddlModel"), "hdn_drpModel", dependentCmbs, "--Select Model--");
        }


        function validateFileSize(objFile) {
            //var ext = $("fl_HomePgPhoto").val().split('.').pop().toLowerCase();
            var ext = objFile.val().split('.').pop().toLowerCase();

            if ($.inArray(ext, ['gif', 'jpg', 'jpeg']) == -1) {
                alert('invalid extension!');
                return false;
            } 
                //this.files[0].size gets the size of your file. file more than 2 MB not allowed
                if (objFile[0].files[0].size > 2097152) {
                    alert("Invalid file size!");
                    return false;
                }
                return true;
        }


        function Validate() {
            $("#spnCategory").html("");
            $("#spnPhoto").html("");
            $("#lblMessage").html("");

            if ($("#inpPhoto").val() == "" && $("#btnSave").is(":hidden") == false) {
                alert("Please browse photo");
                return false;
            }

            if (!validateFileSize($("input:file").filter("#inpPhoto")))
                return false;

            if ($("#ddlDimensions").val() == "500|270") {
                if ($("input:file").filter("#fl_HomePgPhoto").val() != "") {
                    if (!validateFileSize($("input:file").filter("#fl_HomePgPhoto")))
                        return false;
                }
                else {
                    alert("Upload home page image");
                    return false;
                }
            }

            if ($("#ddlCategory").val() <= 0) {
                alert("Please select image category");
                return false;
            }

            if ($("#txtImageName").val().trim() == "") {
                alert("Please enter a name for the Image");
                return false;
            }

            return true;
        }

        function GetModelVal() {
            var model = ddl.val();
            $("#hdn_selModel").val(model);
            $("#hdn_selModelName").val(ddl.find("option[value='" + model + "']").html());
        }

        function EditImage(imageId) {
            $("#btnUpdate").show();
            $("#btnSave").hide();
            $("#lblEditImageId").html(imageId);
        }

        $(document).ready(function () {
            <%
		if (lblEditImageId.Text != "-1")
        {
            %>
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnCancel").show();
            <%
        }
        else
        {
        %>
            $("#btnSave").show();
            $("#btnUpdate").hide();
            $("#btnCancel").hide();
            <%
        }
		%>


        });

        function CancelEdit() {
            location.href = "createalbum.aspx?bid=<%=Request.QueryString["bid"].ToString()%>";
        }

    function GetPendingList() {
        $(".pending").each(function () {
            if ($(this).attr("pending") == "true") {
                pendingList.push(this.id.replace("divPendingImg_", ""));
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

    function UpdatePendingMainImage() {
        var id = $("#divMainImageRepTable").find('img').attr('id').replace("divMainImage_", "");
        var pending = $("#divMainImageRepTable").attr('pending');
        if (pending == 'true') {
            CheckMainImageStatus(id);
        }

    }

    function CheckMainImageStatus(mainImageId) {
        $.ajax({
            type: "POST", url: "/AjaxPro/BikeWaleOpr.Common.Ajax.ImageReplication,BikewaleOpr.ashx",
            data: '{"imageId":"' + mainImageId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "CheckImageStatus"); },
            success: function (response) {
                var ret_response = eval('(' + response + ')');
                var obj_response = eval('(' + ret_response.value + ')');

                if (obj_response.Table.length > 0) {
                    for (var i = 0; i < obj_response.Table.length; i++) {
                        var imgUrl = "http://" + obj_response.Table[i].HostUrl + obj_response.Table[i].ThumbNailURL;
                        $("#divMainImageRepTable").attr("pending", "false");
                        $("#divMainImageRepTable").find('img').attr('src', imgUrl);

                    }

                }
            }
        })

    }
    function CheckImageStatus(imageList) {
        $.ajax({
            type: "POST", url: "/AjaxPro/BikeWaleOpr.Common.Ajax.ImageReplication,BikewaleOpr.ashx",
            data: '{"imageList":"' + imageList + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "FetchProcessedImagesList"); },
            success: function (response) {
                var ret_response = eval('(' + response + ')');
                var obj_response = eval('(' + ret_response.value + ')');
                if (obj_response.Table.length > 0) {
                    for (var i = 0; i < obj_response.Table.length; i++) {

                        imageList = imageList.replace(obj_response.Table[i].Id + ',', '');
                        var imgUrl = "http://" + obj_response.Table[i].HostUrl + obj_response.Table[i].ImagePathThumbnail;

                        if (pendingList.indexOf(obj_response.Table[i].Id) > -1)
                            pendingList.splice(pendingList.indexOf(obj_response.Table[i].Id), 1);

                        $('#divPendingImg_' + obj_response.Table[i].Id).removeClass('show').addClass('hide');
                        $("#divPendingImg_" + obj_response.Table[i].Id).attr("pending", "false");
                        $('#divImg_' + obj_response.Table[i].Id).attr('class', 'show');
                        $('#divImg_' + obj_response.Table[i].Id).find('img').attr('src', imgUrl);
                    }

                }
            }
        })
    }


</script>	
</div>
<!-- #Include file="/includes/footerNew.aspx" -->

