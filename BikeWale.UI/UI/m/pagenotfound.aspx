<%@ Page Language="C#" %>
<% Response.StatusCode = 404 ;%>
<!-- #include file="/includes/headermobile_noad.aspx" -->
<style>
	h1 
	{ 
		color:#003366; 
		font-size:28px;
		font-weight:bold;
		font-family:Verdana, Arial, Helvetica, sans-serif;
		border-bottom:2px solid orange; 
		margin:10px 10px 10px 0;
	}
	#main { padding:20px; }	
    .ul-hrz-col2 li { font-size:13px;color:#666666; background-image: url("https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/icon-sprite.png"); background-position: 0 -111px; background-repeat: no-repeat; display: block; list-style: square outside none; padding: 3px 0 3px 10px; }
</style>
    <div class="padding5">
        <h1>Page Not Found</h1>
        <h2 class="new-line15">Requested page couldn't be found on <a href="/" title="Visit BikeWale home page">BikeWale</a></h2>
		<h2 class="new-line10">Possible causes for this inconvenience are:</h2>
        <div class="padding5 ul-hrz-col2">
            <ul>
			    <li>The requested page might have been removed from server.</li>	
			    <li>The URL might be mis-typed by you.</li>
			    <li>Some maintenance process is going on the server.</li>
		    </ul>
        </div>		
		<div class="new-line10">Please try visiting the page again within few minutes.</div>
    </div>
<!-- #include file="/includes/footermobile_noad.aspx" -->
