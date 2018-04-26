import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import { openSelectBikePopup, closeSelectBikePopup } from '../../actionCreators/SelectBikePopup'
import { fetchCity, openSelectCityPopup, closeSelectCityPopup, selectCity } from '../../actionCreators/FinanceCityPopup'

import SelectBikePopup from '../Shared/SelectBikePopup'
import EMISteps from './EMISteps'
import SelectCityPopup from '../Shared/SelectCityPopup'
import ModelInfo from './ModelInfoContainer'

class EMITab extends React.Component {
  constructor(props) {
    super(props);
  }
  
  handleSelectBikeClick = () => {
    this.props.openSelectBikePopup();
  }
  
  handleSelectCityClick = () => {
    this.props.openSelectCityPopup();
  }

  handleCityClick = (item) => {
    let payload = {
      cityId: item.cityId,
      cityName: item.cityName,
      userChange: true
    }

    this.props.selectCity(payload);
  }

  render() {
    const {
      selectBikePopup,
      FinanceCityPopup,
      fetchCity,
      closeSelectBikePopup,
      closeSelectCityPopup
    } = this.props

    return (
      <div>
        <div className="emi-calculator__head">
          <h2 className="emi-calculator__title">EMI Calculator</h2>
          <p className="emi-calculator__head-description">
            Know the tentative EMI for bike of your choice in 2 simple steps.
          </p>
        </div>
        <ModelInfo />
        <EMISteps />
        <span onClick={this.handleSelectBikeClick}>Select bike</span>
        <span onClick={this.handleSelectCityClick}>Select city</span>
        <SelectBikePopup isActive={selectBikePopup.isActive} onCloseClick={closeSelectBikePopup} />
        <SelectCityPopup isActive={FinanceCityPopup.isActive} data={FinanceCityPopup} fetchCity={fetchCity} onCityClick={this.handleCityClick} onCloseClick={closeSelectCityPopup} />
      </div>
    );
  }
}

var mapStateToProps = (store) => {
  return {
    selectBikePopup: store.getIn(['Finance', 'SelectBikePopup']),
    FinanceCityPopup: store.getIn(['Finance', 'FinanceCityPopup'])
  }
}

var mapDispatchToProps = (dispatch) => {
  return {
    openSelectBikePopup: bindActionCreators(openSelectBikePopup, dispatch),
    closeSelectBikePopup: bindActionCreators(closeSelectBikePopup, dispatch),
    openSelectCityPopup: bindActionCreators(openSelectCityPopup, dispatch),
    closeSelectCityPopup: bindActionCreators(closeSelectCityPopup, dispatch),
    fetchCity: bindActionCreators(fetchCity, dispatch),
    selectCity: bindActionCreators(selectCity, dispatch)
  }
}

module.exports = connect(mapStateToProps, mapDispatchToProps)(toJS(EMITab));
