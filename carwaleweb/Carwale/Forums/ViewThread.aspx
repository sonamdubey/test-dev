<%@ Page Language="C#" validateRequest="false" Inherits="Carwale.UI.Forums.ViewThread" AutoEventWireup="false" trace="false" EnableEventValidation="false" %>
<%@ Register TagPrefix="Vspl" TagName="RTE" src="/Controls/RichTextEditor.ascx" %>

<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 305;
	Title 			= ThreadName + ( Request["Page"] != null && Request["Page"] != "" ? " - Page " + Request["Page"] : "" ) + " | " + ForumName + " | Car Forums";
	Description 	= "CarWale ViewThread section displays  Forum Post related details like number of replies and views in numbers. ";
	Keywords		= "Car Forum, Reply, Views";
	Revisit 		= "15";
	DocumentState 	= "Static";
    canonical       = "https://www.carwale.com/forums/" + threadId + "-" + ThreadUrl + ".html";
    AdId            = "1397024466973";
    AdPath          = "/1017752/Carwale_Forums_";
    prevPageUrl     = prevUrl;
    nextPageUrl     = nextUrl;
    //mobileSiteForumUrl = "https://carwale.com/m/forums/" + threadId + "-" + ThreadUrl +".html";
%>

<!-- #include file="/includes/global/head-script.aspx" -->

