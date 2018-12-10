<%@ Language="C#" ContentType="text/html" Inherits="MobileWeb.Authors.Default" AutoEventWireup="false" trace="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" itemscope itemtype="http://schema.org/WebPage">
<head>
<!-- #include file="/m/includes/global/head-script.aspx" -->
    <style>
        .other-author-img, .author-list-table .table-imgWidth {
    width: 80px;
}
        .other-author-img img, .author-list-table .table-imgWidth img {
    max-width: 100%;
}
    </style>
</head>

<body>
    <!-- #include file="/m/includes/header.aspx" -->
	<!--Outer div starts here-->
	<section class="container">
    	<!--Main container starts here-->
    	<div id="main-container">
			<div class="grid-12">
            <!--Author List starts here-->
            <div class="inner-container light-shadow">
                <h1 class="margin-top10 margin-bottom10">All Authors</h1>
               
                <div class="margin-top10 darkgray">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="author-list-table">
                        <asp:Repeater id="rptAuthors" runat="server">
                            <itemtemplate>
                                <tr>
                                    <td colspan="2" >
                                        <div class="content-inner-block-10 content-box-shadow rounded-corner2 margin-bottom10">
                                    <table  border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td colspan="2"><strong><%#Eval("AuthorName") %></strong>, <span class="lightgray"><%#Eval("Designation") %></span></td>
                                </tr>
                                <tr>
                                    <td colspan="2" height="10"></td>
                                </tr>
                                <tr>
                                    <td valign="top" class="table-list-img">
                                        <div class="table-imgWidth">
                                            <a href='/m/authors/<%#Eval("MaskingName") %>/'><img src='https://<%#Eval("HostUrl") %>/<%#Eval("ProfileImage") %>/<%#Eval("ImageName") %>' alt="Author" title="Author" /></a>
                                        </div>
                                    </td>
                                    <td valign="top">
                                        <div class="margin-left10"><%#Eval("ShortDescription") %>...<a href='/m/authors/<%#Eval("MaskingName") %>/' class="blue">More</a></div>
                                    </td>
                                </tr>
                                <!--<tr>
                                    <td colspan="2"><div class="line-border margin-top20 margin-bottom20"></div></td>
                                </tr> -->
                                        </table> 
                                            </div>
                                        </td>
                                    </tr>
                            </itemtemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
            <!--Author List ends here-->
			</div>
            <div class="clear"></div>
        </div>
        <!--Main container ends here-->
    </section>
    <div class="clear"></div>
    <!--Outer div ends here-->

    <!-- #include file="/m/includes/footer.aspx" -->
	<!-- #include file="/m/includes/global/footer-script.aspx" -->
</body>
</html>