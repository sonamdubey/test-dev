$(document).ready(function () {
    if ($.trim($("#callslugnumber").text()) && callSlugDisplay) {
        $("#callslug").show();
        $('#footerend').css("padding-bottom", "145px");
    }

    var OnRoadPriceBtn = $('#divPrice').is(":visible");
    var CallDealerBtn = $('#callslugnumber').is(":visible");
    var element = document.getElementById('callslug');
    var PageName;
    if (typeof (element) != 'undefined' && element != null)
    {
        PageName = element.getAttribute("data-value");
    }
    FloatingSlugTracking(OnRoadPriceBtn, CallDealerBtn, PageName);
    if (($('#divPrice').data('clicked')) || ($('#callslugnumber').data('clicked'))) {
        FloatingSlugTracking(OnRoadPriceBtn, CallDealerBtn, PageName, $('#divPrice').data('clicked'), $('#callslugnumber').data('clicked'));
    }

    $('#divPrice').click(function () {
        FloatingSlugTracking(OnRoadPriceBtn, CallDealerBtn, PageName, true);
    });
    $('#callslugnumber').click(function () {
        FloatingSlugTracking('', '', '','', true);
    });
});