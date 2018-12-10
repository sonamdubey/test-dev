var filter = (function () {
    var filterTypes = { lessorequal: 0, greaterorequal: 1, equal: 2}; // type of filter operations, that an algorithm can perform
    var chosenFilters = { lessorequal: {}, greaterorequal: {}, equal: {}}; // filter that are chosen
    var data = []; // data given to filter
    var filteredData = []; // filtered data

    function initFilterTypes() {
        var filterSelection = $(".filter-types li[data-filter-type]"); // algo, looks for chosen filters in this section
        if (filterSelection.length) {
            filterSelection.each(function (index, value) {
                var type = $(value).attr("data-filter-type");
                if (filterTypes.hasOwnProperty(type)) {
                    var value = $(value).attr("data-filter-value");
                    if (value) {
                        chosenFilters[type][value] = { resultIndex: -1 };
                    }
                }
                else {
                    console.log("filter type :" + type + ": not valid");
                }
            });
        }
        else {
            console.log("No filter types are defined");
        }
    }

    // gather data to be filtered
    function initData() {
        var filterData = $(".filter-data li[data-filter-year]"); // algo, looks for data to be filtered in this section
        if (filterData.length) {
            filterData.each(function (index, value) {
                var year = $(value).attr('data-filter-year');
                var month = $(value).attr('data-filter-month') - 1; // In javascript, month index start from '0' end with '11'.
                var day = $(value).attr('data-filter-day');         // i.e 0-Jan,11-December
                var d = new Date(year, month, day);
                if (!isNaN(d.valueOf()))
                {
                    data.push({ term: d, node: $(value) });
                }
            });
        }
        else {
            console.log("No filter data found.");
        }
    }

    // + is used when comparing date.
    function lessOrEqualComparator(x,y)
    {
        return (+x <= +y);
    }

    function equalComparator(x, y)
    {
        return (+x === +y);
    }

    function greaterOrEqualComparator(x, y)
    {
        return (+x >= +y);
    }

    // return date comparator function, based on condition
    function getComparer(comparator)
    {
        var comparer;
        switch (comparator) {
            case filterTypes.lessorequal:
                comparer = lessOrEqualComparator;
                break;
            case filterTypes.greaterorequal:
                comparer = greaterOrEqualComparator;
                break;
            case filterTypes.equal:
                comparer = equalComparator;
                break;
            default:
                comparer = equalComparator;
        }
        return comparer;
    }

    function isCorrectDate(x,y)
    {
        var today = new Date();
        today.setHours(0, 0, 0, 0);
        return (+x <= +today && +y <= +today)
    }
    // utilty used by filterData function to filter data.
    function filterDataUtil(filterObj, comparator) {
        var comparer = getComparer(comparator);
        for (var key in filterObj) {
            var res = [];
            var filterDate = new Date();
            filterDate = new Date(filterDate.setDate(filterDate.getDate() - key));
            if (!isNaN(filterDate.valueOf())) {
                $(data).each(function (index, value) {
                    filterDate.setHours(0, 0, 0, 0); // set hours to 0, for comparing date part only.[exclude time i.e hh:mm:ss]
                    if (isCorrectDate(value.term, filterDate) && comparer(value.term, filterDate))
                    {
                        res.push(index);
                    }
                });
            }
            if (res.length) {
                filteredData.push(res);
                filterObj[key].resultIndex = filteredData.length - 1;
            }
        }
    }

    function isEmptyObject(obj) {
        return jQuery.isEmptyObject(obj);
    }
    // filter the data based on chosen filter
    function filterData() {
        if (!isEmptyObject(chosenFilters)) {
            if (!isEmptyObject(chosenFilters.lessorequal)) {
                filterDataUtil(chosenFilters.lessorequal, filterTypes.lessorequal);
            }

            if (!isEmptyObject(chosenFilters.greaterorequal)) {
                filterDataUtil(chosenFilters.greaterorequal, filterTypes.greaterorequal);
            }

            if (!isEmptyObject(chosenFilters.equal)) {
                filterDataUtil(chosenFilters.equal, filterTypes.equal);
            }
        }
        else {
            console.log("No filters are chosen to filter data");
        }
    }

    // return count of specified filter if it is valid
    function selectCount(filterType, filterValue) {
        if (chosenFilters.hasOwnProperty(filterType)) {
            var index = getIndex(filterType, filterValue);
            if (index == -1)
                return 0;
            return filteredData[index].length;
        }
        else {
            console.log("no such filter type :" + filterType + " have been registered");
        }
        return 0;
    }

    function getIndex(filterType, filterValue) {
        return chosenFilters[filterType][filterValue] ? chosenFilters[filterType][filterValue].resultIndex : -1;
    }
    // select filter nodes of specified filter type if it is valid.
    function select(filterType, filterValue) {
        if (chosenFilters.hasOwnProperty(filterType)) {
            var index = getIndex(filterType, filterValue);
            if (index == -1)
                return null;
            var resultArrayIndexes = filteredData[index];
            var resultArray = [];
            $(resultArrayIndexes).each(function (index, value) {
                resultArray.push(data[value].node);
            });
            return resultArray;
        }
        else {
            console.log("no such filter type :" + filterType + " have been registered");
        }
    }
    function initialize() {
        initFilterTypes();
        initData();
        filterData();
    }
    return {
        initialize: initialize,
        select: select,
        selectCount: selectCount
    }
})();

$(document).ready(function () {
    if(typeof inquiries != 'undefined')
    {
        inquiries.initiate();
    }
});