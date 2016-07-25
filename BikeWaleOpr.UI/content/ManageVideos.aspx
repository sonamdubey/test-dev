<%@ Page Inherits="BikeWaleOpr.Content.ManageVideos" AutoEventWireup="false" Language="C#" Trace="false" Debug="false" EnableEventValidation="false" %>
<!-- #include file="/includes/headerNew.aspx" -->
<style type="text/css">
    .left h3 {clear:both;color:#000000; padding:7px 0 7px 5px; font-family:"trebuchet ms",arial,sans-serif; font-size:16px; margin:0 0 0px 5px;display:block;}
</style>
<div class="urh">
		You are here &raquo; Contents &raquo; Manage Videos
</div>
<!-- #Include file="ContentsMenu.aspx" -->
<div class="left">
      
     <div>
        <fieldset>
            <legend><b>Filter Videos</b></legend>
            <table>
                <tr>
                    <td >
                        <span>
                            Select Make<span class="errorMessage">* </span>: <asp:DropDownList ID="ddlFilterMake" runat="server"></asp:DropDownList></span>
                        <span class="margin-left5">
                            Select Model : <asp:DropDownList ID="ddlFilterModel" runat="server"><asp:ListItem Text="--Select Model--" Value="0"></asp:ListItem></asp:DropDownList>
                            <input type="hidden" id="hdn_ddlFilterModel" runat="server" />
                            <asp:HiddenField Id="hdnFilterSelModelId" runat="server" />
                        </span>
                        <span class="margin-left5">
                            IsActive : <asp:CheckBox ID="chkShowActiveVideos" runat="server" />
                        </span>
                        <span class="margin-left10">
                            <asp:Button runat="server" id="btnShow" Text="SHOW" />
                            <%--<asp:Button runat="server" id="btnShowAll"  Text="SHOW ALL" />--%>
                        </span>
                    </td> 
                </tr>
            </table>
        </fieldset>        
    </div>    
    <div style="margin-top:20px;"> 
        <fieldset>
		    <legend><b>Add/Update Video</b></legend>
            <table cellpadding="3" cellspacing="0" style="width:100%;">
                <tr>
                     <td colspan="2"><asp:Label ID="lblMessage" CssClass="errorMessage" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span>
                            <b>Select Make : </b><span class="errorMessage">*</span>
                            <asp:DropDownList ID="ddlMake" runat="server"></asp:DropDownList>
                        </span>
                        <span id="spnMake" style="color:red" ></span>
                        <span class="margin-left5">
                            <b>Select Model : </b><span class="errorMessage">*</span>
                            <asp:DropDownList ID="ddlModel" runat="server">
                                <asp:ListItem Text="--Select Model--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            <input type="hidden" id="hdn_ddlModel" runat="server" />
                            <asp:HiddenField Id="hdnSelModelId" runat="server" />
                        </span>
                        <span id="spnModel" style="color:red" ></span>
                    </td>
                </tr>
                <tr> 
                    <td align="left"><b>Video Source:</b><span class="errorMessage">*</span></td>
                    <td><asp:TextBox id="txtVideoSrc" size="130px" name="VideoSrc" runat="server" />&nbsp;&nbsp;<span id="spnVideoSrc" style="color:red" ></span></td>
                </tr> 
                <tr> 
                    <td align="left"><b>Video Title:</b><span class="errorMessage">*</span></td>
                    <td><asp:TextBox id="txtVideoTitle" size="130px" name="VideoTitle" runat="server" />&nbsp;&nbsp;<span id="spnVideoTitle" style="color:red" ></span></td>
                </tr> 
                <tr>
                    <td align="left">
                        <b>Status</b><span class="errorMessage">*</span>
                    </td>
                    <td>
                        <asp:CheckBox ID= "chkStatus" Text="IsActive" runat= "Server"/><span id="spnStatus" style="color:red" ></span>                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button runat="server" id="btnsub" Text="Submit Video" />
                        <asp:Button runat="server" id="btnUpdate" Text="Update Video" CssClass="hide" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField Id="hdnVideoId" runat="server" />
        </fieldset>
        <asp:Label ID="spnErrNoData" CssClass="errorMessage" style="margin-top:20px; font-size:15px;" runat="server"></asp:Label>       
        <div style="margin-top:20px;">
            <asp:Repeater id="rptModelVideos" runat="server">
                <HeaderTemplate>
                      <table class="tblModelVideo" cellspacing="0" cellpadding="3" rules="all" border="1"  style="border-width:1px; width:70%;border-style:solid;border-collapse:collapse;" >
                        <tr style="background-color:#808080;color:white;" >
                            <th  align="center">Sr No</th>
                            <th  align="center">Model</th>
                            <th  align="center">Video</th>
                            <th  align="center">Status</th>
                            <th  align="center"></th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                        <tr>
                            <td align="center"> <b><%= ++serialNo%></b></td>
                            <td  align="center"><b><%#DataBinder.Eval(Container.DataItem,"BikeName")%></b></td>
                            <td  align="center"><iframe class="youtube-player" type="text/html" src="<%#DataBinder.Eval(Container.DataItem,"VideoSrc").ToString() %>" width="350" height="200" frameborder="0"></iframe> </td>
                            <td  align="center"><%#DataBinder.Eval(Container.DataItem,"IsActive").ToString()=="1"?"Active":"InActive" %></td>
                            <td  align="center"><a style="cursor:pointer;" videoId="<%#DataBinder.Eval(Container.DataItem,"VideoId")%>" videolink="<%#DataBinder.Eval(Container.DataItem,"VideoSrc")%>" isActive="<%#DataBinder.Eval(Container.DataItem,"IsActive")%>" make="<%#DataBinder.Eval(Container.DataItem,"Make")%>" model="<%#DataBinder.Eval(Container.DataItem,"Model")%>" makeId="<%#DataBinder.Eval(Container.DataItem,"MakeId")%>" modelId="<%#DataBinder.Eval(Container.DataItem,"ModelId")%>" videoTitle="<%#DataBinder.Eval(Container.DataItem,"VideoTitle")%>" class="edit"> <img src="http://opr.carwale.com/images/icons/edit.gif" title="edit" border="0"/></a></td>
                        </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </Table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>    
    
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $("#ddlFilterMake").change(function () {
            var requestType = "New";
            var makeId = $(this).val();

            $("#hdnFilterSelModelId").val("");

            if (makeId != 0) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"requestType":"' + requestType + '", "makeId":"' + makeId + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');

                        var dependentCmbs = new Array();
                        bindDropDownList(resObj, $("#ddlFilterModel"), "hdn_ddlFilterModel", dependentCmbs, "--Select Model--");
                    }
                });
            } else {
                $("#ddlFilterModel").val("0").attr("disabled", true);
            }
        });

        $("#ddlFilterModel").change(function () {
            $("#hdnFilterSelModelId").val($(this).val());
        });

        $("#ddlMake").change(function () {           
            var makeId = $(this).val();
            ddlMakeChanged(makeId);
        });

        $("#ddlModel").change(function () {
            ddlModelChanged($(this).val());
        });

        function ddlMakeChanged(makeId)
        {
            var requestType = "NEW";

            $("#hdnSelModelId").val("");

            if (makeId != 0) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                    data: '{"requestType":"' + requestType + '", "makeId":"' + makeId + '"}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModels"); },
                    async: false,
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');

                        var dependentCmbs = new Array();
                        bindDropDownList(resObj, $("#ddlModel"), "hdn_ddlModel", dependentCmbs, "--Select Model--");
                    }
                });
            } else {
                $("#ddlModel").val("0").attr("disabled", true);
            }
        }

        function ddlModelChanged(modelId) {
            $("#hdnSelModelId").val(modelId);
        }

        $(".edit").click(function () {
            var makeId = $(this).attr("makeId");
            var modelId = $(this).attr("modelId");
            var make = $(this).attr("make");
            var model = $(this).attr("model");
            var videolink = $(this).attr("videolink");
            var Status = $(this).attr("isActive");
            var videoid = $(this).attr("videoid");
            var videoTitle = $(this).attr("videoTitle");

            $('#ddlMake').val(makeId);
            ddlMakeChanged(makeId);

            $('#ddlModel').val(modelId);
            ddlModelChanged($('#ddlModel').val());
            
            $('#txtVideoSrc').val(videolink);
            $("#txtVideoTitle").val(videoTitle);

            Status == "True" ? $("#chkStatus").attr('checked', true) : $("#chkStatus").attr('checked', false);
            
            $("#hdnVideoId").val(videoid);
            
            $("#btnUpdate").show();
            $("#btnsub").hide();
        });

        $("#btnsub, #btnUpdate").click(function () {
            if (!IsValidVideoData())
                return false;
        });

        function IsValidVideoData() {
            var isValid = true;

            var spnMake = $("#spnMake");
            var spnModel = $("#spnModel");
            var spnVideoSrc = $("#spnVideoSrc");
            var spnVideoTitle = $("#spnVideoTitle");

            spnMake.text("");
            spnModel.text("");
            spnVideoSrc.text("");
            spnVideoTitle.text("");

            var Make = $("#ddlMake").val();
            var Model = $("#ddlModel").val();
            var VideoSrc = $("#txtVideoSrc").val();
            var videoTitle = $("#txtVideoTitle").val();

            if (Make <= 0) {
                spnMake.text("please select Make.");
                isValid = false;
            }
            if (Model <= 0) {
                spnModel.text("please select Model.");
                isValid = false;
            }
            if (VideoSrc <= "") {
                spnVideoSrc.text("Please enter Video Link");
                isValid = false;
            }
            if (videoTitle <= "") {
                spnVideoTitle.text("Please enter Video Title");
                isValid = false;
            }
            
            return isValid;
        }

    });
</script>
<!-- #include file="/includes/footerNew.aspx" -->