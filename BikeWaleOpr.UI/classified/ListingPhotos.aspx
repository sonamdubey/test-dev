<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.Classified.ListingPhotos" trace="false"%>
<%@ Import Namespace="BikeWaleOpr.Common" %>
<html>
<head>
    <title>Photos By Customer</title>
    <script src="/src/jquery-1.6.min.js"></script>
    <link href="/css/Common.css" rel="stylesheet" />
    <style>
        .tdStyle { background-color:#d4d4d4; text-align:left;height:35px;}
    </style>
</head>
<body>
<div style="padding:5px;">
    <table id="listingPhotos" border="1" class="tableViw" style="border-width:1px;text-align:center;border-style:solid;border-collapse:collapse;width:100%;border-spacing:0; padding:0;">
        <tbody>
            <tr class="dtHeader" >
                <th style="color:black;font-size:13px;height:30px">Photos: Profile S<%= ProfileId %> </th>
            </tr>
            <tr><td class="tdStyle" id="tdVerified" style="font-size:11px;height:15px"><b>Verified Photos</b> &nbsp; <input id="btnVchk" onClick="checkAll(this)" type="button" value="Check All"/> &nbsp; &nbsp; &nbsp;
        <input id="btnVfake" type="button" value="Fake"/></td></tr>
            <tr class="dtItem" id="va">
                <td>
                    <div style="width:1280px;" id="Verified">
            <asp:Repeater id="rptCustomerVerifiedPhotos" runat="server">
                <ItemTemplate>
                    <div class="padding5 margin10" style="border: 1px solid #DBDBCE;overflow:hidden;width:180px;float:left;">
                        <input type="checkbox" id="chkVPhotos_<%# DataBinder.Eval(Container.DataItem,"PhotoId") %>" photoId="<%# DataBinder.Eval(Container.DataItem,"PhotoId") %>"/>
                         <div id='divImg_<%# DataBinder.Eval( Container.DataItem, "PhotoId" ).ToString()%>'>
                             <img class='img-border' id="img1" width="160" src='<%# "http://" + DataBinder.Eval(Container.DataItem, "HostUrl").ToString() + DataBinder.Eval( Container.DataItem, "DirectoryPath" ) + DataBinder.Eval( Container.DataItem, "ImageUrlThumb" ) %>' style='margin:auto; display:block;' />
                             <div style="margin:5px 0;"><b>Description : </b> <%# String.IsNullOrEmpty(DataBinder.Eval( Container.DataItem, "Description" ).ToString()) ? "N/A" : DataBinder.Eval( Container.DataItem, "Description" ).ToString() %></div>
                         </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
             <div style="clear:both"></div>
            </div></td></tr>


            <tr><td class="tdStyle" id="tdPending" style="font-size:11px;height:15px"><b>Pending Photos</b> &nbsp; <input id="btnUchk" onClick="checkAll(this)" type="button" value="Check All"/> &nbsp; &nbsp; &nbsp; <input id="btnUapprove" type="button" value="Approve"/> &nbsp; &nbsp; &nbsp;
        <input id="btnUfake" type="button" value="Fake"/></td></tr>
            <tr class="dtItem" id="pe">
                <td >
                    <div style="width:1300px;" id="Pending">
              <asp:Repeater id="rptCustomerUnVerifiedPhotos" runat="server">
                <ItemTemplate>
                    <div class="padding5 margin-left10" style="border: 1px solid #DBDBCE;overflow:hidden;width:180px;float:left;">
                        <input type="checkbox" id="chkUPhotos_<%# DataBinder.Eval(Container.DataItem,"PhotoId") %>" photoId="<%# DataBinder.Eval(Container.DataItem,"PhotoId") %>"/>
                        <div id='divImg_<%# DataBinder.Eval( Container.DataItem, "PhotoId" ).ToString()%>'> 
                            <img class='img-border' id="img1" width="160" src='<%# "http://" + DataBinder.Eval(Container.DataItem, "HostUrl").ToString() + DataBinder.Eval( Container.DataItem, "DirectoryPath" ) + DataBinder.Eval( Container.DataItem, "ImageUrlThumb" ) %>' style='margin:auto; display:block;' />
                            <div style="margin:5px 0;"><b>Description : </b> <%# String.IsNullOrEmpty(DataBinder.Eval( Container.DataItem, "Description" ).ToString()) ? "N/A" : DataBinder.Eval( Container.DataItem, "Description" ).ToString() %></div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
                    <div style="clear:both"></div>
            </div></td></tr>


            <tr><td class="tdStyle" id="tdFake" style="font-size:11px;height:15px"><b>Fake Photos</b> &nbsp; <input id="btnFchk" onClick="checkAll(this)" type="button" value="Check All"/> &nbsp; &nbsp; &nbsp; <input id="btnFapprove" type="button" value="Approve"/></td></tr>
            <tr class="dtItem" id="fa">
                <td >
                    <div style="width:1300px;" id="Fake">
            <asp:repeater id="rptCustomerFakePhotos" runat="server">
                <ItemTemplate>
                     <div class="padding5 margin-left10" style="border: 1px solid #DBDBCE;overflow:hidden;width:180px;float:left;">
                         <input type="checkbox" id="chkFPhotos_<%# DataBinder.Eval(Container.DataItem,"PhotoId") %>" photoId="<%# DataBinder.Eval(Container.DataItem,"PhotoId") %>"/>
                          <div id='divImg_<%# DataBinder.Eval( Container.DataItem, "PhotoId" ).ToString()%>'>
                        <img class='img-border' id="img1" width="160" src='<%# "http://" + DataBinder.Eval(Container.DataItem, "HostUrl").ToString() + DataBinder.Eval( Container.DataItem, "DirectoryPath" ) + DataBinder.Eval( Container.DataItem, "ImageUrlThumb" ) %>' style='margin:auto; display:block;' />
                            <div style="margin:5px 0;"><b>Description : </b> <%# String.IsNullOrEmpty(DataBinder.Eval( Container.DataItem, "Description" ).ToString()) ? "N/A" : DataBinder.Eval( Container.DataItem, "Description" ).ToString() %></div>
                        </div>
                    </div>
                 </ItemTemplate>
            </asp:repeater>
                         <div style="clear:both"></div>
            </div></td></tr>

        </tbody>
    </table>
    <div style="margin-top:10px">
    </div>
</div>
</body>
<script>
    $(document).ready(function () {
        var fakeCount = 0;
        var pendingCount = 0;
        var verifiedCount = 0;

        $('[id^="chkVPhotos_"]').each(function () {
            verifiedCount++;
        });
        $('[id^="chkFPhotos_"]').each(function () {
            fakeCount++;
        });
        $('[id^="chkUPhotos_"]').each(function () {
            pendingCount++;
        });

        if (fakeCount <= 0) {
            $("#tdFake,#fa").addClass("hide");
        }
        if (pendingCount <= 0) {
            $("#tdPending,#pe").addClass("hide");
        }
        if (verifiedCount <= 0) {
            $("#tdVerified,#va").addClass("hide");
        }
    });
  
    function checkAll(chk) {
        var id = $(chk).attr("id");
        var value = $(chk).attr("value");             

        if (value == "Check All") {
            if (id == "btnVchk") {
                $('[id^="chkVPhotos_"]').each(function () {
                    //alert($(this).is(":checked"));
                    $(this).prop('checked', true);
                });
            }
            else if (id == "btnUchk") {
                $('[id^="chkUPhotos_"]').each(function () {
                    $(this).prop('checked', true);
                });
            }
            else if (id == "btnFchk") {
                $('[id^="chkFPhotos_"]').each(function () {
                    $(this).prop('checked',true);
                });
            }
            $(chk).attr("value", "Uncheck All");
        }
        else {
            if (id == "btnVchk") {
                $('[id^="chkVPhotos_"]').each(function () {
                    //alert($(this).is(":checked"));
                    $(this).prop('checked',false);
                });
            }
            else if (id == "btnUchk") {
                $('[id^="chkUPhotos_"]').each(function () {
                    $(this).prop('checked',false);
                });
            }
            else if (id == "btnFchk") {
                $('[id^="chkFPhotos_"]').each(function () {
                    $(this).prop('checked',false);
                });
            }
            $(chk).attr("value", "Check All");
        }
    }

    $("#btnVfake").click(function () {
        var photoIdList = "";
        $("#va").find("input:checked").each(function () {
            photoIdList += $(this).attr("photoId") + ',';
        });
        if (photoIdList != "") {
            if (confirm("Are you sure you want to discard photos?") == true) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"photoIdList":"' + photoIdList + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "DiscardPhotos"); },
                    success: function (response) {
                        alert("Photos Discarded Successfully!");
                        window.location.reload(true);
                    }
                });
            }
        }
        else { alert("You should select something!") }
    });

    $("#btnUapprove").click(function () {
        var photoIdList = "";
        $("#pe").find("input:checked").each(function () {
            photoIdList += $(this).attr("photoId") + ',';
        });
        if (photoIdList != "") {
            if (confirm("Are you sure you want to approve photos?") == true) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"photoIdList":"' + photoIdList + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ApprovePhotos"); },
                    success: function (response) {
                        alert("Photos Approved Successfully!");
                        window.location.reload(true);
                    }
                });
            }
        }
        else { alert("You should select something!") }
    });

    $("#btnUfake").click(function () {
        var photoIdList = "";
        $("#pe").find("input:checked").each(function () {
            photoIdList += $(this).attr("photoId") + ',';
        });
        if (photoIdList != "") {
            if (confirm("Are you sure you want to discard photos?") == true) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"photoIdList":"' + photoIdList + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "DiscardPhotos"); },
                    success: function (response) {
                        alert("Photos Discarded Successfully!");
                        window.location.reload(true);
                    }
                });
            }
        }
        else { alert("You should select something!") }
    });

    $("#btnFapprove").click(function () {
        var photoIdList = "";
        $("#fa").find("input:checked").each(function () {
            photoIdList += $(this).attr("photoId") + ',';
        });
        if (photoIdList != "") {
            if (confirm("Are you sure you want to approve photos?") == true) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"photoIdList":"' + photoIdList + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ApprovePhotos"); },
                    success: function (response) {
                        alert("Photos Approved Successfully!");
                        window.location.reload(true);
                    }
                });
            }
        }
        else { alert("You should select something!") }
    });

</script>
</html>