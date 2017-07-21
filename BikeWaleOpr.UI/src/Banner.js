var $dateInput = $('.datepicker').pickadate({
    selectMonths: true, // Creates a dropdown to control month   
    closeOnSelect: true,
    onClose: function () { if (dateValue != $("#reviewDateEle").val()) $("#reviewDate").val($("#reviewDateEle").val()); },
    onOpen: function () { dateValue = $("#reviewDateEle").val() },
    onSet: function (ele) { if (ele.select) { this.close(); } }
});

var currentFileDesktop, currentFileMobile, bannerId, imgPathDesktop, imgPathMobile;

var bannerDescriptionAlignment = { right: '.campaign-banner__text-box{float:right}', left: '.campaign-banner__text-box{float:left}' };
var buttonColorDesktop = { transparent : '.campaign__target-btn{background:transparent;color:#fff;border:1px solid #fff}.campaign__target-btn:hover{background:#fff;color:#222;}.campaign__target-btn:hover .arrow-white{background-position:-222px -28px}', orange: '.campaign__target-btn{background:#f04031;color:#fff;border:1px solid transparent}.campaign__target-btn:hover{background:#f85649;color:#fff;}' };
var buttonColorMobile = { transparent: '.campaign__target-btn{background:transparent;color:#fff;border:1px solid #fff}', orange: '.campaign__target-btn{background:#f04031;color:#fff;border:1px solid transparent}' };

$(document).ready(function () {    

    $('#startTimeEle').val("00:00:00");
    $('#endTimeEle').val("00:00:00");    

    if ($(".stepper"))
    {
        $('.stepper').activateStepper({ autoFocusInput : false});
    }

    $("#file-desktop").change(function (e) {
        currentFileDesktop = e.target.files[0];       
    });

    $("#file-mobile").change(function (e) {
        currentFileMobile = e.target.files[0];
    });

   bannerId = 10;   //$('#bannerId').val();

});

$('.validate-step').click(function (event) {
    if(!(bannerId && bannerId > 0))
    {        
        event.preventDefault();
        return false;
    }

});

var uploadPhoto = function (e, platformid) {
    e.preventDefault();
    return false;
    try {

        if ($(e.currentTarget).parent().parent().find('.file-path').val() != '') {

            var path, categoryId, curFile, ext;

            if (platformid == 1) //desktop
            {
                categoryId = 4;
                curFile = currentFileDesktop;
                if (curFile)
                    ext = curFile.name.split('.').pop().toLowerCase();
                path = 'bw/' + $('#environment').val() + 'd/banners/homepagebanner-' + bannerId + '-' + (new Date()).getTime() + '.' + ext;
            }
            else {
                categoryId = 5;
                curFile = currentFileMobile;
                if (curFile)
                    ext = curFile.name.split('.').pop().toLowerCase();
                path = 'bw/' + $('#environment').val() + 'm/banners/homepagebanner-' + bannerId + '-' + (new Date()).getTime() + '.' + ext;
            }

            if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
                Materialize.toast('Invalid extension!', 4000);
                return false;
            }

            var imgUpldUtil = uploadToAWS(curFile, 10, 10, path.toLowerCase(), ext, categoryId);
            if (imgUpldUtil && imgUpldUtil.status) {
                if (imgUpldUtil.status) {

                    if (platformid == 1)
                        imgPathDesktop = 'https://imgd.aeplcdn.com/' + '0x0/' + imgUpldUtil.response.originalImagePath;
                    else
                        imgPathMobile = 'https://imgd.aeplcdn.com/' + '0x0/' + imgUpldUtil.response.originalImagePath;

                    Materialize.toast('Image uploaded succesfull!', 4000);
                }
            }

        }
        else {
            Materialize.toast('Please upload image first', 4000);
            return false;
        }
    }
    catch (e) {
        console.log(e);
    }
};

var uploadToAWS = function (file, photoId, itemId, path, ext, categoryId) {
    var imgUpldUtil = new ImageUploadUtility();
    imgUpldUtil.request = { "originalPath": path, "categoryId": categoryId, "itemId": itemId, "isWaterMark": 0, "isMaster": 0, "isMain": 0, "extension": ext };
    imgUpldUtil.photoId = photoId;
    imgUpldUtil.baseURL = $('#bwOprHostUrl').val();
    file.type = "image/" + ext;
    imgUpldUtil.upload(file);
    $(file._removeLink).attr("photoId", (imgUpldUtil.photoId ? imgUpldUtil.photoId : ''));
    return imgUpldUtil;
};