<script  type="text/javascript" language="javascript"  src="/static/src/tiny_mce/tiny_mce.js" ></script>
<link rel="alternate" type="text/html" media="handheld" href="https://carwale.com/m/forums/<%= threadId%>-<%= ThreadUrl%>.html" title="Mobile/PDA"/>
<script type='text/javascript'>
    googletag.cmd.push(function () {
        googletag.defineSlot('<%= AdPath %>970x90', [[728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());            
        googletag.defineSlot('<%= AdPath %>160X600', [[120, 240], [120, 600],[160, 600],[160, 900]], 'div-gpt-ad-<%= AdId %>-4').addService(googletag.pubads());
        googletag.pubads().setTargeting('UserModelHistory', '<%= CookiesCustomers.UserModelHistory.Replace('~', ',')%>');
        googletag.pubads().collapseEmptyDivs();
        googletag.pubads().enableSingleRequest();
        googletag.enableServices();
    });
</script>

<% if ( modLogin ) { %>

<script type="text/javascript">
    function EditPost( postId ){
        window.open('EditPost.aspx?postId=' + postId+'&thread=' + <%=threadId%>+'&page='+<%=(Request["Page"] != null && Request["Page"] != "" ? Request["Page"] : "1")%>, 'editPost', 'menu=no,address=no,scrollbars=no,width=600,height=425');
	}
	
	function MoveThread( threadId ){
		window.open('MoveThread.aspx?threadId=' + threadId, 'moveThread', 'menu=no,address=no,scrollbars=yes,width=525,height=190');
	}
	
	function SplitPost( threadId ){
	    window.open('SplitPost.aspx?Id=' + threadId + '&threadId=' + <%=threadId%> + '&ForumId=' + <%= ForumId%>+'&page='+<%=(Request["Page"] != null && Request["Page"] != "" ? Request["Page"] : "1")%>, 'splitThread', 'menu=no,address=no,scrollbars=yes,width=525,height=190');
	}
    

</script>
<% } %>
<script  type="text/javascript"  src="/static/src/graybox.js" ></script>
<link rel="stylesheet" href="/static/css/forums.css" type="text/css" >
<style>

.ac {padding:3px;}
.iac {padding:3px;}
.message p { padding:5px 0; }
.msgBox ul { margin:10px 0 10px 10px; } 
.msgBox .message { width:570px; overflow:auto; }
.readable ul li { list-style-type:square; } 
.readable img{border:none;background-color:#FFFFFF;vertical-align:middle;}

</style>
</head>
<body class="header-fixed-inner bg-white special-page special-skin-body">

		
		
	<form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <section>
            <div class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
            </div>
        </section>
        <div class="clear"></div>
        <section class="bg-light-grey ">
        <div id="div_load" class="left_container_top container">
            <div class="grid-12 padding-top20">
                <ul class="breadcrumb margin-bottom20"><li><a href="/">Home</a></li>
                <li><span class="fa fa-angle-right margin-right10"></span><a href="/forums/">Forums</a></li>
                <li><span class="fa fa-angle-right margin-right10"></span><a href="<%= ForumUrl%>/"><%= ForumName%></a></li>
                <li class="current"><span class="fa fa-angle-right margin-right10"></span><strong><%= ThreadName%></strong></li></ul>
            </div>
        </div>
        </section>
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
            <div class="container">
	        <div id="left_container_onethird" class="grid-9">
                <div class="content-box-shadow content-inner-block-10">
                    <div style="padding:5px;" align="right">
			            <% if (CurrentUser.Id != "-1") { %>
			            <label id="lblMsg"></label><asp:HyperLink ID=hyplnk runat="server" style="cursor:pointer" /> <asp:Label id="lblBrk" runat="server"> | </asp:Label>
			            <a href="/forums/Subscriptions.aspx"><b>My Subscriptions</b></a> | 
			            <a href="/forums/Search.aspx?get=new"><b>New Posts</b></a> | 
			            <% } %>
			            <a rel="noindex, nofollow" title="View today's posts" href="/forums/Search.aspx?get=today"><b>Today's Posts</b></a> |
			            <a title="Search discussions" href="/forums/Search.aspx"><b>Search Forums</b></a> |					
		            </div>
                    <input type="hidden" id="hdnCompSelected" runat="server" />
		            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="error" Font-Bold="true" />
		            <div id="divThread" runat="server">
			            <h1 class="forums-hd1"><%= GetTitle(ThreadName) %></h1>
			            <div class="footerStrip" id="divStripTop" Visible="false" align="right" runat="server"></div>
			            <asp:Repeater ID="rptThreads" runat="server">
				            <headertemplate>
				            <div></div>
				            </headertemplate>
				            <itemtemplate>
					            <%# ConcatenatePostedByUserIds(DataBinder.Eval( Container.DataItem, "PostedById").ToString()) %>
					            <%# ConcatenatePostIds(DataBinder.Eval( Container.DataItem, "Id").ToString()) %>
					            <div class="msgBox">
						            <div class="postHead">
							            <span style="float:right;">#<a title="Permanent link to this post" href="#post<%# DataBinder.Eval( Container.DataItem, "Id") %>"><%# ++serial %></a>
							            <% if (modLogin) { %>
							            <asp:label ID="lblId" Text='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Visible="false" runat="server" />
							            <input id='chkItem' runat="server" value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' type="checkbox" onClick="javascript:prepareChecks(this)" />
							            <% } %>
							            </span>
							            <span><a name="post<%# DataBinder.Eval( Container.DataItem, "Id") %>"></a>
								            <%# GetDateTime( DataBinder.Eval(Container.DataItem, "MsgDateTime").ToString()) %>
							            </span>
						            </div>
						            <table width="100%" cellpadding="0" cellspacing="0">
							            <tr>
								            <td valign="top" width="175" class="poster" rowspan="3">
									            <div style="margin-bottom:3px;">
										            <img style="display:none;position:relative;top:4px;" src="<%=ImagingFunctions.GetRootImagePath()%>/images/icons/active.gif" height="14px" width="14px" class="imgActive<%# DataBinder.Eval( Container.DataItem, "PostedById") %>" />
										            <%# DataBinder.Eval(Container.DataItem, "HandleName").ToString() == "anonymous" ? "<b>" + DataBinder.Eval(Container.DataItem, "HandleName") + "</b>" : "<a target='_blank' href='/community/members/" + DataBinder.Eval(Container.DataItem, "HandleName")  + ".html'><b>" + DataBinder.Eval(Container.DataItem, "HandleName") + "</b></a>" %>
									            </div>									
									            <div style="margin-bottom:3px;">
										            <span title="Actual member name"><%# DataBinder.Eval(Container.DataItem, "PostedBy").ToString() %></span> 
										            <%# SendPrivateMessage(DataBinder.Eval(Container.DataItem, "PostedById").ToString(), DataBinder.Eval(Container.DataItem, "HandleName").ToString()) %>
									            </div>
									            <div style="margin-bottom:3px;">
										            <b><%# GetUserTitle( DataBinder.Eval(Container.DataItem, "Role").ToString(), DataBinder.Eval(Container.DataItem, "Posts").ToString(), DataBinder.Eval(Container.DataItem, "BannedCust").ToString() ) %></b>
									            </div>
									            <div style="margin-bottom:3px;">
										            <%# DataBinder.Eval(Container.DataItem, "Avtar").ToString() == "" ? "" : "<img src='" + Carwale.Utility.CWConfiguration._imgHostUrl.Replace("http:","https:") + Carwale.Utility.ImageSizes._110X61 + DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString() + "' align='absmiddle' title=\"" + DataBinder.Eval(Container.DataItem, "PostedBy") + "'s avtar\" />" %>			
									            </div>
									            <div style="margin-bottom:3px;">
										            Joined Date: <span><%# DataBinder.Eval(Container.DataItem, "JoiningDate").ToString() %></span>	
									            </div>
									            <div style="margin-bottom:3px;">
										            <%# DataBinder.Eval(Container.DataItem, "City").ToString() != "" ? "Location: <span>" + DataBinder.Eval(Container.DataItem, "City") + "</span>" : "" %>
									            </div>
									            <div style="margin-bottom:3px;">
										            Posts: <span><%# DataBinder.Eval(Container.DataItem, "Posts").ToString() %></span>
									            </div>
									            <div style="margin-bottom:3px;">
										            <%# DataBinder.Eval(Container.DataItem, "HandleName").ToString() == "anonymous" ? "" : "Likes: <span id='spnThanksToUser"+ DataBinder.Eval(Container.DataItem, "Id") +"' userId='"+ DataBinder.Eval(Container.DataItem, "PostedById") +"' >" + DataBinder.Eval(Container.DataItem, "ThanksReceived").ToString() + "</span><br/>" %>
									            </div>
								            </td>
								            <td class="post" valign="top">
									            <div class="message readable">
										            <%# GetMessage( DataBinder.Eval( Container.DataItem, "Message").ToString() ) %>
										            <br><BR>
										            <%# GetSignature(DataBinder.Eval( Container.DataItem, "Signature").ToString() ) %>
									            </div>
								            </td>
							            </tr>
							            <tr>
								            <td style="padding-left:12px;" valign="bottom">
									            <%# DataBinder.Eval(Container.DataItem, "LastUpdatedHandle").ToString() != "anonymous" ? "<div style='font-style:italic;font-size:10px;'>Last Updated: " + GetDateTime( DataBinder.Eval(Container.DataItem, "LastUpdatedTime").ToString() ) + ", by " + DataBinder.Eval(Container.DataItem, "LastUpdatedHandle").ToString() + "</div>" : "" %>
								            </td>
							            </tr>
							            <tr>
								            <td class="post" align="right" style="vertical-align:middle">
									            <div>
										            <table width="100%">
											            <tr>
												            <td width="70%" align="left" valign="middle">
													            <div>
														            <span class="spnThanksRecvCounter"><span id="spnThanksReceived<%# DataBinder.Eval(Container.DataItem, "Id") %>"><a>0</a></span> members liked this post</span>
														            <span class="spnNoThanksRecvCounter" style="display:none;">Like this post</span>
														            <a id="btnThank<%# DataBinder.Eval(Container.DataItem, "Id") %>" onclick="ThankPost('<%# DataBinder.Eval(Container.DataItem, "Id") %>')" rel="nofollow" title="Like this post" <%# CheckOwnerForVisibility(DataBinder.Eval( Container.DataItem, "PostedById").ToString()) %>><img align="absmiddle" src="<%=ImagingFunctions.GetRootImagePath()%>/images/forums/likeforum.jpg" /></a><br/>
														            <span id="spnThankMsg<%# DataBinder.Eval(Container.DataItem, "Id") %>" style="font-weight:bold;"></span>
														            <label id='lblMsgReport<%# DataBinder.Eval(Container.DataItem, "Id") %>'></label> 
													            </div>	
												            </td>
												            <td width="30%" align="right" valign="middle">
													        <%# IsPostEditable ( DataBinder.Eval(Container.DataItem, "MsgDateTime").ToString(), DataBinder.Eval(Container.DataItem, "PostedById").ToString() ) ? "<a href=EditPostByUser.aspx?thread=" + threadId + "&post=" + DataBinder.Eval(Container.DataItem, "Id") + " title='Edit this post'><img align='absmiddle' src='https://img.carwale.com/images/forums/editforum.gif' /></a>" : "" %>
													            <a href="reply.aspx?thread=<%=threadId%>&page=<%= totalNoOfPages%>&quote=<%# DataBinder.Eval(Container.DataItem, "Id") %>" rel="nofollow" title="Quote this message in the reply"><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/forums/quoteforum.jpg" align="absmiddle" /></a>&nbsp;<a threadId='<%# DataBinder.Eval(Container.DataItem, "Id") %>' class='not-sel-thread' onclick='MultiQuoteClicked(this);' title='Add this post in multi quote'><img src="<%=ImagingFunctions.GetRootImagePath()%>/images/forums/multiquote_off.gif" align="absmiddle" /></a>
													        <%# flagLogin ? "<a onclick='javascript:reportAbuse(" + DataBinder.Eval(Container.DataItem, "Id") + ")' style=\"cursor:pointer\"><img align='absmiddle' src='https://img.carwale.com/images/forums/alertforum.jpg' title='Report this post to administrators' border='0' /></a>" : "" %> 
													        <%# modLogin ? "<a style=\"cursor:pointer;\" onClick=\"if ( confirm('Do you really want to DELETE this post?') ) { __doPostBack('lnkDeletePost','" + DataBinder.Eval(Container.DataItem, "Id") + "') }\"><img src=\"https://img.carwale.com/images/icons/delete.ico\" align='absmiddle' title=\"Delete this post\" border=\"0\" /></a><a style=\"cursor:pointer;\" onClick=\"EditPost('" + DataBinder.Eval(Container.DataItem, "Id") + "')\"><img align='absmiddle' src=\"https://img.carwale.com/images/icons/new/add.gif\" title=\"Edit this post\" border=\"0\" /></a>" : "" %>
													            </span>		
												            </td>
											            </tr>
										            </table>
									            </div>										
									            <div id='divAbuse<%# DataBinder.Eval(Container.DataItem, "Id") %>' style="display:none;" align="left">
										            <table id="table1" bgcolor="#F8F2CF" cellSpacing="2" cellPadding="2" width="100%" border="0">
											            <tr><td><FONT color="#000000"><B>Report to administrators</B></FONT></td></tr>
											            <tr><td><FONT color="#006666"><B>Please select the reason that best describes your complaint: </B></FONT></td></tr>
											            <tr><td></td></tr>
											            <tr>
												            <td>
												            <select id='drpReason<%# DataBinder.Eval(Container.DataItem, "Id") %>'>
												            <option value="0" >--Select--</option>
												            <option value="1">Usage of abusive language</option>
												            <option value="2">Spam</option>
												            <option value="3">Personal attack</option>
												            <option value="4">Usage of copyrighted material</option>
												            <option value="5">Copy of someone else's work without giving reference</option>
												            </select>
												            </td>
											            </tr>
											            <tr>
												            <td>
													            <input type="button" id="btnSubmit" class="btn btn-orange btn-xs" onClick="javascript:btnSubmit_Change('<%# DataBinder.Eval(Container.DataItem, "Id") %>')" value="Submit" />&nbsp;&nbsp;&nbsp;
													            <input type="button" id="btnCancel" class="btn btn-orange btn-xs" onClick="javascript:Cancel('divAbuse<%# DataBinder.Eval(Container.DataItem, "Id") %>')" value="Cancel" />
												            </td>
											            </tr>
										            </table>
									            </div>
								            </td>
							            </tr>
						            </table>
					            </div>	
				            </itemtemplate>
				            <footertemplate>
			                <div></div>
				            <span id="spnPostedByUserIds" style="display:none;"><%# GetPostedByUserIds() %></span>
				            </footertemplate>	
			            </asp:Repeater>
			            <div class="footerStrip" id="divStrip" Visible="false" align="right" runat="server"></div>
			            <!-- Do not delete the following blank link -->
	
				            <asp:Panel HorizontalAlign="Right" ID="pnlModeratorTools" Visible="false" runat="server" style="padding:10px;">
					            <asp:LinkButton ID="lnkDeletePost" runat="server" />
					            <strong>Moderator Tools : </strong>
					            <asp:LinkButton ID="lnkCloseThread" Text="Close Thread" runat="server" /> |
					            <a style="cursor:pointer;" onClick="MoveThread('<%=Request["thread"]%>')">Move Thread</a> | 
					            <asp:LinkButton ID="lnkDeleteThread" Text="Delete Thread" runat="server" />	| <a href="modReportAbuse.aspx">Reported Posts</a>
						            | <a style="cursor:pointer;" onClick="CheckIds()">Split Post(s)</a> | <asp:LinkButton ID="lnkMergePost" Text="Merge Posts" runat="server" />
						            | <asp:HyperLink ID=hyplnkRemoveSticky runat="server" style="cursor:pointer" /><asp:HyperLink ID=hyplnkCreateSticky runat="server" style="cursor:pointer" />
				            </asp:Panel>
			            <div id="divReplyLink" runat="server" style="padding:5px;"><a class="buttons btn btn-orange btn-xs" href='reply.aspx?thread=<%= threadId%>&page=<%= totalNoOfPages%>' rel="nofollow">Reply to this Thread</a></div>
			            <div id="divNoReplyMessage" runat="server" style="padding:5px;"></div>
				
			            <div id="divQuickReply" visible="false" runat="server">
				            <h2 class="red">Quick Reply</h2>
				            <div class="infoTop" style="width:760px;">
					            <div style="float:right;width:620px;">
						            <Vspl:RTE id="rteQR" Rows="15" Cols="60" runat="server" />
						            <span id="spnDesc"></span><span id="spnDescription" class="error"></span>
						            <div><asp:CheckBox ID="chkEmailAlert" Text="Whenever someone replies to this thread, notify me by sending an email" Checked="true" runat="server" /></div>
						            <div style="margin-top:5px;" class="buttons"><asp:Button ID="butSave" CssClass="buttons btn btn-orange btn-xs" runat="server" Text="Quick Reply" />
                                        <tr id="trCaptcha" runat="server">
					                        <td>&nbsp;</td>
					                        <td>
						                        <iframe id="captchaCode" src="/Common/CaptchaImage/JpegImage.aspx" frameborder="0" scrolling="no" width="200" height="55"></iframe><br>
						                        <div>
							                        <span class="inline-block"><strong>Enter the code shown above</strong></span> <asp:TextBox id="txtCaptcha" CssClass="form-control inline-block" Columns="6" MaxLength="6" runat="server"></asp:TextBox><span class="inline-block margin-left5">(If you can't read it: <a onclick="javascript:regenerateCode()">Regenerate Code</a> )</span><br>
							
							                        <asp:Label id="lblCaptcha" CssClass="error" runat="server"></asp:Label>
						                        </div>
					                        </td>
				                        </tr>
						            </div>
					            </div>
					            <div style="width:125px;float:left; color:#555555; font-weight:bold;">Quick reply to this discussion</div>							
				            </div>
			            </div>				
			            </div>
		        </div>
            </div>
            <div class="right_container grid-3">
                <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx("1396440332273", 160, 600, 0, 0, false, 4) %>
            </div>
            <div class="clear"></div>
        </div>
        
        </section>
        <div class="clear"></div>  
        

    <iframe id="ifrKeepAlive" src="/editorial/keepalive.html" frameBorder="no" width="0" height="0" runat="server"></iframe>
    <script type="text/javascript">
        
        var checklist = "";


	    if(document.getElementById("hyplnk"))
		    document.getElementById("hyplnk").onclick = drpSub_Change;
	
	    if(document.getElementById("hyplnkCreateSticky"))
		    document.getElementById("hyplnkCreateSticky").onclick = CreateSticky;
	
	    if(document.getElementById("hyplnkRemoveSticky"))
		    document.getElementById("hyplnkRemoveSticky").onclick = RemoveSticky;
	
		
	    if(document.getElementById("lnkMergePost"))
		    document.getElementById("lnkMergePost").onclick = lnkMergePost_Change;
		
	    var count = 0;
	
	    function RemoveSticky(){
		    window.open('StickyThread.aspx?threadId=<%=threadId%>&type=2', 'splitThread', 'menu=no,address=no,scrollbars=yes,width=525,height=190');
	    }
	
	    function CreateSticky(){
		    window.open('StickyThread.aspx?threadId=<%=threadId%>&type=1', 'splitThread', 'menu=no,address=no,scrollbars=yes,width=525,height=190');
	    }
	    function CheckIds(){
		    var obj = document.getElementsByTagName("input");
		    var ids = "";

		    for(var i = 0; i < obj.length; i++){
			    if(obj[i].type == "checkbox" && obj[i].id.indexOf("chkItem") != -1 && obj[i].checked == true)
				    ids += obj[i].value + ",";
		    }
		
		    if(ids != "")
			    ids = ids.substring(0, ids.length - 1);
		
		    if(ids == ""){
			    alert("Please select atleast one post to continue");
			    return false;
		    }
		    else
			    SplitPost( ids );		
	    }
	
	    function lnkMergePost_Change()
	    {
		    var obj = document.getElementsByTagName("input");
		    var ids = "";
		    var count = 0;
		
		    for(var i = 0; i < obj.length; i++){
			    if(obj[i].type == "checkbox" && obj[i].id.indexOf("chkItem") != -1 && obj[i].checked == true){
				    ids += obj[i].value + ",";
				    count += 1;
			    }
		    }
		
		    if(count < 2){
			    alert("Please select atleast two post to continue");
			    return false;
		    }
		    else {return true;}
	    }
	
	    function prepareChecks( chkId ){		
		    if ( chkId.checked ){
			    count = count+1;
			    document.getElementById("hdnCompSelected").value += chkId.value + ",";
			    checklist = document.getElementById("hdnCompSelected").value;
		    }else{ 
			    document.getElementById("hdnCompSelected").value = 
					    document.getElementById("hdnCompSelected").value.replace(chkId.value + ",", "");
			    checklist = document.getElementById("hdnCompSelected").value;
		    }
	    }
	
	    function reportAbuse(postId)
	    {
		    if (document.getElementById("divAbuse" + postId).style.display=='none'){
			    document.getElementById("divAbuse" + postId).style.display = 'block';
			    return false;
		    }else{
			    document.getElementById("divAbuse" + postId).style.display = 'none';
			    return false;
		    }
	    }
	
	    function drpSub_Change(e)
	    {
		    if(confirm('Are you sure you want to subscribe to this thread?')){
			    var custId = '<%=customerId%>' ;
			    var subId = '<%= threadId%>' ;

			    var response = AjaxForum.AddSubscription(subId, custId, 2);
			    if(response.value == true){
				    document.getElementById('lblBrk').style.visibility = 'hidden';
				    document.getElementById("hyplnk").innerHTML = "";
				    document.getElementById("lblMsg").innerHTML = "<b>[Successfully subscribed]</b>";
			    }
		    }
		    else{return false}
	    }
	
	    function btnSubmit_Change(postId){
		    if (document.getElementById("drpReason" + postId).selectedIndex == 0){
			    alert("Please select reason");
			    return false;
		    }else{
			    if(confirm('Are you sure you want to report abuse this post to the administrator?')){
				    var custId = '<%=customerId%>';
				    var subId = '<%= threadId%>';
				    var PostId = postId;
				    var w = document.getElementById("drpReason" + postId).selectedIndex;
   				    var selected_text = document.getElementById("drpReason" + postId).options[w].text;
				
				    var response = AjaxForum.ForumInsertReportAbuse(subId, custId, PostId, selected_text);
				    if(response.value == true){
					    reportAbuse(postId);
					    document.getElementById("lblMsgReport" + postId).innerHTML = "<b>[Report sent to the moderator for approval]</b>";
				    }
			    }
			    else{return false}
		    }
	    }
		
	    function Cancel(div1){
		    document.getElementById(div1).style.display = 'none';
		    return false;
	    }
	
	    if(document.getElementById("butSave"))
		    document.getElementById("butSave").onclick = verifyForm;
		
	    function verifyForm(e){
		    var isError = false;
		
		    var desc = tinyMCE.get('rteQR_txtContent').getContent();        
			
		    if( desc == "" ){
			    isError = true;
			    document.getElementById("spnDescription").innerHTML = "&nbsp;Why blank reply? Please do write something!";
		    }
		
		    else{
			    document.getElementById("spnDescription").innerHTML = "";
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

	    function ComposeMessage(postedById)
	    {
		    var caption = "Send Message";
		    var url = "/community/pms/composemessage.aspx?rId="+postedById;	 
		    var applyIframe = true;
		    var GB_Html = "";

		    GB_show( caption, url, 300, 600, applyIframe, GB_Html );
        }
	
	    function MultiQuoteClicked(a)
	    {
		    //alert($(a).attr("threadId"));
		    if ($(a).attr("class") == "not-sel-thread")
		    {
			    $(a).attr("class", "sel-thread");
			    $(a).attr("title", "Remove this post from multi quote");
			    $(a).find("img").attr("src","<%=ImagingFunctions.GetRootImagePath()%>/images/forums/multiquote_on.gif");
			
			    var cookieval = GetCookieVal();
			    document.cookie = "Forum_MultiQuotes=" + cookieval + "," + $(a).attr("threadId") + "; path=/";
			
		    }
		    else
		    {
			    $(a).attr("class", "not-sel-thread");
			    $(a).attr("title", "Add this post in multi quote");
			    $(a).find("img").attr("src","<%=ImagingFunctions.GetRootImagePath()%>/images/forums/multiquote_off.gif");
			    var cookieval = GetCookieVal();
			    cookieval = cookieval + ",";
			    var remThread = "," + $(a).attr("threadId") + ",";
			    //alert(cookieval + "  ::  " + remThread);
			    var splitVal = cookieval.split(remThread);
			    cookieval = splitVal[0] + "," + splitVal[1];
			    cookieval = jQuery.trim(cookieval);
			    cookieval = cookieval.substring(0, cookieval.length - 1);
			    document.cookie = "Forum_MultiQuotes=" + cookieval + "; path=/";
			    //alert (cookieval);
		    }
	    }
	
	    function GetCookieVal()
	    {
		    var theCookie=""+document.cookie;
		    var ind=theCookie.indexOf("Forum_MultiQuotes");
		    if (ind==-1 || "Forum_MultiQuotes"=="") return ""; 
		    var ind1=theCookie.indexOf(';',ind);
		    if (ind1==-1) ind1=theCookie.length; 
		    return unescape(theCookie.substring(ind+"Forum_MultiQuotes".length+1,ind1));
	    }
	
	    $(document).ready(function(){
		    var cookieval = GetCookieVal() + ",";
		    $("a.not-sel-thread").each(function(){
			    if (cookieval.indexOf("," + $(this).attr("threadId") + ",") != -1)
			    {
				    $(this).attr("class", "sel-thread");
				    $(this).attr("title", "Remove this post from multi quote");
				    $(this).find("img").attr("src","<%=ImagingFunctions.GetRootImagePath()%>/images/forums/multiquote_on.gif");
			    }
		    });
		
		    var _postedByUserIds = $("#spnPostedByUserIds").text();
		    _postedByUserIds = _postedByUserIds.substring(1, _postedByUserIds.length-1);
		
		    if( _postedByUserIds.length > 0 ){
		        $.ajax({
		            type: "POST", url: "/ajaxpro/CarwaleAjax.AjaxForum,Carwale.ashx", 
		            data:'{"postedByIds":"'+ _postedByUserIds +'"}',
		            beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetOnlineForumUsers"); },
		            success: function(response) {	
		                var jsonString = eval('('+ response +')');
		                var onlineUsers = jsonString.value;		
		                onlineUsers = onlineUsers.substring(0, _postedByUserIds.length);
				
		                if (onlineUsers != "")
		                {
		                    splittedOnlineUsers = onlineUsers.split(","); 
		                    for (i=0;i<splittedOnlineUsers.length;i++)
		                        $("img.imgActive" + splittedOnlineUsers[i].toString()).show();	
		                }
				
		                FetchPostThanksCount("<%=concatenatedPostIds%>");	
		            }
		        });
		    }
	    });
	
	    function FetchPostThanksCount(_concatenatedPostIds)
	    {
	        if(_concatenatedPostIds.length > 0)
	        {
	            $.ajax({
	                type: "POST", url: "/ajaxpro/CarwaleAjax.AjaxForum,Carwale.ashx", 
	                data:'{"postIds":"'+ _concatenatedPostIds +'"}',
	                beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetPostThanksCount"); },
	                success: function(response) {	
	                    var jsonString = eval('('+ response +')');
	                    var resPostsCount = jsonString.value;	
	                    if (resSplit != "")
	                    {
	                        var resSplit = resPostsCount.toString().split(",");
	                        if (resSplit.length > 1)
	                        {
	                            for (var i=0; i<resSplit.length; i=i+2)
	                            {
	                                $("#spnThanksReceived" + resSplit[i]).html("<a onclick='ShowThankHandles("+ resSplit[i] +")'>" + resSplit[i + 1] + "</a>");
	                            }
	                        }
	                    }		
			
	                    var thankedPost = getCookie("postThankPath");
	                    if (thankedPost != "" && thankedPost.indexOf("::") != -1)
	                    {
	                        var thankedPostId = thankedPost.toString().split("::");
	                        if (thankedPostId.length > 0)
	                        {
	                            if (thankedPostId[0] == "Thanked")
	                            {
	                                var _thanksMsg = "";
	                                if (thankedPostId[2] == "-1")
	                                    _thanksMsg = "[ Some error occured ]";
	                                else if (thankedPostId[2] == "-3")
	                                    _thanksMsg = "[ It is your own post ]";
	                                else if (thankedPostId[2] == "1")
	                                    _thanksMsg = "[ You liked this post ]";
	                                else if (thankedPostId[2] == "0")
	                                    _thanksMsg = "[ You already liked this post ]";			
								
	                                $("#spnThankMsg" + thankedPostId[1]).html(_thanksMsg);
							
	                                $("#btnThank" + thankedPostId[1]).hide();
	                                document.cookie = "postThankPath=";
	                            }
	                        }
	                    }
				
	                    $(".spnThanksRecvCounter span a").each(function(){
	                        if ($(this).html() == "0")
	                        {
	                            $(this).parent().parent().hide();	
	                            if (!$(this).parent().parent().next().next().is(":hidden"))
	                                $(this).parent().parent().next().show();
	                        }
	                    });
	                } // ajax success
	            });//ajax
	        }//if
	    }
	
	    function ShowThankHandles(postIdToShowHandles)
	    {
		    var caption = "People who liked this post";
		    var url = "ThankedHandles.aspx?postId=" + postIdToShowHandles;	 
		    var applyIframe = false;
		    var GB_Html = "";
		  
		    GB_show( caption, url, 250, 400, applyIframe, GB_Html );
	    }
	
	    function ThankPost(postId){
	        if( postId.length > 0 ){
	            $.ajax({
	                type: "POST", url: "/ajaxpro/CarwaleAjax.AjaxForum,Carwale.ashx", 
	                data:'{"postId":"'+ postId +'"}',
	                beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ThankThePost"); },
	                success: function(response) {	
	                    var jsonString = eval('('+ response +')');
	                    var res = jsonString.value;			
	                    var resArr = res.toString().split("|");
	                    if (resArr[0] == "-1"){
	                        document.cookie = "postThankPath=<%=HttpContext.Current.Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString()%>";
	                        location.href = "/users/login.aspx?ReturnUrl=<%=HttpUtility.UrlEncode("/forums/RedirectToThank.aspx?params=")%>" + postId + ",1";
	                    }
	                    else if (resArr[1] == "-1"){
	                        document.cookie = "postThankPath=<%=HttpContext.Current.Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString()%>";
	                        location.href = "/users/EditUserHandle.aspx?returnUrl=<%=HttpUtility.UrlEncode("/forums/RedirectToThank.aspx?params=")%>" + postId + ",0";
	                    }
	                    else if (resArr[2] == "0"){
	                        $("#spnThankMsg" + postId).html("[ You already liked this post ]");
	                        $("#btnThank" + postId).hide();
	                    }
	                    else if (resArr[2] == "1"){
	                        var counterThanks = parseInt($("#spnThanksReceived" + postId + " a").text()) + 1;
	                        $("#spnThanksReceived" + postId).html("<a onclick='ShowThankHandles("+ postId +")'>" + counterThanks.toString() + "</a>");
	                        var counterUserThanks = parseInt($("#spnThanksToUser" + postId).text()) + 1;
	                        var postUserId = $("#spnThanksToUser" + postId).attr("userId");
	                        $("span[userId="+ postUserId +"]").text(counterUserThanks);
	                        $("#btnThank" + postId).hide();
					
	                        if(!($("#spnThankMsg" + postId).parent().find(".spnNoThanksRecvCounter").is(":hidden")))
	                            $("#spnThankMsg" + postId).parent().find(".spnNoThanksRecvCounter").html("<b>[ You liked this post ]</b>");
	                        else
	                            $("#spnThankMsg" + postId).html("You liked this post");		
	                    }
	                }
	            }); // ajax
	        }//if
	    }
	
	    function getCookie(c_name)
	    {
		    if (document.cookie.length>0)
	        {
		 	    c_start=document.cookie.indexOf(c_name + "=");
		  	    if (c_start!=-1)
			    {
				    c_start=c_start + c_name.length+1;
				    c_end=document.cookie.indexOf(";",c_start);
				    if (c_end==-1) c_end=document.cookie.length;
				    return unescape(document.cookie.substring(c_start,c_end));
			    }
		    }
		    return "";
	    }
	
	    function regenerateCode(){
	        $("#captchaCode").attr("src", "/Common/CaptchaImage/JpegImage.aspx");
	    }

    </script>
    <!-- #include file="/includes/footer.aspx" -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
    <!-- Footer ends here -->
        <script type="text/javascript"> 
            Common.showCityPopup = false;
</script>
</form>
</body>
</html>