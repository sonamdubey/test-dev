<%@ Page Language="C#" AutoEventWireup="true"  Inherits="Bikewale.Used.BikesByModel" %>
<%
    title = "Model wise used bikes listing - BikeWale";
    keywords = "Model wise used bikes listing,used bikes for sale, second hand bikes, buy used bike";
    description = "bikewale.com Model wise used bikes listing.";
%>
<!-- #include file="/includes/headUsed.aspx" -->
    <div  class="container_12">
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
                    <a href="/used/" itemprop="url">
                        <span itemprop="title">Used Bikes</span>
                    </a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                    <a href="/used/bikes-in-india/" itemprop="url">
                        <span itemprop="title">Search</span>
                    </a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong>Bikes by Model</strong></li>
            </ul>
        </div>
        <div class="clear"></div>
        <!-- Left container start here-->
        <h1 class="grid_8  margin-top10">Model wise used bike listings</h1>
        <div id="divContent" class="grid_8 margin-top10 border-light">
            <asp:DataList ID="dlModel" runat="server"
	            RepeatColumns="4"
	            RepeatDirection="Horizontal" CellPadding="5" CellSpacing="5"  
	            RepeatLayout="Table" style="font-size:13px;">
            <itemtemplate>
	           <a class="link-decoration" href="/used/<%#DataBinder.Eval( Container.DataItem, "MakeMaskingName") %>-<%#DataBinder.Eval( Container.DataItem, "ModelMaskingName")%>-bikes-in-india/">
		            <%# DataBinder.Eval(Container.DataItem, "MakeName") %>  <%#DataBinder.Eval( Container.DataItem, "ModelName") %> (<%# DataBinder.Eval(Container.DataItem, "ModelCount") %>)
	            </a>
            </itemtemplate>
            </asp:DataList>
        </div>
        <!-- Left container ends here-->
    </div>
<!-- #include file="/includes/footerInner.aspx" -->
