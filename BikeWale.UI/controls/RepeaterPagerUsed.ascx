<%@ Control Language="C#" AutoEventWireUp="false" Inherits="Bikewale.Controls.RepeaterPagerUsed" Debug="false" %>
<table id="tbl_res" border="0" width="100%" cellpadding="0" cellspacing="0">
	<tr class="dgNavDivTop" id="TopPager" runat="server">
		<td ><asp:label CssClass="headers" ID="lblRecords" runat="server" /></td>   
        <td  align="right">
             Sort By : 
            <select id="ddlSort" style="width:200px;">
                <option value="sc=-1&so=-1">Relevance</option>
                <option value="sc=2&so=0">Price - Low To High</option>
                <option value="sc=2&so=1">Price - High To Low</option>
                <option value="sc=0&so=1">Year - Latest To Oldest</option>                
                <option value="sc=3&so=0">Kms - Low to High</option>                          
                <option value="sc=6&so=1">Last Updated - Latest To Oldest</option>                
            </select>
        </td>     
	</tr>
	<tr><td colspan="2"><asp:Panel ID="pnlGrid" runat="server"></asp:Panel></td></tr>
	<tr>
        <td colspan="2">
            <div id="div_nec" runat="server" visible="false" class="alert2 ucAlert" style="margin:10px;">
                <span class="price">Not able to find right bike for you?</span>
                <div class="filter search">Not enough bikes? Try searching within <b><a id="entire_state"><%= stateName %></a></b> instead.</div>
                <!-- div to show or hide the alert criteria and to set email alert -->
                <div id="alert_content" class="show">   <!-- class changed from hide to show on 19/3/2012 by Ashish G. Kamble -->
                    <div class="filter msg"><span id="span1">Set an email alert.</span> Inform me whenever matching bike is available for sale</div>
                    <div id="alert_crit">
                        <b>Criteria : </b><span id="alert_selected"></span><div class="clear clear-margin"></div><div class="text-grey2 clear-margin">(Change search parameters to change the criteria)</div>
                    </div>
                    <div>Your Email: <input type="text" id="alertEmail" />&nbsp; Frequency: <select id="selAlertFrq"><option value="1">Weekly</option><option value="2">Daily</option></select>&nbsp; <input id="setAlert" type="button" class="buttons" value="Set Alert" onclick="setBuyerAlerts(this);"/><span class="process-inline hide" style="display:none;"></span></div>
                </div>
                <div id="alert_status" class="hide"></div>
            </div>
        </td>
    </tr>
	<tr class="dgNavDivTop" id="BottomPager" runat="server">
		<td><asp:label CssClass="headers" ID="lblRecordsFooter" runat="server" /></td>
		<td id="pgBot" align="right"><span id="divFirstNav1" runat="server"></span><span id="divPages1" runat="server" align="center"></span><span id="divLastNav1" runat="server"></span></td>				
	</tr>
</table>