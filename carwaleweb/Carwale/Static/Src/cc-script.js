var floatHeader, floatHeaderHt, accordionContainer, footer;
$(document).ready(function () {
    floatHeader = $('#FloatHeader');
    floatHeaderHt = floatHeader.height();
    accordionContainer = $('#accordionContainer');
    footer = $('footer');
    bridgeStoneTyres();
    AdjustspnTitleWidth();
    if (ValVersionCount == 4) {
        if (FeaturedVersionID == -1) {
            $("#tblCVCHeader td:nth-child(4)").css('border-right', '0px');
            $("#tblFloatHeader td:nth-child(4)").css('border-right', '0px');
        }
    }
    $("#tblCVCHeader td:nth-child(5)").css('border-right', '0px');
    $("#tblFloatHeader td:nth-child(5)").css('border-right', '0px');
    HighLightFeaturedCar();
    $(".accordion-tabs").click(function () {
        var a = '#' + this.id + ' a.cc-a-static';
        $(this).next("").slideToggle(250, function () {
            if ($(a).hasClass('cc-up-arrow'))
                $(a).removeClass('cc-up-arrow').addClass('cc-down-arrow');
            else {
                $(a).removeClass('cc-down-arrow').addClass('cc-up-arrow');
            }
        });        
    });
    $("#chkShowDiff").on("click", function () {
        if ($(this).hasClass('chk-box'))
            $(this).removeClass('chk-box').addClass('chk-box-tick');
        else
            $(this).removeClass('chk-box-tick').addClass('chk-box');
        ShowDiff(this);        
    });
    $("#chkHighlightDiff").on("click", function () {
        if ($(this).hasClass('chk-box'))
            $(this).removeClass('chk-box').addClass('chk-box-tick');
        else
            $(this).removeClass('chk-box-tick').addClass('chk-box');
        HighlightDiff(this);        
    });
    $("#chkShowDiffFloat").on("click", function () {
        if ($(this).hasClass('chk-box'))
            $(this).removeClass('chk-box').addClass('chk-box-tick');
        else
            $(this).removeClass('chk-box-tick').addClass('chk-box');
        ShowDiff(this);        
    });
    $("#chkHighlightDiffFloat").on("click", function () {
        if ($(this).hasClass('chk-box'))
            $(this).removeClass('chk-box').addClass('chk-box-tick');
        else
            $(this).removeClass('chk-box-tick').addClass('chk-box');
        HighlightDiff(this);        
    });
    $("#tabsList a").click(function () {
        var showContent = $(this).attr("show_content");
        $('#tabsListFloat a').each(function () {
            if ($(this).attr('show_content') == showContent) {
                if (!$(this).hasClass("active-tab"))
                    $(this).addClass("active-tab").parent().siblings().find("a").removeClass("active-tab");
            }
        });
        if ($("#" + showContent).length == 0)
            $(".accordion-container").append('<div id="' + showContent + '" class="hide">Sorry, no data available.</div>');
        if (!$(this).hasClass("active-tab"))
            $(this).addClass("active-tab").parent().siblings().find("a").removeClass("active-tab");
        $("#" + showContent).siblings().removeClass("show").addClass("hide");
        if (!$("#" + showContent).hasClass("show")) {
            $("#" + showContent).removeClass("hide").addClass("show");
        }        
    });
    $("#tabsListFloat a").click(function () {
        var showContent = $(this).attr("show_content");
        $('#tabsList a').each(function () {
            if ($(this).attr('show_content') == showContent) {
                if (!$(this).hasClass("active-tab")) {
                    $(this).addClass("active-tab").parent().siblings().find("a").removeClass("active-tab");
                }
            }
        });
        if ($("#" + showContent).length == 0)
            $(".accordion-container").append('<div id="' + showContent + '" class="hide">Sorry, data not available.</div>');
        if (!$(this).hasClass("active-tab"))
            $(this).addClass("active-tab").parent().siblings().find("a").removeClass("active-tab");
        if (!$("#" + showContent).hasClass("show")) {
            $("#" + showContent).siblings().removeClass("show").addClass("hide");
            $("#" + showContent).removeClass("hide").addClass("show");
            //ScrollToTop("#data-table-top", 500);
            $('body,html').animate({
                scrollTop: $("#data-table-top").offset().top + 117
            }, 500);
        }       
    });
    $(window).resize(function () {
        FloatingHeaderPosition(floatHeader)
    });
    $(window).scroll(function () {
        FloatingHeaderPosition(floatHeader)
    });
    $('.vName').click(function (event) {
        var vid = $(this).attr('vid');
        $('.ulSelect').each(function () {
            if ($(this).parent().prev().attr('vid') != vid && $(this).is(':visible')) {
                $(this).slideToggle(200);
            }
        });
        $(this).next().find('.ulSelect').eq(0).slideToggle(200);
        $('.liSelect').removeClass('liSelect');
        event.stopPropagation();

    });
    $('.ulSelect li').click(function (event) {
        event.stopPropagation();
        $(this).parent().slideToggle(200);
        if (ValVersionIDs.indexOf($(this).attr('vid')) == -1) {
            var url = window.location.href.replace(/&?source=([^&]$|[^&]*)/i, "");
            var queryIndex = url.indexOf('?');
            if (queryIndex != -1 && queryIndex < url.length - 1)
                window.location = url.toString().replace('=' + $(this).parent().parent().prev().attr('vid'), '=' + $(this).attr('vid'))+"&source=26";
            else
                window.location = canUrl.replace('=' + $(this).parent().parent().prev().attr('vid'), '=' + $(this).attr('vid')) + "&source=26";
        }
        else {
            alert("Please select a different car for comparison.");
        }
    });
    $('.ulSelect li').hover(function () {
        $('.liSelect').removeClass('liSelect');
        $(this).addClass('liSelect');
    });
    $('.vName').keypress(function (evt) {
        if (evt.which == 13 || evt.charCode == 13) {
            if ($(evt.target).hasClass('vName')) {
                if ($(evt.target).next().find('li.liSelect').length != 0) {
                    $('li.liSelect').click();
                }
            }
        }
    });
    $("html").click(function () {
        $('.ulSelect').each(function () {
            if ($(this).is(':visible')) {
                $(this).slideToggle(200);
            }
        });
    });
    $('.vName').each(function () {
        if ($(this).next().find('li').length == 0) {
            $(this).next().remove();
            $(this).removeClass('vName');
            $(this).removeAttr('tabindex');
            $(this).find('.dwnArrow').remove();
        }
    })
    var ulQuickResearchId = document.getElementById('ulQuickResearch');
    if (ulQuickResearchId != undefined && ulQuickResearchId != null) {
        var addCarKVM = eval('(' + genericMakeModelKVM + ')');
        addCarKVM.Makes(carmakejson);
        addCarKVM.Makes.unshift(eval("({ 'makeId': -1, 'makeName': '--Select Make--'})"));
        addCarKVM.Models([{ "ModelId": -1, "ModelName": "--Select Model--", "MaskingName": "" }]);
        addCarKVM.Versions([{ 'ID': -1, 'Name': "--Select Version--" }]);
        ko.applyBindings(addCarKVM, document.getElementById('ulQuickResearch'));
    }
        

    $('#drpMake').change(function () {
        bindModelsList("compareall", $('#drpMake').val(), addCarKVM, "#drpModel", "--Select Model--");
        $('#drpModel').change();
    });
    $('#drpModel').change(function () {
        bindVersionsByModelList("compareall", $('#drpModel').val(), addCarKVM, "#drpVersion", "--Select Version--")
    });    
    initCompareCarsOrpButton();

    $("#trCarImage td > a, #trCarName td > a, #trOrp td > a, #trCarNameFloat td > a, #trOrpFloat td > a").on("click", function () {
        var currentDiv = $(this);
        if (currentDiv.attr('track-sponsored') == "true") {
            var action = currentDiv.attr('data-action');
            var label = currentDiv.attr('data-label');
            Common.utils.trackAction("CWInteractive", "Comparecars_filters", action, label);
        }
    });
    openShowPriceInCityLink();
});
function bridgeStoneTyres() {
    var wheel = $('#CD1 .SubCategoryTable').find('tr[itemMasterId="276"],tr[itemMasterId="202"],tr[itemMasterId="255"],tr[itemMasterId="256"]');
    if (wheel.length > 0) {
        var wheelsAndTyresContainer = "<div class='accordion-tabs' id='cat_bridgeStone'><div class='padding-left10 padding-top10 padding-bottom5 leftfloat'><a class='cc-a-static cc-sprite cc-up-arrow'>";
        wheelsAndTyresContainer += "Wheels & Tyres</a></div><div class='clear'></div></div>";
        wheelsAndTyresContainer += "<div class='accordion-data'><table id='SubCategoryTable5' class='SubCategoryTable'>";
        wheelsAndTyresContainer += "</table></div>";
        $('#CD1').append(wheelsAndTyresContainer);
        $('#SubCategoryTable5').append(wheel);       
    }
}


