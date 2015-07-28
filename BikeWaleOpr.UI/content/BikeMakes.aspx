<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.Content.BikeMakes" trace="false" debug="false" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<script type="text/javascript" src="/src/common/common.js?V1.1"></script>
<script src="/src/bt.js"></script>
<div class="urh">
		You are here &raquo; Contents &raquo; Add Bike Makes
</div>
<div>
    <!-- #Include file="contentsMenu.aspx" -->
</div>
<script type="text/javascript" language="javascript" src="/src/AjaxFunctions.js"></script>
<div class="left">
<h1>Bike Makes</h1>
<form id="Form1" runat="server">
	<span id="spnError" class="error" runat="server"></span>
	<fieldset>
		<legend>Add New Bike Make</legend>
        <div>
            <div  class="margin10">
                <div  class="floatLeft">
                    Make Name : &nbsp;&nbsp;&nbsp;&nbsp;
                </div>
                <div class="floatLeft inputWidth">
		            <asp:TextBox ID="txtMake" MaxLength="50" Width="100" runat="server" />
                </div>
                <div><span class="errorMessage" id="spntxtName"></span></div>
            </div>
            <div class="clear"></div>
            <div class="margin10">
                 <div  class="floatLeft">
                    Masking Name :&nbsp;&nbsp;
                  </div>
                <div  class="floatLeft inputWidth">
                    <asp:TextBox ID="txtMaskingName" MaxLength="50"  runat="server" width="100" />
                </div>
            </div>
            <div  class="floatLeft">
                    <span class="errorMessage" id="spntxtMaskingName"></span>
            </div>     
            <div class="clear"></div>
            <div  class="floatLeft margin10" >
                <span class="greenMsg">[Masking Name will be used for url formation.Only lowercase letters,- and digits are allowed.]</span>
            </div>
            <div class="clear"></div>
            <div>
		    <asp:Button ID="btnSave" Text="Add Make" runat="server" />
            </div>
        </div>
	</fieldset>	<br /><br />
    <asp:Label ID="lblStatus" runat="server" class="errorMessage" />
	<asp:DataGrid ID="dtgrdMembers" runat="server" 
			DataKeyField="ID" 
			CellPadding="5" 
			BorderWidth="1" 
			AllowPaging="true" 
			width="1000"
			PagerStyle-Mode="NumericPages" 
			PageSize="25" 
			AllowSorting="true" 
			AutoGenerateColumns="false" CssClass="margin-top10">
		<itemstyle CssClass="dtItem"></itemstyle>
		<headerstyle CssClass="dtHeader"></headerstyle>
		<alternatingitemstyle CssClass="dtAlternateRow"></alternatingitemstyle>
		<edititemstyle CssClass="dtEditItem"></edititemstyle>
		<columns>
			<asp:TemplateColumn HeaderText="Bike Make" SortExpression="Name" ItemStyle-Width="350">
				<itemtemplate>
					<%--<%# DataBinder.Eval( Container.DataItem, "Name" ) %>--%>
                    	<div class="<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Futuristic" )) ? "yellow" : Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "New" )) ? "green" : "orange" %>"><%# DataBinder.Eval( Container.DataItem, "Name" ) %></div>
				</itemtemplate>				
				<edititemtemplate>
					<asp:TextBox ID="txtMake" Text='<%# DataBinder.Eval( Container.DataItem, "Name" ) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>	
            <asp:TemplateColumn HeaderText="Masking Name" ItemStyle-Width="350">
			<itemtemplate>
			    <span><%# DataBinder.Eval( Container.DataItem, "MaskingName" ) %></span>&nbsp;&nbsp;<a ID='editId_<%# DataBinder.Eval( Container.DataItem, "ID" ) %>' class='pointer <%# string.IsNullOrEmpty(DataBinder.Eval( Container.DataItem, "MaskingName" ).ToString()) ? "hide" : "" %>' title="Update Masking Name">Edit</a> 
			</itemtemplate>            
		    </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Futuristic" >
			<itemtemplate>
				<asp:CheckBox ID="chkFut"  Checked='<%# DataBinder.Eval( Container.DataItem, "Futuristic" ) %>' Enabled="false" runat="server" />
			</itemtemplate>
			<edititemtemplate>
				<asp:CheckBox ID="chkFut" Checked='<%# DataBinder.Eval( Container.DataItem, "Futuristic" ) %>' runat="server" />
			</edititemtemplate>
			</asp:TemplateColumn>	
            <asp:TemplateColumn HeaderText="Used">
				<itemtemplate>
					<asp:CheckBox ID="chkUsed" Checked='<%# DataBinder.Eval( Container.DataItem, "Used" ) %>' Enabled="false" runat="server" />
				</itemtemplate>
				<edititemtemplate>
					<asp:CheckBox ID="chkUsed" Checked='<%# DataBinder.Eval( Container.DataItem, "Used" ) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="New">
				<itemtemplate>
					<asp:CheckBox ID="chkNew" Checked='<%# DataBinder.Eval( Container.DataItem, "New" ) %>' Enabled="false" runat="server" />
				</itemtemplate>
				<edititemtemplate>
					<asp:CheckBox ID="chkNew" Checked='<%# DataBinder.Eval( Container.DataItem, "New" ) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>	
            <asp:TemplateColumn HeaderText="Synopsis" ItemStyle-Width="100">
			    <itemtemplate>
			        <input type="button" value="Add" onclick="javascript:window.open('MakeSynopsis.aspx?make=<%# DataBinder.Eval( Container.DataItem, "ID" ) %>','','left=350,top=80,width=600,height=500,scrollbars=yes')" />
			    </itemtemplate>
			</asp:TemplateColumn>
			<asp:EditCommandColumn EditText="Edit" CancelText="Cancel" UpdateText="Update" />

