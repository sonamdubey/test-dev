function SocialRequest(url) {
    var socialScr = $(document.createElement('script'));
    socialScr.attr('src', url);
    $("head:first").append(socialScr);
}

function fbCountCallback(response) {
    try {
        if (response != null) {
            var parser = new DOMParser();
            var xmlDoc = parser.parseFromString(response, "text/xml");

            var node = xmlDoc.getElementsByTagName('total_count')[0];
            if (node != null) {
                count = node.childNodes[0].nodeValue;
                if (count != null) {
                    $("#FbCount").html(count);
                }
            }
        }
    } catch (e) {
    }
}


function twitterCountCallback(response) {
    try {
        if (response != null && response['count'] != null) {
            $("#TweetCount").html(response['count']);
        }
    } catch (e) {
    }
}   