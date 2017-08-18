

function Show() {
    var status = $("input[name='group1']:checked").val();
    if (status)
        window.location.href = "/pageMetas/?pageMetaStatus=" + status;
}

$('.update-status').on('click', function () {
    var ele = $(this);
    var status = ele.data('setstatus');
    var id = ele.data('pagemetaid');
    var apiPath = `/api/pagemetas/update/${id}/${status}`;
    $.ajax({
        type: "POST",
        url: apiPath,
        success: function () {
            ele.closest('tr').hide();
            Materialize.toast('Page meta data has been updated', 4000);
        },
        error: function (e) {
            Materialize.toast('Error occured', 4000);
        }

    });
});