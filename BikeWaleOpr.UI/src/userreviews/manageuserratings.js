var userRating = $("#UserRatings");
var userId = userRating.data("userid");

function checkSelected() {
    if ($("input:checked").length == 0) {
        $('#checkboxEmpty').modal('open');
        return false;
    }
    else
        return true;
};

var UserRatings = function () {
    var self = this;
    self.reviewIdList = '';

    self.approveReviews = function () {
        if (checkSelected()) {
            self.findSelectedReviews();
            self.changeStatus(2);
        }
    };

    self.discardReviews = function () {
        if (checkSelected()) {
            self.findSelectedReviews();
            self.changeStatus(3);
        }
    };

    self.approveReview = function (reviewId) {
        self.reviewIdList = reviewId;
        self.changeStatus(2);
    };

    self.discardReview = function (reviewId) {
        self.reviewIdList = reviewId;
        self.changeStatus(3);
    };

    self.findSelectedReviews = function () {      
        $("input:checked").each(function () {
            if ($(this).attr('data-review-id'))
                self.reviewIdList = self.reviewIdList + $(this).attr('data-review-id') + ',';
        });
    };

    self.changeStatus = function (status) {
        $.ajax({
            type: "POST",
            url: "/api/userreviews/ids/" + self.reviewIdList + "/update/" + status + "/?moderatedId=" + userId,
            contentType: "application/json",
            dataType: 'json',
            success: function (response) {
                if (response) {                   
                    var array = self.reviewIdList.toString().split(',');
                    for (var i = 0; i < array.length; i++) {

                        if (array[i]) {
                            $("#btnApprove_" + array[i]).closest("tr").fadeOut();
                            $("#btnApprove_" + array[i]).closest("tr").remove();
                            Materialize.toast("Review Id " + array[i] + " Status Updated Successfully", 2000);
                        }
                    }
                   
                }
            },
            complete: function (xhr) {
                if (xhr.status != 200) {
                    Materialize.toast("Failed to update", 2000);
                }
                self.reviewIdList = '';
            }
        });
    }

};

var vmUserRating = new UserRatings;
if (userRating) {
    ko.applyBindings(vmUserRating, userRating[0]);   
}

$('#chkApprove').change(function () {
    if (this.checked)
    {
        $('input:checkbox').each(function () {
            $(this).prop('checked', true);
        });
    }
    else
    {
        $('input:checkbox').each(function () {
            $(this).prop('checked', false);
        });
    }    
});