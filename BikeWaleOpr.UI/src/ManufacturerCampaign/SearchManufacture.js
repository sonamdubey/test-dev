var ddlManufacturers;
var ddlAddManufacturers;
var BwOprHostUrl, msg,userId;
 $(document).ready(function () {
     var tblCampaigns = document.getElementById("tblCampaigns");
     BwOprHostUrl = tblCampaigns.getAttribute("data-BwOprHostUrl");
     userId = tblCampaigns.getAttribute("data-userid")
     $('#tblCampaigns').hide();
     ddlManufacturers = $("#ddlManufacturers");
     ddlAddManufacturers = $("#ddlAddManufacturers");
     
 });
 if (msg != "") { Materialize.toast(msg, 5000); }
var mfgCamp = function () {
    var self = this;
    self.selectedManufacturer = ko.observable();
    self.Manufacturers = ko.observableArray([]);
    self.ManufacturersCampaign = ko.observableArray([]);

    self.redirect = function () {
        dealerId = ddlAddManufacturers.val();
        if (!isNaN(dealerId) && dealerId > 0) {
            var url = '/manufacturercampaign/information/' + dealerId;
            window.location.href = url;
        }
        else {
            Materialize.toast('Please select manufacturer', 5000);
        }
    };
    self.searchCampaigns = function () {
        dealerId = ddlManufacturers.val();
        manufactureName = ddlManufacturers.find("option:selected").text();
        var allActiveCampaign=0;
        if ($('#chkActiveStatus').is(':checked'))
            allActiveCampaign = 2;
        if (!isNaN(dealerId) && dealerId > 0) {
            var element = document.getElementById('DealerCampaignsList');
            $.ajax({
                type: "GET",
                url: "/api/v2/campaigns/manufacturer/search/dealerId/" + dealerId + "/allActiveCampaign/" + allActiveCampaign,
                datatype: "json",
                success: function (response) {
                    if (response.length > 0) {
                        self.ManufacturersCampaign(response);
                        $('#tblCampaigns').removeClass();
                        $('#tblCampaigns').show();
                        $('.tooltipped').tooltip({ delay: 50 });
                    }
                    else {
                        $('#tblCampaigns').hide();
                        Materialize.toast('Campaigns for ' + manufactureName + ' are not available', 5000);
                    }
                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        Materialize.toast('Something went wrong .Please try again !!', 5000);
                    }
                }
            });
        } else {
            Materialize.toast('Please select manufacturer', 5000);
            
        }

    };

    self.changeStatus = function (data, event) {
        if (confirm("Are you Sure you want to " + $(event.currentTarget).data("tooltip"))) {
            $.ajax({
                type: "POST",
                url: "/api/v2/campaigns/manufacturer/updatecampaignstatus/campaignId/" + data.id + '/status/' + $(event.currentTarget).data("status") + '/',
                datatype: "json",
                success: function (response) {
                    self.searchCampaigns();                    
                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        Materialize.toast('Something went wrong .Please try again !!', 5000);
                    }
                    else {
                        Materialize.toast('Campaign for ' + manufactureName + ' has been Updated', 5000);
                    }
                }
            });
        }

    };

    self.resetTotalLeadsDelivered = function (d, e) {
        if (confirm("Are you Sure you want to " + $(e.currentTarget).data("tooltip"))) {
            $.ajax({
                type: "POST",
                url: "/api/campaigns/manufacturer/" + d.id + "/totalleads/reset/?userId=" + userId,
                datatype: "json",
                success: function (response) {
                    self.searchCampaigns();
                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        Materialize.toast('Something went wrong .Please try again !!', 5000);
                    }
                    else {
                        Materialize.toast('Campaign Total Leads Delivered reset for ' + manufactureName + ' has been Updated', 5000);
                    }
                }
            });
        }
    };
}
var viewModel = new mfgCamp();
ko.applyBindings(viewModel, $("#mfgCampaigns")[0]);