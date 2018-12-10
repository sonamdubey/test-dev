<%@ Page Language="C#" ContentType="text/html" Inherits="MobileWeb.Forums.PagewiseThreads" ResponseEncoding="iso-8859-1" %>
<%@ Register TagPrefix="uc" TagName="PageThreads" src="/m/Controls/PageThreads.ascx" %>
<uc:PageThreads id="ucPageThreads" runat="server" />	
<script language="javascript" type="text/javascript">
    Common.showCityPopup = false;
	$(document).ready(function(){
		$("#divLoader").hide();
		$("#pagesContainer div[type='page']").hide();
		$("#pagesContainer div[id='page"+ selPage +"']").show();
	});
</script>