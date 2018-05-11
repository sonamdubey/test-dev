var $sortDiv = $("#sort-by-div"),
    applyFilter = $('#btnApplyFilters'),
    mileage = $('.mileage'),
    CheckBoxFilter = $('.multiSelect .unchecked'),
    multiSelect = $('.multiSelect'),
    nobikediv = $('#nobike'),
    loading = $('#loading'),
    resetButton = $('#btnReset');

var sortEnum = {};

var $window = $(window);
$.effect = 'slide';
$.options = { direction: 'right' };
$.duration = 500;
$.so = '';
$.sc = '';

docReady(function () {
    //Other  functions
    newBikeSearch = function () {
        var self = this;
        self.IsInitialized = ko.observable(false);
        self.IsLoading = ko.observable(false);
        self.searchResult = ko.observableArray([]);
        self.Filters = ko.observable({ pageno: 1, pagesize: 30 });
        self.PreviousQS = ko.observable("");
        self.IsReset = ko.observable(false);
        self.LoadMoreTarget = ko.observable();
        self.IsLoadMore = ko.observable(false);
        self.NewPageNo = ko.observable(0);
        self.IsMoreBikesAvailable = ko.observable(false);
        self.FirstLoad = ko.observable(true);
        self.models = ko.observable([]);
        self.TotalBikes = ko.observable();
        self.noBikes = ko.observable(self.TotalBikes() == 0);
        self.curPageNo = ko.observable();
        self.Filters()['budget'] = $('#min-max-budget').val();
        self.init = function (e) {
            try {
                if (!self.IsInitialized()) {
                    self.IsLoading(true);
                    self.TotalBikes(1); //handle container for loader
                    if (self.FirstLoad()) {
                        var bikeSearchResult = JSON.parse(Base64.decode($('#pageLoadData').val()));
                        self.models(bikeSearchResult.searchResult);
                        self.LoadMoreTarget(bikeSearchResult.pageUrl.nextUrl);
                        if (self.LoadMoreTarget()) {
                            self.IsMoreBikesAvailable(true);
                        }
                    }
                    self.FirstLoad(false);
                    var eleSection = $(".bike-search");
                    ko.applyBindings(self, eleSection[0]);
                    self.IsInitialized(true);
                }
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.setFilters = function (url) {
            try {
                var params = url.split('&');
                for (var index in params) {
                    var pair = params[index].split('=');
                    self.Filters()[pair[0]] = pair[1];
                }
                self.FirstLoad(false);
                self.getBikeSearchResult('through-filters');
                self.init();
                self.Filters()['pageno'] = 1;
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.pushGTACode = function (noOfRecords, filterName) {
            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Search_Page', 'act': 'Filter_Select_' + noOfRecords, 'lab': filterName });
        };

        self.pushGACode = function (noOfRecords) {
            try {
                for (var param in self.Filters()) {
                    if (param.length > 0) {
                        if (param != "pageno" && param != "so" && param != "sc" && param != "budget") {
                            self.pushGTACode(noOfRecords, param);
                        } else if (param == "sc") {
                            var sc = self.getFilterFromQS('sc'), so = self.getFilterFromQS('so');

                            var filterName = "";
                            switch (sc) {
                                case '0':
                                    filterName = "Popular";
                                    break;
                                case '1':
                                    filterName = so == '0' ? "Price :Low to High" : "Price :High to Low";
                                    break;
                                case '2':
                                    filterName = 'Mileage :High to Low';
                                    break;
                            }
                            self.pushGTACode(noOfRecords, filterName);
                        } else if (param == "budget") {
                            var budget = self.Filters()['budget'].split('-');
                            if (!(budget[0] == '0' && budget[1] == '6000000'))
                                self.pushGTACode(noOfRecords, filterName);
                        }

                    }
                }
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.resetFilterUI = function (resetParam) {
            try {
                for (var param in self.Filters()) {
                    if (param) {
                        var node = $('div[name=' + param + ']');
                        if (param == 'bike' || param == 'displacement' || param == 'ridestyle' || param == 'braketype' || param == 'alloywheel' || param == 'starttype') {
                            node.prev().find('.hida').removeClass('hide');
                            node.prev().find('.multiSel').html('');
                            node.find('li').each(function () {
                                $(this).removeClass('checked');
                            });
                        } else if (param == 'ABS') {
                            node.children().each(function () {
                                $(this).removeClass('optionSelected');
                            });
                        } else if (param == 'budget') {
                            self.setSliderRangeQS($('#mSlider-range'), 1, 20);
                            $("#rangeAmount").html('<span class="bw-m-sprite rupee"></span> 0 -' + ' ' + '<span class="bw-m-sprite rupee"></span> Any value');
                        } else if (param == 'mileage') {
                            node.each(function () {
                                $(this).find('span').removeClass('optionSelected');
                            });
                        }

                        if (param !== 'pageno' && param !== 'pagesize' && resetParam) {
                            delete self.Filters()[param];
                        }
                    }
                }
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.findNearest = function (includeLeft, includeRight, value) {
            var nearest = null;
            var diff = null;
            for (var i = 0; i < values.length; i++) {
                if ((includeLeft && values[i] <= value) || (includeRight && values[i] >= value)) {
                    var newDiff = Math.abs(value - values[i]);
                    if (diff == null || newDiff < diff) {
                        nearest = values[i];
                        diff = newDiff;
                    }
                }
            }
            return nearest;
        };

        self.valueFormatter = function (num) {
            try {
                if (num >= 100000) {
                    return (num / 100000).toFixed(1).replace(/\.0$/, '') + 'L';
                }
                if (num >= 1000) {
                    return (num / 1000).toFixed(1).replace(/\.0$/, '') + 'K';
                }
                return num;
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.scrollToTop = function () {
            try {
                $('body,html').animate({
                    scrollTop: 0
                }, 800);
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.bindNextSearchResult = function (json) {
            try {
                if (json.searchResult.length > 0) {
                    $.each(json.searchResult, function (index, val) {
                        self.searchResult.push(val);
                    });
                }
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.setSliderRangeQS = function (element, start, end) {
            try {
                element.slider("values", 0, start);
                if (end != '')
                    element.slider("values", 1, end);
            } catch (e) {
                console.warn(e.message);
            }

        };

        self.getSliderValue = function (budgetValue) {
            try {
                for (var i = 0; i < trueValues.length; i++)
                    if (trueValues[i] == budgetValue)
                        return values[i];
            } catch (e) {
                console.warn(e.message);
            }

        };

        self.selectFiltersPresentInQS = function () {
            try {
                self.resetFilterUI(false);
                for (var param in self.Filters()) {
                    if (param.length > 0) {
                        var node = $('div[name=' + param + ']');
                        if ((param == 'bike' || param == 'displacement' || param == 'ridestyle' || param == 'braketype' || param == 'alloywheel' || param == 'starttype') && self.Filters()[param]) {
                            var values = self.Filters()[param].split('+');
                            var html = '';
                            for (var j = 0; j < values.length; j++) {
                                node.find('li[filterid=' + values[j] + ']').addClass('checked');
                                var title = node.find('li[filterid=' + values[j] + ']').text() + ',';
                                html += '<span data-title="' + title + '">' + title + '</span>';
                            }

                            node.prev().find('.hida').addClass('hide');
                            node.prev().find('.multiSel').html(html);

                        } else if ((param == 'ABS') && self.Filters()[param]) {

                            node.find('span[filterid=' + self.Filters()[param] + ']').addClass('optionSelected');

                        } else if (param == 'budget' && self.Filters()[param]) {
                            values = self.Filters()[param].split('-');
                            values[0] = (values[0] == '0' ? '30000' : values[0]);
                            values[1] = values[1] == '' ? '6000000' : values[1];
                            var minValue = self.getSliderValue(values[0]), maxValue = self.getSliderValue(values[1])

                            self.setSliderRangeQS($('#mSlider-range'), minValue, maxValue);

                            var budgetminValue = self.valueFormatter(values[0]);
                            var budgetmaxValue = self.valueFormatter(values[1]);

                            if (values[0] == 1 && values[1] == 20) {
                                $("#rangeAmount").html('<span class="bw-m-sprite rupee"></span> 0 -' + ' ' + '<span class="bw-m-sprite rupee"></span> Any value');
                            } else {
                                $("#rangeAmount").html('<span class="bw-m-sprite rupee"></span>' + ' ' + budgetminValue + ' ' + '-' + ' ' + '<span class="bw-m-sprite rupee"></span>' + ' ' + budgetmaxValue);
                            }
                        } else if (param == 'mileage' && self.Filters()[param]) {
                            values = self.Filters()[param].split('+');

                            for (j = 0; j < values.length; j++) {
                                node.find('span[filterid=' + values[j] + ']').addClass('optionSelected');
                            }
                        }
                    }
                }
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.getBikeSearchResult = function (filterName) {
            try {
                var qs = "";
                if (self.IsLoadMore()) {
                    qs = self.LoadMoreTarget().split('?')[1].toLowerCase();
                } else {
                    for (var param in self.Filters()) {
                        if (self.Filters()[param]) {
                            qs += "&" + param + "=" + self.Filters()[param];
                        }
                    }
                    qs = qs.substr(1);
                }

                if (self.PreviousQS() != qs) {
                    if (!self.IsLoadMore()) {
                        self.models([]);
                    }

                    if (self.noBikes()) {
                        self.TotalBikes(1); // to show bikes container
                        self.noBikes(false);
                    }
                    self.IsLoading(true);
                    self.PreviousQS(qs);
                    var apiUrl = '/api/NewBikeSearch/?' + qs;
                    $.getJSON(apiUrl)
                        .done(function (response) {
                            if (self.IsLoadMore()) {
                                self.bindNextSearchResult(response);
                                self.NewPageNo(self.NewPageNo() + 1);
                            } else {
                                self.models(response.searchResult);
                                self.NewPageNo(0);
                                self.searchResult([]);
                            }
                            self.TotalBikes(response.totalCount);
                            $('#bikecount').text(self.TotalBikes() + ' Bikes');
                            self.noBikes(false);
                            self.LoadMoreTarget(response.pageUrl.nextUrl);
                            if (response.pageUrl.nextUrl) {
                                self.IsMoreBikesAvailable(true);
                            } else {
                                self.IsMoreBikesAvailable(false);
                            }
                            self.pushGACode(self.TotalBikes());
                        })
                        .fail(function () {
                            self.IsMoreBikesAvailable(false);
                            self.noBikes(true);
                            self.TotalBikes(0);
                            self.searchResult([]);
                            self.models([]);
                            $('#bikecount').text('No bikes found');
                            $('#nobikeresults').show();
                            self.pushGACode(self.TotalBikes());
                        })
                        .always(function () {
                            $('#hidePopup').click();
                            window.location.hash = qs;
                            self.IsLoading(false);
                            self.IsLoadMore(false);
                        });
                } else {
                    $('#hidePopup').click();
                }
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.selectedValueSortTab = function () {
            try {
                var node = $('#sort-by-div');
                if (self.Filters()['so'].length > 0 && self.Filters()['sc'].length > 0) {
                    node.find('div[sc="' + self.Filters()['sc'] + '"] a').addClass('text-bold');

                    if (node.find('a[data-title="sort"]').hasClass('price-sort')) {
                        node.find('div[sc="' + self.Filters()['sc'] + '"]').parent().attr('so', self.Filters()['so']);

                        if (self.Filters()['so'].length > 0) {
                            var sortedText = $('.price-sort').find('span');
                            sortedText.css('display', 'inline-block');
                            sortedText.text(self.Filters()['so'] == '1' ? ": High" : ": Low");
                        }
                    }
                }
                else {
                    node.find('div[sc=""] a').addClass('text-bold');
                }
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.removeValueFromCheckBoxInQS = function (name, value) {
            try {
                var values = self.Filters()[name].split('+');
                var temp = '';
                for (var i = 0; i < values.length; i++) {
                    if (values[i] != value) {
                        if (temp.length > 0)
                            temp = temp + "+" + values[i];
                        else
                            temp = values[i];
                    }
                }
                self.Filters()[name] = temp;
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.getFilterFromQS = function (name) {
            try {
                if (self.Filters()[name]) {
                    return self.Filters()[name]
                } else {
                    return "";
                }
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.updateCheckBoxFilterInQS = function (name, value, toAdd) {
            try {
                if (toAdd == true) {
                    self.Filters()[name] = self.Filters()[name] + '+' + value;
                }
                else {
                    self.removeValueFromCheckBoxInQS(name, value);
                }
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.applyCheckBoxFilter = function () {
            try {
                var selected = 'checked';

                multiSelect.each(function () {
                    var curCheckboxList = $(this),
                        name = curCheckboxList.attr('name'),
                        value = '';
                    if (curCheckboxList) {
                        curCheckboxList.find('ul li').each(function () {
                            if ($(this).hasClass(selected)) {
                                var filterId = $(this).attr('filterid');
                                value += filterId + '+';
                            }
                        });
                        if (value.length > 1) {
                            value = value.substring(0, value.length - 1);
                            self.Filters()[name] = value;
                        } else {
                            delete self.Filters()[name];
                        }
                    }
                });
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.applyMileageFilter = function () {
            try {
                var selected = 'optionSelected',
                    name = "",
                    value = "";
                mileage.each(function () {
                    name = $(this).parent().parent().attr('name');
                    if ($(this).hasClass(selected)) {
                        var filterId = $(this).attr('filterid');
                        value += filterId + '+';
                    }
                });
                if (value.length > 1) {
                    value = value.substring(0, value.length - 1);
                    self.Filters()[name] = value;
                } else {
                    delete self.Filters()[name];
                }
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.getRealValue = function (sliderValue) {
            try {
                for (var i = 0; i < values.length; i++) {
                    if (values[i] >= sliderValue) {
                        return trueValues[i];
                    }
                }
                return 0;
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.applySliderFilter = function (element, name) {
            try {
                var minValue = self.getRealValue(element.slider('values', 0)) == '30000' ? 0 : self.getRealValue(element.slider('values', 0)),
                    maxValue = self.getRealValue(element.slider('values', 1));
                self.Filters()[name] = minValue + '-' + maxValue;
            } catch (e) {
                console.warn(e.message);
            }
        };

        self.applyToggelFilter = function () {
            try {
                var checked = 'optionSelected';
                $('.checkOption').each(function () {
                    if ($(this).hasClass(checked)) {
                        var name = $(this).parent().attr('name'),
                            value = $(this).attr('filterid');
                        self.Filters()[name] = value;
                    }
                    else {
                        delete self.Filters()[name];
                    }
                });
            } catch (e) {
                console.warn(e.message);
            }
        };
    };

    newBikeSearchVM = new newBikeSearch();


    //Sort by div popup
    $("#sort-btn").click(function () {
        $("#sort-by-div").slideToggle('fast');
        $("html, body").animate({ scrollTop: $("header").offset().top }, 0);
    });

    //init when first clicked
    $('#sort-by-div a[data-title="sort"]').click(function () {
        if (newBikeSearchVM && !newBikeSearchVM.IsInitialized()) {
            newBikeSearchVM.init(e);
        }
        newBikeSearchVM.scrollToTop();
        newBikeSearchVM.Filters()['so'] = '0';
        if ($(this).hasClass('price-sort')) {
            var sortOrder = $(this).attr('so');
            var sortedText = $('.price-sort').find('span');

            if (sortOrder == undefined || sortOrder == '0')
                newBikeSearchVM.Filters()['so'] = '0';
            else
                newBikeSearchVM.Filters()['so'] = '1';

            $(this).attr('so', newBikeSearchVM.Filters()['so']);

            if (sortOrder != undefined) {
                if (sortedText.text() === ': Low')
                    newBikeSearchVM.Filters()['so'] = '1';
                else
                    newBikeSearchVM.Filters()['so'] = '0';
            }

            if (newBikeSearchVM.Filters()['so'].length > 0) {
                sortedText.css('display', 'inline-block');
                sortedText.text(newBikeSearchVM.Filters()['so'] == '1' ? ": High" : ": Low");
            }
        }

        $('#sort-by-div a[data-title="sort"]').removeClass('text-bold');
        $(this).addClass('text-bold');
        $(this).parent().removeClass('text-bold');

        newBikeSearchVM.Filters()['sc'] = $(this).parent().attr('sc');

        if (newBikeSearchVM.Filters()['sc'] != '1') {
            $('.price-sort').find('span').text('');
        }
        
        newBikeSearchVM.getBikeSearchResult('sort-filter-applied');
    });

    //filter div popup
    $("#filter-btn").click(function () {
        $("#filter-div").show($.effect, $.options, $.duration, function () {
            //$.selected Filters
        });
        $(".popup-btn-filters").show(); //Filter btn 
        newBikeSearchVM.selectFiltersPresentInQS();
    });

    //Back button press
    $("#back-btn").click(function () {
        $("html,body").removeClass("lock-browser-scroll"); //bodyLock code
        $("#filter-div").hide($.effect, $.options, $.duration);
        $(".popup-btn-filters").hide(); //Filter btn 
        if (location.hash.substring(1) == 'back') {
            history.back();
        }

        isReset = false;
    });

    //Close button//
    $('#hidePopup').click(function () {
        var popupWindow = $(this).attr('popupname');

        if (popupWindow == "filterpopup") {
            //        setFilters();
            $("#back-btn").click();
        }

        $('.popup-btn-submit').hide(); // for hiding the button submit when model selection pop up 
        $("#main-container").show();
    });

    var checkedLen, controlWidth, hidaWidth, remainSpace, multiselWidth;

    $(window).resize(function () {
        $('.dropdown').each(function () {
            var $dropDown = $(this);
            controlWidth = $dropDown.find('.form-control').width();
            hidaWidth = $dropDown.find('.hida').width();
            remainSpace = controlWidth - hidaWidth - 10;
            multiselWidth = $dropDown.find('.multiSel').width();
            $dropDown.find('.multiSel').css('max-width', remainSpace + 'px');
        });
    });

    $window.scroll(function () {
        if ($(window).scrollTop() > 50) {
            $('#sort-by-div,header').addClass('fixed');
        } else {
            $('#sort-by-div,header').removeClass('fixed');
        }

        var winScroll = $window.scrollTop(),
            pageHeight = $(document).height(),
            windowHeight = $window.height(),
            footerHeight = $(".bg-footer").height();

        var position = pageHeight - (windowHeight + footerHeight + 200);
        if ($.lazyLoadingStatus) {
            if ($.nextPageUrl != '' && $.nextPageUrl != undefined) {
                if (winScroll >= position) {
                    $.getNextPageData();
                    $.lazyLoadingStatus = false;
                }
            }
        }
    });

    $(".dropdown .form-control").on('click', function () {
        var $ul = $(this).parent().find('ul');
        $(".dropdown ul").slideUp('fast');
        if ($ul.is(':visible')) {
            $ul.slideUp('fast');
        }
        else {
            $ul.slideDown('fast');
        }
    });

    $(document).bind('click', function (e) {
        var $clicked = $(e.target);
        if (!$clicked.parents().hasClass("dropdown")) $(".dropdown ul").hide();
    });

    CheckBoxFilter.on('click', function () {
        var $dropDown = $(this).closest('.dropdown');
        $(this).toggleClass('checked');
        var title = $(this).closest('.multiSelect').find('span').text(),
            title = $.trim($(this).text()) + ",";

        checkedLen = $dropDown.find('.multiSelect .unchecked.checked').length;
        controlWidth = $dropDown.find('.form-control').width();
        hidaWidth = $dropDown.find('.hida').width();
        remainSpace = controlWidth - hidaWidth - 10;
        multiselWidth;

        if ($(this).hasClass('checked')) {
            var html = '<span data-title="' + title + '">' + title + '</span>';

            $dropDown.find('.multiSel').append(html);
            $dropDown.find(".hida").addClass('hide');
            multiselWidth = $dropDown.find('.multiSel').width();
            if (checkedLen > 1 && multiselWidth > remainSpace) {
                $dropDown.find('.multiSel').css('max+-width', remainSpace + 'px');
            }
        }
        else {
            $dropDown.find('span[data-title="' + title + '"]').remove();
            if (checkedLen < 1) {
                $dropDown.find(".hida").removeClass('hide');
            }
            multiselWidth = $dropDown.find('.multiSel').width();
            if (multiselWidth < remainSpace) {
                $dropDown.find('.multiSel').css('max-width', 'none');
            }
        }
    });

    /* Mileage */
    mileage.click(function () {
        $(this).toggleClass('optionSelected');
    });

    $('.checkOption').click(function () {
        $(this).siblings().removeClass('optionSelected');
        $(this).toggleClass('optionSelected');
    });

    //back button function
    $(function () {
        var backFlag;
        var hash = location.hash.substring(1);
        if (hash != '' && hash == 'back' || hash != '' && hash == 'sellPopup') {
            history.replaceState(null, null, location.pathname + location.search);
        }
        $(window).bind('hashchange', function (e) {
            pagiFlag = false;
            hash = location.hash.substring(1);
            var filterPopname = $('div.filterBackArrow[popupname="filterpopup"]:visible');
            var popname = $('div.filterBackArrow[popupname!="filterpopup"]:visible');
            if (hash == 'back') {
                backFlag = false;

                if (location.hash.substring(1) == 'back' && !($(document).find('#filter-div,#sort-by-div').is(':visible'))) {
                    history.back();
                }
            }
            else if (backFlag == false && hash != 'back' && $(document).find('#filter-div,#sort-by-div').is(':visible')) {
                popname.trigger('click');
                filterPopname.trigger('click');
                backFlag = true;
            }
        });
    });

    $.fn.applyFilterOnButtonClick = function () {
        return $(this).click(function () {
            if (newBikeSearchVM && !newBikeSearchVM.IsInitialized()) {
                newBikeSearchVM.init(e);
            }
            newBikeSearchVM.scrollToTop();
            newBikeSearchVM.applyToggelFilter();
            newBikeSearchVM.applyMileageFilter();
            newBikeSearchVM.applyCheckBoxFilter();
            newBikeSearchVM.applySliderFilter($('#mSlider-range'), $('#mSlider-range').attr('name'));
            newBikeSearchVM.Filters()['sc'] = newBikeSearchVM.Filters()['sc'];
            newBikeSearchVM.Filters()['so'] = $.so;
            newBikeSearchVM.getBikeSearchResult('apply-filters');
        });
    }

    applyFilter.applyFilterOnButtonClick();

    var trueValues = [30000, 40000, 50000, 60000, 70000, 80000, 90000, 100000, 150000, 200000, 250000, 300000, 350000, 500000, 750000, 1000000, 1250000, 1500000, 3000000, 6000000];
    var values = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];
    $("#mSlider-range").slider({
        range: true,
        min: 1,
        max: 20,
        values: [1, 20],
        step: 1,
        slide: function (event, ui) {
            var includeLeft = event.keyCode != $.ui.keyCode.RIGHT;
            var includeRight = event.keyCode != $.ui.keyCode.LEFT;
            var value = newBikeSearchVM.findNearest(includeLeft, includeRight, ui.value);
            if (ui.value == ui.values[0]) {
                $(this).slider('values', 0, value);
            }
            else {
                $(this).slider('values', 1, value);
            }

            var budgetminValue = newBikeSearchVM.valueFormatter(newBikeSearchVM.getRealValue(ui.values[0])) == '30000' ? 0 : newBikeSearchVM.valueFormatter(newBikeSearchVM.getRealValue(ui.values[0]));
            var budgetmaxValue = newBikeSearchVM.valueFormatter(newBikeSearchVM.getRealValue(ui.values[1]));

            if (ui.values[0] == 0 && ui.values[1] == 20) {
                $("#rangeAmount").html('<span class="bw-m-sprite rupee"></span> 0 -' + ' ' + '<span class="bw-m-sprite rupee"></span> Any value');
            } else {
                $("#rangeAmount").html('<span class="bw-m-sprite rupee"></span>' + ' ' + budgetminValue + ' ' + '-' + ' ' + '<span class="bw-m-sprite rupee"></span>' + ' ' + budgetmaxValue);
            }
        }
    });

    newBikeSearchVM.setSliderRangeQS($('#mSlider-range'), 0, 20);

    //init when clicked
    $.fn.resetAll = function () {
        return $(this).click(function () {
            if (newBikeSearchVM && !newBikeSearchVM.IsInitialized()) {
                newBikeSearchVM.init(e);
            }
            newBikeSearchVM.resetFilterUI(true);
            newBikeSearchVM.scrollToTop();
            newBikeSearchVM.getBikeSearchResult('reset-all');
        });
    };

    resetButton.resetAll();

    $('#loadMoreBikes').click(function (e) {
        if (newBikeSearchVM && !newBikeSearchVM.IsInitialized()) {
            newBikeSearchVM.init(e);
        }
        newBikeSearchVM.LoadMoreTarget($("#loadMoreBikes").attr("data-url"));
        newBikeSearchVM.IsLoadMore(true);
        newBikeSearchVM.getBikeSearchResult('load-more');
    });

    if (window.location.hash) {
        var url = window.location.hash.replace('#', '');
        newBikeSearchVM.setFilters(url);
    }

    // Added By    : Rajan Chauhan on 21st Dec 2017
    // Description : For adding href on Load More button for crawler and stopping redirect
    $('#loadMoreBikes').click(function (event) {
        // For stopping redirection on load more button
        event.preventDefault();
    });
});