function checkContainerHt() {
    if (floatHeaderHt > 140) { accordionContainer.css('padding-top', '21px'); }
    else {accordionContainer.css('padding-top', '2px');}
};

function ShowDiff(chk) {
    if ($(chk).hasClass('chk-box-tick')) {
        $("#chkShowDiff").removeClass("chk-box").addClass('chk-box-tick');
        $("#chkShowDiffFloat").removeClass("chk-box").addClass('chk-box-tick');
        var tableCount = $(".SubCategoryTable").length;
        for (var x = 0; x < tableCount; x++) {
            var as = $("#SubCategoryTable" + x + " tr");
            for (var i = 0; i < as.length; i++) {
                var trs = as[i];
                var cellval = new Array();
                for (var j = 1; j < trs.cells.length; j++) {
                    if (trs.cells[j].innerHTML != "&nbsp;" && trs.cells[j].innerHTML != '' && trs.cells[j].innerHTML != "--") {
                        cellval.push(trs.cells[j].innerHTML);
                    }
                }
                var hide = true;
                for (var k = 1; k < cellval.length; k++) {
                    if (cellval[0] != cellval[k]) {
                        hide = false;
                    }
                }
                if (hide) {
                    $(trs).fadeOut(500, function () {
                        $(this).hide(); // needed because fadeOut doesn't work when element is hidden anyway?
                    });
                }
            }
        }
    }
    else {
        $("#chkShowDiff").removeClass("chk-box-tick").addClass('chk-box');
        $("#chkShowDiffFloat").removeClass("chk-box-tick").addClass('chk-box');
        for (var x = 0; x < $(".SubCategoryTable").length; x++) {
            var as = $("#SubCategoryTable" + x + " tr");
            for (var i = 0; i < as.length; i++) {
                $(as[i]).fadeIn(500);
            }
        }

    }
}
function HighlightDiff(chk) {
    if ($(chk).hasClass('chk-box-tick')) {
        $("#chkHighlightDiff").removeClass("chk-box").addClass('chk-box-tick');
        $("#chkHighlightDiffFloat").removeClass("chk-box").addClass('chk-box-tick');
        for (var x = 0; x < $(".SubCategoryTable").length; x++) {
            //var as = $("#SubCategoryTable" + x + " tr");
            $("#SubCategoryTable" + x + " tr").each(function () {
                var cellval = new Array();
                var skipFirst = true;
                $(this.children).each(function () {
                    if (skipFirst == true)
                        skipFirst = false;
                    else {
                        if ($(this)[0].innerHTML != "&nbsp;" && $(this)[0].innerHTML != '' && $(this)[0].innerHTML != "--")
                            cellval.push($(this)[0].innerHTML);
                    }
                });
                var newArray = compressArray(cellval);
                if (newArray.length == 2) {
                    if (!(newArray[0].count == 1 && newArray[1].count == 1)) {
                        if (newArray[0].count == 1) {
                            skipFirst = true;
                            $(this.children).each(function () {
                                if (skipFirst == true)
                                    skipFirst = false;
                                else {
                                    if (newArray[0].value == $(this)[0].innerHTML) {
                                        $(this).css({ "background-color": "#d8dfe6" });
                                    }
                                }
                            });
                        }
                        else {
                            if (newArray[1].count == 1) {
                                $(this.children).each(function () {
                                    if (skipFirst == true)
                                        skipFirst = false;
                                    else {
                                        if (newArray[1].value == $(this)[0].innerHTML) {
                                            $(this).css({ "background-color": "#d8dfe6" });
                                        }
                                    }
                                });
                            }
                        }
                    }
                }
            });
        }
    }
    else {
        $("#chkHighlightDiff").removeClass("chk-box-tick").addClass('chk-box');
        $("#chkHighlightDiffFloat").removeClass("chk-box-tick").addClass('chk-box');
        for (var x = 0; x < $(".SubCategoryTable").length; x++) {
            $("#SubCategoryTable" + x + " tr td").each(function () {
                $(this).css({ "background-color": "" });
            });
        }
    }
}
function HighLightFeaturedCar() {
    if (FeaturedVersionID != -1) {
        $("#tblCVCHeader td:last-child").addClass("cc-sponsored");
        $("#tblFloatHeader td:last-child").addClass("cc-sponsored");
        var colCount = 0;

        $('#tblCVCHeader tr:nth-child(1) td').each(function () {

            if ($(this).attr('colspan')) {
                colCount += +$(this).attr('colspan');
            } else {
                colCount++;
            }
        });
        $(".SubCategoryTable tr").each(function () {
            $(this).find("td:eq(" + colCount + ")").addClass("cc-spsd-colm");
        });
    }
};
function GetCarModels(source, target) {
    $(target).empty().append("<option value\"\">Loading...</option>");
    var onlyNewNeeded = 1;
    $.ajax({
        type: "POST", url: "/ajaxpro/CarwaleAjax.AjaxCompareCars,Carwale.ashx",
        data: '{"makeId":"' + source.val() + '","onlyNew":"' + onlyNewNeeded + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetCarModelHavingSpecs"); },
        success: function (response) {
            var ret_response = eval('(' + response + ')');
            var obj_response = eval('(' + ret_response.value + ')');
            $(target).empty();
            if (obj_response.Table.length > 0) {
                $(target).append("<option value='-1'>--Select Model--</option>");
                for (var i = 0; i < obj_response.Table.length; i++) {
                    $(target).append('<option mask="' + obj_response.Table[i].MaskingName + '" value="' + obj_response.Table[i].Value + '">' + obj_response.Table[i].Text + '</option>');
                }
            }
            else
                target.append("<option value='-1'>No models available</option>");
            target.removeAttr("disabled");
        }
    });
};
function GetCarVersions(source, target) {
    $(target).empty().append("<option value\"\">Loading...</option>");
    var onlyNewNeeded = 1;
    $.ajax({
        type: "POST", url: "/ajaxpro/CarwaleAjax.AjaxCompareCars,Carwale.ashx",
        data: '{"modelId":"' + source.val() + '","onlyNew":"' + onlyNewNeeded + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetCarVersionHavingSpecs"); },
        success: function (response) {
            response = JSON.parse(JSON.parse(response).value);
            target.empty();
            if (response.length > 0) {
                target.append("<option value='-1'>--Select Version--</option>");
                for (var i = 0; i < response.length; i++) {
                    target.append("<option value='" + response[i].ID + "'>" + response[i].Name + "</option>");
                }
            }
            else
                target.append("<option value='-1'>No versions available</option>");
            target.removeAttr("disabled");
        }
    });
};
function compressArray(original) {

    var compressed = [];
    // make a copy of the input array
    var copy = original.slice(0);

    // first loop goes over every element
    for (var i = 0; i < original.length; i++) {

        var myCount = 0;
        // loop over every element in the copy and see if it's the same
        for (var w = 0; w < copy.length; w++) {
            if (original[i] == copy[w]) {
                // increase amount of times duplicate is found
                myCount++;
                // sets item to undefined
                delete copy[w];
            }
        }

        if (myCount > 0) {
            var a = new Object();
            a.value = original[i];
            a.count = myCount;
            compressed.push(a);
        }
    }

    return compressed;
};
function AddCar(versionID) {    
    if (versionID != -1) {
        if (ValVersionIDs.indexOf(versionID) == -1) {
            if ($("#tblCVCHeader tr td").length != 0) {                             
                dataList.push({ id: $('#drpModel').val(), text: formatURL($('#drpMake' + ' option:selected').text()) + '-' + $('#drpModel' + ' option:selected').attr('mask') });
                var currUrl = "/comparecars/" + Common.getCompareUrl(dataList);
                var qs = canUrl.split('?');
                var finalUrl = qs.length > 1 ? currUrl + "/?" + qs[1] + "&c" + (dataList.length) + "=" + versionID+"&source=19" : "";
                window.location = finalUrl;                          
            }
            else {
                window.location = window.location.href + "?c1=" + versionID;
            }
        }
        else
            alert("Please select a different car for comparison.");
    }
    else {
        ShakeFormView($("#acAddCar_ulQuickResearch"));
        alert("Please select a car.");
    }
}
function ScrollToTop(x, y) {
    $('body,html').animate({
        scrollTop: $(x).offset().top
    }, y);
    //return false;
};
$.fn.isOnScreen = function () {

    var win = $(window);

    var viewport = {
        top: win.scrollTop(),
        left: win.scrollLeft()
    };
    viewport.right = viewport.left + win.width();
    viewport.bottom = viewport.top + win.height();

    var bounds = this.offset();
    bounds.right = bounds.left + this.outerWidth();
    bounds.bottom = bounds.top + this.outerHeight();

    return (!(viewport.right < bounds.left || viewport.left > bounds.right || viewport.bottom < bounds.top || viewport.top > bounds.bottom));

};
function AdjustspnTitleWidth() {
    if ($('#spnTitle').width() > 682) {
        $('#spnTitle').width(682);
        $('#spnTitle').css('margin-top', '9px');
    }
};
function FloatingHeaderPosition(floatHeader) {
    
    floatHeader.css({
        top: '50px',
        left: -$(this).scrollLeft() + $("#data-table-top").offset().left
    });
    if ($.browser.msie && $.browser.version == "6.0") {
        floatHeader.css({ "display": "none" });
    }
    else {
        if ($(window).scrollTop() < $("#trCarName").offset().top) {
            floatHeader.fadeOut(0);
            accordionContainer.css('padding-top', '2px');
        }
        else {
            floatHeader.fadeIn(0);
            checkContainerHt();
            if ($(window).scrollTop() > footer.offset().top - 300) {
                floatHeader.fadeOut(0);
                accordionContainer.css('padding-top', '2px');
            }
        }
    }
};


