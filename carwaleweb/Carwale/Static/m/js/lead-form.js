window.popup_promise = window.popup_promise || $.Deferred();
$.when(window.popup_promise).done(function () {
    _campaignViewModel = new campaignViewModel();
    _sellerViewModel = new sellerViewModel();
    emiassist.intialize();
    TestDrive.registerEvents();
    var querystring = window.location.search;
    if (querystring.indexOf("=lead") > -1) {

        //extracting clicksource
        var clicksource;
        if (querystring.indexOf("cs=")) {
            clicksource = Number(querystring.split("cs=")[1].split("&", 1)[0]);
        }

        //extracting versionId
        var versionId = 0;
        if (querystring.indexOf("vid=") > -1) {
            versionId = querystring.split("vid=")[1].split("&", 1)[0];
        }

        window.history.replaceState('', document.title, window.location.href.split('?')[0]);

        switch (clicksource) {
            //for get emi assistence link
            case 3:
            case 139:
            case 305: var emiLink = $("#divEmiAssistance").length > 0 ? "#divEmiAssistance" : "#btnGetOffers";
                emiassist.openEmiPopup(emiLink);
                break;
                //for authorized dealer link
            case 163: emiassist.openEmiPopup("#authDealer");
                break;
                //for dealer locator slug
            case 182: emiassist.openEmiPopup("#btnDealerLocatorSlug");
                break;
            case 184:
            case 185: var genericSlug = $("#btnGenericSlug").length > 0 ? "#btnGenericSlug" : "#btnGetOffers";
                emiassist.openEmiPopup(genericSlug);
                break;
                //for version list get offers link
            case 253: var versionLink = versionId > 0 ? ".getOffersVersionLink[data-versionid='" + versionId + "']" : ".getOffersVersionLink";
                emiassist.openEmiPopup(versionLink);
                break;
            case 303: emiassist.openEmiPopup("#pic-divEmiAssistance");
                break;
                // for floating slug
            case 204:
            case 206: emiassist.openEmiPopup("#btnGetOffers");
                break;
        }
    }
});

