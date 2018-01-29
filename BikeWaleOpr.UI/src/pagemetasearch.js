

function Show() {
    var status = $("input[name='group1']:checked").val();
    if (status)
        window.location.href = "/pageMetas/?pageMetaStatus=" + status;
}

$('.update-status').on('click', function () {
    var ele = $(this);
    var status = ele.data('setstatus');
    var id = ele.data('pagemetaid');
    var modelid = ele.data('modelid');
    var makeid = ele.data('makeid');
    var apiPath = `/api/pagemetas/update/${id}/${status}/${modelid}/${makeid}/`;
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


$('#chkApprove').change(function () {
    if (this.checked)
    {
        $('input:checkbox').each(function () {
            $(this).prop('checked', true);
        });
    }
    else
    {
        $('input:checkbox').each(function () {
            $(this).prop('checked', false);
        });
    }    
});