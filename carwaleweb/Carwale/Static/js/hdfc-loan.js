var loanHdfc = {
    loanCity: {},
    apiInput: {},
    eligibilityApiInput: {},
    dateOfBirth: '',
    chosenInput: '',
    processedScreenIds: [],
    companyId: $('#txtCompany'),
    reNumeric: /^[0-9]*$/,
    commonValidFlag: false,
    selectedMakeId: '',
    selectedMakeName: '',
    selectedModelId: '',
    selectedModelName: '',
    selectedVersionId: '',
    selectedVersionName: '',
    isVersionLoading: false,
    loanAmt: $('#loanAmountTxt'),
    tenorYear: $('#yearTxt'),
    isMobile: $('body').hasClass('cwm-body'),
    trackingCategory: '',
    currentScreenId : 1,
    prefillData: {},
    currentDateTime: new Date(),
    isPrefilled: false,
    versionId: -1,
    modelId: -1,
    isVersion: false,
    queryString: '',
    platformId: "",
    pageLoad: function () {
        loanHdfc.commonEvents.registerEvents();
        loanHdfc.residenceType.registerEvents();
        loanHdfc.personalDetail.registerEvents();
        loanHdfc.employmentDetail.registerEvents();                   
    },
    commonEvents: {
        registerEvents: function () {
            loanHdfc.commonEvents.bindYears();
            loanHdfc.personalDetail.datepicker($("#txtDobdate"), $("#spntxtDobdate"));
            ko.applyBindings(loanHdfc.thankYouPage.koViewModel, $('#thank-you-popup')[0]);
            loanHdfc.commonEvents.disableNext();
            if (loanHdfc.isMobile) {
                loanHdfc.trackingCategory = 'HDFC_failure tracking_m';
                //container min height
                loanHdfc.commonEvents.setPageMinHeight();
                //set floating button
                loanHdfc.commonEvents.setFloatButton();
                $(window)
                    .scroll(function () { loanHdfc.commonEvents.setFloatButton(); })
                    .resize(function () {
                        loanHdfc.commonEvents.setFloatButton();
                        loanHdfc.commonEvents.setPageMinHeight();
                    });
            } else {
                loanHdfc.trackingCategory = 'HDFC_failure tracking_d';
                $(window).load(function () {
                    // on load scroll loan page to car type
                    $('html, body').animate({ scrollTop: $('#loanQuote').offset().top - 50 }, 1000);
                    $('.loan-section').append($('#thank-you-popup'));
                    $('.loan-section').append($('#api-failure-thank-you-popup'));
                    $('.loan-section').append($('#final-submit-thank-you'));
                    $('.loan-section').append($('#bankbazaar-popup'));
                });

                $('.btn.form-cont-btn').on('click', function () {
                    $('#next').click();
                });
            }

            loanHdfc.commonEvents.impressionTracking('residenceType');

            //For App webview
            var url = window.location.href.toLowerCase();
            loanHdfc.platformId = url.indexOf("platformid") < 0 ? "" : Common.utils.getValueFromQS('platformid');
            loanHdfc.platformId = loanHdfc.platformId.indexOf("#") < 0 ? loanHdfc.platformId : loanHdfc.platformId.split("#")[0];

            //Accordion
            loanHdfc.commonEvents.accordion();
            loanHdfc.commonEvents.divShowHideNextClick();

            $('#returnToForm, #returnToFormBBLink').on('click', function () {
                if (loanHdfc.platformId == "83" || loanHdfc.platformId == "74")
                    window.location.href = '/finance/hdfc/?platformid=' + loanHdfc.platformId + '&versionId=' + loanHdfc.selectedVersionId;
                else if(loanHdfc.platformId == "43")
                    window.location.href = '/m/';
                else
                    window.location.href = '/';
            });
            $('#apiFailReturnToForm').on('click', function () {
                if (loanHdfc.platformId == "83" || loanHdfc.platformId == "74")
                    window.location.href = '/finance/hdfc/?platformid=' + loanHdfc.platformId + '&versionId=' + loanHdfc.selectedVersionId;
                else
                    window.location.href = '/finance/hdfc/?versionId=' + loanHdfc.selectedVersionId;
            });
            //to disable arrow image in dropdowns IE
            var isIE11 = !!navigator.userAgent.match(/Trident.*rv\:11\./);
            if (isIE11 || $.browser.msie)
                $('.loan-section .select-box').css('background', "transparent");

            //pill box common click function
            $('.pill-box').on('click', function () {
                var $this = $(this);
                $this.parent().siblings('div').find('.pill-box').removeClass('active-box');
                $this.addClass('active-box');
                if ($('div.page:visible').next().find('.pill-box').length > 0 && !($('div.page:visible').next().find('.pill-box').hasClass('active-box')))
                    loanHdfc.commonEvents.disableNext();
                else loanHdfc.commonEvents.enableNext();
            });

            loanHdfc.commonEvents.backBtnClick(); //back button click function
            loanHdfc.commonEvents.nextBtnClick(); //next button click function
            //prefill car data 
            loanHdfc.apiCalls.processMakes(undefined, loanHdfc.commonEvents.prefillMake);
            window.onhashchange = function (e) {
                if ((e.oldURL.indexOf('#make') > 0 && e.newURL.indexOf('#') < 0) || (e.oldURL.indexOf('#make') > 0 && e.newURL.indexOf('#') > 0))
                    loanHdfc.selectCar.hidepopup($('.MakeDiv div.filterBackArrow')[0], 'loanCarMake');
                else if (e.oldURL.indexOf('#model') > 0 && e.newURL.indexOf('#make') > 0)
                    loanHdfc.selectCar.hidepopup($('.modelDiv div.filterBackArrow')[0], 'loanCarModel');
                else if (e.oldURL.indexOf('#version') > 0 && e.newURL.indexOf('#model') > 0)
                    loanHdfc.selectCar.hidepopup($('.versionDiv div.filterBackArrow')[0], 'loanCarVersion');
                else if (e.oldURL.indexOf('#thankyou') > 0 && (e.newURL.indexOf('#') > 0 || e.newURL.indexOf('') == 0) && ($('#thank-you-popup').is(":visible") || $('#api-failure-thank-you-popup').is(":visible") || $('#final-submit-thank-you').is(":visible") || $('#bankbazaar-popup').is(":visible")))
                    loanHdfc.commonEvents.onBrowserBack();
                else if (e.oldURL.indexOf('#calendar') > 0 && (e.newURL.indexOf('#') > 0 || e.newURL.indexOf('') == 0))
                    loanHdfc.dateOfBirth.hide();
                else if (e.oldURL.indexOf('#company') > 0 && (e.newURL.indexOf('#') > 0 || e.newURL.indexOf('') == 0))
                    $('.companyDiv div.filterBackArrow').click();
            }            
        },
        onBrowserBack: function () {
            $('#loanQuote').html('Please fill in a few details to check your loan eligibility');
            loanHdfc.commonEvents.disableBack();
            loanHdfc.commonEvents.disableNext();
            loanHdfc.apiInput = {};
            loanHdfc.eligibilityApiInput = {};
            loanHdfc.queryString = "";
            loanHdfc.commonEvents.clearControls();
            loanHdfc.commonEvents.pageHide('thank-you-popup', 'residenceType');
            loanHdfc.commonEvents.pageHide('api-failure-thank-you-popup', 'residenceType');
            loanHdfc.commonEvents.pageHide('final-submit-thank-you', 'residenceType');
            loanHdfc.commonEvents.pageHide('bankbazaar-popup', 'residenceType');
            loanHdfc.selectCar.pushState(null, "", '');
            window.location.reload();
        },
        setPageMinHeight: function () {
            var pageHt = $(window).height() - 44; // 44 val is top header height
            $('.loan-section > div.page').css('min-height', pageHt);
        },

        setFloatButton: function () {
            var scrollPosition = (window.pageYOffset !== undefined) ? window.pageYOffset : (document.documentElement || document.body.parentNode || document.body).scrollTop;
            if (scrollPosition + $(window).height() > ($('body').height() - $('footer').height())) {
                $('.extraDivHt').hide();
                $('.float-container').addClass('float');
            }
            else {
                $('.extraDivHt').show();
                $('.float-container').removeClass('float');
            }
        },
        accordion: function () {
            var $this, arrow, plusMinus;
            $('.accordion > .strip').off('click').on('click', function () {
                $this = $(this);
                arrow = $this.find('.icon.arrow .fa');
                plusMinus = $this.find('.icon.plus-minus .fa');
                if ($this.next().is(":visible")) {
                    $this
                        .next().slideToggle(0)
                        .parent().toggleClass('open');
                    arrow.attr('class', 'fa fa-angle-down');
                    plusMinus.attr('class', 'fa fa-plus');
                } else {
                    $this.parent().siblings('.accordion').find('.strip + div').slideUp(0);
                    $('.accordion').removeClass('open');
                    $('.icon.arrow .fa').attr('class', 'fa fa-angle-down');
                    $('.icon.plus-minus .fa').attr('class', 'fa fa-plus');
                    $this
                        .next().slideDown(0)
                        .parent().addClass('open');
                    arrow.attr('class', 'fa fa-angle-up');
                    plusMinus.attr('class', 'fa fa-minus');

                    //scroll to an element
                    var offset = $this.parent().offset().top;
                    var windHt = $(window).scrollTop() + $(window).height() - 300;

                    if (offset > windHt) {
                        $('html, body').animate({
                            scrollTop: offset - (($(window).height() * 0.5))
                        }, 700);
                    }

                }

            });
        },
        divShowHideNextClick: function () {
            loanHdfc.apiInput.Product_Applied_For = 'new';
            setTimeout(function () {
                loanHdfc.selectCar.selCarRegEvents();
            }, 200);
        },
        disableNext: function () {
            $('#next').addClass('btn-disable');
        },
        enableNext: function () {
            $('#next').removeClass('btn-disable');
        },
        disableBack: function () {
            $('#back').addClass('btn-disable');
        },
        enableBack: function () {
            $('#back').removeClass('btn-disable');
        },
        pageTitle: function (currentPageTitle) {            
            switch (currentPageTitle) {
                case "selectCity": currentPageTitle = "Select City"; loanHdfc.currentScreenId = 8; break;
                case "residenceType": currentPageTitle = "Residence Type"; loanHdfc.currentScreenId = 1; break;
                case "shiftToResidence": currentPageTitle = "Shift To Residence"; loanHdfc.currentScreenId = 2; break;
                case "bookingTime": currentPageTitle = "Booking Time"; loanHdfc.currentScreenId = 7; break;
                case "employmentType": currentPageTitle = "Employment Type"; loanHdfc.currentScreenId = 3; break;
                case "Salaried": currentPageTitle = "Salaried"; loanHdfc.currentScreenId = 4; break;
                case "employedProfession": currentPageTitle = "Employed Profession"; loanHdfc.currentScreenId = 4; break;
                case "employedBusiness": currentPageTitle = "Employed Business"; loanHdfc.currentScreenId = 4; break;
                case "selectCar": currentPageTitle = "Select Car"; loanHdfc.currentScreenId = 6; break;
                case "personalDetail": currentPageTitle = "Personal Detail"; loanHdfc.currentScreenId = 9; break;
                case "thank-you-popup": loanHdfc.currentScreenId = 10; break;
                case "dob-iscust": loanHdfc.currentScreenId = 5; break;
                default: currentPageTitle = "Car Loan";
            }
            if (!loanHdfc.isMobile) {
                $('#pageTitle').text(currentPageTitle);
            }
        },
        pageHide: function (currentId, prevId) {
            $('#' + currentId).addClass('hide');
            $('#' + prevId).removeClass('hide');
            loanHdfc.commonEvents.pageTitle(prevId);
        },
        pageShow: function (currentId, nextId) {            
            $('#' + currentId).addClass('hide');
            $('#' + nextId).removeClass('hide');
            if (loanHdfc.isMobile)
                $('html,body').animate({ 'scrollTop': $('.loan-section').offset().top }, 300);
            else {                
                $('html,body').animate({ 'scrollTop': $('#loanQuote').offset().top - 50 }, 300);
            }
            if (nextId != 'bankbazaar-popup' && nextId != 'empSubType') {
                loanHdfc.commonEvents.pageTitle(nextId);
                loanHdfc.commonEvents.impressionTracking(nextId);
            }
        },
        backBtnClick: function () {
            $(document).on('click', '#back:not(".btn-disable")', function () {
                var visiblePage = $('div.page:visible');
                var currentPageId = visiblePage.attr('id');
                var prevPageId = visiblePage.prev('.page').attr('id');
                if (currentPageId === "dob-iscust") {
                    var pillBoxId = $('#employmentType .pill-box.active-box').attr('id');
                    switch (pillBoxId) {
                        case "optSalaried": loanHdfc.commonEvents.pageHide(currentPageId, 'empSubType'); break;
                        case "optSelfEmpBusiness": loanHdfc.commonEvents.pageHide(currentPageId, 'empSubType'); break;
                        case "optSelfEmp": loanHdfc.commonEvents.pageHide(currentPageId, 'empSubType'); break;
                        case "optOthers": loanHdfc.commonEvents.pageHide(currentPageId, 'employmentType'); break;
                    }
                }
                if (prevPageId === "residenceType") {
                    loanHdfc.commonEvents.disableBack();
                    $('#loanQuote').html('Please fill in a few details to check your loan eligibility');
                }

                loanHdfc.commonEvents.pageHide(currentPageId, prevPageId);
                loanHdfc.commonEvents.enableNext();
            });
        },
        nextBtnClick: function () {
            $(document).on('click', '#next:not(".btn-disable")', function () {
                var visiblePage = $('div.page:visible');
                var currentPageId = visiblePage.attr('id');
                var nextPageId = visiblePage.next('.page').attr('id');

                if (visiblePage.hasClass('form')) {
                    if ($('#personalDetail').is(':visible'))
                        loanHdfc.personalDetail.validatePersonalDetails();
                    if ($('#selectCar').is(':visible')) {
                        loanHdfc.selectCar.validateCar();
                    }
                    if ($('#dob-iscust').is(':visible'))
                        loanHdfc.personalDetail.validateExistCustDob();
                    if ($('#Salaried').is(':visible'))
                        loanHdfc.employmentDetail.validateSalaried();
                    if ($('#employedProfession').is(':visible'))
                        loanHdfc.employmentDetail.validateSelfEmployed();
                    if ($('#employedBusiness').is(':visible'))
                        loanHdfc.employmentDetail.validateSelfEmployedProfession();

                    loanHdfc.eligibilityApiInput.CompanyId = loanHdfc.apiInput.Emp_Type == "Salaried" ? loanHdfc.eligibilityApiInput.CompanyId : "0";
                }
                else if (currentPageId == "employmentType") {
                    var pillBoxId = $('#employmentType .pill-box.active-box').attr('id');
                    switch (pillBoxId) {
                        case "optSalaried": $('#optSalaried').click(); break;
                        case "optSelfEmpBusiness": $('#optSelfEmpBusiness').click(); break;
                        case "optSelfEmp": $('#optSelfEmp').click(); break;
                        case "optOthers": $('#optOthers').click(); break;
                    }
                }
                else
                    loanHdfc.commonEvents.pageShow(currentPageId, nextPageId);

                if ($('div.page:visible').find('.pill-box').length > 0 && !($('div.page:visible').find('.pill-box').hasClass('active-box'))) {
                    loanHdfc.commonEvents.disableNext();
                }
                else {
                    if ($('#residenceType').is(':visible') || $('#shiftToResidence').is(':visible')) {
                        if ($('div.page:visible').find('.pill-box.active-box').length > 1 || $('#selectYear .pill-box').text() != "Before 2013")
                            loanHdfc.commonEvents.enableNext();
                        else loanHdfc.commonEvents.disableNext();
                    }
                    else {
                        loanHdfc.commonEvents.enableNext();
                    }
                }
                loanHdfc.commonEvents.enableBack();
                if (nextPageId === "selectCity" && $('#otherCities').val() != "" && $('#otherCities').val() != $('#otherCities').attr('placeholder')) {
                    loanHdfc.commonEvents.enableNext();
                }
                if (currentPageId == "residenceType")
                    $('#loanQuote').html('Get Loan Quote Instantly');
            });
        },
        clearControls: function () {
            $('.pill-box').removeClass('active-box');
            $('.form select, .form input').val('');
            $('#loanCarMake').text('Make').attr('data-val', '');
            $('#loanCarModel').text('Model').attr('data-val', '');
            $('#loanCarVersion').text('Version').attr('data-val', '');
            $('#loanCompany').text('Company Name').attr('data-val', '');
            $('#residenceType .sub-type-wrap').hide();
            $('#car-title').text("");
            $('#otherCities').val("");
            $('#back, #next').removeClass('hide');
            loanHdfc.companyId.text('--Select your Company--').val('0');
            $('.chosen-single span').text('--Select your Company--');            
        },
        prefillCity: function () {
            var url = window.location.href.toLowerCase();
            var cityId = url.indexOf("cityid") < 0 ? "" : Common.utils.getValueFromQS('cityid');
            if (!(url.indexOf("cityid") < 0) && cityId != "" && cityId > 0) {
                cityId = cityId.indexOf("#") < 0 ? cityId : Common.utils.getValueFromQS('cityid').split("#")[0];
                $.when(Common.utils.ajaxCall({
                    type: 'GET',
                    url: '/webapi/GeoCity/GetCityNameById/?cityid=' + cityId,
                    dataType: 'Json'
                })).done(function (cityName) {
                    $('#otherCities').val(cityName);
                    if ($('#otherCities').val() != "") {
                        loanHdfc.eligibilityApiInput.CityId = cityId;
                        loanHdfc.apiInput.CityId = cityId;
                        loanHdfc.apiInput.Res_City = cityName;
                    }
                });
            }
            else if (isCookieExists("_CustCityMaster") && masterCityNameCookie != "Select City") {
                $('#otherCities').val(masterCityNameCookie);
                loanHdfc.eligibilityApiInput.CityId = masterCityIdCookie;
                loanHdfc.apiInput.Res_City = masterCityNameCookie;
                loanHdfc.apiInput.CityId = masterCityIdCookie;
            }
        },
        prefillMake: function (response) {
            if (response) {
                var url = window.location.href.toLowerCase();
                loanHdfc.modelId = url.indexOf("modelid") < 0 ? "" : Common.utils.getValueFromQS('modelid');
                loanHdfc.versionId = url.indexOf("versionid") < 0 ? "" : Common.utils.getValueFromQS('versionid');
                if (url.indexOf("modelid") != -1 && loanHdfc.modelId != "") {
                    loanHdfc.modelId = loanHdfc.modelId.indexOf("#") < 0 ? loanHdfc.modelId : loanHdfc.modelId.split("#")[0];
                    if (loanHdfc.modelId > 0) {
                        try {
                            $.when(loanHdfc.apiCalls.getCarModelData(loanHdfc.modelId)).done(function (data) {
                                loanHdfc.prefillData = data;
                                loanHdfc.commonEvents.prefillCarData();
                            });
                        } catch (e) { console.log(e) };
                    }
                }
                else if (url.indexOf("versionid") != -1 && loanHdfc.versionId != "") {
                    loanHdfc.versionId = loanHdfc.versionId.indexOf("#") < 0 ? loanHdfc.versionId : loanHdfc.versionId.split("#")[0];
                    if (loanHdfc.versionId > 0) {
                        try {
                            loanHdfc.isVersion = true;
                            $.when(loanHdfc.apiCalls.getCarVersionData(loanHdfc.versionId)).done(function (data) {
                                loanHdfc.prefillData = data;
                                loanHdfc.commonEvents.prefillCarData();
                            });
                        } catch (e) { console.log(e) };
                    }
                }
            }
        },
        prefillModel: function (response) {
            if (response) {
                loanHdfc.selectedVersionId = loanHdfc.prefillData.VersionId;
                loanHdfc.selectedVersionName = loanHdfc.prefillData.VersionName;
                loanHdfc.apiInput.VersionId = $.trim(loanHdfc.selectedVersionId);
                loanHdfc.eligibilityApiInput.VersionId = $.trim(loanHdfc.selectedVersionId);
                if (loanHdfc.isMobile) {
                    $('#loanCarMake').text(loanHdfc.prefillData.MakeName).attr('data-val', loanHdfc.prefillData.MakeId);
                    $('#loanCarModel').text(loanHdfc.prefillData.ModelName).attr('data-val', loanHdfc.prefillData.ModelId);
                    loanHdfc.isPrefilled = true;
                }
                else {
                    $("#model option").each(function () {
                        if ($(this).text() == loanHdfc.prefillData.ModelName) {
                            $(this).attr('selected', 'selected');
                        }
                    });
                }
                if (loanHdfc.isVersion) {
                    loanHdfc.apiCalls.processVersions(undefined, loanHdfc.commonEvents.prefillVersion);
                }
                else {
                    loanHdfc.apiCalls.processVersions();
                }
            }
        },
        prefillVersion: function (response) {
            if (response) {
                if (loanHdfc.isMobile) {
                    $('#loanCarVersion').text(loanHdfc.prefillData.VersionName).attr('data-val', loanHdfc.prefillData.VersionId);
                    loanHdfc.isPrefilled = true;
                }
                else {
                    $("#version option").each(function () {
                        if ($(this).text() == loanHdfc.prefillData.VersionName) {
                            $(this).attr('selected', 'selected');
                        }
                    });
                }
            }
        },
        prefillCarData: function () {
            loanHdfc.selectedMakeId = loanHdfc.prefillData.MakeId;
            loanHdfc.selectedMakeName = loanHdfc.prefillData.MakeName;
            loanHdfc.selectedModelId = loanHdfc.prefillData.ModelId;
            loanHdfc.selectedModelName = loanHdfc.prefillData.ModelName;
            loanHdfc.apiInput.Car_Make = $.trim(loanHdfc.selectedMakeName);
            loanHdfc.apiInput.Car_Model = $.trim(loanHdfc.selectedModelName);
            if (loanHdfc.isMobile) {
                $('#loanCarMake').text(loanHdfc.prefillData.MakeName).attr('data-val', loanHdfc.prefillData.MakeId);
            }
            else {
                $("#drpMake option").each(function () {
                    if ($(this).text() == loanHdfc.prefillData.MakeName) {
                        $(this).attr('selected', 'selected');
                    }
                });
            }
            loanHdfc.apiCalls.processModels("", loanHdfc.commonEvents.prefillModel);
        },

        showNextEligibleScreen: function (currentScreen, nextScreen, reason) {

            $.when(loanHdfc.apiCalls.isEligible()).done(function (data) {
                if (!data) {
                    $('#back, #next').addClass('hide');
                    if (loanHdfc.isMobile)
                        $('#hideBackNext').addClass('hide');
                    loanHdfc.selectCar.pushState(null, "", "thankyou");
                    loanHdfc.commonEvents.pageShow(currentScreen, "bankbazaar-popup");
                    Common.utils.trackAction('CWNonInteractive', loanHdfc.trackingCategory, "Eligibility failed", reason);
                }
                else
                    loanHdfc.commonEvents.pageShow(currentScreen, nextScreen);                
            });
        },
        impressionTracking: function (currentScreen)
        {
            if (loanHdfc.processedScreenIds.indexOf(currentScreen) < 0) {
                loanHdfc.processedScreenIds.push(currentScreen);
                Common.utils.trackAction('CWNonInteractive', loanHdfc.trackingCategory, "Page shown", loanHdfc.currentScreenId + '_' + currentScreen);
            }
        },
        bindYears: function(){
            var currentYear = new Date().getFullYear();
            var startYear = currentYear - 4;
            var defaultYears = [];
            for (var index = currentYear; index >= startYear ; index--) {
                defaultYears.push(index);
            }
            defaultYears.push('Before ' + startYear);
            var viewModel = {
                SelectYears: ko.observableArray(defaultYears)
            };
            ko.applyBindings(viewModel, document.getElementById("employedProfession"));
            ko.applyBindings(viewModel, document.getElementById("employedBusiness"));
            ko.applyBindings(viewModel, document.getElementById("shiftToResidence"));            
        }
    },

    residenceType: {
        monthValue: "",
        registerEvents: function () {
            $('#selectCity .pill-box').on('click', function () {
                var $this = $(this);
                loanHdfc.apiInput.CityId = $this.attr('data-value');
                loanHdfc.eligibilityApiInput.CityId = $this.attr('data-value');
                loanHdfc.apiInput.Res_City = $this.text();
                if (loanHdfc.apiInput.CityId > 0)
                    loanHdfc.commonEvents.pageShow('selectCity', 'personalDetail');
                else {
                    $('#back, #next').addClass('hide');
                    loanHdfc.selectCar.pushState(null, "", "thankyou");
                    loanHdfc.commonEvents.pageShow('selectCity', 'bankbazaar-popup');
                    Common.utils.trackAction('CWNonInteractive', loanHdfc.trackingCategory, "Eligibility failed", 'I dont belong to any of the above cities.');
                }
                
                $('#otherCities').val('');
            });
            $('#residenceType .resiType').on('click', function () {
                loanHdfc.apiInput.Resi_type = $(this).text();
            });
            loanHdfc.residenceType.otherCityAutosuggest();
            loanHdfc.residenceType.residenceBox();
        },
        residenceBox: function () {
            $('.residence-box > div > .pill-box').on('click', function () {
                $('.residence-box > div > a + div').hide();
                $(this).closest('.residence-box').css("padding-bottom", '120px');
                $(this).next().fadeIn(700);
            });

            $('.sub-type-wrap div > .pill-box').on('click', function () {
                if ($('.residence-box > div > .pill-box').hasClass('active-box'))
                    setTimeout(function () {
                        loanHdfc.commonEvents.pageShow('residenceType', 'shiftToResidence');
                        $('#loanQuote').html('Get Loan Quote Instantly');
                    }, 200);
                loanHdfc.commonEvents.enableBack();
                if (!($('#selectYear .pill-box').hasClass('active-box') && $('#selectMonth .pill-box').hasClass('active-box')))
                    loanHdfc.commonEvents.disableNext();
            });

            $('#selectYear .pill-box').on('click', function () {
                loanHdfc.apiInput.Res_address_year = $(this).text();
                if ($(this).text() == "Before 2013") {
                    loanHdfc.commonEvents.pageShow('shiftToResidence', 'employmentType');
                    loanHdfc.apiInput.Res_address_month = "";
                    loanHdfc.residenceType.calculateStability();
                }
                else {
                    setTimeout(function () {
                        $('#selectMonth .strip').click();
                        if ($('#selectMonth .pill-box').hasClass('active-box')) {
                            loanHdfc.residenceType.calculateStability();
                            loanHdfc.commonEvents.showNextEligibleScreen('shiftToResidence', 'employmentType', 'Residence stability less than 2Years');
                        }
                    }, 200);
                }
            });

            $('#selectMonth .pill-box').on('click', function () {
                loanHdfc.apiInput.Res_address_month = $(this).text();
                residenceType.monthValue = $(this).attr('data-value');
                setTimeout(function () {
                    if ($('#selectYear .pill-box').hasClass('active-box')) {
                        loanHdfc.residenceType.calculateStability();
                        loanHdfc.commonEvents.showNextEligibleScreen('shiftToResidence', 'employmentType', 'Residence stability less than 2Years');
                    }
                    else if ($('#selectYear .pill-box').text() != "Before 2013")
                        $('#selectYear .strip').click();
                }, 200);
            });

            $('#bookingTime .pill-box').on('click', function () {
                //if ($('#otherCities').val() != "" && $('#otherCities').val() != $('#otherCities').attr('placeholder')) {
                //    loanHdfc.commonEvents.enableNext();
                //}
                loanHdfc.commonEvents.pageShow('bookingTime', 'selectCity');
                loanHdfc.apiInput.BuyingPeriod = $(this).text();
            });
        },

        otherCityAutosuggest: function () {
            if(typeof isMobileDevice == "undefined") {
                $('#otherCities').cw_autocomplete({
                    resultCount: 5,
                    source: ac_Source.allCarCities,

                    click: function (event, ui, orgTxt) {
                        loanHdfc.loanCity.Name = Common.utils.getSplitCityName(ui.item.label);
                        loanHdfc.loanCity.Id = ui.item.id;
                        loanHdfc.apiInput.CityId = loanHdfc.loanCity.Id;
                        loanHdfc.eligibilityApiInput.CityId = loanHdfc.loanCity.Id;
                        loanHdfc.apiInput.Res_City = loanHdfc.loanCity.Name;
                        ui.item.value = loanHdfc.loanCity.Name;
                        $('#selectCity .pill-box').removeClass('active-box');
                        loanHdfc.commonEvents.pageShow('selectCity', 'personalDetail');
                        loanHdfc.commonEvents.enableBack();
                    },
                    open: function (result) {
                        loanHdfc.loanCity.result = result;
                    },
                    afterfetch: function (result, searchtext) {
                        if (result != undefined && result.length > 0) {
                            loanHdfc.apiInput.CityId = loanHdfc.loanCity.Id;
                            loanHdfc.eligibilityApiInput.CityId = loanHdfc.loanCity.Id;
                            loanHdfc.apiInput.Res_City = loanHdfc.loanCity.Name;
                            if ($('#otherCityError').is(':visible'))
                                $('#otherCityError').hide();
                        }
                        else {
                            $('#otherCityError').show();
                            if ($('#selectCity:visible .pill-box.active-box').length < 1)
                                loanHdfc.commonEvents.disableNext();
                        }
                    },
                    focusout: function () {
                        if ($('li.ui-state-focus a:visible').text() != "") {
                            if (loanHdfc.loanCity == undefined) loanHdfc.loanCity = new Object();
                            var focused = loanHdfc.loanCity.result[$('li.ui-state-focus').index()];
                            if (focused != undefined && focused.label == $('#otherCities').val()) {
                                loanHdfc.loanCity.Name = Common.utils.getSplitCityName(focused.label);
                                loanHdfc.loanCity.Id = focused.id;
                                loanHdfc.apiInput.CityId = loanHdfc.loanCity.Id;
                                loanHdfc.eligibilityApiInput.CityId = loanHdfc.loanCity.Id;
                                loanHdfc.apiInput.Res_City = loanHdfc.loanCity.Name;
                            }
                            else {
                                loanHdfc.loanCity = {};
                            }
                        }
                        else {
                            if ($('#otherCityError').is(':visible'))
                                $('#otherCityError').hide();
                            if ($('#selectCity:visible .pill-box.active-box').length < 1)
                                loanHdfc.commonEvents.disableNext();
                        }
                    }
                });
            }
            else {
                var otherCitiesInputField = $('#otherCities');

                $(otherCitiesInputField).cw_easyAutocomplete({
                    inputField: otherCitiesInputField,
                    resultCount: 5,
                    source: ac_Source.allCarCities,

                    click: function (event) {
                        var selectionValue = otherCitiesInputField.getSelectedItemData().value,
                        selectionLabel = otherCitiesInputField.getSelectedItemData().label;

                        loanHdfc.loanCity.Name = Common.utils.getSplitCityName(selectionLabel);
                        loanHdfc.loanCity.Id = selectionValue;
                        loanHdfc.apiInput.CityId = loanHdfc.loanCity.Id;
                        loanHdfc.eligibilityApiInput.CityId = loanHdfc.loanCity.Id;
                        loanHdfc.apiInput.Res_City = loanHdfc.loanCity.Name;

                        $(otherCitiesInputField).val(loanHdfc.loanCity.Name);
                        $('#selectCity .pill-box').removeClass('active-box');
                        loanHdfc.commonEvents.pageShow('selectCity', 'personalDetail');
                        loanHdfc.commonEvents.enableBack();
                    },

                    afterFetch: function (result, searchText) {
                        loanHdfc.loanCity.result = result;

                        if (result != undefined && result.length > 0) {
                            loanHdfc.apiInput.CityId = loanHdfc.loanCity.Id;
                            loanHdfc.eligibilityApiInput.CityId = loanHdfc.loanCity.Id;
                            loanHdfc.apiInput.Res_City = loanHdfc.loanCity.Name;
                            if ($('#otherCityError').is(':visible'))
                                $('#otherCityError').hide();
                            }
                        else {
                            $('#otherCityError').show();
                            if ($('#selectCity:visible .pill-box.active-box').length < 1)
                                loanHdfc.commonEvents.disableNext();
                        }
                    },
                    focusout: function () {
                        if ($('#otherCityError').is(':visible')) {
                            $('#otherCityError').hide();
                        }
                        if ($('#selectCity:visible .pill-box.active-box').length < 1) {
                            loanHdfc.commonEvents.disableNext();
                        }
                    }
                });
            }
        },

        calculateStability: function () {
            var now = loanHdfc.currentDateTime;
            var month = now.getMonth() + 1;                     // for jan value is 0
            var year = now.getFullYear();
            var stability = year - (loanHdfc.apiInput.Res_address_year == "Before 2013" ? 2013 : loanHdfc.apiInput.Res_address_year);
            loanHdfc.eligibilityApiInput.StabilityTime = stability == 2 ? (residenceType.monthValue <= month ? 2 : 1) : stability;
            loanHdfc.eligibilityApiInput.ResidenceTypeId = loanHdfc.apiInput.Resi_type == "Owned" ? 1 : 2;
        }
    },

    personalDetail: {
        registerEvents: function () {
            loanHdfc.personalDetail.pdEvents();
            $('#txtFirstName').on('change', function () {
                loanHdfc.personalDetail.checkNameInvalid($('#txtFirstName'), $('#spntxtFirstName'));
            });
            $('#txtLastName').on('change', function () {
                loanHdfc.personalDetail.checkNameInvalid($('#txtLastName'), $('#spntxtLastName'));
            });
            $('#txtEmail').on('change', function () {
                loanHdfc.personalDetail.checkEmailInvalid();
            });
            $('#txtMobile').on('change', function () {
                loanHdfc.personalDetail.checkMobInvalid();
            });
            $('#txtPanNo').on('change', function () {
                loanHdfc.personalDetail.checkPanNoInvalid($("#txtPanNo"), $("#spntxtPanNo"));
            });

            if ($('#personalDetail').is(':visible'))
                $('#next').click(function () {
                    loanHdfc.personalDetail.validatePersonalDetails();
                });

            $('#btnVerify').on('click', function () {
                loanHdfc.personalDetail.verifyOtp();
            });

            $('#userDetails').on('click', function () {
                $('#quote-dob, #quote-existingCust').addClass('hide');
                loanHdfc.commonEvents.pageShow('thank-you-popup', 'personalDetail');
            });
        },
        pdEvents: function () {
            $('#txtMobile').on('change', function () {
                //loanHdfc.personalDetail.showOtpPopup();
            });

            $('#otp-close').on('click', function () {
                loanHdfc.personalDetail.hideOtpPopup();
            });
        },
        datepicker: function (id, spnId) {
            function pickerOpen() {
                if (loanHdfc.isMobile) {
                    var $this = this;
                    setTimeout(function () {
                        $this.blur();
                    }, 100);
                    $('html').css({ 'overflow': 'hidden' });
                    loanHdfc.selectCar.pushState(null, '', 'calendar');
                }
            };

            function pickerClose() {
                if (loanHdfc.isMobile) {
                    var $this = this;
                    setTimeout(function () {
                        $this.blur();
                    }, 100);
                    $('html').css({ 'overflow': 'auto' });
                }
            };
            var now = loanHdfc.currentDateTime;
            var day = ("0" + now.getDate()).slice(-2);
            var month = ("0" + (now.getMonth() + 1)).slice(-2);
            var minYear = (now.getFullYear() - 65) + "-" + (month) + "-" + (day);
            var maxYear = (now.getFullYear() - 18) + "-" + (month) + "-" + (day);

            id.Zebra_DatePicker({
                view: 'years',
                onOpen: pickerOpen,
                onClose: pickerClose,
                show_icon: false,
                direction: [minYear, maxYear],
                default_position: 'below',
                onSelect: function () {
                    spnId.hide();
                    loanHdfc.apiInput.DateOfBirth = id.val();
                    loanHdfc.eligibilityApiInput.dateOfBirth = id.val();
                }
            });

            loanHdfc.dateOfBirth = id.data('Zebra_DatePicker');
        },

        showOtpPopup: function () {
            Common.utils.trackAction('CWInteractive', loanHdfc.trackingCategory, "Page shown", "OTP shown");
            $('#buyerForm').show();
            if (loanHdfc.isMobile)
                $('#buyerForm .screen2').slideDown(700);
            else $('#buyerForm .screen2').show();
            $("#m-blackOut-window").show();
            if (!loanHdfc.isMobile) Common.utils.lockPopup();
        },

        hideOtpPopup: function () {
            if (loanHdfc.isMobile)
                $('#buyerForm').slideUp();
            else $('#buyerForm').hide();
            $('#buyerForm .screen2').hide();
            $("#m-blackOut-window").hide();
            if (!loanHdfc.isMobile) Common.utils.unlockPopup();
        },

        validateInputField: function (field, regex) {
            try {
                if (!regex.test(field.val().toLowerCase())) {
                    return false;
                }
                return true;
            }
            catch (e) { console.log(e) }
            return false;
        },
        checkMobInvalid: function () {
            var isError = false;
            var reMobile = /^[6789]\d{9}$/;

            if ($('#txtMobile').val() == "") {
                $('#spntxtMobile').show();
                return true;
            }
            else if ($('#txtMobile').val() != "") {

                if (!loanHdfc.reNumeric.test($("#txtMobile").val())) {
                    $('#spntxtMobile').show().text("Please enter numeric data only.");
                    return true;
                }
                else if (!reMobile.test($("#txtMobile").val())) {
                    $('#spntxtMobile').show().text("Please enter valid mobile number.");
                    return true;
                }
                else if (!loanHdfc.reNumeric.test($("#txtMobile").val()) && $("#txtMobile").val().length < 10) {
                    $('#spntxtMobile').show().text("Please enter 10 digit.");
                    return true;
                }
                else {
                    $('#spntxtMobile').hide();
                    return false;
                }
            }
            else {
                $('#spntxtMobile').hide();
                return false;
            }
        },

        checkEmailInvalid: function () {
            var reEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$/;
            if ($('#txtEmail').val() == "") {
                $('#spntxtEmail').show();
                return true;
            }
            else if (!loanHdfc.personalDetail.validateInputField($("#txtEmail"), reEmail)) {
                $('#spntxtEmail').show().text("Please enter valid email.");
                return true;
            }
            else {
                $('#spntxtEmail').hide();
                return false;
            }
        },

        checkPanNoInvalid: function(id, errorId){
            var reName = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}$/;

            if ($.trim(id.val()) == "") {
                errorId.show();
                return true;
            }
            else if (!loanHdfc.personalDetail.validateInputField(id, reName)) {
                errorId.show().text("Please enter valid PAN.");
                return true;
            }
            else {
                errorId.hide();
                return false;
            }
        },

        checkNameInvalid: function (id, errorId) {
            var reName = /^([-a-zA-Z ']*)$/;
            if ($.trim(id.val()) == "") {
                errorId.show();
                return true;
            }
            else if (!loanHdfc.personalDetail.validateInputField(id, reName)) {
                errorId.show().text("Please enter valid name.");
                return true;
            }
            else {
                errorId.hide();
                return false;
            }
        },
        
        validatePersonalDetails: function () {
            var isFirstNameInvalid = loanHdfc.personalDetail.checkNameInvalid($('#txtFirstName'), $('#spntxtFirstName'));
            var isLastNameInvalid = loanHdfc.personalDetail.checkNameInvalid($('#txtLastName'), $('#spntxtLastName'));
            var isMobInvalid = loanHdfc.personalDetail.checkMobInvalid();
            var isEmailInvalid = loanHdfc.personalDetail.checkEmailInvalid();
            var isPanInvalid = loanHdfc.personalDetail.checkPanNoInvalid($("#txtPanNo"), $("#spntxtPanNo"));

            if (isFirstNameInvalid || isLastNameInvalid || isMobInvalid || isEmailInvalid || isPanInvalid) {
                return false;
            }
            else {
                loanHdfc.personalDetail.checkMobileVerification();
                return true;
            }
        },

        validateExistCustDob: function () {
            var isDobInvalid = false;
            
            if ($('#txtDobdate').val() == "" || $('#txtDobdate').val() == "NaN-NaN-NaN") {
                $('#spntxtDobdate').show();
                isDobInvalid = true;
            }
            else {
                $('#spntxtDobdate').hide();
                isDobInvalid = false;
            }
            if (isDobInvalid) {
                return false;
            }
            else {
                loanHdfc.eligibilityApiInput.dateOfBirth = $('#txtDobdate').val();
                loanHdfc.commonEvents.showNextEligibleScreen('dob-iscust', 'selectCar', 'Age issue');
                return true;
            }
        },

        checkMobileVerification: function () {
            var mobileNumber = $.trim($("#txtMobile").val());
            $.ajax({
                type: 'GET',
                headers: { "sourceid": "43" },
                url: '/api/mobile/verify/' + mobileNumber + '/',
                dataType: 'Json',
                success: function (json) {

                    var viewModel = {
                        tollFreeNumber: ko.observable(json.tollFreeNumber)
                    };
                    ko.cleanNode($("#btnMissCallAnchorTag")[0]);
                    ko.applyBindings(viewModel, $("#btnMissCallAnchorTag")[0]);
                    if (!json.isMobileVerified) {
                        loanHdfc.personalDetail.showOtpPopup();
                    }
                    else {
                        Common.utils.showLoading();
                        if (loanHdfc.eligibilityApiInput.IncomeTypeId != 3) {
                            loanHdfc.apiCalls.getLoanParamData();
                        }
                        else
                            loanHdfc.thankYouPage.showThankYouPage("fail");

                        loanHdfc.submitAction.getQuote();
                    }
                }
            });
        },

        verifyOtp: function () {
            var enteredOTP = $.trim($("#txtOTP").val());
            var mobileNumber = $.trim($("#txtMobile").val());
            if (enteredOTP !== '' && $("#txtOTP").attr('placeholder') != enteredOTP) {
                $('#imgLoadingBtnVerify').removeClass('hide');

                $.ajax({
                    type: 'GET',
                    url: '/api/verifymobile/?mobileNo=' + mobileNumber + "&cwiCode=" + enteredOTP,
                    headers: { 'CWK': 'KYpLANI09l53DuSN7UVQ304Xnks=', 'SourceId': '1' },
                    dataType: 'Json',
                    success: function (json) {
                        $('#imgLoadingBtnVerify').addClass('hide');
                        var viewModel = {
                            verificationResponse: ko.observable(json)
                        };

                        if (json.responseCode == 1) {
                            $('#otpError').addClass('hide');
                            loanHdfc.personalDetail.hideOtpPopup();

                            if (loanHdfc.eligibilityApiInput.IncomeTypeId != 3) {
                                loanHdfc.apiCalls.getLoanParamData();
                            }
                            else
                                loanHdfc.thankYouPage.showThankYouPage("fail");

                            loanHdfc.submitAction.getQuote();
                        }
                        else {
                            $('#otpError').removeClass('hide').text('Please enter valid OTP');
                            console.log("verification error: " + json.responseMessage);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        console.log("error: " + errorThrown);
                    }
                });
            }
            else $('#otpError').removeClass('hide').text('Please enter 5 digit OTP');
        }
    },

    employmentDetail: {
        registerEvents: function () {
            $('#drpExp').on('change', function () {
                loanHdfc.employmentDetail.checkDropDownInvalid($(this), $('#spndrpExp'), -1);
            });
            $('#txtIncome').on('change', function () {
                loanHdfc.employmentDetail.checkIncomeInvalid($('#txtIncome'), $('#spntxtIncome'));
            });
            $('#txtEmi').on('change', function () {
                loanHdfc.employmentDetail.checkEmiInvalid();
            });

            $('#employmentType .pill-box').on('click', function () {                
                setTimeout(function () {
                    $('#empSubType > .page').addClass('hide');
                        loanHdfc.commonEvents.pageShow('employmentType', 'empSubType');
                }, 200);
                loanHdfc.apiInput.Emp_Type = $.trim($(this).text());
            });
            $('#optSalaried').on('click', function () {
                loanHdfc.eligibilityApiInput.IncomeTypeId = 1;
                setTimeout(function () {
                    loanHdfc.commonEvents.pageShow('employmentType', 'Salaried');
                    if (!loanHdfc.isMobile)
                        loanHdfc.companyId.chosen({
                            max_selected_options: 1,
                            no_results_text: "If you do not see your employer on our list, simply type the name in this field.",
                            search_contains: true
                        });
                }, 200);
            });
            $('#optSelfEmp').on('click', function () {
                loanHdfc.eligibilityApiInput.IncomeTypeId = 2;
                setTimeout(function () {
                    loanHdfc.commonEvents.pageShow('employmentType', 'employedProfession');
                }, 200);
            });
            $('#optSelfEmpBusiness').on('click', function () {
                loanHdfc.eligibilityApiInput.IncomeTypeId = 2;
                setTimeout(function () {
                    loanHdfc.commonEvents.pageShow('employmentType', 'employedBusiness');
                }, 200);
            });
            $('#optOthers').on('click', function () {
                loanHdfc.eligibilityApiInput.IncomeTypeId = 3;
                setTimeout(function () {
                    $('#empSubType').addClass('hide');
                    $('#back, #next').addClass('hide');
                    if (loanHdfc.isMobile)
                        $('#hideBackNext').addClass('hide');
                    loanHdfc.selectCar.pushState(null, "", "thankyou");
                    loanHdfc.commonEvents.pageShow('employmentType', 'bankbazaar-popup');
                    Common.utils.trackAction('CWNonInteractive', loanHdfc.trackingCategory, "Eligibility failed", 'Employment type Student/Retired/Homemaker selected');
                }, 200);
            });
            $('#optOthers1').on('click', function () {
                loanHdfc.eligibilityApiInput.IncomeTypeId = 3;
                setTimeout(function () {
                    $('#empSubType').addClass('hide');
                    $('#back, #next').addClass('hide');
                    if (loanHdfc.isMobile)
                        $('#hideBackNext').addClass('hide');
                    loanHdfc.selectCar.pushState(null, "", "thankyou");
                    loanHdfc.commonEvents.pageShow('employmentType', 'bankbazaar-popup');
                    Common.utils.trackAction('CWNonInteractive', loanHdfc.trackingCategory, "Eligibility failed", 'Employment type Others selected');
                }, 200);
            });
            if (!loanHdfc.isMobile) {
                $(document).on('blur', '.chosen-search input', function () {
                    if ($('.chosen-search input').val() != "") {
                        $('.cw-body .company-chosen-box .chosen-single span').text($('.chosen-search input').val());
                    }
                });
            }
        },

        checkCompInvalid: function () {
            if ((loanHdfc.isMobile && $('#loanCompany').attr('data-val').trim() == "") || (!loanHdfc.isMobile && ($('.chosen-single span').text() == "--Select your Company--" || $('.chosen-single span').text().trim() == ""))) {
                $('#spntxtCompany').show();
                return true;
            }
            else {
                loanHdfc.apiInput.Company_Name = loanHdfc.isMobile ? $.trim($('#loanCompany').attr('data-val')) : $.trim($('.chosen-single span').text());

                if (!loanHdfc.isMobile || loanHdfc.apiInput.Company_Name != $('#drpCompany li[isselected=true]').text().trim())
                    loanHdfc.eligibilityApiInput.CompanyId = 0;

                if (!loanHdfc.isMobile && loanHdfc.apiInput.Company_Name == $.trim($('#txtCompany option:selected').html())) {
                    //set companyid if company is from drop down list, else set 0
                    loanHdfc.eligibilityApiInput.CompanyId = $("#txtCompany").chosen().val();
                }
                console.log()
                $('#spntxtCompany').hide();
                return false;
            }
        },
        checkDropDownInvalid: function (id, errorId, minVal) {
            if (id.val() <= minVal) {
                errorId.show();
                return true;
            }
            else {
                errorId.hide();
                return false;
            }
        },
        checkIncomeInvalid: function (id, errorId) {
            if (id.val() == "") {
                errorId.show().text("Please enter your monthly income.");
                return true;
            }
            if (!loanHdfc.personalDetail.validateInputField(id, loanHdfc.reNumeric)) {
                errorId.show().text("Please enter numeric data only.");
                return true;
            }
            else if (id.val() < 10000) {
                errorId.show().text("Minimum income should be 10000");
                return true;
            }
            else {
                errorId.hide();
                return false;
            }
        },
        checkEmiInvalid: function () {
            if ($('#txtEmi').val() != "" && !loanHdfc.personalDetail.validateInputField($("#txtEmi"), loanHdfc.reNumeric)) {
                $('#spntxtEmi').show();
                return true;
            }
            else {
                $('#spntxtEmi').hide();
                return false;
            }
        },
        validateSalaried: function () {
            var isCompInvalid = loanHdfc.employmentDetail.checkCompInvalid();
            var isExpInvalid = loanHdfc.employmentDetail.checkDropDownInvalid($('#drpExp'), $('#spndrpExp'), -1);
            var isEmiInvalid = loanHdfc.employmentDetail.checkEmiInvalid();
            var isIncomeInvalid = loanHdfc.employmentDetail.checkIncomeInvalid($('#txtIncome'), $('#spntxtIncome'));
            if (isCompInvalid || isExpInvalid || isEmiInvalid || isIncomeInvalid) {
                return false;
            }
            else {
                loanHdfc.eligibilityApiInput.MonthlyIncome = $('#txtIncome').val();
                loanHdfc.eligibilityApiInput.AnnualIncome = loanHdfc.eligibilityApiInput.MonthlyIncome * 12;
                loanHdfc.eligibilityApiInput.CustomerExp = $('#drpExp').val();
                var reason = '';
                if(loanHdfc.eligibilityApiInput.AnnualIncome < 300000)
                    reason ='Annual salary less than 3Lakh. ';
                if(loanHdfc.eligibilityApiInput.CustomerExp <= 0)
                    reason += 'Experience less than 1year.'

                loanHdfc.commonEvents.showNextEligibleScreen('empSubType', 'dob-iscust', reason);
                return true;
            }
        },
        validateSelfEmployed: function () {
            var isYearInvalid = loanHdfc.employmentDetail.checkDropDownInvalid($('#drpYear'), $('#spndrpYear'), 0);
            var isMonthInvalid = loanHdfc.employmentDetail.checkDropDownInvalid($('#drpMonth'), $('#spndrpMonth'), 0);
            var isProfessionInvalid = loanHdfc.employmentDetail.checkDropDownInvalid($('#drpProfession'), $('#spndrpProfession'), 0);
            var isProfitInvalid = loanHdfc.employmentDetail.checkIncomeInvalid($('#txtProfit'), $('#spntxtProfit'), 0);

            if (isYearInvalid || isMonthInvalid || isProfessionInvalid || isProfitInvalid) {
                return false;
            }
            else {
                loanHdfc.eligibilityApiInput.MonthlyIncome = $('#txtProfit').val();
                loanHdfc.eligibilityApiInput.AnnualIncome = loanHdfc.eligibilityApiInput.MonthlyIncome * 12;
                loanHdfc.commonEvents.showNextEligibleScreen('empSubType', 'dob-iscust', 'Annual salary less than 3Lakh.');
                return true;
            }
        },
        validateSelfEmployedProfession: function () {
            var isYearInvalid = loanHdfc.employmentDetail.checkDropDownInvalid($('#drpYearProf'), $('#spndrpYearProf'), 0);
            var isMonthInvalid = loanHdfc.employmentDetail.checkDropDownInvalid($('#drpMonthProf'), $('#spndrpMonthProf'), 0);
            var isApplicantInvalid = loanHdfc.employmentDetail.checkDropDownInvalid($('#drpApplicantProf'), $('#spndrpApplicantProf'), 0);
            var isProfitInvalid = loanHdfc.employmentDetail.checkIncomeInvalid($('#txtProfitProf'), $('#spntxtProfitProf'));

            if (isYearInvalid || isMonthInvalid || isApplicantInvalid || isProfitInvalid) {
                return false;
            }
            else {
                loanHdfc.eligibilityApiInput.MonthlyIncome = $('#txtProfitProf').val();
                loanHdfc.eligibilityApiInput.AnnualIncome = loanHdfc.eligibilityApiInput.MonthlyIncome * 12;
                loanHdfc.commonEvents.showNextEligibleScreen('empSubType', 'dob-iscust', 'Annual salary less than 3Lakh.');
                return true
            }
        },
        CloseCompanyPopup: function () {
            $(".companyDiv").hide();
            unlockPopup();
        },
        companyChanged: function (companyLi) {
            var selectedCompanyId = $(companyLi).val();
            var selectedCompanyName = $(companyLi).text();
            $('#drpCompany li').removeAttr('isselected');
            $(companyLi).attr('isSelected', true);
            loanHdfc.eligibilityApiInput.CompanyId = selectedCompanyId;
            $('#loanCompany').text(selectedCompanyName).attr('data-val', selectedCompanyName);
            loanHdfc.employmentDetail.CloseCompanyPopup();
        }
    },

    selectCar: {
        selCarRegEvents: function () {
            if (loanHdfc.isMobile) {
                $('#loanCompany').on('click', function () {
                    Common.utils.showLoading();
                    setTimeout(loanHdfc.selectCar.openCompanyPopup, 100);
                });

                $('.companyDiv .filterBackArrow').on('click', function () {
                    if (loanHdfc.companyId.val() != "") {
                        $('#loanCompany').text(loanHdfc.companyId.val()).attr('data-val', loanHdfc.companyId.val());
                        loanHdfc.employmentDetail.CloseCompanyPopup();
                        loanHdfc.companyId.val("").keydown().focus();
                    }
                    else
                        loanHdfc.employmentDetail.CloseCompanyPopup();
                });

                $('#loanCarMake').on('click', function () {
                    loanHdfc.selectCar.pushState(null, "select make", "make");
                    loanHdfc.selectCar.openMakePopup();
                });

                $('#loanCarModel').on('click', function () {
                    if (!loanHdfc.selectCar.checkMakeInvalid()) {
                        loanHdfc.selectCar.pushState(null, "select model", "model");
                        loanHdfc.selectCar.openModelPopup();
                    }
                });

                $('#loanCarVersion').on('click', function () {
                    if (!loanHdfc.selectCar.checkMakeInvalid()) {
                        loanHdfc.selectCar.openVersionPopup();
                        loanHdfc.selectCar.pushState(null, "select version", "version");
                    }
                });
            } else {
                $(document).on('change', '#drpMake', function () {
                    var optionSelected = $("option:selected", this);
                    loanHdfc.selectedMakeId = this.value;
                    if (loanHdfc.selectedMakeId != '') {
                        loanHdfc.apiInput.Car_Make = $.trim(optionSelected.text());
                        $("#model,#version").empty();
                        $("#model").append($("<option></option>").val(-1).html("Loading..."));
                        $("#version").append($("<option></option>").val('').html("Version"));
                        loanHdfc.apiCalls.processModels();
                    } else {
                        $('#model').val($("#model option:first").val()).attr('disabled', true);
                        $('#version').val($("#version option:first").val()).attr('disabled', true);
                    }
                });

                $(document).on('change', '#model', function () {
                    var optionSelected = $("option:selected", this);
                    loanHdfc.selectedModelId = this.value;
                    if (loanHdfc.selectedModelId != '') {
                        loanHdfc.apiInput.Car_Model = $.trim(optionSelected.text());
                        $('#version').empty();
                        $("#version").append($("<option></option>").val(-1).html("Loading..."));
                        loanHdfc.apiCalls.processVersions();
                    } else {
                        $('#version').val($("#version option:first").val()).attr('disabled', true);
                    }
                });

                $(document).on('change', '#version', function () {
                    var optionSelected = $("option:selected", this);
                    loanHdfc.selectedVersionId = this.value;
                    if (loanHdfc.selectedVersionId != '') {
                        loanHdfc.apiInput.VersionId = $.trim(loanHdfc.selectedVersionId);
                        loanHdfc.eligibilityApiInput.VersionId = $.trim(loanHdfc.selectedVersionId);
                        loanHdfc.selectCar.validateCar();
                    }
                });
            }
        },
        checkMakeInvalid: function () {
            if ($('#loanCarMake').attr('data-val') == "" || $('select#drpMake').val() == "") {
                $('#spnloanCarMake').show();
                return true;
            }
            else {
                $('#spnloanCarMake').hide();
                return false;
            }
        },
        checkModelInvalid: function () {
            if ($('#loanCarModel').attr('data-val') == "" || $('select#model').val() == "") {
                $('#spnloanCarModel').show();
                return true;
            }
            else {
                $('#spnloanCarModel').hide();
                return false;
            }
        },
        checkVersionInvalid: function () {
            if ($('#loanCarVersion').attr('data-val') == "" || $('select#version').val() == "" || loanHdfc.isVersionLoading == false) {
                $('#spnloanCarVersion').show();
                return true;
            }
            else {
                $('#spnloanCarVersion').hide();
                return false;
            }
        },
        validateCar: function () {
            var isMakeInvalid = loanHdfc.selectCar.checkMakeInvalid();
            var isModelInvalid = loanHdfc.selectCar.checkModelInvalid();
            var isVersionInvalid = loanHdfc.selectCar.checkVersionInvalid();
            if (isMakeInvalid || isModelInvalid || isVersionInvalid) {
                return false;
            }
            else {
                loanHdfc.commonEvents.pageShow('selectCar', 'bookingTime');
                return true;
            }
        },

        pushState: function (obj, title, hashStr) {
            window.location.hash = hashStr;
        },

        openCompanyPopup: function () {
            var currentPopup = $('.companyDiv');
            currentPopup.addClass("popup_layer").show().scrollTop(0);
            window.scrollTo(0, 0);
            lockPopup();
            var noFoundDiv = '<div class="noFound content-inner-block-10 text-red">If you dont find your company here, please type it and press enter.</div>';
            var crossBox = '.cross-box-wrap .cross-box';
            var list = $('#drpCompany');
            var selCrossBox = list.closest('.fixedSearchPopup').find(crossBox);
            loanHdfc.companyId.val('');
            loanHdfc.companyId.cw_fastFilter('#drpCompany', {
                callback: function (total) {
                    noFoundLen = list.siblings("div.noFound").length;
                    if (loanHdfc.companyId.val() != "") selCrossBox.show();
                    else selCrossBox.hide();
                    //no search found text
                    if (total == 0 && noFoundLen < 1) list.after(noFoundDiv).show();
                    else if (total > 0 && noFoundLen > 0) $('div.noFound').remove();
                }
            });
            loanHdfc.companyId.on('keydown', function (e) {
                if (e.keyCode == '13' && loanHdfc.companyId.val() != "") {
                    $('#loanCompany').text(loanHdfc.companyId.val()).attr('data-val', loanHdfc.companyId.val());
                    loanHdfc.employmentDetail.CloseCompanyPopup();
                    loanHdfc.companyId.val("").keydown().focus();
                }
            });
            loanHdfc.selectCar.pushState(null, "select Company", "company");
            Common.utils.hideLoading();
        },

        openMakePopup: function () {
            var currentPopup = $('.MakeDiv');
            loanHdfc.selectCar.showLoadingForDiv(currentPopup, currentPopup.prev());
            $('#txtMake').val('');
            loanHdfc.apiCalls.processMakes(currentPopup);
        },

        openModelPopup: function () {
            var currentPopup = $('.modelDiv');
            loanHdfc.selectCar.showLoadingForDiv(currentPopup, currentPopup.prev());
            $('#txtModel').val('');
            loanHdfc.apiCalls.processModels(currentPopup);
        },

        openVersionPopup: function () {
            var currentPopup = $('.versionDiv');
            loanHdfc.selectCar.showLoadingForDiv(currentPopup, currentPopup.prev());
            $('#txtVersion').val('');
            loanHdfc.apiCalls.processVersions(currentPopup);
        },

        makeChanged: function (makeLi) {
            loanHdfc.selectedMakeId = $(makeLi).val();
            loanHdfc.selectedMakeName = $(makeLi).text();
            loanHdfc.apiInput.Car_Make = $.trim(loanHdfc.selectedMakeName);
            var currentPopup = $(".modelDiv");
            loanHdfc.selectCar.showLoadingForDiv(currentPopup, currentPopup.prev());
            $('#txtModel').val('');
            loanHdfc.apiCalls.processModels(currentPopup);
        },

        modelChanged: function (modelLI) {
            loanHdfc.selectedModelId = $(modelLI).val();
            loanHdfc.selectedModelName = $(modelLI).text();
            loanHdfc.apiInput.Car_Model = $.trim(loanHdfc.selectedModelName);
            var currentPopup = $(".versionDiv");
            loanHdfc.selectCar.showLoadingForDiv(currentPopup, currentPopup.prev());
            $('#txtVersion').val('');
            loanHdfc.apiCalls.processVersions(currentPopup);
        },

        versionChanged: function (selectedVersion) {
            loanHdfc.selectedVersionId = $(selectedVersion).val();
            loanHdfc.selectedVersionName = $(selectedVersion).text();
            loanHdfc.apiInput.VersionId = loanHdfc.selectedVersionId;
            loanHdfc.eligibilityApiInput.VersionId = loanHdfc.selectedVersionId;

            $('.versionDiv').hide();
            unlockPopup();
            $('#loanCarMake').text(loanHdfc.selectedMakeName).attr('data-val', loanHdfc.selectedMakeName);
            $('#loanCarModel').text(loanHdfc.selectedModelName).attr('data-val', loanHdfc.selectedModelName);
            $('#loanCarVersion').text(loanHdfc.selectedVersionName).attr('data-val', loanHdfc.selectedVersionName);
            $('#errCar').html("").show();
            loanHdfc.selectCar.pushState(null, "", "");
        },

        hideLoadingForDiv: function (currentPopup) {
            if (currentPopup != "" && currentPopup !== undefined) {
                currentPopup.find("div.popup_content").show();
                currentPopup.find("div.m-loading-popup").hide();
            }
        },

        showLoadingForDiv: function (currentPopup, prevPopup) {
            if (prevPopup != null)
                prevPopup.hide();
            currentPopup.find("div.popup_content").hide();
            currentPopup.find("div.m-loading-popup").show();
            currentPopup.addClass("popup_layer").show().scrollTop(0);
            window.scrollTo(0, 0);
            lockPopup();
        },

        hidepopup: function (back, inputId) {
            var chkFlag = inputId;
            inputId = '#' + inputId;
            var divToShow = inputId == "#" && loanHdfc.isPrefilled ? "" : $(back).parent().parent().prev();
            var divToHide = $(back).parent().parent();
            divToHide.hide();
            unlockPopup();
            inputId == "#" && loanHdfc.isPrefilled ? loanHdfc.commonEvents.pageShow('versionDiv', 'selectCar') : divToShow.show();
            if ($('div.popup_layer').is(":visible"))
                lockPopup();
            if ($('div.noFound').length > 0) $('div.noFound').remove();
            divToHide.find('.cross-box-wrap .cross-box').hide();
        },

        CloseWindow: function () {
            $(".MakeDiv").hide();
            $(".modelDiv").hide();
            $(".versionDiv").hide();
            unlockPopup();
        }
    },
    submitAction: {
        source: "",
        getQuote: function () {
            loanHdfc.submitAction.getApiInput();
            loanHdfc.submitAction.source = (loanHdfc.platformId == "83" || loanHdfc.platformId == "74") ? loanHdfc.platformId : (loanHdfc.isMobile ? "43" : "1");
            $.when(loanHdfc.submitAction.getQuoteId()).done(function (data) {
                if (data != null)
                    loanHdfc.apiInput.FinanceLeadId = data.cromaNewLeadId;
            });
        },

        getQuoteId: function () {
            return Common.utils.ajaxCall({
                type: 'POST',
                url: '/api/finance/quote/',
                data: loanHdfc.apiInput,
                contentType: "application/x-www-form-urlencoded",
                dataType: 'Json',
                headers: { "clientId": "6", "sourceId": loanHdfc.submitAction.source }
            });
        },

        getApiInput: function () {
            loanHdfc.apiInput.YearsInEmp = $('#drpExp').val();
            loanHdfc.apiInput.Monthly_Income = loanHdfc.eligibilityApiInput.MonthlyIncome;
            loanHdfc.apiInput.First_Name = $('#txtFirstName').val();
            loanHdfc.apiInput.Last_Name = $('#txtLastName').val();
            loanHdfc.apiInput.Email = $('#txtEmail').val();
            loanHdfc.apiInput.Mobile = $('#txtMobile').val();
            loanHdfc.apiInput.Emi_Paid = $('#txtEmi').val();
            loanHdfc.apiInput.DateOfBirth = $("#txtDobdate").val();
            loanHdfc.apiInput.Res_address = loanHdfc.apiInput.Res_address_month + " " + loanHdfc.apiInput.Res_address_year;
            loanHdfc.apiInput.Res_address2 = ($('#drpYear').val() < "0" ? $('#drpYearProf').val() : $('#drpYear').val()) + "" + ($('#drpMonth').val() <= "0" ? $('#drpMonthProf').val() : $('#drpMonth').val());
            loanHdfc.apiInput.Resi_City_other = $('#drpProfession').val();
            loanHdfc.apiInput.Resi_City_other1 = $('#txtProfitProf').val() == "" ? $('#txtProfit').val() : $('#txtProfitProf').val();
            loanHdfc.apiInput.Designation = $('#drpApplicantProf').val();
            loanHdfc.apiInput.Resi_type = loanHdfc.apiInput.Resi_type == "Owned" ? "Permanent" : "Rented";
            var url = window.location.href;
            var utm = (url.indexOf("utm") < 0 || Common.utils.getValueFromQS('utm') == "") ? ((loanHdfc.platformId == "83" || loanHdfc.platformId == "74") ? "othersapp" : (loanHdfc.isMobile ? "othersmsite" : "othersdesktop")) : Common.utils.getValueFromQS('utm');
            utm = utm.indexOf("#") < 0 ? utm : utm.split("#")[0];
            loanHdfc.apiInput.UtmCode = utm;
            loanHdfc.apiInput.ExistingCust = loanHdfc.eligibilityApiInput.isExstingCust = "No";
            loanHdfc.apiInput.IncomeTypeId = loanHdfc.eligibilityApiInput.IncomeTypeId;
            loanHdfc.apiInput.ClientCustomerId = $('#txtCustId').val();
            loanHdfc.apiInput.Pan_No = $('#txtPanNo').val();
            return loanHdfc.apiInput;
        }
    },
    apiCalls: {
        getMakes: function () {
            return Common.utils.ajaxCall({
                type: 'GET',
                url: '/webapi/CarMakesData/GetCarMakes/?type=new',
                dataType: 'Json',
            });
        },
        getModels: function () {
            return Common.utils.ajaxCall({
                type: 'GET',
                url: '/webapi/CarModelData/GetCarModelsByType/?type=new&makeId=' + loanHdfc.selectedMakeId,
                dataType: 'Json',
            });
        },
        getVersions: function () {
            return Common.utils.ajaxCall({
                type: 'GET',
                url: '/webapi/CarVersionsData/GetCarVersions/?type=new&modelid=' + loanHdfc.selectedModelId,
                dataType: 'Json',
            });
        },
        processMakes: function (currentPopup, callback) {
            try {
                $.when(loanHdfc.apiCalls.getMakes()).done(function (data) {
                    var viewMakes = {
                        CarMakes: ko.observableArray(data)
                    };

                    ko.cleanNode(document.getElementById("drpMake"));
                    ko.applyBindings(viewMakes, document.getElementById("drpMake"));
                    if (loanHdfc.isMobile) {
                        loanHdfc.selectCar.hideLoadingForDiv(currentPopup);
                        $('#txtMake').cw_fastFilter('#drpMake');
                    }
                    if (callback)
                        callback(true);
                }).fail(function () {    //to handle api fail case
                    if (loanHdfc.isMobile)
                        loanHdfc.selectCar.hideLoadingForDiv(currentPopup);
                });
            } catch (e) { console.log(e) };
        },
        processModels: function (currentPopup, callback) {
            try {
                $.when(loanHdfc.apiCalls.getModels()).done(function (data) {
                    var viewModel = {
                        CarModels: ko.observableArray(data)
                    };

                    ko.cleanNode(document.getElementById("model"));
                    ko.applyBindings(viewModel, document.getElementById("model"));
                    if (loanHdfc.isMobile) {
                        loanHdfc.selectCar.hideLoadingForDiv(currentPopup);
                        $('#txtModel').cw_fastFilter('#model');
                    } else $('#model').attr('disabled', false);
                    if (callback)
                        callback(true);
                }).fail(function () {    //to handle api fail case
                    if (loanHdfc.isMobile)
                        loanHdfc.selectCar.hideLoadingForDiv(currentPopup);
                });
            } catch (e) { console.log(e) };
        },
        processVersions: function (currentPopup, callback) {
            loanHdfc.isVersionLoading = false;
            try {
                $.when(loanHdfc.apiCalls.getVersions()).done(function (data) {
                    loanHdfc.isVersionLoading = true;
                    var viewModel = {
                        CarVersions: ko.observableArray(data)
                    };
                    ko.cleanNode(document.getElementById("version"));
                    ko.applyBindings(viewModel, document.getElementById("version"));
                    if (loanHdfc.isMobile) {
                        loanHdfc.selectCar.hideLoadingForDiv(currentPopup);
                        $('#txtVersion').cw_fastFilter('#version');
                    } else $('#version').attr('disabled', false);
                    if (callback)
                        callback(true);
                }).fail(function () {    //to handle api fail case
                    if (loanHdfc.isMobile)
                        loanHdfc.selectCar.hideLoadingForDiv(currentPopup);
                });
            } catch (e) { console.log(e) };
        },
        getCarVersionData: function (versionId) {
            return Common.utils.ajaxCall({
                type: 'GET',
                url: '/webapi/CarVersionsData/GetCarDetailsByVersionId/?versionid=' + versionId,
                dataType: 'Json',
            });
        },
        getCarModelData: function (modelId) {
            return Common.utils.ajaxCall({
                type: 'GET',
                url: '/webapi/CarModelData/GetCarDetailsByModelId/?modelid=' + modelId,
                dataType: 'Json'
            });
        },
        getQueryStringData: function () {
            loanHdfc.queryString = "";
            loanHdfc.queryString += "&residenceTypeId=" + loanHdfc.eligibilityApiInput.ResidenceTypeId;
            loanHdfc.queryString += "&stabilityTime=" + loanHdfc.eligibilityApiInput.StabilityTime;
            loanHdfc.queryString += loanHdfc.eligibilityApiInput.IncomeTypeId >= 0 ? "&incometypeid=" + loanHdfc.eligibilityApiInput.IncomeTypeId : "";
            loanHdfc.queryString += loanHdfc.eligibilityApiInput.AnnualIncome >= 0 ? "&annualincome=" + loanHdfc.eligibilityApiInput.AnnualIncome : "";
            loanHdfc.queryString += loanHdfc.eligibilityApiInput.CustomerExp >= 0 ? "&CustomerExp=" + loanHdfc.eligibilityApiInput.CustomerExp : "";
            loanHdfc.queryString += loanHdfc.eligibilityApiInput.CompanyId >= 0 ? "&companyid=" + loanHdfc.eligibilityApiInput.CompanyId : "";
            loanHdfc.queryString += loanHdfc.eligibilityApiInput.dateOfBirth != undefined ? "&customerdob=" + loanHdfc.eligibilityApiInput.dateOfBirth : "";
            loanHdfc.queryString += loanHdfc.eligibilityApiInput.isExstingCust != undefined ? "&isexistingcustomer=" + loanHdfc.eligibilityApiInput.isExstingCust : "";
            loanHdfc.queryString += loanHdfc.eligibilityApiInput.VersionId > 0 ? "&versionId=" + loanHdfc.eligibilityApiInput.VersionId : "";
            loanHdfc.queryString += loanHdfc.eligibilityApiInput.CityId > 0 ? "&cityId=" + loanHdfc.eligibilityApiInput.CityId : "";
            loanHdfc.queryString += loanHdfc.eligibilityApiInput.tenor > 0 ? "&tenor=" + loanHdfc.eligibilityApiInput.tenor : "";
            loanHdfc.queryString += "&financeleadid=" + -1;
        },

        getLoanParamData: function () {
            try {
                var title = "EMI Quote For " + loanHdfc.apiInput.Car_Make + " " + loanHdfc.apiInput.Car_Model + " In " + loanHdfc.apiInput.Res_City + " From HDFC Bank";
                $('#car-title').text(title);
                loanHdfc.apiInput.IsPermitted = true;
                loanHdfc.thankYouPage.bindLoanData(true);
            } catch (e) { console.log(e) };
        },

        isEligible: function () {
            loanHdfc.apiCalls.getQueryStringData();

            return Common.utils.ajaxCall({
                type: 'GET',
                url: '/api/finance/eligibility/?' + loanHdfc.queryString,
                dataType: 'Json',
                async: false,
            });
        },

    },
    thankYouPage: {
        bindLoanData: function (response) {
            if (response) {
                loanHdfc.thankYouPage.showThankYouPage("success");
            }
            else
                loanHdfc.thankYouPage.showThankYouPage("fail");
        },
        showThankYouPage: function (status) {
            $('#loanQuote').html('');
            Common.utils.hideLoading();
            $('#back, #next').addClass('hide');
            if (loanHdfc.isMobile)
                $('#hideBackNext').addClass('hide');
            loanHdfc.selectCar.pushState(null, "", "thankyou");
            var visiblePage = $('div.page:visible');
            var currentPageId = visiblePage.attr('id');
            if (currentPageId == 'dob-iscust')
                $('#userDetails').removeClass('hide');
            if (status == "fail") {
                loanHdfc.commonEvents.pageShow(currentPageId, 'api-failure-thank-you-popup');
                Common.utils.trackAction('CWNonInteractive', "Hdfc_finance", "Eligibility_fail", "Final_eligibility_fail");
            }
            else
                loanHdfc.commonEvents.pageShow(currentPageId, 'final-submit-thank-you');
        },        
        koViewModel: {
            emi: ko.observable(),
            LoanParams: ko.observableArray([]),
            showProcessingFess: ko.observable(),
            DownPayment: ko.observable(),
            roi: ko.observable(),
        },
    },
}


$(document).ready(function () {
    loanHdfc.pageLoad();
});