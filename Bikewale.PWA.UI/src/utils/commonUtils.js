
function isServer() {
	return !(typeof window !== 'undefined' && window.document) //TODO 
}

function detectBrowser() {
  window.isBrowserUC = navigator.userAgent.toLowerCase().indexOf("ucbrowser") !== -1;
  window.isBrowserSafari = navigator.userAgent.toLowerCase().indexOf("safari") !== -1;
  window.isBrowserChrome = navigator.userAgent.toLowerCase().indexOf("chrome") !== -1;
}

function isInt(value) {

  if (isNaN(value)) {
    return false;
  }
  var x = parseFloat(value);
  return (x | 0) === x;
}
var CMSUserReviewSlugSearchKey = 'showeditcmsreviewslug';
var CMSUserReviewSlugPosition = 5;
function isCMSUserReviewSlugClosed() {
  var value = bwcache.get(CMSUserReviewSlugSearchKey,true);
  if(value) {
    return true;
  }
  else {
    return false;
  }
 }

import {fromJS} from 'immutable'
var updateData = function(state,updateDict) {
  try{
    return state.withMutations(function(state) {
      for(var key in updateDict) {
        state = state.update(key, prevVal => fromJS(updateDict[key]));
      }
      return state;
    })
    // return state.withMutations(state => state.update(statusVarName , prevVal => fromJS(status))
                          // .update(dataVarName , prevVal => fromJS(data))); 
  }
  catch(err){
    console.log(err);
    return state;   
  }
  
}

function RemoveSpecialCharacters(inpString){
  var regex = /[\)'",=!+#\[*\]~;^<\(>]+/g;
    try{
      if(inpString != undefined)
      {
        return inpString.replace(regex, '');
      }
    }
    catch(err){
      console.log(err);
    }
    return inpString;
}

module.exports = {
	isServer,
	isInt,
  	isCMSUserReviewSlugClosed,
  	CMSUserReviewSlugPosition,
  	updateData,
	RemoveSpecialCharacters,
	detectBrowser
}