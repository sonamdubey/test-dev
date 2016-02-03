<%@ Page Language="C#" AutoEventWireup="false" Inherits="BikewaleOpr.MobileApp.AppVersioning" Trace="false" Debug="false" %>
<!-- #Include file="/includes/headerNew.aspx" -->
<script type="text/ecmascript" src="/src/AjaxFunctions.js"></script>
<script src="/src/knockout.js" type="text/javascript"></script>
        <div style="margin:10px; padding:10px; border-style: solid;">
            <h2>Save/Edit Version</h2>
            <p><input type="checkbox" data-bind="checked: isLatest"/> Latest</p>
            <p><input type="checkbox" data-bind="checked: isSupported"/> Supported</p>
            <p>Version <span style="color: red" >&#42;</span> : <input type="text" data-bind="value: versionId" /></p>
            <p>Description : <input type="text" data-bind="value: description" /></p>
            <p><button id="btnUpdate" data-bind="click: saveAppVersion, text:updateText"></button></p>
            
        </div>
    
        <div style="margin:10px">
            <h2>Version Detail: </h2>
            <table border="1" style="border-collapse: collapse;" cellpadding="5" >
                <thead>
                    <tr>
                        <th>Version Id</th>
                        <th>Is Supported</th>
                        <th>Latest Version</th>
                        <th>Description</th>
                        <th>App Type</th>
                        <th>Edit</th>
                    </tr>
                </thead>
                <tbody data-bind="foreach: appVersions">
                    <tr>
                        <td style="text-align: center;" data-bind="text: Id"></td>
                        <td style="text-align: center;" data-bind="text: IsSupported"></td>
                        <td style="text-align: center;" data-bind="text: IsLatest"></td>
                        <td style="text-align: center;" data-bind="text: Description"></td>
                        <td style="text-align: center;" data-bind="text: AppType"></td>
                        <td style="text-align: center;" ><a href="#" data-bind="click: $parent.onEdit">Edit</a></td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div style="margin:10px">
            <span>Note : Default App Type is Android. </span>
        </div>
    <script type="text/javascript">
        var vmAppVersion = function() {
            var self = this;
            self.selectedAppType = ko.observable(3);
            self.appVersions = ko.observableArray([]);
            self.updateText = ko.observable('Save');
            self.appType = ko.observable();
            self.isLatest = ko.observable(false);
            self.isSupported = ko.observable(false);
            self.description = ko.observable();
            self.versionId = ko.observable();

            self.loadAppVersions = function () {
                if (self.selectedAppType() != undefined && self.selectedAppType() > 0) {
                    $.ajax({
                        type: "POST",
                        url: "/ajaxpro/BikewaleOpr.Common.AjaxAppVersion,BikewaleOpr.ashx",
                        data: '{"appType":' + self.selectedAppType() + '}',
                        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetAppVersions");  },
                        success: function (response) {
                            var responseJSON = eval('(' + response + ')');
                            var resObj = eval('(' + responseJSON.value + ')');
                            self.appVersions(ko.toJS(resObj));
                        }
                    });
                }
            }

            self.onEdit = function () {
                self.appType(this.AppType);
                self.isLatest(this.IsLatest);
                self.isSupported(this.IsSupported);
                self.description(this.Description);
                self.versionId(this.Id);
            }

            self.saveAppVersion = function () {
                if (!(/^\+?(0|[1-9]\d*)$/.test(self.versionId())) || self.versionId() == "" || self.versionId() == undefined)
                {
                    alert("Version Must be an integer and not Null");
                    return;
                }

                if (self.selectedAppType() != undefined && self.selectedAppType() > 0) {
                    $.ajax({
                        type: "POST",
                        url: "/ajaxpro/BikewaleOpr.Common.AjaxAppVersion,BikewaleOpr.ashx",
                        data: '{"isLatest":' + self.isLatest() + ', "isSupported":' + self.isSupported()
                            + ', "appType":' + 3 + ', "appVersionId":' + self.versionId() + ', "description": "' + self.description() + '" , "userId": "' + <%=Convert.ToInt32(CurrentUser.Id)%> + '"}',
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader("X-AjaxPro-Method", "SaveAppVersion");
                          },
                        success: function (response) {
                            var responseJSON = eval('(' + response + ')');
                            var resObj = eval('(' + responseJSON.value + ')');
                            self.appVersions(ko.toJS(resObj));
                            self.resetSaveDiv();
                        }
                    });
                }
            }

            self.resetSaveDiv = function () {
                self.isLatest(false);
                self.isSupported(false);
                self.description('');
                self.versionId('');
            }
        }
        var viewModel = new vmAppVersion();
        ko.applyBindings(viewModel);
        viewModel.selectedAppType(3);
        viewModel.loadAppVersions();
        
    </script>
<!-- #Include file="/includes/footerNew.aspx" -->