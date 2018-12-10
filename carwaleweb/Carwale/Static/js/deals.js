var Deals = {
	env: 2,
	isCallCompleted: false,
	doc: $(document),
	apiHostUrl: "",
	trackingImg: $('#stockTrackingImg'),
	environmentId: "",  // declared in webconfig and its value will be 0 for staging and 1 for live
	pageId: "",
	modelId: "",
	cityId: "",
	cityName: "",
	versionId: "",
	isAdvantageCity: false,
	inquirySource: 0, //1 for Next btn, 2 for EMI assistance btn, 3 for photogallery form
	advantageDealerId: "",
	stockId: "",
	utils: {
		registerEvents: function (useHistory) {
			Deals.doc.on('click', '.accordion > .stepStrip', function () {
				Deals.utils.accordionFaq($(this));
			});

			Deals.doc.on('click', '#findCar', function () {
				Deals.utils.scrollToElement("make-model-popup");
			});
		},
		bindNumberSlug: function () {
			$('.ask-expert').addClass('hide');
		},

		removeTilde: function (value) {
			return value.replace(/\~/g, ' ');
		},

		accordionFaq: function (node) {
			$this = node;
			plusminus = $this.find('.icon.plus-minus .fa');
			if ($this.next().is(":visible")) {
				$this.next().slideToggle(0).parent().toggleClass('open');
			} else {
				$this.parent().siblings('.accordion').find('.stepStrip + div').slideUp(0);
				$('.accordion').removeClass('open');
				$this.next().slideDown(0).parent().addClass('open');

				if ($this.find('.icon').hasClass('plus-minus')) {
					$('.icon.plus-minus .fa').attr('class', 'fa fa-plus');
				}
			}

			if ($this.find('.icon').hasClass('plus-minus')) {
				if (plusminus.hasClass('fa-plus'))
					plusminus.removeClass('fa-plus').addClass('fa-minus');
				else
					plusminus.removeClass('fa-minus').addClass('fa-plus');
			}
		},

		getHexStyle: function (data) {
			return ("background:#" + data.colour.versionHexCode + "!important;");
		},
		imgfail: function (img) {
			$(img).attr("src", "https://img.carwale.com/used/no-cars.jpg")
		},

		showError: function (value) {
			alert(value);
		},

		validateInputField: function (field, regex) {
			try {
				var fieldVal;
				fieldVal = field.val();
				if (!regex.test(fieldVal.toLowerCase())) {
					Deals.utils.showErrorMessage(field);
					return false;
				}

				Deals.utils.hideErrorMessage(field);
				return true;
			}
			catch (e) {
				logError(e.message, e.stack, "Deals - deals.utils.validateInputField", location.pathname);
			}
			return false;
		},

		showErrorMessage: function (field) {
			try {
				field.addClass('border-red').siblings('.error-icon,.cw-blackbg-tooltip').removeClass('hide');
			}
			catch (e) {
				logError(e.message, e.stack, "Deals - deals.utils.showErrorMessage", location.pathname);
			}
		},

		showCustomeErrorMessage: function (field, errMsg) {
			try {
				field.addClass('border-red').siblings('.error-icon,.cw-blackbg-tooltip').removeClass('hide');
				field.siblings('.cw-blackbg-tooltip').text(errMsg);
			}
			catch (e) {
				logError(e.message, e.stack, "Deals - deals.utils.showErrorMessage", location.pathname);
			}
		},

		hideErrorMessage: function (field) {
			try {
				field.removeClass('border-red').siblings('.error-icon,.cw-blackbg-tooltip').addClass('hide');
			}
			catch (e) {
				logError(e.message, e.stack, "Deals - deals.utils.showErrorMessage", location.pathname);
			}
		},

		//Detects whether iphone or not and adds target blanc attribute to the call buttons present in the page
		callBtnIphoneDetection: function () {
			if (navigator.userAgent.match(/(iPod|iPhone|iPad)/))
				$("a.callBtn").attr('target', '_blank');
		},

		triggerStockTracking: function (stockId, cityId, act, platformId) {
			var lbl = 'stockid=' + stockId + '|cityid=' + cityId + '|pf=' + platformId + '|envId=' + Deals.environmentId + '|src=';
			var src = Common.utils.getValueFromQS('src');
			if (src != '')
				var lbl = lbl + src;
			else
				var lbl = lbl + 0;
			cwTracking.trackCustomData('advantage', act, lbl, false);
		},

		removeParamFromQS: function (name, url) {
			if (url.length > 0) {
				var prefix = name + '=';
				var pars = url.split(/[&;]/g);
				for (var i = pars.length; i-- > 0;) {
					if (pars[i].indexOf(prefix) > -1) {
						pars.splice(i, 1);
					}
				}
				url = pars.join('&');
				return url;
			}
			else
				return "";
		},

		updateQSParam: function (name, value) {
			var currentUrl = location.href.split('?')[1];

			if (currentUrl != "") {
				currentUrl = Deals.utils.removeParamFromQS(name, currentUrl);
				currentUrl += '&' + name + "=" + value;
			}
			else
				currentUrl = name + "=" + value;
			return currentUrl;
		},

		getFilterFromQS: function (name) {
			var url = window.location.href.toLowerCase();
			name = name.toLowerCase();
			if (url.indexOf('?') > 0 && url.indexOf(name) > 0) {
				var qs = url.split('?')[1];
				var params = qs.split('&');
				var result = {};
				var propval, filterName, value;
				var isFound = false;
				for (var i = 0; i < params.length; i++) {
					var propval = params[i].split('=');
					filterName = propval[0].toLowerCase();
					if (filterName == name) {
						value = propval[1];
						isFound = true;
						break;
					}
				};
				if (isFound && value.length > 0) {
					if (value.indexOf('+') > 0)
						return value.replace(/\+/g, " ");
					else
						return value;
				}
				else
					return "";
			}
			else
				return "";
		},

		scrollToElement: function (id) {
			$('html,body').animate({
				scrollTop: $("#" + id).offset().top - 65
			}, 'slow');
		},

		registerCityPopup: function () {
			$('#advantage-city-select').attr('data-advantage-url', location.href + "?cityId=");
			Common.advantage.popup.registerEvents();
			Common.advantage.popup.showCityPopup();
			Common.advantage.popup.getPopupCities();
		},

		validateOnlyMobile: function (element, errMsg) {
			var mobileNumField = element;
			var mobileNo = $.trim(mobileNumField.val());
			if (mobileNo == "") {
				Deals.utils.showCustomeErrorMessage(element, "Please enter your mobile number");
				if (Deals.env == 2) {
					ShakeFormView($("#bookButton .mobile-text-box"));
				}

				return false;
			}
			else if (!Deals.utils.validateInputField(mobileNumField, /^[6789]\d{9}$/)) {
				if (Deals.env == 2) {
					ShakeFormView($("#bookButton .mobile-text-box"));
				}
				return false;
			}
			return true;
		},

		cookie: {
			setCookie: function (name, value, expiry) {
				var domain = '';
				var date = new Date();
				date.setDate(date.getMonth() + expiry);
				var host = location.host;
				var arr = [];
				if (host.indexOf('.com') > 0) {
					arr = host.split('.');
					domain = '.' + arr[1] + '.' + arr[2];
				}
				if (domain.length > 0)
					document.cookie = name + "=" + value + ";expires=" + date + ";path=/;domain=" + defaultCookieDomain;
				else
					document.cookie = name + "=" + value + ";expires=" + date + ";path=/";
			},

			updateHistoryCookie: function (modelId) {
				var value = $.cookie('_advHistory');
				if (value) {
					var modelIdArray = value.split('~'), len = modelIdArray.length;
					if (len >= 20 && $.inArray(modelId.toString(), modelIdArray) === -1)
						modelIdArray = Deals.utils.cookie.removeModelIdFromCookie(modelIdArray, modelIdArray[len - 1]);
					if ($.inArray(modelId.toString(), modelIdArray) === -1)
						modelIdArray = Deals.utils.cookie.insertModelId(modelIdArray, modelId);
					else {
						modelIdArray = Deals.utils.cookie.removeModelIdFromCookie(modelIdArray, modelId.toString());
						modelIdArray = Deals.utils.cookie.insertModelId(modelIdArray, modelId);
					}
					Deals.utils.cookie.setCookie('_advHistory', modelIdArray.join('~'), 180);
				}
				else
					Deals.utils.cookie.setCookie('_advHistory', modelId, 180);
			},

			insertModelId: function (modelIdArr, modelId) {
				return [modelId.toString()].concat(modelIdArr);
			},

			removeModelIdFromCookie: function (modelIdArr, modelId) {
				modelIdArr.splice(modelIdArr.indexOf(modelId) == -1 ? modelIdArr.length : modelIdArr.indexOf(modelId), 1);
				return modelIdArr;
			}
		}
	},

	dealInquiries: {
		eagernessEnum: { "Hot": 1, "Warm": 2, "Normal": 3 },
		registerEvents: function () {
			$(window).scroll(function () {
				Deals.product.fixButton();
			});
			Deals.doc.on('click touchend', '#divOffer', function () {
				Deals.dealInquiries.showOffer();
			});
			Deals.doc.on('click', '#NextBtn', function () {
				Deals.dealInquiries.showPersonalDetails();
				Deals.dealInquiries.hideCarDetails();
				if (Deals.env == 1)
					Common.utils.trackAction('Advantage', 'deals_mobile', ' detailsnextclick_mobile');
				else
					Common.utils.trackAction('Advantage', 'deals_desktop', ' detailsnextclick_desktop');
			});
			Deals.doc.on('click', '#carDetailsTab', function () {
				if ($("#carDetails").hasClass('hide')) {
					Deals.dealInquiries.showCarDetails();
					Deals.dealInquiries.hidePersonalDetails();
					if (Deals.env == 1)
						Common.utils.trackAction('Advantage', 'deals_mobile', ' cardetailhead_mobile');
					else
						Common.utils.trackAction('Advantage', 'deals_desktop', ' cardetailhead_desktop');
				}
			});
		},

		getLeadData: function (stockId, custMobile, dealerId, campaignId) {
			return {
				"CustomerName": $('#personName').val(),
				"CustomerMobile": custMobile,
				"CustomerEmail": $('#personEmail').val(),
				"IsPaymentSuccess": false,
				"CityId": Deals.product.getCityId(),
				"DealsStockId": stockId,
				"BranchId": dealerId,
				"InquirySourceId": 171,
				"Eagerness": Deals.dealInquiries.getEagerness(),
				"IsAutoVerified": false,
				"ApplicationId": 1,
				"CampaignId": campaignId != null || campaignId != undefined ? campaignId : 0
			}
		},

		pushUnpaidLead: function (data, async) {
			if (data && data.DealsStockId) {
				$.ajax({
					type: 'POST',
					data: data,
					url: Deals.apiHostUrl + "webapi/NewCarInquiries/Post/",
					//dataType: 'Json',
					success: function (response) {
						return true;
					},
					error: function (msg) { }
				});
			}
		},

		getEagerness: function () {
			var eagernessValue = $('.drpPurchaseTimeframe:visible').val();
			if (eagernessValue == 1 || eagernessValue == 2)
				return Deals.dealInquiries.eagernessEnum.Hot;
			else if (eagernessValue == 3)
				return Deals.dealInquiries.eagernessEnum.Warm;
			else
				return Deals.dealInquiries.eagernessEnum.Normal;
		},

		pushMultipleLeads: function (stockIds) {
			var stocksArray = stockIds.split(',');
			$.each(stocksArray, function (key, value) {
				Deals.dealInquiries.pushUnpaidLead(Deals.dealInquiries.getLeadData(value, $.cookie('_CustMobile'), Deals.product.getDealerId(value), Deals.product.getCampaignId(value)), false);
			});
		},

		pushMutipleVariantLead: function (stockIds, node) {
			var stocksArray = stockIds.split(',');
			$.each(stocksArray, function (key, value) {
				var dealerId = node.find("[data-stockid='" + value + "']").attr('data-dealerId');
				var campaignId = node.find("[data-stockid='" + value + "']").attr('data-campaignId');
				Deals.dealInquiries.pushUnpaidLead(Deals.dealInquiries.getLeadData(value, $.cookie('_CustMobile'), dealerId, campaignId), false);
			});
		},

		showOffer: function (e) {
			$(document).on('click touchend', 'div.tool-tip-info', function (e) {
				e.stopPropagation();
				$(this).children('.tooltip').toggleClass('tooltip-details');
			});

			$(document).on('click', 'body', function () {
				if ($('div.tooltip').is(':visible') && !$(e.target).is('.div.tooltip')) {
					$('div.tooltip').removeClass('tooltip-details').css('display', 'none');
				}

			});

		},
		showCarDetails: function () {
			try {
				$("#carDetailsTab").addClass('active-tab');
				$('html, body').animate({ scrollTop: 0 }, 300);
				$("#carDetailsTab").parent().addClass('ticked');
				$("#carDetails").removeClass('hide');
			}
			catch (e) {
				logError(e.message, e.stack, "Deals - deals.dealcalls.showCarDetails", location.pathname);
			}
		},

		hideCarDetails: function () {
			try {
				$("#carDetailsTab").parent().addClass('ticked');
				$("#carDetails").removeClass('show');
				$("#carDetails").addClass('hide');
				$('.first .details-icon-wrap').addClass('first-tick');
			}
			catch (e) {
				logError(e.message, e.stack, "Deals - deals.dealcalls.hideCarDetails", location.pathname);
			}
		},

		showPersonalDetails: function () {
			try {
				$("#personalDetailsTab").removeClass('disabled-tab');
				$("#personalDetailsTab").addClass('active-tab');
				$('html, body').animate({ scrollTop: 0 }, 300);
				$("#personalDetailsTab").parent().addClass('ticked');
				$("#personalDetails").removeClass('hide');
			}
			catch (e) {
				logError(e.message, e.stack, "Deals - deals.dealcalls.showPersonalDetails", location.pathname);
			}

		},

		hidePersonalDetails: function () {
			try {
				$("#personalDetailsTab").parent().removeClass('ticked');
				$("#carDetailsTab").parent().removeClass('ticked');
				$("#personalDetailsTab").removeClass('active-tab');
				$("#personalDetailsTab").addClass('disabled-tab');
				$("#personalDetails").removeClass('show');
				$("#personalDetails").addClass('hide');
			}
			catch (e) {
				logError(e.message, e.stack, "Deals - deals.dealcalls.hidePersonalDetails", location.pathname);
			}
		},

		validatePersonalDetails: function (personMob) {
			try {
				isValid = true;
				var nameField = $("#personName");
				var name = $.trim(nameField.val());
				if (name == "") {
					if (Deals.env == 2) {
						ShakeFormView($(".customerNameBox"));
					}
					Deals.utils.showCustomeErrorMessage(nameField, "Please enter name.")
					isValid = false;
				}
				else if (!Deals.utils.validateInputField(nameField, /^([-a-zA-Z ']*)$/)) {
					if (Deals.env == 2) {
						ShakeFormView($(".customerNameBox"));
					}
					Deals.utils.showCustomeErrorMessage(nameField, "Invalid name.")
					isValid = false;
				}

				var mobileNumField = personMob || $("#personMob");
				var mobileNo = $.trim(mobileNumField.val());
				if (mobileNo == "") {
					if (Deals.env == 2) {
						ShakeFormView($(".customerMobileBox"));
					}
					Deals.utils.showCustomeErrorMessage(mobileNumField, "Please enter mobile number.");
					isValid = false;
				}
				else if (!Deals.utils.validateInputField(mobileNumField, /^[6789]\d{9}$/)) {
					if (Deals.env == 2) {
						ShakeFormView($(".customerMobileBox"));
					}
					Deals.utils.showCustomeErrorMessage(mobileNumField, "Invalid mobile number.");
					isValid = false;
				}

				var emailField = $("#personEmail");
				var emailId = $.trim(emailField.val());
				if (emailId == "") {
					if (Deals.env == 2) {
						ShakeFormView($(".customerEmailBox"));
					}
					Deals.utils.showCustomeErrorMessage(emailField, "Please enter email.")
					isValid = false;
				}
				else if (!Deals.utils.validateInputField(emailField, /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/)) {
					if (Deals.env == 2) {
						ShakeFormView($(".customerEmailBox"));
					}
					Deals.utils.showCustomeErrorMessage(emailField, "Invalid email.")
					isValid = false;
				}

				return isValid;
			}

			catch (e) {
				logError(e.message, e.stack, "Deals - deals.dealcalls.validatePersonalDetails", location.pathname);
			}
		},

		prefillUserDetails: function () {
			if ($.cookie('_CustomerName') != null && $.cookie('_CustomerName').length > 0)
				$('#personName').val($.cookie('_CustomerName'));
			else
				$('#personName').val("unknown");
			if ($.cookie('_CustEmail') != null && $.cookie('_CustEmail').length > 0)
				$('#personEmail').val($.cookie('_CustEmail'));
			else
				$('#personEmail').val("unknown@unknown.com");
			if ($.cookie('_CustMobile') != null) {
				$('#personMob').val($.cookie('_CustMobile'));
				$('#custMobile').val($.cookie('_CustMobile'));
				$('#mobileInput').val($.cookie('_CustMobile'));
				$('#variantsMobField').val($.cookie('_CustMobile'));
			}
		},

		hideMobilenumber: function () {
			if ($.cookie('_CustMobile') != null) {
				$('.mobile-text-box').css("display", "none")
			}
		},

		submitInquiry: function (stockId, cityId) {
			var inquiryId = 0;
			var eagerness = $('.drpPurchaseTimeframe:visible').val();
			if (eagerness == undefined || eagerness == null)
				eagerness = 0;
			$.ajax({
				type: 'POST',
				headers: { "sourceid": Deals.env == 1 ? "43" : "1" },
				data: JSON.stringify({
					"RecordId": "",
					"CustomerName": $('#personName').val(),
					"CustomerEmail": $('#personEmail').val(),
					"CustomerMobile": $.cookie('_CustMobile'),
					"CustomerCity": cityId,
					"StockId": stockId,
					"ResponseId": "",
					"CityId": cityId,
					"Source": Common.utils.getValueFromQS('src'),
					"MasterCityId": $.cookie('_CustCityIdMaster'),
					"Eagerness": eagerness,
					"ABTestValue": $.cookie('_abtest'),
					"CampaignId": Deals.product.getCampaignId(stockId),
					"DealerId": Deals.product.getDealerId(stockId)
				}),
				url: "/api/advantage/inquiry/",
				dataType: 'Json',
				contentType: 'text/json',
				success: function (response) {
					if (response.inquiryId > 0) {
						if (Deals.inquirySource == 1) {
							$('#nextBtnProcessing').hide();
						}
						else if (Deals.inquirySource == 2) {
							if (Deals.env == 2) {
								Deals.product.slideEnquirePopup();
							}
							if (Deals.env == 1) {
								$('.enquiry-first-screen').hide();
								$('.enquiry-second-screen').show();
							}
						}
						else if (Deals.inquirySource == 3) {
							if (Deals.env == 1) {
								$("#detailPopup , #nextBtnProcessing").hide();
								$("#blockCar").show();
							}
							else {
								$('#galleryBtnProcessing').hide();
								$('.gallery-enq-section').hide();
								$(".left-enquiry-form-2").show();
							}
						}
						Deals.utils.cookie.setCookie('_advLead', true, 180);
					}
				}
			});
			Common.utils.trackAction("CWInteractive", "Advantage_general", "Response_Submitted", Deals.env == 1 ? "Mobile" : "Desktop");
		},

		submitMultipleLead: function (stockId, cityId) {
			var inquiryId = 0;
			$.ajax({
				type: 'POST',
				headers: { "sourceid": Deals.env == 1 ? "43" : "1" },
				data: JSON.stringify({
					"RecordId": "",
					"CustomerName": $('#personName').val(),
					"CustomerEmail": $('#personEmail').val(),
					"CustomerMobile": $.cookie('_CustMobile'),
					"CustomerCity": cityId,
					"MultipleStockId": stockId,
					"ResponseId": "",
					"CityId": cityId,
					"Source": Common.utils.getValueFromQS('src'),
					"MasterCityId": $.cookie('_CustCityIdMaster'),
					"Eagerness": $('.drpPurchaseTimeframe:visible').val(),
					"ABTestValue": $.cookie('_abtest')
				}),
				url: "/api/advantage/inquiries/",
				dataType: 'Json',
				contentType: 'text/json',
				success: function (response) {
					if (Deals.inquirySource == 3) {
						if (Deals.env == 1) {
							Deals.dealInquiries.m_closeInquiryPopup();
							Deals.recommendations.m_hideToggleElement();
							$('.enquiry-poup-container').hide();
						}
						else {
							Deals.product.slideEnquirePopup();
						}
					}
					if (Deals.inquirySource == 4) {
						if (Deals.env == 1) {
							$('.assitance-form-fillup').hide();
							$('#assitance-thnk-msg').show();
							$('#close-assistance').show();
							$('.user-expert').hide();
							Deals.doc.callBackArrow.addClass('callback-width-change');

						}
						else {
							$(".req-call-thnk").show();
							$(".req-call-btn-section").hide();

						}
					}
					if (Deals.inquirySource == 5) {
						if (Deals.env == 1) {
							$('.enquiry-first-screen').hide();
							$('.enquiry-second-screen').show();
						}
						else
							Deals.product.slideEnquirePopup();
					}
				}
			});
			Deals.utils.cookie.setCookie('_advLead', true, 180);
			var stockCount = stockId.split(',').length;
			for (var i = 0; i < stockCount; i++)
				Common.utils.trackAction("CWInteractive", "Advantage_general", "Response_Submitted", Deals.env == 1 ? "Mobile" : "Desktop");
		},

		m_closeInquiryPopup: function () {
			$('.contact-det-popup').hide();
			$('.blackOut-window').hide();
			$("#blockCar,.thank-you-screen").hide();
			Common.utils.unlockPopup();
			if ($('.list-rcmd-cars').length > 0) {
				var node = $('#selectRecommendation p');
				node.removeClass('remove-all').addClass('select-all');
				node.find('.text-bold').text("Select All");
				$('#advantage-detail-popup').removeClass('rcmd-car-popup');
				$('#detailPopup').show().css('left', 0);
				$('#recommendedCarsDetail').hide().css('left', '100%');
			}
		},

		m_showRecommendationPopup: function () {
			$('#recommendedCarsDetail').show().css('left', '100%');
		},

		d_closeInquiryPopup: function () {
			$('#advantage-detail-popup').removeClass('show').addClass('hide');
			$("#blockCar,.thank-you-screen").hide();
			Common.utils.unlockPopup();
			if ($('.rcmd-car-popup').length > 0) {
				var node = $('#selectRecommendation p');
				node.removeClass('remove-all').addClass('select-all');
				node.find('.text-bold').text("Select All");
				$('#advantage-detail-popup').removeClass('rcmd-car-popup');
				$('#detailPopup').show().css('left', 0);
				$('#recommendedCarsDetail').hide().css('left', 700);
			}
		},

		m_openInquiryPopup: function () {
			if (Deals.inquirySource == 3) {
				var reasonContainer = $('#reasons .reason');
				for (i = 0; i < 3; i++) {
					$('span.reasonHeading').eq(i).text(reasonContainer.eq(i).find('span').eq(0).text());
					$('span.reasonDescription').eq(i).text(reasonContainer.eq(i).find('span').eq(1).text());
				}
			}
			$("div.blackOut-window").show();
			$('#whatWeGet,#proceedBtn').show();
			$('#contact-det-popup').slideDown().css({ 'overflow-y': 'auto', 'top': '' });
			$('.contact-det-popup-content,.form-helper-content,.form-heading').show();
		}

	},

	dealCalls: {
		getDeals: function (qs) {

			var response = {}; response.dealsCount = 0; response.dealsData = [];
			var whenFail = function () { Deals.isCallCompleted = true; response = null; console.log("api failure"); }

			$.ajax({
				type: 'GET',
				async: false,
				url: Deals.apiHostUrl + '/api/deals/?' + qs + ($.trim(qs) == "" ? 'pn=' : '&pn=') + Deals.paginationCounter,
				dataType: 'Json',
				beforeSend: function () { Deals.isCallCompleted = false; },
				success: function (deals) {
					Deals.isCallCompleted = true;
					if (deals == null) { deals = {}; deals.dealsCount = 0; deals.dealsData = []; }
					if (deals.dealsData.length < 1) Deals.paginationStop = true;
					else Deals.paginationStop = false;
					response = deals;
				},
				error: whenFail,
				failure: whenFail
			});

			return response;
		},

		getCars: function () {
			var cars = [];
			$.ajax({
				type: 'GET',
				async: false,
				url: Deals.apiHostUrl + '/api/deals/cars/',
				dataType: 'Json',
				success: function (response) {
					cars = response;
				}
			});
			return cars;
		},
		getSpecs: function (versionId) {
			var specsData = { specs: [], features: [], overview: [] }

			if (versionId > 0) {
				$.ajax({
					type: 'GET',
					async: false,
					url: '/api/versions/specs/?versiondId=' + versionId,
					dataType: 'Json',
					success: function (response) {
						specsData = response;
					}
				});
			}
			return specsData;
		},

		getDealMakes: function () {
			var Data = [];
			$.ajax({
				type: 'GET',
				async: false,
				url: "/api/deals/makemodel/?cityId=" + Deals.cityId,
				dataType: 'Json',
				success: function (response) {
					Data = response;
				}
			});
			return Data;
		},

		getDealModels: function (makeid) {
			var models = [];
			$.ajax({
				type: 'GET',
				async: false,
				url: '/api/deals/makemodel/?cityId=' + Deals.cityId + '&makeid=' + makeid,
				dataType: 'Json',
				success: function (response) {
					models = response;
				}
			});
			return models;
		},
	},

	offerOfWeek: {
		offers: '',
		setOfferAttributes: function () {
			try {
				var isShowExtraOffer = false;
				if (Deals.offerOfWeek.offers) {
					var offers = Deals.offerOfWeek.offers.split('|');
					var offersList = [];
					offers.forEach(function (offer) {
						var offerDetails = offer.split('-');
						if (offerDetails[0] == Deals.modelId && offerDetails[1] == Deals.cityId) {
							var date = new Date(offerDetails[2]);
							if (date > new Date()) {
								date.setDate(date.getDate() - 1);
								$('span[data-offer-savings="false"]').addClass('hide');
								isShowExtraOffer = true;
								var locale = "en-us";
								var dateFormat = date.toLocaleString(locale, { month: "long" }) + ' ' + date.getDate();
								$('span[data-extra-offer-date]').text(dateFormat);
								return;
							}
						}
					});
				}
				if (!isShowExtraOffer) {
					$('span[data-offer-savings="true"]').addClass('hide');
				}
			}
			catch (ex) {
				$('span[data-offer-savings="true"]').addClass('hide');
			}
		},

		refreshInterval: function () { },

		updateOfferTime: function () {
			try {
				clearInterval(Deals.offerOfWeek.refreshInterval);
				var date;
				if (Deals.offerOfWeek.offers) {
					var offers = Deals.offerOfWeek.offers.split('|');
					var currentTime = new Date();
					offers.forEach(function (offer) {
						var offerDetails = offer.split('-');
						if (offerDetails[1] == $('#advDrpCity').val()) {
							date = new Date(offerDetails[2]);
						}
					});
				}

				if (date != undefined) {
					Deals.offerOfWeek.refreshInterval = setInterval(function () {
						var currentTime = new Date();
						if (date > currentTime) {
							var timeLeft = Deals.offerOfWeek.msToTime(date - currentTime);
							var node = $('#offerTime [data-days]');
							node.text(timeLeft.days);
							node.next().text("day" + (timeLeft.days == 1 ? '' : 's') + ' : ');
							node = $('#offerTime [data-hours]');
							node.text(timeLeft.hours);
							node.next().text("hour" + (timeLeft.hours == 1 ? '' : 's') + ' : ');
							node = $('#offerTime [data-mins]');
							node.text(timeLeft.minutes);
							node.next().text("min" + (timeLeft.minutes == 1 ? '' : 's') + ' : ');
							node = $('#offerTime [data-secs]');
							node.text(timeLeft.seconds);
							node.next().text("sec" + (timeLeft.seconds == 1 ? '' : 's'));
							return;
						}
					}, 1000)
				}
			}
			catch (ex) {
				$('#offerOfWeekBanner').addClass('hide');
				$('#AdvtIntroWrap').removeClass('hide');
			}
		},
		msToTime: function (duration) {
			var seconds = parseInt((duration / 1000) % 60)
				, minutes = parseInt((duration / (1000 * 60)) % 60)
				, hours = parseInt((duration / (1000 * 60 * 60)) % 24)
				, days = parseInt(duration / (1000 * 60 * 60 * 24));

			hours = (hours < 10) ? "0" + hours : hours;
			minutes = (minutes < 10) ? "0" + minutes : minutes;
			seconds = (seconds < 10) ? "0" + seconds : seconds;

			return { days: days, hours: hours, minutes: minutes, seconds: seconds };
		}
	},

	product: {
		modelImageCallCounter: 0,
		registerEvents: function () {

			$(window).bind('popstate', function (event) {
				if ($('#detailPopup').is(':visible')) {
					Deals.dealInquiries.m_closeInquiryPopup();
				}
				else if ($('.enquiry-poup-container').is(':visible')) {
					$('.enquiry-popup-close-btn').trigger('click');
				}
				else if ($('.price-break-up').is(':visible')) {
					$('.price-breakup-close-btn').trigger('click');
				}
			});

			$(window).scroll(function () {
				Deals.product.fixButton();
			});

			Deals.doc.on('click', '#submit-request', function () {
				var element = $('#personMobEnquiryPopup');
				if (Deals.dealInquiries.validatePersonalDetails(element)) {
					var stockId = Deals.product.getStockId();
					var cityId = Deals.product.getCityId();
					Common.utils.setEachCookie("_CustMobile", $('#personMobEnquiryPopup').val());
					if ($('#chkColorFlexible').is(':checked')) {
						Common.utils.trackAction("CWInteractive", Deals.env == 2 ? "deals_desktop" : "deals_mobile", "popup2_CTA_clicked", "validation_true_multiplecolors");
						var stockIds = Deals.product.getUniqueDealersStockId();

						if (stockIds != "") {
							Deals.inquirySource = 5;
							Deals.dealInquiries.pushMultipleLeads(stockIds);
							Deals.dealInquiries.submitMultipleLead(stockIds, cityId);
						}
					}
					else {

						Common.utils.trackAction("CWInteractive", Deals.env == 2 ? "deals_desktop" : "deals_mobile", "popup2_CTA_clicked", "validation_true_singlecolor");
					}
					Deals.inquirySource = 2;
					Deals.dealInquiries.pushUnpaidLead(Deals.dealInquiries.getLeadData(stockId, $.cookie('_CustMobile'), Deals.product.getDealerId(stockId), Deals.product.getCampaignId(stockId)), true);
					Deals.dealInquiries.submitInquiry(stockId, cityId);
					Deals.recommendations.dealsRecommendationBinding();

				}
				else
					Common.utils.trackAction("CWInteractive", "deals_desktop", "popup2_CTA_clicked", "validation_false");
			});

			Deals.doc.on('touchstart', '.tool-tip-info .year-info-icon', function (e) {
				e.stopPropagation();
				$(this).next().toggleClass('tooltip-details');
			});
			Deals.doc.on('touchstart', 'body', function (evt) {
				$('div.tooltip').removeClass('tooltip-details');
			});

			Deals.doc.on('click', '.make-model', function () {
				Deals.makeModel.registerEvents();
			});

			Deals.doc.on('click', 'span.break-up-link', function () {
				Deals.product.bindPriceBreakup();
				var node = $(this);
				btObj.invokeToolTipData.clickElement = node;
				btObj.invokeToolTipData.contentElement = $("#breakUpContent");
				btObj.invokeToolTipData.width = 400;
				btObj.invokeToolTipData.position = ['right'];
				btObj.invokeToolTip();
			});
			Deals.doc.on('mouseover', '#emi-info-tooltip', function () {
				var node = $(this);
				btObj.invokeToolTipData.clickElement = node;
				btObj.invokeToolTipData.contentElement = node.siblings('.emi-car-content');
				btObj.invokeToolTipData.width = 300;
				btObj.invokeToolTipData.position = ['right'];
				Common.utils.trackAction("CWInteractive", "Advantage_general", "emitooltipshown", "Desktop");
				btObj.invokeToolTip();
			});
			Deals.doc.on('click', '#m-emi-info-tooltip', function () {
				var node = $(this);
				btObj.invokeToolTipData.clickElement = node.find('.info-icon');
				btObj.invokeToolTipData.contentElement = node.siblings('.emi-car-content');
				btObj.invokeToolTipData.width = 210;
				Common.utils.trackAction("CWInteractive", "Advantage_general", "emitooltipshown", "Mobile");
				btObj.invokeToolTip();
			});
			Deals.doc.on('change', 'div.year-type select', function (e) {
				if (Deals.isCallCompleted) {
					var node = $(this), stockId = node.val();
					node.attr('currentYear', node.find('option[value="' + stockId + '"]').text());
					//node.parent().find('li.active').removeClass('active');
					//node.addClass('active');
					Deals.product.hideAllDetails();
					node.parent().removeClass('hideImportant');
					Deals.product.showDetailsByStock(stockId);
					Deals.utils.triggerStockTracking(Deals.product.getStockId(), Deals.product.getCityId(), 'yearChange', Deals.env);
					Deals.product.counterAmount($('[data-adv-price]:visible'));
				}

				if (location.pathname.indexOf("/m/") != -1)
					$('#divPhotoGallery a').attr('href', window.location.href.split('?')[0] + "gallery/" + Deals.product.getStockId() + "/?src=" + Common.utils.getValueFromQS('src') + "&modelId=" + Deals.modelId + "&cityId=" + Common.utils.getValueFromQS('cityid'));
			});

			Deals.doc.on('click', '#mobileInput', function () {
				$('.mobile-text-box .err-mobile-icon , .mobile-text-box .cw-blackbg-tooltip, .error-icon').addClass('hide');
			});

			Deals.doc.on('click', '#interest-btn', function () {
				Deals.product.bindReasonsSlugPopup();
				if ($.cookie('_CustMobile') != null && ($.cookie('_advLead') == 'true')) {
					Common.utils.trackAction('CWInteractive', 'deals_desktop', 'CTA1_clicked', 'autosubmit');
					$('#nextBtnProcessing').show();
					var stockId = Deals.product.getStockId();
					Deals.dealInquiries.pushUnpaidLead(Deals.dealInquiries.getLeadData(stockId, $.cookie('_CustMobile'), Deals.product.getDealerId(stockId), Deals.product.getCampaignId(stockId)), true);
					Deals.dealInquiries.submitInquiry(Deals.product.getStockId(), Deals.product.getCityId());
				}
				else {
					Common.utils.trackAction('CWInteractive', 'deals_desktop', 'CTA1_clicked', 'noautosubmit');
					$('#divSavings,#proceedBtn').hide();
					$('#blockCar,#btnAssistance,.contact-det-popup-content,.form-helper-content,.form-heading').show();
					Deals.product.setPopupOffer();
					$("#whatWeGet").hide();
					$('#btnAssistance').text('PROCEED');
					$("#btnAssistance").attr("data-inquiry-source", 1);
					$('#advantage-detail-popup').removeClass('hide').addClass('show');
					Common.utils.lockPopup();
				}
				Deals.utils.triggerStockTracking(Deals.product.getStockId(), Deals.product.getCityId(), 'interest-btn', Deals.env);
			});

			Deals.doc.on('click', '.emi-link , #btnMBookNow', function () {
				if ($.cookie('_CustMobile') != null && ($.cookie('_advLead') == 'true')) {
					$("#nextBtnProcessing").hide();
					$('#personMobEnquiryPopup').val($.cookie('_CustMobile'));
					if (Deals.env == 2) {
						Deals.product.openEnquiryPopup();
					}
					if (Deals.env == 1) {
						$('.enquiry-poup-container').show();
						$('.enquiry-first-screen').show();
						$('.enquiry-second-screen').hide();
					}
				} else {
					if (Deals.env == 2) {
						Deals.product.openEnquiryPopup();
						$('#enquire-detail-popup').removeClass('hide').addClass('show');
					}
					if (Deals.env == 1) {
						$('.enquiry-poup-container').show();
					}
				}
				Common.utils.lockPopup();
			});

			Deals.doc.on('click', '#enq-request', function () {
				$('.enquiry-first-screen').hide();
				$('.enquiry-second-screen').show();
			});

			Deals.doc.on('click', '.linkNotSure', function () {
				$('.enquiry-poup-container').show();
				window.history.pushState("get-emi", "", "");
				Common.utils.trackAction('CWInteractive', Deals.env == 2 ? 'deals_desktop' : 'deals_mobile', 'CTA2_clicked', 'noautosubmit');
			});

			Deals.doc.on('click', '#req-assitance-viewBreakup', function () {
				$('.enquiry-poup-container').show();
				window.history.pushState("priceBreakup-req-asst", "", "");
				Common.utils.trackAction('CWInteractive', Deals.env == 2 ? 'deals_desktop' : 'deals_mobile', ' ViewBreakup_CTA_clicked', '-');
			});

			Deals.doc.on('click', '.blackOut-window, .advantage-close-btn, .recommendation-advantage-close-btn, #globalPopupBlackOut', function () {
				Deals.dealInquiries.d_closeInquiryPopup();
			});

			Deals.doc.on('keyup', function (e) {
				if (e.keyCode == 27) $('.blackOut-window').click();   // esc
			});

			Deals.doc.on('click touchend', 'div.active-color-shade', function () {
				var node = $(this);
				var index = node.index();
				var colorTick = $('span.tick-color');
				colorTick.addClass('hideImportant');
				var dealerArea = "";
				var areaValue = node.find("div.dealer-location").attr('area');
				if (areaValue != undefined && areaValue != null) {
				    dealerArea = areaValue.replace(/\,$/, '');
				}
				$('#color-text').text(node.children().attr('title'));
				if (dealerArea != "") {
				    $('#dealerArea').text(" " + dealerArea + ",");
				}
				else {
				    $('#dealerArea').text(dealerArea);
				}
				$('#dealerCity').text(" " + node.find("div.dealer-location").attr('city'));
				$('#popupDealerLocation').html($('#dealerLocation').text());
				$('#popupColorName').text(node.children().attr('title'));
				if (colorTick.length > 1) {
					node.find('span.tick-color').removeClass('hideImportant');
				}
				var imgNode = $('.car-pic');
				var specsNode = $('div.keySpecsDiv');
				imgNode.hide();
				specsNode.hide();
				imgNode.eq(index).show();
				specsNode.eq(index).show();
				Deals.product.hideAllDetails();
				Deals.product.showDetailsByIndex(index);
				Deals.utils.triggerStockTracking(Deals.product.getStockId(), Deals.product.getCityId(), 'colorChange', Deals.env);

				if (colorTick.length > 1) {
					Deals.product.counterAmount($('[data-adv-price]:visible'));
				}

				if (location.pathname.indexOf("/m/") != -1)
					$('#divPhotoGallery a').attr('href', window.location.href.split('?')[0] + "gallery/" + Deals.product.getStockId() + "/?src=" + Common.utils.getValueFromQS('src') + "&modelId=" + Deals.modelId + "&cityId=" + Common.utils.getValueFromQS('cityid'));

			});

			Deals.doc.on('change', '#advDrpCity', function () {
				var url = window.location.href.split('?');
				var makeName = $('#spnCarName').text();
				window.location.href = (url[0] + "?cityId=" + $(this).val());
			});

			Deals.doc.on('change', '#drpVersions', function () {
				if (Deals.isCallCompleted) {
					var node = $(this);
					var verId = node.val();
					Deals.isCallCompleted = false;
					Deals.product.replaceState(Deals.utils.updateQSParam('versionId', verId));
					var json = Deals.product.getDetails(verId, 'productload', node);
					Deals.offerOfWeek.setOfferAttributes();
					//var json = Deals.product.getDetails(modelId, verId, cityId, 'productload', node);
				}


				setTimeout(function () {
					var tickNode = $('span.tick-color'), imgNode = $('.car-pic'), specsNode = $('div.keySpecsDiv');;
					imgNode.hide();
					specsNode.hide();
					imgNode.eq(0).show();
					specsNode.eq(0).show();
					tickNode.addClass('hideImportant');
					if (tickNode.length > 1) {
						tickNode.eq(0).removeClass('hideImportant');
					}
					$('#color-text').text(tickNode.eq(0).parent().attr('title'));
					tickNode.eq(0)
					Deals.isCallCompleted = true;
				}, 200);

			});

			Deals.doc.on('hover', '.tool-tip-info', function (e) {
				$('.tooltip').addClass('tooltip-details');
			});

			Deals.doc.on('mouseleave', '.tool-tip-info', function (e) {
				$('.tooltip').removeClass('tooltip-details');
			});

			Deals.doc.on('click', '.changeCarPopup', function () {
				$('.blackOut-window').first().show();
				Deals.makeModel.registerEvents();
				$('.make-model-popup').removeClass('hide');
			})

			Deals.doc.on('click', '.blackOut-window, .make-model-close-btn', function () {
				$('.make-model-popup').addClass('hide');
				$('.blackOut-window').first().hide();
			});

			Deals.doc.on('click', '.blackOut-window, .enquiry-popup-close-btn', function () {
				if (Deals.env == 2) {
					$('#enquire-detail-popup').removeClass('show').addClass('hide');
					Deals.product.stopSlideEnquirePopup();
				}
				if (Deals.env == 1) {
					$('.enquiry-poup-container').hide();
					Common.utils.unlockPopup();
				}
				$('.blackOut-window').first().hide();
			});


			Deals.doc.on('click', '.contact-det-close-btn', function () {
				Deals.dealInquiries.m_closeInquiryPopup();
				if (Deals.env == 1) {
					$('.VINSelectError').addClass("hideImportant");
					Deals.recommendations.m_hideToggleElement();
				}

			});

			Deals.doc.on('click', '#btnAssistance', function () {
				var retVal = Deals.dealInquiries.validatePersonalDetails();
				var action = '';
				var element = $(this).attr('data-inquiry-source');
				if (retVal) {
					action = Deals.product.getTrackingAction(element);
					if (action)
						Common.utils.trackAction('CWInteractive', 'deals_desktop', action, 'validation_true');
					$('#nextBtnProcessing').show();
					var stockId = Deals.product.getStockId();
					Deals.utils.triggerStockTracking(stockId, Deals.product.getCityId(), 'emiAssistanceButton', Deals.env);
					if (element)
						Deals.inquirySource = element;
					else
						Deals.inquirySource = 2;
					Common.utils.setEachCookie("_CustMobile", $('#personMob').val());
					Deals.dealInquiries.pushUnpaidLead(Deals.dealInquiries.getLeadData(stockId, $.cookie('_CustMobile'), Deals.product.getDealerId(stockId), Deals.product.getCampaignId(stockId)), true);
					Deals.dealInquiries.submitInquiry(Deals.product.getStockId(), Deals.product.getCityId());
					if (Deals.env == 1) {
						$('#advPopupCarName').text($('.make-model-name').text());
						Deals.recommendations.dealsRecommendationBinding();
					}
				}
				else {
					action = Deals.product.getTrackingAction(element);
					Common.utils.trackAction('CWInteractive', 'deals_desktop', action, 'validation_false');
				}
				return retVal;
			});


			Deals.doc.on('click', '#proceedBtn', function () {
				var retVal = Deals.dealInquiries.validatePersonalDetails();
				if (retVal) {
					$('#nextBtnProcessing').show();
					var stockId = Deals.product.getStockId();
					Deals.utils.triggerStockTracking(stockId, Deals.product.getCityId(), 'proceedButton', Deals.env);
					Deals.inquirySource = 1;
					Common.utils.setEachCookie("_CustMobile", $('#personMob').val());
					Deals.dealInquiries.pushUnpaidLead(Deals.dealInquiries.getLeadData(stockId, $.cookie('_CustMobile'), Deals.product.getDealerId(stockId), Deals.product.getCampaignId(stockId)), true);
					Deals.dealInquiries.submitInquiry(Deals.product.getStockId(), Deals.product.getCityId());
					Common.utils.trackAction('CWInteractive', 'deals_mobile', 'popup1_CTA_clicked', 'validation_true');
				}
				else
					Common.utils.trackAction('CWInteractive', 'deals_mobile', 'popup1_CTA_clicked', 'validation_false');
				return retVal;
			});


			Deals.doc.on('click', '[data-gallery]', function () {
				$('#pgPrice').text(Common.utils.formatNumeric($('.animated-amount:visible').data('adv-price')));
				$('#lblOnRoadPrice').text($('#advDrpCity option:selected').text());
				openImagePopup(Deals.modelId);
			});

			Deals.doc.on('click', '#btnSubmit', function () {
				var isValid = Deals.utils.validateOnlyMobile($('#custMobile'));

				if (isValid) {
					$('#galleryBtnProcessing').show();
					var mobile = $('#custMobile').val();
					$('#personMob').val(mobile);
					Deals.inquirySource = 3;
					Common.utils.setEachCookie("_CustMobile", $('#personMob').val());
					var stockId = Deals.product.getStockId();
					Deals.dealInquiries.pushUnpaidLead(Deals.dealInquiries.getLeadData(stockId, $.cookie('_CustMobile'), Deals.product.getDealerId(stockId), Deals.product.getCampaignId(stockId)), true);
					Deals.dealInquiries.submitInquiry(Deals.product.getStockId(), Deals.product.getCityId());
				}
				else if (Deals.env == 2)
					ShakeFormView($(".photo-gallery-textbox"));
			});

			Deals.doc.on('click', "#btnNoThanks,#mBtnNoThanks", function () {
				Deals.recommendations.dealsRecommendationBinding();
			});

			Deals.doc.on('click', 'ul.list-rcmd-cars li .car-model-name', function (e) {
				e.stopPropagation();
			});

			Deals.doc.on('click', 'ul.list-rcmd-cars li', function () {
				var self = $(this);
				self.toggleClass('activeBox');
				self.find('.cust-checkbox').toggleClass('fa-check-square');
				self.find('.cust-checkbox').attr('data-check', self.find('.cust-checkbox').attr('data-check') == "unchecked" ? "checked" : "unchecked");
				if ($('.recommendedCarsDetail .activeBox').length > 0)
					$('#multiLeadPush').prev().hide();

			});

			Deals.doc.on('change', '#fuelType, #transType', function () {
				Deals.product.filterVersions();
				Deals.variantList.adjustHeight();
			});

			Deals.doc.on('click', '#selectRecommendation', function () {
				var toggleTitle = $('span.toggle-selection-txt');
				var cardLI = $('ul.list-rcmd-cars li');
				var cardLiCheckbox = cardLI.find('.cust-checkbox');
				$('p.toggle-selection-view').toggleClass('select-all remove-all');
				toggleTitle.text(toggleTitle.text() === "Select All" ? "Remove All" : "Select All");
				$('.VINSelectError').addClass("hideImportant");
				if (toggleTitle.text() === "Remove All") {
					cardLI.addClass('activeBox');
					cardLiCheckbox.addClass('fa-check-square').attr('data-check', 'checked');
				}
				else {
					cardLI.removeClass('activeBox');
					cardLiCheckbox.removeClass('fa-check-square').attr('data-check', 'unchecked');
				}
			});

			/* View more versions functionality ends here */
			if (Deals.env == 1) {
				Deals.doc.on('ready', function () {
					if ($(".make-model-name").text().length > 15) {
						$(".make-model-name").addClass('big-make-name');
					}
				});
				Deals.doc.on('click', 'a.view-price-break-up', function () {
					Deals.product.bindPriceBreakup();
					$('.blackOut-window').first().show();
					var priceBreakUpContent = $("#breakUpContent").html();
					var priceBreakUpPopup = $('#price-breakup-popup');
					priceBreakUpPopup.find('.price-break-up-content').html(priceBreakUpContent);
					priceBreakUpPopup.find('[data-make-model]').text($('span.make-model-name').text());
					priceBreakUpPopup.removeClass('hide');
					Common.utils.lockPopup();
					window.history.pushState("price-breakup", "", "");
				});
				Deals.doc.on('click', '.blackOut-window, .price-breakup-close-btn', function () {
					$('#price-breakup-popup').addClass('hide');
					$('.blackOut-window').first().hide();
					Common.utils.unlockPopup();
				});
			}
		},

		openEnquiryPopup: function () {
			$('#enquire-detail-popup').removeClass('hide').addClass('show');
			var currentStock = $('div.product-amt-details[stock="' + Deals.product.getStockId() + '"]');
			var index = currentStock.parent().index();
			if ($.isEmptyObject(currentStock.find('span[data-savings]').eq(0).data('savings')))
				$('#interestedCarPrices').addClass('hide');
			else
				$('#interestedCarPrices').removeClass('hide');

			$('#OnRoadAmount').text(currentStock.find('[data-onroad-price]').eq(0).data('onroad-price'));
			$('#SavingsAmount').text(currentStock.find('span[data-savings]').eq(0).data('savings'));
			$('#OfferAmount').text(Common.utils.formatNumeric(currentStock.find('[data-adv-price]').eq(0).data('adv-price')));
			var stockImg = $("img.car-pic").eq(index);
			$("#interestCarImg").attr({ "src": stockImg.attr("src"), "onerror": stockImg.attr('onerror') });
		},

		getUniqueDealersStockId: function () {
			var stockIds = [];
			//stockIds.push(Deals.product.getStockId().toString());
			var dealerIds = [];
			dealerIds.push($('#priceDetails').find($('div.product-amt-details[stock="' + Deals.product.getStockId() + '"]')).attr('dealerId'));
			var stockElement = $('#priceDetails').find('div.product-amt-details');
			stockElement.each(function () {
				if ($.inArray($(this).attr("dealerId"), dealerIds) == -1) {
					stockIds.push($(this).attr("stock"));
					dealerIds.push($(this).attr("dealerId"));
				}
			});
			return stockIds.toString();

		},

		enquiryPopupVars: {
			enquireFirstScreen: '.enquiry-first-screen',
			enquireSecondScreen: '.enquiry-second-screen',
			leftEnquiryForm: '.enquiry-first-screen .left-enquiry-form',
			leftEnquiryFormTwo: '.enquiry-first-screen .left-enquiry-form-2',
			rightEnquiryForm: $('.enquiry-second-screen .right-enquiry-form'),
			rightEnquiryFormTwo: $('.enquiry-second-screen .right-enquiry-form-2'),
		},
		slideEnquirePopup: function () {
			$(Deals.product.enquiryPopupVars.enquireFirstScreen).animate({ width: '40%' }, function () {
				$(Deals.product.enquiryPopupVars.leftEnquiryForm).animate({ left: '-1000px' }).css({ "position": "relative", "display": "none" });
				$(Deals.product.enquiryPopupVars.leftEnquiryFormTwo).animate({ left: '0' }).css({ "position": "relative", "display": "inline-block" }).addClass('thanks-section');
				$(Deals.product.enquiryPopupVars.enquireSecondScreen).animate({ width: '60%' });
				if ($('.list-rcmd-cars li').length < 1 || Deals.inquirySource == 3) {
					$(Deals.product.enquiryPopupVars.enquireSecondScreen).hide();
					$(Deals.product.enquiryPopupVars.enquireFirstScreen).animate({ width: '100%' });
					$('#enquire-detail-popup').addClass('small-width-enquiry-popup');
				}
				setTimeout(function () {
					if ($('.left-enquiry-form-2').hasClass('thanks-section')) {
						$(Deals.product.enquiryPopupVars.rightEnquiryForm).animate({ left: '-1000px' }).css({ "position": "relative", "display": "none" });
						$(Deals.product.enquiryPopupVars.rightEnquiryFormTwo).animate({ left: '0' }).css({ "position": "relative", "display": "inline-block" });
					}
				}, 0);
			});

		},
		stopSlideEnquirePopup: function () {
			$(Deals.product.enquiryPopupVars.enquireFirstScreen).css({ width: '58%' });
			$(Deals.product.enquiryPopupVars.enquireSecondScreen).css({ width: '40%' });
			$(Deals.product.enquiryPopupVars.enquireSecondScreen).show();
			$('#enquire-detail-popup').removeClass('small-width-enquiry-popup');
			$(Deals.product.enquiryPopupVars.leftEnquiryForm).css({ "position": "relative", "left": "0", "display": "inline-block" });
			$(Deals.product.enquiryPopupVars.leftEnquiryFormTwo).css({ "position": "relative", "right": "-1000px", "display": "none" }).removeClass('abc');
			$(Deals.product.enquiryPopupVars.rightEnquiryForm).css({ "position": "relative", "left": "3px", "display": "inline-block" });
			$(Deals.product.enquiryPopupVars.rightEnquiryFormTwo).css({ "position": "relative", "right": "-1000px", "display": "none" });
		},

		filterVersions: function (element, value) {
			$('#no-variant-found').addClass('hide');
			var fuelValue = $('#fuelType').val();
			var transmissionValue = $('#transType').val();
			var filter = (fuelValue > -1 ? "[data-fuel=" + fuelValue + "]" : "") + (transmissionValue > -1 ? "[data-transmission=" + transmissionValue + "]" : "");
			if (fuelValue >= 0 || transmissionValue >= 0) {
				$('div.variant-info-car').hide();
			}
			$('div.variant-info-car' + filter).show();
			if ($('div.variant-info-car:visible').length == 0)
				$('#no-variant-found').removeClass('hide');
		},

		getTrackingAction: function (element) {
			if (element == 1)
				return 'popup1_CTA_clicked';
			else if (element == 2)
				return 'popup2_CTA_clicked';
		},

		setPopupOffer: function () {
			var savings = $('span[data-savings]:visible').data('savings');
			var carName = $.trim($('.current-car').text());
			var hurryText = $.trim($('.product-amt-details:visible').find('span.text-red').text());
			$('#popupSavings').text(savings);
			$('#popupCarName').text(carName);
			$('#popupHurryText').text(hurryText);
		},

		getTimeMessageFromDays: function (days) {
			var message = " within ";
			switch (days) {
				case 14:
					message += "2 weeks";
					break;
				case 30:
					message += "1 month";
					break;
				case 60:
					message += "2 months";
					break;
				case 90:
					message += "3 months";
					break;
				default:
					message += days + " days";
					break;
			}
			return message;
		},

		setPageTitle: function (makeName, modelName) {
			Deals.doc.prop('title', "CarWale Advantage - " + makeName + " " + modelName + " in " + $('#advDrpCity option:selected').text() + " - CarWale");
		},

		setModelImageSrc: function (imgHostUrl, orgImgPath) {
			if (location.pathname.indexOf("/m/"))
				return "this.src='" + imgHostUrl + '589x331' + orgImgPath + "';this.removeAttribute('onerror');";
			else
				return "this.src='" + imgHostUrl + '272x153' + orgImgPath + "';this.removeAttribute('onerror');";
		},

		getDetails: function (versionId, callSource, versionNode) {
			var button = $('#bookButton');
			button.hide();
			Common.utils.showLoading();
			var apiUrl;

			if (versionId == -1) {
				//apiUrl = '/api/advantage/deals/?modelId=566&cityId=1';
				apiUrl = "/api/advantage/deals/?modelId=" + Deals.modelId + "&cityId=" + Deals.cityId;
			}
			else
				//apiUrl = '/api/advantage/deals/?modelId=566&cityId=1';
				apiUrl = "/api/advantage/deals/?modelId=" + Deals.modelId + "&versionId=" + versionId + "&cityId=" + Deals.cityId;

			var stockDetail;

			$.ajax({
				type: 'GET',
				async: false,
				headers: { "sourceid": "1" },
				url: apiUrl,
				dataType: 'Json',
				success: function (response) {
					stockDetail = response;
					Common.utils.hideLoading();
					button.show();
					Deals.CampaignId = (typeof stockDetail.carColorDetails !== 'undefined' && stockDetail.carColorDetails.length > 0) ? stockDetail.carColorDetails[0].deals[0].campaignId : 0;
					Deals.DealerId = (typeof stockDetail.carColorDetails !== 'undefined' && stockDetail.carColorDetails.length > 0) ? stockDetail.carColorDetails[0].deals[0].dealerId : 0;
					if (callSource == 'photoGallery')
						return stockDetail;
					if (callSource == 'pageload')
						Deals.product.processDetailsOnPageLoad(stockDetail);
					else
						Deals.product.processDetailsOnVersionChange(stockDetail, versionNode, versionId);
					$("#page-container").parent().removeClass("min-height");
				}
			});

			return stockDetail;
		},

		getRecommendations: function () {
			var advHistory = $.cookie('_advHistory');
			var userHistory = $.cookie('_userModelHistory');
			var advModelIdArray = [0];
			var userHistoryArray = [0];
			var recommendationResponse = [];
			var apiUrl = "/api/advantage/recommmendations/?cityId=" + Deals.cityId + "&recommendationCount=3&modelId=" + Deals.modelId;

			if (advHistory.split('~').length > 1) {
				advModelIdArray = advHistory.split('~');
				advModelIdArray = Deals.utils.cookie.removeModelIdFromCookie(advModelIdArray, Deals.modelId);
				apiUrl = apiUrl + "&advHistory=" + advModelIdArray.join(',');
			}
			if (userHistory) {
				userHistoryArray = userHistory.split('~');
				userHistoryArray = Deals.utils.cookie.removeModelIdFromCookie(userHistoryArray, Deals.modelId);
				if (userHistoryArray.length > 0)
					apiUrl = apiUrl + "&pqHistory=" + userHistoryArray.join(',');
			}

			$.ajax({
				type: 'GET',
				async: false,
				headers: {
					"sourceid": "1"
				},
				url: apiUrl,
				dataType: 'Json',
				success: function (response) {
					Deals.product.processRecommendationOnPageLoad(response);
					recommendationResponse = response;
				}
			});
			return recommendationResponse;
		},


		details: {
			stock: ko.observable()
		},

		otherDetails: {
			carColorsDetails: ko.observable()
		},

		priceBreakupDetails: ko.observable(),

		recommendationDetails: {
			koViewModel: {
				advUserHistory: ko.observableArray([]),
				bestSavings: ko.observableArray([]),
				pqUserHistory: ko.observableArray([])
			}
		},

		bindDetails: function (json) {
			Deals.product.details.stock(json);
			Deals.product.otherDetails.carColorsDetails(json.carColorDetails);
		},


		selectYear: function (node, index) {
			var liNodes = node.find('li');
			liNodes.removeClass('active');
			liNodes.eq(index).addClass('active');
		},

		showDetailsByStock: function (stockId) {
			var offerNode = $('div.offerSection[stock="' + stockId + '"]');
			var offerText = offerNode.find('div .offers-title');
			var offerCategory = offerNode.find('div .offer-categories');
			var offers = offerNode.find('div.offer-details-slab');
			if (!(offerNode.find('.offerUlCont').children().length === 0)) {
				offers.removeClass('hideImportant');
				offerText.addClass('hideImportant');
				offerText.next().addClass('hideImportant');
				offerCategory.removeClass('hideImportant');
			}
			else if (!offerText.next().is(':empty')) {
				offers.removeClass('hideImportant');
				offerText.removeClass('hideImportant');
				offerText.next().removeClass('hideImportant');
				offerCategory.addClass('hideImportant');
			}
			if (!offerNode.find('.tnc').is(':empty')) {
				offerNode.find('.tnc').removeClass('hideImportant');
			}
			if (offerNode.find('.offerPrice').text() != 0)
				offerNode.find('.offerValue').removeClass('hideImportant');

			$('#m-3-reasons-slug ul .3-reasons-div[stock="' + stockId + '"]').removeClass('hideImportant');
			$('div.3ReasonsSlug[stock="' + stockId + '"]').removeClass('hideImportant');
			$('div.product-amt-details[stock="' + stockId + '"]').removeClass('hideImportant');
			$('li[stock="' + stockId + '"]').parents().eq(1).removeClass('hideImportant');
			$('.maskingNumber[stock="' + stockId + '"]').removeClass('hideImportant');
		},

		showDetailsByIndex: function (index) {
			var node = $('div.year-type').eq(index), dropdown = node.find('select'),
				currentYear = dropdown.attr('currentYear'),
				stockId = dropdown.find('option:contains("' + currentYear + '")').val(); //stockId = node.find('li.active').attr('stock');
			dropdown.val(stockId);
			if (dropdown.find('option').length > 1)
				node.removeClass('hideImportant');
			else {
				node.removeClass('hideImportant');
				dropdown.attr('disabled', 'disabled');
			}


			Deals.product.showDetailsByStock(stockId);
		},

		hideAllDetails: function () {
			$('div .tnc').addClass('hideImportant');
			$('div.offer-details-slab').addClass('hideImportant');
			$('div.offerValue').addClass('hideImportant');
			$('div.year-type').addClass('hideImportant');
			$('div.3ReasonsSlug').addClass('hideImportant');
			$('div.year-type').find('select').each(function () {
				$(this).removeAttr('disabled');
			});
			$('div.product-amt-details').addClass('hideImportant');
			$('#m-3-reasons-slug ul .3-reasons-div').addClass('hideImportant');
			$('#whatWeGet .3ReasonsSlug').removeClass('hideImportant');
			$('.maskingNumber').addClass('hideImportant');
		},

		setBreadcrumb: function (makeName, modelName, makeId, maskingName) {
			var makePageUrl = (Deals.env == 1 ? "/m" : "") + "/" + Common.utils.formatSpecial(makeName) + "-cars/";
			$("#homelink").attr("href", (Deals.env == 1 ? "/m/" : "/"));
			$('#makeName').text(makeName).attr("href", makePageUrl);
			$('#modelName').text(modelName).attr("href", "/" + Common.utils.formatSpecial(makeName) + "-cars/" + maskingName + "/");
		},

		fixButton: function () {
			Deals.doc.find('.extraDivHt').height($('.aged-car-btn-section').outerHeight());
			Deals.product.setFilterIcon();
		},
		setFilterIcon: function () {
			var scrollPosition = (window.pageYOffset !== undefined) ? window.pageYOffset : (document.documentElement || document.body.parentNode || document.body).scrollTop;
			if (scrollPosition + $(window).height() > ($('body').height() - $('footer').height())) {
				$('.extraDivHt').hide();
				$('.aged-car-btn-section').removeClass('float-fixed').addClass('float');
			}
			else {
				$('.extraDivHt').show();
				$('.aged-car-btn-section').removeClass('float').addClass('float-fixed');
			}
		},

		getCityId: function () {
			//return window.location.href.toLowerCase().split('cityid')[1].replace('=', '');
			return Deals.utils.getFilterFromQS('cityId');
		},
		getStockId: function () {
			return $('.year-type:visible').find('option:contains("' + $('.year-type:visible').find('select').attr('currentyear') + '")').val();
		},

		getDealerId: function (stockId) {
			return $('#priceDetails').find($('div.product-amt-details[stock="' + stockId + '"]')).attr('dealerId');
		},

		getCampaignId: function (stockId) {
			return $('#priceDetails').find($('div.product-amt-details[stock="' + stockId + '"]')).attr('campaignId');
		},

		getMultipleStockIds: function (parentDiv) {
			var stockIds = '';
			$('.' + parentDiv).find(".activeBox").each(function () {
				stockIds += $(this).data().stockid + ','
			});
			return stockIds.slice(0, -1);
		},

		getPushLeadUrl: function () {
			return "api/advantage/inquiry/";
		},

		processDetailsOnVersionChange: function (json, node, verId) {
			Deals.product.bindDetails(json);
			node.val(json.currentVersionId);
			Deals.versionId = json.currentVersionId;
			var yearContainer = $('div.year-type').eq(0);
			var yearDropDown = yearContainer.find('select');
			var stockId = yearDropDown.find('option:contains("' + yearDropDown.attr('currentYear') + '")').val();
			if (yearDropDown.find('option').length > 1)
				yearContainer.removeClass('hideImportant');
			else {
				yearContainer.removeClass('hideImportant');
				yearDropDown.attr('disabled', 'disabled');
			}
			var dealerArea = "";
			var areaValue = $('.active-color-shade').find("div.dealer-location").attr('area');
			if (areaValue != undefined && areaValue != null) {
			    dealerArea = areaValue.replace(/\,$/, '');
			}
			Deals.product.showDetailsByStock(stockId);
			Deals.product.specsData.getAndBindSpecs(verId);
			Deals.utils.triggerStockTracking(Deals.product.getStockId(), Deals.product.getCityId(), 'versionChange', Deals.env);
			$('#specsHeading').html(json.make.makeName + json.model.modelName + " " + $('#drpVersions option:selected').text() + " Features");
			$('#selectedCarInfo').html(json.model.modelName + " " + $('#drpVersions option:selected').text());
			$('#popupColorName').html($('span.tick-color').eq(0).parent().attr('title'));
			if (dealerArea != "") {
			    $('#dealerArea').text(" " + dealerArea + ",");
			}
			else {
			    $('#dealerArea').text(dealerArea);
			}
			$('#dealerCity').text(" " + $('.active-color-shade').find("div.dealer-location").attr('city'));
			$('#popupDealerLocation').html($('#dealerLocation').text());
			Deals.product.counterAmount($('[data-adv-price]:visible'));
			if (location.pathname.indexOf("/m/") != -1)
				$('#divPhotoGallery a').attr('href', window.location.href.split('?')[0] + "gallery/" + stockId + "/?src=" + Common.utils.getValueFromQS('src') + "&modelId=" + json.model.modelId + "&cityId=" + Common.utils.getValueFromQS('cityid'));

		},

		processDetailsOnPageLoad: function (json) {
			Deals.dealInquiries.prefillUserDetails();
			if (json.version != undefined && json.version.length > 0 && json.carColorDetails != undefined && json.carColorDetails.length > 0) {
				Deals.versionId = json.currentVersionId;
				Deals.product.bindDetails(json);
				ko.applyBindings(Deals.product.details, document.getElementById("product"));
				if (Deals.env == 1)
					ko.applyBindings(Deals.product.otherDetails, document.getElementById("productDeals"));
				ko.applyBindings(Deals.product.priceBreakupDetails, document.getElementById("breakUpContent"));
				var yearContainer = $('div.year-type').eq(0);
				var yearDropDown = yearContainer.find('select');
				var stockId = yearDropDown.find('option:contains("' + yearDropDown.attr('currentYear') + '")').val();
				Deals.product.showDetailsByStock(stockId);
				yearDropDown.val(stockId);
				if (yearDropDown.find('option').length > 1)
					yearContainer.removeClass('hideImportant');
				else {
					yearContainer.removeClass('hideImportant');
					yearDropDown.attr('disabled', 'disabled');
				}
				Deals.isCallCompleted = true;
				var dealerArea = "";
				var areaValue = $('.active-color-shade').find("div.dealer-location").attr('area');
				if ( areaValue != undefined && areaValue != null)
				{
				    dealerArea = areaValue.replace(/\,$/,'');
				}
				$('#popupColorName').html($('span.tick-color').eq(0).parent().attr('title'));
				if (dealerArea != "") {
				    $('#dealerArea').text(" " + dealerArea + ",");
				}
				else {
				    $('#dealerArea').text(dealerArea);
				}
				$('#dealerCity').text(" " + $('.active-color-shade').find("div.dealer-location").attr('city'));
				$('#popupDealerLocation').html($('#dealerLocation').text());
			}
			else {
				$('#product,#divBookButton,#howItWorks').hide();
				$('#divNoStock').show();
				$('#spnCarName').text(json.make.makeName + ' ' + json.model.modelName);
				if (location.pathname.indexOf("/m/") == -1)
					$('#imgNostock').attr('src', json.carImage.hostUrl + "589x331" + json.carImage.originalImgPath);
				else
					$('#imgNostock').attr('src', json.carImage.hostUrl + "272x153" + json.carImage.originalImgPath);
			}
			$('#heading').text(json.make.makeName + ' ' + json.model.modelName);
			Deals.product.setPageTitle(json.make.makeName, json.model.modelName);
			Deals.product.setBreadcrumb(json.make.makeName, json.model.modelName, json.make.makeId, json.model.maskingName);

			$('#advDrpCity').val(Deals.product.getCityId());

			Deals.product.specsData.registerEvents();
			Deals.product.specsData.getAndBindSpecs(json.currentVersionId);
			if (location.pathname.indexOf("/m/") != -1) {
				$(window).scrollTop($(window).scrollTop() + 1);
				$('#page-container').removeClass('visibility-hidden').addClass('visibility-visible');
				$('#divPhotoGallery a').attr('href', window.location.href.split('?')[0] + "gallery/" + stockId + "/?src=" + Common.utils.getValueFromQS('src') + "&modelId=" + json.model.modelId + "&cityId=" + Common.utils.getValueFromQS('cityid'));
			}
			else
				$('#page-container').show();
			setTimeout(function () {
				Deals.product.counterAmount($('[data-adv-price]:visible'));
			}, 500);
			$('#specsHeading').html(json.make.makeName + " " + json.model.modelName + " " + $('#drpVersions option:selected').text() + " Features");
			$('#selectedCarInfo').html(json.model.modelName + " " + $('#drpVersions option:selected').text());
			$('#interestedCarName').html(json.make.makeName + " " + json.model.modelName);
			$('#interestedCarCityName').html(Deals.cityName);
			$('#specsFeatures').html(json.make.makeName + ' ' + json.model.modelName + " " + $('#drpVersions option:selected').text() + " Features");
			$('#specsSpecifications').html(json.make.makeName + ' ' + json.model.modelName + " " + $('#drpVersions option:selected').text() + " Specifications");
			if (json.version != undefined && json.version.length > 0 && json.carColorDetails != undefined && json.carColorDetails.length > 0) {
				Deals.utils.triggerStockTracking(Deals.product.getStockId(), Deals.product.getCityId(), 'pageLoad', Deals.env);
			}
			Deals.utils.cookie.updateHistoryCookie(Deals.modelId);
		},

		processRecommendationOnPageLoad: function (json) {
			if (json != null) {
				$('#bestSavingHeading').text(Deals.cityName);
				for (var advHis = 0; advHis < json.advUserHistory.length; advHis++) {
					json.advUserHistory[advHis].src = "d14";
					json.advUserHistory[advHis].dataLabel = "AdvHistoryRecommendation_ProductDetailsPage";
				}
				for (var userHis = 0; userHis < json.pqUserHistory.length; userHis++) {
					json.pqUserHistory[userHis].src = "d15";
					json.pqUserHistory[userHis].dataLabel = "PQHistoryRecommendation_ProductDetailsPage";
				}
				for (var bs = 0; bs < json.bestSavings.length; bs++) {
					json.bestSavings[bs].src = "d16";
					json.bestSavings[bs].dataLabel = "BestOffers_ProductDetailsPage";
				}
				ko.applyBindings(Deals.product.recommendationDetails.koViewModel, document.getElementById("recommendationBestSavings"));
				Deals.product.recommendationDetails.koViewModel.advUserHistory(json.advUserHistory);
				Deals.product.recommendationDetails.koViewModel.bestSavings(json.bestSavings);
				Deals.product.recommendationDetails.koViewModel.pqUserHistory(json.pqUserHistory);
			}

			if (!($('#userRecommendation').is(":visible") || $('#advRecommendation').is(":visible") || $('#bestSavingRecommendation').is(":visible"))) {
				$('.specsFeaturesContainer').addClass('specs-full-width');
			}
			if (($('#userRecommendation').is(":visible") && $('#advRecommendation').is(":visible")) || (json.bestSavings.length == 1 && json.bestSavings[0].savings < 1))
				$('#bestSavingRecommendation').addClass("hideImportant");

		},

		replaceState: function (url) {
			try {
				window.history.replaceState(null, null, "?" + url);
			}
			catch (err) {

			}
		},

		counterAmount: function (node) {
			if (node.length > 0) {
				setTimeout(function () {
					var value = node.data().advPrice;
					$('.animated-amount').each(function () {
						$(this).prop('counting', 0).animate({ counting: value }, {
							duration: 800,
							easing: "linear",
							step: function (now) {
								var countingVal = Math.ceil(this.counting);
								node.text(Common.utils.formatNumeric(countingVal));
							}
						}).promise().done(function () {
							node.text(Common.utils.formatNumeric(value));
						});
					});
				}, 100);
			}
		},

		specsData: {
			koViewModel: {
				overview: ko.observableArray([]),
				features: ko.observableArray([]),
				specs: ko.observableArray([]),
				fueltype: ko.observable(),
				transmission: ko.observable(),
				mileage: ko.observable(),
				displacement: ko.observable()
			},

			registerEvents: function () {
				Deals.doc.on('click', 'div.keySpecsDiv .view-details-link', function () {
					Deals.utils.scrollToElement("specsFeaturesSection");
					//$('html,body').animate({
					//    scrollTop: $("#specsFeaturesContainer").offset().top
					//}, 'slow');
				});
				ko.applyBindings(Deals.product.specsData.koViewModel, document.getElementById('specsFeaturesSection'));
			},

			bindSpecs: function (json) {
				Deals.product.specsData.koViewModel.overview(json.overview);
				Deals.product.specsData.koViewModel.specs(json.specs);
				Deals.product.specsData.koViewModel.features(Deals.product.specsData.discardFeatures(json.features));
				Deals.product.specsData.koViewModel.fueltype(Deals.product.specsData.getOverview(json.overview, "26", "Not Applicable"));
				Deals.product.specsData.koViewModel.transmission(Deals.product.specsData.getOverview(json.overview, "29", "Not Applicable"));
				Deals.product.specsData.koViewModel.mileage(Deals.product.specsData.getOverview(json.overview, "12", "N/A"));
				Deals.product.specsData.koViewModel.displacement(Deals.product.specsData.getOverview(json.overview, "14", "N/A"));
			},

			getAndBindSpecs: function (versionId) {
				var json = Deals.dealCalls.getSpecs(versionId);
				Deals.product.specsData.bindSpecs(json);
				var showSpecs = false,
					showFeatures = false
				showOverview = false;
				if (json.features.length > 0) {
					showFeatures = true;
				} else {
					$("#specsFeaturesContainer li[data-tabs=features]").hide();
				}
				if (json.specs.length > 0) {
					showSpecs = true;
					if (!showFeatures) {
						$("#specsFeaturesContainer li[data-tabs=specifications]").addClass('active').click();
					}
				} else {
					$("#specsFeaturesContainer li[data-tabs=specifications]").hide();
				}
				if (json.overview.length > 0) showOverview = true;
				if (!showFeatures && !showSpecs) $("#specsFeaturesContainer .specsfeatures").hide();
				if (showOverview || showFeatures || showSpecs) {
					$("#specsFeaturesContainer").removeClass('hide');
					$('#specsHeading').removeClass('hide');
				}
				$('div.keySpecsDiv .keySpecsText').text($('#keySpecs').text());
			},
			discardFeatures: function (Features) {
				final = [];
				for (var i = 0; i < Features.length; i++) {
					if (!Deals.product.specsData.hasFeatures(Features[i])) continue;

					var feature = Features[i];
					var featobj = { name: feature.name, items: [] };

					for (var j = 0; j < feature.items.length; j++) {
						var item = feature.items[j];
						if (item.values[0] != "No") featobj.items[featobj.items.length] = { name: item.name, value: item.values[0] };
					}
					final[final.length] = featobj;
				}
				return final;
			},
			hasFeatures: function (feature) {
				if (feature.items == undefined) return false;
				for (var i = 0; i < feature.items.length; i++) {
					if (feature.items[i].values[0] != "No") return true;
				}
				return false;
			},
			getOverview: function (overview, masterId, defaultvalue) {
				for (var i = 0; i < overview.length; i++) {
					if (overview[i].itemMasterId == masterId) {
						if (overview[i].values != undefined && overview[i].values.length > 0)
							return overview[i].values[0];
					}
				}
				return defaultvalue;
			}
		},

		getPriceBreakup: function () {
			var priceBreakup = {};
			$.ajax({
				type: 'GET',
				async: false,
				url: Deals.autobizHostUrl + 'deals/webapi/pricebreakup?stockId=' + Deals.product.getStockId() + '&cityId=' + Deals.product.getCityId(),
				//dataType: 'Json',
				//contentType: "application/json",
				success: function (response) {
					priceBreakup = response;
				}
			});
			return priceBreakup;
		},

		bindPriceBreakup: function () {
			var priceBreakup = Deals.product.getPriceBreakup();
			Deals.product.priceBreakupDetails(priceBreakup);
			var currentStock = $('div.product-amt-details[stock="' + Deals.product.getStockId() + '"]');
			if (currentStock.find('span[data-savings]').length > 0) {
				var discountSection = $('#discountSection');
				discountSection.find('#onRoadPrice').text(currentStock.find('[data-onroad-price]').eq(0).data('onroad-price'));
				discountSection.find('#savings').text(currentStock.find('span[data-savings]').eq(0).data('savings'));
				discountSection.removeClass('hide');
			}
			$('#offerPrice').text(currentStock.find('[data-adv-price]').text());
		},

		getVisibleReasonsSlug: function () {
			var reasons = [];
			var node;
			if (Deals.env == 1)
				node = $('div.3-reasons-div:visible');
			else
				node = $('div.3ReasonsSlug:visible');
			node.find('li').each(function (index) {
				reasons.push(Deals.product.getReasonNode($(this)));
			});
			return reasons;
		},

		bindReasonsSlugPopup: function () {
			var reasons = Deals.product.getVisibleReasonsSlug();
			var node;
			if (Deals.env == 1)
				node = $('#whatWeGet .reasons');
			else
				node = $('#blockCar .reasons');
			node.find('li').each(function (index) {
				if (index < 3) {
					Deals.product.setReasonNode($(this), reasons[index])
				}
			});
		},

		getReasonNode: function (element) {
			return { heading: element.find('.reasonHeading').html(), description: element.find('.reasonDescription').text() }
		},

		setReasonNode: function (element, reason) {
			element.find('.reasonHeading').html(reason.heading);
			element.children('.reasonDescription').text(reason.description);
		}
	},

	recommendations: {
		registerEvents: function () {
			ko.applyBindings(Deals.recommendations.viewModelDeals, document.getElementById('recommendedCarsDetail'));
			Deals.doc.on('click', '#multiLeadPush', function () {
				if ($('.recommendedCarsDetail .activeBox').length > 0 && $.cookie('_CustMobile')) {
					$('#nextBtnProcessing').show();
					Deals.recommendations.trackMultipleStockLeads('recommendedCarsDetail');
					Deals.inquirySource = 3;
					var stockIds = Deals.product.getMultipleStockIds('recommendedCarsDetail');
					Common.utils.trackAction("CWInteractive", Deals.env == 2 ? "deals_desktop" : "deals_mobile", "popup2_recommendations_leadsubmit", (stockIds.split(',').length).toString());
					var node = $('.list-rcmd-cars');
					Deals.dealInquiries.pushMutipleVariantLead(stockIds, node);

					Deals.dealInquiries.submitMultipleLead(stockIds, Deals.product.getCityId());
				} else {
					$('.VINSelectError').removeClass("hideImportant");
					$(this).prev().show();
				}
			});

		},

		viewModelDeals: {
			deals: ko.observableArray()
		},

		dealsRecommendationBinding: function () {
			$.ajax({
				type: 'GET',
				url: '/api/deals/recommendedDeals/?cityid=' + Deals.product.getCityId() + '&recommendationcount=2&dealerId=' + ($('.dealerId').attr('dealerId') || Deals.modelId) + '&currentModel=' + Deals.modelId,
				dataType: 'Json',
				contentType: "application/x-www-form-urlencoded",
				success: function (response) {
					try {
						if (response.length > 0) {
							Deals.recommendations.viewModelDeals.deals(response);
							if (response.length < 3) {
								$('.recommendationCarousel').addClass('hide');
							}
						}
						else {
							if (Deals.env == 1)
								$('#recommendedCarsDetail').hide();

						}
					} catch (err) {
						console.log("api failure dealsRecommendationBinding : " + err.message);
					}
				}
			});
		},

		d_showRecommendations: function () {
			$('#multiLeadPush').prev().hide();
			$('#advantage-detail-popup').addClass('rcmd-car-popup');
			$('#detailPopup').animate({ left: '-700px' }, 700, function () {
				$('#detailPopup').hide();
			});
			setTimeout(function () {
				$('#recommendedCarsDetail').show().animate({ left: 0 }, 500);
			}, 300);
		},

		m_hideToggleElement: function () {
			var toggleTitle = $('span.toggle-selection-txt');
			var selectionView = $('p.toggle-selection-view');
			toggleTitle.text("Select All");
			selectionView.removeClass('remove-all');
			selectionView.addClass('select-all');
		},

		m_showRecommendations: function () {
			$('#multiLeadPush').prev().hide();
			$('#contact-det-popup').addClass('rcmd-car-popup');
			$('#detailPopup').fadeOut(500);
			setTimeout(function () {
				$('#recommendedCarsDetail').show().animate({ left: 0 }, 200);
			}, 300);
		},

		trackMultipleStockLeads: function (element) {
			var arr = Deals.product.getMultipleStockIds(element);
			$.each(arr, function (index, value) {
				Deals.utils.triggerStockTracking(value, Deals.product.getCityId(), 'recommendationButton', Deals.env);
			});
		}
	},

	variantList: {
		callBackArrow: "",
		m_reqAsstToggleFlag: true,
		variantHeight: 0,
		registerEvents: function () {

			Deals.doc.callBackArrow = $(".callback-arrow");
			Deals.variantList.adjustHeight();

			Deals.doc.on('click', ".req-call-thnk-close", function () {
				$(".req-call-thnk").hide();
				$(".req-call-btn-section").show();
			});

			Deals.doc.on('click', '#close-assistance', function () {
				Deals.doc.callBackArrow.removeClass('expert-visible');
				$('#assitance-thnk-msg').hide();
				$('#close-assistance').hide();
				setTimeout(function () {
					$('.assitance-form-fillup').show();
					$('.user-expert').show();
					Deals.doc.callBackArrow.addClass('expert-hide');
					Deals.doc.callBackArrow.removeClass('callback-width-change');
				}, 500);
			});

			Deals.doc.on('click', '#variantsMobField', function () {
				Deals.utils.hideErrorMessage($(this));
			});

			/* Request Assitance Slug Code Starts Here */
			Deals.doc.on('click', '.checkbox-selction', function () {
				var element = $(this);
				if (element.find('input').is(':checked'))
					element.addClass("activeBox");
				else
					element.removeClass("activeBox");

				if (Deals.env == 1)
					Deals.variantList.m_checkBoxClickEvents(element);
				else
					Deals.variantList.d_checkBoxClickEvents(element);

				$("#req-assitance-variantList").data('label', $(".car-title-name").text() + $(".activeBox").length);
			});

			Deals.doc.on('click', '.call-asstn .user-expert', function () {
				Deals.doc.callBackArrow.toggleClass('expert-visible');
				Deals.doc.callBackArrow.toggleClass('expert-hide');
			});

			Deals.doc.on('click', '#req-assitance-variantList', function () {
				if (!$('.checkbox-selction input').is(':checked') || $('.variant-content .activeBox').length == 0) {
					$('.select-version-msg').show();
					if (Deals.env == 2) {
						ShakeFormView($(".call-assistance-form"));
					}
				}
				else if (Deals.utils.validateOnlyMobile($('#variantsMobField'))) {
					Common.utils.setEachCookie("_CustMobile", $('#variantsMobField').val());
					Deals.inquirySource = 4;
					var stockIds = Deals.product.getMultipleStockIds('variant-content');
					var node = $('.variant-content');
					Deals.dealInquiries.pushMutipleVariantLead(stockIds, node);
					Deals.dealInquiries.submitMultipleLead(stockIds, Deals.product.getCityId());
				}
				else if (Deals.env == 2)
					ShakeFormView($(".call-assistance-form"));
			});

			/* Request Assitance Slug Code Ends Here */

			/* View more versions functionality starts here */

			$(".view-more-versions").toggle(function () {
				$(this).text("View few variants");
				$('.variant-content').css({ "height": "auto" });
			}, function () {
				$(this).text("View all variants");
				$('.variant-content').animate({
					height: "262px",
				}, 500);
			});

			$(".checkbox-selction input").attr('checked', false);
		},

		adjustHeight: function () {
			$(".view-more-versions").toggle();
			if ($('.variant-content .variant-info-car:visible').length > 3) {
				$('.variant-content').css({ "height": "262px", 'overflow': 'hidden' });
				$(".view-more-versions").text("View all variants");
				$('.view-more-versions').show();
			}
			else {
				$('.view-more-versions').hide();
				$('.variant-content').css({ "height": "auto", 'overflow': 'hidden' })
			}
		},

		resetReqAstBox: function () {
			Deals.doc.callBackArrow.removeClass('callback-width-change');
			$('#assitance-thnk-msg').hide();
			$('#close-assistance').hide();
			$('.assitance-form-fillup').show();
			$('.user-expert').show();
		},

		m_checkBoxClickEvents: function (element) {
			Deals.variantList.resetReqAstBox();
			if (Deals.variantList.m_reqAsstToggleFlag) {
				Deals.variantList.m_reqAsstToggleFlag = false;
				Deals.doc.callBackArrow.addClass('expert-visible');
				setTimeout(function () {
					Deals.doc.callBackArrow.addClass('expert-hide');
					Deals.doc.callBackArrow.removeClass('expert-visible');
				}, 1000);
			}
			if (!$('.checkbox-selction input').is(':checked')) {
				Deals.variantList.m_reqAsstToggleFlag = true;
				Deals.doc.callBackArrow.removeClass('expert-hide');
				Deals.doc.callBackArrow.removeClass('expert-visible');
			}

		},
		d_checkBoxClickEvents: function (element) {
			if ($('.checkbox-selction input').is(':checked')) {
				$('.select-version-msg').hide();
			}
		}
	},

	photoGallery: {
		registerEvents: function () {
			$(window).bind('popstate', function (event) {
				if ($('#detailPopup').is(':visible')) {
					Deals.dealInquiries.m_closeInquiryPopup();
				}
			});

			Deals.doc.on('click', '#proceedBtn', function () {
				var retVal = Deals.utils.validateOnlyMobile($('#personMob'));
				if (retVal) {
					$('#nextBtnProcessing').show();
					Deals.inquirySource = 3;
					Common.utils.setEachCookie("_CustMobile", $('#personMob').val());
					Deals.dealInquiries.pushUnpaidLead(Deals.dealInquiries.getLeadData(Deals.photoGallery.stockId, $.cookie('_CustMobile'), Deals.DealerId, Deals.CampaignId), true);
					Deals.dealInquiries.submitInquiry(Deals.photoGallery.stockId, Deals.product.getCityId());
					Common.utils.trackAction('CWInteractive', 'deals_mobile', ' PhotoGallery_popup_CTAButton_clicked', 'validation_true');
				}
				else
					Common.utils.trackAction('CWInteractive', 'deals_mobile', ' PhotoGallery_popup_CTAButton_clicked', 'validation_false');
				return retVal;
			});


			Deals.doc.on('click', '.blackOut-window, .contact-det-close-btn', function () {
				Deals.dealInquiries.m_closeInquiryPopup();
			});

			Deals.doc.on('click', '#reqAssisBtn', function () {
				Deals.inquirySource = 3;
				Deals.utils.triggerStockTracking(Deals.product.getStockId(), Deals.product.getCityId(), 'proceedButton', Deals.env);
				Deals.dealInquiries.m_openInquiryPopup();
				window.history.pushState("gallery-inquiry", "", "");
			});

		},

	},

	makeModel: {
		kotemplate: function () {
			self = this;
			this.updateModels = function (vm) {
				if (vm.selectedMake() == undefined) {

					self.models([]);
				} else {
					$('#drpSelectDealModel').removeClass('btn-disable');
					self.models(Deals.dealCalls.getDealModels(vm.selectedMake().makeId));
				}
			}
			this.selectedMake = ko.observable();
			this.selectedModel = ko.observable();
			this.makes = ko.observableArray([]);
			this.models = ko.observableArray([]);
			this.doneClick = function (vm, event) {
				if (vm.selectedMake() != undefined && vm.selectedModel() != undefined) {
					if (Deals.pageId == "A-1" || Deals.pageId == "mA-1")
						location.href = ("/" + (location.pathname.indexOf("/m/") == 0 ? "m/" : "") + vm.selectedMake().makeName.toLowerCase().replace(" ", "") + "-cars/" + vm.selectedModel().maskingName.toLowerCase() + "/advantage/?src=" + $(event.target).attr('data-src') + "&cityId=" + Deals.cityId);
					if (Deals.pageId == "A-4" || Deals.pageId == "mA-4")
						location.href = ("/" + (location.pathname.indexOf("/m/") == 0 ? "m/" : "") + vm.selectedMake().makeName.toLowerCase().replace(" ", "") + "-cars/" + vm.selectedModel().maskingName.toLowerCase() + "/advantage/?src=" + $(event.target).attr('data-src') + "&cityId=" + Deals.cityId);
					return true;
				}
				if (vm.selectedMake() == undefined) { $("#drpSelectDealMake").addClass("border-red"); } else { $("#drpSelectDealMake").removeClass("border-red"); }
				if (vm.selectedModel() == undefined) { $("#drpSelectDealModel").addClass("border-red"); } else { $("#drpSelectDealModel").removeClass("border-red"); }
			}
		},
		koViewModel: {},
		registerEvents: function () {
			if (Deals.makeModel.koViewModel.makes == undefined) {
				Deals.makeModel.koViewModel = new Deals.makeModel.kotemplate();
				ko.applyBindings(Deals.makeModel.koViewModel, document.getElementById('make-model-popup'));
				Deals.makeModel.koViewModel.makes(Deals.dealCalls.getDealMakes(Deals.cityId == undefined ? 1 : Deals.cityId));
			}
		}
	},

	landing: {
		registerEvents: function () {
			Deals.doc.on('change', '#advDrpCity', function () {
				Deals.cityId = $(this).val();
				Deals.makeModel.koViewModel.makes(Deals.dealCalls.getDealMakes(Deals.cityId == (undefined || '0') ? 1 : Deals.cityId));

			});

		}
	},

	coachMarks: {
		registerEvents: function () {
			/* Product Details Coachmark functionality starts here */
			$(window).on('load', function () {
				if ($.cookie('_advCoach') == null) {
					$('.change-car-coachmark').show();
					Deals.utils.cookie.setCookie('_advCoach', '1', 180);
				}
			});

			Deals.doc.on('click', '#advantage-got', function () {
				$(this).closest('div.advantage-coachmark').hide();
			});

			Deals.doc.on('click', '#btnCarCoach', function (e) {
				$('.change-car-coachmark').hide();
				$('.change-version-coachmark').show();
			});

			Deals.doc.on('click', '#btnVersionCoach', function (e) {
				$('.change-car-coachmark').hide();
				$('.change-version-coachmark').hide();
				$('.change-color-coachmark').show();
			});

			Deals.doc.on('click', '#btnColorCoach, .dontShowCoach', function (e) {
				$('.change-car-coachmark').hide();
				$('.change-version-coachmark').hide();
				$('.change-color-coachmark').hide();
			});
			/* Product Details Coachmark functionality ends here */
		}
	},

	pageLoad: {
		enquiryLoad: function () {
			if (location.pathname.indexOf("/m/") == -1) Deals.utils.bindNumberSlug();
			Deals.dealInquiries.registerEvents();
			Deals.utils.registerEvents();

		},

		confirmationLoad: function () {
			if ($.cookie('PGRespCode') == "0300") {
				Common.utils.trackAction("CWNonInteractive", "Advantage_general", "Booking_Success", Deals.env == 1 ? "Mobile" : "Desktop");
			}
		},

		landingLoad: function () {
			if (location.pathname.indexOf("/m/") == -1) {
				Deals.utils.bindNumberSlug();
				if (!Deals.isAdvantageCity) {
					Deals.utils.registerCityPopup();
				}
			}
			else if (!Deals.isAdvantageCity) {
				Deals.utils.registerCityPopup();
			}

			$('#advDrpCity').val(Deals.cityId);
			Deals.makeModel.registerEvents();
			Deals.landing.registerEvents();

			$(window).on('scroll', function () {
				if ($(window).scrollTop() > 40) {
					$('#header').addClass('header-fixed-with-bg');
				} else {
					$('#header').removeClass('header-fixed-with-bg');
				}
			});
		},

		productLoad: function () {
			jQuery.support.cors = true;
			Deals.cityName = $("#advDrpCity option:[value=" + Deals.cityId + "]").text();
			if (location.pathname.indexOf("/m/") == -1) Deals.utils.bindNumberSlug();
			Deals.product.fixButton();
			Deals.product.registerEvents();
			Deals.utils.registerEvents();
			Deals.dealInquiries.hideMobilenumber();
			Deals.dealInquiries.prefillUserDetails();
			var json = Deals.product.getDetails(Deals.versionId != '' ? Deals.versionId : -1, 'pageload');
			if (Deals.env == 2)
				var recommendationResponse = Deals.product.getRecommendations();
			$('#nextBtnProcessing').hide();
			if (Deals.env == 2)
				$('#galleryBtnProcessing').hide();
			Deals.recommendations.registerEvents();
			Deals.variantList.registerEvents();
			Deals.coachMarks.registerEvents();
			Deals.offerOfWeek.setOfferAttributes();
			Deals.offerOfWeek.updateOfferTime();
		},

		photGalleyLoad: function () {
			$('#nextBtnProcessing').hide();
			Deals.cityId = Deals.utils.getFilterFromQS('cityId');
			Deals.photoGallery.registerEvents();
			Deals.recommendations.registerEvents();
			Deals.product.getDetails(Deals.versionId != '' ? Deals.versionId : -1, 'photoGallery');
			Deals.dealInquiries.prefillUserDetails();
		}
	}
};
checkpath = function () { return false; }

