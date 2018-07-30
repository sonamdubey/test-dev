<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.PhotoGallery.SimilarGallery" Trace="false" Debug="false" %>
<%@ Register TagPrefix="BikeWale" TagName="RepeaterPager" src="/Controls/RepeaterPagerPhotoGallery.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="System.Data" %>
<div id="smAlertMsg" runat="server"><img src="http://imgd3.aeplcdn.com/0x0/bw/static/design15/old-images/d/alert.gif" /></div>
<BikeWale:RepeaterPager id="rpgListings" ResultName="Bikes" ShowHeadersVisible="false" PagerPosition="TopBottom" runat="server">
	<asp:Repeater ID="rptListings" runat="server" EnableViewState="false">
	<headertemplate>
	    <div class="dtTable" id="rptListings"  style="margin:0 auto; width:610px; height:250px;">		
	</headertemplate>
	<itemtemplate>
        <div class="img-placer">
			<a href="/<%#DataBinder.Eval(Container.DataItem, "MakeMaskingName").ToString() %>-bikes/<%#DataBinder.Eval(Container.DataItem, "ModelMaskingName").ToString() %>/photos/">			
            <div class="rollover-container">
                <div style="padding:5px 0 0 5px;">
				    <h4 class="rollover-text"><%#DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelName").ToString() %></h4><p><%#DataBinder.Eval(Container.DataItem, "ArticleTitle").ToString() %> <br />See More &raquo;</p>
                </div>
			</div>	
            <div class="imgBox"><img src='<%#DataBinder.Eval(Container.DataItem,"MainImage") %>' width="160px" height="100px"/></div>            
			</a>
		</div>  		
	</itemtemplate>
	<footertemplate>
	    </div>
	</footertemplate>
	</asp:Repeater>
</BikeWale:RepeaterPager>