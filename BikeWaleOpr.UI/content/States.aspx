<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikeWaleOpr.Content.States" trace="false" debug="false" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<script type="text/javascript" src="/src/common/common.js?V1.1"></script>
<script type="text/javascript" src="../src/graybox.js"></script>
<div class="urh">
		You are here &raquo; Contents &raquo; Add States
</div>
<div>
    <!-- #Include file="contentsMenu.aspx" -->
</div>
<style>
    .rptrTable {width:700px;border: 1px solid black;border-collapse: collapse;margin-top:20px;}
    .rptrTable td {padding : 5px;border: 1px solid black;}
    .rptrTable th{background-color:#E5E5E5;font-weight:bold; text-align:center;border: 1px solid black;}
    .rptrTable a {text-decoration:underline;cursor:pointer;}
</style>
<div class="left">
        <div>
            <fieldset>
                <legend>Add New States</legend>
                <div>
                    <div  class="margin10">
                        <div  class="floatLeft   inputWidth">State Name : </div>
                        <div class="floatLeft inputWidth"><asp:TextBox ID="txtState" MaxLength="30" runat="server" /></div>
                        <div class="margin-left10 floatLeft"><span class="errorMessage" id="spntxtName"></span></div>
                        <div class="clear"></div>
                    </div>
                   
                    <div class="margin10">
                        <div  class="floatLeft inputWidth">State Code :</div>
                        <div  class="floatLeft inputWidth"><asp:TextBox ID="txtStdCode" MaxLength="2"  runat="server"  /></div>
                            <div class="margin-left10 floatLeft"><span class="errorMessage" id="spnStdCode"></span></div>
                            <div class="clear"></div>
                        <div  class="margin-top10" >
                            <span class="greenMsg">[State code should be 2 character only]</span>
                        </div>    
                    </div>
                   
                    <div class="margin10">
                        <div  class="floatLeft  inputWidth"> Masking Name :</div>
                        <div  class="floatLeft inputWidth"><asp:TextBox ID="txtMaskingName" MaxLength="40"  runat="server"  /></div>
                        <div  class="margin-left10 floatLeft">
                            <span class="errorMessage" id="spntxtMaskingName"></span>
                        </div>
                        <div class="clear"></div>
                        <div  class="floatLeft margin-top10" >
                            <span class="greenMsg">[Masking Name will be used for url formation.Only lowercase letters,- and digits are allowed.]</span>
                        </div>   
                        <div class="clear"></div>  
                    </div>
                  
                    <div class="margin-top10 margin-left10 floatLeft">
		            <asp:Button ID="btnSave" Text="Add State" runat="server" />
                    </div>        
            </div>
            </fieldset>
        </div>
        <div>
            <asp:repeater runat="server" id="rptStates">
                <HeaderTemplate >
                    <table class="rptrTable">
                        <tr>
                            <th>Sr.No.</th>
                            <th>Name</th>
                            <th>MaskingName</th>
                            <th>StateCode</th>
                            <th>Edit</th>
                            <th>Delete</th>
                        </tr>
                </HeaderTemplate>
                <itemtemplate>  
                    <tr id="row_<%#Eval("ID")%>">
                        <td><%#Container.ItemIndex+1 %></td>
                        <td class="state"><%#Eval("Name") %></td>
                        <td class="maskingname"><%#Eval("MaskingName") %></td>
                        <td class="stdcode"><%#Eval("StateCode") %></td>
                        <td class="edit"><a id="edit_<%#Eval("ID")%>">Edit</a></td>
                        <td class="delete"><a id="delete_<%#Eval("ID")%>">Delete</a></td>
                    </tr>                    
                </itemtemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:repeater>       
         </div>

   <script type="text/javascript">
       $(document).ready(function () {

           $('#btnSave').click(function () {

               $("#spntxtMaskingName").text("");
               $("#spntxtName").text("");
               $("#spnStdCode").text("");

               var isError = false;
               var state = $("#txtState").val();
               var newReg = new RegExp('^[a-zA-Z& ]+$');
               var codeReg = new RegExp('^[a-zA-Z]+$');
               var stateCode = $("#txtStdCode").val();

               if (state == "") {
                   $("#spntxtName").text("Enter State");
                   isError = true;
               }
               else if (!newReg.test(state)){
                   $("#spntxtName").text("It shosuld be characters only.");
                   isError = true;
               }

               $("#spnStdCode").text("");
               if (stateCode == "") {
                   $("#spnStdCode").text("Enter State Code");
                   isError = true;
               }
               else if (!codeReg.test(stateCode))
               {
                   $("#spnStdCode").text("It should be characters only");
                   isError = true;
               }

               if ($("#txtMaskingName").val() == "") {
                   $("#spntxtMaskingName").text("Masking Name Required.");
                   isError = true;
               }
               else if (hasSpecialCharacters($("#txtMaskingName").val())) {
                   $("#spntxtMaskingName").text("Invalid Masking Name. ");
                   isError = true;
               }
               if (isError)
                   return false;
           })

           $("a[id^='delete_']").click(function () {
               var stateId = $(this).attr('id').split('_')[1];
               var stateName = $(this).parents().find(".state").html();

               if (confirm("Are you sure want to delete this state?")) {
                   $.ajax({
                       type: "POST",
                       url: "/ajaxpro/BikeWaleOpr.Common.AjaxCommon,BikewaleOpr.ashx",
                       data: '{"stateId":"' + stateId + '"}',
                       beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "DeleteState"); },
                       success: function (response) {
                           $("#row_" + stateId).html("<td colspan='6'>" + stateName + "  state has been deleted</td>").addClass("orange");
                       }
                   });
               }
           })

           $("a[id^='edit_']").click(function () {            
               var comment = "";
               var caption = "Update State Details";
               var stateId = $(this).attr('id');
               StateId = stateId.split('_')[1];
               var url = "/content/UpdateStateDetails.aspx?id=" + StateId;
               var applyIframe = true;              
               var GB_Html = ""

               GB_show(caption, url, 200, 520, applyIframe, GB_Html);
           });

           $('#txtState').blur(function () {
               var state = jQuery.trim($('#txtState').val());
               state = state.trim();
               state = state.replace(/\s+/g, "-");
               state = state.replace(/[^a-zA-Z0-9\-]+/g, '');
               state = removeHyphens(state);
               $('#txtMaskingName').val(state.toLowerCase());
           });
    })
   </script>
  
 </div>
<!-- #Include file="/includes/footerNew.aspx" -->
