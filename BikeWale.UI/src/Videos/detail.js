$(function () {
    try{
        $("body").floatingSocialShare();
        $('.comma').each(function (i, obj) {
            var y = formatPrice($(this).html());
            if (y != null)
                $(this).html(y);
        });
    }catch(err){}
});