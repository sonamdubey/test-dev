<%@ Page Language="C#" trace="false" validateRequest="false" Inherits="Carwale.UI.Editorial.EditReview" AutoEventWireup="false" %>
<%@ Register TagPrefix="Vspl" TagName="RTE" src="/Controls/RichTextEditor.ascx" %>

<!doctype html>
<html>
<head>
    <%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 43;
	Title 			= "Edit review for : " + CarName;
	Description 	= "Edit review for : " + CarName;
	Keywords		= CarName + " reviews";
	Revisit 		= "15";
	DocumentState 	= "Static";
    %>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <script language="javascript">
        function showCharactersLeft(ftb) {
            var maxSize = 6000;
            var size = ftb.GetHtml().length;

            if (size >= maxSize) {
                ftb.SetHtml(ftb.GetHtml().substring(0, maxSize - 1));
                size = maxSize;
            }

            document.getElementById("spnDesc").innerHTML = "Characters Left : " + (maxSize - size);
        }
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
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/community/">Community</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/reviews-news/">Research</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="reviewdetails.aspx?rid=<%=Request["rid"] %>"><%= CarName%></a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Write Review</li>
                        </ul>
                        <div class="clear"></div>
                    </div>

                    <h1 class="font30 text-black special-skin-text">Edit a Review for <%= CarName%></h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-10">
                        <div>
                            <table width="100%" style="padding-left: 20px;" cellpadding="0" cellspacing="0" id="tblRatings">
                                <tr>
                                    <td valign="bottom" colspan="2" style="padding-right: 14px;">
                                        <div style="margin-top: 7px; margin-bottom: 7px;">
                                            <span style="display: <%=display%>">Please be sure to focus your feedback on the car and how it met your expectations. Please note that your review will be moderated before it goes live.</span><br>
                                            <br>
                                            <asp:Label ID="lblMessage" runat="server" Font-Bold="true" ForeColor="#cc0000"></asp:Label>
                                        </div>
                                    </td>
                                    <% if (!isModerator)
                                    { %>
                                    <td rowspan="10" valign="top">
                                        <div style="margin-left: 1px; width: 260px;">
                                            <div style="background: url(https://img.carwale.com/cw-common/round-bdr-bg.gif) no-repeat scroll 0 -103px;">&nbsp;</div>
                                            <div style="width: 250px; border-left: 1px solid #C1D9E5; border-right: 1px solid #C1D9E5; padding: 4px;">
                                                <h2>CarWale Reviews Guidelines</h2>
                                                <ul class="normal">
                                                    <li>Be objective and truthful. Tell us how you really feel. The useful reviews include not only whether you liked or disliked Vehicle, but also why. Feel free to mention related items and how this car rates in comparison to them.<br />
                                                        Be detailed and specific:<br />
                                                        <ul class="normal">
                                                            <li>Did the car meet your expectations?<br />
                                                            </li>
                                                            <li>How does the car compare to other, similar cars in the marketplace with which you have experience?<br />
                                                            </li>
                                                            <li>What features of the car do you like or dislike?<br />
                                                            </li>
                                                            <li>Would you recommend the car to others?<br />
                                                            </li>
                                                        </ul>
                                                    </li>
                                                    <li>Your comments should focus on the Vehicle context.<br />
                                                    </li>
                                                    <li>Be expressive. Tell people about your experience in details.</li>
                                                </ul>
                                            </div>
                                            <div style="background: url(https://img.carwale.com/cw-common/round-bdr-bg.gif) no-repeat scroll 0 -119px;">&nbsp;</div>
                                        </div>
                                    </td>
                                    <% }
                                    else
                                    { %>
                                    <td>&nbsp;</td>
                                    <% } %>
                                </tr>
                                <tr style="display: <%=display%>">
                                    <td width="<%=leftColumnWidth%>" valign="bottom" align="right">
                                        <br />
                                        How would you rate this car on following</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr style="display: <%=display%>">
                                    <td width="<%=leftColumnWidth%>" valign="bottom" align="right">Exterior/Style <font color="red">*</font></td>
                                    <td style="padding-left: 14px;">
                                        <div id="divRateST" class="rate">&nbsp;</div>
                                        <img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateST1" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateST2" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateST3" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateST4" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateST5" />
                                        <input type="hidden" id="hdnRateST" runat="server" value="0" />
                                        <span id="spnRateST" class="error"></span>
                                    </td>
                                </tr>
                                <tr style="display: <%=display%>">
                                    <td valign="bottom" align="right">Comfort & Space <font color="red">*</font></td>
                                    <td style="padding-left: 14px;">
                                        <div id="divRateCM" class="rate">&nbsp;</div>
                                        <img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateCM1" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateCM2" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateCM3" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateCM4" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateCM5" />
                                        <input type="hidden" id="hdnRateCM" runat="server" value="0" />
                                        <span id="spnRateCM" class="error"></span>
                                    </td>
                                </tr>
                                <tr style="display: <%=display%>">
                                    <td valign="bottom" align="right">Performance (Engine, gearbox & overall) <font color="red">*</font></td>
                                    <td style="padding-left: 14px;">
                                        <div id="divRatePE" class="rate">&nbsp;</div>
                                        <img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRatePE1" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRatePE2" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRatePE3" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRatePE4" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRatePE5" />
                                        <input type="hidden" id="hdnRatePE" runat="server" value="0" />
                                        <span id="spnRatePE" class="error"></span>
                                    </td>
                                </tr>
                                <tr style="display: <%=display%>">
                                    <td valign="bottom" align="right">Fuel Economy (mileage) <font color="red">*</font></td>
                                    <td style="padding-left: 14px;">
                                        <div id="divRateFE" class="rate">&nbsp;</div>
                                        <img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateFE1" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateFE2" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateFE3" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateFE4" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateFE5" />
                                        <input type="hidden" id="hdnRateFE" runat="server" value="0" />
                                        <span id="spnRateFE" class="error"></span>
                                    </td>
                                </tr>
                                <tr style="display: <%=display%>">
                                    <td valign="bottom" align="right">Value for money/Features <font color="red">*</font></td>
                                    <td style="padding-left: 14px;">
                                        <div id="divRateVC" class="rate">&nbsp;</div>
                                        <img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateVC1" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateVC2" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateVC3" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateVC4" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateVC5" />
                                        <input type="hidden" id="hdnRateVC" runat="server" value="0" />
                                        <span id="spnRateVC" class="error"></span>
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td valign="bottom" align="right">Overall <font color="red">*</font></td>
                                    <td style="padding-left: 14px;">
                                        <div id="divRateOA" class="rate">&nbsp;</div>
                                        <img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateOA1" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateOA2" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateOA3" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateOA4" /><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/white.gif" style="cursor: pointer" id="imgRateOA5" />
                                        <input type="hidden" id="hdnRateOA" runat="server" value="0" /><br>
                                        <span id="spnRateOA" class="error"></span>
                                    </td>
                                </tr>
                                <% if (isModerator)
                                { %>
                                <tr>
                                    <td width="<%=leftColumnWidth%>" align="right" valign="middle">&nbsp;</td>
                                    <td style="padding-left: 14px;" valign="middle" colspan="2">
                                        <div style="display: block; width: 100%;">
                                            <table cellpadding="4" cellspacing="0" style="border: 1px solid #D9D9C1; border-collapse: collapse;">
                                                <tr>
                                                    <td style="font-weight: bold;">User's rating </td>
                                                    <td>style : <%=hdnRateST.Value%>,
								                        comfort : <%=hdnRateCM.Value%>,
								                        performance : <%=hdnRatePE.Value%>,
								                        value for money : <%=hdnRateVC.Value%>,
								                        fuel economy : <%=hdnRateFE.Value%>,
								                        overall : <%=hdnRateOA.Value%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font-weight: bold;">Purchased As</td>
                                                    <% if (radNot.Checked)
                                                    {%>
                                                    <td>Not purchased</td>
                                                    <% } %>
                                                    <% else if (radNew.Checked)
                                                    { %>
                                                    <td>New</td>
                                                    <% } %>
                                                    <% else if (radOld.Checked)
                                                    { %>
                                                    <td>Used</td>
                                                    <% } %>
                                                    <% else
                                                    { %>
                                                    <td>&nbsp;</td>
                                                    <% } %>
                                                </tr>
                                                <tr>
                                                    <td style="font-weight: bold;">Familarity</td>
                                                    <% if (ddlFamiliar.SelectedIndex > 0)
                                                    { %>
                                                    <td>
                                                        <%=ddlFamiliar.SelectedItem.Text%>
                                                    </td>
                                                    <% }
                                                    else
                                                    { %>
                                                    <td>&nbsp;
								
                                                    </td>
                                                    <% } %>
                                                </tr>
                                                <tr>
                                                    <td style="font-weight: bold;">Mileage</td>
                                                    <td>
                                                        <%=txtMileage.Text%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <% } %>
                                <tr>
                                    <td width="<%=leftColumnWidth%>" align="right" valign="middle">Title <font color="red">*</font></td>
                                    <td style="padding-left: 14px;" valign="middle">
                                        <br />
                                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="100" Columns="60" CssClass="text form-control" />
                                        <asp:Label ID="lblTitle" runat="server" Visible="false" />
                                        <span id="spnTitle" class="error"></span>
                                        <br />
                                        <span class="example">Max 100 characters.</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="<%=leftColumnWidth%>" align="right" valign="middle">
                                        <% if (!isModerator)
                                        { %>
				                        Pros (things you like) <font color="red">*</font>
                                        <% }
                                               else
                                               { %>
				                        Pros <font color="red">*</font>
                                        <% } %>
                                    </td>
                                    <td style="padding-left: 14px;" valign="middle">
                                        <br />
                                        <asp:TextBox ID="txtPros" runat="server" MaxLength="100" Columns="60" CssClass="text" />
                                        <asp:Label ID="lblPros" runat="server" Visible="false" />
                                        <span id="spnPros" class="error"></span>
                                        <br />
                                        <span>e.g., Good fuel economy, Good style. Max 100 characters.</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="<%=leftColumnWidth%>" align="right" valign="middle">
                                        <% if (!isModerator)
                                        { %>
				                        Cons (things you don’t like) <font color="red">*</font>
                                        <% }
                                               else
                                               { %>
				                        Cons <font color="red">*</font>
                                        <% } %>
                                    </td>
                                    <td style="padding-left: 14px;" valign="middle">
                                        <br>
                                        <asp:TextBox ID="txtCons" runat="server" MaxLength="100" Columns="60" CssClass="text" />
                                        <asp:Label ID="lblCons" runat="server" Visible="false" />
                                        <span id="spnCons" class="error"></span>
                                        <br />
                                        <span>e.g., Bad interiors, Less spacious. Max 100 characters.</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="<%=leftColumnWidth%>" align="right" valign="top">
                                        <br />
                                        Detailed Review <font color="red">*</font></td>
                                    <td style="padding-left: 14px;" valign="middle" colspan="2">
                                        <br />
                                        <Vspl:RTE ID="ftbDescription" Rows="15" Cols="20" runat="server" />
                                        <br>
                                        <asp:Label ID="lblDescription" runat="server" Visible="false" />
                                        <span>Maximum 8000 characters (approx. 2000 words). Minimum 150 words.</span><br>
                                        <span id="spnDesc"></span>
                                        <span id="spnDescription" class="error"></span>
                                    </td>
                                </tr>
                                <% if (isModerator)
                                { %>
                                <tr>
                                    <% }
                                    else
                                    { %>
                                <tr style="display: none;">
                                    <% } %>
                                    <td width="<%=leftColumnWidth%>" align="right" valign="top">
                                        <br />
                                        Update Reason <font color="red">*</font></td>
                                    <td style="padding-left: 14px;" valign="middle" colspan="2">
                                        <br />
                                        <asp:TextBox ID="txtUpdateReason" runat="server" TextMode="MultiLine" Rows="5" />
                                        <span id="spnUpdateReason" class="error"></span>
                                    </td>
                                </tr>
                                <tr style="display: <%=display%>">
                                    <td width="<%=leftColumnWidth%>" align="right" valign="middle">
                                        <br />
                                        Purchased as <font color="red">*</font></td>
                                    <td style="padding-left: 14px;" valign="middle" colspan="2">
                                        <br />
                                        <asp:RadioButton ID="radNew" Text="New" runat="server" GroupName="new" />
                                        &nbsp;
                                        <asp:RadioButton ID="radOld" Text="Used" runat="server" GroupName="new" />
                                        &nbsp;
                                        <asp:RadioButton ID="radNot" Text="Not Purchased" runat="server" GroupName="new" Checked="true" />
                                    </td>
                                </tr>
                                <tr style="display: <%=display%>">
                                    <td width="<%=leftColumnWidth%>" align="right" valign="middle">
                                        <br />
                                        Familiarity with the car  <font color="red">*</font></td>
                                    <td style="padding-left: 14px;" valign="middle" colspan="2">
                                        <br />
                                        <asp:DropDownList ID="ddlFamiliar" runat="server" />
                                        <span id="spnFamiliar" class="error"></span>
                                    </td>
                                </tr>
                                <tr style="display: <%=display%>">
                                    <td width="<%=leftColumnWidth%>" align="right" valign="middle">
                                        <br />
                                        Mileage (km/l)</td>
                                    <td style="padding-left: 14px;" valign="middle" colspan="2">
                                        <br />
                                        <asp:TextBox ID="txtMileage" runat="server" MaxLength="100" CssClass="text" />
                                        <span id="spnMileage" class="error"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="<%=leftColumnWidth%>" align="right" valign="middle">&nbsp;</td>
                                    <td style="padding-left: 14px;" valign="middle" colspan="2">
                                        <br />
                                        <asp:Label ID="lblAuthorId" runat="server" Visible="false" />
                                        <asp:Label ID="lblIsVerified" runat="server" Visible="false" />
                                        <asp:Button ID="butSave" CssClass="btn btn-orange" runat="server" Text="Update Review" />
                                        <input type="button" class="btn btn-orange" value="Cancel" onclick="window.history.back()" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                        </div>
                        <iframe id="ifrKeepAlive" src="/editorial/keepalive.html" frameborder="no" width="0" height="0" runat="server"></iframe>
                    </div>
                </div>

                <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
        <script language="javascript">
            document.getElementById("butSave").onclick = verifyForm;

            function previewReview(e) {
                if (verifyForm() == false)
                    return false;

                document.getElementById("divPreview").style.display = "";
                document.getElementById("divAddEdit").style.display = "none";
                window.scroll(0, 0);
            }

            function verifyForm(e) {
                var isError = false;

                var reEmail = /^[a-z]+(([a-z_0-9]*)|([a-z_0-9]*\.[a-z_0-9]+))*@([a-z_0-9\-]+)((\.[a-z]{3})|((\.[a-z]{2})+)|(\.[a-z]{3}(\.[a-z]{2})+))$/;

                var desc = tinyMCE.get('ftbDescription_txtContent').getContent();
                var descWithoutHtml = "";
                var mydiv = document.createElement("div");
                mydiv.innerHTML = desc;
                var descWithoutHtmlArr = descWithoutHtml.split(" ");

		<% if (!isModerator)
            { %>

                if (document.getElementById("hdnRateST").value == "0") {
                    isError = true;
                    document.getElementById("spnRateST").innerHTML = "Required";
                }
                else
                    document.getElementById("spnRateST").innerHTML = "";

                if (document.getElementById("hdnRateCM").value == "0") {
                    isError = true;
                    document.getElementById("spnRateCM").innerHTML = "Required";
                }
                else
                    document.getElementById("spnRateCM").innerHTML = "";

                if (document.getElementById("hdnRatePE").value == "0") {
                    isError = true;
                    document.getElementById("spnRatePE").innerHTML = "Required";
                }
                else
                    document.getElementById("spnRatePE").innerHTML = "";

                if (document.getElementById("hdnRateVC").value == "0") {
                    isError = true;
                    document.getElementById("spnRateVC").innerHTML = "Required";
                }
                else
                    document.getElementById("spnRateVC").innerHTML = "";

                if (document.getElementById("hdnRateFE").value == "0") {
                    isError = true;
                    document.getElementById("spnRateFE").innerHTML = "Required";
                }
                else
                    document.getElementById("spnRateFE").innerHTML = "";

                if (descWithoutHtmlArr.length < 150) {
                    isError = true;
                    document.getElementById("spnDescription").innerHTML = "Detailed review should contain minimum 150 words";
                }

	    <% } %>

                if (document.all) // IE Stuff
                {
                    descWithoutHtml = mydiv.innerText;

                }
                else // Mozilla does not work with innerText
                {
                    descWithoutHtml = mydiv.textContent;
                }

                if (desc == "") {
                    isError = true;
                    document.getElementById("spnDescription").innerHTML = "Required";
                }
                else if (desc.length > 8000) {
                    isError = true;
                    document.getElementById("spnDescription").innerHTML = "The typed detailed review contains more characters than allowed";
                }
                else
                    document.getElementById("spnDescription").innerHTML = "";

                if ($.trim($("#txtTitle").val()) == "") {
                    isError = true;
                    document.getElementById("spnTitle").innerHTML = "Required";
                }
                else
                    document.getElementById("spnTitle").innerHTML = "";

		<% if (!isModerator)
            { %>
                document.getElementById("spnMileage").innerHTML = "";

                var mileageEntered = trim(document.getElementById("txtMileage").value);
                if (mileageEntered.length != 0) {
                    if (!MileageNumeric(mileageEntered)) {
                        document.getElementById("spnMileage").innerHTML = "Mileage can have only numbers and maximum one decimal";
                        isError = true;
                    }
                }

                if (document.getElementById("ddlFamiliar").value == "0") {
                    isError = true;
                    document.getElementById("spnFamiliar").innerHTML = "Required";
                }
                else
                    document.getElementById("spnFamiliar").innerHTML = "";
		<% } %>

                if (document.getElementById("txtPros").value == "") {
                    isError = true;
                    document.getElementById("spnPros").innerHTML = "Required";
                }
                else
                    document.getElementById("spnPros").innerHTML = "";

                if (document.getElementById("txtCons").value == "") {
                    isError = true;
                    document.getElementById("spnCons").innerHTML = "Required";
                }
                else
                    document.getElementById("spnCons").innerHTML = "";

		<% if (isModerator)
            { %>

                if ($.trim($('#txtUpdateReason').val()) == "") {
                    isError = true;
                    document.getElementById("spnUpdateReason").innerHTML = "Required";
                }
                else
                    document.getElementById("spnUpdateReason").innerHTML = "";

	    <% } %>

                if (isError == true)
                    return false;
            }

            function MileageNumeric(mileageToValidate) {
                var regEx = /^\d+$/;

                var splittedMileage = mileageToValidate.split('.');
                if (splittedMileage.length > 2)
                    return false;

                for (var i = 0; i < splittedMileage.length; i++) {
                    if (trim(splittedMileage[i]).length != 0 && !regEx.test(splittedMileage[i])) {
                        return false;
                    }
                }
                return true;
            }

            function trim(val) {
                var ret = val.replace(/^\s+/, '');
                ret = ret.replace(/\s+$/, '');
                return ret;
            }

            //for changing the images
            var imgs = document.getElementById("tblRatings").getElementsByTagName("img");

            var msgs = new Array("Poor", "Fair", "Good", "Very Good", "Excellent");
            var path = "<%=ImagingFunctions.GetRootImagePath()%>/images/ratings/";
	var imgN = path + "white.gif";
	var imgT = path + "red.gif";
	var imgTOver = path + "greyRed.gif";
	var imgTLess = path + "whiteRed.gif";
	var imgF = path + "white.gif";
	var imgFOver = path + "grey.gif";
	var spnIdIn = "divRate";
	var imgIdIn = "imgRate";
	var hdnIdIn = "hdnRate";

	reInitialize();
	function reInitialize() {
	    for (var i = 1; i <= 6; i++) {
	        var type = getRateType(i);
	        var imgId = imgIdIn + type;
	        var rate = Number(document.getElementById(hdnIdIn + type).value);

	        //show the images
	        for (var j = 1; j <= 5; j++) {
	            if (j <= rate)
	                document.getElementById(imgId + j).src = imgT;
	            else
	                document.getElementById(imgId + j).src = imgF;

	        }
	    }
	}

	function getRateType(val) {
	    var ret = "";
	    switch (val) {
	        case 1:
	            ret = "ST";
	            break;
	        case 2:
	            ret = "CM";
	            break;
	        case 3:
	            ret = "PE";
	            break;
	        case 4:
	            ret = "VC";
	            break;
	        case 5:
	            ret = "FE";
	            break;
	        case 6:
	            ret = "OA";
	            break;
	        default:
	            break;
	    }
	    return ret;
	}

	function mouseHover(e) {
	    var e1 = e ? e.target : event.srcElement;

	    var imgType = e1.id.substring(7, 9);
	    var index = Number(e1.id.substring(9, e1.id.length));

	    var spnId = spnIdIn + imgType;
	    var imgId = imgIdIn + imgType;
	    var rate = Number(document.getElementById(hdnIdIn + imgType).value);

	    document.getElementById(spnId).innerHTML = msgs[index - 1];

	    //show the images
	    for (var i = 1; i <= 5; i++) {
	        if (i <= rate && i <= index)
	            document.getElementById(imgId + i).src = imgTOver;
	        else if (i <= rate && i > index)
	            document.getElementById(imgId + i).src = imgTLess;
	        else if (i > rate && i <= index)
	            document.getElementById(imgId + i).src = imgFOver;
	        else
	            document.getElementById(imgId + i).src = imgN;

	    }
	}

	function mouseOut(e) {
	    var e1 = e ? e.target : event.srcElement;

	    var imgType = e1.id.substring(7, 9);
	    var index = Number(e1.id.substring(9, e1.id.length));


	    var spnId = spnIdIn + imgType;
	    var imgId = imgIdIn + imgType;
	    var rate = Number(document.getElementById(hdnIdIn + imgType).value);

	    document.getElementById(spnId).innerHTML = "&nbsp;";

	    //show the images
	    //show the images
	    for (var i = 1; i <= 5; i++) {
	        if (i <= rate)
	            document.getElementById(imgId + i).src = imgT;
	        else
	            document.getElementById(imgId + i).src = imgF;
	    }
	}

	function imgClick(e) {
	    var e1 = e ? e.target : event.srcElement;

	    var imgType = e1.id.substring(7, 9);
	    var index = Number(e1.id.substring(9, e1.id.length));

	    var imgId = imgIdIn + imgType;
	    //set the value
	    document.getElementById(hdnIdIn + imgType).value = index;
	    //for the images untill the index, set it as true else as false
	    for (var i = 1; i <= 5; i++) {
	        if (i <= index)
	            document.getElementById(imgId + i).src = imgT;
	        else
	            document.getElementById(imgId + i).src = imgF;
	    }


	}

	// create event handlers.
	for (var j = 0; j < imgs.length; j++) {
	    if (imgs[j].id.indexOf("imgRate") != -1) {
	        imgs[j].onmouseover = mouseHover;
	        imgs[j].onmouseout = mouseOut;
	        imgs[j].onclick = imgClick;
	    }
	}
        </script>
    </form>
</body>
</html>
