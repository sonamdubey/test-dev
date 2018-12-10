var effect = 'slide',
    optionRight = { direction: 'right' },
    duration = 500;

var modelList = []
var versionList = [];

var common = {
    sortList: function (property) {
        return function (item1, item2) {
            return (item1[property].toLowerCase() < item2[property].toLowerCase()) ? -1 : (item1[property].toLowerCase() > item2[property].toLowerCase()) ? 1 : 0;
        }
    }
};

var carPopup = {
    container: $('#select-car-cover-popup'),
    loader: $('.cover-popup-loader-body'),
    makeBody: $('#select-make-wrapper'),
    modelBody: $('#select-model-wrapper'),

    open: function () {
        carPopup.container.show(effect, optionRight, duration, function () {
            carPopup.container.addClass('extra-padding');
            $('html, body').addClass('lock-browser-scroll');
        });
    },

    close: function () {
        carPopup.container.hide(effect, optionRight, duration, function () {
            carPopup.container.removeClass('extra-padding');
            $('html, body').removeClass('lock-browser-scroll');
        });
    },

    scrollToHead: function () {
        carPopup.container.animate({ scrollTop: 0 });
    }
};

var carSelection = function () {
    var self = this;
    self.make = ko.observable();
    self.model = ko.observable();
    self.IsLoading = ko.observable(false);
    self.currentStep = ko.observable(0);
    self.lastStep = ko.observable(4);
    self.modelArray = ko.observableArray();
    self.versionArray = ko.observableArray();

    self.makeChanged = function (data, event) {
        self.currentStep(self.currentStep() + 1);

        self.IsLoading(true);
        var element = $(event.currentTarget).find('span');

        self.make({
            id: element.data("id"),
            name: element.text().trim()
        });

        carPopup.scrollToHead();

        try {
            if (self.make() && self.make().id > 0) {
                self.modelArray(null);
                self.getModels();
                self.modelArray(modelList);
            }
        } catch (e) {
            console.warn(e);
        }
    }

    self.modelChanged = function (data, event) {
        self.currentStep(self.currentStep() + 1);

        self.IsLoading(true);
        var element = $(event.currentTarget).find('span');

        self.model({
            id: element.data("id"),
            name: element.text().trim()
        });

        carPopup.scrollToHead();

        try {
            if (self.model() && self.model().id > 0) {
                self.versionArray(null);
                self.getVersions();
                self.versionArray(versionList);
            }
        } catch (e) {
            console.warn(e);
        }
    }

    self.versionChanged = function (data, event) {
        if (self.model() && self.model().id > 0) {
            self.currentStep(self.currentStep() + 1);
            self.IsLoading(true);
            self.currentStep(0);
            carPopup.close();
            window.location.href = '/m/user-reviews/rate-car/?makeId=' + self.make().id + '&modelId=' + self.model().id + '&versionId=' + data.Id;
        }
    }

    self.getModels = function () {
        $.ajax({
            type: 'GET',
            url: '/webapi/CarModelData/GetNewModelsByMake/?&makeId=' + self.make().id + '&dealerId=0',
        }).done(function (response) {
            self.modelArray(JSON.parse(response).sort(common.sortList('ModelName')));
        }).fail(function () {
            self.make(null);
            self.modelArray(null);
        }).always(function () {
            self.IsLoading(false);
            window.history.pushState('selectCarModel', '', '');
        });
    }

    self.getVersions = function () {
        $.ajax({
            type: 'GET',
            url: '/webapi/CarVersionsData/GetVersionSummaryByModel/?&modelId=' +self.model().id,
        }).done(function (response) {
            response = $.grep(response, function (version) { return version.New; });
            self.versionArray(response.sort(common.sortList('Version')));
        }).fail(function () {
            self.make(null);
            self.model(null);
            self.versionArray(null);
        }).always(function () {
            self.IsLoading(false);
            window.history.pushState('selectCarVersion', '', '');
        });
    }

    self.modelBackBtn = function () {
        self.currentStep(self.currentStep() - 1);
    }

    self.closeCarPopup = function () {
        self.currentStep(0);
        window.history.back();
        carPopup.close();
    }

    $(window).on('popstate', function (event) {
        if ($('#select-make-wrapper').is(':visible')) {
            self.closeCarPopup();
        }
        else if ($('#select-model-wrapper').is(':visible')) {
            self.modelBackBtn();
        }
        else if ($('#select-variant-wrapper').is(':visible')) {
            self.modelBackBtn();
        }
    });
};

var rateCarVM = function () {
    var self = this;
    self.carSelection = ko.observable(new carSelection());
    self.carPopup = carPopup;
    self.car = ko.observable();

    self.openCarSelection = function (car) {
        try {
            self.carSelection().IsLoading(false);
            self.carPopup.open();
            window.history.pushState('selectCarMake', null, null);
            self.carSelection().currentStep(1);
        } catch (e) {
            console.warn(e)
        }
    }
};

var vmRateCarVM = new rateCarVM();

ko.applyBindings(vmRateCarVM, document.getElementById('rate-car-landing'));
window.history.pushState('pageLoad', null, null);