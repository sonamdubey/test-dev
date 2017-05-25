// Initialize collapse button for side nav
$(".buttons").sideNav();
$('.modal').modal();
$('.collapsible').collapsible();
// Do not remove this code. Its written to get values from checkbox to controller
$("input:checkbox").change(function () { $(this).val($(this).is(':checked')); });
var select = $('select');
select.each(function () {
    if (!$(this).hasClass('chosen-select')) {
        $(this).material_select();
    }
});
$(document).ready(function () {
    pageFooter.setPosition();
});

var pageFooter = {
    bodyElement: $('body'),

    container: $('#page-footer'),

    setPosition: function () {
        var flag = pageFooter.bodyElement.height() - window.innerHeight > pageFooter.container.height();

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
            else if (options.source == null || options.source == undefined || options.source == '') {
                console.log("please define source");
                return;
            }

            var requestTerm;

            $(this).easyAutocomplete({
                url: function (value) {
                    if (options.beforefetch && typeof (options.beforefetch) == "function") {
                        options.beforefetch();
                    }

                    requestTerm = value.replace(/^\s\s*/, '').replace(/\s\s*$/, '').replace(/-/g, ' ').replace(/[^A-Za-z0-9 ]/g, '').toLowerCase();

                    if (requestTerm.length > 0) {
                        var path = "http://localhost:9011/api/AutoSuggest/?source=" + options.source + "&inputText=" + requestTerm + "&noofrecords=8";

                        return path;
                    }
                },

                getValue: "text",

                sourceType: options.source,

                template: {
                    type: "custom",
                    method: function (value, item) {
                        var listElement = '<div class="list-item" data-cityId="' + item.payload.cityId + '">' + value + '</div>';

                        return listElement;
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