<%--			<asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" class="deleteBike" Text="Delete" />--%>
            <asp:TemplateColumn>
              <itemtemplate>
                    <div class="alignCenter">
                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="http://opr.carwale.com/images/icons/delete.ico" CommandName="Delete" class="deleteBike"/>
                    </div>
                </itemtemplate>
            </asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Created On" ItemStyle-Width="350">
			    <itemtemplate>
			        <%# DataBinder.Eval( Container.DataItem, "CreatedOn" ) %>
			    </itemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Updated On" ItemStyle-Width="350">
			    <itemtemplate>
			        <%# DataBinder.Eval( Container.DataItem, "UpdatedOn" ) %>
			    </itemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Last Updated By" ItemStyle-Width="350">
			    <itemtemplate>
			        <%# DataBinder.Eval( Container.DataItem, "UpdatedBy" ) %>
			    </itemtemplate>
			</asp:TemplateColumn>
		</columns>
	</asp:DataGrid>
    <div id="divMaskName" class="hide">
        <div id="divWarnMsg" class="errorMessage">
            <div>If you change the masking name it will affect the URLs.</div><div>Still you want to change masking name.</div>
            <div class="margin-top10">
                <input type="button" value="Yes" id="btnYes" />
                <input type="button" value="No" id="btnNo" />
            </div>            
        </div>
        <div id="divUpdMaskName" class="hide">
            <div>
                <input type="text" id="txtUpdMaskName" style="width:150px" MaxLength="50"/>
                <input type="button" value="Update" id="btnUpdateMaskName" />    
                <span id="errUpdMaskName" class="errorMessage margin-left5"></span>            
            </div>
            <span class="greenMsg">Masking Name will be used for url formation.<br />Only lowercase letters,- and digits are allowed.</span>
        </div>
    </div>
