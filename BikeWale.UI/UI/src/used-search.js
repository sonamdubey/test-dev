/* Sticky-kit v1.1.2 */
(function () {
    var b, f; b = this.jQuery || window.jQuery; f = b(window); b.fn.stick_in_parent = function (d) {
        var A, w, J, n, B, K, p, q, k, E, t; null == d && (d = {}); t = d.sticky_class; B = d.inner_scrolling; E = d.recalc_every; k = d.parent; q = d.offset_top; p = d.spacer; w = d.bottoming; null == q && (q = 0); null == k && (k = void 0); null == B && (B = !0); null == t && (t = "is_stuck"); A = b(document); null == w && (w = !0); J = function (a, d, n, C, F, u, r, G) {
            var v, H, m, D, I, c, g, x, y, z, h, l; if (!a.data("sticky_kit")) {
                a.data("sticky_kit", !0); I = A.height(); g = a.parent(); null != k && (g = g.closest(k));
                if (!g.length) throw "failed to find stick parent"; v = m = !1; (h = null != p ? p && a.closest(p) : b("<div />")) && h.css("position", a.css("position")); x = function () {
                    var c, f, e; if (!G && (I = A.height(), c = parseInt(g.css("border-top-width"), 10), f = parseInt(g.css("padding-top"), 10), d = parseInt(g.css("padding-bottom"), 10), n = g.offset().top + c + f, C = g.height(), m && (v = m = !1, null == p && (a.insertAfter(h), h.detach()), a.css({ position: "", top: "", width: "", bottom: "" }).removeClass(t), e = !0), F = a.offset().top - (parseInt(a.css("margin-top"), 10) || 0) - q,
                    u = a.outerHeight(!0), r = a.css("float"), h && h.css({ width: a.outerWidth(!0), height: u, display: a.css("display"), "vertical-align": a.css("vertical-align"), "float": r }), e)) return l()
                }; x(); if (u !== C) return D = void 0, c = q, z = E, l = function () {
                    var b, l, e, k; if (!G && (e = !1, null != z && (--z, 0 >= z && (z = E, x(), e = !0)), e || A.height() === I || x(), e = f.scrollTop(), null != D && (l = e - D), D = e, m ? (w && (k = e + u + c > C + n, v && !k && (v = !1, a.css({ position: "fixed", bottom: "", top: c }).trigger("sticky_kit:unbottom"))), e < F && (m = !1, c = q, null == p && ("left" !== r && "right" !== r || a.insertAfter(h),
                    h.detach()), b = { position: "", width: "", top: "" }, a.css(b).removeClass(t).trigger("sticky_kit:unstick")), B && (b = f.height(), u + q > b && !v && (c -= l, c = Math.max(b - u, c), c = Math.min(q, c), m && a.css({ top: c + "px" })))) : e > F && (m = !0, b = { position: "fixed", top: c }, b.width = "border-box" === a.css("box-sizing") ? a.outerWidth() + "px" : a.width() + "px", a.css(b).addClass(t), null == p && (a.after(h), "left" !== r && "right" !== r || h.append(a)), a.trigger("sticky_kit:stick")), m && w && (null == k && (k = e + u + c > C + n), !v && k))) return v = !0, "static" === g.css("position") && g.css({ position: "relative" }),
                    a.css({ position: "absolute", bottom: d, top: "auto" }).trigger("sticky_kit:bottom")
                }, y = function () { x(); return l() }, H = function () { G = !0; f.off("touchmove", l); f.off("scroll", l); f.off("resize", y); b(document.body).off("sticky_kit:recalc", y); a.off("sticky_kit:detach", H); a.removeData("sticky_kit"); a.css({ position: "", bottom: "", top: "", width: "" }); g.position("position", ""); if (m) return null == p && ("left" !== r && "right" !== r || a.insertAfter(h), h.remove()), a.removeClass(t) }, f.on("touchmove", l), f.on("scroll", l), f.on("resize",
                y), b(document.body).on("sticky_kit:recalc", y), a.on("sticky_kit:detach", H), setTimeout(l, 0)
            }
        }; n = 0; for (K = this.length; n < K; n++) d = this[n], J(b(d)); return this
    }
}).call(this);
//---------------------------------------------------------------------------------