var EMIAssistance = function () {
    var _self = this, emiClickSourceId, citySelected;
    _self.crossSellImageResolution = '310x174';
    var citySlider = new Object();
    var _leadFormBrowserNavigator;
    var IsCityConfirmed = false;
    _self.screen = {
        CustomerInfo: 1,
        MLA: 2,
        Recommendation: 3,
        ThankYouWithEmail: 4,
        ThankYouWithImage: 5,
        CustomerInfoWithMLA: 6
    }

    _self.intialize = function () {
        LeadFormState.FlowType = LeadForm.FlowType();
        LeadFormState.FlowText = LeadForm.FlowText();

        if (platform == 43) {
            $(document).on('focus', '.location-popup-container input', function () {
                $('.location-popup-container').animate({
                    scrollTop: $(this).offset().top
                }, 1000);
            });
        }
        LeadFormReady();
        _self.registerEvent();

        _leadFormBrowserNavigator = new LeadFormBrowserNavigator({
            onBrowserBack: function () {
                if (platform != 1) {
                    $("#emiPopup .pq-popup-close__js").trigger("click");
                }
            }
        });

        if (window.campaignId > 0) {
            saveDealerCookie(window.campaignId, LocationInfo.cityId(), LocationInfo.zoneId(), CarDetails.carModelId);
        }
    },

    _self.getDealerObject = function (clicksource) {
        var data = {};
        data.MakeId = CarDetails.carMakeId;
        data.MakeName = CarDetails.carMakeName;
        data.ModelId = CarDetails.carModelId;
        data.ModelName = CarDetails.carModelName
        data.CityId = LocationInfo.cityId(clicksource);
        data.ZoneId = LocationInfo.zoneId(clicksource);
        data.AreaId = LocationInfo.areaId(clicksource);
        data.DealerLeadBusinessType = typeof (sponsorDlrBusType) != "undefined" ? sponsorDlrBusType : "";
        data.DealerName = typeof (sponsorDlrName) != "undefined" ? sponsorDlrName : "";
        data.DealerId = typeof (campaignId) != "undefined" ? campaignId : 0;
        data.DealerAutoAssignPanel = typeof (sponsorDlrLeadPanel) != "undefined" ? sponsorDlrLeadPanel : "";
        data.ShowEmail = typeof (showEmail) != "undefined" ? showEmail.toString() == "true" ? true : false : false;
        data.DealerAdminId = typeof (dealerAdminId) != "undefined" ? dealerAdminId : 0; // Identifies dealers group id
        data.DealerMutualLead = typeof (sponsoredDlrMutualLeads) != "undefined" ? sponsoredDlrMutualLeads : false;
        data.GAActionDifferential = "";
        data.PopupID = "#emiPopup"; // TBD to be removed
        data.ImagePath = (typeof CarDetails.hostURL != "undefined" && typeof CarDetails.originalImgPath != "undefined") ? (CarDetails.hostURL + _self.crossSellImageResolution + CarDetails.originalImgPath) : "";
        data.clicksource = clicksource;
        if (typeof clicksource != "undefined") {
            data.VersionId = CarInfo.versionId(clicksource);
            data.LeadClickSourceId = LeadSource.leadclicksourceId(clicksource);
            data.AdType = GATracking.adType(clicksource);
            data.GACat = GATracking.gaCat(clicksource);
        }
        return data;
    };

    _self.getDealerLocatorObject = function (clicksource) {
        var data = {};
        data.MakeId = CarDetails.carMakeId;
        data.MakeName = CarDetails.carMakeName;
        data.ModelId = CarDetails.carModelId;
        data.ModelName = CarDetails.carModelName
        data.CityId = LocationInfo.cityId();
        data.ZoneId = LocationInfo.zoneId();
        data.AreaId = LocationInfo.areaId();
        data.GAActionDifferential = "";
        data.PopupID = "#emiPopup"; // TBD to be removed
        data.ImagePath = (typeof CarDetails.hostURL != "undefined" && typeof CarDetails.originalImgPath != "undefined") ? (CarDetails.hostURL + _self.crossSellImageResolution + CarDetails.originalImgPath) : "";
        data.clicksource = clicksource;
        if (typeof clicksource != "undefined") {
            data.DealerLeadBusinessType = $(clicksource).attr('dealerleadbusinesstype');
            data.DealerName = $(clicksource).attr('dealername');
            data.DealerId = $(clicksource).attr('campaignid');
            data.DealerAutoAssignPanel = $(clicksource).attr('leadpanel');
            data.ShowEmail = $(clicksource).attr('showemail') == "true" ? true : false;
            data.DealerAdminId = 0;
            data.DealerMutualLead = false;
            data.VersionId = CarInfo.versionId(clicksource);
            data.LeadClickSourceId = LeadSource.leadclicksourceId(clicksource);
            data.AdType = GATracking.adType(clicksource);
            data.GACat = GATracking.gaCat(clicksource);
        }

        return data;
    };

    _self.openEmiPopup = function (clicksource) {
        if ($(clicksource).length > 0) {
            _self.bindControlEvents();
            Common.utils.lockPopup();
            var isDealerCampaignExist = CampaignAdditionalInfo.isDealerCampaignExist(clicksource);
            var isDealerLocatorAvailable = CampaignAdditionalInfo.isDealerLocatorAvailable(clicksource);
            //TBD add inquirysource id in attribute for all cases
            inquirySourceId = LeadSource.inquirySourceId(clicksource);

            if (isDealerCampaignExist === "true") {
                _self.openDealerPopup(clicksource, _self.getDealerObject(clicksource));
            } else if (isDealerLocatorAvailable === "true") {
                _self.openDealerPopup(clicksource, _self.getDealerLocatorObject(clicksource));
            } else if (IsCityConfirmed) {
                if (typeof (CarDetails) != "undefined" && typeof (CampaignDict[CarDetails.carModelId]) != "undefined") {
                    var locationObj = locationPluginMVPObj.getLocation();
                    var cityId = locationObj.cityId;
                    _self.BindCampaign(CampaignDict[CarDetails.carModelId], cityId, clicksource)
                }
                else {
                    _self.OpenLeadForm();
                }

            } else {
                _self.openAskForCityPopup(clicksource);
            }
        }
    };

    _self.openDealerPopup = function (clicksource, data) {
        _self.hideLocationSliderPopup();
        Validate.setSelectors($("#divCustInfo"), $(".form-input"));
        $('.form-container').find('#userName').focus();
        $("#divCustInfo").removeClass('hide');
        if (window.isLeadWebview) {
            data.cwCat = PageInfo.pageName;
            data.cwAct = "AppLinkClick";
            data.cwLabel = "make:" + CarDetails.carMakeName + "|model:" + CarDetails.carModelName + "|city:" + window.webviewCityName;
            data.predictionScore = webviewPredictionScore;
            data.predictionLabel = webviewPredictionLabel;
        }
        else {
            var source = $(clicksource);
            data.cwCat = source.attr("data-cwtccat");
            data.cwAct = source.attr("data-cwtcact");
            data.cwLabel = source.attr("data-cwtclbl");
            if (typeof data.predictionScore == 'undefined' && typeof data.predictionLabel == 'undefined') {
                data.predictionScore = source.attr("data-predictionScore");
                data.predictionLabel = source.attr("data-predictionLabel");
            }

            data.DealerLocatorName = source.attr("dealerLocatorName");
        }
        _self.showDealerPopup();
        initDealerPopup(data);
        dataLayer.push({ event: "DealerLeadPopUpBehaviour", cat: data.GACat, act: "FormOpen", lab: data.ModelName });

        saveDealerCookie(data.DealerId, data.CityId, data.ZoneId, data.ModelId);
    }
    _self.openAskForCityPopup = function (clicksource) {
        _self.emiClickSourceId = clicksource;
        _self.initLocationPluginForLeadForm();
        _self.OpenLocationSliderForm();
        if (platform != 1) {
            _leadFormBrowserNavigator.open();
        }
    }
    _self.initLocationPluginForLeadForm = function () {
        var div = $('#citySlider');
        locationPluginMVPObj = new LocationSearch((div), {
            showCityPopup: false,
            cityClassName: 'padding-right30 ui-autocomplete-input blur city-search',
            areaClassName: 'margin-top30 ui-autocomplete-input blur',
            resultCount: typeof (platform) != "undefined" && platform == 43 ? 5 : 8,
        });
        var sliderLocationObj = { cityId: LocationInfo.cityId(), cityName: LocationInfo.cityName() };
        locationPluginMVPObj.prefill(sliderLocationObj, div);
    }
    _self.OpenLocationSliderForm = function () {
        $(".blackOut-window").show();
        $("#btnsliderConfirmCity").val("Confirm City");
        $("#dealerPopUpWrapper").addClass("location-popup--active");
    }

    _self.addRefreshAttribute = function () {
        $("#pq-popup-close-spn").attr("IsRefresh", true);
        $('#reqAsstncBtn').attr("IsRefresh", true);
    }

    _self.prepopulateCity = function () {
        if ($.cookie('_CustCityMaster') != "Select City" && Number($.cookie('_CustCityIdMaster')) > 0) {
            var name = $.cookie('_CustCityMaster');
            var id = $.cookie('_CustCityIdMaster');
            $("#citySlider").val(name);
            citySlider.Id = id;
            citySlider.Name = name;
        }
    }

    _self.showDealerPopup = function () {
        if (window.isWebview) {
            $(".privacy-policy").hide();
        }
        if (navigator.userAgent.match(/iPhone/)) {
            $('body').addClass('iphone-optional');
            if (window.isWebview) {
                $('.visitor-agreement').attr("href", "ios:openExternalLink?https://www.carwale.com/visitoragreement.aspx")
            }
        }
        $("#lead-popup").addClass('lead-popup--visible');
        if (platform != 1) {
            _leadFormBrowserNavigator.open();
        }
    }

    _self.closeDealerPopup = function () {

        var gaCategory = (typeof (isVersionPage) != "undefined" && isVersionPage == "true") ? "EmiAssistVersionPage" : "EmiAssistModelPage";
        var gaLabel = (typeof (isVersionPage) != "undefined" && isVersionPage == "true") ? "Version" : "Model";

        if ($('#divMassage').is(':visible')) {
            dataLayer.push({
                event: 'DealerLeadPopUpBehaviour', cat: gaCategory, act: "PrematureFormClosed", lab: "NoDealer_" + gaLabel
            });
        }
        if (window.isLeadWebview) {
            WebviewClosed();
        }

        $("#lead-popup").removeClass('lead-popup--visible');

        Common.utils.unlockPopup();
    }

    _self.showHideMatchSliderError = function (error) {
        if (error) {
            $('.global-city-error-icon').css('display', 'inline-block');
            $('.city-error-msg').removeClass('hide');
            $('#citySlider').addClass('border-red')
        }
        else {
            $('.global-city-error-icon').css('display', 'none');
            $('.city-error-msg').addClass('hide');
            $('#citySlider').removeClass('border-red');
        }
    }

    _self.closeLeadForm = function () {
        var isFormVisible = $('#divCustInfo').is(":visible");
        LeadFormState = {};

        if (isFormVisible) {
            hideCustomerFormErrors();

            $("#lead-popup").removeClass('lead-popup--visible');
            dataLayer.push({
                event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: 'PrematureFormClosed', lab: campaignData.ModelName
            });
        }
        else {
            _self.closeDealerPopup();
        }
        if (window.isLeadWebview) {
            WebviewClosed();
        }

        if ($('#pq-popup-close-spn').attr("IsRefresh") == "true") {
            var masterCityId = Number($.cookie('_CustCityIdMaster'));
            if (masterCityId > 0 && masterCityId != LocationInfo.cityId())
                $(document).trigger("mastercitychange", [$.cookie('_CustCityMaster'), $.cookie('_CustCityIdMaster')]);
            else
                _self.reload($('#pq-popup-close-spn'), "IsRefresh");
        }

        Common.utils.unlockPopup(true);

    };

    _self.bindControlEvents = function () {
        $(".pq-popup-close__js").unbind("click");
        $(document).on("click", ".pq-popup-close__js,.blackOut-window", function (event) {

            if ($("#reqSellerAssist").is(":visible")) {
                var totalDealers = $("#recommedationSellerList").find("li").length;
                if (typeof (event.originalEvent) != "undefined") {
                    cwTracking.trackAction('CWInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_' + LeadFormState.FlowText + 'multiple_dealers_closed', totalDealers, 'NA');
                }
                else {
                    cwTracking.trackAction('CWInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_' + LeadFormState.FlowText + 'multiple_dealers_browser_back_clicked', totalDealers, 'NA');
                }
            }

            if (platform != 1) {
                _leadFormBrowserNavigator.close();
            }
            _self.hideLocationSliderPopup(true);
            _self.closeLeadForm();
        });

        if (!window.isKoBinded) {
            if (document.getElementById("reqSellerAssist") != null) {
                applyKOBindingSellerRecommendation();
            }

            if (document.getElementById("reqAssist") != null) {
                applyKOBindingRecommendation();
            }
            isKoBinded = true;
        }

    }

    _self.confirmCity = function () {
        if (locationPluginMVPObj.validateLocation()) {
            if (platform == 43)
                _self.addRefreshAttribute();
            try {
                if (PageInfo.isVersion)
                    cwTracking.trackAction('CWNonInteractive', 'VersionPage_Lead_CTA_' + platformTrackingText, 'City_Filled_In_' + (platform == 1 ? 'Slider' : 'Popup'), $.cookie('_CustCityMaster') + "," + CarDetails.carModelName);
                else if (PageInfo.isModelPage)
                    cwTracking.trackAction('CWNonInteractive', 'ModelPage_Lead_CTA_' + platformTrackingText, 'City_Filled_In_' + (platform == 1 ? 'Slider' : 'Popup'), $.cookie('_CustCityMaster') + "," + CarDetails.carModelName);
                else if (Page.isModelCity)
                    cwTracking.trackAction('CWNonInteractive', 'ModelCityPage_Lead_CTA_' + platformTrackingText, 'City_Filled_In_' + (platform == 1 ? 'Slider' : 'Popup'), $.cookie('_CustCityMaster') + "," + CarDetails.carModelName);

                cwTracking.trackAction('CWNonInteractive', platformTrackingText + 'UserAction_GlobalCitySet', 'ModelVersionSliderCitySet', $.cookie('_CustCityMaster') + "," + USERIP);
            } catch (e) {
                console.log(e);
            }
            IsCityConfirmed = true;
            _self.OpenLeadForm();
        }
    }

    _self.OpenLeadForm = function () {
        var locationObj = locationPluginMVPObj.getLocation();
        var cityId = locationObj.cityId;
        var zoneId = typeof (locationObj.zoneId) != "undefined" ? locationObj.zoneId : 0;
        if (window.isLeadWebview) {
            setWebviewLocation(locationObj);
            SendDataToApp();
        }
        LeadFormState.FlowType = LeadForm.FlowType();
        _self.GetCampaigns(cityId, zoneId);
        LeadFormState.FlowText = LeadForm.FlowText();
    }

    _self.GetCampaigns = function (cityId, zoneId) {
        var objCampaign = new Campaign();
        objCampaign.GetCampaigns(CarDetails.carModelId, cityId, zoneId, _self.emiClickSourceId, _self.BindCampaign);
    }

    _self.BindCampaign = function (dealerCampaign, cityId, clickSource) {
        if (typeof (dealerCampaign) != "undefined" && dealerCampaign != null) {
            var data = _self.getDealerObject(clickSource);
            //TBD to be removed
            data.DealerLeadBusinessType = dealerCampaign.type;
            data.DealerName = dealerCampaign.contactName;
            data.DealerId = dealerCampaign.id;
            data.DealerAutoAssignPanel = dealerCampaign.leadPanel;
            data.ShowEmail = dealerCampaign.isEmailRequired;
            data.CityId = LocationInfo.cityId(clickSource);
            data.AreaId = LocationInfo.areaId(clickSource);
            data.predictionScore = dealerCampaign.predictionData.score;
            data.predictionLabel = dealerCampaign.predictionData.label;
            sponsorDlrBusType = dealerCampaign.type;
            sponsorDlrName = dealerCampaign.contactName;
            campaignId = dealerCampaign.id;
            sponsorDlrLeadPanel = dealerCampaign.leadPanel;
            showEmail = dealerCampaign.isEmailRequired; // TBD : this is gonna be used or not
            data.DealerAdminId = typeof (dealerCampaign.dealerAdminId) != "undefined" ? dealerCampaign.dealerAdminId : 0;
            data.DealerMutualLead = dealerCampaign.mutualLeads;
            var emiCallSlug = $("#emiCallSlug");
            var callslug = $("#callslug");
            if (platform == 43 && !isEligibleForORP)
                emiCallSlug.show();
            callslug.find('#divPrice').attr('class', 'btn btn-orange btn-sm linkButtonBigBlueNew leftfloat');
            emiCallSlug.attr("href", "tel:" + dealerCampaign.contactNumber);
            callslug.show();
            callslug.click(function () { dataLayer.push({ event: "CallSlugTracking", cat: "call-slug-click", act: "modelpage", lab: CarDetails.carMakeName + "_" + CarDetails.carModelName + "_" + sponsorDlrName }); });

            _self.openDealerPopup(clickSource, data);
            try {
                if (PageInfo.isVersion)
                    cwTracking.trackAction('CWNonInteractive', 'VersionPage_Lead_CTA_' + platformTrackingText, 'Lead_Form_Shown_After_City_Filled', $.cookie('_CustCityMaster') + "," + CarDetails.carModelName);
                else if (PageInfo.isModelPage)
                    cwTracking.trackAction('CWNonInteractive', 'ModelPage_Lead_CTA_' + platformTrackingText, 'Lead_Form_Shown_After_City_Filled', $.cookie('_CustCityMaster') + "," + CarDetails.carModelName);
                else if (PageInfo.isModelCity)
                    cwTracking.trackAction('CWNonInteractive', 'ModelCityPage_Lead_CTA_' + platformTrackingText, 'Lead_Form_Shown_After_City_Filled', $.cookie('_CustCityMaster') + "," + CarDetails.carModelName);
            } catch (e) {
                console.log(e);
            }
        } else {
            //_self.BindBazarLink(cityId);           //commented bcoz we have to revert it again for hdfc
            _self.showNoDealerPopup();
            try {
                var page = $("#divEmiAssistance").attr('page');
                if (PageInfo.isVersion)
                    cwTracking.trackAction('CWNonInteractive', 'VersionPage_Lead_CTA_' + platformTrackingText, 'Default_Content_Shown_After_City_Filled', $.cookie('_CustCityMaster') + "," + CarDetails.carModelName);
                else if (PageInfo.isModelPage)
                    cwTracking.trackAction('CWNonInteractive', 'ModelPage_Lead_CTA_' + platformTrackingText, 'Default_Content_Shown_After_City_Filled', $.cookie('_CustCityMaster') + "," + CarDetails.carModelName)
                else if (PageInfo.isModelCity)
                    cwTracking.trackAction('CWNonInteractive', 'ModelCityPage_Lead_CTA_' + platformTrackingText, 'Default_Content_Shown_After_City_Filled', $.cookie('_CustCityMaster') + "," + CarDetails.carModelName)
            } catch (e) {
                console.log(e);
            }
        }
    }

    _self.showNoDealerPopup = function () {
        $("#divMassage").removeClass('hide');
        _self.showDealerPopup();
        $("#divCustInfo,#divEmiAssistance,#reqSellerAssist,.content-wrapper").hide();
    }

    _self.ProcessDone = function () {
        $('#postsubmitmsg').addClass("hide");
        if ($("#emailAssistOptional").val() == "") {
            _self.closeDealerPopup();
        }
        else if (validateOptionalEmail(".email-screen")) {

            if (processPQLead(false)) {
                _self.closeDealerPopup();
            }
        }
        if ($('#reqAsstncBtn').attr("IsRefresh") == "true") {
            _self.reload($('#reqAsstncBtn'), "IsRefresh");
        }
    }

    _self.reload = function (id, attr) {
        id.removeAttr(attr);
        location.reload();
    }

    _self.hideLocationSliderPopup = function (noBrowserBack) {
        if (platform != 1 && noBrowserBack) {
            _leadFormBrowserNavigator.close();
        }
        $("#dealerPopUpWrapper").removeClass("location-popup--active");
    }

    //TBD bankbazar finances link to be removed
    /*_self.BindBazarLink = function () {
        if ((typeof (isVersionPage) != "undefined" && isVersionPage == "true")) {
            dataLayer.push({ event: 'BBLink_finance', cat: 'BBLinkImpressions_mobile', act: 'finance_EMIassistancelink_variantpage', lab: CarDetails.carMakeName + " " + CarDetails.carModelName + " " + defaultVerName + " " + $.cookie('_CustCityMaster') });
            $('#BtnBBLnkBtn').on('click', function () {
                dataLayer.push({ event: 'BBLink_finance', cat: 'BBLinkClicks_mobile', act: 'finance_EMIassistancelink_variantpage', lab: CarDetails.carMakeName + " " + CarDetails.carModelName + " " + defaultVerName + " " + $.cookie('_CustCityMaster') });
                window.open("https://www.bankbazaar.com/car-loan.html?WT.mc_id=bb01|CL|EMIassistance_variant_mobile&utm_source=bb01&utm_medium=display&utm_campaign=bb01&variant=slide&variantOptions=mobileRequired", '_blank');
            });
        }
        else {
            dataLayer.push({ event: 'BBLink_finance', cat: 'BBLinkImpressions_mobile', act: 'finance_EMIassistancelink_modelpage', lab: CarDetails.carMakeName + " " + CarDetails.carModelName + " " + $.cookie('_CustCityMaster') });
            $('#BtnBBLnkBtn').on('click', function () {
                dataLayer.push({ event: 'BBLink_finance', cat: 'BBLinkClicks_mobile', act: 'finance_EMIassistancelink_modelpage', lab: CarDetails.carMakeName + " " + CarDetails.carModelName + " " + $.cookie('_CustCityMaster') });
                window.open("https://www.bankbazaar.com/car-loan.html?WT.mc_id=bb01|CL|EMIassistance_model_mobile&utm_source=bb01&utm_medium=display&utm_campaign=bb01&variant=slide&variantOptions=mobileRequired", '_blank');
            });
        }
        $("#divBankBazarLink").removeClass('hide');
        $("#divMassage,#divDealerLead,#divDealer,#divCustInfo,#divCity,#divEmiAssistance").addClass('hide');
    }*/
    _self.registerEvent = function () {
        $('#divEmiAssistance,#pic-divEmiAssistance,#getDealerOffer,.getLeadAssistance,#btnGetEMIAssistanceOnTestDrive,#btnGetEMIAssistance,#toolTipDealerAssistance,.getOffersLink,.btn-requestcallback,#btnDealerOffer,.campaignLinkCTA,.getCCLeadAssistance,#dlp-cw-navigate,#btnGenericSlug,#btnDealerLocatorSlug,.getOffersVersionLink,#btnGetOffers,.dealer-cta__btn,.price-breakup__offers,.crossSellCTA,.link-campaign-offers-cta,.dealer-assist-link,.getPriceSlug,#authDealer,#toolTipDealerAssistance').css('cursor', 'pointer');
        $(document).on('click', '#divEmiAssistance,#pic-divEmiAssistance,#getDealerOffer,.getLeadAssistance,#btnGetEMIAssistanceOnTestDrive,#btnGetEMIAssistance,#toolTipDealerAssistance,.getOffersLink,.btn-requestcallback,#btnDealerOffer,.campaignLinkCTA,.getCCLeadAssistance,#dlp-cw-navigate,#btnGenericSlug,#btnDealerLocatorSlug,.getOffersVersionLink,#btnGetOffers,.dealer-cta__btn,.price-breakup__offers,.crossSellCTA,.link-campaign-offers-cta,.dealer-assist-link,.getPriceSlug,#authDealer,#toolTipDealerAssistance', function () {
            var pageName = $(this).attr("page");
            if (pageName && (pageName.toLowerCase() == "comparepage" || pageName.toLowerCase() == "quotationpage")) {
                var self = $(this);
                CarDetails = self.data("cardetails");
                sponsorDlrBusType = "";
                sponsorDlrName = self.data("sponsordlrname");
                sponsoredDlrMutualLeads = typeof (self.data("mutualleads")) != "undefined" ? (self.data("mutualleads").toString().toLowerCase() == "true" ? true : false) : false;
                campaignId = self.data("campaignid");
                sponsorDlrLeadPanel = self.data("leadpanel");
                showEmail = typeof (self.data("showemail")) != "undefined" ? (self.data("showemail").toString().toLowerCase() == "true" ? true : false) : false;
                newFlag = true;
                futuristic = false;
                inquirySourceId = self.data("inquirysourceid");
                dealerAdminId = typeof (self.data("dealeradminid")) != "undefined" ? self.data("dealeradminid") : 0;  // Identifies dealers group id
            }
            _self.openEmiPopup(this);
        });

        $(document).on('click', '#closeDealerPopup', function () {

            _self.hideLocationSliderPopup();
            Common.utils.unlockPopup();
            cwTracking.trackAction('DealerLeadPopUpBehaviour', 'Recommended Cars with Tie Up - ' + platformTrackingText, 'Close Button Click', 3);
        });

        $(document).on('click', '#btnSubmit', function () {
            _self.submitClick();
        });

        $(document).on('click', '#btnDone', function () {
            _self.btnDoneClick(this);
        });

        $(document).on('click', '#btnsliderConfirmCity', function () {
            _self.confirmCity();
        });
    };

    _self.submitClick = function () {
        switch ($('#reqAsstncBtn').prop('screenId')) {
            case emiassist.screen.CustomerInfo: processPQLead(); break;
            case emiassist.screen.MLA: reqSellerAsstncFun(); break;
            case emiassist.screen.Recommendation: reqAsstncFun(); break;
            case emiassist.screen.ThankYouWithEmail: _self.ProcessDone(); break;
            case emiassist.screen.CustomerInfoWithMLA: processPQLead(); reqSellerAsstncFun(); break;
        }
    };

    _self.btnDoneClick = function () {
        _self.ProcessDone();
    };

    _self.submitBtnFixedFloating = function () {
        var btnWrapper = $('#reqAsstncBtn').parent();
        btnWrapper.removeClass('other-dealers-section suggested-cars-section');
        switch ($('#reqAsstncBtn').prop('screenId')) {
            case (emiassist.screen.MLA || emiassist.screen.CustomerInfoWithMLA): btnWrapper.addClass('other-dealers-section'); break;
            case emiassist.screen.Recommendation: btnWrapper.addClass('suggested-cars-section'); break;
        }
    }
};

