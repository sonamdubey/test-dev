var modelId;
var environment;
var hostUrl;
var userid;
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

    $('.deleteImage').change(function () {
        var cid = $(this).attr('data-colorId');
        alert(cid);
    });

    $("input[type='file']").change(function (e) {
        var files = e.target.files;
        curFile = files[0];
        var ext = curFile.name.split('.').pop().toLowerCase();
        if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
            alert('invalid extension!');
            return false;
        }
        var td = $(this).parent();
        var imgExists = td.attr('data-isImageExists');
        var responsePhotoId;
        var colorId = td.attr('data-colorId');
        var bikeName = $('#cmbMake').find('option:selected').text() + ' ' + $('#cmbModel').find('option:selected').text() + ' ' + td.attr('data-color');
        bikeName = bikeName.replace(/[^a-z0-9\s]/gi, '').replace(/[_\s]/g, '-') + "-" + Date.now() + '.' + ext;
        var path = 'n/bw/' + environment + bikeName;
        if(imgExists == 'False'){
            $.ajax({
                type: "POST",
                url: "/api/model/images/color/",
                data: '{"Modelid":"' + modelId + '" , "ModelColorId":"' + colorId + '","UserId":' + userid + '}',
                contentType: 'application/json',
                dataType: 'json',
                crossDomain: true,
                async: false,
                beforeSend: function () {
                    return;
                },
                success: function (data) {
                    responsePhotoId = data;
                    console.log(responsePhotoId);
                },
                complete: function (xhr) {
                    if (xhr.status == 404 || xhr.status == 204) {

                    }
                }
            });
        }        
        if (responsePhotoId) {
            var imgUpldUtil = uploadToAWS(curFile, responsePhotoId, modelId, path.toLowerCase(), ext);
            if (imgUpldUtil && imgUpldUtil.status) {
                var status = imgUpldUtil.status;
                if (status) {
                    var imgPath = 'https://imgd5.aeplcdn.com/' + '144x81/' + imgUpldUtil.response.originalImagePath;
                    $(this).closest('tr').find('img').attr('src', imgPath);
                    $(this).val('');
                    alert('Image has been updated');
                }
            }
        }
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
    imgUpldUtil.request = { "originalPath": path, "categoryId": 2, "itemId": itemId, "aspectRatio": "1.777", "isWaterMark": 0, "isMaster": 1, "isMain": 0, "extension": ext };
    imgUpldUtil.photoId = photoId;
    imgUpldUtil.baseURL = hostUrl;
    file.type = "image/" + ext;    
    imgUpldUtil.upload(file);
    $(file._removeLink).attr("photoId", (imgUpldUtil.photoId ? imgUpldUtil.photoId : ''));
    return imgUpldUtil;
};