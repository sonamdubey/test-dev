      function MakeChanged() {
          $("#ddlModels").html("<option value='0'>--Select--</option>");
          var _makeId = $("#ddlMakes").val();
          if (_makeId > 0) {
              $("#imgLoaderModel").show();
              $.ajax({
                  type: "GET",
                  url: "/webapi/carmodeldata/GetCarModelsByType/?type=All&makeId=" + _makeId,
                  beforeSend: function (xhr) {
                      $("#ddlModels").attr('disabled', true);
                      $("#ddlModels").empty();
                  },
                  success: function (response) {
                      $("#imgLoaderModel").hide();
                      if (response)
                      {
                          bindModels(response, $("#ddlModels"), "", "--Select Model--");
                      }
                  }
              });
          }
      }

function bindModels(response, cmbToFill, viewStateId, selectString) {
    if (response != null) {
        if (!selectString || selectString == '') selectString = "--Select--";
        $(cmbToFill).empty().append("<option value=\"0\" title='" + selectString + "'>" + selectString + "</option>").removeAttr("disabled");
        var hdnValues = "";
        for (var i = 0; i < response.length; i++) {
            $(cmbToFill).append("<option value=" + response[i].ModelId + " MaskingName='" + response[i].MaskingName + "' title='" + response[i].ModelName + "'>" + response[i].ModelName + "</option>");
            if (hdnValues == "")
                hdnValues += response[i].ModelName + "|" + response[i].ModelId;
            else
                hdnValues += "|" + response[i].ModelName + "|" + response[i].ModelId;
        }
        if (viewStateId) $("#" + viewStateId).val(hdnValues);
    }
}

$(document).ready(function () {
    if ($.cookie('ReviewRepeated')) {
        $.cookie('ReviewRepeated', null, { expires: new Date(-8640000000000000), path: '/' });
        alert('You have already submitted review for this car');
    }
    SetControlWidth();
});

function IsValid() {
    var retVal = true;
    $("#spnMake").html("");
    $("#spnModel").html("");

    if ($("#ddlMakes").val() <= 0) {
        retVal = false;
        $("#spnMake").html("(Required)");
    }

    if ($("#ddlModels").val() <= 0) {
        retVal = false;
        $("#spnModel").html("(Required)");
    }
    return retVal;
}

function redirectToUserReviews() {
    var makeName = $('#ddlMakes option:selected').data('make-name');
    var maskingName = $('#ddlModels option:selected').attr('maskingname');
    if(IsValid())
        window.location.href = "/m/" + makeName + "-cars/" + maskingName + "/userreviews/";
}
