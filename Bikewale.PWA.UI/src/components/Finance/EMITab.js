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
    this.state = { shouldscroll: false };
  }

  handleSelectBikeClick = () => {
    this.props.openSelectBikePopup();
    lockScroll();
  }

  handleBikeClick = (item) => {
    this.props.selectModel(item.modelId);
    this.props.fetchCity(item.modelId);
    this.props.fetchSelectedBikeDetail(item.modelId);
    this.scrollToNextPopup();
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
      FinanceCityPopup,
      sliderTenure,
      sliderInt
    } = this.props;
    if (prevProps.sliderInt.values[0] !== sliderInt.values[0] || prevProps.sliderTenure.values[0] !== sliderTenure.values[0]) {
      updateSimilarBikesEmi(SimilarBikesEMI, {
        tenure: sliderTenure.values[0],
        rateOfInt: sliderInt.values[0]
      });
    }

    const currentCityId = this.getSelectedCityId(this.props);
    const currentBikeId = this.getSelectedBikeId(this.props);
    if (this.state.shouldscroll) {
      if (this.refs.emiSteps != null) {
        this.refs.emiSteps.scrollCityToView();
      }
      // Open city popup if current city not in fetched city list
      if (FinanceCityPopup != null && !(IsGlobalCityPresent(FinanceCityPopup.Popular, currentCityId) || IsGlobalCityPresent(FinanceCityPopup.Other, currentCityId))) {
        this.props.openSelectCityPopup();
        this.props.selectCity({
          cityId: -1,
          cityName: "",
          userChange: false
        });
        this.setState({ shouldscroll: false });
      }
    }
    // For any change in bike or city we fetch new bike version list
    if ((currentBikeId != this.getSelectedBikeId(prevProps) || currentCityId != this.getSelectedCityId(prevProps)) && currentCityId > 0 && currentBikeId > 0) {
      this.props.fetchBikeVersionList(currentBikeId, currentCityId);
    }
  }

  scrollToNextPopup = () => {
    this.setState({ shouldscroll: true });
  }

  getSelectedBikeId = (props) => {
    return props != null && props.selectBikePopup != null && props.selectBikePopup.Selection != null && props.selectBikePopup.Selection.modelId > 0 ?
      props.selectBikePopup.Selection.modelId : -1;
  }

  getSelectedCityId = (props) => {
    return props != null && props.FinanceCityPopup != null && props.FinanceCityPopup.Selection != null && props.FinanceCityPopup.Selection.cityId > 0 ?
      props.FinanceCityPopup.Selection.cityId : -1;
  }

  render() {
    const {
      selectBikePopup,
      FinanceCityPopup,
      SimilarBikesEMI,
      openSelectBikePopup,
      closeSelectBikePopup,
      fetchMakeModelList,
      openSelectCityPopup,
      closeSelectCityPopup
    } = this.props
    return (
      <div ref="emiTabsContainer">
        <div className="emi-calculator__head">
          <h2 className="emi-calculator__title">EMI Calculator</h2>
          <p className="emi-calculator__head-description">
            Know the tentative EMI for bike of your choice in 2 simple steps.
          </p>
        </div>

        {((this.getSelectedBikeId(this.props) === -1) || (this.getSelectedCityId(this.props) === -1)) &&
          <EMISteps ref="emiSteps" onSelectBikeClick={this.handleSelectBikeClick} onSelectCityClick={this.handleSelectCityClick} model={selectBikePopup.Selection} />}
        {((this.getSelectedBikeId(this.props) !== -1) && (this.getSelectedCityId(this.props) !== -1)) &&
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
          <SelectBikePopup isActive={selectBikePopup.isActive} data={selectBikePopup} onCloseClick={closeSelectBikePopup} onBikeClick={this.handleBikeClick} fetchMakeModelList={fetchMakeModelList} scrollToNextPopup={this.scrollToNextPopup} />
        }
        {
          FinanceCityPopup != null &&
          <SelectCityPopup isActive={FinanceCityPopup.isActive} data={FinanceCityPopup} onCloseClick={closeSelectCityPopup} onCityClick={this.handleCityClick} openSelectCityPopup={openSelectCityPopup} />
        }
      </div>);
  }
}
export default EMITab;
