<%@ Page Language="C#" AutoEventWireup="true" Inherits="AutoExpo.BrandDetails" Trace="false" Debug="false" %>

<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId          = 5;
	Title 			= "Auto Expo 2014 - BikeWale";
	Description 	= "BikeWale's coverage on Auto Expo 2014, the largest auto show in India.";
	Keywords		= "auto expo, auto expo 2014, auto show india, auto expo delhi";
	Revisit 		= "5";
	DocumentState 	= "Static";
    canonical       = "http://bikewale.com/autoexpo/2014/brand.aspx";
%>
<!-- #include file="/autoexpo/includes/headNews.aspx" -->
<script type="text/javascript">
    $(document).ready(function (e) {
        //$('#dropdown-nav .dropdown-menu').dropdown_menu({
        //    sub_indicators: true,
        //    drop_shadows: true,
        //    close_delay: 300
        //});
        $(".tabs-list li a").click(function () {
            $(".tabs-list li a").removeClass("active");
            $(".tabs-list li span").removeClass("tail-bottom");
            $(this).addClass("active");
            $(this).siblings().addClass("tail-bottom");
            $(".tabs-data").hide();
            $(".tabs-data").eq($(this).parent().index()).show();
        });
        $(".map-tabs li a").click(function () {
            $(".map-tabs li a").removeClass("active");
            $(".map-tabs li span").removeClass("tail-top");
            $(this).addClass("active");
            $(this).siblings().addClass("tail-top");
            $(".map-data").hide();
            $(".map-data").eq($(this).parent().index()).show();
        });
        $(".pics").colorbox({ rel: 'nofollow' });
        $(".videos").colorbox({
            iframe: true,
            innerWidth: 640,
            innerHeight: 390
        });
    });
</script>

