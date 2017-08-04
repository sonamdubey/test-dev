var $dateInput = $('.datepicker').pickadate({
    selectMonths: true, // Creates a dropdown to control month
    closeOnSelect: true,
    onSet: function (ele) { if (ele.select) { this.close(); } }
});


var pgContainer = $("#sponsoredComparisons"), vmSponsoredComparison;
var searchComparisons = $("#searchComparisons"), searchStatusChks = searchComparisons.find("input:checkbox");


var sponsoredComparisonManagement = function () {
    var self = this;
    self.description = ko.observable();
    self.startDateTime = ko.observable();
    self.endDateTime = ko.observable();
    self.linkText = ko.observable();
    self.linkUrl = ko.observable();
    self.nameImpression = ko.observable();
    self.imgImpression = ko.observable();
    self.startTime = ko.observable();
    self.endTime = ko.observable();
    self.startDate = ko.observable();
    self.endDate = ko.observable();
    self.currentId = ko.observable(0);
    self.isComparisonEdit = ko.observable(false);

    self.init = function () {
        var compStatus = window.location.search;
        if (/comparisonType/ig.test(compStatus)) {
            statusLst = compStatus.split("=")[1];
            statusLst = statusLst != "" ? decodeURIComponent(statusLst).split(",") : null;
            if (statusLst && statusLst.length > 0) {
                $.each(statusLst, function (i) {
                    searchComparisons.find("input:checkbox[data-value=" + statusLst[i] + "]").prop("checked", true);
                });
            }
            else {
                searchComparisons.find("input:checkbox[data-value=2]").prop("checked", true);
            }
        }
        else {
            searchComparisons.find("input:checkbox[data-value=2]").prop("checked", true);
        }
        $(".picker + label").each(function () {
            $(this).insertBefore($(this).prev());
        });

        var tab = pgContainer.find(".collapsible-header").first();
        if (!tab.hasClass("active")) {
            tab.click();
        }

    }();

    self.checkAllStatus = function (d, e) {
        var ele = $(e.currentTarget);
        if (ele.prop("checked")) {
            searchStatusChks.prop("checked", true);
        }
        else {
            searchStatusChks.prop("checked", false);
        }
        return true;
    };

    self.searchSponsoredCamparisons = function (d, e) {
        var ele = $(e.currentTarget);
        var statusLst = "";
        searchStatusChks.each(function () {
            if ($(this).prop("checked")) {
                statusLst += ("," + $(this).data("value"));
            }

        });
        statusLst = statusLst.substr(1);
        ele.val(statusLst);
        return true;
    };

    self.startDate.subscribe(function () {
        self.startDateTime((self.startDate() || '') + " " + (self.startTime() || ''));
    });
    self.startTime.subscribe(function () {
        self.startDateTime((self.startDate() || '') + " " + (self.startTime() || ''));
    });

    self.endDate.subscribe(function () {
        self.endDateTime((self.endDate() || '') + " " + (self.endTime() || ''));
    });
    self.endTime.subscribe(function () {
        self.endDateTime((self.endDate() || '') + " " + (self.endTime() || ''));
    });

    self.addSponsoredComparison = function () {
        if (self.validateAddComparison()) {
            return true;
        }
        return false;
    }

    self.validateAddComparison = function () {
        var isValid = false;
        if (self.description() && self.description().trim() != "") {
            isValid = true;
        }
        else self.description("");

        if (self.startDateTime() && self.startDateTime().trim() != "") {
            isValid = true;
        }
        else {
            self.startDateTime("");
            isValid = false;
        }

        if (self.endDateTime() && self.endDateTime().trim() != "") {
            isValid = true;
        }
        else {
            self.endDateTime("");
            isValid = false;
        }
        return isValid;
    }

    self.editSponsoredCamparison = function (d, e) {
        var ele = $(e.currentTarget).closest("tr");
        self.currentId(ele.find("td[data-cell=id]").text().trim());
        self.description(ele.find("td[data-cell=description]").text());
        self.startDateTime(ele.find("td[data-cell=startdate]").text());
        self.endDateTime(ele.find("td[data-cell=enddate]").text());
        self.linkUrl(ele.find("td[data-cell=linkurl]").text());
        self.linkText(ele.find("td[data-cell=linktext]").text());
        self.nameImpression(ele.find("td[data-cell=nameimpression]").text());
        self.imgImpression(ele.find("td[data-cell=imgimpression]").text());

        self.isComparisonEdit(true);

        //setdate
        var tempDateTime = new Date(self.startDateTime());
        $("#dtStartDate").pickadate('picker').set('select', tempDateTime);
        var tm = tempDateTime.getHours().toString();
        if (tm.length < 2) tm = ("0" + tm);
        self.startTime(tm + ":" + tempDateTime.getMinutes());

        tempDateTime = new Date(self.endDateTime());
        $("#dtEndDate").pickadate('picker').set('select', tempDateTime);

        var tm = tempDateTime.getHours().toString();
        if (tm.length < 2) tm = ("0" + tm);

        self.endTime(tm + ":" + tempDateTime.getMinutes());

        Materialize.updateTextFields();
        var tab = pgContainer.find(".collapsible-header").last();
        if (!tab.hasClass("active")) {
            tab.click();
        }

    };

    self.cancelEditComparison = function () {
        self.currentId(0);
        self.description(undefined);
        self.startDateTime(undefined);
        self.endDateTime(undefined);
        self.startTime(undefined);
        self.endTime(undefined);
        self.startDate(undefined);
        self.endDate(undefined);
        self.linkUrl("");
        self.linkText("");
        self.nameImpression("");
        self.imgImpression("");
        self.isComparisonEdit(false);

        Materialize.updateTextFields();
    }

    self.changeComparisonStatus = function (d, e) {
        var ele = $(e.currentTarget), status = ele.data("status");
        if (status > 0) {
            $.post("/api/compare/sponsored/" + self.currentId() + "/updatestatus/" + status + "/", function () {
                if (self.currentId() > 0) {
                    pgContainer.find("tr[data-id=" + self.currentId() + "]").fadeOut();
                }
                Materialize.toast("Sponsored comparison status changed", 3000);
            })
            .fail(function () {
                Materialize.toast("Sponsored comparison staus update failed", 3000);
            })
            .always(function () { self.currentId(0); });;

        }
    };

};

$(function () {
    vmSponsoredComparison = new sponsoredComparisonManagement();
    ko.applyBindings(vmSponsoredComparison, pgContainer[0]);
});