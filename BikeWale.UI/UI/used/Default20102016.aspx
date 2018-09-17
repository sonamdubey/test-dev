<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Used.Default20102016" Trace="false" Debug="false" %>

<%@ Register Src="~/UI/controls/TopUsedListedBike.ascx" TagPrefix="ub" TagName="TopUsedListedBikes" %>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/UI/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="TA" TagName="TipsAdvicesMin" Src="/UI/controls/TipsAdvicesMin.ascx" %>
<%@ Register TagPrefix="US" TagName="UsedBike" Src="~/UI/controls/UsedBikeSearch.ascx" %>
<%--<%@ Register TagPrefix="BB" TagName="BrowseBikes" Src="/UI/controls/browsebikes.ascx" %>--%>
<%
    title = "Used Bikes in India - Buy & Sell Second Hand Bikes";
    keywords = "Used bikes, used bike, used bikes for sale, second hand bikes, buy used bike";
    description = "With more than 10,000 used bikes listed for sale, BikeWale is India's largest source of used bikes in India. Find a second hand bikes or list your bikes for sale.";
    alternate = "https://www.bikewale.com/m/used/";
    AdId = "1475575707820";
    AdPath = " /1017752/BikeWale_UsedBikes_HomePage_";
     is300x250Shown = false;
    isAd970x90Shown = true;
    isAd970x90BottomShown = true;
%>
<!-- #include file="/UI/includes/headUsed.aspx" -->
<style type="text/css">
    .col {
        float: left;
        padding-bottom: 5px;
        width: 100px;
    }

    .tabs-container {
        border: 1px solid #bfbfbf;
        overflow: hidden;
        margin-left: 20px;
    }

        .tabs-container li a {
            border-left: 1px solid #bfbfbf;
            color: #5e5e5e;
            padding: 11px 18px;
            cursor: pointer;
            font-weight: bold;
        }

        .tabs-container li {
            font-weight: bold;
            float: left;
            padding: 10px 0px;
            background: #f5f5f5;
        }

            .tabs-container li:hover {
                background-color: #E2E2E2;
                font-weight: bold;
                float: left;
            }

            .tabs-container li a.first {
                border-left: 0px;
            }

            .tabs-container li a.active-tab {
                color: #898585 !important;
                text-decoration: none;
                font-weight: bold;
                background: #fff;
            }

            .tabs-container li a:hover {
                text-decoration: none;
            }

    .find-used-bikes-container td select {
        width: 195px;
    }
