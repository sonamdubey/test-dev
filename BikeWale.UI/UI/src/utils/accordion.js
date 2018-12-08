/**
 * JavaScript `.closest()` polyfill
 * Return the closest element matching a selector up the DOM tree
 * https://github.com/jonathantneal/closest/
 * v2.0.2
 *
 * Polyfill required for:
 * iOS: 7, 6
 * UCBrowser: < 11.5
 */

(function (ElementProto) {
    if (typeof ElementProto.matches !== 'function') {
        ElementProto.matches = ElementProto.msMatchesSelector || ElementProto.mozMatchesSelector || ElementProto.webkitMatchesSelector || function matches(selector) {
            var element = this;
            var elements = (element.document || element.ownerDocument).querySelectorAll(selector);
            var index = 0;

            while (elements[index] && elements[index] !== element) {
                ++index;
            }

            return Boolean(elements[index]);
        };
    }

    if (typeof ElementProto.closest !== 'function') {
        ElementProto.closest = function closest(selector) {
            var element = this;

            while (element && element.nodeType === 1) {
                if (element.matches(selector)) {
                    return element;
                }

                element = element.parentNode;
            }

            return null;
        };
    }
})(window.Element.prototype);



/**
 * A JavaScript Accordion
 * v0.1.3
 *
 *
 * Changelog
 * 0.1.2, 22nd October 2018
 * - Add `closest` polyfill
 * - Remove `click` event from accoriond list items and bind it on accordion list to handle events on dynamic content
 *
 * ---
 *
 * 0.1.3, 26th October 2018
 * - Add `js-accordion-list-item` class for accordion list item
 * - Add `resizeAccordionItemHeight` api method to recalculate accordion item's body height
 *
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
    }

    var _listClass = 'js-accordion-list';
    var _listItemClass = 'js-accordion-list-item';
    var _headerClass = 'js-accordion-heading';
    var _bodyClass = 'js-accordion-content-wrapper';
    var _bodyContentClass = 'js-accordion-content';

    // noop: no-operation function performs nothing (used as a default options for callback functions)
    function _noop() { }

    // this function sets the transition duration of the body element to a value passed by the user 
    // or takes the default value form default options
    function _setAnimation(accordion) {
        var accordionDataWrapper = accordion.container.querySelectorAll('.' + _bodyClass);
        for (var i = 0; i < accordionDataWrapper.length; i++) {
            accordionDataWrapper[i].style.transition = 'height ' + accordion.options.animationDelay + 'ms linear';
        }
    }

    // function calculates the scroll height of the accordion body and then applys the height and
    // adds active class to the accordion element
    function _expandElement(accordionListItem, accordion) {
        var accordionBody = accordionListItem.querySelector('.' + _bodyClass);
        var accordionContent = accordionListItem.querySelector('.' + _bodyContentClass);
        accordionBody.style.height = accordionContent.scrollHeight + 'px';
        accordionListItem.classList.add(accordion.options.activeClass);
    }

    // this function executes as soon the accordion item is clicked 
    // it calls a funtion to calculates the scroll height of the accordion body and then sets the height of the accordion body which is initally 0.
    // it also has a check that if multiple accordion elements should be open at once,
    // if (true) then multiple elements can be open else the active element is collapsed and the click element is expanded
    function _expandAccordion(accordionListItem, accordion) {
        if (accordion.options.multipleOpen) {
            _expandElement(accordionListItem, accordion);
        }
        else {
            var activeElement = document.querySelector('.' + accordion.options.activeClass);
            if (activeElement !== null) {
                activeElement.classList.remove(accordion.options.activeClass);
                activeElement.querySelector('.' + _bodyClass).style.height = 0 + 'px';
            }
            _expandElement(accordionListItem, accordion);
        }

        if (accordion.options.onExpandEvent) {
            accordion.options.onExpandEvent({
                clickedElement: accordionListItem
            });
        }
    }

    // function checks that if the clicked element is active when it is clicked and if it is active it is collapsed and vice versa
    function _collapseAccordion(accordionListItem, accordion) {
        var accordionBody = accordionListItem.querySelector('.' + _bodyClass);
        accordionBody.style.height = 0 + 'px';
        accordionListItem.classList.remove(accordion.options.activeClass);

        if (accordion.options.onCollapseEvent) {
            accordion.options.onCollapseEvent();
        }
    }

    // main functions thats registers the click event
    function _handleToggleEvent(accordion) {
        var accordionList = accordion.container.querySelector('.' + _listClass);
        accordionList.addEventListener('click', function (event) {
            var accordionHead = event.target.closest('.' + _headerClass);
            if (!accordionHead) {
                return;
            }

            var accordionListItem = accordionHead.parentElement;
            if (!accordionListItem.classList.contains(accordion.options.activeClass)) {
                _expandAccordion(accordionListItem, accordion);
            }
            else {
                _collapseAccordion(accordionListItem, accordion);
            }
        })
    }

    function _calculateItemHeight(event) {
        var accordionListItem = event.target.closest('.' + _listItemClass);

        if (accordionListItem) {
            accordionListItem.querySelector('.' + _bodyClass).style.height = accordionListItem.querySelector('.' + _bodyContentClass).scrollHeight + 'px';
        }
    }

    var Accordion = function Accordion(container, params) {
        var self = this;

        if (container && typeof container === 'string') {
            self.container = document.querySelector(container);
        }
        else {
            console.warn('Accordion: Provide a selector');
        }

        var _defaultOptions = {
            activeClass: 'accordion-active', // class added to the expanded element
            multipleOpen: true, // boolean value that decides if multiple elements can be open at once
            animationDelay: 500, // animation delay in milliseconds
            onExpandEvent: _noop, // callback function
            onCollapseEvent: _noop, // callback function
        }

        if (params && typeof params === 'object') {
            self.options = _extends({}, _defaultOptions, params);
        }
        else {
            self.options = _defaultOptions;
        }

        self.registerEvents();
    }

    Accordion.prototype.registerEvents = function () {
        _setAnimation(this);
        _handleToggleEvent(this);
    }

    Accordion.prototype.resizeAccordionItemHeight = function (event) {
        _calculateItemHeight(event);
    }

    window.Accordion = Accordion;
}());