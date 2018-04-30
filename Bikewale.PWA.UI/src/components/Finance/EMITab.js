import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import { openSelectBikePopup } from '../../actionCreators/SelectBikePopup'
import { fetchCity, openSelectCityPopup, setCity } from '../../actionCreators/financeCityPopupActionCreator'

import SelectBikePopup from '../Shared/SelectBikePopup'
import EMISteps from './EMISteps'
import EMICalculator from './EMICalculator'
import SelectCityPopup from '../Shared/SelectCityPopup'

class EMITab extends React.Component {
  constructor(props) {
    super(props);

    this.handleSelectBikeClick = this.handleSelectBikeClick.bind(this);
    this.handleSelectCityClick = this.handleSelectCityClick.bind(this);
    this.handleCityClick = this.handleCityClick.bind(this);
  }

  handleSelectBikeClick() {
    this.props.openSelectBikePopup();
  }
  
  handleSelectCityClick() {
    this.props.openSelectCityPopup();
  }

  handleCityClick(item) {
    let payload = {
      cityId: item.cityId,
      cityName: item.cityName,
      userChange: true
    }

    this.props.setCity(payload);
  }

  render() {
    const {
      selectBikePopup,
      FinanceCityPopup,
      fetchCity
    } = this.props

    return (
      <div>
        <div className="emi-calculator__head">
          <h2 className="emi-calculator__title">EMI Calculator</h2>
          <p className="emi-calculator__head-description">
            Know the tentative EMI for bike of your choice in 2 simple steps.
          </p>
        </div>
        <EMISteps />
        <span onClick={this.handleSelectBikeClick}>Select bike</span>
        <span onClick={this.handleSelectCityClick}>Select city</span>
        <SelectBikePopup isActive={selectBikePopup.isActive} />
        <SelectCityPopup isActive={FinanceCityPopup.isActive} data={FinanceCityPopup} fetchCity={fetchCity} onCityClick={this.handleCityClick} />
        <EMICalculator />
      </div>
    );
  }
}

var mapStateToProps = function (store) {
  return {
    selectBikePopup: store.getIn(['Finance', 'SelectBikePopup']),
    FinanceCityPopup: store.getIn(['Finance', 'FinanceCityPopup'])
  }
}

var mapDispatchToProps = function(dispatch) {
  return {
    openSelectBikePopup: bindActionCreators(openSelectBikePopup, dispatch),
    openSelectCityPopup: bindActionCreators(openSelectCityPopup, dispatch),
    fetchCity: bindActionCreators(fetchCity, dispatch),
    setCity: bindActionCreators(setCity, dispatch)
  }
}

module.exports = connect(mapStateToProps, mapDispatchToProps)(toJS(EMITab));
