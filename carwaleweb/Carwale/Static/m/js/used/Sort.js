var sort = function () {

    var $m_usp_doc = $(document);
    var $sortDivObj = $('#sort-div');
    var sortParams = ["so", "sc", "pn", "lcr", "lir", "ldr", "ps", "stockfetched"];
    function applySortBindings() {
        $(window).on('popstate', _popStateHandler);
        $m_usp_doc.on("click", "#btnSort", _sortScreenOpen);
        $m_usp_doc.on("click", "#btnSortBack", _sortScreenClose);
        $m_usp_doc.on("click", "#sortFilter li", _sortSelection._sortSelected);
    };
    var _popStateHandler = function () {
        if ($sortDivObj.is(':visible')) {
            $sortDivObj.hide('slide', { direction: 'right' }, 500);
        }
    };
    var _sortScreenOpen = function () {
        setSortValues();
        commonUtilities.addToHistory("sort");
        $sortDivObj.show('slide', { direction: 'right' }, 500);
    };
    var setSortValues = function () {
        var url = window.location.search.replace('?', '');
        var set = { "so": true, "sc": true };
        commonUtilities.getBulkValuesFromQs(set, url);
        if (set["so"] == true && set["sc"] == true) {
            set["so"] = set["sc"] = -1;
        }
        $("#sortFilter li").removeClass('active');
        $("#sortFilter li[so='" + set["so"] + "']").filter("[sc='" + set["sc"] + "']").addClass("active");

    };
    var _sortScreenClose = function () {
        history.back();
    };
    var _sortSelection = {
        _sortSelected: function () {
            _sortSelection._sortCriteriaClick(this);
        },
        _sortCriteriaClick: function (currentSortCriteria) {
            $sortDivObj.hide('slide', { direction: 'right' }, 500);
            this._sortTypeSelection($('#sortFilter li'), currentSortCriteria);
            var qs = this._getSortFilterQS(currentSortCriteria);
            var sortUrl = "/m/search/getsearchresults/" + qs;
            search.listing.getData(sortUrl, false, { isSort: true });
            window.history.replaceState(qs, null, "/m/used/cars-for-sale/" + qs);
            window.scrollTo(0, 0);
            cwTracking.trackCustomData('PageViews', '', 'NA', true);
        },
        _sortTypeSelection: function ($element, currentSortCriteria) {
            $element.removeClass("active");
            $(currentSortCriteria).addClass("active");
        },
        _getSortFilterQS: function (currentSortCriteria) {
            var $element = $(currentSortCriteria);
            var so = $element.attr("so");
            var sc = $element.attr("sc");
            var removeFilterSet = {};
            sortParams.forEach(function (param) {
                removeFilterSet[param] = true;
            });
            var newQS = search.getShouldFetchNearByCarQS(removeFilterSet);
            newQS += "&so=" + so + "&sc=" + sc + "&pn=1";
            return "?" + newQS;
        }
    };
    return { applySortBindings: applySortBindings, sortParams: sortParams, setSortValues: setSortValues };
}();
$(document).ready(function () {
    sort.applySortBindings();
});