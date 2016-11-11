<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.ServiceSchedule" %>
<div id="service-schedular" class="container bg-white box-shadow card-bottom-margin padding-15-20">
                <h2 class="margin-bottom5">Is your bajaj bike due for a service?</h2>
                <p class="margin-bottom15">Get your Bajaj bike serviced with time period given below.</p>
                <div class="select-box margin-bottom20">
                    <p class="font12 text-light-grey">Model</p>
                    <select class="chosen-select">
                        <% foreach (var bike in bikeScheduleList)
                           { %>
                            <option value="<%= bike.ModelId %>"><%=bike.ModelName %></option>
                        <% } %>
                    </select>
                </div>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <thead>
                        <tr>
                            <th align="left" width="20%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Service<br />no.</th>
                            <th align="left" width="40%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Validity from the<br />date of purchase</th>
                            <th align="left" width="40%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Validity from the date of previous service</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
							<td width="20%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">1</td>
                            <td width="40%" class="padding-bottom10 padding-top10 padding-right10 divider-top" valign="top">0-600 kms</td>
							<td width="40%" class="padding-bottom10 padding-top10 divider-top" valign="top">60 days</td>
						</tr>
                        <tr>
							<td width="20%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">2</td>
                            <td width="40%" class="padding-bottom10 padding-top10 padding-right10 divider-top" valign="top">500-1000 kms</td>
							<td width="40%" class="padding-bottom10 padding-top10 divider-top" valign="top">100 days</td>
						</tr>
                        <tr>
							<td width="20%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">3</td>
                            <td width="40%" class="padding-bottom10 padding-top10 padding-right10 divider-top" valign="top">1000-5000 kms</td>
							<td width="40%" class="padding-bottom10 padding-top10 divider-top" valign="top">250 days</td>
						</tr>
                        <tr>
							<td width="20%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">4</td>
                            <td width="40%" class="padding-bottom10 padding-top10 padding-right10 divider-top" valign="top">5000-10000 kms</td>
							<td width="40%" class="padding-bottom10 padding-top10 divider-top" valign="top">500 days</td>
						</tr>
                    </tbody>
                </table>
                <!-- no days -->
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="hide">
                    <thead>
                        <tr>
                            <th align="left" width="30%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Service<br />no.</th>
                            <th align="left" width="70%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Validity from the date<br />of purchase</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
							<td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">1</td>
							<td width="70%" class="padding-bottom10 padding-top10 divider-top" valign="top">0-600 kms</td>
						</tr>
                        <tr>
							<td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">2</td>
							<td width="70%" class="padding-bottom10 padding-top10 divider-top" valign="top">500-1000 kms</td>
						</tr>
                        <tr>
							<td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">3</td>
							<td width="70%" class="padding-bottom10 padding-top10 divider-top" valign="top">1000-5000 kms</td>
						</tr>
                        <tr>
							<td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">4</td>
							<td width="70%" class="padding-bottom10 padding-top10 divider-top" valign="top">5000-10000 kms</td>
						</tr>
                    </tbody>
                </table>
                <!-- no days -->
                <!-- no kms -->
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="hide">
                    <thead>
                        <tr>
                            <th align="left" width="30%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Service<br />no.</th>
                            <th align="left" width="70%" class="font12 text-unbold text-x-light padding-bottom5 border-solid-bottom">Validity from the date of previous service</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
							<td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">1</td>
							<td width="70%" class="padding-bottom10 padding-top10 divider-top" valign="top">60 days</td>
						</tr>
                        <tr>
							<td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">2</td>
							<td width="70%" class="padding-bottom10 padding-top10 divider-top" valign="top">100 days</td>
						</tr>
                        <tr>
							<td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">3</td>
							<td width="70%" class="padding-bottom10 padding-top10 divider-top" valign="top">250 days</td>
						</tr>
                        <tr>
							<td width="30%" class="padding-bottom10 padding-top10 padding-right10 font14 divider-top" valign="top">4</td>
							<td width="70%" class="padding-bottom10 padding-top10 divider-top" valign="top">500 days</td>
						</tr>
                    </tbody>
                </table>
                <!-- no kms -->
                <!-- no service -->
                <div id="service-not-avaiable" class="hide">
                    <span class="service-sprite calender-lg"></span>
                    <p class="font14 text-light-grey">Sorry! The service schedule for<br />Bajaj Pulsar is not available.<br />Please check out the service schedule for other Bajaj bikes.</p>
                </div>
                <!-- no service -->
            </div>

<script type="text/javascript">
    var bikeschedule = '<%=jsonBikeSchedule %>'
</script>