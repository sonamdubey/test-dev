<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Insurance.Default" %>

<!DOCTYPE html>

<html lang="en">
<head>
    <%
         title = "BikeWale - Insurance";
         keywords = "calculate insurance premium, premium calculator, calculate insurance, insurance calculator, indian insurance calculator, bike insurance, calulate insurance in india, india insurance, motorcycle insurance";
         description = "BikeWale - India's favourite bike portal. Find new and used bikes, buy or sell your bikes, compare new bikes prices & values.";
         alternate = "https://www.bikewale.com/m/insurance/";
         canonical = "https://www.bikewale.com/insurance/"; 
    
        isAd970x90Shown = false;
        isAd970x90BottomShown = false;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
         %>
    <!-- #include file="/UI/includes/headscript.aspx" -->
    <link href="<%= staticUrl  %>/UI/css/home.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css"/>
    <link href="<%= staticUrl  %>/UI/css/insurance.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css"/>
    <link href="<%= staticUrl  %>/UI/css/pikaday.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css"/>    
        
    <%  isTransparentHeader = true;   %>
</head>
<body class="bg-light-grey">
    <form id="form1" runat="server">
        <!-- #include file="/UI/includes/headBW.aspx" -->
        <div class="clear"></div>
        <header class="insurance-top-banner">    	
            <div class="container">
        	    <div class="welcome-box">
                    <h1 class="text-uppercase margin-bottom10">Insurance</h1>
                </div>
            </div>
        </header>

        <section id="sponsoredContainer" class="container margin-minus60">
    	    <div class="grid-12">
        	    <div class="content-inner-block-20 content-box-shadow rounded-corner2 position-rel">
            	    <p class="position-abt pos-top10 pos-right10 font11 text-light-grey">Sponsored</p>
                    <div class="text-center margin-top10 margin-bottom10">
                	    <span class="sponsored-icon margin-bottom25"></span>
                        <p class="font16 sponsored-heading">Save upto 60 % on Bike Insurance Premium</p>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>
    
    <section id="insuranceAssistance" class="container">
    	<div class="grid-12 text-center">
        	<h2 class="text-bold margin-top50 margin-bottom20">Get assitance for insurance</h2>
        	<div class="content-box-shadow content-inner-block-20 rounded-corner2">
            	<div id="insuranceDetails"><!-- insuranceDetails -->
            		<p class="font24 margin-top10 margin-bottom30">Insurance details</p>
                    <div class="margin-bottom30 selectCityContainer form-control-box ui-widget">
                        <input type="text" class="form-control" id="userSelectCity"  placeholder="Select city" tabindex="1" data-bind="textInput: userSelectCity" />                        
                        <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display:none"></span>
                        <span class="bwsprite error-icon hide"></span>
                        <div class="bw-blackbg-tooltip hide">Please enter your city</div>
                    </div>
                    <div class="typeOfInsuranceContainer margin-bottom25">
                        <p class="font16 margin-bottom5">Select your type of insurance</p>
                        <ul>
                            <li tabindex="2">
                                <span class="bwsprite radio-btn radio-btn-checked margin-right15"></span>
                                <span id = "renew" class="policy-type text-bold text-grey selectedPolicy">Renew policy</span>
                            </li>
                            <li>
                                <span class="bwsprite radio-btn radio-btn-unchecked margin-right15"></span>
                                <span id = "expired" class="policy-type">Expired policy</span>
                            </li>
                            <li>
                                <span class="bwsprite radio-btn radio-btn-unchecked margin-right15"></span>
                                <span id = "new" class="policy-type">New policy</span>
                            </li>
                        </ul>
                    </div>
                    <div class="bikeDetailsContainer">
                    <p class="font16 margin-bottom10">Please provide your bike details</p>
                        <ul>
                            <li>
                                <div class="bikeDetailsBox form-control-box">
                                    <input type="text" class="form-control" id="makeName" placeholder="Select make" tabindex="3" data-bind="textInput: makeName" />
                                    <span class="bwsprite error-icon"></span>
                                    <div class="bw-blackbg-tooltip">Please select make</div>
                                </div>
                            </li>
                            <li>
                                <div class="bikeDetailsBox form-control-box">
                                    <input type="text" class="form-control" id="modelName" placeholder="Select model" tabindex="4" data-bind="textInput: modelName"/>
                                    <span id="modelLoader" style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span> 
                                    <span class="bwsprite error-icon"></span>
                                    <div class="bw-blackbg-tooltip">Please select model</div>
                                </div>
                            </li>
                            <li>
                                <div class="bikeDetailsBox form-control-box">
                                    <input type="text" class="form-control " id="versionName" placeholder="Select version" tabindex="5" data-bind="textInput: versionName"/>
                                    <span id="versionLoader" style="display: none;" class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black hide"></span> 
                                    <span class="bwsprite error-icon"></span>
                                    <div class="bw-blackbg-tooltip">Please select version</div>
                                </div>
                            </li>
                            <li>
                                <div class="bikeDetailsBox form-control-box">
                                    <input type="text" class="form-control calender hasDatepicker" id="bikeRegistrationDate" placeholder="Select registration date" tabindex="6" data-bind="textInput: bikeRegistrationDate"/>
                                    <span class="bwsprite error-icon"></span>
                                    <div class="bw-blackbg-tooltip">Please select registration date</div>
                                </div>
                            </li>
                        </ul>
                        <div class="clear"></div>
                        <input type="button" value="Next" class="btn btn-orange margin-top15 margin-bottom10" tabindex="7" id="insuranceDetailsBtn" />
                    </div>
            	</div>
                
                <div id="contactDetails"><!-- contactDetails -->
                	<p class="font24 margin-top10 margin-bottom15">Contact details</p>
                    <div class="contactDetailsContainer">
                    	<ul>
                        	<li>
                            	<div class="contactDetailsBox form-control-box">
                                	<input type="text" class="form-control" placeholder="First name (mandatory)" id="firstName" tabindex="1" data-bind="textInput: firstName"/>
                                    <span class="bwsprite error-icon"></span>
                                    <div class="bw-blackbg-tooltip">Please enter your first name</div>
                                </div>
                            </li>
                            <li>
                            	<div class="contactDetailsBox form-control-box">
                                	<input type="text" class="form-control" placeholder="Last name" id="lastName" tabindex="2" data-bind="textInput: lastName" />
                                    <span class="bwsprite error-icon"></span>
                                    <div class="bw-blackbg-tooltip">Please enter your last name</div>
                                </div>
                            </li>
                            <li>
                            	<div class="contactDetailsBox form-control-box">
                                	<input type="text" class="form-control" placeholder="Email id (mandatory)" id="email" tabindex="3" data-bind="textInput: email" />
                                    <span class="bwsprite error-icon"></span>
                                    <div class="bw-blackbg-tooltip">Please enter your email id</div>
                                </div>
                            </li>
                            <li>
                            	<div class="contactDetailsBox form-control-box">
                                	<input type="text" class="form-control" maxlength="10" placeholder="Mobile no. (mandatory)" id="mobile" tabindex="4" data-bind="textInput: mobile" />
                                    <span class="bwsprite error-icon"></span>
                                    <div class="bw-blackbg-tooltip">Please enter valid 10 digit mobile no.</div>
                                </div>
                            </li>
                        </ul>
                        <div class="clear"></div>
                        <input type="button" value="Next" id="contactDetailsBtn" class="btn btn-orange margin-bottom10" tabindex="5" data-bind="click: saveUserDetail"/>
                        <p class="font14 margin-bottom30">By continuing you agree that we or our partner Policy Boss may contact you for an enquiry</p>
                    </div>
                </div>
                
                <div id="responseContainer"><!-- responseContainer -->
                	<p class="font18 margin-top10 text-bold">Thank You!</p>
                    <p class="font16">Hi <span id="responseUserName"></span>! <br>
                    Thank you for expressing interest. You will get a call from our partner, PolicyBoss (Landmark Insurance Brokers), with exciting insurance offers.
                    </p>
                </div>
                
            </div>
        </div>
        <div class="clear"></div>
    </section>
    
    <section class="container margin-bottom30">
    	<div class="grid-12">
        	<h2 class="text-bold text-center margin-top50 margin-bottom20">FAQs</h2>
            <div class="content-box-shadow rounded-corner2 faqWrapper">
            	<ul>
                	<li>
                    	<h3>
                        	What is bike insurance?
                            <span class="morelessSign">+</span>
                        </h3>
                        <div class="faqAnswerBox">
                        	<p>
                            	Bike or Two Wheeler Insurance extends protection against theft or any damage to your bike. It also safeguards your bike against third party liabilities. A comprehensive policy not only provides coverage in accidental injuries but also pays specified compensation for death sustained by the owner & driver of the vehicle in direct connection with the vehicle insured or while mounting into / dismounting from or traveling in the insured vehicle as a co-driver, caused by violent, accidental, external and visible means.
                            </p>
                        </div>
                    </li>
                    <li>
                    	<h3>
                        	Is two wheeler insurance mandatory?
                            <span class="morelessSign">+</span>
                        </h3>
                        <div class="faqAnswerBox">
                        	<p>
                            	Two wheeler insurance is a mandate thing as per the law and much needed to safeguard your passengers, your fellow drivers, other people's property, pedestrians, yourself and your beloved bike.
                            </p>
                        </div>
                    </li>
                    <li>
                    	<h3>
                        	What does my Bike insurance cover?
                            <span class="morelessSign">+</span>
                        </h3>
                        <div class="faqAnswerBox">
                        	<p>
	                            Following things get covered in your two wheeler insurance:
                            	<ul>
                                	<li>
                                    	Normal wear-and-tear of the vehicle
                                    </li>
                                    <li>
                                    	Mechanical and electrical breakdown
                                    </li>
                                    <li>
                                    	Vehicle being used other than in accordance with the limitations as to use. For example, if you use your two-wheeler for remuneration purposes
                                    </li>
                                    <li>
                                    	Damage to / by person driving without a valid driving license
                                    </li>
                                    <li>
                                    	Loss or damage caused while NOT riding under the influence of alcohol or any other intoxicating substance
                                    </li>
                                    <li>
                                    	Loss or damage due to depreciation of the vehicle's value
                                    </li>
                                    <li>
                                    	Consequential loss - if the original damage causes subsequent damage / loss, only the original damage will be covered
                                    </li>
                                    <li>
                                    	Compulsory deductibles - a fixed amount that gets deducted at the time of the claim
                                    </li>
                                </ul>
                            </p>
                        </div>
                    </li>
                    <li>
                    	<h3>
                        	What are the key features of insurance that is being offered?
                            <span class="morelessSign">+</span>
                        </h3>
                        <div class="faqAnswerBox">
                            <ul>
                                <li>
                                    Easy and hassle-free claim process with minimum documentation
                                </li>
                                <li>
                                    Comprehensive cover in India
                                </li>
                                <li>
                                    Exhaustive list of network garages with 24X7 roadside assistance facilities
                                </li>
                                <li>
                                    Instant renewal
                                </li>
                            </ul>
                        </div>
                    </li>
                    <li>
                    	<h3>
                        	Does the bike insurance lapse after the death of the insured?
                            <span class="morelessSign">+</span>
                        </h3>
                        <div class="faqAnswerBox">
                        	<p>
                            	In the event of death of the insured, the bike insurance policy will not immediately lapse but will remain valid for a period of three months from the death of insured or until the expiry of the policy (whichever is earlier). During the said period, legal heir of the insured whom the custody and the use of motor vehicle passes may apply to have this policy transferred to the name of the heir or obtain new insurance policy for bike. However, following documents are needed in such case:
                                <ul>
                                	<li>
                                    	Death certificate of the insured
                                    </li>
                                    <li>
                                    	Proof of title of the bike
                                    </li>
                                    <li>
                                    	Original Policy Documents
                                    </li>
                                </ul>
                            </p>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <div class="clear"></div>
    </section>
        <!-- #include file="/UI/includes/footerBW.aspx" -->
        <!-- #include file="/UI/includes/footerscript.aspx" -->
    <script type="text/javascript" src="<%= staticUrl %>/UI/src/home.js?<%= staticFileVersion %>"></script>
    <script type="text/javascript" src="<%= staticUrl %>/UI/src/insurance.js?<%= staticFileVersion %>"></script>
    <script type="text/javascript" src="<%= staticUrl %>/UI/src/pikaday.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            var picker = new Pikaday({
                field: document.getElementById('bikeRegistrationDate')
            })
    
            <% var serializer = new System.Web.Script.Serialization.JavaScriptSerializer(); %>
            var cities = <%= serializer.Serialize(cityList) %>;
            var makes = <%= serializer.Serialize(makeList) %>;
            insauranceModel.leadSourceId = <%= Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["sourceId"]) %>;
            cityAutoComplete();
            makeAutoComplete();
            versionAutoSuggest();
            window.onload = function () {
                var btnRelease = document.getElementById('');
                function setGlobal() {
                    window.onbeforeunload = null;
                }

                $(btnRelease).click(setGlobal);

                $("a").click(function () {
                    window.onbeforeunload = null;
                });
                window.onbeforeunload = function () {
                    return "";
                };
            };
    </script>
    </form>
</body>
</html>
