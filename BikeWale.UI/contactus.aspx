<%@ Page Language="C#" AutoEventWireup="true" %>

<%
    title = "Contact Us - BikeWale";
    description = "Complete contact information, phone numbers, fax number of BikeWale.";
    keywords = "";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
    //Modified By :Sajal Gupta on 03 August 2016
    isAd300x250Shown = false;
    isAd600x270Shown = false;
%>

<script runat="server">
    protected void Page_Load(object Sender, EventArgs e)
    {
        // Modified By :Lucky Rathore on 12 July 2016.
        Form.Action = Request.RawUrl;
        // Modified By :Ashish Kamble on 5 Feb 2016
        string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
        if (String.IsNullOrEmpty(originalUrl))
            originalUrl = Request.ServerVariables["URL"];

        Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(originalUrl);
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
                Phone: (022) 6739 8888<br />
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
