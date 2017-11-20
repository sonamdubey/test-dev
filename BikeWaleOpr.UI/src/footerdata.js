

$("#saveButton").click(function () {
    var ans = "";
    $('.txt-box').each(function (i, obj) {
        if(obj.value)
        {
            ans = ans + obj.getAttribute('data-id') + ':' + obj.value + ',';
        }
    });
    $('#input-footerdata').val(ans);  
});
 

