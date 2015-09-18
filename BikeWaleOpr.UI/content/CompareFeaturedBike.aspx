<%@Page Inherits="BikeWaleOpr.Content.CompareFeaturedBike" Language="C#" AutoEventWireUp="false" Trace="false" Debug="false" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<script language="javascript" src="http://opr.carwale.com/src/common/bt.js"></script>
<!--[if IE]><script language="javascript" src="http://opr.carwale.com/src/common/excanvas.js"></script><![endif]-->
<script language="javascript" src="/src/AjaxFunctions.js"></script>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {

        $("#ddlMakeFeaturedBike").change(function () {
            ddlMake_ChangeFeaturedBike(this);
        });
        $("#ddlModelFeaturedBike").change(function () {
            ddlModel_ChangeFeaturedBike(this);
        });

        $("#divFeaturedBikes").load("FeaturedBikes.aspx");
        $("#hdnBikesAdded").val("");

        selectedFeaturedMake = "";
        selectedFeaturedModel = "";
        selectedFeaturedVersion = "";
    });	
</script>
<div class="urh">
	You are here &raquo; <a href="/content/default.aspx">Contents</a> &raquo; Add Compare Featured Bike
</div>

<!-- #Include file="ContentsMenu.aspx" -->
<div class="left">
	
<h3 style="margin-bottom:14px;">Add Compare Featured Bike</h3>
<div style="float:left;">
	<div>
		<table cellpadding="5" cellspacing="0" style="border:1px solid;border-collapse:collapse;width:450px;" >
			<tr>
				<td style="background-color:#CCCCCC;width:100%;font-size:12px;font-weight:bold;" colspan="2">Select Featured Bike</td>
			</tr>
			<tr>
				<td style="width:75px;">Make</td>
				<td style="width:225px;">
					<asp:DropDownList ID="ddlMakeFeaturedBike" runat="server" />
				</td>
			</tr>
			<tr>
				<td style="width:75px;">Model</td>
				<td style="width:225px;">
					<asp:DropDownList ID="ddlModelFeaturedBike" runat="server" />
					<input type="hidden" id="hdn_drpModelsFeaturedBike" runat="server" />
				</td>
			</tr>
			<tr>
				<td style="width:75px;">Version</td>
				<td style="width:225px;">
                    <div onclick="displayDiv();" id="divMainControl" runat="server">
                        <asp:textbox id="tbxText" runat="server"  style="position:relative;bottom:5px;" onkeydown="javascript:return RestrictTyping(event);"></asp:textbox>
                        <img id="imgarrow" src="/../images/drop-arrow.png" alt="" style="position:relative;left:-7px;height:20px;" />
                    </div>
                    <div style="position:relative;bottom:5px;">
                    <div id="divMain" style="width:220px;height:200px;border:1px solid black; display:none;background-color:#FFFFFF;position:absolute;z-index:100px;">
                        <div>
                            <div style="height:179px;overflow:auto;">
                                <ul id="ulMultiVersions" style="padding-bottom:5px;" />
                            </div>
                            <div style="height:20px;border-top:1px solid;">
                                <a href="javascript:checkAll(true);"><strong>All</strong></a> &nbsp;
                                <a href="javascript:checkAll(false);"><strong>None</strong></a> &nbsp;
                                <a href="javascript:IsDone();"><strong>Done</strong></a>
                                <span id="spnVersions" style=" color:Red; font-size:11px; "></span>
                            </div>
                            <asp:textbox id="txtVersionsHidden" runat="server" class="hidecontent"></asp:textbox>
                        </div>
                    </div>
                    </div>
				</td>
			</tr>
		</table>
	</div>
	<div id="divComparisonBikes" style="width:450px;"></div>
</div>
<div style="float:left;margin-left:14px;">
	<div id="divFeaturedBikes"></div>
