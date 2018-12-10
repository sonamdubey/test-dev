/*
ReadMoreCollapse v 0.1.1
##Added null check in case of initailization and a logic to avoid re-initailization of the same container
##Updated the container search feature i.e the user can pass a parameter named 'baseElement' so as to restrict the search of the element within a container which is by default 'document'
*/

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

    function _getExpandText(readmoreCollapse) {
        var readMoreCollapseButton = readmoreCollapse.container.querySelector('.read-more-collapse-link');
        if (readMoreCollapseButton !== null && typeof readMoreCollapseButton !== 'undefined') {
            var expandText = readMoreCollapseButton.innerHTML;
            if (expandText.length) {
                var updatedData = { expandText: expandText };
                readmoreCollapse.options = _extends({}, readmoreCollapse.options, updatedData);
            }
            else {
                readMoreCollapseButton.innerHTML = readmoreCollapse.options.expandText;
            }
        }
    }

    function _handleExpandCollapseEvent(readmoreCollapse) {
        if (readmoreCollapse.container != null) {
            var readMoreCollapseButton = readmoreCollapse.container.querySelector('.read-more-collapse-link');
            if (readMoreCollapseButton !== null && typeof readMoreCollapseButton !== 'undefined') {
                var expandClass;
                if (readmoreCollapse.options.concatData) {
                    expandClass = readmoreCollapse.options.expandedClass;
                } else {
                    expandClass = readmoreCollapse.options.hideInitialAndExpand;
                }
                readMoreCollapseButton.addEventListener('click', function (event) {
                    if (readmoreCollapse.container.classList.contains(expandClass)) {
                        readmoreCollapse.container.classList.remove(expandClass);
                        this.innerHTML = readmoreCollapse.options.expandText;
                        if (readmoreCollapse.options.ellipsis) {
                            readmoreCollapse.container.querySelector('.ellipsis-text').innerHTML = readmoreCollapse.options.ellipsisText;
                        }

                        if (readmoreCollapse.options.onCollapseClick) {
                            readmoreCollapse.options.onCollapseClick(readmoreCollapse);
                        }
                    }
                    else {
                        readmoreCollapse.container.classList.add(expandClass);
                        this.innerHTML = readmoreCollapse.options.collapseText;
                        if (readmoreCollapse.options.ellipsis) {
                            readmoreCollapse.container.querySelector('.ellipsis-text').innerHTML = readmoreCollapse.options.expandedEllipsisText;
                        }

                        if (readmoreCollapse.options.onExpandClick) {
                            readmoreCollapse.options.onExpandClick(readmoreCollapse);
                        }
                    }
                });
            }
        }
    }

    var ReadMoreCollapse = function ReadMoreCollapse(container, params) {
        var self = this;

        var _defaultOptions = {
            expandedClass: 'text-expanded',
            hideInitialAndExpand: 'text-initial-hide',
            concatData: true,
            expandText: 'More',
            collapseText: 'Collapse',
            ellipsis: true,
            ellipsisText: '...',
            expandedEllipsisText: '...',
            onExpandClick: _noop,
            onCollapseClick: _noop,
            baseElement: document 
        };

        if (params && typeof params === 'object') {
            self.options = _extends({}, _defaultOptions, params);
        }
        else {
            self.options = _defaultOptions;
        }

        if (container && typeof container === 'string') {
            self.container = self.options.baseElement.querySelector(container);
            if (self.container === null || typeof self.container === 'undefined') {
                console.warn('ReadMoreCollapse: container not found');
                return;
            }

            if (self.container.getAttribute('data-readmore') !== null) {
                return;
            }
        }
        else {
            console.warn('ReadMoreCollapse: provide a selector');
            return;
        }

        self.registerEvents();
        self.container.setAttribute('data-readmore', 'true');
    }

    ReadMoreCollapse.prototype.registerEvents = function () {
        _getExpandText(this);
        _handleExpandCollapseEvent(this);
    }

    window.ReadMoreCollapse = ReadMoreCollapse;
}());