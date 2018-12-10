function getPropertyId() {
    var queryString = window.location.search.substring(1);
    var queryParams = queryString.split('&');
    var propertyid, keyValue;
    for (var index in queryParams) {
        keyValue = queryParams[index].split('=');
        if (keyValue.length == 2 && keyValue[0] == 'propertyid') {
            propertyid = keyValue[1];
            break;
        }
    }
    return propertyid;
}

function loadLeadForm() {
    var campaignCta = document.querySelector('[campaigncta]');
    var baseUrl = window.location.href.split('?')[0];
    if (campaignCta) {
        var modelid = campaignCta.getAttribute("modelid");
        var versionid = campaignCta.getAttribute("versionid");
        var userlocation = campaignCta.getAttribute("userlocation");
        var campaignid_dealerid = campaignCta.getAttribute("campaignid_dealerid");
        var others = campaignCta.getAttribute("others");
        var propertyid = getPropertyId();
        var testdrivechecked = campaignCta.getAttribute("testdrivechecked");
        var attributes = {
            modelid: modelid ? { value: modelid } : 0,
            versionid: versionid ? { value: versionid } : 0,
            userlocation: userlocation ? { value: userlocation } : 0,
            campaignid_dealerid: campaignid_dealerid ? { value: campaignid_dealerid } : 0,
            others: others ? { value: others } : 0,
            propertyid: propertyid ? { value: propertyid } : 0,
            testdrivechecked: testdrivechecked ? { value: testdrivechecked } : 0
        }
        window.history.replaceState(null, "", baseUrl);
        window.CampaignCTAClickHandler(attributes);
    }
    else {
        window.history.replaceState(null, "", baseUrl);
    }
    
}

loadLeadForm();
