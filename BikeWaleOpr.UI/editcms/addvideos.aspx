<%@ Page AutoEventWireup="false" Language="C#" Inherits="BikeWaleOpr.EditCms.Videos" Debug="false" Trace="false" %>
<%@ Register TagPrefix="Ec" TagName="EditCmsCommon" Src="/editcms/EditCmsCommon.ascx" %>
<!-- #include file="../includes/headerNew.aspx" -->

<div class="urh">
	<a href="/default.aspx">BikeWale operations</a> &raquo; <a href="/editcms/default.aspx">Editorial Home</a> &raquo; Manage Articles
</div>
<div style="clear:both;">
	<form id="Form1" runat="server">
		<asp:Label ID="lblEditImageId" runat="server" Text="-1" Visible="false" />
		<div>
			<Ec:EditCmsCommon ID="EditCmsCommon" runat="server" />
		</div>
        <div>
            <fieldset style="width:40%;" class="margin-top10">
                <div class="margin-top10">
                    <span>Video URL : </span><asp:textbox id="txtVideoUrl" size="88px" runat="server" />
                    <span id="spnErr" class="errorMessage"></span>
                </div>
                <div class="margin-top10">
                    <asp:button id="btnSave" runat="server" text="Save" onclientclick="javascript:if (GetVideoDetailsFromYouTube()==false) {return false; }" />
                    <asp:button id="btnDelete" runat="server" text="Delete"/>
                </div>
            </fieldset>
        </div>
        <div class="clear:both"></div>
        <% if(!string.IsNullOrEmpty(videoUrl)) {%>
        <div>
            <h3>Video is:</h3><br />
            <fieldset style="width: 40%">
                <iframe class="youtube-player" type="text/html" src="http://www.youtube.com/embed/<%= videoUrl %>" width="520" height="300" frameborder="0"></iframe>                
                <div class="margin-top10">
                    <span><b>Views : </b></span><span id="spnViews"><%= views %></span>
                    <span><b>Likes : </b></span><span id="spnLikes"><%= likes %></span>
                </div>
            </fieldset>
        </div>
        <% } %>
        <asp:HiddenField value="" ID="hdn_views" runat="server"></asp:HiddenField>
        <asp:HiddenField value="" ID="hdn_likes" runat="server"></asp:HiddenField>
        <asp:HiddenField value="" ID="hdn_duration" runat="server"></asp:HiddenField>
    </form>
</div>
<!-- #Include file="../includes/footerNew.aspx" -->

<script type="text/javascript">
    var basicId = "";
    var views = "";
    var likes = "";
    var videoId = "";
    var isComp = false;
    var duration = "";
    $("#spnErr").text("");

    function GetVideoDetailsFromYouTube() {
        if ($('#txtVideoUrl').val() != "") {
            videoUrl = $.trim($('#txtVideoUrl').val());
            videoId = videoUrl.split('=')[1];
            basicId = querySt("bid");
            retrieveParameters();            
        }
        else {
            $("#spnErr").text("Please Enter YouTube video link");
            isComp = false;
        }

        return isComp;
    }

    function retrieveParameters() {
        $("#spnErr").text("Wait while uploading video.");
        $.ajax({
            type: "GET",
            url: "http://gdata.youtube.com/feeds/api/videos/" + videoId + "?v=2&alt=json",
            dataType: 'json',
            async: false,
            success: function (response) {
                $("#hdn_views").val(response.entry.yt$statistics.viewCount);
                $("#hdn_likes").val(response.entry.yt$rating.numLikes);
                $("#hdn_duration").val(response.entry.media$group.media$content[0].duration);
                $("#spnErr").text("");
                isComp = true;
            }
        });
    }


    function querySt(Key) {
        var url = window.location.href;
        KeysValues = url.split(/[\?&]+/);
        //alert(KeysValues);
        for (i = 0; i < KeysValues.length; i++) {
            KeyValue = KeysValues[i].split("=");
            if (KeyValue[0] == Key) {
                return KeyValue[1];
            }
        }
    }

    $("#btnDelete").click(function () {
        var response = confirm("Do you want to Delete this video?");
        return response;
    });

</script>