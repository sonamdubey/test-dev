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

    filter.location($(this), '#other-cities-list', '#city-no-result'); // (input field, list to filter, error message container)
});

var filter = {

    location: function (filterContent, filterList, noResultContent) {
        var inputText = $(filterContent).val(),
            inputTextLength = inputText.length,
            elementList = $(filterList + ' li'),
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

            var list = $(filterList),
                visibilityCount = 0;

            list.each(function () {
                var visibleElements = $(this).find('li[style*="display: block;"]').length;

                if (visibleElements != 0) {
                    visibilityCount++;
                }
            });

            if (visibilityCount == 0) {
                var errorMessage = $(filterList).attr('data-error-message');
                $(noResultContent).show().text(errorMessage);
            }
            else {
                $(noResultContent).hide();
            }
        }
        else {
            for (i = 0; i < len; i++) {
                element = elementList[i];
                element.style.display = "block";
            }
            $(noResultContent).hide();
        }
    }
}