LeadAssignedDealerId = -1;

var Campaign = function () {
    var _campaign = this;

    _campaign.GetCampaigns = function (modelId, cityId, zoneId, clickSource, callback) {
        var apiUrl = "/api/campaign/random/?modelId=" + modelId + "&cityid=" + cityId + "&platformid=" + platform + "&zoneId=" + (zoneId > 0 ? zoneId : 0);
        $.ajax({
            type: "GET",
            url: apiUrl,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                CampaignDict[modelId] = response;
                callback(response, cityId, clickSource);
            },
            error: function () { callback(null, cityId, clickSource); }
        });
    };

    _campaign.predictionScore = 0;
    _campaign.predictionLabel = "";
};

var OpenLeadFormPopup = function (ref, recoInquirySourceId) {
    emiassist.openEmiPopup(ref);
    var recoLeadClickSource = $(ref).attr("recoleadsource");
    if (typeof recoLeadClickSource === 'undefined') {
        //394 is common recommendation lead source for msite
        recoLeadClickSource = 394;
    }
}

function bindGenericDealerSlug(btnclickObj) {
    var genericslug = $(btnclickObj);
    if (typeof genericslug.attr("data-predictionLabel") == 'undefined' || genericslug.attr("data-predictionLabel") == '') {
        genericslug.attr("data-predictionLabel", typeof predictionLabel == 'undefined' ? '' : predictionLabel);
        genericslug.attr("data-predictionScore", typeof predictionScore == 'undefined' ? 0 : predictionScore);
    }
    emiassist.openEmiPopup(btnclickObj);
}

function btnState(node) {
    var parentDiv = node.parent().parent();
    var isValid = node.find('.recommended-checkbox').children().hasClass('checked');
    if (parentDiv.find('.unchecked__js').hasClass('checked')) {
        parentDiv.find('.multipleDealers').removeAttr('disabled');
    }
    else {
        parentDiv.find('.multipleDealers').attr('disabled', 'true');
    }
}

function removeSelection(node) {
    var parentdiv = node.parent().parent()
    var toggleTitle = parentdiv.find('.toggle-selection-txt');
    var element = parentdiv.find('.toggle-selection-view');
    if (parentdiv.find('.recommended-checkbox').children().length == parentdiv.find('.checked').length) {
        element.toggleClass('select-all remove-all');
        toggleTitle.text(toggleTitle.text() === "Select All" ? "Remove All" : "Select All")
    }
    else if (parentdiv.find('.recommended-checkbox').children().length != parentdiv.find('.checked').length && element.hasClass('remove-all')) {
        element.toggleClass('select-all remove-all');
        toggleTitle.text(toggleTitle.text() === "Select All" ? "Remove All" : "Select All")
    }
}


// Dealer-popup js code starts
//global variable
var AdClickedFromSponsoredCar = false;
var OriginalEncryptLeadId = "";
var campaignData = {};
campaignData.PopupID = "";

var TestDrive = {
    checkBox: function () { return $('.chkBoxBookTestDrive__js') },
    disclaimer: function () { return $('.testDriveDisclaimer__js') },
    registerEvents: function () {
        $(document).off('change', TestDrive.checkBox());
        $(document).on('change', TestDrive.checkBox(), function () {
            if (TestDrive.checkBox().is(":checked")) {
                TestDrive.disclaimer().show();
            }
            else {
                TestDrive.disclaimer().hide();
            }
        });
    },
    showHideBookTdOption: function (campaignId) {
        TestDrive.uncheckBookTdOption();
        if ($.inArray(campaignId, testDriveCampaignIds) < 0) {
            TestDrive.hideBookTdOption();
        }
        else {
            $('.book-test-drive').show();
        }
    },
    isTdCheckboxChecked: function (campaignId) {
        return (($.inArray(campaignId, testDriveCampaignIds) > -1) &&
                TestDrive.checkBox().is(':checked'));
    },
    hideBookTdOption: function () {
        TestDrive.uncheckBookTdOption();
        $('.book-test-drive').hide();
        TestDrive.disclaimer().hide();
    },
    uncheckBookTdOption: function () {
        TestDrive.checkBox().prop('checked', false);
        TestDrive.disclaimer().hide();
    },
    checkBookTdOption: function (campaignId) {
        if ($.inArray(campaignId, testDriveCampaignIds) > -1) {
            TestDrive.checkBox().prop('checked', true);
            TestDrive.disclaimer().show();
        }
    }
};

