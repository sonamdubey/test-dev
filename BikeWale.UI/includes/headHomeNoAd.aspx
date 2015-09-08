<%@ Register TagPrefix="BikeWale" TagName="LoginStatus" src="/Controls/loginstatus.ascx" %>
<%@ Register TagPrefix="BM" TagName="BikeMakes" Src="/controls/BrowseBikeManufacturerMin.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:fb="http://ogp.me/ns/fb#">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />    
    <meta name="keywords" content="<%= keywords %>" />
    <meta name="description" content="<%= description %>" />
    <title><%= title %></title>
   <!-- #include file="globalStaticFiles.aspx"-->
    <script language="c#" runat="server">	    
	    private string title = "", description = "", keywords = "";
        private bool isHeaderFix = true;	    
    </script>        
    <script type="text/javascript">
        //facebook like button script
        ( function ( d, s, id ) {
            var js, fjs = d.getElementsByTagName( s )[0];
            if ( d.getElementById( id ) ) return;
            js = d.createElement( s ); js.id = id;
            js.src = "//connect.facebook.net/en_US/all.js#xfbml=1";
            fjs.parentNode.insertBefore( js, fjs );
        }( document, 'script', 'facebook-jssdk' ) );

        //Twitter Tweet Button script
        !function ( d, s, id ) { var js, fjs = d.getElementsByTagName( s )[0]; if ( !d.getElementById( id ) ) { js = d.createElement( s ); js.id = id; js.src = "https://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore( js, fjs ); } }( document, "script", "twitter-wjs" );

        //Google +1 Button script
        ( function () {
            var po = document.createElement( 'script' ); po.type = 'text/javascript'; po.async = true;
            po.src = 'https://apis.google.com/js/plusone.js';
            var s = document.getElementsByTagName( 'script' )[0]; s.parentNode.insertBefore( po, s );
        } )();
    </script>
</head>
<body class="header-fixed-inner">
    <form runat="server">
    <!-- #include file="/includes/gacode.aspx" --> 
	<div class="main-container">
    	<!-- #include file="/includes/headBW.aspx" -->  
        