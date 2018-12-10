<%@ Page trace="false" Language="C#" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<!doctype html>
<html>
<head>
<!-- #include file="/includes/global/head-script.aspx" -->
<!-- Header ends here -->
</head>
<style>
	p { font-size:13px; color:#333333; }
</style>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <!-- #include file="/includes/header.aspx" -->
    <section class="container padding-bottom30">
        <div class="gris-12">
            <h1 class="font30 text-black special-skin-text">Update Confirmation</h1>
            <div class="border-solid-bottom margin-top5 margin-bottom10"></div>
            <!-- For the ad only starts here -->
            <div class="left_container_top">
	            <div id="left_container_onethird">
		            <table width="100%" border="0">
			            <tr>
				            <td valign="top">
					            <!-- For the ad only ends here-->
					            <p>
						            <strong>You have successfully updated the review and it is sent for verification to moderator.</strong><br>
						            After approval these changes will start reflecting on website. 
					            </p>
				            </td>
			            </tr>
		            </table>
	            </div>
            </div>
            <div class="right_container">
	            <div class="addbox"><%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx("1396440332273", 160, 600, 0, 0, false, 4) %></div>
            </div>
            <img height="0" width="0" src='https://www.s2d6.com/x/?x=a&amp;h=17260&amp;o=<%= "UR" + CurrentUser.Id %>' alt="" />
            <img height="0" width="0" src='https://www.s2d6.com/x/?x=r&amp;h=17260&amp;o=<%= "UR" + CurrentUser.Id %>&amp;g=441212112514&amp;s=0&amp;q=0' alt="" />
        </div>
        <div class="clear"></div>    
        </section>
    <!-- #include file="/includes/footer.aspx" -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
</body>
</html>