function initializePopupControls(data) {
    LeadFormState.FlowType = LeadForm.FlowType();
    LeadFormState.FlowText = LeadForm.FlowText();
    hideCustomerFormErrors();
    OriginalEncryptLeadId = "";
    //TBD hide all sections and show cusifo
    $('#postsubmitmsg').hide();
    $('.pq-popup-close__js').removeClass("popup-close-btn--white").addClass("popup-close-btn--black");
    $('.email-screen__js').hide();
    $('#thankYouWithImage').hide();
    $('#reqSellerAssist').hide();
    $('#reqAssist').hide();
    $('#divMassage').addClass("hide");
    $('#skipBtn').hide();
    $("#reqAsstncBtn").prop("disabled", false);
    $("#reqAsstncBtn").val("Submit");
    $("#reqAsstncBtn").prop('screenId', emiassist.screen.CustomerInfo);
    emiassist.submitBtnFixedFloating();
    $("#divCustInfo").show();
    $('.content-wrapper').show();
    $('.user-agreement').show();
    $('.select_all__js').attr('checked', false);
    $('.checkbox-input').attr('checked', false);
    $(".toggle-selection-txt__js").html("Select All");
    $(".drp-dealer-list__js").hide();

    if (data.DealerMutualLead && (LeadFormState.FlowType == LeadForm.Flows.FlowB)) {
        callToSellerEngineFlowB(data.ModelId, data.CityId, platform, 1, data.AreaId, data.DealerId);
    }

    if (data.ShowEmail != "undefined" && !data.ShowEmail) {
        $(".custEmail__js").hide();
        data.GACat += "-nonEmail";
    }

    data.DealerName = data.DealerName.replace(/&amp;/g, '&').replace(/&#39;/g, "'");

    if (typeof data.ImagePath != "undefined" && data.ImagePath != "") {
        $('#popup-title').hide();
        $('#dealerName').text(data.DealerName);
        $('#carImage').attr('src', data.ImagePath);
        $('#carName').text(data.MakeName + ' ' + data.ModelName);
        $('#carDetailsDiv').show();
    }
    else {
        $('#popup-title').show();
        $('#carDetailsDiv').hide();
        if (data.DealerLocatorName) {
            $('#spnPopupTitle').text(data.DealerLocatorName);
        } else {
            $('#spnPopupTitle').text(data.DealerName);
        }
    }
    // Test Drive option
    TestDrive.showHideBookTdOption(Number(data.DealerId));
    var leadClickSourceForTdCta = [128, 129, 133, 204, 205, 206];
    if (typeof data != 'undefined' && leadClickSourceForTdCta.indexOf(Number(data.LeadClickSourceId)) > -1) {
        TestDrive.checkBookTdOption(Number(data.DealerId));
    }
}

function initDealerPopup(data) {
    // defaults if value not passed
    if (typeof (data.GACat) == "undefined") data.GACat = "PQPopupGeneric";
    if (typeof (data.GAActionDifferential) == "undefined") data.GAActionDifferential = "-action not supplied-" + data.AdType;
    if (typeof (data.cwCat) == "undefined") data.cwCat = 'PQPopupGeneric';
    if (typeof (data.cwAct) == "undefined") data.cwAct = 'action not supplied';
    if (typeof (data.cwLabel) == "undefined") data.cwLabel = 'label not supplied';

    initializePopupControls(data);

    bindDealerCity(data);

    campaignData = data;

    preFillCustomerDetails();

    window.scrollTo(0, 0);
    if (typeof data.predictionScore != 'undefined' && data.predictionScore >= -100 && typeof data.predictionLabel != 'undefined' && data.predictionLabel != '') {
        cwTracking.trackCustomData('PredictionFeedback', '1', data.predictionLabel, false);
    }
}

function dealerPopupCityChange() {
    var selectedCityNode = getSelectedOption('#drpCityDealerPopup');
    campaignData.CityId = selectedCityNode.value;
    campaignData.CityName = $(selectedCityNode).text();
    if ($(selectedCityNode).attr('zoneid') != undefined)
        campaignData.ZoneId = $(selectedCityNode).attr('zoneid');
    else
        campaignData.ZoneId = "-1";

    bindDealers(campaignData);
    if (campaignData.DealerAutoAssignPanel != "2" && campaignData.DealerLeadBusinessType != "0") {
        resetDealerHtml();
    }
    hideCustomerFormErrors();
}

function resetDealerHtml() {
    $("#hdnDealerId").val("");
    $("#hdnDealerName").val("");
    $("#tycaption").html("us");
}

function bindDealerCity(data) {
    if (Number(data.CityId) > 0) {
        $("#cityDealerPopup").hide();
        bindDealers(data);
    }
    else
        bindDealerPopupCity(data, processCityDrp);
}

function processCityDrp(data) {
    if (data.CityId != null && Number(data.CityId) > 0 && ModelCars.PQ.checkForCityDealerPopup(data.CityId, 'drpCityDealerPopup')) {
        if (Number(data.CityId) != 646 && Number(data.CityId) != 647 && Number(data.CityId) != 645) {
            if (data.ZoneId != undefined && Number(data.ZoneId) > 0)
                $("#drpCityDealerPopup option[value=" + data.CityId + "][zoneid=" + data.ZoneId + "]").prop('selected', true);
            else
                $("#drpCityDealerPopup option[value=" + data.CityId + "]:not([zoneid])").prop('selected', true);
        }
        $("#drpCityDealerPopup").trigger("change");
        $('#cityDealerPopup').hide();
    }
    else
        bindDealers(data);
}

function bindDealerPopupCity(data, CallBackFunction) {

    $('#drpCityDealerPopup').empty();
    $.ajax({
        type: 'GET',
        url: '/api/campaign/' + data.DealerId + '/cities/?modelid=' + data.ModelId,
        dataType: 'Json',
        success: function (json) {

            var viewModel = {
                pqCities: ko.observableArray(json)
            };

            ko.cleanNode($("#cityDealerPopup")[0]);
            ko.applyBindings(viewModel, $("#cityDealerPopup")[0]);

            ModelCars.PQ.bindDealerpopupZones('drpCityDealerPopup');

            $("#drpCityDealerPopup").prepend('<option value=-1>---Select City---</option>');
            $("#drpCityDealerPopup option[value=" + -1 + "]").attr('disabled', 'disabled');
            $("#drpCityDealerPopup option[value=" + -2 + "]").attr('disabled', 'disabled');
            $("#drpCityDealerPopup").val("-1");

            if (typeof (CallBackFunction) == "function") CallBackFunction(data);
        }
    });
}

function preFillCustomerDetails() {
    if ($('.custEmail__js').is(":visible"))
        $('#emailAssist').val($.cookie('_CustEmail'));
    if ($.cookie('_CustomerName') == null)
        $('#userName').focus();
    else
        $('#userName').val($.cookie('_CustomerName'));
    $('#phoneNumber').val($.cookie('_CustMobile'));
}

function preFillOptionalEmail() {
    $('#emailAssistOptional').val($.cookie('_CustEmail'));
}


function bindDealers(data) {
    $('#otherDealerDropdown').empty();

    if (data.CityId && data.CityId >0 && (data.DealerAutoAssignPanel == "3" || data.DealerAutoAssignPanel == "-1")) {
        $.ajax({
            type: 'GET',
            url: '/api/dealers/ncs/?modelid=' + data.ModelId + '&cityid=' + data.CityId + '&campaignid=' + data.DealerId,
            dataType: 'Json',
            success: function (json) {

                var viewModel = {
                    dealer: ko.observableArray(json)
                };

                ko.cleanNode($('#otherDealerDropdown')[0]);
                ko.applyBindings(viewModel, $('#otherDealerDropdown')[0]);
                if (json.length > 1) {
                    $(".drp-dealer-list__js").show();
                } else if (json.length == 1) {
                    $("#otherDealerDropdown").prop("selectedIndex", 1);
                    $(".drp-dealer-list__js").hide();
                } else {
                    $(".drp-dealer-list__js").hide();
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    }
    else {
        $("#btnSubmit").val("Submit").prop("disabled", false);
    }
}

function drpDealerChange() {
    dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: 'dealerDrpFocus' });
}

function setCustomerCookies() {
    var email = getEmailValue();
    if (email != undefined)
        document.cookie = '_CustEmail=' + email + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';

    document.cookie = '_CustomerName=' + $.trim($('#userName').val()) + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
    document.cookie = '_CustMobile=' + $.trim($('#phoneNumber').val()) + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
}

function getEmailValue() {
    if ($('.custEmail__js').is(":visible"))
        return $.trim($('#emailAssist').val());
    else if ($('.email-screen__js').is(":visible"))
        return $.trim($('#emailAssistOptional').val());
}

function getSelectedOption(id) {
    var e = $(id);
    if (e.length > 0)
        return e[0].options[e[0].selectedIndex];
    else
        return undefined;
}

function processPQLead(isNewLead) {
    LeadAssignedDealerId = $('#otherDealerDropdown').is(':visible') ? $('#otherDealerDropdown  option:selected').val() : (typeof $('#otherDealerDropdown  option:selected').val() === 'undefined' ? -1 : $('#otherDealerDropdown  option:selected').val());
    $('#otherDealerDropdown').empty();
    setCustomerCookies();
    if (typeof campaignData.CityName != 'undefined' && $.cookie('_CustCityIdMaster') < 1) {
        if ((typeof platform !== 'undefined') && platform == 43) {
            globalLocation.setLocationCookies(campaignData.CityId, campaignData.CityName, campaignData.AreaId, typeof campaignData.AreaName != 'undefined' ? campaignData.AreaName : "Select Area", campaignData.ZoneId, typeof campaignData.ZoneName != 'undefined' ? campaignData.ZoneName : "Select Zone");
            $("#pq-popup-close-spn, #reqAsstncBtn").attr("IsRefresh", true);
        }
        else if (window.isLeadWebview) {
            window.webviewCityId = campaignData.CityId;
            window.webviewCityName = campaignData.CityName;
            SendDataToApp();
        }
        else {
            Location.globalSearch.setLocationCookies(campaignData.CityId, campaignData.CityName, campaignData.AreaId, typeof campaignData.AreaName != 'undefined' ? campaignData.AreaName : "Select Area", campaignData.ZoneId, typeof campaignData.ZoneName != 'undefined' ? campaignData.ZoneName : "Select Zone");
        }
    }
    callPostDealerInquiry(isNewLead);
    return true;
}

function processPostSubmitMsg() {
    $("#skipBtn").hide();
    if (LeadFormState.FlowType == LeadForm.Flows.FlowB) {
        $('#reqSellerAssist').hide();
    }
    var askEmail = !campaignData.ShowEmail && ($.cookie('_CustEmail') === null || $.cookie('_CustEmail') == "");
    if (askEmail) {
        $('.pq-popup-close__js').removeClass("popup-close-btn--black").addClass("popup-close-btn--white");
        $('#postsubmitmsg span').text($.cookie('_CustomerName'));
        $("#reqAsstncBtn").prop("disabled", false);
        $('#postsubmitmsg').show();
        preFillOptionalEmail();
        $('.email-screen__js').show();
        $("#divCustInfo").hide();
        $("#reqAsstncBtn").val("Done");
        $('#reqAsstncBtn').prop('screenId', emiassist.screen.ThankYouWithEmail);
        $('.user-agreement__js').hide();
    }
    else {
        $('.pq-popup-close__js').removeClass("popup-close-btn--white").addClass("popup-close-btn--black");
        $('#postsubmitmsg').hide();
        $('#thankYouWithImage').show();
        $('#reqAsstncBtn').prop('screenId', emiassist.screen.ThankYouWithImage);
        $('.content-wrapper').hide();
        $("#divCustInfo").hide();
    }
    emiassist.submitBtnFixedFloating();
}

function GATrackingOnSubmit() {
    if (campaignData.isLeadSubmitted) {
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: platformTrackingText + "_SuccessfulSubmit-Email", lab: campaignData.ModelName });
    } else {
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: platformTrackingText + "_SuccessfulSubmit", lab: campaignData.ModelName });
    }
}

