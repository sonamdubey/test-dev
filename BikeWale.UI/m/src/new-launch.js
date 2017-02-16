// read more - collapse
$('.read-more-target').on('click', function () {
    var element = $(this),
        parentElemtent = element.closest('.collapsible-content');

    if (!parentElemtent.hasClass('active')) {
        parentElemtent.addClass('active');
        element.text(' Collapse');
    }
    else {
        parentElemtent.removeClass('active');
        element.text('...Read more');
    }
});

// more brand - collapse
$('.view-brandType').click(function () {
    var element = $(this),
        elementParent = element.closest('.collapsible-brand-content'),
        moreBrandContainer = elementParent.find('.brandTypeMore');

    if (!moreBrandContainer.is(':visible')) {
        moreBrandContainer.slideDown();
        element.attr('href', 'javascript:void(0)');
        element.text('View less brands');
    }
    else {
        element.attr('href', '#brand-type-container');
        moreBrandContainer.slideUp();
        element.text('View more brands');
    }
});

var directionRight = { direction: 'right' };

// brand
$('#brand-filter-selection').on('click', function () {
    slideInDrawer.open($('#brand-slideIn-drawer'));
    appendState('brand-drawer');
});

$('#close-brand-slideIn-drawer').on('click', function () {
    slideInDrawer.close($('#brand-slideIn-drawer'));
    history.back();
});

// year
$('#year-filter-selection').on('click', function () {
    slideInDrawer.open($('#year-slideIn-drawer'));
    appendState('year-drawer');
});

$('#close-year-slideIn-drawer').on('click', function () {
    slideInDrawer.close($('#year-slideIn-drawer'));
    history.back();
});

var slideInDrawer = {
    open: function (container) {

        if (container) {
            container.show(effect, directionRight, duration, function () {
                container.addClass('fix-header-input');
            });
            windowScreen.lock();
        }
    },

    close: function (container) {
        if (container) {
            container.hide(effect, directionRight, duration, function () { });
            container.removeClass('fix-header-input');
            windowScreen.unlock();
        }
    }
};

var windowScreen = {
    htmlElement: $('html'),

    bodyElement: $('body'),

    lock: function () {
        if ($(document).height() > $(window).height()) {
            var windowScrollTop = windowScreen.htmlElement.scrollTop() ? windowScreen.htmlElement.scrollTop() : windowScreen.bodyElement.scrollTop();
            if (windowScrollTop < 0) {
                windowScrollTop = 0;
            }
            windowScreen.htmlElement.addClass('lock-browser-scroll').css('top', -windowScrollTop);
        }
    },

    unlock: function () {
        var windowScrollTop = parseInt(windowScreen.htmlElement.css('top'));

        windowScreen.htmlElement.removeClass('lock-browser-scroll');
        $('html, body').scrollTop(-windowScrollTop);
    }
};

/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');
};

$(window).on('popstate', function (event) {
    if ($('#brand-slideIn-drawer').is(':visible')) {
        slideInDrawer.close($('#brand-slideIn-drawer'));
    }
    if ($('#year-slideIn-drawer').is(':visible')) {
        slideInDrawer.close($('#year-slideIn-drawer'));
    }
});

$("#brand-slideIn-drawer ul li,#year-slideIn-drawer ul li,#pagination-list-content ul li").click(function (e) {
    if (vmNewLaunches && !vmNewLaunches.IsInitialized()) {
      
        vmNewLaunches.init(e);
    }
});

