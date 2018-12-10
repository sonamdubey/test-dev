import { mapBhriguLogs } from "./ObjectMapping";
import bhriguLogSubmissionApi from "../Api/BhriguLogSubmission";

export function submitBhriguLogs(logData) {
  let bhriguEvents = mapBhriguLogs(logData);
  bhriguLogSubmissionApi
    .set(bhriguEvents)
    .then(status => {})
    .catch(error => {
      console.log("Bhrigu Submission Failed, " + error);
    });
}
