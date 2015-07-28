<%@ Page AutoEventWireUp="false" Language="C#" Inherits="BikeWaleOpr.EditCms.PublishArticle" Trace="false" Debug="false" %>
<h3 class="hd2"><%= ArticleTitle %></h3>
<table <%= (unpublish == "0") ? "" : "style='display:none;'" %>">
	<tr>
		<td>
			<input id="chkAddToForums" type="checkbox" /> Add to forums
		</td>
        <td>
            <input id="chkIsDealerFriendly" type="checkbox" /> Is Dealer Friendly?
        </td>
	</tr>
	<tr>
		<td colspan="2">
			<textarea id="txtMessage" rows="7" cols="40" /><br/>
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<input id="btnPublish" type="button" value="Publish" onclick="PublishThisRecord();" />
			<input id="btnCancel" type="button" value="Cancel" onclick="CloseBox()" />
			<span id="errMessage" style="color:red;" ></span>
		</td>
	</tr>
</table>
<div <%= (unpublish == "1") ? "" : "style='display:none;'" %>">
    <span>Reasons for unpublishing* : </span>
    <textarea id="txtUnpublish" rows="7" cols="40" /><br />
                <span id="errUnpub" style="color:red;"></span> 
    <input id="btnUnpublish" type="button" value="Unpublish" onclick="IsValidUnPublishArticle();" />
    <input id="btnUnPublishCancel" type="button" value="Cancel" onclick="CloseBox()" />
    <span id="errUnpublish" style="color:red;" ></span>
</div>
<script type="text/javascript">
        function PublishThisRecord()
        {
            if (ValidPublishArticle())
            {			
                if ( confirm("Are you sure want to publish this article?") )
                {
			
                    var addToForum;
                    var isDealerFriendly;
                    if ($("#chkAddToForums").is(":checked") == true)
                        addToForum = "1";
                    else	
                        addToForum = "0";

                    if($("#chkIsDealerFriendly").is(":checked")) {
                        isDealerFriendly = 1;
                    } else {
                        isDealerFriendly = 0;
                    }
					
                    var message = $("#txtMessage").val();
                    var path = "view.aspx?id=";
                    var articleType = "0";
                    var customerId = "1406280";
                    var titleText = "";
				
                    var cid = <%=Request.QueryString["cid"].ToString() %>;
                    if ( cid == 1 )
                        titleText = " tested by BikeWale";
				
                    $.ajax({
                        type: "POST",
                        url: "/ajaxpro/BikeWale.AjaxEditCms,BikewaleOpr.ashx",			
                        data: '{"addToForum":"'+ addToForum +'", "message":"'+ message +'", "bid":"<%=Request.QueryString["bid"].ToString()%>","path":"'+ path +'","articleType":"'+ articleType +'","customerId":"'+ customerId +'","titleText":"'+ titleText +'", "isDealerFriendly":"' + isDealerFriendly + '"}',	
                        beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "PublishArticle"); },
                        success: function(response) {
                            var status = eval('('+ response +')');	
                            if (status.value == true)
                            {
                                //$(currentlyClickedLink).parent().html("Yes");
                                GB_hide();	
                                window.location.reload();
                            }	
                        }
                    });
                }	
            }	
        }
	
        function ValidPublishArticle()
        {
            var isValid = true;
            $("#errMessage").html("");
		
            if($("#chkAddToForums").is(":checked") == true)
            {
                if ($("#txtMessage").val().trim() == "")
                {
                    $("#errMessage").html("Enter message");
                    isValid = false;
                }
            }
            return isValid;
        }
	
        function IsValidUnPublishArticle()
        {
            var isValid = true;
            $("#errUnpublish").text("");
			    
            if ($("#txtUnpublish").val().trim() == "")
            {
                $("#errUnpublish").text("Specify reason for unpublishing.");
                isValid = false;
            }
            if(isValid == true)
                validate();
            else
            return isValid;
        }

        function validate(){
            $("#errUnpub").text("");

            var isError=true;
            var comments = $("#txtUnpublish").val();
            var arrLineBreak = comments.match(/\n/g);
            var commentsLength = comments.length;
            if (comments != "")
            {
                if (arrLineBreak != "" && arrLineBreak != null)
                    commentsLength += arrLineBreak.length;
                if (commentsLength > 250) {
                    $("#errUnpub").text("Exceeded maximum character limit. Please remove some characters.");
                    isError = false;
                }        
            }
            
            if(isError== true)
            {
                UnPublishThisRecord();
            }
            else
            {
                return isError;
            }
        }

        function CloseBox()
        {
            GB_hide();	
        }

        function UnPublishThisRecord()
        {
            if ( confirm("Are you sure want to unpublish this article?") )
            {
                var reasonToUnpublish = $("#txtUnpublish").val().trim();
                var customerId = <%= BikeWaleOpr.Common.CurrentUser.Id.ToString()%>;
                var cid = <%=Request.QueryString["cid"].ToString() %>;
				
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWale.AjaxEditCms,BikewaleOpr.ashx",			
                    data: '{"bid":"<%=Request.QueryString["bid"].ToString()%>","customerId":"'+ customerId +'","reasonToUnpublish":"' + reasonToUnpublish + '", "cid":"' + cid + '"}',	
                    beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UnPublishArticle"); },
                    success: function(response) {	
                        var status = eval('('+ response +')');	
                        if (status.value == true)
                        {
                            GB_hide();	
                            window.location.reload();
                        }					
                    }
                });
            }
        }
</script>
