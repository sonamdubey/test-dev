<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Pricequote.BookingConfig" Trace="false" %>

<!DOCTYPE html>
<html>
<head>
    <%
        //title = bikeName + " Bookingbooking-sprite buy-icon customize-icon-grey Summary";
        //description = "Authorise dealer price details of a bike " + bikeName;
       // keywords = bikeName + ", price, authorised, dealer,Booking ";    
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bookingconfig.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
</head>
<body class="header-fixed-inner">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="container">
                <div class="grid-12 margin-bottom5">
                    <div class="breadcrumb margin-bottom10">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/new/">New Bikes</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Booking Config</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black margin-top10">Booking configurator - 
                	<span><%= bikeName %></span>
                    </h1>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container margin-bottom20" id="bookingConfig">
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-20 rounded-corner2">
                        <div id="configTabsContainer" class="margin-bottom10">
                            <div class="horizontal-line position-rel margin-auto"></div>
                            <ul>
                                <li>
                                    <div id="customizeBikeTab" class="bike-config-part active-tab text-bold" data-tabs-config="customizeBike">
                                        <p>Customize your bike</p>
                                        <div class="config-tabs-image">
                                            <span class="booking-sprite booking-config-icon customize-icon-selected"></span>
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div id="financeDetailsTab" class="bike-config-part disabled-tab" data-tabs-config="financeDetails">
                                        <p>Finance details</p>
                                        <div class="config-tabs-image">
                                            <span class="booking-sprite booking-config-icon finance-icon-grey"></span>
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div id="dealerDetailsTab" class="bike-config-part disabled-tab" data-tabs-config="dealerDetails">
                                        <p>Dealer details</p>
                                        <div class="config-tabs-image">
                                            <span class="booking-sprite booking-config-icon confirmation-icon-grey"></span>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <div id="customizeBike" class="show">
                            <div class="select-version-container border-light-bottom">
                                <h4 class="select-versionh4">Select version</h4>
                                <ul class="select-versionUL">
                                    <asp:Repeater ID="rptVarients" runat="server">
                                        <ItemTemplate>
                                            <li class="text-light-grey border-light-grey" versionId="<%#DataBinder.Eval(Container.DataItem,"MinSpec.VersionId") %>">
                                                <span class="bwsprite radio-btn radio-sm-unchecked margin-right5 margin-left10"></span>
                                                <span class="version-title-box"><%#DataBinder.Eval(Container.DataItem,"MinSpec.VersionName") %></span>
                                            </li>
                                        </ItemTemplate>
                                    <%--<li class="text-light-grey border-light-grey">
                                        <span class="bwsprite radio-btn radio-sm-unchecked margin-right5 margin-left10"></span>
                                        <span class="version-title-box">Disc</span>
                                    </li>
                                    <li class="text-light-grey border-light-grey">
                                        <span class="bwsprite radio-btn radio-sm-unchecked margin-right5 margin-left10"></span>
                                        <span class="version-title-box">Drum</span>
                                    </li>
                                    <li class="text-light-grey border-light-grey">
                                        <span class="bwsprite radio-btn radio-sm-unchecked margin-right5 margin-left10"></span>
                                        <span class="version-title-box">ABS</span>
                                    </li>--%>

                                    </asp:Repeater>
                                    <asp:HiddenField id="selectedVersionId" runat=Server></asp:HiddenField>
                                </ul>
                                <p class="font14 margin-top5">Version features</p>
                                <ul class="version-featureUL margin-bottom10">
                                    <li style="display:<%= (selectedVarient.MinSpec.AlloyWheels)?"inline-block":"none" %>">
                                        <span class="booking-sprite ver-feature-img alloy-wheel-icon inline-block"></span>
                                        <span class="inline-block">Alloy wheel</span>
                                    </li>
                                    <li style="display:<%= ((selectedVarient.MinSpec.BrakeType).ToLower().Contains("disc"))?"inline-block":"none" %>">
                                        <span class="booking-sprite ver-feature-img disc-brake-icon inline-block"></span>
                                        <span class="inline-block">Disc brake</span>
                                    </li>
                                    <li style="display:<%= (selectedVarient.MinSpec.ElectricStart)?"inline-block":"none" %>">
                                        <span class="booking-sprite ver-feature-img electric-start-icon inline-block"></span>
                                        <span class="inline-block">Electric Start</span>
                                    </li>
                                    <li style="display:<%= (selectedVarient.MinSpec.AntilockBrakingSystem)?"inline-block":"none" %>">
                                        <span class="booking-sprite ver-feature-img abs-icon inline-block"></span>
                                        <span class="inline-block">ABS</span>
                                    </li>
                                    <li style="display:<%= (!selectedVarient.MinSpec.AlloyWheels)?"inline-block":"none" %>">
                                        <span class="booking-sprite ver-feature-img spoke-wheel-icon inline-block"></span>
                                        <span class="inline-block">Spoke wheel</span>
                                    </li>
                                    <li style="display:<%= (selectedVarient.MinSpec.BrakeType.ToLower().Contains("Drum"))?"inline-block":"none" %>">
                                        <span class="booking-sprite ver-feature-img drum-brake-icon inline-block"></span>
                                        <span class="inline-block">Drum brake</span>
                                    </li>
                                    <li style="display:<%= (!selectedVarient.MinSpec.ElectricStart)?"inline-block":"none" %>">
                                        <span class="booking-sprite ver-feature-img kick-start-icon inline-block"></span>
                                        <span class="inline-block">Kick Start</span>
                                    </li>
                                </ul>
                            </div>
                            <div class="select-color-container border-light-bottom padding-bottom10 margin-bottom15">
                                <h4 class="select-colorh4 margin-top15">Select color</h4>
                                <ul class="select-colorUL">
                                    <asp:Repeater ID="rptVersionColors" runat="server">
                                        <ItemTemplate> 
                                            <li class="text-light-grey border-light-grey" >
                                                <span class="color-box" style='background:#<%#DataBinder.Eval(Container.DataItem,"HexCode")%>'></span>
                                                <span class="color-title-box"><%#DataBinder.Eval(Container.DataItem,"ColorName") %></span>
                                            </li> 
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul> 
                            </div>
                            <div class="customize-onRoadPrice-container text-right">
                                <p class="text-light-grey margin-bottom5 font14">On-road price in <span id="pqArea" class="font16 text-bold text-grey">Andheri<span class="padding-right5">,</span></span><span id="pqCity" class="font16 text-bold text-grey">Mumbai</span></p>
                                <div class="modelPriceContainer margin-bottom15">
                                    <span class="font28"><span class="fa fa-rupee"></span></span>
                                    <span id="bike-price1" class="font30">1,22,000</span>
                                    <span class="font14 text-light-grey viewBreakupText">View breakup</span>
                                </div>
                                <input type="button" value="Next" id="customizeBikeNextBtn" class="btn btn-orange">
                            </div>
                        </div>

                        <div id="financeDetails" class="hide">
                            <div class="finance-options-container">
                                <h4 class="select-financeh4">Finance options</h4>
                                <ul class="select-financeUL">
                                    <li class="finance-required selected-finance text-bold border-dark-grey">
                                        <span class="bwsprite radio-btn radio-sm-checked margin-right5 margin-left10"></span>
                                        <span class="finance-title-box">Required</span>
                                    </li>
                                    <li class="text-light-grey border-light-grey">
                                        <span class="bwsprite radio-btn radio-sm-unchecked margin-right5 margin-left10"></span>
                                        <span class="finance-title-box">Not required</span>
                                    </li>
                                </ul>
                            </div>
                            <div class="finance-emi-container">
                                <div class="finance-emi-left-box alpha border-light-right">
                                    <div class="emi-slider-box">
                                        <div class="emi-slider-box-left-section">
                                            <p>Down payment</p>
                                            <div id="downPaymentSlider"></div>
                                            <div class="slider-range-points">
                                                <ul class="range-five-pointsUL range-pointsUL">
                                                    <li class="range-points-bar"><span>0</span></li>
                                                    <li class="range-points-bar" style="left: 5%"><span>2.5L</span></li>
                                                    <li class="range-points-bar" style="left: 10%"><span>5L</span></li>
                                                    <li class="range-points-bar" style="left: 15%"><span>7.5L</span></li>
                                                    <li class="range-points-bar" style="left: 19.9%"><span>10L</span></li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="emi-slider-box-right-section font16">
                                            <span class="fa fa-rupee"></span>
                                            <span id="downPaymentAmount" class="text-bold"></span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="emi-slider-box">
                                        <div class="emi-slider-box-left-section">
                                            <p>Loan Amount</p>
                                            <div id="loanAmountSlider"></div>
                                            <div class="slider-range-points">
                                                <ul class="range-five-pointsUL range-pointsUL">
                                                    <li class="range-points-bar"><span>0</span></li>
                                                    <li class="range-points-bar" style="left: 5%"><span>2.5L</span></li>
                                                    <li class="range-points-bar" style="left: 10%"><span>5L</span></li>
                                                    <li class="range-points-bar" style="left: 15%"><span>7.5L</span></li>
                                                    <li class="range-points-bar" style="left: 19.9%"><span>10L</span></li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="emi-slider-box-right-section font16">
                                            <span class="fa fa-rupee"></span>
                                            <span id="loanAmount" class="text-bold"></span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="emi-slider-box">
                                        <div class="emi-slider-box-left-section">
                                            <p>Tenure</p>
                                            <div id="tenureSlider"></div>
                                            <div class="slider-range-points">
                                                <ul class="range-pointsUL">
                                                    <li class="range-points-bar"><span>1</span></li>
                                                    <li class="range-points-bar" style="left: 3%"><span>2</span></li>
                                                    <li class="range-points-bar" style="left: 6%"><span>3</span></li>
                                                    <li class="range-points-bar" style="left: 8.2%"><span>4</span></li>
                                                    <li class="range-points-bar" style="left: 11%"><span>5</span></li>
                                                    <li class="range-points-bar" style="left: 13.3%"><span>6</span></li>
                                                    <li class="range-points-bar" style="left: 15.8%"><span>7</span></li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="emi-slider-box-right-section">
                                            <span id="tenurePeriod" class="font16 text-bold"></span>
                                            <span class="font12">Months</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="emi-slider-box">
                                        <div class="emi-slider-box-left-section">
                                            <p>Rate of interest</p>
                                            <div id="rateOfInterestSlider"></div>
                                            <div class="slider-range-points">
                                                <ul class="range-five-pointsUL range-pointsUL">
                                                    <li class="range-points-bar"><span>0</span></li>
                                                    <li class="range-points-bar" style="left: 5%"><span>5</span></li>
                                                    <li class="range-points-bar" style="left: 10%"><span>10</span></li>
                                                    <li class="range-points-bar" style="left: 15%"><span>15</span></li>
                                                    <li class="range-points-bar" style="left: 19.9%"><span>20</span></li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="emi-slider-box-right-section font16">
                                            <span id="rateOfInterestPercentage" class="text-bold"></span>
                                            <span>%</span>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </div>
                                <div class="finance-emi-right-box omega text-center">
                                    <h4 class="margin-top90 text-light-grey margin-bottom20">Indicative EMI</h4>
                                    <div class="indicative-emi-amount margin-bottom5">
                                        <span class="font28"><span class="fa fa-rupee"></span></span>
                                        <span id="emiAmount" class="font30">12,000</span>
                                    </div>
                                    <p class="font16 text-light-grey">per month</p>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="disclaimer-onroadprice-container">
                                <div class="disclaimer-container text-left border-light-right">
                                    <h3 class="padding-bottom10 margin-right20 border-light-bottom">Disclaimer:</h3>
                                    <ul class="disclaimerUL">
                                        <li>The EMI amount is calculated as per the information entered by you. It does not include any other fees like processing fee that are typically charged by some banks / NBFCs</li>
                                        <li>The actual EMI and down payment will vary depending upon your credit profile. Please get in touch with your bank / NBFC to know the exact EMI and downpayment based on your credit profile.</li>
                                    </ul>
                                </div>
                                <div class="disclaimer-onRoadPrice-container text-right">
                                    <p class="text-light-grey margin-bottom5 font14">On-road price in <span id="pqArea" class="font16 text-bold text-grey">Andheri<span class="padding-right5">,</span></span><span id="pqCity" class="font16 text-bold text-grey">Mumbai</span></p>
                                    <div class="modelPriceContainer margin-bottom15">
                                        <span class="font28"><span class="fa fa-rupee"></span></span>
                                        <span id="bike-price" class="font30">1,22,000</span>
                                        <span class="font14 text-light-grey viewBreakupText">View breakup</span>
                                    </div>
                                    <input type="button" value="Next" id="financeDetailsNextBtn" class="btn btn-orange">
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>

                        <div id="dealerDetails" class="hide">
                            <div class="contact-offers-container border-light-bottom padding-bottom15">
                                <div class="grid-5 alpha contact-details-container border-light-right">
                                    <h3 class="padding-bottom10 padding-left5 margin-right20 border-light-bottom"><span class="bwsprite contact-icon margin-right5"></span>Contact details:</h3>
                                    <ul>
                                        <li>
                                            <p class="text-bold">Offers from the nearest dealers</p>
                                            <p class="text-light-grey">Complex, Mahesh Industrial Estate, Opposite Silver Park, M. B Road, Mira Road East Thane - 401107</p>
                                        </li>
                                        <li>
                                            <p class="text-bold">Availability</p>
                                            <p class="text-light-grey">Waiting period of <span class="text-default">25 days</span></p>
                                        </li>
                                    </ul>
                                </div>
                                <div class="grid-7 omega offer-details-container">
                                    <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom"><span class="bwsprite offers-icon margin-right5"></span>Pay <span class="font16"><span class="fa fa-rupee"></span></span>3000</span> to book your bike and get:</h3>
                                    <ul>
                                        <li>Free Vega Cruiser Helmet worth Rs.1500 from BikeWale</li>
                                        <li>Free Zero Dep Insurance worth Rs.1200 from Dealership</li>
                                        <li>Get free helmet from the dealer</li>
                                        <li>Free Zero Dep Insurance worth Rs.1200</li>
                                    </ul>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="grid-8 alpha margin-top15">
                                <p class="font14 padding-left5"><span class="bwsprite call-icon inline-block margin-right10"></span>In case of any queries feel free to call us on <span class="text-bold font18">1800 120 8300</span></p>
                            </div>
                            <div class="dealer-onRoadPrice-container grid-4 omega margin-top15 border-light-left text-right">
                                <p class="text-light-grey margin-bottom5 font14">On-road price in <span id="pqArea" class="font16 text-bold text-grey">Andheri<span class="padding-right5">,</span></span><span id="pqCity" class="font16 text-bold text-grey">Mumbai</span></p>
                                <div class="modelPriceContainer margin-bottom15">
                                    <span class="font28"><span class="fa fa-rupee"></span></span>
                                    <span id="bike-price" class="font30">1,22,000</span>
                                    <span class="font14 text-light-grey viewBreakupText">View breakup</span>
                                </div>
                                <input type="button" value="Book now" id="dealerDetailsNextBtn" class="btn btn-orange">
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM&sensor=false"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/bookingconfig.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            //Need to uncomment the below script
            //var thisBikename = '<%--<%= this.bikeName %>--%>';
            window.onload = function () {
                var btnRelease = document.getElementById('');
                //Find the button set null value to click event and alert will not appear for that specific button
                function setGlobal() {
                    window.onbeforeunload = null;
                }
                $(btnRelease).click(setGlobal);

                // Alert will not appear for all links on the page
                $("a").click(function () {
                    window.onbeforeunload = null;
                });
                window.onbeforeunload = function () {
                    return "";
                };
            };
        </script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
    </form>
</body>
</html>
