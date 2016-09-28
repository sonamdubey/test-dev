var effect = 'slide',
    options = { direction: 'right' },
    duration = 500,
    citySearchInput = $('#getCityInput');

$(document).ready(function () {
    $('#getCityInput').fastLiveFilter('#city-slider-list');
});

/* city slider */
$('#search-form-city').on('click', function () {
    citySlider.open(citySearchInput);
    appendState('citySelection');
});

$('#close-city-slider').on('click', function () {
    citySlider.close();
});

$('#city-slider-list').on('click', 'li', function () {
    citySlider.selection($(this));
    citySlider.close();
});

var citySlider = {
    container: $('#city-slider'),

    open: function (inputBox) {
        citySlider.container.show(effect, options, duration, function () {
            $('html, body').addClass('lock-browser-scroll');
            citySlider.container.addClass('input-fixed');
            inputBox.focus();
        });
    },

    close: function () {
        citySlider.container.removeClass('input-fixed');
        $('html, body').removeClass('lock-browser-scroll');
        citySlider.container.hide(effect, options, duration);
    },

    selection: function (element) {
        var elementText = element.text();
        $('#search-form-city p').addClass('text-default').text(elementText);
    }
};

/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');
};

$(window).on('popstate', function (event) {
    if ($('#city-slider').is(':visible')) {
        citySlider.close();
    }
});

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