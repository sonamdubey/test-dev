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

    function _initialize(Dropdown) {
        var container = Dropdown.container;
        var options = Dropdown.options;

        options.selectedElement = container.getElementsByClassName(options.selectedItemClassName)[0] || null;

        if (options.isActive) {
            container.classList.add(options.selectionBoxActiveClass);
        }
    }
    function _handleDropdownClick(Dropdown) {
        var container = Dropdown.container;
        var options = Dropdown.options;
        var selectionBox = container.classList.contains(options.selectionBoxClass) ? container : container.querySelector('.' + options.selectionBoxClass);
        selectionBox.addEventListener('click', function (event) {
            event.stopPropagation();
            var element = event.currentTarget;
            if (options.isActive) {
                container.classList.remove(options.selectionBoxActiveClass);
                options.isActive = false;
            }
            else {
                container.classList.add(options.selectionBoxActiveClass);
                options.isActive = true;
            }
        });
    };

    function _handleDropdownContentClick(Dropdown) {
        var container = Dropdown.container;
        var options = Dropdown.options;
        var selectionList = container.getElementsByClassName(options.selectionBoxItemClass);

        for (var i = 0; i < selectionList.length; i++) {
            selectionList[i].addEventListener('click', function (event) {
                event.stopPropagation();
                var element = event.currentTarget;
                options.selectedElement && options.selectedElement.classList.remove(options.selectedItemClassName);

                element.classList.add(options.selectedItemClassName);
                container.classList.remove(options.selectionBoxActiveClass);
                options.selectedElement = element;
                options.isActive = false;
                if (options.onDropdownContentClick) {
                    options.onDropdownContentClick(Dropdown, event);
                }
            });
        }
    }

    function _handleDocumentClick(Dropdown) {
        window.addEventListener('click', function () {
            if (Dropdown.options.isActive) {
                Dropdown.container.classList.remove(Dropdown.options.selectionBoxActiveClass);
                Dropdown.options.isActive = false;
            }
        });
    }

    var Dropdown = function Dropdown(container, params) {
        var self = this;

        if (container && typeof container === 'string') {
            self.container = document.querySelector(container);
        }
        else {
            console.warn('Dropdown: provide a selector');
            return;
        }
        
        var _defaultOptions = {
            selectionBoxActiveClass: 'selectcustom-active', // class name for active container
            onDropdownContentClick: _noop,
            selectionBoxClass: 'js-selectcustom-input-box-holder', // selection box container
            selectionBoxItemClass: 'versiondrp__item', // selection list container
            selectedItemClassName: 'selected', // selected item list class
            selectedElement: null,  // selected element
            isActive: false // if true, then dropdown content is visible
        };

        if (params && typeof params === 'object') {
            self.options = _extends({}, _defaultOptions, params);
        }
        else {
            self.options = _defaultOptions;
        }

        self.registerEvents();
    }

    Dropdown.prototype.registerEvents = function () {
        _initialize(this);
        _handleDropdownClick(this);
        _handleDropdownContentClick(this);
        _handleDocumentClick(this);
    }

    window.Dropdown = Dropdown;
}());