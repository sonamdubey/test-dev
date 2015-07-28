<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.EditCms.ManageImageTagging" EnableEventValidation="false"  Trace="false" %>
<%@ Import Namespace="BikeWaleOpr" %>
<%@ Import Namespace="System.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/ecmascript" src="../src/AjaxFunctions.js"></script>
    <script type="text/javascript" src="../src/jquery-1.6.min.js"></script>
</head>
<body>
    <form  runat="server">
        <div id="divUpdateTagging" class="clear">
            <table style="padding:5px; height:200px;">
                <tr>
                    <td>Caption :</td>
                    <td>
                        <asp:TextBox ID="txtCaption" runat="server" style="width:180px;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Make :</td>
                    <td>
                        <asp:DropDownList id="ddlMakes" runat="server" style="width:180px;"></asp:DropDownList><span class="errMake" style="color:red;margin-left:5px;"></span>
                    </td>
                </tr>
                <tr>
                    <td>Model :</td>
                    <td>
                        <asp:DropDownList id="ddlModels" runat="server" style="width:180px;"></asp:DropDownList><span class="errModel" style="color:red;margin-left:5px;"></span>
                        <asp:HiddenField ID="hdnModels" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <label><asp:CheckBox id="chkMainImage" runat="server" />Is Main Image</label>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Button id="btnUpdateTagging"  Text="Update Tagging" runat="server"/>  </td>
                </tr>
            </table>        
        </div>
    </form>
    <script type="text/javascript">
       
        $(document).ready(function () {
            $(".errMake").text("");
            $(".errModel").text("");

            $("#ddlMakes").change(function () {
                var makeId = $(this).val();
                fillmodels(makeId);
            });

            $("#btnUpdateTagging").click(function ()
            {
                var isError = true;
                $(".errMake").text("");
                $(".errModel").text("");

                $("#txtCaption").val();
                $("#ddlMakes").val();
                $("#hdnModels").val($("#ddlModels").val());
                $("#chkMainImage").is(':checked');

                if ($("#ddlMakes").val() == "0") {
                    $(".errMake").text("Required");
                    isError = false;
                }

                if ($("#ddlModels").val() == "0") {
                    $(".errModel").text("Required");
                    isError = false;
                }
                return isError;
            });
           
            function fillmodels(makeid) {
                if (makeid > 0)
                {
                    $.ajax({
                        type: "POST",
                        url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                        data: '{"makeId":"' + makeid + '"}',
                        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                        success: function (response) {
                            var responseJSON = eval('(' + response + ')');
                            var resObj = eval('(' + responseJSON.value + ')');
                            bindDropDownList(resObj, $("#ddlModels"), "", "--Select Model--");
                        }
                    });
                }
                else
                {
                    $("#ddlModels").val("0").attr("disabled", true);
                }
            }
        });
    </script>
</body>
</html>
