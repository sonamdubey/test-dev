var $dateInput = $('.datepicker').pickadate({
    selectMonths: true, // Creates a dropdown to control month   
    closeOnSelect: true,
    onClose: function () { if (dateValue != $("#reviewDateEle").val()) $("#reviewDate").val($("#reviewDateEle").val()); },
    onOpen: function () { dateValue = $("#reviewDateEle").val() },
    onSet: function (ele) { if (ele.select) { this.close(); } }
});

$(document).ready(function () {

    $('#startTimeEle').val("00:00:00");
    $('#endTimeEle').val("00:00:00");

    if ($(".stepper")) {
        $('.stepper').activateStepper({ autoFocusInput: false });
    }

    $("input[type='file']").change(function (e) {
        currentFile = e.target.files[0];
    });

    var configureBanner = function () {
        var self = this;

        self.Configure = function () {
            var substring = $('#textareaBannerDesc').val() + "/" + $('#startDateEle').val() + "/" + $('#endDateEle').val();
            if (window.location.search != "")
                substring += "/" + window.location.search;
            $.ajax({
                type: "POST",
                url: "/api/banner/submit/" + substring,
                contentType: "application/json",
                dataType: "json",
                success: function (response) {

                    $('#campaingid').val(response);
                }

            });

        };

        self.saveDesktop = function () {


            var desktopDetails = {
                "DesktopBannerDetails":
                    {
                        "html": $('#textareaHtmlDesktop').val(),
                        "css": $('#textareaCssDesktop').val(),
                        "js": $('#textareaJsDesktop').val(),
                        "backgroundcolor": $('#txtBackgroundColorDesktop').val(),
                        "bannertitle": $('#txtBannerTitleDesktop').val(),
                        "buttontext": $('#txtButtonDesktop').val(),
                        "targethref": $('#linkButtonDesktop').val(),
                        "horizontalposition": $('#select-hori-pos-Desktop').val(),
                        "verticalposition": $("#select-ver-pos-Desktop").val(),
                        "buttonposition": $("#select-button-pos-Desktop").val(),
                        "buttoncolor": $("#select-button-color-Desktop").val(),
                        "target": $('#radioOpenInNewPage').is(':checked') ? 1 : 2,
                        "buttontype": $('#btnTypeLinkDesktop').is(':checked') ? 1 : 2,
                        "jumbotrondepth": $("#select-button-jmbdepth-Desktop").val()
                    },
                "CampaignId":$('#campaingid').val()

            }

            $.ajax({
                type: "POST",
                url: "/api/desktop/submit/?platformId=1",
                contentType: "application/json",
                data: ko.toJSON(desktopDetails),
                success: function (response) {


                }

            });

        };

        self.saveMobile = function () {


            var mobileDetails = {
                "MobileBannerDetails":
                    {
                        "html": $('#textareaHtmlMobile').val(),
                        "css": $('#textareaCssMobile').val(),
                        "js": $('#textareaJsMobile').val(),
                        "backgroundcolor": $('#txtBackgroundColorMobile').val(),
                        "bannertitle": $('#txtBannerTitleMobile').val(),
                        "buttontext": $('#txtButtonMobile').val(),
                        "targethref": $('#linkButtonMobile').val(),
                        "horizontalposition": $('#select-hori-pos-Mobile').val(),
                        "verticalposition": $("#select-ver-pos-Mobile").val(),
                        "buttonposition": $("#select-button-pos-Mobile").val(),
                        "buttoncolor": $("#select-button-color-Mobile").val(),
                        "target": $('#radioOpenInNewPageMobile').is(':checked') ? 1 : 2,
                        "buttontype": $('#btnTypeLinkMobile').is(':checked') ? 1 : 2,
                        "jumbotrondepth": $("#select-button-jmbdepth-Mobile").val()
                    },
                "CampaignId": $('#campaingid').val()

            }

            $.ajax({
                type: "POST",
                url: "/api/desktop/submit/?platformId=2",
                contentType: "application/json",
                data: ko.toJSON(mobileDetails),
                success: function (response) {


                }

            });

        };

    }
    configureBannerForm = document.getElementById('configureBanner');

    var vmconfigureBanner = new configureBanner();
    ko.applyBindings(vmconfigureBanner, configureBannerForm);

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