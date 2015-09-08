<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.MyBikeWale.RemoveFromListing" %>
<script type="text/javascript" src="http://st.carwale.com/jquery-1.7.2.min.js?v=1.0" ></script>
    <div id="div_RemoveInquiry" runat="server">
        <p><asp:Label ID="lblMsg" runat="server" /></p>	
	    <asp:DropDownList ID="drpStatus" runat="server" />
	    <div style="margin-top:10px;">Comments if any:</div>
	    <asp:TextBox TextMode="MultiLine" ID="txtComments" Rows="5" Columns="33" runat="server"	/>
	    <div align="center" style="margin-top:10px;">
		    <asp:button ID="btnSave" text="Remove My Bike" runat="server" />
		    <input type="button" value="Cancel" onclick="javascript:window.close()" />
	    </div>
    </div>
    <div>
        <asp:Label id="lblRemoveStatus" runat="server"></asp:Label>
    </div>
<script type="text/javascript">
    document.getElementById('btnSave').onclick = form_Submit;    
    inquiryId = '<%= inquiryId%>';

    function form_Submit(e) {
        if (document.getElementById('drpStatus').options[0].selected) {
            alert("Please choose a reason to continue!");
            document.getElementById('drpStatus').focus();
            return false;
        }
    }
    
    function post_status() {
        var objParent = opener.document.getElementById("div_" + inquiryId);
            
        objParent.innerHTML = "<b>Your bike listing S" + inquiryId + " has been successfully removed.</b>";
        self.close();        
    }

    $(document).ready(function () {                
        if ('<%= isRemovedListing%>' == '1')
        {
            post_status();
        }
    });
 </script>