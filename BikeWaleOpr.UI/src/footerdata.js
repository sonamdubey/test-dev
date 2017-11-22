

$("#saveButton").click(function () {

    var makeId = $('#makeId').val();
    var userId = $('#userId').val();

    $('.txt-box').each(function (i, obj) {
        if (obj.value && obj.value != obj.getAttribute('data-value')) {

            $.ajax({
                type: "Post",
                url: "/make/save/footerdata/?makeId=" + makeId + "&categoryId=" + obj.getAttribute('data-id') + "&categoryDescription=" + Base64.encode(obj.value) + "&userId=" + userId,
                contentType: "application/json",
                dataType: 'json',                                
            });
            
        }
    });

    Materialize.toast('Data saved!', 3000);

});
 

