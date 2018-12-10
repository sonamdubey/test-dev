<%@ Page Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.Community.Mods.NewsComments" Trace="false" Debug="false" %>
<!doctype html>
<html>
<head>

<!-- #include file="/includes/global/head-script.aspx" -->
<script  type="text/javascript"  src="/static/src/graybox.js" ></script>
<link rel="stylesheet" href="/static/css/forums.css" type="text/css" >
<style>
	.panelStyle {border:1px solid #777777;width:400px;}
	.panelStyle .panelHeader { background-color:#777777; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; font-weight:bold; color:#ffffff; }
	.errorHandle {color:red;}
</style>

</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
<!-- #include file="/includes/header.aspx" -->
<form id="form1" runat="server">
    
    <section class="bg-light-grey padding-top10 padding-bottom10 no-bg-color">
        <div id="youHere">
        <div class="container">
            <div class="grid-12 alpha omega margin-bottom10">
                <a href="/community/">Community</a>
                <span class="fa fa-angle-right margin-left10 margin-right10"></span>
                <a href="default.aspx">Moderator's Home</a>
                <span class="fa fa-angle-right margin-left10 margin-right10"></span>
                News Comments
            </div>
            <div class="clear"></div>
            <h1 class="font30 text-black special-skin-text">List of news comments</h1>
            <div class="border-solid-bottom margin-top10 margin-bottom15"></div> 
        </div>
        </div>
    </section>
    <div class="clear"></div> 	
	
    <section class="bg-light-grey">
    <div class="container">
        <div class="grid-12 alpha omega">
        <div class="content-box-shadow content-box-shadow content-inner-block-10 rounded-corner2 margin-bottom20">
        <asp:Repeater ID="rptNewsComments" runat="server">
            <ItemTemplate> 
                <div><h3 class="hd3"><a href="/news/<%# DataBinder.Eval(Container.DataItem, "NewsId") %>-<%# DataBinder.Eval(Container.DataItem, "Url") %>.html"><%# DataBinder.Eval(Container.DataItem, "NewsTitle") %></a></h3></div>
                <div style="padding:5px 0">
                    <asp:Repeater DataSource='<%# GetComments(Convert.ToString(DataBinder.Eval(Container.DataItem, "NewsId"))) %>' runat="server">
                        <ItemTemplate> 
                            <div style="padding:5px 0" id="div-<%# DataBinder.Eval(Container.DataItem, "CommentId")%>">
                                <div style="font-size:smaller">Commented By: <%# DataBinder.Eval(Container.DataItem, "UserName")%>&nbsp;&nbsp;&nbsp;&nbsp;Email: <%# DataBinder.Eval(Container.DataItem, "Email") %>&nbsp;&nbsp;&nbsp;&nbsp;On: <%# DataBinder.Eval(Container.DataItem, "CommentDateTime", "{0:f}") %></div>
                                <div id="comment-<%# DataBinder.Eval(Container.DataItem, "CommentId")%>" style="font-size:small"><%# DataBinder.Eval(Container.DataItem, "Comment") %></div>
                                <div><input type="button" id="btnApprove" value="Approve" onclick="ApproveComment(<%# DataBinder.Eval(Container.DataItem, "CommentId")%>)" />&nbsp;&nbsp;<input type="button" value="Delete" id="btnDel" onclick="DeleteComment(<%# DataBinder.Eval(Container.DataItem, "CommentId")%>)" />&nbsp;&nbsp;<input type="button" id="btnEdit" onclick="EditComment(<%# DataBinder.Eval(Container.DataItem, "CommentId")%>)" value="Edit" /></div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </ItemTemplate>
            <SeparatorTemplate>
                <hr />
            </SeparatorTemplate>
        </asp:Repeater>
        </div>
        </div>
        <div class="clear"></div>    
    </div>
    </section>      
    <div class="clear"></div>

<script type="text/javascript">
    function ApproveComment(commentId) {
        ShowLoading("Approving comment...");
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Carwale.Community.Mods.AjaxNewsComments,Carwale.ashx",
            data: '{"commentId":"' + commentId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ApproveComment"); },
            success: function (response) {
                var status = eval('(' + response + ')');
                if (status.value) {
                    $("#div-" + commentId).fadeOut("slow");
                    setTimeout("GB_hide()", 1500);
                }
                else {
                    ShowLoading("Could not approve...");
                    setTimeout("GB_hide()", 1000);
                }

            }
        });
    }

    function DeleteComment(commentId) {
        ShowLoading("Deleting comment...");      
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Carwale.Community.Mods.AjaxNewsComments,Carwale.ashx",
            data: '{"commentId":"' + commentId + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "DeleteComment"); },
            success: function (response) {
                var status = eval('(' + response + ')');
                if (status.value) {
                    $("#div-" + commentId).fadeOut("slow");
                    setTimeout("GB_hide()", 1500);
                }
                else {
                    ShowLoading("Could not delete...");
                    setTimeout("GB_hide()", 1000);
                }
            }
        });
    }

    function EditComment(commentId) {
        var comment = $("#comment-"+commentId).text();
        var caption = "Edit Comment";
        var url = "";
        var applyIframe = true;
        var GB_Html = "<textarea id=\"txtComment\" cols=\"50\" rows=\"10\" >" + comment + "</textarea><br /><input type=\"button\" id=\"btnSaveEdit\" value=\"Save\" onclick=\"SaveComment(" + commentId + ")\" />";

        GB_show(caption, url, 250, 400, applyIframe, GB_Html);
    }

    function SaveComment(commentId) {
        var comment = $("#txtComment").val();
        //alert(comment);
        ShowLoading("Saving comment...");        
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Carwale.Community.Mods.AjaxNewsComments,Carwale.ashx",
            data: '{"commentId":"' + commentId + '", "comment":"' + comment + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SaveModEditComment"); },
            success: function (response) {
                var status = eval('(' + response + ')');
                if (status.value) {
                    setTimeout("GB_hide();window.location.reload()", 1000);
                }
            }
        });       
    }

    function ShowLoading(text) {
        var caption = "";
        var url = "";
        var applyIframe = false;
        var GB_Html = "<div style=\"width:100%;\"><div style=\"margin:10px auto 0 auto;vertical-align:middle;text-align:center;\"><img id=\"loading\" src=\"https://imgd.aeplcdn.com/0x0/statics/loader.gif\"/><br/><b> " + text + " </b></div></div>";
        GB_show(caption, url, 100, 250, true, GB_Html);
    }

</script>
</form>
<!-- #include file="/includes/footer.aspx" -->
<!-- #include file="/includes/global/footer-script.aspx" -->
</body>
</html>