function hideCustomerFormErrors() {
    $(".form-input").each(function () {
        inputFeild = $(this);
        inputFeild.removeClass("input-error");
        inputFeild.siblings(".input__error-message").hide();
    });
}

function validateOptionalEmail(elem) {
    emailElem = $(elem).find(".email-field");
    custEmail = $(emailElem).val().toLowerCase();
    var reEmail = /^[a-z0-9._%+-]+@[a-z-]{2,}\.[a-z]{2,}(\.[a-z]{1,}|$)$/;
    if (!reEmail.test(custEmail)) {
        errorField = $(emailElem).siblings(".input__error-message");
        errorField.html("Enter Correct Email");
        $(emailElem).addClass("input-error");
        errorField.show();
        return false;
    }
    else {
        errorField = $(emailElem).siblings(".input__error-message");
        errorField.html("");
        $(emailElem).removeClass("input-error");
        errorField.hide();
        return true;
    }
}

var campaignViewModel = function () {
    var self = this;
    self.CampaignRecommendation = ko.observableArray();
};

var sellerViewModel = function () {
    var self = this;
    self.SellerRecommendation = ko.observableArray();
}

function getUserModelHistory() {
    if (isCookieExists('_userModelHistory')) {
        var userHistoryString = $.cookie('_userModelHistory');
        var userHistory = userHistoryString.split('~').join(',');
        return userHistory;
    } else {
        return "";
    }
}

function createDealerObject(data) {

    data.Email = $.cookie('_CustEmail');
    data.Name = $.cookie('_CustomerName');
    data.Mobile = $.cookie('_CustMobile');
    data.UtmaCookie = isCookieExists('__utma') ? $.cookie('__utma') : '';
    data.UtmzCookie = isCookieExists('_cwutmz') ? $.cookie('_cwutmz') : '';
    data.InquirySourceId = inquirySourceId;
    data.PlatformSourceId = (typeof platform !== 'undefined') ? platform : "43";
    data.EncryptedPQDealerAdLeadId = (typeof data.EncryptedPQDealerAdLeadId === 'undefined' || data.EncryptedPQDealerAdLeadId == "") ? "" : data.EncryptedPQDealerAdLeadId;
    data.ModelsHistory = getUserModelHistory();

    var AssignedDealerId = LeadAssignedDealerId;

    if ((data.DealerAutoAssignPanel == "3" && AssignedDealerId != "-1") || (data.DealerAutoAssignPanel == -1 && AssignedDealerId != "" && AssignedDealerId != "-1")) {
        data.IsAutoApproved = true;
        data.AssignedDealerId = AssignedDealerId;
    } else {
        data.IsAutoApproved = false;
        data.AssignedDealerId = -1;
    }
    data.SponsoredBannerCookie = isCookieExists('_sb' + data.ModelId) ? $.cookie('_sb' + data.ModelId) : '';
    var dict = {};

    if (window.isWebview) {
        dict['IsWebView'] = '1';
        if (window.webviewAppVersionId) {
            dict['AppVersionId'] = window.webviewAppVersionId;
        }
        // Don't change key Name as its dependent on Enum
    }

    data.Others = dict;
}

function submitSellerLead(data, isNewLead) {
    var dealerInquiry = data;
    $.ajax({
        type: 'POST',
        url: '/api/dealer/inquiries/',
        data: JSON.stringify(dealerInquiry),
        contentType: "application/json",
        success: function (encryptIds) {
            try {
                var encryptIdsArray = encryptIds.split(',');
                if ($.inArray(parseInt(dealerInquiry.LeadSource.LeadClickSourceId), arrLeadClickSource) >= 0) {
                    OriginalEncryptLeadId = encryptIds;
                }
                if (isNewLead != false && typeof (window.isLeadWebview) == "undefined") {
                    for (i = 0; i < dealerInquiry.CarInquiry.length; i++) {
                        leadConversionTracking.track(dealerInquiry.LeadSource.LeadClickSourceId, dealerInquiry.CarInquiry[i].Seller.CampaignId);
                    }
                }
            } catch (err) {
                console.log(err);
            }
        },
        error: function () {
            processSubmitFlowWithEmailId();
        }
    });
    campaignData.isLeadSubmitted = true;
}

function getRecoLeadAndInquirySource() {
    campaignData.InquirySourceId = LeadSource.recoInquirySource(campaignData.clicksource);
    campaignData.LeadClickSourceId = LeadSource.recoLeadClickSource(campaignData.clicksource);
}

AjaxCallState = {
    transit: 0,
    show: 1,
    hide: 2
}

function callPostDealerInquiry(isNewLead) {
    $('#reqAsstncBtn').val("Processing....").prop("disabled", true);
    var data = campaignData;

    createDealerObject(data);

    var dealerInquiry = createNewInquiryObject(data);

    var CarInquiry = {
        CarDetail: { MakeId: data.MakeId, ModelId: data.ModelId, VersionId: data.VersionId },
        Seller: { CampaignId: data.DealerId, AssignedDealerId: data.AssignedDealerId }
    }
    dealerInquiry.CarInquiry.push(CarInquiry);

    if (!isNewLead) {
        dealerInquiry.EncryptedLeadId = OriginalEncryptLeadId;
    }

    if (TestDrive.isTdCheckboxChecked(Number(data.DealerId))) {
        dealerInquiry.Others['TestDrive'] = '1';
        // Don't change key Name as its dependent on Enum
        TestDrive.hideBookTdOption();
    }

    submitSellerLead(dealerInquiry, isNewLead);

    if (LeadFormState.FlowType == LeadForm.Flows.FlowB && data.DealerMutualLead && $('#reqAsstncBtn').prop('screenId') == emiassist.screen.CustomerInfo) {
        cwTracking.trackAction('CWInteractive', 'newcarleadform_flowB_multiple_dealers_submitted', '0/0', 'NA');
    }
    GATrackingOnSubmit();
    LeadFormState.MLAState = AjaxCallState.hide;
    LeadFormState.RecoState = AjaxCallState.hide;

    if ((typeof isNewLead == "undefined" || isNewLead)) {

        if (data.DealerMutualLead && (LeadFormState.FlowType == LeadForm.Flows.FlowA)) {
            LeadFormState.MLAState = AjaxCallState.transit;
            callToSellerEngineFlowA(data.ModelId, data.CityId, platform, 1, data.AreaId, data.DealerId, data.DealerAdminId);
        }

        if (nonRecommendationsCampaignIds.indexOf(data.DealerId.toString()) === -1) {
            LeadFormState.RecoState = AjaxCallState.transit;
            callToRecommendationEngine(data);
        } else if (LeadFormState.MLAState == AjaxCallState.hide && LeadFormState.RecoState == AjaxCallState.hide) {
            processSubmitFlowWithEmailId();
        }
    }
    else {
        processSubmitFlowWithEmailId();
    }

    trackConversion(data, false);
}

function hideDealerForm() {
    $("#divCustInfo").hide();
}

function trackConversion(inquiryObj, isReco) {
    if (isReco) {
        var actText;
        switch (inquiryObj.cwAct) {
            case 'GetOffersClick': actText = 'GetOffersRecommendationLeadSubmit'; break;
            case 'OffersButtonClick': actText = 'OffersButtonRecommendationLeadSubmit'; break;
            case 'SlugButtonClick': actText = 'SlugLeadSubmitRecommendationLeadSubmit'; break;
            case 'EmiAssistanceButtonClick': actText = 'LinkRecommendationLeadSubmit'; break;
            case 'PQLinkClick': actText = 'PQLinkRecommendationLeadSubmit'; break;
            case 'PQTemplateClick': actText = 'PQTemplateRecommendationLeadSubmit'; break;
            case 'PQTemplateLeadSubmit': actText = 'PQTemplateRecommendationLeadSubmit'; break;
            case 'PQLinkLeadSubmit': actText = 'PQLinkRecommendationLeadSubmit'; break;
            case 'EMIQuoteLeadSubmit': actText = 'EMIQuoteRecommendationLeadSubmit'; break;
            case 'EmiAssistanceButtonLeadSubmit': actText = 'EmiAssistanceButtonRecommendationLeadSubmit'; break;
            case 'EmiAssistanceLinkLeadSubmit': actText = 'EmiAssistanceLinkRecommendationLeadSubmit'; break;
            case 'SlugLeadSubmit': actText = 'SlugLeadRecommendationLeadSubmit'; break;
            case 'GetOffersLeadSubmit': actText = 'GetOffersRecommendationLeadSubmit'; break;
            case 'AppLinkClick': actText = 'AndroidRecommendationLeadSubmit'; break;
            default: actText = 'UndefinedRecommendationLeadSubmit';
        }
        var label = 'make:' + inquiryObj.MakeName + '|model:' + inquiryObj.ModelName + '|city:' + inquiryObj.CityName;
        cwTracking.trackCustomData(inquiryObj.cwCat, actText, label, false);
    }
    else if (typeof inquiryObj.EncryptedPQDealerAdLeadId == "undefined" || inquiryObj.EncryptedPQDealerAdLeadId == "") {
        var actText;
        switch (inquiryObj.cwAct) {
            case 'GetOffersClick': actText = 'GetOffersClickLeadSubmit'; break;
            case 'OffersButtonClick': actText = 'OffersButtonClickLeadSubmit'; break;
            case 'SlugButtonClick': actText = 'SlugLeadSubmit'; break;
            case 'EmiAssistanceButtonClick': actText = 'LinkLeadSubmit'; break;
            case 'PQLinkClick': actText = 'PQLinkLeadSubmit'; break;
            case 'PQTemplateClick': actText = 'PQTemplateLeadSubmit'; break;
            case 'EmiAssistanceLinkClick': actText = 'EmiAssistanceLinkLeadSubmit'; break;
            case 'BestDealClick': actText = 'SlugLeadSubmit'; break;
            case 'AppLinkClick': actText = 'AndroidLeadSubmit'; break;
            default: actText = 'UndefinedLeadSubmit';
        }
        cwTracking.trackCustomData(inquiryObj.cwCat, actText, inquiryObj.cwLabel, false);

        if (typeof inquiryObj.predictionScore != 'undefined' && inquiryObj.predictionScore >= -100 && typeof inquiryObj.predictionLabel != 'undefined' && inquiryObj.predictionLabel != '') {
            cwTracking.trackCustomData('PredictionFeedback', '2', inquiryObj.predictionLabel, false);
        }
    }
}

var LeadFormState = {};
var MLADict = {};
var CampaignDict = {};

