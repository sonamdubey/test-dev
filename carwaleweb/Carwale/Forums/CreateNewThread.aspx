<%@ Page Language="C#" Inherits="Carwale.UI.Forums.CreateNewThread" validateRequest="false" trace="false" AutoEventWireup="false" %>
<%@ Register TagPrefix="Vspl" TagName="RTE" src="/Controls/RichTextEditor.ascx" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 305;
	Title 			= "Forums: Create New Thread";
	Description 	= "";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
%>
<!-- #include file="/includes/global/head-script.aspx" -->

<script language="javascript">
<!--
    function showCharactersLeft(ftb)
    {
        var maxSize = 4000;
        var size = ftb.GetHtml().length;
		
        if(size >= maxSize)
        {
            ftb.SetHtml( ftb.GetHtml().substring(0, maxSize -1) );
            size = maxSize;
        }
		
        document.getElementById("spnDesc").innerHTML = "Characters Left : " + (maxSize - size);
    }
    -->
</script>

<link rel="stylesheet" href="/static/css/forums.css" type="text/css" >
</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
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
                            <li><span class="fa fa-angle-right margin-right10"></span>Car Forums</li>
                        </ul>
                        <div class="clear"></div>
                        <h1 class="font30 text-black special-skin-text">Start New Discussion</h1>
                        <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="container">
                <div class="grid-12">
                    <div class="left_container_top content-box-shadow">
	                    <div id="left_container_onethird" class="content-inner-block-10">
		                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="text-red" Font-Bold="true" />
			                    <table class="writePost" border="0" cellpadding="5" cellspacing="0" width="100%">
				                    <tr>
					                    <td colspan="2"><p><%= ForumDescription %></p></td>
				                    </tr>
				                    <tr>
					                    <td colspan="2"></td>
				                    </tr>
				                    <tr>
					                    <td>Title:</td>
					                    <td>
						                    <asp:TextBox ID="txtTopic" CssClass="form-control" runat="server" MaxLength="75" Columns="75" Width="300" />
						                    <span id="spnName" class="text-red"></span>
					                    </td>
				                    </tr>
				                    <tr>
					                    <td width="120" valign="top">Description:</td>
					                    <td class="noPadding noBorder">
						                    <Vspl:RTE id="rteNT" Rows="15" Cols="60" runat="server" />
						                    <span id="spnDesc"></span><br>
						                    <span id="spnDescription" class="text-red"></span>
					                    </td>
				                    </tr>
				                    <tr id="trCustomer" runat="server">
					                    <td>Your Email:*</td>
					                    <td><asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Columns="50" />
						                    <span id="spnEmail" class="text-red"></span>
					                    </td>
				                    </tr>
				                    <tr id="trCaptcha" runat="server">
					                    <td>&nbsp;</td>
					                    <td>
						                    <iframe id="captchaCode" src="/Common/CaptchaImage/JpegImage.aspx" frameborder="0" scrolling="no" width="200" height="55"></iframe><br>
						                    <div>
							                    <strong class="inline-block">Enter the code shown above</strong> <asp:TextBox id="txtCaptcha" CssClass="form-control inline-block margin-left10 margin-right10" style="width:300px;" Columns="6" MaxLength="6" runat="server"></asp:TextBox><span class="inline-block">(If you can't read it: <a onclick="javascript:regenerateCode()">Regenerate Code</a>)</span><br>
							
							                    <asp:Label id="lblCaptcha" CssClass="text-red" runat="server"></asp:Label>
                                                <div class="clear"></div>
						                    </div>
					                    </td>
				                    </tr>
				                    <tr id="trAlert" Visible="false" runat="server">
					                    <td>Email Alert</td>
					                    <td>
						                    <asp:CheckBox ID="chkEmailAlert" Text="Whenever someone replies to this thread, notify me by sending an email" Checked="true" runat="server" />
					                    </td>
				                    </tr>
				                    <tr>
                                        <td>&nbsp;</td>
					                    <td>
						                    <asp:Button ID="butSave" runat="server" Text="Save" CssClass="buttons btn btn-orange" />
						                    <input type="button" class="btn btn-orange" value="Cancel" onClick="javascript:location.href='ViewForums.aspx?forum=<%= forumId%>    '" class="buttons" />
					                    </td>
				                    </tr>
			                    </table>
		                    
	                    </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
         <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
        <script type="text/javascript">
            Common.showCityPopup = false;
            document.getElementById("butSave").onclick = verifyForm;
	
            function verifyForm(e)
            {
                var isError = false;
		
                if(document.getElementById("txtTopic").value == "")
                {
                    isError = true;
                    document.getElementById("spnName").innerHTML = "Required";
                }
                else
                    document.getElementById("spnName").innerHTML = "";
			
                var desc = tinyMCE.get('rteNT_txtContent').getContent();

                if( desc == "" )
                {
                    isError = true;
                    document.getElementById("spnDescription").innerHTML = "&nbsp;Why blank post? Please do write something!";
                }
	
                else
                    document.getElementById("spnDescription").innerHTML = "";
		
		
                if ( document.getElementById('txtEmail') && document.getElementById('txtEmail').value.length == 0 )
                {
                    isError = true;
                    document.getElementById("spnEmail").innerHTML = "&nbsp;Email needed!";
                }	
		
                if ( document.getElementById('txtCaptcha'))
                {
                    if ( document.getElementById('txtCaptcha').value.length == 0 )
                    {
                        document.getElementById('lblCaptcha').innerHTML = "<br>Code Required";	
                        return false;
                    }
                    else
                    {
                        document.getElementById('lblCaptcha').innerHTML = "";	
                    }
                }
		
                if(isError == true)
                    return false;
            }
            function regenerateCode(){
                $("#captchaCode").attr("src", "/Common/CaptchaImage/JpegImage.aspx");
            }


</script>
    </form>
</body>
</html>

<%--<div class="right_container">
	<div class="addbox"><%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx("1396440332273", 160, 600, 0, 0, false, 4) %></div>
</div>
<iframe id="ifrKeepAlive" src="/editorial/keepalive.html" frameBorder="no" width="0" height="0" runat="server"></iframe>--%>

