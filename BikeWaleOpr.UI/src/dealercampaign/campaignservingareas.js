$(document).ready(function () {
    var ddlCityMultiple = $("#ddlCityMultiple");        
    var ddlAdditionalAreas = $("#ddlAdditionalAreas");

    ddlCityMultiple.chosen({ width: "300px", no_results_text: "No matches found!!", search_contains: true });
    
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
                //error: function (xhr, ajaxOptions, thrownError) {                    
                    //ddlAdditionalAreas.empty().append("<option value=\"0\" disabled>Select Areas</option>").removeAttr("disabled");
                    //Materialize.toast("AJAX request failed status : " + xhr.status + " and err : " + thrownError, 4000);
                //},
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
                setMappingData($("#txtServingRadiusForStatus3").val(), "");
                break;
            case "4":
                validateData(campaignServingStatus);
                setMappingData($("#txtServingRadiusForStatus4").val(), "");
                break;
            case "5":
                validateData(campaignServingStatus);
                setMappingData($("#txtServingRadiusForStatus5").val(), "");
                break;
        }
        //return false;
    });

    function setMappingData(servingRadius, cityIdList) {
        $("#hdnServingRadius").val(servingRadius);
        $("#hdnCityIdList").val(cityIdList);
    }

    function validateData(status) {
        
    }

    $("#btnMapAdditionalAreas").click(function () {
        var selectedAreas = "";
        $(ddlAdditionalAreas).find("option:selected").each(function () {
            selectedAreas += $(this).val() + ",";
        });

        $("#txtAreaIdList").val(selectedAreas.substring(0, selectedAreas.length - 1));
    });
});
