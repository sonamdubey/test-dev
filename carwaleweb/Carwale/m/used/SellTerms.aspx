﻿<%
    MenuIndex = "5";
    bool IsWebView = Carwale.Utility.BrowserUtils.IsWebView();
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<!-- #include file="/m/includes/global/head-script.aspx" -->
<style>
	.prem-bullets ul { list-style:disc; margin-left:20px; }
	.prem-bullets li{ margin-bottom:10px; line-height:22px; /*font-size:12px;*/ }
</style>
</head>

<body class="m-special-skin-body m-no-bg-color">
    <% if (!IsWebView) { %>
        <!-- #include file="/m/includes/header.aspx" -->
    <% } %>
	<!--Outer div starts here-->
	<section class="container">
    	<!--Main container starts here-->
    	<div id="main-container">
        	<div class="grid-12">
            <h1 class="pgsubhead margin-top10 margin-bottom10 m-special-skin-text">Terms & Conditions</h1>
            <div class="box content-inner-block-10 rounded-corner2 content-box-shadow margin-bottom10 prem-bullets">
                <ul>
					<li>All listings may go through a review process and the approval of a listing will be at the discretion of CarWale. Prospective buyers will see images after they are approved. Rest of the details will be visible instantly after Ad goes live.</li>
					<li>An individual user can list only two cars as part of the free listings. If user wants to upload third car, he will have to delete one of the existing live cars from CarWale.</li>
					<li>As a Seller you certify that all information provided by you against your listed car is true.</li>
					<li>Seller is alone responsible for completing and verification of the documentation part, before concluding the sale. Any sale concluded on the part of the Seller, shall be the sole responsibility of the Seller.</li>
                </ul>
                <div class="head-big-desc margin-top10">
                    <h4 class="margin-bottom5">Please note:</h4>
                    <ul>
                        <li>Free listing plan is brought to you by Automotive Exchange Private Limited ("CarWale") and is open only to individuals or private party sellers who list their car for sale on CarWale. Dealers, brokers or those individuals who trade in cars are not eligible.</li>
						<li>CarWale reserves the right to withdraw and/or alter any or all of the terms and conditions of the plan at any time without prior notice.</li>
						<li>You authorise CarWale.com/CarTrade.com to call or SMS you in connection with your car Advertisement.</li>
                        <li>Any dispute arising out of or in connection with this scheme shall be subject to the exclusive jurisdiction of the courts in Mumbai only. The existence of a dispute, if any, shall not constitute a claim against CarWale.</li>
						<li>Any individual, who lists his/ her car on CarWale availing the plan, accepts the Terms and Conditions specified above in total.</li>
						<li>User will not abuse or misuse the website or engage in any activity which violates the terms of this Agreement. In any such case, CarWale may suspend user account or permanently debar him/her from accessing the Website.</li>
                    </ul>
                </div>
            </div>
            </div>
           <div class="clear"></div>
        </div>
        <!--Main container ends here-->
    </section>
    <div class="clear"></div>
    <!--Outer div ends here-->
    <% if(!IsWebView) { %>
        <!-- #include file="/m/includes/footer.aspx" -->
    <% } %>
	 <!-- #include file="/m/includes/global/footer-script.aspx" -->
    <!--Outer div ends here-->
</body>
</html>