<%@ Page trace="false" debug="false" Language="C#" AutoEventWireUp="false"  %>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId          = 4;
	Title 			= "BikeWale Auto Expo 2014 List of Exhibitors";
	Description 	= "BikeWale's coverage on Auto Expo 2014, the largest auto show in India.";
	Keywords		= "auto expo, auto expo 2014, auto show india, auto expo delhi";
	Revisit 		= "5";
	DocumentState 	= "Static";
    canonical       = "http://bikewale.com/autoexpo/2014/exhibitorlist.aspx";
%>
<!-- #include file="/autoexpo/includes/headNews.aspx" -->
<% if ( Request.QueryString["pn"] != null && Request.QueryString["pn"].Length > 0 ) { %>
<META NAME="ROBOTS" CONTENT="NOINDEX, FOLLOW">
<% } %>
<script src="http://cdn.topsy.com/topsy.js?init=topsyWidgetCreator" type="text/javascript"></script>
<div class="pthead">
    <h1>Key Exhibitors</h1>    
    <div class="clear"></div>
</div>
<div id="content" class="left-grid">
    <!-- 2 Wheelers/ 3 Wheelers code starts here-->
    <div class="content-box">
        <h3>2 Wheelers/ 3 Wheelers</h3>
        <div class="data-box">
              <table class="tblExhib" width="100%">
             <tr><th>Company</th><th>Hall No</th></tr>
            <tr><td>Bajaj Auto Ltd.</td><td  class="gray">8</td></tr>
            <tr><td>DSK Motowheels Pvt. Ltd.</td><td  class="gray">4</td></tr>
            <tr><td>H-D Motor Company India Pvt. Ltd.</td><td>4</td></tr>
            <tr><td>Hero MotoCorp Ltd.</td><td  class="gray">8</td></tr>
            <tr><td>Honda Motorcycle and Scooter India (P) Ltd.</td><td  class="gray">6</td></tr>
            <tr><td>India Yamaha Motor Pvt. Ltd.</td><td  class="gray">6</td></tr>
            <tr><td>Lohia Auto Industries</td><td>4</td></tr>
            <tr><td>Mahindra & Mahindra Ltd.</td><td  class="gray">10</td></tr>
            <tr><td>Moto Morini (Italy)</td><td  class="gray">6</td></tr>
            <tr><td>Piaggio Vehicles Pvt. Ltd.</td><td  class="gray">8</td></tr>
            <tr><td>Suzuki Motorcycle India Ltd.</td><td  class="gray">6</td></tr>
            <tr><td>Triumph Motorcycles (India) Pvt. Ltd.</td><td  class="gray">4</td></tr>
            <tr><td>TVS Motor Company</td><td  class="gray">6</td></tr>
            <tr><td>UM India Pvt. Ltd.</td><td  class="gray">6</td></tr>
            <tr><td>Vardenchi Motorcycles</td><td  class="gray">6</td></tr>
             </table>
        </div>
    </div>
    <!-- 2 Wheelers/ 3 Wheelers code ends here-->
        <!-- Passenger Cars code starts here-->
    <div class="content-box">
        <h3>Passenger Cars</h3>
        <div class="data-box">
              <table class="tblExhib" width="100%" >
                <tr><th>Company</th><th>Hall No</th></tr>
                <tr><td>Audi India</td><td  class="gray">15</td></tr>
                <tr><td>Bajaj Auto Ltd.</td><td  class="gray">8</td></tr>
                <tr><td>BMW India Pvt. Ltd.</td><td>15</td></tr>
                <tr><td>Fiat Group Automobiles India Pvt. Ltd.</td><td  class="gray">10</td></tr>
                <tr><td>Ford India Pvt Ltd</td><td  class="gray">1</td></tr>
                <tr><td>General Motors India Pvt. Ltd.</td><td  class="gray">5</td></tr>
                <tr><td>Honda Cars India Ltd.</td><td  class="gray">9</td></tr>
                <tr><td>Hyundai Motor India Ltd.</td><td>3</td></tr>
                <tr><td>Isuzu Motors India Private Ltd.</td><td  class="gray">1</td></tr>
                <tr><td>Jaguar Land Rover</td><td  class="gray">14</td></tr>
                <tr><td>JEEP</td><td  class="gray">10</td></tr>
                <tr><td>Mahindra & Mahindra Ltd.</td><td  class="gray">10</td></tr>
                <tr><td>Marutui Suzuki India Ltd.</td><td  class="gray">7</td></tr>
                <tr><td>Mercedes-Benz India Pvt Ltd.</td><td  class="gray">15</td></tr>
                <tr><td>Mini</td><td  class="gray">15</td></tr>
                <tr><td>Mitsuoka Motor Co. Ltd.</td><td  class="gray">15</td></tr>
                <tr><td>Nissan Motor India Pvt. Ltd.</td><td  class="gray">12</td></tr>
                <tr><td>Renault India Pvt. Ltd.</td><td  class="gray">5</td></tr>
                <tr><td>Skoda Auto India Pvt. Ltd.</td><td  class="gray">12</td></tr>
                <tr><td>Tata Motors Ltd.</td><td  class="gray">14</td></tr>
                <tr><td>Toyota Kirloskar Motor Pvt. Ltd.</td><td  class="gray">9</td></tr>
                <tr><td>Volkswagen Group Sales India Pvt. Ltd.</td><td  class="gray">12</td></tr>
                </table> 
        </div>
    </div>
    <!-- Passenger Cars code ends here-->
    <!-- Electric Vehicles code starts here-->
    <div class="content-box">
        <h3>Electric Vehicles</h3>
        <div class="data-box">
            <table class="tblExhib" width="100%">
             <tr><th>Company</th><th>Hall No</th></tr>
            <tr><td>Areion Motors</td><td  class="gray">4</td></tr>
            <tr><td>Cangge Motors</td><td  class="gray">2</td></tr>
            <tr><td>Lohia Auto Industries</td><td>4</td></tr>
            <tr><td>Lovely Professional University</td><td  class="gray">4</td></tr>
            <tr><td>Sway Motors</td><td  class="gray">2</td></tr>
            <tr><td>Terra Motor Technologies</td><td  class="gray">6</td></tr>
            </table>
        </div>
    </div>
    <!-- Electric Vehicles code ends here-->
    <!-- Commercial Vehicles code starts here-->
    <div class="content-box">
        <h3>Commercial Vehicles</h3>
        <div class="data-box">
            <table class="tblExhib" width="100%">
                 <tr><th>Company</th><th>Hall No</th></tr>
            <tr><td>Ashok Leyland Ltd.</td><td>11</td></tr>
            <tr><td>Isuzu Motors India Private Ltd.</td><td  class="gray">1</td></tr>
            <tr><td>Mahindra & Mahindra Ltd.</td><td  class="gray">10</td></tr>
            <tr><td>Paracoat Products Ltd.</td><td  class="gray">11</td></tr>
            <tr><td>SML ISUZU Ltd.</td><td  class="gray">11</td></tr>
            <tr><td>Tata Motors Ltd.</td><td  class="gray">14</td></tr>
            <tr><td>VE Commercial Vehicles Ltd.</td><td  class="gray">11</td></tr>
            <tr><td>Volvo India Pvt Ltd.</td><td  class="gray">11</td></tr>
             </table>
        </div>
    </div>
    <!-- Commercial Vehicles code ends here-->
    <!-- Others code starts here-->
    <div class="content-box">
        <h3>Others</h3>
        <div class="data-box">
            <table class="tblExhib" width="100%">
                 <tr><th>Company</th><th>Hall No</th></tr>
            <tr><td>Hindustan Petroleum Corporation Ltd.</td><td>2</td></tr>
            <tr><td>Mega Telematics Pvt. Ltd.</td><td  class="gray">2</td></tr>
            <tr><td>MIT Institute of Design</td><td  class="gray">4</td></tr>
            <tr><td>Yokohoma</td><td  class="gray">2</td></tr>
             </table>
        </div>
    </div>
    <!-- Others code ends here-->	
       
</div>
<div class="right-grid">
    <div class="content-box">
        <h3>Know More About Brands</h3>
        <div class="brands-list">                	
            <ul>
                <li><a href="/autoexpo/2014/brand.aspx?mid=1">Bajaj</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=8">Hyosung</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=5">Harley Davidson</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=6">Hero</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=7">Honda</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=13">Yamaha</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=10">Mahindra</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=12">Suzuki</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=22">Triumph</a></li>
                <li><a href="/autoexpo/2014/brand.aspx?mid=15">TVS</a></li>
            </ul>
            <div class="clear"></div>
                    
        </div>
    </div>
</div>

<div style="clear:both;"></div>
<!-- #include file="/autoexpo/includes/footer.aspx" -->

  