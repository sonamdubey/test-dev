<%@ Page Language="C#" Inherits="Bikewale.Used.Search" AutoEventWireup="false" Trace="false" Debug="false"%>
<%@ Import Namespace="Bikewale.Common" %>
<%
    title = pageTitle;
    description = pageDescription;
    keywords = pageKeywords;
    canonical = pageCanonical;
    prevPageUrl = prevUrl;
    nextPageUrl = nextUrl;
    AdId = "1395992162974";
    AdPath = "/1017752/BikeWale_UsedBikes_Search_Results_";
%>
<!-- #include file="/includes/headUsed.aspx" -->
<script type="text/javascript">
    var cityId = '<%= cityId %>';
    var isFirstLoad = true;
    var queryString = '<%= queryString%>';
</script>
<script type="text/javascript" src="<%= staticUrlPath != "" ? "http://st1.aeplcdn.com" + staticUrlPath : "" %>/src/used/search.js?<%= staticFileVersion %>"></script>

<style type="text/css">
    .sel_parama{border:1px solid #DFDFDF; color:#445566!important; padding:1px 1px 1px 5px; margin:3px; display:inline-block; text-decoration:none!important; border-radius:3px; cursor:pointer;}
    .sel_parama span{background-color:#DFDFDF; color:#445566; padding:0 3px; margin-left:5px; cursor:pointer;}
    .sel_parama_hover{border:1px solid #cc0000; color:#445566!important; padding:1px 1px 1px 5px; margin:3px; display:inline-block; text-decoration:none!important; border-radius:3px; cursor:pointer;}
    .sel_parama_hover span{background-color:#cc0000; color:#fff; padding:0 3px; margin-left:5px; cursor:pointer;}
    #app_filters li {display:block;} 
    #btnShowinterst { color:#fff; padding:8px;}
    #buyer_form input, #verifiy_mobile input { border:1px solid #ccc; padding:5px; }
</style>
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/" itemprop="url">
                    <span itemprop="title">Home</span>
                </a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/used/" itemprop="url">
                    <span itemprop="title">Used Bikes</span>
                </a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>Search Used Bikes</strong></li>
        </ul><div class="clear"></div>
    </div>
    <div class="grid_12 margin-top10"><h1>Used Bike Search</h1></div>
    <div class="grid_4 margin-top10">
        <div id="parms" class="grey-bg">
            <div class="content-block">
                <h3>Your City <font color="red">*</font></h3>
		            <ul class="ul-params">
			            <li style="width:100%;">							            
					        <asp:DropDownList ID="drpCity" class="{param:'city'}" runat="server" Width="110px"></asp:DropDownList>&nbsp;within&nbsp;
                            <select name="drpCityDist" id="drpCityDist" class="{param:'dist'}" style="width:110px;">
                                <option value="-1" disabled="disabled" >--Select--</option>
						        <option value="0">this city</option>
						        <option value="25">25 km</option>
						        <option value="50" selected="selected">50 km</option>
						        <option value="100">100 km</option>
						        <option value="200">200 km</option>
						        <option value="500">500 km</option>
						        <option value="1000">1000 km</option>
						        <option value="1">entire state</option>
					        </select>
			            </li>                       
		            </ul>
                <div class="hr-sep"></div>
	            <h3>What Budget?</h3>
	            <ul id="budget" class="ul-params">
		            <li><a name="0" class="filter unchecked" href="#budget=0" rel="nofollow">Up to 10,000</a></li>
		            <li><a name="1" class="filter unchecked" href="#budget=1" rel="nofollow">10,000-20,000</a></li>
		            <li><a name="2" class="filter unchecked" href="#budget=2" rel="nofollow">20,000-35,000</a></li>
		            <li><a name="3" class="filter unchecked" href="#budget=3" rel="nofollow">35,000-50,000</a></li>
		            <li><a name="4" class="filter unchecked" href="#budget=4" rel="nofollow">50,000-80,000</a></li>
		            <li><a name="5" class="filter unchecked" href="#budget=5" rel="nofollow">80,000-1,50,000</a></li>
		            <li><a name="6" class="filter unchecked" href="#budget=6" rel="nofollow">1,50,000 or above</a></li>
	            </ul>
	            <div class="hr-sep"></div>
	            <h3>How Old?</h3>
	            <ul id="year" class="ul-params">
		            <li><a name="0" class="filter unchecked" href="#year=0" rel="nofollow">Up to 1 year old</a></li>
		            <li><a name="1" class="filter unchecked" href="#year=1" rel="nofollow">1-3 year old</a></li>
		            <li><a name="2" class="filter unchecked" href="#year=2" rel="nofollow">3-5 year old</a></li>
		            <li><a name="3" class="filter unchecked" href="#year=3" rel="nofollow">5-8 year old</a></li>
		            <li><a name="4" class="filter unchecked" href="#year=4" rel="nofollow">8 years or older</a></li>
	            </ul>
	            <div class="hr-sep"></div>
	            <h3>How Much Ridden?</h3>
	            <ul id="kms" class="ul-params">
		            <li><a name="0" class="filter unchecked" href="#kms=0" rel="nofollow">Up to 5000 km</a></li>
		            <li><a name="1" class="filter unchecked" href="#kms=1" rel="nofollow">5000-15000 km</a></li>
		            <li><a name="2" class="filter unchecked" href="#kms=2">15000-30000 km</a></li>
		            <li><a name="3" class="filter unchecked" href="#kms=3" rel="nofollow">30000-50000 km</a></li>
		            <li><a name="4" class="filter unchecked" href="#kms=4" rel="nofollow">50000-80000 km</a></li>
		            <li><a name="5" class="filter unchecked" href="#kms=5" rel="nofollow">80000 km or above</a></li>
	            </ul>
	            <div class="hr-sep"></div>		
	            <h3>Preferred Manufacturer?</h3>                  
                <ul id="make" class="ul-params makeList">
                <asp:Repeater ID="rptMakes" runat="server">                
                    <ItemTemplate>
                        <li><a name="<%# DataBinder.Eval(Container.DataItem, "ID")%>" class="filter unchecked" rel="nofollow" href='#make=<%# DataBinder.Eval(Container.DataItem, "ID")%>'><%# DataBinder.Eval(Container.DataItem, "Text")%></a></li>
                    </ItemTemplate>
                </asp:Repeater>
                </ul>
		        <div class="clear"></div>               
            </div>
        </div>
    </div>
    <div class="grid_8 margin-top15">
        <div id="selectCity" class="grey-bg content-block hide">Please get started by selecting <b>your city</b></div>
        <div id="app_filt" class="grey-bg content-block">
            <div class="margin-bottom5">You have selected&nbsp;&nbsp;<a onclick="javascript:removeSelection();" class="text-highlight pointer">Remove all selections</a></div>
		    <ul id="app_filters">
			    <li id="_city"><span class="f-small-b">City: </span></li>
			    <li id="_budget" class="hide"><span class="f-small-b">Budget: </span></li>
			    <li id="_year" class="hide"><span class="f-small-b">Year: </span></li>
			    <li id="_kms" class="hide"><span class="f-small-b">Km: </span></li>
			    <li id="_bs" class="hide"><span class="f-small-b">Body: </span></li>
			    <li id="_make" class="hide"><span class="f-small-b">Make: </span></li>
			    <li id="_model" class="hide"><span class="f-small-b">Model: </span></li>
			    <li id="_fuel" class="hide"><span class="f-small-b">Fuel: </span></li>
			    <li id="_engine" class="hide"><span class="f-small-b">Engine: </span></li>
			    <li id="_tm" class="hide"><span class="f-small-b">Transmission: </span></li>
			    <li id="_seller" class="hide"><span class="f-small-b">Seller Type: </span></li>
		    </ul>
		    <div class="clear"></div>
	    </div>
        <div id="searchRes" runat="server">
            <img src='http://img.aeplcdn.com/loader.gif' border='0'alt="Loading.." />            
        </div>
    </div>
</div>
<div id="expended_row" class="hide">
	<div><span id="last_update_row" class="right-float"></span><span id="close_row"><img alt="collapse" src="http://img.aeplcdn.com/used/collapsible.png" title="collapse" border="0" align="left"/></span>&nbsp;<span id="bike_row" class="price2" style="zoom:1;"></span>&nbsp;&nbsp;<span id="profileId_row"></span></div>    
	<div style="width:10%;" class="thumb_img left-float margin-top10" align="center"><img id="front_image" alt="Loading..." src="http://img.aeplcdn.com/bikewaleimg/common/nobike.jpg" border="0" /><br /><a id="photo_count"></a><div id="certified_cont"></div></div>
	<div style="width:80%; display:block;" class="right-float">
		<table class="tbl_row" width="100%" cellspacing="0" border="0" cellpadding="0">
		  <tr>
			<th width="100px">Price</th>
			<td width="100px" class="price2" id="price_row"></td>			
			<th width="100px">City</th>
			<td id="city_row"></td>
		  </tr>
		  <tr>
			<th>km</th>
			<td id="kms_row"></td>
			<th>Transmission</th>
			<td id="_trans"></td>
		  </tr>
		  <tr>
			<th>Model Year </th>
			<td id="model_year_row"></td>
			<th>Color</th>
			<td id="color_row"></td>
		  </tr>
          <tr>
			<td><input id="btnShowinterst" value="Show Interest" type="button" class="buttons" /></td>
			<td>&nbsp;</td>
			<td>&nbsp;</td>
			<td><a id="go_profile" target="_blank">&raquo; View complete details</a></td>
		  </tr>	          
		</table>		
	</div><div class="clear"></div>
</div>
<div id="contact" class="hide">
    <a id="closeBox" class="gb-close right-float" title="Close"></a>
    <a id="backToVerification" class="gb-back hide" title="Back"></a>
    <div id="buyer_form" style="height:260px;">
		<h2 id="form_title" class="hd2" style="margin-top:5px;">Interested? Get Seller Details</h2>
		<p id="byline_text" class="text-grey" style="margin-bottom:5px;">For privacy concerns, We hide owner details. Please fill this form to get owner's details.</p>
		<table class="tbl-default padding-bottom20" width="100%" border="0" cellspacing="0" cellpadding="0">
		<tr>
		  <td>Your Name</td>
		  <td><input type="text" id="txtName" size="27" class="text" /></td>
		</tr>
		<tr>
		  <td>Email</td>
		  <td><input type="text" id="txtEmail" size="27" class="text" /></td>
		</tr>
		<tr>
		  <td>Mobile Number</td>
		  <td><input type="text" id="txtMobile" maxlength="10" class="text" /></td>
		</tr>
		<tr>
		  <td>&nbsp;</td>
		  <td><a id="get_details" style="float:left;" class="buttons">Get Details</a><div id="process_img" class="process-inline"></div></td>
		</tr>
		</table>
	</div>
	<div id="verifiy_mobile" class="hide">
		<h2 class="hd2" style="margin-top:5px;">One-time Mobile Verification</h2>
		<div class="margin-top10">We have just sent you an SMS with a 5-digit verification code on your mobile number. Please enter the verification code below to proceed.</div>				
		<div class="margin-top10">
			<img align="absmiddle" alt="mobile verification" class="redirect-lt" src="http://img.aeplcdn.com/sell/mobi-verif.gif" border="0" /> <input type="text" id="txtCwiCode" maxlength="5" style="margin-left:10px;" class="text redirect-lt" value="Enter your code here" onFocus="javascript:if(this.value == 'Enter your code here') { this.value=''; }" onBlur="javascript:if(this.value == '') { this.value='Enter your code here'; }"  /> 
			<a id="btnVerifyCode" style="margin-left:5px;" class="buttons">Verify</a><div id="processCode" class="process-inline"></div>
		</div><div class="clear"></div>        
	</div>   
	<div id="seller_details" class="hide">
		<h2 class="hd2" style="margin-top:5px;">Seller Details</h2>
		<p class="text-grey">We have sent these details through SMS</p>		
		<table class="tbl-default" width="100%" border="0" cellspacing="0" cellpadding="0">
			<tr>
				<th width="120">Seller Name</th>
				<td><span id="seller_name"></span></td>
			</tr>
			<tr>
				<th>Contact Person</th>
				<td><span id="contact_person"></span></td>
			</tr>
			<tr>
				<th>Email</th>
				<td><span id="seller_email"></span></td>
			</tr>
			<tr>
				<th class="text-grey2">Contact(s)</th>
				<td><span id="seller_mobile"></span></td>
			</tr>
			<tr>
				<th class="text-grey2" valign="top">Address</th>
				<td><span id="seller_address"></span></td>
			</tr>
		</table>
	</div>
	<div id="not_auth" class="alert hide" style="margin-top:25px;"></div>
	<div id="initWait" class="process-inline" style="padding-left:20px;">Loading...</div>
</div>
<div class="hide" id="blackOut-window"></div>
<div class="hide" id="newLoading">
    <div class="loading-popup">
        <span class="loading-icon"></span>
        <div class="clear"></div>
    </div>
</div>


<!-- #include file="/includes/footerInner.aspx" -->
