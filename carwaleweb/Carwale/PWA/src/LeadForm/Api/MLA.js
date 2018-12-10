const MLA_ENDPOINT = "/api/v3/campaigns/?";

/**
 * Returns a promise to ensure that
 * get all the MLA sellers
 * @return {Promise<>}
 */
const get = (cityId, areaId, modelId, page) => {
  return fetch(
    `${MLA_ENDPOINT}modelid=${modelId}&cityId=${cityId}&platformId=${
      page.platform.id
    }&applicationId=${
      page.applicationId
    }&areaId=${areaId}&isDealerLocator=true&dealerAdminFilter=true`,
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

const MLAApi = {
  get
};

export default MLAApi;
