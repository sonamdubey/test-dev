

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
      type: {
          impression: "imp",
          click: "click"
      },
      getSource: function() {
          var t = document.referrer,
              e = location.host;
          return e.indexOf(cwTracking.hostPath) >= 0 ? "" : "src=" + t
      },
      getPageUrl: function() {
          var t = document.location.href;
          return t.indexOf("?") > 0 ? "pi=" + t.split("?")[0] : t.indexOf("#") > 0 ? "pi=" + t.split("#")[0] : "pi=" + t
      },
      getQSFromUrl: function() {
          var t = document.location.href,
              e = "";
          return t.indexOf("?") > 0 ? e = t.split("?")[1] : t.indexOf("#") > 0 && (e = t.split("#")[1]), e.length > 0 ? (e = e.replace(/&+/g, "|"), "qs=" + e) : ""
      },
      getReferrer: function() {
          var t = document.referrer,
              e = location.host;
          return e.indexOf(cwTracking.hostPath) >= 0 && t.length > 0 ? "ref=" + t.split(cwTracking.hostPath)[1] : ""
      },
      addToQS: function(t, e) {
          return t.length > 0 ? t += "&" + e : t = e, t
      },
      getAttributeValue: function(t) {
          switch (t) {
              case "imp":
                  return "cwti";
              case "click":
                  return "cwtc";
              default:
                  return "cwcti"
          }
      }
      ,
      getCompleteQS: function(t, e) {
          var i, c, n, r = "",
              a = cwTracking.getAttributeValue(e),
              o = !1;
          return t.is("[qs]") && (o = !0), i = t.data(a + "cat"), c = t.data(a + "lbl"), n = t.data(a + "act"), r = cwTracking.getFinalQS(i, n, c, o)
      },
      getFinalQS: function(t, e, i, c) {
          var n, r, a, o, g = "";
          return n = cwTracking.getSource(), r = cwTracking.getPageUrl(), o = cwTracking.getReferrer(), void 0 != t && t.length > 0 && (g = cwTracking.addToQS(g, "cat=" + t)), void 0 != e && e.length > 0 && (g = cwTracking.addToQS(g, "act=" + e)), void 0 != i && i.length > 0 && (g = cwTracking.addToQS(g, "lbl=" + i)), void 0 != n && n.length > 0 && (g = cwTracking.addToQS(g, n)), void 0 != r && r.length > 0 && (g = cwTracking.addToQS(g, r)), void 0 != o && o.length > 0 && (g = cwTracking.addToQS(g, o)), c && (a = cwTracking.getQSFromUrl(), void 0 != a && a.length > 0 && (g = cwTracking.addToQS(g, a))), g
      },
      trackCustomData: function(t, e, i, c) {
          var n = cwTracking.getFinalQS(t, e, i, c);
          cwTracking.sendRequest(n)
      },
      trackDataFromNode: function(t, e) {
          var i = cwTracking.getCompleteQS(t, e);
          cwTracking.sendRequest(i)
      },
      sendRequest: function(t) {
          t.length > 0 && (t = "&" + t);
          var e = new Image;
          e.src = cwTracking.getHandlerUrl() + Date.now() + t
      },
      getHandlerUrl: function() {
          switch (cwTracking.hostPath) {
              case "www.bikewale.com":
                  return "https://bhrigu.bikewale.com/pixel.gif?t=";
              case "bikewale.com":
                  return "https://bhrigu.bikewale.com/pixel.gif?t=";
              case "localhost":
                  return "https://bhrigustg.bikewale.com/pixel.gif?t=";
              case "webserver":
                  return "https://bhrigustg.bikewale.com/pixel.gif?t=";
              case "staging.bikewale.com":
                  return "https://bhrigustg.bikewale.com/pixel.gif?t=";
              default:
                  return "https://bhrigustg.bikewale.com/pixel.gif?t="
          }
      },
      // ,
      // trackImpression: function() {
      //     $("[cwt]").bind("inview", function(t, e) {
      //         if (1 == e) cwTracking.trackDataFromNode($(this), cwTracking.type.impression);
      //         else {
      //             var i = $(this);
      //             i.unbind("inview"), i.removeAttr("cwt")
      //         }
      //     })
      // },
      trackPerformace: function(type) {
          var t, e = 0,
              i = "",
              c = "";
          if (void 0 != window.performance && void 0 != window.performance.timing) {
              t = JSON.stringify(window.performance.timing), t = JSON.parse(t);
              for (var n in t) c += n + "=" + t[n] + "|";
              e = c.length, e > 0 && (c = c.substr(0, e - 1)), i = type, cwTracking.trackCustomData("BWPerformance", i, c, !1)
          }
      }
      // ,
      // uiEvents: function() {
      //     var t = "undefined" != typeof bwTrackingCat ? bwTrackingCat : "BWPageViews",
      //         e = "undefined" != typeof bwTrackingAct ? bwTrackingAct : "",
      //         i = "undefined" != typeof bwTrackingLab ? bwTrackingLab : "NA";
      //     cwTracking.trackCustomData(t, e, i, !0), $(document).on("click", "[data-cwtccat]", function() {
      //         cwTracking.trackDataFromNode($(this), cwTracking.type.click)
      //     }), cwTracking.trackImpression(), $(document).ajaxComplete(function() {
      //         cwTracking.trackImpression()
      //     })
      // }
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
	pushNavMenuAnalytics
  
}