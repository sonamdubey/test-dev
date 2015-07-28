<%@ Control Language="C#" Inherits="Bikewale.Mobile.Controls.TopUserReviews" %>
<%if(totalReviews > 0) {%>
<div class="new-line5">
    <h2 class="pgsubhead"><%= _headerText %></h2>
</div>
<div id="allReviews" class="box new-line5" style="padding:0px 5px;">
    <asp:Repeater id="rptUserReviews" runat="server">
        <itemtemplate>
            <a href='/m/<%= makeMaskingName %>-bikes/<%= modelMaskingName %>/user-reviews/<%#DataBinder.Eval(Container.DataItem,"ReviewId")%>.html' class="normal f-12">   
                <div class="container">
                    <div class="sub-heading">
				        <b><%#DataBinder.Eval(Container.DataItem,"ReviewTitle") %></b> &nbsp;&nbsp;<span class="arr-small">&raquo;</span>
			        </div>    
	                <div class="darkgray new-line5">
		                <%# Bikewale.Common.CommonOpn.GetDisplayDate(DataBinder.Eval(Container.DataItem,"ReviewDate").ToString()) %> | By <%# DataBinder.Eval(Container.DataItem,"WrittenBy") %>
	                </div>
                    <div class="new-line5" style="font-size:0px;">
                        <%# Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"OverAllRating.OverAllRating"))) %>            
                    </div>
                </div>
            </a>                
        </itemtemplate>
    </asp:Repeater>
</div>
<%} %>
