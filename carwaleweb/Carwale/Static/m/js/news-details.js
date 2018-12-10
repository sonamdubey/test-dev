$(document).ready(function () {
    $("#divDesc img").addClass("imgWidth").addClass("margin-bottom10");
    SocialRequest('https://api.facebook.com/restserver.php?method=links.getStats&urls=https://carwale.com' + articleUrl + '&callback=fbCountCallback');
    SocialRequest('https://urls.api.twitter.com/1/urls/count.json?url=https://www.carwale.com' + articleUrl + '&callback=twitterCountCallback');
    ContentTracking.tracking.setUpTracking(1, 'News', '.content-details');
});