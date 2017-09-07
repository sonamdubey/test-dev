﻿// Initialize collapse button for side nav
$(".buttons").sideNav();
$('.modal').modal();
$('.collapsible').collapsible();
$('.tooltipped').tooltip({ delay: 50 });
// Do not remove this code. Its written to get values from checkbox to controller
$("input:checkbox").change(function () { $(this).val($(this).is(':checked')); });
var select = $('select');

$(document).ready(function () {
    pageFooter.setPosition();
});

var pageFooter = {
    bodyElement: $('body'),

    container: $('#page-footer'),

    setPosition: function () {
        var flag = pageFooter.bodyElement.height() + 50 - window.innerHeight > pageFooter.container.height();

        if (pageFooter.bodyElement.height() > window.innerHeight && flag) {
            pageFooter.container.addClass('footer-relative');
        }
        else {
            pageFooter.container.addClass('footer-fixed');
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


