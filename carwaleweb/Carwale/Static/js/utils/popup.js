/*!
 * Version 0.0.1
 * Date: 2018-09-05
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
    //no operation function( performs nothing)
    function _noop() { };


    //Function to openpopup using data-attribute of the link passed 
    //Fetches the popup using its id i.e same as data-attribute and shows it 
    function openPopup(popup) {
        var popupLink = document.querySelectorAll(popup.link);
        if (popupLink.length === 0) {
            console.warn('Popup: Please provide valid selector');
            return;
        }
        for (var i = 0; i < popupLink.length; i++) {
            popupLink[i].addEventListener('click', function () {
                var dataPopup = this.getAttribute('data-popup');
                var popupContainer = document.getElementById(dataPopup);
                if (popup.options.lockBodyScroll && typeof HandelBodyScroll === 'object') {
                    HandelBodyScroll.lockScroll();
                }
                popupContainer.classList.add(popup.options.activeClass);
                if (popup.options.onPopupOpen) {
                    popup.options.onPopupOpen(popupContainer);
                }
            });
        }
    }

    //Function to close popup 
    //Close event is binded to the respective popup close button and blackout window during initailization for every popup
    function closePopup(popup, popupContainer) {
        popupContainer.querySelector('.' + popup.options.closeButtonClass).addEventListener('click', function () {
            popupContainer.classList.remove(popup.options.activeClass);
            if (popup.options.lockBodyScroll && typeof HandelBodyScroll === 'object') {
                HandelBodyScroll.unlockScroll();
            }
            if (popup.options.onCloseClick) {
                popup.options.onCloseClick();
            }
            
        });
        popupContainer.querySelector('.' + popup.options.blackoutWindowClass).addEventListener('click', function () {
            popupContainer.classList.remove(popup.options.activeClass);
            if (popup.options.lockBodyScroll && typeof HandelBodyScroll === 'object') {
                HandelBodyScroll.unlockScroll();
            }
            if (popup.options.onCloseClick) {
                popup.options.onCloseClick();
            }
        });
    }


    //This function sets the slide direction of the popup using the direction passed by the user or takes the default value
    function _handelCloseEvent(popup) {
        var popupLink = document.querySelectorAll(popup.link);
        for (var i = 0; i < popupLink.length; i++) {
            var popupBody = popupLink[i].getAttribute('data-popup');
            var popupContainer = document.getElementById(popupBody);
            if (popupContainer !== null) {
                closePopup(popup, popupContainer);
            }
            else {
                console.warn('Please provide id to popup');
            }

        }
    }

    // funtion to invoke open popup and checks if any other operation to execute when link is clicked
    function _handelOpenEvent(popup) {
        openPopup(popup);
    };

    var Popup = function Popup(link, params) {
        var self = this;

        if (link && typeof link === 'string') {
            self.link = link;
        }
        else {
            console.warn('CustomPopup: Please provide a selector');
        }

        var _defaultOptions = {
            activeClass: 'popup-active',
            closeButtonClass: 'popup-close-button',
            blackoutWindowClass: 'popup-overlay',
            lockBodyScroll: true,
            onPopupOpen: _noop,
            onCloseClick: _noop
        }

        if (params && typeof params === 'object') {
            self.options = _extends({}, _defaultOptions, params);
        }
        else {
            self.options = _defaultOptions;
        }

        self.registerEvents();
    }

    Popup.prototype.registerEvents = function () {
        _handelCloseEvent(this);
        _handelOpenEvent(this);
    }
    window.Popup = Popup;
}());