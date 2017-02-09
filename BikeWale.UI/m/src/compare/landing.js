
$(document).on('click', '.compare-box-placeholder', function () {
    $(this).closest('.bike-details-block').attr("data-changed", true);
    bikePopup.open();
    appendState('selectBike');
});

/* close selected model */
$('.comparison-main-card').on('click', '.close-selected-bike', function () {
    compareBox.removeBike($(this));
});

var compareBox = {
    removeBike: function (element) {
        var detailsBlock = $(element).closest('.bike-details-block'),
            detailsBlockIndex = detailsBlock.index();

        compareBox.setBikePlaceholder(detailsBlock, detailsBlockIndex);
    },

    setBikePlaceholder: function (element, elementIndex) {
        $(element).empty();
        placeholderBlock = "<div class='compare-box-placeholder'><div class='bike-icon-wrapper'><span class='grey-bike'></span><p class='font14 text-light-grey'>Tap to select bike " + (elementIndex + 1) + "</p></div></div>";
        $(element).append(placeholderBlock);

    }
};

var bikeSelection = function () {
    var self = this;

    self.makeId = ko.observable('');
    self.modelId = ko.observable('');
    self.versionId = ko.observable('');
    self.compareSource = ko.observable(6);
    self.bikeData = ko.observable();

    self.redirectionUrl = function () {
        var _link = "";
        try {
            if (self.makeId() > 0) {
                makemasking = $("#select-make-wrapper ul li[data-id='" + self.makeId() + "']").data("masking");
                if (self.modelId() > 0) {
                    modelmasking = $("#select-model-wrapper ul li[data-id='" + self.modelId() + "']").data("masking");

                    if (self.versionId() > 0) {
                        var ele = $(".comparison-main-card .bike-details-block[data-changed='true']"), loc = window.location;

                        _link = loc.pathname.replace(ele.data("masking"), makemasking + "-" + modelmasking);

                        if (window.location.search.indexOf("bike") > -1) {
                            _link = _link + loc.search.replace(ele.data("versionid"), self.versionId());
                            _link.replace(/source=\d/, 'source=' + self.compareSource());
                        }
                        else {
                            var searchQuery = "?";
                            $(".bike-details-block").each(function (i) {
                                var el = $(this);
                                if (!el.hasClass('sponsored-bike-details-block') && el.data("versionid")) {
                                    searchQuery += ("&bike" + (i + 1) + "=" + el.data("versionid"));
                                }
                            });
                            searchQuery = searchQuery.replace(ele.data("versionid"), self.versionId());
                            _link += (searchQuery + (searchQuery != "" ? "&source=" + self.compareSource() : ""));
                        }
                    }
                }
            }
        } catch (e) {
            console.warn(e);
        }

        return _link;
    };

    self.modelArray = ko.observableArray();
    self.versionArray = ko.observableArray();

    self.makeChanged = function (data, event) {

        var element = $(event.currentTarget).find("span");

        self.makeId(element.attr("data-id"));

        bikePopup.stageModel();
        bikePopup.scrollToHead();

        try {
            if (self.makeId()) {
                $.ajax({
                    type: "Get",
                    async: false,
                    url: "/api/modellist/?requestType=2&makeId=" + self.makeId(),
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {
                        if (response) {
                            self.modelArray(response.modelList);
                        }
                    },
                    complete: function (xhr) {
                        if (xhr.status != 200) {
                            self.makeId();
                        }
                    }
                });
            }
        } catch (e) {
            console.warn(e);
        }

    };

    self.modelChanged = function (data, event) {

        self.modelId(data.modelId);

        bikePopup.stageVersion();
        bikePopup.scrollToHead();

        try {
            if (self.modelId()) {
                $.ajax({
                    type: "Get",
                    url: "/api/versionList/?requestType=2&modelId=" + self.modelId(),
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {
                        self.versionArray(response.Version);
                    }
                });
            }
        } catch (e) {
            console.warn(e);
        }

    };

    self.versionChanged = function (data, event) {

        self.versionId(data.versionId);

        try {
            if (self.versionId()) {
                $.ajax({
                    type: "Get",
                    url: "/api/version/?versionid=" + self.versionId(),
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {
                        debugger;
                        self.bikeData(response);
                        $('#select-version-wrapper .same-version-toast').hide();

                        if (!bikePopup.checkSameVersion(self.versionId()) && self.versionId() > 0) {
                            self.setCompareBikeHTML();
                        }
                        else {
                            bikePopup.showSameVersionToast();
                        }
                    },
                    complete: function (xhr) {
                        if (xhr.status != 200) {

                        }
                    }
                });
            }
        } catch (e) {
            console.warn(e);
        }
    };

    self.setCompareBikeHTML = function () {

        try {
            if (self.makeId() > 0 && self.modelId() > 0 && self.versionId() > 0) {

                var makeEle = $("#select-make-wrapper ul li span[data-id='" + self.makeId() + "']"),
                modelEle = $("#select-model-wrapper ul li span[data-id='" + self.modelId() + "']"),
                versionEle = $("#select-version-wrapper ul li span[data-id='" + self.versionId() + "']"),
                ele = $(".comparison-main-card .bike-details-block[data-changed='true']"),bdata;
                if (self.bikeData())
                {
                    bdata = self.bikeData();
                    bdata.make = makeEle.text().trim();
                    bdata.model = modelEle.text().trim();
                    bdata.version = versionEle.text().trim();

                    ele.html(self.compareBikeTemplate(bdata));
                    bikePopup.close();
                    ele.attr("data-changed", false);
                    ele.attr("data-bikeversion", self.versionId());
                    ele.attr("data-bikeurl", makeEle.data("masking") + "-" + modelEle.data("masking"));
                }
              
            }
        } catch (e) {
            console.warn(e);
        }

    };

    self.compareBikeTemplate = function (_bdata) {
        var eleHTML = "";
        if (_bdata) {

            eleHTML = ' <span class="close-selected-bike position-abt pos-right5 bwmsprite cross-sm-dark-grey"></span> '
                            + '<a href="" title="' + _bdata.bikeName + '" class="block margin-top10">'
                            + '<span class="font12 text-light-grey text-truncate">' + _bdata.make + '</span>'
                            + '<h2 class="font14 text-truncate margin-bottom5">' + _bdata.model + '</h2> '
                            + '<img class="bike-image-block" src="' + _bdata.hostUrl + '/110x61/' + _bdata.originalImagePath + '" alt="' + _bdata.bikeName + '">'
                            + '</a> '
                            + '<p class="label-text">Version:</p> '
                            + '<p class="padding-bottom10 text-bold dropdown-selected-item option-count-one dropdown-width">' + _bdata.version + '</p>  '
                            + '<p class="text-truncate label-text">Ex-showroom, Mumbai</p> '
                            + '<p class="margin-bottom10"> '
                            + '    <span class="bwmsprite inr-xsm-icon"></span> <span class="font16 text-bold"> ' + self.FormatBikePrice(_bdata.price) + '</span> '
                            + '</p>';
        }

        return eleHTML;
    }

    self.modelBackBtn = function () {
        bikePopup.stageMake();
    };

    self.versionBackBtn = function () {
        bikePopup.stageModel();
    };

    self.closeBikePopup = function () {
        bikePopup.close();
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

var vmBikeSelection = new bikeSelection();




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
            bikePopup.stageMake();
        });

        bikePopup.container.removeClass('extra-padding');
        $('html, head').removeClass('lock-browser-scroll');
    },

    stageMake: function () {
        bikePopup.modelBody.hide();
        bikePopup.versionBody.hide();
        bikePopup.makeBody.show();
    },

    stageModel: function () {
        bikePopup.makeBody.hide();
        bikePopup.versionBody.hide();
        bikePopup.modelBody.show();
    },

    stageVersion: function () {
        bikePopup.makeBody.hide();
        bikePopup.modelBody.hide();
        bikePopup.versionBody.show();
    },

    showLoader: function () {
        bikePopup.container.find(bikePopup.loader).show();
    },

    hideLoader: function () {
        bikePopup.container.find(bikePopup.loader).hide();
    },

    scrollToHead: function () {
        bikePopup.container.animate({ scrollTop: 0 });
    },
    checkSameVersion: function (versionId) {
        var isSameVersionSelected = false;
        $(".bike-details-block").each(function () {
            var ele = $(this);
            if (!ele.hasClass('sponsored-bike-details-block') && ele.data("versionid") == versionId && ele.data("changed").toString() != 'true') {
                isSameVersionSelected = true;
            }
        });

        return isSameVersionSelected;
    },
    showSameVersionToast: function () {
        window.clearTimeout();
        $('section .same-version-toast').slideDown();
        window.setTimeout(function () {
            $('section .same-version-toast').slideUp();
        }, 2000);
    }
};

ko.applyBindings(vmBikeSelection, document.getElementById("select-bike-cover-popup"));

/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');
};

$(window).on('popstate', function (event) {
    if ($('#select-bike-cover-popup').is(':visible')) {
        bikePopup.close();
    }
});

