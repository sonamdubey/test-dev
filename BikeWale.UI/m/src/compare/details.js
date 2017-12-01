var floatingCard, floatingCardHeight, comparisonFooter, overallSpecsTabs, floatingButton, $window, windowScrollTop,
    onRoadPriceButtons, closedBikeCount, compareSource, vmBikeSelection, bikePopup, panel;
var hideCheckbox = $(".hideCheckbox");

var setButton = {
    completeText: function (element, message) {
        $(element).text(message);
    }
};

function setButtonText() {
    if ($window.width() == 320 || ($window.width() >= 360 && document.getElementById("sponsored-column-active") == null) || $window.width() >= 480) {
        setButton.completeText(onRoadPriceButtons, 'Check on-road price');
    }
    else {
        setButton.completeText(onRoadPriceButtons, 'On-road price');
    }
};




var bodyElement = document.getElementsByTagName("body")[0],
    toggleFeaturesBtn = document.getElementById("toggle-features-btn"),
    hideCommonFeatures = true,
    equivalentDataFound = false,
    hideFeaturesClasses = "btn btn-teal btn-full-width",
    showFeaturesClasses = "btn btn-inv-teal btn-full-width";

toggleFeaturesBtn.addEventListener("click", function () {
    if (hideCommonFeatures) {
        if (!equivalentDataFound) {
            var headingRows = $(".hide-features").find(".row-type-heading"),
                dataRows = $(".hide-features").find(".row-type-data"),
                isSponsoredBikeActive = document.getElementById("sponsored-column-active");

            if (isSponsoredBikeActive == null) {
                compareColumns.countTwo(headingRows, dataRows);
            }
            else {
                compareColumns.countThree(headingRows, dataRows);
            }

            equivalentDataFound = true;
        }
        bodyElement.className = "hide-equivalent-data";

        toggleFeaturesBtn.className = showFeaturesClasses;
        toggleFeaturesBtn.innerHTML = "Show all features";

        hideCommonFeatures = false;
    }
    else {
        bodyElement.className = "show-equivalent-data";

        toggleFeaturesBtn.className = hideFeaturesClasses;
        toggleFeaturesBtn.innerHTML = "Hide common features";

        hideCommonFeatures = true;
    }

});

var compareColumns = {
    countTwo: function (headingRows, dataRows) {
        var dataRowLength = dataRows.length;
        for (var i = 0; i < dataRowLength; i++) {
            var rowElement = dataRows[i],
                rowColumns = rowElement.getElementsByTagName("td");

            if (rowColumns[0].innerHTML === rowColumns[1].innerHTML) {
                rowElement.className += " equivalent-data";
                if (headingRows[i]) {
                    headingRows[i].className += " equivalent-data";
                }
            }

        }
    },

    countThree: function (headingRows, dataRows) {
        var dataRowLength = dataRows.length;

        for (var i = 0; i < dataRowLength; i++) {
            var rowElement = dataRows[i],
                rowColumns = rowElement.getElementsByTagName("td");

            if (rowColumns[0].innerHTML === rowColumns[1].innerHTML) {
                if (rowColumns[1].innerHTML === rowColumns[2].innerHTML) {
                    rowElement.className += " equivalent-data";
                    if (headingRows[i]) {
                        headingRows[i].className += " equivalent-data";
                    }
                }
            }

        }
    }
};

var closeSponsoredBikeBtn = document.getElementById("close-sponsored-bike");
if (closeSponsoredBikeBtn) {
    closeSponsoredBikeBtn.addEventListener("click", function () {
        document.getElementById("sponsored-column-active").removeAttribute("id");
        equivalentDataFound = false;
        setButtonText();
    });
}

