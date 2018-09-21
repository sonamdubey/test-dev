<%@ Page Language="C#" AutoEventWireup="false"  EnableEventValidation="false"  Inherits="Bikewale.MyBikeWale.NewsSubscription" %>
<% //Modified By :Sajal Gupta on 03 August 2016
    isAd300x250Shown = false;
    isAd300x250BtfShown = false; %>
<!-- #include file="/UI/Includes/headMyBikeWale.aspx" -->
<div class="container_12 container-min-height">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/" itemprop="url">
                    <span itemprop="title">Home</span>
                </a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/mybikewale/" itemprop="url">
                    <span  itemprop="title">My BikeWale</span>
                </a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>News Subscription</strong></li>
        </ul><div class="clear"></div>
    </div>
	<div id="content" class="grid_8">
        <h2 class="margin-top10 margin-bottom5">BikeWale News Subscription</h2>		
		<!--My Profile Content-->
		<div  id="tb1">
			<table border="0" width="100%" cellspacing="0" cellpadding="5" >	
                <tr>			
					<td colspan="3"><span id="spnError" class="error" runat="server" /></td>			
				</tr>							
				<tr>
					<td >&nbsp;</td>						
					<td colspan="3">&nbsp;</td>
				</tr>
				<tr>
					<td>Email Subscription :</td>
					<td class="text-highlight">Emails you would like to receive</td>
					<td colspan="2"></td>
				</tr>						
				<tr>
					<td>&nbsp;</td>
					<td><asp:CheckBox ID="chkNewsLetter" checked="true" Text=" Do you want us to send you Newsletters?" runat="server" /></td>
					<td colspan="2"></td>
				</tr>	
                <tr>	
					<td>&nbsp;</td>
					<td align="left"><asp:Button ID="btnSave"  CssClass="buttons text_white"  Text="Save"  runat="server" /></td>
					<td colspan="3"></td>
				</tr>	
            	            
			</table>
		</div>				
	</div>
</div>
<!-- #include file="/UI/Includes/footerInner.aspx" -->
<!-- Footer ends here -->

