﻿/* Sticky-kit v1.1.2 */
(function(){var b,f;b=this.jQuery||window.jQuery;f=b(window);b.fn.stick_in_parent=function(d){var A,w,J,n,B,K,p,q,k,E,t;null==d&&(d={});t=d.sticky_class;B=d.inner_scrolling;E=d.recalc_every;k=d.parent;q=d.offset_top;p=d.spacer;w=d.bottoming;null==q&&(q=0);null==k&&(k=void 0);null==B&&(B=!0);null==t&&(t="is_stuck");A=b(document);null==w&&(w=!0);J=function(a,d,n,C,F,u,r,G){var v,H,m,D,I,c,g,x,y,z,h,l;if(!a.data("sticky_kit")){a.data("sticky_kit",!0);I=A.height();g=a.parent();null!=k&&(g=g.closest(k));
if(!g.length)throw"failed to find stick parent";v=m=!1;(h=null!=p?p&&a.closest(p):b("<div />"))&&h.css("position",a.css("position"));x=function(){var c,f,e;if(!G&&(I=A.height(),c=parseInt(g.css("border-top-width"),10),f=parseInt(g.css("padding-top"),10),d=parseInt(g.css("padding-bottom"),10),n=g.offset().top+c+f,C=g.height(),m&&(v=m=!1,null==p&&(a.insertAfter(h),h.detach()),a.css({position:"",top:"",width:"",bottom:""}).removeClass(t),e=!0),F=a.offset().top-(parseInt(a.css("margin-top"),10)||0)-q,
u=a.outerHeight(!0),r=a.css("float"),h&&h.css({width:a.outerWidth(!0),height:u,display:a.css("display"),"vertical-align":a.css("vertical-align"),"float":r}),e))return l()};x();if(u!==C)return D=void 0,c=q,z=E,l=function(){var b,l,e,k;if(!G&&(e=!1,null!=z&&(--z,0>=z&&(z=E,x(),e=!0)),e||A.height()===I||x(),e=f.scrollTop(),null!=D&&(l=e-D),D=e,m?(w&&(k=e+u+c>C+n,v&&!k&&(v=!1,a.css({position:"fixed",bottom:"",top:c}).trigger("sticky_kit:unbottom"))),e<F&&(m=!1,c=q,null==p&&("left"!==r&&"right"!==r||a.insertAfter(h),
h.detach()),b={position:"",width:"",top:""},a.css(b).removeClass(t).trigger("sticky_kit:unstick")),B&&(b=f.height(),u+q>b&&!v&&(c-=l,c=Math.max(b-u,c),c=Math.min(q,c),m&&a.css({top:c+"px"})))):e>F&&(m=!0,b={position:"fixed",top:c},b.width="border-box"===a.css("box-sizing")?a.outerWidth()+"px":a.width()+"px",a.css(b).addClass(t),null==p&&(a.after(h),"left"!==r&&"right"!==r||h.append(a)),a.trigger("sticky_kit:stick")),m&&w&&(null==k&&(k=e+u+c>C+n),!v&&k)))return v=!0,"static"===g.css("position")&&g.css({position:"relative"}),
a.css({position:"absolute",bottom:d,top:"auto"}).trigger("sticky_kit:bottom")},y=function(){x();return l()},H=function(){G=!0;f.off("touchmove",l);f.off("scroll",l);f.off("resize",y);b(document.body).off("sticky_kit:recalc",y);a.off("sticky_kit:detach",H);a.removeData("sticky_kit");a.css({position:"",bottom:"",top:"",width:""});g.position("position","");if(m)return null==p&&("left"!==r&&"right"!==r||a.insertAfter(h),h.remove()),a.removeClass(t)},f.on("touchmove",l),f.on("scroll",l),f.on("resize",
y),b(document.body).on("sticky_kit:recalc",y),a.on("sticky_kit:detach",H),setTimeout(l,0)}};n=0;for(K=this.length;n<K;n++)d=this[n],J(b(d));return this}}).call(this);
//---------------------------------------------------------------------------------

