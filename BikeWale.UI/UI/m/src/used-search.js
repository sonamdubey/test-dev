var budgetValue = [0, 10000, 20000, 35000, 50000, 80000, 125000, 200000];
var bikesList = $("#filter-bike-list");
var citiesList = $("#filter-city-list li");
var isCityFilterFilled = false;

var getQueryString = function () {
    var qsColl = new Object();
    try {
        var requestUrl = window.location.hash.substr(1);
        if (requestUrl && requestUrl != '') {
            var kvPairs = requestUrl.split('&');
            $.each(kvPairs, function (i, val) {
                var kvPair = val.split('=');
                qsColl[kvPair[0]] = kvPair[1];
            });
        }
    } catch (e) {
        console.warn("Unable to get query string");
    }
    return qsColl;
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

ko.bindingHandlers.CurrencyText = {
    update: function (element, valueAccessor) {
        var amount = valueAccessor();
        var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount) : 0;
        $(element).text(formattedAmount);
    }
};

ko.bindingHandlers.NumberOrdinal = {
    update: function (element, valueAccessor) {
        var num = valueAccessor();
        var num = ko.unwrap(num) != null ? num : "";
        num = parseInt(num, 10);
        switch (num % 100) {
            case 11:
            case 12:
            case 13:
                suf = "th"; break;
        }

        switch (num % 10) {
            case 1:
                suf = "st"; break;
            case 2:
                suf = "nd"; break;
            case 3:
                suf = "rd"; break;
            default:
                suf = "th"; break;
        }

        $(element).text(num + suf);
    }
};

ko.bindingHandlers.KOSlider = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var options = allBindingsAccessor().sliderOptions || {};
        var observable = valueAccessor();

        options.slide = function (e, ui) {
            if (ui.values && ui.values.length > 0) {
                if (ui.values[0] != ui.values[1])
                    observable(ui.values);
            }
            else observable(ui.value);
        };

        ko.utils.registerEventHandler(element, "slide", function (event, ui) {
            if (ui.values && ui.values.length > 0 && ui.values[0] == ui.values[1]) {
                return false;
            }
        });

        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).slider("destroy");
        });

        $(element).slider(options);
    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        if (value) {
            $(element).slider(value.length ? "values" : "value", value);
            $(element).change();
        }

    }
};

var vmCities = function () {
    var self = this;
    self.SelectedCity = ko.observable({ "id": 0, "name": "All India" });

    self.cityFilter = ko.observable();

    self.visibleCities = ko.computed(function () {
        filter = self.cityFilter();
        filterObj = citiesList;
        if (filter && filter.length > 0) {
            var pat = new RegExp(filter, "i");
            citiesList.filter(function (place) {
                if (pat.test($(this).text())) $(this).show(); else $(this).hide();
            });

        }
        citiesList.first().show();
    });
}


