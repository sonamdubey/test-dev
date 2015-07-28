<%@ Page trace="false" debug="false" Language="C#" AutoEventWireUp="false" Inherits="AutoExpo.DefaultClass" enableEventValidation="false" EnableViewState="true" %>
<%@ Import NameSpace="Bikewale.Common" %>
<%@ Register TagPrefix="AutoExpo" TagName="RepeaterPager" src="/autoexpo/controls/RepeaterPagerNews.ascx" %>
<%--<%@ Register TagPrefix="AutoExpo" TagName="spContent" src="/autoexpo/controls/sponsoredContent.ascx" %>
<%@ Register TagPrefix="AutoExpo" TagName="ModelThumbnail" src="/autoexpo/controls/modelThumbnail.ascx" %>--%>

<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId          = 1;
	Title 			= "Auto Expo 2014 - BikeWale";
	Description 	= "BikeWale's coverage on Auto Expo 2014, the largest auto show in India.";
	Keywords		= "auto expo, auto expo 2014, auto show india, auto expo delhi";
	Revisit 		= "5";
	DocumentState 	= "Static";
    canonical       = "http://bikewale.com/autoexpo/2014/";
%>
<!-- #include file="/autoexpo/includes/headNews.aspx" -->
<% if ( Request.QueryString["pn"] != null && Request.QueryString["pn"].Length > 0 ) { %>
<META NAME="ROBOTS" CONTENT="NOINDEX, FOLLOW">
<% } %>
<script type="text/javascript">!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = "https://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } }(document, "script", "twitter-wjs");</script>

