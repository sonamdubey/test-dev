<%@Import namespace="BikeWaleOpr.Common"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
<%--<link href="http://st2.aeplcdn.com/bikewale/css/chosen.min.css?v15416" rel="stylesheet" />--%>
    <link rel="stylesheet" href="/css/chosen.min.css" />
<script src="/src/jquery-1.6.min.js" type="text/javascript"></script>
<script src="/src/AjaxFunctions.js" type="text/javascript"></script>
<script src="/src/knockout.js" type="text/javascript"></script>
<script type="text/javascript" src="/src/common/common.js?V1.1"></script>
<script type="text/javascript" src="http://st2.aeplcdn.com/bikewale/src/common/chosen.jquery.min.js?v15416"></script>
<title>BikeWale Operations</title>
<style type="text/css">
    .chosen-container {padding: 8px;}
</style>
</head>
<body>
    <form runat="server">
	<div class="content">
		<div class="header">				
			<div class="logo floatLeft">
				<h1><a href="/default.aspx" title="Centralized Internet Services"><span class="dark">BikeWale</span>Operations</a></h1>
			</div>					
			
            <div class="floatRight">
                <span class="font13 text-bold margin-right20 verical-middle">
				    Welcome <%= CurrentUser.UserName %>
                </span>
                <a href="/common/logout.aspx?logout=logout" class="btn btn-default verical-middle">Logout</a>
            </div>
            <div class="clear"></div>
            <div class=""></div>			
		</div>
			
		<div class="bar" style="width:100%;">
			<ul>
				<li><a href="/content/default.aspx" accesskey="p">Contents</a></li>
                <li><a href="/classified/default.aspx" accesskey="e">Classified</a></li>
                <li><a href="/newbikebooking/default.aspx" accesskey="e">Manage Dealers</a></li>                    
            </ul>
		</div>
	

