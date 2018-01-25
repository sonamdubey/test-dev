var originalPushState = null;
var originalReplaceState = null;

var scrollPosition = { x : -1 , y : -1};

if (typeof Object.assign != 'function') {
  Object.assign = function(target, varArgs) { // .length of function is 2
    'use strict';
    if (target == null) { // TypeError if undefined or null
      throw new TypeError('Cannot convert undefined or null to object');
    }

    var to = Object(target);

    for (var index = 1; index < arguments.length; index++) {
      var nextSource = arguments[index];

      if (nextSource != null) { // Skip over if undefined or null
        for (var nextKey in nextSource) {
          // Avoid bugs when hasOwnProperty is shadowed
          if (Object.prototype.hasOwnProperty.call(nextSource, nextKey)) {
            to[nextKey] = nextSource[nextKey];
          }
        }
      }
    }
    return to;
  };
}


function customPushState() {
  const newStateOfCurrentPage = Object.assign({},window.history.state, {
    scrollToX : window.scrollX,
    scrollToY : window.scrollY
    
  });


  originalReplaceState.call(window.history,newStateOfCurrentPage,'');
  originalPushState.apply(window.history,arguments);
  
}

function customReplaceState(state,...otherArgs) {
  const newState = Object.assign({}, {
    scrollToX : (window.history.state && window.history.state.scrollToX >=0 ) ? window.history.state.scrollToX >=0 : -1 ,
    scrollToY : (window.history.state && window.history.state.scrollToY >=0 ) ? window.history.state.scrollToY >=0 : -1 ,
  },state);
  originalReplaceState.apply(window.history, [newState].concat(otherArgs));

}

function onPopState() {
  
  const state = window.history.state;
  if(state && Number.isFinite(state.scrollToX) && Number.isFinite(state.scrollToY)) {
    scrollPosition.x = state.scrollToX;
    scrollPosition.y = state.scrollToY;
  }
  else {
    resetScrollPosition();
  }
}

function wrapHistoryAPIFunction() {
  originalPushState  = window.history.pushState;
  originalReplaceState = window.history.replaceState;

  window.history.pushState = customPushState ;  
  window.history.replaceState = customReplaceState;

  window.history.scrollRestoration = 'manual';
}


function resetScrollPosition() {

    scrollPosition.x = -1;
    scrollPosition.y = -1;

}


function isBrowserWithoutScrollSupport() {
  var ua = window.navigator.userAgent;
  if(ua.indexOf('UCBrowser') === -1 && (!(ua.indexOf('Safari') != -1 && ua.indexOf('Chrome') === -1)) && ua.indexOf('CriOS') === -1){
    return false;
  }
  return true;
}

module.exports = {
  customPushState,
  customReplaceState,
  onPopState,
  scrollPosition,
  wrapHistoryAPIFunction,
  resetScrollPosition,
  isBrowserWithoutScrollSupport
}