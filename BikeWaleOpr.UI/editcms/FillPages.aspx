<%@ Page AutoEventWireUp="false" Language="C#" Inherits="BikeWaleOpr.EditCms.FillPages" Trace="false" Debug="false" validateRequest="false" %>
<%@ Register TagPrefix="Uc" TagName="DispBasicInfo" src="/editcms/DisplayBasicInfo.ascx" %>
<%@ Register TagPrefix="Vspl" TagName="RTE" src="/Controls/RichTextEditor.ascx" %>
<%@ Register TagPrefix="Ec" TagName="EditCmsCommon" src="/editcms/EditCmsCommon.ascx" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<div class="urh">
	<a href="/default.aspx">BikeWale operations</a> &raquo; <a href="/editcms/default.aspx">Editorial Home</a> &raquo; Fill pages
</div>
<script language="javascript" src="/src/AjaxFunctions.js"></script>
<script language="javascript" type="text/javascript" src="editcmsjs.js"></script>
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $("#btnSave").click(function validate() {
            if ($("#lblPage").html() == '') {
                alert("Select Page");
                return false;
            }
            CleanHtml("rteDetails_txtContent");
            return true;
        });
    });    
</script>
<form id="Form1"  runat="server">
<div>
	<Ec:EditCmsCommon ID="EditCmsCommon" runat="server" />    
    <style type="text/css">
        #rteDetails_txtContent{ width:660px; height:624px;}        
    </style>   
</div>
<div>
	<div style="float:left;">
		<h1 style="padding-left:0px; display:none;">Fill Pages</h1>
		<table cellpadding="5" cellspacing="0">
			<tr>
				<td colspan="3"><asp:Label ID="lblMessage" EnableViewState="false" CssClass="lbl" runat="server"></asp:Label></td>
			</tr>			
			<tr>				
				<td><Vspl:RTE id="rteDetails" runat="server" />
							&nbsp;&nbsp;<span style="font-weight:bold;color:red;" id="spnDetails" class="error" /></td>
			</tr>
			<tr>
				<td colspan="3">
					<asp:Button ID="btnSave" CssClass="submit" Text="Save" runat="server" />
					<a href="addpages.aspx?bid=<%=basicId%>"><asp:Button ID="btnCancel" CssClass="submit" Text="Cancel" runat="server" /></a>
					<% if ( CategoryId == "1" ) { %>
					<input type="button" id="btnShowPreview" value="Preview" class="submit" onclick="ShowPreview(<%= basicId %>)" />
					<% } %>
					<input type="button" id="btnAddSpec" onclick="checkVersion()" runat="server" value="Add specifications chart" />					
				</td>
			</tr>
			<tr>
			<td colspan="3">
				<div id="divVersion" style="display:none;border:1 px solid #16E7A8;background-color:#E1FDF0;">
					<table width="50%" cellpadding="2" cellspacing="2">
						<tr>
							<td><asp:DropDownList ID="drpVersion" CssClass="drpClass" runat="server" Enabled="false">
							<asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
							</asp:DropDownList>
							<input type="hidden" id="hdn_drpVersion" runat="server" /></td>
						</tr>
						<tr>
							<td><input type="button" id="btnAddVersion" onclick="javascript:addVersion()" value="Add version" /></td>
						</tr>
					</table>
				</div>
			</td>
			</tr>
		</table>
	</div>
	<div style="float:left;margin-top:15px; margin-left:-6px;">
		<iframe src="showalbum.aspx?bid=<%= basicId%>" style="border-width:1px; border-style:solid; border-color:#C0C0C0;padding-left:12px;" scrolling="auto" height="630" width="210" frameborder="0"></iframe>
	</div>

	<div id="div" style="display:none">
		<div id="rtspecs">
			<h1>Test Data</h1>
			<h2>Engine Specifications</h2>
			Add engine specifications <span id="spnVersion"></span>
			<table class="testdata" cellspacing="5" width="100%">
				<tr>
					<td valign="top">
						<h2>Speedo Error</h2>
						<table border="1" cellspacing="0">
							<tr>
								<th width="125">Speedo Reading (kph)</th>
								<th width="100">Actual Speed (kph)</th>
							</tr>
							<tr>	
								<td>40</td>
								<td>&nbsp;</td>
			
							</tr>	
							<tr>
								<td>60</td>
								<td>&nbsp;</td>
							</tr>
							<tr>
								<td>80</td>	
								<td>&nbsp;</td>
			
							</tr>
							<tr>								
								<td>100</td>
								<td>&nbsp;</td>
							</tr>
							<tr>	
								<td>120</td>
								<td>&nbsp;</td>
			
							</tr>
							<tr>
								<td>140</td>	
								<td>&nbsp;</td>
							</tr>
						</table>		
					</td>
					<td valign="top">
						<h2>Max in Gear</h2>
			
						<table border="1" cellspacing="0">
							<tr>
								<th>Gear</th>
								<th width="130">Speed (kph)</th>
							</tr>
							<tr>
								<td>1<sup>st</sup></td>
			
								<td>&nbsp;</td>								
							</tr>
							<tr>
								<td>2<sup>nd</sup></td>
								<td>&nbsp;</td>								
							</tr>
							<tr>
								<td>3<sup>rd</sup></td>
			
								<td>&nbsp;</td>
							</tr>
							<tr>	
								<td>4<sup>th</sup></td>
								<td>&nbsp;</td>								
							</tr>
							<tr>
								<td>5<sup>th</sup></td>								
								<td>&nbsp;</td>
			
							</tr>
							<tr>
								<td>6<sup>th</sup></td>
								<td>-</td>
							</tr>
						</table>
					</td>
			
					<td valign="top">
						<h2>Performance Test Data</h2>
						<table border="1" cellspacing="0">
							<tr>
								<th width="155">Top Speed</th>
								<td>&nbsp;</td>
							</tr>
							<tr>
								<th>0-60kph</th>
								<td>&nbsp;</td>
							</tr>
							<tr>
								<th>0-100kph</th>
								<td>&nbsp;</td>
							</tr>
							<tr>
								<th>Quarter Mile (402m)</th>
								<td>&nbsp;</td>
							</tr>
							<tr>
								<th>Braking 80-0kph</th>
								<td>&nbsp;</td>
							</tr>
							<tr>
								<th>30-50kph in 3rd</th>
								<td>&nbsp;</td>
							</tr>
							<tr>
			
								<th>30-50kph in 4th</th>
								<td>&nbsp;</td>
							</tr>
							<tr>
								<th>50-70kph in 5th</th>
								<td>&nbsp;</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan="3">
						<h2>Fuel Efficiency</h2>
						<table border="1" cellspacing="0" width="100%">
							<tr>
			
								<th>&nbsp;</th>
								<th width="18%">City</th>
								<th width="18%">Highway</th>
								<th width="18%">Overall</th>
								<th width="18%">Worst</th>
							</tr>
							<tr>
			
								<th>Mileage (kpl)</th>
								<td>&nbsp;</td>
								<td>&nbsp;</td>
								<td>&nbsp;</td>
								<td>&nbsp;</td>					
							</tr>
						</table>
			
					</td>
				</tr>
			</table>
		</div>
	</div>
