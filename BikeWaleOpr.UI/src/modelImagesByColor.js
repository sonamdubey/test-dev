﻿var modelId;
var environment;
$(document).ready(function () {
    $(document).ready(function () {
        if ($('#hdnModelId').val() > 0) {
            fillDropdowns();
            $("#cmbModel").val($('#hdnModelId').val())
        }
    });

    $('#cmbMake').change(function () {
        fillDropdowns();
    });
    $('#cmbModel').change(function () {
        $('#hdnModelId').val($('#cmbModel').val());
    });

    $("input[type='file']").change(function () {
        var ext = $(this).val().split('.').pop().toLowerCase();
        if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
            alert('invalid extension!');
            return false;
        }
        var imgExists = $(this).parent().attr('data-isImageExists');
        var responsePhotoId = 3;
        var bikeName = $('#cmbMake').find('option:selected').text() + ' ' + $('#cmbModel').find('option:selected').text() +' '+ $(this).parent().attr('data-color');
        bikeName = bikeName.replace(/[^a-z0-9\s]/gi, '').replace(/[_\s]/g, '-') + '.' + ext;
        var path = 'bw/' + environment + 'models/colors/' + bikeName + '?' + Date.now();
        if(!imgExists){
            // call webAPI
        }
        uploadToAWS($(this), responsePhotoId, modelId, path, ext);
    });
});


$("#btnSubmit").live("click", function () {
    if ($("#cmbMake").val() > 0 && $("#cmbModel").val() > 0) {
        return true;
    }
    else {
        alert("Please select Make and Model");
        return false;
    }
});


function fillDropdowns() {
    var response = AjaxFunctions.GetNewModels($('#cmbMake').val());
    var dependentCmbs = new Array;
    dependentCmbs[0] = "cmbModel";
    FillCombo_Callback(response, document.getElementById("cmbModel"), "hdn_cmbModel", dependentCmbs);
}


function uploadToAWS(file, photoId, itemId, path, ext) {
    var imgUpldUtil = new ImageUploadUtility();
    imgUpldUtil.request = { "originalImagePath": path, "categoryId": 2, "itemId": itemId, "aspectRatio": "1.777", "isWaterMark": 0, "isMaster": 1, "isMain": 0, "extension": ext };
    imgUpldUtil.photoId = photoId;
    imgUpldUtil.upload(file);
    var status = imgUpldUtil.status;
    $(file._removeLink).attr("photoId", (imgUpldUtil.photoId ? imgUpldUtil.photoId : ''));

};