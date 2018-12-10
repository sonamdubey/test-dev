import { combineReducers } from "redux";
import { screen } from "./Screen";
import { buyerInfo } from "./Buyer";
import { recommendation } from "./Recommendation";
import { MLASellers } from "./MLASellers";
import { location } from "GlobalReducer/Location";
import { log } from "./Log";
import { others } from "./Others";
import { campaign } from "./campaign";
import { page } from "../../reducers/Page";
import { leadId } from "../Reducers/LeadId";
import { propId } from "../Reducers/PropId";
import { isLeadFormVisible } from "../Reducers/IsLeadFormVisible";
import { isCitySet } from "../Reducers/IsCitySet";
import { interactionId } from "../Reducers/InteractionId";
import { isCityChanged } from "../Reducers/IsCityChanged";

const leadForm = combineReducers({
  buyerInfo,
  leadId,
  recommendation,
  MLASellers,
  others
});

const NC = combineReducers({
  leadForm,
  campaign
});

const leadClickSource = combineReducers({
  propId,
  page,
  isCitySet
});

const rootReducer = combineReducers({
  NC,
  location,
  log,
  screenData: screen,
  leadClickSource,
  isLeadFormVisible: isLeadFormVisible,
  interactionId,
  isCityChanged
});

export default rootReducer;
