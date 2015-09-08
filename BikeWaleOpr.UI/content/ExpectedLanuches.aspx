<%@ Page Language="C#" Inherits="BikeWaleOpr.Content.ExpectedLaunches" AutoEventWireUp="false" Trace="false" Debug="false" %>
<%@ Register TagPrefix="Vspl" TagName="Calendar" Src="/Controls/DateControl.ascx" %>

<!-- #Include file="/includes/headerNew.aspx" -->
<style>
	.dvRed{color: #FF3300; font-weight:bold;}
    .alignCenter {text-align:center;}
</style>
<div class="urh">
	You are here &raquo; <a href="/content/default.aspx">Contents</a> &raquo; Expected Bike Launches
</div>
<!-- #Include file="ContentsMenu.aspx" -->
<script type="text/javascript" language="javascript" src="/src/AjaxFunctions.js"></script>
<script type="text/javascript" language="javascript" src="/src/PopUpDiv.js"></script>

<div class="left">
	<h3>Expected Bike Launches</h3>
    <br />
	<span id="spnError" class="dvRed" runat="server"></span>
	<asp:DataGrid ID="dtgrdLaunches" runat="server" 
			DataKeyField="ID" 
			CellPadding="5" 
			BorderWidth="1" 
			width="100%"
			AllowPaging="true"
			AllowSorting="true" 
            PageSize="20"
            PagerStyle-Mode="NumericPages"
			AutoGenerateColumns="false">
		<itemstyle CssClass="dtItem"></itemstyle>
		<headerstyle CssClass="dtHeader"></headerstyle>
		<alternatingitemstyle CssClass="dtAlternateRow"></alternatingitemstyle>
		<edititemstyle CssClass="dtEditItem"></edititemstyle>
		<columns>
            <asp:TemplateColumn HeaderText="SNo">
                <itemtemplate> 
                    <%= ++serialNo%> 
                    <asp:CheckBox ID="chkLaunched" runat="server" Idvalue='' ></asp:CheckBox>
                    <asp:Label ID="lblId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Id") %>' style="display:none;"></asp:Label>
                    <asp:Label ID="lblModelId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BikeModelId") %>' style="display:none;"></asp:Label>            
                </itemtemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Expected Launch Date" >
                <itemtemplate> 
                    <%# DataBinder.Eval( Container.DataItem, "LaunchDate").ToString() != String.Empty ? Convert.ToDateTime( DataBinder.Eval( Container.DataItem, "LaunchDate")).ToString("dd-MMM-yyyy hh:mm tt") : "" %>
                </itemtemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Bike Name" >
                <itemtemplate> 
                    <asp:Label runat="server" ID="lblCName" Text='<%# DataBinder.Eval( Container.DataItem, "BikeName" ) %>'></asp:Label>
                </itemtemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn HeaderText="Expected Launch" >
                <itemtemplate> 
                    <asp:Label runat="server" ID="lblELaunch" Text='<%# DataBinder.Eval( Container.DataItem, "ExpectedLaunch" ) %>'></asp:Label>
                </itemtemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn HeaderText="Estimated Min Price" >
                <itemtemplate> 
                    <asp:Label runat="server" ID="lblEPMin" Text='<%# DataBinder.Eval( Container.DataItem, "EstimatedPriceMin" ) %>'></asp:Label>
                </itemtemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn HeaderText="Estimated Max Price" >
                <itemtemplate> 
                    <asp:Label runat="server" ID="lblEPMax" Text='<%# DataBinder.Eval( Container.DataItem, "EstimatedPriceMax" ) %>'></asp:Label>
                </itemtemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <itemtemplate>
                     <div class="alignCenter">
                          <a title="Click to edit" onClick='FillDetails("UpdateExpLaunches.aspx","<%# DataBinder.Eval(Container.DataItem, "Id") %>")' style="cursor:pointer;">Edit</a>
                     </div>
                </itemtemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>                 
                <itemtemplate>
                    <div class="alignCenter">
                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="http://opr.carwale.com/images/icons/delete.ico" CommandName="Delete" class="deleteBike"/>
                    </div>
                </itemtemplate>
            </asp:TemplateColumn>
        </columns>
	</asp:DataGrid>
    <asp:Button id="btnSave" Text="Mark As Launched" runat="server"  />	  
</form>
</div>


<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        $(".deleteBike").click(function () {
            if (!confirm("Do you really want to delete this bike."))
            {
                return false;
            }        
        });
    });
    function FillDetails(pageName, Id)
     {

         var leftPos = (screen.width) / 4;
         var topPos = (screen.height) / 3;

         newwindow = window.open(pageName + '?Id=' + Id, 'MyWindow', 'location=no,menubar=no,width=700,height=600,left=' + leftPos + ',top=' + topPos + ',scrollbars=yes');

         // Focus/Maximise window if its minimised
         if (window.focus) {
             newwindow.focus();
         }
     }

   /* function UpdateData()
     {
         var minPrice = document.getElementById("txtEstMinPri").value;
         var maxPrice = document.getElementById("txtEstMaxPri").value;
         var expLaunch = document.getElementById("txtExpLaunch").value;
      
         var hr = $("#ddlHour :selected").val();
         var min = $("#ddlMinutes :selected").val();


         var nextLaunchTime = $("#calFrom_txtYear").val() + "-" + $("#calFrom_cmbMonth").val() + "-" + $("#calFrom_cmbDay").val() + "-" + $("#ddlHour :selected").val() + "-" + $("#ddlMinutes :selected").val();
        
         if (validate())
          	UpdateLaunchDetails(document.getElementById("spnId").innerHTML, minPrice, maxPrice, expLaunch, nextLaunchTime)
    }

    function UpdateLaunchDetails(Id, minPrice, maxPrice, expLaunch, newLaunch)
     {
        filSmall = document.getElementById("filSmall");
        filLarge = document.getElementById("filLarge");
        var modelId = '';
             
        if (filLarge.value != "" && filSmall.value != "") {
            modelId = document.getElementById("spnModelId").innerHTML;
        }

        var updatedId = AjaxFunctions.UpdateLaunchDet(Id, minPrice, maxPrice, expLaunch, newLaunch, modelId);

        if (updatedId) {
            if (filLarge.value != "" && filSmall.value != "")
			{
				alert("updating image" + filSmall.value + "-" + filLarge.value);
                AjaxFunctions.SavePhoto(filSmall.value, filLarge.value, modelId);
			}

            $('#spnMessage').text('Record Updated Successfully.');
        }
        else {
            $('#spnMessage').text('Unable to update');
        }
    }*/

</script>
<!-- #Include file="/includes/footerNew.aspx" -->