function callToRecommendationEngine(data) {
    var modelList = getUserModelHistory();
    var noOfRecommendation = (platform == 1 ? 4 : 3);
    $.ajax({
        type: 'GET',
        url: '/api/v1/campaign/recommendations/?modelId=' + data.ModelId
            + '&cityId=' + data.CityId + '&platformId=' + platform + '&mobile='
            + $.cookie('_CustMobile') + '&recommendationcount=' + noOfRecommendation
            + '&areaId=' + data.AreaId + '&zoneId=' + ((typeof (isLeadWebview) != "undefined" && isLeadWebview) ? window.webviewZoneId : 0) + '&boost=true',
        dataType: 'Json',
        contentType: "application/x-www-form-urlencoded",
        success: function (response) {
            LeadFormState.RecoState = AjaxCallState.hide;
            try {

                if (response != null) {
                    response = FilterRecommendCars(response);
                    LeadFormState.CarRecommendationData = response;
                } else {
                    LeadFormState.CarRecommendationData = [];
                }
                if (response != null && response.length > 0) {
                    LeadFormState.RecoState = AjaxCallState.show;
                }
            } catch (err) {
                console.log("callToRecommendationEngine function");
            }
        },
        error: function (err) {
            console.log("callToRecommendationEngine function failure" + err);
            LeadFormState.RecoState = AjaxCallState.hide;
        },
        complete: function () {
            if (LeadFormState.RecoState == AjaxCallState.show) {
                if (LeadFormState.MLAState == AjaxCallState.show) {
                    $('#skipBtn').show();
                    cwTracking.trackAction('CWNonInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_multiple_dealers_shown_skip_shown', LeadFormState.SellerRecommendationsCount, 'NA');
                } else if (LeadFormState.MLAState == AjaxCallState.hide || !campaignData.DealerMutualLead) {
                    renderCarRecommendation(LeadFormState.CarRecommendationData);
                }
            } else {
                if (LeadFormState.MLAState == AjaxCallState.hide || !campaignData.DealerMutualLead) {
                    processSubmitFlowWithEmailId();
                }
            }
            LeadPopupContent.calculateContentHeight();
        }
    });
}

function callToSellerEngineFlowA(modelId, cityId, platformId, applicationId, areaId, dealerId, dealerAdminId) {
    if (areaId == "" || areaId == "-1" || areaId < 0)
        areaId = 0;
    $.ajax({
        type: 'GET',
        url: '/api/v3/campaigns/?modelid=' + modelId + '&cityId=' + cityId + '&platformId=' + platformId + '&applicationId=' + applicationId + '&areaId=' + areaId + '&isDealerLocator=true' + '&dealerAdminFilter=true',
        dataType: 'Json',
        contentType: 'application/x-www-form-urlencoded',
        success: function (response) {
            LeadFormState.MLAState = AjaxCallState.hide;
            try {
                if (response != null && response.length > 0) {
                    response = $(response).filter(function () { return this.campaign.id != dealerId && this.campaign.mutualLeads });

                    if (dealerAdminId != 0) {
                        response = $(response).filter(function () { return this.campaign.dealerAdminId != dealerAdminId });
                    }

                    if (response.length > 0) {
                        LeadFormState.MLAState = AjaxCallState.show;

                        LeadFormState.SellerRecommendationsCount = response.length;

                        hideDealerForm();
                        $('.pq-popup-close__js').removeClass("popup-close-btn--black").addClass("popup-close-btn--white");
                        $('#postsubmitmsg span').text($.cookie('_CustomerName'));
                        $('#postsubmitmsg').show();
                        $('#reqSellerAssist').show();
                        $('#reqAsstncBtn').prop('screenId', emiassist.screen.MLA);
                        emiassist.submitBtnFixedFloating();
                        $('.user-agreement__js').hide();
                        $('#reqAsstncBtn').val('Submit');

                        _sellerViewModel.SellerRecommendation(response);
                        cwTracking.trackAction('CWNonInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_multiple_dealers_shown', response.length, 'NA');
                    }
                }
            } catch (err) {
                console.log("error occured in callToSellerEngineFlowA function");
                renderCarRecommendation(LeadFormState.CarRecommendationData);
            }
        },
        error: function (err) {
            console.log("error occured in callToSellerEngineFlowA function failure");
            LeadFormState.MLAstate = AjaxCallState.hide;
            renderCarRecommendation(LeadFormState.CarRecommendationData);
        },
        complete: function () {
            if (LeadFormState.MLAState == AjaxCallState.hide) {
                cwTracking.trackAction('CWNonInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_multiple_dealers_not_shown', '0', 'NA');
                if (LeadFormState.RecoState == AjaxCallState.show) {
                    renderCarRecommendation(LeadFormState.CarRecommendationData);
                } else if (LeadFormState.RecoState == AjaxCallState.hide) {
                    processSubmitFlowWithEmailId();
                }
            } else {
                if (LeadFormState.RecoState == AjaxCallState.show) {
                    $("#skipBtn").show();
                    cwTracking.trackAction('CWNonInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_multiple_dealers_shown_skip_shown', LeadFormState.SellerRecommendationsCount, 'NA');
                }
            }
            LeadPopupContent.calculateContentHeight();
        }
    });
}

function callToSellerEngineFlowB(modelId, cityId, platformId, applicationId, areaId, dealerId) {
    if (areaId == "" || areaId == "-1" || areaId < 0)
        areaId = 0;
    if (typeof (MLADict[modelId]) != 'undefined') {
        renderMLAForCustomerInfo(MLADict[modelId], dealerId)
    }
    else {
        $.ajax({
            type: 'GET',
            url: '/api/v3/campaigns/?modelid=' + modelId + '&cityId=' + cityId + '&platformId=' + platformId + '&applicationId=' + applicationId + '&areaId=' + areaId + '&isDealerLocator=true' + '&dealerAdminFilter=true',
            dataType: 'Json',
            contentType: 'application/x-www-form-urlencoded',
            success: function (response) {
                MLADict[modelId] = response;
                renderMLAForCustomerInfo(response, dealerId);
            },
            error: function (err) {
                cwTracking.trackAction('CWNonInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_flowB_multiple_dealers_not_shown', '0', 'NA');
                console.log("error occured in callToSellerEngineFlowB function failure");
            }
        });
        cwTracking.trackAction('CWNonInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_flowB_multiple_dealers_requested', '0', 'NA');
    }

}

function renderMLAForCustomerInfo(response, dealerId) {
    try {
        if (response != null && response.length > 0) {
            response = $(response).filter(function () { return this.campaign.id != dealerId && this.campaign.mutualLeads });

            if (campaignData.DealerAdminId != 0) {
                response = $(response).filter(function () { return this.campaign.dealerAdminId != campaignData.DealerAdminId });
            }
            if (response.length > 0) {
                LeadFormState.SellerRecommendationsCount = response.length;
                if ($('#divCustInfo').is(':visible') && $('#reqAsstncBtn').prop("disabled") == false) {
                    if (!($("#reqSellerAssist .suggested-list-details").hasClass("suggested-list--expand"))) {
                        switch (LeadFormState.SellerRecommendationsCount) {
                            case 1: $('#additionalDealer').hide(); break;
                            case 2: $('#additionalDealer').text('+1 More Dealer'); $('#additionalDealer').show(); break;
                            default: $('#additionalDealer').text('+' + (LeadFormState.SellerRecommendationsCount - 1).toString() + ' More Dealers'); $('#additionalDealer').show();
                        }
                    }
                    $('.mla-header__js').text('Select additional dealers');
                    $('#reqSellerAssist').addClass('additional-list');
                    $('#reqSellerAssist').show();
                    $('#reqAsstncBtn').prop('screenId', emiassist.screen.CustomerInfoWithMLA);
                    emiassist.submitBtnFixedFloating();
                    _sellerViewModel.SellerRecommendation(response);
                    cwTracking.trackAction('CWNonInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_flowB_multiple_dealers_shown', response.length, 'NA');
                }
            } else {
                cwTracking.trackAction('CWNonInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_flowB_multiple_dealers_not_shown', '0', 'NA');
            }
        } else {
            cwTracking.trackAction('CWNonInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_flowB_multiple_dealers_not_shown', '0', 'NA');
        }
    } catch (err) {
        cwTracking.trackAction('CWNonInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_flowB_multiple_dealers_not_shown', '0', 'NA');
        console.log("error occured in callToSellerEngineFlowB function");
    }
}

function renderCarRecommendation(recommendCars) {

    if (recommendCars != null && recommendCars.length > 0) {
        if (recommendCars && recommendCars.length > 0) {
            hideDealerForm();
            if (LeadFormState.FlowType == LeadForm.Flows.FlowB) {
                $('#reqSellerAssist').hide();
            }
            $("#skipBtn").hide();
            $('#reqAsstncBtn').prop("disabled", true);
            $('#reqAsstncBtn').prop('screenId', emiassist.screen.Recommendation);
            emiassist.submitBtnFixedFloating();
            $('.user-agreement__js').hide();
            $('#reqAsstncBtn').val("Request Assistance");
            $('.pq-popup-close__js').removeClass("popup-close-btn--black").addClass("popup-close-btn--white");
            $('#postsubmitmsg span').text($.cookie('_CustomerName'));
            $('#postsubmitmsg').show();
            $('#reqAssist').show();

            _campaignViewModel.CampaignRecommendation(recommendCars);
            Recommendation.trackCampaignReco(recommendCars.length);
            if (recommendCars.length > 1) {
                cwTracking.trackAction('DealerLeadPopUpBehaviour', 'Recommended Cars with Tie Up - ' + platformTrackingText, 'SelectAll_Shown');
            }
        }
        else {
            processSubmitFlowWithEmailId();
        }
    }
    else {
        processSubmitFlowWithEmailId();
        cwTracking.trackAction('DealerLeadPopUpBehaviour', 'Recommended Cars with Tie Up - ' + platformTrackingText, 'Suggestions Not Shown', campaignData.ModelName + "_" + Recommendation.getCityToTrack());
    }
}

function FilterRecommendCars(response) {
    var recommendCars = [], responseLength = response.length;
    for (var i = 0 ; i < responseLength ; i++) {
        if (response[i].carPricesOverview) {
            recommendCars.push(response[i]);
        }
    }
    return recommendCars;
}

function processSubmitFlowWithEmailId() {
    if ($('#divCustInfo').is(":visible")) {
        processPostSubmitMsg();
    }

    GATrackingOnSubmit();
    isValidCustdetails = true;
}

function trackFocusEventsToGAForDealer() {
    $(document).on('change', '#drpDealer', function () {
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: 'dealerDropDownFocus' });
    });

    $(document).on('focus', '#custName', function () {
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: 'nameFocus' });
    });

    $(document).on('focus', '#custEmail', function () {
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: 'emailFocus' });

    });

    $(document).on('focus', '#custMobile', function () {
        dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: 'phoneFocus' });

    });
}

function applyKOBindingRecommendation() {
    ko.cleanNode(document.getElementById("recommendationList"));
    ko.applyBindings(_campaignViewModel, document.getElementById("recommendationList"));
}

function applyKOBindingSellerRecommendation() {
    ko.cleanNode(document.getElementById("recommedationSellerList"));
    ko.applyBindings(_sellerViewModel, document.getElementById("recommedationSellerList"));
}

$(document).on('click', '.suggestion-box li', function () {
    var node = $(this);
    if (node.find('.unchecked__js').hasClass('empty')) {
        node.find('.unchecked__js.empty').removeClass('empty');
        $('#selectOption').hide();
    }
    node.find('.unchecked__js').toggleClass('checked');
    dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: "Recommended Cars with Tie Up -  " + platformTrackingText, act: 'Suggested Checked', lab: node.index });
    btnState(node);
    removeSelection(node);
    if (node.parent().parent().is('#reqSellerAssist')) {
        if (node.find('.recommended-checkbox').children().hasClass('checked')) {
            cwTracking.trackAction('CWInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_' + LeadFormState.FlowText + 'multiple_dealers_dealer_selected', (node.index() + 1) + '/' + LeadFormState.SellerRecommendationsCount, 'NA');
        }
        else {
            cwTracking.trackAction('CWInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_' + LeadFormState.FlowText + 'multiple_dealers_dealer_deselected', (node.index() + 1) + '/' + LeadFormState.SellerRecommendationsCount, 'NA');
        }
    }

});

