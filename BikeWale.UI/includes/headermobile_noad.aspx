<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
	<meta name="keywords" content="<%= keywords %>" />
    <meta name="description" content="<%= description %>" />
    <meta name="canonical" content="<%= canonical %>" />
    <title><%= title %></title>
    <script language="c#" runat="server">	    
	    private string title = "", description = "", keywords = "", AdId = "", AdPath = "", canonical = "",menu="";
        private string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];        
        private string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
    </script> 
   <!-- #include file="\includes\globalStaticFiles_mobile.aspx" -->    
</head>
<body>
    <form runat="server">
    <!-- #include file="/includes/gacode.aspx" --> 
    <div data-role="page" style="position:relative;">
        <div role="main">
        	<!-- Header code starts here-->
            <!-- #include file="/includes/headBW_Mobile.aspx" --> 
            <!-- Header code ends here-->
            <!-- inner-section code starts here-->
            <div class="inner-section">
