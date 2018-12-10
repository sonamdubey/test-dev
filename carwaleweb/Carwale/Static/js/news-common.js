var news = {
    calls: {
        getArticles: function (categories, start, end, make, model) {
            return Common.utils.ajaxCall({
                url: '/webapi/article/ListByCategory/?categoryidlist=' + categories.join(',') + '&applicationid=1&startindex=' + start + '&endindex=' + end + (make != null ? '&makeid=' + make.id : '') + (model != null ? '&modelid=' + model.id : '')
            });
        },
        getMedia: function (categories, start, end, make, model) {
            return Common.utils.ajaxCall({
                url: '/api/media/?categoryidlist=' + categories.join(',') + '&applicationid=1&startindex=' + start + '&endindex=' + end + (make != null ? '&makeid=' + make.id : '') + (model != null ? '&modelid=' + model.id : '') + '&getAllMedia=true',
            });
        },
        getPromise: function (categories, start, end, make, model) {
            switch (news.KOVM.currentTab()) {
                case "images":
                    return news.calls.getMedia(categories, start, end, make, model);
                    break;
                case "videos":
                    return news.calls.getMedia(categories, start, end, make, model);
                    break;
                default:
                    return news.calls.getArticles(categories, start, end, make, model);
                    break;
            }
        },
        getSegments: function (applicationId, make, model) {
            return $.ajax({
                url: "/api/content/segment/" + '?getAllMedia=true',
                data: { applicationId: 1, makeId: make != null ? make.id : 0, modelId: model != null ? model.id : 0 },
                headers: { sourceId: 1 },
            });
        }
    },
    galleryCategories: {"images" : [10], "videos" : [13]},
    ieVersion: function () {
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");
        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) // If Internet Explorer, return ms_ie
            return (parseInt(ua.substring(msie + 5, ua.indexOf(".", msie))));
    },
    mobile: function () {
        if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent))
            return true;
        else return false;
    },
    resetFilter: function () {
        news.filter.val("");
        news.mSearchContainer.removeClass("hide");
        news.filterContainer.removeClass("hide");
        news.clearFilter.addClass("hide");
        news.filterLabel.addClass("hide");
        news.filterLabel.parent().addClass("hide");
        news.unapplyKO();
        if (news.originalKOVM.make() != null) {
            news.calls.getPromise(news.KOVM.getCategories(), 1, 18, null, null).done(function (result) {
                news.originalKOVM = new news.viewModel(news.KOVM.currentTab(), 1, new window.Car(), result);
                news.KOVM = news.originalKOVM;
                news.KOVM.processData(1, false, false);
                news.applyKO(news.originalKOVM);
                news.listing.layoutChangeCall();
                news.listing.applyEvents();
                news.KOVM.updateHistory();
            });
        } else {
            news.originalKOVM.change(news.KOVM.currentTab());
            news.KOVM = news.originalKOVM;
            news.applyKO(news.originalKOVM);
            news.listing.layoutChangeCall();
            news.listing.applyEvents();
            news.KOVM.updateHistory();
        }
        $('.scrollmenu').removeClass('increasePadding');
        $('.fixed-tabs-mobile').css({ 'height': '30px' });
        Common.utils.firePageView(window.location.pathname + window.location.search);
    },
    filterCar: function (car, act, label) {

        var filterPromise = news.calls.getPromise(news.KOVM.getCategories(), 1, 18, car.make, car.model);
        var segmentPromise = news.calls.getSegments(1, car.make, car.model);

        $.when(filterPromise, segmentPromise).done(function (result, segmentResult) {
            var switched = true;
            var category = '';
            var resultLength;
            segmentResult = segmentResult[0];
            switch (news.KOVM.currentTab()) {
                case "images":
                    resultLength = segmentResult[3].count;
                    break;
                case "videos":
                    resultLength = segmentResult[4].count;
                    break;
                case "expert-reviews":
                    resultLength = segmentResult[1].count;
                    break;
                case "features":
                    resultLength = segmentResult[2].count;
                    break;
                default:
                    resultLength = segmentResult[0].count;
                    break;
            }
            if (result[1] == "success" && resultLength > 0) {
                switched = false; result = result[0];
            }
            else {
                switch (news.KOVM.currentTab()) {
                    case "images":
                        if(segmentResult[4].count > 0){
                            resultLength = segmentResult[4].count;
                            category = "videos";
                        }
                    case "videos":
                        if (segmentResult[3].count > 0) {
                            resultLength = segmentResult[3].count;
                            category = "images";
                        }
                        if (resultLength > 0)
                            break;
                    default:
                        if (segmentResult[0].count > 0) {
                            resultLength = segmentResult[0].count;
                            category = "all";
                            $('li.tablinks').eq(0).click();
                        }
                        break;
                }
            } 
            if (resultLength > 0) {
                if (switched) {
                    if (category == 'all') {
                        news.calls.getArticles(news.KOVM.getCategories(true), 1, 18, car.make, car.model).done(
                            function (response) {
                                result = response;
                                news.processFilter(category, car, result, segmentResult);
                            }
                        );
                    }
                    else {
                        news.calls.getMedia(news.galleryCategories[category], 1, 18, car.make, car.model).done(function (response) {
                            result = response;
                            news.processFilter(category, car, result, segmentResult);
                        })
                    }

                }
                else {
                    news.processFilter(news.KOVM.currentTab(), car, result, segmentResult);
                }
                if (window.isMobile) { $('.scrollmenu').addClass('increasePadding'); $('.fixed-tabs-mobile').css({ 'height': '43px' }); }
                Common.utils.trackAction('CWInteractive', 'newsfilter', act, label);
            } else {
                news.nomatch.html("No articles found for " + $.trim(news.filterLabel.html()));
                news.filter.val("")
                news.nomatch.removeClass("hide");
                news.filter.focus();
                act = "noresult-" + act;
                Common.utils.trackAction('CWInteractive', 'newsfilter', act, label);
            }
        }).fail(function () { alert("fail"); });
    },
    processFilter: function (tab, car, result, segmentResult) {
        var newViewModel = new news.viewModel(tab, 1, car, result, segmentResult);

        news.unapplyKO();
        news.KOVM = newViewModel;
        news.applyKO(news.KOVM);

        news.KOVM.processData(1, false, false);
        news.mSearchContainer.addClass("hide");
        news.filterContainer.addClass("hide");
        news.clearFilter.removeClass("hide");
        news.filterLabel.removeClass("hide");
        news.filterLabel.parent().removeClass("hide");
        news.listing.applyEvents();
        news.KOVM.updateHistory();
        Common.utils.firePageView(window.location.pathname + window.location.search);
    },
    initialize: function () {
        $('#viewUl li.news-box').find('.desc-content').show();
        window.isGridView = true;
        news.nomatch = $("#newsFilterNoMatch");
        news.filterLabel = $("#postFilterSelection");
        news.mSearchContainer = $("#mSearchContainer");
        news.filterContainer = $("#newsFilterContainer");
        news.filter = $('#newsFilter');
        news.clearFilter = $('#clearFilter');
        news.clearFilter.click(news.resetFilter);
        $("#viewUl").empty();
        news.originalKOVM = new news.viewModel(window.category, currentPageNumber, window.car, newsListingData, segmentData);
        news.KOVM = news.originalKOVM;
        news.applyKO(news.KOVM);
        news.KOVM.processData(1, false, false);
        $(window).scroll(function (e) {
            news.KOVM.scroll(e);
        });
        news.listing.applyEvents();
        news.listing.applyInitialEvents();
        if (window.car.make != null) {
            if (window.car.model != null) news.filterLabel.html(window.car.model.displayName);
            else news.filterLabel.html(window.car.make.displayName);
            news.mSearchContainer.addClass("hide");
            news.filterContainer.addClass("hide");
            news.clearFilter.removeClass("hide");
            news.filterLabel.removeClass("hide");
            news.filterLabel.parent().removeClass("hide");
        }
        if ((window.car.make != null || window.car.model != null) && window.isMobile) {
            $('.scrollmenu').addClass('increasePadding');
            $('.fixed-tabs-mobile').css({ 'height': '43px' });
        }
        if (window.isMobile) {
            news.filter.cw_easyAutocomplete({
                inputField: news.filter,
                isNew: 1,
                isOnRoadPQ: 1,
                newsFilter: true,
                pQPageId: 57,
                resultCount: 5,
                showFeaturedCar: false,
                currentresult: [],
                textType: ac_textTypeEnum.model + ',' + ac_textTypeEnum.make,
                source: ac_Source.generic,
                onClear: function () {
                    news.nomatch.addClass("hide");
                },
                click: function (event) {
                    var selectionValue = news.filter.getSelectedItemData().value,
                        splitVal = selectionValue.split('|');

                    //var splitVal = ui.item.id.split('|');

                    var make = {};
                    make.name = splitVal[0].split(':')[0];
                    make.id = splitVal[0].split(':')[1];

                    var model = null;
                    if (splitVal[1] != undefined && splitVal[1].indexOf(':') > 0) {
                        model = {};
                        model.name = splitVal[1].split(':')[0];
                        model.id = splitVal[1].split(':')[1];
                    }

                    var act = "";
                    var label = "";
                    var selectionLabel = news.filter.getSelectedItemData().label;
                    if (model != null) {
                        var regEx = new RegExp(make.name, "ig");
                        var tokens = selectionLabel.split(" ");
                        if (tokens.length > 0 && tokens[0].indexOf("-") >= 0) {
                            news.filterLabel.html($.trim(selectionLabel.replace("-", "").replace(regEx, "")));
                        } else {
                            news.filterLabel.html($.trim(selectionLabel.replace(" ", "").replace(regEx, "")));
                        }
                        act = "model-" + news.filterLabel.html().toLowerCase();
                        label = news.filter.val();
                        model.displayName = news.filterLabel.html();
                        make.displayName = $.trim(selectionLabel.replace(model.displayName, ""));
                    } else {
                        news.filterLabel.html($.trim(selectionLabel.replace("All", "").replace("Cars", "")));
                        act = "make-" + news.filterLabel.html().toLowerCase();
                        label = news.filter.val();
                        make.displayName = news.filterLabel.html();
                    }
                    news.filterCar({ make: make, model: model }, act, label);
                },
                afterFetch: function (result, searchtext) {
                    if (result != undefined && result.length > 0) {
                        news.nomatch.addClass("hide");
                    }
                    else {
                        news.nomatch.html("No matching cars found.");
                        var act = "noresult";
                        var label = news.filter.val();
                        Common.utils.trackAction('CWInteractive', 'newsfilter', act, label);
                        news.nomatch.removeClass("hide");
                    }
                },
                focusout: function () {
                    news.nomatch.addClass("hide");
                }
            });
        }
        else {
            news.filter.cw_autocomplete({
                isNew: 1,
                isOnRoadPQ: 1,
                newsFilter: true,
                pQPageId: 57,
                resultCount: 5,
                showFeaturedCar: false,
                currentresult: [],
                textType: ac_textTypeEnum.model + ',' + ac_textTypeEnum.make,
                source: ac_Source.generic,
                onClear: function () {
                    news.nomatch.addClass("hide");
                },
                click: function (event, ui, orgTxt) {
                    var splitVal = ui.item.id.split('|');

                    var make = new Object();
                    make.name = splitVal[0].split(':')[0];
                    make.id = splitVal[0].split(':')[1];

                    var model = null;
                    if (splitVal[1] != undefined && splitVal[1].indexOf(':') > 0) {
                        model = new Object();
                        model.name = splitVal[1].split(':')[0];
                        model.id = splitVal[1].split(':')[1];
                    }

                    var act = "";
                    var label = "";
                    var inputlabel = ui.item.label;
                    if (model != null) {
                        var regEx = new RegExp(make.name, "ig");
                        var tokens = inputlabel.split(" ");
                        if (tokens.length > 0 && tokens[0].indexOf("-") >= 0) {
                            news.filterLabel.html($.trim(inputlabel.replace("-", "").replace(regEx, "")));
                        } else {
                            news.filterLabel.html($.trim(inputlabel.replace(" ", "").replace(regEx, "")));
                        }
                        act = "model-" + news.filterLabel.html().toLowerCase();
                        label = news.filter.val();
                        model.displayName = news.filterLabel.html();
                        make.displayName = $.trim(inputlabel.replace(model.displayName, ""));
                    } else {
                        news.filterLabel.html($.trim(inputlabel.replace("All", "").replace("Cars", "")));
                        act = "make-" + news.filterLabel.html().toLowerCase();
                        label = news.filter.val();
                        make.displayName = news.filterLabel.html();
                    }
                    news.filterCar({ make: make, model: model }, act, label);
                },
                afterfetch: function (result, searchtext) {
                    if (result != undefined && result.length > 0) {
                        news.nomatch.addClass("hide");
                    }
                    else {
                        news.nomatch.html("No matching cars found.");
                        var act = "noresult";
                        var label = news.filter.val();
                        Common.utils.trackAction('CWInteractive', 'newsfilter', act, label);
                        news.nomatch.removeClass("hide");
                    }
                }
            , focusout: function () {
                news.nomatch.addClass("hide");
            }
            });
        }
    },
    showContentTabs: function(evt, tabName) {
        var tabContent;
        tabContent = $(".content-secondary-nav-list");
        tabContent.each(function () {
            $(this).addClass('hide-important');
        });
        var currentTab = $('#' + tabName);
        currentTab.removeClass('hide-important');
        $('li.tablinks.activeBtn').removeClass('activeBtn');
        $(evt.target).addClass('activeBtn');
        setTimeout(function(){currentTab.find('li').not('.disabled').eq(0).click();}, 0);
    },
    setTitle: function (category, make, model) {
        var carText = "Car ";
        if (make != undefined && model != undefined) {
            carText = make + " " + model + " ";
        }
        else if (make != undefined) {
            carText = make + " cars ";
        }
        switch (category) {
            case "features":
                document.title = carText + "Special Reports - Stories, Specials & Travelogues - CarWale";
                break;
            case "expert-reviews":
                document.title = carText + "Road tests, First drives, Expert Reviews of New Cars in India - CarWale";
                break;
            case "images":
                document.title = carText + "photos and images - CarWale";
                break;
            case "videos":
                document.title = carText + "Videos, Expert Video Reviews with Road Test & Car Comparison - CarWale";
                break;
            default:
                document.title = carText + "News, Auto News India - CarWale";
                break;
        }

    },
    listing: {
        leftContainer: "#newsContainer .left-container",
        newsBox: "#newsContainer li.news-box",
        newsBoxLength: 0,
        dataTab: 'all',
        applyInitialEvents: function () {
            $("#head_Car li").click(function () {
                var liValue = $(this).attr("id");

                if (liValue == "popular_Car") {
                    $("#popularCar").show();
                    $("#upcomingCar").hide();
                }
                else {
                    $("#upcomingCar").show();
                    $("#popularCar").hide();
                }
            });
            $("div.right-container #cars-tabs li h2").click(function () {
                $(".cars-tabs-data").hide();
                $(".cars-tabs-data").eq($(this).parent().index()).show();
                $("#cars-tabs li h2").removeClass("active");
                $(this).addClass("active");
            });
            $(window).on("orientationchange", function (event) {
                setTimeout(function () {
                    news.listing.layoutChangeCall();
                }, 0);
            });
            if (!news.mobile()) {
                $(window).on("resize", function () {
                    news.listing.layoutChangeCall();
                });
            }
            $('.scrollToTop').click(function () {
                Common.utils.trackAction('CWNonInteractive', 'EditorialLayout', 'ScrollTopClick', news.KOVM.currentTab());
                $('html, body').animate({ scrollTop: 0 }, 800);
                return false;
            });
        },
        applyEvents: function () {
            news.listing.newsBoxLength = news.KOVM.current()().length;

            if (news.ieVersion() < 10) {
                $(news.listing.newsBox).css('opacity', 1);
            } else {
                news.listing.layoutType = news.listing.setListPos();
            }

            $("div.filter-tab .category-tabs li").click(function (e) {
                var self = $(this);
                if ((window.isMobile) || self.parents('.fixed-tabs').length == 1) {
                    var element = window.isMobile ? $('#msiteNewsFilterContainer') : $("#datatabs");
                    window.scrollTo(0, element.offset().top + 10);
                    news.KOVM.scrolledPages["currentPage"] = 1;
                }
                var disabled = self.hasClass('disabled');
                if (!self.hasClass('active') && !disabled) {
                    news.listing.dataTab = self.attr('data-tabs');
                    news.KOVM.change(news.listing.dataTab);
                    news.listing.postFilterAction();
                    if (news.mobile()) {
                        news.listing.animateMenu('#floatMenu');
                        if ($('#fixedMenu').is(':visible')) {
                            news.listing.animateMenu('#fixedMenu');
                        }
                    }
                    Common.utils.firePageView(window.location.pathname + window.location.search);
                }
                else {
                    e.preventDefault();
                    return;
                }
            });

            setTimeout(news.listing.postFilterAction, 0);

            $(window).on('load', function () {
                var windowWidth = $(window).width();
                if ($('#datatabs').width() > $('.news-car-filter-container').width()) {
                    if (news.mobile()) {
                        news.listing.animateMenu('#floatMenu');
                        if ($('#fixedMenu').is(':visible')) {
                            news.listing.animateMenu('#fixedMenu');
                        }
                    }
                }
            });
        },
        postFilterAction: function (tabName) {
            news.listing.newsBoxLength = news.KOVM.current()().length;
            if (news.ieVersion() < 10) {
                var allFilterList = $('#newsContainer ul li.news-box');
                allFilterList.css({
                    'opacity': 1, 'display': 'block'
                });
            }
            else {
                news.listing.setListContainerHt();
            }

            $("img.lazy").lazyload()
                         .each(function () {
                             $(this).removeClass("lazy");
                         })
            ;
        },
        setListContainerHt: function () {

            $('#newsContainer').show();
            setTimeout(function () {
                var newsBox = $(news.listing.newsBox);
                if (newsBox.length > 0) {
                    var listContHt = $(news.listing.newsBox).last().position().top;
                    var newsBoxHt = $(news.listing.newsBox).height();
                    $(news.listing.leftContainer).height(listContHt + newsBoxHt);
                }
                else {
                    $(news.listing.leftContainer).height(327);
                }


            }, 0);
        },
        setListPos: function () {
            var layoutTypePos = [];
            news.listing.newsBoxLength = news.KOVM.current()().length;
            for (var i = 0; i < news.listing.newsBoxLength; i++) {
                layoutTypePos.push($(news.listing.newsBox).eq(i).position());
            }
            return layoutTypePos;
        },

        layoutChangeCall: function () {
            try {
                news.listing.layoutType = [];
                news.listing.layoutType = news.listing.setListPos();
                news.listing.postFilterAction();
            } catch (e) {
                console.log(e);
            }
        },

        NewsListMore: function () {
            if (window.isGridView) {
                $('.untrimmed').hide();
                $('.trimmed').show();
                $(".show-read-more").each(function () {
                    var maxLength = 310;
                    var myStr = $(this).find('.untrimmed .desc-content').text();
                    var trimmedDesContent = $(this).find('.trimmed .desc-content');
                    if ($.trim(myStr).length > maxLength) {
                        var newStr = myStr.substring(0, maxLength);
                        trimmedDesContent.text(newStr);
                        trimmedDesContent.append(' <a href="' + $(this).find('h2').find('a').attr('href') + '" class="read-more-news">Read More...</a>');
                    }
                });
            }
        },

        animateMenu: function (container) {
            var out = $(container);
            var node = out.find('.active');
            var containerWidth = out.width();
            var nodeWidth = node.outerWidth(true);
            var nodeIndex = node.index();
            var shift = 0;
            var nodes = out.find('li');
            for (var i = 0; i < nodeIndex; i++) {
                shift += $(nodes[i]).outerWidth(true);
            }
            out.animate({ 'scrollLeft': Math.max(0, shift - (containerWidth - nodeWidth) / 2) }, 500, 'swing');
        },
    },
    viewModel: function (cat, page, car, data, segmentResult) {
        this.self = this;
        this.redLineStyle = ko.observable();
        this.isMobile = window.isMobile;
        this.firstProcess = true;
        this.filterApplied = ko.observable(car.make != null || car.model != null);
        this.make = ko.observable(car.make);
        this.model = ko.observable(car.model);
        this.scrollPageNumber = ko.observable(1);
        this.h1Tag = ko.observable();
        this.uiPageNumber = ko.observable(page);
        this.pageNumberNonComputed = 1;
        this.eachPageSize = 9.0;
        this.all = ko.observableArray([]);
        this.allDict = {};
        this.allPagesScrolled = {};
        this.allCount = ko.observable(segmentResult != undefined ? segmentResult[0].count : 1);
        this.expertreviews = ko.observableArray([]);
        this.expertreviewsDict = {};
        this.expertreviewsPagesScrolled = {};
        this.expertReviewsCount = ko.observable(segmentResult != undefined ? segmentResult[1].count : 1);
        this.features = ko.observableArray([]);
        this.featuresDict = {};
        this.featuresPagesScrolled = {};
        this.featuresCount = ko.observable(segmentResult != undefined ? segmentResult[2].count : 1);
        this.photos = ko.observableArray([]);
        this.photosDict = {};
        this.photosPagesScrolled = {};
        this.photosCount = ko.observable(segmentResult != undefined ? segmentResult[3].count : 1);
        this.videos = ko.observableArray([]);
        this.videosDict = {};
        this.videosPagesScrolled = {};
        this.videosCount = ko.observable(segmentResult != undefined ? segmentResult[4].count : 1);
        this.current = ko.observable();
        this.currentDict = {};
        this.currentCount = ko.observable();
        this.scrolledPages = {};
        this.currentTabNonComputed = cat;
        this.currentTab = ko.observable(cat);
        switch (cat) {
            case 'features':
                this.currentDict = this.featuresDict;
                this.featuresPagesScrolled[page] = true;
                this.featuresPagesScrolled[page + 1] = true;
                this.scrolledPages = this.featuresPagesScrolled;
                this.current(this.features);
                this.currentCount(this.featuresCount());
                this.h1Tag("Special Reports");
                break;
            case 'expert-reviews':
                this.currentDict = this.expertreviewsDict;
                this.expertreviewsPagesScrolled[page] = true;
                this.expertreviewsPagesScrolled[page + 1] = true;
                this.scrolledPages = this.expertreviewsPagesScrolled;
                this.current(this.expertreviews);
                this.currentCount(this.expertReviewsCount());
                this.h1Tag("Expert Reviews");
                break;
            case 'videos':
                this.currentDict = this.videosDict;
                this.videosPagesScrolled[page] = true;
                this.videosPagesScrolled[page + 1] = true;
                this.scrolledPages = this.videosPagesScrolled;
                this.current(this.videos);
                this.currentCount(this.videosCount());
                this.h1Tag("Videos");
                break;
            case 'images':
                this.currentDict = this.photosDict;
                this.photosPagesScrolled[page] = true;
                this.photosPagesScrolled[page + 1] = true;
                this.scrolledPages = this.photosPagesScrolled;
                this.current(this.photos);
                this.currentCount(this.photosCount());
                this.h1Tag("Images");
                break;
            default:
                this.currentDict = this.allDict;
                this.allPagesScrolled[page] = true;
                this.allPagesScrolled[page + 1] = true;
                this.scrolledPages = this.allPagesScrolled;
                this.current(this.all);
                this.currentCount(this.allCount());
                this.h1Tag("Car News");
        };
        if (cat == 'images') {
            this.current()(data.photos.photosList);
        }
        else if (cat == 'videos') {
            this.current()(data.videos.videosList);
        }
        else {
            this.current()(data.Articles);
        }
        this.updateRedLineStyle = function () {
            if (!window.isMobile) {
                tabAnimation.$thisLI = $("#datatabs li.active");
                tabAnimation.panel = tabAnimation.$thisLI.closest(".cw-tabs-panel");
                tabAnimation.liClickTabAnimation();
                tabAnimation.$thisLI = $("#datatabstop li.active");
                tabAnimation.panel = tabAnimation.$thisLI.closest(".cw-tabs-panel");
                tabAnimation.liClickTabAnimation();
            }
            else { $("hr.tabHr.tran-ease-out-all").hide(); }
        };
        this.updateRedLineStyle();
        this.featureCategory = [6];
        this.expertCategory = expertCategory;
        this.newsCategory = newsCategory;
        this.ajaxResult = ko.observable();
        this.requestInProgress = false;
        this.pagination = ko.observableArray([]);
        this.months = ["Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec"];
        this.change = function (category) {
            var prevTab = this.currentTabNonComputed;
            this.currentTab($.trim(category));
            this.currentTabNonComputed = this.currentTab();
            switch ($.trim(category)) {
                case 'features':
                    this.currentDict = this.featuresDict;
                    this.scrolledPages = this.featuresPagesScrolled;
                    this.current(this.features);
                    this.currentCount(this.featuresCount());
                    this.h1Tag("Special Reports");
                    break;
                case 'expert-reviews':
                    this.currentDict = this.expertreviewsDict;
                    this.scrolledPages = this.expertreviewsPagesScrolled;
                    this.current(this.expertreviews);
                    this.currentCount(this.expertReviewsCount());
                    this.h1Tag("Expert Reviews");
                    break;
                case 'videos':
                    this.currentDict = this.videosDict;
                    this.scrolledPages = this.videosPagesScrolled;
                    this.current(this.videos);
                    this.currentCount(this.videosCount());
                    this.h1Tag("Videos");
                    break;
                case 'images':
                    this.currentDict = this.photosDict;
                    this.scrolledPages = this.photosPagesScrolled;
                    this.current(this.photos);
                    this.currentCount(this.photosCount());
                    this.h1Tag("Images");
                    break;
                default:
                    this.currentDict = this.allDict;
                    this.scrolledPages = this.allPagesScrolled;
                    this.current(this.all);
                    this.currentCount(this.allCount());
                    this.h1Tag("Car News");
            };
            this.tabChangeDataTrigger(prevTab);
        };
        this.getCategory = function (categoryId) {
            var viewmodel = this.self;
            categoryId = Number(categoryId);
            if ($.inArray(categoryId, viewmodel.newsCategory) >= 0) return "news";
            if ($.inArray(categoryId, viewmodel.expertCategory) >= 0) return "expert-reviews";
            if (categoryId == 6) return "features";
        };
        this.get360Url = function (i) {
            var url = (window.isMobile ? "/m/" : "/") + Common.utils.formatSpecial(i.makeName) + "-cars/" + i.modelMaskingName + "/";
            if ($.cookie('_abtest') >= 21 && $.cookie('_abtest') <= 30)
            {
                if (i.is360InteriorAvailable) url += "360-view/interior/";
                else if (i.is360ExteriorAvailable) url += "360-view/";
                else if (i.is360OpenAvailable) url += "360-view/open/";
            }
            else
            {
                if (i.is360ExteriorAvailable) url += "360-view/";
                else if (i.is360OpenAvailable) url += "360-view/open/";
                else if (i.is360InteriorAvailable) url += "360-view/interior/";
            }
            
            return url;
        };
        this.imageUrl = function (data) {
            return (window.isMobile ? "/m/" : "/") + Common.utils.formatSpecial(data.makeName) + "-cars/" + data.modelMaskingName + "/images/";
        };
        this.videoUrl = function (data) {
            data.title = data.title.replace(/[^a-zA-Z 0-9]+/g, '').replace(/\s+/g, ' ');

            if(data.makeName != undefined && data.makeName != "" && data.makeName != null)
                return (window.isMobile ? "/m/" : "/") + Common.utils.formatSpecial(data.makeName) + "-cars/" + data.modelMaskingName + '/videos/' + $.trim(data.title.replace(/ /g, "-").toLowerCase()) + '-' + data.basicId + '/';
            else
                return (window.isMobile ? "/m/" : "/") + 'videos/' + $.trim(data.title.replace(/ /g, '-').toLowerCase()) + '-' + data.basicId + '/';
        };
        this.getFromDateString = function (datestring) {
            var viewmodel = this.self;
            var date = new Date();
            datestring = datestring.replace("T", "-");
            var parts = datestring.split("-");
            var year = parts[0];
            var month = Number(parts[1]) - 1;
            var day = parts[2];
            parts = parts[3].split(':');
            var hours = parts[0];
            var minutes = parts[1];
            var seconds = parts[2];
            var milliseconds = "0";
            var formattedString = viewmodel.months[month] + " " + Number(day) + ", " + year;//+ " " + hours + ":" + minutes;
            return formattedString;
        };
        this.getVisibility = function (item) {
            var currentTab = this.currentTab();
            if (item.CategoryMaskingName.toLowerCase() == "features" && currentTab == "features") return 'hidden';
            else if (item.CategoryMaskingName.toLowerCase() == "news" && currentTab == "all") return 'hidden';
            return '';
        };
        this.getPaginationArray = function () {
            var remainingPagination = 5;
            var total = this.currentCount();
            var currentPage = this.uiPageNumber();
            var pages = currentPage > 1 ? [0] : [];
            for (var i = 1 ; i <= 2; i++) {
                if (currentPage - 1 >= 1) currentPage--;
                else break;
            }
            var i = 0;
            for (i = 0; i < 5; i++) {
                if (((currentPage + i - 1) * this.eachPageSize) + 1 >= total) break;
                else pages[pages.length] = currentPage + i;
            }
            if (i == 5 && (((currentPage + i - 1) * this.eachPageSize) + 1) <= total) pages[pages.length] = -2;
            return pages;
        };
        this.loadData = function (startIndex, endIndex, triggeredByScrollUp, triggeredByFilter) {
            var categories = this.getCategories();
            if (this.currentTab() === "images") {
                news.calls.getMedia(categories, startIndex, endIndex, this.make(), this.model()).done(
                function (result) {
                    if (!$.isEmptyObject(result)) {
                        var dataToPush = { Articles: result.photos.photosList };
                        news.KOVM.ajaxResult(dataToPush);
                        news.KOVM.gotAjaxResultTrigger(false, triggeredByScrollUp, triggeredByFilter);
                    }
                });
            }
            else if (this.currentTab() === "videos") {
                news.calls.getMedia(categories, startIndex, endIndex, this.make(), this.model()).done(
                function (result) {
                    if (!$.isEmptyObject(result)) {
                        var dataToPush = { Articles: result.videos.videosList };
                        news.KOVM.ajaxResult(dataToPush);
                        news.KOVM.gotAjaxResultTrigger(false, triggeredByScrollUp, triggeredByFilter);
                    }
                });
            }
            else {
                news.calls.getArticles(categories, startIndex, endIndex, this.make(), this.model())
                    .done(function (result) {
                        var categories = news.KOVM.getCategories();
                        var existingArray = news.KOVM.current()();
                        var sliceStart = 0;
                        var dataToPush = { Articles: [] };
                        var push = true;
                        for (var i = 0; i < result.Articles.length; i++) {
                            push = true;
                            if (news.KOVM.currentDict[result.Articles[i].BasicId] == result.Articles[i].BasicId && $.inArray(result.Articles[i].CategoryId, categories) == -1) push = false;
                            else {
                                news.KOVM.currentDict[result.Articles[i].BasicId] = result.Articles[i].BasicId;
                                dataToPush.Articles[dataToPush.Articles.length] = result.Articles[i];
                            }
                        }
                        news.KOVM.ajaxResult(dataToPush);
                        news.KOVM.gotAjaxResultTrigger(false, triggeredByScrollUp, triggeredByFilter);
                    })
                    .fail(function (result) {
                    });
            }
        };
        this.processData = function (pageToLoad, isTriggerdByTabChange, scrollUp) {
            if (this.firstProcess) {
                var viewmodel = this;
                $.each(this.current()(), function (a, item) {
                    viewmodel.currentDict[item.BasicId] = item.BasicId;
                });
                this.pagination(this.getPaginationArray());
                this.scrolledPages["currentPage"] = this.uiPageNumber();
                this.firstProcess = false;
            }
            else {
                if (isTriggerdByTabChange) {
                    if (this.scrolledPages["currentPage"] == undefined) this.scrolledPages["currentPage"] = 1;
                    this.uiPageNumber(this.scrolledPages["currentPage"]);
                    this.scrollPageNumber(1);
                    var currentData = this.current()();
                    if (currentData.length < 18) {
                        this.loadData(currentData.length + 1, 18, false, false);
                        this.scrolledPages[1] = true;
                        this.scrolledPages[2] = true;
                    }
                    this.updateHistory();
                    this.pagination(this.getPaginationArray());
                }
                else if (pageToLoad > 0 && this.scrolledPages[pageToLoad] != true) {
                    var start = Math.ceil((pageToLoad - 1) * this.eachPageSize + 1);
                    var end = Math.ceil(pageToLoad * this.eachPageSize);
                    this.scrolledPages[pageToLoad] = true;
                    this.requestInProgress = true;
                    this.loadData(start, end, scrollUp, false);
                }
                else {
                    if (!window.isMobile && window.toLockPopUp) Common.utils.unlockPopup();
                    Common.utils.hideLoading();
                }

            }
            news.trackReadArticles();
        };
        this.tabChangeDataTrigger = function (prevTab) {
            this.processData(this.pageNumberNonComputed, true, false);
            Common.utils.trackAction('CWInteractive', 'EditorialLayout', 'UserChange', news.KOVM.currentTab());

            if (this.currentTabNonComputed != prevTab) {
                var trackLabel = (this.currentTabNonComputed == 'all') ? 'news' : this.currentTabNonComputed.replace('-', '_');

                if (this.model())
                    trackLabel = 'model_' + trackLabel;
                else if (this.make())
                    trackLabel = 'make_' + trackLabel;

                Common.utils.trackAction('CWInteractive', ((window.location.href.indexOf('/m/') > -1) ? 'msite' : 'desktop') + '_content_landing', trackLabel, trackLabel);
            }
        };
        this.triggerPage = function (isNextPage) {
            this.uiPageNumber(this.uiPageNumber() + (isNextPage ? 1 : -1));
            this.scrolledPages["currentPage"] = this.uiPageNumber();
            this.scrollPageNumber(this.scrollPageNumber() + (isNextPage ? 1 : -1));
            this.pageChangeDataTrigger(!isNextPage);
            this.updateHistory();
            Common.utils.firePageView(window.location.pathname);
            this.pagination(this.getPaginationArray());
        };
        this.pageChangeDataTrigger = function (scrollUp) {
            this.pageNumberNonComputed = this.uiPageNumber();

            if (scrollUp) {
                this.processData(this.pageNumberNonComputed - 1, false, true);
            }
            else {
                this.processData(this.pageNumberNonComputed + 1, false, false);
            }
        };
        this.updateHistory = function () {
            var pageNo = this.uiPageNumber();
            var makeDisplayName, modelDisplayName;
            this.updateRedLineStyle();
            if (!(news.ieVersion() < 10) && typeof (history.replaceState) == "function") {
                var category = news.KOVM.currentTab();
                var isNewImageUrl = category == 'images';
                var isVideoUrl = category == 'videos';
                category = (category == "all") ? "/news/" : "/" + category + "/";
                var makeurl = ""
                if (this.make() != null) {
                    makeurl = "/" + this.make().name + "-cars";
                    makeDisplayName = this.make().displayName;
                    var modelurl = "";
                    if (this.model() != null && !isVideoUrl) {
                        modelurl = "/" + this.model().name;
                        modelDisplayName = this.model().displayName
                    }

                    category = makeurl + modelurl + category;
                }
                var pageurl = (pageNo > 1 ? (!window.isMobile && news.KOVM.currentTab() == "expert-reviews" ? "p" : "page/") + pageNo + "/" : "");
                category = (isNewImageUrl ? this.getNewImageUrl(pageurl) : (category + pageurl))

                var url = ((window.isMobile ? "/m" : "") + category);

                if (this.make() != null && !isNewImageUrl && isVideoUrl) {
                    var modelUrl = "";
                    if (this.model() != null) {
                        modelUrl = "?model=" + this.model().name;
                        modelDisplayName = this.model().displayName
                    }
                    url = url + modelUrl;
                }
                
                if (pageNo > 1) { $('#back-to-top').fadeIn(); }
                else { $('#back-to-top').fadeOut(); }

                var title = this.h1Tag() + ", Auto News India " + "- CarWale";
                history.replaceState(pageNo, title, url);
                news.setTitle(this.currentTab(), makeDisplayName, modelDisplayName);
            }
        };

        this.getNewImageUrl = function (pageUrl) {
            var url = "/images/" + pageUrl;
            var isFirstParam = true;
            if (this.make() != null) {
                url += this.formQueryParameter(isFirstParam, 'make', this.make().name);
                isFirstParam = false;
            }
            if (this.model() != null) {
                url += this.formQueryParameter(isFirstParam, 'model', this.model().name);
                isFirstParam = false;
            }
            return url;
        };

        this.formQueryParameter = function (isFirstParam, key, value) {
            return ((isFirstParam ? '?' : '&') + key + '=' + value);
        };

        this.gotAjaxResultTrigger = function (appendToStart, triggeredByScrollUp, triggeredByFilter) {
            var result = this.ajaxResult();
            if (typeof (news.KOVM) != "undefined") {


                if (triggeredByScrollUp)
                    news.KOVM.current()(result.Articles.concat(news.KOVM.current()()));
                else
                    ko.utils.arrayPushAll(news.KOVM.current(), result.Articles);
                if (!window.isMobile && window.toLockPopUp) Common.utils.unlockPopup();
                Common.utils.hideLoading();
                this.requestInProgress = false;
                setTimeout(news.listing.postFilterAction, 0);
                this.pagination(this.getPaginationArray());
            }
        };
        this.getCategories = function (all) {
            if (all == true) return this.expertCategory.concat([6]).concat(this.newsCategory);
            var categories = null;
            switch (this.currentTab()) {
                case 'features': categories = [6]; break;
                case 'expert-reviews': categories = this.expertCategory; break;
                case 'images': categories = [10]; break;
                case 'videos': categories = [13]; break;
                default: categories = this.expertCategory.concat([6]).concat(this.newsCategory);
            };
            return categories;
        };
        this.scroll = function () {
            var all = $(news.listing.newsBox);
            var inViewport = all.filter(":in-viewport");
            var currentPage = this.scrollPageNumber();
            var currentPageStartIndex = ((currentPage - 1) * this.eachPageSize) + 1;
            var currentPageEndIndex = currentPage * this.eachPageSize;
            var startIndexInView = inViewport.first().index() + 1;
            var endIndexInView = inViewport.last().index() + 1;
            if (startIndexInView >= currentPageStartIndex + 9 && endIndexInView <= currentPageEndIndex + 9 && !this.requestInProgress) {
                this.triggerPage(true);
            }
            else if (this.uiPageNumber() != 1 && startIndexInView >= currentPageStartIndex - 9 && endIndexInView <= currentPageEndIndex - 9 && !this.requestInProgress) {
                this.triggerPage(false);
            }
            else if (endIndexInView <= currentPageStartIndex) {
                this.triggerPage(false);
            }
            else if (startIndexInView >= currentPageEndIndex) {
                this.triggerPage(true);
            }
        };

    },
    applyKO: function (vm) {
        ko.applyBindings(vm, document.getElementById('viewUl'));
        ko.applyBindings(vm, document.getElementById('divStrip'));
        ko.applyBindings(vm, document.getElementById('datatabs'));
        ko.applyBindings(vm, document.getElementById('h1Tag'));
        ko.applyBindings(vm, document.getElementById('liCrumb'));
        ko.applyBindings(vm, document.getElementById('datatabstop'));
    },
    unapplyKO: function () {
        ko.cleanNode(document.getElementById('viewUl'));
        ko.cleanNode(document.getElementById('divStrip'));
        ko.cleanNode(document.getElementById('datatabs'));
        ko.cleanNode(document.getElementById('h1Tag'));
        ko.cleanNode(document.getElementById('liCrumb'));
        ko.cleanNode(document.getElementById('datatabstop'));
    },
    trackReadArticles: function () {
        if (window.localStorage && localStorage.BasicId != undefined) {
            if (localStorage.BasicId != null)
                var readBasicIds = localStorage.BasicId.split('|');
            for (var basicid in readBasicIds) {
                var element = $('.article-read-' + readBasicIds[basicid]);
            }
        }
    }
}
doNotShowAskTheExpert = false;
Common.utils.hideLoading();
news.initialize();

