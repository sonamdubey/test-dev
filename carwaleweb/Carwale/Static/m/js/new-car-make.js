$(document).ready(function () {

    $(".advantage").each(function () {
        var element = $(this);
        if (element.is(":visible")) {
            Common.utils.trackAction('CWNonInteractive', 'deals_mobile', 'dealsimpression_mobile', "Makepagemobile");
        }
    });

    $('.advantage').click(function () {
        var element = $(this);
        if (element.is(":visible")) {
            Common.utils.trackAction('CWInteractive', 'deals_mobile', 'dealsaccess_mobile', "Makepagemobile");
        }

    });

    $(document).on("mastercitychange", function (event, cityName, cityId) {
        Common.masterCityPopup.masterCityChange(cityName, cityId);
    });
    if (makeId == 16)
        $.insertAd();

    $("a.view-more-btn").click(function (e) {
        e.preventDefault();
        $(".brand-type-container ul").toggleClass("animate-brand-ul");
        var label = $('#view-brandLogo').text().replace(/ /g, '')
        var b = $(this).find("span");
        b.text(b.text() === "more" ? "less" : "more");
    });

    $(".colorCircle__js").click(function () {
        $('#toastBox').text("No colour images available for " + $(this).parents('Li.hatchback-content-box').data('modelname')).show();
        setTimeout(function () {
            $('#toastBox').fadeOut('slow')
        },3000);
    });
    $(".versionlink__js").click(function () {
        var versionLink = $(this);        
        var versionContainer = versionLink.closest("li").find('div.versionlistcontainer__js');
        if (!versionContainer.hasClass("active")) {
            if (!versionContainer.hasClass("versionloaded__js")) {
                var loaderHtml = $("#oxygenloader").html();
                versionContainer.append(loaderHtml);
                $.get("/make/action/versionlist/", { modelId: versionLink.data("modelid"), cityId: versionLink.data("cityid"), makeName: versionLink.data("makename") }).done(function (response) {
                    versionContainer.html(response);
                    versionContainer.addClass("versionloaded__js");
                });
            }
            Common.utils.trackAction('CWInteractive', 'MakePage', 'VariantsLink-open-Click', versionLink.data("label"));
            versionContainer.addClass("active");
            versionLink.find(".plussymbol__js").html("-");
        }
        else {
            versionContainer.removeClass("active");
            versionLink.find(".plussymbol__js").html("+");
            Common.utils.trackAction('CWInteractive', 'MakePage', 'VariantsLink-close-Click', versionLink.data("label"));
        }
    });
});

function FullDesc() {
    $("#divFullDesc").show();
    $("#divShortDesc").hide();
}

function HideDesc() {
    $("#divShortDesc").show();
    $("#divFullDesc").hide();
}

$.insertAd = function () {
    var ad1 = "<div class=\"rounded-corner2 sponsored-model\"><div class=\"adunit sponsored\" data-adunit=\"CarWale_Mobile_Make_278x80\" data-dimensions=\"278x80\"></div></div>";
    $("div.content-box-shadow").eq(1).after(ad1);

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
};