if (msg != "") { Materialize.toast(msg, 5000); }
var objAdditionalAreaJson;
var ddlMappedAdditionalAreas = $("#ddlMappedAdditionalAreas");
var ddlAdditionalAreas = $("#ddlAdditionalAreas");
var ddlMappedAdditionalCity = $("#ddlMappedAdditionalCity");

﻿$(document).ready(function () {
    if (strAdditionalAreaJson) { objAdditionalAreaJson = JSON.parse($("<div>").html(strAdditionalAreaJson).text()); }

    $("#autocomplete-addCity").bw_easyAutocomplete({
        inputField: $("#autocomplete-addCity"),
        source: 3,
        click: function () {            
            var objCity = $("#autocomplete-addCity").getSelectedItemData();

            $.ajax({
                type: "GET",
                url: bwHostUrl + "/api/arealist/?cityId=" + objCity.payload.cityId,
                datatype: "json",                
                success: function (response) {
                    ddlAdditionalAreas.empty().append("<option value=\"0\" disabled>Select Areas</option>").removeAttr("disabled");
                    for (var i = 0; i < response.Area.length; i++) {
                        ddlAdditionalAreas.append("<option value=" + response.Area[i].areaId + ">" + response.Area[i].areaName + "</option>");
                    }
                },
                complete: function (xhr) {
                    if (xhr.status == 404) {
                        ddlAdditionalAreas.empty().append("<option value=\"0\" disabled>Select Areas</option>").removeAttr("disabled");
                        Materialize.toast('No areas are added for given city', 4000);
                    }
                    ddlAdditionalAreas.material_select();
                }
            });
        }
    });

    $("#autocomplete-addMulCity").bw_easyAutocomplete({
        inputField: $("#autocomplete-addMulCity"),
        source: 3,
        click: function () {
            var objCity = $("#autocomplete-addMulCity");
            var objSelectedCity = objCity.getSelectedItemData();
            $("#autocomplete-addMulCity-data").append('<div class="chip" data-cityId=' + objSelectedCity.payload.cityId + '>' + objSelectedCity.payload.cityName + '<i class="close material-icons">close</i></div>');
            objCity.val("");
        }
    });

    $("#btnMapAreas").click(function () {
        var campaignServingStatus = $("[name=campaignServingStatus]:checked").val();
        switch (campaignServingStatus) {
            case "1":
                validateData(campaignServingStatus);
                setMappingData("0", "");
                break;
            case "2":
                validateData(campaignServingStatus);
                setMappingData("0", "");
                break;
            case "3":
                validateData(campaignServingStatus);                
                setMappingData($("#txtServingRadiusForStatus3").val(), selectedCities);
                break;
            case "4":
                validateData(campaignServingStatus);
                setMappingData($("#txtServingRadiusForStatus4").val(), "");
                break;
            case "5":
                validateData(campaignServingStatus);
                var selectedCities = "";
                $("#autocomplete-addMulCity-data .chip").each(function () {
                    selectedCities += $(this).attr("data-cityId") + ",";
                });
                selectedCities = (selectedCities.substring(0, selectedCities.length - 1));
                setMappingData($("#txtServingRadiusForStatus5").val(), selectedCities);
                break;
            case "6":
                validateData(campaignServingStatus);                
                setMappingData($("#txtServingRadiusForStatus6").val(), "");
                break;
        }        
    });
   
    $("#btnMapAdditionalAreas").click(function () {
        var selectedAreas = "";
        $(ddlAdditionalAreas).find("option:selected").each(function () {
            selectedAreas += $(this).val() + ",";
        });

        $("#txtAreaIdList").val(selectedAreas.substring(0, selectedAreas.length - 1));
    });

    if (ddlMappedAdditionalCity) {
        ddlMappedAdditionalCity.change(ddlMappedAdditionalCity_onChange);
    }    
});

function setMappingData(servingRadius, cityIdList) {
    $("#hdnServingRadius").val(servingRadius ? servingRadius : 0);
    $("#hdnCityIdList").val(cityIdList);
}

function validateData(status) {

}

function ddlMappedAdditionalCity_onChange(e) {
    var cityId;
    var selectOption = $("<option disabled>Select area</option>");
    try {
        cityId = e.currentTarget.value;
        var areas = $.grep(objAdditionalAreaJson, function (i, n) { return i.City.Id == cityId; });
        ddlMappedAdditionalAreas.empty();
        ddlMappedAdditionalAreas.append(selectOption);
        $.each(areas[0].AdditionalAreas, function (key, value) {
            ddlMappedAdditionalAreas.append($("<option></option>")
                           .attr("value", value.Id)
                           .text(value.Name));
        });
        ddlMappedAdditionalAreas.material_select();
    } catch (e) {
        console.warn(e.message);
    }
}

function removeAdditionalAreas() {
    try {
        var selectedAreas = '';
        $(ddlMappedAdditionalAreas).find("option:selected").each(function () {
            if ($(this).val())
                selectedAreas += $(this).val() + ",";
        });
        if (selectedAreas) {
            $("#hdnMappedAreas").val(selectedAreas.substring(0, selectedAreas.length - 1));            
        }
        else {
            Materialize.toast("Please select area.", 4000);
            return false
        }
    } catch (e) {
        console.warn(e.message);
    }
    return true;
}

