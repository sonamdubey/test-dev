var modelId;
$(document).ready(function () {
    $(document).ready(function () {
        if ($('#hdnModelId').val() > 0) {
            fillDropdowns();
            $("#cmbModel").val($('#hdnModelId').val())
        }
    });

    $('#cmbMake').change(function () {
        debugger;
        fillDropdowns();
    });
    $('#cmbModel').change(function () {
        $('#hdnModelId').val($('#cmbModel').val());
    });

    $("#fileUpload").change(function () {
        debugger;
        var fileName = $(this).val();
        alert(fileName);
        var imgExists = $(this).parent().attr('data-isImageExists');
        var responsePhotoId = 3;
        if(!imgExists)
        {
            // call webAPI
           
        }
        uploadToAWS($(this), responsePhotoId, modelId);
    });
});


$("#btnSubmit").live("click", function () {
    if ($("#cmbMake").val() > 0 && $("#cmbModel").val() > 0) {
        return true;
    }
    else {
        alert("Please select Model");
        return false;
    }
});


function fillDropdowns() {
    var response = AjaxFunctions.GetNewModels($('#cmbMake').val());
    var dependentCmbs = new Array;
    dependentCmbs[0] = "cmbModel";
    //call the function to consume this data
    FillCombo_Callback(response, document.getElementById("cmbModel"), "hdn_cmbModel", dependentCmbs);
}


function uploadToAWS(file, photoId, itemId, path) {
    var imgUpldUtil;
    imgUpldUtil.request = { "originalImagePath":path, "categoryId": 2, "itemId": itemId, "aspectRatio": "1.777", "isWaterMark": 0, "isMaster": 1, "isMain": 0, "extension": file.name.substring(file.name.lastIndexOf('.') + 1).toLowerCase() };
    imgUpldUtil.photoId = photoId;
    imgUpldUtil.upload(file);
    $(file._removeLink).attr("photoId", (imgUpldUtil.photoId ? imgUpldUtil.photoId : ''));
};