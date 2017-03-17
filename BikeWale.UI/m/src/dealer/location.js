$(document).ready(function () {
    // filter no-result message
    $('#no-result').text('No result found!');
});

/* state links */
$('#location-list').on('click', '.type-state', function (event) {
    var item = $(this);

    event.preventDefault();

    if (!item.hasClass('active')) {
        $('.location-list-city').hide();
        item.addClass('active').siblings('.location-list-city').show();
        $('html, body').animate({ scrollTop: item.offset().top });
    }
    else {
        item.removeClass('active').siblings('.location-list-city').hide();
    }
});

/* filter */
$("#getCityInput").on("focus", function (event) {
    var inputbox = $(this).offset();
    $("html, body").animate({ scrollTop: inputbox.top - 20 });
});

$("#getCityInput").on("keyup", function (event) {
    $('.location-list-city').show();
    filter.location($(this));
});

var filter = {

    location: function (filterContent) {
        var inputText = $(filterContent).val(),
            inputTextLength = inputText.length,
            elementList = $('.location-list-city li'),
            len = elementList.length,
            element, i;

        inputText = inputText.toLowerCase();

        if (inputText != "") {
            for (i = 0; i < len; i++) {
                element = elementList[i];

                var locationName = $(element).text().toLowerCase().trim();
                if (/\s/.test(locationName))
                    var splitlocationName = locationName.split(" ")[1];
                else
                    splitlocationName = "";

                if ((inputText == locationName.substring(0, inputTextLength)) || inputText == splitlocationName.substring(0, inputTextLength)) {
                    element.style.display = "block";
                }
                else {
                    element.style.display = "none";
                }
            }

            var cityList = $('.location-list-city'),
                visibleState = 0;

            cityList.each(function () {
                var visibleElements = $(this).find('li[style*="display: block;"]').length;

                if (visibleElements == 0) {
                    $(this).closest('li').hide();
                }
                else {
                    $(this).closest('li').show();
                    visibleState++;
                }
            });

            if (visibleState == 0) {
                $('#no-result').show();
            }
            else {
                $('#no-result').hide();
            }
        }
        else {
            for (i = 0; i < len; i++) {
                element = elementList[i];
                element.style.display = "block";
            }
            $('.item-state').show();
            $('#no-result, .location-list-city').hide();
        }
    }
}
