var dt = '';

$("#sortbike li").on("click", function () {
    sortListLI.removeClass("selected");
    $(this).addClass('selected');
    var sortByText = $(this).text();
    $(".sort-by-title").find(".sort-select-btn").html(sortByText);
    $.sortChangeUp(sortByDiv);
    var id = $(this).attr('id');
    switch (id) {
        case '0':
            dt = sortResults($(".listitems li"), 'ind', true);
            break;
        case '1':
            dt = sortResults($(".listitems li"), 'prc', true);
            break;
        case '2':
            dt = sortResults($(".listitems li"), 'prc', false);
            break;
        case '3':
            dt = sortResults($(".listitems li"), 'mlg', false);
            break;
    }
    var htm = '';
    for (var i = 0, l = dt.length; i < l; i++) {
        htm += dt[i].outerHTML;
    }
    $(".listitems").html('');
    var ul = document.getElementById('listitems');
    ul.insertAdjacentHTML('beforeend', htm);
    applyTabsLazyLoad();
});


function sortResults(mydata, prop, asc) {
    return mydata.sort(function (a, b) {
        if (asc) return (parseInt($(a).attr(prop)) > parseInt($(b).attr(prop))) ? 1 : ((parseInt($(a).attr(prop)) < parseInt($(b).attr(prop))) ? -1 : 0);
        else return (parseInt($(b).attr(prop)) > parseInt($(a).attr(prop))) ? 1 : ((parseInt($(b).attr(prop)) < parseInt($(a).attr(prop))) ? -1 : 0);
    });
}

var sortByDiv = $(".sort-div"),
    sortListDiv = $(".sort-selection-div"),
    sortCriteria = $('#sort'),
    sortByDiv = $(".sort-div"),
    sortListDiv = $(".sort-selection-div"),
    sortListLI = $(".sort-selection-div ul li");

sortByDiv.click(function () {
    if (!sortByDiv.hasClass("open"))
        $.sortChangeDown(sortByDiv);
    else
        $.sortChangeUp(sortByDiv);
});

$.sortChangeDown = function (sortByDiv) {
    sortByDiv.addClass("open");
    sortListDiv.show();
};

$.sortChangeUp = function (sortByDiv) {
    sortByDiv.removeClass("open");
    sortListDiv.slideUp();
};

function applyTabsLazyLoad() {
    $("img.lazy").lazyload({
        failure_limit: 20
    });
}
