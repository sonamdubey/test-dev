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
                <span class="bold">Anand Mohan</span><br />
                <span class="bFontCol">Mobile: +91 9833933784</span><br />
                <span class="bFontCol">Email: <a href="mailto:anand@carwale.com" class="viewDetails">anand@carwale.com</a></span>
            </div>
        </div>
        <div class="grid_3 alpha margin-top15">
            <h2>North Zone</h2>
            <div class="margin-top15">
                <span class="bold">Vishant Jagwani</span><br />
                <span class="bFontCol">Mobile: +91 9891393883</span><br />
                <span class="bFontCol">Email: <a href="mailto:vishant@carwale.com" class="viewDetails">vishant@carwale.com</a></span>
            </div> 
        </div>
        <div class="grid_3 alpha margin-top15">
            <h2>South Zone</h2>
            <div class="margin-top15">
                <span class="bold">Vishnukanth M.S</span><br />
                <span class="bFontCol">Mobile: +91 9845201660</span><br />
                <span class="bFontCol">Email: <a href="mailto:vishnu@carwale.com" class="viewDetails">vishnu@carwale.com</a></span>
            </div>
        </div>
        <div class="grid_3 omega margin-top15">
            <h2>East Zone</h2>
            <div class="margin-top15">
                <span class="bold">Sushil D'souza</span><br />
                <span class="bFontCol">Mobile: +91 9820531220</span><br />
                <span class="bFontCol">Email: <a href="mailto:gauri@carwale.com" class="viewDetails">sushil@carwale.com</a></span>
            </div> 
        </div>
    </div>    
</div>
 <!-- #include file="/includes/footerinner.aspx" -->
