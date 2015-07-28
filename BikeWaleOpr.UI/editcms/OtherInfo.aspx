<%@ Page AutoEventWireUp="false" Inherits="BikeWaleOpr.EditCms.OtherInfo" Language="C#" Trace="false" debug="false" %>
<%@ Register TagPrefix="dt" TagName="DateControl" src="/Controls/DateControl.ascx" %>
<%@ Register TagPrefix="Uc" TagName="DispBasicInfo" src="/editcms/DisplayBasicInfo.ascx" %>
<%@ Register TagPrefix="Ec" TagName="EditCmsCommon" src="/editcms/EditCmsCommon.ascx" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<style>
	.errMessage {color:#FF4A4A;}
</style>
<div class="urh">
	<a href="/default.aspx">BikeWale operations</a> &raquo; <a href="/editcms/default.aspx">Editorial Home</a> &raquo; Manage Articles
</div>

<div style="clear:both;">
	<form runat="server">
		<div>
			<Ec:EditCmsCommon ID="EditCmsCommon" runat="server" />
		</div>
		<div id="divAshish" runat="server" style="float:left;width:525px;"></div>
		<div style="width:400px;float:right;display:none;">
				<Uc:DispBasicInfo ID="BasicInfo" runat="server" />
		</div>
		<div style="clear:both;margin-top:10px;">&nbsp;
			<asp:Label ID="lblIsSaved" runat="server" Text="0" CssClass="errMessage" style="display:none;" />
		</div>
<script language="javascript" type="text/javascript">
	
	function CheckNumeric(event, txt)
	{
		if ( event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 37 || event.keyCode == 39) {
        }
        else {
                if (event.keyCode < 48 || event.keyCode > 57 ) {
                        event.preventDefault(); 
                }       
        }
	}
	
	function CheckDecimal(event, txt)
	{
		var txtVal = $(txt).val();
		if ( event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 37 || event.keyCode == 39 || (event.keyCode == 190 && txtVal.indexOf(".") == -1)) {
        }
        else {
                if (event.keyCode < 48 || event.keyCode > 57 ) {
                        event.preventDefault(); 
                }       
        }
	}
</script>
	<div style="min-height:200px;">&nbsp;</div>
	</form>
</div>
<!-- #Include file="/includes/footerNew.aspx" -->


