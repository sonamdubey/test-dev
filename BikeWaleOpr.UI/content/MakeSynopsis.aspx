<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.content.MakeSynopsis" Trace="false" Debug="true" ValidateRequest="false"%>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
       <style type="text/css">
           .margin20{margin:20px;}
           .margin10 {margin:10px;}
	       .successMessage { text-align:center;color:#f00; }
    </style>
    <script src="/src/jquery-1.6.min.js" type="text/javascript"></script>
</head>
<body>
    <div class="margin20">
        <div>
            <h3>Add Synopsis for <%=make%></h3>
            <asp:Label ID="lblMessage" Visible="false" CssClass="successMessage" runat="server"></asp:Label>
        </div>
        <div class="margin10">
		    <FTB:FreeTextBox ToolbarStyleConfiguration="office2000" 
			    id="makeDescription" Height="200" Width="480"
			    JavaScriptLocation="ExternalFile" 
			    ButtonImagesLocation="ExternalFile"
			    ToolbarImagesLocation="ExternalFile"
			    SupportFolder="~/aspnet_client/FreeTextBox/" 
			    runat="Server" />
        </div>
        <div class="margin10">
            <asp:Button ID="btnSave" Text="Save" runat="server" Visible="false"></asp:Button>
            <asp:Button ID="btnUpdate" Text="Update" runat="server" Visible="false"></asp:Button>
        </div>	
    </div>
    </form>
   <%-- <script type="text/javascript">
        $("#btnSave").click(function () {
            alert($('#makeDescription').html());
            if ($('#makeDescription').val().trim() == "") {
                alert("Enter Description");
                return false;
            }
        });
    </script>--%>
</body>
</html>