<form id="form1" runat="server">
        <!-- Left grid code starts here-->
        <div class="left-grid">
        	<!-- Gallery code starts here-->
            <div class="gallery-bg" id="divGallery" runat="server">
            	<h2>Latest Visuals from the Expo</h2>
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
            <div class="content-box">
            	<div class="tabs-list">
                	<ul>
                    	<li>
                        	<a id="LatestNews" class="active selection" runat="server" tabid="1">Latest News</a>

                            <span  id="LatestNewsSpan" class="ae-sprite tail-bottom" runat="server"></span>
                        </li>
                        <li>
                        	<a id="PopularNews" class="selection" runat="server" tabid="2">Most Popular News</a>
                            <span id="PopularNewsSpan" class="ae-sprite" runat="server"></span>
                        </li>
                        <li>
                        	<a id="Highlights" class="last selection" runat="server" tabid="3">Auto Expo Highlights</a>
                            <span class="ae-sprite"></span>
                        </li>

                    </ul>
                    <div class="clear"></div>                    
                </div>
                <div  id="filters-data" class="margin-left10 margin-top20">
                    <ul>
                        <li>
                            <label>Read News On:</label>
                            
                                <asp:dropdownlist id="ddlMake" class="width120 margin-left15" runat="server" label="Make"  >  </asp:dropdownlist>
                                     <input type="hidden" id="hdn_ddlMake" runat="server" /><input type="hidden" id="hdn_ddlMakeName" runat="server" />
                              
                            
                                <asp:dropdownlist id="ddlModel" class="width120 margin-left15" runat="server" label="Model">
                                    <asp:ListItem Selected="true" Text="Models" Value="-1"></asp:ListItem>
                                    
                                </asp:dropdownlist>
                            <input type="hidden" id="hdn_ddlModel" runat="server" /><input type="hidden" id="hdn_ddlModelName" runat="server" />
                               <asp:dropdownlist id="ddlDays" class="width120 margin-left15" runat="server" label="Date">
                                    <asp:ListItem Value="-1" Text="Date"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="7-Feb-2013"></asp:ListItem>
			                        <asp:ListItem Value="2" Text="8-Feb-2013"></asp:ListItem>
			                        <asp:ListItem Value="3" Text="9-Feb-2013"></asp:ListItem>
			                        <asp:ListItem Value="4" Text="10-Feb-2013"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="11-Feb-2013"></asp:ListItem>
                                </asp:dropdownlist>
                             <input type="hidden" id="hdn_ddlDays" runat="server" /><input type="hidden" id="hdn_ddlDate" runat="server" />
                            <input type="submit" id="FilterData" name="go" value="Go" class="margin-left15" onclick="return Validate();" runat="server" />
                            &nbsp;&nbsp;&nbsp;<a id="viewAll" runat="server" >View All</a>
                        </li>
                    </ul>
                    <div class="clear"></div>
                </div>
                <!-- Latest News data starts here -->
                <div class="tabs-data">
                   <ul>
                      <AutoExpo:RepeaterPager id="rpgNews" PageSize="10" PagerPageSize="10" runat="server"> 
                    <asp:Repeater ID="rptNews" runat="server" EnableViewState="false">
                        <itemtemplate>
                            <li>
                         <div id='post-<%# DataBinder.Eval(Container.DataItem,"ContentId") %>'>	
                            <a href='<%# DataBinder.Eval(Container.DataItem,"ContentId") + "-" + DataBinder.Eval(Container.DataItem,"Url") %>.html'><h4><strong><%# DataBinder.Eval(Container.DataItem, "Title") %></strong></h4> </a>                 
                            <span>
                                <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "DisplayDate")) %> &nbsp; <%# DataBinder.Eval(Container.DataItem,"AuthorName") %>
                            </span>
                            <p>
                                <%#(DataBinder.Eval(Container.DataItem,"Description")) %>
                                <a href='<%# DataBinder.Eval(Container.DataItem,"ContentId") + "-" + DataBinder.Eval(Container.DataItem,"Url") %>.html' target="_blank">More &raquo</a>
                            </p>
                           <%-- <div class ="margin-top10">
                                <%# DataBinder.Eval(Container.DataItem, "IsMainImage").ToString() == "True" ? "<a rel='nofollow' class='pics' href= 'http://"+Eval("HostURL").ToString() + DataBinder.Eval(Container.DataItem, "ImagePathLarge").ToString()+"'><img class='alignleft size-thumbnail img-border-news' src= 'http://"+Eval("HostURL").ToString() + DataBinder.Eval(Container.DataItem, "ImagePathThumbnail").ToString()+"' border='0' /></a>": ""%>
                            </div>--%>
                             <div class="social">
                                <div class="g-plus-btn">
                                    <!-- Place this tag where you want the +1 button to render -->
                                    <g:plusone size="medium" href="http://bikewale.com/news/<%# DataBinder.Eval(Container.DataItem,"ContentId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html" count="true"></g:plusone>
                                    <!-- Place this tag after the last plusone tag -->
                                    <script type="text/javascript">
                                        (function () {
                                            var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
                                            po.src = 'https://apis.google.com/js/plusone.js';
                                            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
                                        })();
                                    </script>
                                </div>
                                <div class="tw-btn">
                                    <a href="https://twitter.com/share?text=<%# DataBinder.Eval(Container.DataItem,"Title")%>&via=BikeWale&url=http://bikewale.com/news/<%# DataBinder.Eval(Container.DataItem,"ContentId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html&counturl=http://bikewale.com/news/<%# DataBinder.Eval(Container.DataItem,"ContentId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html" class="twitter-share-button" data-lang="en">Tweet</a>
                                    <%--<iframe src="http://platform.twitter.com/widgets/tweet_button.html?text=<%# DataBinder.Eval(Container.DataItem,"Title")  %>&via=BikeWale&url=http://bikewale.com/news/<%# DataBinder.Eval(Container.DataItem,"ContentId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html&counturl=http://bikewale.com/news/<%# DataBinder.Eval(Container.DataItem,"ContentId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html" style="width:110px; height:20px;" allowtransparency="true" frameborder="0" scrolling="no"></iframe>--%>
                                </div>
                                <div class="fb-btn">
                                    <iframe src="//www.facebook.com/plugins/like.php?href=http://bikewale.com/news/<%# DataBinder.Eval(Container.DataItem,"ContentId") %>-<%# DataBinder.Eval(Container.DataItem,"Url") %>.html&amp;width=10&amp;layout=button_count&amp;action=like&amp;show_faces=false&amp;height=80" scrolling="no" frameborder="0" style="border:none; overflow:hidden; height:20px;" allowTransparency="true"></iframe>
		                        </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                                </li>
                        </itemtemplate>
                    </asp:Repeater>
                          </AutoExpo:RepeaterPager>
                        </ul>
                    <div class ="clear:both;"></div>
                </div>
                <!-- Latest News data ends here -->
            </div>
        </div>
        <!-- Left grid code ends here-->
        <!-- Right grid code starts here-->
        <div class="right-grid">
        	<%--<div class="content-box">
            	<h3>AMG Engineering</h3>
                <div class="align-center">
                	<img src="/autoexpo/images/engine-pic.jpg" border="0" />
                </div>
                <div class="margin-left10 margin-top10">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td><a href="#">Performance Design</a></td>
                            <td><a href="#">Power Delivery</a></td>
                        </tr>
                        <tr>
                        	<td colspan="2" height="10"></td>
                        </tr>
                        <tr>
                            <td><a href="#">Power Creation</a></td>
                            <td><a href="#">Dynamic Control</a></td>
                        </tr>
                        <tr>
                        	<td colspan="2" height="10"></td>
                        </tr>
                    </table>
            	</div>
            </div>--%>
            <div class="content-box">
            	<h3>Auto Expo 2014 Venue Map</h3>
                <div class="map-view">
                	<div class="map-data">                    	
                        <%--<iframe width="288" height="180" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src="https://maps.google.com/maps?f=q&amp;source=s_q&amp;hl=en&amp;geocode=&amp;q=Noida+New+Delhi&amp;aq=&amp;sll=28.564321,77.316628&amp;sspn=0.164637,0.338173&amp;ie=UTF8&amp;hq=&amp;hnear=Noida,+Gautam+Buddh+Nagar,+Uttar+Pradesh,+India&amp;t=m&amp;z=11&amp;ll=28.535516,77.391026&amp;output=embed"></iframe> --%>    
                        <iframe width="288" height="180" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src="https://maps.google.com/maps?f=q&amp;source=s_q&amp;hl=en&amp;geocode=&amp;q=Greater+Noida,+Uttar+Pradesh,+India&amp;aq=0&amp;oq=greater+noida&amp;sll=28.474388,77.50399&amp;sspn=0.36396,0.676346&amp;ie=UTF8&amp;hq=&amp;hnear=Greater+Noida,+Gautam+Buddh+Nagar,+Uttar+Pradesh,+India&amp;t=m&amp;ll=28.474124,77.503738&amp;spn=0.108645,0.197067&amp;z=11&amp;iwloc=A&amp;output=embed"></iframe>
                    </div>
                    <div class="map-data hide">                    	
                        <iframe width="288" height="180" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src=" https://maps.google.com/maps?source=s_q&amp;f=q&amp;hl=en&amp;geocode=&amp;q=New+Delhi&amp;aq=&amp;sll=28.535516,77.391026&amp;sspn=0.329364,0.676346&amp;ie=UTF8&amp;hq=&amp;hnear=West+Delhi,+Delhi,+India&amp;t=m&amp;ll=28.666491,77.067719&amp;spn=0.15062,0.246506&amp;z=10&amp;iwloc=A&amp;output=embed"></iframe>
                    </div>
                </div>
                <div class="map-tabs">
                    <ul>
                    	<li>
                        	<a class="active">Auto Expo Greater Noida</a>
                            <span class="ae-sprite tail-top"></span>
                        </li>
                        <li>
                        	<a class="last">Auto Expo New Delhi</a>
                            <span class="ae-sprite"></span>
                        </li>
                    </ul>
                    <div class="clear"></div>
            	</div>
            </div>
            <%--<div class="brands-quicker">
                <div class="logo-placer">
                    <div class="margin-bottom10"><img src="/autoexpo/images/renault-logo.jpg" border="0" alt="Renault" title="Renault" /></div>
                    <span class="ae-sprite location-pointer margin-right5"></span><a rel="nofollow" class="pics" href="/autoexpo/images/Map-Renault.jpg">View Map</a>
                </div>
                <div class="add-placer">
                	<strong>Visit Renault</strong> <br />
                    at Auto Expo 2014<br />
                    Hall 5<br />
                    5th - 11th Feb 2014<br />
                </div>
                <div class="clear"></div>
            </div>
             <div class="brands-quicker">
                <div class="logo-placer">
                    <div class="margin-bottom10"><img src="/autoexpo/images/wv-logo.jpg" border="0" alt="Volkswagen" title="Volkswagen" /></div>
                    <span class="ae-sprite location-pointer margin-right5"></span><a rel="nofollow" class="pics" href="/autoexpo/images/Map-Volkswagen.jpg">View Map</a>
                </div>
                <div class="add-placer">
                	<strong>Visit Volkswagen</strong> <br />
                    at Auto Expo 2014<br />
                    Hall 12<br />
                    5th - 11th Feb 2014<br />
                </div>
                <div class="clear"></div>
            </div>--%>
            <div class="content-box">
            	<h3>Know More About Brands</h3>
                <div class="brands-list">                	
                	<ul>
                    	<li><a href="/autoexpo/2014/brand.aspx?mid=1">Bajaj</a></li>
                        <li><a href="/autoexpo/2014/brand.aspx?mid=8">Hyosung</a></li>
                        <li><a href="/autoexpo/2014/brand.aspx?mid=5">Harley Davidson</a></li>
                        <li><a href="/autoexpo/2014/brand.aspx?mid=6">Hero</a></li>
                        <li><a href="/autoexpo/2014/brand.aspx?mid=7">Honda</a></li>
                        <li><a href="/autoexpo/2014/brand.aspx?mid=13">Yamaha</a></li>
                        <li><a href="/autoexpo/2014/brand.aspx?mid=10">Mahindra</a></li>
                        <li><a href="/autoexpo/2014/brand.aspx?mid=12">Suzuki</a></li>
                        <li><a href="/autoexpo/2014/brand.aspx?mid=22">Triumph</a></li>
                        <li><a href="/autoexpo/2014/brand.aspx?mid=15">TVS</a></li>
                    </ul>
                    <div class="clear"></div>
                    
            	</div>
            </div>
        </div>
        <!-- Right grid code ends here-->
        <div class="clear"></div>
    <input type="hidden" id="hdn_panelNumber" runat="server" />
