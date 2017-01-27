var floatingCard = $('#comparison-floating-card'),
    floatingCardHeight = floatingCard.height() - 44,
    comparisonFooter = $('#comparison-footer'),
    overallSpecsTabs = $('#overall-specs-tabs'),
    floatingButton = $('#toggle-float-button'),
    $window = $(window),
    windowScrollTop = $window.scrollTop();

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
            floatingButton.removeClass('fixed-floater');
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
document.getElementById("close-sponsored-bike").addEventListener("click", function () {
    document.getElementById("sponsored-column-active").removeAttribute("id");
    // reset common features found
    equivalentDataFound = false;
});

/* close selected model */
$('.comparison-main-card').on('click', '.close-selected-bike', function () {
    compareBox.removeBike($(this));
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
        var placeholder;

        $(element).empty();
        placeholderBlock = "<div class='compare-box-placeholder'><div class='bike-icon-wrapper'><span class='grey-bike'></span><p class='font14 text-light-grey'>Tap to select bike " + (elementIndex + 1) + "</p></div></div>";
        $(element).append(placeholderBlock);

    }
};


$('.dropdown-select-wrapper').on('click', '.dropdown-selected-item', function () {
    dropdownInteraction.activate($(this));
});

$('.dropdown-select-wrapper').on('click', '.dropdown-menu-list li', function () {
    var element = $(this);
    if (!element.hasClass('active')) {
        dropdownInteraction.selectItem($(this));
        dropdownInteraction.selectOption($(this));
    }
});

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