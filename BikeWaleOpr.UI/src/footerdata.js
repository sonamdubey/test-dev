
var makeId = $('#makeId').val();
var userId = $('#userId').val();


$("#saveButton").click(function () {

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
                contentType: "application/json"                                            
            });
            
        }
    });

    Materialize.toast('Data saved!', 3000);

});
 


$("#disableButton").click(function () {    

    if(confirm("Do you want delete all data for this make ?"))
    {
        $.ajax({
            type: "Post",
            url: "/make/delete/footerdata/?makeId=" + makeId + "&userId=" + userId,
            contentType: "application/json"
        });

        Materialize.toast('Data deleted!', 3000);

        location.reload();
    }

});


