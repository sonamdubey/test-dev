// Initialize collapse button for side nav
$(".buttons").sideNav();
$('.modal').modal();
$('.collapsible').collapsible();
$('.tooltipped').tooltip({ delay: 50 });
// Do not remove this code. Its written to get values from checkbox to controller
$("input:checkbox").change(function () { $(this).val($(this).is(':checked')); });
var select = $('select');

$(document).ready(function () {
	var bodyHeight = $('body').height();
	var bodyHeightTimerId = null;

	pageFooter.setPosition();

	$(document).on('click', function() {
		clearTimeout(bodyHeightTimerId);

		bodyHeightTimerId = setTimeout(function() {
			if ($('body').height() !== bodyHeight) {
				pageFooter.setPosition();
				bodyHeight = $('body').height();
			}
		}, 1000);
	});

});

var pageFooter = {
    bodyElement: $('body'),

    container: $('#page-footer'),

    setPosition: function () {
        var flag = pageFooter.bodyElement.height() + 50 - window.innerHeight > pageFooter.container.height();

        if (pageFooter.bodyElement.height() > window.innerHeight && flag) {
        	pageFooter.container.removeClass('footer-fixed').addClass('footer-relative');
        }
        else {
        	pageFooter.container.removeClass('footer-relative').addClass('footer-fixed');
        }
    }
};

(function ($) {
    $.fn.bw_easyAutocomplete = function (options) {
        return this.each(function () {
            if (options == null || options == undefined) {
                console.log("please define options");
                return;
            }
            else {
                
                if (options.source == null || options.source == undefined || options.source == '') {
                    console.log("please define source");
                    return;
                }

                if (!options.hosturlForAPI) {
                    console.log("please define hosturlForAPI");
                    return;
                }
            }

            var requestTerm;

            $(this).easyAutocomplete({
                url: function (value) {
                    if (options.beforefetch && typeof (options.beforefetch) == "function") {
                        options.beforefetch();
                    }

                    requestTerm = value.replace(/^\s\s*/, '').replace(/\s\s*$/, '').replace(/-/g, ' ').replace(/[^A-Za-z0-9 ]/g, '').toLowerCase();

                    if (requestTerm.length > 0) {
                        var path = options.hosturlForAPI + "/api/AutoSuggest/?source=" + options.source + "&inputText=" + requestTerm + "&noofrecords=8";

                        return path;
                    }
                },

                getValue: "text",

                sourceType: options.source,
                hosturlForAPI: options.hosturlForAPI,

                template: {
                    type: "custom",
                    method: function (value, item) {
                        switch(options.source)
                        {
                            case 1:
                                if (item.payload.modelId > 0 && item.payload.isNew != 'False' && item.payload.futuristic != 'True') {
                                    return ('<div class="list-item" >' + value + '</div>');
                                }
                                
                            case 3:
                              return  ('<div class="list-item" data-cityId="' + item.payload.cityId + '">' + value + '</div>');
                        }
                    }
                },

                list: {
                    maxNumberOfElements: options.resultCount,
                    onChooseEvent: function (event) {
                        options.click(event);
                    },
                    onLoadEvent: function () {
                        var suggestionResult = $(options.inputField).getItems();

                        if (options.afterFetch != null && typeof (options.afterFetch) == "function") {
                            options.afterFetch(suggestionResult, requestTerm);
                        }
                    }
                }
            });

            $(this).keyup(function () {
                if (options.keyup != undefined) {
                    options.keyup();
                }

                if ($(options.inputField).val().replace(/\s/g, '').length == 0 && options.onClear != undefined) {
                    options.onClear();

                    $(options.inputField).closest('.easy-autocomplete').find('ul').hide();
                }
            });

            $(this).focusout(function () {
                if (options.focusout != undefined) {
                    options.focusout();
                }
            });

        });
    };
}(jQuery));

$(document).ready(function () {
    var autocompleteInputField = $('.easy-autocomplete input');

    autocompleteInputField.each(function () {
        var autocompleteElement = $(this).closest('.easy-autocomplete'),
            labelElement = autocompleteElement.next('label');

        if (autocompleteElement.find('label').length == 0) {
            labelElement.insertAfter(autocompleteElement.find('input'));
        }
    });

    select.each(function () {
        if (!$(this).hasClass('chosen-select')) {
            $(this).material_select();
        }
    });
});

var materialSelect = {
    removeLabel: function (selectField) {
        selectField.closest('.select-wrapper').next('label').remove();
    }
}

// error messages
var validate = {
    inputField: {
        showError: function (inputField) {
            inputField.addClass('invalid');
        },
        hideError: function (inputField) {
            inputField.removeClass('invalid');
        }
    },

    selectField: {
        showError: function (selectField, message) {
            var inputField = selectField.siblings('input'),
                selectWrapper = selectField.closest('.select-wrapper');

            inputField.addClass('invalid');

            if(!selectWrapper.hasClass('label-present')) {
                selectWrapper.addClass('label-present');
                selectWrapper.append('<label class="error-label">' + message + '</label>');
            }
        },

        hideError: function (selectField) {
            var inputField = selectField.siblings('input'),
                selectWrapper = selectField.closest('.select-wrapper');

            inputField.removeClass('invalid');

            if (selectWrapper.hasClass('label-present')) {
                selectWrapper.removeClass('label-present');
                selectWrapper.find('label.error-label').remove();
            }
        }
    }
}

var progress = {    
    showProgress: function () {
        $(".progress").removeClass("hide").addClass("show");
    },

    hideProgress: function () {
        $(".progress").removeClass("show").addClass("hide");
    }
}

var fixedTable = function(element) {
	var tableBody = $(element).find('.fixed-table__body'),
		tableHeader = $(element).find('.fixed-table__header table'),
		tableSidebar = $(element).find('.fixed-table__sidebar table');

	return $(tableBody).scroll(function() {
		$(tableSidebar).css('margin-top', -$(tableBody).scrollTop());
		return $(tableHeader).css('margin-left', -$(tableBody).scrollLeft());
	});
}

var priceMonitoringTable = new fixedTable($('#priceMonitoringTable'));

var Base64 = {
    _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",
    encode: function (input) {
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;

        input = Base64._utf8_encode(input);

        while (i < input.length) {

            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);

            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;

            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }

            output = output + this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) + this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);

        }

        return output;
    },
    decode: function (input) {
        var output = "";
        var chr1, chr2, chr3;
        var enc1, enc2, enc3, enc4;
        var i = 0;

        input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

        while (i < input.length) {

            enc1 = this._keyStr.indexOf(input.charAt(i++));
            enc2 = this._keyStr.indexOf(input.charAt(i++));
            enc3 = this._keyStr.indexOf(input.charAt(i++));
            enc4 = this._keyStr.indexOf(input.charAt(i++));

            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;

            output = output + String.fromCharCode(chr1);

            if (enc3 != 64) {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 != 64) {
                output = output + String.fromCharCode(chr3);
            }

        }

        output = Base64._utf8_decode(output);

        return output;

    },
    _utf8_encode: function (string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";

        for (var n = 0; n < string.length; n++) {

            var c = string.charCodeAt(n);

            if (c < 128) {
                utftext += String.fromCharCode(c);
            }
            else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            }
            else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }

        return utftext;
    },
    _utf8_decode: function (utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;

        while (i < utftext.length) {

            c = utftext.charCodeAt(i);

            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            }
            else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            }
            else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }

        }

        return string;
    }
};


