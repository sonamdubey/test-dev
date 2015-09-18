<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.Content.BikeSeries" Trace="false" debug="false"%>
<!-- #Include file="/includes/headerNew.aspx" -->
<script type="text/javascript" src="/src/common/common.js?V1.1"></script>
<script type="text/javascript" src="../src/graybox.js"></script>
<style>
    .rptTable {border: 1px solid black;border-collapse: collapse;margin-top:20px;}
    .rptTable td {padding : 5px 5px 5px 10px;border: 1px solid black;}
    .rptTable th{background-color:#E5E5E5;font-weight:bold; text-align:center;border: 1px solid black;}
    .rptTable a {text-decoration:underline;cursor:pointer;}
    .alignCenter {text-align:center;}
</style>
<!DOCTYPE html>
<div class="urh">
		You are here &raquo; Contents &raquo; Add Bike Series
</div>
<div>
    <!-- #Include file="contentsMenu.aspx" -->
</div>
<body>
    <div class="left">
            <div>
                <div>
                    <fieldset>
                        <legend> Add New Bike Series</legend>
                        <div class="margin10">
                             <div class="floatLeft margin-right10 inputWidth ">Select Make :</div>
                             <div class="floatLeft inputWidth "><asp:DropDownList ID="cmbMakes" AutoPostBack="true" runat="server" Width="120px"/></div>
                             <div class="floatLeft margin-left10 "><span id="spnErrMake" class="errorMessage"></span></div>
                             <div class="clear"></div>
                        </div>
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
                        <div class="margin10"><asp:Button ID="btnSave" Text="Add Series" runat="server" /></div>
                        <div class="margin10"><span id="spnKeyErr" class="errorMessage" runat="server"></span></div>
                    </fieldset>
                </div>
                <div>
                    <asp:Label ID="lblStatus" runat="server" class="errorMessage" />
                    <asp:Repeater id="rptSeries" runat="server">
                        <HeaderTemplate>
                            <table class="rptTable" width="100%">
                                <tr >
                                    <th>Series</th>
                                    <th>MaskingName</th>                                    
                                    <th>Update Photos</th>
                                    <th>Synopsis</th>
                                    <th>Entry Date</th> 
                                    <th>Updated On</th>                              
                                    <th>Last Updated By</th>
                                    <th></th>
                                    <th></th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class='seriesRow_<%#Eval("ID") %>'>
                                <td class="series"><%#Eval("Name")%></td>
                                <td><%#Eval("MaskingName")%></td>                           
                                <td class="alignCenter"><input type="button" value="Upload" onclick="javascript:window.open('seriesphotos.aspx?series=<%# DataBinder.Eval( Container.DataItem, "ID" ) %>','','left=500,top=100,width=500,height=300,scrollbars=yes')"/></td>
                                <td class="alignCenter"><input type="button" value="Add" onclick="javascript:window.open('seriessynopsis.aspx?series=<%# DataBinder.Eval( Container.DataItem, "ID" ) %>    ','','left=350,top=80,width=600,height=500,scrollbars=yes')"/></td>
                                <td><%#Eval("EntryDate")%></td>
                                <td><%#Eval("UpdatedOn")%></td>
                                <td><%#Eval("UserName")%></td> 
                                <td class="alignCenter"><a id='edit_<%#Eval("ID")%>'><img border=0 src="http://opr.carwale.com/images/edit.jpg"/></a></td>
                                <td class="alignCenter"><a id='delete_<%#Eval("ID")%>'><img border=0 src="http://opr.carwale.com/images/icons/delete.ico"/></a></td>              
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                        </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        
   </div>
<!-- #Include file="/includes/footerNew.aspx" -->
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {

            $("a[id^='delete_']").click(function () {
                var seriesId = $(this).attr('id').split('_')[1];
                var seriesName = $(this).parents().find(".series").html();
        
                if (confirm("Are you sure want to delete this series?")) {
                    $.ajax({
                        type: "POST",
                        url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                        data: '{"seriesId":"' + seriesId + '"}',
                        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "DeleteSeries"); },
                        success: function (response) {
                            $(".seriesRow_" + seriesId).html("<td colspan='8'>" + seriesName + "  series has been deleted</td>").addClass("orange");
                        }
                    });
                }
            })

            $("a[id^='edit_']").click(function () {    
                var comment = "";
                var caption = "Update Series Details";
                var seriesId = $(this).attr('id');
                seriesId = seriesId.split('_')[1];
                var url = "/content/UpdateSeriesDetails.aspx?id=" + seriesId;
                var applyIframe = true;              
                var GB_Html = ""

                GB_show(caption, url, 200, 520, applyIframe, GB_Html);
            });
           
            $('#btnSave').click(function () {
               return validate();
            });
        })

        function validate()
        {
            var isValid = true;
            $("#spnErrMake").text("");
            $("#spnErrMaskingName").text("");
            $("#spnErrSeries").text("");

            var series = $("#txtSeriesName").val();
            var newReg = new RegExp('^[0-9a-zA-Z& ]+$');
            var maskingName = $("#txtMaskingName").val();

            if ($("#cmbMakes").val() <= 0 || $("#cmbMakes").val() == "") {
                $("#spnErrMake").text("Please select make.");
                isValid = false;
            }

            if (series == "")
            {
                $("#spnErrSeries").text("Series Name Required");
                isValid = false;
            }
            else if (!newReg.test(series)){
                $("#spnErrSeries").text("It should be characters only");
                isValid = false;
            }


            if ( maskingName == "")
            {
                $("#spnErrMaskingName").text("Masking Name Required");
                isValid = false;
            }
            else if (hasSpecialCharacters(maskingName)) {
                $("#spnErrMaskingName").text("Invalid Masking Name. ");
                isValid = false;
            }

            return isValid;
        }

        $('#txtSeriesName').blur(function () {
            var series = jQuery.trim($('#txtSeriesName').val());
            series = series.trim();
            series = series.replace(/\s+/g, "-");
            series = series.replace(/[^a-zA-Z0-9\-]+/g, '');
            series = removeHyphens(series);
            $('#txtMaskingName').val(series.toLowerCase());
        });
    </script>
</body>

