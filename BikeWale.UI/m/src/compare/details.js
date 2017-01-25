var floatingCard = $('#comparison-floating-card'),
    floatingCardHeight = floatingCard.height() - 44,
    overallSpecsTabs = $('#overall-specs-tabs'),
    $window = $(window);

$(document).ready(function () {
    dropdown.setDropdown();

    $window.on('scroll', function () {
        var windowScrollTop = $window.scrollTop(),
            overallSpecsOffset = overallSpecsTabs.offset().top;

        if (windowScrollTop > overallSpecsOffset - floatingCardHeight) {
            floatingCard.addClass('fixed-card');
        }
        else if (windowScrollTop < overallSpecsOffset - floatingCardHeight) {
            floatingCard.removeClass('fixed-card');
        }
    });

});

/* accordion tab */
$('.model-accordion-tab').on('click', function () {
    var tab = $(this),
        allTabs = $('.model-accordion-tab');

    if (!tab.hasClass('active')) {
        allTabs.removeClass('active');
        tab.addClass('active');
        $('html, body').animate({ scrollTop: tab.offset().top - floatingCardHeight - 47 }, 500); // accordion tab height 47px
    }
    else {
        tab.removeClass('active');
    }
});

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
        var menuElement = document.createElement("div");

        menuElement.className = "dropdown-menu";

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