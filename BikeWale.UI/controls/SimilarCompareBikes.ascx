<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.SimilarCompareBikes" %>
    &nbsp;People who compared above bikes also compared: <br /><br />
    <asp:Repeater ID="rptSimilarBikes" runat="server">        
        <ItemTemplate>
            <div class="grid-3 margin-bottom10">
                <a href='/<%# Bikewale.Utility.UrlFormatter.CreateCompareUrl(DataBinder.Eval(Container.DataItem,"MakeMasking1").ToString(),DataBinder.Eval(Container.DataItem,"ModelMasking1").ToString(),DataBinder.Eval(Container.DataItem,"MakeMasking2").ToString(),DataBinder.Eval(Container.DataItem,"ModelMasking2").ToString(),DataBinder.Eval(Container.DataItem,"VersionId1").ToString(),DataBinder.Eval(Container.DataItem,"VersionId2").ToString()) %>'>
                   <%# Bikewale.Utility.UrlFormatter.CreateCompareTitle(DataBinder.Eval(Container.DataItem, "Make1").ToString(), DataBinder.Eval(Container.DataItem, "Model1").ToString(), DataBinder.Eval(Container.DataItem, "Make2").ToString(),DataBinder.Eval(Container.DataItem, "Model2").ToString())  %>
                 </a>
            </div>
        </ItemTemplate>        
    </asp:Repeater>
