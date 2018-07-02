<%@ Page Inherits="BikeWaleOpr.Content.ShowroomPrices" AutoEventWireup="false" Language="C#" Trace="false" Debug="false" EnableEventValidation="false" EnableViewState="true" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<div class="left min-height600" id="divManagePrices">
    <h1>Manage Showroom Prices</h1>
    <fieldset>
        <legend>Search Bike Prices</legend>
        <div class="form-control-box margin-right10 floatLeft">
            <asp:dropdownlist id="ddlMakes" runat="server" tabindex="1" />
            <span class="bwsprite error-icon hide"></span>
            <div class="bw-blackbg-tooltip hide">Please Select Make</div>
        </div>
        <div class="form-control-box margin-right10 floatLeft">
            <asp:dropdownlist id="ddlStates" runat="server" tabindex="2" />
        </div>
        <div class="form-control-box margin-right10 floatLeft">
            <asp:dropdownlist id="ddlCities" runat="server" tabindex="3" data-bind="value: selectedCity"><asp:ListItem Value="0" Text="--Select City--"/></asp:dropdownlist>
            <span class="bwsprite error-icon hide"></span>
            <div class="bw-blackbg-tooltip hide">Please Select City</div>
        </div>
        <asp:button id="btnSearch" text="Search" runat="server" tabindex="4" />
        <asp:hiddenfield id="hdnSelectedCity" runat="server" />
    </fieldset>
    <div class="margin-top10 floatLeft" style="width: 850px; display: inline-block;">
        <asp:repeater id="rptPrices" runat="server">
			<headertemplate>
				<table class="table-bordered" cellspacing="0" cellpadding="5">
					<tr class="dtHeader">
						<th></th>
						<th>Make</th>
						<th>Model</th>
                        <th>Version</th>                        
						<th>Ex-Showroom</th>
						<th><span style="position:relative;bottom:2px;">Insurance</span><input type="checkbox" id="chkInsurance" checked /></th>
						<th><span style="position:relative;bottom:2px;">RTO</span><input type="checkbox" id="chkRTO" checked/></th>						
                        <th>Last Updated Date</th>
						<th>Last Updated By</th>
					</tr>					
			</headertemplate>
			<itemtemplate>				
				<tr>
					<td>
						<asp:CheckBox ID="chkUpdate" runat="server" name="chkVersions"/>
					</td>
					<td><%# DataBinder.Eval( Container.DataItem, "MakeName" ) %></td>
                    <td>
						<%# DataBinder.Eval( Container.DataItem, "ModelName" ) %>
					</td>
					<td>
						<%# DataBinder.Eval( Container.DataItem, "VersionName" ) %>
					</td>						
					<td>
						<asp:TextBox ID="txtPrice" Index='<%# Container.ItemIndex %>' data-modeldid='<%# DataBinder.Eval(Container.DataItem, "BikeModelId") %>' data-seriesid ='<%# DataBinder.Eval(Container.DataItem, "BikeSeriesId") %>' VersionId='<%# DataBinder.Eval( Container.DataItem, "VersionId" ) %>' Text='<%# DataBinder.Eval( Container.DataItem, "Price" ).ToString() %>' onchange="calculatePrices(this)" Columns="10" MaxLength="9" runat="server" />							
					</td>
					<td>
						<asp:TextBox class="txtInsurance" ID="txtInsurance" Text='<%# DataBinder.Eval( Container.DataItem, "Insurance" ).ToString() %>' Columns="10" MaxLength="9" runat="server"/>						
					</td>
					<td>
						<asp:TextBox class="txtRTO" ID="txtRTO" Text='<%# DataBinder.Eval( Container.DataItem, "RTO" ).ToString() %>' Columns="10" MaxLength="9" runat="server"/>							
					</td>
                    <td class="text-align-center">
						<%# DataBinder.Eval( Container.DataItem, "LastUpdatedDate" ).ToString() %>
					</td>
                    <td class="text-align-center">
						<%# DataBinder.Eval( Container.DataItem, "LastUpdatedBy" ).ToString() %>
					</td>						
				</tr>
			</itemtemplate>
			<footertemplate>
				</table>
			</footertemplate>
		</asp:repeater>
    </div>
    <div class="margin-top10" style="position: fixed; right: 20px;">
        <fieldset class="floatLeft">
            <legend><b>Select Cities to Upload Prices</b></legend>
            <div class="margin-top10">
                Select Versions
                <a class="margin-left5" href="javascript:selectAll('all','chkUpdate')"><strong>All</strong></a>
                <a class="margin-left5" href="javascript:selectAll('none','chkUpdate')"><strong>None</strong></a>
            </div>
            <div class="margin-top10">
                <asp:dropdownlist id="ddlPriceStates" runat="server" />
            </div>
            <div class="margin-top10">
                <input type="checkbox" id="chkAllCities" /><label for="chkAllCities" class="pointer">Select All Cities</label></div>
            <div class="margin-top10">
                <asp:dropdownlist id="ddlPriceCities" multiple="true" runat="server" data-bind="selectedOptions: selectedCities"></asp:dropdownlist>
            </div>
            <asp:hiddenfield id="hdnSelectedCities" runat="server" />            
            <asp:button id="btnSavePrices" text="Save All Prices" runat="server" class="margin-top10" />
            <div id="errSavePrices" class="margin-top10 errorMessage"></div>
        </fieldset>
    </div>
    <div class="clear"></div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $("#ddlMakes").chosen({ width: "180px", no_results_text: "No matches found!!", search_contains: true });
        $("#ddlStates").chosen({ width: "180px", no_results_text: "No matches found!!", search_contains: true });
        $("#ddlCities").chosen({ width: "180px", no_results_text: "No matches found!!", search_contains: true });
        $("#ddlPriceStates").chosen({ width: "180px", no_results_text: "No matches found!!", search_contains: true });
        $("#ddlPriceCities").chosen({ placeholder_text_multiple: "Select Cities" });

        viewModel = { selectedCity: ko.observable($("#hdnSelectedCity").val()), selectedCities: ko.observableArray([]) };
        ko.applyBindings(viewModel, document.getElementById("divManagePrices"));
    });


    $("#ddlStates").change(function () {
        var ddlCities = $("#ddlCities");
        var requestType = "7";
        selectString = "--Select City--";
        var stateId = $(this).val();
        viewModel.selectedCity(0);
        if (stateId > 0) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"requestType":"' + requestType + '", "stateId":"' + stateId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetCities"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    var dependentCmbs = new Array();
                    bindDropDownList(resObj, ddlCities, "", "--Select City--");
                    ddlCities.trigger('chosen:updated');
                }
            });
        }
        else {
            ddlCities.empty().attr("disabled", "disabled").append("<option value=\"0\">" + selectString + "</option>").removeAttr("enabled");
            ddlCities.trigger('chosen:updated');
        }
    });

	$("#chkInsurance").click(function () {
		if ($(this).is(":checked")) {
			$(".txtInsurance").removeAttr("disabled");
		}
		else {
			$(".txtInsurance").attr("disabled", "disabled");
		}
	});

	$("#chkRTO").click(function () {
		if ($(this).is(":checked")) {
			$(".txtRTO").removeAttr("disabled");
		}
		else {
			$(".txtRTO").attr("disabled", "disabled");
		}
	});

    $("#ddlPriceStates").change(function () {
        var ddlPriceCities = $("#ddlPriceCities");
        var requestType = "7";
        var stateId = $(this).val();
        viewModel.selectedCities([]);

        if (stateId > 0) {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                data: '{"requestType":"' + requestType + '", "stateId":"' + stateId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetCities"); },
                success: function (response) {
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    var dependentCmbs = new Array();
                    bindDropDownList(resObj, ddlPriceCities, "", "--Select City--");
                    ddlPriceCities.find("option[value='0']").remove();
                    ddlPriceCities.trigger('chosen:updated');

                }
            });
        }
        else {
            ddlPriceCities.empty().attr("disabled", "disabled").removeAttr("enabled");
            ddlPriceCities.trigger('chosen:updated');
        }
    });

    function selectAll(type, chkId) {
        var obj = document.getElementsByTagName("input");

        if (type == "all") {
            bolVal = true;
        }
        else {
            bolVal = false;
        }
        for (var i = 0 ; i < obj.length ; i++) {
            if (obj[i].type == "checkbox" && obj[i].id.indexOf(chkId) != -1) {
                obj[i].checked = bolVal;
            }
        }
    }

    function calculatePrices(e) {
        var objTxtInsurance = $(e);

        var versionId = objTxtInsurance.attr("versionId");
        var itemIndex = objTxtInsurance.attr("index");
        var price = objTxtInsurance.val();        

	 if (price != "") {
	     if (!$("#chkInsurance").is(":checked")) {
		    calculateInsurancePremium(versionId, price, $("#rptPrices_txtInsurance_" + itemIndex));
	     }
	     if (!$("#chkRTO").is(":checked")) {
		    calculateRegistrationCharges(versionId, price, $("#rptPrices_txtRTO_" + itemIndex));
	     }
	 }
    }

    function calculateInsurancePremium(versionId, price, objTxtIns) {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/BikeWaleOpr.AjaxFunctions,BikewaleOpr.ashx",
            data: '{"bikeVersionId":"' + versionId + '", "cityId":"' + viewModel.selectedCity() + '", "price":' + price + '}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "CalculateInsurancePremium"); },
            success: function (response) {
                var responseJSON = eval('(' + response + ')');
                var resObj = eval('(' + responseJSON.value + ')');
                objTxtIns.val(Math.round(resObj));
            },
	    complete : function(xhr)
            {
               if(xhr.status==200)
               (objTxtIns.is(":focus")) ? objTxtIns.select() :false;
            }
        });
    }

    function calculateRegistrationCharges(versionId, price, objTxtRTO) {
        var url = "/api/price/getregistrationcharges?versionId=" + versionId + "&stateId=" + $('#ddlStates').val() + "&price=" + price;
        $.get(url, function (response) {
            objTxtRTO.val(Math.round(response));
        });
        //$.ajax({
        //    type: "POST",
        //    url: "/ajaxpro/BikeWaleOpr.AjaxFunctions,BikewaleOpr.ashx",
        //    data: '{"bikeVersionId":"' + versionId + '", "cityId":"' + viewModel.selectedCity() + '", "price":' + price + '}',
        //    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "CalculateRegistrationCharges"); },
        //    success: function (response) {
        //        var responseJSON = eval('(' + response + ')');
        //        var resObj = eval('(' + responseJSON.value + ')');
        //        objTxtRTO.val(Math.round(resObj));
        //    }
        //});
    }

    $("#btnSearch").click(function () {
        $("#hdnSelectedCity").val(viewModel.selectedCity());
        
        if (!validateShowPrices())
            return false;
    });

    function validateShowPrices() {
        var isValid = true;
        var ddlMakes = $("#ddlMakes");
        var ddlCities = $("#ddlCities");
        showHideMatchError(ddlMakes, false);
        showHideMatchError(ddlCities, false);

        if (ddlMakes.val() <= 0) {
            isValid = false;
            showHideMatchError(ddlMakes, true);
        }

        if (viewModel.selectedCity() <= 0)
        {
            isValid = false;
            showHideMatchError(ddlCities, true);
        }            

        return isValid;
    }

    $("#chkAllCities").click(function () {
        viewModel.selectedCities([]);
        $("#ddlPriceCities").trigger('chosen:updated');
        if ($(this).is(":checked")) {
            $("#ddlPriceCities option").each(function () { $(this).prop('selected', true); viewModel.selectedCities.push($(this).val()); });
        }
    });

	$("#btnSavePrices").click(function () {
        $("#hdnSelectedCity").val(viewModel.selectedCity());
        $("#hdnSelectedCities").val(viewModel.selectedCities());
		
		if (!validateUploadPrices())
			return false;
		else
		{
			$(".txtRTO").removeAttr("disabled");
			$(".txtInsurance").removeAttr("disabled");
		}
    });

    function validateUploadPrices() {
        var isValid = true;
        var errSaveMsg = "Plese select";
        var errCityMsg = "";
        var errSavePrices = $("#errSavePrices");
        errSavePrices.text("");

        if (viewModel.selectedCities().length <= 0) {
            isValid = false;
            errCityMsg = " cities ";
            errSaveMsg += errCityMsg;
        }

        if ($("span[name='chkVersions'] :checkbox:checked").length <= 0) {
            isValid = false;
            if (errCityMsg != "") { errSaveMsg += " and"; }
            errSaveMsg += " versions "
        }

        if (!isValid) {
            errSaveMsg += " to upload prices";
            errSavePrices.text(errSaveMsg);
        }

        return isValid;
    }

</script>
<!-- #Include file="/includes/footerNew.aspx" -->
