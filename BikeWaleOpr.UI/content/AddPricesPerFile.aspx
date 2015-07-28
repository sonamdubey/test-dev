<%@ Page trace="false" Inherits="BikeWaleOpr.Content.AddPricesPerFile" AutoEventWireUp="false" Language="C#" %>
<html>
<body>
</body>
<script language="javascript" type="text/javascript">
	/**/var finished = <%= finished.ToString().ToLower() %>
	
	if(finished == false)
	{
		location.href = location.href;
	}
	else
	{
		parent.document.getElementById("divMsgUpdate").innerHTML = "All the prices has been added!";
		parent.document.getElementById("butMapUCBikes").disabled = false;
		parent.document.getElementById("butNewEntry").disabled = false;
	}
	
		
</script>
</html>
