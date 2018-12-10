// model popup
var modelPopup = {
    open: function (container) {
        $(container).show();
        $('.modal-background').show();
        $('body').addClass('lock-browser-scroll');
    },

    close: function (container) {
        $(container).hide();
        $(".modal-background").hide();
        $('body').removeClass('lock-browser-scroll');
    }
};

/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');
};

var tyresListPage = {
    tyresListNextPageNo: 0,
    modelIds: 0,
    pageSize: 16,
    versionId: 0,
    processedTyres: [],
    isVersionChanged: false,
    tyreListSelector: $('#tyresList ul'),
    versionTargetSelector: $('#target-version-popup'),
    versionPopup: $('#version-selection-popup'),
    lodingPopupSelector: $('.loading-popup'),
    tyreInfoPopup: $('#tyre-info-popup'),
    makeName: '',
    modelName: '',
    year: '',

    registerEvents: function () {
        tyresListPage.lodingPopupSelector.show();
        tyresListPage.year = Common.utils.getValueFromQS('year');
        $('#tyre-info').on('click', function () {
            $('.tyre-info-img').attr('src', 'https://imgd.aeplcdn.com/0x0/cw/es/tyres/tyre-info2.jpg');
            modelPopup.open(tyresListPage.tyreInfoPopup);
            appendState("tyre-info-popup");
        });
        $('.tyre-info-close').on('click', function () {
            modelPopup.close(tyresListPage.tyreInfoPopup);
            modelPopup.close($(this).closest('.modal-popup-container'));
            window.history.back();
        });

        var url = window.location.href.toLowerCase();
        if (url.indexOf("cmids") > 0)
            tyresListPage.modelIds = Common.utils.getValueFromQS('cmids');
        if (url.indexOf("versionid") > 0) {
            tyresListPage.versionId = tyresListPage.getVersionIdFromUrl();
        }

        tyresListPage.bindTyresData();
        window.addEventListener('scroll', tyresListPage.scrollHandler, false);

        tyresListPage.versionPopup.on('click', '.popup-list li', function () {
            appendState('versionPopup');
            tyresListPage.isVersionChanged = true;
            tyresListPage.processedTyres.length = 0;
            var listElement = $(this);

            if(!listElement.hasClass('active')) {
                if(listElement.hasClass('list-group-label')) {
                    return;
                }
                else {
                    listElement.siblings().removeClass('active');
                    listElement.addClass('active')
                    tyresListPage.versionTargetSelector.text(listElement.text());

                    tyresListPage.versionId = listElement.attr('data-value');
                    tyresListPage.tyresListNextPageNo = 0;
                    tyresListPage.bindTyresData();
                    modelPopup.close(tyresListPage.versionPopup);
                    var currentUrl = Common.utils.updateQSParam(url, 'versionid', tyresListPage.versionId);
                    history.replaceState(null, null, currentUrl);
                    Common.utils.trackAction('CWInteractive', 'Tyres_Section_d', ' Version selected', tyresListPage.modelName + '_' + listElement.text() + '_' + tyresListPage.year);
                }
            }
        });

        tyresListPage.versionTargetSelector.on('click', function () {
            modelPopup.open(tyresListPage.versionPopup);
            Common.utils.trackAction('CWInteractive', 'Tyres_Section_d', 'Select version clicked', tyresListPage.modelName + '_' + tyresListPage.year);
        });

        $('.popup-cross').on('click', function () {
            modelPopup.close($(this).closest('.modal-popup-container'));
        });
        
        $('.modal-background').on('click', function () {
            if(tyresListPage.versionPopup.is(':visible')) {
                modelPopup.close(tyresListPage.versionPopup);
            }
            if (tyresListPage.tyreInfoPopup.is(':visible')) {
                modelPopup.close(tyresListPage.tyreInfoPopup);
                window.history.back();
            }
        });        
    },
    getVersionIdFromUrl: function() {
        var versionId = Common.utils.getValueFromQS('versionid');
        $('#version-selection-popup .popup-list li').each(function () {
            var self = $(this);
            if (self.attr('data-value') == versionId) {
                tyresListPage.versionTargetSelector.text(self.text());
                self.addClass('active');
            }
        });
        return versionId;
    },
    bindTyresData: function () {
        tyresListPage.tyresListNextPageNo += 1;
        var noDataFoundElement = $('#noDataFound');
        var _pageNo = tyresListPage.tyresListNextPageNo;
        var url = '/tyresList/?pageno=' + _pageNo + '&platformid=' + 1 + '&pagesize=' + tyresListPage.pageSize + (tyresListPage.versionId > 0 ? '&versionid=' + tyresListPage.versionId : '&modelids=' + tyresListPage.modelIds);
        $.when(Common.utils.ajaxCall(url)).done(function (data) {

            tyresListPage.lodingPopupSelector.hide();
            if (data != null && $.trim(data) != "") {
                noDataFoundElement.hide();
                tyresListPage.isVersionChanged ? tyresListPage.tyreListSelector.empty().append(data) : (_pageNo > 1 ? tyresListPage.tyreListSelector.find('li:last').after(data) : tyresListPage.tyreListSelector.append(data));
                $("#spnTyreCount").text($("#hdnTyreCount").val());
                var tyreSize = $("#hdnVersionTyreSize").val();

                if (tyreSize) {
                    $('.car-tyre-size').show();
                    $('#spnVersionTyreSize').text(tyreSize);
                }
                else
                    $('.car-tyre-size').hide();

                tyresListPage.isVersionChanged = false;
                $('img.lazy').lazyload();
            }
            else if (_pageNo == 1) {
                tyresListPage.tyreListSelector.empty();
            }

            tyresListPage.makeName = noDataFoundElement.attr('makeName');
            tyresListPage.modelName = $.trim(noDataFoundElement.attr('modelName'));

            var label = tyresListPage.modelName + '_' + tyresListPage.year + (tyresListPage.versionId > 0 ? '_' + $.trim(tyresListPage.versionTargetSelector.text()) : '');
            if(tyresListPage.tyreListSelector.find('li').children().length == 0)
            {
                tyresListPage.tyreListSelector.empty();
                if (tyresListPage.versionId <= 0)
                    $('#carData').hide();
                label = tyresListPage.makeName + '_' + label;
                tyresListPage.impressionTracking(label);
                var noDataFoundHtml = '<h1 class="text-black font18 margin-bottom10">Oops!</h1><p class="font14 text-bold">Sorry we couldn\'t find any tyre for your<span class="text-black block font13">' + (tyresListPage.makeName != undefined ? tyresListPage.makeName : '') + ' ' + (tyresListPage.modelName != undefined ? tyresListPage.modelName : '') + '<span id="versionName">' + (tyresListPage.versionId > 0 ? (' ' + tyresListPage.versionTargetSelector.text()) : '') + '</span></span></p><img src="https://imgd.aeplcdn.com/0x0/cw/es/tyres/tyres-no-result.png" alt="No Tyres Found" title="No Tyres Found">';
                noDataFoundElement.html(noDataFoundHtml);
                noDataFoundElement.show();
            }
            if (tyresListPage.tyreListSelector.find('li').hasClass('sponsored') && _pageNo == 1) {
                Common.utils.trackAction('CWNonInteractive', 'Tyres_sponsored_d', 'apollo_Impression', tyresListPage.makeName + "_" + label);
                $('.carDescWrapper.sponsored a, .imageWrapper.sponsored a').attr('data-label', tyresListPage.makeName + "_" + label);
            }
            else {
                Common.utils.trackAction('CWInteractive', 'Tyres_Section_d', 'Result found SRP', label);
            }           
        });
    },

    scrollHandler: function () {
        var visibleNextPageTrigger = $(".next-page-trigger:in-viewport");
        if (visibleNextPageTrigger.length > 0) {
            var visibleId = visibleNextPageTrigger[0].getAttribute('id');
            if (tyresListPage.processedTyres.indexOf(visibleId) < 0 && tyresListPage.tyreListSelector.find('li').length < $("#spnTyreCount").text()) {
                tyresListPage.processedTyres.push(visibleId);
                tyresListPage.lodingPopupSelector.show();
                tyresListPage.bindTyresData();
            }
        }
    },

    impressionTracking: function (label) {
        Common.utils.trackAction('CWNonInteractive', 'Tyres_Section_d', 'No_results_found_SRP_' + label, label);
    }
}