$(document).on('click', '.selectAllOptions', function () {
    var node = $(this);
    var status = Recommendation.pqRecommendation.toggleSider(node);
    var isSellerRecco = node.parent().parent().is('#reqSellerAssist');
    if (status == 1) {
        if (isSellerRecco) {
            cwTracking.trackAction('CWInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_' + LeadFormState.FlowText + 'multiple_dealers_select_all_selected', LeadFormState.SellerRecommendationsCount, 'NA');
        }
        else {
            cwTracking.trackAction('DealerLeadPopUpBehaviour', 'Recommended Cars with Tie Up - ' + platformTrackingText, 'SelectAll_Clicked');
        }

    }
    else if (isSellerRecco) {
        cwTracking.trackAction('CWInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_' + LeadFormState.FlowText + 'multiple_dealers_select_all_deselected', LeadFormState.SellerRecommendationsCount, 'NA');
    }
    Recommendation.pqRecommendation.enableSelection(status);
});

$(document).on('click', '#skipBtn', function () {
    var totalDealers = $("#recommedationSellerList").find("li").length;
    cwTracking.trackAction('CWInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_multiple_dealers_skipped', totalDealers, 'NA');
    $('#reqSellerAssist').hide();
    renderCarRecommendation(LeadFormState.CarRecommendationData);
    LeadPopupContent.calculateContentHeight();
});

var Recommendation = {

    pqRecommendation: {
        enableSelection: function (status) {
            var components = $('.listTable  li');
            components.each(function () {
                var node = $(this);
                var checkBoxSelections = node.find('.recommended-checkbox').find('span');
                status ? checkBoxSelections.addClass('unchecked__js checked') : checkBoxSelections.removeClass('checked').addClass('unchecked__js');
                btnState(node);
                checkBoxSelections.removeClass('empty');
                $('#selectOption').hide();
            });
        },
        toggleSider: function (node) {
            var parentDiv = node.parent().parent();
            var toggleTitle = parentDiv.find('.toggle-selection-txt');
            parentDiv.find('.toggle-selection-view').toggleClass('select-all remove-all');
            toggleTitle.text(toggleTitle.text() === "Select All" ? "Remove All" : "Select All")
            return toggleTitle.text() === "Select All" ? 0 : 1;
        }
    },

    trackCampaignReco: function (noOfReco) {
        var cityName = Recommendation.getCityToTrack();
        cwTracking.trackAction('DealerLeadPopUpBehaviour', 'Recommended Cars with Tie Up - ' + platformTrackingText, 'Suggestions shown', noOfReco + "_" + campaignData.ModelName + "_" + cityName);
    },

    getCityToTrack: function () {
        var cityName = "";
        if (Common.utils.isQuotationPage())
            cityName = $.cookie('_CustCity');
        else
            cityName = $.cookie('_CustCityMaster');
        return cityName;
    }
};

var ModelCars = {

    PQ: {
        bindDealerpopupZones: function (drpCity) {
            var popularCities = { "10": "Delhi", "1": "Mumbai", "2": "Bangalore", "12": "Pune", "105": "Hyderabad", "176": "Chennai", "128": "Ahmedabad", "198": "Kolkata", "244": "Chandigarh", "160": "Jaipur", "220": "Lucknow" };
            var popularCityOrder = ["10", "1", "2", "12", "105", "176", "128", "198", "244", "160", "220"];
            var cityDrp = $('#' + drpCity);
            var availableCities = [];
            
            popularCityOrder.forEach(function (city) {
                if (ModelCars.PQ.checkForCityDealerPopup(city, drpCity)) {
                    availableCities.push(city);
                }
            });

            var isPopularCityAvailable = availableCities.length > 0 ? true : false
            if (isPopularCityAvailable) {
                cityDrp.prepend('<option value=-2>-------------</option>');

                var otherPopularCityGroup, mumbaiGroup, delhiGroup;
                availableCities.forEach(function (city) {
                    if (city == "1") {
                        mumbaiGroup = $("<optgroup label = 'Mumbai'/>");
                        mumbaiGroup.append("<option value=" + 1 + " ZoneId = '-1' >" + 'Mumbai' + "</option>");

                        if (ModelCars.PQ.checkForCityDealerPopup("40", drpCity)) { // Thane
                            mumbaiGroup.append("<option value=" + 40 + ">" + 'Thane' + "</option>");
                        }

                        if (ModelCars.PQ.checkForCityDealerPopup("13", drpCity)) { //Navi Mumbai
                            mumbaiGroup.append("<option value=" + 13 + ">" + 'Navi Mumbai' + "</option>");
                        }

                        if (ModelCars.PQ.checkForCityDealerPopup("395", drpCity)) { //Navi Mumbai
                            mumbaiGroup.append("<option value=" + 395 + ">" + 'Vasai-Virar' + "</option>");
                        }

                        if (ModelCars.PQ.checkForCityDealerPopup("6", drpCity)) { // Kalyan
                            mumbaiGroup.append("<option value=" + 6 + " >" + 'Kalyan-Dombivali' + "</option>");
                        }

                        if (ModelCars.PQ.checkForCityDealerPopup("8", drpCity)) { // Panvel
                            mumbaiGroup.append("<option value=" + 8 + ">" + 'Panvel' + "</option>");
                        }
                      
                    }
                    else if (city == "10") {
                        delhiGroup = $("<optgroup label = 'Delhi NCR'/>");
                        delhiGroup.append("<option value=" + 10 + " ZoneId = '-1'>" + 'Delhi' + "</option>");

                        if (ModelCars.PQ.checkForCityDealerPopup("246", drpCity)) { // Gurgoan
                            delhiGroup.append("<option value=" + 246 + ">" + 'Gurgaon' + "</option>");
                        }

                        if (ModelCars.PQ.checkForCityDealerPopup("224", drpCity)) { // Noida
                            delhiGroup.append("<option value=" + 224 + ">" + 'Noida' + "</option>");
                        }

                        if (ModelCars.PQ.checkForCityDealerPopup("225", drpCity)) { // Ghaziabad
                            delhiGroup.append("<option value=" + 225 + ">" + 'Ghaziabad' + "</option>");
                        }

                        if (ModelCars.PQ.checkForCityDealerPopup("273", drpCity)) { // Faridabad
                            delhiGroup.append("<option value=" + 273 + ">" + 'Faridabad' + "</option>");
                        }
                        
                    }
                    else {
                        if (!otherPopularCityGroup) {
                            otherPopularCityGroup = $("<optgroup label = 'Other Popular Cities'/>");
                        }
                        otherPopularCityGroup.append("<option value=" + city + ">" + popularCities[city] + "</option>");
                    }
                });
                
                if (otherPopularCityGroup) {
                    cityDrp.prepend(otherPopularCityGroup);
                }
                
                if (mumbaiGroup) {
                    cityDrp.prepend(mumbaiGroup);
                }
                
                if (delhiGroup) {
                    cityDrp.prepend(delhiGroup);
                }
            }
        },

        removeRepeatedCities: function (cityId, drpCity) {
            $("#" + drpCity + " option[value='" + cityId + "']").remove();
        },

        checkForCityDealerPopup: function (cityId, drpCity) {
            if ($("#" + drpCity + " option[value='" + cityId + "']").length > 0)
                return true;
            else
                return false;
        }
    }
};

function chkValid() {
    var checkedBox = $('#reqAssist.unchecked__js.checked');
    var unCheckedBox = $('.suggestion-box li .unchecked__js');
    if (checkedBox.length < 1) {
        unCheckedBox.addClass('empty');
        $('#selectOption').show();
        return false;
    }
    else {
        unCheckedBox.removeClass('empty');
        $('#selectOption').hide(); //TBD
        return true;
    }
}

function reqAsstncFun() {
    campaignData.EncryptedPQDealerAdLeadId = "";
    createDealerObject(campaignData);

    getRecoLeadAndInquirySource();

    var dealerInquiry = createNewInquiryObject(campaignData);

    $("#reqAssist").find("li :checked").each(function () {
        var CarInquiry = {
            CarDetail: { ModelId: $(this).attr('ModelId') },
            Seller: { CampaignId: $(this).attr('CampaignId'), AssignedDealerId: -1 }
        }
        dealerInquiry.CarInquiry.push(CarInquiry);
    });

    submitSellerLead(dealerInquiry);

    processPostSubmitMsg();
    dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: "Recommended Cars with Tie Up - " + platformTrackingText, act: 'Successful Submit', lab: $("#reqAssist").closest("#reqAssist").find("li :checked").length + "/" + $("#reqAssist").closest("#reqAssist").find("li").length });

    $("#reqAssist").hide();
}

function createNewInquiryObject(data, leadClickSourceId) {
    var dealerInquiry = {};
    dealerInquiry.UserInfo = { Name: data.Name, Email: data.Email || "", Mobile: data.Mobile };
    dealerInquiry.UserLocation = { CityId: data.CityId, ZoneId: data.ZoneId, AreaId: data.AreaId };
    dealerInquiry.LeadSource = { LeadClickSourceId: leadClickSourceId || data.LeadClickSourceId, InquirySourceId: data.InquirySourceId, PlatformId: platform, ApplicationId: 1 };
    dealerInquiry.CarInquiry = [];
    dealerInquiry.EncryptedLeadId = "";
    dealerInquiry.Others = data.Others;
    return dealerInquiry;
}

function reqSellerAsstncFun() {
    var dealerInquiry = createNewInquiryObject(campaignData, LeadSource.mlaLeadclickSource); // TBD pass mla lead click source on platform basis
    if (dealerInquiry.Others) {
        dealerInquiry.Others["TestDrive"] = 0;
    }
    $("#recommedationSellerList").find("li :checked").each(function () {
        var CarInquiry = {
            CarDetail: { MakeId: campaignData.MakeId, ModelId: campaignData.ModelId, VersionId: campaignData.VersionId },
            Seller: { CampaignId: $(this).attr('CampaignId'), AssignedDealerId: -1 }
        }
        dealerInquiry.CarInquiry.push(CarInquiry);
    });

    if ($("#recommedationSellerList").find("li :checked").length > 0) {
        submitSellerLead(dealerInquiry);
    }

    if (LeadFormState.FlowType != LeadForm.Flows.FlowB) {
        processPostSubmitMsg();
    }
    var totalDealers = $("#recommedationSellerList").find("li").length;
    var selectedDealers = $("#recommedationSellerList").find("li :checked").length;
    cwTracking.trackAction('CWInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_' + LeadFormState.FlowText + 'multiple_dealers_submitted', selectedDealers + "/" + totalDealers, 'NA');
    if (LeadFormState.FlowType != LeadForm.Flows.FlowB) {
        $('#reqSellerAssist').hide();
    }
}

var LeadFormBrowserNavigator = function (options) {
    var defaults = {
        onBrowserBack: function () { }
    };

    options = $.extend(defaults, options || {});
    var isOpen = false;

    this.open = function () {
        isOpen = true;
        window.history.pushState("", "", null);
    };

    this.close = function () {
        if (isOpen) {
            isOpen = false;
            window.history.back();
        }
    };

    $(window).on("load", function () {
        setTimeout(function () {
            $(window).on("popstate", function (event) {
                if (isOpen) {
                    isOpen = false;
                    options.onBrowserBack();
                }
            });
        }, 0);
    });
    return this;
};
//dealer popup code ends

