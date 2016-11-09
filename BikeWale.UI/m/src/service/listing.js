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
        readMoreTarget.text('Read more');
    }
});