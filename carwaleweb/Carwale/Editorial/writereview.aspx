<%@ Page Language="C#" ValidateRequest="false" Inherits="Carwale.UI.Editorial.UserReviews" AutoEventWireup="false" Trace="false" Debug="false" %>

<%@ Register TagPrefix="Vspl" TagName="RTE" Src="/Controls/RichTextEditor.ascx" %>

<%@ Import Namespace="Carwale.UI.Common" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
    
    <%
    
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,
        // check page, headerCommon.aspx in common folder.

        PageId = 43;
        Title = "Write a Review: " + CarName;
        Description = "Write a review for : " + CarName;
        Keywords = CarName + " reviews";
        Revisit = "15";
        DocumentState = "Static";
        canonical = "https://www.carwale.com/userreviews/";
        noIndex = true;
        AdId = "1396440332273";
        AdPath = "/1017752/ReviewsNews_";
    %>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <script type='text/javascript'>
        googletag.cmd.push(function () {
            googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath %>300x250_BTF', [300, 250], 'div-gpt-ad-<%= AdId %>-1').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
        <% if (Ad643 == true)
           { %>googletag.defineSlot('/7590/CarWale_NewCar/NewCar_Make_Page/NewCar_Model_Page/NewCar_Model_643x65', [643, 65], 'div-gpt-ad-1383197943786-0').addService(googletag.pubads()); <% } %>
            googletag.pubads().setTargeting("<%= targetKey %>", "<%= targetValue %>");
            googletag.pubads().setTargeting("City", "<%= CookiesCustomers.MasterCity.ToString() %>");
            googletag.pubads().setTargeting('UserModelHistory', '<%= CookiesCustomers.UserModelHistory.Replace('~', ',')%>');
            //googletag.pubads().enableSyncRendering();
            googletag.pubads().collapseEmptyDivs();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });
        
    </script>
    
    <script type="text/javascript">
        var isModerator = '<%= isModerator %>' == 'True';
        $(document).ready(function(){
            $('#txtTitle,#txtPros,#txtCons,#ftbDescription,#txtEmail').bt({trigger: ['focus', 'blur'], positions: ['right'], fill: '#ffffee',strokeWidth: 1,strokeStyle: '#666666', spikeLength: 7, width:'250px'});
        });	
    </script>

    <style>
        .blue-block {
            clear: both;
            background-color: #f4f3f3;
            padding: 0 10px 10px;
            border: 1px solid #e5e5e5;
            margin-top: 20px;
        }

        .ul-arrow2 {
            font-size: 12px;
            list-style: none;
        }

            .ul-arrow2 li {
                background: url(https://imgd.aeplcdn.com/0x0/cw-common/ul-arrow.gif) no-repeat top left;
                background-position: 0px 7px;
                padding: 2px 0 2px 7px;
            }
        /*----------------- Share with Friends ---------------------------*/
        .share-page {
            border: 1px solid #CCCCCC;
            background-color: #F3F3F3;
            text-align: right;
            width: 155px;
            margin-top: 10px;
            float: right;
            padding: 5px;
            vertical-align: middle;
            line-height: 100%;
        }

            .share-page span {
                float: left;
                line-height: 15px;
            }

        .share-frnds {
            background: url(https://img.carwale.com/icons/share.gif);
            background-repeat: no-repeat;
            display: inline-block;
        }

        .facebook {
            background-position: 0 0;
            height: 16px;
            width: 20px;
        }

        .twitter {
            background-position: -21px 0;
            height: 16px;
            width: 20px;
        }

        .mail-frnds {
            background-position: -41px 0;
            height: 16px;
            width: 16px;
        }
    </style>
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <section class="container">
            <div class="grid-12 bg-white">
                <div class="padding-bottom10 padding-top10 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
        <div class="clear"></div>
        <section class="bg-light-grey padding-top10 padding-bottom10 no-bg-color">
            <div class="container">
                <div class="grid-12">
                    <asp:HiddenField ID="hdnTest" runat="server"></asp:HiddenField>
                    <div class="breadcrumb">
                        <!-- breadcrumb code starts here -->
                        <ul class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/reviews-news/">Reviews & News</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/<%= UrlRewrite.FormatSpecial( CarMake )%>-cars/"><%= CarMake%></a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/<%= UrlRewrite.FormatSpecial( CarMake )%>-cars/<%= MaskingName %>/"><%=CarModel %></a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/<%= UrlRewrite.FormatSpecial( CarMake )%>-cars/<%= MaskingName %>/userreviews/"><%=CarModel %> Reviews</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Write a Review</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="leftfloat font30 text-black special-skin-text">Write a Review for <%= CarName%></h1>
                    <div class="rightfloat">
                        <!-- #include file="/includes/share.html" -->
                    </div>
                    <div class="clear"></div>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>

                <div class="clear"></div>
                <div class="grid-8">
                    <div class="content-box-shadow content-inner-block-10 margin-bottom10">
                        <p class="text-grey">
                            Please be sure to focus your feedback on the car and how it met your expectations. Please note that your review will be moderated before it goes live!<br>
                            <br>
                            <asp:Label ID="lblMessage" runat="server" Font-Bold="true" ForeColor="#cc0000"></asp:Label>
                        </p>
                        <table width="100%" cellpadding="5" cellspacing="0" border="0" id="tblRatings">
                            <tr style="display: <%=displayVersion%>">
                                <td colspan="2">This review is for which version? <span class="text-red">*</span><br />
                                    <div class="form-control-box grid-6 alpha margin-top10">
                                        <span class="select-box fa fa-angle-down"></span>
                                        <asp:DropDownList ID="drpVersions" CssClass="form-control" runat="server" />
                                    </div>
                                    <div class="clear"></div>
                                    <span id="spnVersions" class="text-red"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <h3 class="hd3">How would you rate this car on following</h3>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">Exterior/Style <font color="red">*</font>
                                    <br />
                                    <img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateST1" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateST2" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateST3" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateST4" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateST5" />
                                    <span id="divRateST" class="text-grey">&nbsp;</span>
                                    <input type="hidden" id="hdnRateST" runat="server" value="0" />
                                    <span id="spnRateST" class="text-red"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">Comfort & Space <font color="red">*</font>
                                    <br />
                                    <img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateCM1" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateCM2" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateCM3" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateCM4" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateCM5" />
                                    <span id="divRateCM" class="text-grey">&nbsp;</span>
                                    <input type="hidden" id="hdnRateCM" runat="server" value="0" />
                                    <span id="spnRateCM" class="text-red"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">Performance<font color="red">*</font><span class="text-grey">(Engine, gearbox & overall)</span><br />
                                    <img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRatePE1" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRatePE2" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRatePE3" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRatePE4" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRatePE5" />
                                    <span id="divRatePE" class="text-grey">&nbsp;</span>
                                    <input type="hidden" id="hdnRatePE" runat="server" value="0" />
                                    <span id="spnRatePE" class="text-red"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">Fuel Economy (mileage) <font color="red">*</font>
                                    <br />
                                    <img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateFE1" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateFE2" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateFE3" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateFE4" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateFE5" />
                                    <span id="divRateFE" class="text-grey">&nbsp;</span>
                                    <input type="hidden" id="hdnRateFE" runat="server" value="0" />
                                    <span id="spnRateFE" class="text-red"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">Value for money/Features <font color="red">*</font>
                                    <br />
                                    <img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateVC1" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateVC2" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateVC3" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateVC4" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateVC5" />
                                    <span id="divRateVC" class="text-grey">&nbsp;</span>
                                    <input type="hidden" id="hdnRateVC" runat="server" value="0" />
                                    <span id="spnRateVC" class="text-red"></span>
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <th>Overall <font color="red">*</font></th>
                                <td>
                                    <div id="divRateOA" class="text-grey">&nbsp;</div>
                                    <img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateOA1" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateOA2" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateOA3" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateOA4" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateOA5" />
                                    <input type="hidden" id="hdnRateOA" runat="server" value="0" /><br>
                                    <span id="spnRateOA" class="text-red"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">Title <font color="red">*</font>
                                    <br />
                                    <div class="grid-6 alpha">
                                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="100" Columns="50" CssClass="text form-control" ToolTip="Max 100 characters." />
                                    </div>
                                    <div class="clear"></div>
                                    <span id="spnTitle" class="text-red"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">Pros<font color="red">*</font><span class="text-grey">(things you like)</span><br />
                                    <div class="grid-6 alpha">
                                        <asp:TextBox ID="txtPros" runat="server" MaxLength="100" Columns="50" CssClass="text form-control" ToolTip="e.g., Good fuel economy, Good style. Max 100 characters" />
                                    </div>
                                    <div class="clear"></div>
                                    <span id="spnPros" class="text-red"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">Cons<font color="red">*</font> <span class="text-grey">(things you don't like)</span><br />
                                    <div class="grid-6 alpha">
                                        <asp:TextBox ID="txtCons" runat="server" MaxLength="100" Columns="50" CssClass="text form-control" ToolTip="e.g., Bad interiors, Less spacious. Max 100 characters." />
                                    </div>
                                    <div class="clear"></div>
                                    <span id="spnCons" class="text-red"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">Detailed Review <font color="red">*</font>
                                    <Vspl:RTE ID="ftbDescription" Rows="15" Cols="20" runat="server" title="Maximum 8000 characters (approx. 2000 words). Minimum 150 words" />
                                    <br />
                                    <span>Maximum 8000 characters (approx. 2000 words). Minimum 150 words.</span><br>
                                    <span id="spnDesc"></span>
                                    <br />
                                    <span id="spnDescription" class="text-red"></span>
                                </td>
                            </tr>
                            <tr>
                                <td class="margin-top10">Purchased as <font color="red">*</font>
                                    <br />
                                    <asp:RadioButton ID="radNew" Text="New" runat="server" GroupName="new" />
                                    &nbsp;
                                    <asp:RadioButton ID="radOld" Text="Used" runat="server" GroupName="new" />
                                    &nbsp;
                                    <asp:RadioButton ID="radNot" Text="Not Purchased" runat="server" GroupName="new" Checked="true" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">Familiarity with the car <font color="red">*</font>
                                    <br />
                                    <div class="form-control-box grid-6 alpha margin-top10">
                                        <span class="select-box fa fa-angle-down"></span>
                                        <asp:DropDownList ID="ddlFamiliar" CssClass="form-control" runat="server">
                                            <asp:ListItem Selected="true" Text="--Select--" Value="0"></asp:ListItem>
                                            <asp:ListItem Selected="false" Text="Haven't driven it" Value="1"></asp:ListItem>
                                            <asp:ListItem Selected="false" Text="Have done a short test-drive once" Value="2"></asp:ListItem>
                                            <asp:ListItem Selected="false" Text="Have driven for a few hundred kilometres" Value="3"></asp:ListItem>
                                            <asp:ListItem Selected="false" Text="Have driven a few thousands kilometres" Value="4"></asp:ListItem>
                                            <asp:ListItem Selected="false" Text="It's my mate since ages" Value="5"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="clear"></div>
                                    <span id="spnFamiliar" class="text-red"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">Fuel Economy (km/l)<br />
                                    <div class="grid-6 alpha">
                                        <asp:TextBox ID="txtMileage" runat="server" MaxLength="100" CssClass="text form-control" Columns="3" />
                                    </div>
                                    <div class="clear"></div>
                                    <span id="spnMileage" class="text-red"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="divEmailLabel" runat="server">Your Email <font color="red">*</font></span>
                                    <div id="divEmail" runat="server">
                                        <div class="grid-6 alpha">
                                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Columns="35" CssClass="text form-control" ToolTip="Max 50 characters." /><br />
                                        </div>
                                        <div class="clear"></div>
                                        <span id="spnEmail" class="text-red"></span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="divNameLabel" runat="server">Your Name <font color="red">*</font></span>
                                    <div id="divName" runat="server">
                                        <div class="grid-6 alpha">
                                            <asp:TextBox ID="txtName" runat="server" MaxLength="50" Columns="35" CssClass="text form-control" ToolTip="Max 50 characters." /><br />
                                        </div>
                                        <div class="clear"></div>
                                        <span id="spnName" class="text-red"></span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button ID="butSave" CssClass="btn btn-orange" runat="server" Text="Post Review" />&nbsp;&nbsp;				
				                    <input type="button" class="btn btn-orange" value="Discard Review" onclick="javascript:location.href='<%= BackUrl%>    '" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="grid-4">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 20, 20, false, 0) %>
                    <div class="">
                        <div class="content-box-shadow content-inner-block-10 margin-bottom10">
                            <h2 class="hd2">Reviews Guidelines</h2>
                            <ul class="ul-arrow2">
                                <li>Be objective and truthful. Tell us how you really feel. The useful reviews include not only whether you liked or disliked Vehicle, but also why. Feel free to mention related items and how this car rates in comparison to them.
				                    Be detailed and specific:
				                    <ul class="ul-arrow-inner">
                                        <li>Did the car meet your expectations?</li>
                                        <li>How does the car compare to other, similar cars in the marketplace with which you have experience?</li>
                                        <li>What features of the car do you like or dislike?</li>
                                        <li>Would you recommend the car to others?</li>
                                    </ul>
                                </li>
                                <li>Your comments should focus on the Vehicle context.</li>
                                <li>Be expressive. Tell people about your experience in details.</li>
                            </ul>
                        </div>
                    </div>
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 0, 20, false, 1) %>
                    <iframe id="ifrKeepAlive" src="/editorial/keepalive.html" frameborder="no" width="0" height="0" runat="server"></iframe>
                    <script language="javascript">
                        var displayVersion = '<%= displayVersion %>';	
                    </script>
                    <script  type="text/javascript"  src="/static/src/write_reviews.js" ></script>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
    </form>
</body>
</html>





