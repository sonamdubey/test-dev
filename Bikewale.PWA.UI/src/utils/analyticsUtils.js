import {PQ_SOURCE_ID} from './constants'
function triggerPageView(url,title) {
    if(typeof dataLayer == "undefined") {
      return;
    }
    if((window.FirstRenderWithAppshell == undefined || window.FirstRenderWithAppshell == null) || (window.FirstRenderWithAppshell == false)) {
      dataLayer.push({
        'event' : 'VirtualPageview',
        'virtualPageURL' : url,
        'virtualPageTitle' : title
      })
    }

}
import {isServer} from './commonUtils'
if(!isServer()) {
    var cwTracking = {
        hostPath: location.host,
        isMobileSite: function () {
            var url = document.location.href;
            if (url.indexOf('/m/') > 0)
                return true;
            else
                return false;
        },
        type: {
            impression: 'imp',
            click: 'click'
        },

        getSource: function () {
            var url = document.referrer;
            if (url.length == 0)
                return 'src=direct';
            else if (url.indexOf(cwTracking.hostPath) < 0)
                return 'src=' + url;

            return '';
        },

        getPageUrl: function () {
            var url = document.location.href;
            if (url.indexOf('?') > 0)
                return 'pi=' + url.split('?')[0];
            else if (url.indexOf('#') > 0)
                return 'pi=' + url.split('#')[0];
            else
                return 'pi=' + url;
        },

        getQSFromUrl: function () {
            var url = document.location.href;
            var qs = '';
            if (url.indexOf('?') > 0) {
                qs = url.split('?')[1];
            }
            else if (url.indexOf('#') > 0) {
                qs = url.split('#')[1];
            }
            if (qs.length > 0) {
                qs = qs.replace(/&+/g, '|');
                return 'qs=' + qs;
            }
            else
                return '';
        },

        getReferrer: function () {
            var url = document.referrer;
            var host = location.host;
            if (url.length > 0 && url.indexOf(host) >= 0) {
                var relativePath = url.split(cwTracking.hostPath)[1];
                return 'ref=' + (relativePath == undefined || relativePath == null ? '' : relativePath.replace(/&+/g, '|'));
            }
            return '';
        },

        addToQS: function (qs, value) {
            if (qs.length > 0)
                qs += '&' + value;
            else
                qs = value;
            return qs;
        },

        getAttributeValue: function (type) {
            switch (type) {
                case 'imp': return 'cwti'; break;
                case 'click': return 'cwtc'; break;
                default: return 'cwcti';
            }
        },

        getCompleteQS: function (node, type) {
            var cat, lbl, act, qs = '', attrValue = cwTracking.getAttributeValue(type), sendQS = false;
            if (node.is('[qs]'))
                sendQS = true;
            cat = node.data(attrValue + 'cat'), lbl = node.data(attrValue + 'lbl'), act = node.data(attrValue + 'act');
            qs = cwTracking.getFinalQS(cat, act, lbl, sendQS);
            return qs;
        },

        getFinalQS: function (cat, act, lbl, sendQS) {
            var src, pageUrl, urlQs, ref, qs = '';
            src = cwTracking.getSource(), pageUrl = cwTracking.getPageUrl(), ref = cwTracking.getReferrer();
            if (cat != undefined && cat.length > 0)
                qs = cwTracking.addToQS(qs, 'cat=' + cat);
            if (act != undefined && act.length > 0)
                qs = cwTracking.addToQS(qs, 'act=' + act);
            if (lbl != undefined && lbl.length > 0)
                qs = cwTracking.addToQS(qs, 'lbl=' + lbl);
            if (src != undefined && src.length > 0)
                qs = cwTracking.addToQS(qs, src);
            if (pageUrl != undefined && pageUrl.length > 0)
                qs = cwTracking.addToQS(qs, pageUrl);

            if (ref != undefined && ref.length > 0)
                qs = cwTracking.addToQS(qs, ref);

            if (sendQS) {
                urlQs = cwTracking.getQSFromUrl();
                if (urlQs != undefined && urlQs.length > 0)
                    qs = cwTracking.addToQS(qs, urlQs);
            }

            return qs;
        },

        trackCustomData: function (cat, act, lbl, sendQS) {
            var qs = cwTracking.getFinalQS(cat, act, lbl, sendQS);
            cwTracking.sendRequest(qs);
        },

        trackDataFromNode: function (node, type) {
            var qs = cwTracking.getCompleteQS(node, type);
            cwTracking.sendRequest(qs);
        },

        sendRequest: function (qs) {
            if (qs.length > 0)
                qs = '&' + qs;
            var img = new Image();
            img.src = cwTracking.getHandlerUrl() + Date.now() + qs;
        },

        getHandlerUrl: function () {
            return "/bhrigu/pixel.gif?t=";
        },

        
        trackPerformace: function () {
            var len = 0, action = '', performanceQS = '', performanceTimings;
            if (window.performance != undefined && window.performance.timing != undefined) {
                performanceTimings = JSON.stringify(window.performance.timing);
                performanceTimings = JSON.parse(performanceTimings);
                for (var prop in performanceTimings) {
                    performanceQS += prop + "=" + performanceTimings[prop] + "|";
                }
                len = performanceQS.length;
                if (len > 0)
                    performanceQS = performanceQS.substr(0, len - 1);
                if (cwTracking.isMobileSite())
                    action = 'Msite';
                else
                    action = 'DesktopSite'
                cwTracking.trackCustomData('Performance', action, performanceQS, false);
            }
        },

        trackInviewGA: function (node) {
            var cat, lbl, act, role;
            cat = node.data('cat'), lbl = node.data('label'), act = node.data('action'), role = node.data('role');
            if (role.indexOf('click') >= 0 && act.indexOf('shown') === -1) {
                act = act + '_shown'
            }
            Common.utils.trackAction('CWNonInteractive', cat, act, lbl);
        },

        prepareLabel: function (trackingparam) {
            var label = '';
            if (trackingparam && typeof (trackingparam) == 'object') {
                for (var property in trackingparam) {
                    label = label + property + '=' + trackingparam[property] + '|';
                }
            }
            return label.substring(0, label.length - 1);
        },

        callImpressionTracking: function (node) {
            var role = node.data('role');
            if (role.indexOf('inview-imp') >= 0) {
                node.unbind('inview');
                node.data('role', role.replace('inview-imp', ''));
                cwTracking.trackInviewGA(node);
                return true;
            }
            return false;
        },
        trackUserReview: function (eventName, label) {
            cwTracking.trackCustomData("BWUserReviews", eventName, label, !1)
        },
        trackImagesInteraction: function (category, action, label) {
            cwTracking.trackCustomData(category, action, label, !1)
        },
    };

  window.onload = function() {
    setTimeout(function() {
          if(window.state !== undefined && window.state !== null) {
            cwTracking.trackPerformace("Msite");  
          }
          else {
            cwTracking.trackPerformace("MsiteAppShell");   
          }
          
      }, 100)
  }


  
}

