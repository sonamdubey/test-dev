<%@ Page trace="false" Inherits="BikeWaleOpr.Content.ModelColorWiseImage" AutoEventWireUp="false" EnableEventValidation="false" Language="C#" %>
<%@ Import Namespace ="System.Linq" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<div>
    <!-- #Include file="contentsMenu.aspx" -->
</div>
<script language="javascript" src="/src/AjaxFunctions.js"></script>
<script language="javascript" src="/src/modelImagesByColor.js"></script>
<script language="javascript" src="/src/imageUpload.js"></script>
<style>
    #one {
        width: 50px;
        height: 50px;
        border: 1px solid #ccc;
        margin: 0 auto 10px;
    }
    td.color{
        border-right: 0;
        border-top: 0;
    }
    .progress-bar {
        width: 0;
        display: none;
        height: 3px;
        background: #16A085;
        bottom: 0px;
        left: 0;
        border-radius: 2px;
    }

    .position-abt {
        position: absolute;
    }
    .position-rel{
        position: relative;
    }
</style>
<div class="left min-height600" id="divManagePrices">
    <h1>Model Images by Color</h1>
    <span id="spnError" class="error" runat="server"></span>
    <fieldset id="inputSection" class="position-rel">
        <legend style="font-weight: bold">Select Model</legend>
        <asp:dropdownlist EnableViewstate="true" id="cmbMake" runat="server" tabindex="1" />
        <asp:dropdownlist id="cmbModel" runat="server" tabindex="2">
			<asp:ListItem Value="0" Text="--Select--" />
		</asp:dropdownlist>
        <input type="hidden" id="hdn_cmbModel" runat="server" />
        <asp:HiddenField ID="hdnModelId" runat="server" value="0" />
        <asp:button id="btnSubmit" text="Show Images" runat="server" tabindex="3" />
        <span class="error" id="selectModel"></span>
        <span class="position-abt progress-bar" style="width: 100%; overflow: hidden;"></span>
    </fieldset>
    <br>
    <% if(modelId > 0){ %>
    <div  class="margin-top10 floatLeft" style="width: 850px; display: inline-block;">
         <%
             if (modelColorCount > 0)
            {
        %>
            
            <table class="table-bordered" cellspacing="0" cellpadding="5">
					<tbody>
                        <tr>
                            <td>Color Name</td>
                            <td>Color</td>
                            <td>Image</td>
                            <td>Upload Image</td>
                            <td class="delcolumn <%= !modelColors.Any(m=>m.IsImageExists) ? "hide" :"" %>">Delete Images</td>
                        </tr>
                <% foreach(var color in modelColors){ %>
                <tr>
                    <td><% = color.Name%></td>
                    <td>
                        <table border="0" id="one" cellspacing="0">
                            <% foreach(var hexColor in color.ColorCodes) {  %>
                             <tr style='background:#<%= hexColor.HexCode %>'>
                                 <td class="color"></td>
                             </tr>
                            <% } %>
                        </table>
                    </td>
                    <td><img id="mainImage" src='<%= Bikewale.Utility.Image.GetPathToShowImages(color.OriginalImagePath,color.Host,Bikewale.Utility.ImageSize._144x81) %>' /></td>
                    <td data-isImageExists="<%= color.IsImageExists %>" data-modelId="<%=modelId %>" data-colorId="<%=color.Id %>" data-color="<%= color.Name %>">
                        <img id="preview" src="" />
                        <input type="file" name="fileUpload" id="fileUpload" accept="image/*" style="width:75px;"/>
                        <input type="button" class="padding10 uploadImage" value="Upload" />
                        
                    </td>
                    <td class="delcolumn <%= !modelColors.Any(m=>m.IsImageExists) ? "hide" :"" %>">
                        <input data-id="<%=color.BikeModelColorId %>" style="<%= (color.IsImageExists && !string.IsNullOrEmpty(color.BikeModelColorId))? "" : "display:none" %>" type="button" class=" padding10 deleteImage" value="Delete" />
                    </td>
                </tr>
             <% } %>
                        </tbody>
            </table>
        <% }
            else
            {
        %>
        <span>No content.</span>
        <% } %>
    </div>
    <% } %>
</div>
<div class='toast' style='display:none'>I did something!</div>
<script type="text/javascript">
    modelId = '<%= modelId %>';
    environment = '<%= ConfigurationManager.AppSettings["AWSEnvironment"] %>';
    bwHostUrl = '<%= Bikewale.Utility.BWOprConfiguration.Instance.BwHostUrlForJs %>';
    userid = '<%= BikeWaleOpr.Common.CurrentUser.Id %>';
</script>
<!-- #Include file="/includes/footerNew.aspx" -->