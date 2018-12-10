<%@ Control Language="C#" AutoEventWireUp="false"  Inherits="Carwale.UI.Controls.NewsRightWidget" %>
<%@ Import Namespace="Carwale.Entity.CMS.Articles" %>
<style>
    .cw-tabs li h2 { width: 145px !important; }
</style>

    <% if(basicId != 0) {%>
    <div>
    <div class="content-inner-block-10">
        <h2 class="font18 text-bold">Things You Shouldn't Miss</h2>
    </div>
    <div class="content-inner-block-10">
        <div class="cw-tabs" id="news-tabs">
        	<ul id="head">
                <li id="popular_News"><h2 class="active first">Popular News</h2></li>
                <li id="recent_News"><h2 >Recent News</h2></li>
               
            </ul>
            <div class="clear"></div>
        </div>

              <div class="news-tab-data">
        	    <div class="data-view" id="popularNews">
                    <ul>
                    <asp:Repeater ID="rptPopularNews" runat="server">
                        <ItemTemplate>
                            <li>
                                <div class="news-title-widget"><a href="<%# Eval("ArticleUrl") %>" title="<%# DataBinder.Eval(Container.DataItem,"Title") %>"><%# DataBinder.Eval(Container.DataItem,"Title") %></a></div>
                                <div class="news-title-count" runat="server"><%# Container.ItemIndex +1 %></div>
                                <div class="clear"></div>
                            </li>
                        </ItemTemplate>
                         </asp:Repeater>
                        </ul>
                     </div>
               </div>

               <div class="news-tab-data">
                  <div class="data-view" >
                    <ul id="forIeRecent" class="hide">
                        <asp:Repeater ID="rptRecentNews" runat="server">
                         <ItemTemplate>
                            <li>
                                <div class="news-title-widget"><a href="<%# Eval("ArticleUrl") %>" title="<%# DataBinder.Eval(Container.DataItem,"Title") %>"><%# DataBinder.Eval(Container.DataItem,"Title") %></a></div>
                                <div class="news-title-count"><%# Container.ItemIndex +1 %></div>
                                <div class="clear"></div>
                            </li>
                        </ItemTemplate>
                       </asp:Repeater>
                    </ul>
                 </div>
              </div>
     </div>
    </div> 
    <%} %>

    <% if (basicId == 0)
       {%>
    <div class="content-inner-block-10">
        <div>
        <h2 class="font18 text-bold">Popular News</h2>
        </div>
        <div>
         
        <div class="news-tab-data">
            <div class="data-view" >
                <ul>
                    <asp:Repeater ID="rptListPagePoularNews" runat="server">
                    <ItemTemplate>
                        <li>
                            <div class="news-title-widget"><a href="<%# Eval("ArticleUrl") %>" title="<%# DataBinder.Eval(Container.DataItem,"Title") %>"><%# DataBinder.Eval(Container.DataItem,"Title") %></a></div>
                            <div class="news-title-count" runat="server"><%# Container.ItemIndex +1 %></div>
                            <div class="clear"></div>
                        </li>
                    </ItemTemplate>
                    </asp:Repeater>
                </ul>
             </div>
        </div>
    </div>
    </div>
    <%} %>
<script>
    
    $("#head li").click(function () {
        var liValue = $(this).attr("id");

        if (liValue == "popular_News")
        {
            $("#popularNews").show();
            $("#recentNews").hide();
            $("#forIeRecent").hide();
        }
        else 
        {
            $("#recentNews").show();
            $("#popularNews").hide();
            $("#forIeRecent").show();
        }
    });
    
</script>









      
     



   








    
