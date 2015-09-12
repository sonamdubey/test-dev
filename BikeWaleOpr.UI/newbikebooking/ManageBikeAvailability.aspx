<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.BikeBooking.ManageBikeAvailability" Async="true" Trace="false" enableEventValidation="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="/src/jquery-1.6.min.js" type="text/javascript"></script>
    <script src="/src/AjaxFunctions.js" type="text/javascript"></script>
    <script src="../src/graybox.js"></script>
    <script src="/src/graybox.js"></script>
    <link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />

    <style type="text/css">
        .errMessage {color:#FF0000;}
        .edit{cursor:pointer;}
        .leftfloat {
                    float: right;
                    margin-bottom: 10px;
                    margin-left: 0;
                    margin-right: 205px;
                    margin-top: 10px;
                    width: 82%;
                    }
        #btnUpdate { margin-top:10px;}
    </style>

    <title>Manage Bike Availability</title>
</head>
<body>
    <form id="addAvailability" runat="server">
        <h1>Manage New Bike Availability</h1>
        <fieldset class="margin-left10">
	            <legend >Add New Bike Availability</legend>
                <div>
                    <div class="floatLeft margin-left10" style="width:1000px;">
                        Select Make : <span class="errMessage">*</span>
                        <asp:DropDownList ID="ddlMake" runat="server"  width="12%"/>
                            <span id="spntxtMake" class="errorMessage"></span>

                        Select Model : <span class="errMessage">*</span>
                        <asp:DropDownList ID="ddlModel" runat="server"  width="12%"/>
                        <span id="spntxtModel" class="errorMessage"></span>

                        Select Version : 
                        <asp:DropDownList ID="ddlVersions" runat="server"  width="10%"/>
                        <span id="spntxtVersion" class="errorMessage"></span>
                    </div>
                    <div class="leftfloat"> 
                        Available Limit (In Days) : <span class="errMessage">*</span>
                        <asp:TextBox ID="txtdayslimit" runat="server" width="10%" />&nbsp;
                        <asp:RangeValidator ID="validatetxtdayslimit" runat="server"
                            ControlToValidate="txtdayslimit"
                            Type="Integer"
                            MinimumValue="0"
                            MaximumValue="366"
                            ErrorMessage="Please Enter Valid Integer Values" />
                        <span class="errorMessage" id="spnavbDays"></span>
                    </div>
                    <div class="floatLeft margin-left10">
                        <asp:Button ID="btnsaveData" Text="Save Data" runat="server" />
                    </div>
                </div>
        </fieldset>
        <div class="hide" id="updHtml" style="float:left;">
            New Availability(In Days) :<asp:TextBox ID="txtUpdate" MaxLength="50" runat="server"/>&nbsp;
                                        <asp:RangeValidator ID="validatetxtUpdate" runat="server"
                                            ControlToValidate="txtUpdate"
                                            Type="Integer"
                                            MinimumValue="0"
                                            MaximumValue="366"
                                            ErrorMessage="Please Enter Only Integer Values" />
                                        <span class="errorMessage" id="spnupdateDays"></span>
                                        <asp:Button ID="btnUpdate" Text="Update Data" runat="server" />
        </div>
         <asp:HiddenField id="hdn_ddlMake" runat="server"/>
         <asp:HiddenField id="hdn_ddlModel" runat="server"/>
         <asp:HiddenField id="hdn_ddlVersions" runat="server"/>
         <asp:HiddenField id="hdn_txtdayslimit" runat="server"/>
   

    <div id="dataTable">
        <div class="errorMessage">
            <asp:Label runat="server" ID="labAdded" ></asp:Label>
        </div>
        <asp:Repeater id="rptavilableData" runat="server">
            <HeaderTemplate>
             <h1>Added Bike(s) Availibilaty :</h1><br />
             <table border="1" style="border-collapse:collapse;" cellpadding ="5" class="margin-left10" >
                    <tr style="background-color:#D4CFCF;">                                
                         <th>Id</th>                                          
                         <th>Make</th>                                          
                         <th>Model</th>                       
                         <th>Version</th>                                            
                         <th>Available Limits (Days)</th>                                   
                         <th>Edit Availability Days</th>                                   
                    </tr>
         </HeaderTemplate>
         <ItemTemplate> 
            <tr>                                                                      
                <td><%# DataBinder.Eval(Container.DataItem,"AvailabilityId") %></td>                                       
                <td class="make_<%#Eval("AvailabilityId")%>"><%# DataBinder.Eval(Container.DataItem,"objMake.MakeName") %></td>                                       
                <td class="model_<%#Eval("AvailabilityId")%>"><%# DataBinder.Eval(Container.DataItem,"objModel.ModelName") %></td>                                     
                <td class="version_<%#Eval("AvailabilityId")%>"><%# DataBinder.Eval(Container.DataItem,"objVersion.VersionName") %></td>                                           
                <td class="days_<%# DataBinder.Eval(Container.DataItem,"AvailabilityId") %>"availabilityId="<%# DataBinder.Eval(Container.DataItem,"AvailabilityId") %>"><%# DataBinder.Eval(Container.DataItem, "AvailableLimit") %></td>
                <td class="edit"><a id="<%#Eval("AvailabilityId")%>" onclick="javascript:btnEdit_Click('<%#Eval("AvailabilityId")%>')">Edit</a></td>                
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
        </asp:Repeater>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ddlMake").change(function () {

                $("#ddlModel").val("0").attr("disabled", "disabled");
                GetModels(this);
            })
            $("#ddlModel").change(function () {
                $("#hdn_ddlModel").val($(this).val());
                $("#ddlVersions").val("0").attr("disabled", "disabled");
                GetVersions(this);
            })
            $("#ddlVersions").change(function () {
                $("#hdn_ddlVersions").val($(this).val());
            })
        })

        function GetModels(e) {
            var makeId = $(e).val();

            if (makeId > 0) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"makeId":"' + makeId + '","requestType":"NEW"}',
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
                    data: '{"modelId":"' + modelId + '","requestType":"NEW"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetVersions"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        bindDropDownList(resObj, $("#ddlVersions"), "", "--Select Version--");
                    }
                });
            }else 
                $("#ddlModel").val("0").attr("disabled", "disabled");
        }

        function btnEdit_Click(availabilityId) {

            var html = $("#updHtml").html();
            var title = "<h3>" + $(".make_" + availabilityId).text() + " " + $(".model_" + availabilityId).text() + " " + $(".version_" + availabilityId).text() + "</h3>";
            html = title + html;
            var hostUrl = '<%=cwHostUrl%>';
            var avbId = availabilityId;
            var url = "";
            var applyIframe = true;
            var caption = "Update Availability Days";
            var GB_Html = html;
            GB_show(caption, url, 200, 300, applyIframe, GB_Html);

            $("#txtUpdate").val($(".days_" + avbId).text());
            $("#btnUpdate").click(function () {
                $("#spnupdateDays").text("");
                var isError = false;

                if ($("#txtUpdate").val() == "") {
                    $("#spnupdateDays").text("Available Limit can not be Empty");
                    isError = true;
                }

                var days = $("#txtUpdate").val();
                if (days >= 0 && days != "") {
                    $.ajax({
                        type: "GET",
                        url: hostUrl + "/api/Dealers/EditAvailabilityDays/?availabilityId=" + avbId + "&days=" + days,
                        success: function (response) {
                            $("#gb-content").html("Bike availability updated Successfully, Please Close this Box");
                        }
                    });
                    
                }
                
                $("#gb-close").click(function () {
                    window.location.href = window.location.href;
                });
                return !isError;
            })
        }
        $("#btnsaveData").click(function () {
            $("#spntxtMake").text("");
            $("#spntxtModel").text("");
            $("#spnavbDays").text("");

            var isError = false;
            if ($("#ddlMake option:selected").val() <= '0') {
                $("#spntxtMake").text("Please Select Make");
                isError = true;
            }
            if ($("#ddlModel option:selected").val() <= '0') {
                $("#spntxtModel").text("Please Select Model");
                isError = true;
            }
            if ($("#txtdayslimit").val()=="") {
                $("#spnavbDays").text("Available Limit can not be Empty");
                isError = true;
            }
            return !isError;
        })

        $("#btnUpdate").click(function () {
            $("#spnupdateDays").text("");
            var isError = false;

            if ($("#txtUpdate").val() == "") {
                $("#spnupdateDays").text("Available Limit can not be Empty");
                isError = true;
            }
            if (isNaN($("#txtUpdate").val())) {
                $("#spnupdateDays").text("Please Enter only Numeric Values");
                isError = true;
            }
            return !isError;
        })

    </script>
         </form>
</body>
</html>
