<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.SimilarCompareBikes" %>
    &nbsp;People who compared above bikes also compared: <br /><br />
    <asp:Repeater ID="rptSimilarBikes" runat="server">        
        <ItemTemplate>
            <div class="related-comparison-wrapper padding-left10 padding-right10 margin-bottom15">
                <a href='/<%# Bikewale.Utility.UrlFormatter.CreateCompareUrl(DataBinder.Eval(Container.DataItem,"MakeMasking1").ToString(),DataBinder.Eval(Container.DataItem,"ModelMasking1").ToString(),DataBinder.Eval(Container.DataItem,"MakeMasking2").ToString(),DataBinder.Eval(Container.DataItem,"ModelMasking2").ToString(),DataBinder.Eval(Container.DataItem,"VersionId1").ToString(),DataBinder.Eval(Container.DataItem,"VersionId2").ToString()) %>'>
                   <%# Bikewale.Utility.UrlFormatter.CreateCompareTitle(DataBinder.Eval(Container.DataItem, "Make1").ToString(), DataBinder.Eval(Container.DataItem, "Model1").ToString(), DataBinder.Eval(Container.DataItem, "Make2").ToString(),DataBinder.Eval(Container.DataItem, "Model2").ToString())  %>
                 </a>
            </div>
        </ItemTemplate>        
    </asp:Repeater>
