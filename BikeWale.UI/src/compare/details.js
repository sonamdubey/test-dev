docReady(function() {
    var dataRows = $('.table-content td');
    var tickIcon = '<span class="bwsprite tick-grey"></span>',
        crossIcon='<span class="bwsprite cross-grey"></span>';

    dataRows.each(function () {
        var td = $(this),
            tdText = $.trim(td.text());

        if (tdText == "Yes") {
            td.html(tickIcon);
        }
        else if (tdText == "No") {
            td.html(crossIcon);
        }
    });
});