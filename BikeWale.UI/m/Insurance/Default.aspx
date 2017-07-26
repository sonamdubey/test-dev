<%@ Page Language="C#" AutoEventWireup="False" Inherits="Bikewale.Mobile.Insurance.Default"  %>

<!DOCTYPE html>

<html>
<head>
    <%
        title = "BikeWale - Insurance";
        keywords = "new bikes, new bikes prices, new bikes comparisons, bikes dealers, on-road price, bikes research, bikes india, Indian bikes, bike reviews, bike images, specs, features, tips & advices";
        description = "New bikes in India. Search for the right new bikes for you, know accurate on-road price and discounts. Compare new bikes and find dealers.";
        canonical = "https://www.bikewale.com/m/insurance/";
     %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
     <link href="<%= staticUrl  %>/m/css/insurance.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css"/>
    <link href="<%= staticUrl  %>/m/css/zebra_datepicker.css?<%= staticFileVersion %>" rel="stylesheet" />
</head>
<body class="bg-light-grey">
    <form id="form1" runat="server">
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
                <div class="ins-content-box">
                    <span class="tag-txt">Sponsored</span>
                    <span class="sponsored-icon margin-bottom25"></span>
                    <img src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/d/policy-boss-logo.png" alt="Policy Boss" width="94">
                    <p>
                        Save upto 60% on
                        <br />
                        Bike Insurance Premium
                    </p>
                </div>
            </div>
        </section>
        <section id="insuranceQuote" class="grid-12 clearfix">
            <h2 class="font24 text-center margin-top20 margin-bottom20">Get an insurance quote</h2>

            <div id="step1" class="container bg-white content-inner-block-10 content-box-shadow rounded-corner2">
                <div class="form-control-box margin-bottom20">
                    <input value="" class="form-control rounded-corner2" type="text" id="userSelectCity" tabindex="1" data-bind="textInput: userSelectCity" placeholder="Select your city" />
                    <span style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span>
                    <span class="bwmsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please enter city name</div>
                </div>
                <p class="font14 text-medium-grey padding-left5 margin-bottom10">Select your insurance type</p>
                <div class="clearfix radioGroup margin-bottom20">
                    <div class="leftfloat margin-right10">
                        <input type="radio" id="renewIns" name="insRadio" checked="checked" value="renew" />
                        <label for="renewIns">Renew</label>
                    </div>
                    <div class="leftfloat margin-left10 margin-right10">
                        <input type="radio" id="expiredIns" name="insRadio" value="expired" />
                        <label for="expiredIns">Expired</label>
                    </div>
                    <div class="leftfloat margin-left10">
                        <input type="radio" id="newIns" name="insRadio" value="new" />
                        <label for="newIns">New</label>
                    </div>
                </div>
                <div>
                    <div class="form-control-box margin-bottom20">
                        <input value="" class="form-control rounded-corner2 ui-autocomplete-input" type="text" id="makeName" tabindex="2" data-bind="textInput: makeName" placeholder="Type to select Make" />
                        <span style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span><span class="bwmsprite error-icon hide"></span>
                        <div class="bw-blackbg-tooltip hide">Please enter make name</div>
                    </div>
                    <div class="form-control-box margin-bottom20">
                        <input value="" class="form-control rounded-corner2 ui-autocomplete-input" type="text" id="modelName" tabindex="3" data-bind="textInput: modelName" placeholder="Type to select Model" />
                        <span id="modelLoader" style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span>
                        <span class="bwmsprite error-icon hide"></span>
                        <div class="bw-blackbg-tooltip hide">Please enter model name</div>
                    </div>
                    <div class="form-control-box margin-bottom20">
                        <input value="" class="form-control rounded-corner2" type="text" id="versionName" tabindex="4" data-bind="textInput: versionName" placeholder="Select version" />
                        <span id="versionLoadaer" style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span>
                        <span class="bwmsprite error-icon hide"></span>
                        <div class="bw-blackbg-tooltip hide">Please enter version name</div>
                    </div>
                    <div class="form-control-box car-month-year margin-top20 main-select-div font14 form-control" style="padding: 0;" id="divRegDate">
                        <input name="txtRegDate" id="bikeRegistrationDate" type="text" tabindex="5" placeholder="Select registration date" />
                        <span style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span>
                        <span class="bwmsprite error-icon hide"></span>
                        <div class="bw-blackbg-tooltip hide">Please select Date</div>
                    </div>
                    <div class="margin-top20 text-center">
                        <input type="button" id="insuranceDetailsBtn" value="Next" class="btn btn-full-width btn-orange" tabindex="6" onclick="nextSection()" />
                    </div>
                </div>
            </div>

            <div id="step2" class="container bg-white content-inner-block-10 content-box-shadow rounded-corner2 hide">
                <h2 class="text-center font20 margin-bottom15">Enter your personal details</h2>
                <div class="form-control-box margin-bottom20">
                    <input value="" class="form-control rounded-corner2" type="text" id="customerName" tabindex="7" data-bind="textInput: customerName" placeholder="Name" />
                    <span style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span>
                    <span class="bwmsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please enter name</div>
                </div>
                <div class="form-control-box margin-bottom20">
                    <input value="" class="form-control rounded-corner2" type="text" id="mobile" maxlength="10" tabindex="8" data-bind="textInput: mobile" placeholder="Mobile no" />
                    <span style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span>
                    <span class="bwmsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please enter valid 10 digit mobile</div>
                </div>
                <div class="form-control-box margin-bottom20">
                    <input value="" class="form-control-box form-control rounded-corner2" type="text" id="email" tabindex="9" data-bind="textInput: email" placeholder="Email id" />
                    <span style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span>
                    <span class="bwmsprite error-icon hide"></span>
                    <div class="bw-blackbg-tooltip hide">Please enter email</div>
                </div>
                <div class="margin-top20 text-center">
                    <input type="button" value="Submit" class="btn btn-full-width btn-orange" tabindex="10" data-bind="click: saveUserDetail" />
                </div>
                <p class="font12 text-medium-grey margin-top15">By continuing you accept that we or our partner Policy Boss may contact you regarding your enquiry. </p>
            </div>
            <div id="responseContainer" class="text-center container bg-white content-inner-block-10 content-box-shadow  hide">
                <!-- responseContainer -->
                <p class="font18 margin-top10 text-bold">Thank You!</p>
                <p class="font14">
                    Hi <span id="responseUserName"></span>!
                    <br/>
                    Thank you for expressing interest. You will get a call from our partner, PolicyBoss (Landmark Insurance Brokers), with exciting insurance offers.
                 </p>
            </div>
        </section>
        <section class="container margin-bottom20">
            <div class="grid-12 faqs">
                <h2 class="text-center margin-top30 margin-bottom20">FAQs</h2>
                <div id="faqs" class="grid-12 bg-white content-box-shadow rounded-corner2 clear">
                    <div class="text-black box accordion">
                        <h3 class="stepStrip">What is bike insurance? <span class="icon plus-minus"><span class="bwmsprite fa fa-plus"></span></span></h3>
                        <div class="hide">
                            <p class="content-inner-block-4 font14 text-grey line-height">
                                Bike or Two Wheeler Insurance extends protection against theft or any damage to your bike. It also safeguards your bike against third party liabilities. A comprehensive policy not only provides coverage in accidental injuries but also pays specified compensation for death sustained by the owner & driver of the vehicle in direct connection with the vehicle insured or while mounting into / dismounting from or traveling in the insured vehicle as a co-driver, caused by violent, accidental, external and visible means.
                            </p>
                        </div>
                    </div>
                    <div class="text-black box accordion">
                        <h3 class="stepStrip">Is it mandatory to take insurance<span class="icon plus-minus"><span class="bwmsprite fa fa-plus"></span></span></h3>
                        <div class="hide">
                            <p class="content-inner-block-4 font14 text-grey line-height">
                                Two wheeler insurance is a mandate thing as per the law and much needed to safeguard your passengers, your fellow drivers, other people's property, pedestrians, yourself and your beloved bike.
                            </p>
                        </div>
                    </div>
                    <div class="text-black box accordion">
                        <h3 class="stepStrip">What does my bike insurance cover?<span class="icon plus-minus"><span class="bwmsprite fa fa-plus"></span></span></h3>
                        <div class="hide">
                            <p class="content-inner-block-4 font14 text-grey line-height">
                                Following things get covered in your two wheeler insurance:
                            	<ul class="faqBox">
                                    <li>Normal wear-and-tear of the vehicle
                                    </li>
                                    <li>Mechanical and electrical breakdown
                                    </li>
                                    <li>Vehicle being used other than in accordance with the limitations as to use. For example, if you use your two-wheeler for remuneration purposes
                                    </li>
                                    <li>Damage to / by person driving without a valid driving license
                                    </li>
                                    <li>Loss or damage caused while NOT riding under the influence of alcohol or any other intoxicating substance
                                    </li>
                                    <li>Loss or damage due to depreciation of the vehicle's value
                                    </li>
                                    <li>Consequential loss - if the original damage causes subsequent damage / loss, only the original damage will be covered
                                    </li>
                                    <li>Compulsory deductibles - a fixed amount that gets deducted at the time of the claim
                                    </li>
                                </ul>
                            </p>
                        </div>
                    </div>
                    <div class="text-black box accordion">
                        <h3 class="stepStrip">What are the key features of insurance that is being offered?<span class="icon plus-minus"><span class="bwmsprite fa fa-plus"></span></span></h3>
                        <div class="hide">
                            <ul class="faqBox">
                                <li>Easy and hassle-free claim process with minimum documentation
                                </li>
                                <li>Comprehensive cover in India
                                </li>
                                <li>Exhaustive list of network garages with 24X7 roadside assistance facilities
                                </li>
                                <li>Instant renewal
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="text-black box accordion">
                        <h3 class="stepStrip">Does the bike insurance lapse after the death of the insured?<span class="icon plus-minus"><span class="bwmsprite fa fa-plus"></span></span></h3>
                        <div class="hide">
                            <p class="content-inner-block-4 font14 text-grey line-height">
                                In the event of death of the insured, the bike insurance policy will not immediately lapse but will remain valid for a period of three months from the death of insured or until the expiry of the policy (whichever is earlier). During the said period, legal heir of the insured whom the custody and the use of motor vehicle passes may apply to have this policy transferred to the name of the heir or obtain new insurance policy for bike. However, following documents are needed in such case:
                                <ul class="faqBox">
                                    <li>Death certificate of the insured
                                    </li>
                                    <li>Proof of title of the bike
                                    </li>
                                    <li>Original Policy Documents
                                    </li>
                                </ul>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <%--<section class="container contact-fixed">
            <div class="grid-12 text-white text-center">
                <p class="margin-bottom25 font14 text-medium-grey text-center">In case of queries call us toll-free on:</p>
                <p class="phone-box"><a href="tel:1800 120 8300" class="rounded-corner2" style="text-decoration: none;"><span class="bwmsprite tel-green-icon margin-right5"></span>1800 120 8300</a></p>
            </div>
            <div class="clear"></div>
        </section>--%>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl %>/m/src/insurance.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl  %>/m/src/zebra_datepicker.js?<%= staticFileVersion %>"></script>

        <script type="text/javascript">
            <% var serializer = new System.Web.Script.Serialization.JavaScriptSerializer(); %>
            var cities = <%= serializer.Serialize(cityList) %>;
            var makes = <%= serializer.Serialize(makeList) %>;
            insauranceModel.leadSourceId = <%= Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["mobileSourceId"]) %>;
            cityAutoComplete();
            makeAutoComplete();
            versionAutoSuggest();   
           

            $("#step1 input[type='text'],#step2 input[type='text']").keypress(function(e){
                if(e.keyCode==13) 
                {
                    e.preventDefault();
                    return false;
                }
            });
            
        </script>
    </form>
</body>
</html>
