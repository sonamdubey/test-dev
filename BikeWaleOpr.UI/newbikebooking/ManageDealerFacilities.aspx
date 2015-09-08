<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.NewBikeBooking.ManageDealerFacilities" Trace="false" Debug="false" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Dealer Facilities</title>
    <%--<script src="/src/jquery-1.6.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="http://st.carwale.com/jquery-1.7.2.min.js?v=1.0" ></script>
    <style type="text/css">
        .errMessage {color:#FF4A4A; margin-left:60px}

    </style>
</head>
<body>
    <div>
        <h1>Manage Dealer Facilities</h1>
        Facilitiy <asp:TextBox ID="txtFacility" runat="server" />
        <asp:CheckBox ID="chkIsActiveFacility" runat="server" /><label for="chkIsActiveFacility">Is Active</label>
        <asp:Button ID="btnAddFacility" runat="server" Text="Add Facility"  style="margin-left:15px;"/>
        <asp:Button ID="btnUpdateFacility" runat="server" Text="Update Facility" style="margin-left:15px;"/>
        <asp:HiddenField ID="hdnFacilityId" runat="server" />
        <div  class="floatLeft margin-left10">
            <span id="spntxtFacility" class="errMessage"></span>
        </div>
    </div>
    <div>
        <asp:Repeater ID="rptFacilities" runat="server">
            <HeaderTemplate>
                <table>
                    <tr>
                        <th>Sr No.</th>
                        <th>Facilities</th>
                        <th>IsActive</th>
                        <th></th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Container.ItemIndex + 1 %></td>
                    <td id="facility_<%# DataBinder.Eval(Container.DataItem, "Id" ) %>"><%# DataBinder.Eval(Container.DataItem, "facility" ) %></td>
                    <td><input type="checkbox" id="chk_<%# DataBinder.Eval(Container.DataItem, "Id" ) %>" <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isActive" )) ?"checked":"" %> /></td>
                    <td><input type="button" facilityid="<%# DataBinder.Eval(Container.DataItem, "Id" ) %>" class="edit" value="Edit" /></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
        <script type="text/javascript">
            $(document).ready(function () {
                $(".edit").click(function () {
                    var facilityid = $(this).attr("facilityid");                    
                    $("#hdnFacilityId").val(facilityid);
                    $("#txtFacility").val($("#facility_" + facilityid).text());
                    $("#chkIsActiveFacility").attr("checked", $("#chk_" + facilityid).is(':checked'));                    
                });
            });
            $("#btnAddFacility, #btnUpdateFacility").click(function () {
                $("#spntxtFacility").text("");                
                var isError = false;
                if (jQuery.trim($("#txtFacility").val()).length == 0) {
                    $("#spntxtFacility").text("Please Enter facility");
                    isError = true;
                }
                return !isError;
            });
        </script>
    </form>
</body>
</html>
