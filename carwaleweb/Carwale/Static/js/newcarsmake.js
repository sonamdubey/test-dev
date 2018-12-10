
var MakePage = {
    prefillDropDown: function (drp) {
        var cityId = Number($.cookie("_CustCityIdMaster"));
        if(cityId>0){
            $(drp).find("[value='" + cityId + "']").attr('selected', 'selected');
        }
    },

    ShowAllDisModels: function () {
        $("#disModels").hide();
        $("#discontinuedModels").show();
    },

    InsertAd: function () {
        if (makeId == 16)//Show add on tata cars
        {
            var ad1 = "<div class=\"sponsored-model\"><div class=\"adunit sponsored\" data-adunit=\"NewCar_Make_622x180\" data-dimensions=\"622x180\"></div></div>";
            $(".list-seperator").eq(1).after(ad1);

            $.dfp({
                dfpID: '1017752',
                enableSingleRequest: false,
                collapseEmptyDivs: true,
                afterEachAdLoaded: function (adunit) {
                    var parentAdUnit = $(adunit).parent();
                    if ($(adunit).hasClass('display-block')) parentAdUnit.removeClass("hide");
                    else if (parentAdUnit.hasClass('sponsored-model'))
                        parentAdUnit.addClass("hide");
                }
            });
        }
    },

    initLocationPluginForORP: function () {
        var div = $('.btnShowOnRoadPrice');
        var location = new LocationSearch((div), {
            showCityPopup: true,
            callback: function (locationObj) {
                var modelMaskingName = location.selector().data('modelmaskingname');
                var makeMaskingName = location.selector().data('makemaskingname');
                NewCar_Common.redirectToModelPage(makeMaskingName, modelMaskingName);
            },
            isDirectCallback: true,
            isAreaOptional: true,
            validationFunction: function () {
                return PriceBreakUp.Quotation.getGlobalLocation();
            }
        });
    },

    initLocationPluginForVPB: function () {
        var div = $('.btnViewPriceBreakup');
        var location = new LocationSearch((div), {
            showCityPopup: true,
            callback: function (locationObj) {
                var carModelId = location.selector().data('model');
                var pageId = location.selector().data('pageid');
                PriceBreakUp.Quotation.RedirectToPQ({ 'modelId': carModelId, 'location': locationObj, 'pageId': pageId });
            },
            isDirectCallback: true,
            validationFunction: function () {
                return PriceBreakUp.Quotation.getGlobalLocation();
            }
        });
    }
}

$(document).ready(function () {
	$("a.view-more-btn").click(function (e) {
		e.preventDefault();
			$(".brand-type-container ul").toggleClass("animate-brand-ul");
			var label = $('#view-brandLogo').text().replace(/ /g, '')
		var b = $(this).find("span");
		b.text(b.text() === "more" ? "less" : "more");
	});
    if ($("#discontinuedModels").length > 0) {                    
        if ($("#discontinuedModels a").length > 2) {
            $("#disModels").show();
            $("#discontinuedModels").hide();
            $("#spnContent").append($("#discontinuedModels a:first").clone());
            $("#spnContent").append("<span>, </span>");
            $("#spnContent").append($("#discontinuedModels a:first").next().clone());            
            $("#spnContent").append("...<a class='f-small' style='cursor:pointer;' onclick='MakePage.ShowAllDisModels()'>View All</a>");
        }
    }
    $('img.lazy').lazyload();
    $(".advantage").each(function () {
        var element = $(this);
        if (element.is(":visible")) {
            Common.utils.trackAction('CWNonInteractive', 'deals_desktop', 'dealsimpression_desktop', 'MakepageDesktop');
        }
    });

    $(".electric-cars").each(function () {
        var element = $(this);
        if (element.is(":visible")) {
            var modelName = element.attr("modelName");
            var label = "Impression_MakePage_"+modelName;
            Common.utils.trackAction('CWNonInteractive', 'CWSpecials', 'ElectricSectionLinks', label);
        }
    });

    $('.advantage').click(function () {
        var element = $(this);
        if (element.is(":visible")) {
            Common.utils.trackAction('CWInteractive', 'deals_desktop', 'dealsaccess_desktop', 'MakepageDesktop');
        }

    });

    $(document).on("mastercitychange", function (event, cityName, cityId) {
		Common.masterCityPopup.masterCityChange(cityName, cityId);
    });
    MakePage.prefillDropDown($("#locateDealerCities"));
    MakePage.InsertAd();
    MakePage.initLocationPluginForORP();
	MakePage.initLocationPluginForVPB();

});


function FullDesc() {
    $("#divFullDesc").show();
    $("#divShortDesc").hide();
}

function HideDesc() {
    $("#divShortDesc").show();
    $("#divFullDesc").hide();
}