$(function () {
    var count = $('#tblCVCHeader tbody > tr:first-child td').length;
    if (count < 3) {
        $("#chkHighlightDiff").hide();
        $("#chkHighlightDiffFloat").hide();
    }
    $('.innerTableWrap').each(function () {
        $(this).addClass('count' + count);
    });

    if ($('#tblCVCHeader tbody > tr:first-child td').hasClass('cc-sponsored')) {
        var index = $('#tblCVCHeader tbody > tr:first-child td.cc-sponsored').index();
        $('#SubCategoryTable0 .innerTableWrap table tr td').removeClass('cc-spsd-colm');
        $('#SubCategoryTable0 .innerTableWrap table tr').each(function () {
            $('td:eq(' + index + ')', this).addClass("cc-spsd-colm");
        });
    }

    $(document).on("mastercitychange", function (event, cityName, cityId) {
        Common.masterCityPopup.masterCityChange(cityName, cityId);
    });

    $(".advantageLinkDiv:visible").each(function () {
        Common.utils.trackAction('CWNonInteractive', 'deals_desktop', 'dealsimpression_desktop', 'ComparepageDes');
    });   
    userHistory.trackUserHistory(modelIdArray);
});

function formatURL(str) {
    str = str.toLowerCase();
    str = str.replace(/[^0-9a-zA-Z]/g, '');
    return str;
}
document.onkeydown = function (evt) {
    if ($(evt.target).hasClass('vName')) {
        if (evt.charCode == 38 || evt.which == 38) {
            if ($(evt.target).next().find('li.liSelect').length != 0) {
                $('.liSelect').removeClass('liSelect').prev().addClass('liSelect');
            }
            else {
                $(evt.target).next().find('li').eq($(evt.target).next().find('li').length - 1).addClass('liSelect');
            }
            return false;
        }
        if (evt.charCode == 40 || evt.which == 40) {
            if ($(evt.target).next().find('li.liSelect').length != 0) {
                $('.liSelect').removeClass('liSelect').next().addClass('liSelect');
            }
            else {
                $(evt.target).next().find('li').eq(0).addClass('liSelect');
            }
            return false;
        }
    }
};
function HideSponsoredAd(modelName,totalCars)
{
    var tableWidth;// variable created 
    $("#tblCVCHeader td:last-child").remove();
    $("#tblFloatHeader td:last-child").remove();
    $(".accordion-data .SubCategoryTable .cc-spsd-colm").remove();
    $(".UsedCars .innerTableWrap tr td:last-child").remove();
    Common.utils.trackAction('CWInteractive', 'Comparecars_filters', 'Click_' + modelName + ' close', 'Click_' + modelName + ' close');
    // getting the width of the lower table
    var overview = $("#tabsListFloat li");
    var overviewLength = overview.length;
    var showContent;
    for (i = 0; i < overviewLength; i++)
    {
        if($(overview[i]).children("a").hasClass("active-tab"))
        {
            showContent = "#" + $(overview[i]).children("a").attr("show_content");
            break;
        }
    }
    tableWidth = parseInt($(showContent + ' .SubCategoryTable tbody tr td:last-child').outerWidth(),10);
    $("#ulQuickResearch").css("width", tableWidth);
    $("#addCar div").css("width", tableWidth);
    totalCars -= 1;
    var totalWidth = tableWidth * totalCars;
    var usedCarDiv = $(".innerTableWrap");
    usedCarDiv.removeClass("count" + (totalCars + 1)).addClass("count" + (totalCars));
    usedCarDiv.css("width", totalWidth);
}

function initCompareCarsOrpButton() {
    var orpButtons = $('.compare-car-price-link');
    var location = new LocationSearch((orpButtons), {
        showCityPopup: true,
        callback: function (locationObj) {
            var carModelId = location.selector().data('modelid');
            var carVersionId = location.selector().data('versionid');
            var pageId = location.selector().data('pageid');
            var pqInput = { 'modelId': carModelId, 'versionId': carVersionId, 'location': locationObj, 'pageId': pageId };
            PriceBreakUp.Quotation.RedirectToPQ(pqInput);
        },
        isDirectCallback: true,
        validationFunction: function () {
            return PriceBreakUp.Quotation.getGlobalLocation();
        }
    });
}

var compareCarSliderinstance = null;

function openShowPriceInCityLink() {
    var div = $('.select-city-link');
    var location = new LocationSearch((div), {
        showCityPopup: true,
        isAreaOptional: true,
        callback: function (locationObj) {
			window.location.reload();
        },
        isDirectCallback: true,
        validationFunction: function () {
            return PriceBreakUp.Quotation.getGlobalLocation();
        }
    })
}