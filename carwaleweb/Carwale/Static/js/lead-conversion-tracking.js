var leadConversionTracking =
{
    //Google AdWords Tracking
    googleAdwords : function ()
    {
        var goog_snippet_vars = function () {
            var w = window;
            w.google_conversion_id = 999894061;
            w.google_conversion_label = "L3GuCJ-X7XQQrdjk3AM";
            w.google_remarketing_only = false;
        }
        // DO NOT CHANGE THE CODE BELOW.
        var goog_report_conversion = function (url) {
            goog_snippet_vars();
            window.google_conversion_format = "3";
            var opt = new Object();
            opt.onload_callback = function () {
                if (typeof (url) != 'undefined') {
                    window.location = url;
                }
            }
            var conv_handler = window['google_trackConversion'];
            if (typeof (conv_handler) == 'function') {
                conv_handler(opt);
            }
        }
        goog_report_conversion();
    },

    facebook: function (leadClickSourceId, dealerId) {
        try {
            var utmz = isCookieExists('__utmz');
            var cwutmz = $.cookie('_cwutmz');
            if (isCookieExists('_cwutmz') && cwutmz.search('facebook') > 0 && cwutmz.search('cpc') > 0) {
                var FACEBOOKPIXELID = '536361110137264';
                fbq('init', FACEBOOKPIXELID);
                fbq('track', 'Lead');
            }
            if (utmz && utmz.search('bing') > 0 && utmz.search('cpc') > 0) {
                var uetq = uetq || [];
                uetq.push({ 'ec': 'Lead', 'ea': 'Lead Sumbit Click', 'el': leadClickSourceId, 'ev': dealerId });
            }
        }
        catch (e) { }
    },

    forkSurge: function () {
        try {
            var identifier = $.cookie('CWC');
            var tag_url = new Image();
            tag_url.src = "https://forksurgetrk.com/p.ashx?o=12988&e=422&f=img&t=" + identifier;         
        }
        catch (e) { }
    },

    track: function (leadClickSourceId, dealerId) {
            leadConversionTracking.facebook(leadClickSourceId, dealerId);
            leadConversionTracking.googleAdwords();
            leadConversionTracking.outbrainPixel();
            leadConversionTracking.forkSurge();
    },

    floodLightTrackingFord: function () {
        var axel = Math.random() + "";
        var num = axel * 10000000000000;
        var tag_url = new Image();
        tag_url.src = "https://1099301.fls.doubleclick.net/activityi;src=1099301;type=33;cat=carwa0;dc_lat=;dc_rdid=;tag_for_child_directed_treatment=;ord=" + num + "?";
    },

    outbrainPixel:function(){
        obApi('track', 'OutbrainLeads');
    }

}