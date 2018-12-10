var EsSpotlight = {
    doc: $(document),
    personLeadCity: {},
    newWidth: null,
    apiInput: {},

    leadForm: {
        selectionLabel: "",
        registerEvent: function () {

            $("#esLeadFormSubmit").click(function () {
                EsSpotlight.leadForm.submit($(this));
            });

            if (isCookieExists("_CustCityMaster") && masterCityNameCookie != "Select City") {
                $('#personCity').val(masterCityNameCookie)
                EsSpotlight.personLeadCity.name = masterCityNameCookie;
                EsSpotlight.personLeadCity.id = masterCityIdCookie;
            }

            if (isMobile) {
                var personCityInputField = $("#personCity");

                personCityInputField.cw_easyAutocomplete({
                    inputField: personCityInputField,
                    resultCount: 5,
                    source: ac_Source.allCarCities,
                    click: function (event) {
                        var selectionValue = personCityInputField.getSelectedItemData().value;
                        EsSpotlight.leadForm.selectionLabel = personCityInputField.getSelectedItemData().label;

                        EsSpotlight.personLeadCity.name = Common.utils.getSplitCityName(EsSpotlight.leadForm.selectionLabel);
                        EsSpotlight.personLeadCity.id = selectionValue;
                        $(personCityInputField).val(EsSpotlight.personLeadCity.name);
                    },

                    afterFetch: function (result, searchText) {
                        EsSpotlight.personLeadCity.result = result;
                        var cityVal = Common.utils.getSplitCityName(EsSpotlight.leadForm.selectionLabel);
                        if (cityVal == $.cookie("_CustCityMaster") && typeof (personCityInputField) != "undefined") {
                        if (typeof result == "undefined" || result.length <= 0) {
                            EsSpotlight.leadForm.showHideMatchError(true, $(personCityInputField).closest('.easy-autocomplete'), "No city Match");
                        }
                        else {
                            EsSpotlight.leadForm.showHideMatchError(false, $(personCityInputField).closest('.easy-autocomplete'));
                        }
                    }
                    }
                });
            }
            else {
                $("#personCity").cw_autocomplete({
                    resultCount: 5,
                    source: ac_Source.allCarCities,
                    click: function (event, ui, orgTxt) {
                        EsSpotlight.personLeadCity.name = Common.utils.getSplitCityName(ui.item.label);
                        EsSpotlight.personLeadCity.id = ui.item.id;
                        ui.item.value = EsSpotlight.personLeadCity.name;
                    },
                    open: function (result) {
                        EsSpotlight.personLeadCity.result = result;
                    },
                    afterfetch: function (result, searchtext) {
                        this.result = result;
                        if (typeof result == "undefined" || result.length <= 0)
                            EsSpotlight.leadForm.showHideMatchError(true, $('#personCity'), "No city Match");
                        else
                            EsSpotlight.leadForm.showHideMatchError(false, $('#personCity'));
                    }
                });
            }
        },

        submit: function (event) {
            var nameEle = $(event).parent().find("#personName");
            //var emailEle = $(event).parent().find("#personEmail");
            var mobileEle = $(event).parent().find("#personMob");
            var cityEle = $(event).parent().find("#personCity");
            var err = form.validation.contact(nameEle.val(), "ameo@carwale.com", mobileEle.val()); //, $.trim(cityEle.val())); emailEle.val(), 

            if (EsSpotlight.leadForm.processErrorResults(err, nameEle, mobileEle, cityEle)) { //emailEle,
                $("#esLeadFormSubmit").html("Submitting...").attr("disabled", true);
                EsSpotlight.apiCall.leadPush(EsSpotlight.leadForm.getUserObject(nameEle.val(), mobileEle.val()), EsSpotlight.leadForm.processResponse); //emailEle.val(), 
            }
        },
        getUserObject: function (custname, custmobile) {
            var customerinfo = {
                carName: carName,
                name: custname,
                mobile: custmobile,
                cityid: EsSpotlight.personLeadCity.id,
                versionId: "",
                modelId: modelid,
                makeId: "",
                leadType: PushleadType,
                cityName: EsSpotlight.personLeadCity.name
            };
            return customerinfo;
        },
        processResponse: function (data) {
            $("#es-leadform").hide();
            $("#es-thankyou").show();
            $(".request-callback-box .grid-4").css("background", "white");
        },
        validateCity: function (targetId) {

            var cityVal = Common.utils.getSplitCityName(targetId.val());
            if (cityVal == $.cookie("_CustCityMaster") && typeof (EsSpotlight.personLeadCity) != "undefined" && Number(EsSpotlight.personLeadCity.id) > 0 && EsSpotlight.personLeadCity.id == $.cookie("_CustCityIdMaster")) {
                if (isMobile) {
                    EsSpotlight.leadForm.showHideMatchError(false, $(targetId).closest('.easy-autocomplete'));
                }
                else {
                    EsSpotlight.leadForm.showHideMatchError(false, targetId);
                }
                return true;
            }
            else if (cityVal == "" || targetId.hasClass('border-red') ||
                        (
                            ($('li.ui-menu-item a:visible:eq(0)').text() != cityVal && cityVal != "") &&
                            (typeof (EsSpotlight.personLeadCity) == "undefined" || ((typeof (EsSpotlight.personLeadCity.name) == "undefined"||EsSpotlight.personLeadCity.name==null)?true:EsSpotlight.personLeadCity.name.toLowerCase() != cityVal.toLowerCase()))
                        )
                   ) {
                if (isMobile) {
                    EsSpotlight.leadForm.showHideMatchError(true, $(targetId).closest('.easy-autocomplete'), "Please Enter City");
                }
                else {
                    EsSpotlight.leadForm.showHideMatchError(true, targetId, "Please Enter City");                    
                }
                return false;
            }
            return true;
        },
        processErrorResults: function (err, nameEle, mobileEle, cityEle) { //emailEle,

            var isFormValid = true;
            if (err && err.length > 2) {

                if (err[0] == "") {
                    EsSpotlight.leadForm.showHideMatchError(false, nameEle);
                }
                else {
                    EsSpotlight.leadForm.showHideMatchError(true, nameEle, err[0]);
                   // $(errSpan).text(err[0]);
                    isFormValid = false;
                }

                //if (err[1] == "") {
                //    EsSpotlight.leadForm.showHideMatchError(false, emailELe);
                //}
                //else {
                //    EsSpotlight.leadForm.showHideMatchError(true, emailEle, err[1]);
                //    isFormValid = false;
                //}

                if (err[2] == "") {
                    EsSpotlight.leadForm.showHideMatchError(false, mobileEle);
                }
                else {
                    EsSpotlight.leadForm.showHideMatchError(true, mobileEle, err[2]);
                    isFormValid = false;
                }
            }
            if (!EsSpotlight.leadForm.validateCity(cityEle)) {
                isFormValid = false;
            }
            return isFormValid;
        },
        showHideMatchError: function (error, TargetId, errText) {
            if (error) {
                TargetId.siblings('.error-icon').removeClass('hide');
                TargetId.siblings('.cw-blackbg-tooltip').removeClass('hide').text(errText);
                TargetId.addClass('border-red');
            }
            else {
                TargetId.siblings().addClass('hide');
                TargetId.removeClass('border-red');
            }
        }

    },

    apiCall: {

        leadPush: function (customerinfo, callback) {

            $.ajax({
                type: "POST",
                url: "/ajaxpro/CarwaleAjax.AjaxResearch,Carwale.ashx",
                data: '{"carName":"' + customerinfo.carName + '", "custName":"' + customerinfo.name + '", "email":"test@test.com", "mobile":"' + customerinfo.mobile + '", "selectedCityId":"' + customerinfo.cityid + '", "versionId":"' + customerinfo.versionId + '", "modelId":"' + customerinfo.modelId + '", "makeId":"' + customerinfo.makeId + '", "leadtype":"' + customerinfo.leadType + '", "cityName":"' + customerinfo.cityName + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "PushCRM"); },
                success: function (response) {
                    callback(response);
                },
                error: function () {
                    callback(null);
                }
            });
        }
    }
}
$(document).ready(function () {
    EsSpotlight.leadForm.registerEvent();    
});