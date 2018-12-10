<%@ Page Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Editorial.SimilarGallery" Trace="false" Debug="false" EnableViewState="false" EnableSessionState=False%>
<%@ Register TagPrefix="Vspl" TagName="RepeaterPager" src="/Controls/RepeaterPagerPhotoGallery.ascx" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Import Namespace="Carwale.Utility" %>
<%@ Import Namespace="System.Data" %>
<div id="smAlertMsg" runat="server"><img src="https://img.carwale.com/adgallery/alert.gif" /></div>
<Vspl:RepeaterPager id="rpgListings" ResultName="Cars" ShowHeadersVisible="false" PagerPosition="TopBottom" runat="server">
	<asp:Repeater ID="rptListings" runat="server" EnableViewState="false">
	<headertemplate>
	<div class="dtTable" id="rptListings"  style="margin:0 auto; width:610px; height:250px;">		
	</headertemplate>
	<itemtemplate>
        <div class="img-placer">
			<a href="/research/<%#UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString()) %>-cars/<%# DataBinder.Eval(Container.DataItem, "ModelBase.MaskingName").ToString() %>/images/">			
            <div class="rollover-container">
                <div style="padding:5px 0 0 5px;">
				    <h4 class="rollover-text"><%#DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %></h4><p><%#DataBinder.Eval(Container.DataItem, "ImageTitle").ToString() %> <br />See More &raquo;</p>
                </div>
			</div>	
            <div class="imgBox"><img alt='<%#DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>' src='<%# ImageSizes.CreateImageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"HostUrl")),ImageSizes._174X98,Convert.ToString(DataBinder.Eval(Container.DataItem,"OriginalImgPath")))%>' width="160px" height="100px"/></div>            
			</a>
		</div>  		
	</itemtemplate>
	<footertemplate>
	</div>
	</footertemplate>
	</asp:Repeater>
</Vspl:RepeaterPager>