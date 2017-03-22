var vmModelTable = function () {
    var self = this;

    var loader = '<div id="loader" class="progress" ><div class="determinate" style="width: 70%" ></div></div>';

    self.deleteUsedPhoto = function (d, e) {
        try {
            var imagepath = $(e.currentTarget).data("imagepath");
            if (imagepath == "")
                Materialize.toast('No image available', 4000);
            else {
                if (confirm('Are you sure?')) {                    
                    var modelId = $(e.currentTarget).data("modelid");
                    var modelName = $(e.currentTarget).data("modelname");
                    $.ajax({
                        type: "POST",
                        url: "/api/used/modelimageupload/deleteusedbikemodelimage/" + modelId + "/",
                        contentType: 'application/json',
                        dataType: 'json',
                        success: function (data) {                            
                            $(e.currentTarget).closest('tr').find('#mainImage').attr('src', 'https://imgd3.aeplcdn.com/144x81/bikewaleimg/images/noimage.png');
                            Materialize.toast(modelName + ' photo deleted!', 4000);
                        },
                        complete: function (xhr) {
                            if (xhr.status != 200) {
                                Materialize.toast('Some error occurred!', 4000);
                            }                                                      
                        }
                    });
                }
            }
        }
        catch (e) {
            console.log(e);
        }
    }

    self.uploadUsedPhoto = function (d, e) {
        try {
            if ($(e.currentTarget).closest('tr').find('.file-path').val() != '') {
                $(e.currentTarget).closest('tr').find('img').parent().append(loader);
                var curFile = currentFile;
                var ext = curFile.name.split('.').pop().toLowerCase();

                if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
                    Materialize.toast('Invalid extension!', 4000);
                    return false;
                }               
                var modelId = $(e.currentTarget).data("modelid");
                var modelName = $(e.currentTarget).data("modelname");
                var makeName = $(e.currentTarget).data("makename");
                var bikFullName = makeName + ' ' + modelName;
                var bikeName = bikFullName.replace(/[^a-z0-9\s]/gi, '').replace(/[_\s]/g, '-') + "-" + Date.now() + '.' + ext;

                var path = 'n/bw/' + $('#environment').val() + 'used/modelimages/' + bikeName;
                
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
                            var imgUpldUtil = self.uploadToAWS(curFile, responsePhotoId, modelId, path.toLowerCase(), ext);
                            if (imgUpldUtil && imgUpldUtil.status) {
                                if (imgUpldUtil.status) {
                                    var imgPath = 'https://imgd5.aeplcdn.com/' + '144x81/' + imgUpldUtil.response.originalImagePath;
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

    self.uploadToAWS = function (file, photoId, itemId, path, ext) {
        var imgUpldUtil = new ImageUploadUtility();
        imgUpldUtil.request = { "originalPath": path, "categoryId": 3, "itemId": itemId, "aspectRatio": "1.777", "isWaterMark": 0, "isMaster": 1, "isMain": 0, "extension": ext };
        imgUpldUtil.photoId = photoId;
        imgUpldUtil.baseURL = $('#bwOprHostUrl').val();
        file.type = "image/" + ext;
        imgUpldUtil.upload(file);
        $(file._removeLink).attr("photoId", (imgUpldUtil.photoId ? imgUpldUtil.photoId : ''));
        return imgUpldUtil;
    };
}

ko.applyBindings(new vmModelTable, $("#model-UsedBike-Images")[0]);

var makeViewModel = function () {
    var self = this;
    self.makeName = ko.observable("Select Make");
    self.makeId = ko.observable();

    self.updateMake = function (data, event) {        
        self.makeName($(event.currentTarget).data("makename"));
        self.makeId($(event.currentTarget).data("makeid"));
    };

    self.validateMakeSubmit = function () {
        var isValid = false;

        if (self.makeId() != null) {
            isValid = true;
            window.location.href = "/Models/UsedModelImageUpload/?makeId=" + self.makeId();
        }
        else {
            Materialize.toast('Please select make', 4000);
        }
        return isValid;
    }
}


var vmMake = new makeViewModel;

ko.applyBindings(vmMake, $("#make-card")[0]);

$(document).ready(function () {
    $("input[type='file']").change(function (e) {
        currentFile = e.target.files[0];
    });
});
