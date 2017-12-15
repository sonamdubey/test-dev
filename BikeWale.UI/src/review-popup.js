// review popup
var reviewPopupCotent, desktopUserReview, reviewPopup;
var reviewId = 0;
var bwSpinner;

// Like dislike review code
function upVoteReview() {
	bwcache.set("ReviewDetailPage_reviewVote_" + reviewId, { "vote": "1" });
	$('#upvoteBtn-' + reviewId).addClass('active');
	$('#downvoteBtn-' + reviewId).attr('disabled', 'disabled');

	$('#upvoteCount').text(parseInt($('#upvoteCount').text()) + 1);
	voteUserReview(1);
}

function downVoteReview() {
	bwcache.set("ReviewDetailPage_reviewVote_" + reviewId, { "vote": "0" });

	$('#downvoteBtn-' + reviewId).addClass('active');
	$('#upvoteBtn-' + reviewId).attr('disabled', 'disabled');
	$('#downvoteCount').text(parseInt($('#downvoteCount').text()) + 1);
	voteUserReview(0);
}

function voteUserReview(vote) {
	$.ajax({
		type: "POST",
		url: "/api/user-reviews/voteUserReview/?reviewId=" + reviewId + "&vote=" + vote,
		success: function (response) {
		}
	});
}

function addCss(fileName) {
	var link = $("<link />", {
		rel: "stylesheet",
		type: "text/css",
		href: fileName
	})
	$('head').append(link)
}



function applyLikeDislikes() {

	var locReviewId = reviewId;
    var listVote = bwcache.get("ReviewDetailPage_reviewVote_" + locReviewId);
    var dnVoteBtn = $('#downvoteBtn' + "-" + locReviewId), upVoteBtn = $('#upvoteBtn' + "-" + locReviewId);

	if (listVote != null && listVote.vote) {
		if (listVote.vote == "0") {
            dnVoteBtn.addClass('active');
            upVoteBtn.removeClass('active').attr('disabled', 'disabled');
		}
		else {
            upVoteBtn.addClass('active');
            dnVoteBtn.removeClass('active').attr('disabled', 'disabled');
		}
	}
	else {
        upVoteBtn.removeClass('active').prop('disabled', false);
        dnVoteBtn.removeClass('active').prop('disabled', false);
	}

}


docReady(function () {
	reviewPopupCotent = $('#reviewPopup');
	desktopUserReview = $('#userReviewContentDesktop').length;
	bwcache.setOptions({ 'EnableEncryption': true });

	bwcache.removeAll(true);
	reviewPopup = {
		settings: {
			effect: 'slide',
			direction: {
				direction: 'down'
			},
			duration: 500
		},
		open: function (element) {
			bwSpinner = reviewPopupCotent.find('#ub-ajax-loader');
			if (desktopUserReview) {
				element.animate({ 'right': 0 });        		
			}
			else {
				element.show(reviewPopup.settings.effect, reviewPopup.settings.direction, reviewPopup.settings.duration, function () { });
			}
			popup.lock();
			bwSpinner.show();
			window.history.pushState('addreviewPopup', '', '');
			$('.review-popup-container').scrollTop(0);
			setTimeout(function () { bwSpinner.hide(); }, 2000)
		},

		close: function (element) {
			if (desktopUserReview) {
				element.animate({ 'right': '-' + ($('#reviewPopup').width() + $('.review-popup-close-btn').outerWidth()) });
			}
			else {
				element.hide(reviewPopup.settings.effect, reviewPopup.settings.direction, reviewPopup.settings.duration, function () { });
			}
			popup.unlock();
		}
	};

	$(".review-popup__link").on('click', function (event) {
	  
		reviewId = $(event.target).data('reviewid');
		applyLikeDislikes();
		vmModelUserReviewDetailPopup.bindReviewSummaryData();
		reviewPopup.open(reviewPopupCotent);
	});

	$(document).on('click','.review-popup .review-popup-close-btn, .blackOut-window', function () {
		reviewPopup.close(reviewPopupCotent);
		window.history.back();
	});

	$(window).on('popstate', function (event) {
		if (reviewPopupCotent.is(':visible')) {
			reviewPopup.close(reviewPopupCotent);
		}
	});

	$(document).keyup(function (e) {
		if (e.keyCode == 27) {
			if (reviewPopupCotent.is(':visible')) {
				reviewPopup.close(reviewPopupCotent);
				window.history.back();
			}
		}
	});


  
	var modelUserReviewDetailPopup = function () {
		try {
			var self = this;
			self.IsInitialized = ko.observable(false);
			self.summaryObj = ko.observable();
			self.reviewId = ko.observable();

			self.init = function () {
				if (!self.IsInitialized()) {
					self.IsInitialized(true);
					if ($("#reviewPopup") != null)
					{
						addCss(reviewPopupCotent.attr('data-csslink'));
						ko.applyBindings(self, $("#reviewPopup")[0]);
					}
				}
			};


			self.bindReviewSummaryData = function () {
				$.ajax({
					type: "GET",
					url: "/api/v2/user-reviews/summary/" + reviewId + "/",
					success: function (response) {
						self.summaryObj(JSON.parse(response));
						self.reviewId(reviewId);
						applyLikeDislikes();
					}
				   
				});
			};
		} catch (e) {
			console.warn(e);
		}

	}

	vmModelUserReviewDetailPopup = new modelUserReviewDetailPopup();

	var reviewSummaryTemplate = bwcache.get("ReviewSummaryTemplate")
	if (reviewSummaryTemplate) {
        reviewPopupCotent.html(reviewSummaryTemplate);
		vmModelUserReviewDetailPopup.init();
	}
	else {
        reviewPopupCotent.load("/Templates/UserReviewDetails_Popup.html", function (responseTxt, statusTxt, xhr) {
			if (statusTxt == "success") {
				bwcache.set("ReviewSummaryTemplate", responseTxt, true);
				vmModelUserReviewDetailPopup.init();
			}
		});
	}

	

});