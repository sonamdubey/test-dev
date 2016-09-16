//*****************************************************************************************************

/* budget slider */
var budgetValue = [0, 10000, 20000, 35000, 50000, 80000, 125000, 200000];
var bikesList = $("#filter-bike-list");

//parse query string
var getQueryString = function () {
    var qsColl = new Object();
    var requestUrl = window.location.hash.substr(1); 
    if (requestUrl && requestUrl != '') {
        var kvPairs = requestUrl.split('&');
        $.each(kvPairs, function (i, val) {
            var kvPair = val.split('=');
            qsColl[kvPair[0]] = kvPair[1];
        });
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
        var div = Math.floor(self.totalData() / self.pageSize());
        div += self.totalData() % self.pageSize() > 0 ? 1 : 0;
        return div - 1;
    });
    self.paginated = ko.computed(function () {
        var pgSlot = self.pageNumber() + self.pageSlot();
        if (pgSlot > self.totalPages()) pgSlot = self.totalPages();
        return pgSlot;
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

        $(element).text(num+suf);
    }
};

ko.bindingHandlers.KOSlider = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var options = allBindingsAccessor().sliderOptions || {};
        var observable = valueAccessor();

        options.slide = function (e, ui) {
            observable(ui.values ? ui.values : ui.value);
        };

        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).slider("destroy");
        });

        $(element).slider(options);
    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        if (value)
        {
            $(element).slider(value.length ? "values" : "value", value);
            $(element).change();
        }        

    }
};

function formatPrice(price) {
    if (price != null)
    {
        price = price.toString();
        var lastThree = price.substring(price.length - 3);
        var otherNumbers = price.substring(0, price.length - 3);
        if (otherNumbers != '')
            lastThree = ',' + lastThree;
        var price = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
    }
    
    return price;
}

