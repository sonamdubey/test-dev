var sellCarTracking = (function () {
    var bhriguCategory = "SellCar";
    var GATrackingCategoryMsite = "MsiteSell";
    var GATrackingCategoryDesktop = "DesktopSell";

    var trackingActions = {
        pageLoad: "PageLoad",
        cityPinDone: "CityPinDone",
        citySelectionDone: "CitySelectionDone",
        contactDetailsDone: "ContactDetailsDone",
        mfgMonthAndYearDone: "MfgMonthAndYearDone",
        makeModelVariantandFuelDone: "MakeModelVariantandFuelDone",
        colorSelectionDone: "ColorSelectionDone",
        ownerSelectionDone: "OwnerSelectionDone",
        kmsAdded: "KmsAdded",
        recommendedPriceShown: "RecommendedPriceShown",
        recommendedPriceSelected: "RecommendedPriceSelected",
        expectedPriceFilled: "ExpectedPriceFilled",
        otpShown: "OtpShown",
        otpNumberChanged: "OtpNumberChanged",
        otpVerified: "OtpVerified",
        listingLive: "ListingLive",
        imageUploaded: "ImageUploaded",
        accidentalQuestion: "AccidentalQuestion",
        partsReplacedQuestion: "PartsReplacedQuestion",
        insuranceClaimedQuestion: "InsuranceClaimedQuestion",
        serviceQuestion: "ServiceQuestion",
        loanQuestion: "LoanQuestion",
        tyreConditionQuestion: "TyreConditionQuestion",
        wearTearQuestion: "WearTearQuestion",
        mechanicalIssueQuestion: "MechanicalIssueQuestion",
        congratsPageShown: "CongratsPageShown",
        voucherShown: "VoucherShown",
        voucherClicked: "VoucherClicked",
        viewAdClicked: "ViewAdClicked",
        userNavigateAway: "UserNavigateAway",
        topNavigationClicked:"TopNavigationClicked",
        //desktop extra tracking action
        mfgYearDone: "MfgYearDone",
        mfgMonthDone: "MfgMonthDone",
        makeSelectionDone: "MakeSelectionDone",
        modelSelectionDone: "ModelSelectionDone",
        versionSelectionDone: "VersionSelectionDone",
        alternateFuelSelectionDone: "AlternateFuelSelectionDone",
    }


    function forMobile(action, label) {
        switch (action.toLowerCase()) {
            case "pageload":
                Common.utils.trackAction("CWNonInteractive", GATrackingCategoryMsite, trackingActions.pageLoad, "PageLoad");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.pageLoad, dataForBhrigu(), false);
                break;
            case "pin":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.cityPinDone, "CityPinDone");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.cityPinDone, dataForBhrigu(), false);
                break;
            case "contact":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.contactDetailsDone, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.contactDetailsDone, dataForBhrigu(trackingActions.contactDetailsDone, label), false);
                break;
            case "mfgyear":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.mfgMonthAndYearDone, "MfgMonthAndYearDone");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.mfgMonthAndYearDone, dataForBhrigu(), false);
                break;
            case "mmv":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.makeModelVariantandFuelDone, "MakeModelVariantandFuelDone");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.makeModelVariantandFuelDone, dataForBhrigu(), false);
                break;
            case "color":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.colorSelectionDone, "ColorSelectionDone");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.colorSelectionDone, dataForBhrigu(), false);
                break;
            case "owner":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.ownerSelectionDone, "OwnerSelectionDone");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.ownerSelectionDone, dataForBhrigu(), false);
                break;
            case "kms":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.kmsAdded, "KmsAdded");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.kmsAdded, dataForBhrigu(), false);
                break;
            case "recompriceshown":
                Common.utils.trackAction("CWNonInteractive", GATrackingCategoryMsite, trackingActions.recommendedPriceShown, "RecommendedPriceShown");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.recommendedPriceShown, dataForBhrigu(), false);
                break;
            case "recompriceselect":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.recommendedPriceSelected, "RecommendedPriceSelected");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.recommendedPriceSelected, dataForBhrigu(), false);
                break;
            case "expectprice":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.expectedPriceFilled, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.expectedPriceFilled, dataForBhrigu(trackingActions.expectedPriceFilled, label), false);
                break;
            case "otpshown":
                Common.utils.trackAction("CWNonInteractive", GATrackingCategoryMsite, trackingActions.otpShown, "OtpShown");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.otpShown, dataForBhrigu(), false);
                break;
            case "otpnumberchanged":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.otpNumberChanged, "OtpNumberChanged");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.otpNumberChanged, dataForBhrigu(), false);
                break;
            case "otpverified":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.otpVerified, "OtpVerified");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.otpVerified, dataForBhrigu(), false);
                break;
            case "live":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.listingLive, "ListingLive");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.listingLive, dataForBhrigu(), false);
                break;
            case "images":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.imageUploaded, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.imageUploaded, dataForBhrigu(trackingActions.imageUploaded, label), false);
                break;
            case "accidentalquestion":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.accidentalQuestion, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.accidentalQuestion, dataForBhrigu(action, label), false);
                break;
            case "partsreplacedquestion":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.partsReplacedQuestion, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.partsReplacedQuestion, dataForBhrigu(action, label), false);
                break;
            case "insuranceclaimedquestion":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.insuranceClaimedQuestion, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.insuranceClaimedQuestion, dataForBhrigu(action, label), false);
                break;
            case "servicequestion":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.serviceQuestion, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.serviceQuestion, dataForBhrigu(action, label), false);
                break;
            case "loanquestion":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.loanQuestion, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.loanQuestion, dataForBhrigu(action, label), false);
                break;
            case "tyreconditionquestion":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.tyreConditionQuestion, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.tyreConditionQuestion, dataForBhrigu(action, label), false);
                break;
            case "weartearquestion":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.wearTearQuestion, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.wearTearQuestion, dataForBhrigu(action, label), false);
                break;
            case "mechanicalissuequestion":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.mechanicalIssueQuestion, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.mechanicalIssueQuestion, dataForBhrigu(action, label), false);
                break;
            case "congrats":
                Common.utils.trackAction("CWNonInteractive", GATrackingCategoryMsite, trackingActions.congratsPageShown, "CongratsPageShown");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.congratsPageShown, dataForBhrigu(), false);
                break;
            case "vouchershown":
                Common.utils.trackAction("CWNonInteractive", GATrackingCategoryMsite, trackingActions.voucherShown, "VoucherShown");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.voucherShown, dataForBhrigu(), false);
                break;
            case "voucherclicked":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.voucherClicked, "VoucherClicked");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.voucherClicked, dataForBhrigu(), false);
                break;
            case "viewad":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.viewAdClicked, "ViewAdClicked");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.viewAdClicked, dataForBhrigu(), false);
                break;
            case "close":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.userNavigateAway, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.userNavigateAway, dataForBhrigu(action, label), false);
                break;
            case "topnavigationclicked":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryMsite, trackingActions.topNavigationClicked, label);
        }
    };
    function forDesktop(action, label) {        
        switch (action.toLowerCase()) {
            case "pageload":
                Common.utils.trackAction("CWNonInteractive", GATrackingCategoryDesktop, trackingActions.pageLoad, "PageLoad");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.pageLoad, dataForBhrigu(), false);
                break;
            case "city":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.citySelectionDone, "CitySelectionDone");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.citySelectionDone, dataForBhrigu(), false);
                break;
            case "contact":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.contactDetailsDone, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.contactDetailsDone, dataForBhrigu(trackingActions.contactDetailsDone, label), false);
                break;
            case "caryearform":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.mfgYearDone, "MfgYearDone");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.mfgYearDone, dataForBhrigu(), false);
                break;
            case "carmonthform":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.mfgMonthDone, "MfgMonthDone");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.mfgMonthDone, dataForBhrigu(), false);
                break;
            case "carmakeform":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.makeSelectionDone, "MakeSelectionDone");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.makeSelectionDone, dataForBhrigu(), false);
                break;
            case "carmodelform":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.modelSelectionDone, "ModelSelectionDone");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.modelSelectionDone, dataForBhrigu(), false);
                break;
            case "carversionform":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.versionSelectionDone, "VersionSelectionDone");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.versionSelectionDone, dataForBhrigu(), false);
                break;
            case "bodycolorform":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.colorSelectionDone, "ColorSelectionDone");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.colorSelectionDone, dataForBhrigu(), false);
                break;
            case "alternatefuelform":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.alternateFuelSelectionDone, "AlternateFuelSelectionDone");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.alternateFuelSelectionDone, dataForBhrigu(), false);
                break;
            case "bodyownerform":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.ownerSelectionDone, "OwnerSelectionDone");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.ownerSelectionDone, dataForBhrigu(), false);
                break;
            case "recompriceshown":
                Common.utils.trackAction("CWNonInteractive", GATrackingCategoryDesktop, trackingActions.recommendedPriceShown, "RecommendedPriceShown");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.recommendedPriceShown, dataForBhrigu(), false);
                break;
            case "recompriceselect":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.recommendedPriceSelected, "RecommendedPriceSelected");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.recommendedPriceSelected, dataForBhrigu(), false);
                break;
            case "expectprice":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.expectedPriceFilled, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.expectedPriceFilled, dataForBhrigu(trackingActions.expectedPriceFilled, label), false);
                break;
            case "otpshown":
                Common.utils.trackAction("CWNonInteractive", GATrackingCategoryDesktop, trackingActions.otpShown, "OtpShown");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.otpShown, dataForBhrigu(), false);
                break;
            case "otpnumberchanged":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.otpNumberChanged, "OtpNumberChanged");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.otpNumberChanged, dataForBhrigu(), false);
                break;
            case "otpverified":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.otpVerified, "OtpVerified");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.otpVerified, dataForBhrigu(), false);
                break;
            case "live":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.listingLive, "ListingLive");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.listingLive, dataForBhrigu(), false);
                break;
            case "carinsuranceform":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, "CarInsuranceDone", "CarInsuranceDone");
                cwTracking.trackCustomData(bhriguCategory, "CarInsuranceDone", dataForBhrigu(), false);
                break;
            case "images":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.imageUploaded, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.imageUploaded, dataForBhrigu(trackingActions.imageUploaded, label), false);
                break;
            case "accidentalquestion":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.accidentalQuestion, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.accidentalQuestion, dataForBhrigu(action, label), false);
                break;
            case "partsreplacedquestion":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.partsReplacedQuestion, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.partsReplacedQuestion, dataForBhrigu(action, label), false);
                break;
            case "insuranceclaimedquestion":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.insuranceClaimedQuestion, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.insuranceClaimedQuestion, dataForBhrigu(action, label), false);
                break;
            case "servicequestion":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.serviceQuestion, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.serviceQuestion, dataForBhrigu(action, label), false);
                break;
            case "loanquestion":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.loanQuestion, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.loanQuestion, dataForBhrigu(action, label), false);
                break;
            case "tyreconditionquestion":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.tyreConditionQuestion, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.tyreConditionQuestion, dataForBhrigu(action, label), false);
                break;
            case "weartearquestion":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.wearTearQuestion, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.wearTearQuestion, dataForBhrigu(action, label), false);
                break;
            case "mechanicalissuequestion":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.mechanicalIssueQuestion, label);
                cwTracking.trackCustomData(bhriguCategory, trackingActions.mechanicalIssueQuestion, dataForBhrigu(action, label), false);
                break;
            case "congrats":
                Common.utils.trackAction("CWNonInteractive", GATrackingCategoryDesktop, trackingActions.congratsPageShown, "CongratsPageShown");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.congratsPageShown, dataForBhrigu(), false);
                break;
            case "vouchershown":
                Common.utils.trackAction("CWNonInteractive", GATrackingCategoryDesktop, trackingActions.voucherShown, "VoucherShown");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.voucherShown, dataForBhrigu(), false);
                break;
            case "voucherclicked":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.voucherClicked, "VoucherClicked");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.voucherClicked, dataForBhrigu(), false);
                break;
            case "viewad":
                Common.utils.trackAction("CWInteractive", GATrackingCategoryDesktop, trackingActions.viewAdClicked, "ViewAdClicked");
                cwTracking.trackCustomData(bhriguCategory, trackingActions.viewAdClicked, dataForBhrigu(), false);
                break;            
        }
    };

    function dataForBhrigu(action, label) {
        var trackingParam = {};
        trackingParam['tmpid'] = $.cookie("TempInquiry");
        trackingParam['inqid'] = $.cookie("SellInquiry");
        trackingParam['source'] = parentContainer.getSourceId();
        trackingParam['abtest'] = $.cookie("_abtest");
        if (action && label) {
            if (action == "close") {
                trackingParam[action] = label.replace(/ /g, '').toLowerCase();
            }
            else {
                trackingParam[action] = label;
            }
        }
        return cwTracking.prepareLabel(trackingParam);
    };
    return {
        forMobile: forMobile,
        forDesktop : forDesktop
    };
})();