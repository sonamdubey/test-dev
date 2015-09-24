/*
* jQuery history plugin
*
* Copyright (c) 2006 Taku Sano (Mikage Sawatari)
* Licensed under the MIT License:
*   http://www.opensource.org/licenses/mit-license.php
*
* Modified by Lincoln Cooper to add Safari support and only call the callback once during initialization
* for msie when no initial hash supplied.
* API rewrite by Lauris Buk?is-Haberkorns
*/

(function ($) {

    function History() {
        this._curHash = '';
        this._callback = function (hash) { };
    };

    $.extend(History.prototype, {
        init: function (callback) {
            this._callback = callback;
            this._curHash = location.hash;

            if ($.browser.msie) {
                // To stop the callback firing twice during initilization if no hash present
                if (this._curHash == '') {
                    this._curHash = '#';
                }

                // add hidden iframe for IE
                $("body").prepend('<iframe id="jQuery_history" style="display: none;"></iframe>');
                var iframe = $("#jQuery_history")[0].contentWindow.document;
                iframe.open();
                iframe.close();
                iframe.location.hash = this._curHash;
            }
            else if ($.browser.safari) {
                // etablish back/forward stacks
                this._historyBackStack = [];
                this._historyBackStack.length = history.length;
                this._historyForwardStack = [];
                this._isFirst = true;
                this._dontCheck = false;
            }
            this._callback(this._curHash.replace(/^#/, ''));
            setInterval(this._check, 100);
        },

        add: function (hash) {
            // This makes the looping function do something
            this._historyBackStack.push(hash);

            this._historyForwardStack.length = 0; // clear forwardStack (true click occured)
            this._isFirst = true;
        },

        _check: function () {
            if ($.browser.msie) {
                // On IE, check for location.hash of iframe
                var ihistory = $("#jQuery_history")[0];
                var iframe = ihistory.contentDocument || ihistory.contentWindow.document;
                var current_hash = iframe.location.hash;
                if (current_hash != $.history._curHash) {

                    location.hash = current_hash;
                    $.history._curHash = current_hash;
                    $.history._callback(current_hash.replace(/^#/, ''));

                }
            } else if ($.browser.safari) {
                if (!$.history._dontCheck) {
                    var historyDelta = history.length - $.history._historyBackStack.length;

                    if (historyDelta) { // back or forward button has been pushed
                        $.history._isFirst = false;
                        if (historyDelta < 0) { // back button has been pushed
                            // move items to forward stack
                            for (var i = 0; i < Math.abs(historyDelta); i++) $.history._historyForwardStack.unshift($.history._historyBackStack.pop());
                        } else { // forward button has been pushed
                            // move items to back stack
                            for (var i = 0; i < historyDelta; i++) $.history._historyBackStack.push($.history._historyForwardStack.shift());
                        }
                        var cachedHash = $.history._historyBackStack[$.history._historyBackStack.length - 1];
                        if (cachedHash != undefined) {
                            $.history._curHash = location.hash;
                            $.history._callback(cachedHash);
                        }
                    } else if ($.history._historyBackStack[$.history._historyBackStack.length - 1] == undefined && !$.history._isFirst) {
                        // back button has been pushed to beginning and URL already pointed to hash (e.g. a bookmark)
                        // document.URL doesn't change in Safari
                        if (document.URL.indexOf('#') >= 0) {
                            $.history._callback(document.URL.split('#')[1]);
                        } else {
                            $.history._callback('');
                        }
                        $.history._isFirst = true;
                    }
                }
            } else {
                // otherwise, check for location.hash
                var current_hash = location.hash;
                if (current_hash != $.history._curHash) {
                    $.history._curHash = current_hash;
                    $.history._callback(current_hash.replace(/^#/, ''));
                }
            }
        },

        load: function (hash) {
            var newhash;

            if ($.browser.safari) {
                newhash = hash;
            } else {
                newhash = '#' + hash;
                location.hash = newhash;
            }
            this._curHash = newhash;

            if ($.browser.msie) {
                var ihistory = $("#jQuery_history")[0]; // TODO: need contentDocument?
                var iframe = ihistory.contentWindow.document;
                iframe.open();
                iframe.close();
                iframe.location.hash = newhash;
                this._callback(hash);
            }
            else if ($.browser.safari) {
                this._dontCheck = true;
                // Manually keep track of the history values for Safari
                this.add(hash);

                // Wait a while before allowing checking so that Safari has time to update the "history" object
                // correctly (otherwise the check loop would detect a false change in hash).
                var fn = function () { $.history._dontCheck = false; };
                window.setTimeout(fn, 200);
                this._callback(hash);
                // N.B. "location.hash=" must be the last line of code for Safari as execution stops afterwards.
                //      By explicitly using the "location.hash" command (instead of using a variable set to "location.hash") the
                //      URL in the browser and the "history" object are both updated correctly.
                location.hash = newhash;
            }
            else {
                this._callback(hash);
            }
        }
    });

    $(document).ready(function () {
        $.history = new History(); // singleton instance
    });

})(jQuery);

var multiple_select = false;
var isParamChecked = false;

$("#parms").ready(function(){	
	bindEventsOnPageLoad();	
	$.history.init(loadHistory);	
});

function loadHistory(hash){
	hashParams = hash;	
	if( validateHash(hashParams) ){       
        if( hashParams == "" ){
			$("#searchRes").html( $("#alert_msg").html() );
			$("#app_filt").hide();
			$("#res_msg").removeClass("hide").show();
		}else if (!multiple_select) {        			
			//processingWait(true);            
			setCheckedStatus(hashParams);
			filterResults();
			$("#res_msg").hide();
		}
	}else{		
	    $("#searchRes").html($("#alert_msg2").html());
	    $("#res_msg").hide();
	}
}

function setCheckedStatus(hash){
	var paramCollection = hash.split("&");	
	
	for( var i=0; i<paramCollection.length; i++){
		var param = paramCollection[i].split('=');
		if (param.length == 2) {
		    var obj_ul_params = $("#" + param[0]);
		    obj_ul_params.removeClass("hide").addClass("show"); // show the respective ul of selected param

		    $("#" + param[0] + "_exp_col").attr("title", "collapse").removeClass("expnd").addClass("collapse"); // manage expend/collapse icons

		    // manage checkbox state
		    var objFilter = obj_ul_params.find("a[name=" + param[1] + "]");
		    objFilter.removeClass("unchecked").addClass("checked");

		    if (param[0] == "mm") {
		        if (param[1] == "1") {
		            $("#more_makes").parent().nextAll().removeClass("hide");
		            $("#more_makes").find("span").hide();
		        }
		    }		    
		    appliedFilters(objFilter, param[1]);
		}
        if (hash.length == 0) {         
            $("#dvDefaultMsg").show();            
        } else {
            $("#dvDefaultMsg").hide();
        }
	}	
}

function appliedFilters(paramObj, paramVal) {
    var ulCriteria = $(paramObj).closest('ul').attr('id'),
	appCriteriaObj = $('#_' + ulCriteria), // parent <ul>		
	append_param_id = ulCriteria + paramVal;
   
    if ($(paramObj).hasClass('checked')) {
        var child_length = appCriteriaObj.children().length;
        var clause_name = child_length == 1 ? " " + $(paramObj).html() : "" + $(paramObj).html(),
		append_param = '<span id="' + ulCriteria + "_" + paramVal + '" class="text-grey2 sel_parama">' + clause_name + '<span id="removeSel">X</span></span>';
        appCriteriaObj.removeClass("hide").show().append(append_param);
    } else {
        $('#' + append_param_id).remove();
    }
}

function bindEventsOnPageLoad() {
    onTableRowHover();
    onRemFilterHover();
    onRemFilterClick();
	onChkBoxClick();
	onBodyStyleClick();
	onCompareBikesClick();
	onModelRowClick();
}

/************************************************************************************/
	// Page Load Event Bindings
/************************************************************************************/

function onTableRowHover() {
    $(".dt_body").live('mouseenter', function () {
        var _objRow = $(this);
        _objRow.addClass('dt_body_hover');
    }).live('mouseleave', function () {
        var _objRow = $(this);
        _objRow.removeClass('dt_body_hover');
    });
}

function onRemFilterHover() {
    $(".sel_parama").live('mouseover', function () {
        $(this).removeClass("sel_parama").addClass("sel_parama_hover");
    });
}

function onRemFilterClick() {
    $(".sel_parama_hover").live('mouseout', function () {
        $(this).removeClass("sel_parama_hover").addClass("sel_parama");
        $("span.remove_sel").remove();
    }).live('click', function () {
        var rem_filt = $(this).attr("id");
        var crit = rem_filt.split('_')[0];
        var critVal = rem_filt.split('_')[1];
        $("#" + crit).find("li a" + "[name=" + critVal + "]").toggleClass("checked unchecked");
        clearAppliedCriteria();
        if ($("#" + crit).find("li a" + "[name=" + critVal + "]").hasClass("checked")) {
            isParamChecked = true;
            hashParams += hashParams == "" ? crit + "=" + critVal : "&" + crit + "=" + critVal;
        } else {
            removeParamFromHashURL(crit + "=" + critVal);
            isParamChecked = false;
        }
        if (hashParams.indexOf("pn") > -1) {
            var tmpParams = hashParams.substring(0, hashParams.indexOf("pn"));
            if (tmpParams != "") { // show applied filters
                $("#app_filt").show();
            } else {
                $("#dvDefaultMsg").show();
            }
        } else {
            if (hashParams != "") {
                $("#app_filt").show();
            } else {
                $("#dvDefaultMsg").show();
            }
        }
        addHashParam();
    });
}

function onChkBoxClick(){
    $("#parms a.filter").live('click', function (e) {
        e.preventDefault();

        $(this).toggleClass("checked unchecked");

        var paramName = this.href.split('?')[1];
        if (paramName.indexOf('model') >= 0)
            multiple_select = true;
        else
            clearAppliedCriteria();

        if ($(this).hasClass("checked")) {
            isParamChecked = true;
            hashParams += hashParams == "" ? paramName : "&" + paramName;
            //alert(hashParams);
        } else {
            removeParamFromHashURL(paramName);
            isParamChecked = false;
        }        
        if (hashParams.indexOf("pn") > -1) {
            var tmpParams = hashParams.substring(0, hashParams.indexOf("pn"));
            if (tmpParams != "") { // show applied filters
                $("#app_filt").show();
            } else {               
                $("#dvDefaultMsg").show();
            }
        } else {
            if (hashParams != "") {
                $("#app_filt").show();
            } else {
                $("#dvDefaultMsg").show();
            }
        }
        addHashParam();
    });	
}

function onBodyStyleClick() {
    $("div.body-style").click(function () {
        var body_style = $(this).next();
        var paramName = body_style.attr("href").split('?')[1];

        body_style.toggleClass("checked unchecked");

        if (body_style.hasClass("checked")) {
            isParamChecked = true;
            hashParams += hashParams == "" ? paramName : "&" + paramName;
        } else {
            isParamChecked = false;
            removeParamFromHashURL(paramName);
        }
        clearAppliedCriteria();

        addHashParam();
    });
}

function onCompareBikesClick() {
    $("input[name=button].buttons").live('click', function (e) {
        e.preventDefault();
        var qs = "?";
        var j = 0;
        var baseUrl = "/comparebikes/";
        $("tr.version-row").find("input:checkbox:checked").each(function (i) {
            if (qs.length == 1) {
                baseUrl += $(this).attr("modelName");
                qs += "bike" + (i + 1) + "=" + $(this).val();
            }
            else {
                baseUrl += "-vs-" + $(this).attr("modelName");
                qs += "&bike" + (i + 1) + "=" + $(this).val();
            }
            j++;
        });
        if (qs != "") {
            if (j >= 2 && j <= 4)
                location.href = baseUrl + "/" + qs;
            else if (j < 2) {
                alert("Please select minimum two bikes to compare");
            }
            else if (j > 4) {
                alert("Please select maximum four bikes to compare");
            }
        }
        else {
            alert("Please choose at least two bikes to compare.\nClick on the Matching Versions link to see the bikes available for comparison.");
        }
    });
}


function onModelRowClick() {
    $("tr.model-row").live('click', function () {        
        expandModel($("#" + $(this).attr("id").split("_")[1]));
    });
}

function onViewVersionsClick() {
    $("a.viewVersions").live('click', function (e) {
        e.stopPropagation();
        expandModel(this);
    });
}

function addHashParam(){
	if( !multiple_select ){		
		if( hashParams.indexOf("pn") >= 0){
			var reg_str = "(&?pn=)[0-9]*";
			var regExp = new RegExp(reg_str);
			hashParams = hashParams.replace(regExp, "");
		}    
		$.history.load(hashParams);
	}	
}

function removeParamFromHashURL(paramName){
    var _hashParams = "";

    if (hashParams != "") {
        var hashParamArray = hashParams.split('&');
       
        if (hashParamArray.length > 0) {
            for (var i = 0; i < hashParamArray.length; i++) {
                if (hashParamArray[i] != paramName) {
                    _hashParams = _hashParams + hashParamArray[i] + "&";
                }
            }
        }        
    }

    if (_hashParams != "") {
        hashParams = _hashParams.substr(0, _hashParams.length - 1); // remove the last '&'
    } else { // last parameter to remove
        hashParams = "";
    }
}

function replaceKeyFromHashURL(paramKey){ //remove on the basis of only parameter key
	var regEx = new RegExp("&?("+ paramKey +"=)[0-9]*", "g");
	hashParams = hashParams.replace(regEx, "");
	replaceFirstAmp();	
}

function replaceFirstAmp(){
	if( hashParams.indexOf("&") == 0 )
		hashParams = hashParams.replace("&", "");
}

function loadingDone(){
	
}

function removeSelection(){	
	beginNewSearch();
	$("#searchRes").html( $("#alert_msg").html() );
}

function clearAppliedCriteria(){
	$("#app_filters li span:not(:first-child)").remove();
	$("#app_filters li").hide();
}

function filterResults() {
    if (hashParams != "") { // param selected
        $("#searchRes").html("<img src='http://img.aeplcdn.com/loader.gif' border='0'/>");
        setTimeout("loadSearchResults()", 300);
    } else {
        $("#searchRes").load("search_result.aspx");
    }
}

function loadSearchResults() {
    $("#searchRes").load("search_result.aspx?" + hashParams);
}

$(".dgNavDivTop a,.sortLink").live('click', function () {
    $('html,body').animate({ scrollTop: "200px" }, 0);
    clearAppliedCriteria();
    //processingWait(true);
    var navi_lnk = this.href;
    var qs = navi_lnk.split("?")[1];
    $.history.load(qs);
    setTimeout(function () { loadNavigation(navi_lnk); }, 500);
    return false;
});

function loadNavigation(navi_lnk) {
    $("#searchRes").load(navi_lnk, function () {
        loadingDone();
    });
}

function validateHash(hash){
	if(hash != ""){		
		var regEx = new RegExp("^(&?[a-z]*=[0-9]*([.][0-9]*)?)+$");		
		return regEx.test(hash);
	}else {return true};
}

function beginNewSearch() {
    $("a.checked").removeClass("checked").addClass("unchecked");    
    clearAppliedCriteria();
    $.history.load("");
    $("#dvDefaultMsg").show();    
}

function showMoreMakes(){
	$("#more_makes").click( function(){
		$(this).parent().nextAll().removeClass("hide");	
		$(this).find('span').hide();
		hashParams != "" ? hashParams += "&mm=1" : hashParams += "mm=1";
		addHashParam();
	});
}

function expandModel(obj) {
    var objModel = $(obj);
    var modelId = objModel.attr("id");

    var objHidden = objModel.find("#modHide");
    var objShow = objModel.find("#modShow");
    var objModelIcon = objModel.find("#modShowIcon");
   
    if (objHidden.hasClass("hide")) {
        objHidden.removeClass("hide").addClass("show");
        objShow.removeClass("show").addClass("hide");
        objModelIcon.removeClass("right-arrow").addClass("down-arrow2");
        
        $(".cls_" + modelId).show().removeClass("hide");        
        $(".cls_" + modelId + ":last").addClass("version-bot");
        $("#mod_" + modelId).addClass("model-row-bot");
    } else {
        objHidden.removeClass("show").addClass("hide");
        objShow.removeClass("hide").addClass("show");
        objModelIcon.removeClass("down-arrow2").addClass("right-arrow");

        $(".cls_" + modelId).hide().removeClass("show");;
        $(".cls_" + modelId + ":last").removeClass("version-bot");
        $("#mod_" + modelId).removeClass("model-row-bot");
    }
}

function compareSelected() {
    var qs = ""
    var j = 0;
    $("tr.version-row").find("input:checkbox:checked").each(function (i) {
        if (qs == "")
            qs += "bike" + (i + 1) + "=" + $(this).val();
        else
            qs += "&bike" + (i + 1) + "=" + $(this).val();

        j++
    });

    if (qs != "") {
        
        if (j >= 2)
            location.href = "/new/compare/comparespecs.aspx?" + qs;
        else {
        
            alert("Please select minimum two bikes to compare");
            return false;
        }
    }
    else {
        
        alert("Please choose at least two bikes to compare.\nClick on the Matching Versions link to see the bikes available for comparison.");
        return false;
    }
}	