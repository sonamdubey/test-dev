var pendingList = new Array();
var refreshTime = 2000;
$(document).ready(function () {
    GetPendingList();
    setInterval(UpdatePendingImages, refreshTime);
});
/*
for binding images 
*/
function CheckImageStatus(imageList) {
    $.ajax({
        type: "POST", url: "/ajaxpro/CarwaleAjax.AjaxCommon,Carwale.ashx",
        data: '{"imageList":"' + imageList + '","imgCategory":"' + imgCategory + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "FetchProcessedImagesList"); },
        success: function (response) {
            var ret_response = eval('(' + response + ')').value;
            if (ret_response != "") {
                var obj_response = eval('(' + ret_response + ')');
                for (var items in obj_response) {
                    imageList = imageList.replace(obj_response[items].Id + ',', '');
                    var imgUrlA = obj_response[items].HostUrl + "110x61" + obj_response[items].AvtOriginalImgPath;
                    var imgUrlR = obj_response[items].HostUrl + "110x61" + obj_response[items].RealOriginalImgPath;
                    if (pendingList.indexOf(obj_response[items].Id) > -1)
                        pendingList.splice(pendingList.indexOf(obj_response[items].Id), 1);
                    $('.pending').removeClass('show').addClass('hide');
                    $('.pending').removeClass('show').attr("pending", "false");
                    $('#dtlstPhotosA_' + obj_response[items].Id).removeClass('hide').addClass('show');
                    $('#dtlstPhotosA_' + obj_response[items].Id).find('img').attr('src', imgUrlA);
                    $('#dtlstPhotosR_' + obj_response[items].Id).removeClass('hide').addClass('show');
                    $('#dtlstPhotosR_' + obj_response[items].Id).find('img').attr('src', imgUrlR);
                }
            }
            else
            {
                if (pendingList.length > 0) { pendingList.pop(); }
            }
        }
    })
}

function GetPendingList() {
    $(".pending").each(function () {
        if ($(this).attr("pending") == "true") {
            pendingList.push(this.id.replace("dtlstPhotosPending_", ""));
        }
    });
}
function UpdatePendingImages() {
    var list = "";
    if (pendingList.length > 0) {
        for (var i = 0; i < pendingList.length; i++) {
            list += pendingList[i] + ",";
        }
        CheckImageStatus(list);
    }
}