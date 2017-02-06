var floatingCard = $('#comparison-floating-card'),
    floatingCardHeight = floatingCard.height() - 44,
    comparisonFooter = $('#comparison-footer'),
    overallSpecsTabs = $('#overall-specs-tabs'),
    floatingButton = $('#toggle-float-button'),
    $window = $(window),
    windowScrollTop = $window.scrollTop(),
    onRoadPriceButtons = $('.bike-orp-btn'), closedBikeCount = 0, compareSource = 0;

floatingButton.addClass('fixed-floater');

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

(function () {
    var dataRows = $('.table-content tr.row-type-data td');
    var tickIcon = '<span class="bwmsprite tick-grey"></span>',crossIcon='<span class="bwmsprite cross-grey"></span>';
    dataRows.each(function () {
        var td = $(this), tdText = $.trim(td.text());
        if (tdText == "Yes") td.html(tickIcon);
        else if (tdText == "No") td.html(crossIcon);
    });

}());

setButtonText();

$(window).resize(function () {
    setButtonText();
});

$(document).ready(function () {    
    dropdown.setDropdown();

    var windowHeight = $window.height();

    $window.on('scroll', function () {
        var overallSpecsOffset = overallSpecsTabs.offset().top - floatingCardHeight,
            footerOffsetForButton = comparisonFooter.offset().top - windowHeight,
            footerOffsetForCard = comparisonFooter.offset().top - floatingCardHeight - 88;

        if ($window.scrollTop() < windowScrollTop) { // current scroll position < previous position
            floatingButton.addClass('fixed-floater');

            if ($window.scrollTop() > footerOffsetForButton) {
                floatingButton.removeClass('fixed-floater');
            }
        }
        else {
            floatingButton.removeClass('fixed-floater');
        }

        windowScrollTop = $window.scrollTop();

        if (windowScrollTop > overallSpecsOffset) {
            floatingCard.addClass('fixed-card');

            if (windowScrollTop > footerOffsetForCard) {
                floatingCard.removeClass('fixed-card');
            }
        }
        else if (windowScrollTop < overallSpecsOffset) {
            floatingCard.removeClass('fixed-card');
        }

    });

    var floatingTabs = $('.overall-specs-tabs-wrapper'),
        floatingTabLength = floatingTabs.length;

    for (var i = 0; i < floatingTabLength; i++) {
        var element = floatingTabs[i];
        if ($(element).find('li').length < 4) {
            $(element).addClass('inline-tabs');
        }
    }



});


$(document).on('click', '.compare-box-placeholder', function () {
    bikePopup.open();
    appendState('selectBike');
});

$('.dropdown-select-wrapper').on('click', '.dropdown-menu-list li', function () {
    var element = $(this);
    if (!element.hasClass('option-active') && !bikePopup.checkSameVersion(element.data("option-value"))) {
        preSel = element.siblings(".option-active").first();
        dropdownInteraction.selectItem(element);
        dropdownInteraction.selectOption(element);
        if (window.location.search.indexOf("bike") > -1)
        {
            window.location.search = window.location.search.replace(preSel.data("option-value"), element.data("option-value"));
        }
        else {
            var searchQuery = "?";
            $(".bike-details-block").each(function (i) {
                if(!$(this).hasClass('sponsored-bike-details-block') && $(this).data("versionid"))
                {
                    searchQuery += "&bike=" + (i + 1) + $(this).data("versionid");
                }
            });
            window.location.search = searchQuery + searchQuery != "" ? "&source=" + compareSource : "";
        }       
    }
    else {
        bikePopup.showSameVersionToast();
    }
});

$('.dropdown-select-wrapper').on('click', '.dropdown-selected-item', function () {
    dropdownInteraction.activate($(this));
});

/* accordion tab */
$('.model-accordion-tab').on('click', function () {
    var tab = $(this),
        allTabs = $('.model-accordion-tab');

    if (!tab.hasClass('active')) {
        allTabs.removeClass('active');
        tab.addClass('active');
        $('html, body').animate({ scrollTop: tab.offset().top - floatingCardHeight - 44 }, 500); // 44px accordion tab height
    }
    else {
        tab.removeClass('active');
    }
});

