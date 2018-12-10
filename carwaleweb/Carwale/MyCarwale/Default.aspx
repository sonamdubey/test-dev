<%@ Page Language="C#" Inherits="Carwale.UI.MyCarwale.Default" AutoEventWireup="false" trace="false"%>
<%@ Import NameSpace="Carwale.UI.Common" %>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 7;
	Title 			= "My CarWale Home";
	Description 	= "CarWale.com forum section involves to view all the cars listed by you for sale and used car purchase request";
	Keywords		= "Car , inquiry , used car";
	Revisit 		= "15";
	DocumentState 	= "Static";
    canonical       = "https://www.carwale.com/mycarwale/";
    AdId            = "1336723206235";
    AdPath          = "/7590/CarWale_MyCarWale/CarWale_MyCarWale_";
%>
<!doctype html>
<html>
    <head>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <style type="text/css">
    
	    .paddLeft {padding-left:25px;}
	    .inq {padding:4px 2px 4px 2px;width:270px;height:120px; border-bottom:2px solid #555555;border-right:2px solid #999999;border-left:1px solid #AAAAAA;border-top:1px solid #AAAAAA;}
	
	    .upgrade{ border:8px solid #DDF0F8;}
	    .upgrade th{ background-color:#DDF0F8; text-align:left; vertical-align:middle; color:#006699; font-size:12px; font-weight:bold; height:20px; font-family:Verdana, Arial, Helvetica, sans-serif; padding-bottom:5px;}
	    .txtHl {color:#555555; font-size:12px; font-weight:bold;}
        .my-cw-sprite {
            background: url(https://img.carwale.com/newtemplate/my-cw-sprite.png) no-repeat;
            display: inline-block;
        }
        .manage-icon {
    background-position: 0 0px;
    width: 72px;
    height: 40px;
}
.view-icon {
    background-position: 0 -61px;
    width: 60px;
    height: 40px;
}
.garage-icon {
    background-position: 0 -120px;
    width: 60px;
    height: 40px;
}
    .link-list li span.arrow-grey {
        width: 6px;
        height: 5px;
        margin-right: 5px;
        position: relative;
        top: -1px;
    }
	.highlight-box li .link-list li span.arrow-grey {
        background-position: 0 -181px;
    }
    .highlight-box li .link-list li span.arrow-blue, .link-list li span.arrow-grey {
        width: 6px;
        height: 5px;
        margin-right: 5px;
        position: relative;
        top: -1px;
    }
    .highlight-box li .link-list li span.arrow-blue {
        background-position: 0 -171px;
    }    

    </style>
    <!-- Header ends here -->
    </head>
    <body class="bg-light-grey rsz-lyt">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <section class="margin-top70">
            <div class="container">
                <div class="grid-12">
                    <ul class="breadcrumb margin-top10 margin-bottom10">
                        <li><a href="/">Home</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span></li>
                        <li class="current"><strong> My CarWale</strong></li>
                    </ul>
                    <div class="clear"></div>
                    
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <section>
            <div class="container">
                
                <div class="grid-8 alpha omega">    
                            <div class="content-box-shadow content-inner-block-10 rounded-corner2 margin-bottom20 padding-bottom20">
                                <h1 class="font30 text-black special-skin-text">My Inquiries</h1>
                                <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                                    <div class="margin-top10">
	                                    <div>
    	                        <div class="highlight-box">
                                    <ul>
                                        <li class="grid-4 alpha">
                    	                        <span class="my-cw-sprite manage-icon leftfloat margin-right10"></span>
                                            <div class="title-place">
                    	                        <h3><a href="/mycarwale/myinquiries/mysellinquiry.aspx" class="text-black">Manage your car listing(s)</a></h3>
                                            </div>    
                                            <div class="clear"></div>
                                            <div class="link-list margin-top10">
                    	                        <ul>
                        	                        <li><span class="my-cw-sprite arrow-blue"></span><a href="/mycarwale/myinquiries/mysellinquiry.aspx">Modify your ad</a></li>
                                                    <li><span class="my-cw-sprite arrow-blue"></span><a href="/mycarwale/mypayments.aspx">View payments made</a></li>
                                                </ul>
                                            </div>
                                        </li>
                                    </ul>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            </div>
                            </div>
                            </div>
                       
                <div class="grid-4">    
                            <div class="content-box-shadow content-inner-block-10 rounded-corner2 margin-bottom20 padding-bottom20">
                                <h1 class="font30 text-black special-skin-text">In Community</h1>
                                <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                                <div class="white-shadow content-inner-block">
                                    <%--<div>
                                        <h2><a href="/community/photos/myphotos/">My Photos</a></h2>
				                        <div class="margin-top5">Upload and share your photos.</div>
                                    </div>--%>
                                    <div class="margin-top10">
                                        <h2><a href="/forums/">Forums</a></h2>
				                        <div class="margin-top5">Ask questions, or comment on a wide range of topics</div>
                                    </div>
	  	                        </div>
                            </div>
                        </div>
                <div class="clear"></div>
             </div>
            <div class="clear"></div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
    </form>
    </body>
</html>
