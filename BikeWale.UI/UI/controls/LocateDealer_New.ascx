<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.LocateDealer_New" %>
<div class="locate-dealer-container text-center" id="NBLocateDealer">
    <div class="margin-bottom40">
        <span class="bw-circle-icon locate-dealer-logo"></span>
    </div>
    <p class="font16 margin-bottom30">Find a bike dealer near your current location</p>
    <div class="locate-dealer-search-container">
        <div class="locate-dealer-search">
            <div class="locate-dealer-bikeSelect">
                <div class="form-control-box">
                    <select id="cmbMake" class="form-control rounded-corner0 no-border" data-bind="options: Makes, optionsText: 'makeName', optionsValue: 'makeId', value: SelectedMake, optionsCaption: 'Select Make', event: { change: UpdateCity }"></select>
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please select a bike</div>
                </div>
            </div>
            <div class="locate-dealer-citySelect">
                <div class="form-control-box">
                    <select id="cmbCity" class="form-control rounded-corner0" data-bind="options: Cities, optionsText: 'cityName', optionsValue: 'cityMaskingName', value: SelectedCity, optionsCaption: 'Select City', enable: SelectedMake"></select>
                    <span class="bwsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please select a city</div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="locate-dealer-btn">
            <button id="btnLocateDealer" class="font16 btn btn-orange btn-lg rounded-corner-no-left" data-bind="event: { click: btnLocateDealer_click }">Locate dealer</button>
        </div>

        <div class="clear"></div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        var viewModel = new ViewModelLD();
        ko.applyBindings(viewModel, $('#NBLocateDealer')[0]);
        viewModel.SelectedMake(0);
        viewModel.SelectedCity(0);        
    });
    function ViewModelLD() {
        var self = this;
        self.Makes = ko.observableArray([]);
        self.Cities = ko.observableArray([]);
        self.SelectedMake = ko.observable();
        self.SelectedCity = ko.observable();
        self.cityId = ko.observable();
        self.UpdateCity = function () { FillCity(self); };
        self.btnLocateDealer_click = function () { handleLocateDealer(self); };
        //$.getJSON("/api/DealerMakes/", self.Makes);
        $.ajax({
            type: "GET",
            url: "/api/DealerMakes/",
            dataType: 'json',
            success: function (response) {                
                var makes = response.makes;                
                if (makes) {                    
                    self.Makes(ko.toJS(makes));
                }
            }
        });
    }

    function findMakeById(vm, id) {
        return ko.utils.arrayFirst(vm.Makes(), function (child) {
            return child.makeId === id;
        });
    }

    function FillCity(vm) {
        if (vm.SelectedMake()) {
            $.getJSON("/api/DealerCity/?makeId=" + vm.SelectedMake(), vm.Cities);
        }
        else {
            vm.Cities([]);
        }
    }

    function handleLocateDealer(vm) {
        if (vm.SelectedMake() && vm.SelectedCity()) {
            toggleErrorMsg($('#cmbMake'), false);
            toggleErrorMsg($('#cmbCity'), false);
            location.href = "/dealer-showrooms/" + findMakeById(vm, vm.SelectedMake()).maskingName + "/"+ vm.SelectedCity().split('_')[1] + "/";
        }
        else {
            if ($('#cmbMake').val() == undefined || $('#cmbMake').val() == 0)
                toggleErrorMsg($('#cmbMake'), true, "Please select a bike");
            else
                toggleErrorMsg($('#cmbMake'), false);
            if ($('#cmbCity').val() == undefined || $('#cmbCity').val() == 0)
                toggleErrorMsg($('#cmbCity'), true, "Please select a city");
            else
                toggleErrorMsg($('#cmbCity'), false);
        }
    }

    function replaceAll(str, rep, repWith) {
        var occurrence = str.indexOf(rep);

        while (occurrence != -1) {
            str = str.replace(rep, repWith);
            occurrence = str.indexOf(rep);
        }
        return str;
    }
</script>
