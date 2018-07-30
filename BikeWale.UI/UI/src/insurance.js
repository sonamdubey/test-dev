$(".faqWrapper h3").on("click", function () {
    if (!$(this).hasClass("active")) {
        $(".faqWrapper li").find("h3").removeClass("active").find("span.morelessSign").text("+");
        $(".faqWrapper li").find("div.faqAnswerBox").slideUp();
        $(this).addClass("active").find("span.morelessSign").text("-");
        $(this).next("div.faqAnswerBox").slideDown();
    }
    else if ($(this).hasClass("active")) {
        $(this).removeClass("active").find("span").text("+");
        $(this).next("div.faqAnswerBox").slideUp();
    }
});

$(".typeOfInsuranceContainer li").on("click focus", function () {
    $(".typeOfInsuranceContainer li").find("span.radio-btn").removeClass("radio-btn-checked").addClass("radio-btn-unchecked").next("span.policy-type").removeClass("text-bold text-grey selectedPolicy");
    $(this).find("span.radio-btn").removeClass("radio-btn-unchecked").addClass("radio-btn-checked").next("span.policy-type").addClass("text-bold text-grey selectedPolicy");
});


$("#insuranceDetailsBtn").on("click", function () {
    var isValidDetail = true;
    if (!isValidCity()) {
        validationError($("#userSelectCity"));
        isValidDetail = false;
    }
    else {
        validationSuccess($("#userSelectCity"));
    }

    if (!isValidMake()) {
        validationError($("#makeName"));
        isValidDetail = false;
    }
    else {
        validationSuccess($("#makeName"));
    }

    if (!isValidModel()) {
        validationError($("#modelName"));
        isValidDetail = false;
    }
    else {
        validationSuccess($("#modelName"));
    }

    if (!isValidVersion()) {
        validationError($("#versionName"));
        isValidDetail = false;
    }
    else {
        validationSuccess($("#versionName"));
    }

    if (!isValidDate()) {
        validationError($("#bikeRegistrationDate"));
        isValidDetail = false;
    }
    else {
        validationSuccess($("#bikeRegistrationDate"));
    }

    if (isValidDetail) {
        $("#insuranceDetails").hide();
        $("#contactDetails").show();
    }

});

function isValidPersonalDetail() {
    var isValid = true;
    var firstName = $("#firstName"),
		email = $("#email"),
		mobile = $("#mobile");
    var userName = firstName.val().length;
    var userEmail = email.val();
    var userMobile = mobile.val();

    if (validateName(userName)) {
        validationSuccess(firstName);
        if (validateEmail(userEmail)) {
            validationSuccess(email);
            if (validateMobile(userMobile)) {
                validationSuccess(mobile);
            }
            else {
                isValid = false;
                validationError(mobile);
            }
        }
        else {
            isValid = false;
            validationError(email);
        }
    }
    else {
        isValid = false;
        validationError(firstName);
    }
    return isValid;
}

var validateName = function (userName) {
    var isValid = true;
    if (userName <= 0)
        isValid = false;
    return isValid;
}

var validateEmail = function (userEmail) {
    var isValid = true;
    var reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;
    if (!reEmail.test(userEmail))
        isValid = false;
    return isValid;
}

var validateMobile = function (leadMobile) {
    var isValid = true;
    var reMobile = /^[0-9]{10}$/;
    var mobileNo = leadMobile;
    if (mobileNo == "" || mobileNo[0] == '0')
        isValid = false;
    else if (!reMobile.test(mobileNo))
        isValid = false;
    return isValid;
}

$("#firstName, #email, #mobile").on("focus", function () {
    validationSuccess($(this));
});

var validationError = function (element) {
    element.addClass("border-red");
    element.siblings("span, div").show();
};

var validationSuccess = function (element) {
    element.removeClass("border-red");
    element.siblings("span, div").hide();
};

var modelList = [],
    versionList = [],
    cityIndex,
    makeIndex,
    modelIndex,
    versionIndex,
    insauranceModel = new insuranceDetailViewModel();

$(function () {
    $("#bikeRegistrationDate").keydown(function (e) {
        e.preventDefault();
        return false;
    });
    ko.applyBindings(insauranceModel, $("#insuranceAssistance")[0]);
});

$("#userSelectCity").blur(function () {
    if (isValidCity()) {
        validationSuccess($("#userSelectCity"));
    }
    else {
        validationError($("#userSelectCity"));
    }

});

