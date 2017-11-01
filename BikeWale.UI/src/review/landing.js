var reviewSelectModel, reviewSelectMake;

docReady(function () {
    $('#globalSearch').parent().hide();
    $('#discoverBrand').removeClass('bw-tabs-data'); // to avoid hidind of brands widget on user reviews
    $('.bw-tabs li').click(function () {
        var dataTabs = $(this).attr('data-tabs');
        var bannerReveiw = $(this).closest('.banner-review')
        if (dataTabs == 'userReviewContent') {
            bannerReveiw.attr('data-bg-image', '0');
        }
        else if (dataTabs == 'expertReviewContent') {
            bannerReveiw.attr('data-bg-image', '1');
        }
    });
    reviewSelectModel = $("#reviewSelectModel"), reviewSelectMake = $("#reviewSelectMake");

    $('.make-model-chosen').chosen({ no_results_text: "No matches found!!" });
    $('div.chosen-container').attr('style', 'width:100%;border:0');

    $('select').prop('selectedIndex', 0);

    $("#applyFiltersBtn").click(function () {
        if (!isNaN(reviewSelectMake.val()) && reviewSelectMake.val() != "0") {
            if (!isNaN(reviewSelectModel.val()) && reviewSelectModel.val() != "0") {
                var returnUrl = $(this).data('query');
                window.location.href = "/rate-your-bike/" + Number(reviewSelectModel.val()) + "/?q=" + returnUrl;

            }
            else {
                toggleErrorMsg(reviewSelectModel, true, "Choose a Model");
            }
        }
        else {
            toggleErrorMsg(reviewSelectMake, true, "Choose a Make");
        }
    });
    reviewSelectModel.change(function () {
        toggleErrorMsg(reviewSelectModel, false);

    });
    reviewSelectMake.change(function () {
        toggleErrorMsg(reviewSelectMake, false);
        var selectedMakeId = reviewSelectMake.val();
        reviewSelectModel.empty();
        if (!isNaN(selectedMakeId) && selectedMakeId != "0") {
                $.ajax({
                    type: "GET",
                    url: "/api/modellist/?requestType=3&makeId=" + selectedMakeId,
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (data) {
                        setOptions(data.modelList);
                    },
                    complete: function (xhr) {
                        if (xhr.status == 404 || xhr.status == 204) {
                            setOptions(null);
                        }
                    }
                });
        }
        else {
            setOptions(null);
        }
    });
    function setOptions(optList) {
        toggleErrorMsg(reviewSelectModel, false);
        if (optList != null) {
            reviewSelectModel.append($('<option>').text(" Select Model ").attr({ 'value': "0" }));
            $.each(optList, function (i, value) {
                reviewSelectModel.append($('<option>').text(value.modelName).attr({ 'value': value.modelId, 'maskingName': value.maskingName }));
            });
        }

        reviewSelectModel.trigger('chosen:updated');
        $("#reviewSelectModel_chosen .chosen-single.chosen-default span").text("No cities available");
    }

    $(".tabs-type-switch").click(function (e) {
        if (e.target.getAttribute('data-tabs') == 'expertReviewContent') {
            $('#nonUpcomingBikes').attr('data-contentTab', "expertReview");
            $('#nonUpcomingBikes').val('');
            triggerGA('Bike_Reviews', 'Toggle_ExpertReviews_Clicked', '');
        }

        if (e.target.getAttribute('data-tabs') == 'userReviewContent') {
            $('#nonUpcomingBikes').attr('data-contentTab', "userReview");
            $('#nonUpcomingBikes').val('');
            triggerGA('Bike_Reviews', 'Toggle_UserReviews_Clicked', '');
        }
    });
});

