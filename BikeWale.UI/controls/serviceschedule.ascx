<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ServiceSchedule" %>
<% if(BikeScheduleList!= null) { %>
<div id="service-scheduler" class="container section-bottom-margin">
    <div class="grid-12">
        <div class="content-box-shadow padding-15-20-20">
            <h2 class="section-h2-title margin-bottom10">Is your <%=MakeName %> bike due for a service?</h2>
            <p class="font14 margin-bottom25">Get you <%=MakeName %> bike serviced within given time period or km range, whichever condition gets satisfied earlier.</p>
         <div id="scheduler-left-column" class="grid-4 alpha">
                <div class="select-box">
                    <p class="font12 text-xt-light-grey">Model</p>
                    <select class="chosen-select" data-bind="event: { change: GetModelId }" data-placeholder="Select model">
                        <% foreach (var bike in BikeScheduleList)
                           { %>
                        <option value="<%= bike.ModelId %>"><%=bike.ModelName %></option>
                        <% } %>
                    </select>
                </div>
                <img data-bind="attr: { src: imagePath() }" id="service-model-image" src="" />
            </div>
            <div class="grid-8 omega">
                <table data-bind="visible: isDataExist()" width="100%" border="0" cellspacing="0" cellpadding="0">
                    <thead>
                        <tr>
                            <th align="left" width="20%">Service no.</th>
                            <th data-bind="visible: isKms()" align="left" width="40%">Validity from the date of purchase</th>
                            <th data-bind="visible: isDays()" align="left" width="40%">Validity from the date of previous service</th>
                        </tr>
                    </thead>
                    <tbody data-bind="foreach: bikesList">
                        <!-- ko foreach : Schedules -->
                        <tr>
                            <td data-bind="text: ServiceNo"></td>
                            <td data-bind="visible: $root.isKms(), text: Kms + ' Kms'"></td>
                            <td data-bind="visible: $root.isDays(), text: Days + ' days'"></td>
                        </tr>
                        <!-- /ko -->
                    </tbody>
                </table>
                <!-- no service -->
                <div data-bind="visible: !isDataExist()" id="service-not-available">
                    <span class="service-sprite calender-lg"></span>
                    <p class="font14 text-light-grey">Sorry! The service schedule for <span data-bind="text: currentBikeName()"></span>is not available.<br />
                        Please check out the service schedule for other <%= MakeName %> bikes.</p>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </div>
	<div class="clear"></div>
</div>
<% } %>
<script type="text/javascript">
    <% if (!string.IsNullOrEmpty(jsonBikeSchedule))
       { %>
    var bikeschedule = '<%=jsonBikeSchedule.Replace("'", string.Empty) %>';
    bikeschedule = JSON.parse(bikeschedule.replace(/\s/g, ' '));
    function SchedulesViewModel() {
        var self = this;
        self.bikes = ko.observable(bikeschedule);
        self.currentBikeName = ko.observable();
        self.bikesList = ko.observable(self.bikes());
        self.isDataExist = ko.observable();
        self.imagePath = ko.observable();
        self.isDays = ko.observable();
        self.isKms = ko.observable();
        self.selectedModelId = ko.observable();
        self.GetModelId = function (data, event) {
            self.selectedModelId($(event.target).val());
            var bikename = ko.utils.arrayFirst(self.bikes(), function (bike) {
                return bike.ModelId == self.selectedModelId();
            });
            if (bikename != null) {
                self.currentBikeName(bikename.ModelName);
            }
        };
        function isDaysDataExists(sch) {
            var isFound = false;
            ko.utils.arrayForEach(sch, function (s) {
                if (s.Days && s.Days > 0) {
                    isFound = true;
                }
            });
            return isFound;
        }
        function isKmsDataExists(sch) {
            var isFound = false;
            ko.utils.arrayForEach(sch, function (s) {
                if (s.Kms && s.Kms.length > 0) {
                    isFound = true;
                }
            });
            return isFound;
        }
        self.selectedModelId.subscribe(function () {
            var selbike = ko.utils.arrayFirst(self.bikes(), function (b) {
                return b.ModelId == self.selectedModelId();
            });
            self.bikesList(null);
            self.bikesList(selbike);
            if (selbike.Schedules.length > 0)
                self.isDataExist(true);
            else
                self.isDataExist(false);
            self.isDays(isDaysDataExists(selbike.Schedules));
            self.isKms(isKmsDataExists(selbike.Schedules));
            if (selbike.HostUrl != '' && selbike.OriginalImagePath != '') {
                self.imagePath(selbike.HostUrl + "227x128" + selbike.OriginalImagePath);
            }
            else {
                self.imagePath('https://imgd.aeplcdn.com/0x0/bikewaleimg/images/noimage.png');
            }
        });
    }
    var vm = new SchedulesViewModel();
    ko.applyBindings(vm, $("#service-schedular")[0]);
    vm.selectedModelId(vm.bikes()[0].ModelId);
    <% } %>
</script>
