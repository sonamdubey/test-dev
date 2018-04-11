
function isServer() {
	return !(typeof window !== 'undefined' && window.document) //TODO 
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


(function () {

    'use strict';
    var errorLog = function (error) {
        try {
            if (error) {
                var xmlhttp = new XMLHttpRequest();
                xmlhttp.open("POST", "/api/JSException/");
                xmlhttp.setRequestHeader('Content-Type', ' application/x-www-form-urlencoded; charset=UTF-8');
                xmlhttp.send(formPostDataString(error));

            }
        } catch (e) {
            return false;
        }
    }

    window.onerror = function (message, filename, lineno, colno, err) {
        var error = {};
        var log_source = new RegExp(["aeplcdn", "bikewale"].join('|'));
        try {
                if(err != null) {
                    error.Details = err.message;
                    error.SourceFile = err.fileName;
                    error.ErrorType = err.name;
                    error.Trace = err.stack.toString();                
                }
                
                error.Details = error.Details || message || "";
                error.SourceFile = error.SourceFile || filename || "";
                error.ErrorType = error.ErrorType || "Uncaught Exception";
                error.LineNo = lineno || "Unable to trace";
                error.Trace = error.Trace || '-';
                errorLog(error);
            
        }
        catch (e) {
            return false;
        }
    };

    window.errorLog = errorLog;

    function formPostDataString(postData) {
        var str = '';
        var keys = Object.keys(postData);
        for(var i = 0 ; i < keys.length ; i++) {
            str += keys[i]+'=';
            str += (postData[keys[i]] == null || postData[keys[i]] == undefined ? '' : postData[keys[i]]) +'&';
        }
        return str;
		
    }
})();


module.exports = {
	isServer,
	isInt,
	isCMSUserReviewSlugClosed,
    CMSUserReviewSlugPosition,
    updateData
}