</div>
<div style="clear:both;"></div>
</div>
<div id="bidPopUp" style="display:none;">	
	<span id="closeBox" title="Close the box" style="float:right;text-decoration:underline;cursor:pointer;">Close</span>
	<div id="bidMsg" class="msg-box" style="display:none; clear:both; width:350px;">You don’t have enough token(s) to bid for this Bike. Please request more tokens or contact us!<p style="margin-top:5px;"><a href="#"><span onclick="getMoreTokens()"><strong>Request More Tokens</strong></span></a></p></div>
	<table border="0" cellpadding="5" cellspacing="0">
		<tr>
			<td>Make</td>
			<td><asp:DropDownList ID="ddlMake" runat="server" ></asp:DropDownList></td>
		</tr>
		<tr>
			<td>Model</td>
			<td>
				<asp:DropDownList ID="ddlModel" runat="server" ></asp:DropDownList>
				<input type="hidden" id="hdn_drpModels" runat="server" />
			</td>
		</tr>
		<tr>
			<td>Version</td>
			<td>
				<asp:DropDownList ID="ddlVersion" runat="server" Multiple="Multiple" style=" width:125px; height:100px; " ></asp:DropDownList>
				<input type="hidden" id="hdn_drpVersions" runat="server" />
                <br />(Use 'Ctrl' key for multiple selection)
			</td>
		</tr>
        <tr class="details">
			<td>Spotlight Url</td>
			<td>
				<asp:textbox runat="server" id="txtUrl"></asp:textbox> 
            </td>
		</tr>
        <tr class="details">
			<td></td>
			<td>
                <asp:CheckBox ID="chkCompare" runat="server" text="IsCompare" ></asp:CheckBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:CheckBox ID="chkNSearch" runat="server" text="IsNewSearch" ></asp:CheckBox>
            </td>
		</tr>
        <tr class="details">
			<td></td>
			<td>
                <asp:CheckBox ID="chkRecommend" runat="server" text="IsRecommend" ></asp:CheckBox>&nbsp;&nbsp;
				<asp:CheckBox ID="chkResearch" runat="server" text="IsResearch" ></asp:CheckBox>
			</td>
		</tr>
		<tr>
			<td>&nbsp;</td>
			<td>
				<input id="btnAdd" type="button" value="Add" />
                <input id="btnCopy" type="button" value="Copy" />
				<span id="spnError" style="display:none;color:red;font-size:11px;">Kindly Select Version</span>
                <span id="spnUrl" style="display:none;color:red;font-size:11px;">Kindly Enter Valid Url.</span>
			</td>
		</tr>
	</table>
</div>