ko.bindingHandlers.CurrencyText = {
    update: function (element, valueAccessor) {
        var amount = valueAccessor();
        var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount) : 0;

        if (amount > 0) $(element).text(formattedAmount);
        else $(element).text("N/A");
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
            console.log(1);
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

var vmPagination = function (curPgNum, pgSize, totalRecords) {
    var self = this;
    self.totalData = ko.observable(totalRecords);
    self.pageNumber = ko.observable(curPgNum);
    self.pageSize = ko.observable(pgSize);
    self.pageSlot = ko.observable(5);
    self.totalPages = ko.computed(function () {
        var div = Math.ceil(self.totalData() / self.pageSize());
        return div;
    });
    self.paginated = ko.computed(function () {
        var pgSlot;

        if (self.pageNumber() < 4) {
            pgSlot = self.pageSlot();
        } else {
            pgSlot = self.pageNumber() + self.pageSlot() - 3;
        }

        if (self.totalPages() > pgSlot) {
            return pgSlot;
        } else {
            return self.totalPages();
        }

    });
    self.hasPrevious = ko.computed(function () {
        return self.pageNumber() != 1;
    });
    self.hasNext = ko.computed(function () {
        return self.pageNumber() != self.totalPages();
    });
    self.next = function () {
        if (self.pageNumber() < self.totalPages())
            return self.pageNumber() + 1;
        return self.pageNumber();
    }
    self.previous = function () {
        if (self.pageNumber() > 1) {
            return self.pageNumber() - 1;
        }
        return self.pageNumber();
    }
};


var newLaunches = function () {
    var self = this;
    self.IsInitialized = ko.observable(false);
    self.Filters = ko.observable({ pageNo: 1, pageSize: 10 });
    self.PreviousQS = ko.observable("");
    self.IsReset = ko.observable(false);
    self.QueryString = ko.computed(function () {
        var qs = "";
        $.each(self.Filters(), function (i, val) {
            if (val != null && val != "")
                qs += "&" + i + "=" + val;
        });
        qs = qs.substr(1);
        return qs;
    });

    self.models = ko.observable([]);
    self.Pagination = ko.observable(new vmPagination());
    self.TotalBikes = ko.observable();
    self.PageUrl = ko.observable();
    self.CurPageNo = ko.observable();
    self.selectedMake = ko.observable("All brands");
    self.selectedYear = ko.observable("All years");
    self.noBikes = ko.observable(self.TotalBikes() == 0);
    self.IsLoading = ko.observable(false);
    self.PagesListHtml = ko.observable("");
    self.PrevPageHtml = ko.observable("");
    self.NextPageHtml = ko.observable("");

    self.init = function (e) {
        if (!self.IsInitialized()) {

            var eleSection = $("#newlaunched-bikes");
            ko.applyBindings(self, $("#newlaunched-bikes")[0]);

            self.Filters()["make"] = eleSection.data("make-filter") || "";
            self.Filters()["year"] = eleSection.data("year-filter") || "";

            var filterType = $(event.target).closest("ul").data("filter");
            if (filterType) {
                if (filterType == 'makewise') {
                    self.setMakeFilter(e);
                }
                else if (filterType == "yearwise") {
                    self.setYearFilter(e);
                }
            }
            else {
                self.ChangePageNumber(e);
            }

            $("#brand-slideIn-drawer ul li,#year-slideIn-drawer ul li,#pagination-list-content ul li").off("click", self.init);

            $(document).on("click", "#pagination-list-content ul li", function (e) {
                if (self.IsInitialized()) {
                    self.ChangePageNumber(e);
                }
            });

            self.IsInitialized(true);
        }
    };

    self.ApplyPagination = function () {
        try {
            var pag = new vmPagination(self.CurPageNo(), self.Filters().pageSize, self.TotalBikes());
            self.Pagination(pag);
            if (self.Pagination()) {
                var n = self.Pagination().paginated(), pages = '', prevpg = '', nextpg = '';
                var qs = window.location.pathname + window.location.hash;
                var rstr = qs.match(/page-[0-9]+/i);
                var startIndex = (self.Pagination().pageNumber() - 2 > 0) ? (self.Pagination().pageNumber() - 2) : 1;
                for (var i = startIndex ; i <= n; i++) {
                    var pageUrl = qs.replace(rstr, "page-" + i);
                    pages += ' <li class="page-url ' + (i == self.CurPageNo() ? 'active' : '') + ' "><a  data-bind="click : function(d,e) { $root.ChangePageNumber(e); } " data-pagenum="' + i + '" href="' + pageUrl + '">' + i + '</a></li>';
                }
                self.PagesListHtml(pages);

                if (self.Pagination().hasPrevious()) {
                    prevpg = "<a  data-bind='click : $root.ChangePageNumber' data-pagenum='" + self.Pagination().previous() + "' href='" + qs.replace(rstr, "page-" + self.Pagination().previous()) + "' class='bwmsprite prev-page-icon'/>";
                } else {
                    prevpg = "<a href='javascript:void(0)' class='bwmsprite prev-page-icon'/>";
                }
                self.PrevPageHtml(prevpg);
                if (self.Pagination().hasNext()) {
                    nextpg = "<a  data-bind='click : $root.ChangePageNumber' data-pagenum='" + self.Pagination().next() + "' href='" + qs.replace(rstr, "page-" + self.Pagination().next()) + "' class='bwmsprite next-page-icon'/>";
                } else {
                    nextpg = "<a href='javascript:void(0)' class='bwmsprite next-page-icon'/>";
                }
                self.NextPageHtml(nextpg);
                $("#pagination-list li[data-pagenum=" + self.Pagination().pageNumber() + "]").addClass("active");

            }
        } catch (e) {
            console.warn("Unable to apply pagination.");
        }

    };

    self.ChangePageNumber = function (e) {
        try {
            var pnum = parseInt($(e.target).attr("data-pagenum"), 10);
            var selHash = $(e.target).attr("data-hash");
            self.Filters()["pageNo"] = pnum;

            if (selHash) {
                var arr = selHash.split('&');
                var curcityId = arr[0].split("=")[1], curmakeId = arr[1].split("=")[1], curmodelId = arr[2].split("=")[1];
                if (curcityId && curcityId != "0") self.Filters()["city"] = curcityId;
                if (curmakeId && curmakeId != "0") self.Filters()["make"] = curmakeId;
            }
            self.CurPageNo(pnum);
            self.getNewLaunchedBikes();
            e.preventDefault();
            $('html, body').scrollTop(0);
        } catch (e) {
            console.warn("Unable to change page number : " + e.message);
        }
        return false;
    };

    self.setMakeFilter = function (e) {
        var ele = $(e.currentTarget);
        var make = {
            id: parseInt(ele.data("makeid")),
            name: ele.data("makename")
        };
        self.selectedMake(make.name || "All brands");
        self.Filters()["make"] = make.id || "";
        self.Filters()["pageNo"] = 1;
        self.CurPageNo(1);
        self.getNewLaunchedBikes($('#make-slideIn-drawer'));
    };

    self.setYearFilter = function (e) {

        var ele = $(e.currentTarget), year = ele.data("bikeyear");
        self.selectedYear(year || "All years");
        self.Filters()["yearLaunch"] = year || "";
        self.Filters()["pageNo"] = 1;
        self.CurPageNo(1);
        self.getNewLaunchedBikes($('#year-slideIn-drawer'));

    };

    self.getNewLaunchedBikes = function (ele) {

        self.Filters.notifySubscribers();

        var qs = self.QueryString();

        if (self.PreviousQS() != qs) {
            self.IsLoading(true);
            self.PreviousQS(qs);
            var apiUrl = "/api/v2/newlaunched/?" + qs;
            $.getJSON(apiUrl)
            .done(function (response) {
                self.models(response.bikes);
                self.TotalBikes(response.totalCount);
                self.noBikes(false);
            })
            .fail(function () {
                self.noBikes(true);
            })
            .always(function () {
                self.ApplyPagination();
                slideInDrawer.close(ele);
                window.location.hash = qs;
                self.IsLoading(false);
            });
        }
        else {
            slideInDrawer.close(ele);
            history.back();
        }
    };
};

var vmNewLaunches = new newLaunches();

