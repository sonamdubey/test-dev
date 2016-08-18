$(document).ready(function () {
    // Initial setting when the page loads
    var StartIndex = 0;
    var LiIndex = 0;
    $('#galleryList li').each(function () {
        if ($(this).children('a').attr('href').indexOf(ImageName) > 0)
            StartIndex = LiIndex;
        LiIndex++
    });
    var galleries = $('.ad-gallery').adGallery({
        start_at_index: StartIndex,
        loader_image: "http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/circleloader.gif",
        callbacks: { // CallBack function after the image is loaded and is visible
            afterImageVisible: function () {
                $('#artTitle').html(this.images[this.current_index].artTitle);
                $('#artTitle').attr('href', this.images[this.current_index].artUrl);
                var gallery_info = this.gallery_info.html();
                if (this.images[this.current_index].imgCnt == 0) {
                    this.gallery_info.hide();
                    $('div.ad-forward').add('div.ad-back').add('div.ad-next').add('div.ad-prev').hide();
                }
            }
        }
    });
    InitGallery(galleries); // Initialize the gallery
    LoadGallery(MakeId, ModelId); // Load the Similar and other models Gallery

    // Paging Controls
    $(".dgNavDivTop span.pointer,.sortLink").live('click', function (e) {
        e.preventDefault();
       // alert($(this).attr("url"));
        //var navi_lnk = this.href;
        //var qs = navi_lnk.split("?")[1];
        var qs = $(this).attr("url").split("?")[1];
        var qsNew = qs.substr(0, qs.length - 1);
        //alert(qsNew);
        //alert($(this).closest('div').html());

        $("#divOtherModelsGallery").load("/new/photos/othermodelsgallery.aspx?" + qsNew, function () {
                    $("#omLoading").hide();
        });

        //Commented By : Ashwini Todkar on 7 Oct 2014

        //if ($(this).closest('div').attr("id") == 'divOtherModelsGallery') {
        //    $("#omLoading").show();
        //    $("#divOtherModelsGallery").load("/new/photos/othermodelsgallery.aspx?" + qsNew, function () {
        //        $("#omLoading").hide();
        //    });
        //} else {
        //    $("#sgLoading").show();
        //    $("#divSimilarGallery").load("/new/photos/othermodelsgallery.aspx?" + qsNew, function () {
        //        $("#sgLoading").hide();
        //    });
        //}
    });

    // Information about an album for the Similar and Other models gallery
    $("div.img-placer").live('mouseenter', function () {
        $(this).find('div.rollover-container').stop(true, true).slideDown();
    }).live('mouseleave', function () {
        $(this).find('div.rollover-container').stop(true, true).slideUp();
    });
});


// Get the values required to load the Similar and Other Models Gallery
function LoadGallery(makeId, modelId) {
    var mId = makeId;
    var moId = modelId;
    //var makehashParams = "mId=" + mId;
    var modelhashParams = "moId=" + moId;
    //LoadPhotoGalleries(makehashParams + '&' + modelhashParams);
   // alert(modelhashParams);
    LoadPhotoGalleries(modelhashParams);
}
// Load the Similar and Other Models Gallery
function LoadPhotoGalleries(params) {
    //alert(params);
    //alert("/new/photos/othermodelsgallery.aspx?" + params);
    //alert("in load photo gallery");
    $("#divOtherModelsGallery").load("/new/photos/othermodelsgallery.aspx?" + params);
    $("#divSimilarGallery").load("/new/photos/othermodelsgallery.aspx?" + params);
}

// Gallery Initialization function.
function InitGallery(galleries) {
    galleries[0].settings.description_wrapper = $('#descriptions');
    galleries[0].slideshow.disable();
    // Add custom Effects to the Gallery
    galleries[0].addAnimation('fadeNew', function (img_container, direction, desc) {
        img_container.css('opacity', 0);
        if (desc) {
            desc.animate({ opacity: 1 }, 5);
        };
        if (this.current_description) {
            this.current_description.animate({ opacity: 0 }, 5);
        };
        return {
            old_image: { opacity: 0 },
            new_image: { opacity: 1 }
        };
    });
    galleries[0].settings.effect = "fadeNew";
    //alert(galleries[0].settings.start_at_index);
}