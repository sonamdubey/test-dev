<%--<%@ Page Language="C#" AutoEventWireup="false" CodeFile="QuickPQWidget.aspx.cs" Inherits="new_QuickPQWidget" %>--%>

<%@ Page Language="C#" Trace="false" %>

<%
    string ModelId = string.Empty;
    string PageId = string.Empty;
    string VersionId = string.Empty;
    
    HtmlGenericControl hdn_ModelId = new HtmlGenericControl(); ;
    if (!string.IsNullOrEmpty(Request.QueryString["model"]))
    {
        ModelId = Request.QueryString["model"].ToString();
    }
    if (!string.IsNullOrEmpty(Request.QueryString["version"]))
    {
        VersionId = Request.QueryString["version"].ToString();
    }
    if (!string.IsNullOrEmpty(Request.QueryString["pageid"]))
    {
        PageId = Request.QueryString["pageid"].ToString();
    }     
   
%>
<style>
    .width320 {
        width: 320px;
    }

    .margin-right45 {
        margin-right: 45px;
    }
</style>
<!-- Make popUp code starts hete -->
<div id="pqPopUpMake" class="">
    <div class="pq-popup">
        <div class="pq-popup-content">
            <div id="cityDiv" class="margin-bottom10">
                <label class="inline-block margin-right45"><b>City</b></label>
                <div class="form-control-box margin-bottom5 inline-block width320">
                    <span class="select-box fa fa-angle-down"></span>
                    <select id="drpCity" class="form-control" data-bind='options: pqCities, optionsText: "CityName", optionsValue: "CityId", event: { change: setCityCookieKo }'></select>
                </div>
                <span id="spnCity" class="text-red block text-right"></span>
            </div>
            <div class="rightfloat margin-right10">
                <span id="hdn_ModelId" runat="server" value=""></span>
                <input type="button" id="btn-show-on-road-price" onclick="checkOnRoadPQ()" class="btn btn-orange btn-xs inline-block" value="Show On-Road Price" style="padding: 9px 20px;" />
            </div>
            <div class="clear"></div>
        </div>
    </div>
</div>
<!-- Make popUp code ends hete -->

<script type="text/javascript">
    var isHashHaveModel = false;
    var modelId = '<%= ModelId%>';
    var versionId = '<%= VersionId%>';
    var pageId = '<%= PageId%>';
    var isCityPage = pageId == 55 ? true : false;
    bindCity(modelId, function () {
        ModelCar.PQ.preselectPQDropDown('drpCity', isCityPage);
    });
    function checkOnRoadPQ() {
        if (IsValid()) {
            $.cookie('_PQModelId', modelId, { path: '/' });
            setCityCookie('drpCity');
            $.cookie('_PQVersionId', versionId, { path: '/' });
            $.cookie('_PQPageId', pageId, { path: '/' });
            if (pageId != '36')
                window.location.href = '/new/quotation.aspx';
            else {
                try {
                    getNewPQ(pageId);
                    $("html, body").animate({ scrollTop: 0 }, "slow");
                    GB_hide();
                }
                catch (err) {
                    window.location.href = '/new/quotation.aspx';
                }
            }
        }
    }

    function IsValid() {
        var retVal = true;
        var errorMsg = "";
        if ($('#drpCity option:selected').val() <= 0) {
            ShakeFormView($("#drpCity, #spnCity"));
            retVal = false;
            $('#spnCity').text("Please select city");
        } else {
            $('#spnCity').text("");
        }
        return retVal;
    }
</script>
