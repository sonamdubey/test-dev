import {
    setPricesData,
    setEMIModel,
    setEmiResult,
    updateEmiModel,
} from '../actionCreators/emiData'

import {
    showEmiPopupState,
    hideEmiPopupState
} from '../actionCreators/emiPopupState'

import {
  EmiCalculation
} from '../components/EmiCalculator'

let emiPricesEvents = {
    setPricesData,
    showEmiPopupState,
    hideEmiPopupState,
    setEMIModel,
    EmiCalculation,
    updateEmiModel,
}

window.EMI_PRICES_EVENTS = emiPricesEvents;
