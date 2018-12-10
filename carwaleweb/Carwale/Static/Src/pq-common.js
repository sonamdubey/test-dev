//functions for both prices and quotaion pages starts
function makeChange() {
    selectedMakeId = $('#drpMake Option:selected').val();
    selectedMakeName = $('#drpMake Option:selected').text();
    bindModel(selectedMakeId);
    $("#drpModel").prop('disabled', false);
    if ($('#drpVersion option:selected').val() > 0) {
        $("#drpVersion").val(-1);
    }
}

function modelChange() {
    selectedModelId = $('#drpModel Option:selected').val();
    selectedModelName = $('#drpModel Option:selected').text();
    bindVersion(selectedModelId);
    bindCity(selectedModelId, function () {
        ModelCar.PQ.preselectPQDropDown('drpCity');
    });
    $("#drpVersion").prop('disabled', false);
    $("#drpCity").prop('disabled', false);
    $.cookie('_PQModelId', selectedModelId, { path: '/' });
}

function versionChange() {
    selectedVersionId = $('#drpVersion Option:selected').val();
    selectedVersionName = $('#drpVersion Option:selected').val();
    $.cookie('_PQVersionId', selectedVersionId, { path: '/' });
}

function bindMake() {
    $.ajax({
        type: 'GET',
        url: '/webapi/CarMakesData/GetCarMakes/?type=new',
        dataType: 'Json',
        success: function (json) {
            var viewModel = {
                pqCarMakes: ko.observableArray(json)
            };
            ko.cleanNode(document.getElementById("drpMake"));
            ko.applyBindings(viewModel, document.getElementById("drpMake"));

            bindCommonMakes();
            
            if (!isHashHaveModel) {
                $("#drpMake option[value=" + -1 + "]").attr('disabled', 'disabled');
                $("#drpMake option[value=" + -2 + "]").attr('disabled', 'disabled');
                $('#drpMake').val("-2");
            }
            else {
                $("#drpMake").prop('disabled', false);
                $('#drpMake').val(selectedMakeId);
            }
        }
    });
}

function bindModel(selectedMakeId) {
    $("#drpModel").prepend('<option value=-1>---Loading---</option>');
    $("#drpModel option[value=" + -1 + "]").attr('disabled', 'disabled');
    $('#drpModel').val("-1");
    $.ajax({
        type: 'GET',
        url: '/webapi/CarModelData/GetCarModelsByType/?type=new&makeId=' + selectedMakeId,
        dataType: 'Json',
        success: function (json) {
            var viewModel = {
                pqCarModels: ko.observableArray(json)
            };
            ko.cleanNode(document.getElementById("drpModel"));
            ko.applyBindings(viewModel, document.getElementById("drpModel"));

            if (!isHashHaveModel) {
                $("#drpModel").prepend('<option value=-1>---Select Model---</option>');
                $("#drpModel option[value=" + -1 + "]").attr('disabled', 'disabled');
                $('#drpModel').val("-1");
            }
            else {
                $("#drpModel").prop('disabled', false);
                $('#drpModel').val(selectedModelId);
            }
        }
    });
}

function bindVersion(selectedModelId) {
    $("#drpVersion").prepend('<option value=-1>---Loading---</option>');
    $("#drpVersion option[value=" + -1 + "]").attr('disabled', 'disabled');
    $('#drpVersion').val("-1");

    $.ajax({
        type: 'GET',
        url: '/webapi/CarVersionsData/GetCarVersions/?type=new&modelid=' + selectedModelId,
        dataType: 'Json',
        success: function (json) {

            var viewModel = {
                pqCarVersions: ko.observableArray(json)
            };
            ko.cleanNode(document.getElementById("drpVersion"));
            ko.applyBindings(viewModel, document.getElementById("drpVersion"));

            if (!isHashHaveModel) {
                $("#drpVersion").prepend('<option value=-1>---Select Version---</option>');
                $("#drpVersion option[value=" + -1 + "]").attr('disabled', 'disabled');
                $('#drpVersion').val("-1");
            } else {
                $("#drpVersion").prepend('<option value=-1>---Select Version---</option>');
                $('#drpVersion').val(selectedVersionId);
                $("#drpVersion").prop('disabled', false);
            }
        }
    });
}

function bindCommonMakes() {
    $("#drpMake").prepend('<option value=-1>------------</option>');
    $("#drpMake").prepend('<option value=7>Honda</option>');
    $("#drpMake").prepend('<option value=15>Skoda</option>');
    $("#drpMake").prepend('<option value=20>Volkswagen</option>');
    $("#drpMake").prepend('<option value=17>Toyota</option>');
    $("#drpMake").prepend('<option value=5>Ford</option>');
    $("#drpMake").prepend('<option value=2>Chevrolet</option>');
    $("#drpMake").prepend('<option value=9>Mahindra</option>');
    $("#drpMake").prepend('<option value=16>Tata</option>');
    $("#drpMake").prepend('<option value=8>Hyundai</option>');
    $("#drpMake").prepend('<option value=10>Maruti Suzuki</option>');
    $("#drpMake").prepend('<option value=-2>---Select Make---</option>');
}

function IsValid() {
    var retVal = true;
    var errorMsg = "";
    
    if ($('#drpMake option:selected').val() <= 0) {
        retVal = false;
        errorMsg = "Please select make";
        $('#spnCity').text(errorMsg);
        $('#addCarError').text(errorMsg);
    }
    else if ($('#drpModel option:selected').val() <= 0) {
        retVal = false;
        errorMsg += "Please select model";
        $('#spnCity').text(errorMsg);
        $('#addCarError').text(errorMsg);
    }
    else if ($('#drpVersion option:selected').val() <= 0) {
        retVal = false;
        errorMsg += "Please select version";
        $('#spnCity').text(errorMsg);
        $('#addCarError').text(errorMsg);
    }
    else if ($('#drpCity option:selected').val() <= 0) {
        retVal = false;
        errorMsg += "Please select city";
        $('#spnCity').text(errorMsg);
        $('#addCarError').text(errorMsg);
    }
    return retVal;
}
//functions for both prices and quotaion pages ends