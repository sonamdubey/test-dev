var bikeCityId, selCityId, key, ddlCities, ddlMakes;


docReady(function () {

ddlCities = $("#ddlCities"), ddlMakes = $("#ddlMakes");
bikeCityId = $("#ddlCities").val();
selCityId = $('#page-data').data('cityid');
bwcache.removeAll(true);

key = "ServiceCenterCitiesByMake_";
bwcache.setScope('SCPage');

$(function () {
                
    $('select').prop('selectedIndex', 0);

    $("#applyFiltersBtn").click(function () {
        ddlmakemasking = $("#ddlMakes option:selected").attr("maskingName");
        ddlcityId = $("#ddlCities option:selected").val();
        ddlmakeId = $("#ddlMakes option:selected").val();
        if (!isNaN(ddlmakeId) && ddlmakeId != "0") {
            if (!isNaN(ddlcityId) && ddlcityId != "0") {
                ddlcityMasking = $("#ddlCities option:selected").attr("maskingName");
                bwcache.remove("userchangedlocation", true);
                window.location.href = "/service-centers/" + ddlmakemasking + "/" + ddlcityMasking + "/";
            }
            else {
                toggleErrorMsg(ddlCities, true, "Choose a city");
            }
        }
        else {
            toggleErrorMsg(ddlMakes, true, "Choose a brand");
        }
    });        

    ddlMakes.change(function () {
        toggleErrorMsg(ddlMakes, false);
        selMakeId = ddlMakes.val();
        ddlCities.empty();        
        if (!isNaN(selMakeId) && selMakeId != "0") {
            if (!checkCacheCityAreas(selMakeId)) {
                $.ajax({
                    type: "GET",
                    url: "/api/servicecenter/cities/make/" + selMakeId + "/",
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (data) {
                        bwcache.set(key + selMakeId, data, 30);
                        setOptions(data);
                    },
                    complete: function (xhr) {
                        if (xhr.status == 404 || xhr.status == 204) {
                            bwcache.set(key + selMakeId, null, 30);
                            setOptions(null);
                        }
                    }
                });
            }
            else {
                data = bwcache.get(key + selMakeId.toString());
                setOptions(data);
            }
        }
        else {
            setOptions(null);
        }
    });

    ddlCities.change(function () {
        toggleErrorMsg(ddlCities, false);
    });

    if ($("#ddlCities option").length < 2) {
        ddlCities.empty();
        ddlCities.trigger('chosen:updated');
        $("#ddlCities_chosen .chosen-single span").text("Select City");
    }
});

function checkCacheCityAreas(cityId) {
    bKey = key + cityId;
    if (bwcache.get(bKey)) return true;
    else return false;
}

function setOptions(optList) {
    if (optList != null) {
        ddlCities.append($('<option>').text(" Select City ").attr({ 'value': "0" }));
        $.each(optList, function (i, value) {
            ddlCities.append($('<option>').text(value.cityName).attr({ 'value': value.cityId, 'maskingName': value.cityMaskingName }));
        });
    }
    else {
                  
        $("#ddlCities_chosen .chosen-single.chosen-default span").text("No cities available");
    }
    if (optList) {
        var selectedElement = $.grep(optList, function (element, index) {
            return element.cityId == selCityId;
        });
        if (selectedElement.length > 0) {
            $('#ddlCities').val(selectedElement[0].cityId);
                       
        }
    }
    ddlCities.trigger('chosen:updated');
}

ddlCities.chosen({ no_results_text: "No matches found!!" });
ddlMakes.chosen({ no_results_text: "No matches found!!" });
$('div.chosen-container').attr('style', 'width:100%;border:0');

// faqs
$('.accordion-list').on('click', '.accordion-head', function () {
    var element = $(this);

    if (!element.hasClass('active')) {
        accordion.open(element);
    }
    else {
        accordion.close(element);
    }
});

var accordion = {
    open: function (element) {
        var elementSiblings = element.closest('.accordion-list').find('.accordion-head.active');
        elementSiblings.removeClass('active').next('.accordion-body').slideUp();

        element.addClass('active').next('.accordion-body').slideDown();
    },

    close: function (element) {
        element.removeClass('active').next('.accordion-body').slideUp();
    }
};

});