var processBannerPhoto = function (platformId) {
    var css;

    if(platformId == 1) // desktop
    {
        if ($('#select-hori-pos-Desktop').val())
            css = [css, 'background-position-x:', $('#select-hori-pos-Desktop').val(), ';'].join('');
        else
            css = [css, 'background-position-x:center;'].join('');

        if($('#select-ver-pos-Desktop').val())
            css = [css, 'background-position-y:', $('#select-ver-pos-Desktop').val(), ';'].join('');
        else
            css = [css, 'background-position-y:center;'].join('');

        if ($('#txtBackgroundColorDesktop').val())
            css = [css, 'background-color:', $('#txtBackgroundColorDesktop').val(), ';'].join('');
        else
            css = [css, 'background-color:#82888b;'].join('');

        if (imgPathDesktop)
            css = [css, 'background-image:url(', imgPathDesktop, ');'].join('');

        css = [css, 'background-repeat:no-repeat;background-size:cover;height:490px;position:relative'].join('');

        return ['.home-top-banner{', css, '}'].join('');
    }
    else
    {
        if ($('#select-hori-pos-Mobile').val())
            css = [css, 'background-position-x:', $('#select-hori-pos-Mobile').val(), ';'].join('');
        else
            css = [css, 'background-position-x:center;'].join('');

        if ($('#select-ver-pos-Mobile').val())
            css = [css, 'background-position-y:', $('#select-ver-pos-Mobile').val(), ';'].join('');
        else
            css = [css, 'background-position-y:center;'].join('');

        if ($('#txtBackgroundColorMobile').val())
            css = [css, 'background-color:', $('#txtBackgroundColorMobile').val(), ';'].join('');
        else
            css = [css, 'background-color:#82888b;'].join('');

        if (imgPathMobile)
            css = [css, 'background-image:url(', imgPathMobile, ');'].join('');

        css = [css, 'background-repeat:no-repeat;background-size:cover;height:268px;position:relative'].join('');

        return ['.banner-home{', css, '}'].join('');
    }
};

var processJumbotron = function (platformId) {
   
    if (platformId == 1) // desktop
    {
        if ($('#select-jumbotron-depth-Desktop').val())
            return ['.top-campaign-banner-container .welcome-box{margin-top:', $('#select-jumbotron-depth-Desktop').val(), 'px}'].join('');
        else
            return '.top-campaign-banner-container .welcome-box{margin-top:90px}';
    }
    else
    {
        if ($('#select-jumbotron-depth-Mobile').val())
            return ['.top-campaign-banner-container.banner-container{padding-top:', $('#select-jumbotron-depth-Mobile').val(), 'px}'].join('');
        else
            return '.top-campaign-banner-container.banner-container{padding-top:15px}';
    }

};

var processButtoncolor = function (platformId) {

    if(platformId == 1)
    {
        if ($('#select-button-color-Desktop').val())
            return buttonColorDesktop[$('#select-button-color-Desktop').val()];
        else
            return buttonColorDesktop['transparent'];
    }
    else
    {
        if ($('#select-button-color-Mobile').val())
            return buttonColorMobile[$('#select-button-color-Mobile').val()];
        else
            return buttonColorMobile['transparent'];
    }

};

var processBannerPosition = function (platformId) {
    
    if(platformId == 1)
    {
        if ($('#select-banner-pos-Desktop').val())
            return bannerDescriptionAlignment[$('#select-banner-pos-Desktop').val()];
        else
            return bannerDescriptionAlignment['right'];

    }
    else
    {
        if ($('#select-banner-pos-Mobile').val())
            return bannerDescriptionAlignment[$('#select-banner-pos-Mobile').val()];
        else
            return bannerDescriptionAlignment['right'];
    }
};

var processButtonHtml = function (platformId) {

    if(platformId == 1)
    {
        var el = $("<section></section>");
        el.html($('#textareaHtmlDesktop').val());

        if ($('#txtBannerTitleDesktop').val() != "" && el.find('.campaign__title').length > 0)        
            el.find('.campaign__title').text($('#txtBannerTitleDesktop').val());
        
        if(el.find('.campaign__target-btn').length > 0)
        {
            var button = el.find('.campaign__target-btn');

            if ($('#txtCategoryDesktop').val() != "")
                button.attr('c', $('#txtCategoryDesktop').val());

            if($('#txtActionDesktop').val() != "")
                button.attr('a', $('#txtActionDesktop').val());

            if ($('#txtLabelDesktop').val() != "")
                button.attr('l', $('#txtLabelDesktop').val());

            if($("input[name='pageOpenDesktop']:checked").val() == "1")
                button.attr("target", "_blank");

            if (el.find('.campaign-banner-button-text').length > 0 && $('#txtButtonDesktop').val() != "")
                el.find('.campaign-banner-button-text').text($('#txtButtonDesktop').val());

        }

    }
    else {
        
    }

};

//processBannerPhotoPositionDesktop = function () {
//    var desktopcss = $('#textareaCssDesktop').val();

//    if ($('#select-hori-pos-Desktop').val() && desktopcss.contains('background-position-x:'))
//        desktopcss = [desktopcss.slice(0, desktopcss.indexOf('background-position-x:') + 'background-position-x:'.length), $('#select-hori-pos-Desktop').val(), desktopcss.slice(desktopcss.indexOf('background-position-x:') + 'background-position-x:'.length)].join('');

//    if ($('#select-ver-pos-Desktop').val() && desktopcss.contains('background-position-y:'))
//        desktopcss = [desktopcss.slice(0, desktopcss.indexOf('background-position-y:') + 'background-position-y:'.length), $('#select-hori-pos-Desktop').val(), desktopcss.slice(desktopcss.indexOf('background-position-y:') + 'background-position-y:'.length)].join('');

//    if ($('#txtBackgroundColorDesktop').val() && desktopcss.contains('background-color:'))
//        desktopcss = [desktopcss.slice(0, desktopcss.indexOf('background-color:') + 'background-color:'.length), $('#txtBackgroundColorDesktop').val(), desktopcss.slice(desktopcss.indexOf('background-color:') + 'background-color:'.length)].join('');
//};