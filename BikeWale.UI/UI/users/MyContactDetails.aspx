<%@ Page trace="false" Language="C#" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<% //Modified By :Sajal Gupta on 03 August 2016
    isAd300x250Shown = false;
    isAd300x250BtfShown = false; %>
<!-- #include file="/Includes/headMyBikeWale.aspx" -->

<script language="c#" runat="server">
	
	public CustomerDetails cd;
	private string _hdlName = "", _abtYou = "", _signature = "", _avtarPhoto = "", _realPhoto = "";
	
	void Page_Load(object sender, EventArgs e)
	{
		if ( CurrentUser.Id == "-1" )
			Response.Redirect( "/Users/Login.aspx?returnUrl=/users/MyContactDetails.aspx" );
			
		cd = new CustomerDetails(CurrentUser.Id);
	}	
</script>
<style type="text/css">
    #tblContactDetails span { font-weight:bold; }    
    #tblContactDetails td { color:#000; }
</style>
    <div class="container_12 container-min-height">
        <div class="grid_12">
            <ul class="breadcrumb">
                <li>You are here: </li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                    <a href="/" itemprop="url">
                        <span itemprop="title">Home</span>
                    </a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                    <a href="/mybikewale/" itemprop="url">
                        <span  itemprop="title">My BikeWale</span>
                    </a>
                </li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong>My Profile</strong></li>
            </ul><div class="clear"></div>
        </div>
	    <div class="grid_8 margin-top15">		
			<!--My Community Profile Content-->	
            <h2 style="display:inline;">Contact Details</h2><a href="/mybikewale/editcontactdetails/" class="margin-left5">Edit</a>
			<table id="tblContactDetails" class="margin-top10 grey-bg border-light" border="0" cellpadding="5" cellspacing="0" width="100%">					
				<tr>
					<td><span>Name :</span></td>
					<td><%=cd.Name%> </td>
				</tr>
				<tr>
					<td><span>EMail :</span></td>
					<td><%=cd.Email%></td>
				</tr>
				<%--<tr>
					<td><span>Phone :</span></td>
					<td><%=cd.Phone1%></td>
				</tr>--%>
				<tr>
					<td><span>Mobile :</span></td>
					<td><%=cd.Mobile %></td>
				</tr>
				<tr>
					<td><span>City :</span></td>
					<td><%=cd.City%></td>
				</tr>
					<!--<td><span>State :</span></td>
				<tr>
					<td><%=cd.State%></td>
				</tr>	-->					
			</table>		
	    </div>
        <div class="grid_4 margin-top15">
            
        </div>
    </div>
<!-- #include file="/UI/includes/footerinner.aspx" -->
<!-- Footer ends here -->
