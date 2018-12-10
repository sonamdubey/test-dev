$("#ddlCategory").val(subCategoryId);

$("#ddlCategory").change(function () {

    if ($("#ddlCategory option:selected").index() == 0)
        location.href = "/m/tipsadvice/";

    else if ($("#ddlCategory option:selected").index() > 0) {
        var cat = $("#ddlCategory option:selected").text().toLowerCase().replace(/\s+/g, '-').trim();
        cat = cat.toString().replace("&", "and");
        location.href = "/m/tipsadvice/" + cat + "/";
    }

});