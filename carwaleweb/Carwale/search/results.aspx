<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>

<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 0;
	Title 			= "Search Result";
	Description 	= "Google Custom Search";
	Keywords		= "Google Custom Search";
	Revisit 		= "15";
	DocumentState 	= "Static";
    AdId            = "1337757786091";
    AdPath          = "/7590/CarWale_Search/CarWale_Search_";
%>
 <!-- #include file="/includes/global/head-script.aspx" -->   
<style>
	.gsearch{background-color:#E7F8FF; border:1px solid #D4E8F1; padding:10px; width:540px; text-align:center; margin-top:20px;}
	.breadcrumb{list-style:none; padding:0; font-size:11px; height:15px; margin-top:15px;}
	.breadcrumb li{float:left; padding:0 2px;}
	.breadcrumb li.fwd-arrow{font-size:12px;}
	.breadcrumb .current{color:#5B5B5B;}
</style>
<script type="text/javascript">
	$(document).ready(function(){		
		/* related to google search */
		$("#queryText2").keypress(function(e){		
			if(e.keyCode == 13) doSearch( $(this).val() );
		});
		
		$("#doSearch2").click(function(){
		    doSearch($("#queryText2").val());
		});

		var searchtext = getParameterByName("q");
		$("#queryText2").val(searchtext);

        function getParameterByName(name) {
            var match = RegExp('[?&]' + name + '=([^&]*)')
                            .exec(window.location.search);
            return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
        }
        
	});
</script>

</head>
<body class="bg-light-grey header-fixed-inner">
     <!-- #include file="/includes/header.aspx" -->
    <section class="bg-light-grey padding-top10 no-bg-color">
        <div class="container">
            <div class="grid-12">
            <ul class="breadcrumb special-skin-text">
                <li><a href="/">Home</a></li>
                <li class="current">&rsaquo; <strong>Search Result</strong></li>
            </ul>
            <div class="clear"></div>
            <div class="border-solid-bottom margin-top10"></div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    <div class="clear"></div>
    <section class="bg-light-grey">
        <div class="container">
            <div class="grid-12">
                <div class="gsearch content-box-shadow content-inner-block-10 margin-bottom20">
	                <strong>Search</strong> <input id="queryText2" type="text" size="35" class="text" style="padding:0.5em; -moz-border-radius:3px;"  />
	                <input type="button" id="doSearch2" value="Search" class="btn btn-link btn-xs"/>
                </div>
                <div id="cse-search-results" class=" content-box-shadow content-inner-block-10 margin-bottom20"></div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    <script type="text/javascript">
      var googleSearchIframeName = "cse-search-results";
      var googleSearchFormName = "cse-search-box";
      var googleSearchFrameWidth = 974;
      var googleSearchDomain = "www.google.com";
      var googleSearchPath = "/cse";
    </script>
    <script type="text/javascript" src="https://stc.aeplcdn.com/js/show_afs_search.js"></script>
    <!-- #include file="/includes/footer.aspx" -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
</body>
</html>