var fixedTabShown = false;
var prevScroll = 0;
var $window = $(window);
$(window).on("scroll", function () {
    var nextScroll = window.scrollY;
    if (nextScroll > $("#datatabs").offset().top && nextScroll < prevScroll) {
        if (!fixedTabShown && !Common.isScrollLocked) {
            $(".fixed-tabs").show();
            news.KOVM.updateRedLineStyle();
            if (news.mobile()) {
                news.listing.animateMenu('#fixedMenu');
            }
            fixedTabShown = true;
        }
    }
    else {
        if (fixedTabShown) {
            $(".fixed-tabs").hide();
            fixedTabShown = false;
        }
    }
    prevScroll = nextScroll;
    if (window.isMobile) {
        var $mainMenuBar = $('.content-filter-tabs'),
                $floatMenuAnchor = $('#floatMenuAnchor');
        var window_top = $window.scrollTop();
        var div_top = $floatMenuAnchor.offset().top;
        if (window_top > div_top) {
            // Make the Filter Tabs sticky.
            $mainMenuBar.addClass('fixed');
            $floatMenuAnchor.height($mainMenuBar.height());
        }
        else {
            // Unstick the Filter Tabs.
            $mainMenuBar.removeClass('fixed');
            $floatMenuAnchor.height(0);
        }
    }
});