$(window).on('popstate', function (event) {
    tyresListPage.isVersionChanged = true;
    tyresListPage.processedTyres.length = 0;
    if (tyresListPage.versionPopup.is(':visible')) {
        modelPopup.close(tyresListPage.versionPopup);
    }
    if (window.location.href.toLowerCase().indexOf("versionid") > 0) {
        $('#version-selection-popup .popup-list li.active').removeClass('active');
        tyresListPage.versionId = tyresListPage.getVersionIdFromUrl();
    }
    else {
        tyresListPage.versionId = 0;
        tyresListPage.versionTargetSelector.text("Select Version");
        $('#version-selection-popup .popup-list li.active').removeClass('active');
        tyresListPage.tyreListSelector.empty();
    }
    if (tyresListPage.tyreInfoPopup.is(':visible')) {
        modelPopup.close(tyresListPage.tyreInfoPopup);
    }
    tyresListPage.tyresListNextPageNo = 0;
    tyresListPage.bindTyresData();
});

$(document).ready(function () {
    tyresListPage.registerEvents();
});

$(document).keyup(function (e) {
    if ((e.keyCode === 27) && ($('#tyre-info-popup').is(":visible"))) {
        modelPopup.close(tyresListPage.tyreInfoPopup);
        modelPopup.close($(this).closest('.modal-popup-container'));
        window.history.back();
    }
});
