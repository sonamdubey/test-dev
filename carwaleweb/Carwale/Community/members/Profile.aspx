<%@ Page Language="C#" Inherits="Carwale.UI.Community.MemberProfile" AutoEventWireup="false" Debug="false" Trace="false" %>
<%@ Register TagPrefix="ur" TagName="UserReviews" Src="/Controls/CarwaleReviews.ascx" %>
<%@ Import Namespace="Carwale.UI.Common" %>

<!doctype html>
<html>
<head>
<%
    // Define all the necessary meta-tags info here.
    // To know what are the available parameters,
    // check page, headerCommon.aspx in common folder.
    PageId = 1;
    Title = customerName + "(" + Request["handle"] + ")'s Profile";
    Description = "Profile Reviews are displayed with photo album ,activity summary,posts";
    Keywords = "Photo Albums,Activity Summary,Posts in Forums";
    Revisit = "15";
    DocumentState = "Static";
    canonical = "https://www.carwale.com/community/members/" + Request["handle"] + ".html";
    AdId = "1397027524795";
    AdPath = "/1017752/Carwale_Community_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->
<script type="text/javascript" src="/static/src/graybox.js"></script>
<style type="text/css">
   
    .rp th { font-weight: bold; background: #f0f0f0; padding: 4px; }
    .rp td, .rp th { border: 1px solid #cccccc; font-size: 13px; text-align: right; padding-right: 5px; }
        .rp td ul { margin-left: 0px; padding-left: 20px; }
    h1 { margin-top: 15px; }
    .profileMenu { border-top: 1px solid #eeeeee; border-left: 1px solid #eeeeee; border-right: 1px solid #dddddd; border-bottom: 1px solid #dddddd; background-color: #f9f9f9; }
        .profileMenu div { padding: 5px; border-bottom: 1px solid #eeeeee; }
            .profileMenu div div { padding: 5px; border: 0px; }
    .profile a:hover { text-decoration: underline; color: #cc0000; }
    .lineSep { border-bottom: 1px dashed #D9D9C1; padding: 4px 0; }
    
    .greyboxTop { margin-bottom: 20px; background: #F6F6F6;border: 1px solid #D3D3D3; padding: 12px 10px 15px 8px;}
    .greyboxTop h2 {font-size:16px;}
    #divFeedback p { margin: 10px 0;}
</style>
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
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/forums/">Forums</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><%=customerName%></li>
                        </ul>
                        <div class="clear"></div>
                    </div>                    
                    <h1 class="font30 text-black special-skin-text"><%=customerName%></h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-10">
                    <div class="content-box-shadow content-inner-block-10">
						<div class="">
                            <div id="content">
        
       
                                <div class="note" id="divEditProfile" runat="server" visible="false" align="right" style="margin: 5px 0;">
                                    <strong>Update your profile: <a href="/MyCarWale/EditCustomerDetails.aspx"><b>Name/Place</b></a>
                                        | <a href="../../users/EditUserProfile.aspx"><strong>Profile</strong></a></strong>
                                </div>
                                <div style="width: 170px; float: left">
                                    <%--<div class="imageBorder">
                                        <img id="imgReal" src='' runat="server" /></div>--%>
                                    <div class="profileMenu" style="margin: 10px 0;" align="left">
                                        <div align="left"><%=customerName%></div>
                                        <div align="left"><%= customerCity != "" ? customerCity + ", " + customerState : "" %></div>
                                       <%-- <div align="left">
                                            <strong>Cars owned </strong>
                                            <br>
                                            <%--<div id="divCarsOwned" runat="server"></div>--%>
                                        <%--</div>--%>
                                       <%-- <div visible="false" id="divMessages" runat="server">
                                            <img src="<%=ImagingFunctions.GetRootImagePath()%>/images/icons/new/email.gif"><a href="/community/pm/">Messages</a></div>
                                        <div visible="false" id="divSendMessage" runat="server">--%>
                                        <%--    <img src="<%=ImagingFunctions.GetRootImagePath()%>/images/icons/new/email.gif">--%>
                                           <%-- <a href="/community/pm/sendMsg.aspx?to=<%=communityId%>">Send Message</a></div>--%>
                                        <div>
                                           <%-- <img align="absmiddle" src="<%=ImagingFunctions.GetRootImagePath()%>/images/icons/posts.gif">--%>
                                            <a href="/Forums/Search.aspx?postsby=<%=Carwale.Utility.CarwaleSecurity.EncryptUserId(Convert.ToInt64(customerId))%>">View Forum Posts</a></div>
                                       <%-- <div>--%>
                                            <%--<img align="absmiddle" src="<%=ImagingFunctions.GetRootImagePath()%>/images/icons/albums.gif">
                                            <a href="/community/photos/viewalbums.aspx?userid=<%=Carwale.Utility.CarwaleSecurity.EncryptUserId(Convert.ToInt64(customerId))%>">View Photo Albums</a></div>
                                    </div>--%>

                                    <%--<div style="padding: 3px;" id="divAvtar" runat="server"></div>--%>
                                </div>
                                    </div>
                                <div>
                                    <div class="profile" style="width: 415px; margin-left: 180px;">
                                        <div id="divAboutMe" style="line-height: 1.5; font-size: 12px; font-weight: 400;" runat="server"></div>

                                        <h2>Reviews Written</h2>
                                        <ur:UserReviews ID="urUserReviewsMostRead" ShowComment="false" RetriveBy="MostRecent" runat="server" />

                                      <%--  <h2 class="margin-top10">Photo Albums</h2>
                                        <Carwale:CommunityUserAlbums ID="uaRecentAlbums" runat="server" />--%>

                                        <h2 class="margin-top10">Posts in Forums</h2>
                                        <div visible="false" id="divForums" runat="server">
                                            <asp:repeater id="rptPosts" runat="server">
						                            <headertemplate>
						                            <div>
						                            </headertemplate>
						                            <itemtemplate>
							                            <li><a href="/Forums/viewthread.aspx?thread=<%# DataBinder.Eval(Container.DataItem, "ForumId").ToString() %>&amp;post=<%# DataBinder.Eval(Container.DataItem, "PostId") %>"><%# DataBinder.Eval(Container.DataItem, "Topic") %></a></li>
							                            <div class="lineSep"><%# GetMessage( DataBinder.Eval(Container.DataItem, "Message").ToString() ) %></div>
						                            </itemtemplate>
						                            <footertemplate>
                                        </div>
                                        </footertemplate>
					                            </asp:Repeater>
					                            <div style="padding: 10px 0;">
                                                    <a rel="nofollow" href="/Forums/Search.aspx?postsBy=<%=Carwale.Utility.CarwaleSecurity.EncryptUserId(Convert.ToInt64(customerId))%>"><strong>All posts by member</strong></a> | <a rel="nofollow" href="/Forums/Search.aspx?threadsBy=<%=Carwale.Utility.CarwaleSecurity.EncryptUserId(Convert.ToInt64(customerId))%>"><strong>All discussions started by member</strong></a>
                                                </div>

                                        <h1>Activity Summary</h1>
                                        <table id="tblPointsDetailed" runat="server" class="rp" width="100%" border="1" cellspacing="0">
                                            <tr>
                                                <th>&nbsp;</th>
                                                <th>Posts</th>
                                                <%--<th>Points Earned</th>--%>
                                            </tr>
                                            <%--<tr>
                                                <th>Photos</th>
                                                <td><%=photos %></td>
                                                <td><%=photosTotal %></td>
                                            </tr>--%>
                                            <tr>
                                                <th>Reviews</th>
                                                <td><%=reviews %></td>
                                                <%--<td><%=reviewsTotal %></td>--%>
                                            </tr>
                                            <tr>
                                                <th>Answers</th>
                                                <%--<td><%=answers %></td>
                                                <td><%=answersTotal %></td>--%>
                                            </tr>
                                            <tr>
                                                <th>Forums</th>
                                                <td><%=forums %></td>
                                                <%--<td><%=forumsTotal %></td>--%>
                                            </tr>
                                           <%-- <tr>
                                                <th colspan="2">Total Reward Points</th>
                                                <th><%=total%></th>
                                            </tr>--%>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        
                        </div>
                        <div class="clear"></div>
					</div>
                    <div class="clear"></div>
				</div>
				







				<div class="clear"></div>
            </div>
                <div class="grid-2">
                    <div class="">
						<div class="">
                            <div>
                                        <div class="greyboxTop">
                                            <h2>Community Tools</h2>
                                            <ul class="normal">
                                                <li><a href="/mycarwale/forums/">Forums</a> </li>
                                              <%--  <li><a href="/community/photos/">Photos</a> </li>--%>
                                                <li><a href="/userreviews/" data-cwtccat="ReviewLandingLinkage" data-cwtcact="MemberProfileLinkClick" data-cwtclbl="source=1">Car Reviews</a> </li>
                                                <li class="weiard">&nbsp;</li>
                                            </ul>
                                            <p>
                                                <div style="width: 28px; float: left">
                                                    <img src="<%=ImagingFunctions.GetRootImagePath()%>/images/mail_icon.jpg" alt="mail" height="14" /></div>
                        
                                            </p>
                                             <div class="clear"></div>
                                        </div>
                                    </div>
                            <div class="addbox">
                                <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx("1396440332273", 160, 600, 0, 0, false, 4) %>
                            </div>
                        </div>
					</div>
				</div>
        </section>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
        <script language="javascript">
            function OpenWindowRealImage() {
                var leftPos = (screen.width - 500) / 2;
                var topPos = (screen.height - 470) / 2;

                window.open('UsersRealImage.aspx?real=<%=realImage%>', 'MyWindow', "width=500,height=470,top=" + topPos + ",left=" + leftPos + ",scrollbars=1");
    }
</script>
</form>
</body>
</html>

