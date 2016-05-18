<%@ Page Language="C#" AutoEventWireup="false"  Inherits="BikeWaleOpr.Content.UpdateStateDetails" Trace="false" %>
<form runat="server">
<script type="text/javascript" src="/src/common/common.js?V1.1"></script>
    <div>
        <div  class="margin10">
            <div  class="floatLeft inputWidth">State Name :</div>
            <div class="floatLeft inputWidth"><asp:TextBox ID="txtState" MaxLength="50"  runat="server" /></div>
            <div><span class="errorMessage margin-left10" id="spntxtName"></span></div>
        </div>
        <div class="clear"></div>
        <div class="margin10">
            <div  class="floatLeft  inputWidth">State Code :</div>
            <div  class="floatLeft inputWidth">
                <asp:textbox ID="txtStdCode" maxlength="2" runat="server"  /></div>
            <div><span class="errorMessage margin-left10" id="spnStdCode"></span></div>
                <div class="clear"></div>
            <div  class="margin10" >
                <span class="greenMsg">[State code should be 2 character only]</span>
            </div>   
            <div class="clear"></div> 
        </div>
      
        <div class="margin10">
            <div  class="floatLeft  inputWidth"> Masking Name : &nbsp</div>
            <div  class="floatLeft inputWidth"><asp:TextBox ID="txtMaskingName" MaxLength="50"  runat="server"  /></div>
            <div  class="floatLeft">
                <span class="errorMessage margin-left10" id="spntxtMaskingName"></span>
            </div>
            <div class="clear"></div>
            <div  class=" margin-top10" >
                <span class="greenMsg">[Masking Name will be used for url formation.Only lowercase letters,- and digits are allowed.]</span>
            </div> 
            <div class="clear"></div>    
        </div>
        
        <div class="floatLeft margin10" >
		    <asp:Button ID="btnUpdate" Text="Update State" runat="server"  />                
        </div>
        <div class="clear"></div>
     </div>
</form>
<script>
    $(document).ready(function ()
    {
        $('#btnUpdate').click(function ()
        {
            $("#spntxtMaskingName").text("");
            $("#spntxtName").text("");
            $("#spnStdCode").text("");
            $("#spnStdCode").text("");

            var isError = false;
            var state = $("#txtState").val();
            var newReg = new RegExp('^[a-zA-Z& ]+$');
            var codeReg = new RegExp('^[a-zA-Z]+$');
            var stateCode = $("#txtStdCode").val();

            if (state == "") {
                $("#spntxtName").text("Enter State");
                isError = true;
            }
            else if (!newReg.test(state)) {
                $("#spntxtName").text("It shosuld be characters only.");
                isError = true;
            }
            
            if (stateCode == "")
            {
                $("#spnStdCode").text("Enter State Code");
                isError = true;
            }
            else if (!codeReg.test(stateCode)) {
                $("#spnStdCode").text("It should be characters only");
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
            if (isError)
                return false;
        })
    });
</script>
