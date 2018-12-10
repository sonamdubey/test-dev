<%@ Control Language="C#" AutoEventWireUp="false"  Inherits="Carwale.UI.Controls.QuickResearch" %>
<style>
	.qr-hor{list-style:none;}
	.qr-hor li{float:left; padding-right:5px;}
	.qr-hor li select{width:150px;}
	
	.qr-ver{list-style:none;}
	.qr-ver li{padding:2px 0;}
	
</style>
<ul id="ulQuickResearch" runat="server" class="qr-ver">
	<li>
        <div class="form-control-box margin-bottom10">
            <asp:DropDownList CssClass="form-control input-sm" ID="drpMake" data-bind="value: Make,foreach: Makes" runat="server" ><asp:ListItem data-bind="value: makeId, text: makeName"></asp:ListItem></asp:DropDownList>
        </div>
	</li>
	<li>
        <div class="form-control-box margin-bottom10">
            <asp:DropDownList CssClass="form-control input-sm" ID="drpModel" data-bind="value:Model,foreach: Models" runat="server" ><asp:ListItem data-bind="value: ModelId,text:ModelName, attr: { 'mask': MaskingName }"></asp:ListItem></asp:DropDownList>
        </div>
	</li>
	<li>
        <div class="form-control-box margin-bottom10">
            <asp:DropDownList CssClass="form-control input-sm" ID="drpVersion" data-bind="value:Version,foreach: Versions" runat="server" ><asp:ListItem data-bind="value: ID,text:Name, attr:{'maskingName': MaskingName }"></asp:ListItem></asp:DropDownList>
        </div>
	</li>
	<li><a class="buttons btn btn-orange" id="btnQuickResearch" onclick="javascript:QuickResearch_click()">Go</a></li>

</ul>
<div style="clear:both;"></div>
<input style="clear:both;" type="hidden" id="hdn_drpModel" runat="server" />
<input type="hidden" id="hdn_drpVersion" runat="server" />
<input type="hidden" id="hdn_selModel" runat="server" />
<input type="hidden" id="hdn_selVersion" runat="server" />

<script type="text/javascript">
	function QuickResearch_click() {
		if($("#<%=drpMake_Id %>").val() < 1) {
		    window.location.href = "/new/";
		    return false;
		}
	 	var makeId = $("#<%=drpMake_Id%>").val();
		var makeName =  $("#<%=drpMake_Id%> option:selected").text().replace("-","");
		var modelId = $("#<%=drpModel_Id%>").val() ;
	    var modelName =  $("#<%=drpModel_Id%> option:selected").attr('mask') != undefined ? $("#<%=drpModel_Id%> option:selected").attr('mask').replace("+","") : '';
		var versionId = $("#<%=drpVersion_Id%>").val();
	    var versionMasking = $("#<%=drpVersion_Id%> option:selected").attr('maskingName') != undefined ? $("#<%=drpVersion_Id%> option:selected").attr('maskingName') : '';
	    var strRedirect = "/";
	    if (makeId > 0) {
	        strRedirect += qrFormatSpecial(makeName) + "-cars/";
	        if (modelId != "" && modelId > 0) {
	            strRedirect += qrFormatSpecial(modelName) + "/";
	            if (versionMasking != "")
	                strRedirect += versionMasking + "/";
	        }
	    }
	    location.href = strRedirect;
	}

    function qrFormatSpecial(str) {
        str = str.replace(/\./g, "");
        str = str.replace(/ /gi, "");
        return str.toLowerCase();
    }

    
    $(document).ready(function () {
        var qRMakeId =<%=MakeId%> +0;
        var qRModelId =<%=ModelId%> +0;

        var koViewModel = eval('(' + genericMakeModelKVM + ')');
        koViewModel.Models([]);
        koViewModel.Versions([]);
        koViewModel.Make(<%=MakeId%>);
        //alert(<%=drpMakeJson%>);
        var drpMakeJson = eval('(<%=drpMakeJson%>)');
        koViewModel.Makes(drpMakeJson);

        if (qRMakeId > 0) {
            var drpModelJson = eval('(<%=drpModelJson%>)');
            koViewModel.Models(drpModelJson);
            if (qRModelId > 0) { koViewModel.Model(qRModelId); $('#<%=drpModel_Id%>').change(); }
        }

        if (qRModelId > 0) {
            var drpVersionJson = eval('(<%=drpVersionJson%>)');
            koViewModel.Versions(drpVersionJson);
        }
        koViewModel.Makes.unshift({ "makeId": -1, "makeName": "--Select Make--" });
        koViewModel.Models.unshift({ "ModelId": -1, "ModelName": "--Select Model--", "MaskingName": "" });
        koViewModel.Versions.unshift({ "ID": -1, "Name": "--Select Version--", "MaskingName": "" });

        ko.applyBindings(koViewModel, $('#<%=drpMake_Id%>').parent().parent().parent()[0]);

        if (!(qRMakeId > 0)) { $('#<%=drpModel_Id%>').attr('disabled',true) }
        if (qRModelId > 0) {
            $('#<%=drpModel_Id%>').val(qRModelId).change(); koViewModel.Version(-1);
        }
        else {
            $('#qrQuickResearch_drpModel').val(-1);
            $('#<%=drpVersion_Id%>').attr('disabled',true);
        }
        $("#<%=drpMake_Id%>").change(function () {
            bindModelsList("new",$('#<%=drpMake_Id%> option:selected').val(), koViewModel, '#<%=drpModel_Id%>',"--Select Model--");
        });

        $("#<%=drpModel_Id%>").change(function () {
            bindVersionsByModelList("new", $('#<%=drpModel_Id%> option:selected').val(), koViewModel, '#<%=drpVersion_Id%>', "--Select Version--");
        });

        $("#<%=drpVersion_Id%>").change(function () {
            $("#<%=hdn_selVersion_Id%>").val($("#<%=drpVersion_Id%> option:selected").text());
        	});
    });
	
</script>