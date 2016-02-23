var pageNo = 1, maxPage = 2; //ASK Default catId is 1.

var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"];

lscache.setBucket('catVideos');

ko.bindingHandlers.formateDate = {
    update: function (element, valueAccessor) {
        var date = new Date(valueAccessor());
        var formattedDate = monthNames[date.getMonth()] + ' ' + date.getDay() + ', ' + date.getFullYear();
        $(element).text(formattedDate);
    }
};

(function ($, ko) {
    'use strict';
    // TODO: Hook into image load event before loading others...
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
    console.log(winScroll + " " + position + " " + pageHeight + " " + windowHeight + " "+ footerHeight);
    if (winScroll >= position && pageNo < maxPage && isNextPage) {
        isNextPage = false;
        //pageNo++;
        pageNo = $.getPageNo() + 1;
        console.log(pageNo);
        $.getVideos();
    }
});

$.getVideos = function () {
    $('#loading').show();
    var cacheVideos = lscache.get("catVideo_" + catId + "_" + pageNo);
   // console.log("catVideo_" + catId + "_" + pageNo);
    if (cacheVideos) {
       // console.log("catVideo_" + catId + "_" + pageNo);
        $.bindVideos(cacheVideos);
        maxPage = Math.ceil(cacheVideos.TotalRecords / 9);
        window.location.hash = "pn=" + pageNo;
        isNextPage = true;//HOw ?
    }
    else
    {//"47,48,49,50,51,52,53,55,57,58,59,60"
        var catURL = "http://172.16.1.254:9020/api/v1/videos/subcategory/" + catId + "/?appId=2&pageNo=" + pageNo + "&pageSize=9";
        $.ajax({
            type: 'GET',
            url: catURL,
            dataType: 'json',
            success: function (response) {
                if (response.TotalRecords > 0) {
                    $.bindVideos(response);
                    //console.log(response);
                    maxPage = Math.ceil(response.TotalRecords / 9);//should be from back End
                    isNextPage = true;//HOw ?
                    lscache.set("catVideo_" + catId + "_" + pageNo, response, 60);//setcache
                   // console.log("1");
                    window.location.hash = "pageno=" + pageNo;
                   // console.log("2");
                }
            },
            complete: function (xhr) {
                if (xhr.status == 404 || xhr.status == 204) {
                    lscache.set(key + selCityId, null, 60);
                }
            },
            error: function (error) {
                //errorNoBikes();
            }
        });
    }
    if (pageNo == maxPage) $('#loading').hide();
}; 
$.bindVideos = function (reponseVideos) {
    var koHtml = '<div class="miscWrapper container">'
                        + '<ul id="listVideos' + pageNo + '"  data-bind="template: { name: \'templetVideos\', foreach: Videos }">'
                        + '</ul>'
                    + '<div class="clear"></div></div>';
    $('#listVideos' + (pageNo - 1)).parent().after(koHtml);
    //console.log("listVideos" + pageNo);
    ko.applyBindings(new SearchViewModel(reponseVideos), document.getElementById("listVideos" + pageNo));
};
var SearchViewModel = function (model) {
    ko.mapping.fromJS(model, {}, this);
};

$.getPageNo = function () {
    var params = window.location.hash.replace('#', '');
    return params.length > 0 ? parseInt(params.split('=')[1]) : 1;
};