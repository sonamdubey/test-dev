$(document).ready(function () {
    var modelIdArray = modelId.split(",");
    trackUserHistory(modelIdArray);
    $('.divVersion').hide();
    //to add border to seperate header section of left from header section of right
    var divCarColorFirstChild = $("#divCarHeader").find(":nth-child(1)");
    if (divCarColorFirstChild.height() > divCarColorFirstChild.next().height()) {
        divCarColorFirstChild.find(".compareCarBox").addClass("colorborderRt");
    }
    else {
        divCarColorFirstChild.next().find(".compareCarBox").addClass("colorborderLt");
    }

    //to position vs on original carheader div
    var htVs = (parseInt($("#divCarHeader").height()) - 26) / 2;
    var vsDiv = $(document.createElement("div")).attr("style", "width:23px;height:24px;position:absolute;top:" + htVs + "px;z-index:1;right:-10px;background: url('/m/images/icons-sheet.png?v=5.2') no-repeat scroll 0 0 transparent;background-position: 0 -1148px;");
    $("#divCarHeader div:first").append(vsDiv);

    var divCarColorNameFirstChild = $("#divCarColourName div:first");
    if (divCarColorNameFirstChild.height() > divCarColorNameFirstChild.next().height()) {
        divCarColorNameFirstChild.find("div:first").addClass("colorborderRt");
    }
    else
        divCarColorNameFirstChild.next().find("div:first").addClass("colorborderLt");
    //alert($("#divCarColourName div:first").next().html());

    //to add border to seperate color section of left from color section of right
    AddBorder($("#divCarColour").find(":nth-child(1)"));

    $("#divFloat").hide();
    divCarMake = $("#divCarMake");
    divFloat = $("#divFloat");
    window.addEventListener('scroll', throttle(function () {
        var offset = $(this).scrollTop();
        if(offset > parseInt(divCarMake.position().top) +250) {
            divFloat.show();
        }
        else {
            divFloat.hide();
        }
    },300));

    $(".table tr").each(function () {
        $(this).find("td:last").removeClass("compareCarItemBorder-Rt");
    });

    $(".divCompareCarMenu li").click(function () {
        var contentType = $(this).attr("contentType");

        $(".divCompareCarMenu li").each(function () {
            if ($(this).attr("contentType") == contentType) {
                if ($(this).hasClass("list")) {
                    $(this).removeClass("list").addClass("listActive");
                    $("#" + contentType).show();
                    window.scrollTo(0, 1);
                }
            }
            else {
                if ($(this).hasClass("listActive")) {
                    $(this).removeClass("listActive").addClass("list");
                    $("#" + $(this).attr("contentType")).hide();
                }
            }
        });
    });

    $(".advantageSlugCC").each(function () {
        var element = $(this);
        if (element.is(":visible"))
            Common.utils.trackAction('CWInteractive', 'deals_mobile', 'dealsimpression_mobile', 'Comparepagemob')
    });


    AddBridgeStoneAdd();

    if (typeof EmiCalculator !== "undefined") {
        EmiCalculator.EmiCalculatorDocReady();
    }
    if (typeof EmiCalculatorExtended !== "undefined") {
        EmiCalculatorExtended.setInitialEMIModelResult();
    }
});

$(document).on("mastercitychange", function (event, cityName, cityId) {
    Common.masterCityPopup.masterCityChange(cityName, cityId);
});

function AddBorder(divCarColorFirstChild) {
    if (divCarColorFirstChild.height() > divCarColorFirstChild.next().height())
        divCarColorFirstChild.find(":nth-child(1)").addClass("colorborderRt");
    else
        divCarColorFirstChild.next().find(":nth-child(1)").addClass("colorborderLt");
}

function BoxClicked(div) {
    if ($(div).hasClass("rightMinus")) {
        $(div).removeClass("rightMinus").addClass("rightPlus");
        $(div).parent("td").parent("tr").next().hide();
    }
    else if ($(div).hasClass("rightPlus")) {
        $(div).removeClass("rightPlus").addClass("rightMinus");
        $(div).parent("td").parent("tr").next().show();
    }

}

