<%@ Page Language="C#" ContentType="text/html" Inherits="MobileWeb.Forums.CreateThread"  AutoEventWireup="false" trace="false" %>
<%
	Title = "Forums: Create New Thread - CarWale";
    MenuIndex = "7";
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" itemscope itemtype="http://schema.org/WebPage">
<head>
<!-- #include file="/m/includes/global/head-script.aspx" -->
</head>

<body>
    <!-- #include file="/m/includes/header.aspx" -->
	<!--Outer div starts here-->
	    <section class="container">
    	<!--Main container starts here-->
    	<div id="main-container">
            <div class="grid-12">
            <div id="br-cr" class="margin-top10 margin-bottom10"><a href="/m/forums/" class="normal">Forums</a> &rsaquo; <a href='/m/forums/<%=hdnThreadUrl.Value.ToString()%>/' class="normal"><%=hdnSubCatName.Value.ToString()%></a></div>
            <h1 id="pghead" class="pgsubhead margin-bottom10 m-special-skin-text">Start New Discussion</h1>
            <form runat="server">
                <asp:Panel id="pnlError" runat="server" Visible="false" style="color:red;font-weight:bold;font-size:13px;margin-bottom:5px;margin-top:5px;">
                    Error occured. Please try again.
                </asp:Panel>
                <div id="divCreateForm"  class="box content-inner-block-10 content-box-shadow rounded-corner2 margin-bottom10" runat="server" visible="true">
                    <div>Title&nbsp;&nbsp;<span id="spnTitle" class="error" style="display:none;">(Required)</span></div>
                    <div class="new-line5"><asp:TextBox id="txtTitle" class="form-control" runat="server" /></div>
                    <div class="margin-top10">Description&nbsp;&nbsp;<span id="spnDesc" class="error" style="display:none;">(Required)</span></div>
                    <div class="new-line5"><asp:TextBox id="txtDesc" class="form-control" runat="server" TextMode="MultiLine" style="height:75px;" /></div>
                  
                    <% if (Convert.ToInt32(hdnPostCount.Value) < 50)
                       {
                     %>
                    <div id="divCapcha" class="margin-top10"><iframe id="captchaCode" width="200" scrolling="no" height="55" frameborder="0" src="JpegImage.aspx"></iframe></div>
                    <div class="new-line10">Enter the code shown above&nbsp;&nbsp;<span id="spnCapcha" class="error" style="display:none;">(Code is required)</span></div>
                    <div class="new-line5"><asp:TextBox id="txtCapcha" class="form-control" runat="server"></asp:TextBox></div>   
                    <div class="new-line5">(If you can't read it: <span style="color:rgb(3,79,182);" onclick="ReloadCapcha()">Regenerate Code </span>)</div>
                    <% } %>
                    <div style="color:red;font-weight:bold;font-size:13px;margin-bottom:5px;margin-top:5px;"><asp:Label id="lblCaptchaError" runat="server" text=""></asp:Label></div>        
                    <div class="new-line10" class="f-10">
                        <table class="table" cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="20" valign="top" id="chk-thread"><asp:CheckBox id="chkAlert" runat="server" data-role="none" /></td>
                                <td>Whenever someone replies to this thread, notify me by sending an email</td>
                            </tr>
                        </table>
                    </div>
                    <div class="margin-top10">
                        <asp:LinkButton id="btnSave" runat="server" class="linkButtonBig btn btn-xs btn-orange">&nbsp;&nbsp;&nbsp;&nbsp;Save&nbsp;&nbsp;&nbsp;&nbsp;</asp:LinkButton>&nbsp;&nbsp;
                        <span id="btnCancel" class="button" style="display:none;">&nbsp;&nbsp;&nbsp;&nbsp;Cancel&nbsp;&nbsp;&nbsp;&nbsp;</span>	
                        <asp:HiddenField id="hdnPostCount" value="0" runat="server"></asp:HiddenField>	
                        <asp:HiddenField id="hdnLatestPostTime" runat="server"></asp:HiddenField>
                    </div>
                </div>
                <div style="color:red;font-weight:bold;font-size:13px;margin-bottom:5px;margin-top:5px;"><asp:Label id="errorMsg" runat="server" text=""></asp:Label></div>
                <div id="divBannedMsg" class="box" style="padding-top:5px;padding-bottom:5px;margin-top:10px;" runat="server" visible="false">
                    Your 'CarWale Forum' membership has been suspended. You cannot post in CarWale Forums anymore. If it looks like a mistake to you, please write to banwari@carwale.com.<br><br>
                    <asp:HiddenField id="hdnForumId" runat="server" />
                </div>
                <asp:HiddenField id="hdnSubCatName" runat="server" />
                <asp:HiddenField id="hdnThreadUrl" runat="server" />
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
        function Validate() {
            isValid = true;
            $("#spnTitle").hide();
            $("#spnDesc").hide();
            $("#lblCaptchaError").hide();
            if (jQuery.trim($("#txtTitle").val()) == "") {
                isValid = false;
                $("#spnTitle").show();
            }

            if (jQuery.trim($("#txtDesc").val()) == "") {
                isValid = false;
                $("#spnDesc").show();
            }
                    <% if (Convert.ToInt32(hdnPostCount.Value) < 50){ %>
                    if (jQuery.trim($("#txtCapcha").val()) == "") {
                        isValid = false;
                        $("#spnCapcha").show();
                    }
                    <% } %>
                    return isValid;
                }

                function ReloadCapcha() {
                    document.getElementById('captchaCode').contentWindow.location.reload();
                }
            </script>

</body>
</html>