var currentFileDesktop, currentFileMobile, bannerId, imgPathDesktop, imgPathMobile;

var bannerDescriptionAlignment = { right: '.campaign-banner__text-box{float:right}', left: '.campaign-banner__text-box{float:left}' };
var buttonColorDesktop = { transparent : '.campaign__target-btn{background:transparent;color:#fff;border:1px solid #fff}.campaign__target-btn:hover{background:#fff;color:#222;}.campaign__target-btn:hover .arrow-white{background-position:-222px -28px}', orange: '.campaign__target-btn{background:#f04031;color:#fff;border:1px solid transparent}.campaign__target-btn:hover{background:#f85649;color:#fff;}' };
var buttonColorMobile = { transparent: '.campaign__target-btn{background:transparent;color:#fff;border:1px solid #fff}', orange: '.campaign__target-btn{background:#f04031;color:#fff;border:1px solid transparent}' };

var compulsoryDesktopCss = ".top-campaign-banner-container .welcome-box h1{margin-bottom:5px}.top-campaign-banner-container .margin-top60{margin-top:25px}.campaign-banner__wrapper{width:996px;margin:0 auto;position:absolute;bottom:20px;left:0;right:0}.campaign-banner__wrapper:after{content:'';display:block;clear:both}.campaign-banner__text-box{width:280px;}.campaign__title{font-size:14px;text-align:left;color:#fff;margin-bottom:5px}.campaign__target-btn{font-size:14px;padding:5px 8px;}.arrow-white{width:6px;height:10px;background-position:-222px -14px;margin-left:5px}";
    
var compulsoryMobileCss = ".top-campaign-banner-container.banner-container h1{margin-bottom:5px}.top-campaign-banner-container.banner-container .banner-subheading{margin-bottom:15px}.campaign-banner__wrapper{position:absolute;bottom:20px;left:10px;right:10px}.campaign-banner__wrapper:after{content:'';display:block;clear:both}.campaign__title{width:60%;font-size:12px;text-align:left;float:left;color:#fff}.campaign__target-btn{font-size:14px;padding:5px 8px;float:right}.arrow-white{width:6px;height:10px;background-position:-204px -49px;margin-left:5px}";

var compulsoryHtml = '<div class="campaign-banner__wrapper"><div class="campaign-banner__text-box"><p class="campaign__title"></p><a class="btn campaign__target-btn bw-ga"><span class="campaign-banner-button-text"></span><span class="bwsprite arrow-white"></span></a></div></div>';

