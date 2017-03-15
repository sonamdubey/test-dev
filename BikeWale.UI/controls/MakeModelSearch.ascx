<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.MakeModelSearch" %>

<div class="margin-top10 content-block grey-bg">
    <a id="lnkShowAll" href="#" class="right-float">Show all</a>   
    <span>Search by:&nbsp;</span>
	<asp:DropDownList ID="drpMake" runat="server"/>&nbsp;
	<asp:DropDownList ID="drpModel" runat="server" Enabled="false"><asp:ListItem Selected="true" Text="--Select Model--" Value="-1"></asp:ListItem></asp:DropDownList>&nbsp;
	<a id="btnMakeModelSearch" class="buttons" onclick="javascript:MakeModelSearch_click()">Go</a>     
    <input style="clear:both;" type="hidden" id="hdn_drpModel" runat="server" />
    <input type="hidden" id="hdn_selModel" runat="server" />
    
</div>
<script type="text/javascript">
    $(document).ready(function () {

        //if ($("#<%=drpMake.ClientID.ToString() %>").val() > 0) {            
        //    fillModel();
        //}

        $("#lnkShowAll").click(function (e) {
            e.preventDefault();
            var url = '/expert-reviews/';
            var loc = '/';
            if (url.indexOf("expert-reviews") > 0) {
                loc += "expert-reviews/";
            } else if (url.indexOf("comparos") > 0) {
                loc += "comparos/";
            }
            window.location.href = loc;
        });       
    });

    function MakeModelSearch_click() {
        var url = '/expert-reviews/';
        if ($("#MakeModelSearch_drpMake").val() == 0) {
            if ($("#MakeModelSearch_drpModel").val() == "") {
                window.location.href = '/new/' + url.split('/')[2] + '/';
                return;
            } else {
                alert("Please select a make.");
                return false;
            }
        }

        var makeValueArray = $("#MakeModelSearch_drpMake option:selected").val();
        var makeId = makeValueArray.split('_')[0];
        var makeName = makeValueArray.split('_')[1];
        var modelValueArray = $("#MakeModelSearch_drpModel option:selected").val();
        var modelId = modelValueArray.split('_')[0];
        var modelName = modelValueArray.split('_')[1];
        var base = '/';
        var loc = base + makeName + "-bikes/";

        if (modelId != "" && modelId != null && modelId > 0) {            
            loc += modelName + "/";
        }
        if (url.indexOf("expert-reviews") > 0) {
            loc += "expert-reviews/";
        } else if (url.indexOf("comparos") > 0) {
            loc += "comparos/";
        }      
        window.location.href = loc;
    }

   

    $("#<%=drpMake.ClientID.ToString() %>").change(function () {
        fillModel();
    });

    function fillModel() {
        if ($("#<%=drpMake.ClientID.ToString() %>").val() == "0") {
            $("#<%=drpModel.ClientID.ToString() %>").empty().append("<option value\"-1\">--Select Model--</option>").attr("disabled", "disabled");
        }
        else {
            //alert("hi");
            var MakeId =  $("#<%=drpMake.ClientID.ToString() %>").val().split('_')[0];
            $("#<%=drpModel.ClientID.ToString() %>").empty().append("<option value\"\">Loading...</option>");
            var reqType = '<%= RequestType %>';
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"requestType":"' + reqType + '" , "makeId":"' + MakeId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModelsWithMappingName"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    if (responseJSON.value != "") {
                        var resObj = eval('(' + responseJSON.value + ')');
                        var dependentCmbs = new Array;
                        bindDropDownList(resObj, $("#<%= drpModel.ClientID.ToString() %>"), "MakeModelSearch_hdn_drpModel", dependentCmbs, "All Models");
                    }
                    else
                        $("#<%=drpModel.ClientID.ToString() %>").empty().append("<option value\"\">All Models</option>");
                }
            });
        }
    }

</script>