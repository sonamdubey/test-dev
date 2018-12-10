<%@ Control Language="C#" AutoEventWireUp="false" Inherits="Carwale.UI.Controls.UCSimilarCars" %>
<%@ Import Namespace="Carwale.UI.Common"%>
<style>
	.sc-ver {width:100%;}
	.sc-ver .lt {width:100%;float:left;padding:8px 2px;}
	.sc-ver .lt img {float:left;border:1px solid #E1E1E1;padding:8px 5px;margin-right:5px;background-color:#ffffff;}
	
	.sc-hor {width:100%;}
	.sc-hor .lt {width:158px;float:left;padding:8px 0px;}
	.sc-hor .lt img {float:left;border:1px solid #E1E1E1;padding:5px;margin-right:46px;background-color:#ffffff;}
	.featuredSim {background-color:#FBF5B2;}
</style>

<% if(rptCars.Items.Count>0) { %>
<div runat="server" class="sc-ver">
<asp:Repeater ID="rptCars" runat="server">
	<itemtemplate>
		<div class="lt<%# ((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).IsFeatured.ToString() == "1" ? " featuredSim" : "" %>">
			<a href='<%# GetLandingUrl(((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).IsFeatured.ToString(),((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).MakeName,((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).MaskingName,((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).SpotlightUrl) %>'>
				<img class="lazy" src="<%# Carwale.Utility.ImageSizes.CreateImageUrl(((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).HostUrl, Carwale.Utility.ImageSizes._110X61,((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).ModelImageOriginal) %>" data-original='<%# Carwale.Utility.ImageSizes.CreateImageUrl(((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).HostUrl, Carwale.Utility.ImageSizes._110X61,((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).ModelImageOriginal) %>' title="<%#((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).MakeName %> <%#((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).ModelName%>" alt="<%#((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).MakeName %> <%# ((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).ModelName %>"/>
			</a>
			<a class="text-grey text-bold" href='<%# GetLandingUrl(((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).IsFeatured.ToString(),((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).MakeName, ((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).MaskingName,((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).SpotlightUrl) %>'><%# ((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).MakeName%> <%# ((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).ModelName%></a>
			<div title="Ex-showroom, <%= ConfigurationManager.AppSettings["DefaultCityName"] %>" class="fontblack margin-top5">₹ <%# FormatPrice.FormatFullPrice(((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).MinPrice.ToString(), ((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).MaxPrice.ToString()) %></div>			
            <div><a href="javascript:void(0)" class="font12"  onclick="GetPriceQuote('','<%# ((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).ModelId%>','<%=PQPageId%>',this)">Check on-road price</a></div>
            <% if(MakeName != "" && ModelName != "") { %>
            <div class="margin-top5 ccsm">
                <a class="font12" href="<%#((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).CompareCarUrl%>" title="Compare <%= ModelName %> with <%# ((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).ModelName %> ">Compare with <%= ModelName %></a>
            </div>
            <% } %>
			<%# ((Carwale.Entity.CarData.SimilarCarModels)Container.DataItem).IsFeatured.ToString() == "1" ? "<p class='text-grey' style='float:right;padding-right:2px;'>sponsored</p>" : "" %>				
		</div></itemtemplate>
</asp:Repeater>
</div>
<%} %>
<div class="clear"></div>