//$(document).ready(function () {
//    //set filters
//    filters.set.all();
//});

$('#listing-left-column').stick_in_parent();
$('.city-chosen-select').chosen();

var budgetValue = [0, 10000, 20000, 35000, 50000, 80000, 125000, 200000];
var bikesList = $("#filter-bike-list");
var citiesList = $("#filter-city-list li");

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
        div += self.totalData() % self.pageSize() > 0 ? 1 : 0;
        return div - 1;
    });
    self.paginated = ko.computed(function () {
        var pgSlot = self.pageNumber() + self.pageSlot();
        if (pgSlot > self.totalPages()) pgSlot = self.totalPages() + 1;
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

        $(element).text(num + suf);
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
    self.noBikes = ko.observable(false);
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
    self.Pagination = ko.observable(new vmPagination());

    self.FilterCity = function (d, e) {
        var ele = $(e.target);
        if (!ele.hasClass("active")) {
            ele.addClass("active").siblings().removeClass("active");
            self.SelectedCity({ "id": ele.attr("data-cityid"), "name": ele.text() });
        };
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
        try {
            self.ResetFilters();
            self.ApplyBikeFilter();
            if (self.SelectedCity() && self.SelectedCity().id > 0) self.Filters()["city"] = self.SelectedCity().id;
            if (self.KmsDriven() != 200000) self.Filters()["kms"] = self.KmsDriven();
            if (self.BikeAge() != 8) self.Filters()["age"] = self.BikeAge();
            if (self.BudgetValues()) {
                var minBuget = self.BudgetValues()[0], maxBuget = self.BudgetValues()[1];
                self.Filters()["budget"] = budgetValue[minBuget];
                if (maxBuget != 7) self.Filters()["budget"] += "+" + budgetValue[maxBuget];
            }
            self.FilterOwners();
            self.FilterSellers();
            self.GetUsedBikes();
        } catch (e) {
            console.warn("Unable to apply current filters");
        }
    };
    self.ResetFilters = function () {
        var so = self.Filters()["so"];
        self.Filters(new Object());
        self.Filters()["so"] = so;
    };

    self.SetDefaultFilters = function () {
        try {
            self.SelectedCity({ "id": 0, "name": "All India" });
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
        self.ApplyBikeFilter();
        if (self.SelectedCity() && self.SelectedCity().id > 0) self.Filters()["city"] = self.SelectedCity().id;
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
                for (var i = self.Pagination().pageNumber() ; i < n; i++) {
                    var pageUrl = qs.replace(rstr, "page-" + i);
                    pages += ' <li class="page-url"><a  data-bind="click : ChangePageNumber" data-pagenum="' + i + '" href="' + pageUrl + '">' + i + '</a></li>';
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

    self.GetUsedBikes = function () {
        try {
            self.Filters.notifySubscribers();

            var qs = self.QueryString();
            $.ajax({
                type: 'GET',
                url: '/api/used/search/?bikes=1&' + qs.replace(/[\+]/g, "%2B"),
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
                    if (self.TotalBikes() > 0) self.noBikes(false); else self.noBikes(true);
                    filters.loader.close();
                    self.OnInit(false);
                    self.IsReset(false);
                    self.ApplyPagination();
                }
            });
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

            self.GetUsedBikes();
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
            else if (window.location.hash && window.location.hash != "") self.GetUsedBikes();
        } catch (e) {
            console.warn("Unable to set page filters");
        }

    };

};

$(document).on("click", "#pagination-list li a,span.pagination-control-prev a,span.pagination-control-next a", function (d, e) {
    vwUsedBikes.ChangePageNumber(d, e);
})

var vwUsedBikes = new usedBikes();
ko.applyBindings(vwUsedBikes, document.getElementById("usedBikesSection"));

var objFilters = vwUsedBikes.Filters();

$(function () {
    vwUsedBikes.SetDefaultFilters();
    vwUsedBikes.SetPageFilters();

    if (selectedModelId && selectedModelId != "" && selectedModelId != "0") {
        var ele = bikesList.find("ul.bike-model-list span[data-modelid=" + selectedModelId + "]");
        ele.closest("ul.bike-model-list li").addClass("active");
    }
    else if (selectedMakeId && selectedMakeId != "0") {
        var ele = bikesList.find("span[data-makeid=" + selectedMakeId + "]");
        ele.closest(".accordion-tab").trigger("click");
        ele.closest(".accordion-checkbox").trigger("click");
    }
    $('#set-bikes-filter').trigger('click');

    if (selectedCityId)
        $("#filter-city-list li[data-cityid=" + selectedCityId + "]").click();
    vwUsedBikes.TotalBikes() > 0 ? vwUsedBikes.OnInit(true) : vwUsedBikes.OnInit(false);


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


///* budget slider */
//var budgetValue = ['0', '10,000', '20,000', '35,000', '50,000', '80,000', '1,25,000', '2,00,000+'],
//    budgetKey = [0, 1, 2, 3, 4, 5, 6, 7];

//$('#budget-range-slider').slider({
//    orientation: 'horizontal',
//    range: true,
//    min: 0,
//    max: 7,
//    step: 1,
//    values: [0, 7],
//    slide: function (event, ui) {
//        var left = event.keyCode != $.ui.keyCode.RIGHT,
//            right = event.keyCode != $.ui.keyCode.LEFT,
//            value = findNearest(left, right, ui.value);

//        if (ui.values[0] == ui.values[1]) {
//            return false;
//        }

//        filters.budgetAmount(ui.values);
//        filters.selection.set.slider('budget-amount');
//    }
//});

//function findNearest(left, right, value) {
//    var nearest = null;
//    var diff = null;
//    for (var i = 0; i < budgetKey.length; i++) {
//        if ((left && budgetKey[i] <= value) || (right && budgetKey[i] >= value)) {
//            var newDiff = Math.abs(value - budgetKey[i]);
//            if (diff == null || newDiff < diff) {
//                nearest = budgetKey[i];
//                diff = newDiff;
//            }
//        }
//    }
//    return nearest;
//}

//function getRealValue(sliderValue) {
//    for (var i = 0; i < budgetKey.length; i++) {
//        if (budgetKey[i] >= sliderValue) {
//            return budgetValue[i];
//        }
//    }
//    return 0;
//}

///* kms slider */
//$("#kms-range-slider").slider({
//    range: 'min',
//    value: 80000,
//    min: 5000,
//    max: 80000,
//    step: 5000,
//    slide: function (event, ui) {
//        filters.kilometerAmount(ui.value);
//        filters.selection.set.slider('kms-amount'); // div id of slider values
//    }
//});

///* bike age slider */
//$("#bike-age-slider").slider({
//    range: 'min',
//    value: 8,
//    min: 1,
//    max: 8,
//    step: 1,
//    slide: function (event, ui) {
//        filters.bikeAgeAmount(ui.value);
//        filters.selection.set.slider('bike-age-amount');
//    }
//});

//$('#previous-owners-list').on('click', 'li', function () {
//    var item = $(this);

//    if (!item.hasClass('active')) {
//        $(this).addClass('active');
//        filters.selection.set.owner(item);
//    }
//    else {
//        $(this).removeClass('active');
//        filters.selection.reset.owner(item);
//    }
//});

//$('#seller-type-list').on('click', 'li', function () {
//    var item = $(this);

//    if (!item.hasClass('checked')) {
//        $(this).addClass('checked');
//        filters.selection.set.seller(item);
//    }
//    else {
//        $(this).removeClass('checked');
//        filters.selection.reset.seller(item);
//    }
//});

//$('#reset-filters').on('click', function () {
//    filters.reset.all();
//    accordion.resetAll();
//    $('#selected-filters li').empty();
//    filters.set.clearButton();
//});

//$('#reset-bikes-filter').on('click', function () {
//    accordion.resetAll();
//    filters.set.clearButton();
//    $('#bike').empty();
//});

/* bike filters */

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
        filters.selection.set.make(tab);
    }
    else {
        tab.removeClass('tab-checked');
        filters.selection.reset.make(tab);
        accordion.resetTab(tab);
    }

    filters.set.clearButton();
});

bikeFilterList.on('click', '.bike-model-list li', function () {
    var item = $(this);

    if (!item.hasClass('active')) {
        item.addClass('active');
        accordion.setCount(item);
        filters.selection.set.model(item);
    }
    else {
        item.removeClass('active');
        var tab = item.closest('.bike-model-list-content').siblings('.accordion-tab');

        if (!tab.hasClass('tab-checked')) {
            filters.selection.reset.model(item);
        }
        else {
            filters.selection.reset.tab(tab);
        }

        accordion.setCount(item);
    }
    
    filters.set.clearButton();
});

/* remove selected filters */
/* bike */
$('#selected-filters').on('click', '#bike p', function () {
    var item = $(this);

    if ($(this).attr('data-type') != 'make') {
        filters.selection.cancel.model(item);
    }
    else {
        filters.selection.cancel.make(item);
    }
});

/* sliders */
$('#selected-filters').on('click', '.type-slider p', function () {
    var contentType = $(this).closest('li').attr('data-id');
    filters.selection.cancel.slider(contentType);

    switch (contentType) {
        case 'budget-amount':
            filters.reset.budget();
            break;

        case 'kms-amount':
            filters.reset.kilometers();
            break;

        case 'bike-age-amount':
            filters.reset.bikeAge();
            break;

        default:
            break;
    }
    //filters.reset.kilometers();
});

/* owner */
$('#selected-filters').on('click', '#owners p', function () {
    filters.selection.cancel.owner($(this));
});

/* seller */
$('#selected-filters').on('click', '#seller p', function () {
    filters.selection.cancel.seller($(this));
});

/* set slider default values */
var filters = {
    
    budgetAmount: function (units) {
        var budgetminValue = getRealValue(units[0]),
            budgetmaxValue = getRealValue(units[1]);

        if (units[0] == 0 && units[1] == 7) {
            $("#budget-amount").html('<span class="bwsprite inr-sm-dark"></span> 0 - <span class="bwsprite inr-sm-dark"></span> ' + budgetmaxValue);
        }
        else {
            $("#budget-amount").html('<span class="bwsprite inr-sm-dark"></span> ' + budgetminValue + ' - <span class="bwsprite inr-sm-dark"></span> ' + budgetmaxValue);
        }
    },

    kilometerAmount: function (unit) {
        var kilometerValue = unit.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

        if (unit == 80000) {
            $("#kms-amount").html('0 - ' + kilometerValue + '+ kms');
        }
        else {
            $("#kms-amount").html('0 - ' + kilometerValue + ' kms');
        }

    },

    bikeAgeAmount: function (unit) {
        if (unit == 8) {
            $("#bike-age-amount").html('0 - ' + unit + '+ years');
        }
        else {
            $("#bike-age-amount").html('0 - ' + unit + ' years');
        }
    },

    set: {

        all: function () {
            //filters.set.city();
            filters.set.bike();
            filters.set.budget();
            filters.set.kilometers();
            filters.set.bikeAge();
            filters.set.previousOwners();
            filters.set.sellerType();
        },

        city: function () {
            $('#filter-type-city .selected-filters').text('All India');
        },

        bike: function () {
            //filterTypeBike.find('.selected-filters').text('All Bikes');
            var inputBoxes = $('.getModelInput');
            inputBoxes.each(function () {
                var filterList = $(this).closest('.bike-model-list-content').find('.bike-model-list');
                $(this).fastLiveFilter(filterList);
            });
        },

        budget: function () {
            var values = [3, 5];
            $('#budget-range-slider').slider('option', 'values', values);

            filters.budgetAmount(values);
        },

        kilometers: function () {
            var kilometerSlider = $('#kms-range-slider'),
                kmSliderValue;

            kilometerSlider.slider('option', 'value', 50000);
            kmSliderValue = kilometerSlider.slider('value');

            filters.kilometerAmount(kmSliderValue);
        },

        bikeAge: function () {
            var ageSlider = $('#bike-age-slider'),
                ageSliderValue;

            ageSlider.slider('option', 'value', 5);
            ageSliderValue = ageSlider.slider('value');

            filters.bikeAgeAmount(ageSliderValue);
        },

        previousOwners: function () {
            $('#previous-owners-list li.active').removeClass('active');
        },

        sellerType: function () {
            $('.filter-type-seller.checked').removeClass('checked');
        },

        clearButton: function () {
            var activeTab = $('.accordion-tab.tab-checked').length,
                activeModel = $('.bike-model-list li.active').length,
                bikeFilter = $('#filter-type-bike');

            if (activeTab > 0 || activeModel > 0) {
                bikeFilter.addClass('active-clear');
            }
            else {
                bikeFilter.removeClass('active-clear');
            }

        }

    },

    reset: {

        all: function () {
            //filters.reset.city();
            //filters.reset.bike();
            filters.reset.budget();
            filters.reset.kilometers();
            filters.reset.bikeAge();
            filters.reset.previousOwners();
            filters.reset.sellerType();
        },

        city: function () {
            $('#filter-type-city .selected-filters').text('All India');
        },

        bike: function () {
            filterTypeBike.find('.selected-filters').text('All Bikes');
        },

        budget: function () {
            $('#budget-range-slider').slider('option', 'values', [0, 7]);
            $('#budget-amount').html('<span class="bwmsprite inr-xxsm-icon"></span>0 - <span class="bwmsprite inr-xxsm-icon"></span>2,00,000+');
        },

        kilometers: function () {
            $('#kms-range-slider').slider('option', 'value', 80000);
            $("#kms-amount").html('0 - ' + $("#kms-range-slider").slider("value").toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ' kms');
        },

        bikeAge: function () {
            $('#bike-age-slider').slider('option', 'value', 8);
            $("#bike-age-amount").html('0 - ' + $("#bike-age-slider").slider("value") + '+ years');
        },

        previousOwners: function () {
            $('#previous-owners-list li.active').removeClass('active');
        },

        sellerType: function () {
            $('.filter-type-seller.checked').removeClass('checked');
        }
    },

    selection: {
        list: $('#selected-filters'),

        bikeList: $('#bike'),

        ownerList: $('#owners'),

        sellerList: $('#seller'),

        set: {
            make: function (item) {
                var itemID = item.attr('id'),
                    itemLabel = item.find('.category-label').text();

                filters.selection.bikeList.append('<p data-id="' + itemID + '" data-type="make">' + itemLabel + '<span class="bwsprite cross-icon"></span></p>');

                var selectedBike = filters.selection.bikeList.find('p'),
                    selectedBikeLength = selectedBike.length,
                    modelList = item.siblings('.bike-model-list-content').find('.bike-model-list li'),
                    modelListLength = modelList.length,
                    i;

                selectedBike.each(function (index) {
                    if (selectedBikeLength > 0) {
                        for (i = 0; i < modelListLength; i++) {
                            var item = modelList[i];
                            if ($(this).attr('data-id') == $(item).attr('id')) {
                                $(this).remove();
                            }
                        }
                    }
                });
            },

            model: function (item) {
                var itemID = item.attr('id'),
                    itemLabel = item.find('.category-label').text();

                filters.selection.bikeList.append('<p data-id="' + itemID + '" data-type="model">' + itemLabel + '<span class="bwsprite cross-icon"></span></p>');
            },

            slider: function (content) {
                var amount = $('#'+ content).html(),
                    sliderItem = filters.selection.list.find('li[data-id="' + content + '"]');
               
                sliderItem.empty().append('<p>' + amount + '<span class="bwsprite cross-icon"></span></p>');
            },

            owner: function (item) {
                var itemID = item.attr('id'),
                    itemLabel = item.text();

                switch (itemLabel) {
                    case '1':
                        itemLabel = itemLabel + 'st owner';
                        break;

                    case '2':
                        itemLabel = itemLabel + 'nd owner';
                        break;

                    case '3':
                        itemLabel = itemLabel + 'rd owner';
                        break;

                    case '4':
                        itemLabel = itemLabel + 'th owner';
                        break;

                    case '4+':
                        itemLabel = itemLabel + ' owner';
                        break;

                    default:
                        break;
                }

                filters.selection.ownerList.append('<p data-id="' + itemID + '">' + itemLabel + '<span class="bwsprite cross-icon"></span></p>');
            },

            seller: function (item) {
                var itemID = item.attr('id'),
                    itemLabel = item.text();

                filters.selection.sellerList.append('<p data-id="' + itemID + '">' + itemLabel + '<span class="bwsprite cross-icon"></span></p>');
            }
        },

        reset: {
            make: function (tab) {
                var itemID = tab.closest('.accordion-tab').attr('id');

                filters.selection.bikeList.find($('p[data-id="' + itemID + '"]')).remove();
            },

            model: function (item) {
                var itemID = item.attr('id');

                filters.selection.bikeList.find('p[data-id="' + itemID + '"]').remove();
            },

            tab: function (tab) {
                var activeModels = tab.siblings('.bike-model-list-content').find('li.active'),
                    activeModelLength = activeModels.length,
                    i;

                filters.selection.bikeList.find('p[data-id="' + tab.attr('id') + '"').remove();

                for (i = 0; i < activeModelLength; i++) {
                    var item = activeModels[i],
                        itemID = $(item).attr('id'),
                        itemLabel = $(item).find('.category-label').text();

                    filters.selection.bikeList.append('<p data-id="' + itemID + '" data-type="model">' + itemLabel + '<span class="bwsprite cross-icon"></span></p>');
                }                

            },
            
            owner: function (item) {
                var itemID = item.attr('id');

                filters.selection.ownerList.find('p[data-id="' + itemID + '"]').remove();
            },

            seller: function (item) {
                var itemID = item.attr('id');

                filters.selection.sellerList.find('p[data-id="' + itemID + '"]').remove();
            }
            
        },

        cancel: {
            make: function (item) {
                var itemID = item.attr('data-id');

                $('#filter-sidebar').find($('#' + itemID)).find('.accordion-checkbox').trigger('click');
                item.remove();
            },

            model: function (item) {
                var itemID = item.attr('data-id');

                $('#filter-sidebar').find($('#' + itemID)).trigger('click');
                item.remove();
            },

            slider: function (content) {
                var sliderItem = filters.selection.list.find('li[data-id="' + content + '"]');
                sliderItem.empty();
            },

            owner: function (item) {
                var itemID = item.attr('data-id');

                $('#previous-owners-list').find('li[id="' + itemID + '"]').trigger('click');
            },

            seller: function (item) {
                var itemID = item.attr('data-id');

                $('#seller-type-list').find('li[id="' + itemID + '"]').trigger('click');
            }
        }
    }
};

/* fastLiveFilter jQuery plugin 1.0.3 */
jQuery.fn.fastLiveFilter = function(list, options) {
	// Options: input, list, timeout, callback
	options = options || {};
	list = jQuery(list);
	var input = this;
	var lastFilter = '', noResultLen = 0;
	var timeout = options.timeout || 0;
	var callback = options.callback || function (total) {
	    noResultLen = list.siblings('.no-result').length;

	    if (total == 0 && noResultLen < 1)
	        list.after(noResultDiv).show();
	    else if (total > 0 && noResultLen > 0)
	        $('.no-result').remove();
	};

	var keyTimeout;

	var noResultDiv = '<div class="no-result content-inner-block-10 text-light-grey">No search found!</div>';

	var lis = list.children();
	var len = lis.length;
	var oldDisplay = len > 0 ? lis[0].style.display : "block";
	callback(len); // do a one-time callback on initialization to make sure everything's in sync

	input.change(function() {
		var filter = input.val().toLowerCase();
		var li, innerText;
		var numShown = 0;
		for (var i = 0; i < len; i++) {
			li = lis[i];
			innerText = !options.selector ?
				(li.textContent || li.innerText || "") :
				$(li).find(options.selector).text();

			if (innerText.toLowerCase().indexOf(filter) >= 0) {
				if (li.style.display == "none") {
					li.style.display = oldDisplay;
				}
				numShown++;
			} else {
				if (li.style.display != "none") {
					li.style.display = "none";
				}
			}
		}

		callback(numShown);
		return false;
	}).keydown(function() {
		clearTimeout(keyTimeout);
		keyTimeout = setTimeout(function() {
			if( input.val() === lastFilter ) return;
			lastFilter = input.val();
			input.change();
		}, timeout);
	});
	return this; // maintain jQuery chainability
}

/* accordion */
var accordion = {

    tabs: $('#filter-bike-list .accordion-tab'),

    open: function (item) {
        accordion.inputBox($('#filter-bike-list .accordion-tab.active').siblings('.bike-model-list-content'));
        accordion.tabs.removeClass('active');
        accordion.tabs.siblings('.bike-model-list-content').slideUp();
        item.addClass('active');
        item.siblings('.bike-model-list-content').slideDown();
    },

    close: function (item) {
        item.removeClass('active');
        item.siblings('.no-result').remove();
        item.siblings('.bike-model-list-content').slideUp();
        accordion.inputBox(item.siblings('.bike-model-list-content'));
    },

    inputBox: function (listContent) {
        listContent.find('input[type="text"]').val('');
        listContent.find('li').show();
    },

    setCount: function (item) {
        var modelList = item.closest('.bike-model-list'),
            modelsCount = modelList.find('li.active').length,
            tab = modelList.closest('.bike-model-list-content').siblings('.accordion-tab'),
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
        var modelList = tab.siblings('.bike-model-list-content');

        modelList.find('li').addClass('active');
        tab.find('.accordion-count').html('(All models)');
    },

    resetTab: function (tab) {
        var modelList = tab.siblings('.bike-model-list-content');

        modelList.find('li').removeClass('active');
        tab.find('.accordion-count').empty();
    },

    resetAll: function () {
        var bikeList = $('#filter-bike-list');

        bikeList.find('.accordion-tab.active').siblings('.bike-model-list-content').hide();
        bikeList.find('.accordion-tab.active').removeClass('active');
        bikeList.find('.accordion-tab.tab-checked').removeClass('tab-checked');
        bikeList.find('.bike-model-list li.active').removeClass('active');
        accordion.tabs.find('.accordion-count').text('');
    }
};

/* sort by */
var sortByDiv = $(".sort-div"),
    sortListDiv = $(".sort-selection-div");

sortByDiv.on('click', function () {
    if (!sortByDiv.hasClass("open"))
        sortBy.open();
    else
        sortBy.close();
});

$('#sort-listing').on('click', 'li', function () {
    var item = $(this);

    if (!item.hasClass('selected')) {
        $('#sort-listing li.selected').removeClass('selected');
        item.addClass('selected');
        sortBy.selection(item);
    }
    else {
        sortBy.close();
    }
});

var sortBy = {
    open: function () {
        sortByDiv.addClass('open');
        sortListDiv.show();
    },

    close: function () {
        sortByDiv.removeClass('open');
        sortListDiv.slideUp();
    },

    selection: function (item) {
        var itemText = item.text();
        sortByDiv.find('.sort-select-btn').text(itemText);
        sortBy.close();
    }
}

/* close sortby */
$(document).mouseup(function (e) {
    var container = $('#sort-by-content');
    if (container.find('.sort-div').hasClass('open') && $('.sort-selection-div').is(':visible')) { 
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            sortByDiv.trigger('click');
        }
    }
});