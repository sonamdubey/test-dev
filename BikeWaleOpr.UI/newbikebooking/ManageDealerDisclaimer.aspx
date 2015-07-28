<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.BikeBooking.ManageDealerDisclaimer" Async="true" Trace="false" Debug="false" EnableEventValidation="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Dealer Disclaimer</title>
    <link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
    <script type="text/javascript" src="http://st.carwale.com/jquery-1.7.2.min.js?v=1.0" ></script>    
    <script src="/src/AjaxFunctions.js" type="text/javascript"></script>
    <script src="/src/graybox.js"></script>   
</head>
<body>
    <form id="form1" runat="server">
        <h1 class="margin-left10">Manage Dealer Disclaimer :</h1>
        <fieldset class="margin-left10 margin-top10 padding10">
	        <legend >Add New Disclaimer</legend>            
            <div>
                Select Make<span class="errorMessage">*</span> : 
                <asp:DropDownList ID="ddlMake" runat="server" CssClass="margin-right5"/>
                <span id="spntxtMake" class="errorMessage margin-right5"></span>
                Select Model :
                <asp:DropDownList ID="ddlModel" runat="server" CssClass="margin-right5">
                    <asp:ListItem value="0">--Select Model--</asp:ListItem>
                </asp:DropDownList>
                Select Version : 
                <asp:DropDownList ID="ddlVersions" runat="server">
                    <asp:ListItem value="0">--Select Versions-- </asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="margin-top10">
                Detailed Dealer Disclaimer :<asp:TextBox runat="server" ID="txtAddDisclaimer" />
                <span id="spntxtValidDisclaimer" class="errorMessage"></span>
            </div>
            <div class="margin-top10">
                <asp:Button ID="btnAddDisclaimer" runat="server" Text="Add" />
            </div>            
        </fieldset>
        <div class="hide" id="updHtml" style="float:left;">
            New Disclaimer : <asp:TextBox ID="txtEditText" runat="server"/>&nbsp;                            
                            <asp:Button ID="btnUpdate" Text="Update" runat="server" />
                            <span class="errorMessage" id="spnUpdateText"></span>
        </div>        
        <asp:HiddenField id="hdn_ddlMake" runat="server"/>
        <asp:HiddenField id="hdn_ddlModel" runat="server"/>
        <asp:HiddenField id="hdn_ddlVersions" runat="server"/>
        <asp:HiddenField id="hdn_Disclaimer" runat="server"/>
        <div id="dataTable">        
            <asp:Label runat="server" ID="labAdded" class="errorMessage" ></asp:Label>
            <asp:Repeater id="rptAddedDisclaimer" runat="server">
                <HeaderTemplate>
                 <h3 class="margin-left10 margin-bottom10;">Added Bike Disclaimers :</h3>
                 <table border="1" style="border-collapse:collapse;" cellpadding ="5" class="margin-left10 lstTable" >
                    <tr>
                        <th>Id</th>
                        <th>Make</th>
                        <th>Model</th>
                        <th>Version</th>
                        <th>Disclaimer</th>
                        <th>Edit Disclaimer</th>
                        <th>Delete Disclaimer</th>
                    </tr>
                 </HeaderTemplate>
                 <ItemTemplate> 
                    <tr>
                        <td><%# DataBinder.Eval(Container.DataItem,"disclaimerId") %></td>                                                                          
                        <td class="make_<%#Eval("disclaimerId")%>"><%# DataBinder.Eval(Container.DataItem,"objMake.MakeName") %></td>                                            
                        <td class="model_<%#Eval("disclaimerId")%>"><%# DataBinder.Eval(Container.DataItem,"objModel.ModelName") %></td>                                   
                        <td class="version_<%#Eval("disclaimerId")%>"><%# DataBinder.Eval(Container.DataItem,"objVersion.VersionName") %></td>                                   
                        <td class="text_<%#Eval("disclaimerId")%>"><%# DataBinder.Eval(Container.DataItem,"disclaimerText") %></td>
                        <td class="edit"><a onclick="javascript:btnEdit_Click(<%#Eval("disclaimerId")%>)" class="pointer">Edit</a></td>
                        <td class="delete"><a onclick="javascript:btnDelete_Click(<%#Eval("disclaimerId")%>)" class="pointer">Delete</a></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </form>    
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ddlMake").change(function () {
                $("#hdn_ddlMake").val($(this).val());
                $("#hdn_ddlModel").val(0);
                $("#hdn_ddlVersions").val(0);
                $("#ddlModel").val("0").attr("disabled", "disabled");
                GetModels(this);
            });
            $("#ddlModel").change(function () {
                $("#hdn_ddlModel").val($(this).val());
                $("#hdn_ddlVersions").val(0);
                $("#ddlVersions").val("0").attr("disabled", "disabled");
                GetVersions(this);
            });
            $("#ddlVersions").change(function () {
                $("#hdn_ddlMake").val($(this).val());
                $("#hdn_ddlVersions").val($(this).val());
            });
        });

        function GetModels(e) {
            var makeId = $(e).val();

            if (makeId > 0) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"makeId":"' + makeId + '","requestType":"New"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        bindDropDownList(resObj, $("#ddlModel"), "", "--Select Models--");
                    }
                });
            } else {
                $("#ddlModel").val("0").attr("disabled", "disabled");
            }
        }

        function GetVersions(e) {
            var modelId = $(e).val();

            if (modelId > 0) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"modelId":"' + modelId + '","requestType":"New"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetVersions"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        bindDropDownList(resObj, $("#ddlVersions"), "", "--Select Version--");
                    }
                });
            } else
                $("#ddlModel").val("0").attr("disabled", "disabled");
        }
        var host = '<%=_abHostUrl%>';

        function btnDelete_Click(disclaimerId) {
            $.ajax({
                type: "POST",
                url: host + "/api/Dealers/DeleteDealerDisclaimer/?disclaimerId=" + disclaimerId,
                success: function () {
                    window.location.href = window.location.href;
                }
            });

        }
        function btnEdit_Click(disclaimerId) {

            var html = $("#updHtml").html();
            var title = "<h3>" + $(".make_" + disclaimerId).text() + " " + $(".model_" + disclaimerId).text() + " " + $(".version_" + disclaimerId).text() + "</h3>";
            html = title + html;
            
            var discId = disclaimerId;
            var url = "";
            var applyIframe = true;
            var caption = "Edit Disclaimer Text";
            var GB_Html = html;

            GB_show(caption, url, 200, 500, applyIframe, GB_Html);
            $("#txtEditText").val($('.text_' + disclaimerId).text());
            $("#txtEditText").focus();
            $("#btnUpdate").click(function () {
                $("#spnUpdateText").text("");
                var newtxt = $("#txtEditText").val();
                var isError = false;

                if (newtxt.trim() == "") {
                    isError = true;
                    $("#spnUpdateText").text("Disclaimer Text Can't Be Empty");
                    return !isError;
                }

                if (newtxt != "" && newtxt != null) {
                    $.ajax({
                        type: "POST",
                        url: host + "/api/Dealers/EditDisclaimer/?disclaimerID=" + discId + "&newDisclaimerText=" + encodeURIComponent(newtxt.trim()),
                        success: function (response) {
                            $("#gb-content").html("Disclaimer Text updated Successfully, Please Close this Box");
                        }
                    });

                }

                $("#gb-close").click(function () {
                    window.location.href = window.location.href;
                });
            })
        }

        $("#btnAddDisclaimer").click(function () {

            $("#spntxtMake").text("");
            $("#spntxtValidDisclaimer").text("");
            var txt = $("#txtAddDisclaimer").val();
            $("#txtAddDisclaimer").focus();
            var isValid = true;
            if ($("#ddlMake option:selected").val() <= '0') {
                $("#spntxtMake").text("Please Select Make");
                isValid = false;
            }
            if (txt.trim() == "") {
                isValid = false;
                $("#spntxtValidDisclaimer").text("Please Enter Disclaimer");
            }
            return isValid;
        })
    </script>
</body>
</html>