</form>

<script type="text/javascript">
    $("#ddlMake").change(function () {

        $("#ddlModel").val("-1");
        $("#ddlModel").empty();
        $("#ddlModel").append("<option value=-1 >Models</option>");
            var makeId = $(this).val();
            $("#hdn_ddlMake").val(makeId);
            
            if (makeId > 0) {
                $.ajax({
                    type: "POST", url: "/ajaxpro/AutoExpo.AjaxAutoExpoFilter,bikewale.ashx",
                    data: '{"makeId":"' + makeId + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetFilterData"); },
                    success: function (response) {
                        var jsonString = eval('(' + response + ')');
                        var objData = eval(jsonString.value);
                        console.log(objData);
                        for (var i = 0 ; i < objData.length ; i++) {
                            $("#ddlModel").append("<option value=" + objData[i].ID + ">" + objData[i].Name + "</option>");
                        }
                    }
                });
            }
        });


        if ($("#hdn_ddlMake").val() != "-1")
        {
            var makeId = $("#hdn_ddlMake").val();
            var modelId = $("#hdn_ddlModel").val();
            if (makeId > 0) {
                $.ajax({
                    type: "POST", url: "/ajaxpro/AutoExpo.AjaxAutoExpoFilter,bikewale.ashx",
                    data: '{"makeId":"' + makeId + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetFilterData"); },
                    success: function (response) {
                        var jsonString = eval('(' + response + ')');
                        var objData = eval(jsonString.value);
                        console.log(objData);
                        for (var i = 0 ; i < objData.length ; i++) {
                            $("#ddlModel").append("<option value=" + objData[i].ID + ">" + objData[i].Name + "</option>");
                            if (modelId > 0) {
                                $("#ddlModel").val(modelId);
                            }
                        }
                    }
                });
            }
        }
        $(".selection").click(function () {
            $("#hdn_panelNumber").val($(this).attr('tabid'));
        });

        $("#ddlModel").change(function () {
            $("#hdn_ddlModel").val($(this).val());
        });

        //$("#ddlModel").change(function () {
        //    var modelName = document.getElementById("ddlModel").options[document.getElementById("ddlModel").selectedIndex].text;
        //    document.getElementById("hdn_ddlModelName").value = modelName;
        //});

        //$("#ddlMake").change(function () {
        //    $("#hdn_ddlMake").val($(this).val());
        //    //drpModel_Change();
        //});

        
        //var abcd = $("#hdn_ddlModel").val();
        //alert(abcd);
        //$("#ddlModel").val(abcd);
        $("#ddlDays").change(function () {
            $("#hdn_ddlDays").val($(this).val());
            //drpDays_Change();
        });

        //$("#ddlDays").change(function () {
        //    var eventDate = document.getElementById("ddlDays").options[document.getElementById("ddlDays").selectedIndex].text;
        //    document.getElementById("hdn_ddlDate").value = eventDate;
        //    //alert(eventDate);
        //});

        //$("#ddlMake").change(function () {
        //    var makeName = document.getElementById("ddlMake").options[document.getElementById("ddlMake").selectedIndex].text;
        //    document.getElementById("hdn_ddlMakeName").value = makeName;
        //});

        $("#Highlights").click(function () {
            $("#filters-data").css('visibility','hidden');
        });

        function Validate()
        {
            IsError = false;
            if ($("#ddlMake").val() <= 0 && $("#ddlDays").val() <= 0)
            {
                alert("Please Select Field.");
                IsError = true;
            }
            return !IsError;
        }    
    </script>
<!-- #include file="/autoexpo/includes/footer.aspx" -->
