<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.RecentLaunchedBikesMin" %>
<style>
    h3 { height:40px; }
    table td { height:20px;}
</style>
<div id="divControl" runat="server">
    <asp:Repeater ID="rptLaunchedBikes" runat="server">        
        <ItemTemplate>
            <li>
                <div class="grid_3">
                    <div style='height:270px;' class="border-light border-radius5 content-block">
                        <a href="/<%# DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%#DataBinder.Eval(Container.DataItem,"MaskingName") %>/">
                            <%--<img alt="<%#DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelName") %>" title="<%#DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelName") %>" src="<%# Bikewale.Common.ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/" + DataBinder.Eval(Container.DataItem,"LargePicUrl").ToString(),DataBinder.Eval(Container.DataItem,"HostUrl").ToString()) %>" width="196" hspace="0" vspace="0" border="0" />--%>
                            <img alt="<%#DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelName") %>" title="<%#DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelName") %>" src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem,"HostUrl").ToString(),Bikewale.Utility.ImageSize._210x118) %>" width="196" hspace="0" vspace="0" border="0" />
                        </a>                        
                        <h3 class="margin-top5">
                            <a href="/<%# DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%#DataBinder.Eval(Container.DataItem,"MaskingName") %>/"><%#DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelName") %></a>
                        </h3>
                        <div class="margin-top5 margin-bottom10" style="border-bottom:1px dotted #E9E9E9;"></div>
                        <div>
                            <div><b>Price : Rs. <%#Bikewale.Common.CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"MinPrice").ToString()) %></b></div>
                            <span class="ex-showroom">Ex-showroom, <%= ConfigurationManager.AppSettings["defaultName"].ToString() %></span>                        
                        </div>
                        <div class="margin-top5 margin-bottom10" style="border-bottom:1px dotted #E9E9E9;"></div>
                        <div >                                        	
                            <table border="0" cellspacing="0" cellpadding="0">
                                <tbody><tr>
                                    <td width="120">
                                        <span class="leftfloat">
                                            &#62;&nbsp;<a class="getquotation" data-modelId="<%# DataBinder.Eval(Container.DataItem,"ModelId") %>" href="/pricequote/default.aspx?model=<%# DataBinder.Eval(Container.DataItem,"ModelId") %>" title="Check <%#DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelName") %> On-road Price">On-Road Price</a>
                                        </span>
                                    </td>
                                    <td width="100">
                                        <span class="leftfloat">
                                            &#62;&nbsp;<a href="/<%#DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%#DataBinder.Eval(Container.DataItem,"MaskingName") %>/user-reviews/" title="<%#DataBinder.Eval(Container.DataItem,"MakeBase.MakeName") + " " + DataBinder.Eval(Container.DataItem,"ModelName") %> User Reviews">User Reviews</a>
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="leftfloat">
                                            &#62;&nbsp;<a href="/<%#DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%#DataBinder.Eval(Container.DataItem,"MaskingName") %>/#specification-and-features">Spec &amp; Features</a>
                                        </span>
                                    </td>
                                    <td>
                                        <span class="leftfloat ">
                                            &#62;&nbsp;<a href="/<%#DataBinder.Eval(Container.DataItem,"MakeBase.MaskingName") %>-bikes/<%#DataBinder.Eval(Container.DataItem,"MaskingName") %>/images/" >Images</a>
                                        </span>
                                    </td>                                                                                        
                                </tr>
                            </tbody></table>
                        </div>             
                    </div>
                </div>
            </li>
        </ItemTemplate>       
    </asp:Repeater>
</div>