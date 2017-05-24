$(document).ready(function () {
    var ddlCityAdditional = $("#ddlCityAdditional");
    var ddlCityMultiple = $("#ddlCityMultiple");

    ddlCityAdditional.chosen({ no_results_text: "No matches found!!", search_contains: true });
    ddlCityMultiple.chosen({ width: "300px", no_results_text: "No matches found!!", search_contains: true });

    $('#divMultipleCities').material_chip({
        autocompleteOptions: {
            data: {
                'Apple': null,
                'Microsoft': null,
                'Google': null
            },
            limit: Infinity,
            minLength: 1
        }
    });

    //$('#autocomplete-addCity').autocomplete({
    //    data: {
    //        "Apple": null,
    //        "Microsoft": null,
    //        "Google": 'http://placehold.it/250x250'
    //    },
    //    limit: 5, // The max amount of results that can be shown at once. Default: Infinity.
    //    onAutocomplete: function (val) {
    //        // Callback function when value is autcompleted.
    //    },
    //    minLength: 1, // The minimum length of the input for the autocomplete to start. Default: 1.
    //});

    $(function () {
        $.ajax({
            type: 'GET',
            url: 'https://restcountries.eu/rest/v2/all?fields=name',
            success: function (response) {
                var countryArray = response;
                var dataCountry = {};
                for (var i = 0; i < countryArray.length; i++) {
                    //console.log(countryArray[i].name);
                    dataCountry[countryArray[i].name] = countryArray[i].flag; //countryArray[i].flag or null
                }
                $('#autocomplete-addCity').autocomplete({
                    data: dataCountry,
                    limit: 5, // The max amount of results that can be shown at once. Default: Infinity.
                });
            }
        });
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
});
