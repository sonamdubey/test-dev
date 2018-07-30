
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

var objMakesList = $("#brand-slideIn-drawer ul li").slice(1), makeInputEle = $("#brand-slideIn-drawer input[type='text']");

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


ko.bindingHandlers.CurrencyText = {
    update: function (element, valueAccessor) {
        var amount = valueAccessor();
        var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount) : 0;

        if (amount > 0) $(element).text(formattedAmount);
        else $(element).text("N/A");
    }
};

$(window).on('scroll', applyLazyLoad);
$(window).on('resize', applyLazyLoad);
$(window).on('load', applyLazyLoad);

function applyLazyLoad() {
    $("img.lazy").lazyload({
        event: "imgLazyLoad"
    });
}

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
    self.selectedCityId = ko.observable();
    self.selectedCityName = ko.observable();

    self.makeFilter = function () {
        if (objMakesList) {
            var term = $.trim(makeInputEle.val()).toLowerCase();
            objMakesList.each(function () {
                var li = $(this);
                var found = li.data("makename-lower").indexOf(term) > -1;
                if (found) {
                    if (!li.is(":visible"))
                        li.show();
                } else if (li.is(":visible")) {
                    li.hide();
                }

            });
        }
    };

    self.init = function (e) {
        if (!self.IsInitialized()) {

            var eleSection = $("#newlaunched-bikes");
            ko.applyBindings(self, eleSection[0]);

            self.CheckCookies();

            self.Filters()["make"] = self.Filters()["make"] || eleSection.data("make-filter") || "";
            self.Filters()["yearLaunch"] = self.Filters()["yearLaunch"] || eleSection.data("year-filter") || "";
            self.Filters()["city"] = self.Filters()["city"] || eleSection.data("city") || self.selectedCityId() || "";

            var filterType = $(e.target).closest("ul").data("filter");
            if (filterType) {

                if (filterType == 'makewise') {
                    self.setMakeFilter(e);
                }
                else if (filterType == "yearwise") {
                    self.setYearFilter(e);
                }
            }
            else if (e.target) {
                self.ChangePageNumber(e);
            }
            else {
                self.getNewLaunchedBikes();
            }

            $("#brand-slideIn-drawer ul li,#year-slideIn-drawer ul li,#pagination-list-content ul li").off("click", self.init);

            $(document).on("click", "#pagination-list-content ul li,.pagination-control-prev a,.pagination-control-next a", function (e) {
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
                $(".pagination-control-prev,.pagination-control-next").removeClass("active inactive");
                if (self.Pagination().hasPrevious()) {
                    prevpg = "<a  data-bind='click : $root.ChangePageNumber' data-pagenum='" + self.Pagination().previous() + "' href='" + qs.replace(rstr, "page-" + self.Pagination().previous()) + "' class='bwmsprite prev-page-icon'/>";
                } else {
                    prevpg = "<a href='javascript:void(0)' class='bwmsprite prev-page-icon'></a>";
                    $(".pagination-control-prev").addClass("inactive");
                }
                self.PrevPageHtml(prevpg);
                if (self.Pagination().hasNext()) {
                    nextpg = "<a  data-bind='click : $root.ChangePageNumber' data-pagenum='" + self.Pagination().next() + "' href='" + qs.replace(rstr, "page-" + self.Pagination().next()) + "' class='bwmsprite next-page-icon'/>";
                } else {
                    nextpg = "<a href='javascript:void(0)' class='bwmsprite next-page-icon'></a>";
                    $(".pagination-control-next").addClass("inactive");
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
            var ele = $(e.target),pnum = parseInt(ele.attr("data-pagenum"), 10);
            if (pnum && !isNaN(pnum) && !ele.parent().hasClass("active"))
            {
                var selHash = ele.attr("data-hash");
                self.Filters()["pageNo"] = pnum;

                if (selHash) {
                    var arr = selHash.split('&');
                    var curcityId = arr[0].split("=")[1], curmakeId = arr[1].split("=")[1], curmodelId = arr[2].split("=")[1];
                    if (curcityId && curcityId != "0") self.Filters()["city"] = curcityId;
                    if (curmakeId && curmakeId != "0") self.Filters()["make"] = curmakeId;
                }
                self.CurPageNo(pnum);
                self.getNewLaunchedBikes();
            }
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
        triggerGA("Newly_Launched", "Clicked_on_Brand_Filter", self.selectedMake());
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
            $('.new-launches-list .list-item img').attr('src', '');
            self.IsLoading(true);
            self.models();
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
                $('html, body').scrollTop($('#newlaunched-bikes').offset().top);
            });
        }
        else {
            slideInDrawer.close(ele);
            history.back();
        }
    };

    self.setPageFilters = function (e) {       
        var currentQs = window.location.hash.substr(1);
        if (currentQs != "") {
            var _filters = currentQs.split("&"), objFilter = {};
            for (var i = 0; i < _filters.length; i++) {
                var f = _filters[i].split("=");
                self.Filters()[f[0]] = f[1];
            }
            self.CurPageNo((self.Filters()["pageNo"] ? parseInt(self.Filters()["pageNo"]) : 0));

            if (self.Filters()["make"] && self.Filters()["make"] != "") {
                var selOption = $("#brand-slideIn-drawer ul li[data-makeid='" + self.Filters()["make"] + "']");
                self.selectedMake(selOption.data("makename"));
            }

            if (self.Filters()["yearLaunch"] && self.Filters()["yearLaunch"] != "") {
                var selOption = $("#year-slideIn-drawer ul li[data-bikeyear='" + self.Filters()["yearLaunch"] + "']");
                self.selectedYear(selOption.data("bikeyear"));
            }

            self.init(e);
        }

    };

    self.CheckCookies = function () {
        try {
            c = document.cookie.split('; ');
            for (i = c.length - 1; i >= 0; i--) {
                C = c[i].split('=');
                if (C[0] == "location") {
                    var cData = (String(C[1])).split('_');
                    self.selectedCityId(parseInt(cData[0]) || 0);
                    self.selectedCityName(cData[1] || "");
                    break;
                }
            }
        } catch (e) {
            console.warn(e);
        }
    };
};

var vmNewLaunches = new newLaunches();

$(function () {
    
    vmNewLaunches.setPageFilters(e);

    $("#brand-slideIn-drawer ul li,#year-slideIn-drawer ul li,#pagination-list-content ul li").click(function (e) {
        if (vmNewLaunches && !vmNewLaunches.IsInitialized()) {
            if (!$(e.target).parent().hasClass("active"))
                vmNewLaunches.init(e);
            $('html, body').scrollTop(0);
            return false;
        }
    });

    makeInputEle.keyup(function () {
        vmNewLaunches.makeFilter();
    });

});

