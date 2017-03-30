var chosenSelectBox = $('.chosen-select');

chosenSelectBox.chosen();

docReady(function () {
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

    $('#makeFilter').trigger("chosen:updated");
    $('#yearFilter').trigger("chosen:updated");
});