

$("#saveButton").click(function () {

    var makeId = $('#makeId').val();
    var userId = $('#userId').val();

    $('.txt-box').each(function (i, obj) {
        if (obj.value != obj.getAttribute('data-value')) {

            var footerData = {
                "makeId": makeId,
                "categoryId": obj.getAttribute('data-id'),
                "categoryDescription": obj.value,
                "userId": userId
            }

            $.ajax({
                type: "Post",
                data: ko.toJSON(footerData),
                dataType: 'json',
                url: "/make/save/footerdata/",
                contentType: "application/json",
                dataType: 'json',                                
            });
            
        }
    });

    Materialize.toast('Data saved!', 3000);

});
 

