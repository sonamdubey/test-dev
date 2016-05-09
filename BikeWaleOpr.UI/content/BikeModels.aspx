<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.Content.BikeModels" Trace="false" Debug="false" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<script type="text/javascript" src="/src/common/common.js?V1.1"></script>
<script src="/src/bt.js"></script>
<div class="urh">
		You are here &raquo; Contents &raquo; Add Bike Models
</div>
<div>
    <!-- #Include file="contentsMenu.aspx" -->
</div>
<script language="javascript" src="/src/AjaxFunctions.js"></script>
<div class="left">

<h1>Bike Models</h1>
<style>
	.doNotDisplay { display:none; }
    .color-code { list-style:none; margin-top:10px; }
    .color-code li { float:left; margin-left:5px; }
    .color-code .upcoming { border:1px solid #000000; background-color:#FFFF7F; width:12px; height:12px; }
    .color-code .new { border:1px solid #000000; background-color:#B2Df75; width:12px; height:12px; }
    .color-code .discontinued { border:1px solid #000000; background-color:#FDBA2B; width:12px; height:12px; }
</style>
	<span id="spnError" class="error" runat="server"></span>
    <div>
        <div class="floatLeft" style=" width:620px;">
            <fieldset>
	            <legend>Add New Bike Model</legend>
                <div>
                    <div class="margin10" >
                        <div  class="floatLeft">
                            Select Make :
                        </div>
                        <div class="floatLeft margin-left10">
                            <asp:DropDownList ID="cmbMakes" AutoPostBack="true" runat="server"  width="100%"/>
                        </div>
                        <div  class="floatLeft margin-left10">
                                <span id="spntxtMake" class="errorMessage"></span>
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="margin10" >
                        <div  class="floatLeft">
                            Select Series :
                        </div>
                        <div class="floatLeft margin-left10">
                                <asp:DropDownList ID="ddlSeries" runat="server"  width="100%">
                                    <asp:ListItem Text="--Select Series--" Value="-1" />
                                </asp:DropDownList>
                        </div>
                        <div  class="floatLeft margin-left10">
                                <span id="spntxtSeries" class="errorMessage"></span>
                        </div>
                        <div class="floatLeft margin-left10">
                            Select Segment :
                        </div>
                        <div class="floatLeft margin-left10">
                            <asp:DropDownList ID="ddlSegment" runat="server" width="100%">
                                    <asp:ListItem Text="--Select Segment--" Value="-1" />
                            </asp:DropDownList>
                        </div>
                        <div class="margin-left10">
                            <span id="spnSegment" class="errorMessage"></span>
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="margin10">
                        <div  class="floatLeft">
                            Model Name :
                        </div>
                        <div  class="floatLeft inputWidth margin-left10" >
                            <asp:TextBox ID="txtModel" MaxLength="50" runat="server" width="100%" />
                        </div>
                        <div class="floatLeft margin-left10"><span class="errorMessage" id="spntxtName"></span></div>    
                    </div>
                    <div class="clear"></div>
                    <div class="margin10">
                        <div  class="floatLeft">
                            Masking Name :
                        </div>
                        <div  class="floatLeft inputWidth margin-left10">
                            <asp:TextBox ID="txtMaskingName" MaxLength="50"  runat="server" width="100%" />
                        </div>
                        <div  class="floatLeft">
                                &nbsp;&nbsp;&nbsp;<span class="errorMessage" id="spntxtMaskingName"></span>
                                <asp:CheckBox ID="chkIsMaskingNameChanged"  style="display:none;" runat="server" />
                        </div>                         
                    </div>
                    <div class="clear"></div>
                    <div  class="floatLeft margin10" >
                                <span class="greenMsg">[Masking Name will be used for url formation.Only lowercase letters,- and digits are allowed.]</span>
                    </div>
                    <div class="clear"></div>
                    <div  class="floatLeft margin10">
                        <asp:Button ID="btnSave" Text="Add Model" runat="server" />              
                    </div>
                    <div class="clear"></div>
                    <div>                
                        <ul class="color-code">
                            <li><div class="upcoming">&nbsp</div></li>
                            <li>Upcoming Bikes</li>
                            <li><div class="new">&nbsp</div></li>
                            <li>New Bikes</li>
                            <li><div class="discontinued">&nbsp</div></li>
                            <li>Discontinued Bikes</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="floatLeft" style="width:450px;">
            <fieldset style="height:110px;">
                <legend>Update Existing Model Series</legend>
                <div>
                    <div class="margin10" >
                        <div  class="floatLeft">
                            Select Series :
                        </div>
                        <div class="floatLeft margin-left10">
                            <asp:DropDownList id="ddlUpdateSeries" runat="server">
                                <asp:ListItem Text="--Select Series--" Value="-1" />
                            </asp:DropDownList>
                        </div>
                        <div  class="floatLeft margin-left10">
                              <span id="spnUpdateSeries" class="errorMessage"></span>
                        </div>
                        <div  class="floatLeft margin-left10">
                             <input type="button" id="btnModel" value="Select Bike Model(s)" />
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="margin-top10" >
                        <div  style="text-align:center;margin-top:20px;">
                            <input type="submit" id="btnUpdateSeries" value="Update Model Series" onclick="return ValidateUpdateSeries();" runat="server"/><span id="spnErrSeries" class="errorMessage"></span>
                            <asp:HiddenField id="hdnModelIdList" runat="server"></asp:HiddenField>
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
            </fieldset>	
        </div>
        <div class="floatLeft" style="width:450px;">
            <fieldset style="height:110px;">
                <legend>Update Existing CC Segments</legend>
                <div>
                    <div class="margin10" >            
                        <div  class="floatLeft">
                            Select Segment :
                        </div>
                        <div class="floatLeft margin-left10">
                            <asp:DropDownList id="ddlUpdateSegment" runat="server">
                                <asp:ListItem Text="--Select Segment--" Value="-1" />
                            </asp:DropDownList>
                        </div>
                        <div  class="floatLeft margin-left10">
                                <span id="spnUpdateSeg" class="errorMessage"></span>
                        </div>
                        <div  class="floatLeft margin-left10">
                             <input type="button" id="btnSelModel" value="Select Bike Model(s)" />
                        </div>                    
                    </div>
                    <div class="clear"></div>
                    <div class="margin-top10">
                        <div  style="text-align:center;margin-top:20px;">
                            <input type="submit" id="btnUpdateSegment" value="Update Model Segment" runat="server"/><span id="spnSeg" class="errorMessage"></span>
                            <asp:HiddenField id="hdnModelIdsList" runat="server"></asp:HiddenField>
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
            </fieldset>	
        </div>     
    </div>
    <div class="clear"></div>
    <br /><br /><br />

    <asp:Label ID="lblStatus" runat="server" class="errorMessage" />

	<asp:DataGrid ID="dtgrdMembers" runat="server"
			DataKeyField="ID"
			CellPadding="5"
			BorderWidth="1"
			width="100%"
			AllowPaging="false"
			AllowSorting="true" 
			AutoGenerateColumns="false">
		<itemstyle CssClass="dtItem"></itemstyle>
		<headerstyle CssClass="dtHeader"></headerstyle>
		<alternatingitemstyle CssClass="dtAlternateRow"></alternatingitemstyle>
		<edititemstyle CssClass="dtEditItem"></edititemstyle>
		<columns>
			<asp:TemplateColumn HeaderText="Models" SortExpression="Name" ItemStyle-Width="200">
                <%--<ItemStyle BackColor="#FFFF7F"></ItemStyle>--%>
				<itemtemplate>
					<div class="<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Futuristic" )) ? "yellow" : Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "New" )) ? "green" : "orange" %>"><%# DataBinder.Eval( Container.DataItem, "Name" ) %></div>
				</itemtemplate>
				<edititemtemplate>
					<asp:TextBox ID="txtModelName" MaxLength="50" Columns="15" Text='<%# DataBinder.Eval( Container.DataItem, "Name" ) %>' runat="server" />
					<asp:Label Visible="false" ID="lblMakeId" Text='<%# DataBinder.Eval( Container.DataItem, "BikeMakeId" ) %>' runat="server"></asp:Label>
				</edititemtemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Masking Name" ItemStyle-Width="350">
			    <itemtemplate>
			      <span><%# DataBinder.Eval( Container.DataItem, "MaskingName" ) %></span>&nbsp;&nbsp;<a ID="editId_<%# DataBinder.Eval( Container.DataItem, "ID" ) %>" class='pointer <%# string.IsNullOrEmpty(DataBinder.Eval( Container.DataItem, "MaskingName" ).ToString()) ? "hide" : "" %>' title="Update Masking Name">Edit</a> 
			    </itemtemplate>            
			</asp:TemplateColumn>
            <asp:BoundColumn DataField="BikeMakeId" ReadOnly="true" ItemStyle-CssClass="doNotDisplay" HeaderStyle-CssClass="doNotDisplay" />
            <asp:TemplateColumn HeaderText="Series Name" ItemStyle-Width="1100">
			    <itemtemplate>
			        <input type="checkbox" name="chkSeries" modelId='<%#DataBinder.Eval(Container.DataItem,"ID") %>' disabled="disabled" />&nbsp;&nbsp;&nbsp;<span><%# DataBinder.Eval( Container.DataItem, "SeriesName" ) %></span>
			    </itemtemplate>            
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="CC Segment" ItemStyle-Width="1100">
                <itemtemplate>
                    <input    type="checkbox" name="chkSegment" modelId='<%# DataBinder.Eval(Container.DataItem,"ID") %>' disabled="disabled"/><span><%#DataBinder.Eval(Container.DataItem,"ClassSegmentName") %></span>
                </itemtemplate>
            </asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Used">
				<itemtemplate>
					<asp:CheckBox ID="chkUsed" Checked='<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Used" )) %>' Enabled="false" runat="server" />
				</itemtemplate>
				<edititemtemplate>
					<asp:CheckBox ID="chkUsed" Checked='<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Used" )) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="New">
				<itemtemplate>
					<asp:CheckBox ID="chkNew" Checked='<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "New" )) %>' Enabled="false" runat="server" />
				</itemtemplate>
				<edititemtemplate>
					<asp:CheckBox ID="chkNew" Checked='<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "New" )) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Ind.">
				<itemtemplate>
					<asp:CheckBox ID="chkIndian" Checked='<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Indian" )) %>' Enabled="false" runat="server" />
				</itemtemplate>
				<edititemtemplate>
					<asp:CheckBox ID="chkIndian" Checked='<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Indian" )) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Imp.">
				<itemtemplate>
					<asp:CheckBox ID="chkImported" Checked='<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Imported" )) %>' Enabled="false" runat="server" />
				</itemtemplate>
				<edititemtemplate>
					<asp:CheckBox ID="chkImported" Checked='<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Imported" )) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Clas.">
				<itemtemplate>
					<asp:CheckBox ID="chkClassic" Checked='<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Classic" )) %>' Enabled="false" runat="server" />
				</itemtemplate>
				<edititemtemplate>
					<asp:CheckBox ID="chkClassic" Checked='<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Classic" )) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Mod.">
				<itemtemplate>
					<asp:CheckBox ID="chkModified" Checked='<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Modified" )) %>' Enabled="false" runat="server" />
				</itemtemplate>
				<edititemtemplate>
					<asp:CheckBox ID="chkModified" Checked='<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Modified" )) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Futur.">
				<itemtemplate>
					<asp:CheckBox ID="chkFuturistic" Checked='<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Futuristic" )) %>' Enabled="false" runat="server" />
				</itemtemplate>
				<edititemtemplate>
					<asp:CheckBox ID="chkFuturistic" Checked='<%# Convert.ToBoolean(DataBinder.Eval( Container.DataItem, "Futuristic" )) %>' runat="server" />
				</edititemtemplate>
			</asp:TemplateColumn>
			<asp:EditCommandColumn EditText="<img border=0 src=http://opr.carwale.com/images/edit.jpg />" CancelText="Cancel" UpdateText="Update" />
		<%--	<asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="<img border=0 src=http://opr.carwale.com/images/icons/delete.ico />" />--%>
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
            <asp:TemplateColumn HeaderText="Update Photo" ItemStyle-Width="350">
			    <itemtemplate>
			        <input type="button" value="Upload" onclick="javascript:window.open('versionphotos.aspx?model=<%# DataBinder.Eval( Container.DataItem, "ID" ) %>','','left=200,width=900,height=600,scrollbars=yes')" />
			    </itemtemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Synopsis" ItemStyle-Width="350">
			    <itemtemplate>
			        <input type="button" value="Add" onclick="javascript:window.open('bikesynopsis.aspx?model=<%# DataBinder.Eval( Container.DataItem, "ID" ) %>','','left=200,width=900,height=600,scrollbars=yes')" />
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
	<script type="text/javascript" language="javascript">

	    $(".deleteBike").click(function () {
	        if (!confirm("Do you really want to delete this model."))
	        {
	            return false;
	        }        
	    });

	    function cmbMakes_Change(e) {	        
	        var el = document.getElementById('cmbMakes');
	        var alter = false;
	        var rows = document.getElementById('dtgrdMembers').getElementsByTagName('tr');

	        for (var i = 1; i < rows.length; i++) {
	            var tds = rows[i].getElementsByTagName('td');	            
	            if (parseInt(tds[1].innerHTML) != el.value) {
	                rows[i].className = rows[i].className = 'doNotDisplay';
	            }
	            else {
	                rows[i].className = alter ? 'dtAlternateRow' : 'dtItem';
	                alter = alter ? false : true;
	            }
	        }
	    }

	   /* $("#editMaskingName").click(function (){
	        alert( $(this).parent().html());
	        $(this).replaceWith("");
    
	    })*/

	    if (document.getElementById('btnSave'))
	        document.getElementById('btnSave').onclick = btnSave_Click;

	    function btnSave_Click() 
	    {
	        $("#spntxtSeries").text("");
	        $("#spntxtName").text("");
	        $("#spntxtMaskingName").text("");
	        $("#spnSegment").text("");
	        var isError = false;
	        //alert($("#cmbMakes option:selected").val());
	        if ($("#cmbMakes option:selected").val() == '0') 
	        {
	            $("#spntxtMake").text(" Select Make ");
	            isError = true;
	        }
	        if ($("#ddlSeries option:selected").val() <= '0') 
	        {
	            $("#spntxtSeries").text(" Select Series ");
	            isError = true;
	        }
	        if($("#ddlSegment option:selected").val() <= 0)
	        {
	            $("#spnSegment").text(" Select Segment ");
	            isError = true;
	        }

	        if ($("#txtModel").val() == "") {
	            $("#spntxtName").text("Model Name Required ");
	            isError = true;
	        }
	        if ($("#txtMaskingName").val()=="")
	        {
	            $("#spntxtMaskingName").text("Model Masking Name Required");
	            isError=true;
	        }
	        if (validate()==false)
	            isError = true;
	        if (isError)
	            return false;
	    }

	    $('#txtModel').blur(function () {   
	        var model =jQuery.trim( $('#txtModel').val());
	        model = model.trim();
	        model = model.replace(/\s+/g, "-");
	        model = model.replace(/[^a-zA-Z0-9\-]+/g, '');
	        model = removeHyphens(model);
	        $('#txtMaskingName').val(model.toLowerCase());
	    });

	    function removeHyphens(str)
	    {
	       // if( str.indexOf("-")==0)
	         //   str=str.substr(1);//for removing starting hyphen...	        
	        str = str.replace(/(-)+/g, '-');//for replacing multiple consecutive hyphens with one hyphen...
	        str= str.replace(/^\-+|\-+$/g,'');
	        return str;
	    }

	    function validate()
	    {
	        var isValid = true;
	        var model = $('#txtModel').val();
	        var maskedmodel = $('#txtMaskingName').val();
	
	        if (jQuery.trim(model).length == 0)
	        {
	            $("#spntxtName").text("Model Name Required ");
	            isValid = false;
	        }
	        if (jQuery.trim(maskedmodel).length == 0)
	        {
	            $("#spntxtMaskingName").text("Model Masking Name Required");
	            isValid = false;
	        }
	        else if (hasSpecialCharacters(maskedmodel) == true)
	        {
	            //alert("Inside special character");
	            $('#spntxtMaskingName').text("Invalid Masking Name."); 
	            isValid = false;
	        }
            return isValid;
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
	            var modelId = $(this).attr("id").split("_")[1];	
	            var objOldMask = $(this).siblings();
	            var oldMaskingName = objOldMask.text();

	            objYes.click(function(){
	                boxObj.find("#divWarnMsg").hide();
	                boxObj.find("#txtUpdMaskName").val(oldMaskingName);
	                boxObj.find("#divUpdMaskName").show();
	            });

	            objNo.click(function(){
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

	                if (maskName == ""|| maskName == null)
	                {
	                    errMask.text("Required");
	                }
	                else if (hasSpecialCharacters(maskName) == true)
	                {
	                    errMask.text("Invalid Masking Name");
	                }
	                else
	                {
	                    maskName = maskName.trim();
	                    maskName = maskName.replace(/\s+/g, "-");
	                    maskName = removeHyphens(maskName);
	                    //$('#txtUpdMaskName').val(maskName);
                    
	                    $.ajax({
	                        type: "POST",
	                        url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
	                        data: '{"maskingName":"' + maskName + '","updatedBy":"'+ updBy +'","modelId":"' + modelId + '"}',
	                        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateModelMaskingName"); },
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

	    //$("#cmbMakes").change(function(){
	    //    var makeId=$("#cmbMakes").val();
	    //    alert(makeId);
	    //    FillSeries(makeId);
	    //});

	    $("#btnModel").click(function(){	        
	        $("#spnUpdateSeries").text("");

	        if ($("#ddlUpdateSeries").val() <= '0') 
	        {
	            $("#spnUpdateSeries").text("Select Series");
	        }
	        else
	        {
	            $("input[name=chkSeries]").removeAttr("disabled");
	            $("input[name=chkSeries]:first").focus();
	        }
	    });

	    $("#btnSelModel").click(function(){	        
	        $("#spnUpdateSeg").text("");

	        if ($("#ddlUpdateSegment").val() <= 0) 
	        {
	            
	            $("#spnUpdateSeg").text("Select Segment");
	            return false;
	        }
	        else
	        {
	            $("input[name=chkSegment]").removeAttr("disabled");
	            $("input[name=chkSegment]:first").focus();
	        }
	    });

	    function ValidateUpdateSeries(){
	        var ModelIdList = "";
	        var isError = false;
	        if($("input[name=chkSeries]:checked").length != 0)
	        {
	            $("input[name=chkSeries]:checked").each(function(){
	            
	                ModelIdList += $(this).attr("modelId") + ",";
	                $("#hdnModelIdList").val(ModelIdList);
	            });
	        }
	        else
	        {
	            alert("Select Models to Update Series");
	            $("input[name=chkSeries]:first-of-type").removeAttr("disabled");
	            isError=true;
	        }
	        return !isError;
	    }

	    $("#btnUpdateSegment").click(function(){
	        var ModelIdList = "";
	        var isError = false;
	        if($("input[name=chkSegment]:checked").length != 0)
	        {
	            $("input[name=chkSegment]:checked").each(function(){
	            
	                ModelIdList += $(this).attr("modelId") + ",";
	                $("#hdnModelIdsList").val(ModelIdList);
	            });
	        }
	        else
	        {
	            alert("Select Models to Update Segement");
	            $("input[name=chkSegment]:first-of-type").removeAttr("disabled");
	            isError=true;
	        }
	        return !isError;
	    });
	    //document.getElementById('cmbMakes').onchange = cmbMakes_Change;
	    //cmbMakes_Change();
	</script>

</div>

<!-- #Include file="/includes/footerNew.aspx" -->