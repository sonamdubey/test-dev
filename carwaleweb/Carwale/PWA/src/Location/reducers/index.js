import { combineReducers } from 'redux';

import isActive, { shouldShowCrossIcon } from './Popup';

import { location } from '../../reducers/Location';

import { toast } from '../../reducers/Toast';

const rootReducer = combineReducers({
  isActive,
  location,
  toast,
  shouldShowCrossIcon,
})

export default rootReducer;
