<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.content.frmManageAppVersions" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<%--<script type="text/javascript" src="/src/common/common.js?V1.1"></script>--%>
<script type="text/ecmascript" src="/src/AjaxFunctions.js"></script>
<script src="/src/knockout.js" type="text/javascript"></script>

<script type="text/javascript">
    function vmAppVersion() {
        var self = this;
        self.selectedAppType = ko.observable();
        self.appVersions = ko.observableArray([]);
        self.appTypes = ko.observableArray();        
        self.loadAppVersions = function () {
            if (self.selectedAppType() != undefined && self.selectedAppType() > 0) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikewaleOpr.Common.AjaxAppVersion,BikewaleOpr.ashx",
                    data: '{"appType":' + self.selectedAppType() + '}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetAppVersions"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        self.appVersions(ko.toJS(resObj));
                    }
                });
            }
        }
        self.saveAppVersion = function () {
            if (self.selectedAppType() != undefined && self.selectedAppType() > 0) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/BikewaleOpr.Common.AjaxAppVersion,BikewaleOpr.ashx",
                    data: '{"isLatest":' + + ', "isSupported":' + + ', "appType":' + + ', "appVersionId":' + + ', "description": ' + + ', "userId":' +  + '}',
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "SaveAppVersion"); },
                    success: function (response) {
                        var responseJSON = eval('(' + response + ')');
                        var resObj = eval('(' + responseJSON.value + ')');
                        self.appVersions(ko.toJS(resObj));
                    }
                });
            }
        }
    }
    var vmApp = new vmAppVersion();
    vmApp.selectedAppType(3);
    vmApp.loadAppVersions();
</script>
<!-- #Include file="/includes/footerNew.aspx" -->