$(document).ready(function () {
bannerId = $('#bannerId').val();
    $('#startTimeEle').val("00:00:00");
    $('#endTimeEle').val("00:00:00");
    if ($(".stepper")) {
        $('.stepper').activateStepper({ autoFocusInput: false });
    }
    $("#file-desktop").change(function (e) {
        currentFileDesktop = e.target.files[0];
    });
    $("#file-mobile").change(function (e) {
        currentFileMobile = e.target.files[0];
    });

    if($('#textareaHtmlDesktop').val() == '')
        $('#textareaHtmlDesktop').val(compulsoryHtml);

    if ($('#textareaHtmlMobile').val() == '')
        $('#textareaHtmlMobile').val(compulsoryHtml);

    imgPathDesktop = $('#desktop-image').data('src');
    imgPathMobile = $('#mobile-image').data('src');

    var configureBanner = function () {
        var self = this;

        self.bannerDescriptionMsg = ko.observable("");
        self.campaignStartDateMsg = ko.observable("");

        self.validateBasicDetails = function () {
            var isValid = true;

            if($('#textareaBannerDesc').val() == "")
            {
                self.bannerDescriptionMsg('*Required');
                isValid = false;
            }

            if ($('#startDateEle').val() == "")
            {
                self.campaignStartDateMsg('*Required');
                isValid = false;
            }

            return isValid;
        };

        self.Configure = function () {

            if (self.validateBasicDetails()) {
                var queries = {};
                if (document.location.search != "") {
                    $.each(document.location.search.substr(1).split('&'), function (c, q) {
                        var i = q.split('=');
                        queries[i[0].toString()] = i[1].toString();
                    });
                }

                var basicDetails = {
                    "startdate": $('#startDateEle').val() + ' ' + $('#startTimeEle').val(),
                    "enddate": $('#endDateEle').val() + ' ' + $('#endTimeEle').val(),
                    "bannerdescription": $('#textareaBannerDesc').val(),
                    "campaignid": queries
                }
                $.ajax({
                    type: "POST",
                    url: "/api/bannerbasic/save/",
                    contentType: "application/json",
                    data: ko.toJSON(basicDetails),
                    success: function (response) {
                        $('#bannerId').val(response);
                        bannerId = response;
                        Materialize.toast('Banner basic details saved', 4000);
                        $('.stepper').nextStep();
                    }
                });
            }
        };

        self.backgroundColorMobileMsg = ko.observable("");
        self.bannerTitleMobileMsg = ko.observable("");
        self.buttonTextMobileMsg = ko.observable("");
        self.buttonLinkMobileMsg = ko.observable("");
        self.categoryMobileMsg = ko.observable("");
        self.actionMobileMsg = ko.observable("");
        self.labelMobileMsg = ko.observable("");

        self.backgroundColorDesktopMsg = ko.observable("");
        self.bannerTitleDesktopMsg = ko.observable("");
        self.buttonTextDesktopMsg = ko.observable("");
        self.buttonLinkDesktopMsg = ko.observable("");
        self.categoryDesktopMsg = ko.observable("");
        self.actionDesktopMsg = ko.observable("");
        self.labelDesktopMsg = ko.observable("");


        self.validateDesktop = function () {
            var isValid = true;
            self.backgroundColorDesktopMsg('');
            self.bannerTitleDesktopMsg('');
            self.buttonTextDesktopMsg('');
            self.buttonLinkDesktopMsg('');
            self.categoryDesktopMsg('');
            self.actionDesktopMsg('');
            self.labelDesktopMsg('');
            Materialize.updateTextFields();

            if ($('#txtBackgroundColorDesktop').val() == '') {
                self.backgroundColorDesktopMsg('Please enter back ground color');
                isValid = false;
            }

            if ($('#txtBannerTitleDesktop').val() == '') {
                self.bannerTitleDesktopMsg('Please enter banner title');
                isValid = false;
            }

            if ($('#txtButtonDesktop').val() == '') {
                self.buttonTextDesktopMsg('Please enter button text');
                isValid = false;
            }

            if ($('#txtButtonlinkDesktop').val() == '') {
                self.buttonLinkDesktopMsg('Please enter button link');
                isValid = false;
            }

            if ($('#txtCategoryDesktop').val() == '') {
                self.categoryDesktopMsg('Required');
                isValid = false;
            }

            if ($('#txtActionDesktop').val() == '') {
                self.actionDesktopMsg('Required');
                isValid = false;
            }

            if ($('#txtLabelDesktop').val() == '') {
                self.labelDesktopMsg('Required');
                isValid = false;
            }

            return isValid;
        };

        self.saveDesktop = function () {

            if (self.validateDesktop()) {

                $('#ModifiedCssDesktop').val(processCss(1));
                $('#ModifiedHtmlDesktop').val(processButtonHtml(1));

                var desktopDetails = {
                    "DesktopBannerDetails":
                        {
                            "html": $('#ModifiedHtmlDesktop').val(),
                            "css": $('#ModifiedCssDesktop').val(),
                            "js": $('#textareaJsDesktop').val(),
                            "unmodifiedhtml": $('#textareaHtmlDesktop').val(),
                            "unmodifiedcss": $('#textareaCssDesktop').val(),
                          
                            "backgroundcolor": $('#txtBackgroundColorDesktop').val(),
                            "bannertitle": $('#txtBannerTitleDesktop').val(),
                            "buttontext": $('#txtButtonDesktop').val(),
                            "targethref": $('#txtButtonlinkDesktop').val(),
                            "horizontalposition": $('#select-hori-pos-Desktop').val(),
                            "verticalposition": $("#select-ver-pos-Desktop").val(),
                            "buttonposition": $("#select-banner-pos-Desktop").val(),
                            "buttoncolor": $("#select-button-color-Desktop").val(),
                            "target": $('#radioOpenInNewPage').is(':checked') ? 1 : 2,
                            "buttontype": $('#btnTypeLinkDesktop').is(':checked') ? 1 : 2,
                        "jumbotrondepth": $("#select-button-jmbdepth-Desktop").val(),
                        "category":$('#txtCategoryDesktop').val(),
                        "action": $('#txtActionDesktop').val(),
                        "label": $('#txtLabelDesktop').val()
                        },
                    "CampaignId": $('#bannerId').val()
                }
                $.ajax({
                    type: "POST",
                    url: "/api/bannerproperties/save/1",
                    contentType: "application/json",
                    data: ko.toJSON(desktopDetails),
                    success: function (response) {
                        Materialize.toast('Desktop banner configured', 4000);
                        $('.stepper').nextStep();
                    }
                });
            }
        };

        self.validateMobile = function () {

            var isValid = true;
            self.backgroundColorMobileMsg('');
            self.bannerTitleMobileMsg('');
            self.buttonTextMobileMsg('');
            self.buttonLinkMobileMsg('');
            self.categoryMobileMsg('');
            self.actionMobileMsg('');
            self.labelMobileMsg('');
            Materialize.updateTextFields();

            if ($('#txtBackgroundColorMobile').val() == '') {
                self.backgroundColorMobileMsg('Please enter back ground color');
                isValid = false;
            }

            if ($('#txtBannerTitleMobile').val() == '') {
                self.bannerTitleMobileMsg('Please enter banner title');
                isValid = false;
            }

            if ($('#txtButtonMobile').val() == '') {
                self.buttonTextMobileMsg('Please enter button text');
                isValid = false;
            }

            if ($('#txtButtonlinkMobile').val() == '') {
                self.buttonLinkMobileMsg('Please enter button link');
                isValid = false;
            }

            if ($('#txtCategoryMobile').val() == '') {
                self.categoryMobileMsg('Required');
                isValid = false;
            }

            if ($('#txtActionMobile').val() == '') {
                self.actionMobileMsg('Required');
                isValid = false;
            }

            if ($('#txtLabelMobile').val() == '') {
                self.labelMobileMsg('Required');
                isValid = false;
            }

            return isValid;
        };

        self.saveMobile = function () {

            if (self.validateMobile()) {

                $('#ModifiedCssMobile').val(processCss(2));
                $('#ModifiedHtmlMobile').val(processButtonHtml(2));

                var mobileDetails = {
                    "MobileBannerDetails":
                        {
                            "html": $('#ModifiedHtmlMobile').val(),
                            "css": $('#ModifiedCssMobile').val(),
                            "js": $('#textareaJsMobile').val(),
                            "unmodifiedhtml": $('#textareaHtmlMobile').val(),
                            "unmodifiedcss": $('#textareaCssMobile').val(),
                            "backgroundcolor": $('#txtBackgroundColorMobile').val(),
                            "bannertitle": $('#txtBannerTitleMobile').val(),
                            "buttontext": $('#txtButtonMobile').val(),
                            "targethref": $('#txtButtonlinkMobile').val(),
                            "horizontalposition": $('#select-hori-pos-Mobile').val(),
                            "verticalposition": $("#select-ver-pos-Mobile").val(),
                            "buttonposition": $("#select-banner-pos-Mobile").val(),
                            "buttoncolor": $("#select-button-color-Mobile").val(),
                            "target": $('#radioOpenInNewPageMobile').is(':checked') ? 1 : 2,
                            "buttontype": $('#btnTypeLinkMobile').is(':checked') ? 1 : 2,
                        "jumbotrondepth": $("#select-button-jmbdepth-Mobile").val(),
                        "category": $('#txtCategoryMobile').val(),
                        "action": $('#txtActionMobile').val(),
                        "label": $('#txtLabelMobile').val()
                        },
                    "CampaignId": $('#bannerId').val()
                }
                $.ajax({
                    type: "POST",
                    url: "/api/bannerproperties/save/2",
                    contentType: "application/json",
                    data: ko.toJSON(mobileDetails),
                    success: function (response) {
                        Materialize.toast('Mobile banner configured', 4000);
                        $('.stepper').nextStep();
                    }
                });
            }
        }

        self.uploadToAWS = function (file, photoId, itemId, path, ext, categoryId) {
            if (file) {
                var imgUpldUtil = new ImageUploadUtility();
                imgUpldUtil.request = { "originalPath": path, "categoryId": categoryId, "itemId": itemId, "isWaterMark": 0, "isMaster": 0, "isMain": 0, "extension": ext };
                imgUpldUtil.photoId = photoId;
                imgUpldUtil.baseURL = $('#bwOprHostUrl').val();
                file.type = "image/" + ext;
                imgUpldUtil.upload(file);
                $(file._removeLink).attr("photoId", (imgUpldUtil.photoId ? imgUpldUtil.photoId : ''));
                return imgUpldUtil;
            }
        };

         self.uploadPhoto = function (e, platformid) {
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

                    if (curFile && $.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1 && (bannerId > 0)) {
                        Materialize.toast('Invalid extension!', 4000);
                        return false;
                    }

                    var imgUpldUtil = self.uploadToAWS(curFile, bannerId, bannerId, path.toLowerCase(), ext, categoryId);
                    if (imgUpldUtil && imgUpldUtil.status) {
                        if (imgUpldUtil.status) {

                            if (platformid == 1)
                                imgPathDesktop = 'https://imgd.aeplcdn.com/' + '0x0/' + imgUpldUtil.response.originalImagePath;
                            else
                                imgPathMobile = 'https://imgd.aeplcdn.com/' + '0x0/' + imgUpldUtil.response.originalImagePath;

                            Materialize.toast('Image uploaded succesfull!', 4000);

                            if (platformid == 1)
                                Materialize.toast('Please configure desktop banner to reflect photo', 4000);
                            else
                                Materialize.toast('Please configure mobile banner to reflect photo', 4000);
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
    }
    configureBannerForm = document.getElementById('configureBanner');
    var vmconfigureBanner = new configureBanner();
    ko.applyBindings(vmconfigureBanner, configureBannerForm);
});

var $dateInput = $('.datepicker').pickadate({
    selectMonths: true, // Creates a dropdown to control month   
    closeOnSelect: true,
    onClose: function () { if (dateValue != $("#reviewDateEle").val()) $("#reviewDate").val($("#reviewDateEle").val()); },
    onOpen: function () { dateValue = $("#reviewDateEle").val() },
    onSet: function (ele) { if (ele.select) { this.close(); } }
    });

$('.validate-step').click(function (event) {
    if (!(bannerId && bannerId > 0)) {
        event.preventDefault();
        Materialize.toast('Please configure bannner first', 4000);
        return false;
    }
});



var processCss = function (platformId) {
    if (platformId == 1) {
        return ['<style type="text/css">', $('#textareaCssDesktop').val(), compulsoryDesktopCss, processBannerPhoto(1), processJumbotron(1), processButtoncolor(1), processBannerPosition(1), '</style>'].join('');
    }
    else
    {
        return ['<style type="text/css">', $('#textareaCssMobile').val(), compulsoryMobileCss, processBannerPhoto(2), processJumbotron(2), processButtoncolor(2), processBannerPosition(2), '</style>'].join('');
    }
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

    var el = $("<section></section>");

    if(platformId == 1)
    {        
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

            if ($("input[name='group4']:checked").val() == "1")
                button.attr("target", "_blank");

            if (el.find('.campaign-banner-button-text').length > 0 && $('#txtButtonDesktop').val() != "")
                el.find('.campaign-banner-button-text').text($('#txtButtonDesktop').val());

            if($('#txtButtonlinkDesktop').val() != "")
                button.attr('href', $('#txtButtonlinkDesktop').val());
        }

    }
    else {

        el.html($('#textareaHtmlMobile').val());

        if ($('#txtBannerTitleMobile').val() != "" && el.find('.campaign__title').length > 0)
            el.find('.campaign__title').text($('#txtBannerTitleMobile').val());

        if (el.find('.campaign__target-btn').length > 0) {
            var button = el.find('.campaign__target-btn');

            if ($('#txtCategoryMobile').val() != "")
                button.attr('c', $('#txtCategoryMobile').val());

            if ($('#txtActionMobile').val() != "")
                button.attr('a', $('#txtActionMobile').val());

            if ($('#txtLabelMobile').val() != "")
                button.attr('l', $('#txtLabelMobile').val());

            if ($("input[name='group2']:checked").val() == "1")
                button.attr("target", "_blank");

            if (el.find('.campaign-banner-button-text').length > 0 && $('#txtButtonMobile').val() != "")
                el.find('.campaign-banner-button-text').text($('#txtButtonMobile').val());

            if ($('#txtButtonlinkMobile').val() != "")
                button.attr('href', $('#txtButtonlinkMobile').val());
        }        
    }

    return el.html();
};
