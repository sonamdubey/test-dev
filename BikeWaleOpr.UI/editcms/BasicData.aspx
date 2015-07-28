<%@ Page AutoEventWireUp="false" Language="C#" Inherits="BikeWaleOpr.EditCms.BasicData" %>
<script language="javascript" src="http://opr.carwale.com/src/common/bt.js"></script>
<!--[if IE]><script language="javascript" src="http://opr.carwale.com/src/common/excanvas.js"></script><![endif]-->
<h3><%=Request.QueryString["cat"].ToString().Replace("_"," ")%></h3>
<table id="tblBasicData" style="width:600px;" class="lstTable">
	<tr class="lstTableHeader">
		<td width="250px">Title</td>
		<td width="100px">Display Date</td>
		<td width="150px">Author</td>
		<td width="100px">&nbsp;</td>
	</tr>
	<asp:Repeater ID="rptBasicData" runat="server">
		<itemtemplate>
			<tr id="tblBasicDataRow<%# DataBinder.Eval( Container.DataItem, "ID" ) %>">
				<td id="tdTitle<%# DataBinder.Eval( Container.DataItem, "ID" ) %>" width="350px" style="border-bottom:1px solid #6C757A;">
					<span style="cursor:pointer;text-decoration:underline;color:blue;" class="title" basicId="<%# DataBinder.Eval( Container.DataItem, "ID" ) %>" id="spnTitle<%# DataBinder.Eval( Container.DataItem, "ID" ) %>"><%# DataBinder.Eval( Container.DataItem, "Title" ) %></span>
					<span style="display:none;" id="spnDescription<%# DataBinder.Eval( Container.DataItem, "ID" ) %>"><%# DataBinder.Eval( Container.DataItem, "Description" ) %></span>
				</td>
				<td id="tdDisplayDate<%# DataBinder.Eval( Container.DataItem, "ID" ) %>" width="100px" style="border-left:1px solid #6C757A;border-bottom:1px solid #6C757A;"><%# DataBinder.Eval( Container.DataItem, "DisplayDate" ) %></td>
				<td id="tdAuthorName<%# DataBinder.Eval( Container.DataItem, "ID" ) %>" width="150px" style="border-left:1px solid #6C757A;border-bottom:1px solid #6C757A;"><%# DataBinder.Eval( Container.DataItem, "AuthorName" ) %></td>
				<td width="100px" style="border-left:1px solid #6C757A;border-bottom:1px solid #6C757A;"><a class="edit" basicId="<%# DataBinder.Eval( Container.DataItem, "ID" ) %>" style="cursor:pointer;" >Action</a></td>
			</tr></itemtemplate>
	</asp:Repeater>
</table>
<div id="divHey" style="display:none;">
	<a id="aEditBasicInfo" style="cursor:pointer;text-decoration:none;">Edit basic info</a><br/>
	<a id="aViewSelectedBikes" style="cursor:pointer;text-decoration:none;">View selected Bikes</a><br/>
	<a id="aViewOtherInfo" style="cursor:pointer;text-decoration:none;">View other info</a><br/>
	<a id="aViewAlbum" style="cursor:pointer;text-decoration:none;">View album</a><br/>
	<a id="aViewPages" style="cursor:pointer;text-decoration:none;">View pages</a>
</div>
<script language="javascript" type="text/javascript">
	function EditRecord(id)
	{
		$("#txtTitle").val($("#spnTitle" + id).html());
		$("#txtDescription").val($("#spnDescription" + id).html());
		$("#txtAuthorName").val($("#tdAuthorName" + id).html());
		var displayDate = $("#tdDisplayDate" + id).html();
		var dDate = displayDate.split("/");
		$("#dtDate_cmbDay").val(dDate[0]);
		if (parseInt(dDate[0]) > 9)
			$("#dtDate_cmbDay").val(dDate[0]);
		else
			$("#dtDate_cmbDay").val(dDate[0].toString().charAt(1));
		if (parseInt(dDate[1]) > 9)
			$("#dtDate_cmbMonth").val(dDate[1]);
		else
			$("#dtDate_cmbMonth").val(dDate[1].toString().charAt(1));	
		$("#dtDate_txtYear").val(dDate[2]);
		
		$("#btnSave").hide();
		$("#btnUpdate").show();
		$("#btnCancel").show();
		//$("#aSelectedBikes").show();
		//$("#aSelectedBikes").attr("href","SelectBikes.aspx?bid=" + id + "&cid=" + $("#ddlCategory").val());
		
		if ($("#hdnBasicId").val() != "-1")
		{
			$("#tblBasicDataRow" + $("#hdnBasicId").val()).removeAttr("style");
			$(editClicked).parent().parent().attr("style","background-color:#FFCC66;");
		}
		else
		{
			$(editClicked).parent().parent().attr("style","background-color:#FFCC66;");
		}
		
		$("#hdnBasicId").val(id);
	}
	
	function CancelUpdate()
	{
		$("#txtTitle").val("");
		$("#txtDescription").val("");
		$("#txtAuthorName").val("");
		
		$("#dtDate_cmbDay").val($("#hdnCurrentDay").val());
		$("#dtDate_cmbMonth").val($("#hdnCurrentMonth").val());
		$("#dtDate_txtYear").val($("#hdnCurrentYear").val());
	
		$("#btnSave").show();
		$("#btnUpdate").hide();
		$("#btnCancel").hide();	
		//$("#aSelectedBikes").hide();
		
		$("#tblBasicDataRow" + $("#hdnBasicId").val()).removeAttr("style");
		$("#hdnBasicId").val("-1");
	}
	
	$(document).ready(function(){
		$("#tblBasicInfo input,textarea,select").removeAttr("disabled");
		
		
		$("span.title").bt({contentSelector: "$('#spnDescription' + $(this).attr('basicId')).html()", fill: '#fff',strokeWidth: 1,strokeStyle: '#79B7E7',spikeLength: 10,
			cssStyles: {fontSize: '11px'},width:'320px',trigger:['mouseover','none'],
			preShow: function (box){ 
				$(".bt-wrapper ").hide(); 
			},			 	
			showTip: function(box){	
				$(box).show();	
			},	 		
			hoverIntentOpts: {	interval: 0,timeout: 0}
		});		
		
		$("a.edit").bt({contentSelector: "$('#divHey').html()", fill: '#fff',strokeWidth: 1,strokeStyle: '#79B7E7',spikeLength: 10,
			cssStyles: {fontSize: '11px'},width:'120px',trigger:['mouseover','none'], positions: 'right',
			preShow: function (box){ 
				$(".bt-wrapper ").hide(); 
			},			 	
			showTip: function(box){	
				boxClicked = box;
				$(box).show();	
				editClicked = this;
				var bIdEdit = $(editClicked).attr("basicId");
				$(box).find("#aViewSelectedBikes").attr("href","SelectBikes.aspx?bid=" + bIdEdit);
				$(box).find("#aViewOtherInfo").attr("href","OtherInfo.aspx?bid=" + bIdEdit);
				$(box).find("#aViewAlbum").attr("href","CreateAlbum.aspx?bid=" + bIdEdit);
				$(box).find("#aViewPages").attr("href","AddPages.aspx?bid=" + bIdEdit);
				$(box).find("#aEditBasicInfo").attr("onClick","javascript:EditRecord('"+ bIdEdit +"');HideBox();");
			},	 		
			hoverIntentOpts: {	interval: 0,timeout: 0}
		});		

	});
	
	function HideBox()
	{
		$(boxClicked).hide(); 	
	}
</script>