var vmPagination, newLaunches, vmNewLaunches;
docReady(function () {
    var chosenSelectBox = $('.chosen-select');
    var selectDropdownBox = $('.select-box-no-input');   
    chosenSelectBox.chosen();
    chosenSelectBox.each(function () {
        var text = $(this).attr('data-placeholder');
        $(this).siblings('.chosen-container').find('input[type=text]').attr('placeholder', text);
    });

    $(".chosen-select").chosen().change(function () {
        if ($(this).val() > 0) {
            $(this).siblings('.select-label').hide();
        }
    });

    newLaunches = function () {        
        var self = this;
        self.IsInitialized = ko.observable(false);
        self.Filters = ko.observable({ pageNo: 1, pageSize: 15 });
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

        self.init = function (e) {
            if (!self.IsInitialized()) {
                self.IsLoading(true);
                self.TotalBikes(1); //handle container for loader
                var eleSection = $("#newlaunched-bikes");
                self.CheckCookies();
                self.Filters()["make"] = self.Filters()["make"] || eleSection.data("make-filter") || "";
                self.Filters()["yearLaunch"] = self.Filters()["yearLaunch"] || eleSection.data("year-filter") || "";
                self.Filters()["city"] = self.Filters()["city"] || eleSection.data("city") || self.selectedCityId() || "";

                ko.applyBindings(self, eleSection[0]);

                var filterType = $(e.target).data("filter");
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
                    $(".pagination-control-prev,.pagination-control-next").removeClass("active").removeClass("inactive");
                    if (self.Pagination().hasPrevious()) {
                        prevpg = "<a  data-bind='click : $root.ChangePageNumber' data-pagenum='" + self.Pagination().previous() + "' href='" + qs.replace(rstr, "page-" + self.Pagination().previous()) + "' class='bwmsprite bwsprite prev-page-icon'/>";
                    } else {
                        prevpg = "<a href='javascript:void(0)' class='bwmsprite bwsprite prev-page-icon'></a>";
                        $(".pagination-control-prev").addClass("inactive");
                    }
                    self.PrevPageHtml(prevpg);
                    if (self.Pagination().hasNext()) {
                        nextpg = "<a  data-bind='click : $root.ChangePageNumber' data-pagenum='" + self.Pagination().next() + "' href='" + qs.replace(rstr, "page-" + self.Pagination().next()) + "' class='bwmsprite bwsprite next-page-icon'/>";
                    } else {
                        nextpg = "<a href='javascript:void(0)' class='bwmsprite bwsprite next-page-icon'></a>";
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
                var ele = $(e.target), pnum = parseInt(ele.attr("data-pagenum"), 10);
                if (pnum && !isNaN(pnum) && !ele.parent().hasClass("active")) {
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
            var ele = $(e.target).find("option:selected");
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
            var ele = $(e.target).find("option:selected"), year = ele.data("bikeyear");
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
                $('.model-jcarousel-image-preview img').attr('src', '');
                self.models([]);
                if (self.noBikes()) {
                    self.TotalBikes(1); // to show bikes container
                    self.noBikes(false);
                }
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
                    self.TotalBikes(0);
                })
                .always(function () {
                    self.ApplyPagination();
                    window.location.hash = qs;
                    self.IsLoading(false);
                });
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

                if (self.Filters()["make"] != "") {
                    var selOption = $("#makeFilter option[data-makeid='" + self.Filters()["make"] + "']");
                    self.selectedMake(selOption.text());
                    selOption.prop('selected', true);
                    selOption.parent().chosen().trigger("chosen:updated");
                }
                if (self.Filters()["yearLaunch"] != "") {
                    var selOption = $("#yearFilter option[data-bikeyear='" + self.Filters()["yearLaunch"] + "']");
                    self.selectedYear(selOption.text());
                    selOption.prop('selected', true);
                    selOption.parent().chosen().trigger("chosen:updated");
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
    vmNewLaunches = new newLaunches();

    $("#pagination-list-content ul li a").click(function (e) {
        if (vmNewLaunches && !vmNewLaunches.IsInitialized()) {
            if (!$(e.target).parent().hasClass("active"))
                vmNewLaunches.init(e);
            $('html, body').scrollTop(0);
            return false;
        }
    });

    $("#makeFilter,#yearFilter").change(function (e) {
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

    ko.bindingHandlers.chosen = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var $element = $(element);
            var options = ko.unwrap(valueAccessor());
            if (typeof options === 'object')
                $element.chosen(options);

            ['options', 'selectedOptions', 'value'].forEach(function (propName) {
                if (allBindings.has(propName)) {
                    var prop = allBindings.get(propName);
                    if (ko.isObservable(prop)) {
                        prop.subscribe(function () {
                            $element.trigger('chosen:updated');
                        });
                    }
                }
            });
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

    selectDropdownBox.each(function () {
        var text = $(this).find('.chosen-select').attr('data-title'),
            searchBox = $(this).find('.chosen-search')

        searchBox.empty().append('<p class="no-input-label">' + text + '</p>');
    });

    $('#makeFilter').trigger("chosen:updated");
    $('#yearFilter').trigger("chosen:updated");
    vmNewLaunches.setPageFilters(e);
});



vmPagination = function (curPgNum, pgSize, totalRecords) {
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

        if (self.pageNumber() < (self.pageSlot()-1)) {
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