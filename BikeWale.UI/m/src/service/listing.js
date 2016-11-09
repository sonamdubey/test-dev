// read more-collapse
var readMoreTarget = $('#read-more-target'),
    serviceMoreContent = $('#service-more-content');

readMoreTarget.on('click', function () {
    if (!serviceMoreContent.hasClass('active')) {
        serviceMoreContent.addClass('active');
        readMoreTarget.text('Collapse');
    }
    else {
        serviceMoreContent.removeClass('active');
        readMoreTarget.text('... Read more');
    }
});

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
    contactServicePopup.initial.selection();
});

$('.cancel-popup-btn').on('click', function () {
    var container = $(this).closest('.modal-popup-container');
    history.back();
    modalPopup.close(container);
});

$('#apply-sort-by').on('click', function () {
    history.back();
    sortBy.close();
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