</div>
</form>

<script type="text/javascript" language="javascript">

    var selected_text;
    var versionId = '<%=versionId%>';
    function addVersion() {
        if (document.getElementById("drpVersion").value != "0") {
            var strVersion = document.getElementById("drpVersion").value;
            checkVersion(strVersion);
        }
        else
            alert("Please select version");
    }
	
	
	function ShowPreview( basicId )
	{
		var rowsReturned = '<%= RowsReturned %>'		
		rowsReturned = rowsReturned.toLowerCase();
		
		if( rowsReturned == 'true' )
		{
			window.open ("http://webserver/news/preview/"+basicId+"-<%= ArticleUrl %>.html","Preview of "+"<%= ArticleTitle %>","scrollbars=1,width=800,height=600");
		}
	}

	

	function checkVersion()
	{
		var str1 = versionId;

		document.getElementById('spnVersion').innerHTML = "";
		if (str1 == "" || str1 == "0" || str1 == "-1")
		{
			alert("Please Select Version");
			if(document.getElementById('divVersion').style.display == 'none')
			{
				document.getElementById('divVersion').style.display = 'block';
			}
		}
		else
		{
			var VersionId = str1;
			var w = document.Form1.drpVersion.selectedIndex;
			selected_text = document.Form1.drpVersion.options[w].text;
			
			var response = AjaxFunctions.GetSpecificationsUrl("<%= makeName%>", "<%= modelName%>", selected_text, VersionId);
			if (response.error != null)
			{
				alert("ERROR : " + response.error);
				return;
			}

			var strUrl = "http://www.carwale.com/research/" + response.value;
			
			document.getElementById('spnVersion').innerHTML = "<a href=" + strUrl + ">View specifications</a>";
			var conSpec = document.getElementById('div').innerHTML;
			
			tinyMCE.execCommand('mceInsertContent',false,conSpec);
		
			document.getElementById('divVersion').style.display = 'none';
			document.getElementById("drpVersion").value = "0";
		}
	}
			
 </script>
<!-- #Include file="/includes/footerNew.aspx" -->
