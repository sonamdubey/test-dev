<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.DealersInState" EnableViewState="false" %>
<!DOCTYPE html>
<html>
<head>
     <% 
        description = string.Format("{0} bike dealers/showrooms in {1}. Find dealer information for more than {2} dealers in {3} cities. Dealer information includes full address, phone numbers, email, pin code etc.", objMMV.MakeName, stateName, DealerCount, citiesCount);
        keywords = string.Format("{0} bike dealers, {0} bike showrooms, {0} dealers, {0} showrooms, {0} dealerships, dealerships, test drive, {0} dealer contact number", objMMV.MakeName);
        title = string.Format("{0} Bike Dealers in {1} | {0} Bike Showrooms in {1} - BikeWale", objMMV.MakeName, stateName);
        canonical = string.Format("http://www.bikewale.com/{0}-bikes/dealers-in-{1}-state/", objMMV.MaskingName, stateMaskingName);
        AdPath = "/1017752/Bikewale_Mobile_Model";
        AdId = "1444028976556";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;
    %>

    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";#no-result,.location-list-city{display:none}.padding-15-20{padding:15px 20px}.form-control-box .search-icon-grey{position:absolute;right:10px;top:10px;cursor:pointer;z-index:2;background-position:-34px -275px}.form-control-box .fa-spinner{display:none;right:14px;top:12px;z-index:3}#location-list .item-state{border-top:1px solid #f1f1f1}#location-list .item-state:first-child{border-top:0}#location-list a{color:#4d5057;font-size:14px;display:block;padding-top:13px;padding-bottom:13px}#location-list .type-state,#no-result{font-size:16px}#location-list li .type-state:hover{color:#2a2a2a;text-decoration:none}#location-list .type-state.active{padding-bottom:8px}#location-list .location-list-city a{color:#82888b;padding-top:9px;padding-bottom:9px}#location-list .location-list-city a:hover{color:#4d5057;text-decoration:none}#no-result{padding:13px 0;color:#82888b}.text-truncate{width:100%;text-align:left;text-overflow:ellipsis;white-space:nowrap;overflow:hidden}
    </style>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">                  
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
         <% if(Ad_320x50){ %>
            <section>            
                <div>
                    <!-- #include file="/ads/Ad320x50_mobile.aspx" -->
                </div>
            </section>
        <% } %>

        <section>
            <div class="container margin-bottom10">
                <div class="bg-white">
                    <h1 class="box-shadow padding-15-20">Bajaj dealer showrooms in India</h1>
                    <div class="box-shadow font14 text-light-grey padding-15-20">
                        Honda sells bikes through a vast network of dealer showrooms. The network consists of 102 authorized Honda showrooms spread across 11 cities in India. The Honda dealer showroom locator will help you find the nearest authorized dealer in your city. In case, there are no Honda showrooms in your city, you can get in touch with an authorized dealer in your nearby city.
                    </div>
                </div>
            </div>
        </section>

        <section>
            <div class="container bg-white margin-bottom10 box-shadow">
                <h2 class="padding-15-20 border-solid-bottom">577 Bajaj dealer showrooms in 401 cities</h2>
                <div class="content-inner-block-20">
                    <div class="form-control-box">
                        <span class="bwmsprite search-icon-grey"></span>
                        <input type="text" class="form-control padding-right40" placeholder="Type to select city" id="getCityInput" />
                        <span class="fa fa-spinner fa-spin position-abt text-black"></span>
                    </div>
                    <ul id="location-list">
                        <li class="item-state">
                            <a href="" class="type-state" rel="nofollow">Andhra Pradesh</a>
                            <ul class="location-list-city">
                                <li>
                                    <a href="">Amalapuram (1)</a>
                                </li>
                                <li>
                                    <a href="">Anantapur (1)</a>
                                </li>
                                <li>
                                    <a href="">Bhimavaram (1)</a>
                                </li>
                                <li>
                                    <a href="">Chirala (1)</a>
                                </li>
                                <li>
                                    <a href="">Chittoor (1)</a>
                                </li>
                            </ul>
                        </li>
                        <li class="item-state">
                            <a href="" class="type-state" rel="nofollow">Assam</a>
                            <ul class="location-list-city">
                                <li>
                                    <a href="">Barpeta (1)</a>
                                </li>
                                <li>
                                    <a href="">Bongaigaon (1)</a>
                                </li>
                                <li>
                                    <a href="">Dibrugarh (1)</a>
                                </li>
                            </ul>
                        </li>
                        <li class="item-state">
                            <a href="" class="type-state" rel="nofollow">Delhi</a>
                            <ul class="location-list-city">
                                <li>
                                    <a href="">New Delhi (11)</a>
                                </li>
                            </ul>
                        </li>
                        <li class="item-state">
                            <a href="" class="type-state" rel="nofollow">Goa</a>
                            <ul class="location-list-city">
                                <li>
                                    <a href="">Margao (1)</a>
                                </li>
                                <li>
                                    <a href="">Panjim (1)</a>
                                </li>
                            </ul>
                        </li>
                        <li class="item-state">
                            <a href="" class="type-state" rel="nofollow">Gujarat</a>
                            <ul class="location-list-city">
                                <li>
                                    <a href="">Amalapuram (1)</a>
                                </li>
                                <li>
                                    <a href="">Anantapur (1)</a>
                                </li>
                                <li>
                                    <a href="">Bhimavaram (1)</a>
                                </li>
                                <li>
                                    <a href="">Chirala (1)</a>
                                </li>
                                <li>
                                    <a href="">Chittoor (1)</a>
                                </li>
                            </ul>
                        </li>
                        <li class="item-state">
                            <a href="" class="type-state" rel="nofollow">Haryana</a>
                            <ul class="location-list-city">
                                <li>
                                    <a href="">Margao (1)</a>
                                </li>
                                <li>
                                    <a href="">Panjim (1)</a>
                                </li>
                            </ul>
                        </li>
                        <li class="item-state">
                            <a href="" class="type-state" rel="nofollow">Himachal Pradesh</a>
                            <ul class="location-list-city">
                                <li>
                                    <a href="">Amalapuram (1)</a>
                                </li>
                                <li>
                                    <a href="">Chirala (1)</a>
                                </li>
                                <li>
                                    <a href="">Chittoor (1)</a>
                                </li>
                            </ul>
                        </li>
                        <li class="item-state">
                            <a href="" class="type-state" rel="nofollow">Maharashtra</a>
                            <ul class="location-list-city">
                                <li>
                                    <a href="">Amalapuram (1)</a>
                                </li>
                                <li>
                                    <a href="">Anantapur (1)</a>
                                </li>
                                <li>
                                    <a href="">Chittoor (1)</a>
                                </li>
                            </ul>
                        </li>
                    </ul>
                    <div id="no-result"></div>
                </div>
            </div>
        </section>

        <section>
            <div class="container bg-white margin-bottom10 box-shadow">
                <h2 class="padding-15-20">Newly launched Bajaj bikes</h2>

                <div class="margin-right20 margin-left20 border-solid-bottom"></div>
                <h2 class="padding-15-20">Upcoming Bajaj bikes</h2>
            </div>
        </section>
        
        <!--
        <ul id="listingUL" class="city-listing">
            <asp:Repeater ID="rptCity" runat="server">
                <ItemTemplate>
                    <li>
                        <a href="/m/<%=objMMV.MaskingName %>-bikes/dealers-in-<%#DataBinder.Eval(Container.DataItem,"cityMaskingName") %>/" data-item-id="<%#DataBinder.Eval(Container.DataItem,"cityId") %>"><%#DataBinder.Eval(Container.DataItem,"CityName") %> (<%#DataBinder.Eval(Container.DataItem,"DealersCount") %>)</a>
                    </li>

                </ItemTemplate>
            </asp:Repeater>
        </ul>
        -->
    
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/dealer/location.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
    </form>
</body>
</html>