<form runat="server">

        <div class="pthead">
        	<h1> <%= makeName %> Showcase at Auto Expo 2014</h1>
            <span class="sponsored hide">Sponsored</span>
            <div class="clear"></div>
        </div>
        <!-- Left grid code starts here-->
        <div class="left-grid">
        	<!-- welcome text box code starts here-->
            <h4 id="errorMsg" runat="server">Updates coming soon.  Please check back in a while.</h4>
            <div class="content-box" id="divArticle" runat="server">
            	<h3>Cover Story from Auto Expo - <%= makeName %></h3>
                <div class="data-box">
                	<ul>
                    	<li>
                            <div class="thumb-area">
                                <img src="http://<%= articleImg%>"" border="0" alt="<%= articleTitle %>" title="<%= articleTitle %>" />
                            </div>
                            <div class="intro-content">
                                <h5 class="sub-title"><%= articleTitle %></h5>
                                <p><%= TruncateDesc(articleDesc,"314") %></p>
                                <a href="<%=articleUrl %>">Read More &raquo;</a>
                            </div>
                            <div class="clear"></div>
                         </li>
                    </ul>   
                </div>
            </div>
            <!-- welcome text box code ends here-->
            <!-- Gallery code starts here-->
            <div class="gallery-bg" id="divGallery" runat="server">
            	<h2>Gallery</h2>
                <div class="big-img">
                    <ul>
                        <li>
                            <a rel="nofollow" class="pics" href="<%= imgArr[0] %>">
                                <img src="<%= imgArr[0] %>" border="0" alt="" />
                            </a>
                        </li>
                        <li id="video1" runat="server">
                            <a class='videos' href="<%= vidArr[0] %>">
                                <div class="ae-sprite play-icon"></div>
                                <img src="http://img.youtube.com/vi/<%= vidIdArr[0] %>/mqdefault.jpg" border="0" alt="" />
                            </a>
                        </li>
                        <li id="image1" runat="server">
                            <a rel="nofollow" class="pics" href="<%= imgArr[5] %>">
                                <img src="<%= imgArr[5] %>" border="0" alt="" />
                            </a>
                        </li>
                    </ul>
                </div>
                <div class="clear"></div>
                <div class="thumb-img">
                	<ul>
                        <li>
                            <a rel="nofollow" class="pics" href="<%= imgArr[1] %>">
                                <img src="<%= imgArr[1] %>" border="0" alt="" />
                            </a>
                        </li>
                        <li>
                            <a rel="nofollow" class="pics" href="<%= imgArr[2] %>">
                                <img src="<%= imgArr[2] %>" border="0" alt="" />
                            </a>
                        </li>
                        <li>
                            <a rel="nofollow" class="pics" href="<%= imgArr[3] %>">
                                <img src="<%= imgArr[3] %>" border="0" alt="" />
                            </a>
                        </li>
                        <li id="video2" runat="server">
                            <a rel="nofollow" class="videos" href="<%= vidArr[1] %>">
                                <div class="ae-sprite play-small-icon"></div>
                                <img src="http://img.youtube.com/vi/<%= vidIdArr[1] %>/1.jpg" border="0" alt="" />
                            </a>
                        </li>
                        <li id="image2" runat="server">
                            <a rel="nofollow" class="pics" href="<%= imgArr[6] %>">
                                <img src="<%= imgArr[6] %>" border="0" alt="" />
                            </a>
                        </li> 
                        <li>
                            <a rel="nofollow" class="pics" href="<%= imgArr[4] %>">
                                <img src="<%= imgArr[4] %>" border="0" alt="" />
                            </a>
                        </li>                        
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
            <!-- Gallery code ends here-->
            
            <!-- News code starts here-->
            <div class="content-box" id="divNews" runat="server">
            	<h3>News</h3>
                <div class="data-box">
                    <ul>
                        <asp:Repeater ID="rptNews" runat="server">
                            <itemtemplate>
                            <li>
                        	    <div class="thumb-area">
                                    <img src="http://<%# DataBinder.Eval(Container.DataItem,"HostUrl").ToString() %><%# DataBinder.Eval(Container.DataItem,"ImagePathLarge").ToString() %>" border="0" alt="<%# DataBinder.Eval(Container.DataItem,"Title").ToString() %>" title="<%# DataBinder.Eval(Container.DataItem,"Title").ToString() %>" />
                                </div>
                                <div class="intro-content">
                                    <h4><%# DataBinder.Eval(Container.DataItem,"Title").ToString() %></h4>
                                    <span class="font11"><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"DisplayDate")).ToString("dd MMM yyyy") %> by <%# DataBinder.Eval(Container.DataItem,"AuthorName").ToString() %></span>
                                    <p class="margin-top15">
                                        <%# TruncateDesc(DataBinder.Eval(Container.DataItem,"Description").ToString(),"145") %>
                                        <a  class="news-url" href="/autoexpo/2014/<%# DataBinder.Eval(Container.DataItem,"BasicId").ToString() %>-<%# DataBinder.Eval(Container.DataItem,"Url").ToString()%>.html">More &raquo;</a>
                                    </p>
                                </div>
                                <div class="clear"></div>
                            </li>
                            </itemtemplate>
                        </asp:Repeater>
                	</ul>
                </div>
            </div>
            <!-- News code ends here-->
            
        </div>
        <!-- Left grid code ends here-->
        <!-- Right grid code starts here-->
        <div class="right-grid">
            <div class="content-box">
            	<h3>About <%=makeName %></h3>
                <div class="data-box">
                	<p><%= TruncateDesc( makeDesc,"180") %></p>
                	<%--<a href="/<%= FormatSpecial(makeName) %>-bikes/#spnSmallDescSynopsis" class="margin-top10" target="_blank">Read More &raquo;</a>--%>
                    
                    <div class="sub-section">
                        <div id="divUpcoming" runat="server">
                            <h4>Upcoming <%=makeName %> Bikes</h4>
                            <div class="sub-section-thumbs">
                                <ul>
                                    <asp:Repeater ID="rptUpcoming" runat="server">
                                        <itemtemplate>
                                        <li><a href="/<%# DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString() %>-bikes/<%# DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() %>/" target="_blank" style="color:#666;">
                                            <div><img src="http://<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"ImgPath").ToString())? DataBinder.Eval(Container.DataItem,"ImgPath").ToString() : "img.carwale.com/bikewaleimg/common/nobike.jpg" %>" border="1" alt="<%# DataBinder.Eval(Container.DataItem,"MakeName").ToString() %>  <%# DataBinder.Eval(Container.DataItem,"ModelName").ToString() %>" title="<%# DataBinder.Eval(Container.DataItem,"MakeName").ToString() %>  <%# DataBinder.Eval(Container.DataItem,"ModelName").ToString() %>"/></div>
                                            <h5><%# DataBinder.Eval(Container.DataItem,"MakeName").ToString() %>  <%# DataBinder.Eval(Container.DataItem,"ModelName").ToString() %></h5>
                                        </a></li>
                                        </itemtemplate>
                                    </asp:Repeater>
                                </ul>
                                <div class="clear"></div>
                                <a href="/<%= makeMaskingName %>-bikes/upcoming/" target="_blank" class="margin-top15 margin-left5">All Upcoming Bikes &raquo;</a>
                            </div>
                        </div>
                        <div id="divModels" runat="server">
                            <h4>Explore <%=makeName %> Bikes</h4>
                            <div class="wheel-icon">
                                <ul>
                                    <asp:Repeater ID="rptModels" runat="server">
                                         <itemtemplate>
                                        <li><a href="/<%# DataBinder.Eval(Container.DataItem,"MakeMaskingName").ToString() %>-bikes/<%# DataBinder.Eval(Container.DataItem,"ModelMaskingName").ToString() %>/" target="_blank"><%# DataBinder.Eval(Container.DataItem,"BikeName").ToString() %></a></li>
                                        </itemtemplate>
                                    </asp:Repeater>
                                </ul>
                                <div class="clear"></div>                            
                            </div>
                        </div>
                    </div>
                </div>                
            </div>
            <div class="content-box">
            	<h3>More On <%=makeName %></h3>
	            <div class="data-box">
                    <div class="arrow-icon">
                        <ul>
                            <li><a href="/pricequote/" target="_blank">On Road Price</a></li>
                            <li><a href="/new/<%=MakeMappingName %>-dealers/" target="_blank">Locate a Dealer</a></li>
                        </ul>
                        <div class="clear"></div>                            
                    </div>
                </div>
            </div>
        </div>
        <!-- Right grid code ends here-->
        <div class="clear"></div>
    </form>
    <!-- #include file="/autoexpo/includes/footer.aspx" -->

