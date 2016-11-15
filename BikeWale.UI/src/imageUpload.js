/*
    Author      : Sumit Kate
    Created On  : 10 Nov 2016
    Description : Utility js to upload image to amazon S3
*/
function ImageUploadUtility() {
    var self = this;
    self.baseURL = "";
    self.apiKey = "";
    self.request = {};               //object to be used to generate request , ex : {"categoryId":2,"itemId":634738,"aspectRatio":"1.777","isWaterMark":1,"isMaster":1,"isMain":1}
    self.response = {};               //to be used internally within this namespace
    self.getList = null;             //gets list of processed images
    self.status = false;             //flag to check whether image was sent for processing to Rabbit MQ
    self.photoId = null;
    self.itemId = null;

        //function to get token from server 
    self.getToken = function (request) {        
        return $.ajax({
            type: "POST", async: false, url: "/api/image/request/", dataType: 'json',
            headers: { 'apiKey': self.apiKey },
            contentType: "application/json;charset=utf-8", data: JSON.stringify(request),
            success: function (response) {
                if (response != null && response != "")
                    self.response = response;
            }
        });
    };

        /* function to upload image to amazon S3 */
    self.upload = function (file) {
        try {
            $.when(self.getToken(self.request)).done()
            {
                var response = self.response;
                var awsURI = response.uri;
                var objData = new FormData();
                var xmlhttp = "";

                objData.append('key', self.response.key);
                objData.append('acl', 'public-read');
                objData.append('success_action_status', '201');
                objData.append('Content-Type', file.type);
                objData.append('AWSAccessKeyId', response.accessKeyId);
                objData.append('policy', response.policy)
                objData.append('signature', response.signature);
                objData.append("file", file);

                if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
                    xmlhttp = new XMLHttpRequest();
                }
                else {// code for IE6, IE5
                    xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
                }
                xmlhttp.addEventListener("load", self.uploadComplete, false);
                xmlhttp.open('POST', awsURI, false);
                xmlhttp.send(objData);
                xmlhttp.onreadystatechange = function () {
                    if (xmlhttp.readyState == XMLHttpRequest.DONE) {
                        alert(xmlhttp.responseText);
                    }
                }

            }
        }
        catch (e) {
            alert("some error has occurred");
        }
    };
        /* This event is raised when the amazon S3 sends back a response */
    self.uploadComplete = function (event) {
        var xmlResponse = event.target.responseXML.getElementsByTagName("PostResponse");
        if (xmlResponse) {
                var key = xmlResponse[0].getElementsByTagName("Key")[0].textContent;
                var requestObject = { key: key, id: self.response.id, uri: self.response.uri, accessKeyId: self.response.accessKeyId, policy: self.response.policy, signature: self.response.signature, photoId: self.photoId };

                //call api that processes image through Rabbit MQ and saves its details in database
                $.ajax({
                    type: "POST", async: false, url: "/api/image/", dataType: 'json', contentType: "application/json;charset=utf-8",
                    headers: { 'apiKey': self.apiKey },
                    data: JSON.stringify(requestObject)
                }).done(function (response) {
                    if (response != null)
                        self.status = response;
                }).fail(function () { self.status = false; });
            };
    };
        /* This function fetches processed images details */
    self.fetchProcessedImages = function (ids) {
        if (ids != "") {
            return $.ajax({
                type: "GET", url: "/api/image/" + ids + "/", dataType: 'json', async: false, headers: { 'apiKey': self.apiKey }
            }).done(function (response) {
                if (response != null && response != "")
                    self.getList = response;
            }).fail(function () { alert("Some error occurred") });
        }
    };
}