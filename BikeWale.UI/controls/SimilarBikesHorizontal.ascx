<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.SimilarBikesHorizontal" %>
<div id="divControl" class='<%= recordCount > 0 ? "" :"hide" %>'>
    <div class="<%= recordCount > 0 ? "inner-content" : "hide" %>">
        <%if (bikeVersionEntity != null) {  %>
        <h2 class="margin-bottom5"><%=bikeVersionEntity.MakeBase.MakeName %> <%=bikeVersionEntity.ModelBase.ModelName %> Alternatives</h2>
        <div class="margin-bottom10">
        <span>Not sure about the <%=bikeVersionEntity.MakeBase.MakeName %> <%=bikeVersionEntity.ModelBase.ModelName %>? Check out these bikes which are similar to the <%=bikeVersionEntity.MakeBase.MakeName %> <%=bikeVersionEntity.ModelBase.ModelName %> and compete with it in terms of price and features.</span>
        </div>
        <% } %>
        <div class="margin-top10">
            <asp:Repeater ID="rptSimilarBikes" runat="server">
                <ItemTemplate>
                    <div class="grid_4 alpha omega">
                        <%--<div class="margin-bottom5">
                            <a href='/<%# DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%#DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName") %>/'><b><%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName")+ " " + DataBinder.Eval(Container.DataItem,"ModelBase.ModelName") %></b></a>
                        </div>--%>
                        <div class="grid_2 alpha omega">
                            <%--<a href='/<%# DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%#DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName") %>/'><img width="130" vspace="0" hspace="0" border="0" src='<%# Bikewale.Common.ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"LargePicUrl").ToString(),DataBinder.Eval(Container.DataItem,"HostUrl").ToString())%>' hspace="0" vspace="0" border="0"/></a>--%>
                            <a href='/<%# DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%#DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName") %>/'><img width="130" vspace="0" hspace="0" border="0" src='<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem,"HostUrl").ToString(),Bikewale.Utility.ImageSize._210x118)%>' hspace="0" vspace="0" border="0"/></a>
                        </div>
                        <div class="grid_2 alpha omega">
                            <a href='/<%# DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%#DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName") %>/'><b><%# DataBinder.Eval(Container.DataItem,"MakeBase.MakeName")+ " " + DataBinder.Eval(Container.DataItem,"ModelBase.ModelName") %></b></a>
                            <p class="margin-top5"><span class="WebRupee">Rs.</span><strong><%#Bikewale.Common.CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"VersionPrice").ToString()) %></strong></p>
                            <div><a class="blue fillPopupData" href='/pricequote/default.aspx?model=<%#DataBinder.Eval(Container.DataItem,"ModelBase.ModelId") %>'  modelId="<%#DataBinder.Eval(Container.DataItem,"ModelBase.ModelId") %>">Check On-Road Price</a></div>
                            <%if (bikeVersionEntity != null && IsNew) {  %>
                            <a class="blue" href='/comparebikes/<%=bikeVersionEntity.MakeBase.MaskingName %>-<%=bikeVersionEntity.ModelBase.MaskingName %>-vs-<%#DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-<%#DataBinder.Eval(Container.DataItem,"ModelBase.MaskingName") %>/'>Compare with <%= bikeVersionEntity.ModelBase.ModelName %></a>
                            <% } %>
                        </div>
                    </div>                    
                </ItemTemplate>
            </asp:Repeater>
            <div class="clear"></div>
        </div>
    </div>