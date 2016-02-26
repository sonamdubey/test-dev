﻿var pageNo = 1, cacheKey = catId.replace(",", "_");
var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"];

lscache.setBucket('catVideos');

ko.bindingHandlers.CurrencyText = {
    update: function (element, valueAccessor) {
        var amount = valueAccessor();
        var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount) : 0;
        $(element).text(formattedAmount);
    }
};

(function ($, ko) {
    'use strict';
    function KoLazyLoad() {
        var self = this;

        var updatebit = ko.observable(true).extend({ throttle: 50 });

        var handlers = {
            img: updateImage
        };

        function flagForLoadCheck() {
            updatebit(!updatebit());
        }

        $(window).on('scroll', flagForLoadCheck);
        $(window).on('resize', flagForLoadCheck);
        $(window).on('load', flagForLoadCheck);

        function isInViewport(element) {
            var rect = element.getBoundingClientRect();
            return rect.bottom > 0 && rect.right > 0 &&
              rect.top < (window.innerHeight || document.documentElement.clientHeight) &&
              rect.left < (window.innerWidth || document.documentElement.clientWidth);
        }

        function updateImage(element, valueAccessor, allBindings, viewModel, bindingContext) {
            var value = ko.unwrap(valueAccessor());
            if (isInViewport(element)) {
                element.src = value;
                $(element).data('kolazy', true);
            }
        }

        function init(element, valueAccessor, allBindings, viewModel, bindingContext) {
            var initArgs = arguments;
            updatebit.subscribe(function () {
                update.apply(self, initArgs);
            });
        }

        function update(element, valueAccessor, allBindings, viewModel, bindingContext) {
            var $element = $(element);

            if ($element.is(':hidden') || $element.css('visibility') == 'hidden' || $element.data('kolazy')) {
                return;
            }

            var handlerName = element.tagName.toLowerCase();
            if (handlers.hasOwnProperty(handlerName)) {
                return handlers[handlerName].apply(this, arguments);
            } else {
                throw new Error('No lazy handler defined for "' + handlerName + '"');
            }
        }

        return {
            handlers: handlers,
            init: init,
            update: update
        }
    }

    ko.bindingHandlers.lazyload = new KoLazyLoad();

})(jQuery, ko);
$(window).scroll(function () {
    var winScroll = $(window).scrollTop(),
        pageHeight = $(document).height(),
        windowHeight = $(window).height(),
        footerHeight = $("#bg-footer").height();
    var position = pageHeight - (windowHeight + 286 + 200);
    if (winScroll >= position && pageNo < maxPage && isNextPage) {
        isNextPage = false;
        pageNo = $.getPageNo() + 1;
        $.getVideos();
    }
});
$.getVideos = function () {
    $('#loading').show();
    var cacheVideos = lscache.get("catVideo_" + cacheKey + "_" + pageNo);
    if (cacheVideos) {
        $.bindVideos(cacheVideos);
        window.location.hash = "pn=" + pageNo;
        isNextPage = true;
        $('#loading').hide();
    }
    else {
        var catURL = cwHostUrl + "/api/v1/videos/subcategory/" + catId + "/?appId=2&pageNo=" + pageNo + "&pageSize=6";
        $.ajax({
            type: 'GET',
            url: catURL,
            dataType: 'json',
            success: function (response) {
                if (response.TotalRecords > 0) {
                    $.bindVideos(response);
                    isNextPage = true;
                    lscache.set("catVideo_" + cacheKey + "_" + pageNo, response, 60);
                    window.location.hash = "pageno=" + pageNo;
                }
            },
            complete: function (xhr) {
                if (xhr.status == 404 || xhr.status == 204) {
                    lscache.set("catVideo_" + cacheKey + "_" + pageNo, null, 60);
                }
                $('#loading').hide();
            }
        });
    }
};
$.bindVideos = function (reponseVideos) {
    var koHtml = '<div class="miscWrapper container bottom-shadow margin-bottom30">'
                        + '<ul id="listVideos' + pageNo + '"  data-bind="template: { name: \'templetVideos\', foreach: Videos }">'
                        + '</ul>'
                    + '<div class="clear"></div></div>';
    $('#listVideos' + (pageNo - 1)).parent().after(koHtml);
    ko.applyBindings(new VideoViewModel(reponseVideos), $("#listVideos" + pageNo)[0]);
};
var VideoViewModel = function (model) {
    ko.mapping.fromJS(model, {}, this);
};
$.getPageNo = function () {
    var params = window.location.hash.replace('#', '');
    return params.length > 0 ? parseInt(params.split('=')[1]) : 1;
};
lscache.flushExpired();