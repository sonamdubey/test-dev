<%@ Page trace="false" Inherits="Carwale.UI.Users.Login" AutoEventWireUp="false" Language="C#" %>
<%@ Register TagPrefix="Carwale" TagName="Register" src="/Controls/RegisterControl.ascx" %>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 2;
	Title 			= "Member Registration";
	Description 	= "CarWale.com Member Login";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
    AdId            = "1396439626285";
    AdPath          = "/1017752/Homepage_";
%>
<!-- #include file="/includes/headUsers.aspx" -->
<style type="text/css">
    .newblue-block{background-color:#f4f3f3; border:1px solid #e5e5e5; padding:5px; }
</style>
<form runat="server">
    <div class="column grid_8 margin-top10">
	    <div class="content-block-white">		    
			<div id="tb2"><Carwale:Register id="ctlRegister" runat="server" /></div>		    
	    </div><!-- content eends -->
    </div>
    <div class="column grid_4 margin-top10">
        <div class="content-block-white content-inner-block">            
	        <h2 class="hd2">Dealers Affiliation Program</h2>
            <%--<div class="clear"></div>--%>
            <div class="margin-top5 grey-text">Want to join the growing CarWale dealership network.</div>
            <div class="margin-top5 grey-text">It is easy and fast!</div>
            <br />
            <a href="/dealer/dealerregister.aspx" class="action-button grey-btn" title="Register Now"><b>Register Now</b></a>            
        </div>
    </div>
</form>	
<script language="javascript" type="text/javascript">
	function switchTab(index)
	{
		var minIndex = 1;
		var maxIndex = 2;
		var liInit = "liTabBot";
		var divInit = "divTabBot";
		
		for(var i = minIndex; i <= maxIndex; i++)
		{
			if(i != index)
			{
				document.getElementById(liInit + i).className = "";
				document.getElementById(divInit + i).style.display = "none";
				
				var childs = document.getElementById(liInit + i).getElementsByTagName("a");
				
				if(childs.length == 0)
				{
					document.getElementById(liInit + i).innerHTML = "<a onclick='switchTab(" + i + ")' style='cursor:pointer'>" + 
																		document.getElementById(liInit + i).innerHTML + "</a>";	
				}
				else
				{
					document.getElementById(liInit + i).innerHTML = "<a onclick='switchTab(" + i + ")' style='cursor:pointer'>" + 
																		childs[0].innerHTML + "</a>";	
				}
			}
			else
			{
				document.getElementById(liInit + i).className = "sel";
				document.getElementById(divInit + i).style.display = "";
				
				var childs = document.getElementById(liInit + i).getElementsByTagName("a");
				
				if(childs != 0)
				{
					document.getElementById(liInit + i).innerHTML = childs[0].innerHTML;	
				}
			}
		}
	}

</script>
<!-- Footer starts here -->
<!-- #include file="/includes/footer-old.aspx" -->
<!-- Footer ends here -->   