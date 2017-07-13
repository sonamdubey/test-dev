<%@ Page Language="C#" Inherits="Bikewale.Used.UploadPreview" Trace="false" Debug="false" AutoEventWireUp="false" %>
<%@ Import Namespace="Bikewale.Common" %>
<% Title = "Bike Photos Preview"; %>
<!-- #include file="sell_header.aspx" -->
<script language="javascript" src="/src/common/bt.js?v1.1"></script>
<!--[if IE]><script language="javascript" src="/src/common/excanvas.js?v=1.0"></script><![endif]-->
<script type="text/javascript" src="<%= staticUrl %>/src/common/process.js?v=1.0"></script>
<script language="javascript">
	var inquiryId = '<%= inquiryId %>';	
	var requestCount = 0;
	var responseCount = 0;
	nextStepUrl = "/used/sell/confirmation.aspx";
</script>
<div class="sell_container">
	<div class="sell_block moz-round">
		<h2 class="hd2-red"><span><a title="Add images to your listing" href="uploadphotos.aspx">Add <%= objPhotos.ClassifiedImageCount != 0 ? "More" : "" %> Photos</a></span> Bike Photos Preview <span class="price2">+</span></h2>		
		<div class="mid-box"><h2 class="hd2"><%= objPhotos.ClassifiedImageCount %> Photos available with this listing</h2></div>
		<asp:Repeater ID="rptImageList" runat="server">
			<itemtemplate>
				<div id="<%# DataBinder.Eval(Container.DataItem,"Id")%>" class="img-preview">
					<table width="100%" border="0">
						<tr>
							<td width="100"><img class='img-border' src="<%# ImagingFunctions.GetImagePath("/ucp/", DataBinder.Eval(Container.DataItem,"HostUrl").ToString() ) %>S<%= inquiryId + "/" %><%# DataBinder.Eval(Container.DataItem,"ImageUrlThumbSmall")%>" /></td>
							<td width="250"><textarea id="desc<%# DataBinder.Eval(Container.DataItem,"Id")%>" onfocus="if (this.value == 'Describe this image here') { this.value=''; }" onblur="if (this.value == '') { this.value='Describe this image here'; }" rows="3" cols="30" class="text-grey">Describe this image here</textarea></td>
							<td width="200"><input type="radio" id="rdo<%# DataBinder.Eval(Container.DataItem,"Id")%>" name="mianimg" onclick="javascript:makeMainImg('<%# DataBinder.Eval(Container.DataItem,"Id")%>')" />Make Profile Image[<a title="This will be the main photo displayed in search results." class="front-img">?</a>]</td>
							<td><a id="remove<%# DataBinder.Eval(Container.DataItem,"Id")%>" onclick="javascript:deleteImg('<%# DataBinder.Eval(Container.DataItem,"Id")%>')" class="icons-sell delete-photo"></a></td>								
						</tr>
					</table>
				</div>
			</itemtemplate>
		</asp:Repeater>
		<div id="done" class="mid-box" align="right"><a class="buttons" onclick="javascript:mDone();">I'm Done</a></div>			
	</div>	
</div>
<script language="javascript">
	$(".front-img").bt({fill: '#FCF5A9',strokeWidth: 1,strokeStyle: '#D3D3D3',spikeLength:20,shadow: true,positions:['right']});
</script>
<!-- #include file="sell_footer.aspx" -->