function triggerGA(cat, act, lab) {
    try {
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': cat, 'act': act, 'lab': lab });
    }
    catch (e) {// log error   
    }
}

function setPQSourceId(pqPageSource) {
    return PQ_SOURCE_ID[pqPageSource];
}

function pushNavMenuAnalytics(menuItem) {
  try {
    var categ = GetCatForNav();
    if (categ != null) {
        triggerGA(categ, 'Hamburger_Menu_Item_Click', menuItem);
    }
  }
  catch(err) {
    console.log(err);
  }
    
}

function GetCatForNav() {
    var ret_category = "other";
    if (ga_pg_id != null && ga_pg_id != "0") {
        switch (ga_pg_id) {
            case "1":
                ret_category = "HP";
                break;
            case "2":
                ret_category = "Model_Page";
                break;
            case "3":
                ret_category = "Make_Page";
                break;
            case "4":
                ret_category = "New_Bikes_Page";
                break;
            case "5":
                ret_category = "Search_Page";
                break;
            case "6":
                ret_category = "BikeWale_PQ";
                break;
            case "7":
                ret_category = "Dealer_PQ";
                break;
            case "8":
                ret_category = "Booking_Config_Page";
                break;
            case "9":
                ret_category = "Booking_Page";
                break;
            case "10":
                ret_category = "News_Page";
                break;
            case "11":
                ret_category = "News_Detail";
                break;
            case "12":
                ret_category = "Expert_Reviews_Page";
                break;
            case "13":
                ret_category = "Expert_Reviews_Detail";
                break;
            case "14":
                ret_category = "BookingSummary_New";
                break;
            case "15":
                ret_category = "SpecsAndFeature";
                break;
            case "16":
                ret_category = "Price_in_City_Page";
                break;
            case "39":
                ret_category = "BookingListing";
                break;
        }
    }
    return ret_category;
}

function triggerDataToBhrigu(cat,act,lbl){
    try {
      cwTracking.trackCustomData(cat,act,lbl);
    }catch(err) {}
}
if(!isServer()) {
  window.triggerDataToBhrigu = triggerDataToBhrigu;  
}

function triggerNonInteractiveGA(cat, act, lab) {
    try {

        dataLayer.push({ 'event': 'Bikewale_noninteraction', 'cat': cat, 'act': act, 'lab': lab });
    }
    catch (e) {// log error   
    }
}

module.exports = {
	triggerPageView,
  	cwTracking,
  	triggerGA,
    GetCatForNav,
    triggerDataToBhrigu,
    triggerNonInteractiveGA,
    pushNavMenuAnalytics,
    setPQSourceId
  
}