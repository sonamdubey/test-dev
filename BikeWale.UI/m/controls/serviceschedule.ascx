<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.ServiceSchedule" %>
<% if(BikeScheduleList!= null) { %>
<div id="service-schedular" class="container bg-white box-shadow card-bottom-margin padding-15-20">
                <h2 class="margin-bottom5">Is your <%= MakeName %> bike due for a service?</h2>
                <p class="margin-bottom15">Get your <%= MakeName %> bike serviced within given time period or km range, whichever condition gets satisfied earlier.</p>
                <div class="select-box size-small margin-bottom20">
                    <p class="font12 text-light-grey">Model</p>
                    <select id="selBikes" class="chosen-select" data-bind="event: { change: GetModelId }"
                        <% foreach (var bike in BikeScheduleList)
                           { %>
                            <option value="<%= bike.ModelId %>"><%=bike.ModelName %></option>
                        <% } %>
                    </select>
                </div>
                <table data-bind="visible: isDataExist()" id="one" width="100%" border="0" cellspacing="0" cellpadding="0">
                    <thead>
                        <tr>
                            <th align="left" width="20%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Service<br />no.</th>
                            <th data-bind="visible: isKms()" align="left" width="40%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Validity from the<br />date of purchase</th>
                            <th data-bind="visible: isDays()" align="left" width="40%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Validity from the date of previous service</th>
                        </tr>
                    </thead>
                    <tbody data-bind="foreach: bikesList">
                        <!-- ko foreach : Schedules -->
                        <tr>
							<td data-bind="text: ServiceNo" width="20%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top"></td>
                            <td data-bind="visible: $root.isKms(),text: Kms + ' Kms'" width="40%" class="padding-bottom10 padding-top10 padding-right10 divider-top" valign="top"></td>
							<td data-bind="visible: $root.isDays(), text: Days + ' days'" width="40%" class="padding-bottom10 padding-top10 divider-top" valign="top"></td>
                        </tr>
                        <!-- /ko -->
                    </tbody>
                </table>
                <!-- no service -->
                <div data-bind="visible: !isDataExist()" id="service-not-avaiable">
                    <span class="service-sprite calender-lg"></span>
                    <p class="font14 text-light-grey">Sorry! The service schedule for<br />
                        <span data-bind="text: currentBikeName()"></span> is not available.<br />
                        Please check out the service schedule for other <%= MakeName %> bikes.</p>
                </div>
                <!-- no service -->
            </div>
<% } %>
<script type="text/javascript">
    <% if (!string.IsNullOrEmpty(jsonBikeSchedule))
       {%>
        var bikeschedule = '<%=jsonBikeSchedule.Replace("'", string.Empty) %>';
        bikeschedule = JSON.parse(bikeschedule.replace(/\s/g, ' '));
        function SchedulesViewModel() {
            var self = this;
            self.bikes = ko.observable(bikeschedule);
            self.currentBikeName = ko.observable();
            self.bikesList = ko.observable(self.bikes());
            self.isDataExist = ko.observable();
            self.isDays = ko.observable();
            self.isKms = ko.observable();
            self.selectedModelId = ko.observable();
            self.GetModelId = function (data, event) {
                self.selectedModelId($(event.target).val());
                var bikename = ko.utils.arrayFirst(self.bikes(), function (bike) {
                    return bike.ModelId == self.selectedModelId();
                });
                if (bikename != null)
                    self.currentBikeName(bikename.ModelName);
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
                var arr = ko.utils.arrayFirst(self.bikes(), function (b) {
                    return b.ModelId == self.selectedModelId();
                });
                self.bikesList(null);
                self.bikesList(arr);
                if (arr.Schedules.length > 0)
                    self.isDataExist(true);
                else
                    self.isDataExist(false);
                self.isDays(isDaysDataExists(arr.Schedules));
                self.isKms(isKmsDataExists(arr.Schedules));
            });
        }
        var vm = new SchedulesViewModel();
        ko.applyBindings(vm, $("#service-schedular")[0]);
        vm.selectedModelId(vm.bikes()[0].ModelId);
        vm.currentBikeName($("#selBikes option:selected").text());
    <% } %>
</script>
