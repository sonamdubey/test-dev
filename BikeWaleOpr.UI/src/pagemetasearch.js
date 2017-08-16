

function Show() {
    var status = $("input[name='group1']:checked").val();
    if (status)
        window.location.href = "/pageMetasconfigure/search/index/" + status + "/";
}