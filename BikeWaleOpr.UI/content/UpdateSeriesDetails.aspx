<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.Content.UpdateSeriesDetails" trace="false"%>
<script type="text/javascript" src="/src/common/common.js?V1.1"></script>
<link rel="stylesheet" href="/css/common.css?V1.1" type="text/css" />
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="margin10">
            <div class="floatLeft margin-right10 inputWidth ">Series Name : </div>
            <div class="floatLeft inputWidth "><asp:TextBox  ID="txtSeriesName" MaxLength="30" runat="server"></asp:TextBox></div>
            <div class="floatLeft margin-left10 "><span id="spnErrSeries" class="errorMessage"></span></div>
            <div class="clear"></div>
        </div>
        <div class="margin10">
            <div class="floatLeft margin-right10 inputWidth ">Masking Name : </div>
            <div class="floatLeft inputWidth "><asp:TextBox ID="txtMaskingName" MaxLength="50" runat="server"></asp:TextBox></div>
            <div class="floatLeft margin-left10"><span id="spnErrMaskingName" class="errorMessage"></span></div>
            <div class="clear"></div>
        </div>
        <div class="margin10"><input type="button" ID="btnUpdate" value="Update Series"/></div>
        <div class="margin10"><span id="spnKeyErr" class="errorMessage"/></div>
    </div>
    </form>
    <script>
        $(document).ready(function () {
            var maskingName = '';
            var series = '';
            var seriesId = '';

            $('#btnUpdate').click(function () {

                maskingName = $("#txtMaskingName").val();
                series = $("#txtSeriesName").val();
                seriesId = '<%=Request.QueryString["id"].ToString() %>';

                if (!validate())
                {
                    //alert("series name :" + series + " masking name :" + maskingName + "series id : " + seriesId);
                    $.ajax({
                        type: "POST",
                        url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                        data: '{"name":"' + series + '","maskingName":"' + maskingName + '","seriesId":"' + seriesId + '"}',
                        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateSeriesMaskingName"); },
                        success: function (response) {
                            if (!eval('(' + response + ')').value) {
                                $("#spnKeyErr").text("Series name or series masking already exists. Can not insert duplicate name.");
                            }
                            else {
                                GB_hide();
                                //window.close();
                                window.location.reload();
                                //window.location.href = window.location.pathname;
                                //window.location.href = window.location.href;
                                //RefreshForm.submit();
                                //window.opener.history.go(0);
                                //window.close();
                                //window.opener.location.reload();
                                //window.opener.location.reload(true); self.close();
                                //opener.Form.submit() & self.close();
                                //window.location.reload();
                                //window.location = window.location;
                            }
                        }
                    });
                }
            });

            function validate() {
                $("#spnErrSeries").text("");
                $("#spnErrMaskingName").text("");
                $("#spnKeyErr").text("");
              
                var isError = false;
                //var series = $("#txtSeriesName").val();
                var newReg = new RegExp('^[0-9a-zA-Z& ]+$');
            
                if (series === '') {            
                    $("#spnErrSeries").text("Enter Series");                  
                    isError = true;
                }
                else if (!newReg.test(series)) {
                    $("#spnErrSeries").text("It shosuld be characters only.");
                    isError = true;
                }

                if (maskingName === "") {
                    $("#spnErrMaskingName").text("Masking Name Required.");
                    isError = true;
                }
                else if (hasSpecialCharacters(maskingName)) {
                    $("#spnErrMaskingName").text("Invalid Masking Name. ");
                    isError = true;
                }
                if (isError)
                    return false;
            }
        });
    </script>
</body>
</html>
