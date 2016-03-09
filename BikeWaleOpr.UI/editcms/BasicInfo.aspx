<%@ Page AutoEventWireUp="false" Language="C#" Inherits="BikeWaleOpr.EditCms.BasicInfo" Trace="false" Debug="false" ValidateRequest="false" %>
<%@ Register TagPrefix="dt" TagName="DateControl" src="/Controls/DateControl.ascx" %>
<%@ Register TagPrefix="Uc" TagName="DispBasicInfo" src="/editcms/DisplayBasicInfo.ascx" %>
<%@ Register TagPrefix="Ec" TagName="EditCmsCommon" src="/editcms/EditCmsCommon.ascx" %>
<%@ Register TagPrefix="Vspl" TagName="RTE" src="/Controls/RichTextEditor.ascx" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<script type="text/javascript" src="/src/greybox.js"></script>
<script language="javascript" type="text/javascript" src="editcmsjs.js"></script>
<style type="text/css">
	.errMessage {color:#FF4A4A;}
	.alert{background:url(http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/m/bg-alert.gif) repeat-x; padding:15px; border:1px solid #F7EC64;}
	.moz-round{-moz-border-radius:3px;}
	/* grey box css */
    .gb-content { border: 0px solid #a6c9e2; color: #222222; }
    #gb-window { position: absolute; width: 300px; z-index:100002; display:none; border: 2px solid #d5d5d7; background-color:#fff;}
    #gb-inner{padding:5px 15px 0 15px;}
    #gb-head{position:relative; border-bottom:0px solid #d5d5d7;}
    #gb-title { float: left; margin: .2em 0 .2em .3em; font-size:18px; font-weight:bold;} 
    #gb-close{ display: none; float:right; margin-top:5px;}
    #gb-content { border: 0;background: none; overflow:auto; zoom: 1;}
    #loading{margin:7px 0 7px 7px;}
    #gb-overlay {position: absolute; top: 0; left: 0; width: 100%; height: 100%; background-color:#F7F7F7; opacity: .30;filter:Alpha(Opacity=30); display:none; z-index:100001; background: url("http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/overlay.png") repeat scroll 0 0 transparent;}
</style>
<div class="urh">
	<a href="/default.aspx">Bikewale operations</a> &raquo; <a href="/editcms/default.aspx">Editorial Home</a> &raquo; Manage Articles
</div>
<script language="javascript" src="/src/AjaxFunctions.js"></script>
<script language="javascript" src="http://opr.carwale.com/src/common/bt.js"></script>
<!--[if IE]><script language="javascript" src="http://opr.carwale.com/src/common/excanvas.js"></script><![endif]-->
<div style="clear:both;">
	<% if(bid == "") {%>
			<div><h1 style="padding-left:0px;">Add New Article <a href="default.aspx" style="font-size:12px;">&lsaquo; Back to All Articles</a></h1></div>
		<% }else{%>
			<Ec:EditCmsCommon ID="EditCmsCommon" runat="server" />
            <script type="text/javascript">
	            tinyMCE.init({
                    width: "660px",
                    height:"300px",
                });
            </script>
	<%}%>
	
	<div style="width:800px;float:left;">
		<input id="hdnCurrentDay" type="hidden" />
		<input id="hdnCurrentMonth" type="hidden" />
		<input id="hdnCurrentYear" type="hidden" />
		<input id="hdnBasicId" type="hidden" value="-1" runat="server" />
        <div class="alert moz-round" id="alertObj" runat="server" style="display:none;"></div>
		<table id="tblBasicInfo" width="100%" cellpadding="2" cellspacing="3">
			<tr>
			  <td width="100px">Select Category <span class="errMessage">*</span></td>
			  <td>
			  	<asp:DropDownList ID="ddlCategory" runat="server"  />
			  	<span class="errMessage" id="spnCategory"></span>
			  </td>
		  	</tr>
			<tr>
			  <td>Title <span class="errMessage">*</span></td>
			  <td>
			  	<asp:TextBox ID="txtTitle" runat="server" Columns="56" />
				<span id="spnTitle" class="errMessage"></span>
			</td>
		  	</tr>
			<tr>
			  <td>Display Date <span class="errMessage">*</span></td>
			  <td>
			  	<dt:DateControl id="dtDate" runat="server"/><asp:DropDownList ID="ddlHours" runat="server" /><asp:DropDownList ID="ddlMins" runat="server" />
			  	<span id="spnDisplayDate" class="errMessage"></span>
			  </td>
		  	</tr>		
			<tr>
			  <td>Author Name <span class="errMessage">*</span></td>
			   <td><asp:DropDownList ID="ddlAuthor" runat="server"  /><span class="errMessage" id="spnAuthor"></span></td>
		  	</tr>                        
            </table>

            <div style="margin:5px 0 5px 110px; padding:3px; border:1px solid #CCCCCC;" class="tab"><asp:CheckBox ID="chkIsFeatured" runat="server" Text="Is Featured ?" /></div>		  	
            <%--<div id="divDynamicControl" runat="server" style="border:0"></div>  --%>          
            
        <table width="100%" cellpadding="2" cellspacing="3">
			<tr>
			  <td width="100px">Description <span class="errMessage">*</span></td>
			  <td>
			  	<Vspl:RTE id="rteDescription" Rows="20" Cols="75" runat="server" />&nbsp;	<span id="spnDescription" class="errMessage"></span>	
			  </td>
		  	</tr>
			<tr>
				<td></td>
				<td>
					<input type="button" id="btnSave" runat="server" value="Continue" />
                    <input type="button" id="btnUpdate" runat="server" value="Update" />					
					<input type="button" id="btnCancel" value="Cancel" />
					<input type="hidden" id="hdnSubCat" runat="server" />
                    <input type="hidden" id="hdnExtdInfoId" runat="server" />
                    <input type="hidden" id="hdnExtdInfoIdVals" />
                    <input type="hidden" id="hdnExtdInfoValTypes" />
				</td>
			</tr>
		</table>
	</div>
	<span id="spnSubCat" class="errMessage"></span><br />   
	<div style="width:300px; height:400px; float:left; overflow:auto; border:1px solid #CCCCCC;" >
		<span class="grey">Sub Categories</span>
		<div id="subCatContainer" runat="server"></div>
	</div>
	<div style="clear:both;"></div>
	<div>
		<div id="divBasicData"></div>
	</div>
	<div style="min-height:200px;">&nbsp;</div>
	
<script language="javascript" type="text/javascript" >
	
    var bid; 
    <%if( bid != "" ) {%>
        bid = <%= bid %>;       
    <%} else { %>
        bid = "";
    <% } %>
	
    var ExtdInfoLabel = "";
    var ExtdInfoType = "";
    var NewRowCount = 0;    
    var AllowBikeSelect = false;

	function Validate()
	{
		var retVal = true;
		
		$("#spnCategory").html("");
		$("#spnTitle").html("");
		$("#spnAuthor").html("");
		$("#spnDescription").html("");      

		if ( $("#ddlCategory").val() <= 0 ) {
			$("#spnCategory").html("Select Category");
			retVal = false;
		}
		
		if ( $("#txtTitle").val().trim() == "" ) {
			$("#spnTitle").html("Enter Title");
			retVal = false;
		}		
		
		if ($("#ddlAuthor").val() <= 0) {
		    $("#spnAuthor").html("Select Author");
			retVal = false;
		}       
		
		if ( tinyMCE.get("rteDescription_txtContent").getContent().trim() == "" ) {
			$("#spnDescription").html("Enter Description");
			retVal = false;
		}

		if( $("#ddlCategory").val() != 0 && $("#subCatContainer").html() != "" ) {
			if( $("#hdnSubCat").val() == "" ){
				$("#spnSubCat").html( "Select Sub Category" );
				retVal = false;
			}else{
				$("#spnSubCat").html( "" );
            }
		}
		return retVal;
	}

	$(document).ready(function(){

		$("#hdnCurrentDay").val($("#dtDate_cmbDay").val());
		$("#hdnCurrentMonth").val($("#dtDate_cmbMonth").val());
		$("#hdnCurrentYear").val($("#dtDate_txtYear").val());
		
        <%
		if (Request.QueryString["bid"] != null) {%>
			$("#btnSave").hide();
			$("#btnUpdate").show();
			$("#btnCancel").hide();
			$("#hdnBasicId").val(<%=bid%>);
			$("#ddlCategory").attr("disabled","disabled");
		<% } else { %>
            $("#btnSave").show();
			$("#btnUpdate").hide();
			$("#btnCancel").show();						
		<% } %>

		$("#subCatContainer input:checkbox").live('click', function(){
			if($(this).is(':checked')){
				if($("#hdnSubCat").val() == ""){
					$("#hdnSubCat").val($(this).attr("id"));
				}else{
					$("#hdnSubCat").val($("#hdnSubCat").val() + "," + $(this).attr("id"));
                }
			}
			else
			{
				if( $("#hdnSubCat").val().indexOf(',') > 0 ){
                    var ids = new String($("#hdnSubCat").val()).split(',');                    
                    for( var i = 0; i < ids.length; ++i)
                    {
                        if(ids[i] == $(this).attr("id")){                            
                            ids.splice(i,1);
                        }
                    }
                    $("#hdnSubCat").val(ids);
				}else{
					$("#hdnSubCat").val($("#hdnSubCat").val().replace($(this).attr("id"),""));
				}                
			}
		});		
		if(bid != "") { //alert(1);
            GetExtdInfo($('#ddlCategory').val(), true);
        }

        $("#btnSave").live('click',function(){
            //alert(NewRowCount);
            SaveRecord();
        });

        $("#btnUpdate").live('click',function(){            
            UpdateRecord();            
        });

        $("#btnCancel").live('click',function(){
            location.href="default.aspx";
        });

        $("#ddlCategory").change(function(){	
            $("#spnSubCat").html( "" );
			var categoryId = $(this).val();
			GetSubCategories(categoryId);
            AllowBikeSelection(categoryId);
            GetExtdInfo(categoryId, false);            
		});		
	});
	
    function GetSubCategories(categoryId) {
        ShowLoading("Loading Subcategories...");		
        $.ajax({
			type: "POST",
			url: "/ajaxpro/BikeWale.AjaxEditCms, BikewaleOpr.ashx",			
			data: '{"categoryId":"'+ categoryId + '"}',	
			beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetSubCategory"); },
			success: function(response) {
                   
				var response = eval('('+ response +')');						
				var obj_response = eval('(' + response.value + ')');
				if( obj_response.Table.length > 0 ){
					var str = "<ul>";
					for( var i = 0; i < obj_response.Table.length; i++ ){
						str += "<li><input id=\""+obj_response.Table[i].Id+"\" type=\"checkbox\" name=\"chk\" /><label for=\""+obj_response.Table[i].Id+"\">"+obj_response.Table[i].Name+"</label></li>";
					}
					str += " </ul> ";
					$("#subCatContainer").html(str);
				}else{
					$("#subCatContainer").html("");
				}
			}	
		});
        GB_hide();			
    }

    function AllowBikeSelection(categoryId) {
        ShowLoading("Checking properties...");		
        $.ajax({
			type: "POST",
			url: "/ajaxpro/BikeWale.AjaxEditCms, BikewaleOpr.ashx",			
			data: '{"categoryId":"'+ categoryId + '"}',	
			beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "AllowBikeSelection"); },
			success: function(response) {
                   
				var response = eval('('+ response +')');						
				var obj_response = eval('(' + response.value + ')');
				if( obj_response ) {
                    AllowBikeSelect = true;
                } else {
                    AllowBikeSelect = false;
                }
			}	
		});
        GB_hide();			
    }

    function GetExtdInfo(categoryId, isUpdateOpr) {
        CheckInfoExist(categoryId, isUpdateOpr);
    }

    function CheckInfoExist(categoryId, isUpdateOpr) {
        ShowLoading("Checking Extended Info...");	
        $.ajax({
			type: "POST",
			url: "/ajaxpro/BikeWale.AjaxEditCms, BikewaleOpr.ashx",			
			data: '{"categoryId":"'+ categoryId + '"}',	
			beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "CheckExtdInfoExist"); },
			success: function(response) {                   
				var responseObj = eval('('+ response +')'); 
                //alert(responseObj.value);              				
				var obj_response = eval('(' + responseObj.value + ')');
                DeleteRow();
                ExtdInfoLabel = "";
                ExtdInfoType = "";
                if(obj_response) {
                    //alert(responseObj.value);                    
                    CreateExtdInfoHtml(categoryId, isUpdateOpr);
                }
                else {
                    GB_hide();
                }
			}
		});
        
    }

    function CreateExtdInfoHtml(categoryId, isUpdateOpr) {
        ShowLoading("Creating Html...");	
        $.ajax({
			type: "POST",
			url: "/ajaxpro/BikeWale.AjaxEditCms, BikewaleOpr.ashx",			
			data: '{"categoryId":"'+ categoryId + '"}',	
			beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "FetchExtdInfo"); },
			success: function(response) { //alert(response);                   
				var responseObj = eval('('+ response +')');
                CreateHtml(responseObj.value, isUpdateOpr);
			}	
		});
    }

    function CreateHtml(value, isUpdateOpr)
    {
        var arrFields = value.split('|');        
        for( var i=0; i<arrFields.length; ++i) {
            var nameType = arrFields[i].split(',');           
            if( nameType != "" ){                
                if( nameType[2].toLowerCase().indexOf("textbox") >= 0 || nameType[2].toLowerCase().indexOf("checkbox") >= 0 ) { 
                    AddRow("input", nameType[1], nameType[2], nameType[0], isUpdateOpr);
                }
            }
        }
        GB_hide();
    }
     
    // Add a new row to the table specified(tblBasicInfo). Parameters are as follows:
    // element: The html element that needs to be created (usually "input" element)
    // label: This is the label that will be placed before the created element for identification.
    // type: This has a set of 2 values separated by an underscore('_'). The first part tells us what type of element needs to be created,
    //       eg. textbox. The second part tells us what sort of value goes into the element. They are :
    //            1 : Boolean Value
    //            2 : Numeric Value
    //            3 : Decimal Value
    //            4 : Text Value
    //            5 : DateTime Value
    //      So the type element would look like "textbox_4" indicating textbox element and Text Value
    // cfId: This tells us for which category the fields have been created. Reference table - Con_EditCms_CategoryFields
    function AddRow(element, label, type, cfId, isUpdateOpr) {    
        var table = document.getElementById("tblBasicInfo"); 
        var rowCount = table.rows.length;               
        var row = table.insertRow(rowCount);        
        var cell1 = row.insertCell(0);        
        cell1.innerHTML = label;
 
        var cell2 = row.insertCell(1);
        var element1 = document.createElement("input");      
        
        ExtdInfoType = type.split('_')[0];
        element1.type = ExtdInfoType;
        element1.size = "56";
        if( ExtdInfoType == "textbox" ) {
            ExtdInfoLabel = "txt" + label.replace(/ /g, "_").toLowerCase();        
        } else {
            ExtdInfoLabel = "chk" + label.replace(/ /g, "_").toLowerCase();        
        }
        element1.id = ExtdInfoLabel;
        element1.title = cfId + "_" + type.split('_')[1];        
        cell2.appendChild(element1);
        NewRowCount++;
        if(isUpdateOpr) {
            FetchExtdInfoData(bid, type.split('_')[1], cfId, ExtdInfoLabel);
        }
        $("#hdnExtdInfoIdVals").val($("#hdnExtdInfoIdVals").val() + ExtdInfoLabel + '|');
        $("#hdnExtdInfoValTypes").val($("#hdnExtdInfoValTypes").val() + type.split('_')[1] + '|');
    }

    // Delete a row from the table specified(tblBasicInfo)
    function DeleteRow() {
        try {
            var table = document.getElementById("tblBasicInfo"); 
            var rowCount = table.rows.length;                         
            if( rowCount > 4 ) {
                while (rowCount > 4) {
                    var row = table.rows[rowCount - 1];                
                    table.deleteRow(rowCount - 1);
                    NewRowCount--;
                    rowCount--;                    
                }     
            }
        } catch(e) {
            //alert(e);
        }
    }

    // Splash screen effect.
    function ShowLoading(text) {  
       var caption = "";
       var url = "";
       var applyIframe = false;
       var GB_Html = "<div style=\"width:100%;\"><div style=\"margin:10px auto 0 auto;vertical-align:middle;text-align:center;\"><img id=\"loading\" src=\"http://img.carwale.com/loader.gif\"/><br/><b>"+ text +"</b></div></div>";

       GB_show(caption, url, 200, 100, applyIframe, GB_Html);
    }

    function FetchExtdInfoData(basicId, valType, cfId, elemId) {
        ShowLoading("Getting Extended Info Data...");
        $.ajax({
			type: "POST",
			url: "/ajaxpro/BikeWale.AjaxEditCms, BikewaleOpr.ashx",			
			data: '{"basicId":"'+ basicId + '", "valType":"'+ valType + '", "cfId":"'+ cfId +'"}',	
			beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "FetchExtdInfoData"); },
			success: function(response) { // alert(response);               
                GB_hide();
				var responseObj = eval('('+ response +')');
                var str = responseObj.value.split('|');
                //alert($("#hdnExtdInfoId").val());
                if( str[0] != "")
                {
                    $("#hdnExtdInfoId").val($("#hdnExtdInfoId").val() + str[0] + '|');
                    $("#"+elemId).val(str[1]);
                }
                //alert($("#hdnExtdInfoId").val());
			}	
		});
    }
	function UpdateRecord()
	{
        var _extdInfoId = "";
        if (ExtdInfoLabel != "") {
            if($("#"+ExtdInfoLabel).val() != "") {
                if ($("#hdnExtdInfoId").val() == ""){
                    _extdInfoId = "0";
                } else {
                    _extdInfoId = $("#hdnExtdInfoId").val();
                }
            }
        }        
		if (Validate())
		{
            CleanHtml("rteDescription_txtContent");
			var _id = $("#hdnBasicId").val();
			var _title = $("#txtTitle").val().trim();		
			var tmp = _title.replace(/[^a-zA-Z 0-9]+/g,'').replace(/\s+/g,' ');			
			//var _url = tmp.replace(/ /g, "-").toLowerCase(); // URL will not be changed in the Update operation
			//var _authorName = $("#txtAuthor").val();
			var _authorName = $("#ddlAuthor option:selected").text();
			var _authorId = $("#ddlAuthor").val();//-1;
			var _description = tinyMCE.get("rteDescription_txtContent").getContent().replace(/"/g, "'").replace(/<p>&nbsp;<\/p>/g,'');
            //alert(_description);
			var _displayDate = $("#dtDate_txtYear").val() + "-" + $("#dtDate_cmbMonth").val() + "-" + $("#dtDate_cmbDay").val()+ "-" + $("#ddlHours").val()+ "-" + $("#ddlMins").val();
			var _subCatId = $("#hdnSubCat").val();
            var tempLabel = $("#hdnExtdInfoIdVals").val().split('|');
            var _value = "";
            var _cfId = "";
            var _valType = $("#hdnExtdInfoValTypes").val();
            for(var i = 0; i < tempLabel.length-1; ++i){            
                _value += $("#"+tempLabel[i]).val() + "|"; 
                _cfId += $("#"+tempLabel[i]).attr("title").split('_')[0] + "|"; 
            }
            var _extdInfoId = $("#hdnExtdInfoId").val();
            var _isFeatured = $("#chkIsFeatured").is(":checked");
            //alert($("#chkIsFeatured").is(":checked"));

			ShowLoading("Updating data...");
			$.ajax({
				type: "POST",
				url: "/ajaxpro/BikeWale.AjaxEditCms, BikewaleOpr.ashx",			
				data: '{"id":"'+ _id +'", "title":"'+ _title +'", "authorName":"'+ _authorName +'", "authorId":"'+ _authorId  +'","description":"'+ _description +'", "displayDate":"'+ _displayDate +'", "subCatId":"'+ _subCatId  +'", "cfId":"'+ _cfId +'", "valType":"'+ _valType +'", "value":"'+ _value +'", "extdInfoId":"'+ _extdInfoId + '", "isFeatured":"' + _isFeatured  + '"}',	
				beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateBasicInfo"); },
				success: function(response) { 
                    GB_hide();
                    var status = eval('('+ response +')');	
					if(status.value == true)
					{							
                        $("#alertObj").show();
                        $("#alertObj").text("Record updated successfully!");                        
					}		
                    else
					{							
                        $("#alertObj").show();
                        $("#alertObj").text("Record could not be updated!");
					}			
				}
			});
		}
	}                                       
	
    function SaveRecord()
    { 
        if(Validate())
        {
            CleanHtml("rteDescription_txtContent");
            var _categoryId = $("#ddlCategory").val();
            var _title = $("#txtTitle").val().trim();
			var tmp = _title.replace(/[^a-zA-Z 0-9]+/g,'').replace(/\s+/g,' ');			
			var _url = tmp.replace(/ /g, "-").toLowerCase();
//			var _authorName = $("#txtAuthor").val();
			var _authorName = $("#ddlAuthor option:selected").text();
			var _authorId = $("#ddlAuthor").val();//-1;
			var _description = tinyMCE.get("rteDescription_txtContent").getContent().replace(/"/g, "'").replace(/<p>&nbsp;<\/p>/g,'');
//            alert(_description);
			var _displayDate = $("#dtDate_txtYear").val() + "-" + $("#dtDate_cmbMonth").val() + "-" + $("#dtDate_cmbDay").val()+ "-" + $("#ddlHours").val()+ "-" + $("#ddlMins").val();
			var _subCatId = $("#hdnSubCat").val();
//            var _cfId = ExtdInfoLabel == "" ? "" : $("#"+ExtdInfoLabel).attr("title").split('_')[0];
//            var _valType = ExtdInfoLabel == "" ? "" : $("#"+ExtdInfoLabel).attr("title").split('_')[1];
//            var _value = ExtdInfoLabel == "" ? "" : $("#"+ExtdInfoLabel).val();
            //alert(_value); 
            var tempLabel = $("#hdnExtdInfoIdVals").val().split('|');
            var _value = "";
            var _cfId = "";
            var _valType = $("#hdnExtdInfoValTypes").val();
            for(var i = 0; i < tempLabel.length-1; ++i){            
                _value += $("#"+tempLabel[i]).val() + "|"; 
                _cfId += $("#"+tempLabel[i]).attr("title").split('_')[0] + "|"; 
            } 
            var _isFeatured = $("#chkIsFeatured").is(":checked");

			ShowLoading("Saving data...");
			$.ajax({
				type: "POST",
				url: "/ajaxpro/BikeWale.AjaxEditCms, BikewaleOpr.ashx",			
				data: '{"categoryId":"'+ _categoryId +'", "title":"'+ _title  +'", "url":"'+ _url +'", "authorName":"'+ _authorName +'", "authorId":"'+ _authorId  +'","description":"'+ _description +'", "displayDate":"'+ _displayDate +'", "subCatId":"'+ _subCatId  +'", "cfId":"'+ _cfId +'", "valType":"'+ _valType +'", "value":"'+ _value + '", "isFeatured":"' + _isFeatured  + '"}',	
				beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SaveBasicInfo"); },
				success: function(response) {//alert(response);
                    GB_hide();
                    var id = eval('('+ response +')');	
					if(id.value > 0)
					{
                        if( AllowBikeSelect ) {
                            window.location.href = "SelectBikes.aspx?bid=" + id.value;                      
                        } else {
                            window.location.href = "createalbum.aspx?bid=" + id.value;
                        }
					}		
                    else
					{							
                        $("#alertObj").show();
                        $("#alertObj").text("Record could not be saved!");
					}			
				}
			});
        }
    }

    function SaveExtdInfo( basicId, value, type, idValType )
    {
        ShowLoading("Saving Extended Info...");
        var cfId = idValType.split('_')[0];
        var valType = idValType.split('_')[1];
        $.ajax({
			type: "POST",
			url: "/ajaxpro/BikeWale.AjaxEditCms, BikewaleOpr.ashx",			
			data: '{"basicId":"'+ basicId +'", "value":"'+ value +'", "type":"'+ type  +'", "cfId":"'+ cfId +'", "valType":"'+ valType +'"}',	
			beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SaveExtdInfo"); },
			success: function(response) {
                GB_hide();
            }
        });
    }

</script>	
</div>
<!-- #Include file="/includes/footerNew.aspx" -->

