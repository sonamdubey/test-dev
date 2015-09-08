<%@ Page AutoEventWireUp="false" Inherits="Bikewale.New.BikeComparison" Language="C#" trace="false" Debug="false" %>
<%
    title = "Compare Colours - " + pageTitle;
    description = "CarWale&reg; - Compare " + pageTitle + " colours. Compare various colours of the cars on CarWale.";
%>
<!-- #include file="/includes/headNew.aspx" -->
<script type="text/javascript">
	$(document).ready(function(){
		if( $("#tblCompare").length > 0 ){
			var featuredBikeIndex = '<%= featuredBikeIndex + 2 %>';					
			$("#tblCompare tr td:nth-child("+ featuredBikeIndex +")").css({"background-color":"#FCF8D0"});
			$("#tblCompare tr:first td:nth-child("+ featuredBikeIndex +")").html("Sponsored Bike").addClass("td-featured");
		}
	});
</script>
<div class="container_12">
     <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/new/compare/">Compare Bikes in India</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>Bike Comparison</strong></li>
        </ul><div class="clear"></div>
    </div>
    <div class="grid_12 margin-top10"><!--    Left Container starts here --> 
        <span id="spnError" class="error" runat="server"></span>
		<h1 class="margin-bottom15">Colours Comparison - <asp:Literal id="ltrTitle" runat="server"></asp:Literal></h1>
		<div class="padding-top20 featured-bike-tabs">
            <ul class="featured-bike-tabs-inner padding-top20">
                <li class="first_tab"><a href="compareSpecs.aspx?bike1=<%=Request["bike1"]%>&amp;bike2=<%=Request["bike2"]%>&amp;bike3=<%=Request["bike3"]%>&amp;bike4=<%=Request["bike4"]%>">Specs</a></li>
                <li><a href="compareFeatures.aspx?bike1=<%=Request["bike1"]%>&amp;bike2=<%=Request["bike2"]%>&amp;bike3=<%=Request["bike3"]%>&amp;bike4=<%=Request["bike4"]%>">Features</a></li>
                <li class="fbike-active-tab">Colours</li>                          
            </ul>
        </div>	
		<div class="tab_inner_container">
			<table width="100%" id="tblCompare" runat="server" class="tbl-compare" cellpadding="5" border="0" cellspacing="0">
			<!-- #include file="compareCommon.aspx" -->						
				<tr class="headerSpecs">
					<th>Available Colors</th>
					<th><%= bike[0] %></th>
					<th><%= bike[1] %></th>
					<th><%= bike[2] %></th>
					<th><%= bike[3] %></th>
					<th><%= bike[4] %></th>
				</tr>
				
				<tr>
				    <td width="180" valign="top" class="headerSpecs">&nbsp;</td>
				    <td valign="top"><%=GetModelColors( versionId[0], 0 ) %></td>
				    <td valign="top"><%=GetModelColors( versionId[1], 1 ) %></td>
				    <td valign="top"><%=GetModelColors( versionId[2], 2 ) %></td>
				    <td valign="top"><%=GetModelColors( versionId[3], 3 ) %></td>
				    <td valign="top"><%=GetModelColors( versionId[4], 4 ) %></td>
				</tr>
			</table>
		</div>	  
    </div> 
</div>     
<!-- #include file="/includes/footerInner.aspx" -->
