$(document).ready(function () {
    var filterColumn = $('#filter-column'),
        filterFooter = $('#filters-footer'),
        initialPosition = 0,
        windowHeight = window.innerHeight;

    $(window).on('scroll', function () {
        var windowTop = $(this).scrollTop(),
            filterColumnOffset = filterColumn.offset(),
            filterFooterOffset = filterFooter.offset();

    });
});

$('#previous-owners-list').on('click', 'li', function () {
    var item = $(this);

    if (!item.hasClass('active')) {
        $(this).addClass('active');
    }
    else {
        $(this).removeClass('active');
    }
});

$('.filter-type-seller').on('click', function () {
    var item = $(this);

    if (!item.hasClass('checked')) {
        $(this).addClass('checked');
    }
    else {
        $(this).removeClass('checked');
    }
});