$('#listing-left-column').stick_in_parent();

var isCityFilterFilled = false;
var budgetValue = [0, 10000, 20000, 35000, 50000, 80000, 125000, 200000];
var bikesList = $("#filter-bike-list");
var citiesList = $("#filter-type-city select option");
var listingStartPoint = $('#listing-start-point'),
    spinnerBackground = $('#loader-bg-window'),
    bwSpinner = $('#ub-ajax-loader'),
    loaderColumn = $('#loader-right-column'),
    cityModelCarousel = $('#city-model-used-carousel');
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
        console.warn("Unable to get query string : " + e.message);
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

        ko.utils.registerEventHandler(element, "slidestop", function (event, ui) {
            vwUsedBikes.GetUsedBikes();
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


    self.OnInit = ko.observable(true);
    self.noBikes = ko.observable(OnInitTotalBikes<=0);
    self.TotalBikes = ko.observable(OnInitTotalBikes);
    self.BikeDetails = ko.observableArray();
    self.PageUrl = ko.observable();
    self.PreviousQS = ko.observable("");
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
     try {
            var ele = $(e.target).find("option:selected");
            var cityMaskingName = ele.data("citymasking") + "/", arrLocation = window.location.pathname.split("bikes-in-");
            arrLocation[arrLocation.length - 1] = cityMaskingName;
            var redirectUrl = arrLocation.join("bikes-in-");
            window.location.hash = window.location.hash.replace(/city=[0-9]+/i, ("city=" + ele.data("cityid")));
            window.location.pathname = redirectUrl;
        } catch (e) {
            console.log("Error in city change : " + e);
        }
    };
    self.ApplyBikeFilter = function (d, e) {
        $("#city-model-used-carousel").hide();
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
            self.GetUsedBikes();
        } catch (e) {
            console.warn("Unable to set apply bikes filter : " + e.message);
        }
    };

    self.ResetBikeFilters = function () {
        bikesList.find(".accordion-tab").removeClass("tab-checked");
        bikesList.find('.accordion-tab.active').removeClass('active');
        bikesList.find(".accordion-count").empty();
        $('.bike-model-list-content').hide();
        $('.bike-model-list li').removeClass("active");
        self.ApplyBikeFilter();
    };

    self.ApplyMakeFilter = function (d, e) {
        var tab = $(e.currentTarget).closest('.accordion-tab');

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
        self.ApplyBikeFilter(d, e);
    };

    self.ApplyModelFilter = function (d, e) {
        var item = $(e.currentTarget);

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
        self.ApplyBikeFilter(d, e);
    };

    self.SelectedCity = ko.observable({ "id": 0, "name": "All India" });
    self.BudgetValues = ko.observable();
    self.ShowBudgetRange = ko.computed(function (d, e) {

        if (self.BudgetValues()) {
            var minBuget = self.BudgetValues()[0], maxBuget = self.BudgetValues()[1];
            if (minBuget == 0 && maxBuget == 7) {
                $("#budget-amount").html('<span class="bwsprite inr-xxsm-icon"></span>0 - <span class="bwsprite inr-xxsm-icon"></span>' + formatPrice(budgetValue[maxBuget]) + ((maxBuget == 7) ? '+' : ''));
            }
            else {
                $("#budget-amount").html('<span class="bwsprite inr-xxsm-icon"></span>' + formatPrice(budgetValue[minBuget]) + ' - <span class="bwsprite inr-xxsm-icon"></span>' + formatPrice(budgetValue[maxBuget]) + ((maxBuget == 7) ? '+' : ''));
            }
        }
    });
    self.KmsDriven = ko.observable();
    self.BikeAge = ko.observable();

    self.BikeAge.subscribe(function (value) {
        if (self.BikeAge() != 8) {
            self.Filters()["age"] = self.BikeAge();
            filters.bikeAgeAmount(self.BikeAge());
            filters.selection.set.slider('bike-age-amount');
        } else {
            self.Filters()["age"] = "";
        }

    });

    self.KmsDriven.subscribe(function (value) {
        if (self.KmsDriven() != 200000) {
            self.Filters()["kms"] = self.KmsDriven();
            filters.kilometerAmount(self.KmsDriven());
            filters.selection.set.slider('kms-amount');
        } else self.Filters()["kms"] = "";

    });

    self.BudgetValues.subscribe(function (value) {
        var minBuget = self.BudgetValues()[0], maxBuget = self.BudgetValues()[1];
        self.Filters()["budget"] = budgetValue[minBuget];
        if (minBuget != 0 || maxBuget != 7) {
            self.Filters()["budget"] += "+" + budgetValue[maxBuget];
            filters.budgetAmount(self.BudgetValues());
            filters.selection.set.slider('budget-amount');
        }
    });

    self.FilterOwners = function (d, e) {

        var item = $(e.currentTarget);

        if (!item.hasClass('active')) {
            item.addClass('active');
            filters.selection.set.owner(item);
        }
        else {
            item.removeClass('active');
            filters.selection.reset.owner(item);
        }

        var owners = $(item).parent().find("li.active"), ownerList = "";
        owners.each(function () {
            ownerList += "+" + $(this).attr("data-ownerid");
        });

        self.Filters()["owner"] = ownerList.substr(1);
        self.GetUsedBikes();
    };
    self.FilterSellers = function (d, e) {
        var item = $(e.currentTarget);

        if (!item.hasClass('checked')) {
            item.addClass('checked');
            filters.selection.set.seller(item);
        }
        else {
            item.removeClass('checked');
            filters.selection.reset.seller(item);
        }

        var sellers = $(item).parent().find("li.checked"), sellerList = "";
        sellers.each(function () {
            sellerList += "+" + $(this).attr("data-sellerid");
        });

        self.Filters()["st"] = sellerList.substr(1);
        self.GetUsedBikes();
    };

    self.ResetFilters = function () {
        var so = self.Filters()["so"];
        var ct = self.Filters()["city"];
        self.Filters(new Object());
        self.Filters()["so"] = so;
        self.Filters()["city"] = ct;
        self.SetDefaultFilters();
        self.GetUsedBikes();
    };

    self.SetDefaultFilters = function () {
        try {
            if (!self.Filters()["kms"])
                self.KmsDriven(200000);
            if (!self.Filters()["age"])
                self.BikeAge(8);
            if (!self.Filters()["budget"])
                self.BudgetValues([0, 7]);
            if (!self.Filters()["pn"])
                self.CurPageNo(1);
            self.ApplyPagination();

            $("#previous-owners-list li").removeClass("active");
            $("#seller-type-list li").removeClass("checked");

            $("#sort-listing li").first().addClass("selected").siblings().removeClass("selected");
            $('#sort-by-container .sort-select-btn').text($("#sort-listing li.selected").text());


            var checkedTabs = $('#filter-type-bike').find('.accordion-tab');
            checkedTabs.each(function () {
                $(this).find('.accordion-count').text('');
                $(this).removeClass('tab-checked active');
                $(this).closest('li').find('.bike-model-list li').removeClass('active');
            });
            $('#bike, #owners, #seller, .type-slider').empty();

        } catch (e) {
            console.warn("Unable to set default records : " + e.message);
        }
    };

    self.ApplySort = function (d, e) {

        var item = $(e.currentTarget);

        if (!item.hasClass('selected')) {
            $('#sort-listing li.selected').removeClass('selected');
            item.addClass('selected');
            sortBy.selection(item);
            var so = $('#sort-listing li.selected').attr("data-sortorder");
            self.Filters()["so"] = so;
            self.Filters()["pn"] = "";
            self.GetUsedBikes();
        }
        else {
            sortBy.close();
        }


    };

    self.PagesListHtml = ko.observable("");
    self.PrevPageHtml = ko.observable("");
    self.NextPageHtml = ko.observable("");

    self.ApplyPagination = function (d, e) {
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
                    prevpg = "<a  data-bind='click : ChangePageNumber' data-pagenum='" + self.Pagination().previous() + "' href='" + qs.replace(rstr, "page-" + self.Pagination().previous()) + "' class='bwsprite prev-page-icon'/>";
                } else {
                    prevpg = "<a href='javascript:void(0)' class='bwsprite prev-page-icon'/>";
                }
                self.PrevPageHtml(prevpg);
                if (self.Pagination().hasNext()) {
                    nextpg = "<a  data-bind='click : ChangePageNumber' data-pagenum='" + self.Pagination().next() + "' href='" + qs.replace(rstr, "page-" + self.Pagination().next()) + "' class='bwsprite next-page-icon'/>";
                } else {
                    nextpg = "<a href='javascript:void(0)' class='bwsprite next-page-icon'/>";
                }
                self.NextPageHtml(nextpg);
                $("#pagination-list li[data-pagenum=" + self.Pagination().pageNumber() + "]").addClass("active");
            }
        } catch (e) {
            console.warn("Unable to apply pagination. : " + e.message);
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
                $('body').addClass('loader-active');
                spinnerBackground.show();
                loaderColumn.show();
                bwSpinner.show();
                self.PreviousQS(qs);
                $.ajax({
                    type: 'GET',
                    url: '/api/used/search/?bikes=1&' + qs.replace(/[\+]/g, "%2B") + ((selectedCityId > 0 && qs.indexOf("city")==-1) ? "&city=" + selectedCityId : ""),
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
                        if (cityModelCarousel && cityModelCarousel.offset()) {
                            if (!$('body').hasClass('city-model-carousel-inactive')) {
                                $('html, body').scrollTop(cityModelCarousel.offset().top - 50);
                            }
                            else {
                                $('html, body').scrollTop(listingStartPoint.offset().top - 50);
                            }
                        }
                        else {
                            $('html, body').scrollTop(listingStartPoint.offset().top - 50);
                        }

                        self.noBikes(self.TotalBikes() <= 0);
                        self.OnInit(false);
                        self.IsReset(false);
                        self.ApplyPagination();
                        $('body').removeClass('loader-active');
                        spinnerBackground.hide();
                        loaderColumn.hide();
                        bwSpinner.hide();
                    }
                });
            }

            if (loaderColumn.next().text().trim() == "") loaderColumn.next().hide();

        } catch (e) {
            console.warn("Unable to set fetch used bike records : " + e.message);
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
        } catch (e) {
            console.warn("Unable to change page number : " + e.message);
        }
        return false;
    };

    self.SetPageFilters = function (d, e) {

        try {

            if (self.Filters()["so"]) {
                var item = $("#sort-listing li[data-sortorder=" + self.Filters()["so"] + "]");
                item.addClass("selected").siblings().removeClass("selected");
                sortBy.selection(item);
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
                    var element = $("#previous-owners-list li[data-ownerid=" + val + "]");
                    element.addClass("active");
                    filters.selection.set.owner(element);
                });
            }

            if (self.Filters()["st"]) {
                var arr = self.Filters()["st"].split("+");
                $.each(arr, function (i, val) {
                    var element = $("#seller-type-list li[data-sellerid=" + val + "]");
                    element.addClass("checked");
                    filters.selection.set.seller(element);
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
                    var ele = bikesList.find("ul.bike-model-list span[data-modelid=" + val + "]"),
                        tab = ele.closest("li"),
                        selectedBikeFilters = $('#bike');
                    ele.closest("li").addClass("active");
                    accordion.setCount(ele);
                    selectedBikeFilters.append('<p data-id="md-' + ele.attr("data-modelid") + '" data-type="model">' + tab.find('.category-label').text() + '<span class="bwsprite cross-icon"></span></p>');
                });
            }

            if (self.Filters()["make"]) {
                var arr = self.Filters()["make"].split("+"),
                    selectedBikeFilters = $('#bike');

                $.each(arr, function (i, val) {
                    var ele = bikesList.find("span[data-makeid=" + val + "]"),
                        tab = ele.closest(".accordion-tab"),
                        selectedBikeFilters = $('#bike'),
                        selectedMakeLength = selectedBikeFilters.find("p[data-id='mk-" + selectedMakeId + "'").length;

                    tab.addClass("tab-checked").find(".accordion-count").text('(All models)');
                    tab.closest("li").find(".bike-model-list li").addClass("active");

                    if (selectedMakeLength == 0) {
                        selectedBikeFilters.append('<p data-id="mk-' + ele.attr("data-makeid") + '" data-type="make">' + tab.find('.category-label').text() + '<span class="bwsprite cross-icon"></span></p>');
                    }
                });
            }

            self.GetUsedBikes(self.Filters()["pn"] != "" ? e : null);

        } catch (e) {
            console.warn("Unable to set page filters : " + e.message);
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
        var ele = bikesList.find("span[data-makeid=" + selectedMakeId + "]"),
            selectedBikeFilters = $('#bike'),
            tab = ele.closest(".accordion-tab"),
            selectedMakeLength = selectedBikeFilters.find("p[data-id='mk-" + selectedMakeId + "'").length;

        tab.addClass("tab-checked").find(".accordion-count").text('(All models)');
        tab.closest("li").find(".bike-model-list li").addClass("active");

        if (selectedMakeLength == 0) {
            selectedBikeFilters.append('<p data-id="mk-' + ele.attr("data-makeid") + '" data-type="make">' + tab.find('.category-label').text() + '<span class="bwsprite cross-icon"></span></p>');
        }

        var mkIds = (vwUsedBikes.Filters()["make"]) ? vwUsedBikes.Filters()["make"].split("+") : null;

        if (mkIds != null && mkIds.length > 0) {
            if ($.inArray(selectedMakeId, mkIds) == -1)
                vwUsedBikes.Filters()["make"] += "+" + selectedMakeId;
        }
        else vwUsedBikes.Filters()["make"] = selectedMakeId;
    }

    if (selectedCityId && selectedCityId != "0") {

        var ele = $("#filter-type-city select  option[data-cityid=" + selectedCityId + "]");
        ele.addClass("active").siblings().removeClass("active");
        vwUsedBikes.SelectedCity({ "id": ele.attr("data-cityid"), "name": ele.text() });
        $('#filter-type-city .chosen-single span').text(ele.text());
        $('#ddlCity option[value="' + selectedCityId + '"]').prop('selected', true);
        $("#ddlCity").trigger("chosen:updated");
    }


    filters.set.bike();
    vwUsedBikes.SetPageFilters(null, event);

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


