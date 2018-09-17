<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.Search_old" Trace="false" Debug="false" %>
<%@ Import Namespace="System.Data" %>
<% //modified by SajalGupta for unfilled impression of ads on 04 Aug 2016.
    isAd300x250Shown = false;
    isAd300x250_BTFShown = false; %>
<!-- #include file="/UI/includes/headNew.aspx" -->
<script type="text/javascript" src="/UI/src/new/search.js?v=1.1"></script>
<style type="text/css">
    .sel_parama{border:1px solid #DFDFDF; color:#445566!important; padding:1px 1px 1px 5px; margin:3px; display:inline-block; text-decoration:none!important; border-radius:3px; cursor:pointer;}
    .sel_parama span{background-color:#DFDFDF; color:#445566; padding:0 3px; margin-left:5px; cursor:pointer;}
    .sel_parama_hover{border:1px solid #cc0000; color:#445566!important; padding:1px 1px 1px 5px; margin:3px; display:inline-block; text-decoration:none!important; border-radius:3px; cursor:pointer;}
    .sel_parama_hover span{background-color:#cc0000; color:#fff; padding:0 3px; margin-left:5px; cursor:pointer;}
    #app_filt li {display:block;}    
</style>
        <div class="container_12 margin-top15">
        <h1 class="grid_12">Search New Bikes <span>Find new bikes by budget, make-model</span></h1>       
        <div class="grid_4 margin-top15"><!--    Left Container starts here -->
            <div id="parms" class="grey-bg">
                <div class="content-block">
                    <h3>Budget</h3>
                    <ul id="budget" class="ul-params">
                        <li><a name="1" rel="nofollow" class="filter unchecked" href="search.aspx?budget=1">Up 55,000</a></li>
                        <li><a name="2" rel="nofollow" class="filter unchecked" href="search.aspx?budget=2">55,000-70,000</a></li>
                        <li><a name="3" rel="nofollow" class="filter unchecked" href="search.aspx?budget=3">70,000-80,000</a></li>
                        <li><a name="4" rel="nofollow" class="filter unchecked" href="search.aspx?budget=4">80,000-1,40,000</a></li>
                        <li><a name="5" rel="nofollow" class="filter unchecked" href="search.aspx?budget=5">1,40,000-2,50,000</a></li>
                        <li><a name="6" rel="nofollow" class="filter unchecked" href="search.aspx?budget=6">2,50,000-5,00,000</a></li>
                        <li><a name="7" rel="nofollow" class="filter unchecked" href="search.aspx?budget=7">5,00,000 and above</a></li>
                    </ul>
                    <div class="parms-sept"></div>
                    <h3>Make(s)</h3>                    
                    <ul id="make" class="ul-params">
                        <asp:repeater id="rptMakes" enableviewstate="false" runat="server"><itemtemplate><li><a rel="nofollow" class="filter unchecked" name="<%# ((DataRowView)Container.DataItem)["Value"] %>" href="search.aspx?make=<%# ((DataRowView)Container.DataItem)["Value"] %>"><%# ((DataRowView)Container.DataItem)["Text"] %></a></li></itemtemplate></asp:repeater>
                    </ul>
                    <div class="clear"></div>                   
                    <div class="parms-sept"></div>
                    <h3>Transmission</h3>
                    <ul id="transmission" class="ul-params">
                        <li><a name="1" rel="nofollow" class="filter unchecked" href="search.aspx?transmission=1">Automatic</a></li>
                        <li><a name="2" rel="nofollow" class="filter unchecked" href="search.aspx?transmission=2">Manual</a></li>
                    </ul>
                    <div class="parms-sept"></div>
                    <h3>Body Types</h3>
                    <div class="ul-params">
                        <ul id="bs" class="ul-horz ul-bs">
                            <li>
                                <div class="body-style Cruiser" title="Cruiser"></div>
                                <a name="1" rel="nofollow" href="search.aspx?bs=1" class="filter unchecked">Cruiser</a></li>
                            <li>
                                <div class="body-style Fully faired" title="Fully faired"></div>
                                <a name="2" rel="nofollow" href="search.aspx?bs=2" class="filter unchecked">Fully faired</a></li>
                            <li>
                                <div class="body-style Naked" title="Naked"></div>
                                <a name="3" rel="nofollow" href="search.aspx?bs=3" class="filter unchecked">Naked</a></li>
                            <li>
                                <div class="body-style Semi-faired" title="Semi-faired"></div>
                                <a name="4" rel="nofollow" href="search.aspx?bs=4" class="filter unchecked">Semi-faired</a></li>
                            <li>
                                <div class="body-style Scooter" title="Scooter"></div>
                                <a name="5" rel="nofollow" href="search.aspx?bs=5" class="filter unchecked">Scooter</a></li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <div class="parms-sept"></div>
                    <h3>Displacement</h3>
                    <ul id="ep" class="ul-params">
                        <li><a rel="nofollow" name="1" class="filter unchecked" href="search.aspx?ep=1">Up to 110 cc</a></li>
                        <li><a rel="nofollow" name="2" class="filter unchecked" href="search.aspx?ep=2">110-150 cc</a></li>
                        <li><a rel="nofollow" name="3" class="filter unchecked" href="search.aspx?ep=3">150-200 cc</a></li>
                        <li><a rel="nofollow" name="4" class="filter unchecked" href="search.aspx?ep=4">200-250 cc</a></li>
                        <li><a rel="nofollow" name="5" class="filter unchecked" href="search.aspx?ep=5">250-500 cc</a></li>
                        <li><a rel="nofollow" name="6" class="filter unchecked" href="search.aspx?ep=6">500 cc and more</a></li>
                    </ul>
                </div>
            </div>	        
        </div><!--    Left Container ends here -->
        <div class="grid_8 margin-top15"><!--    Right Container starts here -->         
            <div id="res_msg" visible="false" class="grey-bg content-block">
	            <h3>Oops! No bikes matching your criteria.</h3>
	            <p>Try broadening your search criteria</p>
            </div>
            <div id="app_filt" class="grey-bg content-block">
			    <span>You have selected&nbsp;&nbsp;<a onClick="javascript:removeSelection();" class="text-highlight pointer">Remove all selections</a></span>
			    <ul id="app_filters" class="margin-top5">
				    <li id="_budget" class="hide"><span>Budget: </span></li>
                    <li id="_make" class="hide"><span>Make(s): </span></li>
                    <li id="_fuel" class="hide"><span>Fuel Types: </span></li>
				    <li id="_transmission" class="hide"><span>Transmission: </span></li>
				    <li id="_bs" class="hide"><span>Body Types: </span></li>
				    <li id="_seat" class="hide"><span>Seating Capacity: </span></li>				
				    <li id="_ep" class="hide"><span>Engine Power: </span></li>
				    <li id="_feature" class="hide"><span>Important Features: </span></li>				
			    </ul>
			    <div class="hr-sep margin-bottom15"></div>
			    <div class="clear"></div>
		    </div><div class="clear"></div>        
            <div id="searchRes" class="margin-top10"></div>
        </div><!--    Right Container ends here -->
        <div class="clear"></div>
            </div>
 <script type="text/javascript">
     var qs_params = '<%= Request.ServerVariables["QUERY_STRING"] %>';
     if (qs_params.length == 0) {

         $("#dvDefaultMsg").show();
         $("#divSelectedCriteria").hide();
     }
 </script>
<!-- #include file="/UI/includes/footerInner.aspx" -->