var compareBox = {
    removeBike: function (element) {
        var detailsBlock = $(element).closest('.bike-details-block'),
            detailsBlockIndex = detailsBlock.index();

        compareBox.setBikePlaceholder(detailsBlock, detailsBlockIndex);

        compareBox.emptyBikeTable(detailsBlockIndex);
        compareBox.removeFloatingCardBike(detailsBlockIndex);
    },

    emptyBikeTable: function (elementIndex) {
        var dataRows = $('.table-content .row-type-data'),
            dataRowLength = dataRows.length;

        for (var i = 0; i < dataRowLength; i++) {
            var element = dataRows[i];
            $(element).find('td:eq(' + elementIndex + ')').empty();
        };

    },

    removeFloatingCardBike: function (elementIndex) {
        var detailsBlock = $('#comparison-floating-card .bike-details-block:eq(' + elementIndex + ')');

        compareBox.setBikePlaceholder(detailsBlock, elementIndex);
    },

    setBikePlaceholder: function (element, elementIndex) {
        $(element).empty();
        $(element).attr("data-changed", true);
        var placeholderBlock = "<div class='compare-box-placeholder'><div class='bike-icon-wrapper'><span class='grey-bike'></span><p class='font14 text-light-grey'>Tap to select bike " + (elementIndex + 1) + "</p></div></div>";
        $(element).append(placeholderBlock);

    }
};

var dropdown = {
    setDropdown: function () {
        var selectDropdown = document.getElementsByClassName('dropdown-select'),
            selectDropdownLength = selectDropdown.length;

        for (var i = 0; i < selectDropdownLength; i++) {
            dropdown.setMenu(selectDropdown[i]);
        }
    },

    setMenu: function (element) {
        var menuElement = document.createElement("div"),
            classes = ["dropdown-menu", "dropdown-width"]

        menuElement.className = classes.join(" ");

        element.parentNode.insertBefore(menuElement, element.nextSibling);
        dropdown.setStructure(element);
    },

    setStructure: function (element) {
        var elementText = element.options[element.selectedIndex].text,
            optionLength = element.options.length,
            selectLabel = element.getAttribute("data-title"),
            dropdownMenu = element.nextSibling;

        var selectedItem = document.createElement("p");
        selectedItem.className = "dropdown-selected-item";
        selectedItem.innerHTML = elementText;

        dropdownMenu.appendChild(selectedItem);

        var dropdownListWrapper = document.createElement("div");
        dropdownListWrapper.className = "dropdown-list-wrapper";

        var dropdownLabel = document.createElement("p");
        dropdownLabel.className = "dropdown-label";
        dropdownLabel.innerHTML = selectLabel;

        var dropdownList = document.createElement("ul");
        dropdownList.className = "dropdown-menu-list";

        for (var i = 0; i < optionLength; i++) {
            var optionItem = element.options[i],
                listOption = document.createElement("li");

            listOption.setAttribute("data-option-value", optionItem.value);
            listOption.innerHTML = optionItem.text;

            if (optionItem.selected) {
                listOption.className = "option-active";
            }

            dropdownList.appendChild(listOption);
        }

        dropdownListWrapper.appendChild(dropdownLabel);
        dropdownListWrapper.appendChild(dropdownList);

        dropdownMenu.appendChild(dropdownListWrapper);
    }
};

var dropdownInteraction = {
    activate: function (label) {
        $('.dropdown-select-wrapper').find('.dropdown-menu').removeClass('dropdown-active');
        label.closest('.dropdown-menu').addClass('dropdown-active');
    },

    deactivate: function () {
        $('.dropdown-select-wrapper').find('.dropdown-menu').removeClass('dropdown-active');
    },

    selectItem: function (element) {
        var elementText = element.text(),
            menu = element.closest('.dropdown-menu'),
            selectedItem = menu.find('.dropdown-selected-item');

        element.siblings('li').removeClass('option-active');
        element.addClass('option-active');
        selectedItem.text(elementText);
    },

    selectOption: function (element) {
        var elementValue = element.attr('data-option-value'),
            wrapper = element.closest('.dropdown-select-wrapper'),
            selectDropdown = wrapper.find('.dropdown-select');

        selectDropdown.val(elementValue).trigger('change');

    },

    dimension: function () {
        var windowWidth = dropdown.deviceWidth();
        if (windowWidth > 480) {
            dropdown.resizeWidth(windowWidth);
        }
        else {
            $('.dropdown-select-wrapper').find('.dropdown-list-wrapper').css('width', 'auto');
        }
    },

    deviceWidth: function () {
        var windowWidth = $(window).width();
        return windowWidth;
    },

    resizeWidth: function (newWidth) {
        $('.dropdown-select-wrapper').find('.dropdown-list-wrapper').css('width', newWidth / 2);
    }
}


