﻿    var ga_pg_id = '0';

    (function (w, d, s, l, i) {
    w[l] = w[l] || []; w[l].push({
    'gtm.start':
    new Date().getTime(), event: 'gtm.js'
    }); var f = d.getElementsByTagName(s)[0],
    j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
    '//www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
    })(window, document, 'script', 'dataLayer', 'GTM-5CSHD6');

    setTimeout(function () {
        var a = document.createElement("script");
        var b = document.getElementsByTagName("script")[0];
        a.src = document.location.protocol + "//script.crazyegg.com/pages/scripts/0012/9477.js?" + Math.floor(new Date().getTime() / 3600000);
        a.async = true; a.type = "text/javascript"; b.parentNode.insertBefore(a, b)
    }, 1);

    var googletag = googletag || {};
    googletag.cmd = googletag.cmd || [];
    (function () {
        var gads = document.createElement('script');
        gads.async = true;
        gads.type = 'text/javascript';
        var useSSL = 'https:' == document.location.protocol;
        gads.src = (useSSL ? 'https:' : 'http:') +
            '//www.googletagservices.com/tag/js/gpt.js';
        var node = document.getElementsByTagName('script')[0];
        node.parentNode.insertBefore(gads, node);
    })();

    googletag.cmd.push(function () {
        <% if(Ad_320x50) { %>googletag.defineSlot('<%= AdPath%>_Top_320x50', [320, 50], 'div-gpt-ad-<%= AdId%>-0').addService(googletag.pubads());<% } %>
        <% if(Ad_Bot_320x50) { %>googletag.defineSlot('<%= AdPath%>_Bottom_320x50', [320, 50], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads());<% } %>
        <% if (Ad_300x250) { %>googletag.defineSlot('<%= AdPath%>_300x250', [300, 250], 'div-gpt-ad-<%= AdId%>-2').addService(googletag.pubads());<% } %>
        <% if (Ad320x150_I) { %>
        googletag.defineSlot('<%= AdPath%>_FirstSlot_320x150', [[320, 150], [320, 50], [320, 100], [320, 425]], 'div-gpt-ad-<%= AdId%>-3').addService(googletag.pubads());
        <% } %>
        <% if (Ad320x150_II) { %>
        googletag.defineSlot('<%= AdPath%>_SecondSlot_320x150', [[320, 150], [320, 50], [320, 100], [320, 425]], 'div-gpt-ad-<%= AdId%>-4').addService(googletag.pubads());
        <% } %>
        <% if (!String.IsNullOrEmpty(TargetedModel)) { %>googletag.pubads().setTargeting("Model", "<%= TargetedModel %>");<% } %>
        <% if (!String.IsNullOrEmpty(TargetedMakes)){ %>googletag.pubads().setTargeting("Make", "<%= TargetedMakes %>");<% } %>
        <% if (!String.IsNullOrEmpty(TargetedModels)){ %>googletag.pubads().setTargeting("CompareBike-M", "<%= TargetedModels %>");<% } %>
        <% if (!String.IsNullOrEmpty(TargetedCity)){%>googletag.pubads().setTargeting("City", "<%= TargetedCity %>");<%}%>
        googletag.pubads().enableSingleRequest();
        googletag.pubads().collapseEmptyDivs();
        googletag.enableServices();
    });
