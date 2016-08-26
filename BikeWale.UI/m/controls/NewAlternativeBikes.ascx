<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.NewAlternativeBikes" %>
<!-- Most Alternative Bikes Starts here-->
<div id="modelAlternateBikeContent" class="bw-model-tabs-data padding-top15 font14 margin-bottom20">
    <h2 class="padding-left20 padding-right20 margin-bottom20">Alternative bikes for <%=modelName%></h2>
    <div class="swiper-container padding-top5 padding-bottom5">
        <div class="swiper-wrapper font14"> 
          <asp:Repeater ID="rptAlternateBikes" runat="server">   
            <ItemTemplate>
                    <div class="swiper-slide">
                        <div class="model-swiper-image-preview">
                            <a href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName"))) %>'>
                                <img class="swiper-lazy" data-src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" alt="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>" />
                                <span class="swiper-lazy-preloader"></span>
                            </a>
                        </div>
                        <div class="model-swiper-details">
                            <a href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName"))) %>'
                                 class="target-link font13 text-truncate" title="<%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %>"><%# DataBinder.Eval(Container.DataItem, "MakeBase.MakeName").ToString() + " " + DataBinder.Eval(Container.DataItem, "ModelBase.ModelName").ToString() %></a>
                            <p class="text-truncate text-light-grey font12 margin-top5">Ex-showroom, <%= ConfigurationManager.AppSettings["defaultName"] %></p>
                            <p class="font18 text-bold margin-bottom10">
                                <span class="bwmsprite inr-xsm-icon"></span>
                                <span><%# Bikewale.Utility.Format.FormatPrice(DataBinder.Eval(Container.DataItem, "MinPrice").ToString()) %></span>
                            </p>
                            <a href="javascript:void(0)" data-pqSourceId="<%= PQSourceId %>" data-modelid="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ModelBase.ModelId")) %>" class="<%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))!="0")?"":"hide" %> btn btn-xs btn-full-width btn-white margin-top10 getquotation font12" rel="nofollow">Check on-road price</a>
                        </div>
                    </div>         
                </ItemTemplate>
            </asp:Repeater>
            
        </div>
    </div>
    <!--- Most Alternative Bikes Ends Here-->

</div>