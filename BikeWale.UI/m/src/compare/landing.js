
//var bikePopup = function () {

//};

var effect = 'slide',
    optionRight = { direction: 'right' },
    duration = 500;

var bikePopup = {

    container: $('#select-bike-cover-popup'),
    loader: $('.cover-popup-loader-body'), 
    makeBody: $('#select-make-wrapper'),
    modelBody: $('#select-model-wrapper'), 
    versionBody: $('#select-version-wrapper'),

    open: function () {
        bikePopup.container.show(effect, optionRight, duration, function () {
            bikePopup.container.addClass('extra-padding');
        });

        $('html, head').addClass('lock-browser-scroll');
    },

    close: function () {
        bikePopup.container.hide(effect, optionRight, duration, function () {

        });

        bikePopup.container.removeClass('extra-padding');
        $('html, head').removeClass('lock-browser-scroll');
    },
    scrollToHead: function () {
        bikePopup.container.animate({ scrollTop: 0 });
    },
    showSameVersionToast: function () {
        window.clearTimeout();
        $('section .same-version-toast').slideDown();
        window.setTimeout(function () {
            $('section .same-version-toast').slideUp();
        }, 2000);
    }
};

var bikeSelection = function () {
    var self = this;
    self.prevVersionId = ko.observable(0);
    self.make = ko.observable();
    self.model = ko.observable();
    self.version = ko.observable();
    self.bikeData = ko.observable();
    self.currentBike = ko.observable();
    self.IsLoading = ko.observable(false);
    self.LoadingText = ko.observable('Loading...');
    self.currentStep = ko.observable(0);
    self.lastStep = ko.observable(4);

    self.modelArray = ko.observableArray();
    self.versionArray = ko.observableArray();

    self.makeChanged = function (data, event) {
        self.currentStep(2);

        self.LoadingText("Loading bike models...");
        var element = $(event.currentTarget).find("span");

        self.make({
            id: element.data("id"),
            name: element.text().trim(),
            masking: element.data("masking")
        })

        bikePopup.scrollToHead();

        try {
            if (self.make() && self.make().id > 0) {
                self.modelArray(null);
                self.IsLoading(true);
                $.ajax({
                    type: "Get",
                    async: false,
                    url: "/api/modellist/?requestType=2&makeId=" + self.make().id,
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {
                        if (response) {
                            self.modelArray(response.modelList);
                        }
                    },
                    complete: function (xhr) {
                        if (xhr.status != 200) {
                            self.make(null);
                            self.modelArray(null) ;
                        }
                        self.IsLoading(false);
                    }
                });
            }
        } catch (e) {
            console.warn(e);
        }

    };

    self.modelChanged = function (data, event) {

        self.model(data);
        self.LoadingText("Loading bike versions...");
        self.currentStep(3);
        bikePopup.scrollToHead();

        try {
            if (self.model() && self.model().modelId > 0) {
                self.versionArray(null);
                self.IsLoading(true);
                $.ajax({
                    type: "Get",
                    url: "/api/versionList/?requestType=2&modelId=" + self.model().modelId,
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {
                        self.versionArray(response.Version);
                    },
                    complete : function(xhr)
                    {
                        if (xhr.status != 200) {
                            self.model(null);
                            self.versionArray(null);
                        }
                        self.IsLoading(false);
                    }
                });
            }
        } catch (e) {
            console.warn(e);
        }

    };

    self.versionChanged = function (data, event) {
        self.version(data);
        self.LoadingText("Loading bike version details...");
        try {
            if (self.version() && self.version().versionId > 0 && self.version().versionId != self.prevVersionId()) {
                self.IsLoading(true);
                self.prevVersionId(self.version().versionId);
                $.ajax({
                    type: "Get",
                    url: "/api/version/?versionid=" + self.version().versionId,
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {
                        self.bikeData(response);
                        self.setCompareBikeHTML();
                        self.currentStep(4);
                    },
                    complete: function (xhr) {
                        if (xhr.status != 200) {
                            self.bikeData(null);
                        }
                        self.IsLoading(false);
                    }
                });
            }
            else if (self.version() && self.version().versionId > 0) {
                bikePopup.showSameVersionToast();
            }
        } catch (e) {
            console.warn(e);
        }
    };

    self.setCompareBikeHTML = function () {

        try {
            if (self.make() && self.model() && self.version() && self.bikeData()) {

                bdata = self.bikeData();
                bdata.make = self.make();
                bdata.model = self.model();
                bdata.version = self.version();
                bdata.price = self.FormatBikePrice(bdata.price);
                bdata.bikeUrl = "/m/" + self.make().masking + "-bikes/" + self.model().maskingName + "/";
                self.currentBike()(bdata);
                bikePopup.close();

            }
        } catch (e) {
            console.warn(e);
        }

    };

    self.modelBackBtn = function () {
        self.currentStep(self.currentStep()-1);
    };

    self.versionBackBtn = function () {
        self.currentStep(self.currentStep() - 1);
    };

    self.closeBikePopup = function () {
        self.currentStep(0);
        history.back();
    };

    self.FormatBikePrice = function (price) {
        price = price.toString();
        try {
            var lastThree = price.substring(price.length - 3);
            var otherNumbers = price.substring(0, price.length - 3);
            if (otherNumbers != '')
                lastThree = ',' + lastThree;
            var price = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
            return price;
        } catch (e) {
            console.warn(e)
        }

        return price;
    }
};

var compareBike = function () {
    var self = this;
    self.bikeSelection = ko.observable(new bikeSelection());
    self.bikePopup = bikePopup;
    self.bike1 = ko.observable();
    self.bike2 = ko.observable();
    self.bike1Changed = ko.observable(false);
    self.bike2Changed = ko.observable(false);
    self.compareSource = ko.observable(6);

    self.openBikeSelection = function (bike) {
        try {
            self.bikeSelection().currentBike(bike);
            self.bikePopup.open();
            window.history.pushState('selectBike', '', '');
            self.bikeSelection().currentStep(1);
        } catch (e) {
           console.warn(e)
        }
    };

    self.compareLink = ko.computed(function () {
        var _link = "/m/comparebikes/";
        if(self.bike1() && self.bike2())
        {
            _link += (self.bike1().make.masking + "-" + self.bike1().model.maskingName + "-vs-"
                  + self.bike2().make.masking + "-" + self.bike2().model.maskingName + "/"
                  + "?bike1=" + self.bike1().version.versionId + "&bike2=" + self.bike2().version.versionId
                  + "&source=6");
        }
        return _link;
    });
};

ko.applyBindings(new compareBike(), document.getElementById("compare-bike-landing"));


$(window).on('popstate', function (event) {
    if ($('#select-bike-cover-popup').is(':visible')) {
        bikePopup.close();
    }
});

