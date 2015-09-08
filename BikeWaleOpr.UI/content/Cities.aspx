<%@ Page Language="C#" AutoEventWireup="false"  Inherits="BikeWaleOpr.Content.Cities" Trace="false" Debug="false" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<script type="text/javascript" src="/src/common/common.js?V1.1"></script>
<script type="text/javascript" src="../src/graybox.js"></script>
<div class="urh">
		You are here &raquo; Contents &raquo; Add Cities
</div>
<div>
    <!-- #Include file="contentsMenu.aspx" -->
</div>
<style>
    .rptrTable {width:700px;border: 1px solid black;border-collapse: collapse;margin-top:20px;}
    .rptrTable td {padding : 5px;border: 1px solid black;}
    .rptrTable th{background-color:#E5E5E5;font-weight:bold; text-align:center;border: 1px solid black;}
    .rptrTable a {text-decoration:underline;cursor:pointer;}
</style>
<div class="left">
        <div>
        <fieldset>
            <legend>Add New Cities</legend>
            <div>
                 <div  class="margin10">
                    <div  class="floatLeft inputWidth">Select State :</div>
                    <div class="floatLeft"><asp:DropDownList ID="ddlStates" AutoPostBack="true" runat="server" class="inputWidth" /></div>   
                    <div class="floatLeft inputWidth"><span class="errorMessage margin-left10" id="spnState" runat="server"></span></div>  
                    <div  class="floatLeft">City : </div>
                    <div class="floatLeft inputWidth  margin-left10"><asp:TextBox ID="txtCity" MaxLength="50" runat="server" class="floatLeft" /></div>
                    <div class="floatLeft"><span class="errorMessage margin-left10" id="spntxtCity"></span></div>
                    <div class="clear"></div>
                </div>
                <div class="margin10">
                    <div  class="floatLeft inputWidth">STD Code :</div>
                    <div  class="floatLeft inputWidth"><asp:TextBox ID="txtStdCode"  runat="server"  /></div>
                    <div><span class="errorMessage margin-left10" id="spnStdCode"></span></div>   
                    <div class="clear"></div>
                </div>               
                <div class="margin10">
                    <div  class="floatLeft inputWidth"> Masking Name :</div>
                    <div  class="floatLeft inputWidth"><asp:TextBox ID="txtMaskingName" MaxLength="60"  runat="server" /></div>
                    <div  class="floatLeft">
                        <span class="errorMessage margin-left10" id="spntxtMaskingName"></span>
                    </div>
                    <div class="clear"></div>
                    <div  class="margin-top10 " >
                        <span class="greenMsg">[Masking Name will be used for url formation.Only lowercase letters,- and digits are allowed.]</span>
                    </div>     
               </div>
                <div  class="margin10">
                    <div  class="floatLeft inputWidth">Latitude :</div>
                    <div class="floatLeft inputWidth"><asp:TextBox ID="txtLatitude" MaxLength="50" runat="server" /></div>
                    <div  class="floatLeft  margin-left10">Longitude : </div>
                    <div class="floatLeft inputWidth margin-left10"><asp:TextBox ID="txtLongitude" MaxLength="50" runat="server" /></div>
                    <div><span class="errorMessage margin-left10" id="spnLatLong"></span></div>
                    <div class="clear"></div>
                </div>         
                <div  class="margin10">
                    <div  class="floatLeft inputWidth">Default Pin Code : </div>
                    <div class="floatLeft inputWidth"><asp:TextBox ID="txtPin" MaxLength="6" runat="server" /></div>
                    <div><span class="errorMessage margin-left10" id="spnPin"></span></div>
                    <div class="clear"></div>
                </div>          
               <div class="margin-top10 margin-left10 floatLeft">
		        <asp:Button ID="btnSave" Text="Add City" runat="server" />               
               </div>  
               <div class="clear"></div>              
        </div>
        </fieldset>
    </div>
    <div>
        <asp:repeater runat="server" id="rptCities">
            <HeaderTemplate >
                <table class="rptrTable">
                    <tr>
                        <th>Sr.No.</th>
                        <th>Name</th>
                        <th>MaskingName</th>
                        <th>Latitude</th>
                        <th>Longitude</th>
                        <th>DefaultPinCode</th>
                        <th>Edit</th>
                        <th>Delete</th>
                    </tr>
            </HeaderTemplate>
            <itemtemplate>  
                    <tr id="row_<%#Eval("ID")%>">
                        <td><%#Container.ItemIndex+1 %></td>
                        <td class="city"><%#Eval("Name") %></td>
                        <td class="maskingname"><%#Eval("MaskingName") %></td>
                        <td class="latitude"><%#Eval("Lattitude") %></td>
                        <td class="longitude"><%#Eval("Longitude") %></td>
                        <td class="defaultPinCode"><%# Eval("DefaultPinCode") %></td>
                        <td class="edit"><a id="edit_<%# Eval("ID") %>">Edit</a></td>
                        <td class="delete"><a id="delete_<%#Eval("ID") %>">Delete</a></td>
                    </tr>                    
            </itemtemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:repeater>       
</div>
</form>
</div>
<script type="text/javascript">
    $(document).ready(function ()
    {
        $("#btnSave").click(function () {

            if (!isValid())
                return true;
            else
                return false;
        });

        $("a[id^='delete_']").click(function () {
            var CityId = $(this).attr('id');
            CityId = CityId.split('_')[1];
            var cityName = $(this).parents().find(".city").html();

            if (confirm("Are you sure want to delete this City?")) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"cityId":"' + CityId + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "DeleteCity"); },
                    success: function (response) {
                        $("#row_" + CityId).html("<td colspan='8'>" + cityName + " city has been deleted</td>").addClass("orange");
                    }
                });
                alert("City Deleted Sucessfully...");
            }
        });

        $('#txtCity').blur(function () {
            var city = jQuery.trim($('#txtCity').val());
            city = city.trim();
            city = city.replace(/\s+/g, "-");
            city = city.replace(/[^a-zA-Z0-9\-]+/g, '');
            city = removeHyphens(city);
            $('#txtMaskingName').val(city.toLowerCase());
        });

        $("a[id^='edit_']").click(function () {
            var CityId = $(this).attr('id');
            CityId = CityId.split('_')[1];
            var comment = "";
            var caption = "Update City Details";

            var url = "/content/UpdateCityDetails.aspx?id=" + CityId;
            var applyIframe = true;
            var GB_Html = ""
            GB_show(caption, url, 230, 650, applyIframe, GB_Html);
        });
    })

    function isValid()
    {
        $("#spntxtMaskingName").text("");
        $("#spntxtCity").text("");
        $("#spnState").text("");
        $("#spnLatLong").text("");
        $("#spnPin").text("");
        $("#spnStdCode").text("");
        
        var reg = new RegExp('^[0-9\.]+$');
        var newReg = new RegExp('^[a-zA-Z& ]+$');
        var stdCode = $("#txtStdCode").val();
        var pinCode = $("#txtPin").val();
        var lattitude = $("#txtLatitude").val();
        var longitude = $("#txtLongitude").val();
        var isError = false;

      
        if ($("#ddlStates").val() == 0) {
            $("#spnState").text("Select State.")
            isError = true;
        }

        if ($("#txtCity").val() == "") {
            $("#spntxtCity").text("Enter City.");
            isError = true;
        }
        else if (!newReg.test($("#txtCity").val()))
        {
            $("#spntxtCity").text("It shosuld be characters only.");
            isError = true;
        }

        if (!reg.test(stdCode) && stdCode != "")
        {
            $("#spnStdCode").text("STD code should be numeric.");
            isError = true;
        }

        if (stdCode.length > 5 && stdCode != "" )
        {
            $("#spnStdCode").text("Max 5 digits allowed.");
            isError = true;
        }
        else if (stdCode.length < 2 && stdCode != "")
        {
            $("#spnStdCode").text("Min 2 digits allowed.");
            isError = true;
        }
     
        if (lattitude == "" || longitude == "")
        {
            $("#spnLatLong").text("Enter Latitude and Longitude.");
            isError = true;
        }
        else if (!reg.test(lattitude) || !reg.test(longitude))
        {
            $("#spnLatLong").text("Both should be numeric.");
            isError = true;
        }

        if ($("#txtMaskingName").val() == "")
        {
            $("#spntxtMaskingName").text("Enter Masking Name.");
            isError = true;
        }
        else if (hasSpecialCharacters($("#txtMaskingName").val()) == true) {
            $('#spntxtMaskingName').text("Invalid Masking Name.");
            isError = true;
        }

        if (!reg.test(pinCode) && pinCode != "")
        {
            $("#spnPin").text("Pin Code should be numeric.");
            isError = true;
        }
        else if (pinCode.length != 6 && pinCode != "")
        {
            $("#spnPin").text("Pin Code requires 6 digits.");
            isError = true;
        }
        return isError;
    }
</script>
<!-- #Include file="/includes/footerNew.aspx" -->