var usedBikes = function () {
    var self = this;
    self.Filters = ko.observable(getQueryString());
    self.PreviousQS = ko.observable("");
    self.IsReset = ko.observable(false);
    self.QueryString = ko.computed(function () {
        var qs = "";
        $.each(self.Filters(), function (i, val) {
            if (val != null && val != "")
                qs += "&" + i + "=" + val;
        });
        qs = qs.substr(1);
        window.location.hash = qs;
        return qs;
    });

    self.Cities = ko.observable(new vmCities());

    self.OnInit = ko.observable(true);
    self.noBikes = ko.observable(OnInitTotalBikes<=0);
    self.TotalBikes = ko.observable(OnInitTotalBikes);
    self.BikeDetails = ko.observableArray();
    self.PageUrl = ko.observable();
    self.CurPageNo = ko.observable();
    self.BikePhotos = function () {
        var self = this;
        self.hostUrl = ko.observable();
        self.OriginalImgPath = ko.observable();
        self.imgPath = ko.observable();
    };
    self.PrevPageUrl = ko.observable();
    self.NextPageUrl = ko.observable();
    self.RedirectUrl = ko.observable();
    self.Pagination = ko.observable(new vmPagination());

    self.FilterCity = function (d, e) {
        var ele = $(e.target);
        if (!ele.hasClass("active")) {
            ele.addClass("active").siblings().removeClass("active");
            self.SelectedCity({ "id": ele.attr("data-cityid"), "name": ele.text() });
        };

        var cityMaskingName = ele.data("citymasking") + "/", arrLocation = window.location.pathname.split("bikes-in-");
        arrLocation[arrLocation.length - 1] = cityMaskingName;
        self.RedirectUrl(arrLocation.join("bikes-in-"));

    };
    self.ApplyBikeFilter = function () {

        try {
            var selMakes = bikesList.find("div.accordion-tab.tab-checked span.unchecked-box");
            var selModels = bikesList.find("div.accordion-tab:not(.tab-checked)");
            var mkList = "", moList = "";
            selMakes.each(function () {
                var ele = $(this);
                mkList += "+" + ele.attr("data-makeid");
            });
            selModels.each(function () {
                var eleList = $(this).parents("li").find(".bike-model-list  li.active span.unchecked-box");
                eleList.each(function () {
                    var ele = $(this);
                    moList += "+" + ele.attr("data-modelid");
                });
            });
            self.Filters()["make"] = mkList.substr(1);
            self.Filters()["model"] = moList.substr(1);
        } catch (e) {
            console.warn("Unable to set apply bikes filter");
        }
    };
    self.SelectedCity = ko.observable({ "id": 0, "name": "All India" });
    self.BudgetValues = ko.observable();
    self.ShowBudgetRange = ko.computed(function (d, e) {

        if (self.BudgetValues()) {
            var minBuget = self.BudgetValues()[0], maxBuget = self.BudgetValues()[1];
            if (minBuget == 0 && maxBuget == 7) {
                $("#budget-amount").html('<span class="bwmsprite inr-xxsm-icon"></span>0 - <span class="bwmsprite inr-xxsm-icon"></span>' + formatPrice(budgetValue[maxBuget]) + ((maxBuget == 7) ? '+' : ''));
            }
            else {
                $("#budget-amount").html('<span class="bwmsprite inr-xxsm-icon"></span>' + formatPrice(budgetValue[minBuget]) + ' - <span class="bwmsprite inr-xxsm-icon"></span>' + formatPrice(budgetValue[maxBuget]) + ((maxBuget == 7) ? '+' : ''));
            }
        }
    });
    self.KmsDriven = ko.observable();
    self.BikeAge = ko.observable();
    self.FilterOwners = function () {
        var owners = $("#previous-owners-list li.active"), ownerList = "";
        owners.each(function () {
            ownerList += "+" + $(this).attr("data-ownerid");
        });

        self.Filters()["owner"] = ownerList.substr(1);
    };
    self.FilterSellers = function () {
        var sellers = $("#sellerTypes .filter-type-seller.checked"), sellerList = "";
        sellers.each(function () {
            sellerList += "+" + $(this).attr("data-sellerid");
        });

        self.Filters()["st"] = sellerList.substr(1);
    };
    self.ApplyFilters = function () {
        if (isCityFilterFilled) {
            $("#city-model-used-carousel").hide();
        }
        try {
            self.ResetFilters();
            self.ApplyBikeFilter();
            if (self.KmsDriven() != 200000) self.Filters()["kms"] = self.KmsDriven();
            if (self.BikeAge() != 8) self.Filters()["age"] = self.BikeAge();
            if (self.BudgetValues()) {
                var minBuget = self.BudgetValues()[0], maxBuget = self.BudgetValues()[1];
                self.Filters()["budget"] = budgetValue[minBuget];
                if (maxBuget != 7) self.Filters()["budget"] += "+" + budgetValue[maxBuget];
            }
            self.FilterOwners();
            self.FilterSellers();

            if (self.SelectedCity())
            {
                if(self.SelectedCity().id != selectedCityId.toString())
                {
                    self.Filters.notifySubscribers();
                    window.location.hash = self.QueryString();
                    window.location.pathname = self.RedirectUrl();
                }
                else if(self.SelectedCity().id > 0) {
                    self.Filters()["city"] = self.SelectedCity().id;
                }               
            }

            self.GetUsedBikes();
          
        } catch (e) {
            console.warn("Unable to apply current filters : " + e);
        }
    };
    self.ResetFilters = function () {
        var so = self.Filters()["so"];
        self.Filters(new Object());
        self.Filters()["so"] = so;
    };

    self.SetDefaultFilters = function () {
        try {
            self.KmsDriven(200000);
            self.BikeAge(8);
            self.BudgetValues([0, 7]);
            self.CurPageNo(1);
            self.ApplyPagination();
            $("#previous-owners-list li").removeClass("active");
            $("#sellerTypes .filter-type-seller").removeClass("checked");
            $("#sort-by-list li").first().addClass("active").siblings().removeClass("active");
            $('#filter-type-bike').find('.selected-filters').text('All Bikes');

        } catch (e) {
            console.warn("Unable to set default records");
        }
    };

    self.applySort = function (d, e) {
        var so = $("#sort-by-list li.active").attr("data-sortorder");
        self.Filters()["so"] = so;
        self.Filters()["pn"] = "";
        self.GetUsedBikes();
    };

    self.PagesListHtml = ko.observable("");
    self.PrevPageHtml = ko.observable("");
    self.NextPageHtml = ko.observable("");

    self.ApplyPagination = function () {
        try {
            var pag = new vmPagination(self.CurPageNo(), 20, self.TotalBikes());
            self.Pagination(pag);
            if (self.Pagination()) {
                var n = self.Pagination().paginated(), pages = '', prevpg = '', nextpg = '';
                var qs = window.location.pathname + window.location.hash;
                var rstr = qs.match(/page-[0-9]+/i);
                var startIndex = (self.Pagination().pageNumber() - 2 > 0) ? (self.Pagination().pageNumber() - 2) : 1;
                for (var i = startIndex ; i <= n; i++) {
                    var pageUrl = qs.replace(rstr, "page-" + i);
                    pages += ' <li class="page-url ' + (i == self.CurPageNo() ? 'active' : '') + ' "><a  data-bind="click : ChangePageNumber" data-pagenum="' + i + '" href="' + pageUrl + '">' + i + '</a></li>';
                }
                self.PagesListHtml(pages);

                if (self.Pagination().hasPrevious()) {
                    prevpg = "<a  data-bind='click : ChangePageNumber' data-pagenum='" + self.Pagination().previous() + "' href='" + qs.replace(rstr, "page-" + self.Pagination().previous()) + "' class='bwmsprite prev-page-icon'/>";
                } else {
                    prevpg = "<a href='javascript:void(0)' class='bwmsprite prev-page-icon'/>";
                }
                self.PrevPageHtml(prevpg);
                if (self.Pagination().hasNext()) {
                    nextpg = "<a  data-bind='click : ChangePageNumber' data-pagenum='" + self.Pagination().next() + "' href='" + qs.replace(rstr, "page-" + self.Pagination().next()) + "' class='bwmsprite next-page-icon'/>";
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

    self.GetUsedBikes = function (e) {
        try {
            if (self.Filters()["pn"] && e == null) {
                self.Filters()["pn"] = "";
            }

            self.Filters.notifySubscribers();

            var qs = self.QueryString();

            if (self.PreviousQS() != qs) {
                self.PreviousQS(qs);
                $.ajax({
                    type: 'GET',
                    url: '/api/used/search/?bikes=1&' + qs.replace(/[\+]/g, "%2B") + ((selectedCityId > 0 && qs.indexOf("city")==-1) ? "&city=" + selectedCityId : ""),
                    beforeSend: function () { filters.loader.open(); },
                    dataType: 'json',
                    success: function (response) {

                        self.TotalBikes(response.totalCount);
                        self.CurPageNo(response.currentPageNo);
                        self.BikeDetails(ko.toJS(response.result));
                        window.location.hash = qs;
                    },
                    complete: function (xhr) {
                        if (xhr && xhr.status != 200) {
                            self.noBikes(true);
                            self.TotalBikes(0);
                            self.CurPageNo(1);
                        }
                        self.noBikes(self.TotalBikes() <= 0);
                        filters.loader.close();
                        self.OnInit(false);
                        self.IsReset(false);
                        self.ApplyPagination();
                    }
                });
            }
        } catch (e) {
            console.warn("Unable to set fetch used bike records");
        }
    };

    self.ChangePageNumber = function (e) {
        try {
            var pnum = $(e.target).attr("data-pagenum");
            var selHash = $(e.target).attr("data-hash");
            self.Filters()["pn"] = pnum;

            if (selHash) {
                var arr = selHash.split('&');
                var curcityId = arr[0].split("=")[1], curmakeId = arr[1].split("=")[1], curmodelId = arr[2].split("=")[1];
                if (curcityId && curcityId != "0") self.Filters()["city"] = curcityId;
                if (curmakeId && curmakeId != "0") self.Filters()["make"] = curmakeId;
                if (curmodelId && curmodelId != "0") { self.Filters()["make"] = ""; self.Filters()["model"] = curmodelId; }
            }

            self.GetUsedBikes(e);
            e.preventDefault();
            $('html, body').scrollTop(0);
        } catch (e) {
            console.warn("Unable to change page number");
        }
        return false;
    };

    self.SetPageFilters = function () {

        try {

            if (self.Filters()["so"]) {
                $("#sort-by-list li[data-sortorder=" + self.Filters()["so"] + "]").addClass("active").siblings().removeClass("active");
            }

            if (self.Filters()["city"]) {
                var ele = $("#filter-city-list li[data-cityid=" + self.Filters()["city"] + "]");
                ele.addClass("active").siblings().removeClass("active");
                self.SelectedCity({ "id": ele.attr("data-cityid"), "name": ele.text() });
            }
            if (self.Filters()["kms"]) {
                self.KmsDriven(parseInt(self.Filters()["kms"], 10));
            }

            if (self.Filters()["age"]) {
                self.BikeAge(parseInt(self.Filters()["age"], 10));
            }
            if (self.Filters()["pn"]) {
                self.CurPageNo(self.Filters()["pn"]);
            }

            if (self.Filters()["owner"]) {
                var arr = self.Filters()["owner"].split("+");
                $.each(arr, function (i, val) {
                    $("#previous-owners-list li[data-ownerid=" + val + "]").addClass("active");
                });
            }

            if (self.Filters()["st"]) {
                var arr = self.Filters()["st"].split("+");
                $.each(arr, function (i, val) {
                    $("#sellerTypes div[data-sellerid=" + val + "]").addClass("checked");
                });
            }

            if (self.Filters()["budget"]) {
                var arr = self.Filters()["budget"].split("+");

                if (arr.length > 0) {
                    self.BudgetValues([$.inArray(parseInt(arr[0], 10), budgetValue), 7]);
                    if (arr.length > 1) self.BudgetValues([$.inArray(parseInt(arr[0], 10), budgetValue), $.inArray(parseInt(arr[1], 10), budgetValue)]);
                }

            }

            if (self.Filters()["model"]) {
                var arr = self.Filters()["model"].split("+");
                $.each(arr, function (i, val) {
                    var ele = bikesList.find("ul.bike-model-list span[data-modelid=" + val + "]");
                    ele.closest("ul.bike-model-list li").addClass("active");
                    accordion.setCount(ele);
                });
            }

            if (self.Filters()["make"]) {
                var arr = self.Filters()["make"].split("+");
                $.each(arr, function (i, val) {
                    var ele = bikesList.find("span[data-makeid=" + val + "]");
                    ele.closest(".accordion-tab").trigger("click");
                    ele.closest(".accordion-checkbox").trigger("click");
                });
            }
            $('#set-bikes-filter').trigger('click');

            if (event.target.id == "filterStart") return false;

            self.GetUsedBikes();

        } catch (e) {
            console.warn("Unable to set page filters");
        }

    };

}

$(document).on("click", "#pagination-list li a,span.pagination-control-prev a,span.pagination-control-next a", function (d, e) {
    vwUsedBikes.ChangePageNumber(d, e);
})

var vwUsedBikes = new usedBikes();
ko.applyBindings(vwUsedBikes, document.getElementById("usedBikesSection"));

var objFilters = vwUsedBikes.Filters();

$(function () {
    vwUsedBikes.SetDefaultFilters();
    vwUsedBikes.PreviousQS(pageQS);

    if (selectedModelId && selectedModelId != "" && selectedModelId != "0") {
        var ele = bikesList.find("ul.bike-model-list span[data-modelid=" + selectedModelId + "]");
        ele.closest("ul.bike-model-list li").addClass("active");
        var moIds = (vwUsedBikes.Filters()["model"]) ? vwUsedBikes.Filters()["model"].split("+") : null;
        if (moIds != null && moIds.length > 0) {
            if ($.inArray(selectedModelId, moIds) == -1)
                vwUsedBikes.Filters()["model"] += "+" + selectedModelId;
        }
        else vwUsedBikes.Filters()["model"] = selectedModelId;

    }
    else if (selectedMakeId && selectedMakeId != "0") {
        var ele = bikesList.find("span[data-makeid=" + selectedMakeId + "]");
        ele.closest(".accordion-tab").trigger("click");
        ele.closest(".accordion-checkbox").trigger("click");
        var mkIds = (vwUsedBikes.Filters()["make"]) ? vwUsedBikes.Filters()["make"].split("+") : null;
        if (mkIds != null && mkIds.length > 0) {
            if ($.inArray(selectedMakeId, mkIds) == -1)
                vwUsedBikes.Filters()["make"] += "+" + selectedMakeId;
        }
        else vwUsedBikes.Filters()["make"] = selectedMakeId;

    }

    if (selectedCityId) {
        $("#filter-city-list li[data-cityid=" + selectedCityId + "]").trigger('click');
    }


    $('#set-bikes-filter').trigger('click');
    vwUsedBikes.SetPageFilters();



});


function formatPrice(price) {
    if (price != null) {
        price = price.toString();
        var lastThree = price.substring(price.length - 3);
        var otherNumbers = price.substring(0, price.length - 3);
        if (otherNumbers != '')
            lastThree = ',' + lastThree;
        var price = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
    }

    return price;
}

$(document).ready(function () {

    var sortFilter = $('#sort-filter-wrapper'),
        footerHeight = $('footer').height();

    $(window).scroll(function () {
        var scrollPosition = $(window).scrollTop();

        if (scrollPosition + $(window).height() > (bodyHeight - footerHeight)) {
            sortFilter.hide();
        }
        else {
            sortFilter.show();
        }
    });
});

var filterContainer = $("#filter-container"),
    effect = 'slide',
    options = { direction: 'right' },
    duration = 500;

$('#filter-floating-btn').on('click', function () {
    filters.open();
    appendState('filter');
});

$('#close-filter').on('click', function () {
    history.back();
    filters.close();
});

$('#reset-filters').on('click', function () {
    accordion.resetAll();
});

$('#apply-filters').on('click', function () {
    filters.close();
});


var filterTypeBike = $('#filter-type-bike');
/* city filter */
var cityFilter = $('#filter-city-container');

/* set slider default values */
var filters = {

    open: function () {
        filterContainer.show(effect, options, duration, function () {
            $('html, body').addClass('lock-browser-scroll');
            filterContainer.addClass('fixed');
        });
    },

    close: function () {
        filterContainer.removeClass('fixed');
        filterContainer.hide(effect, options, duration, function () { });
        $('html, body').removeClass('lock-browser-scroll');
    },

    bike: {

        open: function () {
            bikeFilter.show(effect, options, duration, function () {
                filterContainer.addClass('bikes-footer');
            });
        },

        close: function () {
            bikeFilter.hide(effect, options, duration, function () { });
            filterContainer.removeClass('bikes-footer');
        },

        setSelection: function () {
            var selection = accordion.selectedItems();
            if (!selection.length == 0) {
                filterTypeBike.find('.filter-option-key').show();
                filterTypeBike.find('.selected-filters').text(selection);
            }
        }
    },

    city: {

        open: function () {
            cityFilter.show(effect, options, duration, function () {
                cityFilter.addClass('city-header');
            });
        },

        close: function () {
            cityFilter.hide(effect, options, duration, function () { });
            cityFilter.removeClass('city-header');
        }

    },

    set: {

        all: function () {
            filters.set.bike();
            filters.set.previousOwners();
            filters.set.sellerType();
        },

        bike: function () {
            filterTypeBike.find('.selected-filters').text('All Bikes');
        },

        previousOwners: function () {
            $('#previous-owners-list li.active').removeClass('active');
        },

        sellerType: function () {
            $('.filter-type-seller.checked').removeClass('checked');
        }

    },

    loader: {
        open: function () {
            $('html, body').addClass('lock-browser-scroll');
            $('#sort-filters-loader').show();
        },

        close: function () {
            $('html, body').removeClass('lock-browser-scroll');
            $('#sort-filters-loader').hide();
        }
    }
};

$('#previous-owners-list').on('click', 'li', function () {
    var item = $(this);

    if (!item.hasClass('active')) {
        $(this).addClass('active');
    }
    else {
        $(this).removeClass('active');
    }
});

$('.filter-type-seller').on('click', function () {
    var item = $(this);

    if (!item.hasClass('checked')) {
        $(this).addClass('checked');
    }
    else {
        $(this).removeClass('checked');
    }
});



$('#filter-type-city').on('click', '.filter-option-value', function () {
    filters.city.open();
    appendState('filterCity');
});

$('#close-city-filter').on('click', function () {
    filters.city.close();
});

$('#filter-city-list').on('click', 'li', function () {
    filters.city.close();

});


/* bikes filter */
var bikeFilter = $('#filter-bike-container');

$('#filter-type-bike').on('click', '.filter-option-value', function () {
    filters.bike.open();
    appendState('filterBikes');
});

$('#close-bike-filter, #set-bikes-filter').on('click', function () {
    filters.bike.setSelection();
    filters.bike.close();
});

$('#reset-bikes-filter').on('click', function () {
    accordion.resetAll();
});

var bikeFilterList = $('#filter-bike-list');

bikeFilterList.on('click', '.accordion-label-tab', function () {
    var tab = $(this).closest('.accordion-tab');
    if (!tab.hasClass('active')) {
        accordion.open(tab);
    }
    else {
        accordion.close(tab);
    }
});

bikeFilterList.on('click', '.accordion-tab .accordion-checkbox', function () {
    var tab = $(this).closest('.accordion-tab');

    if (!tab.hasClass('tab-checked')) {
        tab.addClass('tab-checked');
        accordion.setTab(tab);
    }
    else {
        tab.removeClass('tab-checked');
        accordion.resetTab(tab);
    }
});

bikeFilterList.on('click', '.bike-model-list li', function () {
    var item = $(this);

    if (!item.hasClass('active')) {
        item.addClass('active');
        accordion.setCount(item);
    }
    else {
        item.removeClass('active');
        accordion.setCount(item);
    }
});

/* accordion */
var accordion = {

    tabs: $('#filter-bike-list .accordion-tab'),

    open: function (item) {
        accordion.tabs.removeClass('active');
        accordion.tabs.siblings('ul').slideUp();
        item.addClass('active');
        item.siblings('ul').slideDown();
    },

    close: function (item) {
        item.removeClass('active');
        item.siblings('ul').slideUp();
    },

    setCount: function (item) {
        var modelList = item.closest('.bike-model-list'),
            modelsCount = modelList.find('li.active').length,
            tab = modelList.siblings('.accordion-tab'),
            tabCountLabel = tab.find('.accordion-count');

        if (tab.hasClass('tab-checked')) {
            tab.removeClass('tab-checked');
        }

        if (!modelsCount == 0) {
            if (modelsCount == 1) {
                tabCountLabel.html('(' + modelsCount + ' Model)');
            }
            else {
                tabCountLabel.html('(' + modelsCount + ' Models)');
            }
        }
        else {
            tabCountLabel.empty();
        }

    },

    setTab: function (tab) {
        var modelList = tab.siblings('.bike-model-list');

        modelList.find('li').addClass('active');
        tab.find('.accordion-count').html('(All models)');
    },

    resetTab: function (tab) {
        var modelList = tab.siblings('.bike-model-list');

        modelList.find('li').removeClass('active');
        tab.find('.accordion-count').empty();
    },

    selectedItems: function () {
        var selection = '',
            tabs = $('#filter-bike-list .accordion-tab');

        tabs.each(function (index) {
            if ($(this).hasClass('tab-checked')) {
                selection += ', ' + $(this).find('.accordion-label').text();

            }
            else {
                var list = $(this).siblings('ul.bike-model-list').find('li.active');

                list.each(function (index) {
                    selection += ', ' + $(this).find('.bike-model-label').text();
                });
            }
        });

        return selection.substr(1);

    },

    resetAll: function () {
        var bikeList = $('#filter-bike-list');

        bikeList.find('.accordion-tab.active').siblings('.bike-model-list').hide();
        bikeList.find('.accordion-tab.active').removeClass('active');
        bikeList.find('.accordion-tab.tab-checked').removeClass('tab-checked');
        bikeList.find('.bike-model-list li.active').removeClass('active');
        accordion.tabs.find('.accordion-count').text('');
    }
};

/* sort by */
var sortByList = $('#sort-by-list');

sortByList.on('click', 'li', function () {
    var item = $(this);

    if (!item.hasClass('active')) {
        sortByList.find('li.active').removeClass('active');
        item.addClass('active');
    }
});

$('#sort-floating-btn').on('click', function () {
    sortBy.open();
    appendState('sortBy');
});

$('#cancel-sort-by').on('click', function () {
    history.back();
    sortBy.close();
});

$('#apply-sort-by').on('click', function () {
    history.back();
    sortBy.close();
});

var sortBy = {
    popup: $('#sort-by-container'),

    open: function () {
        sortBy.popup.show();
        $('html, body').addClass('lock-browser-scroll');
        $('.modal-background').show();
    },

    close: function () {
        sortBy.popup.hide();
        $('html, body').removeClass('lock-browser-scroll');
        $('.modal-background').hide();
    }
};

/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');
};

$(window).on('popstate', function (event) {
    if ($('#filter-container').is(':visible')) {
        if ($('#filter-city-container').is(':visible')) {
            filters.city.close();
        }
        else if ($('#filter-bike-container').is(':visible')) {
            filters.bike.close();
        }

        else {
            filters.close();
        }
    }
    if ($('#sort-by-container').is(':visible')) {
        sortBy.close();
    }
});

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

$('#close-city-model-carousel').on('click', function () {
    $('#city-model-used-carousel').slideUp();
    SetUsedCookie();
});
function SetUsedCookie() {
    var arr = getCookie("Used").split('&');
    switch (usedPageIdentifier) {
        case "0":
            arr[0] = "BrandIndia=0";
            break;
        case "1":
            arr[1] = "BrandCity=0";
            break;
        case "2":
            arr[2] = "ModelIndia=0";
            break;
        case "3":
            arr[3] = "UsedCity=0";
            break;


    }
    var newKeyValuePair = arr.join('&');
    SetCookie("Used", newKeyValuePair);
}