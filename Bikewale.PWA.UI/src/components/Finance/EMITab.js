import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { toJS } from '../../immutableWrapperContainer'

import { openSelectBikePopup, closeSelectBikePopup } from '../../actionCreators/SelectBikePopup'
import { fetchCity, openSelectCityPopup, closeSelectCityPopup, selectCity, selectCityNext } from '../../actionCreators/FinanceCityPopup'
import { fetchSimilarBikes, updateSimilarBikesEmi } from '../../actionCreators/SimilarBikesEMI'

import SelectBikePopup from '../Shared/SelectBikePopup'
import EMISteps from './EMISteps'
import EMICalculator from './EMICalculator'
import SelectCityPopup from '../Shared/SelectCityPopup'
import ModelInfo from './ModelInfoContainer'
import SwiperContainer from '../Shared/SwiperContainer'
import SwiperSimilarBikesEMI from '../Shared/SwiperSimilarBikesEMI'
import { openEmiCalculator } from '../../actionCreators/emiDownPaymentSlider'
import { scrollTop } from '../../utils/scrollTo';

import { lockScroll, unlockScroll } from '../../utils/scrollLock';
import { slider } from '../../reducers/emiInterest';

class EMITab extends React.Component {
  constructor(props) {
    super(props);
  }

  handleSelectBikeClick = () => {
    this.props.openSelectBikePopup();
    lockScroll();
  }

  handleSelectCityClick = () => {
    this.props.openSelectCityPopup();
    lockScroll();
  }

  handleCityClick = (item) => {
    let payload = {
      cityId: item.cityId,
      cityName: item.cityName,
      userChange: true
    }

    this.props.selectCity(payload);
  }
  handleSimilarEMISwiperCardClick = (modelId, onRoadPrice) => {
    const {
      selectBikePopup,
      sliderDp,
      sliderTenure,
      sliderInt,
      fetchSimilarBikes,
      openEmiCalculator
    } = this.props
    fetchSimilarBikes({
      modelId: modelId,
      downPayment: onRoadPrice * .3,
      tenure: sliderTenure.values[0],
      rateOfInt: sliderInt.values[0]
    })
    openEmiCalculator(onRoadPrice)
    scrollTop(window, this.refs.emiTabsContainer.getBoundingClientRect().top + (window.pageYOffset || document.documentElement.scrollTop))
  }
  componentDidMount() {
    const {
      selectBikePopup,
      sliderDp,
      sliderTenure,
      sliderInt,
      fetchSimilarBikes,
      openEmiCalculator
    } = this.props;
    fetchSimilarBikes({
      modelId: selectBikePopup.Selection.modelId,
      downPayment: sliderDp.values[0],
      tenure: sliderTenure.values[0],
      rateOfInt: sliderInt.values[0]
    });
    openEmiCalculator(168021);
  }

  componentDidUpdate(prevProps, prevState) {
    const {
      SimilarBikesEMI,
      updateSimilarBikesEmi,
      sliderTenure,
      sliderInt
    } = this.props;
    if (prevProps.sliderInt.values[0] !== sliderInt.values[0]
      || prevProps.sliderTenure.values[0] !== sliderTenure.values[0]) {

      updateSimilarBikesEmi(SimilarBikesEMI, {
        tenure: sliderTenure.values[0],
        rateOfInt: sliderInt.values[0]
      });
    }
  }
  render() {
    const {
      selectBikePopup,
      FinanceCityPopup,
      fetchCity,
      closeSelectBikePopup,
      closeSelectCityPopup,
      selectCityNext,
      SimilarBikesEMI
    } = this.props

    return (
      <div ref="emiTabsContainer">
        <div className="emi-calculator__head">
          <h2 className="emi-calculator__title">EMI Calculator</h2>
          <p className="emi-calculator__head-description">
            Know the tentative EMI for bike of your choice in 2 simple steps.
          </p>
        </div>

        <EMISteps onSelectBikeClick={this.handleSelectBikeClick} onSelectCityClick={this.handleSelectCityClick} />

        <ModelInfo />

        <EMICalculator />

        {
          SimilarBikesEMI.data && (
            <SwiperContainer
              type="carousel__similar-emi"
              heading="Other Bikes in similar range"
              data={SimilarBikesEMI.data}
              carouselCard={SwiperSimilarBikesEMI}
              onCarouselCardClick={this.handleSimilarEMISwiperCardClick}
            />
          )
        }

        <SelectBikePopup isActive={selectBikePopup.isActive} onCloseClick={closeSelectBikePopup} />
        <SelectCityPopup isActive={FinanceCityPopup.isActive} data={FinanceCityPopup} fetchCity={fetchCity} onCityClick={this.handleCityClick} onCloseClick={closeSelectCityPopup} onNextClick={selectCityNext}/>

      </div>
    );
  }
}

var mapStateToProps = (store) => {
  return {
    selectBikePopup: store.getIn(['Finance', 'SelectBikePopup']),
    FinanceCityPopup: store.getIn(['Finance', 'FinanceCityPopup']),
    SimilarBikesEMI: store.getIn(['Finance', 'SimilarBikesEMI']),
    sliderDp: store.getIn(['Emi', 'VehicleDownPayment', 'slider']),
    sliderTenure: store.getIn(['Emi', 'VehicleTenure', 'slider']),
    sliderInt: store.getIn(['Emi', 'VehicleInterest', 'slider'])
  }
}

var mapDispatchToProps = (dispatch, store) => {
  return {
    openSelectBikePopup: bindActionCreators(openSelectBikePopup, dispatch),
    closeSelectBikePopup: bindActionCreators(closeSelectBikePopup, dispatch),
    openSelectCityPopup: bindActionCreators(openSelectCityPopup, dispatch),
    closeSelectCityPopup: bindActionCreators(closeSelectCityPopup, dispatch),
    selectCityNext: bindActionCreators(selectCityNext, dispatch),
    fetchCity: bindActionCreators(fetchCity, dispatch),
    selectCity: bindActionCreators(selectCity, dispatch),
    fetchSimilarBikes: bindActionCreators(fetchSimilarBikes, dispatch),
    updateSimilarBikesEmi: bindActionCreators(updateSimilarBikesEmi, dispatch),
    openEmiCalculator: bindActionCreators(openEmiCalculator, dispatch)
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(toJS(EMITab));
