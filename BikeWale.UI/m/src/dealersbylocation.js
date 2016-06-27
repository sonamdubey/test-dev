$("#getStateInput, #getCityInput").on("keyup", function (event) {
    if (event.keyCode != 13) {
        filter.location($(this));
    }
    else {
        if ($(this).val().length > 0) {
            filter.targetSelection();
        }
    }
});

$("#getStateInput, #getCityInput").on("focus", function (event) {
    var inputbox = $("#listingHeader .form-control-box").offset();
    $("html, body").animate({ scrollTop: inputbox.top - 60 });
});

$("#listingUL").on("click", "li", function () {
    var targetLink = $(this).find("a"),
        targetText = targetLink.text(),
        inputbox = $(this).parents("ul").prev("#listingHeader").find(".form-control");
    inputbox.val(targetText);
});

var filter = {

    list: $("#listingUL"),

    location: function (filterContent) {
        var inputText = $(filterContent).val();
        inputText = inputText.toLowerCase();
        var inputTextLength = inputText.length;
        if (inputText != "") {
            $(filterContent).parents("#listingHeader").siblings("ul").find("li").each(function () {
                var locationName = $(this).text().toLowerCase().trim();
                if (/\s/.test(locationName))
                    var splitlocationName = locationName.split(" ")[1];
                else
                    splitlocationName = "";

                if ((inputText == locationName.substring(0, inputTextLength)) || inputText == splitlocationName.substring(0, inputTextLength))
                    $(this).show().addClass("filtered");
                else
                    $(this).hide().removeClass("filtered highlight");
            });
            var filteredList = filter.list.find("li.filtered");
            filteredList.removeClass("highlight");
            filteredList.first().addClass("highlight");
        }
        else {
            $(filterContent).parents("#listingHeader").siblings("ul").find("li").each(function () {
                $(this).show().removeClass("filtered highlight");
            });
        }
    },

    targetSelection: function () {
        var currentSelection = filter.list.find("li.filtered.highlight"),
            targetLink = currentSelection.find("a").attr("href");
        if (typeof (targetLink) != 'undefined') {
            window.location.href = targetLink;
        }
    }

}