<script language="javascript" type="text/javascript">

	function ddlMake_Change(ddl){
		ddl_Make = ddl;
		AjaxFunctions.GetModels(ddl.value, modelCallback);
	}
	
	function modelCallback(response){
		var table = ddl_Make.parentNode.parentNode.parentNode;
		var ddls = table.getElementsByTagName("select");
		dependentCmbs = new Array();
		dependentCmbs[0] = "";
		FillCombo_Callback(response, ddls[1], "hdn_drpModels" , dependentCmbs);
		ddls[2].innerHTML = "<option value='-1'>--Select--</option>";
	}
	
	function ddlMake_ChangeFeaturedBike(ddl){
		$("#divComparisonBikes").hide();
		AjaxFunctions.GetModels(ddl.value, modelCallbackFeaturedBike);
	}
	
	function modelCallbackFeaturedBike(response){
		dependentCmbs2 = new Array();
		dependentCmbs2[0] = "";
		FillCombo_Callback(response, document.getElementById("ddlModelFeaturedBike"), "hdn_drpModelsFeaturedBike" , dependentCmbs2);
		
		if (selectedFeaturedModel != ""){ 
			$("#ddlModelFeaturedBike").val(selectedFeaturedModel);
			ddlModel_ChangeFeaturedBike(document.getElementById("ddlModelFeaturedBike"));
		}	
	}
	
	function ddlModel_Change(ddl){
		ddl_Model = ddl;
		AjaxFunctions.GetNewVersions(ddl.value, versionCallback);
	}	
	
	function versionCallback(response){
		var table = ddl_Model.parentNode.parentNode.parentNode;
		var ddls = table.getElementsByTagName("select");
		dependentCmbs1 = new Array();
		dependentCmbs1[0] = "";
		FillCombo_Callback(response, ddls[2], "hdn_drpVersions" , dependentCmbs1);
	}

	function AddBike(btn) {
        
		var table = btn.parentNode.parentNode.parentNode;
		var ddls = table.getElementsByTagName("select");
		var txt = $(table).find("input[type='text']");
		var chks = $(table).find("input[type='checkbox']");
		var _bikesAdded = $("#hdnBikesAdded").val();

		if (_bikesAdded != "") {
		    _bikesAdded = _bikesAdded.substring(0, _bikesAdded.length - 1);
		    _bikesAdded = _bikesAdded.substring(1, _bikesAdded.length);
		}

		if (_bikesAdded != "" && $("#hdnBikesAlreadyAdded").val() != "")
		    _bikesAdded = _bikesAdded + "," + $("#hdnBikesAlreadyAdded").val();
		else if (_bikesAdded == "" && $("#hdnBikesAlreadyAdded").val() != "")
		    _bikesAdded = $("#hdnBikesAlreadyAdded").val();

        //vaibhav k (12-07-2012)
        //Get version Ids as comma separated from multivalued dropdown
        var versions = $(ddls[2]).val().toString().split(',');
        
        //Get selected version names
        var allVers = "";
        var firstValue = true;
        $(ddls[2]).find(":selected").each(function(){
            if(firstValue)
            {
                allVers += $(this).text();
                firstValue = false;
            } else {
                allVers += "," + $(this).text();
            }
            
        });
        //Split the version names as per comma and store in array
        var versionNames = allVers.toString().split(',');

        for(var i=0; i < versions.length; i++)
        {
            //alert("start i " + i + " "  + versions[i] + "-" + versionNames[i]);
            if(versions[i] > 0)
            {
                if (_bikesAdded != "") {
		            var _bikesAddedArray = _bikesAdded.split(',');
		            var bikeAlreadyPresent = false;

		            for (var j = 0; j < _bikesAddedArray.length; j++) {
		                if (versions[i] == _bikesAddedArray[j])
		                    bikeAlreadyPresent = true;
		            }
                    //Check for whether the bike is already added or not
                    //if added go to next iteration
		            if (bikeAlreadyPresent) {
		                alert("You have already " + ddls[0].options[ddls[0].selectedIndex].text + ' ' + ddls[1].options[ddls[1].selectedIndex].text + " " + versionNames[i] + " in current list");
		                continue;
		            }
		        }

                //Actual addition of the bike if it not added
                var row;
                var cell;
                var spnRemove;
                var spnData;

                row = $(document.createElement("tr"));
                row.attr("id", "tr" + versions[i].toString());

                cell = $(document.createElement("td"));
                cell.attr({
	                style: "border-bottom:1px solid #CCCCCC;width:375px;background-color:#FFC6FF;"
                });B

                cell.html(ddls[0].options[ddls[0].selectedIndex].text + ' ' + ddls[1].options[ddls[1].selectedIndex].text + ' ' + versionNames[i]);
                cell.appendTo(row);

                cell = $(document.createElement("td"));
                cell.attr({
	                style: "border-bottom:1px solid #CCCCCC;width:125px;background-color:#FFC6FF;"
                });

                spnRemove = $(document.createElement("span"));
                spnRemove.html("Remove");
                spnRemove.attr({
	                style: "cursor:pointer;text-decoration:underline;color:blue;"
                });
                $(spnRemove).bind("click", function (e) { RemoveBike(this, versions[i]); });

                spnRemove.appendTo(cell);
                cell.appendTo(row);

                cell = $(document.createElement("td"));
                spnData = $(document.createElement("span"));
                spnData.html($(txt).val());
                spnData.appendTo(cell);
                spnData = $(document.createElement("span"));
                spnData.html(GetCheckBoxVal(chks[0]));
                spnData.appendTo(cell);
                spnData = $(document.createElement("span"));
                spnData.html(GetCheckBoxVal(chks[1]));
                spnData.appendTo(cell);
                spnData = $(document.createElement("span"));
                spnData.html(GetCheckBoxVal(chks[2]));
                spnData.appendTo(cell);
                spnData = $(document.createElement("span"));
                spnData.html(GetCheckBoxVal(chks[3]));
                spnData.appendTo(cell);
                cell.hide();
                cell.appendTo(row);

                row.prependTo("#tblBikesAdded");

                if ($("#hdnBikesAdded").val() == "") {
	                $("#hdnBikesAdded").val("*" + versions[i] + "*");
                }
                else {
	                $("#hdnBikesAdded").val($("#hdnBikesAdded").val().substring(0, $("#hdnBikesAdded").val().length - 1))
	                $("#hdnBikesAdded").val($("#hdnBikesAdded").val() + "," + versions[i] + "*");
                }
            }
        }
	}

	//Modified :  Vaibhav K (2-May-2012) 
    //Summary  :  Copy comparison Bikes from selected version into new featured version
    //Modified2:  Vaibhav K (29-May-2012)
    //         :  Copy into multiple versions at single time 
	function CopyBike(btn) {
		var table = btn.parentNode.parentNode.parentNode;
		var ddls = table.getElementsByTagName("select");
		var txt = $(table).find("input[type='text']");
		var chks = $(table).find("input[type='checkbox']");
		var _bikesAdded = $("#hdnBikesAdded").val();
		//to show the Bike name for which data is shown
        var versionIds = $("#txtVersionsHidden").val().split(",");
        var versionNames = $("#tbxText").val().split(",");
        var selectedFeaturedBike = ''; 
        for(var j = 0; j < versionIds.length; j++)
        {
		    selectedFeaturedBike = selectedFeaturedBike + document.getElementById("ddlMakeFeaturedBike").options[document.getElementById("ddlMakeFeaturedBike").selectedIndex].text;
		    selectedFeaturedBike = selectedFeaturedBike + " " + document.getElementById("ddlModelFeaturedBike").options[document.getElementById("ddlModelFeaturedBike").selectedIndex].text;
		    selectedFeaturedBike = selectedFeaturedBike + " " + versionNames[j];
		    arrSelectedFeaturedBike = selectedFeaturedBike.split(" ");
		
		    for (var i=0; i<arrSelectedFeaturedBike.length; i++){
			    selectedFeaturedBike = selectedFeaturedBike.replace(" ","_");
		    }
            if(j != versionIds.length-1) {
                selectedFeaturedBike = selectedFeaturedBike + ",";
            }
        }

		var copyBike = ddls[2].value + "|" + txt[0].value + "|" + GetCheckBoxVal(chks[0]) + "|" + GetCheckBoxVal(chks[1]) + "|" + GetCheckBoxVal(chks[2]) + "|" + GetCheckBoxVal(chks[3]);
		////change ddlVersionFeaturedBike to new chkbx values $("#txtVersionsHidden").val()
        var featuredBike = $("#txtVersionsHidden").val().split(",");
        var reply;
        for(var i=0; i < featuredBike.length; i++)
        {
            //alert(featuredBike[i]);
		    $.ajax({
		        type: "POST",
		        url: "/ajaxpro/BikeWaleOpr.Common.AjaxCompareFeaturedBike,BikeWaleOpr.Common.ashx",
		        data: '{"copyBike":"' + copyBike + '", "featuredBike":"' + featuredBike[i] + '"}',
		        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "CopyComparedBikes"); },
		        success: function (response) {
		            //alert(response);
		            var status = eval('(' + response + ')');
		            if (status.value == true) {
		                reply = true;
		            }
		            else if (status.value == false) {
		                reply = false;
		            }
		        }
		    });
        }
        $("#divComparisonBikes").load("ComparisonBikes.aspx?featureBikeId=" + featuredBike + "&featureBike=" + selectedFeaturedBike);
		$("#divComparisonBikes").show();
		//location.href = "/contents/CompareFeaturedBike.aspx";
    }

    function GetCheckBoxVal(_chk) {
        if ($(_chk).is(":checked").toString() == "true")
            return "1";
        else
            return "0";
    }
	
	function RemoveBike(spnRemove, versionToRemove){
		//alert($("#hdnBikesAdded").val());
		//res = confirm("Are you sure want to remove the Bike?")
		res = true;
		if (res){
			var bikesAdded = $("#hdnBikesAdded").val();
			if (bikesAdded.lastIndexOf("," + versionToRemove + ",") != -1){
				var bikesRemaining = bikesAdded.split("," + versionToRemove + ",");
				bikesAdded = bikesRemaining[0] + "," + bikesRemaining[1];
			}
			else if (bikesAdded.lastIndexOf("*" + versionToRemove + ",") != -1){
				var bikesRemaining = bikesAdded.split("*" + versionToRemove + ",");
				bikesAdded = "*" + bikesRemaining[1];
			}
			else if (bikesAdded.lastIndexOf("," + versionToRemove + "*") != -1){
				var bikesRemaining = bikesAdded.split("," + versionToRemove + "*");
				bikesAdded = bikesRemaining[0] + "*";
			}
			else{
				bikesAdded = "";
			}
			
			$("#hdnBikesAdded").val(bikesAdded);
			$(spnRemove).parent().parent().remove();
			//alert($("#hdnCarsAdded").val());
		}	
	}
	
	function DeletePreviouslySavedBike(spnDelete, comparisonBikeToDelete){
		resDelete = confirm("You are about to delete saved comaprison Bike from database.\n\nAre you sure want to proceed ?")
		var featuredBikeToDelete = $("#hdnSelectedFeaturedBike").val().split(",");
		if (resDelete){
            for(var i = 0; i < featuredBikeToDelete.length; i++)
            {
                //alert(featuredBikeToDelete[i]);
			    $.ajax({
				    type: "POST",
				    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCompareFeaturedBike,BikeWaleOpr.Common.ashx",			
				    data: '{"comparisonBike":"'+ comparisonBikeToDelete +'", "featuredBike":"'+ featuredBikeToDelete[i] +'"}',	
				    beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "DeleteCompareFeaturedBike"); },
				    success: function(response) {
					    var status = eval('('+ response +')');												
					    if(status.value == true){
					
						    var bikesAlreadyAdded = $("#hdnBikesAlreadyAdded").val();
						    bikesAlreadyAdded = "*" + bikesAlreadyAdded + "*";
						
						    if (bikesAlreadyAdded.lastIndexOf("," + comparisonBikeToDelete + ",") != -1){
							    var bikesRemaining = bikesAlreadyAdded.split("," + comparisonBikeToDelete + ",");
							    bikesAlreadyAdded = bikesRemaining[0] + "," + bikesRemaining[1];
						    }
						    else if (bikesAlreadyAdded.lastIndexOf("*" + comparisonBikeToDelete + ",") != -1){
							    var bikesRemaining = bikesAlreadyAdded.split("*" + comparisonBikeToDelete + ",");
							    bikesAlreadyAdded = "*" + bikesRemaining[1];
						    }
						    else if (bikesAlreadyAdded.lastIndexOf("," + comparisonBikeToDelete + "*") != -1){
							    var bikesRemaining = bikesAlreadyAdded.split("," + comparisonBikeToDelete + "*");
							    bikesAlreadyAdded = bikesRemaining[0] + "*";
						    }
						    else{
							    bikesAlreadyAdded = "";
						    }
						
						    if (bikesAlreadyAdded != ""){
							    bikesAlreadyAdded = bikesAlreadyAdded.substring(0, bikesAlreadyAdded.length - 1);
							    bikesAlreadyAdded = bikesAlreadyAdded.substring(1, bikesAlreadyAdded.length);
						    }
						
						    $("#hdnBikesAlreadyAdded").val(bikesAlreadyAdded);

						    $(spnDelete).parent().parent().remove();
					    }								
				    }
			    });
            }
		}
	}
	
	function SaveCompareFeaturedBike(){
	    if (ValidData()){
	        var _compBikes = "";
			var comparisonBikes = $("#hdnBikesAdded").val();
			comparisonBikes = comparisonBikes.substring(0, comparisonBikes.length - 1);
			comparisonBikes = comparisonBikes.substring(1, comparisonBikes.length);

			//alert(comparisonBikes);
			_splitComparisonBikes = comparisonBikes.split(',');
			for (var i = 0; i < _splitComparisonBikes.length; i++) {
			    var _spans = $("#tr" + _splitComparisonBikes[i].toString() + " td:nth-child(3) span");
			    if (_compBikes == "")
			        _compBikes = _splitComparisonBikes[i].toString() + "|" + $(_spans[0]).html() + "|" + $(_spans[1]).html() + "|" + $(_spans[2]).html() + "|" + $(_spans[3]).html() + "|" + $(_spans[4]).html();
			    else
			        _compBikes = _compBikes + "," + _splitComparisonBikes[i].toString() + "|" + $(_spans[0]).html() + "|" + $(_spans[1]).html() + "|" + $(_spans[2]).html() + "|" + $(_spans[3]).html() + "|" + $(_spans[4]).html();
			}
            
            ////change ddlVersionFeaturedBike to new chkbx $("#txtVersionsHidden").val()
			var featuredBike = $("#txtVersionsHidden").val().split(",");
            for(var i=0; i < featuredBike.length; i++) {
			    $.ajax({
				    type: "POST",
				    url: "/ajaxpro/BikeWaleOpr.Common.AjaxCompareFeaturedBike,BikeWaleOpr.Common.ashx",			
				    data: '{"comparisonBikes":"'+ _compBikes +'", "featuredBike":"'+ featuredBike[i] +'"}',	
				    beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SaveCompareFeaturedBike"); },
				    success: function(response) {
					    //alert(response);
					    var status = eval('('+ response +')');												
					    if(status.value == true){
						    alert("Data saved successfully");
					    }								
				    }
			    });
                location.href = "/content/CompareFeaturedBike.aspx";
            }
		}
	}
	
	function ValidData(){
		var isValidData = true;
		
		if ($("#hdnBikesAdded").val() == ""){
			alert("Please add the Bikes for comparison");
			isValidData = false;
		}
		//change ddlVersionFeaturedBike to new chkbx $("#txtVersionsHidden").val()
		if ($("#txtVersionsHidden").val() == "" || $("#txtVersionsHidden").val() == null){
			alert("Please select featured Bike");
			isValidData = false;
		}
		
		return isValidData;
	}
	
	function FeatureBikeClicked(featuredMake, featuredModel, featuredVersion){
		selectedFeaturedMake = featuredMake; 
		selectedFeaturedModel = featuredModel; 
		selectedFeaturedVersion = featuredVersion;
		$("#ddlMakeFeaturedBike").val(selectedFeaturedMake);
		ddlMake_ChangeFeaturedBike(document.getElementById("ddlMakeFeaturedBike"));
        alert("Showing result for selected Bike");        
        $("#ulMultiVersions input[type='checkbox'][valufield='" + selectedFeaturedVersion + "']").attr("checked", "checked");
        //alert($("#ulMultiVersions input[type='checkbox'][valufield='" + selectedFeaturedVersion + "']").attr("checked"));
        IsDone();
	}
	
	function DeleteFeaturedBike(featureBikeToBeDeleted){
		resDelFeaturedBike = confirm("Are you sure want to delete it as featured Bike ?");
		if (resDelFeaturedBike){
			$.ajax({
				type: "POST",
				url: "/ajaxpro/BikeWaleOpr.Common.AjaxCompareFeaturedBike,BikeWaleOpr.Common.ashx",			
				data: '{"featuredBike":"'+ featureBikeToBeDeleted +'"}',	
				beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "DeleteFeaturedBike"); },
				success: function(response) {
					var status = eval('('+ response +')');												
					if(status.value == true){
						location.href = "/contents/CompareFeaturedBike.aspx";
					}								
				}
			});
		}
}

