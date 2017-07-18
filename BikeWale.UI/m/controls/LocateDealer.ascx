<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.LocateDealer" %>
<h2 class="pgsubhead"><%=HeaderText %></h2>
<div>
    <input type="hidden" id="hdnCity" value=""/>
    <div class="box1 new-line5">  
        <div id="divMakes">
            <asp:DropDownList ID="ddlMake" runat="server" data-mini="true">
                <asp:ListItem Text="--Select Make--" Value="0" />
            </asp:DropDownList>
        </div>
        <div id="divCities">
            <div class="new-line15">
                <img id="imgLoaderCity" src="https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/circleloader.gif" width="16" height="16" style="position:relative;top:3px;display:none;" /> 
                <select data-mini="true" id="cmbCity">
                        <option value="0">--Select City--</option>         
                </select>
                <%--<asp:DropDownList ID="cmbCity" runat="server" class="textAlignLeft"/>--%>
            </div>
        </div>	     
	    <div class="new-line15">
       	     <input type="button" value="Locate Dealer" data-theme="b" id="btnSubmit" data-mini="true" data-role="button">
         </div>
    </div>
</div>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {

        var makeClientId = '<%=ddlMake_Id%>';
        
       
        if ($("#" + makeClientId).val().split('_')[0] <= 0) {
            $("#cmbCity").val(0).attr("disabled", true);
        }

        makeIdNew = $("#" + makeClientId).val().split('_')[0];
            if (makeIdNew != 0) {
                loadCities(makeIdNew);
            } else {
                $("#cmbCity").val(0).attr("disabled", true);
            }


            $("#" + makeClientId).change(function () {
          
            $("#cmbCity").val(0);
            $("#cmbCity").selectmenu("refresh", true);

            var makeId = $(this).val().split('_')[0];

                if (makeId != 0)
                {
                    $("#imgLoaderCity").show();
                  loadCities(makeId)
                }
                else 
                {
                    $("#cmbCity").val(0).attr("disabled", true);
                }
        });

        $("#btnSubmit").click(function () {
         
            var makeId = $("#" + makeClientId).val().split('_')[0];
            var cityId = $("#cmbCity").val().split('_')[0];
            var city = $("#cmbCity").val().split('_')[1];
            var make = $("#" + makeClientId + " option:selected").val().split('_')[1];

            if (Number(cityId) <= 0 && Number(makeId) <= 0)
            {
                alert("Please select city and make to locate dealers.");    //change the message
                return false;
            }
            else if (Number(cityId) <= 0) {
                alert("Please select city to locate dealers.");    //change the message
                return false;
            }
            else if (Number(makeId) <= 0) {
                alert("Please select make to locate dealers.");    //change the message
                return false;
            }
            else {
                var path = "/m/new/" + make + "-dealers/" + cityId + "-" + city + ".html";
                location.href = path;
            }

        });

        function loadCities(makeId)
        {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"makeId":"' + makeId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetDealersCitiesListByMakeId"); },
                success: function (response) {
                    $("#imgLoaderCity").hide();

                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');

                    var dependentCmbs = new Array();
                  
                    bindDropDownList(resObj, $("#cmbCity"), "", dependentCmbs, "--Select City--");
                }
            });
        }
    });
</script>
