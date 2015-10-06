﻿<%@ Page Language="C#" AutoEventWireup="true" %>
<script runat="server">
    protected void Page_Load(object Sender, EventArgs e)
    {
        Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
        dd.DetectDevice();
    }
</script>
<!-- #include file="/includes/headhome.aspx" -->
<div class="container_12 margin-top15 container-min-height">
    <div class="grid_12 min-height">
        <h1>Contact BikeWale</h1>
        <div class="margin-top15">           
            <div style="line-height:18px;">
                Automotive Exchange Pvt Ltd,
                12th floor, Vishwaroop IT Park,<br />
                Sector 30A, Vashi,<br />
                Navi Mumbai - 400705<br />
                Phone: 1800 120 8300<br />
                Fax: (022) 6645 9665, 6739 8877<br />
                Email: contact@bikewale.com<br />
                Working Hours: 10 a.m. - 7 p.m. (closed on Sundays and Holidays)
            </div>
        </div>
    </div>    
</div>
<style type="text/css">
    .container-min-height { min-height:530px; }
</style>
<!-- #include file="/includes/footerinner.aspx" -->