var bikeSelection = function () {
    var self = this;

    self.makes = ko.observableArray();
    self.modelArray = ko.observableArray();
    self.versionArray = ko.observableArray();
    self.makeId = ko.observable('');
    self.modelId = ko.observable('');
    self.versionId = ko.observable('');
    self.compareSource = ko.observable(compareSource);
    self.areMakesLoaded = false;

    self.openPopup = function (data, event) {

        bikePopup.stageMake();
        bikePopup.scrollToHead();

        try {
            $.ajax({
                type: "GET",
                async: false,
                url: "/api/makelist/?requestType=2",
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if (response) {
                        self.makes(response.makes);
                        areMakesLoaded = true;
                    }
                }
            });
        } catch (e) {
            console.warn(e);
        }
    }

    self.returnCompareUrl = function (toBeReplacedModelId, newModelId, newmasking) {
        var compareUrl;
        var first = $(".comparison-main-card .bike-details-block")[0];
        var second = $(".comparison-main-card .bike-details-block")[1];
        if ($(first).data('modelid') == toBeReplacedModelId) {
            if (newModelId > $(second).data('modelid')) {
                compareUrl = $(second).data('masking') + '-vs-' + newmasking;
            } else {
                compareUrl = newmasking + '-vs-' + $(second).data('masking');
            }

        } else {
            if (newModelId > $(first).data('modelid')) {
                compareUrl = $(first).data('masking') + '-vs-' + newmasking;
            } else {
                compareUrl = newmasking + '-vs-' + $(first).data('masking');
            }
        }
        return compareUrl;
    }
    self.redirectionUrl = function () {
        var _link = "";
        try {
            if (self.makeId() > 0) {
                var makemasking = $("#select-make-wrapper ul li[data-id='" + self.makeId() + "']").data("masking");
                if (self.modelId() > 0) {
                    var modelmasking = $("#select-model-wrapper ul li[data-id='" + self.modelId() + "']").data("masking");
                    if (self.versionId() > 0) {
                        var ele = $(".comparison-main-card .bike-details-block[data-changed='true']"), loc = window.location;
                        var newmasking = makemasking + "-" + modelmasking;
                        var compareUrl = self.returnCompareUrl(ele.data("modelid"), self.modelId(), newmasking);
                        _link = '/m/comparebikes/' + compareUrl;
                        if (window.location.search.indexOf("bike") > -1) {
                            _link = _link + loc.search.replace(ele.data("versionid"), self.versionId());
                            _link.replace(/source=\d/, 'source=' + self.compareSource());
                        }
                        else {
                            var searchQuery = "?";
                            $(".bike-details-block").each(function (i) {
                                var el = $(this);
                                if (!el.hasClass('sponsored-bike-details-block') && el.data("versionid")) {
                                    searchQuery += ("&bike" + (i + 1) + "=" + el.data("versionid"));
                                }
                            });
                            searchQuery = searchQuery.replace(ele.data("versionid"), self.versionId());
                            _link += (searchQuery + (searchQuery != "" ? "&source=" + self.compareSource() : ""));
                        }
                    }
                }
            }
        } catch (e) {
            console.warn(e);
        }

        return _link;
    };

    self.makeChanged = function (data, event) {

        var element = $(event.currentTarget);

        self.makeId(element.attr("data-id"));

        bikePopup.stageModel();
        bikePopup.scrollToHead();

        try {
            if (self.makeId()) {
                $.ajax({
                    type: "Get",
                    async: false,
                    url: "/api/modellist/?requestType=2&makeId=" + self.makeId(),
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {
                        if (response) {
                            self.modelArray(response.modelList);
                        }
                    },
                    complete: function (xhr) {
                        if (xhr.status != 200) {
                            self.makeId();
                        }
                    }
                });
            }
        } catch (e) {
            console.warn(e);
        }

    };

    self.modelChanged = function (data, event) {

        self.modelId(data.modelId);

        bikePopup.stageVersion();
        bikePopup.scrollToHead();

        try {
            if (self.modelId()) {
                $.ajax({
                    type: "Get",
                    url: "/api/versionList/?requestType=2&modelId=" + self.modelId(),
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {
                        self.versionArray(response.Version);
                    }
                });
            }
        } catch (e) {
            console.warn(e);
        }

    };

    self.versionChanged = function (data, event) {
        self.versionId(data.versionId);
        $('#select-version-wrapper .same-version-toast').hide();
        if (!bikePopup.checkSameVersion(self.versionId()) && self.versionId() > 0) {
            window.location = self.redirectionUrl();
        }
        else {
            bikePopup.showSameVersionToast();
        }

    };

    self.modelBackBtn = function () {
        bikePopup.stageMake();
    };

    self.versionBackBtn = function () {
        bikePopup.stageModel();
    };

    self.closeBikePopup = function () {
        bikePopup.close();
        history.back();
    };
};



