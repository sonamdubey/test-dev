<%@ Page trace="false" Inherits="BikeWaleOpr.Content.BulkPriceUpload" AutoEventWireUp="false" Language="C#" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<style type="text/css">
	.divCont {padding:3px;}
	.divCont h2{background-color:#F0F0F0; font-size:16px; padding:5px; }
	.tblStock { border-collapse:collapse; border:1px solid #BDBDBD; font-family:Arial, Helvetica, sans-serif; font-size:12px;}
	.tblStock th{background:#eeeeee; }
</style>
<div class="left">
    <h1>Bulk Price Upload</h1>
    <fieldset>
        <legend>Bulk Price Upload</legend>
        <div>
            <span id="spnErr" runat="server"></span>s
            <div id="divStep1" runat="server" class="divCont">
	            <h2>Step 1 : Upload the XML file</h2><br />
	            Select Make : <asp:DropDownList ID="drpMakes" runat="server" />
	            <span id="spnMakes" class="errorMessage"></span>
	            <br /><br />
	
	            Upload only XML(.xml) file : 
	            <input type="file" id="flUpload" runat="server" />
	            <asp:Button ID="btnUploadFile" runat="server" Text="Upload File"></asp:Button>
	            <p>&nbsp;</p>
	            <p>&nbsp;</p>
	            <p>&nbsp;</p>
	            <p>&nbsp;</p>
	            <p>&nbsp;</p>
	            <p>&nbsp;</p>
            </div>
            <div id="divStep2" runat="server" class="divCont">
	            <h2>Step 2 : Process the file</h2><br />
	            <asp:Button ID="btnProcessFile" runat="server" Text="Process File"></asp:Button>
	            <p>&nbsp;</p>
	            <p>&nbsp;</p>
	            <p>&nbsp;</p>
	            <p>&nbsp;</p>
	            <p>&nbsp;</p>
	            <p>&nbsp;</p>
            </div>
            <div id="divStep3" runat="server" class="divCont">
	            <h2><%= updatedPrice%> Prices Can Be Added</h2><br />
	            <div style="font-size:18px;" id="divMsgUpdate">Price updation is in progress... <img src="https://opr.carwale.com/images/icons/loading.gif"  /></div>
	            <iframe src="/content/AddPricesPerFile.aspx" id="ifrAddPrices" width="600" height="10" scrolling="no" frameborder="0"></iframe>
	            <br />
	            <h2>Unmapped Bikes-Map the bikes</h2> <br />
	            <asp:repeater ID="rptUCV" runat="server">
		            <headertemplate>
			            <table width="100%" border="1" cellpadding="3" class="tblStock"> 
				            <tr>
					            <th colspan="2" align="left">Bike Name</th>
				            </tr>
		            </headertemplate>	
		            <itemtemplate>
				            <tr id="trUC<%# ++ucvSerial%>">
					            <td width="200">
						            <span class="spnBikeName" id="spnBikeName<%# ucvSerial%>"><%# DataBinder.Eval(Container.DataItem, "BikeName") %></span>
					            </td>
					            <td>
						            <select id="drpModel" class="drpModel" runat="server" DataTextField="Text" DataValueField="Value" 
								            DataSource='<%# dtModels %>' onchange='drpModelChange(this)' />
						            <select id="drpVersion" class="drpVersion" runat="server" disabled="disabled">
							            <option value="0">Select Version</option>
						            </select>
						            <input type="button" value="Map this bike" id="butMapBike<%# ucvSerial%>" onclick='mapBike(<%# ucvSerial%>)' />
						            <input type="button" value="Update Mapping" style="display:none" id="butUMapBike<%# ucvSerial%>" onclick='mapUBike(<%# ucvSerial%>)' />
					            </td>
				            </tr>
		            </itemtemplate>
		            <footertemplate>
			            </table>
		            </footertemplate>
	            </asp:repeater>
	            <input type="hidden" id="hdnVersions" />
	            <br />
	            <h2>Unmapped Cities-Map the cities</h2><br />
	            <asp:repeater ID="rptUC" runat="server">
		            <headertemplate>
			            <table width="100%" border="1" cellpadding="3" class="tblStock"> 
				            <tr>
					            <th colspan="2" align="left">City Name</th>
				            </tr>
		            </headertemplate>	
		            <itemtemplate>
				            <tr id="trUC<%# ++ucSerial%>">
					            <td width="200">
						            <span class="spnCityName" id="spnCityName<%# ucSerial%>"><%# DataBinder.Eval(Container.DataItem, "CityName") %></span>
					            </td>
					            <td>
						            <select id="drpState" runat="server" DataTextField="Text" DataValueField="Value" 
								            DataSource='<%# dtStates %>' onchange='drpStateChange(this)' />
						            <select id="drpCity" runat="server" disabled="true" class="drpCity">
							            <option value="0">Select City</option>
						            </select>
						            <input type="button" value="Map this city" id="butMapCity<%# ucSerial%>" onclick='mapCity(<%# ucSerial%>)' />
						            <input type="button" value="Update Mapping" style="display:none" id="butUMapCity<%# ucSerial%>" onclick='mapUCity(<%# ucSerial%>)' />
					            </td>
				            </tr>
		            </itemtemplate>
		            <footertemplate>
			            </table>
		            </footertemplate>
	            </asp:repeater><br />
	            <asp:Button ID="butMapUCBikes" runat="server" Enabled="false" Text="Map Unmapped Bikes" />
	            <input type="button" value="Make New Entry" id="butNewEntry" disabled="disabled" onclick="javascript:location.href=location.href" />
	            <input type="hidden" id="hdnCities" />
	
            </div>
        </div>
    </fieldset>
</div>

<script language="javascript">
	if(document.getElementById("btnUploadFile"))
		document.getElementById("btnUploadFile").onclick = form_Submit;

	function drpModelChange(e)
	{
		var modelId = e.id;
		var modelVal = e.value;
		var versionId = modelId.replace("drpModel", "drpVersion");
		var versions = AjaxFunctions.GetVersions(modelVal);
		//call the function to consume this data
		FillCombo_Callback(versions, document.getElementById(versionId), "hdnVersions","","Select Version");
	}
	
	function drpStateChange(e)
	{
		var stateId = e.id;
		var stateVal = e.value;
		var cityId = stateId.replace("drpState", "drpCity");
		
		var cities = AjaxFunctions.GetCities(stateVal);
	
		//call the function to consume this data
		FillCombo_Callback(cities, document.getElementById(cityId), "hdnCities","","Select City");
	}
	
	function mapBike(index)
	{	    
	    var idInd = index < 10 ? "0" + index : index;
	    var obj = $("#trUC" + index);	    
			
	    var bikeName = obj.find(".spnBikeName").html(); //document.getElementById("spnBikeName" + index).innerHTML;		    
	    var bikeId = obj.find(".drpVersion").val(); //document.getElementById("rptUCV_ctl" + idInd + "_drpVersion").value;	    
	    var cwBikeName = '<%= MakeName %>' + " " + 
						obj.find(".drpModel option:selected").text() +
	                    obj.find(".drpVersion option:selected").text();
                        //document.getElementById("rptUCV_ctl" + idInd + "_drpModel").options[document.getElementById("rptUCV_ctl" + idInd + "_drpModel").selectedIndex].text + " " +
						//document.getElementById("rptUCV_ctl" + idInd + "_drpVersion").options[document.getElementById("rptUCV_ctl" + idInd + "_drpVersion").selectedIndex].text;
        	    
		if(bikeName != "" && bikeId != "0")
		{
			AjaxFunctions.MapOemBikes(bikeName, bikeId, cwBikeName);
			document.getElementById("butMapBike" + index).disabled = true;
			document.getElementById("butMapBike" + index).value = "Mapped Successfully!";
			document.getElementById("butUMapBike" + index).style.display = "";
		}
	}
	
	function mapUBike(index)
	{
	    var idInd = index < 10 ? "0" + index : index;
	    var obj = $("#trUC" + index);
		
	    var bikeName = obj.find(".spnBikeName").html(); //document.getElementById("spnBikeName" + index).innerHTML;	
	    var bikeId = obj.find(".drpVersion").val(); //document.getElementById("rptUCV_ctl" + idInd + "_drpVersion").value;
	    var cwBikeName = '<%= MakeName %>' + " " + 
						obj.find(".drpModel option:selected").text() +
	                    obj.find(".drpVersion option:selected").text();
						//document.getElementById("rptUCV_ctl" + idInd + "_drpModel").options[document.getElementById("rptUCV_ctl" + idInd + "_drpModel").selectedIndex].text + " " +
						//document.getElementById("rptUCV_ctl" + idInd + "_drpVersion").options[document.getElementById("rptUCV_ctl" + idInd + "_drpVersion").selectedIndex].text;
						
		if(bikeName != "" && bikeId != "0")
		{
			AjaxFunctions.MapUOemBikes(bikeName, bikeId, cwBikeName);
			document.getElementById("butMapBike" + index).disabled = true;
			document.getElementById("butMapBike" + index).value = "Re-Mapped Successfully!";
			document.getElementById("butUMapBike" + index).style.display = "";
		}
	}
	
	function mapCity(index)
	{
		var idInd = index < 10 ? "0" + index : index;
		var obj = $("#trUC" + index);

		var cityName = obj.find(".spnCityName").html(); //document.getElementById("spnCityName" + index).innerHTML;	
		var cityId = obj.find(".drpCity").val(); //document.getElementById("rptUC_ctl" + idInd + "_drpCity").value;
		var cwCityName = obj.find(".drpCity option:selected").text(); //document.getElementById("rptUC_ctl" + idInd + "_drpCity").options[document.getElementById("rptUC_ctl" + idInd + "_drpCity").selectedIndex].text;
		
		alert(cityName + " : "  + cityId + " : " + cwCityName);

		if(cityName != "" && cityId != "0")
		{
			AjaxFunctions.MapOemCities(cityName, cityId, cwCityName);
			document.getElementById("butMapCity" + index).disabled = true;
			document.getElementById("butMapCity" + index).value = "Mapped Successfully!";
			document.getElementById("butUMapCity" + index).style.display = "";
		}
	}
	
	function mapUCity(index)
	{
	    var idInd = index < 10 ? "0" + index : index;
	    var obj = $("#trUC" + index);
		
	    var cityName = obj.find(".spnCityName").html(); //document.getElementById("spnCityName" + index).innerHTML;	
	    var cityId = obj.find(".drpCity").val(); //document.getElementById("rptUC_ctl" + idInd + "_drpCity").value;
	    var cwCityName = obj.find(".drpCity option:selected").text(); //document.getElementById("rptUC_ctl" + idInd + "_drpCity").options[document.getElementById("rptUC_ctl" + idInd + "_drpCity").selectedIndex].text;
		if(cityName != "" && cityId != "0")
		{
			AjaxFunctions.MapUOemCities(cityName, cityId, cwCityName);
			document.getElementById("butMapCity" + index).disabled = true;
			document.getElementById("butMapCity" + index).value = "Re-Mapped Successfully!";
			document.getElementById("butUMapCity" + index).style.display = "";
		}
	}
	
	function form_Submit(e)
	{
		var isError = false;
		
		if ( document.getElementById('drpMakes').options[0].selected ) 
		{
		    //document.getElementById('spnMakes').innerHTML = "Please select make.";
		    $("#spnMakes").html("Please select make.");
			isError = true;
		}
		else
		{
		    //document.getElementById('spnMakes').innerHTML = "";
		    $("#spnMakes").html("");
		}
		
		if ( isError == true ) return false;
	}
</script>

<!-- #Include file="/includes/footerNew.aspx" -->