$("#makeName").blur(function () {
    if (!isValidCity()) {
        validationError($("#userSelectCity"));
    }
    else {
        validationSuccess($("#userSelectCity"));
    }

    if (isValidMake()) {
        $("#modelName").removeAttr("disabled");
        $("#versionName").removeAttr("disabled");        
        $("#insuranceDetailsBtn").removeAttr("disabled");
        validationSuccess($("#makeName"));
        setMakeList();
        modelAutoSuggest();

        $("#modelLoader").show();
    }
    else {
        $("#modelName").attr("disabled", "disabled");
        $("#versionName").attr("disabled", "disabled");
        validationError($("#makeName"));
    }
    
    //$("#modelName").val("");
    //$("#versionName").val("");
    $("#modelName").focus();
});

$("#modelName").blur(function () {
    if (isValidModel()) {
        setVersionList();
        versionAutoSuggest();
        validationSuccess($("#modelName"));
        $("#versionLoader").show();
    }
    else {
        validationError($("#modelName"));
    }
    //$("#versionName").val("");
    
});

$("#versionName").blur(function () {
    if (isValidModel()) {
        validationSuccess($("#versionName"));
    }
    else {
        validationError($("#versionName"));
    }
});

function cityAutoComplete() {
    var citySrc;
    $("#userSelectCity").autocomplete({
        source: function (request, response) {
            var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex(request.term), "i");
            citySrc = $.map(cities, function (cities) {
                if (matcher.test(cities.CityName))
                    return { label: cities.CityName + ", " + cities.StateName, value: cities.CityName + ", " + cities.StateName, id: cities.CityId }
            });
            response(citySrc.slice(0, 6));
        },
        select: function (event, ui) {
            $('#userSelectCity').val(ui.item.label);
            insauranceModel.userSelectCity = ui.item.label;
            if (isValidCity()) {
                validationSuccess($("#userSelectCity"));
            }
        },
        change: function (event, ui) {
            if (isValidCity()) {
                validationSuccess($("#userSelectCity"));
            }
        },
        minLength: 0
    });
}

function makeAutoComplete() {
    var makeSrc;
    $("#makeName").autocomplete({
        source: function (request, response) {
            var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex(request.term), "i");
            makeSrc = $.map(makes, function (makes) {
                if (matcher.test(makes.MakeName))
                    return { label: makes.MakeName, value: makes.MakeName, id: makes.MakeId }
            });
            response(makeSrc.slice(0, 6));
        },
        select: function (event, ui) {
            $('#makeName').val(ui.item.label);
            insauranceModel.makeName = ui.item.label;
            if (isValidMake()) {
                validationSuccess($("#makeName"));
            }
        },
        change: function (event, ui) {
            modelList = [];
            if (isValidMake()) {
                validationSuccess($("#makeName"));
            }
        },
        minLength: 0
    });
}

function modelAutoSuggest() {

    $("#modelName").autocomplete({
        source: function (request, response) {
            var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
            var modelSrc = $.map(modelList, function (modelList) {
                if (matcher.test(modelList.modelName))
                    return { label: modelList.modelName, value: modelList.modelName, id: modelList.modelId }
            });
            response(modelSrc.slice(0, 6));
        },
        select: function (event, ui) {
            insauranceModel.modelName = ui.item.label;
            if (isValidModel()) {
                validationSuccess($("#modelName"));
            }
        },
        change: function (event, ui) {
            versionList = [];
            if (isValidModel()) {
                validationSuccess($("#modelName"));
            }
        },
        minLength: 0
    });
}

function versionAutoSuggest() {
    $("#versionName").autocomplete({
        source: function (request, response) {
            var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
            var versionSrc = $.map(versionList, function (versionList) {
                if (matcher.test(versionList.versionName))
                    return { label: versionList.versionName, value: versionList.versionName, id: versionList.versionId, clientPrice: versionList.exShowroomPrice }
                    });
                    response(versionSrc.slice(0, 6));
                
        },
        select: function (event, ui) {
            insauranceModel.versionName = ui.item.label;
            if (isValidVersion()) {
                validationSuccess($("#versionName"));
            }
        },
        change: function (event, ui) {
            if (isValidVersion()) {
                validationSuccess($("#versionName"));
            }
        },
        minLength: 0
    });
}

function isValidCity() {
    var city = $("#userSelectCity").val();
    var state = city.split(", ")[1];
    city = city.split(", ")[0];
    if (city == null && city.length == 0) return false;
    var isValidPresent = $.map(cities, function (cities, index) {
        if (city.indexOf(cities.CityName) != -1 && state.indexOf(cities.StateName) != -1) {
            //$("#hdnCityId").val(cities.CityId); //cityId = cities.CityId
            cityIndex = index;
            return true;
        }
    });
    return (isValidPresent[0] == true) ? true : false;
}

