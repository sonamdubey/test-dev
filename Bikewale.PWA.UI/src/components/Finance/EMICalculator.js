import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import { openSelectBikePopup } from '../../actionCreators/SelectBikePopup'

import SelectBikePopup from '../Shared/SelectBikePopup'
import EMICalculatorSelection from './EMICalculatorSelection'

class EMICalculator extends React.Component {
  constructor(props) {
    super(props);

    this.handleSelectBikeClick = this.handleSelectBikeClick.bind(this);
  }

  handleSelectBikeClick() {
		this.props.openSelectBikePopup();
  }

  render() {
    return (
      <div>
        <div className="emi-calculator__head">
          <h2 className="emi-calculator__title">EMI Calculator</h2>
          <p className="emi-calculator__head-description">
            Know the tentative EMI for bike of your choice in 2 simple steps.
          </p>
        </div>
        <EMICalculatorSelection />
        <span onClick={this.handleSelectBikeClick}>Select bike</span>
				<SelectBikePopup isActive={this.props.SelectBikePopup.open} />
      </div>
    );
  }
}

var mapStateToProps = function (store) {
	return {
		SelectBikePopup: store.getIn(['Finance', 'SelectBikePopup'])
	}
}

var mapDispatchToProps = function(dispatch) {
	return {
		openSelectBikePopup: bindActionCreators(openSelectBikePopup, dispatch)
	}
}

module.exports = connect(mapStateToProps, mapDispatchToProps)(toJS(EMICalculator));
