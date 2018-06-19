<%@ Page Language="C#" AutoEventWireup="true" %>

<%
    title = "Advertise with CarWale and reach your customers - BikeWale";
    description = "Advertise your brands and products through BikeWale. BikeWale provides sophisticated tools for appropriate placing of your advertisement to target relavant customers. BikeWale provides various inputs to track visitor's behavious for better marketing decisions.";
    keywords = "Visitor Agreement, Relationship and Notice, Fees and Services, Use of the Site, Privacy";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
    //Modified By :Sajal Gupta on 03 August 2016
    isAd300x250Shown = false;
    isAd600x270Shown = false;
%>
<script runat="server">
    protected void Page_Load(object Sender, EventArgs e)
    {
        // Modified By :Ashish Kamble on 5 Feb 2016
        Form.Action = Request.RawUrl;
        string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
        if (String.IsNullOrEmpty(originalUrl))
            originalUrl = Request.ServerVariables["URL"];

        Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(originalUrl);
        dd.DetectDevice();        
    }
</script>
<!-- #include file="/includes/headhome.aspx" -->

<div class="container_12 container-min-height">
    <div class="grid_12 margin-top10">
        <h1>Advertise with Us</h1>
        <div class="margin-top10">We ensure that your advertisement is linked to the relevant pages and reach your customers in the right way. Please contact for any advertisement related queries:</div>
        <div class="grid_3 alpha margin-top15 min-height">
            <h2>Contact Details</h2>
            <div class="margin-top15">
                <span class="bold">Vijay Menon</span><br />
                <span class="bFontCol">Email: <a href="mailto:vijay@bikewale.com" class="viewDetails">vijay@bikewale.com</a></span>
            </div>
        </div>        
    </div>    
</div>
<style type="text/css">
    .container-min-height { min-height:530px; }
</style>
 <!-- #include file="/includes/footerinner.aspx" -->