var LeadPopup = (function () {
    var popupWindow;
    function _setSelectors() {
        popupWindow = $("#lead-popup");
    }

    function registerEvents() {
        _setSelectors();
        $(document).on('click', ".popup-btn", function () {
            popupWindow.addClass('lead-popup--visible');
            Common.utils.lockPopup();
        });

        $('#reqAsstncBtn').prop('screenId', emiassist.screen.CustomerInfo);
    }

    return { registerEvents: registerEvents }
})();

var Validate = (function () {
    var inputText;
    function _setSelectors(form, element) {
        inputText = (form).find(element);
        $(document).on("click", element.selector, function () { ValidatePrevSibilings(this, FormScroll); });
    }
    function ValidateText(elem) {
        if (elem.hasClass("email-field")) {
            var emailValid = ValidateEmail(elem);
            if (emailValid.isValid) {
                dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: 'errEmailNotValid' });
            }
            return emailValid;
        }
        else {
            return ValidateName(elem);
        }
        return {
            isValid: true
        };
    }
    function ValidateName(elem) {
        var custName = elem.val().trim();
        var reName = /^([-a-zA-Z ']*)$/;
        if (custName == "") {
            dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: 'errNameNotFilled' });
            return {
                isValid: false,
                errMessage: "Please enter your name"
            };
        }
        else if (!reName.test(custName)) {
            dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: 'errNonAlphabetName' });//chnage action acorrding to error
            return {
                isValid: false,
                errMessage: "Please enter only alphabets"
            };
        }
        else if (custName.length == 1) {
            dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: 'errOneCharName' });
            return {
                isValid: false,
                errMessage: "Please enter your complete name"
            };
        }
        else
            return {
                isValid: true
            };
    }
    function ValidateEmail(elem) {
        var reEmail = /^[a-z0-9._%+-]+@[a-z-]{2,}\.[a-z]{2,}(\.[a-z]{1,}|$)$/;
        var custEmail = elem.val().toLowerCase();
        if (custEmail == "") {
            dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: 'errEmailNotFilled' });
            return {
                isValid: false,
                errMessage: "Please enter your email"
            };
        }
        else if (!reEmail.test(custEmail)) {
            dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: 'errEmailNotValid' });
            return {
                isValid: false,
                errMessage: "Please enter your email"
            };
        }
        else
            return {
                isValid: true
            };
    }
    function ValidateNumber(elem) {
        var reMobile = /^[6789]\d{9}$/;
        var custMobile = elem.val();
        if (custMobile == "") {
            dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: 'errMobileNotFilled' });
            return {
                isValid: false,
                errMessage: "Please enter your mobile number"
            };
        }
        else if (custMobile.length != 10) {
            dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: 'errMobileNot10Digits' });
            return {
                isValid: false,
                errMessage: "Mobile number should be of 10 digits"
            };
        }
        else if (!reMobile.test(custMobile)) {
            dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: 'errMobileNotValid' });
            return {
                isValid: false,
                errMessage: "Please provide a valid 10 digit Mobile number"
            };
        }
        else
            return {
                isValid: true
            };
    }
    function ValidateDropDown(elem) {
        if (elem.prop('required')) {
            if (elem[0].value > 0) {
                return {
                    isValid: true
                };
            }
            else {
                return {
                    isValid: false,
                    errMessage: "Please select one option"
                };
            }
        }
        else {
            return {
                isValid: true
            };
        }
    }
    function ValidateMultiSelect(elem) {
        multiInput = $(elem).find(".checkbox-input");
        for (var x = 0; x < multiInput.length; x++) {
            if ($(multiInput[x]).is(":checked")) {
                return {
                    isValid: true
                };
            }
        }
        return {
            isValid: false,
            errMessage: "Please select one option"
        };

    }
    function ValidateFields(elem) {
        inputField = $(elem);
        var validate;
        if (inputField.is(":visible")) {
            inputType = typeof inputField.prop("type") != "undefined" ? inputField.prop("type").toLowerCase() : inputField.data("type").toLowerCase();
            switch (inputType) {
                case "text":
                    {
                        validate = ValidateText;
                        break;
                    }
                case "tel":
                    {
                        validate = ValidateNumber;
                        break;
                    }
                case "select-one":
                    {
                        validate = ValidateDropDown;
                        break;
                    }
                case "multiselect":
                    {
                        validate = ValidateMultiSelect;
                        break;
                    }
                default:
                    {
                        validate = function (inputField) {
                            return {
                                isValid: true
                            };
                        };
                    }
            }
            var valid = validate(inputField);
            if (!valid.isValid) {
                inputField.addClass("input-error");
                errorField = inputField.siblings(".input__error-message");
                errorField.html(valid.errMessage);
                errorField.show();
                return false;
            }
            else {
                inputField.removeClass("input-error");
                inputField.siblings(".input__error-message").hide();
                return true;
            }
        }
        return true;
    }
    function ValidateInput() {
        var isValid = true;
        inputText.each(function () {
            isValid = ValidateFields(this) && isValid;
        });
        return isValid;
    }
    function ValidatePrevSibilings(elem, callback) {
        $(elem).parent().prevAll().each(function () {
            ValidateFields($(this).find("input"));
        });
        callback(elem);
    }

    function registerEvents(element) {
        $(document).off('click', element.selector);
        $(document).on('click', element.selector, function () {
            if (ValidateInput()) {
                emiassist.submitClick();
            }
            else {
                dataLayer.push({ event: 'DealerLeadPopUpBehaviour', cat: campaignData.GACat, act: 'Unsuccessful-Submit' });
            }
        });
    }
    return { registerEvents: registerEvents, setSelectors: _setSelectors }
})();


var FormScroll = (function (elem) {
    if (!(/iPad|iPhone|iPod/.test(navigator.userAgent))) {
        var prevSibilings = $(elem).parent().prevAll();
        errorFields = prevSibilings.find(".input__error-message:visible").length;
        if (errorFields > 0) {
            elem = $(prevSibilings.find(".input__error-message:visible")[errorFields - 1]);
        }
        $('#lead-popup').animate({
            scrollTop: $(elem).parent().offset().top - $("#lead-popup .details-wrapper").offset().top - 10
        }, 300);
        $(elem).focus();
    }
});

var ListExpand = (function () {
    function registerEvents() {
        $(document).on('click', '.additional-list-count', function () {
            var List = $(this).closest('.suggested-list-details');
            List.addClass('suggested-list--expand');
            $("#additionalDealer").hide();
            cwTracking.trackAction('CWInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_flowB_multiple_dealers_dealer_list_expanded', LeadFormState.SellerRecommendationsCount, 'NA');
        });
    }

    return { registerEvents: registerEvents }
})();


var checkboxListSelection = (function () {
    var allSelector, checkbox, parent, toggleText, button, label;
    function _setSelector() {
        allSelector = '.select_all__js';
        checkbox = '.checkbox-input';
        button = '.lead-form-btn';
    }

    function toggleSelectAll() {
        _setSelector();
        $(document).on('change', allSelector, function () {
            elem = $(this);
            label = elem.closest('.select-all-label').find('.toggle-selection-txt__js')
            parent = elem.closest('.checkbox-list');
            var status = elem.is(':checked');
            if (status) {
                parent.find('.checkbox-input:not(:checked)').each(function () {
                    $(this).attr('checked', true);
                    label.html('Remove All');
                });
                if (elem.closest(".multi-dealer").length > 0) {
                    cwTracking.trackAction('CWInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_' + LeadFormState.FlowText + 'multiple_dealers_select_all_selected', LeadFormState.SellerRecommendationsCount, 'NA');
                }
            }
            else {
                parent.find('.checkbox-input:checked').each(function () {
                    $(this).attr('checked', false);
                    label.html('Select All');
                });
                if (elem.closest(".multi-dealer").length > 0) {
                    cwTracking.trackAction('CWInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_' + LeadFormState.FlowText + 'multiple_dealers_select_all_deselected', LeadFormState.SellerRecommendationsCount, 'NA');
                }
            }
            disabledToggle();
        });
    }

    function toggleCheckBox(element) {
        parent = element.closest('.checkbox-list');
        var child = parent.find('.select_all__js');
        label = parent.find('.toggle-selection-txt__js');
        if (parent.find('.checkbox-input:checked').length === parent.find('.checkbox-input').length) {
            child.attr('checked', true);
            label.html('Remove All');
        }
        else if ($(this).is(':checked') === false) {
            child.attr('checked', false);
            label.html('Select All');
        }
    }

    function disabledToggle() {
        _setSelector();
        if ($(checkbox).is(':checked')) {
            $(button).removeAttr('disabled');
        } else {
            if (!($('#divCustInfo').is(':visible')))
                $(button).prop('disabled', true);
        }
    }

    function checkBoxState() {
        _setSelector();
        $(document).on('change', checkbox, function () {
            var element = $(this);
            toggleCheckBox(element);
            disabledToggle();
            if (element.closest(".multi-dealer").length) {
                var liElement = element.closest('.suggested-list-item');
                if (element.is(":checked")) {
                    cwTracking.trackAction('CWInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_' + LeadFormState.FlowText + 'multiple_dealers_dealer_selected', (liElement.index() + 1) + '/' + LeadFormState.SellerRecommendationsCount, 'NA');
                }
                else {
                    cwTracking.trackAction('CWInteractive', (platform == 43 ? '' : platformTrackingText.toLowerCase() + '_') + 'newcarleadform_' + LeadFormState.FlowText + 'multiple_dealers_dealer_deselected', (liElement.index() + 1) + '/' + LeadFormState.SellerRecommendationsCount, 'NA');
                }
            }
        });
    }

    function registerEvents() {
        toggleSelectAll();
        checkBoxState();
    }

    return { registerEvents: registerEvents }
})();

function LeadFormReady() {
    ListExpand.registerEvents();
    LeadPopup.registerEvents();
    checkboxListSelection.registerEvents();
    Validate.registerEvents($(".lead-form-btn"));
    trackFocusEventsToGAForDealer();
};

var LeadForm = {
    Flows: {
        FlowA: 1,
        FlowB: 2
    },
    FlowType: function () {
        return LeadFormFlowCitiesArray.indexOf(LocationInfo.cityId()) >= 0 ? LeadForm.Flows.FlowB : LeadForm.Flows.FlowA;
    },
    FlowText: function () {
        return LeadFormState.FlowType == LeadForm.Flows.FlowA ? '' : 'flowB_';
    }
};

///Scroll content
var LeadPopupContent = function () {
    function calculateContentHeight() {
        var header = $(".thankyou-header") ? $(".thankyou-header").outerHeight() : 0,
            popupFooter = $(".content-wrapper").not(":hidden") ? $(".content-wrapper").not(":hidden").outerHeight() : 0,
            popupHeight = $("#lead-popup").outerHeight(),
        contentHeight = popupHeight - (header + popupFooter) - 20; // here, 20 is content padding from bottom
        $(".additional-suggested-list").not(":hidden").css("max-height", contentHeight);

    }

    return {
        calculateContentHeight: calculateContentHeight
    }
}();
///End Scroll Content
var emiassist = new EMIAssistance();
var _campaignViewModel;
var _sellerViewModel;
$(document).ready(function () {


});
