<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Page Language="C#" Inherits="Carwale.UI.Forums.ReplyToThread" validateRequest="false" Trace="false" AutoEventWireup="false" %>
<%@ Register TagPrefix="Vspl" TagName="RTE" src="/Controls/RichTextEditor.ascx" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 305;
	Title 			= "Forums: Reply To Thread";
	Description 	= "";
	Keywords		= "";
	Revisit 		= "15";
	DocumentState 	= "Static";
	AdId            = "1397024466973";
    AdPath          = "/1017752/Carwale_Forums_";
%>
<!-- #include file="/includes/global/head-script.aspx" -->

<link rel="stylesheet" href="/static/css/forums.css" type="text/css" >

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
<style>
    .writePost {background:#fff;}
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
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/community/">Community</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="./">Forums</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/forums/<%= ForumUrl%>/"><%= ForumName%></a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><a href="/forums/<%= threadId%>-<%= ThreadUrl%>.html"><%= ThreadName%></a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>Reply To Topic</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="text-black font30">Reply to Thread: <%= GetTitle(ThreadName) %></h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
            </div>
            <div class="container">
                <div class="grid-9">
                    <div class="content-box-shadow">
	                <div id="left_container_onethird" class="content-inner-block-10 bg-white">
		                
			                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="error" Font-Bold="true" />
			                <table class="writePost" width="100%" border="0" cellpadding="3" cellspacing="0">
				                <tr>
					                <td colspan="2"></td>					
				                </tr>
				                <tr>
					                <td width="150" valign="top">Reply:</td>
					                <td>
						                <Vspl:RTE id="rteRT" Rows="15" Cols="60" runat="server" />	
						                <span id="spnDesc"></span><span id="spnDescription" class="error"></span>
					                </td>
				                </tr>
				                <tr id="trCustomer" runat="server">
					                <td>Email</td>
					                <td><asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Columns="50" /><span id="spnEmail" class="error"></span></td>						
				                </tr>
				                <tr id="trCaptcha" runat="server">
					                <td>&nbsp;</td>
					                <td>
						                <iframe id="captchaCode" src="/Common/CaptchaImage/JpegImage.aspx" frameborder="0" scrolling="no" width="200" height="55"></iframe><br>
						                <div>
							                <span class="inline-block"><strong>Enter the code shown above</strong></span> <span class="inline-block"><asp:TextBox id="txtCaptcha" Columns="6" MaxLength="6" runat="server" CssClass="form-control" Width="120"></asp:TextBox></span><span class="inline-block margin-left5">(If you can't read it: <a onclick="javascript:regenerateCode()">Regenerate Code</a> )</span><br>
							
							                <asp:Label id="lblCaptcha" CssClass="error" runat="server"></asp:Label>
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
					                <td class="dtHeader" align="center">

						                <asp:Button ID="butSave" CssClass="buttons btn btn-orange" runat="server" Text="Save" />
						                <input type="button" class="buttons btn btn-orange" value="Cancel" onClick="javascript:location.href='<%= threadId%>-<%= ThreadUrl%>.html'" />
					                </td>
				                </tr>
			                </table>
			
			                <div id="divThread" runat="server" class="margin-top10">
				                <asp:Repeater ID="rptThreads" runat="server">
					                <headertemplate>
						                <table border="0" width="100%" class="bdr" cellpadding="5" cellspacing="0">
					                </headertemplate>
					                <itemtemplate>
							                <tr>
								                <td width="20%" valign="top">
									                <%# DataBinder.Eval(Container.DataItem, "PostedBy") %>
								                </td>
								                <td>
									                <%# GetMessage( DataBinder.Eval(Container.DataItem, "Message").ToString()) %>
								                </td>
							                </tr>
					                </itemtemplate>
					                <alternatingitemtemplate>
							                <tr bgcolor="#f0f0f0">
								                <td valign="top">
									                <%# DataBinder.Eval(Container.DataItem, "PostedBy") %>
								                </td>
								                <td>
									                <%# GetMessage( DataBinder.Eval(Container.DataItem, "Message").ToString()) %>
								                </td>
							                </tr>
					                </alternatingitemtemplate>
					                <footertemplate>
						                </table>
					                </footertemplate>
				                </asp:Repeater>
			                </div>
		            </div>
                </div>
                </div>
                <div class="grid-3">
                    <div class="content-box-shadow">
                        <div class="content-inner-block-5">
	                        <div class="addbox"><%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx("1396440332273", 160, 600, 0, 0, false, 4) %></div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <div class="clear"></div>
        <iframe id="ifrKeepAlive" src="/editorial/keepalive.html" frameBorder="no" width="0" height="0" runat="server"></iframe>
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
        <script type="text/javascript">
            Common.showCityPopup = false;
            document.getElementById("butSave").onclick = verifyForm;
	
            function verifyForm(e)
            {
                var isError = false;
		
                var desc = tinyMCE.get('rteRT_txtContent').getContent();
			
                if( desc == "" )
                {
                    isError = true;
                    document.getElementById("spnDescription").innerHTML = "&nbsp;Why blank reply? Please do write something!";
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



