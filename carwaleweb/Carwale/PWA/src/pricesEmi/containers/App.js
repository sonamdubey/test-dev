import React from 'react'
import PropTypes from 'prop-types'

import appstyles from '../../../style/prices.scss';
import SpeedometerLoader from '../../../src/components/SpeedometerLoader';

import Toast from '../../components/Toast'
import EmiPrices from './EmiPrices'
const bodyUnlock = () =>{
  document.body.classList.add('bodyunlock')
  document.body.classList.remove('bodylock')
}
class App extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      addClass: false,
      toggle: false,
      emiTogglePopUp: false
   }
  }
  removeOverlay = () => {
      this.setState({addClass: false, toggle: false});
  }
  toggleEmiPopupNew = () => {
    EMI_PRICE_STORE.dispatch(EMI_PRICES_EVENTS.hideEmiPopupState(false));
    this.removeOverlay();
    bodyUnlock();
}
  render() {
    return (
      <div>
        <div className={this.state.isLoaderVisible ? "" : "hide"}>
					{SpeedometerLoader()}
				</div>
        <div className="emi-popup_blackout-window" onClick={this.toggleEmiPopupNew}></div>
        <EmiPrices />
        <Toast />
      </div>
    )
  }
}

export default App
