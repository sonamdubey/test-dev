<%@ Page Inherits="BikeWaleOpr.Content.VersionPhotos" AutoEventWireUp="false" Language="C#" Trace="false" Debug="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <script src="/src/jquery-1.6.min.js" type="text/javascript"></script>
    <style type="text/css">
	    .doNotDisplay { display:none; }
	    td, tr, table { border-color:white; }
	    .panel { background-color:#FFF0E1; border:1px solid orange;padding:5px; }
	    #versionTable { border-collapse:collapse;  }
	    #versionTable td, #versionTable th { border:1px solid #dddddd; padding:4px; }
    </style>
</head>
<body>
<div class="left">
	<form runat="server">
		<h3>Version Photos <asp:Label ID="lblBike" CssClass="errorMessage" runat="server"></asp:Label> </h3>
		<span id="spnError" class="errorMessage" runat="server"></span><br />
		<asp:Repeater ID="rptFeatures" runat="server" >
			<headertemplate><br />
				<table border="0" id="versionTable">
					<tr>
						<th>&nbsp;</th>
						<th>Version Name</th>
						<th>Original Pic</th>
						<th>Model Photo</th>
					</tr>
			</headertemplate>
			<itemtemplate>
					<tr>
						<td>
							<asp:CheckBox id="chkUpload" runat="server" /> 
							<asp:Literal ID="ltId" Visible="false" Text='<%# DataBinder.Eval( Container.DataItem, "id" ) %>' runat="server" />
						</td>
						<td style="color:#cc0000;font-weight:bold;padding-right:15px;"><%# DataBinder.Eval( Container.DataItem, "Name" ) %></td>
						<td>
                            <div id ="div1">
                                <img id="imgSmall" class='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsReplicated")) ? "": "checkImage" %>'  image-id="<%= verId%>" src='<%# !String.IsNullOrEmpty(DataBinder.Eval( Container.DataItem, "OriginalImagePath" ).ToString()) ? BikeWaleOpr.ImagingOperations.GetPathToShowImages(DataBinder.Eval( Container.DataItem, "HostURL").ToString(),"227X128" , DataBinder.Eval( Container.DataItem, "OriginalImagePath" ).ToString()) : "http://img.carwale.com/bikewaleimg/common/nobike.jpg"%>'/>
                            </div>
                        </td>
						<td><input type="radio" id="optModel" value="<%# DataBinder.Eval( Container.DataItem, "id" ) %>" <%# DataBinder.Eval( Container.DataItem, "SmallPic" ).ToString() == DataBinder.Eval( Container.DataItem, "ModelSmall" ).ToString() ? "checked" : "" %> name="optModel" /></td>
					</tr>
			</itemtemplate>
			<footertemplate>
				</table>
			</footertemplate>
		</asp:Repeater><br>
		<br>
		<asp:Panel ID="pnlAdd" CssClass="panel" runat="server" Visible="false">
			<div style="padding-bottom:5px;font-weight:bold; color:#FF3300">Upload Photos</div>
			Large Pic <input type="file" id="filLarge" runat="server" /><br />
			<span id="err" class="error"></span>
			<input ID="chkAll" onClick="checkAll(this)" type="checkbox" /> Check All
			<asp:Button ID="btnSave" Text="Upload Photos" runat="server" />
			<asp:Button ID="btnUpdateModel" Text="Update Model Photo" runat="server" />
		</asp:Panel>
	</form>
</div>
<script type="text/javascript">
    var refreshTime = 2000;

	function checkAll( chk )
	{
		var chks = document.getElementById( 'versionTable' ).getElementsByTagName( 'input' );
		
		if ( chks.length && chks.length > 0 )
		{
			for ( var i=0; i<chks.length; i++ )
			{
				if ( chk.checked ) chks[i].checked = true;
				else  chks[i].checked = false;
			}
		}
	}
	
	function checkFind( e )
	{
		if ( document.getElementById('cmbModel').options[0].selected ) 
		{
			document.getElementById('selectModel').innerHTML = "Select Model First"; 
			document.getElementById('cmbModel').focus();
			return false;
		}
		else document.getElementById('selectModel').innerHTML = ""; 
	}
	
	function checkSave( e )
	{
		if ( document.getElementById('filSmall').value == '' && document.getElementById('filLarge').value == '' ) 
		{
			document.getElementById('err').innerHTML = "Small Picture and/or Large Picture path is a must.<br>"; 
			return false;
		}
		else document.getElementById('err').innerHTML = ""; 
		
		var inp = document.getElementById('versionTable').getElementsByTagName('input');
		
		var isSelected = false;
		
		for ( var i=0; i<inp.length; i++ )
		{
			if ( inp[i].checked ) isSelected = true;
		}
		
		if ( !isSelected && !document.getElementById('chkAll').checked )
		{
			document.getElementById('err').innerHTML = "At least one Version Should be checked or check 'Upload For All Versions'.<br>"; 
			return false;
		}
		else document.getElementById('err').innerHTML = ""; 
	}
	
	function checkUpdateModel()
	{
		var opts = document.getElementById("versionTable").getElementsByTagName("input");
		var checked = 0;
		
		for ( var i = 0; i < opts.length; i++ )
		{
			if ( opts[i].id = "optModel" && opts[i].checked )
			{
				checked++; 
			}
		}
		
		if ( checked == 0 )
		{
			alert( "Please choose one photo to be updated as Model Photo!" )
			return false;
		}
	}
	
	if ( document.getElementById('btnSave') )
	{
		document.getElementById('btnSave').onclick = checkSave;
		document.getElementById('btnUpdateModel').onclick = checkUpdateModel;
	}


	function UpdatePendingMainImage() {
	    var event = $(".checkImage");
	    var id = event.attr('image-id');
	    //alert(id);
	    CheckMainImageStatus(event, id);
	}

	function CheckMainImageStatus(event, mainImageId) {
	    var category = 'BIKEVERSION';
	    if (mainImageId != undefined) {
	        $.ajax({
	            type: "POST", url: "/AjaxPro/BikeWaleOpr.Common.Ajax.ImageReplication,BikewaleOpr.ashx",
	            data: '{"imageId":"' + mainImageId + '","Category":"' + category + '"}',
	            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "CheckImageStatusByCategory"); },
	            success: function (response) {
	                var ret_response = eval('(' + response + ')');
	                //alert(ret_response.value);
	                var obj_response = eval('(' + ret_response.value + ')');
	                if (obj_response.Table.length > 0) {
	                    for (var i = 0; i < obj_response.Table.length; i++) {
	                        var imgUrlLarge = obj_response.Table[i].HostUrl + "/310X174/" + obj_response.Table[i].OriginalImagePath;

	                        event.attr('src', imgUrlLarge);
	                    }

	                }
	            }
	        });
	    }
	}

	$(document).ready(function () {
	    setInterval(UpdatePendingMainImage, refreshTime)
	});
 </script>
</body>
</html>
