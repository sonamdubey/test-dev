genericCompareCarVM = '{Make: ko.observable(),Makes: ko.observableArray(),Model: ko.observable(),Models: ko.observableArray([{ "ModelId": -1, "ModelName": "--Select Model--", "MaskingName": "" }]),Version: ko.observable(),Versions: ko.observableArray([{ "ID": -1, "Name": "--Select Version--" }])}';
var koViewModel1 = eval('(' + genericCompareCarVM + ')');
var koViewModel2 = eval('(' + genericCompareCarVM + ')');
var koViewModel3 = eval('(' + genericCompareCarVM + ')');
var koViewModel4 = eval('(' + genericCompareCarVM + ')');
var isCached = false;
var preSelectedCount = 0;
if (preSelectJson != null) {
    preSelectedCount = preSelectJson.length;
}
    $("#drpMake1").val($("#drpMake1 option:first").val());
    $("#drpMake2").val($("#drpMake1 option:first").val());
    $("#drpMake3").val($("#drpMake1 option:first").val());
    $("#drpMake4").val($("#drpMake1 option:first").val());

$(document).ready(function () {
    ko.applyBindings(koViewModel1, $("#drpMake1").parent().parent()[0]);
    ko.applyBindings(koViewModel2, $("#drpMake2").parent().parent()[0]);
    ko.applyBindings(koViewModel3, $("#drpMake3").parent().parent()[0]);
    ko.applyBindings(koViewModel4, $("#drpMake4").parent().parent()[0]);
    $('.drpMake').change(function () {
        var selectedMake = this.value;
        var makeIndex = $(this).attr('id');
        if (makeIndex != null) {
            var makeControlIndex = makeIndex.substring(makeIndex.length - 1, makeIndex.length);
        }
        var relatedModeldrp = "drpModel" + makeControlIndex;
        var relatedVersiondrp = "drpVersion" + makeControlIndex;
        bindModelsList("compareall", selectedMake, ko.dataFor(this), "#" + relatedModeldrp, "--Select Model--");
        $("#" + relatedModeldrp).val(-1).change();
        var sequenceNumber = $(this).attr('id').substring($(this).attr('id').length - 1);
    });

    $('.drpModel').change(function () {
        var selectedModel = this.value;
        var modelIndex = $(this).attr('id');
        if (modelIndex != null) {
            var modelControlIndex = modelIndex.substring(modelIndex.length - 1, modelIndex.length);
        }
        var relatedVersiondrp = "drpVersion" + modelControlIndex;
        bindVersionsByModelList("compareall", selectedModel, ko.dataFor(this), "#" + relatedVersiondrp, "--Select Version--");
        $("#" + relatedVersiondrp).val(-1).change();
        var sequenceNumber = $(this).attr('id').substring($(this).attr('id').length - 1);
           

    });

    // calling prefill of cars if page is from cache, ie user is coming back by clicking back button in the browser
    if ($('#hdnIsPageFromCache').val() == "1") {
        isCached = true;
        CompareCarJs.carBinding.prefillCars();
    } else {
        $('#hdnIsPageFromCache').val("1");
    }
    if (!isCached) {
        if (preSelectedCount == 0) { $('#drpMake1, #drpMake2, #drpMake3, #drpMake4').change(); }
        else if (preSelectedCount > 0) {
            CompareCarJs.carBinding.preSelect();
        }
    }

});
CompareCarJs = {
    drpMake: "",
    drpModel: "",
    drpVersion:"",
    carBinding: {
        prefillCars: function () {
            preSelectedCount = 4;
            for (count = 0 ; count < 4; count++) {
                $("#drpMake" + (count + 1)).val("");
                $("#drpModel1" + (count + 1)).val("");
                $("#drpVersion1" + (count + 1)).val("");
            }
            for (var count = 0; count < preSelectedCount; count++) {
                var drpMakeId = $("#hdnDrpMake" + (count + 1)).val();
                if (drpMakeId != undefined && drpMakeId != "") {
                    drpMake = "#drpMake" + (count + 1);
                    drpModel = "#drpModel" + (count + 1);
                    drpVersion = "#drpVersion" + (count + 1);
                    $(drpMake).val($("#hdnDrpMake" + (count + 1)).val());
                    CompareCarJs.carBinding.bindDropdowns(ko.dataFor($(drpModel)[0]), ko.dataFor($(drpVersion)[0]), drpMakeId, $("#hdnDrpModel" + (count + 1)).val(), $("#hdnDrpVersion" + (count + 1)).val(), drpModel, drpVersion);
                }
            }
        },
        preSelect: function () {
            for (var count = 0; count < preSelectedCount; count++) {
                var drpMake = "#drpMake" + (count + 1);
                var drpModel = "#drpModel" + (count + 1);
                var drpVersion = "#drpVersion" + (count + 1);
                $(drpMake).val(preSelectJson[count].MakeId);
                CompareCarJs.carBinding.bindDropdowns(ko.dataFor($(drpModel)[0]), ko.dataFor($(drpVersion)[0]), preSelectJson[count].MakeId, preSelectJson[count].ModelId, preSelectJson[count].VersionId, drpModel, drpVersion);
            }
        },
        bindDropdowns: function (modelViewModel, versionViewModel, makeId, modelId, versionId, drpModel, drpVersion) {
            bindModelsList("compareall", makeId, modelViewModel, drpModel, "--Select Model--").done((function (drpModel, modelId, drpVersion) {
                return function (data) {
                    $(drpModel).find('option[value=' + modelId + ']').prop('selected', true);
                    $(drpVersion).prop('disabled', false);
                    bindVersionsByModelList("compareall", modelId, versionViewModel, drpVersion, "--Select Version--").done((function (drpVersion, versionId) {
                        return function (data) {
                            $(drpVersion).find('option[value=' + versionId + ']').prop('selected', true);
                        };
                    })(drpVersion, versionId));
                };
            })(drpModel, modelId, drpVersion));
        }
    },
    URLRewrite: {
        genUrl: function (pageSource) {
            var count = 0;

            var dataForUrl = new Array;
            var versionArr = new Array;
            var version1 = $('#drpVersion1').val();
            var version2 = $('#drpVersion2').val();
            var version3 = $('#drpVersion3').val();
            var version4 = $('#drpVersion4').val();
            for (count = 0 ; count < 4; count++) {
                $("#hdnDrpMake" + (count + 1)).val("");
                $("#hdnDrpModel1" + (count + 1)).val("");
                $("#hdnDrpVersion1" + (count + 1)).val("");
            }
            if (Number(version1) > 0) {
                var model = $('#drpModel1').find('option:selected');
                var make = $('#drpMake1').find('option:selected');
                dataForUrl.push({ id: Number(model.val()), text: CompareCarJs.URLRewrite.formatURL(make.text()) + "-" + model.attr('mask') });
                versionArr.push(version1);
                $("#hdnDrpMake1").val(make.val());
                $("#hdnDrpModel1").val(model.val());
                $("#hdnDrpVersion1").val(version1);
                count++;
            }
            if (Number(version2) > 0) {
                var model = $('#drpModel2').find('option:selected');
                var make = $('#drpMake2').find('option:selected');
                dataForUrl.push({ id: Number(model.val()), text: CompareCarJs.URLRewrite.formatURL(make.text()) + "-" + model.attr('mask') });
                versionArr.push(version2);
                $("#hdnDrpMake2").val(make.val());
                $("#hdnDrpModel2").val(model.val());
                $("#hdnDrpVersion2").val(version2);
                count++;
            }
            if (Number(version3) > 0) {
                var model = $('#drpModel3').find('option:selected');
                var make = $('#drpMake3').find('option:selected');
                dataForUrl.push({ id: Number(model.val()), text: CompareCarJs.URLRewrite.formatURL(make.text()) + "-" + model.attr('mask') });
                versionArr.push(version3);
                $("#hdnDrpMake3").val(make.val());
                $("#hdnDrpModel3").val(model.val());
                $("#hdnDrpVersion3").val(version3);
                count++;
            }
            if (Number(version4) > 0) {
                var model = $('#drpModel4').find('option:selected');
                var make = $('#drpMake4').find('option:selected');
                dataForUrl.push({ id: Number(model.val()), text: CompareCarJs.URLRewrite.formatURL(make.text()) + "-" + model.attr('mask') });
                versionArr.push(version4);
                $("#hdnDrpMake4").val(make.val());
                $("#hdnDrpModel4").val(model.val());
                $("#hdnDrpVersion4").val(version4);
                count++;
            }
            $("#hdnCount").val(count);
            var url = Common.getCompareUrl(dataForUrl);
            var queryString = '';
            for (var c = 0; c < dataForUrl.length; c++) {
                if (c != dataForUrl.length - 1) {
                    queryString += 'c' + (c + 1).toString() + '=' + versionArr[c] + '&';
                }
                else {
                    queryString += 'c' + (c + 1).toString() + '=' + versionArr[c];
                }
            }
            window.location = url + '/?' + (queryString.length > 0 ? queryString + "&source=" + pageSource : "source="+ pageSource);
        },
        formatURL: function (str) {
            str = str.toLowerCase();
            str = str.replace(/[^0-9a-zA-Z]/g, '');
            return str;
        }
    },
    validation: {
        verifyVersions: function (pageSource) {
            var isSame = false;
            var isError = false;
            var selected = 0;

            var versionArr = [];
            var versionArrLength = 4;
            for (var count = 0 ; count < versionArrLength; count++) {
                versionArr[count] = 0;
                var versionVal = $('#drpVersion' + (count + 1)).val();
                if (versionVal > 0) {
                    selected++;
                    versionArr[count] = versionVal;
                }
            }
            if (selected < 2) {
                ShakeFormView($(".divBtnContainer"));
                $('#spn').html("Choose at least two cars for comparison.");
                return false;
            }

            isSame = CompareCarJs.validation.hasDuplicateVersion(versionArr);
            if (isSame) {
                ShakeFormView($(".divBtnContainer"));
                $('#spn').html("Please choose different cars for comparison.");
                return false;
            }
            CompareCarJs.URLRewrite.genUrl(pageSource);
            return false;
        },
        hasDuplicateVersion: function (versionArray) {
            var hashTable = Object.create(null);
            var arrayLength = versionArray.length;
            for (var count = 0 ; count < arrayLength; count++) {
                var curVersion = versionArray[count];
                if (curVersion > 0) {
                    if (curVersion in hashTable) {
                        return true;
                    }
                    hashTable[curVersion] = true;
                }
            }
            return false;
        }
    }
};