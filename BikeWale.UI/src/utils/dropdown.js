(function () {
    // private functions
    var _extends = Object.assign || function (target) {
        for (var i = 1; i < arguments.length; i++) {
            var source = arguments[i];
            for (var key in source) {
                if (Object.prototype.hasOwnProperty.call(source, key)) {
                    target[key] = source[key];
                }
            }
        }

        return target;
    };

    function _menuTabEvent(dropdown) {
        $(dropdown.container).on('click', '.dropdown-box__menu-tab', function (event) {
            $(dropdown.container).toggleClass(dropdown.options.activeSelectBoxClass);

            if (dropdown.options.onMenuClick) {
                dropdown.options.onMenuClick(dropdown);
            }
            _killEvent(event);
        });
    }

    function _submenuListEvent(dropdown) {
        $(dropdown.container).on('click', '.dropdown-box-submenu-list__item', function (event) {
            var item = $(this);
            var itemTitle = item.find('.submenu-list-item__title');
            var itemTitleText = '';

            if (itemTitle.find('.submenu-list-item-title__left-col').length) {
                itemTitleText = itemTitle.find('.submenu-list-item-title__left-col').html();
            }
            else {
                itemTitleText = itemTitle.html();
            }

            dropdown.activeOption = {
                value: item.attr('data-value'),
                name: itemTitleText,
                element: item
            }

            item.addClass(dropdown.options.activeSubmenuItemClass).siblings('.' + dropdown.options.activeSubmenuItemClass).removeClass(dropdown.options.activeSubmenuItemClass);
            item.closest('.dropdown-box').toggleClass(dropdown.options.activeSelectBoxClass).find('.dropdown-box__menu-value').html(itemTitleText);

            if (dropdown.options.onChange) {
                dropdown.options.onChange(dropdown);
            }

            _killEvent(event);
        });
    }

    function _closeEvent(dropdown) {
        $(document).on('click', function (event) {
            var clickedElement = $(event.currentTarget);

            if ($('.dropdown-box.' + dropdown.options.activeSelectBoxClass).length) {
                if (!clickedElement.hasClass('dropdown-box__menu-tab') || !clickedElement.hasClass('dropdown-box-submenu-list__item')) {
                    $('.dropdown-box.' + dropdown.options.activeSelectBoxClass).removeClass(dropdown.options.activeSelectBoxClass);
                }
            }
        });
    };

    function _killEvent(event) {
        event.stopPropagation();
    }

    // constructor function
    var DropdownMenu = function DropdownMenu(container, params) {
        var self = this;

        if (container && typeof container === 'string') {
            self.container = document.querySelector(container);
            self.activeOption = {};
        }
        else {
            console.warn('DropdownMenu: provide a selector')
            return;
        }

        var _defaultOptions = {
            activeSelectBoxClass: 'dropdown-box--active',
            activeSubmenuItemClass: 'submenu-list-item--active',
            onMenuClick: function () { },
            onChange: function () { }
        };

        if (params && typeof params === 'object') {
            self.options = _extends({}, _defaultOptions, params);
        }
        else {
            self.options = _defaultOptions;
        }

        self.registerEvents();
    }

    // prototype
    DropdownMenu.prototype.registerEvents = function () {
        _menuTabEvent(this);
        _submenuListEvent(this);
        _closeEvent(this);
    }

    DropdownMenu.prototype.activeOption = function () {

    }

    window.DropdownMenu = DropdownMenu;
}());
