<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.controls.LocateDealer_New" %>
<div class="locate-dealer-container text-center margin-bottom50">
    <div class="margin-bottom40">
        <span class="bw-circle-icon locate-dealer-logo"></span>
    </div>
    <p class="font16 margin-bottom30">Find a car dealer near your current location</p>
    <div class="locate-dealer-search-container">
        <div class="locate-dealer-search">
            <div class="locate-dealer-bikeSelect">
                <div class="form-control-box">
                    <select id="cmbMake" class="form-control rounded-corner0" data-bind="options: Makes, optionsText: 'text', optionsValue: 'value', value: SelectedMake, optionsCaption: 'Select Make', event: { change: UpdateCity }"></select>
                </div>
            </div>
            <div class="locate-dealer-citySelect">
                <div class="form-control-box">
                    <select id="cmbCity" class="form-control rounded-corner0" data-bind="options: Cities, optionsText: 'cityName', optionsValue: 'cityMaskingName', value: SelectedCity, optionsCaption: 'Select City', enable: SelectedMake"></select>
                </div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="locate-dealer-btn">
            <button id="btnLocateDealer" class="font18 btn btn-orange btn-lg rounded-corner-no-left" data-bind="event: { click: btnLocateDealer_click }">Locate dealer</button>
        </div>

        <div class="clear"></div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        var viewModel = new ViewModelLD();
        ko.applyBindings(viewModel);
    });
    function ViewModelLD() {
        var self = this;
        self.Makes = ko.observableArray([]);
        self.Cities = ko.observableArray([]);
        self.SelectedMake = ko.observable();
        self.SelectedCity = ko.observable();
        self.cityId = ko.observable();
        self.UpdateCity = function () { FillCity(self); }
        self.btnLocateDealer_click = function () { handleLocateDealer(self); }
        $.getJSON("/api/DealerMakes/", self.Makes);
    }

    function FillCity(vm) {
        if (vm.SelectedMake()) {
            $.getJSON("/api/DealerCity/?makeId=" + vm.SelectedMake().split('_')[0], vm.Cities);
        }
    }

    function handleLocateDealer(vm) {
        if (vm.SelectedMake() && vm.SelectedCity()) {
            location.href = "/new/" + vm.SelectedMake().split('_')[1] + "-dealers/" + vm.SelectedCity().split('_')[0] + "-" + vm.SelectedCity().split('_')[1] + ".html";
        }
        else {
            alert();
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