</form>
<script type="text/javascript" language="javascript">

    //$(document.ready(function(){
       //alert("");
        $(".deleteBike").click(function () {
            if (!confirm("Do you really want to delete this make."))
            {
                return false;
            }        
        });
    //}));
    if (document.getElementById('btnSave'))
        document.getElementById('btnSave').onclick = btnSave_Click;

    function btnSave_Click() {
        $("#spntxtMaskingName").text("");
        var isError = false;
        if ($("#txtMake").val() == "") {
            $("#spntxtName").text("Fill Make");
            isError = true;
        }
        if ($("#txtMaskingName").val() == "") {
            $("#spntxtMaskingName").text("Masking Name Required.");
            isError = true;
        }
        else if (hasSpecialCharacters($("#txtMaskingName").val()))
        {
            $("#spntxtMaskingName").text("Invalid Masking Name. ");
            isError = true;
        }
        if (isError)
            return false;
    }

    $('#txtMake').blur(function () {
        var make = jQuery.trim($('#txtMake').val());
        make = make.trim();
        make = make.replace(/\s+/g, "-");
        make = make.replace(/[^a-zA-Z0-9\-]+/g, '');
        make = removeHyphens(make);
        $('#txtMaskingName').val(make.toLowerCase());
    });

    function removeHyphens(str) {
        // if( str.indexOf("-")==0)
        //   str=str.substr(1);//for removing starting hyphen...	        
        str = str.replace(/(-)+/g, '-');//for replacing multiple consecutive hyphens with one hyphen...
        str = str.replace(/^\-+|\-+$/g, '');
        return str;
    }

    updBy = <%= BikeWaleOpr.Common.CurrentUser.Id %>
    $("a[id^='editId_']").bt({
        contentSelector: "$('#divMaskName').html()", fill: '#ffffff', strokeWidth: 1, strokeStyle: '#D3D3D3', width: '360px', spikeLength: 10, shadow: true,
        positions: ['right', 'left', 'bottom'], trigger: ['click'],
        preShow: function (box) {
            $(".bt-wrapper").hide();
        },
        showTip: function (box) {
            boxObj = $(box);
            boxObj.show();
            //alert(updBy);
            var objYes = boxObj.find("#btnYes");
            var objNo = boxObj.find("#btnNo");
            var makeId = $(this).attr("id").split("_")[1];
            var objOldMask = $(this).siblings();
            var oldMaskingName = objOldMask.text();

            objYes.click(function () {
                boxObj.find("#divWarnMsg").hide();
                boxObj.find("#txtUpdMaskName").val(oldMaskingName);
                boxObj.find("#divUpdMaskName").show();
            });

            objNo.click(function () {
                $(".bt-wrapper").hide();
            });

            //var imageId = $(this).attr("image-id");
            //var objMakes = boxObj.find("#ddlUpdateMakes");
            //var objModels = boxObj.find("#ddlUpdateModels");

            //FillMakes(boxObj);
            //objMakes.val($(this).attr("make-id"));

            //FillModels(boxObj, $(this).attr("make-id"));
            //objModels.val($(this).attr("model-id"));

            //objMakes.change(function () {
            //    var makeId = $(this).val();
            //    FillModels(boxObj, makeId);
            //});

            boxObj.find("#btnUpdateMaskName").click(function () {
                var maskName = boxObj.find("#txtUpdMaskName").val();
                var errMask = boxObj.find("#errUpdMaskName");

                errMask.text("");

                if (maskName == "" || maskName == null) {
                    errMask.text("Required");
                }
                else if (hasSpecialCharacters(maskName) == true) {
                    errMask.text("Invalid Masking Name");
                }
                else {
                    maskName = maskName.trim();
                    maskName = maskName.replace(/\s+/g, "-");
                    maskName = removeHyphens(maskName);
                    //$('#txtUpdMaskName').val(maskName);
                    //alert(maskName + updBy + makeId);
                    $.ajax({
                        type: "POST",
                        url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                        data: '{"maskingName":"' + maskName + '","updatedBy":"' + updBy + '","makeId":"' + makeId + '"}',
                        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateMakeMaskingName"); },
                        success: function (response) {
                            if(eval('(' + response + ')').value)
                            {
                                boxObj.find("#divUpdMaskName").html("Masking Name Updated Successfully.");
                                objOldMask.text(maskName);
                            }
                            else
                            {
                                boxObj.find("#divUpdMaskName").html("Masking Name Should be Unique.").addClass('errorMessage');	
                            }
                            //$("#"+ID).remove();
                        }
                    });
                }
            });
        }
    });

</script>
</div>
<!-- #Include file="/includes/footerNew.aspx" -->