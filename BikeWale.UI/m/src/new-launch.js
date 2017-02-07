// read more - collapse
$('.read-more-target').on('click', function () {
    var element = $(this),
        parentElemtent = element.closest('.collapsible-content');

    if (!parentElemtent.hasClass('active')) {
        parentElemtent.addClass('active');
        element.text(' Collapse');
    }
    else {
        parentElemtent.removeClass('active');
        element.text('...Read more');
    }
});

// more brand - collapse
$(".view-brandType").click(function () {
    var element = $(this),
        elementParent = element.closest('.collapsible-brand-content');

    elementParent.find('.brandTypeMore').slideToggle();    
    element.text(element.text() == 'View more brands' ? 'View less brands' : 'View more brands');
});