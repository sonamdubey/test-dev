(function () {

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

    function _noop() { }

    function _handleClick(ScrollSpy) {
        var container = ScrollSpy.container;
        var options = ScrollSpy.options;
        var elements = container.getElementsByClassName(options.navigationTabItemClass);
        var navigationWrapper = container.querySelector(options.navigationTabContainer);

        for (var i = 0; i < elements.length; i++) {
            elements[i].addEventListener('click', function (event) {
                var dataTabsScrollTop = container.querySelector('.' + options.dataTabSectionClass + '[data-id="' + event.currentTarget.getAttribute('data-href') + '"]').offsetTop - navigationWrapper.offsetHeight - navigationWrapper.firstElementChild.offsetTop;
                scrollToTop(window, dataTabsScrollTop);

                if (options.onNavigationTabClick) {
                    options.onNavigationTabClick(ScrollSpy, event);
                }
            })
        }
    };

    function _handleScroll(ScrollSpy) {
        var options = ScrollSpy.options
        ScrollSpy.container.querySelector('.' + options.navigationTabItemClass + ':first-child').classList.add(options.activeNavItemClass); // To make first element in navigation active on load

        window.addEventListener('scroll', function () {
            // To set fix/relative position of navigation
            _setNavigationPosition(ScrollSpy);

            // To set current active navigation tab
            _setCurrentActiveTab(ScrollSpy);

        }, { passive: true });

        function _setNavigationPosition(ScrollSpy) {
            var options = ScrollSpy.options;
            var navigationTabContainer = ScrollSpy.container.querySelector(options.navigationTabContainer);

            if (window.pageYOffset >= navigationTabContainer.offsetTop) {
                navigationTabContainer.classList.add(options.fixNavigationClass);
            }
            else {
                navigationTabContainer.classList.remove(options.fixNavigationClass);
            }
        };

        function _setCurrentActiveTab(ScrollSpy) {
            var container = ScrollSpy.container;
            var options = ScrollSpy.options;
            var navigationHeight = container.querySelector(options.navigationTabContainer).offsetHeight;
            var dataTabElements = container.getElementsByClassName(options.dataTabSectionClass);
            var i = 0;

            while (i < dataTabElements.length) {
                var dataTab = dataTabElements[i];
                var elementRect = dataTab.getBoundingClientRect();
                var firstChildOffsetTop = container.querySelector(options.navigationTabContainer).firstElementChild.offsetTop;

                if ((elementRect.top - navigationHeight <= firstChildOffsetTop && elementRect.bottom - navigationHeight >= firstChildOffsetTop)) {
                    var activeNavItemClass = container.querySelector('.' + options.navigationTabItemClass + '[data-href="' + dataTab.getAttribute('data-id') + '"]');

                    if (!activeNavItemClass.classList.contains(options.activeNavItemClass)) {
                        container.querySelector('.' + options.navigationTabItemClass + '.' + options.activeNavItemClass).classList.remove(options.activeNavItemClass);
                        activeNavItemClass.classList.add(options.activeNavItemClass);

                        _setNavigationScrollPosition(activeNavItemClass, options.activeNavItemPosition);
                    }

                    i = dataTab.length + 1; // To break while loop if we found element
                }
                i++;
            }

            if (options.onScrollContainer) {
                options.onScrollContainer(ScrollSpy);
            }
        };
        
        function _setNavigationScrollPosition(activeNavItemClass, position) {
            var overallTabList = activeNavItemClass.parentElement;
            var left = overallTabList.scrollLeft + Math.round(activeNavItemClass.getBoundingClientRect().left);

            if (position === 'center') {
                left = left - (overallTabList.parentElement.offsetWidth / 2) + (activeNavItemClass.offsetWidth / 2)
            }

            if (overallTabList.offsetWidth < overallTabList.scrollWidth) {
                scrollLeft(overallTabList, left);
            }
        };
    }

    var ScrollSpy = function ScrollSpy(container, params) {
        var self = this;

        if (container && typeof container === 'string') {
            self.container = document.querySelector(container);
        }
        else {
            console.warn('ScrollSpy: provide a selector');
            return;
        }

        var _defaultOptions = {
            fixNavigationClass: 'overall-tabs-container-fixed', // class name for to fix navigation container 
            onNavigationTabClick: _noop,
            onScrollContainer: _noop,
            navigationTabContainer: '.overall-tabs-container', // navigation container element
            dataTabSectionClass: 'data-tab',  //section class related to navigation tab
            navigationTabItemClass: 'tab-list__item', // navigation tab class name
            activeNavItemClass: 'tab-list__item-active', //class name for active navigation list
            activeNavItemPosition: 'left'  //active nav position in screen
        };

        if (params && typeof params === 'object') {
            self.options = _extends({}, _defaultOptions, params);
        }
        else {
            self.options = _defaultOptions;
        }

        self.registerEvents();
    }

    ScrollSpy.prototype.registerEvents = function () {
        _handleClick(this);
        _handleScroll(this);
    }

    window.ScrollSpy = ScrollSpy;
}());