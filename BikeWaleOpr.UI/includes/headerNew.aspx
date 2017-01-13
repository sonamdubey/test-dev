<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="stylesheet" href="/css/common.css?03102016" type="text/css" />
<link rel="stylesheet" href="/css/chosen.min.css?03102016" />
<script src="https://st1.aeplcdn.com/bikewale/min/src/frameworks.js?09Jan2017v1" type="text/javascript"></script>
<script src="/src/AjaxFunctions.js?03102016" type="text/javascript"></script>
<script type="text/javascript" src="/src/common/common.js?03102016"></script>
<script type="text/javascript" src="https://st2.aeplcdn.com/bikewale/src/common/chosen.jquery.min.js?v15416"></script>
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
				    Welcome <%= BikeWaleOpr.Common.CurrentUser.UserName %>
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
	    <div class='toast' style='display:none'></div>

