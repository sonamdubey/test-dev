var AdvantageSearch = {
    doc: $(document),
    env: 2,
    isAdvantageCity: false,
    pageSize: 24,
    cityId: 0,
    sortFilterDiv: '.sort-filter-div',
    sortFilterOffsetTop: $('.sort-filter-div').offset().top,
    scrollPosition: '',
    utils: {
        hideLoading: function () {
            $('#advLoading').hide();
            $('#loadingCarImg').hide();
        },
        showLoading: function () {
            $('#advLoading').show();
            $('#loadingCarImg').show();
        },
        registerEvents: function () {
            AdvantageSearch.doc.on('click', '#closeIconBanner', function () {
                $('.listingBanner').hide("slow");
            });
        }
    },
    listing: {
        getDeals: function (qs, pageNo) {
            AdvantageSearch.utils.showLoading();
            if (!pageNo)
                pageNo = 1;
            var apiUrl = "/api/advantage/getDeals/" + qs + "&pn=" + pageNo;
            $.ajax({
                type: 'GET',
                async: false,
                headers: { "sourceid": "1" },
                url: apiUrl,
                dataType: 'Json',
                success: function (response) {
                    var listingDetail = response;
                    AdvantageSearch.listing.bindResults(listingDetail);
                    if (listingDetail.deals.length > 0 && pageNo == 1) {
                        $('#noCarsFoundContainer').hide();
                        AdvantageSearch.pagination.bindPagination({ totalCount: listingDetail.totalCount, pageSize: AdvantageSearch.pageSize, activePage: 1, range: 5, prev: true, next: true, onchange: AdvantageSearch.pagination.callBack });
                        $('#pager').show();
                    }

                    else if (listingDetail.deals.length == 0) {
                        $('#pager').hide();
                        $('#noCarsFoundContainer').show();
                    }
                    else {
                        $('#noCarsFoundContainer').hide();
                        $('#pager').show();
                    }
                    //Back to top link code starts here
                    if (listingDetail.deals.length < 7 && AdvantageSearch.env == 2)
                        $('.backtotop-link').addClass('hide');
                    else if (listingDetail.deals.length < 3 && AdvantageSearch.env == 1)
                        $('.backtotop-link').addClass('hide');
                    else
                        $('.backtotop-link').removeClass('hide');
                    //Back to top link code ends here
                    $('html, body').animate({ scrollTop: $('#listingDetails').position().top }, 'slow');
                    if (listingDetail.offerOfTheWeek)
                        AdvantageSearch.offerOfWeek.updateOfferTime();
                    return listingDetail;
                },
                complete: function () {
                    $('#mainContent').show();
                    AdvantageSearch.utils.hideLoading();
                }
            });
            cwTracking.trackCustomData("AdvantageSearch", "filterapplied", "pf="+AdvantageSearch.env, true);
        },

        getDealMakes: function () {
            var Data = [];
            $.ajax({
                type: 'GET',
                async: false,
                url: "/api/deals/makemodel/?cityId=" + AdvantageSearch.filters.get.city(),
                dataType: 'Json',
                success: function (response) {
                    AdvantageSearch.filters.koViewModel.makes(response);
                }
            });
        },

        registerEvents: function () {
            ko.applyBindings(AdvantageSearch.listing.koViewModel, document.getElementById('dataDiv'));
            if (AdvantageSearch.env == 1)
                ko.applyBindings(AdvantageSearch.listing.koViewModel, document.getElementById('divChangeFilter'));
            AdvantageSearch.doc.on('click', '#resetFilters', function () {
                AdvantageSearch.filters.resetfilter();
            });
        },

        bindResults: function (json) {
            AdvantageSearch.listing.koViewModel.stocks(json.deals);
            AdvantageSearch.listing.koViewModel.visibleCarsCount(json.visibleCarsCount);
            AdvantageSearch.listing.koViewModel.totalCount(json.totalCount);
            AdvantageSearch.filters.koViewModel.makes(json.filterCount.makes);
            AdvantageSearch.filters.set.checkBoxFilter(AdvantageSearch.filters.selectors.make, AdvantageSearch.filters.manufacturers);
            AdvantageSearch.filters.appendMakes();
            AdvantageSearch.filters.set.checkBoxFilter(AdvantageSearch.filters.selectors.fuel, AdvantageSearch.filters.fuels);
            AdvantageSearch.filters.set.checkBoxFilter(AdvantageSearch.filters.selectors.transmission, AdvantageSearch.filters.transmissions);
            AdvantageSearch.filters.set.checkBoxFilter(AdvantageSearch.filters.selectors.bodyType, AdvantageSearch.filters.bodyTypes);

            AdvantageSearch.budget.set(AdvantageSearch.budget.budgetRange);
            AdvantageSearch.offerOfWeek.koViewModel.offerOfTheWeek(json.offerOfTheWeek);
        },
        koViewModel: {
            stocks: ko.observableArray([]),
            visibleCarsCount: ko.observable(),
            totalCount: ko.observable()
        },

        expandFilter: function () {
            return this.click(function () {
                var node = $(this);
                var nextNode = node.next();
                if (nextNode.hasClass('budgetContainerMain')) {
                    if (nextNode.is(':visible')) {
                        nextNode.removeClass('overflowVisible').slideUp();
                    }
                    else {
                        nextNode.slideDown(function () {
                            nextNode.addClass('overflowVisible');
                        });
                    }
                }
                else {
                    nextNode.slideToggle();
                }
                node.find('.fa-angle-up').toggleClass('transform');
                node.find('.fa-angle-down').toggleClass('transformy');
            });
        }
    },

    offerOfWeek: {
        koViewModel: {
            offerOfTheWeek: ko.observable()
        },

        offers: '',

        refreshInterval: function () { },

        updateOfferTime: function () {
            try {
                clearInterval(AdvantageSearch.offerOfWeek.refreshInterval);
                var date;
                if (AdvantageSearch.offerOfWeek.offers) {
                    var offers = AdvantageSearch.offerOfWeek.offers.split('|');
                    var currentTime = new Date();
                    offers.forEach(function (offer) {
                        var offerDetails = offer.split('-');
                        if (offerDetails[1] == $('#advDrpCity').val()) {
                            date = new Date(offerDetails[2]);
                        }
                    });
                }

                if (date != undefined) {
                    AdvantageSearch.offerOfWeek.refreshInterval = setInterval(function () {
                        var currentTime = new Date();
                        if (date > currentTime) {
                            var timeLeft = AdvantageSearch.offerOfWeek.msToTime(date - currentTime);
                            var node = $('#offerTime [data-days]');
                            node.text(timeLeft.days);
                            node.next().text("day" + (timeLeft.days == 1 ? '' : 's') + ' : ');
                            node = $('#offerTime [data-hours]');
                            node.text(timeLeft.hours);
                            node.next().text("hour" + (timeLeft.hours == 1 ? '' : 's') + ' : ');
                            node = $('#offerTime [data-mins]');
                            node.text(timeLeft.minutes);
                            node.next().text("min" + (timeLeft.minutes == 1 ? '' : 's') + ' : ');
                            node = $('#offerTime [data-secs]');
                            node.text(timeLeft.seconds);
                            node.next().text("sec" + (timeLeft.seconds == 1 ? '' : 's'));
                            return;
                        }
                    }, 1000)
                }
            }
            catch (ex) {
                $('#offerOfWeekBanner').addClass('hide');
                $('#AdvtIntroWrap').removeClass('hide');
            }
        },
        msToTime: function (duration) {
            var seconds = parseInt((duration / 1000) % 60)
                , minutes = parseInt((duration / (1000 * 60)) % 60)
                , hours = parseInt((duration / (1000 * 60 * 60)) % 24)
                , days = parseInt(duration / (1000 * 60 * 60 * 24));

            hours = (hours < 10) ? "0" + hours : hours;
            minutes = (minutes < 10) ? "0" + minutes : minutes;
            seconds = (seconds < 10) ? "0" + seconds : seconds;

            return { days: days, hours: hours, minutes: minutes, seconds: seconds };
        },
        registerEvents: function () {
            ko.applyBindings(AdvantageSearch.offerOfWeek.koViewModel, document.getElementById('offerOfWeek'));
        }
    },

    filters: {
        selectors: {
            make: 'li.makeLi',
            fuel: 'li.fuelLi',
            transmission: 'li.transmissionLi',
            bodyType: 'li.bodytypeLi'
        },
        manufacturers: '',
        fuels: '',
        transmissions: '',
        bodyTypes: '',
        get: {
            city: function () {
                return $("#advDrpCity").val();
            },

            checkBoxFilter: function (selector) {
                var idList = '';
                $(selector + ' .parentCheck.active').each(function (index) {
                    if (index != 0)
                        idList = idList + '+' + $(this).find('.filterText').data('filterid');
                    else
                        idList = $(this).find('.filterText').data('filterid');
                });
                return idList;
            }
        },

        set: {
            city: function () {
                $("#advDrpCity").val(AdvantageSearch.cityId)
            },

            checkBoxFilter: function (selector, values) {
                var valuesArray = values.toString().split('+');
                $(selector + ' .parentCheck').each(function (index) {
                    if (valuesArray.indexOf($(this).find('.filterText').data('filterid').toString()) > -1) {
                        $(this).addClass('active');
                    }
                    else {
                        $(this).removeClass('active');
                    }
                });
            },
            filterIcons: function (selector, values) {
                var valuesArray = values.toString().split('+');
                $(selector + ' .parentCheck').each(function (index) {
                    var node = $(this);
                    if (valuesArray.indexOf(node.find('.filterText').data('filterid').toString()) > -1) {
                        node.addClass('active');
                        node.find('.filterText').addClass('active-blue');
                    }
                    else {
                        node.removeClass('active');
                        node.find('.filterText').removeClass('active-blue');
                    }
                });
            }
        },
        koViewModel: {
            makes: ko.observableArray([]),

        },

        toggleSelection: function (element) {
            var checkBoxNode = element.find('.parentCheck');
            if (checkBoxNode.hasClass('active')) {
                checkBoxNode.removeClass('active');
            }
            else {
                checkBoxNode.addClass('active');
            }

        },

        openFilterPopup: function () {
            $('#filter-div').show('slide', { direction: 'right' }, 500);
            $('#btnApplyFilters').show('slide', { direction: 'right' }, 500, function () { lockPopup(); });
            window.history.pushState("filters", "", "");
        },

        closeFilterPopup: function () {
            unlockPopup();
            $('#filter-div').hide('slide', { direction: 'right' }, 500);
            $('#btnApplyFilters').hide('slide', { direction: 'right' }, 500);
            $('#carLabel').show();
            AdvantageSearch.filters.removeActiveMakes();
        },

        removeActiveMakes: function () {
            var allMakes = $("#makeListing li div");
            allMakes.removeClass('active');
        },

        openMakePopup: function () {
            $('#manfListDiv').show('slide', { direction: 'right' }, 500);
            $('#btnMakeSubmit').show('slide', { direction: 'right' }, 500);
            window.history.pushState("makes", "", "");
        },

        closeMakePopup: function () {
            $('#manfListDiv').hide('slide', { direction: 'right' }, 500);
            $('#btnMakeSubmit').hide('slide', { direction: 'right' }, 500);
            AdvantageSearch.filters.setMakesPopup();
        },

        appendMakes: function () {
            var carLabel = $('#carLabel');
            var makeListing = $('#makeListing li div.active');
            var selectedMakeName = '', makesLi = '';
            carLabel.empty();
            makeListing.each(function () {
                selectedMakeName = $(this).text();
                makesLi = '<li><span class="font14 carname nextmake">' + selectedMakeName + '</span> <span class="close-box fa fa-close cur-pointer"></span></li>';
                carLabel.append(makesLi);
            })
        },

        setMakesPopup: function () {
            var selectedMake = $('#carLabel li').text();
            var carLabels = [];
            $("#carLabel li span.carname").each(function () { carLabels.push($.trim($(this).text())) });
            var allMakes = $("#makeListing li div");

            var makesListDict = {};
            $.each(allMakes, function (count, element) { makesListDict[$.trim($(element).text())] = element; });
            allMakes.removeClass('active');
            $.each(carLabels, function (key, value) {
                $(makesListDict[value]).addClass("active");
            });
        },

        removeMakes: function (thisClass) {
            var $this = $(thisClass);
            var carLabel = $this.prev().text();
            var thisClassLi = $this.parent();
            var makeListing = $('#makeListing li div.active');
            $(thisClassLi).remove();
            makeListing.each(function () {
                var self = $(this);
                if (self.text() == carLabel)
                    self.removeClass('active');
            });
        },

        refreshMakes: function () {
            $('#makeListing li div.active').each(function () {
                $(this).removeClass('active');
            });
            $('#carLabel').html("");
        },

        resetfilter: function () {
            $('.parentCheck').removeClass("active");
            AdvantageSearch.filters.manufacturers = "";
            AdvantageSearch.filters.fuels = "";
            AdvantageSearch.filters.transmissions = "";
            AdvantageSearch.filters.bodyTypes = "";
            if (AdvantageSearch.env == 2) {
                Common.utils.trackAction("CWInteractive", "deals_desktop", "landingpage_filter_applied", "reset-filters");
                $('#drpSort').val(0);
                $('#advantage-city').text($('#advDrpCity option:selected').text());
            }
            else {
                $('.more-filter-icons').removeClass("active-blue");
            }

            AdvantageSearch.budget.budgetRange = "";
            AdvantageSearch.budget.setBudgetRange("", "");
            var completeQS = AdvantageSearch.generateQS();
            AdvantageSearch.changeUrl(completeQS);
            AdvantageSearch.listing.getDeals(completeQS);
        },

        moreFilterScroll: function () {
            $("#btnMoreFilter").text("More Filters");
            $("#more-filters-ul").hide();
        },

        moreLessFilter: function () {
            $("#more-filters-ul").slideToggle(1000);
            if ($("#btnMoreFilter").text() == "More Filters")
                $("#btnMoreFilter").text("Less Filters");
            else
                $("#btnMoreFilter").text("More Filters");

            $("#filter-div").animate({ scrollTop: 0 }, 1000);
        },
        filterCheckBoxClick: function (node, selector) {
            AdvantageSearch.filters.toggleSelection(node);
            if (AdvantageSearch.env == 2) {
                switch (selector) {
                    case AdvantageSearch.filters.selectors.make:
                        AdvantageSearch.filters.manufacturers = AdvantageSearch.filters.get.checkBoxFilter(selector);
                        break;
                    case AdvantageSearch.filters.selectors.fuel:
                        AdvantageSearch.filters.fuels = AdvantageSearch.filters.get.checkBoxFilter(selector);
                        break;
                    case AdvantageSearch.filters.selectors.transmission:
                        AdvantageSearch.filters.transmissions = AdvantageSearch.filters.get.checkBoxFilter(selector);
                        break;
                    case AdvantageSearch.filters.selectors.bodyType:
                        AdvantageSearch.filters.bodyTypes = AdvantageSearch.filters.get.checkBoxFilter(selector);
                        break;
                }
                var qs = AdvantageSearch.generateQS();
                AdvantageSearch.changeUrl(qs);
                AdvantageSearch.listing.getDeals(qs);
            }
        },

        filterIconsClick: function (node, selector) {
            node.find('.parentCheck').toggleClass('active');
            node.find('.more-filter-icons').toggleClass('active-blue');
        },

        registerEvents: function () {
            ko.applyBindings(AdvantageSearch.filters.koViewModel, document.getElementById('divMakesList'));
            AdvantageSearch.doc.on('click', AdvantageSearch.filters.selectors.make, function () {
                Common.utils.trackAction("CWInteractive", "deals_desktop", "landingpage_filter_applied", "make");
                AdvantageSearch.filters.filterCheckBoxClick($(this), AdvantageSearch.filters.selectors.make);

            });

            AdvantageSearch.doc.on('click', AdvantageSearch.filters.selectors.fuel, function () {
                if (AdvantageSearch.env == 2) {
                    AdvantageSearch.filters.filterCheckBoxClick($(this), AdvantageSearch.filters.selectors.fuel);
                    Common.utils.trackAction("CWInteractive", "deals_desktop", "landingpage_filter_applied", "fuel");
                }
                else
                    AdvantageSearch.filters.filterIconsClick($(this), AdvantageSearch.filters.selectors.fuel);
            });

            AdvantageSearch.doc.on('click', AdvantageSearch.filters.selectors.transmission, function () {
                if (AdvantageSearch.env == 2) {
                    AdvantageSearch.filters.filterCheckBoxClick($(this), AdvantageSearch.filters.selectors.transmission);
                    Common.utils.trackAction("CWInteractive", "deals_desktop", "landingpage_filter_applied", "transmission");
                }
                else
                    AdvantageSearch.filters.filterIconsClick($(this), AdvantageSearch.filters.selectors.transmission);
            });

            AdvantageSearch.doc.on('click', AdvantageSearch.filters.selectors.bodyType, function () {
                if (AdvantageSearch.env == 2) {
                    AdvantageSearch.filters.filterCheckBoxClick($(this), AdvantageSearch.filters.selectors.bodyType);
                    Common.utils.trackAction("CWInteractive", "deals_desktop", "landingpage_filter_applied", "bodytype");
                }
                else
                    AdvantageSearch.filters.filterIconsClick($(this), AdvantageSearch.filters.selectors.bodyType);
            });

            AdvantageSearch.doc.on('change', "#advDrpCity", function () {
                if (AdvantageSearch.env == 2)
                    AdvantageSearch.filters.resetfilter();
                else
                    AdvantageSearch.listing.getDealMakes();
                AdvantageSearch.filters.refreshMakes();
                $('#carLabel').show();

            });

            AdvantageSearch.doc.on('click', '.sub-values', function () {
                var element = $(this);
                element.parent().find('.filter-accordian-content').toggle('slow');
                if (element.find('span').hasClass('fa-angle-down'))
                    element.find('span').addClass('fa-angle-up').removeClass('fa-angle-down');
                else
                    element.find('span').addClass('fa-angle-down').removeClass('fa-angle-up');
            });

            AdvantageSearch.filters.set.city();


            $('#btnFilter,.change-filter-link').on('click', function () {

                AdvantageSearch.filters.set.checkBoxFilter(AdvantageSearch.filters.selectors.make, AdvantageSearch.filters.manufacturers);
                AdvantageSearch.filters.set.filterIcons(AdvantageSearch.filters.selectors.fuel, AdvantageSearch.filters.fuels);
                AdvantageSearch.filters.set.filterIcons(AdvantageSearch.filters.selectors.transmission, AdvantageSearch.filters.transmissions);
                AdvantageSearch.filters.set.filterIcons(AdvantageSearch.filters.selectors.bodyType, AdvantageSearch.filters.bodyTypes);
                AdvantageSearch.filters.appendMakes();
                AdvantageSearch.filters.openFilterPopup();
            });

            $('#btnSort').on('click', function () {
                AdvantageSearch.sort.openSortPopup();
            });

            $('#btnSortBack,#btnFilterBack,#btnMakeBack').on('click', function () {
                setTimeout(function () { window.history.back() }, 10);
            });


            $('#btnApplyFilters').on('click', function () {
                budget = validateBudget(0);
                if (budget.wrongURLFlag != 1) {
                    $("#maxPriceList").css("display", "none");
                    $("#budgetListContainer").removeClass("show").addClass("hide");
                    $("#minMaxContainer").removeClass('open');
                }
                else {
                    return false;
                }
                showPrefilLoading();
                if (!($('#carLabel').is(':visible')))
                    AdvantageSearch.filters.refreshMakes();
                AdvantageSearch.filters.manufacturers = AdvantageSearch.filters.get.checkBoxFilter(AdvantageSearch.filters.selectors.make);

                AdvantageSearch.filters.fuels = AdvantageSearch.filters.get.checkBoxFilter(AdvantageSearch.filters.selectors.fuel);
                AdvantageSearch.filters.transmissions = AdvantageSearch.filters.get.checkBoxFilter(AdvantageSearch.filters.selectors.transmission);
                AdvantageSearch.filters.bodyTypes = AdvantageSearch.filters.get.checkBoxFilter(AdvantageSearch.filters.selectors.bodyType);

                AdvantageSearch.budget.get();
                AdvantageSearch.changeUrl(AdvantageSearch.generateQS());
                AdvantageSearch.filters.closeFilterPopup();
                AdvantageSearch.listing.getDeals(window.location.search);
                setTimeout(hidePrefilLoading, 700); //change here to hide loader 
                AdvantageSearch.cityId = AdvantageSearch.filters.get.city();
                $('#advantage-city').text($('#advDrpCity option:selected').text());
            });

            $(document).on("click", "#sortFilter li", function () {
                $('#sortFilter li').removeClass('active');
                $(this).addClass('active');
                AdvantageSearch.sort.closeSortPopup();
            });

            $('#manufacturerSelection').on('click', function () {
                AdvantageSearch.filters.openMakePopup();
                if (!($('#carLabel').is(':visible')))
                    AdvantageSearch.filters.removeActiveMakes();
            });

            $('#btnMakeSubmit').on('click', function () {
                AdvantageSearch.filters.appendMakes();
                AdvantageSearch.filters.closeMakePopup();
                $('#carLabel').show();
            });

            $('#resetAllFilters').on('click', function () {
                $('#carLabel').hide();
                AdvantageSearch.filters.removeActiveMakes();
                AdvantageSearch.filters.resetfilter();

                $('#btnApplyFilters').trigger('click');
            });

            $(document).on('click', '#carLabel span.close-box', function () {
                AdvantageSearch.filters.removeMakes(this);
            });

            $(document).on("click", "#makeListing li a", function () {
                $(this).toggleClass('active');
            });

            $(document).on("click", "#btnMoreFilter", function () {
                AdvantageSearch.filters.moreLessFilter();
            });
            $(document).on("click", "#btnFilter", function () {
                AdvantageSearch.filters.moreFilterScroll();
            });
        },
    },

    sort: {
        sc: 1,
        so: 0,
        registerEvents: function () {
            AdvantageSearch.doc.on('change', '#drpSort', function () {
                var completeQS = AdvantageSearch.generateQS();
                AdvantageSearch.changeUrl(completeQS);
                AdvantageSearch.listing.getDeals(completeQS);
            });

            AdvantageSearch.doc.on('click', '#sortFilter li', function () {
                $('#sortFilter li').removeClass('active');
                $(this).addClass('active');
                AdvantageSearch.sort.closeSortPopup();

                var completeQS = AdvantageSearch.generateQS();
                AdvantageSearch.changeUrl(completeQS);
                AdvantageSearch.listing.getDeals(completeQS);
            });

            AdvantageSearch.sort.set();
        },

        get: function () {
            var sc, so, sortOption;
            if (AdvantageSearch.env == 2)
                sortOption = $('#drpSort option:selected');
            else
                sortOption = $("#sortFilter li.active");
            sc = sortOption.data('sc');
            so = sortOption.data('so');
            return 'sc=' + sc + '&so=' + so;
        },

        set: function () {
            var sortValue;
            if (AdvantageSearch.env == 2) {
                sortValue = $("#drpSort option[data-sc=" + AdvantageSearch.sort.sc + "][data-so=" + AdvantageSearch.sort.so + "]").val();
                $('#drpSort').val(sortValue);
            }
            else {
                $("#sortFilter li.active").removeClass('active')
                $("#sortFilter li[data-sc=" + AdvantageSearch.sort.sc + "][data-so=" + AdvantageSearch.sort.so + "]").addClass('active');
            }
        },

        openSortPopup: function () {
            $('#sort-div').show('slide', { direction: 'right' }, 500, function () { lockPopup(); });
            window.history.pushState("sort", "", "");
        },

        closeSortPopup: function () {
            unlockPopup();
            $('#sort-div').hide('slide', { direction: 'right' }, 500);
        }

    },

    budget: {
        budgetRange :'',
        setBudgetRange : function (start, end) {
            if (start == "" && end == "") {
                $("#budgetBtn").text("Choose budget");
                $('#minInput').val("");
                $('#maxInput').val("");
            }
            else {
                if(end == "")
                    $("#budgetBtn").text(start + "L - ");
                else
                    $("#budgetBtn").text(start + "L - " + end + "L");
                $('#minInput').val(start);
                $('#maxInput').val(end);
            }
        },
        
        registerEvents: function () {
            AdvantageSearch.doc.on('click', '#btnSetBudget', function () {
                Common.utils.trackAction("CWInteractive", "deals_desktop", "landingpage_filter_applied", "budget");
                AdvantageSearch.budget.get();
                var qs = AdvantageSearch.generateQS();
                AdvantageSearch.changeUrl(qs);
                AdvantageSearch.listing.getDeals(qs);
            });
        },
        
        get: function () {
            var minBudget = 0, maxBudget;
            minBudget = $('#minInput').val();
            maxBudget = $('#maxInput').val();
            minBudget = (minBudget == "" || minBudget == "0") ? 0 : parseFloat(minBudget);
            maxBudget = parseFloat(maxBudget) == 0 ? 0 : parseFloat(maxBudget);
            
            if (isNaN(maxBudget))
                maxBudget = "";

            if(minBudget == 0 &&(maxBudget == 0 || maxBudget=="") || (maxBudget != "" && minBudget>maxBudget))
            {
                AdvantageSearch.budget.budgetRange = "";
            }
            else
            {
                AdvantageSearch.budget.budgetRange= minBudget + '-' + maxBudget;
            }
        },

        set:function(budgetRange){
            AdvantageSearch.budget.budgetRange = budgetRange;
            if(budgetRange != "" && budgetRange != undefined)
            {   
                var range = budgetRange.split("-");
                AdvantageSearch.budget.setBudgetRange(range[0], range[1]);
            }
            else
            {
                AdvantageSearch.budget.setBudgetRange("", "");
            }
        }
       
    },
    generateQS: function () {
        var cityQS = AdvantageSearch.filters.get.city();
        var makesQS = AdvantageSearch.filters.manufacturers;
        var fuelQS = AdvantageSearch.filters.fuels;
        var transmissionQS = AdvantageSearch.filters.transmissions;
        var bodyTypeQS = AdvantageSearch.filters.bodyTypes;
        var sortQS = AdvantageSearch.sort.get();

        var budgetRange = AdvantageSearch.budget.budgetRange;
        var qs = "?cityId=" + cityQS
            + (makesQS != "" ? "&makes=" + makesQS : "")
            + (fuelQS != "" ? "&fuels=" + fuelQS : "")
            + (transmissionQS != "" ? "&transmissions=" + transmissionQS : "")
            + (bodyTypeQS != "" ? "&bodytypes=" + bodyTypeQS : "")
            + (budgetRange != "" ? "&budget=" +budgetRange : "")
            + '&' + sortQS;
        return qs;
    },

    changeUrl: function (qs) {
        window.history.pushState(null, null, qs);
    },

    pagination: {
        currentConfig: '', currentPages: '',
        bindPagination: function (config) {
            AdvantageSearch.pagination.currentConfig = config;
            config = AdvantageSearch.pagination.setTotalPages(config);
            AdvantageSearch.pagination.setStartEndPage(config);
            if (config.totalPages == 1)
                $('.advantage-pagination').addClass('hide');
            else
                $('.advantage-pagination').removeClass('hide');
            var pages = AdvantageSearch.pagination.getPageJson(config).pages;
            pages = AdvantageSearch.pagination.showPages(config, pages);
            AdvantageSearch.pagination.currentConfig = config;
            AdvantageSearch.pagination.currentPages = pages;
            AdvantageSearch.pagination.koViewModel.pages(pages);
        },
        setTotalPages: function (config) {
            config.totalPages = Math.ceil(config.totalCount / config.pageSize);
            return config;
        },
        setStartEndPage: function (config) {
            if (config.totalPages > 0) {
                config.start = 1;
                config.end = config.totalPages;
            }
            return config;
        },
        getPageJson: function (config) {
            var pages = [], page = {};
            for (var i = config.start; i <= config.end; i++) {
                page = {};
                page["no"] = i;
                page["fieldText"] = i;
                page["className"] = "hideImportant";
                pages.push(page);
            }
            return $.parseJSON('{ "pages" : ' + JSON.stringify(pages) + '}');
        },
        setPrevNextPages: function (config, pages) {
            var page = {};
            if (config.prev) {
                page = {};
                if (config.activePage == 1)
                { page["no"] = 1; page["className"] = "disabled prev"; }
                else
                { page["no"] = config.activePage - 1; page["className"] = "prev"; }
                page["fieldText"] = "Prev";
                pages.unshift(page);
            }
            if (config.next) {
                page = {};
                if (config.activePage == config.totalPages)
                { page["no"] = config.totalPages; page["className"] = "disabled next"; }
                else
                { page["no"] = config.activePage + 1; page["className"] = "next"; }
                page["fieldText"] = "Next";
                pages.push(page);
            }
            return pages;
        },
        setFirstLastPages: function (config, pages) {
            var page = {};
            if (config.first) {
                page = {};
                page["no"] = 1; page["fieldText"] = "First";
                if (config.activePage == 1)
                    page["className"] = "disabled first";
                else
                    page["className"] = "first";
                pages.unshift(page);
            }
            if (config.last) {
                page = {};
                page["no"] = config.totalPages; page["fieldText"] = "Last";
                if (config.activePage == config.totalPages)
                    page["className"] = "disabled last";
                else
                    page["className"] = "last";
                pages.push(page);
            }
            return pages;
        },
        showPages: function (config, pages) {
            config.startRange = config.activePage - Math.floor(config.range / 2);
            if (config.range % 2 == 0)
                config.endRange = config.activePage + Math.floor(config.range / 2) - 1;
            else
                config.endRange = config.activePage + Math.floor(config.range / 2);
            if (config.startRange <= 0) {
                config.startRange = 1;
                var diff = config.range - (config.endRange - config.startRange);
                if (diff < config.range - 1)
                    config.endRange = (config.endRange + diff) - 1;
            }
            if (config.endRange >= config.totalPages) {
                config.endRange = config.totalPages;
                var diff = config.range - (config.endRange - config.startRange);
                if (diff < config.range - 1) {
                    config.startRange = (config.startRange - diff) + 1;
                    if (config.startRange <= 0)
                        config.startRange = 1;
                }
            }

            for (var i = config.startRange - 1; i < config.endRange; i++) {
                pages[i]["className"] = "pager";
            }

            pages[config.activePage - 1]["className"] = "pager active";
            AdvantageSearch.pagination.setPrevNextPages(config, pages);
            return pages;
        },
        prevClick: function (pageNo) {
            var node;
            $('#pager li').removeClass('active');
            node = $('#pager li[data-pageno="' + pageNo + '"]');
            if (node.prev().hasClass('hideImportant'))
                AdvantageSearch.pagination.rearrange(node.next(), node.prev(), pageNo);
            else {
                AdvantageSearch.pagination.setActiveNode(pageNo);
            }
        },
        nextClick: function (pageNo) {
            var node;
            $('#pager li').removeClass('active');
            node = $('#pager li[data-pageno="' + pageNo + '"]');
            if (node.next().hasClass('hideImportant'))
                AdvantageSearch.pagination.rearrange(node.next(), node.prev(), pageNo);
            else {
                AdvantageSearch.pagination.setActiveNode(pageNo);
            }
        },
        setActiveNode: function (pageNo) {
            $('#pager li[data-pageno="' + pageNo + '"]').filter(function () {
                var node = $(this);
                if (node.text() != "Prev" && node.text() != "Next" && node.text() != "First" && node.text() != "Last")
                    node.addClass('active');
            });
        },
        numberClick: function (node) {
            var nextNode, prevNode, pageNo;
            if (node.hasClass('prev') || node.hasClass('next') || node.hasClass('first') || node.hasClass('last')) {
                node = $('#pager li[data-pageno="' + node.data().pageno + '"]');
            }
            nextNode = node.next(), prevNode = node.prev(), pageNo = node.data().pageno;
            AdvantageSearch.pagination.rearrange(nextNode, prevNode, pageNo);
        },
        rearrange: function (nextNode, prevNode, pageNo) {
            var node = $('#pager li[data-pageno="' + pageNo + '"]');
            $('#pager li').removeClass('active');
            AdvantageSearch.pagination.currentConfig.activePage = pageNo;
            $('#pager li.prev').data().pageno = pageNo - 1;
            $('#pager li.next').data().pageno = pageNo + 1;
            if (nextNode.hasClass('hideImportant') || prevNode.hasClass('hideImportant'))
                AdvantageSearch.pagination.bindPagination(AdvantageSearch.pagination.currentConfig);
            else {
                $('#pager li[data-pageno="' + pageNo + '"]').filter(function (index) {
                    if ($(this).text() != "Prev" && $(this).text() != "Next" && $(this).text() != "First" && $(this).text() != "Last") $(this).addClass('active');
                });
            }
        },
        getActivePageNo: function () {
            return $('#pager li.active').data().pageno;
        },
        callBack: function () {
            AdvantageSearch.listing.getDeals(AdvantageSearch.generateQS(), AdvantageSearch.pagination.getActivePageNo());
        },
        setEnableDisabled: function () {
            var currentPage = $('#pager li.active').data().pageno;
            if (currentPage == 1)
                $('#pager li.prev').addClass('disabled');
            else
                $('#pager li.prev').removeClass('disabled');
            if (currentPage == $('.pager').length)
                $('#pager li.next').addClass('disabled');
            else
                $('#pager li.next').removeClass('disabled');


        },
        koViewModel: {
            pages: ko.observableArray([])
        },
        registerEvents: function () {
            AdvantageSearch.doc.on('click', '#pager li.pager', function () {
                AdvantageSearch.pagination.numberClick($(this));
                AdvantageSearch.pagination.currentConfig.onchange();
                AdvantageSearch.pagination.setEnableDisabled();
            });
            AdvantageSearch.doc.on('click', '#pager li.prev', function () {
                if ($(this).hasClass('disabled'))
                    return false;
                var pageNo = $(this).data().pageno;
                if (pageNo >= 1) {
                    AdvantageSearch.pagination.prevClick(pageNo);
                    $(this).data().pageno = pageNo - 1;
                    $('#pager li.next').data().pageno = pageNo + 1;
                    AdvantageSearch.pagination.currentConfig.onchange();
                }
                AdvantageSearch.pagination.setEnableDisabled();
            });
            AdvantageSearch.doc.on('click', '#pager li.next', function () {
                if ($(this).hasClass('disabled'))
                    return false;
                var pageNo = $(this).data().pageno;
                if (pageNo <= AdvantageSearch.pagination.currentConfig.totalPages) {
                    AdvantageSearch.pagination.nextClick(pageNo);
                    $(this).data().pageno = pageNo + 1;
                    $('#pager li.prev').data().pageno = pageNo - 1;

                    AdvantageSearch.pagination.currentConfig.onchange();
                }
                AdvantageSearch.pagination.setEnableDisabled();
            });
        }
    },

    versions: {
        getFilteredVersions: function (element) {
            var qs = AdvantageSearch.generateQS();
            var modelId = element.parents('[data-model-id]').data('model-id');
            var versionId = element.parents('[data-version-id]').data('version-id');
            qs += (qs ? '&' : '?') + 'modelId=' + modelId + '&' + 'versionId=' + versionId;
            var apiUrl = "/api/deals/filtered-versions/" + qs;
            var versions = [];
            $.ajax({
                type: 'GET',
                async: false,
                headers: { "sourceid": "1" },
                url: apiUrl,
                dataType: 'Json',
                success: function (response) {
                     versions = response;            
                }
            });
           return versions;
        },
        registerEvents: function () {
            AdvantageSearch.doc.on('click', '.version-link', function () {
                var self = $(this);
                var koModelData = {
                                    filteredVersions: AdvantageSearch.versions.getFilteredVersions(self), maskingName: self.parents('[data-model-maskingname]').data('model-maskingname'),
                                    modelName: self.parents('[data-model-name]').data('model-name'), makeName: self.parents('[data-make-name]').data('make-name')
                                   };
                AdvantageSearch.versions.bindResults(koModelData);
                var node = self.parents('.aged-car-listing').find('.contentWrapper');
                $('#bestDeal').html(node.html());
                $('#bestDeal').attr('href', node.parent().attr('href'));
                $('#advantage-version-popup').show();
                $('.blackOut-window').show();
                window.history.pushState("version-popup", "", "");
                Common.utils.lockPopup();
            });           
            AdvantageSearch.doc.on('click', '.advantage-close-btn, .blackOut-window', function () {
                $('#advantage-version-popup').hide();
                $('.blackOut-window').hide();
                Common.utils.unlockPopup();
            });
            ko.applyBindings(AdvantageSearch.versions.koViewModel, document.getElementById('filteredVersions'));
        },
        bindResults: function (data) {
            AdvantageSearch.versions.koViewModel.filteredVersions(data.filteredVersions);
            AdvantageSearch.versions.koViewModel.maskingName(data.maskingName);
            AdvantageSearch.versions.koViewModel.modelName(data.modelName);
            AdvantageSearch.versions.koViewModel.makeName(data.makeName);
        },
        koViewModel: {
            filteredVersions: ko.observableArray([]),
            maskingName: ko.observable(),
            modelName: ko.observable(),
            makeName: ko.observable()
        },
    },

    registerEvents: function () {
        $(window).on('scroll', function () {
            if ($(window).scrollTop() > 40) {
                $('#header').addClass('header-fixed-with-bg');
            } else {
                $('#header').removeClass('header-fixed-with-bg');
            }
        });
        $(window).scroll(function () {
            AdvantageSearch.scrollPosition = $(this).scrollTop();
            if (AdvantageSearch.scrollPosition > AdvantageSearch.sortFilterOffsetTop)
                $(AdvantageSearch.sortFilterDiv).addClass('float-fixed');
            else $(AdvantageSearch.sortFilterDiv).removeClass('float-fixed');
        });
        if (AdvantageSearch.env == 1)
            $(window).load(function () { hidePrefilLoading(); });
        AdvantageSearch.listing.registerEvents();
        AdvantageSearch.filters.registerEvents();

        $('.backtotop-link').on('click', function () {
            $('html, body').animate({ scrollTop: 0 }, 'slow');
        });

    },

    handleBack: function () {
        $(window).bind('popstate', function (event) {
            if ($('#manfListDiv').is(':visible')) {
                AdvantageSearch.filters.closeMakePopup();
            }
            else if ($('#filter-div').is(':visible')) {
                    AdvantageSearch.filters.closeFilterPopup();
                    AdvantageSearch.filters.set.city();
                    AdvantageSearch.listing.getDealMakes();                
            }
            else if ($('#sort-div').is(':visible')) {
                AdvantageSearch.sort.closeSortPopup();
            }
           else if ($('#advantage-version-popup').is(':visible')) {
                $('.advantage-close-btn').trigger('click');
            }
        });
    },

    pageLoad: {
        listingLoad: function () {
            AdvantageSearch.sort.registerEvents();
            AdvantageSearch.registerEvents();
            ko.applyBindings(AdvantageSearch.pagination.koViewModel, document.getElementById('pager'));
            AdvantageSearch.listing.getDeals(AdvantageSearch.generateQS());
            AdvantageSearch.offerOfWeek.registerEvents();
            AdvantageSearch.pagination.registerEvents();
            AdvantageSearch.utils.registerEvents();
            AdvantageSearch.budget.registerEvents();
            AdvantageSearch.versions.registerEvents();
            AdvantageSearch.handleBack();
            $('div.ask-expert').addClass('hide');
        }

    },
   
    getSelectedCityId: function () {
        return $("#advDrpCity").val();
},
}
