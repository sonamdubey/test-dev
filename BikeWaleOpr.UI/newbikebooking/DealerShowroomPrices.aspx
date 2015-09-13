<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.NewBikeBooking.DealerShowroomPrices" Trace="false" Async="true" EnableEventValidation="false" %>

<link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
<script src="/src/jquery-1.6.min.js" type="text/javascript"></script>
<script src="/src/AjaxFunctions.js" type="text/javascript"></script>
<style>
    #prices {border-collapse: collapse;border-color: #cccccc;}
    #prices td {text-align: center;}
    #prices th {padding: 10px;background-color: #FF9148;}
    #prices input {background-color: #f3f3f3;border: 1px solid #dddddd;}
    .met {background-color: #FFFF66;}
    .row_error { background-color:red; color:#fff; }
    .red { color:red;}
</style>
<form runat="server">
<div>
    <h1>Add Showroom Prices</h1>
        <div style="background-color: #f4f3f3; padding: 10px; margin-top: 10px;">
            Make-Model-City                    
            <asp:dropdownlist id="cmbMake" runat="server" tabindex="1"></asp:dropdownlist>
            <asp:dropdownlist id="cmbModel" runat="server" tabindex="2">
				<asp:ListItem Value="0" Text="--Select--" />
			</asp:dropdownlist>
            <asp:HiddenField  ID="hdnCmbModel" runat="server" />
            <asp:DropDownList ID="ddlStates" runat="server" tabindex="3"/>
            <asp:dropdownlist id="drpCity" runat="server" tabindex="4">
                <asp:ListItem Value="0" Text="--Select--" />
            </asp:dropdownlist>
            <input type="hidden" id="hdn_drpCityName" runat="server" />
            <asp:button id="btnShow" text="Show" runat="server" tabindex="5"></asp:button>

            <span id="selectMake" class="red"></span>
            <span id="selectModel" class="red"></span>
            <span id="selectCity" class="red"></span>
        </div>

       <%-- category Items Binding--%>
        <div>
             <asp:Repeater Id="rptcatItem" runat="server">
                    <HeaderTemplate>
                        <div style="background-color: #f4f3f3; padding: 10px; margin-top: 10px;min-height:110px;"  class="padding10">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="floatLeft padding5" style="width:140px;">
                            <input type="checkbox" runat="server" categoryid='<%# Eval("CategoryId")%>' id="chkCat"/>
                            <label for="chk_<%# DataBinder.Eval(Container.DataItem, "CategoryId") %>"><%# DataBinder.Eval(Container.DataItem, "CategoryName") %></label>                        
                        </div>
                    </ItemTemplate>
                 <FooterTemplate>      
                        </div>
                 </FooterTemplate>
             </asp:Repeater>
             <div class="clear"></div>
             <asp:button id="btnAddCat" text="Add to Price Sheet" runat="server"></asp:button>
        </div>
        <div class="margin10">
            <asp:Repeater id="rptVersions" runat="server"> 
                    <HeaderTemplate>
                        <table border="1" cellpaddng="5" cellspacing="0" id="prices" style="font-size :12px !important">   
                        <tr>
                            <th style="width:40px;"><input type="checkbox" id="checkAllNone" /></th>
                            <th style="width:200px;">Version Name</th>
                            <th style="width:150px;">UpdatedBefore(In Days)</th>
                            <asp:Repeater Id="rptHeader" runat="server" DataSource="<%# GetPQCommonAttrs() %>">
                                <ItemTemplate>
                                       <th> <%# Eval("ItemName") %></th>            
                                </ItemTemplate>
                            </asp:Repeater>
                       </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                         <tr id='<%#DataBinder.Eval(Container.DataItem, "VersionId")%>'>
                              <td>
                                <asp:Label style="display:none;" id="lblVersionId" Text='<%# DataBinder.Eval( Container.DataItem, "VersionId" ) %>' runat="server"></asp:Label>
                                <input type="checkbox" versionid='<%#Eval("VersionId") %>' class="checkbox" runat="server" id="chkUpdate" />
                              </td>
                          <td><%# Eval("VersionName") %></td>
                          <td class="days"><%# Eval("UpdatedBeforeDays") %></td>
                          <asp:Repeater ID="rptValues" DataSource="<%# GetPQCommonAttrs() %>"  runat="server" >
                            <ItemTemplate>
                                  <td style="width:90px">
                                      <asp:Label style="display:none;" id="lblCategoryId" Text='<%# DataBinder.Eval( Container.DataItem, "ItemCategoryId" ) %>' runat="server"></asp:Label>
                                      <asp:Textbox ID="txtValue" class="met" style="width:60px;" MaxLength="9" Text='<%# GetItemValue(DataBinder.Eval(((RepeaterItem)Container.Parent.Parent).DataItem,"VersionId").ToString(), Eval("ItemCategoryId").ToString()) %>' runat="server" categoryid='<%# DataBinder.Eval( Container.DataItem, "ItemCategoryId" ) %>'></asp:Textbox>
                                      <span class="spnValueError"></span>
                                  </td>
                            </ItemTemplate>
                        </asp:Repeater>

                         <%-- <asp:Repeater id="rptPrice" runat="server" DataSource='<%# GetCategoryValues(Convert.ToUInt32(Eval("Version.VersionId"))) %>'>
                                <ItemTemplate>
                                    <td> <input type="text" value='<%# Eval("Price").ToString() %>' /></td>
                                </ItemTemplate>
                            </asp:Repeater>--%>
                        </tr>
                    </ItemTemplate>
                <FooterTemplate>
                     </table>
                </FooterTemplate>         
            </asp:Repeater>
        </div>
        <br />
        <div id="ModifyPrices" align="left">
            <asp:button id="btnSave" text="Save Prices" runat="server" />
            <asp:button id="btnRemove" text="Remove Price" runat="server"></asp:button>
        </div>
        <asp:HiddenField Id="hdnSelectedCityId" runat="server" />
</form>
</div>
 <script language="javascript">

     $(document).ready(function () {

       
         $("#btnShow").click(function () {
                 if (checkFind())
                     return true;
                 else return false;
         });

         $('#btnAddCat').click(function () {
             if (checkFind()) {
                 return true;
             }
             else {
                 return false;
             }
         });

         $('#btnSave').click(function () {
             
             if (isValidatePriceSheet()) {
                 return true;
             }
             else {
                 //alert("Please select version prices to be saved or updated.");
                 return false;
             }
         });

         //validatePrices

         $('#btnRemove').click(function () {
             if (checkFind() && validatePrices() > 0) {
                 return confirm("Do you really want to remove prices of all selected verisons for selected city?");
             }
             else {
                 alert("Please select versions to be remove prices.");
                 return false;
             }
         });


         $("#cmbModel").change(function () {
             $("#hdnCmbModel").val($(this).val());
         });

         $("#ddlStates").click(function () {
             if ($(this).val() > 0)
                 $("#drpCity").prop("disabled", false);
         });

         $("#ddlStates").change(function () {

             var requestType = "ALL";
             var stateId = $(this).val();

             $("#hdnSelectedCityId").val("0");

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
                         bindDropDownList(resObj, $("#drpCity"), "hdn_ddlCities", dependentCmbs, "--Select City--");
                     }
                 });
             } else {
                 $("#drpCity").val("0").attr("disabled", true);
             }
         });

         $("#drpCity").change(function () {
             if ($("#ddlStates").val() > 0) {
                 $("#hdnSelectedCityId").val($(this).val());
             }
             else {
                 $("#hdnSelectedCityId").val("0");
             }

             //alert($("#hdnSelectedCityId").val());
             //alert($("#hdnSelectedCityId").val());
         });

         qryStrMake = '<%=qryStrMake%>';
         qryStrModel = '<%=qryStrModel%>';
         qryStrCity = '<%=qryStrCity%>';

         make = document.getElementById('cmbMake');
         model = document.getElementById('cmbModel');
         city = document.getElementById('drpCity');

         <% if ( IsPostBack ) 
	{ %>
         qryStrMake = '<%=cmbMake.SelectedValue%>';
         qryStrModel = '<%=Request.Form["cmbModel"]%>';
         qryStrCity = '<%=drpCity.SelectedValue%>'
         <%}%>

         for (var i = 0; i < make.options.length; i++) {
             if (make.options[i].value == qryStrMake) make.options[i].selected = true;
         }
         cmbMake_OnChange();
         for (var i = 0; i < model.options.length; i++) {
             if (model.options[i].value == qryStrModel) model.options[i].selected = true;
         }

         for (var i = 0; i < city.options.length; i++) {
             if (city.options[i].value == qryStrCity) city.options[i].selected = true;
         }

         $("#checkAllNone").click(function () {
             //alert($(this).html());
             if ($(this).is(':checked')) {
                 $("input:checkbox.checkbox").attr("checked", "checked");
             } else {
                 $("input:checkbox.checkbox").removeAttr("checked");
             }
         });

         function checkFind(e) {
             if (document.getElementById('cmbModel').options[0].selected) {
                 document.getElementById('selectModel').innerHTML = "Select Model First";
                 document.getElementById('cmbModel').focus();
                 return false;
             }
             else document.getElementById('selectModel').innerHTML = "";

             //alert($("#drpCity").val());
             if ($("#drpCity").val() <= 0) {
                 $("#spnCity").text("Select City");
                 return false;
             }
             else
                 $("#spnCity").text("");

             if ($("#ddlStates").val() <= 0) {
                 $("#spnCity").text("Select state");
                 return false;
             }
             else
                 $("#spnCity").text("");

             return true;
         }


         function checkFind() {
             if (document.getElementById('cmbMake').options[0].selected) {
                 document.getElementById('selectMake').innerHTML = "Please Select Make";
                 document.getElementById('cmbMake').focus();
                 return false;
             }
             else document.getElementById('selectMake').innerHTML = "";

             if (document.getElementById('cmbModel').options[0].selected) {
                 document.getElementById('selectModel').innerHTML = "Please Select Model";
                 document.getElementById('cmbModel').focus();
                 return false;
             }
             else document.getElementById('selectModel').innerHTML = "";


             if (document.getElementById('drpCity').options[0].selected) {
                 document.getElementById('selectCity').innerHTML = "Please Select City";
                 document.getElementById('drpCity').focus();
                 return false;
             }
             else document.getElementById('selectCity').innerHTML = "";
             return true;
         }

         function isValidatePriceSheet() {
             var isValid = true;
             var CheckBoxChecked = false;
             var re = /^[0-9]*$/;

             $("#prices tr").each(function () {
                 if ($(this).find("input:checkbox.checkbox").is(":checked")) {
                     CheckBoxChecked = true;
                     $(this).find("input.met").each(function () {
                         var isNotValidPrice = ($(this).val() != "" && !re.test($(this).val()));
                         if (isNotValidPrice) {
                             isValid = false;
                             $(this).next('.spnValueError').html("Input is not in correct format").addClass('red');
                         
                         }
                     });
                 }
             });

             if (!CheckBoxChecked) {
                 alert("Please Check corresponding checkbox to save the prices");
                 isValid = false;
             }
           
             return isValid;
         }

         function validatePrices() {
             var showroomvalue = 0;
             $('#prices tbody tr').each(function () {
                 if ($(this).find("input:checkbox.checkbox").is(":checked")) {
                     showroomvalue = showroomvalue + 1;
                 }
             });
             return showroomvalue;
         }



     });
</script>

