<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.content.UnitSoldUpload" %>
<%@ Register TagPrefix="Vspl" TagName="Calendar" Src="/controls/DateControl.ascx" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<script type="text/javascript" src="/src/common/common.js?V1.1"></script>
<script src="/src/bt.js"></script>

<div>
    <!-- #Include file="contentsMenu.aspx" -->
</div>

<style type="text/css">          
	       .successMessage { text-align:center;color:#008000; }
    </style>
<script language="javascript" src="/src/AjaxFunctions.js"></script>

     <div class="floatLeft" style="width: 450px; margin-left:50px;">
            <fieldset>
                <legend>Bulk upload unit sold</legend>
                <div>
                    <div class="margin10">
                        <p>For date</p>
                        <Vspl:Calendar DateId="calFrom" id="calFrom" runat="server" FutureTolerance="2" />
                        <br /><br />
                        <p>Upload last month unit sold file</p>
                        <span id="spnFile" class="errorMessage" runat="server"> </span>                        
                        <input type="file" id="flUpload" runat="server" required/>
                        <asp:button id="btnUploadFile" runat="server" text="Upload File"></asp:button>                                               
                    </div>
                    <asp:Label ID="lblMessage" Visible="false" CssClass="successMessage" runat="server">Updation Successfull</asp:Label>
                </div>
            </fieldset>
        </div>

<!-- #Include file="/includes/footerNew.aspx" -->