var BwOprHostUrl;
var ddlServiceCenterCity = $('#ddlServiceCenterCity');
$(document).ready(function () {
    BwOprHostUrl = document.getElementById("tblServiceCenter").getAttribute("data-BwOprHostUrl");
    $('#tblServiceCenter').hide();
    $(".chosen-select").chosen({
        "width": "150px"
    });
});

var serviceCenter = function ()
{
    var self = this;
    self.Id = ko.observable('');
    self.selectedCity = ko.observable('');
    self.cityList = ko.observableArray([]);
    self.allServiceCenters = ko.observableArray([]);
    self.getCityList = function () {
        var makeId = $("#ddlServiceCenter").val();
        if (makeId > 0) {
            $.ajax({
                type: "GET",
                url: BwOprHostUrl + "/api/servicecenter/cities/make/" + parseInt(makeId) +"/",
                success: function (data) {
                    if (data) {
                        self.cityList(data);
                    }

                },
                complete: function (xhr) {
                   
                    $("#ddlServiceCenterCity .chosen-single span").text("Select City");
                    ddlServiceCenterCity.trigger('chosen:updated'); 
                },
                error: function (e) {
                    Materialize.toast('error occured', 4000);
                }
            });
        }
        else {
            self.cityList([]);
            Materialize.toast('Please select valid make', 4000);
        }
    };

    
    self.activeStatus = ko.observable(true);
    self.searchServiceCenters = function () {
        var makeId = $("#ddlServiceCenter").val();
        var cityId = $("#ddlServiceCenterCity :selected").val();
        var activeStatus = 0;
      
        if ($('#chkActiveStatus').is(':checked'))
        {
            activeStatus = 1;
            self.activeStatus(true);
        }
        else
        {
            activeStatus = 0;
            self.activeStatus(false);
        }

        if ((makeId > 0) && (cityId > 0)) {

            $.ajax({
                type: "GET",

                url: BwOprHostUrl + "/api/servicecenter/make/" + +parseInt(makeId) + "/city/" + parseInt(cityId) + "/active/" + parseInt(activeStatus) + "/",
                success: function (data) {

                    if (data.length > 0) {
                        self.allServiceCenters(data);
                        $('#tblServiceCenter').removeClass();
                        $('#tblServiceCenter').show();

                    }
                    else
                    {
                        $('#tblServiceCenter').removeClass();
                        $('#tblServiceCenter').hide();
                        Materialize.toast('No data to display', 5000);
                    }
                    
                },

                error: function (e) {
                    $('#tblServiceCenter').removeClass();
                    $('#tblServiceCenter').hide();
                    Materialize.toast('error occured', 4000);
                }

            });

        }
        else {
            Materialize.toast('Please choose Make and City', 4000);
        }

    };


    self.UpdateServiceCenterStatus = function (d, e) {
        var currentUserId = $('#serviceCenter').attr("data-currentuser");
        var Id = d.Id;
        if (Id > 0) {
            $.ajax({
                type: "GET",

                url: BwOprHostUrl + "/api/updatestatus/currentuser/" + parseInt(currentUserId) +"/servicecenter/"+ parseInt(Id) + "/",
                success: function () {
                    e.currentTarget.closest('tr').remove();
                    Materialize.toast('Service Center Status has been updated', 4000);
                },

                error: function (e) {
                    Materialize.toast('Error occured', 4000);
                }

            });
        }

    }



    self.redirect = function () {
        makeId = $("#ddlAddServiceCenter").val();
        makeName = $("#ddlAddServiceCenter option:selected").text();
        cityId = $("#ddlSelectCity").val();
        cityName = $("#ddlSelectCity option:selected").text();
        if ((makeId) > 0 && cityId > 0) {
            var url = BwOprHostUrl + '/servicecenter/details/make/' + makeId + '/' + makeName + '/city/' + cityId + "/" + cityName + '/';
         
            window.open(url);
        }
        else if ((makeId) == 0 && cityId == 0) {
            Materialize.toast('Please select city and Make', 5000);  
        }
        else if(makeId > 0)
        {
            Materialize.toast('Please select City', 5000);
        }
        else
        {
            Materialize.toast('Please select Make   ', 5000);
        }
    };

   
}

var viewModel = new serviceCenter();
ko.applyBindings(viewModel, $("#serviceCenter")[0]);

