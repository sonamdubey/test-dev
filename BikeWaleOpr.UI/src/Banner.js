var $dateInput = $('.datepicker').pickadate({
    selectMonths: true, // Creates a dropdown to control month   
    closeOnSelect: true,
    onClose: function () { if (dateValue != $("#reviewDateEle").val()) $("#reviewDate").val($("#reviewDateEle").val()); },
    onOpen: function () { dateValue = $("#reviewDateEle").val() },
    onSet: function (ele) { if (ele.select) { this.close(); } }
});

$(document).ready(function () {    

    $('#startTimeEleDesktop').val("00:00:00");
    $('#endTimeEleDesktop').val("00:00:00");
    $('#startTimeEleMobile').val("00:00:00");
    $('#endTimeEleMobile').val("00:00:00");

    if ($(".stepper"))
    {
        $('.stepper').activateStepper();
    }

    $("input[type='file']").change(function (e) {
        currentFile = e.target.files[0];
    });

});

uploadUsedPhoto = function (d, e) {
    try {
        if ($(e.currentTarget).closest('tr').find('.file-path').val() != '') {
            $(e.currentTarget).closest('tr').find('img').parent().append(loader);
            var curFile = currentFile;
            var ext = curFile.name.split('.').pop().toLowerCase();

            if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
                Materialize.toast('Invalid extension!', 4000);
                return false;
            }
           
            var path = 'n/bw/' + $('#environment').val() + 'homepagebanner' + ext;

            $.ajax({
                type: "POST",
                url: "/api/used/modelimageupload/fetchphotoid/",
                data: '{"Modelid":"' + modelId + '","UserId":' + $("#userid").val() + '}',
                contentType: 'application/json',
                dataType: 'json',
                success: function (data) {
                    var responsePhotoId = data;
                    $(e.currentTarget).closest('tr').find('#filePath').val('');
                    if (responsePhotoId) {
                        var imgUpldUtil = uploadToAWS(curFile, responsePhotoId, modelId, path.toLowerCase(), ext);
                        if (imgUpldUtil && imgUpldUtil.status) {
                            if (imgUpldUtil.status) {
                                var imgPath = 'https://imgd.aeplcdn.com/' + '144x81/' + imgUpldUtil.response.originalImagePath;
                                Materialize.toast(modelName + ' image uploaded succesfull!', 4000);
                                $(e.currentTarget).closest('tr').find('img').attr('src', imgPath);
                                $(e.currentTarget).closest('tr').find('a').attr('data-imagepath', imgPath);
                            }
                        }
                    }
                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        console.log('some error occurred');
                    }
                    $(e.currentTarget).closest('tr').find('#loader').remove();
                }
            });

        }
        else {
            Materialize.toast('Please upload image first', 4000);
        }
    }
    catch (e) {
        console.log(e);
    }
}

uploadToAWS = function (file, photoId, itemId, path, ext) {
    var imgUpldUtil = new ImageUploadUtility();
    imgUpldUtil.request = { "originalPath": path, "categoryId": 3, "itemId": itemId, "isWaterMark": 0, "isMaster": 0, "isMain": 0, "extension": ext };
    imgUpldUtil.photoId = photoId;
    imgUpldUtil.baseURL = $('#bwOprHostUrl').val();
    file.type = "image/" + ext;
    imgUpldUtil.upload(file);
    $(file._removeLink).attr("photoId", (imgUpldUtil.photoId ? imgUpldUtil.photoId : ''));
    return imgUpldUtil;
};