function isValidMake() {
    var makeName = $("#makeName").val().toUpperCase();
    if (makeName == null || makeName.length == 0) return false;
    var isMakePresent = $.map(makes, function (makes, index) {
        if (makeName.indexOf(makes.MakeName) != -1) {
            makeIndex = index;
            return true;
        }
    });
    return (isMakePresent[0]) ? true : false;
}

function isValidModel() {
    var modelName = $("#modelName").val().toUpperCase();
    if (modelName == null || modelName.length == 0) return false;

    var isModelPresent = $.map(modelList, function (modelList, index) {
        if (modelName.indexOf(modelList.modelName) != -1) {
            modelIndex = index;
            return true;
        }
    });
    return (isModelPresent[0]) ? true : false;
}

function isValidVersion() {
    var name = $("#versionName").val();
    if (name == null || name.length == 0) return false;
    var isVersionPresent = $.map(versionList, function (versionList, index) {
        if (name.indexOf(versionList.versionName) != -1) {
            versionIndex = index;
            return true;
        }
    });
    return (isVersionPresent[0]) ? true : false;
}

function isValidDate() {
    var name = $("#bikeRegistrationDate").val();
    if (name == null || name.length == 0) return false;
    return true;
}

function insuranceDetailViewModel() {
    var self = this;
    self.firstName = ko.observable();
    self.lastName = ko.observable();
    self.customerName = ko.computed(function () {
        return ((self.lastName() == undefined || self.lastName() == "") ? self.firstName() : (self.firstName() + " " + self.lastName()));
    }, this);
    self.email = ko.observable();
    self.mobile = ko.observable();
    self.makeName = ko.observable();
    self.modelName = ko.observable();
    self.versionName = ko.observable();
    self.bikeRegistrationDate = ko.observable();
    self.userSelectCity = ko.observable();
    self.saveUserDetail = function () {
        if (!isValidPersonalDetail()) return;

        self.cityId = cities[cityIndex].CityId;
        self.makeId = makes[makeIndex].MakeId;
        self.modelId = modelList[modelIndex].modelId;
        self.versionId = versionList[versionIndex].versionId;
        self.clientPrice = versionList[versionIndex].exShowroomPrice;

        self.insurancePolicyType = getInsuranceType();

        self.cityName = ko.computed(function () { return $("#userSelectCity").val().split(", ")[0]; }, this);
        self.stateName = ko.computed(function () { return $("#userSelectCity").val().split(", ")[1]; }, this);

        $.ajax({
            type: "POST",
            url: "/api/InsuranceLead/",
            contentType: "application/json",
            dataType: 'json',
            data: ko.toJSON(self),
            success: function (response) {                
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
        $("#contactDetails").hide();
        $("#responseContainer").show();
        $("#responseUserName").text($("#firstName").val() + " " + $("#lastName").val());
    }
}

function getInsuranceType() {
    if ($("#new").hasClass("text-bold")) return 1;
    else if ($("#renew").hasClass("text-bold")) return 2;
    return 3;
}

function setMakeList() {
    var makeId = makes[makeIndex].MakeId;;
    if (makeId == null) {
        return;
    }
    $.ajax({
        type: "GET",
        url: "/api/InsuranceModels?makeid=" + makeId,
        contentType: "application/json",
        dataType: "json",
        success: function (response) {
            if (response == null) {
                return;
            }
            modelList = response;
            $("#modelLoader").hide();
        }
    });
}

function setVersionList() {
    var modelId = modelList[modelIndex].modelId;
    if (modelId == null) {
        return;
    }
    $.ajax({
        type: "GET",
        url: "/api/InsuranceVersions?modelId=" + modelId,
        contentType: "application/json",
        dataType: "json",
        success: function (response) {
            if (response == null) {
                return;
            }
            versionList = response;
            $("#versionLoader").hide();
        }
    });

}

$("#makeName").focus(function () { 
    $("#makeName").keydown();
});

$("#userSelectCity").focus(function () {
    $("#userSelectCity").keydown();
});

$("#versionName").focus(function () {
    $("#versionName").keydown();
});

$("#modelName").focus(function () {
    $("#modelName").keydown();
});

$("#makeName").keyup(function(){
    $("#modelName").val("");
    $("#versionName").val("");
});

$("#modelName").keyup(function () {
    $("#versionName").val("");
});