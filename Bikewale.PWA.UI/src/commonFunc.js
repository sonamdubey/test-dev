// function extractPageNoFromURL(url) {
// 	var url = window.location.pathname;
// 	//var matches =  url.match(/page\/(\d+)\/$/);
// 	var regex = /\d+(?=\D*$)/;
// 	var matches = regex.exec(url);

	
// 	if(matches) {
//   	return matches[0];
// 	}
// 	else{
//     return 1; // no /page/no in url means first page
//   } 
// }

// function mapNewsArticleDataToInitialData (article) {
// 	if(!article)
// 		return null;
// 	var initialData = {
//             ArticleUrl : article.ArticleUrl?article.ArticleUrl:"",
//             BasicId: article.BasicId?article.BasicId:"",
//             Title : article.Title?article.Title:"",
//             AuthorName : article.AuthorName?article.AuthorName:"",
//             DisplayDateTime : article.DisplayDateTime?article.DisplayDateTime:"",
//             ArticleApi : article.ArticleApi?article.ArticleApi:"",
//             LargePicUrl :  article.LargePicUrl?article.LargePicUrl:"",
//             HostUrl : article.HostUrl?article.HostUrl:"",
// 			AuthorMaskingName : article.AuthorMaskingName || ''
//         }
       
// 	return initialData;

// }

// function isServer() {
// 	return !(typeof window !== 'undefined' && window.document) //TODO 
// }


// function isInt(value) {

//   if (isNaN(value)) {
//     return false;
//   }
//   var x = parseFloat(value);
//   return (x | 0) === x;
// }

// function addAdToGTcmd(adUnitPath , adDimension , adDivId) {
//   googletag.cmd.push(function () {
//               var slot = googletag.defineSlot(adUnitPath, adDimension, adDivId);
//               if(!slot) return;
//               slot.addService(googletag.pubads());
//               googletag.pubads().enableSingleRequest();
//               googletag.pubads().collapseEmptyDivs();
//               googletag.pubads().disableInitialLoad();
//               googletag.enableServices();
//               googletag.display(adDivId);
//           });
// }

// function addAdSlot(adUnitPath , adDimension , adDivId) {
//   try {
//     if(!googletag && !googletag.cmd)
//       return;

//     var adExists = false; // remains false if the ad slot has already been defined irrespective of whether googletag has loaded

//     // event listener slotrenderended is added in appshell itself as it is not specific to any particuler ad
//     if(googletag.apiReady) 
//     {
//       var slots = googletag.pubads().getSlots();
//       for(var i=0;i<slots.length;i++) {
//         var ad = slots[i];
//         if(ad.getAdUnitPath() === adUnitPath) {
//             adExists = true;
//             break;
//         }
//       }
//     }  
//     if(!adExists) {
//         addAdToGTcmd(adUnitPath , adDimension , adDivId);
//     }    
    
//   }
//   catch(e) {
    
//   }
	  
    
// }

// function removeAdSlot(adUnitPath) {
//   try {
//     if(typeof googletag == "undefined" || !googletag.apiReady) {
//       return;
//     }
//     var slots = googletag.pubads().getSlots();
//     for(var i = 0;i<slots.length;i++) {
//       var ad = slots[i];
//       if(ad.getAdUnitPath() === adUnitPath) {
//         googletag.destroySlots([ad]);
//         break;
//       }
//     }  
//   }
//   catch(e) {
   
//   }
  
  
// }


// function refreshGPTAds() {
//  try {
//   setTimeout(function(){
//     if(typeof googletag != "undefined") {
//       googletag.cmd.push(function () {
//         googletag.pubads().refresh();
//       });
     
//     }
//   },100)
    
//  }
//  catch(e) {
//  }
  
// }

// function triggerPageView(url,title) {
//     if(typeof dataLayer == "undefined") {
//       return;
//     }
//     if((window.FirstRenderWithAppshell == undefined || window.FirstRenderWithAppshell == null) || (window.FirstRenderWithAppshell == false)) {
//       dataLayer.push({
//         'event' : 'VirtualPageview',
//         'virtualPageURL' : url,
//         'virtualPageTitle' : title
//       })
//     }

// }

// function lockPopup() {
//     document.body.classList.add('lock-browser-scroll');
//     document.getElementsByClassName('blackOut-window')[0].style.display = 'block';

// }

// function openGlobalSearchPopUp() {
//   try{
//     document.getElementById('global-search-popup').style.display = 'block';
//     document.getElementById('globalSearch').focus();
//     lockPopup();

//   }
//   catch(err) {}
  
// }

// function openGlobalCityPopUp() {
//   try {
//     document.getElementById('globalcity-popup').style.display = 'block';
  
