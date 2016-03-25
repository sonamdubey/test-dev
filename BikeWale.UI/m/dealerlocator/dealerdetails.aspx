<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.DealerDetails" %>

<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <style type="text/css">
        #dealerHeader {
            background: #313131;
            color: #fff;
            width: 100%;
            height: 48px;
            position: fixed;
            overflow: hidden;
            z-index: 2;
        }

        .dealer-back-btn {
            padding: 12px 15px;
            cursor: pointer;
        }

        .fa-arrow-back {
            width: 12px;
            height: 20px;
            background-position: -63px -162px;
        }

        .dealer-header-text {
            width: 80%;
            text-align: left;
            text-overflow: ellipsis;
            white-space: nowrap;
            overflow: hidden;
        }

        .padding-top48 {
            padding-top: 48px;
        }

        .box-shadow {
            -webkit-box-shadow: 0 0 1px #e2e2e2;
            -moz-box-shadow: 0 0 1px #e2e2e2;
            box-shadow: 0 0 1px #e2e2e2;
        }

        .text-pure-black {
            color: #1a1a1a;
        }

        .featured-tag {
            position: relative;
            left: -20px;
            top: -5px;
            width: 100px;
            background: #4d5057;
            z-index: 1;
            line-height: 28px;
        }

            .featured-tag:after {
                content: '';
                width: 12px;
                height: 28px;
                background: url(http://imgd1.aeplcdn.com/0x0/bw/static/sprites/m/upcoming-ribbon.png?v=15Mar2016) no-repeat right top;
                position: absolute;
                left: 98px;
            }

        .dealer-details-section {
            line-height: 1.8;
        }

        .tel-sm-grey-icon {
            width: 12px;
            height: 15px;
            background-position: -86px -323px;
            position: relative;
            top: 2px;
        }

        .mail-grey-icon {
            width: 15px;
            height: 9px;
            background-position: -19px -437px;
        }

        .text-default {
            color: #4d5057;
        }

        .get-direction-icon, .sendto-phone-icon {
            width: 12px;
            height: 10px;
        }

        .get-direction-icon {
            background-position: -31px -421px;
        }

        .sendto-phone-icon {
            background-position: -49px -421px;
        }

        .divider-left {
            border-left: 1px solid #82888b;
            padding-left: 7px;
            margin-left: 7px;
        }

        .border-light-bottom {
            border-bottom: 1px solid #f1f1f1;
        }

        .tel-grey-icon {
            position: relative;
            top: 2px;
        }

        .float-button.float-fixed {
            position: fixed;
            bottom: 0;
            z-index: 8;
            left: 0;
            right: 0;
        }

        .float-button {
            background-color: #f5f5f5;
            padding: 0px 10px 10px 10px;
        }

        #bikesAvailableList .front {
            margin-top: 20px;
            height: auto;
            border-radius: 0;
            box-shadow: none;
            -moz-box-shadow: none;
            -ms-box-shadow: none;
            border: 0 none;
        }

        #bikesAvailableList .bikeDescWrapper {
            padding: 0;
        }

        #bikesAvailableList .imageWrapper {
            height: 143px;
        }

            #bikesAvailableList .imageWrapper img {
                width: 254px;
                height: 143px;
            }
    </style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <header id="dealerHeader">
            <div class="leftfloat dealer-back-btn">
                <a href="javascript:void(0)"><span class="bwmsprite fa-arrow-back"></span></a>
            </div>
            <div class="dealer-header-text leftfloat margin-top10 font18"><%= dealerDetails.Name %></div>
            <div class="clear"></div>
        </header>
        <!--Dealer Details section-->
        <section class="container bg-white padding-top48">
            <div id="dealerDetailsCard" class="padding-top20 padding-right20 padding-left20 font14">
                <%if (dealerDetails.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium) || dealerDetails.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe))
                  { %>
                <div class="featured-tag text-white text-center margin-bottom10">
                    Featured
                </div>
                <%} %>
                <h1 class="font18 text-pure-black margin-bottom5"><%= dealerDetails.Name %></h1>
                <div class="dealer-details-section text-light-grey padding-bottom15 border-light-bottom">
                    <p class="margin-bottom5"><%= dealerDetails.Address %></p>
                    <% if (!string.IsNullOrEmpty(dealerDetails.EMail))
                       { %>
                    <div class="margin-bottom5">
                        <a href="mailto:<%= dealerDetails.EMail %>" class="text-light-grey"><span class="bwmsprite mail-grey-icon"></span><%= dealerDetails.EMail %></a>
                    </div>
                    <% } if (!string.IsNullOrEmpty(dealerDetails.MaskingNumber))
                       { %>
                    <div class="margin-bottom5">
                        <a href="tel:<%= dealerDetails.MaskingNumber %>" class="text-default font16 text-bold"><span class="bwmsprite tel-sm-grey-icon"></span><%= dealerDetails.MaskingNumber %></a>
                    </div>
                    <% } if (!string.IsNullOrEmpty(dealerDetails.WorkingHours))
                       { %>
                    <p>
                        Working hours:<br />
                        <%= dealerDetails.WorkingHours %>
                    </p>
                    <%} %>

                    <%--<a href=""><span class="bwmsprite get-direction-icon margin-right5"></span>Get directions</a>
                    <a href="" class="divider-left"><span class="bwmsprite sendto-phone-icon margin-right5"></span>Send to phone</a>--%>
                </div>
                <%--<div class="padding-top15 padding-bottom20 border-light-bottom">
                    <h3 class="font14 margin-bottom15">Get commute distance and time:</h3>
                    <div class="form-control-box">
                        <input type="text" class="form-control" placeholder="Enter your location" />
                    </div>
                </div>--%>
            </div>
            <div class="grid-12 float-button clearfix float-fixed">
                <div class="show padding-top10">
                    <div class="grid-6 alpha omega">
                        <a id="calldealer" class="btn btn-white btn-full-width btn-sm rightfloat text-bold text-default font14" href="tel:<%= dealerDetails.MaskingNumber %>"><span class="bwmsprite tel-grey-icon margin-right5"></span>Call dealer</a>
                    </div>
                    <div class="grid-6 alpha omega padding-left10">
                        <a id="getAssistance" class="btn btn-orange btn-full-width btn-sm rightfloat font14" href="javascript:void(0);">Get assistance</a>
                    </div>
                </div>
            </div>
        </section>
        <!--Dealer Deatail section end and models section start.-->
        <%if (dealerBikesCount > 0)
          { %>
        <section class="container bg-white margin-bottom20">
            <div class="padding-right20 padding-bottom10 padding-left20 box-shadow font14">
                <div class="padding-top15">
                    <h3 class="font14 margin-bottom15">Models available with the dealer:</h3>
                    <div id="bikesAvailableList">
                        <asp:Repeater ID="rptModels" runat="server">
                            <ItemTemplate>
                                <div class="front">
                                    <div class="contentWrapper">
                                        <div class="imageWrapper margin-bottom20">
                                            <a class="modelurl" href="<%# String.Format("/{0}-bikes/{1}/",DataBinder.Eval(Container.DataItem, "objMake.MaskingName"),DataBinder.Eval(Container.DataItem, "objModel.MaskingName")) %>">
                                                <img class="lazy"
                                                    data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>"
                                                    title="<%# DataBinder.Eval(Container.DataItem, "BikeName") %>"
                                                    alt="<%# DataBinder.Eval(Container.DataItem, "BikeName") %>" src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif">
                                            </a>
                                        </div>
                                        <div class="bikeDescWrapper">
                                            <h3 class="margin-bottom5"><a href="<%# String.Format("/{0}-bikes/{1}/",DataBinder.Eval(Container.DataItem, "objMake.MaskingName"),DataBinder.Eval(Container.DataItem, "objModel.MaskingName")) %>" class="text-pure-black" title="<%# DataBinder.Eval(Container.DataItem, "BikeName") %>"><%# DataBinder.Eval(Container.DataItem, "BikeName") %></a></h3>
                                            <div class="margin-bottom5 text-default text-bold">
                                                <span class="bwmsprite inr-sm-icon"></span>
                                                <span class="font18"><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))) %><span class="font16"> Onwards</span></span>
                                            </div>
                                            <div class="font14 text-light-grey">
                                                <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower"))) %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </section>
        <%} %>
        <!--Dealer models section start.-->
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript">
            var bodHt, footerHt, scrollPosition;
            $(window).scroll(function () {
                bodHt = $('body').height();
                footerHt = $('footer').height();
                scrollPosition = $(this).scrollTop();
                if ($('.float-button').hasClass('float-fixed')) {
                    if (scrollPosition + $(window).height() > (bodHt - footerHt))
                        $('.float-button').removeClass('float-fixed');
                }
                if (scrollPosition + $(window).height() + 60 < (bodHt - footerHt))
                    $('.float-button').addClass('float-fixed');
            });
            //validation code
            var validateUserDetail = function () {
                var isValid = true;
                isValid = validateName();
                isValid = validateEmail();
                isValid = validateMobile();
                return isValid;
            };



            var validateName = function () {
                var isValid = true,
                    name = $("#getLeadName"),
                    nameLength = name.val().length;
                if (name.val().indexOf('&') != -1) {
                    setError(name, 'Invalid name');
                    isValid = false;
                }
                else if (nameLength == 0) {
                    setError(name, 'Please enter your name');
                    isValid = false;
                }
                else if (nameLength >= 1) {
                    hideError(name);
                    isValid = true;
                }
                return isValid;
            };

            var validateEmail = function () {
                var isValid = true,
                    emailId = $("#getEmailID"),
                    emailVal = emailId.val(),
                    reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;
                if (emailVal == "") {
                    setError(emailId, 'Please enter email address');
                    isValid = false;
                }
                else if (!reEmail.test(emailVal)) {
                    setError(emailId, 'Invalid Email');
                    isValid = false;
                }
                return isValid;
            };

            var validateMobile = function () {
                var isValid = true,
                    mobileNo = $("#getMobile"),
                    mobileVal = mobileNo.val(),
                    reMobile = /^[0-9]{10}$/;
                if (mobileVal == "") {
                    setError(mobileNo, "Please enter your Mobile Number");
                    isValid = false;
                }
                else if (!reMobile.test(mobileVal) && isValid) {
                    setError(mobileNo, "Mobile Number should be 10 digits");
                    isValid = false;
                }
                else
                    hideError(mobileNo)
                return isValid;
            };

            var setError = function (element, msg) {
                element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
                element.siblings("div.errorText").text(msg);
            };

            var hideError = function (element) {
                element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
            };

            $("#getMobile,#getLeadName,#getEmailID,#getOTP,#getUpdatedMobile").on("focus", function () {
                hideError($(this));
            });
        </script>
    </form>
</body>
</html>