function AddBridgeStoneAdd() {
    var wheelsAndTyers = $('#CD1 .tblItem').find('tr[itemMasterId="276"],tr[itemMasterId="202"],tr[itemMasterId="255"],tr[itemMasterId="256"]');
    if ($(wheelsAndTyers).length > 0) {
        var wheelsAndTyeresContainer = "<table class='table' cellpadding='0' cellspacing='0'><tbody><tr style='font-weight: bold;'><td class='subCategoryBorder' style='text-align: left;'>";
        wheelsAndTyeresContainer += "Wheels & Tyres</td><td class='subCategoryBorder'><div class='rightMinus' onclick='BoxClicked(this);'></div></td></tr><tr><td colspan='2'>";
        wheelsAndTyeresContainer += "<table class='table tblItem' cellpadding='0' cellspacing='0'><tbody id='bridgeStone'>";
        wheelsAndTyeresContainer += "</tbody></table></td></tr></tbody></table>";
        $('#CD1').append(wheelsAndTyeresContainer);
        $('#bridgeStone').append($(wheelsAndTyers));
        $('#logolink').attr('href', logoUrl);
    }
}

var selectVersion = {
    apiCall: {
        getVersion: function (callback, selectedModel) {
            Common.utils.trackAction('CWInteractive', 'ShowORPTest', "CompareCars_VersionClick", $(selectedModel).attr("MaskingName"));
            var modelid = $(selectedModel).attr("id");
            var type = "compareall";
            $.ajax({
                type: "GET", url: "/webapi/carversionsdata/GetCarVersions/?type=" + type + "&modelId=" + modelid,
                success: function (response) {
                    callback(response, selectedModel);
                }
            });
        },
    },
    bindVersions: {
        /// change version on compare car detail page
        selectVersionBinding: function (response, selectedModel) {
            response = JSON.parse(response);
            var type = $(selectedModel).attr("type");
            var retVal = "";
            if (response.length > 0);
            {
                for (var count = 0; count < response.length; count++) {
                    if (type == "1") {
                        var onclickUrl = window.location.href.replace(/c1=([0-9]+)/g, "c1=" + response[count].ID);
                        if (onclickUrl.indexOf("c1") < 0) {
                            onclickUrl = location.pathname + "?c1=" + response[count].ID + "&c2=" + versionIDS[1];
                        }
                        onclickUrl = onclickUrl.replace(/source=([0-9]+)/g, "source=27");
                        if (onclickUrl.indexOf("source=") < 0)
                        {
                            onclickUrl += "&source=27";
                        }                        
                        if (response[count].ID != versionIDS[1]) retVal += "<li><a href='" + onclickUrl + "' id ='" + response[count].ID + "' type='1'>" + response[count].Name + "</a></li>";
                    }
                    else if (type == "2") {
                        var onclickUrl = window.location.href.replace(/c2=([0-9]+)/g, "c2=" + response[count].ID);
                        if (onclickUrl.indexOf("c1") < 0) {
                            onclickUrl = location.pathname + "?c1=" + versionIDS[0] + "&c2=" + response[count].ID;
                        }
                        onclickUrl = onclickUrl.replace(/source=([0-9]+)/g, "source=27");
                        if (onclickUrl.indexOf("source=") < 0) {
                            onclickUrl += "&source=27";
                        }
                        if (response[count].ID != versionIDS[0]) retVal += "<li><a href='" + onclickUrl + "' id ='" + response[count].ID + "' type='2'>" + response[count].Name + "</a></li>";
                    }
                }
            }
            if (retVal != "") {
                $(".divVersion ul").html('');
                if (type == "1") {
                    modelName1 = $(selectedModel).attr("MaskingName").toString();
                    $(".divVersion ul").attr("type", "ddlVersion1").append(retVal);
                }
                else if (type == "2") {
                    modelName2 = $(selectedModel).attr("MaskingName").toString();
                    $(".divVersion ul").attr("type", "ddlVersion1").append(retVal);
                }
                $(".divVersion").show();
            }
        },
    },
    closeWindow: function () {
        $(".divVersion").hide();
    }
}