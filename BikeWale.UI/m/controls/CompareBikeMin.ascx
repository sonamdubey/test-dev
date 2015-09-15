<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.CompareBikeMin" %>
<style>
    @media screen and (min-width:315px) {
        .compareImageContainer {width:100%;margin:auto;}
    }

    @media screen and (max-width:329px) {
        .compareImageContainer img {max-width:100%;height:auto;}
    
    }
</style>
<h2>Popular Comparisons</h2>
<asp:Repeater ID="rptCompareList" runat="server">
    <ItemTemplate>
        <a href="/m/comparebikes/<%#DataBinder.Eval(Container.DataItem,"MakeMaskingName1")%>-<%#DataBinder.Eval(Container.DataItem,"ModelMaskingName1")%>-vs-<%#DataBinder.Eval(Container.DataItem,"MakeMakingName2")%>-<%#DataBinder.Eval(Container.DataItem,"ModelMaskingName2")%>/" class="normal">
            <div class="box1 new-line5">
                <div class="compareImageContainer">
                    <%--<img src="<%# Bikewale.Common.ImagingFunctions.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "ImagePath").ToString()+DataBinder.Eval(Container.DataItem,"ImageName").ToString() , DataBinder.Eval(Container.DataItem,"HostURL").ToString()) %>" /></div>--%>
                    <img src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem,"HostURL").ToString(),Bikewale.Utility.ImageSize._640x348) %>" /></div>

                <div style="text-align: center;"><%# DataBinder.Eval( Container.DataItem, "Bike1" ) %>&nbsp;<span class="red-text">vs</span>&nbsp;<%# DataBinder.Eval( Container.DataItem, "Bike2" ) %></div>
            </div>
        </a>
    </ItemTemplate>
</asp:Repeater>
