var ddlCities , ddlMakes ;
var bikeCityId ;

var key = "dealerCitiesByMake_";

docReady(function () {
    bwcache.removeAll(true);
    bwcache.setScope('DLPage');

    ddlCities = $("#ddlCities"), ddlMakes = $("#ddlMakes");
    bikeCityId = $("#ddlCities").val()
    
    
    ddlCities.chosen({ no_results_text: "No matches found!!" });
    ddlMakes.chosen({ no_results_text: "No matches found!!" });
    $('div.chosen-container').attr('style', 'width:100%;border:0');
    $('select').prop('selectedIndex', 0);

    $("#applyFiltersBtn").click(function () {
        ddlmakemasking = $("#ddlMakes option:selected").attr("maskingName");
        ddlcityId = $("#ddlCities option:selected").val();
        ddlmakeId = $("#ddlMakes option:selected").val();
        if (!isNaN(ddlmakeId) && ddlmakeId != "0") {
            if (!isNaN(ddlcityId) && ddlcityId != "0") {
                ddlcityMasking = $("#ddlCities option:selected").attr("maskingName");
                bwcache.remove("userchangedlocation", true);
                window.location.href = "/dealer-showrooms/" + ddlmakemasking + "/" + ddlcityMasking + "/";
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
                    url: "/api/v2/DealerCity/?makeId=" + selMakeId,
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (data) {
                        bwcache.set(key + selMakeId, data.City, 30);
                        setOptions(data.City);
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
    toggleErrorMsg(ddlCities, false);
    if (optList != null) {
        ddlCities.append($('<option>').text(" Select City ").attr({ 'value': "0" }));
        $.each(optList, function (i, value) {
            ddlCities.append($('<option>').text(value.cityName).attr({ 'value': value.cityId, 'maskingName': value.cityMaskingName }));
        });
        var obj = GetGlobalLocationObject();
        if (obj != null) {
            ddlCities.val(obj.CityId);
        }
    }

    ddlCities.trigger('chosen:updated');
    $("#ddlCities_chosen .chosen-single.chosen-default span").text("No cities available");
}

                

