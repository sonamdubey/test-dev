$(document).ready(function () {
    if (typeof preselectCars != "undefined" && preselectCars.length > 0) CompareCarJs.cars = preselectCars;
    CompareCarJs.carBinding.registerEvents();
});


/// Namespace
var CompareCarJs = {
    container: null,
    containerIndex: 0,
    cars: [],
    currentCar:{},
    response: "",
    jsonString: "",
    resObj: "",
    divListContainer: $("#divListContainer"),
    carBinding: {
        showModel: function (selectedMake) {
            CompareCarJs.currentCar = {};
            CompareCarJs.currentCar.carName = $(selectedMake).text();
            $("#imgLoaderModel").show();
            CompareCarJs.apiCall.getmodel(CompareCarJs.carBinding.modelBinding, selectedMake);
        },
        showVersion: function (selectedModel) {
            CompareCarJs.currentCar.carName += " " + $(selectedModel).text();
            CompareCarJs.currentCar.makeModel = CompareCarJs.currentCar.carName;
            modelId = $(selectedModel).attr("id");
            CompareCarJs.currentCar.modelId = modelId;
            CompareCarJs.apiCall.getVersion(CompareCarJs.carBinding.versionBinding, selectedModel);
        },
        showCarName: function (selectedCar) {
            selectedCar = $(selectedCar);
            $(".divMake").hide();
            $(".divModel").hide();
            $(".divVersion").hide();
            $("#divForPopup").hide();
            $("#divParentPageContainer").show();
            CompareCarJs.currentCar.carName += " " + selectedCar.text();
            CompareCarJs.currentCar.version = selectedCar.text();
            CompareCarJs.currentCar.img = "https://imgd.aeplcdn.com/110x61/" + selectedCar.attr("imgurl");
            CompareCarJs.container.find("img").attr('src', CompareCarJs.currentCar.img);
            CompareCarJs.container.find("div.model-select").text(CompareCarJs.currentCar.makeModel).next().html('<span>' + CompareCarJs.currentCar.version + '</span><span class="cw-m-sprite select-arrow-down">').removeClass("hide");
            CompareCarJs.currentCar.formatedMake = CompareCarJs.URLRewrite.formatURL(makeName1);
            CompareCarJs.currentCar.formatedModel = modelName1;
            CompareCarJs.currentCar.versionId = selectedCar.attr("id");
            CompareCarJs.cars[CompareCarJs.containerIndex] = CompareCarJs.currentCar;
        },
        modelBinding: function (response, selectedMake) {
           
            var retVal = "";
            var type = 1;
            if (response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    retVal += "<li><a onclick=\"CompareCarJs.carBinding.showVersion(this);\" id = '" + response[i].ModelId + "' type = '" + type + "' MaskingName ='" + response[i].MaskingName + "' >" + response[i].ModelName + "</a></li>";
                }
            }
            $("#imgLoaderModel").hide();
            if (retVal != "") {
                makeName1 = $(selectedMake).text();
                $("#divForPopup ul").attr("id", "ddlModel1").html(retVal);

                $(".divModel").show();
                window.scrollTo(0, 1);
                $(".divMake").hide();
                $(".divVersion").hide();
            }
        },
        versionBinding: function (response, selectedModel) {
            response = JSON.parse(response);
            var retVal = "";
            if (response.length > 0) {
                for (var count = 0; count < response.length; count++) {
                    retVal += "<li><a onclick=\"CompareCarJs.carBinding.showCarName(this);\" id ='" + response[count].ID + "' type='1' imgurl='" + response[count].OriginalImgPath + "'>" + response[count].Name + "</a></li>";
                }
            }
            
            if (retVal != "") {
                modelName1 = $(selectedModel).attr("MaskingName").toString();
                $("#divForPopup ul").attr("id", "ddlVersion1").html(retVal);
                $(".divVersion").show();
                window.scrollTo(0, 1);
                $(".divMake").hide();
                $(".divModel").hide();
            }
        },
        openPopup: function (divMakeddl, containerIndex) {
            CompareCarJs.container = $(divMakeddl);
            CompareCarJs.containerIndex = containerIndex;
            $("#divParentPageContainer").hide();
            $("#divForPopup").attr("style", "z-index:1002; width:100%; height:100%; position:fixed; overflow-y:scroll; display:block; top:0; bottom:0; left:0; right:0; background-color:#F6F6F6;-webkit-overflow-scrolling: touch;");
            $(CompareCarJs.divListContainer).show();
            $(".divMake").show();
            $("#divForPopup").html($(CompareCarJs.divListContainer).html());
            $(".divModel").hide();
            $(".divVersion").hide();
        },
        closeWindow: function () {
            $(".divMake").hide();
            $(".divModel").hide();
            $(".divVersion").hide();
            $("#divForPopup").hide();
            $("#divParentPageContainer").show();
        },
        registerEvents: function () {
            $(document).on('click', "#popupOkBtn", function () {
                $("#divOverlay").hide();
                $(".popup").addClass("hide");
            });
        }
    },

    validation: {
        checkCarsArray: function() {
            if (typeof(CompareCarJs.cars) == "undefined" || CompareCarJs.cars.length < 2) return false;
            for(var i = 0;i< CompareCarJs.cars.length;i++) if(typeof CompareCarJs.cars[i] == "undefined" || typeof CompareCarJs.cars[i].version == "undefined") return false;
            return true;
        },
        verifyVersion: function (pageSource) {
            var isError = false;
            if (CompareCarJs.validation.checkCarsArray()) {
                var ver1 = CompareCarJs.cars[0].versionId;
                var ver2 = CompareCarJs.cars[1].versionId;
                if (ver1 == "-1" && ver2 == "-1") {
                    $("#spnError").html("Please select cars to compare <br>");
                    $("#divOverlay").show();
                    $("#popupDialog").removeClass("hide");
                    isError = true;
                }
                else if (ver1 == ver2) {
                    $("#spnError").html("Please choose different cars for comparison <br>");
                    $("#divOverlay").show();
                    $("#popupDialog").removeClass("hide");
                    isError = true;
                }
            }
            else {
                $("#spnError").html("Please select cars to compare <br>");
                $("#divOverlay").show();
                $("#popupDialog").removeClass("hide");
                isError = true;
            }
            if (isError)
                return false;
            else
                CompareCarJs.URLRewrite.generateURL(pageSource);
        }
    },
    apiCall: {
        getmodel: function (callback, selectedMake) {
            var makeId = $(selectedMake).attr("id");
            var type = "compareall";
            $.ajax({   
            type: "GET", url: "/webapi/carmodeldata/GetCarModelsByType/?type="+type+"&makeId="+makeId,
                success: function (response) {
                    callback(response, selectedMake);
                }
            });
        },
        getVersion: function (callback, selectedModel) {
            var type = "compareall";
            $.ajax({
                type: "GET", url: "/webapi/carversionsdata/GetCarVersions/?type="+type+"&modelId=" + modelId,
                success: function (response) {
                    callback(response, selectedModel);
                }
            });
        },
    },
    URLRewrite: {
        generateURL: function (pageSource) {
            var versionArr = new Array;
            var data = new Array;

            versionArr.push(CompareCarJs.cars[0].versionId);
            versionArr.push(CompareCarJs.cars[1].versionId);
            $.each(CompareCarJs.cars, function (a, b) { data.push({ id: b.modelId, text: (b.formatedMake + "-" + b.formatedModel) }) });

            var url = "/m/comparecars/"+Common.getCompareUrl(data)+"/";
            var queryString = '';

            for (var count = 0; count < versionArr.length; count++) {
                if (count != versionArr.length - 1) {
                    queryString += 'c' + (count + 1).toString() + '=' + versionArr[count] + '&';
                }
                else {
                    queryString += 'c' + (count + 1).toString() + '=' + versionArr[count];
                }
            }            
            location.href = url + '?' + (queryString.length > 0 ? queryString + "&source=" + pageSource : "source=" + pageSource);
        },
        formatURL: function (str) {
            str = str.toLowerCase();
            str = str.replace(/[^0-9a-zA-Z]/g, '');
            return str;
        },
    }
};