var modelId, environment, userid, bwHostUrl, bikeFullName, currentFile;
$(document).ready(function () {
    if ($('#hdnModelId').val() > 0) {
        fillDropdowns();
        $("#cmbModel").val($('#hdnModelId').val())
        setBikeName();
    }
    
    $('#cmbMake').change(function () {
        fillDropdowns();
    });
    $('#cmbModel').change(function () {
        $('#hdnModelId').val($('#cmbModel').val());
    });

    $("input[type='file']").change(function (e) {
        currentFile = e.target.files[0];
        if (currentFile != null) {
            readURL(e, $(this));
        }
    });
    $(".uploadImage").live("click", function () {
        var td = $(this).parent();
        var uploadControl = td.find('#fileUpload');
        if (uploadControl.val().length == 0) {
            showToast('Select file to upload');
            return false;
        }
        uploadControl.trigger('change');
        var curFile = currentFile;
        if (curFile == null) {
            showToast('Select image');
            return false;
        }
        var ext = curFile.name.split('.').pop().toLowerCase();
        if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
            showToast('invalid extension!');
            return false;
        }
        startLoading($("#inputSection"));
        var imgExists = td.attr('data-isImageExists');
        var responsePhotoId;
        var colorId = td.attr('data-colorId');
        var bikeName = bikeFullName + ' ' + td.attr('data-color');
        bikeName = bikeName.replace(/[^a-z0-9\s]/gi, '').replace(/[_\s]/g, '-') + "-" + Date.now() + '.' + ext;
        var path = 'n/bw/' + environment + 'models/colors/' + bikeName;
            $.ajax({
                type: "POST",
                url: "/api/model/images/color/",
                data: '{"Modelid":"' + modelId + '" , "ModelColorId":"' + colorId + '","UserId":' + userid + '}',
                contentType: 'application/json',
                dataType: 'json',
                crossDomain: true,
                async: false,
                success: function (data) {
                    responsePhotoId = data;
                },
                complete: function (xhr) {
                    if (xhr.status == 404 || xhr.status == 204) {
                        console.log('some error occurred');
                    }
                }
            });

        if (responsePhotoId) {
            var imgUpldUtil = uploadToAWS(curFile, responsePhotoId, modelId, path.toLowerCase(), ext);
            if (imgUpldUtil && imgUpldUtil.status) {
                if (imgUpldUtil.status) {
                    var imgPath = 'https://imgd.aeplcdn.com/' + '144x81/' + imgUpldUtil.response.originalImagePath;
                    showToast('Image uploaded');
                    $('.delcolumn').show();
                    $(this).closest('tr').find('#mainImage').attr('src', imgPath);
                    $(this).closest('tr').find('#preview').hide();
                    td.find('#fileUpload').val('');
                    $(this).closest('tr').find('.deleteImage').show();
                    stopLoading($("#inputSection"));
                }
            }
        }
    });
});

function setBikeName() {
    bikeFullName = $('#cmbMake').find('option:selected').text() + ' ' + $('#cmbModel').find('option:selected').text();
}

function readURL(input,control) {
    if (input.target.files && input.target.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            var td = control.parent();
            td.find('#preview').attr('src', e.target.result);
            td.find('#preview').height(60).width(105);
        }
        reader.readAsDataURL(input.target.files[0]);
    }
}

$("#btnSubmit").live("click", function () {
    if ($("#cmbMake").val() > 0 && $("#cmbModel").val() > 0) {
        setBikeName();
        return true;
    }
    else {
        showToast("Select Make & Model");
        return false;
    }
});

$('.deleteImage').live("click", function () {
    var delBtn = $(this);
    var colorId = $(this).attr('data-id');
    $.ajax({
        type: "POST",
        url: "/api/image/delete/modelid/?photoId=" + colorId + "&modelid=" + modelId,
        contentType: 'application/json',
        dataType: 'json',
        crossDomain: true,
        async: false,
        beforeSend: function (xhr) {
            startLoading($("#inputSection"));
        },
        success: function (data) {
            delBtn.closest('tr').find('#mainImage').attr('src', 'https://imgd.aeplcdn.com/144x81/bikewaleimg/images/noimage.png');
            showToast('Image deleted');            
            delBtn.hide();
        },
        complete: function (xhr) {
            if (xhr.status == 404 || xhr.status == 204) {
                console.log('some error occurred');
            }
            stopLoading($("#inputSection"));
        }
    });
});

// Modified by : Vivek Singh Tomar on 1st Aug 2017
// Functions modified : clearCombo, bindDropdowns and fillDropdown to bind data to model dropdown
function clearCombo(cmb, selectString) {
    try{
        cmb.options.length = null;
        if (selectString == '' || !selectString) selectString = "Any"
        cmb.options[0] = new Option(selectString, 0);
    } catch (ex) {
        console.warn(ex);
    }
}

function bindDropdowns(data, cmbToFill, hdnId) {
    try{
        var _delimiter = "|";
        var objHdn = document.forms[0][hdnId];

        if (cmbToFill) {
            clearCombo(cmbToFill);
            var content = "";
            var j = 1
            for (var obj in data) {
                cmbToFill.options[j] = new Option(data[obj].ModelName, data[obj].ModelId);
                if (content == "") {
                    content = data[obj].ModelName + _delimiter + data[obj].ModelId;
                } else {
                    content += _delimiter + data[obj].ModelName + _delimiter + data[obj].ModelId;
                }
                ++j;
            }

            if (objHdn) {
                objHdn.value = content;
            }

            cmbToFill.disabled = false;
        }
    } catch (ex) {
        console.warn(ex);
    }        
}

function fillDropdowns() {
    try {
        $.ajax({
            type: "POST",
            url: "/api/makes/" + $('#cmbMake').val() + "/getmodels/",
            contentType: "application/json",
            dataType: 'json',
            async: false,
            success: function (data) {
                if (data == null && typeof (data) == "object") {
                    console.log('data not present');
                } else {
                    bindDropdowns(data, document.getElementById("cmbModel"), "hdn_cmbModel");
                }
            },
            complete: function (xhr) {
                if (xhr.status == 400 || xhr.status == 500 || xhr.status == 404 || xhr.status == 204) {
                    console.log('some error occurred');
                }
            }
        });
    } catch (ex) {
        console.warn(ex);
    }
}

function uploadToAWS(file, photoId, itemId, path, ext) {
    var imgUpldUtil = new ImageUploadUtility();
    imgUpldUtil.request = { "originalPath": path, "categoryId": 2, "itemId": itemId, "aspectRatio": "1.777", "isWaterMark": 0, "isMaster": 1, "isMain": 0, "extension": ext };
    imgUpldUtil.photoId = photoId;
    imgUpldUtil.baseURL = bwOprHostUrl;
    file.type = "image/" + ext;    
    imgUpldUtil.upload(file);
    $(file._removeLink).attr("photoId", (imgUpldUtil.photoId ? imgUpldUtil.photoId : ''));
    return imgUpldUtil;
};

function startLoading(ele) {
    try {
        var _self = $(ele).find(".progress-bar").css({ 'width': '0' }).show();
        _self.animate({ width: '100%' }, 5000);
    }
    catch (e) { return };
}

function stopLoading(ele) {
    try {
        var _self = $(ele).find(".progress-bar");
        _self.stop(true, true).css({ 'width': '100%' }).fadeOut(1000);
    }
    catch (e) { return };
}

function showToast(msg) {
    $('.toast').text(msg).stop().fadeIn(400).delay(3000).fadeOut(400);
}