var usedBikes = function()
{
    var self = this;
    self.Filters = ko.observable(getQueryString());
    self.QueryString = ko.computed(function () {
        var qs = "";
        $.each(self.Filters(), function (i, val) {
            if (val != null && val != "")
                qs += "&" + i + "=" + val;
        });
        qs = qs.substr(1);
        window.location.hash = qs;
        $('#hdnHash').val(qs);
        return qs;
    });     
    self.OnInit = ko.observable(true);
    self.noBikes = ko.observable(false);
    self.PageHeading = ko.observable();
    self.TotalBikes = ko.observable();
    self.BikeDetails = ko.observableArray();
    self.PageUrl = ko.observable();
    self.CurPageNo = ko.observable(0);
    self.BikePhotos = function () {
        var self = this;
        self.hostUrl = ko.observable();
        self.OriginalImgPath = ko.observable();
        self.imgPath = ko.observable();
    };
    self.PrevPageUrl = ko.observable();
    self.NextPageUrl = ko.observable();
    self.Pagination = ko.observable(new vmPagination(self.CurPageNo(), 20, self.TotalBikes()));

    self.FilterCity = function (d, e) {
        var ele = $(e.target);
        if (!ele.hasClass("active")) {
            ele.addClass("active").siblings().removeClass("active");
            self.SelectedCity({ "id": ele.attr("data-cityid"), "name": ele.text() });
        };
    };
    self.ApplyBikeFilter = function () {
        var selMakes = bikesList.find("div.accordion-tab.tab-checked span.unchecked-box");
        var selModels = bikesList.find("div.accordion-tab:not(.tab-checked)");
        var mkList = "", moList = "";
        selMakes.each(function () {
            var ele = $(this);
            mkList += "+"+ele.attr("data-makeid");
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
    };  
    self.SelectedCity = ko.observable({ "id": 0, "name": "All India" });
    self.BudgetValues = ko.observable([0, 7]);
    self.ShowBudgetRange = ko.computed(function (d, e) {

        if(self.BudgetValues())
        {
            var minBuget = self.BudgetValues()[0] ,maxBuget =self.BudgetValues()[1]; 
            if (minBuget == 0 && maxBuget == 7) {
                $("#budget-amount").html('<span class="bwmsprite inr-xxsm-icon"></span>0 - <span class="bwmsprite inr-xxsm-icon"></span>' + formatPrice(budgetValue[maxBuget]));
            }
            else {
                $("#budget-amount").html('<span class="bwmsprite inr-xxsm-icon"></span>' + formatPrice(budgetValue[minBuget]) + ' - <span class="bwmsprite inr-xxsm-icon"></span>' + formatPrice(budgetValue[maxBuget]) + ((maxBuget == 7)?'+':''));
            }
        }
    });
    self.KmsDriven = ko.observable(10000);
    self.BikeAge = ko.observable(2);
    self.FilterOwners = function () {
        var owners = $("#previous-owners-list li.active"),ownerList="";
        owners.each(function () {
            ownerList += "+" + $(this).attr("data-ownerid");
        });

        self.Filters()["owner"] = ownerList.substr(1);
    }; 
    self.FilterSellers = function () {
        var owners = $("#sellerTypes .filter-type-seller.checked"), sellerList = "";  
        owners.each(function () {
            sellerList += "+" + $(this).attr("data-sellerid");
        });

        self.Filters()["st"] = sellerList.substr(1);
    };
    self.ApplyFilters = function () {
        self.ResetFilters();
        self.ApplyBikeFilter();
        if (self.SelectedCity() && self.SelectedCity().id > 0) self.Filters()["city"] = self.SelectedCity().id;
        if (self.KmsDriven() > 10000) self.Filters()["kms"] = self.KmsDriven();
        if (self.BikeAge() > 0) self.Filters()["age"] = self.BikeAge();
        if (self.BudgetValues())        {
            var minBuget = self.BudgetValues()[0], maxBuget = self.BudgetValues()[1];
            self.Filters()["budget"] = budgetValue[minBuget];
            if (maxBuget != 7) self.Filters()["budget"] += "+" + budgetValue[maxBuget];
        }
        self.FilterOwners();
        self.FilterSellers();  
        self.GetUsedBikes(); 
    };
    self.ResetFilters = function () {
        self.Filters()["city"] = "";
        self.Filters()["kms"] = "";
        self.Filters()["age"] = "";
        self.Filters()["budget"] = "";
        self.Filters()["owner"] = "";
        self.Filters()["st"] = "";
        self.Filters()["make"] = "";
        self.Filters()["model"] = "";
        self.Filters()["pn"] = "";
    };
    self.objSorts = ko.observableArray([{ id: 1, text: "Most recent" }, { id: 2, text: "Price - Low to High" }, { id: 3, text: "Price - High to Low" }, { id: 4, text: "Kms - Low to High" }, { id: 5, text: "Kms - High to Low" }]);
    
    self.applySort = function (d, e) {
        var so = $("#sort-by-list li.active").attr("data-sortorder");
        self.Filters()["so"] = so;
        self.GetUsedBikes();
    };

    self.PagesListHtml = ko.observable("");
    self.PrevPageHtml = ko.observable("");
    self.NextPageHtml = ko.observable("");

    self.ApplyPagination = function() {
        var pag = new vmPagination(self.CurPageNo(), 20, self.TotalBikes());
        self.Pagination(pag);
        if (self.Pagination())
        {
            var n = self.Pagination().paginated(), pages = '',prevpg='',nextpg='';
            var qs = window.location.pathname + window.location.hash;
            var rstr = qs.match(/page-[0-9]+/i);
            for (var i = self.Pagination().pageNumber() ; i < n; i++) {
                var pageUrl = qs.replace(rstr, "page-" + i);
                pages += ' <li class="page-url"><a  data-bind="click : ChangePageNumber" data-pagenum="' + i + '" href="' + pageUrl + '">' + i + '</a></li>';
            }
            self.PagesListHtml(pages);

            if (self.Pagination().hasPrevious()) {
                prevpg = "<a  data-bind='click : ChangePageNumber' data-pagenum='" + self.Pagination().previous() + "' href='" + qs.replace(rstr, "page-" + self.Pagination().previous()) + "' class='bwmsprite prev-page-icon'/>"; 
            } else
            {
                prevpg = "<a href='javascript:void(0)' class='bwmsprite prev-page-icon'/>";
            }
            self.PrevPageHtml(prevpg);
            if (self.Pagination().hasNext()) {
                nextpg="<a  data-bind='click : ChangePageNumber' data-pagenum='" + self.Pagination().next() + "' href='" + qs.replace(rstr, "page-" + self.Pagination().next()) + "' class='bwmsprite next-page-icon'/>";
            } else {
                nextpg="<a href='javascript:void(0)' class='bwmsprite next-page-icon'/>";
            }
            self.NextPageHtml(nextpg);
            $("#pagination-list li[data-pagenum=" + self.Pagination().pageNumber() + "]").addClass("active");
        }
       
        
    };

    self.GetUsedBikes = function () {
        self.Filters.notifySubscribers();
        var qs = self.QueryString();
        $.ajax({
            type: 'GET',
            url: '/api/used/search/?' + qs.replace(/[\+]+/g, "%2B"),
            dataType: 'json',
            success: function (response) {
                window.location.hash = qs;
                self.OnInit(false);
                self.TotalBikes(response.totalCount);
                self.CurPageNo(response.currentPageNo);
                self.PageUrl(response.pageUrl);
                self.BikeDetails(ko.toJS(response.result));
                if (!self.TotalBikes()) self.noBikes(true);
            },
            complete: function (xhr) {
                if (xhr && xhr.status!= 200) {
                    self.noBikes(true);
                    self.TotalBikes(0);
                    self.CurPageNo(0);
                }

                self.ApplyPagination();
                
            }
        });
    };

    self.ChangePageNumber = function (e) {
        var pnum = $(e.target).attr("data-pagenum");
        self.Filters()["pn"] = pnum;
        self.GetUsedBikes();
        e.preventDefault();
        return false;
    };

    self.SetPageFilters = function () {
        //set sort filter
        if (self.Filters()["so"]) {
            $("#sort-by-list li[data-sortorder=" + objFilters["so"] + "]").addClass("active").siblings().removeClass("active");
        }
        //set city filter
        if (self.Filters()["so"]) {
            $("#sort-by-list li[data-sortorder=" + objFilters["so"] + "]").addClass("active").siblings().removeClass("active");
        }
        if (self.Filters()["so"]) {
            $("#sort-by-list li[data-sortorder=" + objFilters["so"] + "]").addClass("active").siblings().removeClass("active");
        }
        if (self.Filters()["so"]) {
            $("#sort-by-list li[data-sortorder=" + objFilters["so"] + "]").addClass("active").siblings().removeClass("active");
        }
        if (self.Filters()["so"]) {
            $("#sort-by-list li[data-sortorder=" + objFilters["so"] + "]").addClass("active").siblings().removeClass("active");
        }
    };

}

$(document).on("click", "#pagination-list li a,span.pagination-control-prev a,span.pagination-control-next a", function (d,e) {
    vwUsedBikes.ChangePageNumber(d,e);
})

var vwUsedBikes = new usedBikes();
ko.applyBindings(vwUsedBikes, document.getElementById("usedBikesSection"));

var objFilters = vwUsedBikes.Filters();

$(function () {
    
    setSortFilter();
    //GetUsedBikes();
});

function setSortFilter()
{
    if(objFilters && objFilters["so"])
    {
        $("#sort-by-list li[data-sortorder=" + objFilters["so"] + "]").addClass("active").siblings().removeClass("active");
    }

}




//*****************************************************************************************************

$(document).ready(function () {

    var sortFilter = $('#sort-filter-wrapper'),
        bodyHeight = $('body').height(),
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

    //set filters
    filters.set.all();
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
    filters.reset.all();
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
            else {
                filters.reset.bike();
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

    reset: {

        all: function () {
            filters.reset.bike();
            filters.reset.previousOwners();
            filters.reset.sellerType();
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

var bikeFilterList =  $('#filter-bike-list');

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
        var list = $('#filter-bike-list .bike-model-list li.active'),
            listLength = list.length,
            selection = '';

        list.each(function (index) {
            if (index == 7) {
                return false;
            }
            else {
                if (!index == 0) {
                    selection += ', ' + $(this).find('.bike-model-label').text();
                }
                else {
                    selection = $(this).find('.bike-model-label').text();
                }
            }
        })

        return selection;
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
