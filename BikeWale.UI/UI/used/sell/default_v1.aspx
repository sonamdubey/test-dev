<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Used.AboutBike" Trace="false" Debug="false" EnableEventValidation="false"  %>
<%@ Register TagPrefix="BikeWale" TagName="Calender" Src="~/controls/DateControl.ascx" %>
<%
    title = "Sell Bike | Sell Used Bike in India - BikeWale";
    description = "Sell Your Used / pre-owned bike at bikewale.com. Selling at bikewale.com is easy, quick, effective and guaranteed.";
    keywords = "sell bike, bike sale, used bike sell, second-hand bike sell, sell bike India, list your bike";
    AdId = "1475577527140";
    AdPath = "/1017752/BikeWale_UsedSellBikes_";
    isAd300x250Shown= true;
    isAd300x250BTFShown = false;
    isAd970x90Shown = true;
%>
<!-- #include file="/includes/headSell.aspx" -->
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/src/classified/sellbike.js?<%= staticFileVersion %>"></script>
<div class="container_12">
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
                <a href="/used/" itemprop="url">
                    <span itemprop="title">Used Bikes</span>
                </a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>Sell Your Bike</strong></li>
        </ul><div class="clear"></div>
    </div>
    <%--<div id="div_NotAuthorised" runat="server" class="grid_8 min-height margin-top10"></div>--%>
    <div class="grid_8 margin-top10" ><!--    Left Container starts here -->
        <div id="div_NotAuthorised" runat="server" class="min-height">
            <h1>Sell Your Bike - Easy & Fast</h1>
            <h3 class="grey-bg border-light padding5 margin-top15">You are not authorized to edit this listing.</h3>
        </div>
        <div id="div_FakeCustomer" runat="server" class="min-height">
            <h1>Sell Your Bike - Easy & Fast</h1>
            <h3 class="grey-bg border-light padding5 margin-top15 isfake">You are not authorized to add any listing. Please contact us on <u>contact@bikewale.com</u></h3>
        </div>
        <div id="div_sellBike" runat="server">
            <h1>Sell Your Bike - Easy & Fast</h1>
            <p class="desc-para">All <span class="required">*</span> marked fields are required</p>
		    <div id="alertObj" class="bg-hilight moz-round" runat="server" style="display:none; margin-bottom:10px;">Please fill all the required fields.</div>
            <h2>About Your Bike</h2>
            <table id="tbl-default" border="0" width="100%" cellspacing="0" class="tbl-default margin-top10">
                <tr>
                    <th width="130" id="bike">Your Bike<span class="required">*</span></th>
                    <td>
                        <asp:dropdownlist id="drpMake" runat="server" width="150">
					        <asp:ListItem Selected="true" Text="--Select Make--"></asp:ListItem>
				        </asp:dropdownlist>
                        <asp:dropdownlist id="drpModel" runat="server" width="150" enabled="false">
					        <asp:ListItem Selected="true" Text="--Select Model--"></asp:ListItem>
				        </asp:dropdownlist>
                        <input type="hidden" id="hdn_drpModel" runat="server" />
                        <input type="hidden" id="hdn_drpModelName" runat="server" />
                        <asp:dropdownlist id="drpVersion" runat="server" width="160" enabled="false">
					        <asp:ListItem Selected="true" Text="--Select Version--"></asp:ListItem>					
				        </asp:dropdownlist>
                        <span id="msgYourBike" runat="server" class="error"></span>
                        <input type="hidden" id="hdn_drpVersion" runat="server" />
                        <input type="hidden" id="hdn_drpSelectedVersion" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th id="make-yr">Make Year<span class="required">*</span></th>
                    <td>
                        <BikeWale:Calender DateId="calMakeYear" ID="calMakeYear" MonthYear="true" runat="server" />
                        <span id="msgMakeYear" runat="server" class="error"></span></td>
                </tr>
                <tr>
                    <th id="owner">Owner<span class="required">*</span></th>
                    <td>
                        <asp:dropdownlist id="drpOwner" runat="server">
					        <asp:ListItem Value="0" Text="-- Select --" Selected="true" />
					        <asp:ListItem Value="1" Text="I bought it New" />
					        <asp:ListItem Value="2" Text="I'm the Second owner"/>
					        <asp:ListItem Value="3" Text="I'm the Third owner" />
					        <asp:ListItem Value="4" Text="I'm the Fourth owner" />
					        <asp:ListItem Value="5" Text="Four or more previous owners" />						
				        </asp:dropdownlist>
                        <span id="msgOwner" runat="server" class="error"></span>
                    </td>
                </tr>
                <tr>
                    <th id="color">Colour<span class="required">*</span></th>
                    <td>
                        <div class="hide">
                            <input type="hidden" id="hdnBikeColor" runat="server" />
                            <div id="comboColors" style="display: inline-block;" title="Choose color of your bike">
                                <div id="colorPreview"></div>
                                <img id="drpColorImg" style="padding: 2px 2px 0 0;" src="http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/drop.png" border="0" align="right" /><div id="selectedColor" runat="server" style="padding: 5px;">-- Select Color --</div>
                            </div>
                            <div id="colors">
                                <ul class="ul-normal">
                                    <asp:repeater id="rptColors" runat="server">
							    <itemtemplate><li><span class="color-bg" style="background-color:#<%# DataBinder.Eval( Container.DataItem, "HexCode") %>;"></span><div id="color-name" class="color-item"><%# DataBinder.Eval( Container.DataItem, "ColorName") %></div><div class="color-code" style="display:none;"><%# DataBinder.Eval( Container.DataItem, "HexCode") %></div></li>
							    </itemtemplate>
						    </asp:repeater>
                                </ul>
                            </div>
                        </div>
                        <asp:textbox id="txtColor" runat="server" cssclass="text" MaxLength="20"></asp:textbox>
                        <span id="msgBikeColor" runat="server" class="error"></span>
                    </td>
                </tr>
                <tr>
                    <th id="kms">Kms Done<span class="required">*</span></th>
                    <td>
                        <asp:textbox id="txtKms" cssclass="text" runat="server" tooltip="How many kilometers you have driven your bike." MaxLength="7"></asp:textbox>
                        <span id="msgKms" runat="server" class="error"></span></td>
                </tr>
                <tr>
                    <th>City<span class="required">*</span></th>
                    <td>
                        <asp:dropdownlist id="drpStates" runat="server" width="180"></asp:dropdownlist>
                        <asp:dropdownlist id="drpCities" runat="server" width="180" enabled="false">
					    <asp:ListItem Selected="true" Text="--Select City--"></asp:ListItem>				
				    </asp:dropdownlist>
                        <span id="msgCity" runat="server" class="error"></span>
                        <input type="hidden" id="hdn_drpCities" runat="server" />
                        <input type="hidden" id="hdn_drpSelectedCity" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th id="reg-no">Registration No.<span class="required">*</span></th>
                    <td>
                        <asp:textbox id="txtRegNo" cssclass="text" runat="server"  MaxLength="20"></asp:textbox>
                        <span id="msgRegNo" runat="server" class="error"></span></td>
                </tr>
                <tr>
                    <th id="reg-at">Registered At<span class="required">*</span></th>
                    <td>
                        <asp:textbox id="txtRegAt" cssclass="text" runat="server"  MaxLength="30"></asp:textbox>
                        <span id="msgRegAt" runat="server" class="error"></span></td>
                </tr>
                <tr>
                    <th id="tax">Lifetime Tax<span class="required">*</span></th>
                    <td>
                        <asp:radiobutton id="btnTaxI" runat="server" text="Individual" groupname="Tax"></asp:radiobutton>
                        &nbsp;&nbsp;
				        <asp:radiobutton id="btnTaxC" runat="server" text="Corporate" groupname="Tax"></asp:radiobutton>
                        <span id="msgLifeTax" runat="server" class="error"></span>
                    </td>
                </tr>
                <tr>
                    <th id="ins">Bike Insurance<span class="required">*</span></th>
                    <td>
                        <asp:radiobutton id="rdoComprehensive" runat="server" text="Comprehensive" groupname="Insurance"></asp:radiobutton>
                        &nbsp;&nbsp;
				    <asp:radiobutton id="rdoThirdParty" runat="server" text="Third Party" groupname="Insurance"></asp:radiobutton>
                        &nbsp;&nbsp;
				    <asp:radiobutton id="rdoNoInsurance" runat="server" text="No Insurance" groupname="Insurance"></asp:radiobutton>
                        <span id="msgBikeIns" runat="server" class="error"></span>
                    </td>
                </tr>
                <tr id="valid-till" class="<%=rdoNoInsurance.Checked == true ? "hide" : "show" %>">
                    <th>Insurance Valid Till</th>
                    <td>
                        <BikeWale:Calender DateId="calValidTill" ID="calValidTill" MonthYear="true" runat="server" FutureTolerance="1" />
                        <span id="msgValidTill" runat="server" class="error"></span></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="hr-dotted clear-margin"></div>
                    </td>
                </tr>
                <tr>
                    <th id="price">Expected Price<span class="required">*</span> (Rs.)</th>
                    <td>
                        <asp:textbox id="txtPrice" cssclass="text" runat="server" MaxLength="7"></asp:textbox>
                        <span id="msgPrice" runat="server" class="error"></span>
                        <div id="cw-valuation" class="blue-block hide margin-top10">
                            <p class="price2">BikeWale Valuation</p>
                            <table class="tblInternal" width="100%" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td width="140">Excellent Condition</td>
                                    <td><span id="excellent" class="price2"></span></td>
                                </tr>
                                <tr>
                                    <td>Good Condition</td>
                                    <td><span id="good" class="price2"></span></td>
                                </tr>
                                <tr>
                                    <td>Fair Condition</td>
                                    <td><span id="fair" class="price2"></span></td>
                                </tr>
                            </table>
                            <p class="text-grey">This value is only indicative and should not be treated as actual. Actual bike value would depend on factors like bike condition, accessories, tyres, etc.</p>
                        </div>

                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="hr-dotted clear-margin"></div>
                    </td>
                </tr>
               
                <tr>
                    <th style="vertical-align:top;">Your Comments <br /><span style="font-weight:normal;">(Max limit is 250 characters)</span></th>
                    <td>
                        <asp:textbox id="txtComments" cssclass="text" runat="server" rows="2" columns="60" textmode="MultiLine" text="Your Comments" onfocus="if (this.value == 'Your Comments') { this.value=''; }" onblur="if (this.value == '') { this.value='Your Comments'; }" ></asp:textbox>
                        <p class="text-grey">Your comments are optional. Use this space to tell buyers in brief why do you wish to sell this bike & what is so special about this bike.</p>
                        <span id="errComments" class="required"></span>
                    </td>
                </tr>
                <tr>
                    <th>Available Warranties <br />(If Any)</th>
                    <td>
                        <asp:textbox id="txtWaranties" cssclass="text" runat="server" rows="2" columns="60"  textmode="MultiLine"></asp:textbox>
                        <p class="text-grey">If you have any warranty card of bike spare part. </p>
                        <span id="errWarranties" class="required"></span>
                    </td>
                </tr>
                <tr>
                    <th>Major Modifications <br />(If Any)</th>
                    <td>
                        <asp:textbox id="txtModifications" cssclass="text" runat="server" rows="2" columns="60" textmode="MultiLine"></asp:textbox>
                        <p class="text-grey">If you have changed any spare part of your bike.</p>
                        <span id="errModification" class="required"></span>
                    </td>
                </tr>
            </table>       
            <div id="div_AboutYou" runat="server">
                <h2>About You</h2>
                <table width="100%" cellspacing="0" class="tbl-default" border="0">
                    <tr>
                        <th width="130">Your Name<span class="required">*</span></th>
                        <td>
                            <asp:textbox id="txtName" runat="server" cssclass="text" columns="40" MaxLength="40"></asp:textbox>
                            <span id="msgName" runat="server" class="error"></span></td>
                    </tr>
                    <tr>
                        <th>Email Address<span class="required">*</span></th>
                        <td>
                            <asp:textbox id="txtEmail" runat="server" cssclass="text" columns="40" MaxLength="40"></asp:textbox>
                            <span id="msgEmail" runat="server" class="error"></span></td>
                    </tr>
                    <tr>
                        <th>Mobile Number<span class="required">*</span></th>
                        <td>
                            <asp:textbox id="txtMobile" runat="server" cssclass="text" MaxLength="10"></asp:textbox>
                            <span id="msgMobile" runat="server" class="error"></span>
                            <p class="text-grey">SMS responses from prospective buyers will be sent on this number.</p>
                        </td>
                    </tr>
                    <tr>
                        <th>&nbsp;</th>
                        <td>
                            <asp:checkbox id="chkTerms" runat="server" text=" I agree with BikeWale sell bike"></asp:checkbox>
                            <a onclick="window.open('/TermsConditions.aspx','termsListing','address=no,scrollbars=yes,width=750,height=550')">Terms & Conditions</a>, <a target="_blank" href="/visitoragreement.aspx">visitor agreement</a> and <a target="_blank" href="/privacypolicy.aspx">privacy policy</a><span class="required">&nbsp;*</span><p id="msgTerms" runat="server" class="error"></p>
                            <p class="margin-top10">I agree that by clicking the 'Continue' button below I am explicitly soliciting a call from BikeWale on my 'Mobile Number' provided above to assist me in completing this transaction.</p>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <div>
                                <asp:button id="btnContinue" runat="server" cssclass="action-btn text_white" text="Save & Continue"></asp:button>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        <%--    <div id="div_Update" class="<%=inquiryId == "-1" ? "hide" : "show" %> margin-top20">
                <asp:button id="btnUpdate" runat="server" cssclass="buttons" text="Update" style="padding:5px 8px;"></asp:button>
            </div>--%>
		    <%--<input type="hidden" id="fairValue" runat="server" />
            <input type="hidden" id="goodValue" runat="server" />
            <input type="hidden" id="excellentValue" runat="server" />	              --%>
        </div>        
    </div><!--    Left Container ends here -->
    <div class="grid_4 margin-top15"><!--    Right Container starts here -->       
        <!-- BikeWale_SellBike/BikeWale_SellBike_300x250 -->
        <!-- #include file="/ads/Ad300x250.aspx" -->
    </div><!--    Right Container ends here -->