//     lockPopup();  
//       document.getElementById('error-icon').classList.add('hide');
//       document.getElementById('bw-blackbg-tooltip').classList.add('hide');
//       document.getElementById("globalCityPopUp").classList.remove('border-red');

    
//     document.getElementById("loaderGlobalCity").style.display = 'none';

//     window.location.hash = "globalCity";
//   }
//   catch(err) {}

// }

////// GA related functions 

// function triggerNonInteractiveGA(cat, act, lab) {
//     try {

//         dataLayer.push({ 'event': 'Bikewale_noninteraction', 'cat': cat, 'act': act, 'lab': lab });
//     }
//     catch (e) {// log error   
//     }
// }

// function triggerDataToBhrigu(cat,act,lbl){
//     try {
//       cwTracking.trackCustomData(cat,act,lbl);
//     }catch(err) {}
// }
// if(!isServer()) {
//   window.triggerDataToBhrigu = triggerDataToBhrigu;  
// }

// function triggerGA(cat, act, lab) {
//     try {
//         dataLayer.push({ 'event': 'Bikewale_all', 'cat': cat, 'act': act, 'lab': lab });
//     }
//     catch (e) {// log error   
//     }
// }

// function pushNavMenuAnalytics(menuItem) {
//   try {
//     var categ = GetCatForNav();
//     if (categ != null) {
//         triggerGA(categ, 'Hamburger_Menu_Item_Click', menuItem);
//     }
//   }
//   catch(err) {
//     console.log(err);
//   }
    
// }

// function GetCatForNav() {
//     var ret_category = "other";
//     if (ga_pg_id != null && ga_pg_id != "0") {
//         switch (ga_pg_id) {
//             case "1":
//                 ret_category = "HP";
//                 break;
//             case "2":
//                 ret_category = "Model_Page";
//                 break;
//             case "3":
//                 ret_category = "Make_Page";
//                 break;
//             case "4":
//                 ret_category = "New_Bikes_Page";
//                 break;
//             case "5":
//                 ret_category = "Search_Page";
//                 break;
//             case "6":
//                 ret_category = "BikeWale_PQ";
//                 break;
//             case "7":
//                 ret_category = "Dealer_PQ";
//                 break;
//             case "8":
//                 ret_category = "Booking_Config_Page";
//                 break;
//             case "9":
//                 ret_category = "Booking_Page";
//                 break;
//             case "10":
//                 ret_category = "News_Page";
//                 break;
//             case "11":
//                 ret_category = "News_Detail";
//                 break;
//             case "12":
//                 ret_category = "Expert_Reviews_Page";
//                 break;
//             case "13":
//                 ret_category = "Expert_Reviews_Detail";
//                 break;
//             case "14":
//                 ret_category = "BookingSummary_New";
//                 break;
//             case "15":
//                 ret_category = "SpecsAndFeature";
//                 break;
//             case "16":
//                 ret_category = "Price_in_City_Page";
//                 break;
//             case "39":
//                 ret_category = "BookingListing";
//                 break;
//         }
//     }
//     return ret_category;
// }

