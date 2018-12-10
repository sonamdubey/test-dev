<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Carwale.UI.Controls.CommonPager" %>
<%@ Import Namespace="Carwale.Entity" %>

<style>	
    .footerStrip a { display:inline-block; text-decoration:none; }
    .footerStrip span.pg:hover, 
    .footerStrip span.pgSel:hover { background : #ccc; }
    span.pg{padding:2px 5px; border:1px solid #A3B5D9; margin:0 2px;}
    span.pgSel{background-color:#CCDBF8; padding:2px 5px; border:1px solid #A3B5D9; margin:0 2px; color:#5B5B5B; font-weight:bold;}
	span.pgEnd{padding:2px 5px; border:1px solid gray; margin:0 2px; color:gray;  }
    span.pgEnd a { cursor:default;}
    span.pgEnd a:hover { color:#034FB6; }
	.dgNavDiv td{padding:5px;}
</style>

<span id="firstPageSpan" runat="server" hidden="hidden" > <asp:HyperLink  ID="firstPage" runat="Server" Text="FIRST" ></asp:HyperLink> </span>
<span id="prevPageSpan" runat="server"> <asp:HyperLink  ID="prevPage" runat="Server" Text="PREV"  ></asp:HyperLink> </span>
<asp:Repeater id="rptPagerList" runat="server">
    <ItemTemplate>    
        <a href=<%# ((PagerUrlList)Container.DataItem).PageUrl %>>
            <span class='<%# ApplyPageClass(((PagerUrlList)Container.DataItem).PageNo.ToString())%>'><%# ((PagerUrlList)Container.DataItem).PageNo %></span> 
        </a>
    </ItemTemplate>
</asp:Repeater>
<span id="nextPageSpan" runat="server"> <asp:HyperLink  ID="nextPage" runat="Server" Text="NEXT" ></asp:HyperLink> </span>

<span id="lastPageSpan" runat="server" hidden="hidden"> <asp:HyperLink  ID="lastPage" runat="Server" Text="LAST" ></asp:HyperLink> </span>

<%--<script>
$(document).ready(function() {
    $('a.NoLink').removeAttr('href')
});
    </script>--%>