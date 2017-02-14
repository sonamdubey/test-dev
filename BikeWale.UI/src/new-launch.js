var chosenSelectBox = $('.chosen-select');

chosenSelectBox.chosen();

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
$('.view-brandType').click(function () {
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
});