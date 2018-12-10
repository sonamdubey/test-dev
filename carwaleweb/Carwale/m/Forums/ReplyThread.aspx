<%@ Page Language="C#" ContentType="text/html" Inherits="MobileWeb.Forums.ReplyThread"  AutoEventWireup="false" trace="false" %>
<%
	Title = "Forums: Reply To Thread - CarWale";
    MenuIndex = "7";
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" itemscope itemtype="http://schema.org/WebPage">
<head>
<!-- #include file="/m/includes/global/head-script.aspx" -->
</head>

<body class="m-special-skin-body m-no-bg-color">
    <!-- #include file="/m/includes/header.aspx" -->
	<!--Outer div starts here-->
	
        <section class="container">
    	<!--Main container starts here-->
    	<div id="main-container">
		    <div class="grid-12">
            <div id="br-cr" class="margin-top10 margin-bottom10">
                <a href="/m/forums/" class="normal">Forums</a>
                »
                <%--<a href="/m/forums/viewforum-<%=forumSubCatId%>.html" class="normal"><%=forumSubCatName%></a>--%>
                <a href='/m/forums/<%=hdnThreadUrl.Value.ToString()%>/' class="normal"><%=hdnForumSubCatName.Value.ToString()%></a>
                »
                <%--<a href="/m/forums/viewthread-<%=threadId%>.html" class="normal"><%=topic%></a>--%>
                <a class="normal" href='/m/forums/<%=hdnThreadId.Value.ToString()%>-<%=hdnPostUrl.Value.ToString()%>/.html'><%=hdnTopic.Value.ToString()%></a>
                »
                Reply to thread
            </div>
            <h1 class="pgsubhead margin-bottom10 m-special-skin-text">Reply to thread</h1>
            <form runat="server">
            <div id="divReplyForm" class="box content-inner-block-10 content-box-shadow rounded-corner2 margin-bottom20" runat="server" visible="true">
                <div class="margin-bottom10"><asp:TextBox id="txtReply"  CssClass="form-control" runat="server" TextMode="MultiLine" Rows="2" /></div>
                <%if((Request.QueryString["quote"] != null && Request.QueryString["quote"] != "") || (Request.Cookies["Forum_MultiQuotes"] != null && Request.Cookies["Forum_MultiQuotes"].Value.ToString() != "")){%>
                <div class="new-line10">Quoted posts:</div>
                <div class="margin-top10 quote"><asp:Label id="lblQuotedPostsVisible" runat="server" Text="" /><asp:Label id="lblQuotedPosts" runat="server" Text="" Visible="false" /></div>
                <div class="new-line10">
                    <table cellpadding="0" cellspacing="0" border="0" style="width:100%">
                        <tr>
                            <td valign="top" style="width:25px;"><asp:CheckBox id="chkQuotedPosts" runat="server" /></td>
                            <td style="padding-left:5px;">Include "Quoted posts" in reply.</td>
                        </tr> 
                    </table>
                </div>
                <%}%>
            
                <div class="new-line10">
                    <table cellpadding="0" cellspacing="0" class="table">
                        <tr>
                            <td valign="top" style="width:25px;"><asp:CheckBox id="chkNotify" runat="server" Checked="true" /></td>
                            <td style="padding-left:5px;">Notify me by email when someone replies to this thread.</td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField id="hdnPostCount" value="0" runat="server"></asp:HiddenField>	
                <asp:HiddenField id="hdnLatestPostTime" runat="server"></asp:HiddenField>
                <% if (Convert.ToInt32(hdnPostCount.Value) < 50) {  %>
                <div id="divCapcha" class="margin-top10"><iframe id="captchaCode" width="200" scrolling="no" height="55" frameborder="0" src="JpegImage.aspx"></iframe></div>
                <div class="margin-bottom10">Enter the code shown above&nbsp;&nbsp;<span id="spnCapcha" class="error" style="display:none;">(Code is required)</span></div>
                <div class="margin-bottom5"><asp:TextBox id="txtCapcha" CssClass="form-control" runat="server"></asp:TextBox></div>   
                <div class="">(If you can't read it: <span style="color:rgb(3,79,182);" onclick="ReloadCapcha()">Regenerate Code </span>)</div>
                <% } %>
                <div style="color:red;font-weight:bold;font-size:13px;margin-bottom:5px;margin-top:5px;"><asp:Label id="lblCaptchaError" runat="server" text=""></asp:Label></div>
                
                <div class="padding-top10 padding-bottom10"><asp:LinkButton id="btnSave" runat="server" class="btn btn-xs btn-orange" Text="&nbsp;&nbsp;&nbsp;&nbsp;Save&nbsp;&nbsp;&nbsp;&nbsp;" />&nbsp;&nbsp; <span id="spnReply" class="error" style="display:none;"></span><asp:Label id="lblServerError" runat="server" Visible="false" class="error" /></div>
            </div>
            
            <div style="color:red;font-weight:bold;font-size:13px;margin:5px;"><asp:Label id="errorMsg" runat="server" text=""></asp:Label></div>
            
            <div id="divBannedMsg" class="box" style="padding-top:5px;padding-bottom:5px;margin-top:10px;" runat="server" visible="false">
                Your 'CarWale Forum' membership has been suspended. You cannot post in CarWale Forums anymore. If it looks like a mistake to you, please write to banwari@carwale.com.
                <asp:HiddenField id="hdnCustomerId" runat="server" />
                <asp:HiddenField id="hdnThreadId" runat="server" />
                <asp:HiddenField id="hdnThreadTopic" runat="server" />
                <asp:HiddenField id="hdnThreadUrl" runat="server" />
                <asp:HiddenField id="hdnPostUrl" runat="server" />
                <asp:HiddenField id="hdnForumSubCatName" runat="server" />
                <asp:HiddenField id="hdnTopic" runat="server" />
            </div>
            </form>
            
            </div>
            <div class="clear"></div>
        </div>
        <!--Main container ends here-->
        </section>
        <div class="clear"></div>
    
    <!--Outer div ends here-->

    <!-- #include file="/m/includes/footer.aspx" -->
	<!-- #include file="/m/includes/global/footer-script.aspx" -->

    <script language="javascript" type="text/javascript">
        Common.showCityPopup = false;
        $(document).ready(function () {
            $("#lblQuotedPostsVisible a").each(function () {
                $(this).removeAttr("href");
                $(this).html($(this).text());
            });
            $("#lblQuotedPostsVisible img").remove();
        });

        function ReloadCapcha() {
            document.getElementById('captchaCode').contentWindow.location.reload();
        }
        function InputValid() {
            $("#lblCaptchaError").hide();
            var retVal = true;
            $("#spnReply").html("");
            $("#spnReply").hide();

            var _reply = $("#txtReply").val();

            if (_reply == "") {
                retVal = false;
                $("#spnReply").html("(Reply Required)");
                $("#spnReply").show();
            }
            else if (_reply.length > 4000) {
                retVal = false;
                $("#spnReply").html("(Max 4000 characters)");
                $("#spnReply").show();
            }
                    <% if (Convert.ToInt32(hdnPostCount.Value) < 50) {  %>
                    if (jQuery.trim($("#txtCapcha").val()) == "") {
                        retVal = false;
                        $("#spnCapcha").show();
                    }
                    <% } %>
                    return retVal;
                }
            </script>	

</body>
</html>