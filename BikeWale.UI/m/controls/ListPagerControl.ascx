<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.ListPagerControl" %>
<table style="width:100%; margin-top:20px;" cellspacing="0" cellpadding="0" border="0" class="new-line5">
	<tr>
         <%if(TotalPages > 1 ) { %>
		        <td style="width:60px;">
			        <%if (!String.IsNullOrEmpty(prevPageUrl))
                    {%>
				        <a class="normal" href="<%= prevPageUrl %>">
				            <span><span class="arr-big position-rel" style="top:2px;">&laquo;</span>&nbsp;Prev</span>
				        </a>
			        <%}%>
		        </td>
		        <td style="text-align:center;">
			        Page 
                    <asp:DropDownList id="ddlPage" runat="server" data-role="none" data-inline="true" data-theme="a" data-mini="true" style="margin:0px 8px;">
                    </asp:DropDownList>
		            of <span id="Span1"><%= TotalPages %></span>
		        </td>
		        <td style="width:60px;">
			        <%if (!String.IsNullOrEmpty(nextPageUrl))
                    {%>
				        <a class="normal" href="<%= nextPageUrl %>">
				            <span style="position:relative;top:-4px;">Next<span class="arr-big position-rel" style="top:2px;">&nbsp;&raquo;</span></span>
				        </a>
			        <%}%>
		        </td>
           <%} %>
	</tr>
</table>
<script type="text/javascript">
    var ddlPage = "#<%=ddlPage.ClientID.ToString()%>";
    $(ddlPage).change(function () {
        window.location= $(this).val();
    });

</script>