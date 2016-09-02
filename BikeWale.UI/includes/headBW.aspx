<%--<%@ Register Src="~/controls/LoginControlNew.ascx" TagPrefix="BW" TagName="Login" %>--%>
<%@ Register Src="~/controls/LoginStatusNew.ascx" TagPrefix="BW" TagName="LoginStatus" %>
<%@ Register Src="~/controls/PopupWidget.ascx" TagPrefix="BW" TagName="PopupWidget" %>

    <div id="header" class='<%= isHeaderFix ? "header-fixed": "header-not-fixed" %> <%=  isTransparentHeader?"header-landing":String.Empty   %>'> <!-- Fixed Header code starts here -->
        <div class="leftfloat">
            <span class="navbarBtn bwsprite nav-icon margin-right25"></span>
            <a href="/" id="bwheader-logo" class="bwsprite bw-logo" title="Bikewale" alt="Bikewale"></a>
           
        </div>
        <div class="rightfloat">
            <div class="global-search" style="display:none">
                <span class="bwsprite search-icon-grey" id="btnGlobalSearch" style="z-index:2"></span>
                <input type="text" name="globalSearch" placeholder="Search" id="globalSearch" class="blur ui-autocomplete-input" autocomplete="off">
                <span class="fa fa-spinner fa-spin position-abt  text-black" style="display:none;right:14px;top:7px;background:#fff"></span>
                <ul id="errGlobalSearch" style="width:420px;position:absolute;top:30px;left:3px" class="ui-autocomplete ui-front ui-menu hide">
                    <li class="ui-menu-item" tabindex="-1">
                       <span class="text-bold">Oops! No suggestions found</span><br /> <span class="text-light-grey font12">Search by bike name e.g: Honda Activa</span>
                    </li>
                </ul>
            </div>
            <div class="global-location">
                <div class="gl-default-stage">
                	<div id="globalCity-text">
                        <span class="cityName" id="cityName">Select City</span>
                        <span class="bwsprite global-map-marker margin-left10"></span>
                    </div>
                </div>            
            </div>

            <BW:LoginStatus ID="ctrlLoginStatus" runat="server" />
            <div class="clear"></div>
        </div>
        <div class="clear"></div>
    </div> <!-- ends here -->
    <div class="clear"></div>    
<% if(isAd970x90Shown){ %>
    <div class="bg-white ">
        <div class="container">
            <div class="grid-12">
                <div>
                    <!-- #include file="/ads/Ad970x90.aspx" -->
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </div>
<% } %>
