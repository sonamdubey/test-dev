var $dateInput = $('.datepicker').pickadate({
    selectMonths: true, // Creates a dropdown to control month
    selectYears: 15 // Creates a dropdown of 15 years to control year
});
// Use the picker object directly.
var $dateInput = $dateInput.pickadate('picker')
var userReview = $("#UserReviews");
var userId = userReview.data("userid");

var dummyJSON = '{"overallRating":{"id":0,"value":0,"description":"","heading":"Superb!!","responseHeading":null},"make":{"makeId":0,"makeName":"","maskingName":"","hostUrl":null,"logoUrl":null},"model":{"modelId":0,"modelName":"","maskingName":""},"originalImgPath":"","hostUrl":"","description":"","title":"","tips":"","overallRatingId":0,"questions":[{"qId":0,"qtype":0,"heading":"","description":"","selectedRatingId":0,"displayType":0,"rating":[{"ratingId":0,"ratingValue":"0","ratingText":""}],"isRequired":"true","visibility":"true","subQuestionId":0}],"customerName":"","customerEmail":""}';

var UserReviews = function () {
    var self = this;
    self.selectedMakeId = ko.observable();
    self.selectedModel = ko.observable();
    self.bikeModels = ko.observableArray();
    self.selectedReviewStatus = ko.observable();
    self.selectedDate = ko.observable();
    self.selectedReviewId = ko.observable();
    self.reviewSummary = ko.observable(JSON.parse(dummyJSON));
    self.reviewTitle = ko.observable();
    self.reviewDescription = ko.observable();
    self.reviewTips = ko.observable();
    self.disapprovalId = ko.observable();

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

                $(document).find('.modal').modal();
            
        }

    };

    self.getUserReviewDetails = function (reviewId) {
        self.selectedReviewId(reviewId);
       
        if (self.selectedReviewId() && self.selectedReviewId() > 0) {
            $.ajax({
                type: "GET",
                url: "/api/userreviews/id/" + self.selectedReviewId() + "/summary/",
                contentType: "application/json",
                dataType: 'json',
                success : function(response)
                {
                    if (response)
                    {
                        debugger;
                        self.reviewSummary(response);
                        self.reviewTitle(response.title);
                        self.reviewDescription(response.description);
                        self.reviewTips(response.tips);
                        Materialize.toast("Successfully fetched details for user review", 5000);
                    }
                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        alert("Failed to load user data");
                    }                    
                }
            });
        }
    };

    self.approveReview = function () {
        if (self.selectedReviewId() && self.selectedReviewId() > 0 && self.reviewSummary()) {
            var summary = self.reviewSummary();
            objData = {
                "reviewid" : self.selectedReviewId(),
                "ReviewStatus" : "2",
                "ModeratorId": userId,
                "Review": self.reviewDescription(),
                "ReviewTitle": self.reviewTitle(),
                "ReviewTips": self.reviewTips(),
                "CustomerName" : summary.customerName,
                "CustomerEmail" : summary.customerEmail,
                "BikeName" : summary.make.makeName + " " + summary.model.modelName,
                "MakeMaskingName" : summary.model.maskingName,
                "ModelMaskingName" : summary.make.maskingName

            };

            $.ajax({
                type: "POST",
                url : "/api/userreviews/id/" + self.selectedReviewId()+"/updatestatus/",
                contentType: "application/json",
                dataType: 'json',
                data : ko.toJSON(objData),
                success: function (response) {
                    if (response) { 
                        $("#btnViewDetails_" + vmUserReview.selectedReviewId()).closest("tr").fadeOut();
                        $('#reviewdetails').modal('close');
                        self.selectedReviewId(0);
                        Materialize.toast("User Review approved successfully", 5000);
                    }
                    else {
                        Materialize.toast("User review not approved", 5000);
                    }

                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        Materialize.toast("User review not approved",5000);
                    }
                   
                }
            });
        }
    };

    self.rejectReview = function () {
        self.disapprovalId($("input[name='disapprovalReason']:checked").val());
        if (self.selectedReviewId() && self.selectedReviewId() > 0 && self.reviewSummary() && self.disapprovalId()) {
            var summary = self.reviewSummary();
            objData = {
                "reviewid": self.selectedReviewId(),
                "ReviewStatus": "3",
                "ModeratorId": userId,
                "DisapprovalReasonId": self.disapprovalId(),
                "Review": self.reviewDescription(),
                "ReviewTitle": self.reviewTitle(),
                "ReviewTips": self.reviewTips(),
                "CustomerName": summary.customerName,
                "CustomerEmail": summary.customerEmail,
                "BikeName": summary.make.makeName + " " + summary.model.modelName,
                "MakeMaskingName": summary.model.maskingName,
                "ModelMaskingName": summary.make.maskingName

            };

            $.ajax({
                type: "POST",
                url: "/api/userreviews/id/" + self.selectedReviewId() + "/updatestatus/",
                contentType: "application/json",
                dataType: 'json',
                data: ko.toJSON(objData),
                success: function (response) {
                    if (response) {
                        $("#btnViewDetails_" + vmUserReview.selectedReviewId()).closest("tr").fadeOut();
                        $('#reviewdetails').modal('close');
                        $('#rejectionReason').modal('close');
                        self.disapprovalId(0);
                        self.selectedReviewId(0);
                        $("input[name='disapprovalReason']:checked").prop("checked", false);
                        Materialize.toast("User Review rejected successfully", 5000);
                    }
                    else {
                        Materialize.toast("User review not rejected", 5000);
                    }

                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        Materialize.toast("User review not rejected",5000);
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





