<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="false" Trace="false" Debug="false" Inherits="Carwale.UI.NewCars.PriceQuote.Default"  %>

<%
    //Define all the necessary meta-tags info here.
    PageId = 24;
    Title = " Check On-Road Price Quote";
    Description = "Know On-Road Price of any new car in India. On-road price of a car includes ex-showroom price of the car in your city, insurance charges. road-tax, registration charges, handling charges etc. Finance option is also provided so that you can get a fair idea of EMI and down-payment.";
    Revisit = "5";
    DocumentState = "Dynamic";
    canonical = "https://www.carwale.com/new/prices.aspx";
    altUrl = "https://www.carwale.com/quotation/landing/";
    landingPage = true;
%>

<html xmlns="http://www.w3.org/1999/xhtml" itemscope itemtype="http://schema.org/WebPage">
<head>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <link rel="stylesheet" href="/static/css/pqlanding.css" type="text/css" >
<style>
    .loading-popup { background:transparent; width:90px; padding:20px; border-radius:5px; position:absolute; top:40%; left:45%;-moz-border-radius:5px; -webkit-border-radius:5px; -o-border-radius:5px; -ms-border-radius:5px; margin:0 auto; z-index:999;}
    .loading-icon { background:url(https://img.carwale.com/used/cw-loader.gif?v=1.1) no-repeat; width:90px; height:61px; display:block; float:left; }
</style>
</head>
<body class="special-page special-skin-body no-bg-color">
    <!-- #include file="/includes/header.aspx" -->
    <div id="blackOut-window-pq" class="blackOut-window" style="display: none;"></div>
    <div id="loadingCarImg" style="display:none;">
        <div class="loading-popup">
            <span class="loading-icon"></span>
            <div class="clear"></div>
        </div>
    </div>
    <div>
        <header class="price-quote-banner">
            <div class="container">
                <div class="welcome-box">
                    <h1 class="text-uppercase margin-bottom10">Check On-Road Price</h1>
                    <p class="font20">On-Road Price = Ex-showroom Price + RTO + Insurance</p>
                </div>
            </div>
        </header>
        <section class="container">
            <!-- Get Final Price code starts here -->
            <div class="grid-12">
                <div class="pqlanding-container content-inner-block-10 margin-minus50 content-box-shadow">
                    <div class="pqlanding-form-container text-center">
                        <div class="form-control-box margin-top40 margin-bottom30">
                            <input class="form-control" type="text" placeholder="<%=(PQPlaceHolder ?? "Type to select car name")%>" id="pqCarSelect" />
                            <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display:none"></span>
                            <span class="cwsprite error-icon hide brand-error-icon"></span>
                            <div class="cw-blackbg-tooltip brand-err-msg hide">Please select car model</div>
                        </div>
                        <div class="form-control-box margin-bottom30">
                            <span class="select-box fa fa-angle-down"></span>
                            <select disabled="disabled" class="form-control btn-disable" id="drpPQCity" data-bind='options: pqCities, optionsText: "CityName", optionsValue: "CityId"'>
                                <option value="-1">Select City</option>
                            </select>
                            <span class="cwsprite error-icon hide city-error-icon"></span>
                            <div class="cw-blackbg-tooltip city-err-msg hide">Please select your city</div>
                        </div>
                        <button class="btn btn-orange text-uppercase margin-bottom40" id="btnGetPQ">Check Now</button>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>
        <section class="container">

            <!-- No sawaal. only jawaab code start here -->
            <div class="grid-12">
                <h2 class="text-uppercase text-center margin-top50 margin-bottom30 special-skin-text">No  Approximate. Only  Accurate.</h2>
                <div class="pq-nosawaal-container content-inner-block-10 content-box-shadow margin-bottom30">
                    <div class="pq-nosawaal-title-box margin-top20 margin-bottom30 text-center">
                        <p class="font16">
                            Getting lost with approximate prices?
                    <br />
                            We help you find your way to accurate On-Road Price of any car in your city.
                        </p>
                    </div>
                    <div class="text-center margin-bottom30">
                        <div><img src="<%= Carwale.Utility.CWConfiguration._imgHostUrl %>0x0/cw/static/pricequote-rupee-icon.png?qnxcvnxcsfdsaf"></div>
                    </div>
                    <div class="text-center margin-bottom20 font16">
                        What’s On-Road Price? <a href="" class="FAQsLink">Click here.</a>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>
    </div>
    <div class="faqPopUpContainer content-inner-block-20 hide" id="faqPopUpContainer">
        <h3 class="margin-bottom5">FAQs</h3>
        <div class="faqsCloseBtn position-abt pos-top20 pos-right20 cwsprite cross-lg-lgt-grey cur-pointer"></div>
        <div class="faqs-text-container padding-bottom10 border-solid-bottom">
            <h5 class="faqs-header">What is On Road Price?</h5>
            <p class="answer-text font12">
                On road price is the final price that an Indian buyer pays for buying a car. It is inclusive of the price of the car, all applicable taxes (excise duty, VAT, local state taxes and road tax), first-year insurance premium for the car and all other charges levied by the dealer. The formula goes like this:
                <br>
                On Road Price = Ex-Showroom Price + Octroi (or Municipal Tax, if any) + Road Tax and Registration Charges + Insurance Premium+ Other Charges
            </p>
        </div>
        <div class="faqs-text-container padding-bottom10 border-solid-bottom">
            <h5 class="margin-top10 margin-bottom5 faqs-header">What is Ex-Showroom Price? Is it different for the same car for various cities?</h5>
            <p class="answer-text font12">
                Ex-Showroom price is the price of the car typically declared by its manufacturer for a particular city. It includes ex-factory price of the car, excise duty, other duties and VAT. It may or may not include local taxes such as octroi and other municipal taxes.
                Ex-Showroom price varies for different cities. In fact it could differ from area to area of the same city in case there are different local tax structures for different areas.
            </p>
        </div>
        <div class="faqs-text-container padding-bottom10 border-solid-bottom">
            <h5 class="margin-top10 margin-bottom5 faqs-header">What are typical dealer levied charges and other charges mentioned in the on road price quote?</h5>
            <p class="answer-text font12">
                Sometimes dealers charge a small amount as handling, incidental, and service or customer care charge. These charges are levied to offset the expenses incurred in selling the car to you. Other charges mentioned in the on road price quote could include depot charges, local taxes etc. 
            </p>
        </div>
        <div class="faqs-text-container padding-bottom10 border-solid-bottom">
            <h5 class="margin-top10 margin-bottom5 faqs-header">Does Road tax have to be paid every year, just like insurance? </h5>
            <p class="answer-text font12">
                No. Road tax is typically a one-time tax (in some states, it’s paid for five years as well) that needs to be paid while registering a new car. It is paid to the state government and need not be paid again as long as the car is driven in the same state.
            </p>
        </div>
        <div class="faqs-text-container padding-bottom10">
            <h5 class="margin-top10 margin-bottom5 faqs-header">Does On Road Price indicated by CarWale give information on all of the above aspects?</h5>
            <p class="answer-text font12">
                Yes. CarWale works hard to get you highly accurate prices all across the country along with various discounts and promotional offers. Our motto is to simplify car buying and we help car buyers make informed purchase decisions without giving up on the comfort of their home or office.
            </p>
        </div>

    </div>
    <!-- #include file="/includes/footer.aspx" -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
    <script  type="text/javascript"  src="/static/js/pqlanding.js" ></script>
</body>
</html>

