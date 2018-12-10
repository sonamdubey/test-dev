const GET_CAMPAIGN_ENDPOINT = "/api/dealerAd/?";

const get = (location, modelDetail, platformId, applicationId) => {
  return fetch(
    GET_CAMPAIGN_ENDPOINT +
      "modelId=" +
      modelDetail.modelId +
      "&cityid=" +
      location.cityId +
      "&platformid=" +
      platformId +
      "&areaid=" +
      location.areaId +
      "&applicationId=" +
      applicationId,
    {
      headers: { sourceid: applicationId },
      // This attribute is required to support cookies in old browsers
      credentials: "same-origin"
    }
  ).then(response => {
    if (!response.ok || response.status == 204) {
      throw response.statusText;
    }
    return response.json();
  });
};

const campaignApi = {
  get
};

export default campaignApi;
