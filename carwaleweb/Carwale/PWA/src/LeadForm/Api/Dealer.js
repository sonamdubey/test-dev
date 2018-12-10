const DEALER_ENDPOINT = "/api/dealers/ncs/?";

/**
 * Returns a promise to ensure that
 * get all the dealers
 * @return {Promise<>}
 */
const get = (campaignId, modelId, cityId) => {
  return fetch(
    `${DEALER_ENDPOINT}modelid=${modelId}&cityid=${cityId}&campaignid=${campaignId}`,
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

const dealersApi = {
  get
};

export default dealersApi;
