$(document).ready(function () {
    $('#cityInput').fastLiveFilter('#other-cities-list');
});

$('#cityInput').on('focus', function () {
    $('html, body').animate({
        scrollTop: $(this).offset().top - 20
    });
});

$('#cityInput').on('keyup', function () {
    var inputBox = $(this);

    if (inputBox.val().length > 0) {
        $('body').addClass('filter-active');
    }
    else {
        $('body').removeClass('filter-active');
    }
});

/* cw_fastFilter for popup search box code starts here */
jQuery.fn.fastLiveFilter = function (list, options) {
    // Options: input, list, timeout, callback
    options = options || {};
    list = jQuery(list);
    var input = this;
    var lastFilter = '', noResultLen = 0;
    var noResult = '<div class="noResult">No search found!</div>';
    var timeout = options.timeout || 100;
    var callback = options.callback || function (total) {
        noResultLen = list.siblings(".noResult").length;
        
        if (total == 0 && noResultLen < 1) {
            list.after(noResult).show();
        }
        else if (total > 0 && noResultLen > 0) {
            $('.noResult').remove();
        }
    };

    var keyTimeout;
    var lis = list.children();
    var len = lis.length;
    var oldDisplay = len > 0 ? lis[0].style.display : "block";
    callback(len); // do a one-time callback on initialization to make sure everything's in sync

    input.change(function () {
        // var startTime = new Date().getTime();
        var filter = input.val().toLowerCase();
        var li, innerText;
        var numShown = 0;
        for (var i = 0; i < len; i++) {
            li = lis[i];
            innerText = !options.selector ?
                (li.textContent || li.innerText || "") :
                $(li).find(options.selector).text();

            if (innerText.toLowerCase().indexOf(filter) >= 0) {
                if (li.style.display == "none") {
                    li.style.display = oldDisplay;
                }
                numShown++;
            } else {
                if (li.style.display != "none") {
                    li.style.display = "none";
                }
            }
        }
        callback(numShown);
        return false;
    }).keydown(function () {
        clearTimeout(keyTimeout);
        keyTimeout = setTimeout(function () {
            if (input.val() === lastFilter) return;
            lastFilter = input.val();
            input.change();
        }, timeout);
    });
    return this; // maintain jQuery chainability
}