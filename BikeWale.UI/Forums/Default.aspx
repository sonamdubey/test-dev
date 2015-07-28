<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Forums.Default" Trace="false" %>
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
            <li class="current"><strong>Forums</strong></li>
        </ul><div class="clear"></div>
    </div>
	<div class="grid_12 margin-top10">                
		<form id="Form1" runat="server">
			<asp:Repeater ID="rptParent" runat="server">
				<headertemplate><table class="tbl-std"></headertemplate>
				<itemtemplate>
						<tr><td><div class="grey-bg content-block-forum"><h1><%# DataBinder.Eval(Container.DataItem, "Name")%></h1></div></td></tr>
						<tr>
							<td>
								<!-- contains the forums -->
								<asp:Repeater ID="rptChild" runat="server"
									DataSource='<%# GetChild(DataBinder.Eval(Container.DataItem, "ID").ToString()) %>'>
									<headertemplate>
										<table>
											<tr>
												<td style="width:2%">&nbsp;</td>
												<td style="width:48%"><strong>Forum</strong></td>
												<td style="width:30%"><strong>Last Post</strong></td>
												<td style="width:20%;"><strong>Threads</strong></td>
												<td style="width:20%;text-align:right;"><strong>Posts</strong></td>
											</tr>
									</headertemplate>
									<itemtemplate>
											<tr>
												<td valign="top">&nbsp;</td>
												<td valign="top">	
                                                    <a href='viewforum-<%# DataBinder.Eval(Container.DataItem, "SubCatId") %>.html'><%# DataBinder.Eval(Container.DataItem, "SubCatName") %></a>  <span class="startBy"><%# GetCategoryViews(DataBinder.Eval(Container.DataItem, "SubCatId").ToString()) %></span><br />
													<%# DataBinder.Eval(Container.DataItem, "Description") %>
												</td>
												<td valign="top"><%# GetLastPost( DataBinder.Eval(Container.DataItem, "LastThreadId").ToString(), DataBinder.Eval(Container.DataItem, "LastThread").ToString(), DataBinder.Eval(Container.DataItem, "Handle").ToString(), DataBinder.Eval(Container.DataItem, "LastPostDate").ToString(), DataBinder.Eval(Container.DataItem, "LastPostedById").ToString() ) %></td>
												<td><%# DataBinder.Eval(Container.DataItem, "Threads") %></td>
												<td align="right"><%# DataBinder.Eval(Container.DataItem, "Posts") %></td>												
											</tr>
									</itemtemplate>
									<footertemplate>
										</table>
									</footertemplate>
								</asp:Repeater>
							</td>
						</tr>
						<tr id="Tr1" runat="server" visible='<%# (++serial) == 2 ? true : false %>'><td>&nbsp;</td></tr>	
						<tr><td style="height:25px">&nbsp;</td></tr>						
				</itemtemplate>
				<footertemplate></table></footertemplate>				
			</asp:Repeater>			
			<style>
				<!--
				.forumStats { background-color:#f2faff; border:1px solid #6B9FBF; }
				.forumStats .statHead { padding:5px; color:#164F72; font-weight:bold; border-bottom:1px solid #6B9FBF; }
				.forumStats .statContents { padding:5px; line-height:18px; }
				-->
			</style>
			<div class="forumStats hide">
				<div class="statHead">About BikeWale Forums</div>
				<div class="statContents">
					<div><b>Currently active users: </b> <%=total%> (<%=members%> members and <%=guests%> guests)</div>
					<div>
						<asp:Repeater ID="rptMembers" runat="server">
							<itemtemplate>
							<a href='/community/members/<%# DataBinder.Eval(Container.DataItem, "HandleName") %>.html'><%# DataBinder.Eval(Container.DataItem, "HandleName") %></a></itemtemplate>
							<separatortemplate>, </separatortemplate>
						</asp:Repeater>
					</div>
					<%--<div style="display:none;">Most users ever online was <%=mostUsers%>, <%=Convert.ToDateTime(mostUsersDate).ToString("dd MMM yyyy h:mm tt")%></div>--%>
					<div>Total Discussions: <b><%=discussions%></b>, Posts: <b><%=posts%></b>, Contributors: <b><%=contributors%></b>.</div>
					<div><b>Top Contributors : </b>
					<asp:Repeater ID="rptTopContributors" runat="server">
						<itemtemplate><%# rptTopContributors.Items.Count == 0 ? "" : ",&nbsp;"  %><a target="_blank" href="/Users/Profile-<%# Bikewale.Common.BikewaleSecurity.EncryptUserId( long.Parse( DataBinder.Eval(Container.DataItem, "CustomerId").ToString() ) ) %>.html"><%# DataBinder.Eval(Container.DataItem, "Name") %></a></itemtemplate>
					</asp:Repeater>
					</div>
					<div><b>Top Contributors (current month): </b>
					<asp:Repeater ID="rptCurrentTopContributors" runat="server">
						<itemtemplate><%# rptCurrentTopContributors.Items.Count == 0 ? "" : ",&nbsp;"  %><a target="_blank" href="/Users/Profile-<%# Bikewale.Common.BikewaleSecurity.EncryptUserId( long.Parse( DataBinder.Eval(Container.DataItem, "CustomerId").ToString() ) ) %>.html"><%# DataBinder.Eval(Container.DataItem, "Name") %></a></itemtemplate>
					</asp:Repeater>
					</div>
				</div>
			</div>	
		</form>
	</div>
</div>

<!-- #include file="/includes/footerInner.aspx" -->
<!-- Footer ends here -->
