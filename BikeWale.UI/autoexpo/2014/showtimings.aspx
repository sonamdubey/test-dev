<%@ Page trace="false" debug="false" Language="C#" AutoEventWireUp="false"  %>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId          = 7;
	Title 			= "BikeWale Auto Expo 2014 Show Timings and Rates";
	Description 	= "BikeWale's coverage on Auto Expo 2014, the largest auto show in India.";
	Keywords		= "auto expo, auto expo 2014, auto show india, auto expo delhi";
	Revisit 		= "5";
	DocumentState 	= "Static";
    canonical       = "http://bikewale.com/autoexpo/2014/showtimings.aspx";
%>
<!-- #include file="/autoexpo/includes/headNews.aspx" -->
<% if ( Request.QueryString["pn"] != null && Request.QueryString["pn"].Length > 0 ) { %>
<META NAME="ROBOTS" CONTENT="NOINDEX, FOLLOW">
<% } %>

<script src="http://cdn.topsy.com/topsy.js?init=topsyWidgetCreator" type="text/javascript"></script>
<div class="pthead">
    <h1>Show Timings</h1>    
    <div class="clear"></div>
</div>
<div id="content" class="left-grid">
    <div class="content-box">
        <h3>7 Feb - 11 Feb 2014</h3>
        <div class="data-box">	
            <table class="tblShowtime" width="100%">
                <tr>
                    <th>Day & Date</th>
                    <th>Business Hours</th>
                    <th>General Public Hours</th>
                </tr>
                <tr>
                    <td class="showDt" >Friday, 7 February,2014</td>
                    <td>10 AM - 1 PM</td>
                    <td>1 PM - 6 PM</td>
                </tr>
                <tr>
                    <td class="showDt">Saturday, 8 February,2014</td>
                    <td>--</td>  
                    <td>10 AM - 7 PM</td>              
                </tr>
                <tr>
                    <td class="showDt"> Sunday, 9 February , 2014 </td>
                    <td> -- </td>
                    <%--<td rowspan="3" class="ltyellow">Weekdays <br /> <span>Premium Hours:</span> INR 500 <br /><span>General Hours:</span> INR 150 <br /><br />Weekends: INR 200 </td>--%>
                    <td> 10 AM - 7 PM  </td>
                </tr>
                <tr>
                    <td class="showDt"> Monday , 10 February, 2014</td>
                    <td>10 AM - 1 PM</td>       
                    <td> 1 PM - 6 PM  </td>         
                </tr>
                <tr>
                    <td class="showDt"> Tuesday, 11 February, 2014</td>
                    <td>10 AM - 1 PM</td>
                    <td> 1 PM - 5 PM </td>
                </tr>
            </table>
        <%--<p class="conditions">*Timings are subject to change *Entry will close one hour prior to the close timings *Entry strictly on invitation or tickets</p>
        <p class="conditions">*Entry opens for Public only from 7th January 2014</p>--%>
            <div class="clear"></div>
    </div>	    
    </div>
</div>

<div class="clear"></div>
<!-- #include file="/autoexpo/includes/footer.aspx" -->
  