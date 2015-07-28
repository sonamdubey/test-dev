<%@ Page AutoEventWireUp="false" Inherits="BikeWaleOpr.EditCms.ViewImage" Language="C#" Trace="false" Debug="false" %>
<%@ Import Namespace="BikeWaleOpr.Common" %>
<%@ Import Namespace="BikeWaleOpr" %>
<style type="text/css">
	.lp{text-align:center;}
	.cap{text-align:center;font-weight:bold;}
</style>
<html>
	<form>
		<div class="lp"><img src='<%= showImage %>' /></div>
		<%--<div class="cap"><%=caption%></div>--%>
	</form>
</html>