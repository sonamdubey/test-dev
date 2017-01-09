<%@ Page trace="false" Inherits="BikeWaleOpr.Content.ModelColorWiseImage" AutoEventWireUp="false" EnableEventValidation="false" Language="C#" %>
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
</style>
<div class="left min-height600" id="divManagePrices">
    <h1>Model Images by Color</h1>
    <span id="spnError" class="error" runat="server"></span>
    <fieldset>
        <legend style="font-weight: bold">Select Model</legend>
        <asp:dropdownlist EnableViewstate="true" id="cmbMake" runat="server" tabindex="1" />
        <asp:dropdownlist id="cmbModel" runat="server" tabindex="2">
			<asp:ListItem Value="0" Text="--Select--" />
		</asp:dropdownlist>
        <input type="hidden" id="hdn_cmbModel" runat="server" />
        <asp:HiddenField ID="hdnModelId" runat="server" value="0" />
        <asp:button id="btnSubmit" text="Show Images" runat="server" tabindex="3" />
        <span class="error" id="selectModel"></span>
    </fieldset>
    <br>
    <% if(modelId > 0){ %>
    <div class="margin-top10 floatLeft" style="width: 850px; display: inline-block;">
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
                            <td></td>
                            <td></td>
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
                    <td><img src='<%= Bikewale.Utility.Image.GetPathToShowImages(color.OriginalImagePath,color.Host,Bikewale.Utility.ImageSize._144x81) %>' /></td>
                    <td data-isImageExists="<%= color.IsImageExists %>" data-modelId="<%=modelId %>" data-colorId="<%=color.Id %>" data-color="<%= color.Name %>">
                        <input type="file" name="fileUpload" id="fileUpload" accept="image/*" />
                        <%--<input data-id="<%=color.Id %>" name="uploadImage" type="button" class="padding10" value="Upload Image" />--%>
                    </td>
                    <td><input data-id="<%=color.Id %>" name="deleteImage" type="button" class="padding10 deleteImage" value="Delete Image" /></td>
                </tr>
             <% } %>
                        </tbody>
            </table>
                        <%-- <a href="javascript:openEditColorWindow(<%#DataBinder.Eval(Container.DataItem,"Id") %>, <%= modelId %>)" class="editBtn">Edit</a>
                        <a runat="server" id="lnkDelete" href="javascript:confirmDelete(<%#DataBinder.Eval(Container.DataItem,"Id") %>" class="editBtn">Delete</a>                                                
                        <asp:button id="btnDelete" text="Delete" runat="server" />--%>
        <% }
            else
            {
        %>
        <span>No content.</span>
        <% } %>
    </div>
    <% } %>
</div>
<script type="text/javascript">
    modelId = '<%= modelId %>';
    environment = '<%= ConfigurationManager.AppSettings["AWSEnvironment"] %>';
    hostUrl = '<%= Bikewale.Utility.BWOprConfiguration.Instance.BwHostUrlForJs %>';
    userid = '<%= BikeWaleOpr.Common.CurrentUser.Id %>';
</script>
<!-- #Include file="/includes/footerNew.aspx" -->