<%@ Page Language="C#" ContentType="text/html" Inherits="MobileWeb.Forums.PagewisePosts" ResponseEncoding="iso-8859-1" %>
<%@ Register TagPrefix="uc" TagName="PagePosts" src="/m/Controls/PagePosts.ascx" %>
<uc:PagePosts id="ucPagePosts" runat="server" />	
<script language="javascript" type="text/javascript">
    Common.showCityPopup = false;
	$(document).ready(function(){
		$("#divLoader").hide();
		$("#pagesContainer div[type='page']").hide();
		$("#pagesContainer div[id='page"+ selPage +"']").show();
	});
</script>
