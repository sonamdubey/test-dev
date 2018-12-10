const RECOMMENDATION_ENDPOINT = "/api/v1/campaign/recommendations/?";

/**
 * Returns a promise to ensure that
 * get all the Recommendation details
 * @return {Promise<>}
 */
const get = (modelId, platformId, location, userMobileNumber) => {
  return fetch(
    `${RECOMMENDATION_ENDPOINT}modelId=${modelId}&cityId=${
      location.cityId
    }&platformId=${platformId}&mobile=${userMobileNumber}&recommendationcount=${
      platformId == 1 ? 4 : 3
    }&areaId=${location.areaId}&boost=true`,
    {
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

const recommendationApi = {
  get
};

export default recommendationApi;
