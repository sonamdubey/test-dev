<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.MyBikeWale.RemoveFromListing" EnableViewState="false" %>
<%
    title = "My BikeWale - Remove Sell Bike Ad";
%>
<!-- #include file="/includes/headMyBikeWale.aspx" -->

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
                <li class="current"><strong>My BikeWale</strong></li>
            </ul><div class="clear"></div>
        </div>
    <div class="grid_12 margin-top10">
        <div id="div_<%= inquiryId %>"></div>
    <div id="div_RemoveInquiry" class="content-block grey-bg border-light margin-top15" runat="server">
        <% if(isAuthorised) { %>  
        <p><asp:Label ID="lblMsg" runat="server" /></p>	
	    <asp:DropDownList ID="drpStatus" runat="server" Width="250px"/>
	    <div style="margin-top:10px;">Comments if any:</div>
	    <asp:TextBox TextMode="MultiLine" ID="txtComments" Rows="5" Columns="33" runat="server"	/>
		   <div style="margin-top:10px;"><asp:button cssClass="action-btn text_white" ID="btnSave" onclientclick="javascript:form_Submit" text="Remove My Bike" runat="server" />		    
               </div> 
        <% } 
        else
        { %>
     <div class="margin-top20 text-bold">You don't have access to remove this listing because this listing was posted using a different login id.</div>
                    <div class="margin-top10">You should use the same login credentials (email and password) that you had used while posting this listing on BikeWale.</div>

     <% } %>
    </div>
    <div>
        <asp:Label id="lblRemoveStatus" runat="server"></asp:Label>
    </div>
    </div>
<script type="text/javascript">    
   var inquiryId = '<%= inquiryId%>';

    function form_Submit(e) {
        if (document.getElementById('drpStatus').options[0].selected) {
            alert("Please choose a reason to continue!");
            document.getElementById('drpStatus').focus();
            return false;
        }
    }
    
    function post_status() {
        var objParent = document.getElementById("div_" + inquiryId);
        document.getElementById("div_RemoveInquiry").style.visibility = 'hidden';
        objParent.innerHTML = "<b>Your bike listing S" + inquiryId + " has been successfully removed.</b>";
        self.close();        
    }

    function postFailureMessage() {
        var objParent = document.getElementById("div_" + inquiryId);
        document.getElementById("div_RemoveInquiry").style.visibility = 'hidden';
        objParent.innerHTML = "<b>Your bike listing S" + inquiryId + " has not been removed.</b>";
        self.close();
    }

    $(document).ready(function () {

        switch ('<%= isRemovedListing%>') {
            case '200':
                post_status();
            case '401':
                postFailureMessage();
            default:
                return;
        }
    });
 </script>     
     </div>

<!-- #include file="/includes/footerInner.aspx" -->