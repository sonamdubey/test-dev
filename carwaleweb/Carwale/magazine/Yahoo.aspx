<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Carwale.UI.Common" %>
   
<!DOCTYPE html>
<html lang="en" class="no-js" itemscope itemtype="http://schema.org/WebPage">
<head>    
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
	<meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <title itemprop="name">CarWale - Yahoo</title>
    
    <link rel="stylesheet" type="text/css" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="/static/js/frameworks.js" ></script>
    <script type="text/javascript" src="/static/js/plugins.js" ></script>
    <script type="text/javascript" src="https://cars.tatacapital.com/m/js/carousel-ui-touch-min.js"></script>
   
    <style type="text/css">body,html{height:100%}.form-control,article,aside,dialog,figcaption,figure,footer,header,hgroup,main,nav,section{display:block}.form-control,html{line-height:1.42857143}.btn,.btn-green:hover,.btn-grey:hover,.btn-orange:hover,.btn-white:hover{text-decoration:none}a,abbr,acronym,address,applet,article,aside,audio,b,big,blockquote,body,canvas,caption,center,cite,code,dd,del,details,dfn,div,dl,dt,em,embed,fieldset,figcaption,figure,footer,form,h1,h2,h3,h4,h5,h6,header,hgroup,html,i,iframe,img,ins,kbd,label,legend,li,mark,menu,nav,object,ol,output,p,pre,q,ruby,s,samp,section,small,span,strike,sub,summary,sup,table,tbody,td,tfoot,th,thead,time,tr,tt,u,ul,var,video{margin:0;padding:0;border:0;font-weight:400;vertical-align:baseline}sup{vertical-align:top}*{outline:0}@import url(https://fonts.googleapis.com/css?family=Open+Sans:300italic,400italic,700italic,300,400,700);html{-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;-webkit-overflow-scrolling:touch}body{font-size:13px;font-family:'LatoWeb',sans-serif,Arial;color:#565a5c}blockquote,body,code,dd,div,dl,dt,fieldset,form,h1,h2,h3,h4,h5,h6,input,label,legend,li,ol,p,pre,textarea,ul{margin:0;padding:0}ol,ul{list-style:none}#globalCity-input input[type=text],*,.global-search input[type=text],::after,::before{box-sizing:border-box}h1{font-size:35px;font-weight:700}h2{color:#1a1a1a;font-size:25px;font-weight:500}.form-control,.ui-autocomplete-input:focus,input,select,textarea{color:#666}h3{font-size:18px;font-weight:600}h4{font-size:16px}h5{font-size:14px}h6{font-size:12px}.font10{font-size:10px}.font12{font-size:12px}.font20{font-size:20px}.font16{font-size:16px}.padding-top5{padding-top:5px}.padding-top10{padding-top:10px}.padding-bottom20{padding-bottom:20px}.padding-right5{padding-right:5px}.padding-left5{padding-left:5px}.padding-left10{padding-left:10px}.margin-auto{margin:0 auto}.margin-bottom5{margin-bottom:5px}.margin-bottom10{margin-bottom:10px}.margin-bottom15{margin-bottom:15px}.margin-bottom20{margin-bottom:20px}.margin-top5{margin-top:5px}.margin-top10{margin-top:10px}.margin-top15{margin-top:15px}.margin-top20{margin-top:20px}.margin-top25{margin-top:25px}.margin-right5{margin-right:5px}.margin-right10{margin-right:10px}.margin-right15{margin-right:15px}.margin-right20{margin-right:20px}.margin-right25{margin-right:25px}.margin-right30{margin-right:30px}.margin-left5{margin-left:5px}.margin-left10{margin-left:10px}.margin-left15{margin-left:15px}.margin-left20{margin-left:20px}.margin-left25{margin-left:25px}button,input,select{border:none;outline:0}button{margin:0}button::-moz-focus-inner,button::-webkit-focus-inner,input::-moz-focus-inner,input::-webkit-focus-inner{border:0;padding:0;outline:0}.ui-autocomplete-input,input::-webkit-input-placeholder{color:#666;font-size:13px}.ui-autocomplete-input,input:-moz-placeholder{color:#666;font-size:13px;opacity:1}.ui-autocomplete-input,input::-moz-placeholder{color:#666;font-size:13px;opacity:1}.ui-autocomplete-input,input:-ms-input-placeholder{color:#666;font-size:13px}.form-control-box{position:relative}.form-control{width:100%;padding:10px;background-color:#fff;background-image:none;border:1px solid #ccc;border-radius:4px;-webkit-box-shadow:inset 0 1px 1px rgba(0,0,0,.075);box-shadow:inset 0 1px 1px rgba(0,0,0,.075);-webkit-transition:border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;-o-transition:border-color ease-in-out .15s,box-shadow ease-in-out .15s;transition:border-color ease-in-out .15s,box-shadow ease-in-out .15s}.form-control:focus{border-color:#66afe9;outline:0;-webkit-box-shadow:inset 0 1px 1px rgba(0,0,0,.075),0 0 8px rgba(102,175,233,.6);box-shadow:inset 0 1px 1px rgba(0,0,0,.075),0 0 8px rgba(102,175,233,.6)}select.form-control{-moz-appearance:none;-webkit-appearance:none;background:url(https://img.aeplcdn.com/design15/d/dropArrowBg.png?v1=19082015) 96% 50% no-repeat #fff;padding-right:30px;background:#fff\9;padding-right:5px\9}.select-box.fa-angle-down{position:absolute;top:41%;right:15px;z-index:1;line-height:.5;font-size:18px;display:none\9;display:none}.input-xs{padding:5px 10px}.input-sm{padding:8px 10px}.input-md{padding:10px}.input-lg{padding:12px 10px}.input-xlg{padding:14px 10px}.btn{display:inline-block;padding:8px 42px;font-size:16px;line-height:1.42857143;text-align:center;white-space:nowrap;border:1px solid transparent;border-radius:2px;outline:0;vertical-align:middle;-ms-touch-action:manipulation;touch-action:manipulation;cursor:pointer;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;background-image:none;-webkit-border-fit:border}.btn-orange{background:#f04031;color:#fff}.btn-orange:hover{background:#f85649}.btn-orange:focus{background:#df3828}.btn-white{background:#fff;color:#ef402f;border:1px solid #f04130}.btn-white:hover{background:#f04031;color:#fff}.btn-white:focus{background:#df3828;color:#fff;border:1px solid #f04130}.btn-disable{cursor:not-allowed;opacity:.5;-ms-filter:"progid:DXImageTransform.Microsoft.Alpha(Opacity=50)";filter:alpha(opacity=50);-moz-opacity:.5;-khtml-opacity:.5}.btn-grey{background:#f5f5f5;color:#82888b;border:1px solid #ccc}.btn-grey:hover{background:#f04031;color:#fff;border:1px solid #f04031}.btn-green{background:#62a507;color:#fff}.btn-green:hover{background:#4a8d00;color:#fff}.btn-whiteFixedRed{background:#f04031;color:#fff;border:1px solid #f04130}.btn-green-sm{padding:2px 10px}.btn-xs{padding:8px}.btn-sm{padding:8px 25px}.btn-md{padding:8px 30px}.btn-lg{padding:10px 20px 8px}.btn-xlg{padding:12px 52px}.btn-full-width{display:block;width:100%}.leftfloat{float:left}.rightfloat{float:right}.text-left{text-align:left}.text-right{text-align:right}.text-center{text-align:center}.text-justify{text-align:justify}.text-wrap{white-space:normal}.text-nowrap{white-space:nowrap}.text-lowercase{text-transform:lowercase}.text-uppercase{text-transform:uppercase}.text-capitalize{text-transform:capitalize}.text-bold{font-weight:700}.text-unbold{font-weight:400}.text-italic{font-style:italic}.text-light-grey{color:#82888b}.text-medium-grey{color:#999}.text-grey{color:#666}.text-dark-grey{color:#333}.text-black{color:#1a1a1a}.text-red{color:#ef3f30}.text-white{color:#fff}.text-link{cursor:pointer;color:#0288d1}.text-orange{color:#f04031}.text-orange-light{color:#F88379}.clear{clear:both}body{background-color:#444;min-width:auto}input[type=button]{background-color:#198fff;padding:5px;color:#fff;-moz-border-radius:3px;-webkit-border-radius:3px;-o-border-radius:3px;-ms-border-radius:3px;border-radius:3px;cursor:pointer}select{padding:4px;-moz-border-radius:5px;-webkit-border-radius:5px;-o-border-radius:5px;-ms-border-radius:5px;border-radius:5px;width:100PX;border:1px solid #666}.ulNum li{padding:10px;cursor:pointer;float:left}.ui-state-default,.ui-widget-content .ui-state-default,.ui-widget-header .ui-state-default{border:none;background:url(https://img.aeplcdn.com/magzine/yahoo/slider.png) no-repeat}.ui-slider .ui-slider-handle{width:20px;height:20px;top:-.4em}.ui-widget-header{background:0 0}.error{border:1px solid red!important}#divSlider,#slider{width:84%}#yahooContainer{color:#fff;width:100%}.ui-slider-horizontal .ui-slider-handle{margin-left:-14px}.yahooContent{float:left;width:307px;height:300px;background-color:#444;overflow:hidden}.yahooContent.YleftContent{background:#888}.YleftTopContent{background:#666}.rangeTxt span{background:url(https://img.aeplcdn.com/magzine/yahoo/white-line.jpg) center top no-repeat;display:inline-block;height:24px;padding-top:10px}.rangeTxt{width:98%}.margin-left10.forthFormClick{width:100px}.carwLink{color:#fff}.margin-left-parent20{margin-left:20px}@media only screen and (max-width:610px){.yahooContent{float:none;margin:0 auto;width:100%}.contentW{margin:0 auto;width:88%}.margin-left-parent20{margin-left:0}.rangeTxt span{background:url(https://img.aeplcdn.com/magzine/yahoo/blue-line.jpg) center top no-repeat}.YleftTopContent{background:url(https://img.aeplcdn.com/magzine/yahoo/newcar-shadow.png) top center no-repeat #eee}.yahooContent.YleftContent{background:#eee}.carwLink,.yahooContent{color:#222}select{box-shadow:0 -1px 2px 0 #979797}.yahooContent{background:0 0}.YleftBottomContent{background:-webkit-linear-gradient(#fff,#eee);background:-o-linear-gradient(#fff,#eee);background:-moz-linear-gradient(#fff,#eee);background:linear-gradient(#fff,#eee)}.YrightContent{background:-webkit-linear-gradient(#fff,#f0f0f0);background:-o-linear-gradient(#fff,#f0f0f0);background:-moz-linear-gradient(#fff,#f0f0f0);background:linear-gradient(#fff,#f0f0f0)}}</style>
    <script runat="server">        
        protected void Page_Load(object sender, EventArgs e)
        {
            BindUsedCities();
        }

        private void BindUsedCities()
        {
            System.Collections.Generic.IList<Carwale.Entity.Dealers.DealerCityEntity> cities =
                Carwale.Service.UnityBootstrapper.Resolve<Carwale.Interfaces.Classified.ICommonOperationsCacheRepository>().GetLiveListingCities();
            drpUsedCities.DataTextField = "CityName";
            drpUsedCities.DataValueField = "CityId";
            drpUsedCities.DataSource = cities;
            drpUsedCities.DataBind();
            //obj.FillCity(drpUsedCities);
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="yahooContainer">
            <div class="yahooContent YleftContent">
                <div class="YleftTopContent text-center padding-bottom20">
                <div class="contentW">
                    <h3 class="padding-top10 font20">New Cars</h3>
                    <div class="font20">Search new cars by</div>
                    <div class="margin-top10">
                        <div id="divSlider" class="margin-auto"></div>
                    </div>                    
                    <div class="margin-top5">
                        <div class="rangeTxt padding-left5 padding-right5 margin-auto">
                            <span class="leftfloat yMake">Make</span>
                            <span class="yBodyType">Body Type</span>
                            <span class="rightfloat yBudget">Budget</span>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <div class="margin-top10">
                        <div class="margin-auto">
                            <div id="selection1" class="selectionBox">
                                <select data-bind="options: Make, optionsCaption: 'Select Make',
                                    optionsValue: function (item) { return item.makeId; },
                                    optionsText: function (item) { return item.makeName; }"
                                        id="drpMakeSearch" name="MakeSearch" class="secondForm">
                                </select>
                                <select data-bind="options: Modelsearch, optionsCaption: 'Select Model',
                                    optionsValue: function (item) { return item.MaskingName; },
                                    optionsText: function (item) { return item.ModelName; }"
                                        id="drpModelsSearch" name="Models" class="secondForm margin-left10">
                                </select>
                                <input type="button" id="btnByCar" value="Go" class="margin-left10 secondFormClick" />
                            </div>
                            <div id="selection2" class="selectionBox" style="display:none;">
                                <select id="drpBodyType" class="sixthForm" name="BodyType">
                                    <option value="">Select BodyType</option>
                                    <option value="3">Hatchback</option>
                                    <option value="1">Sedan</option>
                                    <option value="6">SUV/MUV</option>
                                    <option value="4">Minivan/Van</option>
                                    <option value="7">Truck</option>
                                    <option value="8">Station Wagon</option>
                                    <option value="2">Coupe</option>
                                    <option value="5">Convertible</option>
                                </select>
                                <input type="button" id="btnByBodtType" value="Go" class="margin-left10 sixthFormClick" />
                            </div>
                            <div id="selection3" class="selectionBox" style="display:none;">
                                <select id="drpBudget" class="fifthForm" name="Budget">
                                    <option value="">Select Budget</option>
                                    <option value="1">Up to 3 lakh</option>
                                    <option value="2">3-4 lakh</option>
                                    <option value="3">4-6 lakh</option>
                                    <option value="4">6-8 lakh</option>
                                    <option value="5">8-12 lakh</option>
                                    <option value="6">12-18 lakh</option>
                                    <option value="7">18-25 lakh</option>
                                    <option value="8">25-40 lakh</option>
                                    <option value="9">40 lakh and above</option>
                                </select>
                                <input type="button" id="btnBudget" value="Go" class="margin-left10 fifthFormClick" />
                            </div>
                        </div>
                    </div>
                    </div>
                </div>
                <div class="YleftBottomContent">
                <div class="contentW">
                    <div class="padding-top5 text-center">
                        <div class="font16">Check on-road price</div>
                    </div>                        
                    <div class="margin-top5 margin-left20  margin-bottom5">
                        <div>
                        <select data-bind="options: Make, optionsCaption: 'Select Make',
                            optionsValue: function (item) { return item.makeId; },
                            optionsText: function (item) { return item.makeName; }"
                                id="drpMakePrice" name="MakeSearch" class="thirdForm">
                        </select>
                        <select data-bind="options: ModelPrice, optionsCaption: 'Select Model',
                            optionsValue: function (item) { return item.ModelId; },
                            optionsText: function (item) { return item.ModelName; }"
                                id="drpModelsPrice" name="Models" class="margin-left10 thirdForm">
                        </select>
                        </div>
                        <div class="margin-top5">
                             <select data-bind="options: Cities, optionsCaption: 'Select City',
                                                optionsValue: function (item) { return item.CityId; },
                                                optionsText: function (item) { return item.CityName; }"
                                     id="drpCities" name="Cities" class="thirdForm">
                             </select>
                            <input type="button" id="btnOnRoadPrice" value="Go" class="margin-left10 thirdFormClick" />
                        </div>
                    </div>
                    <div class="font10 text-center text-bold">Brought to you by <a href="https://www.carwale.com/?utm_source=yahoo&utm_medium=sponsorship&utm_content=brought_to_you_link&utm_campaign=all_about_cars" target="_blank" class="carwLink">carwale.com</a></div>
                    </div>
                </div>                
            </div>
            <div class="yahooContent YrightContent">
            <div class="contentW">
                    <h3 class="text-center padding-top10 font20">Used Cars</h3>
                    <div class=" font20 text-center">Search used cars by city</div>                   
                    
                    <div class="margin-top10">
                        <div class="leftfloat margin-left-parent20 text-left">
                           <asp:DropDownList ID="drpUsedCities" runat="server" class="forthForm" style="width:180px;"></asp:DropDownList>
                        </div>
                        <div class="rightfloat margin-right0">
                            <div><input type="button" id="btnUsedSearch" value="Search Cars" class="forthFormClick"/>
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>
                    
                    <div class="margin-top20">
                        <div class="leftfloat margin-left-parent20 text-left" style="width:167px;">
                            <div class="font16">Sell your car</div>
                            <div class="font12 margin-top10">Sell your car at the best possible price 28+ lakh searches per month</div>
                        </div>
                        <div class="rightfloat  margin-left0 margin-right0">
                            <div class="margin-left20"><img src=" https://img.aeplcdn.com/magzine/yahoo/freelisting.png" alt="" /></div>
                            <div class="margin-top20 rightfloat">
                                <input type="button" id="btnSellCar" value="Sell Your Car" />
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                    </div>  
                    </div>          
            </div>
            <div class="clear"></div>
        </div>        
    </form>
    <script type="text/javascript">
        $(document).ready(function () {            
            var isError = false;           
            //to disables "---------" options from the used car city dropdown
            $("#drpUsedCities option[value=-1]").attr("disabled", true);
            $("#drpUsedCities option[value=-2]").attr("disabled", true);

            viewModel = {
                Make: ko.observableArray(),
                Model: ko.observableArray(),
                Modelsearch: ko.observableArray(),
                ModelPrice: ko.observableArray(),
                Version: ko.observableArray(),
                Cities: ko.observableArray()
            };

            $("#drpMakeSearch").change(function () {               
                $("#drpModelsSearch").empty();
                
                if ($(this).val() == "") {
                    $('#drpModelsSearch').append(new Option("Select Model", ""));                   
                }
                GetModels($(this).val(), "Modelsearch");
            });
            $("#drpMakePrice").change(function () {
                $("#drpModelsPrice").empty();
                $("#drpCities").empty();
                if ($(this).val() == "") {
                    $('#drpModelsPrice').append(new Option("Select Model", ""));
                    $("#drpCities").empty().append(new Option("Select City", ""));
                }
                GetModels($(this).val(), "ModelPrice");
            });
            $("#drpModelsPrice").change(function () {
                $("#drpCities").empty();             
                GetVersionsCities($(this).val());
                if ($(this).val() == "") {
                    $("#drpCities").empty().append(new Option("Select City", ""));
                }
            });
            $(".secondFormClick").click(function () {                
                isError = false;
                $(".secondForm").each(function () {
                    if ($(this).val() == "") {
                        $(this).addClass("error");
                        isError = true;
                    }
                    else {
                        $(this).removeClass("error");
                        //isError = false;
                    }
                })
                if (isError == false) {
                    window.open("/" + $("#drpMakeSearch option:selected").text().toLowerCase() + "-cars/" + $("#drpModelsSearch").val() + "?utm_source=yahoo&utm_medium=sponsorship&utm_content=search_make_button&utm_campaign=all_about_cars", '_blank');
                }
            });
            $(".fifthFormClick").click(function () {                
                isError = false;
                $(".fifthForm").each(function () {
                    if ($(this).val() == "") {
                        $(this).addClass("error");
                        isError = true;
                    }
                    else {
                        $(this).removeClass("error");
                        //isError = false;
                    }
                })
                if (isError == false) {
                    window.open("/new/search/?utm_source=yahoo&utm_medium=sponsorship&utm_content=search_budget_button&utm_campaign=all_about_cars#budget=" + $("#drpBudget").val(), '_blank');
                }
            });
            $(".sixthFormClick").click(function () {                
                isError = false;
                $(".sixthForm").each(function () {
                    if ($(this).val() == "") {
                        $(this).addClass("error");
                        isError = true;
                    }
                    else {
                        $(this).removeClass("error");
                        //isError = false;
                    }
                })
                if (isError == false) {
                    window.open("/new/search/?utm_source=yahoo&utm_medium=sponsorship&utm_content=search_bodytype_button&utm_campaign=all_about_cars#bs=" + $("#drpBodyType").val(), '_blank');
                }
            });

            $(".forthFormClick").click(function () {                
                isError = false;
                $(".forthForm").each(function () {
                    if ($(this).val() == "0" || $(this).val() == "-1" || $(this).val() == "-2") {
                        $(this).addClass("error");
                        isError = true;
                    }
                    else {
                        $(this).removeClass("error");
                        //isError = false;
                    }
                })
                if (isError == false) {
                    window.open("/used/cars-in-" + $("#drpUsedCities option:selected").text().toLowerCase().replace(" ", "") + "/?utm_source=yahoo&utm_medium=sponsorship&utm_content=search_by_city_button&utm_campaign=all_about_cars#city=" + $("#drpUsedCities").val(), '_blank');
                }
            });

            $(".thirdFormClick").click(function () {
                isError = false;
                $(".thirdForm").each(function () {
                    if ($(this).val() == "") {
                        $(this).addClass("error");
                        isError = true;
                    }
                    else {
                        $(this).removeClass("error");
                        //isError = false;
                    }
                })

                if (isError == false) {

                    document.cookie = '_PQModelId=' + $("#drpModelsPrice").val() + '; path =/';
                    document.cookie = '_PQVersionId=0; path =/';

                    /*if ($.cookie("_CustCityId") != null && $.cookie("_CustCityId") != "")
                        document.cookie = '_CustCityId=' + $.cookie("_CustCityId") + '; path =/';
                    else*/
                    document.cookie = '_CustCityId=' + $("#drpCities").val() + '; path =/';

                    document.cookie = '_PQZoneId=; path =/';

                    window.open("/new/quotation.aspx?", '_blank');
                }
            });
            $("#btnSellCar").click(function () {
                window.open('/used/sell/?utm_source=yahoo&utm_medium=sponsorship&utm_content=sell_car_button&utm_campaign=all_about_cars', '_blank');
            })            

            $("select").change(function () {
                if ($(this).val() > 0)
                    $(this).removeClass("error");
            });

            GetAllMakes();
            ko.applyBindings(viewModel);
            var slider = $("#divSlider").append("<div id='slider'></div>").slider({
                min: 1,
                max: 3,
                range: "min",
                value: 1,
                slide: function (event, ui) {
                    ShowTabContent(ui.value);
                }
            });
        });

        function GetAllMakes() {
            $.ajax({
                url: '/webapi/CarMakesData/GetCarMakes/?type=new',
                type: "GET",
                data: '',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    viewModel.Make(response);
                    viewModel.Make.sort(function (l, r) { return l.makeName == r.makeName ? 0 : (l.makeName < r.makeName ? -1 : 1) });
                }
            });
        }
        
        function GetModels(makeId, source) {
            if (makeId != null && makeId != "") {
                $.ajax({
                    url: '/webapi/CarModelData/GetCarModelsByType/?type=new&makeId=' + makeId,
                    type: "GET",
                    data: '',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response != null) {
                            response.unshift({ 'ModelId': '', 'ModelName': 'Select Model' })
                            if (source == "Model") {
                                viewModel.Model(response);                               
                            }
                            else if (source == "Modelsearch") {
                                viewModel.Modelsearch(response);                                
                            }
                            else if (source == "ModelPrice") {
                                viewModel.ModelPrice(response);                              
                            }
                        }
                    }
                });
            }
        }
        function GetVersionsCities(modelId) {
            if (modelId != null && modelId != "") {
                $.ajax({
                    url: '/webapi/GeoCity/GetPQCitiesByModelId/?modelid=' + modelId,
                    type: "GET",
                    data: '',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response != null) {
                            response.unshift({ 'CityId': '', 'CityName': 'Select City' });
                            viewModel.Cities(response);
                            $("#drpCities").attr('disabled', false);
                        }
                    }
                });
            }
        }
        function ShowTabContent(tabValue) {
            $(".selectionBox").hide();
            $("#selection" + tabValue).show();
        }
    </script>
</body>
</html>