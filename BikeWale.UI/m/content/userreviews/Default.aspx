<%@ Page Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Content.Default" Trace="false" %>
<%     
    title 			= "User Reviews on Bikes in India";
	description 	= "Know what users are saying about the bike you aspire to buy. Read first hand user feedback on bikes in India. Write your own review or write comments on others' reviews to let people know about your experience.";
	keywords		= "bike user reviews, bike users reviews, customer bike reviews, customer bike feedback, bike reviews, bike owner feedback, owner bike reviews, owner report, owner comments";
    canonical       = "http://www.bikewale.com/user-reviews/";
    AdPath = "/1017752/Bikewale_Mobile_NewBikes";
    AdId = "1398766302464";
    menu = "9";
    %>
<!-- #include file="/includes/headermobile.aspx" -->
<script type="text/javascript" src="http://st2.aeplcdn.com/bikewale/src/BikeWaleCommon.js?v=3.2"></script>
<%--    <input type="hidden" id="hdnMake" runat="server"/>
    <input type="hidden" id="hdnModel" runat="server" />--%>
    <div class="padding5">
        <div id="br-cr">
            <span itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
            <a href="/m/new/" itemprop="url" class="normal"><span itemprop="title">New Bikes</span></a></span> &rsaquo; 
            <span class="lightgray">User Reviews</span>
        </div>
        <h1>User Reviews</h1>
        <div class="box1 new-line5">
            <div class="new-line5"><asp:DropDownList ID="ddlMake" runat="server" class="textAlignLeft" data-mini="true"><asp:ListItem Text="--Select Make--" Value="0" /></asp:DropDownList></div> 
            <div id="divModel" class="new-line15">
                <img id="imgLoaderMake" src="http://img.aeplcdn.com/bikewaleimg/images/circleloader.gif" width="16" height="16" style="position:relative;top:3px;display:none;" /> 
                <select data-mini="true" id="ddlModel">
                        <option value="0">--Select Model--</option>         
                </select>
            </div>
            <div class="new-line15">
    	         <input type="button" value="Go" data-theme="b" id="btnSubmit" data-mini="true" data-role="button" data-inline="true" class="ui-corner-all">
            </div>
        </div>
        <div>
            <h2 class="margin-top30 margin-bottom20">Most Reviewed Bikes</h2>
        </div>
        <div id="mostReviewed" class="box new-line5" style="padding:0px 5px;">
            <asp:Repeater id="rptMostReviewed" runat="server">
                <itemtemplate>
                    <a href='/m/<%#DataBinder.Eval(Container.DataItem,"MakeEntity.MaskingName") %>-bikes/<%#DataBinder.Eval(Container.DataItem,"ModelEntity.MaskingName") %>/user-reviews/' style="text-decoration:none;" class="normal">                           
                        <div class="container">
                            <div class="sub-heading">
		                        <b><%# DataBinder.Eval(Container.DataItem, "ModelEntity.ModelName")%> </b> (<%#DataBinder.Eval(Container.DataItem, "ReviewsCount ")%>)&nbsp;&nbsp;<span class="arr-small">&raquo;</span>
	                        </div>    
                        </div>
                    </a>
                </itemtemplate>
            </asp:Repeater>
        </div>
        <div>
            <h2 class="margin-top30 margin-bottom20">Most Read Bikes</h2>
        </div>
        <div  id="divMostRead" class="box new-line5" style="padding:0px 5px;">          
        <asp:Repeater id="rptMostRead" runat="server">
		    <itemtemplate>
                <a href='/m/<%#DataBinder.Eval(Container.DataItem,"TaggedBike.MakeEntity.MaskingName")%>-bikes/<%#DataBinder.Eval(Container.DataItem,"TaggedBike.ModelEntity.MaskingName") %>/user-reviews/<%#DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewId") %>.html' style="text-decoration:none;" class="normal">    
		            <div class="container">
                        <div class="sub-heading">
				            <b><%#DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewTitle ") %></b> &nbsp;&nbsp;<span class="arr-small">&raquo;</span>
			            </div>    
                        <div class="darkgray new-line5 f-12">
                            By <%#DataBinder.Eval(Container.DataItem,"ReviewEntity.WrittenBy") %>
                        </div>
                        <div class="darkgray new-line5" style="font-size:0px;">
                           <%# Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble( DataBinder.Eval(Container.DataItem,"ReviewRating.OverAllRating"))) %>
                            <span class="f-12"> &nbsp;&nbsp;on <%#DataBinder.Eval(Container.DataItem,"TaggedBike.ModelEntity.ModelName") %>  &nbsp;[<%# Convert.ToDateTime (DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewDate")).ToString("dd-MMM-yyyy")%>]</span>                   
                        </div>
		            </div>
                </a>              
	        </itemtemplate>
	    </asp:Repeater>
      </div>
         <div>
            <h2 class="margin-top30 margin-bottom20">Most Helpful</h2>
        </div>
        <div id="mostHelpful" class="box new-line5" style="padding:0px 5px;">
            <asp:Repeater id="rptMostHelpful" runat="server">
                <itemtemplate>
                  <a href='/m/<%#DataBinder.Eval(Container.DataItem,"TaggedBike.MakeEntity.MaskingName")%>-bikes/<%#DataBinder.Eval(Container.DataItem,"TaggedBike.ModelEntity.MaskingName") %>/user-reviews/<%#DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewId") %>.html' style="text-decoration:none;" class="normal">    
		                <div class="container">
                            <div class="sub-heading">
				                <b><%#DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewTitle ") %></b> &nbsp;&nbsp;<span class="arr-small">&raquo;</span>
			                </div>    
                            <div class="darkgray new-line5 f-12">
                                By <%#DataBinder.Eval(Container.DataItem,"ReviewEntity.WrittenBy") %>
                            </div>
                            <div class="darkgray new-line5" style="font-size:0px;">
                               <%# Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble( DataBinder.Eval(Container.DataItem,"ReviewRating.OverAllRating"))) %> 
                                <span class="f-12"> &nbsp;&nbsp;on <%#DataBinder.Eval(Container.DataItem,"TaggedBike.ModelEntity.ModelName") %> &nbsp; [<%# Convert.ToDateTime (DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewDate")).ToString("dd-MMM-yyyy")%>]</span>                   
                            </div>
		                </div>
                    </a>             
                </itemtemplate>
            </asp:Repeater>
        </div>
        <div>
            <h2 class="margin-top30 margin-bottom20">Most Recent</h2>
        </div>
        <div id="mostRecent" class="box new-line5" style="padding:0px 5px;">
            <asp:Repeater id="rptMostRecent" runat="server">
                <itemtemplate>
                    <a href='/m/<%#DataBinder.Eval(Container.DataItem,"TaggedBike.MakeEntity.MaskingName")%>-bikes/<%#DataBinder.Eval(Container.DataItem,"TaggedBike.ModelEntity.MaskingName") %>/user-reviews/<%#DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewId") %>.html' style="text-decoration:none;" class="normal">    
		                <div class="container">
                            <div class="sub-heading">
				                <b><%#DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewTitle ") %></b> &nbsp;&nbsp;<span class="arr-small">&raquo;</span>
			                </div>    
                            <div class="darkgray new-line5 f-12">
                                By <%#DataBinder.Eval(Container.DataItem,"ReviewEntity.WrittenBy") %>
                            </div>
                            <div class="darkgray new-line5" style="font-size:0px;">
                               <%# Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble( DataBinder.Eval(Container.DataItem,"ReviewRating.OverAllRating"))) %> 
                                <span class="f-12"> &nbsp;&nbsp;on <%#DataBinder.Eval(Container.DataItem,"TaggedBike.ModelEntity.ModelName") %> &nbsp; [<%# Convert.ToDateTime (DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewDate")).ToString("dd-MMM-yyyy")%>]</span>                   
                            </div>
		                </div>
                    </a>           
                </itemtemplate>
            </asp:Repeater>
        </div>
         <div>
            <h2 class="margin-top30 margin-bottom20">Most Rated</h2>
        </div>
        <div id="mostRated" class="box new-line5" style="padding:0px 5px;">
            <asp:Repeater id="rptMostRated" runat="server">
                <itemtemplate>
                     <a href='/m/<%#DataBinder.Eval(Container.DataItem,"TaggedBike.MakeEntity.MaskingName")%>-bikes/<%#DataBinder.Eval(Container.DataItem,"TaggedBike.ModelEntity.MaskingName") %>/user-reviews/<%#DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewId") %>.html' style="text-decoration:none;" class="normal">    
		                <div class="container">
                            <div class="sub-heading">
				                <b><%#DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewTitle ") %></b> &nbsp;&nbsp;<span class="arr-small">&raquo;</span>
			                </div>    
                            <div class="darkgray new-line5 f-12">
                                By <%#DataBinder.Eval(Container.DataItem,"ReviewEntity.WrittenBy") %>
                            </div>
                            <div class="darkgray new-line5" style="font-size:0px;">
                               <%# Bikewale.Common.CommonOpn.GetRateImage(Convert.ToDouble( DataBinder.Eval(Container.DataItem,"ReviewRating.OverAllRating"))) %> 
                                <span class="f-12"> &nbsp;&nbsp;on <%#DataBinder.Eval(Container.DataItem,"TaggedBike.ModelEntity.ModelName") %>  [<%# Convert.ToDateTime (DataBinder.Eval(Container.DataItem,"ReviewEntity.ReviewDate")).ToString("dd-MMM-yyyy")%>]</span>                   
                            </div>
		                </div>
                    </a>           
                </itemtemplate>
            </asp:Repeater>
        </div>
    </div>
    <div data-role="popup" id="popupDialog" data-overlay-theme="a" data-theme="c" data-dismissible="false"  class="ui-corner-all">
        <div data-role="header" data-theme="a" class="ui-corner-top" style="background-color:#000">
            <h1 style="color:#fff;">Error !!</h1>
        </div>
        <div data-role="content" data-theme="d" class="ui-corner-bottom ui-content" style="background-color:#ffffff;">
            <span id="spnError" style="font-size:14px;line-height:20px;" class="error"></span>
            <a href="#" data-role="button" data-rel="back" data-theme="c" data-mini="true">OK</a>
        </div>
    </div>