// if(!isServer()) {
//     var cwTracking = {
//       hostPath: location.host,
//       type: {
//           impression: "imp",
//           click: "click"
//       },
//       getSource: function() {
//           var t = document.referrer,
//               e = location.host;
//           return e.indexOf(cwTracking.hostPath) >= 0 ? "" : "src=" + t
//       },
//       getPageUrl: function() {
//           var t = document.location.href;
//           return t.indexOf("?") > 0 ? "pi=" + t.split("?")[0] : t.indexOf("#") > 0 ? "pi=" + t.split("#")[0] : "pi=" + t
//       },
//       getQSFromUrl: function() {
//           var t = document.location.href,
//               e = "";
//           return t.indexOf("?") > 0 ? e = t.split("?")[1] : t.indexOf("#") > 0 && (e = t.split("#")[1]), e.length > 0 ? (e = e.replace(/&+/g, "|"), "qs=" + e) : ""
//       },
//       getReferrer: function() {
//           var t = document.referrer,
//               e = location.host;
//           return e.indexOf(cwTracking.hostPath) >= 0 && t.length > 0 ? "ref=" + t.split(cwTracking.hostPath)[1] : ""
//       },
//       addToQS: function(t, e) {
//           return t.length > 0 ? t += "&" + e : t = e, t
//       },
//       getAttributeValue: function(t) {
//           switch (t) {
//               case "imp":
//                   return "cwti";
//               case "click":
//                   return "cwtc";
//               default:
//                   return "cwcti"
//           }
//       }
//       ,
//       getCompleteQS: function(t, e) {
//           var i, c, n, r = "",
//               a = cwTracking.getAttributeValue(e),
//               o = !1;
//           return t.is("[qs]") && (o = !0), i = t.data(a + "cat"), c = t.data(a + "lbl"), n = t.data(a + "act"), r = cwTracking.getFinalQS(i, n, c, o)
//       },
//       getFinalQS: function(t, e, i, c) {
//           var n, r, a, o, g = "";
//           return n = cwTracking.getSource(), r = cwTracking.getPageUrl(), o = cwTracking.getReferrer(), void 0 != t && t.length > 0 && (g = cwTracking.addToQS(g, "cat=" + t)), void 0 != e && e.length > 0 && (g = cwTracking.addToQS(g, "act=" + e)), void 0 != i && i.length > 0 && (g = cwTracking.addToQS(g, "lbl=" + i)), void 0 != n && n.length > 0 && (g = cwTracking.addToQS(g, n)), void 0 != r && r.length > 0 && (g = cwTracking.addToQS(g, r)), void 0 != o && o.length > 0 && (g = cwTracking.addToQS(g, o)), c && (a = cwTracking.getQSFromUrl(), void 0 != a && a.length > 0 && (g = cwTracking.addToQS(g, a))), g
//       },
//       trackCustomData: function(t, e, i, c) {
//           var n = cwTracking.getFinalQS(t, e, i, c);
//           cwTracking.sendRequest(n)
//       },
//       trackDataFromNode: function(t, e) {
//           var i = cwTracking.getCompleteQS(t, e);
//           cwTracking.sendRequest(i)
//       },
//       sendRequest: function(t) {
//           t.length > 0 && (t = "&" + t);
//           var e = new Image;
//           e.src = cwTracking.getHandlerUrl() + Date.now() + t
//       },
//       getHandlerUrl: function() {
//           switch (cwTracking.hostPath) {
//               case "www.bikewale.com":
//                   return "https://bhrigu.bikewale.com/pixel.gif?t=";
//               case "bikewale.com":
//                   return "https://bhrigu.bikewale.com/pixel.gif?t=";
//               case "localhost":
//                   return "https://bhrigustg.bikewale.com/pixel.gif?t=";
//               case "webserver":
//                   return "https://bhrigustg.bikewale.com/pixel.gif?t=";
//               case "staging.bikewale.com":
//                   return "https://bhrigustg.bikewale.com/pixel.gif?t=";
//               default:
//                   return "https://bhrigustg.bikewale.com/pixel.gif?t="
//           }
//       },
//       // ,
//       // trackImpression: function() {
//       //     $("[cwt]").bind("inview", function(t, e) {
//       //         if (1 == e) cwTracking.trackDataFromNode($(this), cwTracking.type.impression);
//       //         else {
//       //             var i = $(this);
//       //             i.unbind("inview"), i.removeAttr("cwt")
//       //         }
//       //     })
//       // },
//       trackPerformace: function(type) {
//           var t, e = 0,
//               i = "",
//               c = "";
//           if (void 0 != window.performance && void 0 != window.performance.timing) {
//               t = JSON.stringify(window.performance.timing), t = JSON.parse(t);
//               for (var n in t) c += n + "=" + t[n] + "|";
//               e = c.length, e > 0 && (c = c.substr(0, e - 1)), i = type, cwTracking.trackCustomData("BWPerformance", i, c, !1)
//           }
//       }
//       // ,
//       // uiEvents: function() {
//       //     var t = "undefined" != typeof bwTrackingCat ? bwTrackingCat : "BWPageViews",
//       //         e = "undefined" != typeof bwTrackingAct ? bwTrackingAct : "",
//       //         i = "undefined" != typeof bwTrackingLab ? bwTrackingLab : "NA";
//       //     cwTracking.trackCustomData(t, e, i, !0), $(document).on("click", "[data-cwtccat]", function() {
//       //         cwTracking.trackDataFromNode($(this), cwTracking.type.click)
//       //     }), cwTracking.trackImpression(), $(document).ajaxComplete(function() {
//       //         cwTracking.trackImpression()
//       //     })
//       // }
//   };
//   window.onload = function() {
//     setTimeout(function() {
//           if(window.state !== undefined && window.state !== null) {
//             cwTracking.trackPerformace("Msite");  
//           }
//           else {
//             cwTracking.trackPerformace("MsiteAppShell");   
//           }
          
//       }, 100)
//   }
// // $(window).load(function() {
// //     setTimeout(function() {
// //         cwTracking.trackPerformace()
// //     }, 100)
// // }), $(document).ready(function() {
// //     cwTracking.uiEvents()
// // });

  
// }

