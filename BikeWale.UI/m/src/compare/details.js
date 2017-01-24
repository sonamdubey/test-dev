$(document).ready(function () {
    dropdown.setDropdown();
});

$('.dropdown-select-wrapper').on('click', '.dropdown-label', function () {
    dropdown.active($(this));
});

$('.dropdown-select-wrapper').on('click', '.dropdown-menu-list.dropdown-with-select li', function () {
    var element = $(this);
    if (!element.hasClass('active')) {
        dropdown.selectItem($(this));
        dropdown.selectOption($(this));
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
			menu = element.next('.dropdown-menu');
        menu.append('<p id="defaultVariant" class="dropdown-label">' + elementValue + '</p><div class="dropdown-list-wrapper"><p class="dropdown-selected-item">' + elementValue + '</p><ul id="templist" class="dropdown-menu-list dropdown-with-select"></ul></div>');
        dropdown.setOption(element);
    },

    setOption: function (element) {
        var selectedIndex = element.find('option:selected').index(),
			menu = element.next('.dropdown-menu'),
			menuList = menu.find('ul');

        element.find('option').each(function (index) {
            if (selectedIndex == index) {
                menuList.append('<li><input value="' + $(this).text() + '" type="submit" runat="server" class="active fullwidth" id="temp_' + index + '" data-option-value="' + $(this).val() + '" title="' + $(this).text() + '"></li>');
            }
            else {
                menuList.append('<li><input value="' + $(this).text() + '" type="submit" runat="server" class="fullwidth" id="temp_' + index + '" data-option-value="' + $(this).val() + '" title="' + $(this).text() + '"></li>');
            }
        });
    },

    active: function (label) {
        $('.dropdown-select-wrapper').find('.dropdown-menu').removeClass('dropdown-active');
        label.closest('.dropdown-menu').addClass('dropdown-active');
    },

    inactive: function () {
        $('.dropdown-select-wrapper').find('.dropdown-menu').removeClass('dropdown-active');
    },

    selectItem: function (element) {
        var elementText = element.find('input[type="submit"]').val(),
			menu = element.closest('.dropdown-menu'),
			dropdownLabel = menu.find('.dropdown-label'),
			selectedItem = menu.find('.dropdown-selected-item');

        element.siblings('li').removeClass('active');
        element.addClass('active');
        selectedItem.text(elementText);
        dropdownLabel.text(elementText);
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
            dropdown.inactive();
        }
    }
});