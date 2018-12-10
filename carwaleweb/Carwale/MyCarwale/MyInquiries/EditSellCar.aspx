<%@ Page Language="C#" Inherits="Carwale.UI.MyCarwale.MyInquiries.EditSellCar" Trace="false" AutoEventWireUp="false" Debug="false" %>
<%@ Register TagPrefix="Vspl" TagName="Calender" src="/Controls/DateControl.ascx" %>
<!doctype html>
<html>
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 72;
	Title 			= "Edit your car details";
	Description 	= "Sell Your Used Car at Carwale.com. Its easy and free.";
	Keywords		= "sell car, car sale, used car sell, second-hand car sell, selling car, indian, india, sell imported car";
	Revisit 		= "15";
	DocumentState 	= "Static";
    AdId            = "1337162297840";
    AdPath          = "/7590/CarWale_MyCarWale/CarWale_MyCarWale_Misc/CarWale_MyCarWale_Misc_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<style>
.features td{padding:3px;}
.features th{text-align:left;}
.features label{padding-left:3px;}

.ul-normal li{height:25px;}
.colors{width:220px; display:none; position:absolute; background-color:#fff; height:300px; overflow:auto; z-index:1001; border:1px solid #A6C9E2; padding:5px 0 5px 2px;}
.colors .color-bg{display:inline-block;float:left;  width:30px; height:20px; border:1px solid #B4B4B4;}
#color-name{float:right; width:160px; padding:3px 3px;}
.colors .color-selected{background-color:#409FFF; color:#fff; cursor:pointer;}
.comboColors{width:222px; border:1px solid #A6C9E2; margin-bottom:1px;}
.colorPreview{width:30px; height:20px; border:1px solid #B4B4B4; display:none; float:left; margin:2px 5px 0 2px;}
.bgSelect{font-weight:bold;}
.bgUnSelect{font-weight:normal;}
.error { color:#f04130;}
select {
    border: 1px solid #d3d3d3;
    border-radius: 5px;
    padding: 5px 13px;
}
.tblDefault {
    border-collapse: collapse;
    width: 100%;
}
.tblDefault th {
    background-color: #eeeeee;
    border-bottom: 1px solid #cccccc;
    color: #5b5b5b;
    font-weight: bold;
    padding: 5px;
    text-align: left;
}
.tblDefault td {
    border-bottom: 1px solid #e5e5e5;
    padding: 8px;
}
.tblDefault p {
    margin: 0;
    padding: 5px 0;
}
.tbl-std th {
    color: #858585;
    font-weight: normal;
    padding: 4px 0;
    text-align: left;
}
.tbl-std td {
    padding: 4px 0;
}
.hr-dotted {
    border-bottom: 1px dashed #dfdfdf;
    margin-top: 15px;
    padding: 0;
}
h3.hd3 {
    font-size: 14px;
    margin: 10px 0 5px;
}
.text-grey {
    color: #999999;
    font-size: 11px;
}
input[type="text"] { border-radius:5px;}
textarea, input[type="text"] {
    border: 1px solid #c9c9c9;
    color: #858585;
    font-family: "Open Sans",sans-serif,Arial;
    font-size: 12px;
    padding: 5px;
}
 input[type='radio'].nocursor { cursor: not-allowed;  }
</style>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <input type="hidden" id="hdnIsPageFromCache" runat="server" />
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
        <div class="clear"></div>
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/MyCarwale/default.aspx">My CarWale</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/mycarwale/myinquiries/mysellinquiry.aspx">My Inquiries</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Car(s) Listed For Sale</li>
                        </ul>
                        <div class="clear"></div>
                    </div>                   
                    <h1 class="font30 text-black special-skin-text">Edit your car details</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                 <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-10">
		                <div class="">
                            <div>
	                            <div class="gray-block clear-margin">
		                            <h3 class="hd3">Necessary Car Details</h3>
		                            <div id="alertObj" class="alert moz-round" runat="server" style="display:none;"></div>
		                            <table class="tbl-std" width="50%" border="0" cellpadding="0" cellspacing="0">	 	
		                              <tr>
			                            <th width="280">Make-Model</th>
			                            <td><asp:Label ID="lblMake" CssClass="lblFont" runat="server"></asp:Label> <asp:Label ID="lblModel"  runat="server"></asp:Label></td>
		                              </tr>	 
		                              <tr>
			                            <th>Version <font color="red">*</font></th>
			                            <td>
                                            <div class="form-control-box">
                                                <span class="select-box fa fa-angle-down"></span>
                                                <asp:DropDownList ID="cmbVersion" class="form-control" runat="server">
				                                <asp:ListItem Value="0" Text="--Select--" />
			                                    </asp:DropDownList>
                                            </div>
			                              <span id="spnVersion" class="error" />
			                            </td>
		                              </tr>
		                              <tr>
			                            <th>Make-Year <font color="red">*</font></th>
			                            <td>
                                            <Vspl:Calender DateId="calMakeYear" id="calMakeYear" MonthYear="true" runat="server" />
                                             <span id="spnMakeYear" class="error" /> 
			                            </td>		 
		                              </tr>
                                    <tr>
                                        <th>Registration Type<font color="red">*</font></th>
                                        <td>
                                            <div class="form-control-box">
                                            <span class="select-box fa fa-angle-down"></span>
                                                <asp:DropDownList ID="drpCarRegistrationType" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="0" Text="--Select Registration Type--" Selected="True"></asp:ListItem>
				                                    <asp:ListItem Value="1" Text="Individual" />
				                                    <asp:ListItem Value="2" Text="Corporate"/>
                                                    <asp:ListItem Value="3" Text="Taxi"/>
				                                </asp:DropDownList>
                                           </div>
                                            <span id="spnRegistrationType" class="error" />
                                        </td>
                                    </tr>
		                              <tr>
			                            <th>Registration No</th>
			                            <td><asp:TextBox ID="txtRegistrationNo" MaxLength="15" runat="server" CssClass="text form-control"/><span id="spnReg" class="error" /></td>
		                              </tr>
		                              <tr>
			                            <th>Kilometers <font color="red">*</font></th>
			                            <td><asp:TextBox ID="txtKilometers" MaxLength="8" runat="server" CssClass="text form-control" /> <span id="spnKm" class="error" /></td>		 
		                              </tr>
		                              <tr>
			                            <th>Expected Price <font color="red">*</font></th>
			                            <td><asp:TextBox ID="txtPrice" MaxLength="9" runat="server" CssClass="text form-control" /> <span id="spnPrice" class="error" /></td>		 
		                              </tr>
		                              <tr>
			                            <th>Color <font color="red">*</font></th>
			                            <td>
				                            <input type="hidden" id="hdnColor" runat="server" />
				                             <asp:TextBox ID="txtColor" runat="server" CssClass="text form-control" MaxLength="15"></asp:TextBox> <span id="spnColor" class="error" ></span>
			                            </td>		
		                              </tr>
                                        <tr>
			                            <th width="171">Registration Place</th>
			                            <td><asp:TextBox ID="txtRegistrationPlace" runat="server" CssClass="text form-control" MaxLength="25"/> <span id="spnRegPlace" class="error" /></td>		 
		                              </tr>		                            
		                              <tr>
			                            <th>No of Owners<font color="red">*</font></th>
			                            <td>
                                            <div class="form-control-box">
                                            <span class="select-box fa fa-angle-down"></span>
                                                <asp:DropDownList ID="drpOwners" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="0" Text="--Select Owners--" Selected="True"></asp:ListItem>
				                                <asp:ListItem Value="1" Text="I'm the First owner" />
				                                <asp:ListItem Value="2" Text="I'm the Second owner"/>
				                                <asp:ListItem Value="3" Text="I'm the Third owner" />
				                                <asp:ListItem Value="8" Text="Four or More owners" />
				                                </asp:DropDownList>
                                           </div>
                                            <span id="spnOwners" class="error" />
			                            </td>
		                              </tr>
		                              <tr style="display:none;">
			                            <th>One Time Tax</th>
			                            <td>
                                            <div class="form-control-box">
                                                <span class="select-box fa fa-angle-down"></span>
                                                <asp:DropDownList ID="drpOneTimeTax" runat="server" CssClass="form-control">
				                                <asp:ListItem Value="Individual" Text="Individual" />
				                                <asp:ListItem Value="Corporate" Text="Corporate" />
			                                    </asp:DropDownList>
                                           </div>
			                            </td>
		                              </tr>
		                              <tr>
			                            <th>Insurance</th>
			                            <td>
                                            <div class="form-control-box">
                                             <span class="select-box fa fa-angle-down"></span>
                                                <asp:DropDownList ID="drpInsurance" runat="server" CssClass="form-control">
				                                <asp:ListItem Value="N/A" Text="No Insurance" />
				                                <asp:ListItem Value="Comprehensive" Text="Comprehensive" />
				                                <asp:ListItem Value="Third Party" Text="Third Party" />
			                                    </asp:DropDownList>
                                           </div>
			                            </td>
		                              </tr>
		                              <tr id="trInsExp">
			                            <th>Insurance Expiry</th>
			                            <td><Vspl:Calender id="calInsuranceExpiry" FutureTolerance="1" runat="server" /></td>		
		                              </tr>
		                              <tr>
			                            <th>Interior Color</th>
			                            <td>
				                            <asp:TextBox ID="txtIColor" runat="server" CssClass="text form-control" MaxLength="50"></asp:TextBox>
				                            <input type="hidden" id="hdnIColor" runat="server" />
			                            </td>	
		                              </tr>
          	                            <tr>
                                            <th>Additional Fuel</th>
                                            <td>
                                                 <div class="form-control-box">
                                                  <span class="select-box fa fa-angle-down"></span>
                                                     <asp:DropDownList id="drpAdditionalFuel" runat="server" CssClass="form-control">
                                                        <asp:ListItem Selected="true" Text="--Select--" Value="-1"></asp:ListItem>
                                                        <asp:ListItem Text="CNG" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="LPG" Value="4"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </td>
                                        </tr>
		                              <tr>
			                            <th>Car mileage in city (Kmpl)</th>
			                            <td><asp:TextBox ID="txtMileage" MaxLength="5" Columns="3" runat="server" CssClass="text form-control" /> <span id="spnMileage" class="error" /></td>
		                              </tr>
		                            </table>
                                    <div class="hr-dotted"></div>
		                            <table width="100%" border="0" cellspacing="0" class="tbl-std mid-box">
		                              <tr>
			                            <td valign="top" width="250">Available Warranties (If Any)<br>
			                              <span class="text-grey">e.g. Lifetime warranty on wheels, two free services remaining etc.</span></td>
			                            <td><asp:TextBox ID="txtWarranties" TextMode="MultiLine" Rows="3" Columns="40" runat="server" />
			                              <span id="spnWarranties" class="error" /></td>
		                              </tr>		                             
		                              <tr>
			                            <td valign="top">Comments<br>
			                              <span class="text-grey">something special about your car, you want to tell your buyers!</span></td>
			                            <td><asp:TextBox ID="txtComments" TextMode="MultiLine" Rows="3" Columns="40" runat="server" />
			                              <span id="spnComments" class="error" /></td>
		                              </tr>
		                            </table>
	                            </div>
	                            <div class="mid-box"><asp:button ID="btnSave" CssClass="btn btn-orange" text="Update Sell Inquiry" runat="server" /></div>
	                            <p class="mid-box"></p>
                              </div>
                        </div>
		            </div>
	            </div>                
                <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
        <script language="javascript" type="text/javascript">
            $(document).ready(function () {               
                $(".tbl_sell,#sellcont,#mc").click(function (event) {
                    var asv = $(event.target);
                    if (asv.attr("id") == "" || asv.attr("id") == "sellcont" || asv.attr("id") == "ctl00") {
                        hideColorCombo();
                        hideIColorCombo();
                    }
                });

                if ($("#drpInsurance").val() == "N/A") {
                    $("#trInsExp").hide();
                } else {
                    $("#trInsExp").show();
                }

                $("#drpInsurance").change(function () {
                    if ($(this).val() == "N/A") {
                        $("#trInsExp").hide();
                    } else {
                        $("#trInsExp").show();
                    }
                });
            });
        </script>

    <script language="javascript">
        /* Validations */
        document.getElementById('btnSave').onclick = form_Submit;

        function form_Submit() {
            var isError = false;
            if (document.getElementById('cmbVersion').options[0].selected) {
                document.getElementById('spnVersion').innerHTML = "Select Version";
                isError = true;
            }
            else {
                document.getElementById('spnVersion').innerHTML = "";
            }

            var re = /^[0-9]*$/;
            if (document.getElementById('txtKilometers').value == "") {
                document.getElementById('spnKm').innerHTML = "Required";
                isError = true;
            }
            else if (!re.test(document.getElementById('txtKilometers').value)) {
                document.getElementById('spnKm').innerHTML = "Numbers Only";
                isError = true;
            }
            else {
                document.getElementById('spnKm').innerHTML = "";
            }

            if (document.getElementById('txtPrice').value == "") {
                document.getElementById('spnPrice').innerHTML = "Required";
                isError = true;
            }
            else if (!re.test(document.getElementById('txtPrice').value)) {
                document.getElementById('spnPrice').innerHTML = "Numbers Only";
                isError = true;
            }
            else {
                document.getElementById('spnPrice').innerHTML = "";
            }

            if (document.getElementById('txtColor').value == "") {
                document.getElementById('spnColor').innerHTML = "Required";
                isError = true;
            }
            else {
                document.getElementById('spnColor').innerHTML = "";
            }            

            var reg = /<(.|\n)*?>/g;
            if (reg.test(document.getElementById('txtComments').value)) {
                document.getElementById('spnComments').innerHTML = "Only alphabets/numbers allowed";
                isError = true;
            }
            else if (document.getElementById('txtComments').value.length > 1000) {
                document.getElementById('spnComments').innerHTML = "Maximum 1000 letters";
                isError = true;
            }
            else {
                document.getElementById('spnComments').innerHTML = "";
            }

            if (reg.test(document.getElementById('txtWarranties').value)) {
                document.getElementById('spnWarranties').innerHTML = "Only alphabets/numbers allowed";
                isError = true;
            }
            else if (document.getElementById('txtWarranties').value.length > 1000) {
                document.getElementById('spnWarranties').innerHTML = "Maximum 1000 letters";
                isError = true;
            }
            else {
                document.getElementById('spnWarranties').innerHTML = "";
            }

            var re = /^-?\d*(\.\d+)?$/;
            if (!re.test(document.getElementById('txtMileage').value)) {
                document.getElementById('spnMileage').innerHTML = "Numbers Only";
                isError = true;
            }
            else if (document.getElementById('txtMileage').value > 50) {
                document.getElementById('spnMileage').innerHTML = "Car mileage invalid.";
                isError = true;
            }
            else {
                document.getElementById('spnMileage').innerHTML = "";
            }

            if ($("#drpOwners").val() < 1) {
                $("#spnOwners").html("Owners Required");
                isError = true;
            } else {
                $("#spnOwners").html("");
            }

            if ($("#drpCarRegistrationType").val() < 1)
            {
                $("#spnRegistrationType").html("Registration Type Required");
                isError = true;
            }
            else
            {
                $("#spnRegistrationType").html("");
            }

            if (isError == true) return false;
        }
    </script>
</form>
</body>
</html>