<script type="text/javascript">

    $(document).ready(function () {        
        if ($("#ddlMake").val() != 0) {
            var make = $("#ddlMake").val().split('_')[0];
            var request = "USERREVIEW";
            LoadModels(make, request);
        }
        else {
            $("#ddlModel").val(0).attr("disabled", true);
        }

        $("#ddlMake").change(function () {

            $("#ddlModel").val(0);
            $("#ddlModel").selectmenu("refresh", true);

            var makeId = $(this).val().split('_')[0];
            var requestType = "USERREVIEW";

            if (makeId != 0) {

                $("#imgLoaderMake").show();
                LoadModels(makeId, requestType);
            }
            else {
                $("#ddlModel").val(0).attr("disabled", true);
            }
        });

        $("#btnSubmit").click(function () {
            if (isValid())
                location.href ='/m/' + $("#ddlMake").val().split('_')[1] + '-' + "bikes/" + $("#ddlModel").val().split('_')[1] + "/user-reviews/";
        });

        function isValid()
        {
            var isError = true;
            var errormsg = "";

            if ($("#ddlMake").val() == "0")
            {
                errormsg = "Please select make";
                if ($("#ddlModel").val() == "0")
                    errormsg = "Please select make & model ";
                isError = false;
            }
            else if ($("#ddlModel").val() == "0")
            {
                errormsg = "Please select model ";
                isError = false;
            }

            if (!isError)
            {
                $("#spnError").html(errormsg);
                $("#popupDialog").popup("open");
            }

            return isError;
        }

        function LoadModels(makeId, requestType)
        {
            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
                data: '{"requestType":"' + requestType + '", "makeId":"' + makeId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetModelsWithMappingName"); },
                success: function (response) {
                    $("#imgLoaderMake").hide();
                    var responseJSON = eval('(' + response + ')');
                    var resObj = eval('(' + responseJSON.value + ')');
                    var dependentCmbs = new Array;
                    bindDropDownList(resObj, $("#ddlModel"), "", dependentCmbs, "--Select Model--");
                }
            });
            $("#ddlModel").val(0).attr("disabled", false);
        }
    });
</script>
<!-- #include file="/includes/footermobile.aspx" -->
