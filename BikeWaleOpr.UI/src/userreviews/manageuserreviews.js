var $dateInput = $('.datepicker').pickadate({
    selectMonths: true, // Creates a dropdown to control month
    selectYears: 15 // Creates a dropdown of 15 years to control year
});
// Use the picker object directly.
var $dateInput = $dateInput.pickadate('picker')

var userReview = $("#UserReviews");

var UserReviewSummary = function(reviewId)
{
    var self = this;
    self.selectedReviewId = ko.observable(reviewId);
    self.title = ko.observable();
    self.description = ko.observable();
    self.ratings = ko.observableArray([]);
    self.selectedRatingValue = ko.observable();
    self.selectedRating = ko.observable();
    self.questions = ko.observableArray();

    self.getUserReviewDetails = function () {
        if (self.selectedReviewId() && self.selectedReviewId() > 0) {
            $.ajax({
                type: "GET",
                url: "localhost:9011/api/user-reviews/summary/" + self.selectedReviewId() + "/",
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if (response) {
                        //user review details fetched
                        console.log(response);
                        //self.title();
                    }
                    else {
                        alert("User Review failed Approved");
                    }

                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        alert("User failed Review Approved");
                    }
                }
            });
        }
    };

    self.approveReview = function () {
        if (self.selectedReviewId() && self.selectedReviewId() > 0) {
            $.ajax({
                type: "GET",
                url: "/api/UpdateUserReviewsStatus/?reviewStatus=2&moderatorId=1&disapprovalReasonId=1&reviewId=" + self.selectedReviewId(),
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if (response) {
                        alert("User Review Approved");
                    }
                    else {
                        alert("User Review failed Approved");
                    }

                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        alert("User failed Review Approved");
                    }
                }
            });
        }
    };

    self.rejectReview = function () {
        if (self.selectedReviewId() && self.selectedReviewId() > 0) {
            $.ajax({
                type: "GET",
                url: "/api/UpdateUserReviewsStatus/?reviewStatus=3&moderatorId=1&disapprovalReasonId=" + disapprovalReasonId + "&reviewId=" + self.selectedReviewId(),
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if (response) {
                        alert("User Review Approved");
                    }
                    else {
                        alert("User Review failed Approved");
                    }

                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        alert("User failed Review Approved");
                    }
                }
            });
        }
    };

}

var UserReviews = function () {
    var self = this;

    self.selectedMakeId = ko.observable();
    self.selectedModel = ko.observable();
    self.bikeModels = ko.observableArray();
    self.selectedReviewStatus = ko.observable();
    self.selectedDate = ko.observable();
    self.selectedReviewId = ko.observable();
    self.reviewSummary = ko.observable();

    self.changeMake = function (d, e) {
        var makeId = $(e.target).val();
        if (makeId) {
            self.selectedMakeId(parseInt(makeId));
        }

        if (self.selectedMakeId() && self.selectedMakeId() > 0) {
            $.ajax({
                type: "GET",
                url: "api/models/makeid/" + self.selectedMakeId() + "/requesttype/8/",
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if (response && response.length > 0) {
                        self.bikeModels(response);
                        $('select[name="ModelId"]').val(userReview.data("modelid")).material_select();
                    }
                    else {
                        self.bikeModels([]);
                    }

                },
                complete: function (xhr) {
                    if (xhr.status != 200)
                    {
                        self.bikeModels([]);
                    }
                    $('select[name="ModelId"]').material_select();
                }
            });
        }

    };

    self.changeStatus = function(d, e)
    {
        var reviewStatus = $(e.target).val();
        if(reviewStatus && reviewStatus > 0)
        {
            $("input[type='hidden'][name='ReviewStatus']").val(reviewStatus);
        }
    }

    self.setPageFilters = function () {

        if(userReview)
        {
            self.selectedMakeId(userReview.data("makeid"));
            if(self.selectedMakeId())
            {
                $('select[name="MakeId"]').val(self.selectedMakeId()).trigger("change").material_select();
            }
            var reviewStatus = userReview.data("reviewstatus");
            if (reviewStatus && reviewStatus > 0)
            {
                $("input[type='radio'][name='rdoReviewStatus'][value=" + reviewStatus + "]").trigger("click");
                $("input[type='hidden'][name='ReviewStatus']").val(reviewStatus);
            }

            $dateInput.set('select', new Date(userReview.data("date")));
        }

    };

    self.getUserReviewDetails = function (reviewId) {
        self.selectedReviewId(reviewId);
        if (self.selectedReviewId() && self.selectedReviewId() > 0) {
            $.ajax({
                type: "GET",
                url: "localhost:9011/api/user-reviews/summary/" + self.selectedReviewId() + "/",
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if (response) {
                        //user review details fetched
                        console.log(response);
                        //self.title();
                    }
                    else {
                        alert("User Review failed Approved");
                    }

                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        alert("User failed Review Approved");
                    }
                }
            });
        }
    };



};

var vmUserReview = new UserReviews;
if (userReview)
{
    ko.applyBindings(vmUserReview, userReview[0]);
    vmUserReview.setPageFilters();
}

//initialize model popup
$('.modal').modal();