$('#reset-bikes-filter').on('click', function () {
    accordion.resetAll();
    filters.set.clearButton();
    $('#bike').empty();
});

/* bike filters */

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
            vwUsedBikes.BudgetValues([0, 7]);
            break;

        case 'kms-amount':
            vwUsedBikes.KmsDriven(200000);
            break;

        case 'bike-age-amount':
            vwUsedBikes.BikeAge(8);
            break;

        default:
            break;
    }

    vwUsedBikes.GetUsedBikes();
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
        var budgetminValue = budgetValue[units[0]],
            budgetmaxValue = budgetValue[units[1]];

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

        bike: function () {
            var inputBoxes = $('.getModelInput');

            inputBoxes.each(function () {
                var filterList = $(this).closest('.bike-model-list-content').find('.bike-model-list');
                $(this).fastLiveFilter(filterList);
            });
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

        city: function () {
            $('#filter-type-city .selected-filters').text('All India');
        },

        bike: function () {
            filterTypeBike.find('.selected-filters').text('All Bikes');
        },

        budget: function () {
            $('#budget-range-slider').slider('option', 'values', [0, 7]);
            $('#budget-amount').html('<span class="bwsprite inr-xxsm-icon"></span>0 - <span class="bwsprite inr-xxsm-icon"></span>2,00,000+');
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
            $('#seller-type-list li.checked').removeClass('checked');
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
                var amount = $('#' + content).html(),
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
                var itemID = item.attr('data-sellerid'),
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
                var itemID = item.attr('data-id');

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

                $('#seller-type-list').find('li[data-sellerid="' + itemID + '"]').trigger("click");
                filters.selection.reset.seller(item);
            }
        }
    }
};

/* fastLiveFilter jQuery plugin 1.0.3 */
jQuery.fn.fastLiveFilter = function (list, options) {
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

    input.change(function () {
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
    }).keydown(function () {
        clearTimeout(keyTimeout);
        keyTimeout = setTimeout(function () {
            if (input.val() === lastFilter) return;
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

$('#close-city-model-carousel').on('click', function () {
    $('body').addClass('city-model-carousel-inactive');
    $('#city-model-used-carousel').slideUp();
    SetUsedCookie();
});

function SetUsedCookie() {
    try {
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
    } catch (e) {
        console.log(e);
    }
}