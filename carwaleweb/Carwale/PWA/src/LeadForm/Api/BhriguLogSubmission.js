const LOG_SUBMISSION_ENDPOINT = "/bhrigu/events/";

const set = bhriguEvents => {
  let bhriguLogData = {
    ts: Date.now(),
    events: bhriguEvents
  };

  return fetch(LOG_SUBMISSION_ENDPOINT, {
    method: "POST",
    headers: {
      "Content-Type": "application/json;charset=UTF-8"
    },
    // This attribute is required to support cookies in old browsers
    credentials: "same-origin",
    body: JSON.stringify(bhriguLogData)
  }).then(response => {
    if (!response.ok) {
      throw response.statusText;
    }
    return response.text();
  });
};

const bhriguLogSubmissionApi = {
  set
};

export default bhriguLogSubmissionApi;
