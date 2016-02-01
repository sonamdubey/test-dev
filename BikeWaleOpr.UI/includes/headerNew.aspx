<%@Import namespace="BikeWaleOpr.Common"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
<script src="/src/jquery-1.6.min.js" type="text/javascript"></script>
<script src="/src/AjaxFunctions.js" type="text/javascript"></script>
<title>BikeWale Operations</title>
</head>
<body>
    <form runat="server">
	<div class="content">
			<div class="header">
				<div class="top_info_right">
					<p align="right" style="padding-right:10px;vertical-align:top" >
						<%--<a href="/users/ChangePswd.aspx">Change Password</a> |--%>
						<a href="/common/logout.aspx?logout=logout">Logout</a>
					</p>
				</div>
				
				<div class="logo">
					<h1><a href="/default.aspx" title="Centralized Internet Services"><span class="dark">BikeWale</span>Operations</a></h1>
				</div>
					
				<div class="top_info_left">
					&nbsp;&nbsp;&nbsp;Welcome <%= CurrentUser.UserName %><br />
				</div>
				
				<div class="top_info_right">
							<p align="right" style="padding-right:10px;vertical-align:top" >
							<b><%=DateTime.Today.ToString("dd MMMM yyyy")%></b> - <%=DateTime.Today.ToString("dddd")%>
							</p>
				</div>
			</div>
			
			<div class="bar" style="width:100%;">
				<ul>
					<li><a href="/content/default.aspx" accesskey="p">Contents</a></li>
<%--					<li><a href="/editcms/default.aspx" accesskey="e">EditCMS</a></li>--%>
                    <li><a href="/classified/default.aspx" accesskey="e">Classified</a></li>
                    <li><a href="/newbikebooking/default.aspx" accesskey="e">Manage Dealers</a></li>
                    <li><a href="/newbikebooking/DealerDetailedPriceQuote.aspx" accesskey="e">Dealer Detail</a></li>
                    <li><a href="/content/frmManageAppVersions.aspx" accesskey="e">Manage App Version</a></li>
                </ul>
			</div>
	

