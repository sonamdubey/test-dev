<%@ Page Language="C#" AutoEventWireup="false"  Inherits="BikeWaleOpr.Content.SeriesPhotos"  Trace="false" debug="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script src="/src/jquery-1.6.min.js" type="text/javascript"></script>
    <link href="/css/Common.css" rel="stylesheet" />
    <style type="text/css">
        .panel { background-color: #FFF0E1;border: 1px solid orange;padding: 5px;}
        .border { border:1px solid #808080;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table id="imgTable" class="tableViw margin10">
            <tr>
                <th>Original Image</th>
            </tr>
            <tr>
                <td  class="border" style="padding:10px;">
                    <img id="imgSmall" image-id="<%=seriesId %>" src='<%= !String.IsNullOrEmpty(originalImagePath) ? BikeWaleOpr.ImagingOperations.GetPathToShowImages(hostUrl,"227X128",originalImagePath) : "http://img.carwale.com/bikewaleimg/common/nobike.jpg"%>'/>
                </td>
            </tr>
        </table>
        <div>
            <asp:Panel ID="pnlAdd" CssClass="panel" runat="server" Visible="true">
            <div style="padding-bottom:5px;font-weight:bold; color:#FF3300">Upload Photos</div>   
            Upload Series Pic <input type="file" id="filPhoto" runat="server" /><br />
            <span id="err" class="error"></span><br />
                <span style="color:red;">Image Size : 196 X 106 px</span><br />
            <asp:Button ID="btnSave" Text="Upload Photo" runat="server" />
        </asp:Panel>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        var refreshTime = 2000;
        $(document).ready(function () {
            setInterval(UpdatePendingMainImage, refreshTime)

            $('#btnSave').click(function () {
                if ($('#filPhoto').val() == "") {
                    alert("Please upload bike series photo. ");
                    return false;
                }
            });
        });

        function UpdatePendingMainImage() {
            var id = $(".border").find('img').attr('image-id');
            CheckMainImageStatus(id);
        }

        function CheckMainImageStatus(mainImageId) {
            $.ajax({
                type: "POST", url: "/AjaxPro/BikeWaleOpr.Common.Ajax.ImageReplication,BikewaleOpr.ashx",
                data: '{"imageId":"' + mainImageId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "checkImageStatus_SeriesPhotos"); },
                success: function (response) {
                    var ret_response = eval('(' + response + ')');
                    //alert(ret_response.value);
                    var obj_response = eval('(' + ret_response.value + ')');
                    if (obj_response.Table.length > 0) {
                        for (var i = 0; i < obj_response.Table.length; i++) {
                            var imgUrlLarge = obj_response.Table[i].HostUrl + "227X128" + obj_response.Table[i].OriginalImagePath;

                            $("#imgSmall").attr('src', imgUrlLarge);
                        }

                    }
                }
            })

        }


    </script>
        
</body>
</html>
