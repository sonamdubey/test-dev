const LEAD_SUBMISSION_ENDPOINT = "/api/dealer/inquiries/";

/**
 * Returns a promise to ensure that
 * buyer details have been posted
 * @return {Promise<>}
 */
const set = lead => {
  return fetch(LEAD_SUBMISSION_ENDPOINT, {
    method: "POST",
    headers: {
      "Content-Type": "application/json;charset=UTF-8"
    },
    // This attribute is required to support cookies in old browsers
    credentials: "same-origin",
    body: JSON.stringify(lead)
  }).then(response => {
    if (!response.ok || response.status == 204) {
      throw response.statusText;
    }
    return response.text();
  });
};

const leadSubmissionApi = {
  set
};

export default leadSubmissionApi;
