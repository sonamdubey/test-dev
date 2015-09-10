<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Forums.Threads" Trace="false" %>
<%@ Import Namespace="Bikewale.Forums.Common" %>
<%
    title = "Bike Forums | Ask, Answer and Discuss about Bikes - BikeWale";
    keywords = "Bike forum, auto forum, Bike forum India, Bike forums, Bike discussions, Bike help, Bike howtos";
    description = "India's finest bike discussion forum. Discuss anything related to bikes in India. Ask bike related questions and get fast response.";  
%>
<!-- #include file="/includes/headForums.aspx" -->
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/forums/">Forums</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong><%= ForumName %> Bikes</strong></li>
        </ul><div class="clear"></div>
    </div>
	<div class="grid_12 margin-top10">        
        <h1><%= ForumName%> <span><%= ForumDescription %></span></h1>		
		<table width="100%">
			<tr>
				<td valign="top">
						<asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="error" />						
						<div id="divForum" runat="server">
							<div class="grey-bg margin-top10 footerStrip" id="divStripTop" Visible="false" align="right" runat="server"></div>							
							<table border="0" class="tbl-std" cellpadding="5" cellspacing="0">
								<tr>
									<td style="width:2%">&nbsp;</td>
									<td style="width:50%"><strong>Thread</strong></td>
									<td style="width:16%"><strong>Last Post</strong></td>
									<td style="width:16%;text-align:right;"><strong>Replies</strong></td>
									<td style="width:16%;text-align:right;"><strong>Views</strong></td>
								</tr>
							<asp:Repeater ID="rptStickyForums" runat="server">
								<itemtemplate>
									<tr>
										<td style="border-right:0px;padding-top:10px;" valign="top"><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/arrow_img.gif" /></td>
										<td><b>Sticky : </b>
											<%# ForumsCommon.GetLastPost
												(
													DataBinder.Eval(Container.DataItem, "Topic").ToString(),
													DataBinder.Eval(Container.DataItem, "HandleName").ToString(),
													DataBinder.Eval(Container.DataItem, "StartDateTime").ToString(),
													DataBinder.Eval(Container.DataItem, "TopicId").ToString(),
													DataBinder.Eval(Container.DataItem, "Replies").ToString(),
													DataBinder.Eval(Container.DataItem, "StartedById").ToString()
												)
											%>
										</td>
										<td>
											<%# ForumsCommon.GetLastPostThread
												(
													DataBinder.Eval(Container.DataItem, "PostHandleName").ToString(),
													DataBinder.Eval(Container.DataItem, "LastPostTime").ToString(),
													DataBinder.Eval(Container.DataItem, "LastPostedById").ToString()
												)
											%>
										</td>
										<td style="width:16%;text-align:right;"><%# ( Convert.ToInt32( DataBinder.Eval(Container.DataItem, "Replies") ) - 1 ).ToString() %></td>
										<td style="width:16%;text-align:right;"><%# DataBinder.Eval(Container.DataItem, "Reads").ToString() %></td>																				
									</tr>
								</itemtemplate>
							</asp:Repeater>
							<asp:Repeater ID="rptForums" runat="server">
								<headertemplate>
								</headertemplate>
								<itemtemplate>
										<tr>
											<td style="border-right:0px;padding-top:10px;" valign="top"><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/arrow_img.gif" /></td>
											<td>
												<%# ForumsCommon.GetLastPost
													(
														DataBinder.Eval(Container.DataItem, "Topic").ToString(),
														DataBinder.Eval(Container.DataItem, "HandleName").ToString(),
														DataBinder.Eval(Container.DataItem, "StartDateTime").ToString(),
														DataBinder.Eval(Container.DataItem, "TopicId").ToString(),
														DataBinder.Eval(Container.DataItem, "Replies").ToString(),
														DataBinder.Eval(Container.DataItem, "StartedById").ToString()
													)
												%>
											</td>
											<td>
												<%# ForumsCommon.GetLastPostThread
													(
														DataBinder.Eval(Container.DataItem, "PostHandleName").ToString(),
														DataBinder.Eval(Container.DataItem, "LastPostTime").ToString(),
														DataBinder.Eval(Container.DataItem, "LastPostedById").ToString()
													)
												%>
											</td>
											<td style="width:16%;text-align:right;"><%# ( Convert.ToInt32( DataBinder.Eval(Container.DataItem, "Replies") ) - 1 ).ToString() %></td>
											<td style="width:16%;text-align:right;"><%# DataBinder.Eval(Container.DataItem, "Reads").ToString() %></td>											
										</tr>
								</itemtemplate>
								<footertemplate>
									</table>
								</footertemplate>
							</asp:Repeater>
							<div class="grey-bg footerStrip" id="divStrip" align="right" Visible="false" runat="server"></div>
						</div>				
						<input type="button" value="Create New Thread" style="margin-top:5px;" onclick="location='CreateThreads.aspx?forum=<%=forumId%>'" class="buttons" />
				</td>
			</tr>
		</table>
	</div>
</div>

<!-- #include file="/Includes/footerInner.aspx" -->
<!-- Footer ends here -->

