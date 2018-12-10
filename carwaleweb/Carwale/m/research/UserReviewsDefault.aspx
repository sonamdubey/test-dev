<%@ Page Language="C#" Inherits="MobileWeb.Research.UserReviewsDefault" ContentType="text/html" AutoEventWireup="false" Trace="false" EnableEventValidation="false" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%	
    Title = "User Reviews on Cars in India - CarWale";
    Keywords = "car user reviews, car users reviews, customer car reviews, customer car feedback, car reviews, car owner feedback, owner car reviews, owner report, owner comments";
    Description = "Know what users are saying about the car you aspire to buy. Read first hand user feedback on cars in India. Write your own review or write comments on others' reviews to let people know about your experience.";
    Canonical = "https://www.carwale.com" + Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString().Replace("/m/", "/");
    MenuIndex = "1";
    DeeplinkAlternatives = Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString();
%>

<!DOCTYPE html>
<html>

<!DOCTYPE html >
<html itemscope itemtype="http://schema.org/WebPage">
<head>
    <!-- #include file="../includes/global/head-script-old.aspx" -->
    <style type="text/css">
        .circle-icon-placeholder {
            width: 80px;
            height: 80px;
            border: 1px solid #e2e2e2;
            background: #f5f5f5;
            margin: 0 auto;
            -webkit-border-radius: 50%;
            -moz-border-radius: 50%;
            -ms-border-radius: 50%;
            border-radius: 50%;
        }
        .write-icon {
            width: 30px;
            height: 32px;
            background: url(https://imgd.aeplcdn.com/0x0/cw/static/icons/m/write-icon.png) no-repeat;
            display: inline-block;
            position: relative;
            left: 2px;
        }
        input#globalSearchPopup {width: 100%!important;}    
    </style>
    <link rel="stylesheet" href="/Static/m/css/cover-window-popup.css" type="text/css">
</head>

<body class="bg-light-grey special-page special-skin-body no-bg-color">
    <!-- #include file="/m/includes/header.aspx" -->
    <!--Outer div starts here-->
    <div data-role="page">
        <section class="container">
        <!--Main container starts here-->
        <div id="main-container">
            <div id="br-cr"><a href="/m/new/" class="normal m-special-skin-text">New Cars</a><span class="lightgray m-special-skin-text"> &rsaquo; User Reviews </span></div>
            <form runat="server">
                <asp:TextBox ID="txtMake" runat="server" Style="display: none;" Text="" />
                <asp:TextBox ID="txtModel" runat="server" Style="display: none;" Text="" />
                <div class="pgsubhead">
                   <h1 class="m-special-skin-text"> User Reviews</h1>
                </div>
                <div class="content-box-shadow content-inner-block-10 rounded-corner2 margin-bottom10 text-black">
                    <div class="heading">Search By : </div>
                    <div class="margin-top5">
                        <div class="margin-top10">Make &nbsp;&nbsp; <span id="spnMake" class="text-red"></span></div>
                        <div class="form-control-box margin-bottom10">
                            <asp:DropDownList ID="ddlMakes" class="form-control" runat="server" onchange="MakeChanged();" data-role="none" />
                        </div>
                    </div>
                    <div class="margin-top5">
                        <div class="margin-top10">Model &nbsp;&nbsp; <span id="spnModel" class="text-red"></span></div>
                        <div class="margin-top5">
                            <asp:DropDownList ID="ddlModels" class="form-control" runat="server" onchange="ModelChanged();" data-role="none" /></div>
                        <img id="imgLoaderModel" src="/m/images/circleloader.gif" width="16" height="16" style="position: relative; top: 3px; display: none;" />
                    </div>
                    <div class="margin-top10">
                        <asp:LinkButton ID="btnSubmit" runat="server" class="btn btn-orange" Text="&nbsp;&nbsp;&nbsp;&nbsp;Go&nbsp;&nbsp;&nbsp;&nbsp;" /></div>
                    <div class="new-line5">&nbsp;</div>
                </div>

                <div id="rate-car-landing">
                    <div class="pgsubhead">
                    <h1 class="m-special-skin-text">Rate your car</h1>
                    </div>
                    <div class="content-box-shadow box content-inner-block-10 rounded-corner2 margin-bottom10 text-black">
                        <div class="circle-icon-placeholder text-center">
                            <span class="write-icon margin-top25"></span>
                        </div>
                        <p class="text-light-grey font16 margin-top15 margin-bottom15">Rate your car and help others with your experience. Share your ratings and review now!</p>
                        <a data-role="click-tracking" data-event="CWInteractive" data-action="rate_your_car" data-cat="msite_user_review" data-label="" href="javascript:void(0)" class="btn btn-orange btn-xs" data-bind="click: function (d, e) { openCarSelection(); }" rel="nofollow">Select your car to rate</a>
                    </div>

                    <!-- select car starts here -->
                    <div id="select-car-cover-popup" data-bind="with: carSelection" class="cover-window-popup">
                        <div class="ui-corner-top">
                            <div id="close-car-popup" class="cover-popup-back cur-pointer leftfloat" data-bind="click: closeCarPopup">
                                <span class="cwmsprite back-arrow-white"></span>
                            </div>
                            <div class="cover-popup-header leftfloat">Select car</div>
                            <div class="clear"></div>
                        </div>
                        <div class="car-banner"></div>
                        <div id="select-make-wrapper" class="cover-popup-body" data-bind="visible: currentStep() == 1">
                            <div class="cover-popup-body-head">
                                <p class="no-back-btn-label head-label inline-block">Select Make</p>
                            </div>
                            <ul class="cover-popup-list with-arrow">
                                <asp:Repeater ID="rptMakes" runat="server">
                                  <ItemTemplate>
										<li data-role="click-tracking" data-event="CWInteractive" data-action="make_selection" data-cat="msite_user_review" data-label="<%# DataBinder.Eval(Container.DataItem, "makeName")%>" data-bind="click: makeChanged"><span data-id="<%# DataBinder.Eval(Container.DataItem, "makeId")%>"><%# DataBinder.Eval(Container.DataItem, "makeName")%></span></li>
                                  </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>

                        <div id="select-model-wrapper" class="cover-popup-body" data-bind="visible: currentStep() == 2">
                            <div class="cover-popup-body-head">
                                <div data-bind="click: modelBackBtn" class="body-popup-back cur-pointer inline-block">
                                    <span class="cwmsprite back-long-arrow-left"></span>
                                </div>
                                <p class="head-label inline-block">Select Model</p>
                            </div>
                            <ul class="cover-popup-list with-arrow" data-bind="foreach: modelArray">
                                <li data-bind="click: $parent.modelChanged">
                                    <span data-role="click-tracking" data-event="CWInteractive" data-action="model_selection" data-cat="msite_user_review" data-bind="text: ModelName, attr: { 'data-id': ModelId, 'data-label' : ModelName }"></span>
                                </li>
                            </ul>
                        </div>

                        <div id="select-variant-wrapper" class="cover-popup-body" data-bind="visible: currentStep() == 3">
                            <div class="cover-popup-body-head">
                                <div data-bind="click: modelBackBtn" class="body-popup-back cur-pointer inline-block">
                                    <span class="cwmsprite back-long-arrow-left"></span>
                                </div>
                                <p class="head-label inline-block">Select Variant</p>
                            </div>
                            <ul class="cover-popup-list with-arrow" data-bind="foreach: versionArray">
                                <li data-bind="click: $parent.versionChanged">
                                    <span data-role="click-tracking" data-event="CWInteractive" data-action="variant_selection" data-cat="msite_user_review" data-label="" data-bind="text: Version, attr: { 'data-id': Id, 'data-label' : Version}"></span>
                                </li>
                            </ul>
                        </div>
                        <div id="loadingCarImg" data-bind="attr: { class: IsLoading() ? '' : 'hide' }">
                            <div class="m-loading-popup">
                                <span class="m-loading-icon"></span>
                                <div class="clear"></div>
                            </div>
                        </div>
                    </div>
                    <!-- select car ends here -->
                </div>

                <div class="pgsubhead">
                  <h1 class="m-special-skin-text">  Most Reviewed Cars</h1>
                </div>
                <div id="divMostReviewed" class="margin-top5">
                    <asp:Repeater ID="rptMostReviewed" runat="server">
                        <ItemTemplate>
                            <a href='/m/<%# MobileWeb.Common.CommonOpn.FormatSpecial(DataBinder.Eval(Container.DataItem, "MakeName").ToString()) %>-cars/<%# DataBinder.Eval(Container.DataItem, "MaskingName").ToString() %>/userreviews/' style="text-decoration: none;">
                                <div class="text-black box content-box-shadow content-inner-block-5 rounded-corner2 margin-bottom20">
                                    <div class="sub-heading">
                                        <b><%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() + " "+ DataBinder.Eval(Container.DataItem, "ModelName").ToString()%></b> (<%# DataBinder.Eval(Container.DataItem, "TotalReviews").ToString()%>)&nbsp;&nbsp;<span class="arr-small">&raquo;</span>
                                    </div>
                                </div>
                            </a>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="pgsubhead">
                 <h1 class="m-special-skin-text">  Most Read</h1> 
                </div>
                <div id="divMostRead" style="padding: 0px 5px;">
                    <asp:Repeater ID="rptMostRead" runat="server">
                        <ItemTemplate>
                            <a href='/m/<%# MobileWeb.Common.CommonOpn.FormatSpecial(DataBinder.Eval(Container.DataItem, "MakeName").ToString()) %>-cars/<%# DataBinder.Eval(Container.DataItem, "MaskingName").ToString() %>/userreviews/<%#(DataBinder.Eval(Container.DataItem, "ReviewId").ToString()) %>.html' style="text-decoration: none;">
                                <div class="text-black box content-box-shadow content-inner-block-10 rounded-corner2 margin-bottom20">
                                    <div class="sub-heading">
                                        <b><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></b> &nbsp;&nbsp;<span class="arr-small">&raquo;</span>
                                    </div>
                                    <div class="darkgray margin-top5">
                                        <%#DataBinder.Eval(Container.DataItem, "CustomerName")%>
                                    </div>
                                    <div class="darkgray margin-top5">
                                        <%# Carwale.UI.Common.CommonOpn.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem, "ReviewRate")))%> On <%# DataBinder.Eval(Container.DataItem,"ModelName")%>
                                    </div>
                                </div>
                            </a>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="pgsubhead">
                    <h1 class="m-special-skin-text">Most Helpful</h1>
                </div>
                <div id="div1" style="padding: 0px 5px;">
                    <asp:Repeater ID="rptMostHelpful" runat="server">
                        <ItemTemplate>
                            <a href='/m/<%# MobileWeb.Common.CommonOpn.FormatSpecial(DataBinder.Eval(Container.DataItem, "MakeName").ToString()) %>-cars/<%#DataBinder.Eval(Container.DataItem, "MaskingName").ToString() %>/userreviews/<%#(DataBinder.Eval(Container.DataItem, "ReviewId").ToString()) %>.html' style="text-decoration: none;">
                                <div class="text-black box content-box-shadow content-inner-block-10 rounded-corner2 margin-bottom20">
                                    <div class="sub-heading">
                                        <b><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></b> &nbsp;&nbsp;<span class="arr-small">&raquo;</span>
                                    </div>
                                    <div class="darkgray margin-top5">
                                        <%#DataBinder.Eval(Container.DataItem, "CustomerName")%>
                                    </div>
                                    <div class="darkgray margin-top5">
                                        <%# Carwale.UI.Common.CommonOpn.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem, "ReviewRate")))%> On <%# DataBinder.Eval(Container.DataItem,"ModelName")%>
                                    </div>
                                </div>
                            </a>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="pgsubhead">
                  <h1 class="m-special-skin-text"> Most Recent</h1> 
                </div>
                <div id="div2" style="padding: 0px 5px;">
                    <asp:Repeater ID="rptMostRecent" runat="server">
                        <ItemTemplate>
                            <a href='/m/<%# MobileWeb.Common.CommonOpn.FormatSpecial(DataBinder.Eval(Container.DataItem, "MakeName").ToString()) %>-cars/<%# DataBinder.Eval(Container.DataItem, "MaskingName").ToString() %>/userreviews/<%#(DataBinder.Eval(Container.DataItem, "ReviewId").ToString()) %>.html' style="text-decoration: none;">
                                <div class="text-black box content-box-shadow content-inner-block-10 rounded-corner2 margin-bottom20">
                                    <div class="sub-heading">
                                        <b><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></b> &nbsp;&nbsp;<span class="arr-small">&raquo;</span>
                                    </div>
                                    <div class="darkgray margin-top5">
                                        <%#DataBinder.Eval(Container.DataItem, "CustomerName")%>
                                    </div>
                                    <div class="darkgray margin-top5">
                                        <%# Carwale.UI.Common.CommonOpn.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem, "ReviewRate")))%> On <%# DataBinder.Eval(Container.DataItem,"ModelName")%>
                                    </div>
                                </div>
                            </a>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="pgsubhead">
                  <h1 class="m-special-skin-text">  Most Rated</h1>
                </div>
                <div id="div3" style="padding: 0px 5px;">
                    <asp:Repeater ID="rptMostRated" runat="server">
                        <ItemTemplate>
                            <a href='/m/<%# MobileWeb.Common.CommonOpn.FormatSpecial(DataBinder.Eval(Container.DataItem, "MakeName").ToString()) %>-cars/<%# DataBinder.Eval(Container.DataItem, "MaskingName").ToString() %>/userreviews/<%#(DataBinder.Eval(Container.DataItem, "ReviewId").ToString()) %>.html' style="text-decoration: none;">
                                <div class="text-black box content-box-shadow content-inner-block-10 rounded-corner2 margin-bottom20">
                                    <div class="sub-heading">
                                        <b><%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></b> &nbsp;&nbsp;<span class="arr-small">&raquo;</span>
                                    </div>
                                    <div class="darkgray margin-top5">
                                        <%#DataBinder.Eval(Container.DataItem, "CustomerName")%>
                                    </div>
                                    <div class="darkgray margin-top5">
                                        <%# Carwale.UI.Common.CommonOpn.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem, "ReviewRate")))%> On <%# DataBinder.Eval(Container.DataItem,"ModelName")%>
                                    </div>
                                </div>
                            </a>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </form>
        </div>
        <!--Main container ends here-->
     </section>
    </div>

     <!-- #include file="/m/includes/footer.aspx" -->
    <!-- #include file="/m/includes/global/footer-script.aspx" -->
    <script  type="text/javascript" src="/static/m/js/car-selection-popup.js"></script>
    <!--Outer div ends here-->
      <script language="javascript" type="text/javascript">
          function MakeChanged() {
              $("#ddlModels").html("<option value='0'>--Select--</option>");
              var _makeId = $("#ddlMakes").val();
              if (_makeId > 0) {
                  $("#imgLoaderModel").show();
                  $("#txtMake").val($("#ddlMakes option:selected").text());
                  $.ajax({
                      type: "GET",
                      url: "/webapi/carmodeldata/GetCarModelsByType/?type=All&makeId=" + _makeId,
                      beforeSend: function (xhr) {
                          $("#ddlModels").attr('disabled', true);
                          $("#ddlModels").empty();
                      },
                      success: function (response) {
                          $("#imgLoaderModel").hide();
                          if (response)
                          {
                              bindModels(response, $("#ddlModels"), "", "--Select Model--");
                          }
                      }
                  });
              }
          }

          function bindModels(response, cmbToFill, viewStateId, selectString) {
              if (response != null) {
                  if (!selectString || selectString == '') selectString = "--Select--";
                  $(cmbToFill).empty().append("<option value=\"0\" title='" + selectString + "'>" + selectString + "</option>").removeAttr("disabled");
                  var hdnValues = "";
                  for (var i = 0; i < response.length; i++) {
                      $(cmbToFill).append("<option value=" + response[i].ModelId + " MaskingName='" + response[i].MaskingName + "' title='" + response[i].ModelName + "'>" + response[i].ModelName + "</option>");
                      if (hdnValues == "")
                          hdnValues += response[i].ModelName + "|" + response[i].ModelId;
                      else
                          hdnValues += "|" + response[i].ModelName + "|" + response[i].ModelId;
                  }
                  if (viewStateId) $("#" + viewStateId).val(hdnValues);
              }
          }

          function ModelChanged() {
              var _modelId = $("#ddlModels").val();
              if (_modelId > 0) {           
                  $("#txtModel").val($("#ddlModels option:selected").attr("MaskingName").toString());               
              }
          }

          $(document).ready(function () {
              if ($.cookie('ReviewRepeated')) {
                  $.cookie('ReviewRepeated', null, { expires: new Date(-8640000000000000), path: '/' });
                  alert('You have already submitted review for this car');
              }
              SetControlWidth();
          });

          function IsValid() {
              var retVal = true;
              $("#spnMake").html("");
              $("#spnModel").html("");

              if ($("#ddlMakes").val() <= 0) {
                  retVal = false;
                  $("#spnMake").html("(Required)");
              }

              if ($("#ddlModels").val() <= 0) {
                  retVal = false;
                  $("#spnModel").html("(Required)");
              }
              return retVal;
          }
    </script>
</body>
</html>
