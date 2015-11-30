<%@ Page Language="C#" AutoEventWireup="False" Inherits="Bikewale.m.Insurance.Default" EnableViewState="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%
        title = "BikeWale - Insurance";
        keywords = "new bikes, new bikes prices, new bikes comparisons, bikes dealers, on-road price, bikes research, bikes india, Indian bikes, bike reviews, bike photos, specs, features, tips & advices";
        description = "New bikes in India. Search for the right new bikes for you, know accurate on-road price and discounts. Compare new bikes and find dealers.";
        canonical = "http://www.bikewale.com/insurance/";
     %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-newbikes.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css"/>
    <link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" type="text/css" rel="stylesheet" /> 
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/insurance.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css"/>
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-style.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css"/>
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/zebra_datepicker.css?<%= staticFileVersion %>" rel="stylesheet" />
    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->        
        <!--Add banner ends here-->
        <section class="clearfix">
          <div class="container">
            <div class="newcars-banner-div"> 
              <!-- Top banner code starts here -->
              <h2 class="font25 text-uppercase text-white text-center padding-top40">INSURANCE</h2>
            </div>
            <!-- Top banner code ends here --> 
          </div>
        </section>
        <section class="margin-minus60 clearfix">
          <div class="ins-container">
            <div class="ins-content-box"> <span class="tag-txt">Sponsored</span> 
            <span class="sponsored-icon margin-bottom25"></span>
            <img src="http://img.aeplcdn.com/bikewaleimg/images/policy-boss-logo.png" alt="Policy Boss" width="94">
              <p>Save upto 60% on <br/>
                Car Insurance Premium</p>              
            </div>
          </div>
        </section>
        <section class="grid-12 clearfix">
          <h2 class="font24 text-center margin-top20 margin-bottom20">Get your insurance quote</h2>
  
          <div id="step1" class="container bg-white content-inner-block-10 content-box-shadow rounded-corner2">
            <div class="form-control-box margin-bottom20">
              <input value="" class="form-control rounded-corner2" type="text" id="userSelectCity" data-bind="textInput: userSelectCity" placeholder="Select your city"/>
              <span style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span>
              <span class="bwmsprite error-icon hide"></span>
              <div class="bw-blackbg-tooltip hide">Please enter model name</div>
            </div>
            <p class="font14 text-medium-grey padding-left5 margin-bottom10">Select your insurance type</p>
            <div class="clearfix radioGroup margin-bottom20">
              <div class="leftfloat margin-right10">
                <input type="radio" name="insRadio" checked="checked" value="renew" />
                <label for="renewIns">Renew</label>
              </div>
              <div class="leftfloat margin-left10 margin-right10">
                <input type="radio" name="insRadio" value="expired" />
                <label for="expiredIns">Expired</label>
              </div>
              <div class="leftfloat margin-left10">
                <input type="radio" name="insRadio" value="new" />
                <label for="newIns">New</label>
              </div>
            </div>
            <div>
              <div class="form-control-box margin-bottom20">
                <input value="" class="form-control rounded-corner2 ui-autocomplete-input" type="text" id="makeName" data-bind="textInput: makeName" placeholder="Type to select Make" />
                <span style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span> <span class="bwmsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please enter make name</div>
              </div>
              <div class="form-control-box margin-bottom20">
                <input value="" class="form-control rounded-corner2 ui-autocomplete-input" type="text" id="modelName" data-bind="textInput: modelName" placeholder="Type to select Model" />
                <span style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span> 
                <span class="bwmsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please enter model name</div>
              </div>
              <div class="form-control-box margin-bottom20">
                <input value="" class="form-control rounded-corner2" type="text" id="versionName" data-bind="textInput: versionName" placeholder="Select version"/>
                <span style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span> 
                <span class="bwmsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please enter version name</div>
              </div>
              <div class="form-control-box car-month-year margin-top20 main-select-div font14 form-control" style="padding: 0;" id="divRegDate">
                <input name="txtRegDate" id="bikeRegistrationDate" type="text" placeholder="Select registration date" />
                <span style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span> 
                <span class="bwmsprite error-icon hide"></span>
                <div class="bw-blackbg-tooltip hide">Please select Date</div>
              </div>
              <div class="margin-top20 text-center">
                <input type="button" id="insuranceDetailsBtn" value="Next" class="btn btn-full-width btn-orange" onclick="nextSection()"/>
              </div>
              <input type="hidden" id="hdnCityId" />
              <input type="hidden" id="hdnMakeId" />
              <input type="hidden" id="hdnModelId" />
              <input type="hidden" id="hdnVersionId" />
              <input type="hidden" id="hdnClientPrice" />
            </div>
          </div>
  
          <div id="step2" class="container bg-white content-inner-block-10 content-box-shadow rounded-corner2 hide">
            <h2 class="text-center font20 margin-bottom15">Enter your personal details</h2>
            <div class="form-control-box margin-bottom20">
              <input value="" class="form-control rounded-corner2" type="text" id="customerName" data-bind="textInput: customerName" placeholder="Name"/>
              <span style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span> 
              <span class="bwmsprite error-icon hide"></span>
              <div class="bw-blackbg-tooltip hide">Please enter Name</div>  
            </div>
            <div class="form-control-box margin-bottom20">
              <input value="" class="form-control rounded-corner2" type="text" id="mobile" maxlength="10" data-bind="textInput: mobile" placeholder="Mobile no"/>
              <span style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span>
              <span class="bwmsprite error-icon hide"></span>
              <div class="bw-blackbg-tooltip hide">Please enter valid 10 digit Mobile</div>  
            </div>
            <div class="form-control-box margin-bottom20">
              <input value="" class="form-control-box form-control rounded-corner2" type="text" id="email" data-bind="textInput: email" placeholder="Email id"/>
              <span style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span> 
              <span class="bwmsprite error-icon hide"></span>
              <div class="bw-blackbg-tooltip hide">Please enter Email</div>  
            </div>
            <div class="margin-top20 text-center">
              <input type="button" value="Submit" class="btn btn-full-width btn-orange" data-bind="click: saveUserDetail"/>
            </div>
            <p class="font12 text-medium-grey margin-top15"> By continuing you accept that we or our partner, 
              Cholamandalam MS General Insurance Company Ltd. may contact you regarding your enquiry. </p>
          </div>
          <div id="responseContainer" class="text-center hide"><!-- responseContainer -->
                	<p class="font18 margin-top10 text-bold">Thank You!</p>
                    <p class="font16">Hi <span id="responseUserName"></span>! <br>
                    Thank you for your interest in our Insurance policy. You can expect a call back soon from our Excutives regarding the same. We assure you, your details will not shared with anyone.
                    </p>
          </div>
        </section>
        <section class="container margin-bottom20">
          <div class="grid-12 faqs">
            <h2 class="text-center margin-top30 margin-bottom20">FAQs</h2>
            <div id="faqs" class="grid-12 bg-white content-box-shadow rounded-corner2 clear">
              <div class="text-black box accordion">
                <h3 class="stepStrip">What is bike insurance? <span class="icon plus-minus"><span class="fa fa-plus"></span></span></h3>
                <div class="hide">
                  <p class="content-inner-block-4 font14 text-grey line-height"> Car insurance policy is mandatory for car 
                    owners as per Indian Motor Vehicles Act 
                    1988, so select it wisely. This Plan is 
                    designed to give coverage for losses 
                    which insured might incur in case his 
                    vehicle gets stolen or damaged. 
                    The amount of motor insurance premium 
                    is decided based on the Insured Declared 
                    Value of a car. The premium will increase, 
                    if you raise the IDV limit and vice versa. </p>
                </div>
              </div>
              <div class="text-black box accordion">
                <h3 class="stepStrip">Is it mandatory to take insurance<span class="icon plus-minus"><span class="fa fa-plus"></span></span></h3>
                <div class="hide">
                  <p class="content-inner-block-4 font14 text-grey line-height"> As per law, it's mandatory to have third-
                    party insurance for your car. Opting for a 
                    comprehensive policy is a matter of choice. 
                    CarWale recommends you a comprehensive
                    policy, though. </p>
                </div>
              </div>
              <div class="text-black box accordion">
                <h3 class="stepStrip">What is NCB?<span class="icon plus-minus"><span class="fa fa-plus"></span></span></h3>
                <div class="hide">
                  <p class="content-inner-block-4 font14 text-grey line-height"> Comprehensive car insurance not only 
                    protects you against third party liabilities 
                    but also helps you avoid unnecessary 
                    expenses that might occur due to accidents 
                    or theft. </p>
                </div>
              </div>
              <div class="text-black box accordion">
                <h3 class="stepStrip">Can I get zero dep insurance?<span class="icon plus-minus"><span class="fa fa-plus"></span></span></h3>
                <div class="hide">
                  <p class="content-inner-block-4 font14 text-grey line-height"> It is a benefit offered by the insurer to 
                    those who have not claimed insurance 
                    during the previous year of cover. It means 
                    that the next premium amount to be paid 
                    would be lower. </p>
                </div>
              </div>
              <div class="text-black box accordion">
                <h3 class="stepStrip">What documents will I need?<span class="icon plus-minus"><span class="fa fa-plus"></span></span></h3>
                <div class="hide">
                  <p class="content-inner-block-4 font14 text-grey line-height"> It is a benefit offered by the insurer to 
                    those who have not claimed insurance 
                    during the previous year of cover. It means 
                    that the next premium amount to be paid 
                    would be lower. </p>
                </div>
              </div>
            </div>
          </div>
          <div class="clear"></div>
        </section>

        <section class="container contact-fixed">
          <div class="grid-12 text-white text-center">
            <p class="margin-bottom25 font14 text-medium-grey text-center">In case of queries call us toll-free on:</p>
            <p class="phone-box"><a href="tel:1800 457 9781" class="rounded-corner2" style="text-decoration: none;"><span class="fa fa-phone"></span>1800 457 9781 </a></p>
          </div>
          <div class="clear"></div>
        </section>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/bwm-newbikes.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/chosen-jquery-min-mobile.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/m/src/insurance.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/m/src/zebra_datepicker.js?<%= staticFileVersion %>"></script>
        
        <script type="text/javascript" >
            <% var serializer = new System.Web.Script.Serialization.JavaScriptSerializer(); %>
            var cities = <%= serializer.Serialize(cityList) %>;
            var makes = <%= serializer.Serialize(makeList) %>;
            insauranceModel.leadSourceId = <%= Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["mobileSourceId"]) %>;
            cityAutoComplete();
            makeAutoComplete();
            versionAutoSuggest();       
        </script>
    </form>
</body>
</html>
