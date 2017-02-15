
var chosenSelectBox = $('.chosen-select');

chosenSelectBox.chosen();

$(window).on("scroll", function () {
    if ($(window).scrollTop() > 40)
        $('.header-transparent').removeClass("header-landing").addClass("header-fixed");
    else
        $('.header-transparent').removeClass("header-fixed").addClass("header-landing");
});

$(document).ready(function () {    
    chosenSelectBox.each(function () {
        var text = $(this).attr('data-placeholder');
        $(this).siblings('.chosen-container').find('input[type=text]').attr('placeholder', text);
    });

    var selectDropdownBox = $('.select-box-no-input');

    selectDropdownBox.each(function () {
        var text = $(this).find('.chosen-select').attr('data-title'),
            searchBox = $(this).find('.chosen-search')

        searchBox.empty().append('<p class="no-input-label">' + text + '</p>');
    });
});

$(".chosen-select").chosen().change(function () {
    if ($(this).val() > 0) {
        $(this).siblings('.select-label').hide();
    }
});

// more brand - collapse
$('.view-brandType').click(function (e) {
    debugger;
    var element = $(this),
        elementParent = element.closest('.collapsible-brand-content'),
        moreBrandContainer = elementParent.find('.brandTypeMore');

    if (!moreBrandContainer.is(':visible')) {
        moreBrandContainer.slideDown();
        element.attr('href', 'javascript:void(0)');
        element.text('View less brands');
    }
    else {
        element.attr('href', '#brand-type-container');
        moreBrandContainer.slideUp();
        element.text('View more brands');
    }

    e.preventDefault();
    e.stopPropagtion();

});