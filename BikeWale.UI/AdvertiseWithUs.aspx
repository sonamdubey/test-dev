<%@ Page Language="C#" AutoEventWireup="true" %>

<%
    title = "Advertise with CarWale and reach your customers - BikeWale";
    description = "Advertise your brands and products through BikeWale. BikeWale provides sophisticated tools for appropriate placing of your advertisement to target relavant customers. BikeWale provides various inputs to track visitor's behavious for better marketing decisions.";
    keywords = "Visitor Agreement, Relationship and Notice, Fees and Services, Use of the Site, Privacy";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
%>
<script runat="server">
    protected void Page_Load(object Sender, EventArgs e)
    {
        // Modified By :Ashish Kamble on 5 Feb 2016
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
            <h2>West Zone</h2>
            <div class="margin-top15">
                <span class="bold">Mohd. Kashif Chilmai</span><br />
                <span class="bFontCol">Email: <a href="mailto:kashif.chilmai@carwale.com" class="viewDetails">kashif.chilmai@carwale.com</a></span>
            </div>
        </div>
        <div class="grid_3 alpha margin-top15">
            <h2>North & East Zone</h2>
            <div class="margin-top15">
                <span class="bold">Rahul Gupta</span><br />
                <span class="bFontCol">Email: <a href="mailto:rahul.gupta@carwale.com" class="viewDetails">rahul.gupta@carwale.com</a></span>
            </div> 
        </div>
        <div class="grid_3 alpha margin-top15">
            <h2>South Zone</h2>
            <div class="margin-top15">
                <span class="bold">Amal Siby Marian</span><br />
                <span class="bFontCol">Email: <a href="mailto:amal.siby@carwale.com" class="viewDetails">amal.siby@carwale.com</a></span>
            </div>
        </div>        
    </div>    
</div>
<style type="text/css">
    .container-min-height { min-height:530px; }
</style>
 <!-- #include file="/includes/footerinner.aspx" -->
