<%@ Page Language="C#" AutoEventWireup="true" %>
<script runat="server">
    protected void Page_Load(object Sender, EventArgs e)
    {
        Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
        dd.DetectDevice();
    }
</script>
<!-- #include file="/includes/headhome.aspx" -->
<div class="container_12 margin-top15">
    <div class="grid_12">
        <h1>Advertise with Us</h1>
        <div class="margin-top10">We ensure that your advertisement is linked to the relevant pages and reach your customers in the right way. Please contact for any advertisement related queries:</div>
        <div class="grid_3 alpha margin-top15 min-height">
            <h2>West Zone</h2>
            <div class="margin-top15">
                <span class="bold">Mohd. Kashif Chilmai</span><br />
                <span class="bFontCol">Mobile: +91 9773405602</span><br />
                <span class="bFontCol">Email: <a href="mailto:kashif.chilmai@carwale.com" class="viewDetails">kashif.chilmai@carwale.com</a></span>
            </div>
        </div>
        <div class="grid_3 alpha margin-top15">
            <h2>North Zone</h2>
            <div class="margin-top15">
                <span class="bold">Rahul Gupta</span><br />
                <span class="bFontCol">Mobile: +91 9810531466</span><br />
                <span class="bFontCol">Email: <a href="mailto:rahul.gupta@carwale.com" class="viewDetails">rahul.gupta@carwale.com</a></span>
            </div> 
        </div>
        <div class="grid_3 alpha margin-top15">
            <h2>South Zone</h2>
            <div class="margin-top15">
                <span class="bold">Amal Siby Marian</span><br />
                <span class="bFontCol">Mobile: +91 9003105155</span><br />
                <span class="bFontCol">Email: <a href="mailto:amal.siby@carwale.com" class="viewDetails">amal.siby@carwale.com</a></span>
            </div>
        </div>
        <div class="grid_3 omega margin-top15">
            <h2>East Zone</h2>
            <div class="margin-top15">
                <span class="bold">Romita Choudhury</span><br />
                <span class="bFontCol">Mobile: +91 8454831114</span><br />
                <span class="bFontCol">Email: <a href="mailto:romita.choudhury@carwale.com" class="viewDetails">romita.choudhury@carwale.com</a></span>
            </div> 
        </div>
    </div>    
</div>
 <!-- #include file="/includes/footerinner.aspx" -->