// function isBrowserWithoutScrollSupport() {
//   var ua = window.navigator.userAgent;
//   if(ua.indexOf('UCBrowser') === -1 && (!(ua.indexOf('Safari') != -1 && ua.indexOf('Chrome') === -1)) && ua.indexOf('CriOS') === -1){
//     return false;
//   }
//   return true;
// }


////// scroll position handling
// if (typeof Object.assign != 'function') {
//   Object.assign = function(target, varArgs) { // .length of function is 2
//     'use strict';
//     if (target == null) { // TypeError if undefined or null
//       throw new TypeError('Cannot convert undefined or null to object');
//     }

//     var to = Object(target);

//     for (var index = 1; index < arguments.length; index++) {
//       var nextSource = arguments[index];

//       if (nextSource != null) { // Skip over if undefined or null
//         for (var nextKey in nextSource) {
//           // Avoid bugs when hasOwnProperty is shadowed
//           if (Object.prototype.hasOwnProperty.call(nextSource, nextKey)) {
//             to[nextKey] = nextSource[nextKey];
//           }
//         }
//       }
//     }
//     return to;
//   };
// }


// function customPushState() {
//   const newStateOfCurrentPage = Object.assign({},window.history.state, {
//     scrollToX : window.scrollX,
//     scrollToY : window.scrollY
    
//   });


//   originalReplaceState.call(window.history,newStateOfCurrentPage,'');
//   originalPushState.apply(window.history,arguments);
  
// }

// function customReplaceState(state,...otherArgs) {
//   const newState = Object.assign({}, {
//     scrollToX : (window.history.state && window.history.state.scrollToX >=0 ) ? window.history.state.scrollToX >=0 : -1 ,
//     scrollToY : (window.history.state && window.history.state.scrollToY >=0 ) ? window.history.state.scrollToY >=0 : -1 ,
//   },state);
//   originalReplaceState.apply(window.history, [newState].concat(otherArgs));

// }

// function onPopState() {
  
//   const state = window.history.state;
//   if(state && Number.isFinite(state.scrollToX) && Number.isFinite(state.scrollToY)) {
//     scrollPosition.x = state.scrollToX;
//     scrollPosition.y = state.scrollToY;
//   }
//   else {
//     resetScrollPosition();
//   }
// }

// function wrapHistoryAPIFunction() {
//   originalPushState  = window.history.pushState;
//   originalReplaceState = window.history.replaceState;

//   window.history.pushState = customPushState ;  
//   window.history.replaceState = customReplaceState;

//   window.history.scrollRestoration = 'manual';
// }

// var originalPushState = null;
// var originalReplaceState = null;

// var scrollPosition = { x : -1 , y : -1};

// function resetScrollPosition() {

//     scrollPosition.x = -1;
//     scrollPosition.y = -1;

// }

////// end scroll position handling

// USER REVIEW SLUG LOGIC
// var CMSUserReviewSlugSearchKey = 'showeditcmsreviewslug';
// var CMSUserReviewSlugPosition = 5;
// function isCMSUserReviewSlugClosed() {
//   var value = bwcache.get(CMSUserReviewSlugSearchKey,true);
//   if(value) {
//     return true;
//   }
//   else {
//     return false;
//   }
//  }

////  REDUCER COMMON FUNC
// import {fromJS} from 'immutable'
// var updateData = function(state,updateDict) {
//   try{
//     return state.withMutations(function(state) {
//       for(var key in updateDict) {
//         state = state.update(key, prevVal => fromJS(updateDict[key]));
//       }
//       return state;
//     })
//     // return state.withMutations(state => state.update(statusVarName , prevVal => fromJS(status))
//                           // .update(dataVarName , prevVal => fromJS(data))); 
//   }
//   catch(err){
//     console.log(err);
//     return state;
//   }
  
// }


module.exports = {
  // openGlobalSearchPopUp,
  // openGlobalCityPopUp,
  // triggerNonInteractiveGA,
	
	// isServer,
	// isInt,
	// addAdSlot,
  // removeAdSlot,
  // refreshGPTAds,
  // triggerPageView,
  // customPushState,
  // customReplaceState,
  // onPopState,
  // scrollPosition,
  // cwTracking,
  // triggerGA,
  // GetCatForNav,
  // pushNavMenuAnalytics,
  // wrapHistoryAPIFunction,
  // resetScrollPosition,
  // isBrowserWithoutScrollSupport,
  // triggerDataToBhrigu,
  // isCMSUserReviewSlugClosed,
  // CMSUserReviewSlugPosition,
  // updateData
}