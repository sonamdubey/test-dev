<%@ Page Language="C#" Inherits="Carwale.UI.Forums.modReportAbuse" Trace="false" AutoEventWireup="false" %>

<%@ Import Namespace="Carwale.UI.Forums" %>
<%
    // Define all the necessary meta-tags info here.
    // To know what are the available parameters,
    // check page, headerCommon.aspx in common folder.

    PageId = 305;
    Title = "Moderator | Forums Report Abuse Summary";
    Description = "";
    Keywords = "";
    Revisit = "15";
    DocumentState = "Static";
    AdId = "1397024466973";
    AdPath = "/1017752/Carwale_Forums_";
%>
<!-- #include file="/includes/headCommunity.aspx" -->
<link rel="stylesheet" href="/static/css/forums.css" type="text/css" >
<style>
    <!--
    .footerStrip { background-color: #FFFFD9; border: #FFFF79 1px solid; padding: 5px; }
        .footerStrip, .footerStrip a, .footerStrip a:link, .footerStrip a:visited, .footerStrip a:active { font-weight: bold; }
    .ac { padding: 3px; }
    .iac { padding: 3px; }
    .message { padding: 8px; margin: 5px; background: #E9FEEA; color: #006231; font-size: 13px; font-weight: bold; border: 1px solid #A9D5A8; width: 350px; }
    -->
</style>
<script language="javascript">
    function selectAll(type,chkId) 
    {
        var obj = document.getElementsByTagName("input");
		
        if(type == "all"){
            bolVal = true;
        }
        else{
            bolVal = false;
        }
        for ( var i = 0 ; i < obj.length ; i++ ) {
            if ( obj[i].type == "checkbox" && obj[i].id.indexOf(chkId) != -1 ) {
                obj[i].checked = bolVal;
            }
        }
    }
</script>
<div class="left_container_top">
    <div id="left_container_onethird">
        <div id="youHere">
            <img src="<%=ImagingFunctions.GetRootImagePath()%>/images/bullet/arrow.gif" align="absmiddle" />
            <span style="font-weight: bold">You are here</span> : 
			<a href="/community/">Community</a> &raquo; <a href="./">Forums</a> &raquo; Reported Posts
        </div>
        <form runat="server">
            <h1>Reported Posts</h1>
            <br>
            <div id="divMessage" visible="false" class="message" runat="server"></div>
            <div id="divForum" runat="server">
                <asp:repeater id="rptReport" runat="server">
					<headertemplate>
						<table width="100%" cellspacing="0" cellpadding="5" class="bdr" border="0">
							<tr class="dtHeader">
								<td width="10">&nbsp;</td>
								<td>Thread</td>
								<td>Post</td>
								<td >Abused by</td>
								<td>Reason</td>
								<td>Date</td>
							</tr>
					</headertemplate>
					<itemtemplate>
							<tr>
								<td valign="top"><asp:CheckBox ID="chkID" runat="server"></asp:CheckBox> <asp:Label ID="lblId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>'></asp:Label>
								<asp:Label ID="lblReportId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RID") %>'></asp:Label>
								</td>
								<td valign="top"><%# "<a href='ViewThread.aspx?thread="+ DataBinder.Eval(Container.DataItem, "ThreadId") + "&post=" + DataBinder.Eval(Container.DataItem,"ID") +"' target='_blank'>" + DataBinder.Eval(Container.DataItem,"Thread")  + "</a>" %></td>
								<td valign="top"><%# DataBinder.Eval(Container.DataItem,"Post") %></td>
								<td width="70" valign="top"><%# "<a href='/Users/Profile-" + Carwale.Utility.CarwaleSecurity.EncryptUserId( long.Parse( DataBinder.Eval(Container.DataItem, "CustomerId").ToString() ) ) + ".html' target='_blank'>" + DataBinder.Eval(Container.DataItem, "Customer") + "</a>" %></td>
								<td width="100" valign="top"><%# DataBinder.Eval(Container.DataItem,"Comment") %></td>
								<td width="60" valign="top"><%# DataBinder.Eval(Container.DataItem,"Date1", "{0:dd-MMM-yy}") %></td>
							</tr>
					</itemtemplate>
					<footertemplate>
						</table>
					</footertemplate>
				</asp:repeater>
                <div align="left" style="padding: 5px;">
                    <strong>Select :</strong>
                    <a href="javascript:selectAll('all','chkID')"><strong>All</strong></a>
                    <a href="javascript:selectAll('none','chkID')"><strong>None</strong></a>
                    <asp:button id="btnApprove" cssclass="submit" text="Approve Post(s)" runat="server"></asp:button>
                    <asp:button id="btnDelete" cssclass="submit" text="Delete Post(s)" runat="server"></asp:button>
                </div>
            </div>
        </form>
    </div>
</div>
<div class="right_container">
    <div class="addbox">
        <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx("1396440332273", 160, 600, 0, 0, false, 4) %>
    </div>
</div>
<script language="javascript">
<!--
    document.getElementById("btnDelete").onclick = btnDelete_Change;

    function btnDelete_Change(e)
    {
        var chks = document.getElementById( "divForum" ).getElementsByTagName( "input" );
        var checkCount = 0;
	
        for ( var i=0; i<chks.length; i++ )
        {
            if ( chks[i].checked )
                checkCount ++;
        }
	
        if ( checkCount == 0 )
        {
            alert("Please select at least one Post to continue.");
            return false;
        }
	
        return true;
    }

    -->
</script>
<script type="text/javascript">
    Common.showCityPopup = false;
</script> 
<!-- #include file="/includes/footer-old.aspx" -->
<!-- Footer ends here -->
