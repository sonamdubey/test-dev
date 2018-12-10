<%@ Page Trace="false" Debug="false" Inherits="Carwale.UI.Dealer.UsedCarDealerShowroom" AutoEventWireup="false" Language="C#" %>

<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Register TagPrefix="slider" TagName="Corousel" Src="/Controls/Carousel_Home_940x320.ascx" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI.HtmlControls" Assembly="System.Web" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
    <%
        PageId = 82;
        Title = DealerName + " : " + " Dealer Showroom";
        Description = "Carwale UsedShowroom describes dealers contact details and the list of cars that dealer showroom contains.";
        Keywords = "Dealer contact detail , Showroom";
        Revisit = "5";
        DocumentState = "Dynamic";
        canonical = "https://www.carwale.com/used/dealers-in-" + UrlRewrite.FormatSpecial(DealerCity) + "/" + UrlRewrite.FormatSpecial(DealerName) + "-" + DealerId;
        AdId = "1396440544094";
        AdPath = "/1017752/UsedCar_";
    //Ad300           = false;
    %>
    


    <!-- #include file="/includes/global/head-script.aspx" -->
    <script type='text/javascript'>
        googletag.cmd.push(function () {
            googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());

            //googletag.pubads().enableSyncRendering();
            googletag.pubads().setTargeting('<%=targetKey%>', '<%=targetValue%>');
            googletag.pubads().collapseEmptyDivs();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });
    </script>
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
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/used/">Used Cars</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/used/dealers-in-<%= UrlRewrite.FormatSpecial(DealerCity)%>/">Dealers in <%= DealerCity %></a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><%=DealerName%></li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black special-skin-text"><%=DealerName%>, <%= DealerCity %></h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>

                <div id="overview">
                    <div class="grid-12">
                        <div class="content-box-shadow content-inner-block-10">
                            <div class="">
                                <!-- Carousel banner code start here... -->
                                <div class="white-shadow content-inner-block ">
                                    <div>
                                        <div id="nav2">
                                            <ul id="tabContent">
                                                <li><a id="menu1" href="#stockavailable" target="_self" style="cursor: pointer;">All Available Cars</a></li>
                                                <li id="liAboutus" runat="server"><a id="menu2" href="#aboutus" target="_self" style="cursor: pointer;">About Us</a></li>
                                                <li id="liFacilities" runat="server"><a id="menu3" href="#facilities" target="_self" style="cursor: pointer;">Facilities</a></li>
                                                <li><a id="menu4" href="#locateus" target="_self" style="cursor: pointer;">Locate Us</a></li>
                                                <li><a id="menu5" href="#contactdealer" target="_self" style="cursor: pointer;">Ask Dealer to Call You</a></li>
                                            </ul>
                                        </div>
                                        <!-- Carousel banner code start here... -->
                                        <div id="carousel" class="container_12 alpha omega block-spacing" style="margin: 5px 5px 0 5px;">
                                            <div>
                                                <slider:Corousel ID="corouselHome" runat="server" isRoundedCorner="false"></slider:Corousel>
                                                <div class="clear"></div>
                                                <div class="carousel-shadow"></div>
                                            </div>
                                        </div>
                                        <!-- Carousel banner code end here... -->
                                    </div>
                                    <!-- Carousel banner code end here... -->
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clear"></div>

                    <div class="grid-12 margin-top20">
                        <div class="content-box-shadow content-inner-block-10">
                            <div id="stockavailable">
                                <div>
                                    <h2 class="leftfloat font18">Available Stock (<%=StockCount%>)</h2>
                                    <p class="rightfloat"><a id="lnkUsed" href="/used/cars-in-<%= UrlRewrite.FormatSpecial(DealerCity)%>/" target="_blank">View all Used Cars in <%= DealerCity %> </a></p>
                                    <div class="clear"></div>
                                </div>

                                <div class="margin-top10" style="position: relative;">
                                    <div>
                                        <div class="leftfloat margin-top10">Filter by :</div>
                                        <div class="leftfloat position-rel">
                                            <span class="fa fa-angle-down position-abt pos-top15 pos-right5"></span>
                                            <div id="txtselect" class="drpMake content-inner-block-10"></div>
                                            <ul id="make" class="hide">
                                                <asp:repeater id="rptMakes" runat="server">
                                                    <itemtemplate>
                                                                    <li>
                                                                    <input type="checkbox" id='<%# Eval("Id") %>' class ="checkbox margin-right5" />
                                                                    <span style="color:#034FB6;"><%# Eval("Make") %></span>
                                                                        </li>
                                                                </itemtemplate>
                                                </asp:repeater>
                                            </ul>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div id="dealerStock" style="padding-top: 3px;">
                                        <div class="clear"></div>
                                    </div>
                                    <div id="loadingmsg" class="hide">
                                        <div class="loading-popup">
                                            <span class="loading-icon"></span>
                                            <p style="font-size: 13px; color: blue;">Please wait, we are fetching the results for you...</p>
                                            <div class="clear"></div>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clear"></div>

                    <div class="grid-12 margin-top20">
                        <div class="content-box-shadow content-inner-block-10">
                            <input name="citylatlng" type="hidden" id="citylatlng" runat="server" />
                            <input name="citieslatlng" type="hidden" id="citieslatlng" runat="server" />
                            <div class=" margin-top10 border-solid-bottom padding-bottom10" id="about" runat="server">
                                <div id="aboutus">
                                    <div class="content-inner-block">
                                        <h2 class="font18">About Us</h2>
                                    </div>
                                    <div class="white-shadow content-inner-block">
                                        <div id="divvideo" style="overflow: hidden; float: right;" runat="server" visible="false">
                                            <iframe id="iframevideo" width="400" height="250" src="" frameborder="0" allowfullscreen runat="server" />
                                        </div>
                                        <div id="divabt" style="overflow: hidden;">
                                            <asp:literal id="ltAboutus" visible="false" runat="server"></asp:literal>
                                        </div>
                                        <span id="spnRead" class="rightfloat"><a>Read More</a></span>
                                        <span id="spnLess" class="rightfloat hide"><a>Show Less</a></span>
                                    </div>
                                </div>
                                <div id="facilities">
                                    <div class="content-inner-block">
                                        <h2 class="font18">Facilities</h2>
                                    </div>
                                    <div id="divfacilities" class="white-shadow content-inner-block">
                                        <asp:label id="lblFicilities" class="lbl" runat="server"></asp:label>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>

                            <div id="locateus">
                                <div class="grid-9 padding-top10">
                                    <h2 class="font18">Locate Us</h2>
                                    <div class="">
                                        <div class="grid-5 alpha">
                                            <div>
                                                <div class="margin-top5">
                                                    <p class="margin-top5">
                                                        <asp:literal id="ltAddress1" runat="server"></asp:literal>
                                                        <asp:literal id="ltAddress2" runat="server"></asp:literal>
                                                    </p>
                                                    <p class="margin-top5">
                                                        <asp:literal id="ltArea" runat="server"></asp:literal>
                                                    </p>
                                                    <p class="margin-top5">
                                                        <asp:literal id="ltCity" runat="server"></asp:literal>
                                                    </p>
                                                    <p class="margin-top5">
                                                        <asp:literal id="ltState" runat="server"></asp:literal>
                                                        -
                                                                <asp:literal id="ltPin" runat="server"></asp:literal>
                                                    </p>
                                                    <p class="margin-top5">
                                                       <% if (!String.IsNullOrWhiteSpace(dealerWebsite))
                                                       {%>
                                                            <a href="<%= dealerWebsite.IndexOf("http", StringComparison.OrdinalIgnoreCase) == 0 ? dealerWebsite :"http://" + dealerWebsite%>" target="_blank">Visit Website</a>
                                                    <%} %>
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="grid-3 omega">
                                            <div class="">
                                                <div id="map_canvas" class="margin-left10">
                                                </div>
                                            </div>
                                        </div>
                            <div class="clear"></div>
                                    </div>
                                </div>
                                <div class="grid-3 padding-top10">
                                    <div id="contactdealer" style="width: 100%;">
                                        <h2 class="font18">Ask Dealer to Call You</h2>
                                        <span id="commonError" class="hide">All fields are mandatory</span>
                                        <div>
                                            <input id="txtName" class="form-control margin-top10" placeholder="Name" type="text" />
                                        </div>
                                        <div>
                                            <input id="txtMobile" class="form-control margin-top5" maxlength="15" placeholder="Mobile" type="text" />
                                        </div>
                                        <div>
                                            <div class="leftfloat margin-right5 position-rel">
                                                <input name="txtsDate" type="text" id="txtsDate" style="width: 76px; padding:10px 5px;" placeholder="Date" class="margin-top5 leftfloat  form-control" readonly="true" /></div>
                                            <div class="leftfloat margin-right5 margin-top5">
                                                <select id="drpHrs" style="width: 60px; padding-right: 10px !important;" class="drpClass form-control" placeholder="Hr" runat="server"></select></div>
                                            <div class="leftfloat margin-top5 margin-right5">
                                                <select id="drpMins" style="width: 60px; padding-right: 10px !important;" class="drpClass form-control" placeholder="Mn" runat="server"></select></div>
                                            <div class="clear"></div>
                                        </div>
                                        <div>
                                            <input id="btnSubmit" class="btn btn-orange margin-top15" type="button" value="Submit" />
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
    </form>
    <%--<link rel="stylesheet" href="/static/css/used-search.css" type="text/css" >--%>
    <script   src="/static/src/jquery.jcarousel.min.js"  type="text/javascript"></script>
    <script   src="/static/src/graybox.js" ></script>
    <script   src="/static/src/datepicker.js" ></script>
    <link rel="stylesheet" href="/static/css/datepicker.css" type="text/css" >
    <script   src="/static/src/dealerlocator.js" ></script>
    <link rel="stylesheet" href="/static/css/dealers.css" type="text/css" >
    <script type="text/javascript">
        var facHeight = 0;
        var height = 0;
        var hei = 0;
        $(document).ready(function () {
            regenerateCode();
            $('#lblCaptcha').hide();
            $('#divSmsSendMsg').hide();

            $('#txtselect').text('Select Make');

            var dId = '<%= DealerId%>';
            var hashParams = "mId=" + '0' + "&dId=" + dId;
            LoadStock(hashParams);
            $('#loadingmsg').hide();

            var latlongArray = new Array();
            var latlng = $("#citieslatlng").val().split('$');
            var cityCoord = $("#citylatlng").val();
            for (var i = 0; i < latlng.length; ++i) {
                latlongArray.push(latlng[i]);
            }
            Common.utils.loadGoogleApi(preGoogleApiUsedCars, {
                latlongArray: latlng,
                cityCoord: cityCoord,
                imgPath: 'http://oem.carwale.com/skoda/dealernw/img/bullet-big.png'
            });

            //height = $('#divabt').css('height');

            //if ('<%= videoExists%>' == 'True') {
            //$('#divabt').css('overflow', '');
            //hei = $('#divabt').css('height');
            //$('#divabt').css('overflow', 'hidden');
            //hei = parseInt(hei.replace('px', ''));
        //}

        facHeight = $('#divfacilities').css('height');

        //height = parseInt(height.replace('px', ''));
        //facHeight = parseInt(facHeight.replace('px', ''));

        if (height < 100 && facHeight < 100) {
            //$('#divabt').css('height', facHeight + 'px');
            $('#divfacilities').css('height', facHeight + 'px');
            $('#spnRead').css('display', 'none');
        }

        if (facHeight > 250) {
            //$('#divabt').css('height', facHeight + 'px');
        }
        else if ('<%= videoExists%>' == 'True') {
            facHeight = 250;
            //$('#divabt').css('height', facHeight + 'px');
        }
        else {
            facHeight = 100;
            //$('#divabt').css('height', facHeight + 'px');
        }

            if (height <= facHeight) {
                $('#spnRead').css('display', 'none');
            }

            //google.maps.event.addDomListener(window, 'load', initialize);
        });

    $('#lnkUsed').live('click', function () {
        var dealerCity = '<%= DealerCity %>';
    });
        function regenerateCode() {
            $("#captchaCode").attr("src", "/Common/CaptchaImage/JpegImage.aspx");
        }

        $(window).load(function () {
            $("#txtsDate").datepicker({
                showOn: 'both', buttonImage: 'https://img.carwale.com/newdealers/calender.png', buttonImageOnly: true, minDate: 0,
                changeMonth: true, changeYear: true, dateFormat: 'mm/dd/yy',
                onSelect: function (selected) {
                }
            });
        });

        $('#spnRead').click(function () {
            if ('<%= videoExists%>' == 'True') {
                //$('#divabt').css('overflow', '');
                //$('#divabt').animate({
                //    height: hei + 'px'
                //}, 10, function () {
                //});
            }
            else {
                //$('#divabt').css('overflow', '');
                //$('#divabt').animate({
                //    height: height + 'px'
                //}, 10, function () {
                //});
            }
            //$('#divabt').removeClass('divcontentaboutus');     
            $('#spnRead').hide();
            $('#spnLess').show();
        });

            $('#spnLess').click(function () {
                //$('#divabt').css('overflow', 'hidden');

                //$('#divabt').animate({
                //    height: facHeight + 'px'
                //}, 10, function () {
                //});
                $('#spnRead').show();
                $('#spnLess').hide();
            });

            $("#btncontact").click(function () {
                $('#lblCaptcha').hide();
                var caption = "SMS Contact Details of <br/> <%=DealerName%>";
                var applyIframe = false;
                var x = $('#grayBoxContent').html();
                var GB_Html = x;

                GB_show(caption, url, 300, 480, applyIframe, GB_Html);
            });

            $('#btnSubmit').click(function () {
                var re = /^[0-9]*$/;
                var reName = /^([-a-zA-Z ']*)$/;
                var name = $.trim($('#txtName').val());
                var mobile = $.trim($('#txtMobile').val());
                var txtDate = $('#txtsDate').val();
                var hr = $('#drpHrs').val();
                var Min = $('#drpMins').val();

                $('#commonError').removeClass('smsMsg');

                if (name == "" || name == "Name" || name == "name") {
                    $('#commonError').text("*Please enter name");
                    $('#commonError').css('color', '#ce0001');
                    return false;
                } else if (reName.test(name) == false) {
                    $('#commonError').text("*Please enter only alphabets..");
                    $('#commonError').css('color', '#ce0001');
                    return false;
                }
                else if (name.length == 1) {
                    $('#commonError').text("*Please enter your complete name");
                    $('#commonError').css('color', '#ce0001');
                    return false;
                }

                if (mobile == "" || mobile == "Mobile") {
                    $('#commonError').text("*Please enter your mobile no.");
                    $('#commonError').css('color', '#ce0001');
                    return false;
                } else if (mobile != "" && re.test(mobile) == false) {
                    $('#commonError').text("*Please provide numeric data only in your mobile number.");
                    $('#commonError').css('color', '#ce0001');
                    return false;
                } else if (mobile.length != 10) {
                    $('#commonError').text("*Your mobile number should be of 10 digits.");
                    $('#commonError').css('color', '#ce0001');
                    return false;
                }

                if (txtDate.length == "") {
                    $('#commonError').text('*Please Select Date');
                    $('#commonError').css('color', '#ce0001');
                    return false;
                }

                if (hr == 'Hr') {
                    $('#commonError').text('*Please Select Hrs');
                    $('#commonError').css('color', '#ce0001');
                    return false;
                }

                if (Min == 'Mn') {
                    $('#commonError').text('*Please Select Minutes');
                    $('#commonError').css('color', '#ce0001');
                    return false;
                }

                SendUserRequest(name, mobile, txtDate, hr, Min);

            });

            function SendUserRequest(name, mobile, txtDate, hr, Min) {
                var dealerId = '<%= DealerId%>';
            var dealerEmailId = '<%= DealerEmail%>';
            var dealerName = '<%= DealerName%>';
            var date = txtDate + ' ' + hr + ':' + Min;

            $.ajax({
                type: "POST", url: "/ajaxpro/CarwaleAjax.AjaxDealers,Carwale.ashx",
                data: '{"dealerId":"' + dealerId + '","username":"' + name + '","mobile":"' + mobile + '","comments":"' + date + '","dealerName":"' + dealerName + '","dealerEmail":"' + dealerEmailId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SendUserRequest"); },
                success: function (response) {
                    var jsonString = eval('(' + response + ')');
                    var resObj = eval('(' + jsonString.value + ')');
                    if (resObj) {
                        $('#commonError').text('Request sent successfully to Dealer.');
                        $('#commonError').css('color', '#000000');
                        $('#commonError').addClass('smsMsg');
                        $('#txtMobile').val('');
                        $('#txtName').val('');
                        $('#txtsDate').val('');
                        $('#drpHrs').val('');
                        $('#drpMins').val('');
                    }
                    else {
                        $('#commonError').text('* Please fill required details.');
                        $('#commonError').css('color', '#ce0001');
                    }
                }
            });
        }


        $(".dgNavDivTop a").live('click', function (e) {
            e.preventDefault();
            var navi_lnk = this.href;
            var qs = navi_lnk.split("?")[1];
            LoadStock(qs);
        });

        $('.checkbox').click(function () {
            var arr = new Array();
            var selected;
            $('.checkbox:checked').each(function () {
                arr.push($(this).attr('id'));
                selected = arr.join(',') + ",";
            });
            var dId = '<%=DealerId%>';
        var hashParams = "mId=" + selected + "&dId=" + dId;
        LoadStock(hashParams);
    });

    $('#txtselect').live('click', function () {
        $('#make').show();
    });

    $('#txtselect').blur(function () {
        $('#make').show();
    });

    $(document).mouseup(function (e) {
        var container = $("#make");

        if (!container.is(e.target) // if the target of the click isn't the container...
            && container.has(e.target).length === 0) // ... nor a descendant of the container
        {
            container.css('display', 'none');
        }
    });

    $('#imgarrow').click(function () {
        $('#make').show();
    });

    var re = /^\s*$/

    function LoadStock(params) {
        $('#loadingmsg').show();
        $("#dealerStock").load("/dealer/dealerstockdetails.aspx?" + params, function () {
            $('#loadingmsg').hide();
        });
    }
    </script>
<style>
    .ui-datepicker-trigger {margin-left: -17px; position:absolute; top:4px; right:0;}
    .margin {margin-right: 40px;}
    div#nav2 {border-bottom: none !important;}
    div#nav2 .selected {border-top-color: #FFFFFF;border-left-color: #FFFFFF;border-right-color: #FFFFFF;}
    .white_contener {background-color: transparent !important;}
    .block-space {border: none !important;}
    .jcarousel-next-horizontal {right: -21px !important;}
    .jcarousel-prev-horizontal {left: 4px !important;}
    .content_padding {padding: 0px !important;}
    .content-block-white {margin-bottom: 0px !important;}
    .content-inner-block h2 {color: #1f1a17 !important;}
    .button-on {background-color: Red;}
    #tbl_res {border: none !important;}
    .drpMake {margin-left: 15px;margin-top: inherit;}
    .locatedealer {border-left: 1px solid #e2e2e2;height: 40px;position: absolute;margin-top: -10px;right: 0px;}
    /* paging navigation */
    span.pg {padding: 2px 5px;border: 1px solid #A3B5D9;margin: 0 2px;}
    span.pgSel {background-color: #CCDBF8;padding: 2px 5px;border: 1px solid #A3B5D9;margin: 0 2px;color: #5B5B5B;font-weight: bold;}
    span.pgEnd {padding: 2px 5px;border: 1px solid #ABABAB;margin: 0 2px;color: #898989;cursor: default;}
    .dgNavDiv td {padding: 20px 0 10px 0;background-color: #F6F6F6;border: 0;}
    .dgNavDivTop a:hover {text-decoration: none;}
    .dgNavDivTop td {padding: 10px;background: url(https://imgd.aeplcdn.com/0x0/statics/used/search_head.gif) repeat-x;}
    #map_canvas {width: 390px;}
    .smsMsg {font-size: 11px;}
    #tbl_res {box-sizing: border-box;-webkit-box-sizing: border-box;}
    #ul-stock li.stock-item {width: 316px;}
    #banner .jcarousel-skin-tango .jcarousel-clip-horizontal, #banner-full .jcarousel-skin-tango .jcarousel-clip-vertical {height: 300px;}
    .text-box {height: auto;}
    .drpMake {height: auto;}
    #facilities {padding-left:10px;}
    #aboutus {padding-right:10px;}
</style>
</body>
</html>
