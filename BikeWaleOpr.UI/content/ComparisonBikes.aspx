<%@ Page Language="C#" Inherits="BikeWaleOpr.Content.ComparisonBikes" AutoEventWireUp="false" %>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {

        $(".divAddBike").bt({ contentSelector: "$('#bidPopUp').html()", fill: '#fff', strokeWidth: 1, strokeStyle: '#79B7E7', spikeLength: 10,
            cssStyles: { fontSize: '11px' }, width: '320px', trigger: ['click', 'none'],
            preShow: function (box) {
                $(".bt-wrapper ").hide();
            },
            showTip: function (box) {
                $(box).show();
                $(box).find("#btnCopy").hide();
                $(box).find("#closeBox").click(function () {
                    $(box).hide();
                });
                $(box).find("#ddlMake").change(function () {
                    ddlMake_Change(this);
                });
                $(box).find("#ddlModel").change(function () {
                    ddlModel_Change(this);
                });
                $(box).find("#btnAdd").click(function () {
                    /*if ($(box).find("#ddlVersion").val() > 0)
                    {
                    $(box).find("#spnError").hide();
                    AddBike(this);
                    $(box).hide();
                    }
                    else
                    {
                    $(box).find("#spnError").show();
                    }*/
                    //alert($(box).find("#ddlVersion").val());
                    if ($(box).find("#ddlVersion").val() <= 0) {
                        $(box).find("#spnUrl").hide();
                        $(box).find("#spnError").show();
                    }
                    else if ($(box).find("#txtUrl").val() != "") {

                        if (!CheckUrl($(box).find("#txtUrl"))) {
                            $(box).find("#spnUrl").show();
                            $(box).find("#spnError").hide();
                        }
                        else {
                            $(box).find("#spnError").hide();
                            $(box).find("#spnUrl").hide();
                            AddBike(this);
                            $(box).hide();
                        }
                    }
                    else {
                        $(box).find("#spnError").hide();
                        $(box).find("#spnUrl").hide();
                        AddBike(this);
                        $(box).hide();
                    }
                });
            },
            hoverIntentOpts: { interval: 0, timeout: 0 }
        });
        /*
        $(".divCopyBike").bt({ contentSelector: "$('#bidPopUp').html()", fill: '#fff', strokeWidth: 1, strokeStyle: '#79B7E7', spikeLength: 10,
            cssStyles: { fontSize: '11px' }, width: '300px', height: '50px', trigger: ['click', 'none'],
            preShow: function (box) {
                $(".bt-wrapper ").hide();
            },
            showTip: function (box) {
                $(box).show();
                $(box).find("#btnAdd").hide();
                $(box).find(".details").hide();
                $(box).find("#closeBox").click(function () {
                    $(box).hide();
                });
                $(box).find("#ddlMake").change(function () {
                    ddlMake_Change(this);
                });
                $(box).find("#ddlModel").change(function () {
                    ddlModel_Change(this);
                });
                $(box).find("#btnCopy").click(function () {
                    if ($(box).find("#ddlVersion").val() <= 0) {
                        $(box).find("#spnError").show();
                    }
                    else {
                        $(box).find("#spnError").hide();
                        CopyBike(this);
                        $(box).hide();
                    }
                });
            },
            hoverIntentOpts: { interval: 0, timeout: 0 }
        });*/
    });
    function CheckUrl(txt) {

        var pattern = /^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$/;

        OK = pattern.test($(txt).val());

        if (!OK) {
            return false;
        }
        else {
            return true;
        }
    }
</script>
<table cellpadding="5" cellspacing="0" style="border:1px solid;border-collapse:collapse;width:450px;margin-top:14px;" >
	<tr>
		<td style="background-color:#CCCCCC;width:100%;font-size:12px;font-weight:bold;" colspan="2">
			Comparison Bikes for : <%=selectedFeaturedBike%>
			<input id="hdnSelectedFeaturedBike" type="hidden" value="<%=Request.QueryString["featureBikeId"].ToString()%>" />
		</td>
	</tr>
	<tr>
		<td>
			<table cellpadding="5" border="0" cellspacing="0" width="100%">
				<tr>
					<td colspan="2" style="border-bottom:1px solid #CCCCCC;width:100%;">
						<span class="divAddBike" style="color:#003366;cursor:pointer;font-weight:bold;font-size:12px;">Add Bike</span>
                        <div style=" display:none; ">
                            <span style="font-size:12px;"> or </span>
                            <span class="divCopyBike" style="color:#003366;cursor:pointer;font-weight:bold;font-size:12px;">Copy from</span>
                        </div>
						<input type="hidden" id="hdnBikesAdded" value="" />
						<input type="hidden" id="hdnBikesAlreadyAdded" value="" />
					</td>
				</tr>
			</table>	
			<table id="tblBikesAdded" cellpadding="5" cellspacing="0" border="0">
				<tr style="display:none;">
					<td style="width:375px;">&nbsp;</td>
					<td style="width:125px;">&nbsp;</td>
				</tr>	
				<asp:Repeater ID="rptComparisonBikes" runat="server">
					<headertemplate>
						<script language="javascript" type="text/javascript">
						    $("#hdnBikesAlreadyAdded").val("");
						</script>
					</headertemplate>
					<itemtemplate>
						<script language="javascript" type="text/javascript">
							if ($("#hdnBikesAlreadyAdded").val() == "")
							{
								$("#hdnBikesAlreadyAdded").val("<%# DataBinder.Eval( Container.DataItem, "VersionId" ) %>");	
							}
							else
							{
								$("#hdnBikesAlreadyAdded").val($("#hdnBikesAlreadyAdded").val() + ",<%# DataBinder.Eval( Container.DataItem, "VersionId" ) %>");	
							}
						</script>
						
						<tr>
							<td style="border-bottom:1px solid #CCCCCC;width:375px;"><%# DataBinder.Eval( Container.DataItem, "ComparisonBike" ) %></td>
							<td style="border-bottom:1px solid #CCCCCC;width:125px;"><span style="cursor:pointer;text-decoration:underline;color:blue;" onclick="DeletePreviouslySavedBike(this,'<%# DataBinder.Eval( Container.DataItem, "VersionId" ) %>')">Delete Saved Bike</span></td>
						</tr>	
					</itemtemplate>
				</asp:Repeater>
			</table>
		</td>
	</tr>
</table>
<div style="margin-top:14px;"><input type="button" id="btnSave" value="Save" onclick="SaveCompareFeaturedBike()" /></div>