</style>
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li  itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url"><span itemprop="title">Home</span></a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>Used Bikes</strong></li>
        </ul>
        <div class="clear"></div>
    </div>
    <div class="grid_8 margin-top10">
        <!--    Left Container starts here -->
        <h1>Used Bikes <span>Buy &amp; sell used bikes</span></h1>
        <div class="grid_4 alpha grey-bg margin-top15" style="height: 288px;">
            <div class="content-block">
                <h2 class="hd2">Find Used Bikes</h2>
                <p class="margin-top5">Thousands of used bikes listed for sale</p>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="tbl-default margin-top10 find-used-bikes-container">
                    <tr>
                        <td>Your City</td>
                        <td>
                            <asp:dropdownlist id="ddlCity" runat="server" tabindex="1"></asp:dropdownlist>
                        </td>
                    </tr>
                    <tr>
                        <td>Budget</td>
                        <td>
                            <asp:dropdownlist id="ddlPriceRange" runat="server" tabindex="2">
                                    <asp:ListItem Value="-1" Text="-- Any Budget --"></asp:ListItem>
				                    <asp:ListItem Value="0" Text="Up to 10,000"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="10,000-20,000"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="20,000-35,000"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="35,000-50,000"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="50,000-80,000"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="80,000-150,000"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="150,000 or above"></asp:ListItem>   
                                </asp:dropdownlist>
                        </td>
                    </tr>
                    <tr>
                        <td>Bike Make</td>
                        <td>
                            <asp:dropdownlist id="ddlMake" runat="server" tabindex="3"></asp:dropdownlist>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <div>
                                <input type="button" id="btnSearchUsedBike" tabindex="4" runat="server" value="Find Used Bike" class="action-btn text_white" onclick="SearchByCriteria()" />
                            </div>
                            <div id="process_img" class="hide text-highlight">Redirecting Please wait...</div>
                        </td>
                    </tr>
                </table>
                <div class="sept-dashed margin-top10"></div>
                <div class="margin-top15">
                    Search By Profile Id
                        <%--<input type="text" id="txtlocateProfile" style="width:100px;" />&nbsp;<a id="btnLocateProfile" class="buttons">Go</a>--%>

                    <input name="txtProfileId" type="text" value="Profile Id" id="txtProfileId" tabindex="1" placeholder="Profile Id" style="width: 90px;" /><span class="error">*</span>
                    <input name="btnSearchProfileId" type="button" id="btnSearchProfileId" tabindex="2" value="Go" class="buttons text_white btn-xs" onclick="SearchByProfile()" />
                    <div id="spn_txtProfile" class="error"></div>
                </div>
                <div class="margin-top10"></div>
            </div>
        </div>
        <div class="grid_4 omega grey-bg margin-top15">
            <div class="content-block">
                <h2>Sell Your Bike Here</h2>
                <p class="black-text">Sell your bike in a faster and easiest way</p>
                <div class="dotted-line margin-top5"></div>
                <div id="sybh-list" class="padding-top10">
                    <div><a class="person pointer" title="BikeWale team works with you to get you best price for your bike">Get Expert Help</a></div>
                    <div class="sep"></div>
                    <div><a class="timer pointer" title="Your bike is listed for sale until it is sold">Unlimited Time Period</a></div>
                    <div class="sep"></div>
                    <div><a class="watch pointer" title="BikeWale is committed to give your bike maximum exposure">Maximum Visibility</a></div>
                    <div class="sep"></div>
                    <div><a class="award pointer" title="Buyers' mobile numbers are verified before they are sent to you">Genuine Buyers</a></div>
                    <div class="sep"></div>
                </div>
                <div class="action-btn margin-top10 center-align"><a href="/used/sell/">Sell My Bike Now</a></div>
            </div>
        </div>
        <div class="clear"></div>
        <%-- <div class="margin-top15">
                <h2>Browse Used Bikes by City</h2>
                <ul class="ul-hrz margin-top10">
                    <li><a href="/used-bikes-in-newdelhi/">New Delhi</a></li>
                     <li><a href="/used-bikes-in-bangalore/">Bangalore</a></li>
                     <li><a href="/used-bikes-in-pune/">Pune</a></li>
                     <li><a href="/used-bikes-in-ahmedabad/">Ahmedabad</a></li>
                     <li><a href="/used-bikes-in-coimbatore/">Coimbatore</a></li>
                     <li><a href="/used-bikes-in-chandigarh/">Chandigarh</a></li>                 
                     <li><a href="/used-bikes-in-mumbai/">Mumbai</a></li>
                     <li><a href="/used-bikes-in-chennai/">Chennai</a></li>
                     <li><a href="/used-bikes-in-hyderabad/">Hyderabad</a></li>
                     <li><a href="/used-bikes-in-kolkata/">Kolkata</a></li>
                     <li><a href="/used-bikes-in-kochi/">Kochi</a></li>
                     <li><a href="/used-bikes-in-pondicherry/">Pondicherry</a></li>
                </ul>
            </div><div class="clear"></div>--%>
        <US:UsedBike ID="ubSearch" runat="server"></US:UsedBike>
        <div class="grey-bg content-block margin-top15">
            <ub:TopUsedListedBikes ID="ubUsedBikesRecentMin" runat="server" TopRecords="20" DisplayTwoColumn="true"></ub:TopUsedListedBikes>
            <div class="clear"></div>
        </div>
    </div>
    <div class="grid_4 right-grid">
        <%--<div>
            <!-- BikeWale_UsedBike/BikeWale_UsedBike_300x250 -->
            <!-- #include file="/UI/ads/Ad300x250.aspx" -->
        </div>--%>
        <div class="light-grey-bg content-block border-radius5 padding-bottom20 margin-top15">
            <BP:InstantBikePrice ID="getPriceQuoteMin" runat="server" />
        </div>
        <div class="margin-top15">
            <TA:TipsAdvicesMin runat="server" ID="TipsAdvicesMin" />
        </div>
        <div>
                <%--<!-- BikeWale_UsedBike/BikeWale_UsedBike_300x250 -->--%>
                <!-- #include file="/UI/ads/Ad300x250BTF.aspx" -->
        </div>        
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $("#ddlCity option[value='-1']").attr("disabled", "disabled");        
    });


    $("#txtProfileId").focusin(function () {
        if ($.trim($(this).val()) == "Profile Id")
            $(this).val("");
    });

    function SearchByProfile() {
        $("#spn_txtProfile").text("Processing Please wait..");
        var profileId = $.trim($("#txtProfileId").val());
        profileId = profileId.replace(" ", "");
        var customerId = <%=customerId%>;

        if (profileId != "" && profileId != "ProfileId") {
            var profileType = profileId.substring(0, 1);
            var profileIdVal = profileId.substring(1);
            var re = /^[0-9]*$/;
            var profileEx = /^[sS]+$/;

            if (re.test(profileIdVal) && profileEx.test(profileType)) {
                $.ajax({
                    type: "GET",
                    url: "/api/used/inquiry/url/" + profileId + "/" + customerId,
                    headers: { "platformId": 1 },
                    dataType: 'json',
                    success: function (data) {
                        if(data != null)
                        {
                            if (data.isRedirect == false)                           
                                $("#spn_txtProfile").text(data.message);
                            else
                                location.href = data.url;
                        }
                    },
                    complete: function (xhr) {
                        if (xhr.status == 400 || xhr.status == 500) {
                            $("#spn_txtProfile").text('Please enter correct profile id');
                        }
                    }
                });
            } else {
                $("#spn_txtProfile").text("Invalid Profile Id");
                $("#txtProfileId").focus();
            }
        } else {
            $("#spn_txtProfile").text("Enter profile Id");
        }
    }

    function SearchByCriteria() {
        if ($("#ddlCity").val() != "0") {
            var cityId = $("#ddlCity").val().split('_')[0];

            //var city = $("#ddlCity option:selected").text().toLowerCase().replace(/ /g, "");
            //alert(city);
            var city = $("#ddlCity option:selected").val().split('_')[1];
            //alert(cityName);
            var distance = "50";
        }
        else {
            cityId = $("#ddlCity").val();
        }

        var search_href = "";
        var searchCriteria = "";

        //if ($("#ddlCity").val() == "-1") {
        //   $("#spn_City").text("Select city");
        //    $("#ddlCity").focus();
        //    return false;
        //}

        //    var modelId = $("#ddlMakeModel").val();
        //    var makeId = modelId.split(".")[0];

        //var modelId = $("#ddlMake").val();
        var makeId = $("#ddlMake").val().split('_')[0];//modelId// modelId.split(".")[0];
        var make = $("#ddlMake").val().split('_')[1];
        var budget = $("#ddlPriceRange").val();

        if (cityId > 0) {
            if (makeId != "0") {
                if (budget != "-1") {
                    search_href = "/used/" + make + "-bikes-in-" + city + "/";
                    searchCriteria = "make=" + makeId + "&city=" + cityId + "&dist=" + distance + "&budget=" + budget;
                }
                else {
                    search_href = "/used/" + make + "-bikes-in-" + city + "/";
                    searchCriteria = "make=" + makeId + "&city=" + cityId + "&dist=" + distance;
                }
            }
            else {
                if (makeId == "0" && budget != "-1") {
                    search_href = "/used/bikes-in-" + city + "/";
                    searchCriteria = "budget=" + budget + "&city=" + cityId + "&dist=" + distance;
                }
                else {
                    search_href = "/used/bikes-in-" + city + "/";
                    searchCriteria = "city=" + cityId + "&dist=" + distance;
                }
            }
        }
        else {
            if (makeId != "0") {
                if (budget != "-1") {
                    search_href = "/used/" + make + "-bikes-in-india/";
                    searchCriteria = "make=" + makeId + "&budget=" + budget;
                }
                else {
                    search_href = "/used/" + make + "-bikes-in-india/";
                    searchCriteria = "make=" + makeId;
                }
            }
            else {
                if (makeId == "0" && budget != "-1") {
                    search_href = "/used/bikes-in-india/";
                    searchCriteria = "budget=" + budget;
                }
                else {
                    search_href = "/used/bikes-in-india/";
                    //searchCriteria = "";//"city=" + cityId + "&dist=" + distance;
                }
            }
        }

        //else if (makeId == "0" && budget != "-1") {
        //    search_href = "/used-bikes-in-" + city + "/#budget=" + budget;
        //    searchCriteria = "city=" + cityId + "&dist=" + distance + "&budget=" + budget;
        //}
        //else {
        //    search_href = "/used-bikes-in-" + city + "/";
        //    searchCriteria = "city=" + cityId + "&dist=" + distance;
        //}

        $("#btnSearchUsedBike").addClass("hide");
        $("#process_img").removeClass("hide").addClass("show");

        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxClassifiedSearch,Bikewale.ashx",
            data: '{"queryString":"' + searchCriteria + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SaveUsedSearchCriteria"); },
            success: function (response) {
                var status = eval('(' + response + ')');

                if (status.value == true) {
                    location.href = search_href;
                }
                else {
                    $("#btnSearchUsedBike").removeClass("hide").addClass("show");
                }
            }
        });
    }


</script>
<!-- #include file="/UI/includes/footerInner.aspx" -->
