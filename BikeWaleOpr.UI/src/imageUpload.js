function ImageUploadUtility() {
    var self = this;
    self.baseURL = "";
    self.apiKey = "";
    self.request = {};
    self.response = {};
    self.getList = null;
    self.status = false;
    self.photoId = null;
    self.itemId = null;

    //function to get token from server 
    self.getToken = function (request) {
        return $.ajax({
            type: "POST",
            async: false,
            url: "/api/image/request/",
            dataType: 'json',
            crossDomain: true,
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

                objData.append('key', self.response.originalImagePath);
                objData.append('acl', 'public-read');
                objData.append('success_action_status', '201');
                objData.append('Content-Type', file.type);
                objData.append('x-amz-credential', response.accessKeyId + "/" + response.datetimeiso + "/ap-south-1/s3/aws4_request");
                objData.append('x-amz-algorithm', 'AWS4-HMAC-SHA256');
                objData.append('x-amz-date', response.datetimeisolong);
                objData.append('policy', response.policy);
                objData.append('x-amz-signature', response.signature);
                objData.append("file", file);

                var awsReqPromise = $.ajax({
                    url: awsURI,
                    data: objData,
                    async: false,
                    processData: false,
                    contentType: false,
                    type: "POST",
                    dataType: "xml"

                });
                awsReqPromise.done(function (response) { self.uploadComplete(response, file) });
                awsReqPromise.fail(function () { self.status = false; });
            }
        }
        catch (e) {
            console.log("some error has occurred");
        }
    };
    /* This event is raised when the amazon S3 sends back a response */
    self.uploadComplete = function (response, file) {
        if (response) {
            var xmlResponse = response.getElementsByTagName("PostResponse");;
            if (xmlResponse) {
                var originalImagePath = xmlResponse[0].getElementsByTagName("Key")[0].textContent;
                var requestObject = { key: self.response.key, originalImagePath: originalImagePath, id: self.response.id, uri: self.response.uri, accessKeyId: self.response.accessKeyId, policy: self.response.policy, signature: self.response.signature, photoId: self.photoId };

                //call api that processes image through Rabbit MQ and saves its details in database
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/api/image/savepath/",
                    dataType: 'json',
                    contentType: "application/json;charset=utf-8",
                    crossDomain: true,
                    data: JSON.stringify(requestObject)
                }).done(function (response) {
                    if (response != null)
                        self.status = response;                    
                }).fail(function () { self.status = false;});
            };
        }
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