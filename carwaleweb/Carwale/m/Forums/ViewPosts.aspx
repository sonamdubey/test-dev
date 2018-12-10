<%@ Page Language="C#" ContentType="text/html" AutoEventWireup="false" trace="false" Inherits="MobileWeb.Forums.ViewPosts" %>
<%@ Import Namespace="Carwale.BL.Experiments" %>
<%
    Title = topic + " | " + forumSubCatName + " | Car Forums - CarWale";
    Keywords = "Car Forum, Reply, Views";
    Description = "CarWale ViewThread section displays  Forum Post related details like number of replies and views in numbers. ";
    //Canonical = "https://www.carwale.com" + Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString().ToString().Replace("/m/", "/");
    Canonical = "https://www.carwale.com/forums/" + threadId + "-" + postUrl + ".html" ;
    if (Convert.ToInt32(currPage) != 1)
        PrevPageUrl = "https://www.carwale.com/forums/" + threadId + "-" + postUrl + "-p" + Convert.ToString(Convert.ToInt32(currPage) - 1) + ".html";
    if(Convert.ToInt32(currPage) != totalPages && totalPages != 0)
        NextPageUrl = "https://www.carwale.com/forums/" + threadId + "-" + postUrl + "-p" + Convert.ToString(Convert.ToInt32(currPage) + 1) + ".html";

    MenuIndex = "7";
    bool showExperimentalColor = ProductExperiments.IsShowExperimentalColor(CookiesCustomers.AbTest);

%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" itemscope itemtype="http://schema.org/WebPage">
<head>
<!-- #include file="/m/includes/global/head-script.aspx" -->
<!--<link rel="stylesheet" href="/static/m/css/design.css" type="text/css" >-->
</head>

