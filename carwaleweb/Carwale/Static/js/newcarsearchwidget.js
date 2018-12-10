$(document).ready(function () {
    NewCarLanding.search.registerEvents();
    NewCarLanding.emiSlider();
    $(".brand-type-container a").click(function () {
        var value = $(this).text().trim();
		dataLayer.push({ event: 'CWInteractive', cat: 'NewCarsPage', act: 'BrandBudgetBodytypeValue', lab: 'Brand_' + value });
    });
    $(".budget-container a").click(function () {
        var value = $(this).text().trim().replace(/\s/g, '');
        dataLayer.push({
			event: 'CWInteractive', cat: 'NewCarsPage', act: 'BrandBudgetBodytypeValue', lab: 'Budget_' + value
        });
    });
    $(".body-type-container a").click(function () {
        var value = $(this).text().trim();
		dataLayer.push({ event: 'CWInteractive', cat: 'NewCarsPage', act: 'BrandBudgetBodytypeValue', lab: 'BodyType_' + value });
    });
    $("a.view-more-btn").click(function (e) {
        e.preventDefault();		
        if ($(this).attr("id") == "view-brandLogo") {
            $(".brand-type-container ul").toggleClass("animate-brand-ul");
            var label = $('#view-brandLogo').text().replace(/ /g, '');
            dataLayer.push({
                event: 'CWInteractive', cat: 'NewCarsPage', act: 'BrandBudgetBodytypeLink', lab: label
            });
        }
        else if ($(this).attr("id") == "view-bodyType") {
            var a = $(this).parent().parent().find("ul.brand-body-moreBtn");
            a.slideToggle();
            var label = $('#view-bodyType').text().replace(/ /g, '');
			dataLayer.push({ event: 'CWInteractive', cat: 'NewCarsPage', act: 'BrandBudgetBodytypeLink', lab: label });
        }
        var b = $(this).find("span");
        b.text(b.text() === "more" ? "less" : "more");
    });
    $("ul.brand-budget-body-UL li").click(function () {
        $("ul.brand-body-moreBtn").slideUp();
        $('.view-more-btn').find("span").text("more");        
    });    
});
var NewCarLanding = {
    doc: $(document),
    emiSliderMin: 0,
    emiSliderMax: 60000,

    search: {
        registerEvents: function () {
            NewCarLanding.doc.on('click', '#btnEmiSearch', function () {
                var min = $.trim($("#min").attr("value"));
                var max = $.trim($("#max").attr("value"));
                NewCarLanding.redirectToSearchPage("emi", min + "-" + max);
                cwTracking.trackAction("CWInteractive" , "NewCarsPage" , "EMISearchClick" , min + "-" + max);
            });
            googletag.cmd.push(function () {
                googletag.pubads().addEventListener('slotRenderEnded', function (event) {
                    if (event.isEmpty) {
                        var id = event.slot.getSlotElementId();
                        if (id == "div-gpt-ad-1491317900891-0")
                            NewCarLanding.showHideBodyTypeAdslot(id);
                    }
                });
            });
            if (typeof (adblockDetecter) == 'undefined' || typeof (FirstSlot_976x400) == 'undefined')
                NewCarLanding.showHideBodyTypeAdslot('div-gpt-ad-1491317900891-0');
        }
    },

    showHideBodyTypeAdslot: function (id) {
        var adunit = $('#' + id);
        if (adunit.length > 0)
        {
            adunit.parent().addClass('hide');
        }
            var divId = $('#body-type-grid-9');
            divId.removeClass('grid-9');
            divId.addClass('grid-12');
            $('#truck-body-up').show();
            $('#truck-body-down').hide();                
    },

    emiSlider: function () {
        var rupee = '₹ ';
        var min = NewCarLanding.emiSliderMin;
        var max = NewCarLanding.emiSliderMax;

        $("#emi-range").slider({
            range: true,
            min: min,
            max: max,
            step: 1000,
            values: [min, max],
            animate: '500',
            create: function () {
                $('#min').appendTo($('#emi-range > span').eq(0));
                $('#max').appendTo($('#emi-range > span').eq(1));
            },
            slide: function (event, ui) {
                $(ui.handle).find('span').attr("value", ui.value);
                if (ui.value == max) {
                    $(ui.handle).find('span').html(rupee + NewCarLanding.formateSliderPrice(ui.value));
                }
                else {
                    $(ui.handle).find('span').html(rupee + NewCarLanding.formateSliderPrice(ui.value));
                }
            }
        });

        // only initially needed
        $("#min").attr("value", $('#emi-range').slider('values', 0));
        $("#max").attr("value", $('#emi-range').slider('values', 1));

        $('#min').html(rupee + NewCarLanding.formateSliderPrice($('#emi-range').slider('values', 0))).position({
            at: 'center top',
            of: $('#emi-range > span').eq(0)
        });

        $('#max').html(rupee + (NewCarLanding.formateSliderPrice($('#emi-range').slider('values', 1)))).position({
            at: 'center top',
            of: $('#emi-range > span').eq(1)
        });
    },

    redirectToSearchPage: function (key, value) {
        location.href = "/new/search.aspx#" + key + "=" + value;
    },

    formateSliderPrice: function (price) {
        var formattedPrice = price / 1000;
        if (price == NewCarLanding.emiSliderMax)
            formattedPrice += "K+";
        else if (price != 0)
            formattedPrice += "K";

        return formattedPrice;
    }
};
