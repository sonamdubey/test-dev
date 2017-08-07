var $dateInput = $('.datepicker').pickadate({
    selectMonths: true, // Creates a dropdown to control month
    closeOnSelect: true,
    onSet: function (ele) { if (ele.select) { this.close(); } }
});


var pgContainer = $("#sponsoredComparisons"), vmSponsoredComparison;
var searchComparisons = $("#searchComparisons"), searchStatusChks = searchComparisons.find("input:checkbox");
var bwHostUrl = pgContainer.data("bwhosturl");


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
            self.currentId(0);
            return true;
        }
        return false;
    }

    self.validateAddComparison = function () {
        var isValid = false;
        if (self.description() && self.description().trim() != "") {
            isValid = true;
            $("#txtComparisonDescription").removeClass("invalid");
        }
        else {
            self.description("");
            $("#txtComparisonDescription").addClass("invalid");
        }

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

        var startDt = new Date(self.startDateTime()), EndDt = new Date(self.endDateTime()), sTime, eTime;


        if (startDt && EndDt) {
            sTime = startDt.getTime(), eTime = EndDt.getTime();
        }
        else {
            sTime = 0; eTime = 0;
        }

        if (isValid && startDt < EndDt) {
            isValid = true;
        }
        else {
            isValid = false;
            Materialize.toast("Comparison End Date must be greater than Start Date", 5000);
        }

        return isValid;
    }

    self.editSponsoredCamparison = function (d, e) {
        try {
            var ele = $(e.currentTarget).closest("tr");
            self.currentId(ele.find("td[data-cell=id]").text().trim());
            self.description(ele.find("td[data-cell=description]").text().trim());
            self.startDateTime(ele.find("td[data-cell=startdate]").text().trim());
            self.endDateTime(ele.find("td[data-cell=enddate]").text().trim());
            self.linkUrl(ele.find("td[data-cell=linkurl]").text().trim());
            self.linkText(ele.find("td[data-cell=linktext]").text().trim());
            self.nameImpression(ele.find("td[data-cell=nameimpression]").text().trim());
            self.imgImpression(ele.find("td[data-cell=imgimpression]").text().trim());

            self.isComparisonEdit(true);

            //setdate

            if (self.startDateTime().trim() != "") {
                var tempDateTime = new Date(self.startDateTime());
                $("#dtStartDate").pickadate('picker').set('select', tempDateTime);
                var th = tempDateTime.getHours();
                if (th < 10) th = ("0" + th + "");
                var tm = tempDateTime.getHours();
                if (tm < 10) tm = ("0" + tm + "");
                self.startTime(th + ":" + tm);
            }



            if (self.endDateTime().trim() != "") {
                var tempDateTime = new Date(self.endDateTime());
                $("#dtEndDate").pickadate('picker').set('select', tempDateTime);
                var th = tempDateTime.getHours();
                if (th < 10) th = ("0" + th + "");
                var tm = tempDateTime.getHours();
                if (tm < 10) tm = ("0" + tm + "");
                self.endTime(th + ":" + tm);
            }

            Materialize.updateTextFields();
            var tab = pgContainer.find(".collapsible-header").last();
            if (!tab.hasClass("active")) {
                tab.click();
            }
        } catch (e) {
            console.warn(e.msg);
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
        var ele = $(e.currentTarget), status = ele.attr("data-status");
        if (status && status!="0" && status > 0) {
            $.post("/api/compare/sponsored/" + self.currentId() + "/updatestatus/" + status + "/", function () {
                if (self.currentId() > 0) {
                    pgContainer.find("tr[data-id=" + self.currentId() + "]").fadeOut();
                }
                Materialize.toast("Sponsored comparison status changed", 3000);
            })
            .fail(function () {
                Materialize.toast("Sponsored comparison staus update failed", 3000);
            })
            .always(function () {
                self.currentId(0);
                $(".material-tooltip").hide();
                if (status == 2) {
                    //for abort
                    var abEle = ele.clone();
                    abEle.empty();
                    abEle.append("<i class='material-icons icon-red'>cancel</i>");
                    abEle.attr("data-status", 5)
                    abEle.appendTo(ele.parent());
                    ko.applyBindings(self, abEle[0]);

                    ele.empty();
                    ele.append("<i class='material-icons icon-blue'>pause_circle_filled</i>");
                    ele.attr("data-status", 3);
                    ele.closest("td").find("span").text("Active");
                    
                }
                else if (status == 5) {
                    ele.closest("td").text("Aborted");
                } else if(status==3) {
                    ele.empty();
                    ele.attr("data-status", 2);
                    ele.append("<i class='material-icons icon-green'>play_circle_filled</i>");
                    ele.next().remove();
                    ele.closest("td").find("span").text("Paused");
                }
            });;

        }
        return false;
    };

    self.ConvertTimeformat = function (format, time) {
        var hours = Number(time.match(/^(\d+)/)[1]);
        var minutes = Number(time.match(/:(\d+)/)[1]);
        var AMPM = time.match(/\s(.*)$/)[1];
        if (AMPM == "PM" && hours < 12) hours = hours + 12;
        if (AMPM == "AM" && hours == 12) hours = hours - 12;
        var sHours = hours.toString();
        var sMinutes = minutes.toString();
        if (hours < 10) sHours = "0" + sHours;
        if (minutes < 10) sMinutes = "0" + sMinutes;
        return (sHours + ":" + sMinutes);
    }

};

$(function () {
    vmSponsoredComparison = new sponsoredComparisonManagement();
    ko.applyBindings(vmSponsoredComparison, pgContainer[0]);
});