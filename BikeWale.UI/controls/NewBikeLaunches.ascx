<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.NewBikeLaunches" %>
<%@ Import Namespace="Bikewale.Common" %> 
<style>
	.nl-ver {width:100%;}	
	.nl-ver .nl-bike .nl-image{float:left;width:110px;height:65px;border:1px solid #E1E1E1;background-repeat: no-repeat; background-position: center;}
	.nl-ver .nl-bike .nl-links{float:left;width:140px;height:80px;margin-left:5px;}
	.nl-ver .nl-bike .nl-clear{clear:both;}
</style>
<div id="divNewLaunch" runat="server" class="nl-ver">
    <h2>New Launches</h2>
    <div class="margin-bottom15">Check out just launched bikes, their prices, specs, pictures etc.</div>
    <asp:Repeater ID="rptData" runat="server">
	    <itemtemplate>
		    <div class="nl-bike">
			    <a href='/<%# DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString()%>-bikes/<%#DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() %>/'>				    
                    <div class="nl-image" style="background-image: url('<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem,"OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem,"HostUrl").ToString(),Bikewale.Utility.ImageSize._110x61) %>');" title="<%# String.Format("{0} {1}", DataBinder.Eval(Container.DataItem,"Make"), DataBinder.Eval(Container.DataItem,"Model")) %>"></div>
			    </a>
			    <div class="nl-links">
				    <a href='/<%# DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString()%>-bikes/<%#DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() %>/'><%# String.Format("{0} {1}", DataBinder.Eval(Container.DataItem,"Make"),DataBinder.Eval(Container.DataItem,"Model")) %></a>
                    <br/><span>Starts At : Rs. <%#!String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MinPrice").ToString()) ? CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"MinPrice").ToString() ): "N/A"%></span>

				    <br/><a class="href-grey getquotation" data-pqSourceId="<%= PQSourceId%>" href="/pricequote/default.aspx?model=<%# DataBinder.Eval(Container.DataItem,"ModelId") %>" data-modelId="<%# DataBinder.Eval(Container.DataItem,"ModelId") %>" style="<%# (DataBinder.Eval(Container.DataItem,"MinPrice").ToString()!="0")?"":"display:none"%>">Check On-Road Price</a>

			    </div>
			    <div class="nl-clear"></div>
		    </div>
	    </itemtemplate>
    </asp:Repeater>

    <div class="readmore margin-top20 right-float"><a href="/new-bike-launches/" >All New Launches</a></div>
</div>
<div class="clear"></div>
