<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.WriteReviews" Trace="false" ValidateRequest="false" %>
<%@ Register TagPrefix="BW" TagName="RTE" src="/Controls/RichTextEditor.ascx" %>
<%  //modified by SajalGupta for unfilled impression of ads on 04 Aug 2016.
    isAd300x250Shown = false;
    isAd300x250_BTFShown = false; %>
<!-- #include file="/includes/headnew.aspx" -->
    <div class="container_12">
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
                    <a href="/new/" itemprop="url">
                        <span itemprop="title">New</span>
                    </a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                    <a href="/user-reviews/" itemprop="url">
                        <span itemprop="title">User Reviews</span>
                    </a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong>Write a Review</strong></li>
            </ul><div class="clear"></div>            
        </div>
        <div class="grid_8  margin-top10">
	        <h1>Write a Review for <%= BikeName%></h1>
	        <p>
		        Please be sure to focus your feedback on the bike and how it met your expectations. Please note that your review will be moderated before it goes live!<br><br>
		        <asp:Label ID="lblMessage" runat="server" Font-Bold="true" ForeColor="#cc0000"></asp:Label>
	        </p>					
	        <table width="100%" cellpadding="5" cellspacing="0" border="0" id="tblRatings">
		        <tr style="display:<%=displayVersion%>">			
			        <td colspan="2"> This review is for which version? <font color="red">*</font><br /><asp:DropDownList ID="drpVersions" runat="server" />&nbsp;<span id="spnVersions"></span></td>
		        </tr>
		        <tr><td colspan="2"><h3>How would you rate this bike on following</h3></td></tr>		
		        <tr>			
			        <td colspan="2">
				        Style <font color="red">*</font><br />				
				        <img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateST1" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateST2" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateST3" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateST4" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateST5" />
				        <span id="divRateST">&nbsp;</span>
				        <input type="hidden" id="hdnRateST" runat="server" value="0" />
				        <span id="spnRateST" class="required"></span>
			        </td>
		        </tr>
		        <tr>			
			        <td colspan="2">
				        Comfort <font color="red">*</font><br />		
				        <img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateCM1" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateCM2" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateCM3" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateCM4" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateCM5" />
				        <span id="divRateCM">&nbsp;</span>
				        <input type="hidden" id="hdnRateCM" runat="server" value="0" />
				        <span id="spnRateCM" class="required"></span>				
			        </td>
		        </tr>
		        <tr>			
			        <td colspan="2">	
				        Performance <span>(Engine, gearbox & overall)</span><font color="red"> *</font><br />			
				        <img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRatePE1" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRatePE2" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRatePE3" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRatePE4" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRatePE5" />
				        <span id="divRatePE">&nbsp;</span>
				        <input type="hidden" id="hdnRatePE" runat="server" value="0" />
				        <span id="spnRatePE" class="required"></span>
			        </td>
		        </tr>
		        <tr>			
			        <td colspan="2">
				        Fuel Economy (mileage) <font color="red">*</font><br />				
				        <img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateFE1" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateFE2" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateFE3" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateFE4" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateFE5" />
				        <span id="divRateFE">&nbsp;</span>
				        <input type="hidden" id="hdnRateFE" runat="server" value="0" />
				        <span id="spnRateFE" class="required"></span>
			        </td>
		        </tr>
		        <tr>			
			        <td colspan="2">	
				        Value for money/Features <font color="red">*</font><br />			
				        <img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateVC1" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateVC2" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateVC3" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateVC4" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateVC5" />
				        <span id="divRateVC">&nbsp;</span>
				        <input type="hidden" id="hdnRateVC" runat="server" value="0" />
				        <span id="spnRateVC" class="required"></span>
			        </td>
		        </tr>
		        <tr style="display:none;">
			        <th>Overall <font color="red">*</font></th>
			        <td>
				        <div id="divRateOA">&nbsp;</div>
				        <img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateOA1" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateOA2" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateOA3" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateOA4" /><img src="<%=Bikewale.Common.ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor:pointer" id="imgRateOA5" />
				        <input type="hidden" id="hdnRateOA" runat="server" value="0" /><br>
				        <span id="spnRateOA" class="required"></span>
			        </td>
		        </tr>
		        <tr>			
			        <td colspan="2">				
				        Title <font color="red">*</font><br />
				        <asp:TextBox ID="txtTitle" runat="server" MaxLength="100" Columns="50" CssClass="text" ToolTip="Max 100 characters." />				
				        <span id="spnTitle" class="required"></span>				
			        </td>
		        </tr>
		        <tr>			
			        <td colspan="2">
				        Pros<font  color="red">*</font><span>(things you like)</span><br />
				        <asp:TextBox ID="txtPros" runat="server" MaxLength="100" Columns="50" CssClass="text" ToolTip="e.g., Good fuel economy, Good style. Max 100 characters" />
				        <span id="spnPros" class="required"></span>				
			        </td>
		        </tr>
		        <tr>		
			        <td colspan="2">
				        Cons<font color="red">*</font> <span>(things you don't like)</span><br />
				        <asp:TextBox ID="txtCons" runat="server" MaxLength="100" Columns="50" CssClass="text" ToolTip="e.g., Bad interiors, Less spacious. Max 100 characters." />							
				        <span id="spnCons" class="required"></span>
			        </td>
		        </tr>
		        <tr>			
			        <td colspan="2">
				        Detailed Review <font color="red">*</font>														
				        <BW:RTE id="ftbDescription" Rows="15" Cols="20" runat="server" title="Maximum 8000 characters (approx. 2000 words). Minimum 50 words" /><br />
				        <span>Maximum 8000 characters (approx. 2000 words). Minimum 50 words.</span><br>
				        <span id="spnDesc"></span><br />
				        <span id="spnDescription" class="required"></span>		
			        </td>			
		        </tr>
		        <tr>			
			        <td>
				        Purchased as <font color="red">*</font><br />
				        <asp:RadioButton ID="radNew" Text="New" runat="server" GroupName="new" />
				        &nbsp; <asp:RadioButton ID="radOld" Text="Used" runat="server" GroupName="new" />
				        &nbsp; <asp:RadioButton ID="radNot" Text="Not Purchased" runat="server" GroupName="new" Checked="true" />
			        </td>
		        </tr>
		        <tr>			
			        <td colspan="2">
				        Familiarity with the bike <font color="red">*</font><br />				
				        <asp:DropDownList ID="ddlFamiliar" runat="server">
					        <asp:ListItem Selected="true" Text="--Select--" Value="0"></asp:ListItem>
					        <asp:ListItem Selected="false" Text="Haven't ridden it" Value="1"></asp:ListItem>
					        <asp:ListItem Selected="false" Text="Have done a short test-drive once" Value="2"></asp:ListItem>
					        <asp:ListItem Selected="false" Text="Have ridden for a few hundred kilometres" Value="3"></asp:ListItem>
					        <asp:ListItem Selected="false" Text="Have ridden a few thousands kilometres" Value="4"></asp:ListItem>
					        <asp:ListItem Selected="false" Text="It's my mate since ages" Value="5"></asp:ListItem>
				        </asp:DropDownList>
				        <span id="spnFamiliar" class="required"></span>
			        </td>
		        </tr>
		        <tr>		
			        <td colspan="2">Fuel Economy (km/l)<br /><asp:TextBox ID="txtMileage" runat="server" MaxLength="100" CssClass="text" Columns="3" /><span id="spnMileage"></span></td>
		        </tr>
                <tr id="trName" runat="server">
			        <td colspan="2">			
				        <span>Your Name <font color="red">*</font></span>
				        <div>					
					        <asp:TextBox ID="txtName" runat="server" MaxLength="50" Columns="35" CssClass="text" ToolTip="Max 50 characters." /><br />
					        <span id="spanName"></span>				
				        </div>
			        </td>
		        </tr>
		        <tr id="trEmail" runat="server">
			        <td colspan="2">			
				        <span>Your Email <font color="red">*</font></span>
				        <div>					
					        <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Columns="35" CssClass="text" ToolTip="Max 50 characters." /><br />
					        <span id="spnEmail"></span>				
				        </div>
			        </td>
		        </tr>
		        <tr>			
			        <td colspan="2" align="center">				
				        <div>
                            <asp:Button ID="butSave" CssClass="action-btn text_white" runat="server" Text="Post Review" />
                            <span class="margin-top5 margin-left10"><input type="button" class="action-btn text_white" value="Discard Review" onClick="javascript:location.href='<%= BackUrl%>'" /></span>
				        </div>
				        
			        </td>
		        </tr>
	        </table>
        </div>
    </div>
<script language="javascript">
    var displayVersion = '<%= displayVersion %>';
</script>
<script type="text/javascript" src="<%= staticUrlPath != "" ? "http://st1.aeplcdn.com" + staticUrlPath : "" %>/src/Research/write_reviews.js?<%= staticFileVersion %>"></script>
<!-- #include file="/includes/footerinner.aspx" -->