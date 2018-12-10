import { fireInteractiveTracking } from "../../utils/Analytics";
import { setLeadId } from "../ActionCreators/LeadId";
import LeadSubmissionApi from "../Api/LeadSubmission";
import { setErrorLog, setInfoLog } from "../ActionCreators/LogState";
import store from "../store";
import { screenType } from "../Enum/ScreenEnum";

export function submitLead(lead, action = "") {
  let state = store.getState();
  let interactionId = state.interactionId;
  let screenId = state.screenData.screen;

  LeadSubmissionApi.set(lead)
    .then(status => {
      let screenName = getScreenName(screenId);
      store.dispatch(setLeadId(status));
      store.dispatch(
        setInfoLog(
          `${screenName} lead submitted with encryptedLeadId, ${status}`,
          interactionId
        )
      );
    })
    .catch(error => {
      if (action != "") {
        let platformName = state.leadClickSource.page.platform.name;
        let pageName = state.leadClickSource.page.page.name;
        let category = `LeadForm-${platformName}-${pageName}`;
        let label = "";
        fireInteractiveTracking(category, action, label);
      }

      store.dispatch(
        setErrorLog(
          `Error: ${error}. Found in LeadSubmission API`,
          interactionId
        )
      );
    });
}

function getScreenName(screenId) {
  let screenNames = Object.keys(screenType);
  let screenName;
  for (screenName of screenNames) {
    if (screenType[screenName] == screenId) {
      return screenName;
    }
  }
}
