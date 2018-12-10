<%@ Page Language="C#" Inherits="Carwale.UI.Forums.Search" EnableViewState="false" AutoEventWireup="false" trace="false" %>
<%@ Import Namespace="Carwale.UI.Forums" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Register tagprefix="Vspl" TagName="RepeaterPager" Src="/Controls/AddPageListToLink.ascx" %>

<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
<% 
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 305;
	Title 			= "Search Topics | Forums";
	Description 	= "Search CarWale Forums to view topics discussed";
	Keywords		= "Car,Forums,topics";
	Revisit 		= "15";
	DocumentState 	= "Static";
    //noIndex         = true;
    AdId            = "1397024466973";
    AdPath          = "/1017752/Carwale_Forums_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<link rel="stylesheet" href="/static/css/forums.css" type="text/css" >
<style>

.footerStrip { background-color:#FFFFD9;border:#FFFF79 1px solid; padding:5px; }
.footerStrip, .footerStrip a, .footerStrip a:link, .footerStrip a:visited, .footerStrip a:active { font-weight:bold; }
.ac {padding:3px;}
.iac {padding:3px;}
.message { overflow:auto; }

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
                    </div>
                    <h1 class="font30 text-black special-skin-text">Search CarWale Forums</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-10">
                    <div class="content-box-shadow">
                    <div class="left_container_top content-inner-block-10">
                        <div id="left_container_onethird">
	                         <div style="display:none!important;displabackground-color:#eeeeee;padding:5px;border:1px solid #dddddd;"><img style="cursor:pointer" onclick="toggleSearchOptions()" id="imgExpand" title="View sharing URLs" class="collapsed" src="<%=ImagingFunctions.GetRootImagePath()%>/images/icons/plus.gif"/>&nbsp;<a style="cursor:pointer;" onclick="toggleSearchOptions()" id="expMessage"><b>Search Options</b></a></div>
		                    <div id="divSearch" style="display:none;">
			                    <table align="center" style="display:none!important">
				                    <tr>
                                        <td><strong class="font16">Search</strong></td>
					                    <td colspan="2"> <asp:TextBox CssClass="form-control" ID="txtSearch" Columns="50" runat="server" /></td>
                                        <td><div class="buttons"><asp:Button CssClass="buttons btn btn-orange" ID="btnSearch" Text="Search" runat="server" /></div></td>
				                    </tr>
				                    <%--<tr>
					                    <td align="center">
						                    <asp:RadioButton ID="optTopics" Text="Search Titles Only" GroupName="search" runat="server" /> 
						                    <asp:RadioButton id="optAll" Text="Entire Thread" Checked="true" GroupName="search" runat="server" />
					                    </td>
					                    <td><div class="buttons"><asp:Button CssClass="buttons" ID="btnSearch" Text="Search" runat="server" /></div></td>			
				                    </tr>--%>
			                    </table>
		                    </div>	
		                    <br> 
		                    <p style="padding:5px;"><asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="text-red" /></p>
		                    <div id="divForum" runat="server">
			                    <div class="footerStrip" id="divStripTop" Visible="false" align="right" runat="server"></div>
			                    <asp:Repeater ID="rptForums" runat="server" >
				                    <headertemplate>
					                    <table border="0" width="100%" class="bdr" cellpadding="5" cellspacing="0">
						                    <tr class="dtHeader">
							                    <td width="2" style="border-right:0px;">&nbsp;</td>
							                    <td><strong>Thread</strong></td>
							                    <td width="140"><strong>Last Post</strong></td>
							                    <td width="140"><strong>Forum Category</strong></td>
							                    <td width="10"><strong>Replies</strong></td>
							                    <td width="10"><strong>Reads</strong></td>
						                    </tr>
				                    </headertemplate>
				                    <itemtemplate>
						                    <tr>
							                    <td style="border-right:0px;padding-top:10px;" valign="top"><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/arrow_img.gif" /></td>
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
                                                            DataBinder.Eval(Container.DataItem, "Url").ToString()
									                    )
								                    %>
							                    </td>
							                    <td>
								                    <%# GetLastPostThread
									                    (
										                    DataBinder.Eval(Container.DataItem, "PostHandleName").ToString(),
										                    DataBinder.Eval(Container.DataItem, "LastPostTime").ToString(),
										                    DataBinder.Eval(Container.DataItem, "LastPostedById").ToString()
									                    )
								                    %>
							                    </td>
							                    <td>
								                    <a target="_blank" href="<%# DataBinder.Eval(Container.DataItem, "ForumUrl").ToString() %>/"><%# DataBinder.Eval(Container.DataItem, "ForumCategory").ToString() %></a>
							                    </td>
							                    <td align="right">
								                    <%# ( Convert.ToInt32( DataBinder.Eval(Container.DataItem, "Replies") ) - 1 ).ToString() %>
							                    </td>
							                    <td align="right">
								                    <%# DataBinder.Eval(Container.DataItem, "Reads").ToString() %>
							                    </td>
						                    </tr>
				                    </itemtemplate>
				                    <footertemplate>
					                    </table>
				                    </footertemplate>
			                    </asp:Repeater>
			                    <asp:Repeater ID="rptPosts" runat="server">
				                    <headertemplate>
					                    <table border="0" width="100%" class="bdr" cellpadding="5" cellspacing="0">
						                    <tr class="dtHeader">
							                    <td width="2" style="border-right:0px;">&nbsp;</td>
							                    <td><strong>Post</strong></td>
							                    <td width="140"><strong>Forum Category</strong></td>
							                    <td width="10"><strong>Replies</strong></td>
							                    <td width="10"><strong>Reads</strong></td>
						                    </tr>
				                    </headertemplate>
				                    <itemtemplate>
						                    <tr>
							                    <td style="border-right:0px;padding-top:10px;" valign="top"><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/arrow_img.gif" /></td>
							                    <td>
								                    <p><a target="_blank" href="viewthread.aspx?thread=<%# DataBinder.Eval(Container.DataItem, "TopicId").ToString() %>&amp;post=<%# DataBinder.Eval(Container.DataItem, "PostId").ToString() %>"><%# DataBinder.Eval(Container.DataItem, "Topic").ToString() %></a></p>									
								                    <div class="message"><%# FormatMessage( DataBinder.Eval(Container.DataItem, "Message").ToString() ) %></div>
							                    </td>
							                    <td>
								                    <a target="_blank" href="<%# DataBinder.Eval(Container.DataItem, "ForumUrl").ToString() %>/"><%# DataBinder.Eval(Container.DataItem, "ForumCategory").ToString() %></a>
							                    </td>
							                    <td align="right">
								                    <%# ( Convert.ToInt32( DataBinder.Eval(Container.DataItem, "Replies") ) - 1 ).ToString() %>
							                    </td>
							                    <td align="right">
								                    <%# DataBinder.Eval(Container.DataItem, "Reads").ToString() %>
							                    </td>
						                    </tr>
				                    </itemtemplate>
				                    <footertemplate>
					                    </table>
				                    </footertemplate>
			                    </asp:Repeater>
			                    <%--<div class="footerStrip" id="divStrip" align="right" Visible="false" runat="server"></div>--%>
                                <div class="footerStrip" id="divStrip" align="right" runat="server" ><Vspl:RepeaterPager id="pagestrip" Visible="false" runat="server" ></Vspl:RepeaterPager></div>
		                    </div>
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
        var totalRecords = '<%=totalResults%>';
	
                if ( totalRecords == "" )
                {
                    var imgExpand = document.getElementById("imgExpand");
		
                    imgExpand.src = imgExpand.src.replace( 'plus.gif', 'minus.gif' );
                    document.getElementById("divSearch").style.display = "";
                }
        </script>
    </form>
</body>
</html>
