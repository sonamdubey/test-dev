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
        hosturlForAPI: bwHostUrl,
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
                    materialSelect.removeLabel(ddlAdditionalAreas);
                }
            });

        }
    });

    $("#autocomplete-addMulCity").bw_easyAutocomplete({
        inputField: $("#autocomplete-addMulCity"),
        source: 3,
        hosturlForAPI: bwHostUrl,
        click: function () {
            var objCity = $("#autocomplete-addMulCity");
            var objSelectedCity = objCity.getSelectedItemData();
            $("#autocomplete-addMulCity-data").append('<div class="chip" data-cityId=' + objSelectedCity.payload.cityId + '>' + objSelectedCity.text + '<i class="close material-icons">close</i></div>');
            objCity.val("");
        }
    });

    $("#btnMapAreas").click(function () {
        var isValid = true;

        if (!$("input[name='campaignServingStatus']:checked").val()) {
            isValid = false;
            Materialize.toast("Please select campaign mapping status", 6000);
        }
        else {
            var campaignServingStatus = $("[name=campaignServingStatus]:checked").val();
            switch (campaignServingStatus) {
                case "1":
                    setMappingData("0", "");
                    break;
                case "2":
                    setMappingData("0", "");
                    break;
                case "3":
                    var objServingRadius = $("#txtServingRadiusForStatus3");
                    var servingRadius = parseInt(objServingRadius.val());

                    if (isNaN(servingRadius) || parseInt(servingRadius) <= 0) {
                        isValid = false;
                        validate.inputField.showError(objServingRadius);
                    }

                    setMappingData(servingRadius, "");
                    break;
                case "4":
                    var objServingRadius = $("#txtServingRadiusForStatus4")
                    var servingRadius = parseInt(objServingRadius.val());

                    if (isNaN(servingRadius) || parseInt(servingRadius) <= 0) {
                        isValid = false;
                        validate.inputField.showError(objServingRadius);
                    }

                    setMappingData(servingRadius, "");
                    break;
                case "5":
                    var selectedCities = "";
                    $("#autocomplete-addMulCity-data .chip").each(function () {
                        selectedCities += $(this).attr("data-cityId") + ",";
                    });
                    selectedCities = (selectedCities.substring(0, selectedCities.length - 1));

                    if (selectedCities == "") {
                        isValid = false;
                        validate.inputField.showError($("#autocomplete-addMulCity"));
                    }

                    var objServingRadius = $("#txtServingRadiusForStatus5");
                    var servingRadius = objServingRadius.val();

                    if (servingRadius)
                    {
                        servingRadius = parseInt(servingRadius);

                        if (isNaN(servingRadius) || servingRadius <= 0) {
                            isValid = false;
                            validate.inputField.showError(objServingRadius);
                        } 
                    }

                    setMappingData($("#txtServingRadiusForStatus5").val(), selectedCities);
                    break;
                case "6":
                    if ($("#ddlStates").val() == null) {
                        isValid = false;
                        validate.selectField.showError($("#ddlStates"), "Please select atleast one state.");
                    }

                    setMappingData("0", "");
                    break;
            }
        }

        if (isValid)
            progress.showProgress();
        
        return isValid;
    });
   
    $("#btnMapAdditionalAreas").click(function () {
        var selectedAreas = "";
        var txtAreaIdList = $("#txtAreaIdList");       

        $(ddlAdditionalAreas).find("option:selected").each(function () {
            selectedAreas += $(this).val() + ",";
        });

        if (selectedAreas == "" && selectedAreas.length <= 0) {
            validate.selectField.showError(ddlAdditionalAreas, "Please select atleast one area");
            return false;
        }
        else {
            progress.showProgress();
            txtAreaIdList.val(selectedAreas.substring(0, selectedAreas.length - 1));
        }        
    });

    if (ddlMappedAdditionalCity) {
        ddlMappedAdditionalCity.change(ddlMappedAdditionalCity_onChange);
    }    
});

$("#ddlStates").on("change", function () {
    if ($(this).val() != null) {
        validate.selectField.hideError($(this));
    }
});

function setMappingData(servingRadius, cityIdList) {
    $("#hdnServingRadius").val(servingRadius ? servingRadius : 0);
    $("#hdnCityIdList").val(cityIdList);
}

function ddlMappedAdditionalCity_onChange(e) {
    var cityId;
    var selectOption = $("<option disabled>Select area</option>");
    try {
        cityId = e.currentTarget.value;
        ddlMappedAdditionalAreas.empty();
        ddlMappedAdditionalAreas.append(selectOption);
        if (cityId > 0) {
            var areas = $.grep(objAdditionalAreaJson, function (i, n) { return i.City.Id == cityId; });                        
            $.each(areas[0].AdditionalAreas, function (key, value) {
                ddlMappedAdditionalAreas.append($("<option></option>")
                               .attr("value", value.Id)
                               .text(value.Name));
            });
        }
        ddlMappedAdditionalAreas.material_select();
        materialSelect.removeLabel(ddlMappedAdditionalAreas);
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

        if (selectedAreas == "" && selectedAreas.length <= 0) {
            validate.selectField.showError(ddlMappedAdditionalAreas, "Please select atleast one area");
            return false;
        } else {
            progress.showProgress();
            $("#hdnMappedAreas").val(selectedAreas.substring(0, selectedAreas.length - 1));
        }
    } catch (e) {
        console.warn(e.message);
    }
    return true;
}