<body class="<%= (showExperimentalColor ? "btn-abtest" : "")%> m-special-skin-body m-no-bg-color">
    <!-- #include file="/m/includes/header.aspx" -->
	<!--Outer div starts here-->
	
        <section class="container">
    	<!--Main container starts here-->
    	<div id="main-container">
            <div class="grid-12">
			
            <div id="br-cr" class="margin-top10 margin-bottom10 m-special-skin-text"><a href="/m/forums/" class="normal m-special-skin-text ">Forums</a> &raquo; <a href="/m/forums/<%=threadUrl%>" class="normal m-special-skin-text"><%=forumSubCatName%></a></div>
            <h1 class="pgsubhead margin-bottom10 m-special-skin-text"><%=topic%></h1>
            <div class="darkgray margin-bottom10">
                <div class="btn btn-xs btn-orange  rightfloat" onclick="RedirectToReply();">Reply to this thread</div>
                <div style="clear:both;"></div>
            </div>
            <asp:Repeater ID="rptPosts" runat="server">
                <itemtemplate>
                    <div class="box content-inner-block-10 content-box-shadow rounded-corner2 margin-bottom10" id="post<%# DataBinder.Eval(Container.DataItem, "ID").ToString() %>">
                        <div><span class="sub-heading"><%# DataBinder.Eval(Container.DataItem, "PostedBy").ToString() %></span><%# DataBinder.Eval(Container.DataItem, "City").ToString() != "" ? ", " + DataBinder.Eval(Container.DataItem, "City").ToString() : "" %></div>
                        <div class="new-line lightgray"><%# GetUserTitle( DataBinder.Eval(Container.DataItem, "Role").ToString(), DataBinder.Eval(Container.DataItem, "Posts").ToString(), DataBinder.Eval(Container.DataItem, "BannedCust").ToString() ) %></div>
                        <div class="new-line lightgray"><%# DataBinder.Eval(Container.DataItem, "Posts").ToString() %> Posts | <span id="spnThanksToUser<%# DataBinder.Eval(Container.DataItem,"ID")%>" userId="<%#DataBinder.Eval(Container.DataItem,"PostedById")%>"><%# DataBinder.Eval(Container.DataItem, "ThanksReceived").ToString() %></span> Likes</div>
                    </div>	
                    <div class="box-bot content-inner-block-10 content-box-shadow rounded-corner2 margin-bottom10">
                        <div><%# GetDateTime( DataBinder.Eval(Container.DataItem, "MsgDateTime").ToString()) %></div>
                        <div class="darkgray" style="padding-top:10px;padding-bottom:10px;" type="PostMessage">
                            <%# GetMessage(DataBinder.Eval(Container.DataItem, "Message").ToString()) %>	
                        </div>
                        <%# DataBinder.Eval(Container.DataItem, "LastUpdatedHandle").ToString() != "anonymous" ? "<div>Last Updated: <br/>" + GetDateTime( DataBinder.Eval(Container.DataItem, "LastUpdatedTime").ToString() ) + ", by " + DataBinder.Eval(Container.DataItem, "LastUpdatedHandle").ToString() + "</div>" : "" %>
                        <span id="spnPostThanksCount<%#DataBinder.Eval(Container.DataItem,"ID") %>"> <%#DataBinder.Eval(Container.DataItem, "PostThanksCount").ToString()%> </span>member(s) liked the post            	
                        <div class="margin-top10">
                            <a class="normal" id="btnThank<%# DataBinder.Eval(Container.DataItem, "ID") %>" onclick="ThankPost('<%# DataBinder.Eval(Container.DataItem, "ID") %>')" <%#CheckOwnerForVisibility(DataBinder.Eval(Container.DataItem,"PostedById").ToString())%>>
                                <div style="width:32px;height:32px;background-image: url('/m/images/icons-sheet.png');background-position: -2px -340px;float:left;"></div>
                            </a>
                            <div style="width:32px;height:32px;background-image: url('/m/images/icons-sheet.png');background-position: -2px -460px;float:left;margin-left:10px;display:none;"></div>
                            <a class="normal" href='/m/forums/replythread.aspx?thread=<%=Request.QueryString["thread"].ToString()%>&quote=<%# DataBinder.Eval(Container.DataItem, "ID").ToString() %>'>
                                <div style="width:32px;height:32px;background-image: url('/m/images/icons-sheet.png');background-position: -2px -380px;float:left; margin-left:10px;">&nbsp;</div>
                            </a>
                            <a class="not-sel-thread" threadId="<%#DataBinder.Eval(Container.DataItem,"ID") %>" title="Add this post in multi quote" onclick="MultiQuoteClicked(this);">
                                <div style="width:32px;height:32px;background-image: url('/m/images/icons-sheet.png');background-position: -2px -420px;float:left;margin-left:10px;">&nbsp;</div>
                            </a>
                            <div style="clear:both;"></div>
                        </div>
                        <span id="spnThankMsg<%#DataBinder.Eval(Container.DataItem,"ID")%>" style=" font-weight:bold;"></span>
                       </div>
                </itemtemplate>
            </asp:Repeater>
            
            <div class="darkgray margin-bottom10">
                <div class="btn btn-xs btn-orange rightfloat" onclick="RedirectToReply();">Reply to this thread</div>
                <div style="clear:both;"></div>
            </div>
            <div class="new-line5 f-14">
                <table style="width:100%;" cellspacing="0" cellpadding="0" border="0" class="m-special-skin-text">
                    <tr>
                        <td style="width:60px;">
                            <%if(Convert.ToInt32(currPage) != 1){%>
                            <a style="position:relative;top:-4px;" href="<%="/m/forums/" + threadId + "-" + postUrl + "-p" + (Convert.ToInt32(currPage)-1) + ".html" %>"><span class="m-special-skin-text"><span class="arr-big">&laquo;</span>&nbsp;Prev</span></a>
                            <%}%>
                        </td>
                        <td style="text-align:center;">
                            <%if (totalPages > 1){%>
                            Page  
                            <select id="ddlPages" onChange="PageChanged()" data-role="none">
                                <%for(int i=1; i<=totalPages; i++){%>
                                <option value="<%=i%>"><%=i%></option>
                                <%}%>
                            </select>
                            of <%=totalPages.ToString()%>
                            <%}%>
                        </td>
                        <td style="width:60px;">
                            <%if(Convert.ToInt32(currPage) != totalPages && totalPages != 0){%>
                            <a href="<%="/m/forums/" + threadId + "-" + postUrl + "-p" + (Convert.ToInt32(currPage)+1) + ".html" %>" style="position:relative;top:-4px;"><span class="m-special-skin-text">Next<span class="arr-big">&nbsp;&raquo;</span></span></a>
                            <%}%>
                        </td>
                    </tr>
                </table>
            </div>
            <form id="formPosts" method="post">
                 <input type="hidden" name="hdnPosts" id="hdnPosts" value="<%=hdnPosts%>" />
            </form>
            
            
            </div>
            <div class="clear"></div>
        </div>
        <!--Main container ends here-->
            <div class="clear"></div>
        </section>
    
    <!--Outer div ends here-->

    <!-- #include file="/m/includes/footer.aspx" -->
	<!-- #include file="/m/includes/global/footer-script.aspx" -->
    <script type="text/javascript">
        var prevPage = '<%=Convert.ToString(Convert.ToInt32(currPage)-1)%>';
        var nextPage = '<%=Convert.ToString(Convert.ToInt32(currPage)+1)%>';
        var threadId = '<%=threadId%>';
        var postUrl = '<%=postUrl%>';
    </script>
    <script  type="text/javascript"  src="/static/m/js/forums.js" ></script>

    <script language="javascript" type="text/javascript">
        Common.showCityPopup = false;
        function MultiQuoteClicked(a) {
            if ($(a).attr("class") == "not-sel-thread") {
                $(a).attr("class", "sel-thread");
                $(a).attr("title", "Remove this post from multi quote");
                $(a).find("div").attr("style", "width:32px;height:32px;background-image: url('/m/images/icons-sheet.png');background-position: -2px -876px;float:left;margin-left:10px;");

                var expiryTime = 1000 * 60 * 15;  //15 min
                //var expiryTime = 120000; //2min
                var expires = new Date((new Date()).valueOf() + expiryTime);

                var cookieval = GetCookieVal();
                if (cookieval == "") {
                    document.cookie = "Forum_MultiQuotes=<%=Request.QueryString["thread"].ToString()%>:," + $(a).attr("threadId") + ";expires=" + expires.toUTCString() + ";path=/";
                    } else {
                        document.cookie = "Forum_MultiQuotes=" + cookieval + "," + $(a).attr("threadId") + ";expires=" + expires.toUTCString() + ";path=/";
                    }

                }
                else {
                    $(a).attr("class", "not-sel-thread");
                    $(a).attr("title", "Add this post in multi quote");
                    $(a).find("div").attr("style", "width:32px;height:32px;background-image: url('/m/images/icons-sheet.png');background-position: -2px -420px;float:left;margin-left:10px;");

                    var cookieval = GetCookieVal();
                    cookieval = cookieval + ",";
                    var remThread = "," + $(a).attr("threadId") + ",";
                    var splitVal = cookieval.split(remThread);
                    cookieval = splitVal[0] + "," + splitVal[1];
                    cookieval = jQuery.trim(cookieval);
                    cookieval = cookieval.substring(0, cookieval.length - 1);
                    var expiryTime = 1000 * 60 * 15;  //15 min
                        //var expiryTime = 120000; //2min
                    var expires = new Date((new Date()).valueOf() + expiryTime);
                    document.cookie = "Forum_MultiQuotes=" + cookieval + ";expires=" + expires.toUTCString() + ";path=/";
                        //alert(cookieval);
                }
            }

            function GetCookieVal() {
                var theCookie = "" + document.cookie;
                var ind = theCookie.indexOf("Forum_MultiQuotes");
                if (ind == -1 || "Forum_MultiQuotes" == "") return "";
                var ind1 = theCookie.indexOf(';', ind);
                if (ind1 == -1) ind1 = theCookie.length;
                return unescape(theCookie.substring(ind + "Forum_MultiQuotes".length + 1, ind1));
            }

            function DeleteCookie() {
                //alert("splitThread" + splitThread[0]);
                var expiryTime = 1000 * 60 * 60 * 24 * 1;  //1 day
                var expires = new Date((new Date()).valueOf() - expiryTime);
                document.cookie = "Forum_MultiQuotes=;expires=" + expires.toUTCString() + ";path=/";
            }

            $(document).ready(function () {
                var cookieval = GetCookieVal() + ",";
                if (cookieval != ",") {
                    splitThread = cookieval.split(":");
                    //alert("splitThread :" + splitThread[0]);
                    var currentThread = '<%=Request.QueryString["thread"].ToString()%>';
                        if (currentThread != splitThread[0]) {
                            //alert("Thread not matching");
                            DeleteCookie();
                        }
                    }
                    $("a.not-sel-thread").each(function () {
                        if (cookieval.indexOf("," + $(this).attr("threadId") + ",") != -1) {
                            $(this).attr("class", "sel-thread");
                            $(this).attr("title", "Remove this post from multi quote");
                            $(this).find("div").attr("style", "width:32px;height:32px;background-image: url('/m/images/icons-sheet.png');background-position: -2px -876px;float:left;margin-left:10px;");
                        }
                    });

                    $("#ddlPages").val(<%=currPage%>);
                    <%if(Request.QueryString["postId"] != null && Request.QueryString["postId"].ToString() != ""){%>
                    location.href = "#post<%=Request.QueryString["postId"].ToString()%>";
                    <%}%>

                    $("div[type='PostMessage'] img").attr("class", "imgWidth");
                    GetThankMsg();
                });



                function RedirectToReply() {
                    location.href = "/m/forums/replythread.aspx?thread=<%=Request.QueryString["thread"].ToString()%>";
                }

                function getCookie(c_name) {
                    if (document.cookie.length > 0) {
                        c_start = document.cookie.indexOf(c_name + "=");
                        if (c_start != -1) {
                            c_start = c_start + c_name.length + 1;
                            c_end = document.cookie.indexOf(";", c_start);
                            if (c_end == -1) c_end = document.cookie.length;
                            return unescape(document.cookie.substring(c_start, c_end));
                        }
                    }
                    return "";
                }

                function GetThankMsg() {
                    var thankedPost = getCookie("postThankPath");
                    if (thankedPost != "" && thankedPost.indexOf("::") != -1) {
                        var thankedPostId = thankedPost.toString().split("::");
                        if (thankedPostId.length > 0) {
                            if (thankedPostId[0] == "Thanked") {
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
                }

                function ThankPost(postId) {
                    if (postId.length > 0) {
                        $.ajax({
                            type: "POST",
                            url: "/ajaxpro/MobileWeb.Ajax.Forums,Carwale.ashx",
                            data: '{"postId":"' + postId + '"}',
                            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ThankThePost"); },
                            success: function (response) {
                                var jsonString = eval('(' + response + ')');
                                var res = jsonString.value;
                                var resArr = res.toString().split("|");
                                if (resArr[0] == "-1") {

                                    document.cookie = "postThankPath=<%=HttpContext.Current.Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString()%>";
                                    location.href = "/m/users/auth.aspx?act=log&returnUrl=<%=HttpUtility.UrlEncode("/m/forums/RedirectToThank.aspx?params=")%>" + postId + ",1";
                                }
                                else if (resArr[1] == "-1") {

                                    document.cookie = "postThankPath=<%=HttpContext.Current.Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString()%>";
                                    location.href = "/m/users/CreateHandle.aspx?returnUrl=<%=HttpUtility.UrlEncode("/m/forums/RedirectToThank.aspx?params=")%>" + postId + ",0";
                                }
                                else if (resArr[2] == "0") {
                                    $("#spnThankMsg" + postId).html("[ You already liked this post ]");
                                    $("#btnThank" + postId).hide();
                                }
                                else if (resArr[2] == "1") {

                                    var counterThanks = parseInt($("#spnPostThanksCount" + postId).text()) + 1;
                                    $("#spnPostThanksCount" + postId).text(counterThanks);

                                    var counterUserThanks = parseInt($("#spnThanksToUser" + postId).text()) + 1;
                                    var postUserId = $("#spnThanksToUser" + postId).attr("userId");
                                    $("span[userId=" + postUserId + "]").text(counterUserThanks);

                                    $("#spnThankMsg" + postId).html("[ You liked this post ]");
                                    $("#btnThank" + postId).hide();
                                }
                            }
                        });
                }
            }
            </script>	
</body>
</html>