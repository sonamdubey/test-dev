<%@ Page AutoEventWireUp="false" Language="C#" Inherits="BikeWaleOpr.EditCms.Author" Trace="false" Debug="false" ValidateRequest="false" %>
<%@ Register TagPrefix="Vspl" TagName="RTE" src="/Controls/RichTextEditor.ascx" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<style type="text/css">
	.errMessage {color:#FF4A4A;}
	.alert{background:url(http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/m/bg-alert.gif) repeat-x; padding:15px; border:1px solid #F7EC64;}
	.moz-round{-moz-border-radius:3px;}
</style>
<div class="urh">
	<a href="/default.aspx">BikeWale operations</a> &raquo; <a href="/editcms/default.aspx">Editorial Home</a> &raquo; Manage Articles
</div>
<script language="javascript" src="/src/AjaxFunctions.js"></script>
<script language="javascript" src="http://opr.carwale.com/src/common/bt.js"></script>
<!--[if IE]><script language="javascript" src="http://opr.carwale.com/src/common/excanvas.js"></script><![endif]-->
<div style="clear:both;">	
    <div><h1 style="padding-left:0px;">Authors Information <a href="default.aspx" style="font-size:12px;">&lsaquo; Back to All Articles</a></h1></div>   	
	<br />
    
        <div style="width:800px;float:left;">
		    <input id="hdnCurrentDay" type="hidden" />
		    <input id="hdnCurrentMonth" type="hidden" />
		    <input id="hdnCurrentYear" type="hidden" />
		    <input id="hdnBasicId" type="hidden" value="-1" runat="server" />
            <div class="alert moz-round" id="alertObj" runat="server"></div>
		    <table id="tblBasicInfo" width="100%" cellpadding="2" cellspacing="3">
                <tr>
                    <td colspan="2"><asp:label id="lblResult" runat="server" forecolor="red" ></asp:label></td>
                </tr>
			    <tr>
			        <td width="72">Author <span style="color:red;">*</span></td>
			        <td><asp:DropDownList ID="ddlAuthor" runat="server"  /><span class="errMessage" id="spnAuthor"></span></td>
                </tr>
			    <tr>
			        <td>Designation<span class="errMessage">*</span></td>
			        <td><asp:TextBox ID="txtDesignation" runat="server" Columns="56" /><span id="spnDesignation" class="errMessage"></span></td>
		  	    </tr>
			    <tr>
                    <td width="72">Profile Picture<span class="errMessage">*</span></td>
			        <td ><input name="file" type="file" id="inpPhoto" runat="server" width="196" /><span class="errMessage" id="spnPhoto"></span></td>
		  	    </tr>			
			    <tr>
			        <td>Brief Profile <span class="errMessage">*</span></td>
			        <td><asp:TextBox ID="txtBriefProfile" runat="server" Columns="56" /><span id="spnBriefProfile" class="errMessage"></span></td>
                </tr> 
			    <tr>
			        <td>Full Profile <span class="errMessage">*</span></td>
			        <td><Vspl:RTE id="rteDescription" Rows="20" Cols="75" runat="server" />&nbsp;	<span id="spnDescription" class="errMessage"></span></td>
		  	    </tr>
			    <tr>
				    <td></td>
				    <td>
					    <asp:Button ID="btnSave" runat="server" Text="Save" />                    
					    <!--input type="button" ID="btnUpdate" value="Update" style="display:none;" onclick="UpdateRecord();" /-->
					    <input type="button" ID="btnCancel" value="Cancel" style="display:none;" onclick="CancelUpdate();" />
					    <input type="hidden" id="hdnSubCat" runat="server" />
					    <!--<a id="aSelectedCars" style="display:none;">View selected cars</a>-->
				    </td>
			    </tr>
		    </table>
	    </div>	
	<div style="clear:both;"></div>
	<div>
		<div id="divBasicData"></div>
	</div>
	<div style="min-height:200px;">&nbsp;</div>
	
<script language="javascript" type="text/javascript" >
//	<%
//		if (Request.QueryString["bid"] != null)
//		{
//	%>
//			var bid = <%= Request.QueryString["bid"] %>;
//	<%
//		}
//	%>	
	
	function Validate()
	{
		var retVal = true;

		$("#spnAuthor").html("");
		$("#spnDesignation").html("");
		$("#spnBriefProfile").html("");
		$("#spnDescription").html("");

		if ($("#ddlAuthor").val() <= 0)
		{
		    $("#spnAuthor").html("Select Author");
			retVal = false;
		}


       if ($("#txtDesignation").val().trim() == "")
		{
		    $("#spnDesignation").html("Enter Designation");
			retVal = false;
		}


        if ($("#txtBriefProfile").val().trim() == "")
		{
		    $("#spnBriefProfile").html("Enter Brief Profile");
			retVal = false;
		}
		
		if ( tinyMCE.get("rteDescription_txtContent").getContent().trim() == "" )
		{
			$("#spnDescription").html("Enter Description");
			retVal = false;
		}


       if ($("#inpPhoto").val() == "") 
       {
           $("#spnPhoto").html("Please browse photo"); 
           retVal = false;
       }	
			
		return retVal;
	}
	

//	$(document).ready(function(){
//	
//		$("#hdnCurrentDay").val($("#dtDate_cmbDay").val());
//		$("#hdnCurrentMonth").val($("#dtDate_cmbMonth").val());
//		$("#hdnCurrentYear").val($("#dtDate_txtYear").val());
//		
//		<%
//		if (Request.QueryString["bid"] != null)
//		{
//		%>
//			$("#btnSave").hide();
//			$("#btnUpdate").show();
//			$("#btnCancel").hide();
//			$("#hdnBasicId").val(<%=bid%>);
//			$("#ddlCategory").attr("disabled","disabled");
//		<%
//		}
//		%>
//		
//		$("#subCatContainer  input:checkbox").live('click',function(){
//			if($(this).is(':checked'))
//			{
//				if($("#hdnSubCat").val() == "")
//					$("#hdnSubCat").val($(this).attr("id"));
//				else
//					$("#hdnSubCat").val($("#hdnSubCat").val() + "," + $(this).attr("id"));
//			}
//			else
//			{
//				if( $("#hdnSubCat").val().indexOf(',') > 0 )
//				{
//                    var ids = new String($("#hdnSubCat").val()).split(',');                    
//                    for( var i = 0; i < ids.length; ++i)
//                    {
//                        if(ids[i] == $(this).attr("id")){                            
//                            ids.splice(i,1);
//                        }
//                    }
//                    $("#hdnSubCat").val(ids);
//				}
//				else
//				{
//					$("#hdnSubCat").val($("#hdnSubCat").val().replace($(this).attr("id"),""));
//				}                
//			}
//		});		
//		
//        $("#btnUpdate").live('click',function(){
//            if(!Validate())
//            {
//                return false;
//            }
//        });

//		$("#ddlCategory").change(function(){			
//			var categoryId = $(this).val();		
//			$.ajax({
//				type: "POST",
//				url: "/ajaxpro/Carwale.EditCms.AjaxEditCms, Carwale.EditCms.ashx",			
//				data: '{"categoryId":"'+ categoryId + '"}',	
//				beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetSubCategory"); },
//				success: function(response) {
//					var response = eval('('+ response +')');						
//					var obj_response = eval('(' + response.value + ')');
//					if( obj_response.Table.length > 0 ){
//						var str = "<ul>";
//						for( var i = 0; i < obj_response.Table.length; i++ ){
//							str += "<li><input id=\""+obj_response.Table[i].Id+"\" type=\"checkbox\" name=\"chk\" /><label for=\""+obj_response.Table[i].Id+"\">"+obj_response.Table[i].Name+"</label></li>";
//						}
//						str += " </ul> ";
//						$("#subCatContainer").html(str);
//					}else{
//						$("#subCatContainer").html("");
//					}
//				}	
//			});			
//		});
//		
//	});
//	
//	function UpdateRecord()
//	{
//		if (Validate())
//		{
//			var _id = $("#hdnBasicId").val();
//			var _title = $("#txtTitle").val();			
//			var tmp = _title.replace(/[^a-zA-Z 0-9]+/g,'').replace(/\s+/g,' ');			
//			var _url = tmp.replace(/ /g, "-").toLowerCase();
//			var _authorName = $("#txtAuthor").val();
//			//var _authorName = $("#ddlAuthor option:selected").text();
//			var _authorId = -1;//$("#ddlAuthor").val();
//			var _description = tinyMCE.get("rteDescription_txtContent").getContent().replace(/"/g, "'");
//			var _displayDate = $("#dtDate_txtYear").val() + "-" + $("#dtDate_cmbMonth").val() + "-" + $("#dtDate_cmbDay").val()+ "-" + $("#ddlHours").val()+ "-" + $("#ddlMins").val();
//			var _subCatId = $("#hdnSubCat").val();				
//			
//			$.ajax({
//				type: "POST",
//				url: "/ajaxpro/Carwale.EditCms.AjaxEditCms,Carwale.EditCms.ashx",			
//				data: '{"id":"'+ _id +'", "title":"'+ _title  +'", "url":"'+ _url +'", "authorName":"'+ _authorName +'", "authorId":"'+ _authorId  +'","description":"'+ _description +'", "displayDate":"'+ _displayDate +'", "subCatId":"'+ _subCatId  +'"}',	
//				beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateBasicInfo"); },
//				success: function(response) {
//					var status = eval('('+ response +')');	
//					if(status.value == true)
//					{									
//						alert("Record successfully updated");
//					}					
//				}
//			});
//		}
//	}                                       
	
</script>	
</div>
<!-- #Include file="/includes/footerNew.aspx" -->

