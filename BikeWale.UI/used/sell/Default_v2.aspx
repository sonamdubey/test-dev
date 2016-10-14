<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default_v2.aspx.cs" Inherits="Bikewale.Used.Sell.Default_v2" %>

<!DOCTYPE html>

<html>
<head>
    <title>Sell Bike</title>

    <!-- #include file="/includes/headscript_desktop_min.aspx" -->

    <link href="/min/css/sell-bike.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->

        <section class="bg-light-grey padding-top10" id="breadcrumb">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url">
                                <span itemprop="title">Home</span></a>
                            </li>
                            <li itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span>
                              <a href="/used/" itemprop="url"><span>Used Bikes</span></a>
                            </li>
                            <li itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span>
                              <span>Sell Your Bike</span>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <div class="content-box-shadow card-header">
                            <h1>Sell your bike</h1>
                        </div>
                        <div id="sell-bike-content">
                            <div id="sell-bike-left-col" class="grid-7 panel-group">
                                <div class="panel">
                                    <div class="panel-head">
                                        <span class="sell-bike-sprite" data-bind="click: gotoStep1, css: formStep() == 1 ? 'step-1-active' : 'edit-step'"></span>
                                        <span class="panel-title">Bike details</span>
                                    </div>
                                    <div class="panel-body" data-bind="visible: formStep() == 1">
                                        <div class="panel-row margin-bottom10">
                                            <div class="grid-4 alpha select-box">
                                                <p class="select-label">Make<sup>*</sup></p>
                                                <select class="chosen-select" data-placeholder="Select make" data-bind="chosen: {}, value: bikeDetails().make, validationElement: bikeDetails().make">
                                                    <option value></option>
                                                    <option value="10">Honda</option>
                                                    <option value="11">Bajaj</option>
                                                    <option value="12">Hero</option>
                                                    <option value="13">TVS</option>
                                                    <option value="14">Royal Enfield</option>
                                                    <option value="15">Harley Davidson</option>
                                                    <option value="16">KTM</option>
                                                    <option value="17">Aprilia</option>
                                                    <option value="18">Benelli</option>
                                                    <option value="19">Yamaha</option>
                                                </select>
                                                <span class="boundary"></span>
                                                <span class="error-text" data-bind="validationMessage: bikeDetails().make"></span>
                                            </div>
                                            <div class="grid-4 select-box">
                                                <p class="select-label">Model<sup>*</sup></p>
                                                <select class="chosen-select" data-placeholder="Select model" data-bind="chosen: {}, value: bikeDetails().model, validationElement: bikeDetails().model">
                                                    <option value></option>
                                                    <option value="50">125 Scooter</option>
                                                    <option value="51">Activa</option>
                                                    <option value="52">CB Hornet 160R</option>
                                                    <option value="53">CB Shine</option>
                                                    <option value="54">Avenger 220 Cruise</option>
                                                    <option value="55">Avenger 220 Street</option>
                                                </select>
                                                <span class="boundary"></span>
                                                <span class="error-text" data-bind="validationMessage: bikeDetails().model"></span>
                                            </div>
                                            <div class="grid-4 omega select-box">
                                                <p class="select-label">Version<sup>*</sup></p>
                                                <select class="chosen-select" data-placeholder="Select version" data-bind="chosen: {}, value: bikeDetails().version, validationElement: bikeDetails().version">
                                                    <option value></option>
                                                    <option value="80">Kick/Drum/Spokes</option>
                                                    <option value="81">Electric Start/Drum/Alloy</option>
                                                    <option value="82">CBS</option>
                                                </select>
                                                <span class="boundary"></span>
                                                <span class="error-text" data-bind="validationMessage: bikeDetails().version"></span>
                                            </div>
                                            <div class="clear"></div>
                                        </div>

                                        <div class="panel-row margin-bottom20">
                                            <div class="input-box form-control-box" data-bind="css: bikeDetails().kmsRidden().length > 0 ? 'not-empty' : ''">
                                                <input type="number" id="kmsRidden" min="1" data-bind="textInput: bikeDetails().kmsRidden, validationElement: bikeDetails().kmsRidden" />
                                                <label for="kmsRidden">Kms ridden<sup>*</sup></label>
                                                <span class="boundary"></span>
                                                <span class="error-text" data-bind="validationMessage: bikeDetails().kmsRidden"></span>
                                            </div>
                                        </div>

                                        <div class="panel-row margin-bottom20">
                                            <div class="select-box">
                                                <p class="select-label">City<sup>*</sup></p>
                                                <select class="chosen-select" data-placeholder="Select city" data-bind="chosen: {}, value: bikeDetails().city, validationElement: bikeDetails().city">
                                                    <option value></option>
                                                    <option value="14">Ahmednagar</option>
                                                    <option value="361">Alibag</option>
                                                    <option value="1">Mumbai</option>
                                                    <option value="13">Navi Mumbai</option>
                                                    <option value="8">Panvel</option>
                                                    <option value="15">Ahmednagar</option>
                                                    <option value="362">Alibag</option>
                                                    <option value="2">Mumbai</option>
                                                    <option value="15">Navi Mumbai</option>
                                                    <option value="9">Panvel</option>
                                                </select>
                                                <span class="boundary"></span>
                                                <span class="error-text" data-bind="validationMessage: bikeDetails().city"></span>
                                            </div>
                                        </div>

                                        <div class="panel-row margin-bottom20">
                                            <div class="input-box form-control-box" data-bind="css: bikeDetails().expectedPrice().length > 0 ? 'not-empty' : ''">
                                                <input type="number" id="expectedPrice" min="1" data-bind="textInput: bikeDetails().expectedPrice, validationElement: bikeDetails().expectedPrice" />
                                                <label for="expectedPrice">Expected price<sup>*</sup></label>
                                                <span class="boundary"></span>
                                                <span class="error-text" data-bind="validationMessage: bikeDetails().expectedPrice"></span>
                                            </div>
                                        </div>

                                        <div class="panel-row margin-bottom20">
                                            <div class="select-box select-box-no-input">
                                                <p class="select-label">Owner<sup>*</sup></p>
                                                <select class="owner-chosen-select chosen-select" data-bind="chosen: {}, value: bikeDetails().owner, validationElement: bikeDetails().owner">
                                                    <option value></option>
                                                    <option value="1">I bought it new</option>
                                                    <option value="2">I'm the second owner</option>
                                                    <option value="3">I'm the third owner</option>
                                                    <option value="4">I'm the fourth owner</option>
                                                    <option value="5">Four or more previous owners</option>
                                                </select>
                                                <span class="boundary"></span>
                                                <span class="error-text" data-bind="validationMessage: bikeDetails().owner"></span>
                                            </div>
                                        </div>

                                        <div class="panel-row margin-bottom20">
                                            <div class="select-box">
                                                <p class="select-label">Bike registered at<sup>*</sup></p>
                                                <select class="chosen-select" data-placeholder="Select city" data-bind="chosen: {}, value: bikeDetails().registeredCity, validationElement: bikeDetails().registeredCity">
                                                    <option value></option>
                                                    <option value="14">Ahmednagar</option>
                                                    <option value="361">Alibag</option>
                                                    <option value="1">Mumbai</option>
                                                    <option value="13">Navi Mumbai</option>
                                                    <option value="8">Panvel</option>
                                                    <option value="15">Ahmednagar</option>
                                                    <option value="362">Alibag</option>
                                                    <option value="2">Mumbai</option>
                                                    <option value="15">Navi Mumbai</option>
                                                    <option value="9">Panvel</option>
                                                </select>
                                                <span class="boundary"></span>
                                                <span class="error-text" data-bind="validationMessage: bikeDetails().registeredCity"></span>
                                            </div>
                                        </div>

                                        <div class="panel-row margin-bottom10">
                                            <div class="color-box-content">
                                                <div id="select-color-box" class="select-color-box">
                                                    <p class="select-color-label color-box-default">Colour<sup>*</sup></p>
                                                    <p id="selected-color" class="color-box-default" data-bind="text: bikeDetails().color, validationElement: bikeDetails().color"></p>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: bikeDetails().color"></span>

                                                    <div class="color-dropdown">
                                                        <p class="dropdown-label">Colour</p>
                                                        <ul>
                                                            <li class="color-list-item" data-bind="click: bikeDetails().colorSelection">
                                                                <div class="color-box color-count-one">
                                                                    <span style="background-color:#c83333"></span>
                                                                </div>
                                                                <p class="color-box-label">Red</p>
                                                            </li>
                                                            <li class="color-list-item" data-bind="click: bikeDetails().colorSelection">
                                                                <div class="color-box color-count-two">
                                                                    <span style="background-color:#c83333"></span>
                                                                    <span style="background-color:#1b1a1a"></span>
                                                                </div>
                                                                <p class="color-box-label">Black and Red</p>
                                                            </li>
                                                            <li class="color-list-item" data-bind="click: bikeDetails().colorSelection">
                                                                <div class="color-box color-count-three">
                                                                    <span style="background-color:#c83333"></span>
                                                                    <span style="background-color:#1b1a1a"></span>
                                                                    <span style="background-color:#3a5cee"></span>
                                                                </div>
                                                                <p class="color-box-label">Black, Red and Blue</p>
                                                            </li>
                                                            <li class="other-color-item">
                                                                <div class="color-box">
                                                                    <span></span>
                                                                </div>
                                                                <div class="input-box input-color-box form-control-box" data-bind="css: bikeDetails().otherColor().length > 0 ? 'not-empty' : '', validationElement: bikeDetails().otherColor">
                                                                    <input type="text" id="otherColor" data-bind="textInput: bikeDetails().otherColor" />
                                                                    <label for="otherColor">Other, please specify</label>
                                                                    <span class="boundary"></span>
                                                                    <span class="error-text" data-bind="validationMessage: bikeDetails().otherColor"></span>
                                                                </div>
                                                            </li>
                                                        </ul>
                                                        <div class="text-center padding-bottom20" data-bind="visible: bikeDetails().otherColor().length > 0">
                                                            <button type="button" class="btn btn-orange btn-sm" data-bind="click: bikeDetails().submitOtherColor">Done</button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="panel-row">
                                            <input type="button" class="btn btn-orange" value="Save and Continue" data-bind="click: bikeDetails().saveBikeDetails" />
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="panel">
                                    <div class="panel-head">
                                        <span class="sell-bike-sprite" data-bind="click: gotoStep2, css: (formStep() == 2) ? 'step-2-active' : (formStep() > 1) ? 'edit-step' : 'step-2-inactive'"></span>
                                        <span class="panel-title">Personal details</span>
                                    </div>
                                    <div class="panel-body" data-bind="visible: formStep() == 2">
                                        <div class="panel-row margin-bottom20">
                                            <ul id="seller-type-list">
                                                <li data-bind="click: personalDetails().sellerType">
                                                    <span class="bwsprite radio-icon"></span>
                                                    <span class="seller-label">I am an Individual</span>
                                                </li>
                                                <li data-bind="click: personalDetails().sellerType">
                                                    <span class="bwsprite radio-icon"></span>
                                                    <span class="seller-label">I am a dealer</span>
                                                </li>
                                            </ul>
                                            <div class="clear"></div>
                                        </div>

                                        <%--<div class="panel-row margin-bottom20">
                                            <div class="input-box form-control-box" data-bind="css: bikeDetails().kmsRidden().length > 0 ? 'not-empty' : ''">
                                                <input type="number" id="sellerName" min="1" data-bind="textInput: bikeDetails().kmsRidden, validationElement: bikeDetails().kmsRidden" />
                                                <label for="kmsRidden">Kms ridden<sup>*</sup></label>
                                                <span class="boundary"></span>
                                                <span class="error-text" data-bind="validationMessage: bikeDetails().kmsRidden"></span>
                                            </div>
                                        </div>--%>

                                        <div class="panel-row margin-bottom20">
                                            <div class="terms-content">
                                                <span></span>
                                                <p>I agree with BikeWale sell bike <a href="" target="_blank">Terms & Conditions</a>, visitor agreement and privacy policy *. I agree that by clicking 'List your bike’ button, I am permitting buyers to contact me on my Mobile number.</p>
                                            </div>
                                        </div>

                                        <div class="panel-row">
                                            <input type="button" class="btn btn-orange margin-right20" value="List your bike" data-bind="click: personalDetails().savePersonalDetails" />
                                            <input type="button" class="btn btn-white" value="Previous" data-bind="click: personalDetails().backToBikeDetails" />
                                        </div>

                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="panel">
                                    <div class="panel-head">
                                        <span class="sell-bike-sprite" data-bind="css: (formStep() == 3) ? 'step-3-active' : 'step-3-inactive'"></span>
                                        <span class="panel-title">More details</span>
                                    </div>
                                </div>
                            </div>
                            <div id="sell-bike-right-col" class="grid-5">

                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>


        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="/src/knockout.validation.js"></script>
        <script type="text/javascript" src="/src/sell-bike.js"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->

    </form>
</body>
</html>
