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

//Update Log

/*
Accordion v0.1.0
##A normal accordion functionality with two scenarios handled
##1: Multiple Open 2:Single Open
##The body height is calculated on click of every Aaccordion element so that transition can be achieved
*/

/*
Accordion v0.1.1
##Added a feature to scroll the accordion to the offsetTop of the clicked accordion element using utility file scrollTo.js
##Removed transition for elements body when another element is clicked to avoid page jump due to two animations running at the same time
##if scroll is set to false the animation is never reset 
##scroll handled for both multiple and single open scenario
##polyfill for 'Object.assign' updated 
*/

/*
Accordion v0.1.2
##Added functionality to be executed when the accordion has expanded and before the accordion element collapses
##Added null check in case of initailization and a logic to avoid re-initailization of the same accordion
##Added polyfill for closest
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
    }

    const _listClass = 'js-accordion-list';
    const _wrapperClass = 'js-accordion-list-item';
    const _headerCLass = 'js-accordion-heading';
    const _bodyClass = 'js-accordion-content-wrapper';
    const _activeElementClass = 'accordion-item-active';
    const _bodyDataClass = 'js-accordion-content-list';

    //noop: no-operation function performs nothing (used as a default options for callback functions)
    function _noop() { }

    //this function sets the transition duration of the body element to a value passed by the user 
    //or takes the default value form default options
    function _setAnimation(element, accordion, animation) {
        if (animation) {
            element.querySelector('.' + _bodyClass).style.transition = 'height ' + accordion.options.animationDelay + 'ms ease-in-out';
        }
        else {
            element.querySelector('.' + _bodyClass).style.transition = 'height 0ms ease-in-out';
        }
    }

    //function calculates the scroll height of the accordion body and then applys the height and adds active class to the accordion element
    function _expandElement(accordionListItem, accordion) {
        var accordionBody = accordionListItem.querySelector('.' + _bodyClass);
        var accordionContent = accordionListItem.querySelector('.' + _bodyDataClass);
        accordionBody.style.height = accordionContent.scrollHeight + 'px';
        accordionListItem.classList.add(accordion.options.activeClass);
    }

    //this function executes as soon the accordion item is clicked 
    //it calls a funtion to calculates the scroll height of the accordion body and then sets the height of the accordion body which is initally 0.
    //it also has a check that if multiple accordion elements should be open at once,
    // if (true) then multiple elements can be open else the active element is collapsed and the click element is expanded
    function _expandAccordion(accordionListItem, accordion) {

        _setAnimation(accordionListItem, accordion, true);
        if (accordion.options.multipleOpen) {
            _expandElement(accordionListItem, accordion);
        }
        else {
            var activeElement = document.querySelector('.' + _activeElementClass);
            if (activeElement !== null) {
                if (accordion.options.animateOnFocus) {
                    _setAnimation(activeElement, accordion, false);
                }
                activeElement.classList.remove(accordion.options.activeClass);
                activeElement.querySelector('.' + _bodyClass).style.height = 0 + 'px';
            }
            _expandElement(accordionListItem, accordion);
        }

        if (accordion.options.onExpandTransitionStart) {
            accordion.options.onExpandTransitionStart(accordionListItem);
        }

        if (accordion.options.afterExpandEvent) {
            accordion.options.afterExpandEvent(accordionListItem);
        }

        if (accordion.options.animateOnFocus && typeof scrollToTop === 'function') {
            scrollToTop(window, accordionListItem.getBoundingClientRect().top + (window.pageYOffset || document.documentElement.scrollTop) - accordion.options.accordionMargin, 500);
        }
        else {
            console.warn('Accordion: please add scrollTo.js');
        }

    }

    // function checks that if the clicked element is active when it is clicked and if it is active it is collapsed and vice versa
    function _collapseAccordion(accordionListItem, accordion) {

        if (accordion.options.beforeCollapseEvent) {
            accordion.options.beforeCollapseEvent(accordionListItem);
        }

        var accordionBody = accordionListItem.querySelector('.' + _bodyClass);
        setTimeout(function () {
            accordionBody.style.height = 0 + 'px';
        }, 0);
        accordionListItem.classList.remove(accordion.options.activeClass);

        if (accordion.options.onCollapseEvent) {
            accordion.options.onCollapseEvent(accordionListItem);
        }
    }

    //main functions thats registers the click event on each accordion element
    function _handleToggleEvent(accordion) {
        var accordionListWrapper = accordion.container.querySelector('.' + _listClass);
        var accordionList = accordionListWrapper.children;
        for (var i = 0; i < accordionList.length; i++) {
            var accordionHead = accordionList[i].querySelector('.' + _headerCLass);
            if (accordionHead) {
                accordionHead.addEventListener('click', function () {
                    var accordionListItem = this.parentElement;
                    if (!accordionListItem.classList.contains(accordion.options.activeClass)) {
                        _expandAccordion(accordionListItem, accordion);
                    }
                    else {
                        _collapseAccordion(accordionListItem, accordion);
                    }
                });
            }
        }
    }

    var Accordion = function Accordion(container, params) {
        var self = this;

        var _defaultOptions = {
            activeClass: 'accordion-item-active',// class added to the expanded element
            multipleOpen: true,//boolean value that decides if multiple elements can be open at once
            animationDelay: 500, //animation delay in milliseconds
            onExpandTransitionStart: _noop,//a function that executes when the accordion expand event is fired , default value _noop (used to provide extra functionality on expand event i.e callback function) 
            onCollapseEvent: _noop,//a function that executes when the accordion collapse event is fired , default value _noop (used to provide extra functionality on collapse event i.e callback function) 
            animateOnFocus: false,// body scroll boolean value
            accordionMargin: 0,// value to be subtracted from the offset value of the element to be scrolled 
            afterExpandEvent: _noop,// action to be executed after expanding the accordion element
            beforeCollapseEvent: _noop, // action to be executed before collapsing the accordion element
            baseElement: document //baseElement used to restrict search it acts as a wrapper or parent element in which the user can search for elements to perform action on which is by default document
        }

        if (params && typeof params === 'object') {
            self.options = _extends({}, _defaultOptions, params);
        }
        else {
            self.options = _defaultOptions;
        }

        if (container && typeof container === 'string') {
            self.container = self.options.baseElement.querySelector(container);

            if (self.container.getAttribute('data-accordion') !== null) {
                return;
            }
        }
        else {
            console.warn('Accordion: Provide a selector');
        }

        self.registerEvents();
        self.container.setAttribute('data-accordion', 'true');
    }

    Accordion.prototype.registerEvents = function () {
        _handleToggleEvent(this);
    }

    window.Accordion = Accordion;
}());