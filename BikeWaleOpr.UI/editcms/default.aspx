<%@ Page AutoEventWireUp="false" Language="C#" Inherits="BikeWaleOpr.EditCms.Default" Trace="false" Debug="false" %>
<%--<%@ Register TagPrefix="dt" TagName="DateControl" src="/Controls/DateControl.ascx" %>--%>
<!-- #Include file="/includes/headerNew.aspx" -->
<div class="urh">
	<a href="/default.aspx">BikeWale operations</a> &raquo; Editorial Home<br />
    Not Updated Author ? Please Update <a href="/EditCms/Author.aspx"> Here</a>
</div>
<script type="text/javascript" language="javascript" src="/src/AjaxFunctions.js"></script>
<script type="text/javascript" language="javascript" src="http://opr.carwale.com/src/common/bt.js"></script>

<div style="clear:both;">
	<br />
	<div>
		<h1 class="urh">Manage Articles</h1>
		[<a href="basicinfo.aspx">Add New</a>]
	</div>
	
		<div>
			<table cellpadding="2" cellspacing="3">
				<tr>
					<td style="width:100px;">Filter Article(s)</td>
					
					<td>
						<asp:DropDownList ID="ddlPeriod" runat="server">
							<asp:ListItem Text="--Select--" Value=""></asp:ListItem>
							<asp:ListItem Text="Today" Value="TODAY"></asp:ListItem>
							<asp:ListItem Text="Yesterday" Value="YESTERDAY"></asp:ListItem>
							<asp:ListItem Text="Last 7 Days" Value="L7D"></asp:ListItem>
							<asp:ListItem Text="Last 30 Days" Value="L30D"></asp:ListItem>
							<asp:ListItem Text="All Time" Value="AT"></asp:ListItem>																												
						</asp:DropDownList>
					</td>
					<td><asp:DropDownList ID="ddlCategory" runat="server">
							<asp:ListItem Text="Category" Value=""></asp:ListItem>
						</asp:DropDownList>
					</td>
					<td style="padding-left:10px;">
						<asp:Button ID="btnGo" Text="Go" runat="server"></asp:Button>
					</td>
				</tr>
			</table>
		</div><br />
		<div>
			<asp:DataGrid ID="dgArticles" runat="server" CssClass="lstTable" 
				CellPadding="5" BorderWidth="1" AllowPaging="true" width="700px"
				PagerStyle-Mode="NumericPages" PageSize="30" AllowSorting="false" AutoGenerateColumns="false">
				<headerstyle CssClass="lstTableHeader"></headerstyle>
				<columns>
					<asp:TemplateColumn HeaderText="Date" ItemStyle-Width="60px">
						<itemtemplate>
							<%# DateTime.Parse(DataBinder.Eval( Container.DataItem, "DisplayDate" ).ToString()).ToString("dd-MM-yyyy") %>
						</itemtemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Article Title" ItemStyle-Width="320px">
						<itemtemplate>
							<a href="summary.aspx?bid=<%# DataBinder.Eval( Container.DataItem, "Id" ) %>"><%# DataBinder.Eval( Container.DataItem, "Title" ) %></a>
						</itemtemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Category" ItemStyle-Width="150px">
						<itemtemplate>
							<%# DataBinder.Eval( Container.DataItem, "CatName" ) %>
						</itemtemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Is Published" ItemStyle-Width="100px">
						<itemtemplate>
							<%# GetIsPublishedData(DataBinder.Eval( Container.DataItem, "Id" ).ToString(), DataBinder.Eval( Container.DataItem, "CategoryId" ).ToString(), DataBinder.Eval( Container.DataItem, "Published" ).ToString(), DataBinder.Eval(Container.DataItem, "CatName").ToString()) %>
						</itemtemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="View Online" ItemStyle-Width="70px">
						<itemtemplate>
							<a target="_blank" href="<%# GetOnlineLink(DataBinder.Eval( Container.DataItem, "CategoryId" ).ToString(), DataBinder.Eval( Container.DataItem, "Id" ).ToString()) %>">View Online</a>
						</itemtemplate>
					</asp:TemplateColumn>	
				</columns>			
			</asp:DataGrid>
		</div>
<style>
	.gb-content { border: 1px solid #a6c9e2; color: #222222; }
	#gb-window { position: absolute; padding: .1em; width: 300px; z-index:100002; display:none; border: 1px solid #a6c9e2; background-color:#fff;}
	#gb-window #gb-head{position:relative; border: 1px solid #4297d7; background: #5c9ccc url(http://img.carwale.com/cw-common/jq-ui/ui-bg_gloss-wave_55_5c9ccc_500x100.png) 50% 50% repeat-x; color: #ffffff; font-weight: bold; height:25px;}
	#gb-window #gb-title { float: left; margin: .4em 0 .3em .3em;} 
	#gb-window #gb-close { position: absolute; right: .3em; top: 50%; width: 19px; margin: -10px 0 0 0; padding: 1px; height: 18px;}
	#gb-window #gb-close span { display: block; margin: 1px; }
	#gb-window #gb-content { border: 0; padding:10px; background: none; overflow:auto; zoom: 1; }
	#gb-window #loading{margin:7px 0 7px 7px;}
	#gb-overlay {position: absolute; top: 0; left: 0; width: 100%; height: 100%; background-color:#F7F7F7; opacity: .30;filter:Alpha(Opacity=30); display:none; z-index:100001;}
</style>
<script type="text/javascript" src="/src/graybox.js"></script>		
<script language="javascript" type="text/javascript">
    function Publish(_basicId, _catId, clickedLink, categoryName, unpublish)
    {
		currentlyClickedLink = clickedLink;
		if (unpublish == 0) {
		    var caption = "Publish " + categoryName;
		}
		else {
		    var caption = "Unpublish " + categoryName;
		}
		var url = "PublishArticle.aspx?bid=" + _basicId + "&cid=" + _catId + "&unpublish=" + unpublish;
		var applyIframe = true;
		var GB_Html = "";
	  
		GB_show(caption, url, 225, 400, applyIframe, GB_Html);
        

	}
</script>		
		
	
</div>
<!-- #Include file="/includes/footerNew.aspx" -->