<%@ Page Language="C#" Inherits="Carwale.UI.Forums.UnansweredThreads" EnableViewState="false" AutoEventWireup="false" Trace="false" %>
<%@ Import Namespace="Carwale.UI.Forums" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
<%
    // Define all the necessary meta-tags info here.
    // To know what are the available parameters,
    // check page, headerCommon.aspx in common folder.

    PageId = 305;
    Title = "Search Topics | Forums";
    Description = "Search CarWale Forums to view topics discussed";
    Keywords = "Car,Forums,topics";
    Revisit = "15";
    DocumentState = "Static";
    AdId = "1397024466973";
    AdPath = "/1017752/Carwale_Forums_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<link rel="stylesheet" href="/static/css/forums.css" type="text/css" >
<style>
    <!--
    .footerStrip { background-color: #FFFFD9; border: #FFFF79 1px solid; padding: 5px; }
        .footerStrip, .footerStrip a, .footerStrip a:link, .footerStrip a:visited, .footerStrip a:active { font-weight: bold; }
    .ac { padding: 3px; }
    .iac { padding: 3px; }
    .message { overflow: auto; }
    -->
</style>
<script type="text/javascript">
    function toggleSearchOptions()
    {
        var imgExpand = document.getElementById("imgExpand");
		
        if ( imgExpand.src.indexOf( 'plus.gif' ) > 0 )
        {
            imgExpand.src = imgExpand.src.replace( 'plus.gif', 'minus.gif' );
            document.getElementById("divSearch").style.display = "";
        }
        else
        {
            imgExpand.src = imgExpand.src.replace( 'minus.gif', 'plus.gif' );
            document.getElementById("divSearch").style.display = "none";
        }
    }
   
</script>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
        <div class="clear"></div>
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Car Forums</li>
                        </ul>
                        <div class="clear"></div>
                        <h1 class="font30 text-black special-skin-text">Unanswered Threads</h1>
                        <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                    </div>
                    <div class="clear"></div>
                    </div>
              </div>
              <div class="container">
                <div class="grid-10 margin-bottom20">
                    <div class="left_container_top content-box-shadow">
                        <div id="left_container_onethird" class="content-inner-block-10">                            
                                <p style="padding: 5px;">
                                    <asp:label id="lblMessage" runat="server" enableviewstate="false" cssclass="error" />
                                </p>
                                <div id="divForum" runat="server">
                                    <div class="footerStrip" id="divStripTop" visible="false" align="right" runat="server"></div>
                                    <asp:repeater id="rptForums" runat="server">
					                    <headertemplate>
						                    <table border="0" width="100%" class="bdr" cellpadding="5" cellspacing="0">
							                    <tr class="dtHeader">
								                    <td width="2" style="border-right:0px;">&nbsp;</td>
								                    <td><strong>Thread</strong></td>
								                    <td width="140"><strong>Forum Category</strong></td>
								                    <td width="10"><strong>Reads</strong></td>
							                    </tr>
					                    </headertemplate>
					                    <itemtemplate>
							                    <tr>
								                    <td style="border-right:0px;padding-top:10px;" valign="middle"><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/arrow_img.gif" /></td>
								                    <td>
									                    <%# GetLastPost
										                    (
											                    DataBinder.Eval(Container.DataItem, "Topic").ToString(),
											                    DataBinder.Eval(Container.DataItem, "HandleName").ToString(),
											                    DataBinder.Eval(Container.DataItem, "StartDateTime").ToString(),
											                    DataBinder.Eval(Container.DataItem, "TopicId").ToString(),
											                    DataBinder.Eval(Container.DataItem, "Replies").ToString(),
											                    DataBinder.Eval(Container.DataItem, "StartedById").ToString(),
											                    true,
                                                                DataBinder.Eval(Container.DataItem,"url").ToString()
										                    )
									                    %>
								                    </td>
								                    <td>
									                    <a target="_blank" href="/forums/<%# DataBinder.Eval(Container.DataItem, "ForumUrl").ToString() %>/"><%# DataBinder.Eval(Container.DataItem, "ForumCategory").ToString() %></a>
								                    </td>
								                    <td align="right">
									                    <%# DataBinder.Eval(Container.DataItem, "Reads").ToString() %>
								                    </td>
							                    </tr>
					                    </itemtemplate>
					                    <footertemplate>
						                    </table>
					                    </footertemplate>
				                    </asp:repeater>
                                    <div class="footerStrip" id="divStrip" align="right" visible="false" runat="server"></div>
                                </div>
                        </div>
                    </div>
                </div>
                <div class="grid-2">
	                        <div class="addbox"><%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx("1396440332273", 160, 600, 0, 0, false, 4) %></div>
                </div>
                <div class="clear"></div>
                </div>
       </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
        <script type="text/javascript">
            Common.showCityPopup = false;
            </script>
     </form>
</body>
</html>