var effect = 'slide',
    optionRight = { direction: 'right' },
    duration = 500;


var appendState = function (state) {
    window.history.pushState(state, '', '');
};


docReady(function () {

    floatingCard = $('#comparison-floating-card');
    floatingCardHeight = floatingCard.height() - 44;
    comparisonFooter = $('#comparison-footer');
    overallSpecsTabs = $('#overall-specs-tabs');
    floatingButton = $('#toggle-float-button');
    $window = $(window);
    windowScrollTop = $window.scrollTop();
    onRoadPriceButtons = $('.bike-orp-btn'); closedBikeCount = 0;
    compareSource = $("#compare-detailspage").data("comparesource");

    bikePopup = {

        container: $('#select-bike-cover-popup'),

        loader: $('.cover-popup-loader-body'),

        makeBody: $('#select-make-wrapper'),

        modelBody: $('#select-model-wrapper'),

        versionBody: $('#select-version-wrapper'),

        open: function () {
            bikePopup.container.show(effect, optionRight, duration, function () {
                bikePopup.container.addClass('extra-padding');
            });

            $('html, head').addClass('lock-browser-scroll');
        },

        close: function () {
            bikePopup.container.hide(effect, optionRight, duration, function () {
                bikePopup.stageMake();
            });

            bikePopup.container.removeClass('extra-padding');
            $('html, head').removeClass('lock-browser-scroll');
        },

        stageMake: function () {
            bikePopup.modelBody.hide();
            bikePopup.versionBody.hide();
            bikePopup.makeBody.show();
        },

        stageModel: function () {
            bikePopup.makeBody.hide();
            bikePopup.versionBody.hide();
            bikePopup.modelBody.show();
        },

        stageVersion: function () {
            bikePopup.makeBody.hide();
            bikePopup.modelBody.hide();
            bikePopup.versionBody.show();
        },

        showLoader: function () {
            bikePopup.container.find(bikePopup.loader).show();
        },

        hideLoader: function () {
            bikePopup.container.find(bikePopup.loader).hide();
        },

        scrollToHead: function () {
            bikePopup.container.animate({ scrollTop: 0 });
        },
        checkSameVersion: function (versionId) {
            var isSameVersionSelected = false;
            $(".bike-details-block").each(function () {
                var ele = $(this);
                if (!ele.hasClass('sponsored-bike-details-block') && ele.data("versionid") == versionId && ele.data("changed").toString() != 'true') {
                    isSameVersionSelected = true;
                }
            });

            return isSameVersionSelected;
        },
        showSameVersionToast: function () {
            window.clearTimeout();
            $('section .same-version-toast').slideDown();
            window.setTimeout(function () {
                $('section .same-version-toast').slideUp();
            }, 2000);
        }
    };

    floatingButton.addClass('fixed-floater');
    setButtonText();

    (function () {
        var dataRows = $('.table-content tr.row-type-data td');
        var tickIcon = '<span class="bwmsprite tick-grey"></span>', crossIcon = '<span class="bwmsprite cross-grey"></span>';
        dataRows.each(function () {
            var td = $(this), tdText = $.trim(td.text());
            if (tdText == "Yes") td.html(tickIcon);
            else if (tdText == "No") td.html(crossIcon);
        });

    }());

    $('.comparison-main-card').on('click', '.close-selected-bike', function () {

        if (!$(this).parent().hasClass("sponsored-bike-details-block")) {
            closedBikeCount++;
        }

        if (closedBikeCount == 2) {
            window.location.href = '/m/comparebikes/';
        }
        else {
            compareBox.removeBike($(this));
        }
    });

    dropdown.setDropdown();



    $('.overall-specs-tabs-wrapper').on('click', 'li', function () {
        var elementIndex = $(this).index(),
            tabId = $(this).attr('data-tabs'),
            panel = $(this).closest('.bw-tabs-panel'),
            floatingTabs = panel.find('.overall-specs-tabs-wrapper');

        if (elementIndex < 4) { 
            $('html, body').animate({ scrollTop: Math.round($('#' + tabId).offset().top - (floatingCardHeight + 48)) }, 500);
        }
        else {
            $('html, body').animate({ scrollTop: Math.round($('#' + tabId).offset().top - 40) }, 500);
        }

        centerItVariableWidth($(this), '#overallSpecsTab');
    });
    var windowHeight = $window.height();
    var modelSpecsTabsContentWrapper = $('#overallSpecsTabContainer');
    var tabElementThird = modelSpecsTabsContentWrapper.find('.bw-tabs-data:eq(2)'),
        tabElementSixth = modelSpecsTabsContentWrapper.find('.bw-tabs-data:eq(4)'),
        tabElementNinth = modelSpecsTabsContentWrapper.find('.bw-tabs-data:eq(8)');
    $window.on('scroll', function () {
        var overallSpecsOffset = overallSpecsTabs.offset().top - floatingCardHeight,
            bwTab = overallSpecsTabs.offset().top - floatingCardHeight
        footerOffsetForCard = Math.round($('.bw-tabs-panel').height() - overallSpecsOffset);
         if ($window.scrollTop() < windowScrollTop) { 
            floatingButton.addClass('fixed-floater');

            if ($window.scrollTop() > footerOffsetForCard) {
                floatingButton.removeClass('fixed-floater');
            }
        }
        else {
            floatingButton.removeClass('fixed-floater');
        }

        windowScrollTop = $window.scrollTop();

        if (windowScrollTop > overallSpecsOffset) {
            floatingCard.addClass('fixed-card');
            floatingCard.find('.overall-specs-tabs-container').removeClass('fixed-overall-tab');
            if (windowScrollTop > (footerOffsetForCard + overallSpecsOffset)) {
                floatingCard.removeClass('fixed-card');
                floatingCard.find('.overall-specs-tabs-container').addClass('fixed-overall-tab');
            }
        }
        else if (windowScrollTop < overallSpecsOffset) {
            floatingCard.removeClass('fixed-card');
        }
        var bwTabs=$('#overallSpecsTabContainer').find('.bw-tabs-data').length;
        $('#overallSpecsTabContainer .bw-tabs-data').each(function () {
            var top, bottom;
            if ($(this).index() != 0) {
            top = $(this).offset().top - (floatingCardHeight + 50);
            }
            else {
                top = $(this).offset().top - 44;
            }
            bottom = top + $(this).outerHeight();
            if (windowScrollTop >= top && windowScrollTop <= bottom) {
                floatingTabs.find('li').removeClass('active');
                $('#overallSpecsTabContainer .bw-tabs-data').removeClass('active');

                $(this).addClass('active');

                var currentActiveTab = floatingTabs.find('li[data-tabs="' + $(this).attr('data-id') + '"]');
                floatingTabs.find(currentActiveTab).addClass('active');


            }
        });

        if (tabElementThird.length != 0) {
            focusFloatingTab(tabElementThird, 250, 0);
        }

        if (tabElementSixth.length != 0) {
            focusFloatingTab(tabElementSixth, 500, 250);
        }

        if (tabElementNinth.length != 0) {
            focusFloatingTab(tabElementNinth, 750, 500);
        }

        function focusFloatingTab(element, startPosition, endPosition) {
            if (windowScrollTop > element.offset().top - 45) {
                if (!$('#overallSpecsTab').hasClass('scrolled-left-' + startPosition)) {
                    $('.overall-specs-tabs-container').addClass('scrolled-left-' + startPosition);
                    scrollHorizontal(startPosition);
                }
            }

            else if (windowScrollTop < element.offset().top) {
                if ($('#overallSpecsTab').hasClass('scrolled-left-' + startPosition)) {
                    $('.overall-specs-tabs-container').removeClass('scrolled-left-' + startPosition);
                    scrollHorizontal(endPosition);
                }
            }

        };
    });



    function scrollHorizontal(pos) {
        $('.overall-specs-tabs-container').animate({ scrollLeft: pos - 50 }, 500);
    }

    var floatingTabs = $('.overall-specs-tabs-wrapper'),
        floatingTabLength = floatingTabs.length;

    for (var i = 0; i < floatingTabLength; i++) {
        var element = floatingTabs[i];
        if ($(element).find('li').length < 4) {
            $(element).addClass('inline-tabs');
        }
    }

    $('a.read-more-model-preview').click(function () {
        if (!$(this).hasClass('open')) {
            var self = $(this);
            $('.model-preview-main-content').hide();
            $('.model-preview-more-content').show();
            self.text(self.text() === 'Read more' ? 'Collapse' : 'Read more');
            self.addClass("open");
        }
        else if ($(this).hasClass('open')) {
            var self = $(this);
            $('.model-preview-main-content').show();
            $('.model-preview-more-content').hide();
            self.text(self.text() === 'Read more' ? 'Collapse' : 'Read more');
            self.removeClass('open');
        }
    });

    if ($(".sponsored-bike-details-block") && $(".sponsored-bike-details-block").length > 0) {
        var sponsoredBike = $(".sponsored-bike-details-block").data("bikename");
        dataLayer.push({ "event": "Bikewale_noninteraction", "cat": "Comparison_Page", "act": "Sponsored_Version_Shown", "lab": sponsoredBike });
    }

    if ($(".know-more-btn-shown") && $(".know-more-btn-shown").length > 0) {
        var sponsoredBike = $(".sponsored-bike-details-block").data("bikename");
        dataLayer.push({ "event": "Bikewale_noninteraction", "cat": "Comparison_Page", "act": "Sponsored_Comparison_Know_more_shown", "lab": sponsoredBike });
    }



    $(window).resize(function () {
        setButtonText();
    });



    $(document).on('click', '.compare-box-placeholder', function () {
        if (vmBikeSelection && !vmBikeSelection.areMakesLoaded) {
            vmBikeSelection.openPopup(null, event);
        }
        bikePopup.open();
        appendState('selectBike');
    });

    $('.dropdown-select-wrapper').on('click', '.dropdown-menu-list li', function () {
        var element = $(this), selValue = element.attr("data-option-value");
        try {
           
            if (!element.hasClass('option-active') && !bikePopup.checkSameVersion(selValue)) {
                var preSel = element.siblings(".option-active").first();
                dropdownInteraction.selectItem(element);
                dropdownInteraction.selectOption(element);
                element.closest("div.bike-details-block").attr("data-versionId", selValue);
                if (window.location.search.indexOf("bike") > -1) {
                    var qrStr = window.location.search;
                    var container = element.closest("div.bike-details-block");
                    if (container.hasClass("sponsored-bike-details-block")) {
                        if (qrStr.indexOf("sponsoredbike") > -1) {
                            qrStr = qrStr.replace(preSel.attr("data-option-value"), selValue);
                        }
                        else if (qrStr.indexOf("?") > -1) {
                            qrStr += "&sponsoredbike=" + selValue;
                        }
                        else {
                            qrStr += "?sponsoredbike=" + selValue;
                        }
                    }
                    else {
                        qrStr = qrStr.replace(preSel.attr("data-option-value"), selValue);
                    }
                    window.location.search = qrStr;
                }
                else {
                    var searchQuery = "?";
                    $(".bike-details-block").each(function (i) {
                        var curEle = $(this);
                        if (!curEle.hasClass('sponsored-bike-details-block') && curEle.attr("data-versionid")) {
                            searchQuery += ("&bike" + (i + 1) + "=" + curEle.attr("data-versionid"));
                        }
                        else if (curEle.hasClass('sponsored-bike-details-block') && i < 3) {
                            searchQuery += ("&sponsoredbike=" + selValue);
                        }
                    });
                    window.location.search = searchQuery + (searchQuery != "" ? "&source=" + compareSource : "");
                }
            }
            else {
                bikePopup.showSameVersionToast();
            }
        } catch (e) {
            console.log(e.message);
        }
    });

    $('.dropdown-select-wrapper').on('click', '.dropdown-selected-item', function () {
        dropdownInteraction.activate($(this));
    });

    

    $(document).on('click', function (event) {
        if ($('.dropdown-list-wrapper').is(':visible')) {
            event.stopPropagation();
            var bodyElement = $('body'),
                dropdownLabel = bodyElement.find('.dropdown-label'),
                dropdownList = bodyElement.find('.dropdown-menu-list'),
                noSelectLabel = bodyElement.find('.dropdown-selected-item');

            if (!$(event.target).is(dropdownLabel) && !$(event.target).is(dropdownList) && !$(event.target).is(noSelectLabel)) {
                dropdownInteraction.deactivate();
            }
        }
    });

    $(window).on('popstate', function (event) {
        if ($('#select-bike-cover-popup').is(':visible')) {
            bikePopup.close();
        }
    });

    vmBikeSelection = new bikeSelection();
    if ($('#select-bike-cover-popup') && $('#select-bike-cover-popup').length) {
        ko.applyBindings(vmBikeSelection, $('#select-bike-cover-popup')[0]);
    }

    var gallerySwiper = new Swiper('#similar-bikes-swiper', {
        effect: 'slide',
        speed: 300,
        slidesPerView: 'auto',
        spaceBetween: 10,
        preloadImages: false,
        lazyLoading: true,
        lazyLoadingInPrevNext: true,
        watchSlidesProgress: true,
        watchSlidesVisibility: true,
        onTouchStart: function (swiper, event) {
            if ($("#comparisonText").length > 0) {
                triggerGA("Compare_Bikes", "Clicked_on_carousel", $("#comparisonText").val());
            }
            else
            {
                triggerGA("Compare_Bikes", "Clicked_on_carousel", $("#similar-bikes-swiper").data('comptext'));
            }
        }           
    });

    $(".reviewTab").on('click', function () {
        hideCheckbox.hide();
    })

    $(".quickAcessTab").on('click', function () {
        if (hideCheckbox.is(":hidden")) {
            hideCheckbox.show();
        }
    })
    
});
