<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.Content.UpdateCityDetails"  Trace="false"%>
<script type="text/javascript" src="/src/common/common.js?V1.1"></script>
  <div>
                <div  class="margin10">
                    <div  class="floatLeft inputWidth">Select State : </div>
                    <div class="floatLeft"><asp:DropDownList ID="ddlStates"  runat="server" class="inputWidth"/></div>     
                    <div  class="floatLeft margin-left10">City : </div>
                    <div class="floatLeft inputWidth  margin-left10"><asp:TextBox ID="txtCity" MaxLength="50"  runat="server" class="floatLeft" /> </div><div><span class="errorMessage  margin-left10" id="spntxtCity"></span></div>
                    <div class="clear"></div>
                </div>             
                <div class="margin10">
                    <div  class="floatLeft  inputWidth">STD Code :</div>
                    <div  class="floatLeft inputWidth"><asp:TextBox ID="txtStdCode"  runat="server" /></div>
                    <div><span class="errorMessage  margin-left10" id="spnStdCode"></span></div>   
                    <div class="clear"></div>
                </div>              
                <div class="margin10">
                    <div  class="floatLeft inputWidth"> Masking Name : </div>
                    <div  class="floatLeft inputWidth"><asp:TextBox ID="txtMaskingName" MaxLength="60"  runat="server"  /></div>
                    <div  class="floatLeft"><span class="errorMessage  margin-left10" id="spntxtMaskingName"></span></div>
                    <div class="clear"></div>
                    <div  class="margin-top10" >
                        <span class="greenMsg">[Masking Name will be used for url formation.Only lowercase letters,- and digits are allowed.]</span>
                    </div>  
                    <div class="clear"></div>   
               </div>              
                <div  class="margin10">
                    <div  class="floatLeft  inputWidth">Latitude : </div>
                    <div class="floatLeft inputWidth"><asp:TextBox ID="txtLatitude" MaxLength="50"  runat="server" /></div>
                    <div  class="floatLeft margin-left10">Longitude :</div>
                    <div class="floatLeft inputWidth margin-left10"><asp:TextBox ID="txtLongitude" MaxLength="50"  runat="server" /></div>
                    <div class="margin-left10"><span class="errorMessage" id="spnLatLong"></span></div>
                    <div class="clear"></div>
                </div>               
                <div  class="margin10">
                    <div  class="floatLeft  inputWidth">Default Pin Code : </div>
                    <div class="floatLeft inputWidth"><asp:TextBox ID="txtPin" MaxLength="50"  runat="server" /></div>
                    <div><span class="errorMessage margin-left10" id="spnPin"></span></div>
                    <div class="clear"></div>
                </div>
               <div class="margin10  floatLeft">
		        <asp:Button ID="btnUpdate" Text="Update City" runat="server" />
               </div>  
               <div class="clear"></div>      
        </div>
</form>
<script>
    $(document).ready(function () {
        $('#btnUpdate').click(function () {

            $("#spnLatLong").text("");
            $("#spntxtMaskingName").text("");
            $("#spntxtCity").text("");
            $("#spnStdCode").text("");

            var reg = new RegExp('^[0-9\.]+$');
            var newReg = new RegExp('^[a-zA-Z& ]+$');
            var stdCode = $("#txtStdCode").val();
            var pinCode = $("#txtPin").val();
            var lattitude = $("#txtLatitude").val();
            var longitude = $("#txtLongitude").val();
            var isError = false;

            if ($("#txtCity").val() == "") {
                $("#spntxtCity").text("Enter City");
                isError = true;
            }
            else if (!newReg.test($("#txtCity").val())) {
                $("#spntxtCity").text("It should be characters only.");
                isError = true;
            }

            if (lattitude == "" || longitude == "") {
                $("#spnLatLong").text("Enter Latitude and Longitude.");
                isError = true;
            }
            else if (!reg.test(lattitude) || !reg.test(longitude)) {
                $("#spnLatLong").text("Both should be numeric.");
                isError = true;
            }

            if (!reg.test(stdCode) && stdCode != "") {
                $("#spnStdCode").text("STD code should be numeric.");
                isError = true;
            }

            if (stdCode.length > 5 && stdCode != "") {
                $("#spnStdCode").text("Max 5 digits allowed.");
                isError = true;
            }
            else if (stdCode.length < 2 && stdCode != "") {
                $("#spnStdCode").text("Min 2 digits allowed.");
                isError = true;
            }

            if ($("#txtMaskingName").val() == "") {
                $("#spntxtMaskingName").text("Masking Name Required.");
                isError = true;
            }
            else if (hasSpecialCharacters($("#txtMaskingName").val())) {
                $("#spntxtMaskingName").text("Invalid Masking Name. ");
                isError = true;
            }

            if (!reg.test(pinCode) && pinCode != "") {
                $("#spnPin").text("Pin Code should be numeric.");
                isError = true;
            }
            else if (pinCode.length != 6 && pinCode != "") {
                $("#spnPin").text("Pin Code requires 6 digits.");
                isError = true;
            }

            if (isError)
                return false;
            else
                alert("City updated successfully");
        })
    });
</script>

