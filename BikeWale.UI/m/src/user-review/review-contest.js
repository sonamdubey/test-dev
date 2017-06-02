// read more list - collapse
$(document).on('click','.read-more-list', function () {
    var element = $(this),
        parentElemtent = element.closest('.collapsible-list');

    if (!parentElemtent.hasClass('active')) {
        parentElemtent.addClass('active');
        element.text('Collapse [-]');
    }
    else {
        parentElemtent.removeClass('active');
        element.text('Read more [+]');
        $('body').scrollTop(parentElemtent.offset().top - 20);
    }
});

var winnerSwiper = new Swiper('.winner-review-swiper', {
    pagination: '.winner-review-swiper .swiper-pagination',
    paginationClickable: 'true',
    effect: 'coverflow',
    grabCursor: true,
    centeredSlides: true,
    slidesPerView: 'auto',
    coverflow: {
        rotate: 50,
        stretch: 0,
        depth: 100,
        modifier: 1,
        slideShadows : true
    }
});

docReady(function () {

	var effect = 'slide',
		optionRight = { direction: 'right' },
		duration = 500;

	var makeField = $('#form-make-field'),
		modelField = $('#form-model-field');

    var bikePopup = {

        container: $('#select-bike-cover-popup'),

        open: function () {
            bikePopup.container.show(effect, optionRight, duration, function () {
                bikePopup.container.addClass('extra-padding');
                popup.lock();
            });
        },

        close: function () {
            bikePopup.container.hide(effect, optionRight, duration, function () {
                bikePopup.container.removeClass('extra-padding');
                popup.unlock();
            });
        },

        scrollToHead: function () {
            bikePopup.container.animate({ scrollTop: 0 });
        }
    };

    var bikeSelection = function () {
        var self = this;
        self.make = ko.observable();
        self.model = ko.observable();
        self.IsLoading = ko.observable(false);
        self.LoadingText = ko.observable('Loading...');
        self.currentStep = ko.observable(0);
        self.modelArray = ko.observableArray();

		self.makeName = ko.observable('Select make');
		self.modelName = ko.observable('Select model');

        self.makeChanged = function (data, event) {
            self.currentStep(2);

            self.LoadingText("Loading bike models...");
            var element = $(event.currentTarget).find("span"), _modelsCache;

            self.make({
                id: element.data("id"),
                name: element.text().trim()
            });

			self.makeName(self.make().name);
			makeField.attr('data-make-id', self.make().id);
			hideError(makeField);

			self.modelName('Select model');
			modelField.attr('data-model-id', '');
			

            bikePopup.scrollToHead();

            try {
                if (self.make() && self.make().id > 0) {
                    self.modelArray(null);
                    self.IsLoading(true);
                    var _cmodelsKey = "models_" + self.make().id;
                    _modelsCache = bwcache.get(_cmodelsKey, true);
                    if (!_modelsCache) {
                        $.getJSON("/api/modellist/?requestType=2&makeId=" + self.make().id)
                        .done(function (res) {
                            self.modelArray(res.modelList);
                            bwcache.set(_cmodelsKey, res, true);
                        })
                        .fail(function () {
                            self.make(null);
                            self.modelArray(null);
                        })
                        .always(function () {
                            self.IsLoading(false);
                        });
                    }
                    else {
                        self.modelArray(_modelsCache.modelList);
                        self.IsLoading(false);
                    }
                }
            } catch (e) {
                console.warn(e);
            }

        };

        self.modelChanged = function (data, event) {
            self.model(data);
			self.modelName(data.modelName);
			modelField.attr('data-model-id', data.modelId);
			hideError(modelField);

            if (self.model() && self.model().modelId > 0) {
                self.currentStep(self.currentStep() + 1);
				self.closeBikePopup();
            }
        }

        self.modelBackBtn = function () {
            self.currentStep(self.currentStep() - 1);
        };

        self.closeBikePopup = function () {
            self.currentStep(0);
            history.back();
        };
	};

	var rateBikeVM = function () {
        var self = this;
        self.bikeSelection = ko.observable(new bikeSelection());
        self.bikePopup = bikePopup;

		self.makeName = ko.computed(function() {
			return self.bikeSelection().makeName();
		});

		self.modelName = ko.computed(function() {
			return self.bikeSelection().modelName();
		});

        self.openBikeSelection = function () {
            try {
                self.bikePopup.open();
                window.history.pushState('selectBike', '', '');
                self.bikeSelection().currentStep(1);
            } catch (e) {
                console.warn(e)
            }
        };

		self.openModelSelection = function () {
			try {
				if(makeField.attr('data-make-id') > 0) {
					self.bikePopup.open();
					window.history.pushState('selectBike', '', '');
					self.bikeSelection().currentStep(2);
				}
            } catch (e) {
                console.warn(e)
            }
		};
    };

    var vmRateBikeVM = new rateBikeVM();

    ko.applyBindings(vmRateBikeVM, document.getElementById("bike-selection-form"));

    $(window).on('popstate', function (event) {
        if ($('#select-bike-cover-popup').is(':visible')) {
            bikePopup.close();
        }
    });


	$("#submit-bike-selection").click(function () {
		if(makeField.attr('data-make-id') > 0) {
			if(modelField.attr('data-model-id') > 0) {
			    var returnUrl = $('#querystring').data('query');
				window.location = "/m/rate-your-bike/" + Number(modelField.attr('data-model-id')) + "/?q=" + returnUrl;
			}
			else {
				setError(modelField, 'Please select bike model!');
			}
		}
		else {
			setError(makeField, 'Please select bike make!');
		}
    });

	var setError = function (element, msg) {
        element.addClass("border-red").find(".errorIcon, .errorText").show();
        element.find(".errorText").text(msg);
    };

    var hideError = function (element) {
        element.removeClass("border-red").find(".errorIcon, .errorText").hide();
    };

	var popup = {
        lock: function () {
            var htmlElement = $('html'), bodyElement = $('body');
            
            if ($(document).height() > $(window).height()) {
                var windowScrollTop = htmlElement.scrollTop() ? htmlElement.scrollTop() : bodyElement.scrollTop();
                if (windowScrollTop < 0) {
                    windowScrollTop = 0;
                }
                htmlElement.addClass('lock-browser-scroll').css('top', -windowScrollTop);
            }
        },

        unlock: function () {
            var htmlElement = $('html'),
                windowScrollTop = parseInt(htmlElement.css('top'));
            
            htmlElement.removeClass('lock-browser-scroll');
            $('html, body').scrollTop(-windowScrollTop);
        }
	};
});