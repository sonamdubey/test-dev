<%@ Page Language="C#"  %>
<%@ Register TagPrefix="Vspl" TagName="RTE" src="/Controls/RichTextEditor.ascx" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
<!-- #include file="/includes/global/head-script.aspx" -->
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	DocumentState 	= "Static";
    AdId            = "1397024466973";
    AdPath          = "/1017752/Carwale_Forums_";
%>
</head>
<style>
    .ac {padding:3px;}
    .iac {padding:3px;}
    .message p { padding:5px 0; }
    .msgBox ul { margin:10px 0 10px 10px; } 
    .msgBox .message { width:570px; overflow:auto; }
    .readable ul li { list-style-type:square; } 
.readable img{border:none;background-color:#FFFFFF;vertical-align:middle;}
</style>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color" >
    <!-- #include file="/includes/header.aspx" -->
    <section class="container padding-bottom30">
        <div class="grid-12">
            <div class="breadcrumb margin-bottom10">
                <!-- breadcrumb code starts here -->
                <ul>
                    <li><a href="/">Home</a></li>
                    <li><span class="fa fa-angle-right margin-right10"></span><a href="/community/">Community</a></li>
                    <li><span class="fa fa-angle-right margin-right10"></span><a href="/forums/">Forums</a></li>
                    <li><span class="fa fa-angle-right margin-right10"></span>Message</li>
                </ul>
                <div class="clear"></div>
            </div>
            <div id="divThread" style="float:left;"  >
		        <h1 class="font30 text-black special-skin-text">Moderation Message</h1>
                <div class="border-solid-bottom margin-top5 margin-bottom10"></div>
                <p class="text-black"> Your post is in moderation. Please check again after 1 hour. </p>
                <p class="text-black"> Inconvenience is deeply regretted.</p>
		        <div class="footerStrip" id="divStripTop" Visible="false" align="right" ></div>
		        <div class="footerStrip" id="divStrip" Visible="false" align="right" ></div>
		        <!-- Do not delete the following blank link -->
	        </div> 
            <div class="clear"></div>
        </div>
        <div class="clear"></div>
    </section>
    <!-- #include file="/includes/footer.aspx" -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
    <script type="text/javascript">
    Common.showCityPopup = false;
</script> 
</body>
</html>