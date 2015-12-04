<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.content.ManageNewModelColor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Model Color</title>
    <link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
    <script type="text/javascript" src="http://st.carwale.com/jquery-1.7.2.min.js?v=1.0"></script>
    <script src="/src/AjaxFunctions.js" type="text/javascript"></script>
    <script src="/src/graybox.js"></script>
    <style>
        .yellow {
            background-color: #ffff48;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Update Model Color</h2>            
            <h3 class="margin-left10 margin-bottom10;">Color Name : <%= ModelColorName %></h3>            
            <span id="spnError" class="error" runat="server"></span>
            <asp:Repeater ID="rptHexCode" runat="server" EnableViewState="false">
                <HeaderTemplate>                 
                 <table border="1" style="border-collapse:collapse;" cellpadding ="5" class="margin-left10 lstTable" >
                    <tr>
                        <th>Color</th>
                        <th>Hex Code</th>
                        <th>Is Active</th>
                    </tr>
                 </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><span style="width:40px;height:40px;display:inline-block;background-color:#<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"HexCode")) %>"></span></td>
                        <td><asp:textbox runat="server" id="txtHexCode" maxlength="6" columns="6" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"HexCode"))%>' /><asp:HiddenField id="hdnColorID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ID") %>' /></td>
                        <td><asp:CheckBox ID="chkActive" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsActive"))%>' /></td>
                    </tr>       
                </ItemTemplate>                
            </asp:Repeater>
            <table class="margin-left10">
                <tr>                    
                    <td><asp:textbox runat="server" id="txtNewHexCode" maxlength="6" columns="6" /></td>                    
                    <td><asp:button id="btnSave" text="Add Color" runat="server"/></td>
                </tr>
            </table>  
            <asp:button id="btnUpdate" style="margin-left:13px;" runat="server" text="Update Model Color" Enabled="false" />            
        </div>
    </form>
    <script type="text/javascript">
        
        function refreshParent() {
            window.opener.location.reload();
        }
        $(":checkbox").live("click", function isChecked(e) {
            $("#btnUpdate").prop("disabled",false);
            $(this).attr("isModified", "true");
        });

        $("input[id*='rptHexCode_txtHexCode']").live("keyup", function () {
            $("#btnUpdate").prop("disabled", false);
        });

        function validateHexColorCode(val) {
            var patt = new RegExp("^(?:[0-9a-fA-F]{3}){1,2}$");
            return patt.test(val);
        }

        $("#btnSave").live("click", function () {
            refreshParent();
        });

        $("#btnUpdate").live("click", function () {
            var itemCount = 0;
            var isValid = true;
            var totalCount = $(":text").length;
            $("input[id*='rptHexCode_txtHexCode']").each(function () {
                if (totalCount != itemCount) {
                    isValid &= validateHexColorCode($(this).val());
                }
            });
            if (!isValid) {
                alert("Input is invalid.");
                return false;
            }
            else {
                refreshParent();
                return true;
            }
        });
    </script>
</body>
</html>
