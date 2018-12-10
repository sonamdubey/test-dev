const CITY_ENDPOINT = "/api/campaign/";

/**
 * Returns a promise to ensure that
 * get all the cities
 * @return {Promise<>}
 */
const get = (campaignId, modelId) => {
  return fetch(`${CITY_ENDPOINT}${campaignId}/cities/?modelid=${modelId}`,
  {
    // This attribute is required to support cookies in old browsers
    credentials: "same-origin"
  }).then(
    response => {
      if (!response.ok || response.status == 204) {
        throw response.statusText;
      }
      return response.json();
    }
  );
};

const citiesApi = {
  get
};

export default citiesApi;