</div>
<script type="text/javascript">
    $(document).ready(function () {

        inquiryId = '<%= inquiryId%>';
        drpModel = $("#drpModel");
        drpVersion = $("#drpVersion");
        drpCities = $("#drpCities");

        $("#btnContinue").click(function () {
            var isError = false;
            
            if (!valBikeDetails())
                isError = true;
            
            if (!validateUserDetails())
                isError = true;

            if (isError) return false;
        });

        $("#btnUpdate").click(function () {
            var isError = false;

            if (!valBikeDetails())
                isError = true;

            if (isError) return false;
        });

        $("#drpMake").change(function () {
            drpMake_Change($(this));
        });

        $("#drpModel").change(function () {
            drpModel_Change($(this));
        });

        $("#rdoNoInsurance").click(function () {
            if (!$("#valid-till").hasClass("hide"))
            {
                $("#valid-till").addClass("hide");
            }
        });

        $("#rdoThirdParty,#rdoComprehensive").click(function () {
            $("#valid-till").removeClass("hide");
        });

        $("#txtKms,#calMakeYear_txtYear").blur(function () {
            //requestValuation();
        });

        $("#drpVersion").change(function () {
            if ($(this).val() != "0") {
                $("#hdn_drpSelectedVersion").val($(this).val() + "|" + $(this).find("option:selected").text());
            } else {
                $("#hdn_drpSelectedVersion").val("");
            }
            //requestValuation();
        });

        $("#drpStates").change(function () {
            drpState_Change($(this));
        });

        $("#drpCities").change(function () {
            //drpCity_Change();
            if ($(this).val() != "0") {
                $("#hdn_drpSelectedCity").val($(this).val() + "|" + $(this).find("option:selected").text());
            } else {
                $("#hdn_drpSelectedCity").val("");
            }
        });

        if (inquiryId == "-1") {            
            maintainFormState();
        }

        var MaxLength = 250;
        $('#txtComments').keypress(function (e) {
            if ($(this).val().length >= MaxLength && e.keyCode !== 8 && e.keyCode !== 46 && (e.keyCode < 37  || e.keyCode > 40)) {
                e.preventDefault();
            }
        });

        $('#txtWaranties').keypress(function (e) {
            if ($(this).val().length >= MaxLength && e.keyCode !== 8 && e.keyCode !== 46 && (e.keyCode < 37 || e.keyCode > 40)) {
                e.preventDefault();
            }
        });


        $('#txtModifications').keypress(function (e) {
            if ($(this).val().length >= MaxLength && e.keyCode !== 8 && e.keyCode !== 46 && (e.keyCode < 37 || e.keyCode > 40)) {
                e.preventDefault();
            }
        });
      
    }); // End of document.ready
</script>
<!-- #include file="/includes/footerInner.aspx" -->
