<%@ Page Language="C#" Trace="false" %>

<!doctype html>
<html>
<head>
    <%
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,
        // check page, headerCommon.aspx in common folder.

        PageId = 2;
        Title = "Advertise with CarWale and reach your customers";
        Description = "Advertise your brands and products through CarWale. CarWale provides sophisticated tools for appropriate placing of your advertisement to target relavant customers. CarWale provides various inputs to track visitor's behavious for better marketing decisions.";
        Keywords = "";
        Revisit = "30";
        DocumentState = "Static";
        AdId = "1398233965520";
        AdPath = "/1017752/AboutUs_";
    %>

    <!-- #include file="/includes/global/head-script.aspx" -->
<style>   
	.abt ul li{border-top:1px solid #CCCCCC; border-left:1px solid #CCCCCC; border-right:1px solid #CCCCCC; padding:7px; margin:0; height:auto; list-style-image:none; list-style:none;}
	.abt ul li.sel{background:url(https://img.carwale.com/images/common/menumidbg.gif); repeat:repeat-x;color:#CC0000; font-weight:bold;}
	.abt ul li.end{border-bottom:1px solid #CCCCCC;}
	.abt ul li a{font-weight:bold; color:#6C6C6C;}
	.readable a { text-decoration:underline; }
	.zone-block{float:left; width:220px; border-right:1px dashed #CCCCCC; height:150px;}

    </style>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
        <section class="container">
            <!-- #include file="/includes/header.aspx" -->
            <div class="grid-12">
                        <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            <section class="container">
                <div class="grid-12">
                   
                    <h1 class="font30 text-black special-skin-text">Advertise With Us</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
            </section>
            <div style="width: 954px;">
                <div class="boxxl_rd_top">&nbsp;</div>
                <div class="boxxl_rd_mid" style="height: auto;">
                    <div class="boxxl_rd_container" style="height: auto;">
                        <div class="grid-3">
                            <div class="content-box-shadow">
                                <div class="content-inner-block-10">
                                    <div class="abt">
                                        <ul class="normal">
                                            <li>&raquo; <a href="/aboutus.aspx">About us</a></li>
                                            <li>&raquo; <a href="/carwalestory.aspx">The CarWale Story</a></li>
                                            <li>&raquo; <a href="/award/">Awards &amp; Recognitions</a></li>
                                           <%-- <li>&raquo; <a href="/media">CarWale in News</a></li>--%>
                                            <li>&raquo; <a href="/PressReleases/">Press Releases</a></li>
                                            <li>&raquo; <a href="/career.aspx">Careers</a></li>
                                            <li class="sel">&raquo; Advertise With Us</li>
                                            <li class="end">&raquo; <a href="/contactus.aspx">Contact Us</a></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="grid-9">
                            <table>
                                <tr>
                                    <td>
                                        <div class="content-box-shadow content-inner-block-10 margin-bottom90" style="text-align: justify;">
                                           
                                            <p>
                                                We ensure that your advertisement is linked to the relevant 
								pages and reach your customers in the right way.
                                            </p>
                                            <p>Please contact for any advertisement related queries:</p>
                                            <br />
                                            <br />
                                            <div class="zone-block readable grid-4">
                                                <b>West Zone</b><br>
                                                Anurag Nigam<br>
                                                Email: <a href="mailto:anurag.nigam@carwale.com">anurag.nigam@carwale.com</a>
                                                <br>
                                                <br>
                                            </div>
                                            <div class="zone-block readable" style="padding-left: 20px;">
                                                <b>North Zone</b><br>
                                                Rahul Gupta<br>
                                                Email: <br><a href="mailto:rahul.gupta@carwale.com">rahul.gupta@carwale.com</a>
                                                <br>
                                                <br>
                                            </div>
                                            <div class="zone-block readable" style="padding-left: 20px; border-right: 20px;">
                                                <b>South Zone</b><br>
                                                Anurag Nigam<br>
                                                Email: <a href="mailto:anurag.nigam@carwale.com">anurag.nigam@carwale.com</a>
                                                <br>
                                                <br>
                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            
                        </div>
                    </div>
                    <div class="boxxl_rd_btm">&nbsp;</div>
                </div>
        </section>
        <div class="clear"></div>
    </section>
    <div class="clear"></div>
    <!-- #include file="/includes/footer.aspx" -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
</body>
</html>
