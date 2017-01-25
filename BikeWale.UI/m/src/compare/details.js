$(document).ready(function () {
    dropdown.setDropdown();
});

$('.dropdown-select-wrapper').on('click', '.dropdown-selected-item', function () {
    dropdown.activate($(this));
});

$('.dropdown-select-wrapper').on('click', '.dropdown-menu-list li', function () {
    var element = $(this);
    if (!element.hasClass('active')) {
        dropdown.selectItem($(this));
        dropdown.selectOption($(this));
    }
});

/* accordion tab */
$('.model-accordion-tab').on('click', function () {
    var tab = $(this),
        allTabs = $('.model-accordion-tab');

    if (!tab.hasClass('active')) {
        allTabs.removeClass('active');
        tab.addClass('active');
        $('html, body').animate({ scrollTop: tab.offset().top - 44 }, 500);
    }
    else {
        tab.removeClass('active');
    }
});

var dropdown = {
    setDropdown: function () {
        var selectDropdown = $('.dropdown-select');

        selectDropdown.each(function () {
            dropdown.setMenu($(this));
        });
    },

    setMenu: function (element) {
        $('<div class="dropdown-menu"></div>').insertAfter(element);
        dropdown.setStructure(element);
    },

    setStructure: function (element) {
        var elementValue = element.find('option:selected').text(),
			menu = element.next('.dropdown-menu'),
            labelValue = element.attr('data-title');

        menu.append('<p class="dropdown-selected-item">' + elementValue + '</p><div class="dropdown-list-wrapper"><p class="dropdown-label">' + labelValue + '</p><ul class="dropdown-menu-list"></ul></div>');
        dropdown.setOption(element);
    },

    setOption: function (element) {
        var selectedIndex = element.find('option:selected').index(),
			menuList = element.next('.dropdown-menu').find('ul');

        element.find('option').each(function (index) {
            if (selectedIndex == index) {
                menuList.append('<li class="option-active" data-option-value="' + $(this).val() + '">' + $(this).text() + '</li>');
            }
            else {
                menuList.append('<li data-option-value="' + $(this).val() + '">' + $(this).text() + '</li>');
            }
        });
    },

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
};

$(document).on('click', function (event) {
    if ($('.dropdown-list-wrapper').is(':visible')) {
        event.stopPropagation();
        var bodyElement = $('body'),
		    dropdownLabel = bodyElement.find('.dropdown-label'),
		    dropdownList = bodyElement.find('.dropdown-menu-list'),
		    noSelectLabel = bodyElement.find('.dropdown-selected-item');

        if (!$(event.target).is(dropdownLabel) && !$(event.target).is(dropdownList) && !$(event.target).is(noSelectLabel)) {
            dropdown.deactivate();
        }
    }
});