<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.SimilarCompareBikes" %>   
    <asp:Repeater ID="rptSimilarBikes" runat="server">        
        <ItemTemplate>   
               <div class="swiper-slide related-comparison-carousel-content">
                   <div class="font14 margin-top5 margin-bottom15"><%# DataBinder.Eval(Container.DataItem, "BikeName") %> : </div>
                    <asp:Repeater ID="rptSimilarBikesInner" runat="server" DataSource='<%# getChildData(Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionId"))) %>'> 
                        <ItemTemplate>                                                              
                            <a href="/m/<%# Bikewale.Utility.UrlFormatter.CreateCompareUrl(DataBinder.Eval(Container.DataItem,"MakeMasking1").ToString(),DataBinder.Eval(Container.DataItem,"ModelMasking1").ToString(),DataBinder.Eval(Container.DataItem,"MakeMasking2").ToString(),DataBinder.Eval(Container.DataItem,"ModelMasking2").ToString(),DataBinder.Eval(Container.DataItem,"VersionId1").ToString(),DataBinder.Eval(Container.DataItem,"VersionId2").ToString()) %>">
                                <%# Bikewale.Utility.UrlFormatter.CreateCompareTitle(DataBinder.Eval(Container.DataItem, "Make1").ToString(), DataBinder.Eval(Container.DataItem, "Model1").ToString(), DataBinder.Eval(Container.DataItem, "Make2").ToString(),DataBinder.Eval(Container.DataItem, "Model2").ToString())  %>
                            </a>
                        </ItemTemplate>      
                    </asp:Repeater>
               </div>         
        </ItemTemplate>      
    </asp:Repeater>
  