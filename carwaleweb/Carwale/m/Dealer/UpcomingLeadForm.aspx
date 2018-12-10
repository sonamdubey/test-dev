<%@ Page Language="C#" AutoEventWireup="false" Inherits="Carwale.MobileWeb.Dealer.upcomingleadform" %>
<%@ Import Namespace="Carwale.BL.Experiments" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <% 
        Title = upcomingModel.MakeName+ " " + upcomingModel.ModelName + " - CarWale";
        Canonical = "https://carwale.com/new/testdrive.aspx";
        bool showExperimentalColor = ProductExperiments.IsShowExperimentalColor(CookiesCustomers.AbTest);
     %>
<head>
    <!-- #include file="/m/includes/global/head-script.aspx" -->
</head>
<body class="m-special-skin-body m-no-bg-color <%= (showExperimentalColor ? "btn-abtest" : "")%>">
    <!-- #include file="/m/includes/header.aspx" -->
    <!--Outer div starts here-->
    <section class="container">
    	<!--Main container starts here-->
    	<div id="main-container" class="position-rel">
            <div class="grid-12 font11 text-right grid-12">Ad</div>
            <div class="clear"></div>

        	<div class="grid-12">
            <h2 class="pgsubhead margin-top10 margin-bottom10 m-special-skin-text"><%=string.Format("{0} {1}",upcomingModel.MakeName,upcomingModel.ModelName) %>: Be The First To Know</h2>   
            <div class="content-inner-block-10 bg-white content-box-shadow margin-bottom30">
                <%if (upcomingModel.ModelId == 852)
                  {%>
                 <div class="font16">The Tata Zica has been renamed as the Tata Tiago</div> 
                <%} %>
                 <div class="margin-top10 text-center">
                     <img src="<%=defaultImgUrl %>" style="width:100%;max-width:100%;"/>
                 </div> 
                 <div class="margin-top20 font16 postSubmit">Provide your contact details. </div>
                 <div class="font13 postSubmit">All fields are mandatory.</div>
                 <form runat="server">  
                    <div class="margin-top20">    
                        <div class="box-bot">
                            <div class="margin-bottom15 position-rel">     
                                <input id="txtName" class="form-control" type="text" prefill="name" placeholder="Name" data-role="none" /> 
                                <span style="" class="cwmsprite error-icon brand-error-icon hide"></span>
                                <div id="errtxtName" class="cw-blackbg-tooltip brand-err-msg hide">Please enter your name</div>    
                            </div>
                            <div class="margin-bottom15 position-rel">
                                <span class="leftfloat" style="padding-top: 10px;padding-right: 5px;">+91-</span> 
                                <input id="txtMobile" class="form-control" type="text" prefill="mobile" placeholder="Mobile" data-role="none" maxlength="10" style="width: 90%;margin-bottom: 15px;"/> 
                                <span style="" class="cwmsprite error-icon brand-error-icon hide"></span>
                                <div id="" class="cw-blackbg-tooltip brand-err-msg hide">Invalid Mobile Number</div> 
                            </div>
                            <div class="form-control-box margin-bottom15 position-rel"> 
                                <span class="select-box fa fa-angle-down"></span>
                                <asp:DropDownList prefill="state" ID="drpStates" class="form-control" data-role="none" runat="server" onChange="javascript:Leadform.methods.drpState_Change(this);">
                        </asp:DropDownList>      
                                <!--<select class="form-control" data-role="none">
                                    <option>State</option>
                                     <option>1</option>
                                     <option>1</option>
                                </select>-->
                                <span style="" class="cwmsprite error-icon brand-error-icon hide"></span>
                                <div id="" class="cw-blackbg-tooltip brand-err-msg hide">Please select state</div>
                            </div>
                            <div class="form-control-box margin-bottom15 position-rel"> 
                                <span class="select-box fa fa-angle-down"></span>      
                                <select id="drpCities" prefill="city" class="form-control" data-role="none">
                                    <option>--Select City--</option>
                                </select>
                                <span style="" class="cwmsprite error-icon brand-error-icon hide"></span>
                                <div id="" class="cw-blackbg-tooltip brand-err-msg hide">Please select city</div>
                            </div>
                            <div class="margin-bottom15 position-rel">     
                                <input id="txtEmail" class="form-control" type="email" prefill="email" placeholder="Email" data-role="none"/> 
                                <span style="" class="cwmsprite error-icon brand-error-icon hide"></span>
                                <div id="" class="cw-blackbg-tooltip brand-err-msg hide">Invalid Email</div>    
                            </div>
                            <div class="new-line15">
                                <a id="" class="linkButtonBig btn btn-xs btn-orange btn-full-width margin-bottom10" onclick="javascript:Leadform.methods.process();"  data-rel="popup" data-role="button">SUBMIT </a>
                            </div>
                            <div class="font13 margin-top10">A <%=upcomingModel.MakeName.ToLower()=="tata"?"Tata Motors":upcomingModel.MakeName %> representative will get in touch with you shortly regarding the <%=entireCarName %>.</div>
                        </div>
                        <div id="postMsg" class="text-center margin-top20 border-solid content-inner-block-10 hide">
                            <div class="font16">Thank You!</div>
                            <div class="font13 border-solid-bottom padding-bottom5 margin-top5"> Thank you for expressing interest in the <%=entireCarName %>.</div>
                            <div class="font13 margin-top5">A <%=upcomingModel.MakeName.ToLower()=="tata"?"Tata Motors":upcomingModel.MakeName %> representative will get in touch with you shortly regarding the <%=entireCarName %>.</div>
                        </div>
                        <div id="postMsgFail" class="text-center margin-top20 border-solid content-inner-block-10 hide">
                            <div class="font13 border-solid-bottom padding-bottom5 margin-top5"> Please try again after sometime.</div>
                        </div> 
                    </div>           
                </form>
            </div>      
            </div>
           <div class="clear"></div>
        </div>
        <!--Main container ends here-->
    </section>
    <div class="clear"></div>
    <!--Outer div ends here-->
    <!-- #include file="/m/includes/footer.aspx" -->
    <!-- #include file="/m/includes/global/footer-script.aspx" -->
    <!--Outer div ends here-->
    <script>
        var Leadform = {
            modelId:<%=defaultModelVal%>,
            methods: {
                drpState_Change: function (e) {
                    var stateId = $("#drpStates").val();
                    dependentCmbs = new Array();

                    if (Number(stateId) > 0) {
                        $.ajax({
                            type: "POST", url: "/ajaxpro/CarwaleAjax.AjaxCommon,Carwale.ashx",
                            data: '{"stateId":"' + stateId + '"}',
                            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetCities"); },
                            success: function (response) {
                                var jsonString = eval('(' + response + ')');
                                var resObj = eval('(' + jsonString.value + ')');
                                Leadform.methods.bindDropDownList(resObj, document.getElementById("drpCities"), 'hdn_drpCities', '', '--Select City--');
                            }
                        });
                    } else {
                        dependentCmbs[0] = "drpCities";
                        Leadform.methods.resetDependentFields(dependentCmbs);
                    }
                },
                bindDropDownList: function (response, cmbToFill, viewStateId, dependentCmbs, selectString) {
                    if (response.Table != null) {
                        if (!selectString || selectString == '') selectString = "--Select--";
                        $(cmbToFill).empty().append("<option value=\"0\">" + selectString + "</option>").attr("disabled", false);

                        var hdnValues = "";

                        for (var i = 0; i < response.Table.length; i++) {
                            $(cmbToFill).append("<option value=" + response.Table[i].Value + ">" + response.Table[i].Text + "</option>");

                            if (hdnValues == "")
                                hdnValues += response.Table[i].Text + "|" + response.Table[i].Value;
                            else
                                hdnValues += "|" + response.Table[i].Text + "|" + response.Table[i].Value;
                        }
                        if (viewStateId) $("#" + viewStateId).val(hdnValues);
                    }

                    if (dependentCmbs && dependentCmbs.length > 0) {
                        for (var i = 0; i < dependentCmbs.length; i++) {
                            $("#" + dependentCmbs[i]).attr("disabled", true);
                        }
                    }
                },
                resetDependentFields: function (arrDependencies) {
                    if (arrDependencies && arrDependencies.length > 0) {
                        for (var i = 0; i < arrDependencies.length; i++) {
                            $("#" + arrDependencies[i]).empty().attr("disabled", true);
                        }
                    }
                },
                resetErrors: function () {
                    $('.error-icon,.cw-blackbg-tooltip').addClass("hide");
                },
                process: function () {
                    var isValid = true;
                    $("#lblMsg").text("");
                    var city = $("#drpCities").val();
                    var state = $("#drpStates").val();
                    var email = $.trim($('#txtEmail').val());
                    var reName = /^([-a-zA-Z ']*)$/;
                    var re = /^[0-9]*$/;
                    var name = $.trim($('#txtName').val());
                    var mobile = $.trim($('#txtMobile').val());
                    var emailFilter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;

                    Leadform.methods.resetErrors();

                    if (name.length == 0) {
                        $("#txtName").siblings('.error-icon,.cw-blackbg-tooltip').removeClass("hide");
                        return false;
                    }
                    else if (reName.test(name) == false) {
                        $("#txtName").siblings('.error-icon,.cw-blackbg-tooltip').removeClass("hide");
                        return false;
                    }
                    else if (name.length == 1) {
                        $("#txtName").siblings('.error-icon,.cw-blackbg-tooltip').removeClass("hide");
                        return false;
                    }

                    if (mobile == "") {
                        $("#txtMobile").siblings('.error-icon,.cw-blackbg-tooltip').removeClass("hide");
                        return false;
                    } else if (re.test(mobile) == false) {
                        $("#txtMobile").siblings('.error-icon,.cw-blackbg-tooltip').removeClass("hide");
                        return false;
                    } else if (mobile.length != 10) {
                        $("#txtMobile").siblings('.error-icon,.cw-blackbg-tooltip').removeClass("hide");
                        return false;
                    }

                    if (Number(state) < 1) {
                        $("#drpStates").siblings('.error-icon,.cw-blackbg-tooltip').removeClass("hide");

                        return false;
                    }

                    if (Number(city) < 1) {
                        $("#drpCities").siblings('.error-icon,.cw-blackbg-tooltip').removeClass("hide");
                        return false;
                    }

                    if (email == "") {
                        $("#txtEmail").siblings('.error-icon,.cw-blackbg-tooltip').removeClass("hide");
                        return false;
                    }
                    else if (!emailFilter.test(email)) {
                        $("#txtEmail").siblings('.error-icon,.cw-blackbg-tooltip').removeClass("hide");
                        return false;
                    }
                    if (isValid) {
                        var utm=location.search.indexOf("&")==-1?"":location.search.substring(location.search.indexOf("&")+1);
                        $.ajax({
                            type: "POST",
                            url: "/ajaxpro/CarwaleAjax.AjaxResearch,Carwale.ashx",
                            data: '{"carName":"<%=string.Format("{0} {1}",upcomingModel.MakeName,upcomingModel.ModelName)%>", "custName":"' + name + '", "email":"' + email + '", "mobile":"' + mobile + '", "selectedCityId":"' + $("#drpCities").val() + '", "versionId":"' + "" + '", "modelId":"' + Leadform.modelId + '", "makeId":"' + "" + '", "leadtype":"' + 16 + '", "cityName":"' + $("#drpCities option[value='" + $("#drpCities").val() + "']").text() + '","utm":"'+$.cookie("__utmz")+'"}',
                            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "PushCRM"); },
                            success: function (response) {
                                var responseObj = eval('(' + response + ')');
                                if (responseObj.value) {
                                    $("div.box-bot").addClass("hide");
                                    $(".postSubmit").addClass("hide");
                                    $("#postMsg").removeClass("hide");
                                }
                                else {
                                    $("div.box-bot").addClass("hide");
                                    $("#postMsgFail").removeClass("hide");
                                }
                            }
                        });
                    }
                }
            }
        }
    </script>
</body>
</html>

