import React from 'react'
import { fetchSimilarBikes, updateSimilarBikesEmi } from '../../actionCreators/SimilarBikesEMI'

import SelectBikePopup from '../Shared/SelectBikePopup'
import EMISteps from './EMISteps'
import EMICalculator from './EMICalculator'
import SelectCityPopup from '../Shared/SelectCityPopup'
import ModelInfo from './ModelInfoContainer'
import SwiperContainer from '../Shared/SwiperContainer'
import SwiperSimilarBikesEMI from '../Shared/SwiperSimilarBikesEMI'
import { IsGlobalCityPresent } from '../../utils/popUpUtils'
import { scrollTop } from '../../utils/scrollTo';

import { lockScroll, unlockScroll } from '../../utils/scrollLock';
import { slider } from '../../reducers/emiInterest';

class EMITab extends React.Component {
  constructor(props) {
    super(props);
    this.scrollToNextPopup = this.scrollToNextPopup.bind(this);
    this.getSelectedBikeId = this.getSelectedBikeId.bind(this);
    this.getSelectedCityId = this.getSelectedCityId.bind(this);
    this.setBikeChange = this.setBikeChange.bind(this);
    this.checkBikeChange = this.checkBikeChange.bind(this);
    this.state = { shouldscroll: false, isBikeChanged: false };
  }

  componentDidMount() {
    this.props.fetchSimilarBikes();
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

  componentDidUpdate(prevProps) {
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
    if (this.state.shouldscroll) {
      if (this.refs.emiSteps != null) {
        this.refs.emiSteps.scrollCityToView();
      }
      if (this.props.FinanceCityPopup && ((this.props.FinanceCityPopup.Popular.length > 0 || this.props.FinanceCityPopup.Other.length > 0) && !(IsGlobalCityPresent(this.props.FinanceCityPopup.Popular, this.getSelectedCityId()) || IsGlobalCityPresent(this.props.FinanceCityPopup.Other, this.getSelectedCityId())))) {
        this.props.openSelectCityPopup();
        this.setState({ ...this.state, shouldscroll: false });
      }
    }
  }

  setBikeChange = (value) => {
    this.setState({ ...this.state, isBikeChanged: value });
  }

  checkBikeChange = () => {
    return this.state.isBikeChanged;
  }

  scrollToNextPopup = () => {
    this.setState({ ...this.state, shouldscroll: true });
  }

  getSelectedBikeId = () => {
    return this.props != null && this.props.selectBikePopup != null && this.props.selectBikePopup.Selection != null && this.props.selectBikePopup.Selection.modelId > 0 ?
      this.props.selectBikePopup.Selection.modelId : -1;
  }

  getSelectedCityId = () => {
    return this.props != null && this.props.FinanceCityPopup != null && this.props.FinanceCityPopup.Selection != null && this.props.FinanceCityPopup.Selection.cityId > 0 ?
      this.props.FinanceCityPopup.Selection.cityId : -1;
  }

  render() {
    const {
      selectBikePopup,
      FinanceCityPopup,
      SimilarBikesEMI,
      openSelectBikePopup,
      closeSelectBikePopup,
      fetchMakeModelList,
      fetchBikeVersionList,
      openSelectCityPopup,
      closeSelectCityPopup,
      selectCityNext,
      fetchCity,
      selectCity,
      selectModel,
      fetchSelectedBikeDetail
    } = this.props
    const currentCityId = FinanceCityPopup.Selection != null && FinanceCityPopup.Selection.cityId != null ? FinanceCityPopup.Selection.cityId : -1;
    return (
      <div ref="emiTabsContainer">
        <div className="emi-calculator__head">
          <h2 className="emi-calculator__title">EMI Calculator</h2>
          <p className="emi-calculator__head-description">
            Know the tentative EMI for bike of your choice in 2 simple steps.
          </p>
        </div>

        {((this.getSelectedBikeId() === -1) || (this.getSelectedCityId() === -1)) &&
          <EMISteps ref="emiSteps" onSelectBikeClick={this.handleSelectBikeClick} onSelectCityClick={this.handleSelectCityClick} model={selectBikePopup.Selection} />}
        {((this.getSelectedBikeId() !== -1) && (this.getSelectedCityId() !== -1)) &&
          <ModelInfo />}

        <EMICalculator />

        {
          SimilarBikesEMI != null && SimilarBikesEMI.data != null && (
            <SwiperContainer
              type="carousel__similar-emi"
              heading="Other Bikes in similar range"
              data={SimilarBikesEMI.data}
              carouselCard={SwiperSimilarBikesEMI}
              onCarouselCardClick={this.handleSimilarEMISwiperCardClick}
            />
          )
        }

        {
          selectBikePopup != null &&
          <SelectBikePopup isActive={selectBikePopup.isActive} data={selectBikePopup} onCloseClick={closeSelectBikePopup} selectBike={selectModel} fetchMakeModelList={fetchMakeModelList} scrollToNextPopup={this.scrollToNextPopup} getSelectedCityId={this.getSelectedCityId} setBikeChange={this.setBikeChange} fetchBikeVersionList={fetchBikeVersionList} />
        }
        {
          FinanceCityPopup != null &&
          <SelectCityPopup isActive={FinanceCityPopup.isActive} data={FinanceCityPopup} fetchCity={fetchCity} onCityClick={this.handleCityClick} onCloseClick={closeSelectCityPopup} openSelectCityPopup={openSelectCityPopup} getSelectedBikeId={this.getSelectedBikeId} checkBikeChange={this.checkBikeChange} setBikeChange={this.setBikeChange} fetchBikeVersionList={fetchBikeVersionList} />
        }
      </div>);
  }
}
export default EMITab;