/* floating tabs */
$('.overall-specs-tabs-wrapper').on('click', 'li', function () {
    var elementIndex = $(this).index(),
        tabId = $(this).attr('data-tabs'),
        panel = $(this).closest('.bw-tabs-panel'),
        floatingTabs = panel.find('.overall-specs-tabs-wrapper');

    floatingTabs.find('li').removeClass('active');
    floatingTabs.each(function () {
        $(this).find('li:eq(' + elementIndex + ')').addClass('active');
    });

    panel.find('.bw-tabs-data').removeClass('active');
    $('#' + tabId).addClass('active');
    $('html, body').animate({ scrollTop: overallSpecsTabs.offset().top - floatingCardHeight + 7 }, 500); // 44px accordion tab height
});

/* toggle common features */
var bodyElement = document.getElementsByTagName("body")[0],
    toggleFeaturesBtn = document.getElementById("toggle-features-btn"),
    hideCommonFeatures = true,
    equivalentDataFound = false,
    hideFeaturesClasses = "btn btn-teal btn-full-width",
    showFeaturesClasses = "btn btn-inv-teal btn-full-width";

toggleFeaturesBtn.addEventListener("click", function () {
    if (hideCommonFeatures) {
        if (!equivalentDataFound) {
            var headingRows = document.getElementsByClassName("row-type-heading"),
                dataRows = document.getElementsByClassName("row-type-data"),
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
                headingRows[i].className += " equivalent-data";
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
                    headingRows[i].className += " equivalent-data";
                }
            }

        }
    }
};

/* close sponsored bike */
var closeSponsoredBikeBtn = document.getElementById("close-sponsored-bike");
if (closeSponsoredBikeBtn) {
    closeSponsoredBikeBtn.addEventListener("click", function () {
        document.getElementById("sponsored-column-active").removeAttribute("id");
        // reset common features found
        equivalentDataFound = false;
        setButtonText();
    });
}

/* close selected model */

$('.comparison-main-card').on('click', '.close-selected-bike', function () {
    closedBikeCount++;

    if (closedBikeCount == 2) {
        window.location.href = '/m/comparebikes/';
    }
    else {
        compareBox.removeBike($(this));
    }
});

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
        placeholderBlock = "<div class='compare-box-placeholder'><div class='bike-icon-wrapper'><span class='grey-bike'></span><p class='font14 text-light-grey'>Tap to select bike " + (elementIndex + 1) + "</p></div></div>";
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

var bikeSelection = function() {
    var self = this;

    self.makeId = ko.observable('');
    self.modelId = ko.observable('');
    self.versionId = ko.observable('');
    self.compareSource = ko.observable(compareSource);

    self.redirectionUrl = function()
    {
        var _link = "";
        try {
            if (self.makeId() > 0) {
                makemasking = $("#select-make-wrapper ul li[data-id='" + self.makeId() + "']").data("masking");
                if (self.modelId() > 0) {
                    modelmasking = $("#select-model-wrapper ul li[data-id='" + self.modelId() + "']").data("masking");

                    if (self.versionId() > 0) {
                        var ele = $(".comparison-main-card .bike-details-block[data-changed='true']"),loc = window.location;

                        _link = loc.pathname.replace(ele.data("masking"), makemasking + "-" + modelmasking);
                        _link = _link + loc.search.replace(ele.data("versionid"), self.versionId());

                        if (loc.search.indexOf("source") > -1)
                        {
                            _link.replace(/source=\d/, 'source=' + self.compareSource());
                        }
                        else {
                            _link += ("&source=" + self.compareSource());
                        }                        
                    }
                }
            }
        } catch (e) {
            console.warn(e);
        }

        return _link;
    };

    self.modelArray = ko.observableArray();
    self.versionArray = ko.observableArray();

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
                    complete : function(xhr)
                    {
                        if(xhr.status!= 200)
                        {
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

        if (!bikePopup.checkSameVersion(self.versionId()) && self.versionId()>0)
        {
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

var vmBikeSelection = new bikeSelection();


var effect = 'slide',
    optionRight = { direction: 'right' },
    duration = 500;

var bikePopup = {

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
    checkSameVersion : function(versionId)
    {
        var isSameVersionSelected = false;
        $(".bike-details-block").each(function () {
            if(!$(this).hasClass('sponsored-bike-details-block') && versionId == $(this).data("versionid"))
                isSameVersionSelected = true;
        });

        return isSameVersionSelected;
    },
    showSameVersionToast : function()
    {
        window.clearTimeout();
        $('section .same-version-toast').slideDown();
        window.setTimeout(function () {
            $('section .same-version-toast').slideUp();
        }, 2000);
    }
};

ko.applyBindings(vmBikeSelection, document.getElementById("select-bike-cover-popup"));

/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');
};

$(window).on('popstate', function (event) {
    if ($('#select-bike-cover-popup').is(':visible')) {
        bikePopup.close();
    }
});