////functions for multiselect dropdown
    $(document).ready(function () {
        $("body").attr("onclick", "BodyClicked(event);");
        //to set text of version textbox of multiselect dropdown
        $("#tbxText").val('--Select--');
        
    });

    //Dispay or hide of divMain on Main Control click
    function displayDiv() 
    {
        if ($("#divMain").is(":hidden"))
            $("#divMain").show();
        else 
        {
            $("#divMain").hide();
            VersionChanged();
        }
    }

    //On All or None Click of Control
    function checkAll(state) 
    {
        var checkboxCollection = document.getElementById("ulMultiVersions").getElementsByTagName('input');
        bolval = state;
        for (var i = 0; i < checkboxCollection.length; i++) 
        {
            if (checkboxCollection[i].type.toString().toLowerCase() == "checkbox") 
            {
                checkboxCollection[i].checked = bolval;
            }
        }
        VersionChanged();
    }

    //On Done Click of Control
    function IsDone() 
    {
        VersionChanged();
        if($("#txtVersionsHidden").val() == null || $("#txtVersionsHidden").val() == "") {
            document.getElementById("spnVersions").innerHTML="Kindly select version";
        }
        else {
            $("#divMain").hide();
            document.getElementById("spnVersions").innerHTML="";
            var versionIds = $("#txtVersionsHidden").val().split(',');
            var versionNames = $("#tbxText").val().split(",");
            var selectedFeaturedBike = ''; 
            for(var j = 0; j < versionIds.length; j++)
            {
		        selectedFeaturedBike = selectedFeaturedBike + document.getElementById("ddlMakeFeaturedBike").options[document.getElementById("ddlMakeFeaturedBike").selectedIndex].text;
		        selectedFeaturedBike = selectedFeaturedBike + " " + document.getElementById("ddlModelFeaturedBike").options[document.getElementById("ddlModelFeaturedBike").selectedIndex].text;
		        selectedFeaturedBike = selectedFeaturedBike + " " + versionNames[j];
		        arrSelectedFeaturedBike = selectedFeaturedBike.split(" ");
		
		        for (var i=0; i<arrSelectedFeaturedBike.length; i++){
			        selectedFeaturedBike = selectedFeaturedBike.replace(" ","_");
		        }
                if(j != versionIds.length-1) {
                    selectedFeaturedBike = selectedFeaturedBike + ",";
                }
            }
            //alert("main selctd cr: " + selectedFeaturedBike);
		    $("#divComparisonBikes").load("ComparisonBikes.aspx?featureBikeId=" + versionIds + "&featureBike=" + selectedFeaturedBike);
		    $("#divComparisonBikes").show();
        }
    }

    //On Body Click
    function BodyClicked(e) 
    {
        if ($(e.target).attr("id") != "divMainControl" && $(e.target).parent().attr("id") != "divMainControl") 
        {
            var divMainClicked = false;
            $(e.target).parents().each(function () 
            {
                if ($(this).attr("id") != "undefined" && $(this).attr("id") == "divMain")
                    divMainClicked = true;
            });

            if (divMainClicked == false)
                $("#divMain").hide();
            VersionChanged();
        }
    }
	
    //On Change Event of Make fill Model
    function ddlModel_ChangeFeaturedBike(ddl){
		$("#divComparisonBikes").hide();
        var response =AjaxFunctions.GetNewVersions(ddl.value);
        //alert(response.value);
        $("#ulMultiVersions").empty();
        ds = response.value;

        for (var i = 0; i < ds.Tables[0].Rows.length; i++) {
            var tempModel = $(document.createElement("li"));
            var tempChk = $(document.createElement("input"));
            tempChk.attr("type", "checkbox");
            tempChk.attr("ValuField", ds.Tables[0].Rows[i].Value.toString());
            tempChk.attr("TextField", ds.Tables[0].Rows[i].Text);
            tempChk.attr("onchange", "VersionChanged();")
            var tempSpan = $(document.createElement("span"));
            tempSpan.attr("style", "position:relative;bottom:2px;");
            tempModel.append(tempChk);
            tempSpan.html(ds.Tables[0].Rows[i].Text);
            tempModel.append(tempSpan);
            $("#ulMultiVersions").append(tempModel);
        }
        var _ispostback = (<%= IsPostBack.ToString().ToLower() %>);
        if(_ispostback != true)
        {
            $("#tbxText").val('--Select--');
        }
        else
        {
        var tbxtext =  $("#tbxText").val();
        var array = tbxtext.split(',');
        for (a in array ) 
            {
            $("#ulMultiVersions input[type='checkbox']").each(function () 
                {
                    if(array[a] ==$(this).attr("TextField"))
                       $(this).attr('checked','ckecked');
                
                });
            }
        }
        }

    //Version Checked in CheckBox is saved in TextBox
	function VersionChanged() 
    {
	    var modelValue = "";
	    var modelText = "";
	    var isFirst = true;

	    $("#ulMultiVersions input[type='checkbox']").each(function () 
        {
	        if ($(this).is(":checked")) 
            {
                if (isFirst == true) 
                {
	                modelValue = $(this).attr("valufield");
	                modelText = $(this).attr("TextField");
	                $("#tbxText").val(modelText);
	                isFirst = false;
	            }
	            else 
                {
	                modelValue = modelValue + "," + $(this).attr("valufield");
	                modelText = modelText + "," + $(this).attr("TextField");
	                $("#tbxText").val(modelText);
	            }
	        }
	    });

	    if (modelText == "")
        {
	        $("#tbxText").val('--Select--');
             $("#txtVersionsHidden").val('')
        }
	    else 
        {
	        $("#txtVersionsHidden").val(modelValue);
	        $("#tbxText").val(modelText);
	    }
	}
</script>

<!-- #Include file="/includes/footerNew.aspx" -->