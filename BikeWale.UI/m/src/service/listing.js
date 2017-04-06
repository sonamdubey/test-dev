/* service center number */
var contactPopup = $('#contact-service-popup');

contactPopup.on('click', '.popup-list li', function () {
    var item = $(this);

    if (!item.hasClass('active')) {
        contactServicePopup.set.selection(item);
    }
});

$('.contact-service-btn').on('click', function () {
    modalPopup.open(contactPopup);
    appendState('contactService');
    contactServicePopup.set.contactList($(this));
});

$('.cancel-popup-btn').on('click', function () {
    var container = $(this).closest('.modal-popup-container');
    history.back();
    modalPopup.close(container);
});

var modalPopup = {
    open: function (container) {
        container.show();
        $('html, body').addClass('lock-browser-scroll');
        $('.modal-background').show();
    },

    close: function (container) {
        container.hide();
        $('html, body').removeClass('lock-browser-scroll');
        $('.modal-background').hide();
    }
};

var contactServicePopup = {
    initial: {
        selection: function () {
            var element = contactPopup.find('.popup-list li').first(),
            elementValue = element.find('.list-label').text();

            element.addClass('active');
            contactServicePopup.set.contact(elementValue);
        }
    },

    set: {
        contactList: function (element) {
            var serviceName = element.attr('data-service-name'),
                serviceNumbers = element.attr('data-service-number'),
                contacts = serviceNumbers.split(','),
                contactsLength = contacts.length,
                popupList = contactPopup.find('.popup-list');

            popupList.empty();

            contactPopup.find('.popup-header').text(serviceName);
            for (var i = 0; i < contactsLength; i++) {
                popupList.append('<li><span class="bwmsprite radio-uncheck"></span><span class="list-label">' + contacts[i] + '</span></li>');
            };

            contactServicePopup.initial.selection();
        },

        selection: function (element) {
            var elementValue = element.find('.list-label').text();

            contactPopup.find('li.active').removeClass('active');
            element.addClass('active');
            
            contactServicePopup.set.contact(elementValue);
        },

        contact: function(elementValue) {
            contactPopup.find('#call-service-btn').attr('href', 'tel:' + elementValue);
        }
    }
}

/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');
};

$(window).on('popstate', function (event) {
    if ($('#contact-service-popup').is(':visible')) {
        modalPopup.close('#contact-service-popup');
    }
});

function initializeCityMaps() {
    $(".city-map").each(function (index) {
        var lat = $(this).attr("data-item-lat");
        var lng = $(this).attr("data-item-long");
        var latlng = new google.maps.LatLng(lat, lng);
        var mapOptions = {
            zoom: 10,
            center: latlng,
            streetViewControl: false,
            scrollwheel: false,
            mapTypeControl: false,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        var map = new google.maps.Map($(".city-map")[index], mapOptions);
    });
}

docReady(function () {
    initializeCityMaps();
});