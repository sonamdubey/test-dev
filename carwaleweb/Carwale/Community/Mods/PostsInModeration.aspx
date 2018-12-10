<%@Page Language="C#" Inherits="Carwale.UI.Community.Mods.PostsInModeration" trace="false" AutoEventWireup="false" EnableViewState="true" %>
<!doctype html>
<html>
<head>
    <title>Posts In Moderation</title>

<!-- #include file="/includes/global/head-script.aspx" -->


<style>
	.inquiries { border-collapse:collapse; border-color:#eeeeee; }
	.inquiries th { text-align:left; white-space:nowrap;background-color:#777;color:#fff;padding:4px 2px 4px 2px; }
	.inquiries .item { background-color:#f3f3f3; }
	.inquiries .altItem { background-color:#ffffff; }
	.sendMail { background-color:#FFF0E1; border:1px solid orange;padding:5px; }
	.btn {background-color:#666666;padding:2px;font-weight:bold;color:#ffffff;}
    #ModerateData img { width:200px; }
</style>
<script type="text/javascript">

    $(document).ready(function () {
        var str = '<td valign="top" ><input class="clearButtons" type="button" value="clear" onclick = "ClearRadio($(this))" /></td>';

        $('#ModerateData tr').append(str);
        $('.dtHeader td:last').html('');
       
    });

    function ClearRadio(btnclear)   
    {
        btnclear.parent().parent().find('.approve').attr('checked',false);
        btnclear.parent().parent().find('.ban').attr('checked', false);
        btnclear.parent().parent().find('.Deactivate').attr('checked', false);
    }

    function Moderate(DataStringApprove, DataStringBan, DataStringDeactivate)
    {
              
        $.ajax({
            type: "POST",
            url: "/ajaxpro/CarwaleAjax.AjaxCommunityModerate,Carwale.ashx",
            data: '{"DataStringApprove":"' + DataStringApprove + '","DataStringBan":"' + DataStringBan + '","ModId":"' + <%= modid%> + '","DataStringDeactivate":"' + DataStringDeactivate + '"}',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("X-AjaxPro-Method", "Moderate")},
            success: function (response)
            {
               alert("Records saved successfully");
               window.location.href = "/Community/mods/PostsInModeration.aspx"
            }
        });
    }
   
    function save()
    {
        var DataStringApprove = '';
        var DataStringBan = '';
        var DataStringDeactivate = '';
        $('.approve').each(function () {
            if ($(this).is(":checked")) {
                DataStringApprove += $(this).attr('threadid') + '/';
                DataStringApprove += $(this).attr('customerid') + '/';
                DataStringApprove += $(this).attr('forumid') + '#';
            }
        });
        $('.ban').each(function () {
            if ($(this).is(":checked")) {
                DataStringBan += $(this).attr('threadid') + '/';
                DataStringBan += $(this).attr('customerid') + '/';
                DataStringBan += $(this).attr('forumid') + '#';
            }
           
        });
        $('.Deactivate').each(function () {
            if ($(this).is(":checked")) {
                DataStringDeactivate += $(this).attr('threadid') + '/';
                DataStringDeactivate += $(this).attr('customerid') + '/';
                DataStringDeactivate += $(this).attr('forumid') + '#';
            }
        });
        if (DataStringApprove != '' || DataStringBan != '' || DataStringDeactivate != '') {
            Moderate(DataStringApprove, DataStringBan, DataStringDeactivate);
        }
        else
            alert("No Records To Save");
    }
</script>
</head>
<body class="header-fixed-inner sellcar">
    <form id="form1" runat="server">
         <!-- #include file="/includes/header.aspx" -->
        <div class="container margin-bottom20">
             <div class="grid-12">
                        <div class="breadcrumb margin-bottom15">
                            <!-- breadcrumb code starts here -->
                            <ul class="special-skin-text">
                                <li><a href="/community/">Community</a></li>
                                <li><span class="fa fa-angle-right margin-right10"></span><a href="default.aspx">Moderator's Home</a></li>
                                <li><span class="fa fa-angle-right margin-right10"></span>Posts To Moderate</li>
                            </ul>
                            <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black special-skin-text">Posts In Moderation</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
            <section class="grid-12">
                <div>
            <asp:HiddenField ID="ID" runat="server" />
            <asp:HiddenField ID="CustomerId" runat="server" />
            <asp:Repeater ID="rptReport" runat="server" OnItemCommand="rptReport_ItemCommand">
					    <headertemplate>
						    <table id="ModerateData" width="962" cellspacing="0" cellpadding="5" class="bdr" border="1">
							    <tr class="dtHeader">
								    <%--<td>Id</td>--%>
								    <td>Handle Name</td>
								    <td width="150">Topic </td>
                                    <td width="300">Message</td>
								    <td>Date</td>
                                    <td>Approve</td>
                                    <td>Ban</td>
                                    <td>Delete</td>
								    </tr>
					    </headertemplate>
					    <itemtemplate>
							    <tr>
								    <%--<td valign="top">
                                        <%# Eval("ID") %>
								    </td>--%>
								    <td valign="top">
                                        <%# Eval("HandleName") %>
								    </td>
                                    <td valign="top">
                                        <%# Eval("Topic") %>
								    </td>
								    <td valign="top">
                                        <%# Eval("Message") %>
								    </td>
								    <td valign="top">
                                        <%# Eval("MsgDateTime") %>
								    </td>
                                    <td valign="top">
                                        <input name="rd<%=counter %>" forumid = "<%# Eval("ForumId") %>" customerid="<%# Eval("CustomerId") %>" threadid="<%# Eval("ID") %>" type="radio" class="approve"/>                                    
                                    </td>
                                    <td valign="top">
                                        <input name="rd<%=counter %>" type="radio" forumid = "<%# Eval("ForumId") %>" customerid="<%# Eval("CustomerId") %>" threadid="<%# Eval("ID") %>" class="ban" />                                  
                                    </td> 
                                     <td valign="top">
                                        <input name="rd<%=counter %>" type="radio" forumid = "<%# Eval("ForumId") %>" customerid="<%# Eval("CustomerId") %>" threadid="<%# Eval("ID") %>" class="Deactivate" />                                    
                                    </td> 
							    </tr><% counter++; %>
					    </itemtemplate>
					    <footertemplate>
						    </table>
                            <input id="btnSave" type="button" class="btn btn-orange btn-xs margin-top10" value="Save" onclick="javascript: save();"/>                  
					    </footertemplate>
				    </asp:Repeater>	
        </div>
            </section>
            <div class="clear"></div>
        </div>
     <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
    </form>
</body>
</html>
