import React from 'react'
import { fetchSimilarBikes, updateSimilarBikesEmi } from '../../actionCreators/SimilarBikesEMI'

import SelectBikePopup from '../Shared/SelectBikePopup'
import EMISteps from './EMISteps'
import EMICalculator from './EMICalculator'
import SelectCityPopup from '../Shared/SelectCityPopup'
import ModelInfo from './ModelInfoContainer'
import SwiperContainer from '../Shared/SwiperContainer'
import SwiperSimilarBikesEMI from '../Shared/SwiperSimilarBikesEMI'
import { IsGlobalCityPresent, openPopupWithHash} from '../../utils/popUpUtils'
import { scrollTop } from '../../utils/scrollTo';

import { lockScroll, unlockScroll } from '../../utils/scrollLock';
import { slider } from '../../reducers/emiInterest';
import { getGlobalCity } from '../../utils/popUpUtils'
import { GetCatForNav, triggerGA } from '../../utils/analyticsUtils'
class EMITab extends React.Component {
  constructor(props) {
    super(props);
    this.scrollToNextPopup = this.scrollToNextPopup.bind(this);
    this.getSelectedBikeId = this.getSelectedBikeId.bind(this);
    this.getSelectedCityId = this.getSelectedCityId.bind(this);
    this.handleCityClick = this.handleCityClick.bind(this);
    this.handleBikeClick = this.handleBikeClick.bind(this);
    this.handleSimilarEMISwiperCardClick = this.handleSimilarEMISwiperCardClick.bind(this)
    this.handleSelectBikeClick = this.handleSelectBikeClick.bind(this)
    this.handleSelectCityClick = this.handleSelectCityClick.bind(this)
    this.state = {
      shouldscroll: false,
      isGlobalCityInList: true,
      shouldFetchSimilarBikes: true,
      shouldOpenEmiCalculator: true
    };
  }

  handleSelectBikeClick = () => {
    openPopupWithHash(this.props.openSelectBikePopup, this.props.closeSelectBikePopup, "SelectBike");
    lockScroll();
  }

  handleBikeClick = (item) => {
    this.props.fetchCity(item.modelId);
    this.scrollToNextPopup();
    this.props.selectModel(item.modelId);
    this.props.fetchSelectedBikeDetail(item.modelId);

    const currentCityId = this.getSelectedCityId(this.props);
    this.props.fetchBikeVersionList(item.modelId, currentCityId);
  }

  handleSelectCityClick = () => {
    openPopupWithHash(this.props.openSelectCityPopup, this.props.closeSelectCityPopup, "SelectCity");
    lockScroll();
  }

  handleCityClick = (item) => {
    let payload = {
      cityId: item.cityId,
      cityName: item.cityName,
      userChange: true
    }
    this.props.selectCity(payload);
    this.setState({ ...this.state, isGlobalCityInList: true, shouldFetchSimilarBikes: true });
    const currentBikeId = this.getSelectedBikeId(this.props);
    this.props.fetchBikeVersionList(currentBikeId, item.cityId);
  }

  handleSimilarEMISwiperCardClick = (modelObj, event) => {
    this.props.fetchSelectedBikeDetail(modelObj.modelId)
    const currentCityId = this.getSelectedCityId(this.props);
    this.props.fetchBikeVersionList(modelObj.modelId, currentCityId);
    this.state.shouldFetchSimilarBikes = true;
    let quickLinksTabElement = document.getElementById("quickLinksTab");
    scrollTop(window, this.refs.modelInfoComponent.base.getBoundingClientRect().top + (window.pageYOffset || document.documentElement.scrollTop) - quickLinksTabElement.offsetHeight)
    event.currentTarget.parentElement.scrollLeft = 0
    if (typeof (gaObj) != 'undefined') {
      triggerGA(gaObj.name, 'Similar_EMI_Widget_Clicked', this.props.selectBikePopup.Selection.modelName + '_' + modelObj.modelName); 
    }
  }
  componentWillReceiveProps(nextProps) {
		
		const {
			selectBikePopup,
      openEmiCalculator,
      fetchSimilarBikes
    } = this.props
    let downPayment = 0
    if (nextProps.selectBikePopup.Selection.version.versionList.length > 0 && nextProps.selectBikePopup.Selection.version.selectedVersionIndex > -1
      && (selectBikePopup.Selection.version.modelId != nextProps.selectBikePopup.Selection.version.modelId
        || selectBikePopup.Selection.version.selectedVersionIndex != nextProps.selectBikePopup.Selection.version.selectedVersionIndex
        || selectBikePopup.Selection.version.cityId != nextProps.selectBikePopup.Selection.version.cityId)) {
      openEmiCalculator(nextProps.selectBikePopup.Selection.version.versionList[nextProps.selectBikePopup.Selection.version.selectedVersionIndex].price)
      downPayment = nextProps.sliderDp.values[0] ? nextProps.sliderDp.values[0] : parseInt(.3 * nextProps.selectBikePopup.Selection.version.versionList[nextProps.selectBikePopup.Selection.version.selectedVersionIndex].price)
    }
   if(downPayment > 0 && selectBikePopup.Selection.version.modelId != nextProps.selectBikePopup.Selection.version.modelId
    || selectBikePopup.Selection.version.cityId != nextProps.selectBikePopup.Selection.version.cityId){
      fetchSimilarBikes({
        modelId: nextProps.selectBikePopup.Selection.version.modelId,
        cityId: nextProps.selectBikePopup.Selection.version.cityId,
        downPayment: downPayment,
        tenure: this.props.sliderTenure.values[0],
        rateOfInt: this.props.sliderInt.values[0]
      })
    }
	}
  componentDidUpdate(prevProps) {
    const {
      SimilarBikesEMI,
      updateSimilarBikesEmi,
      FinanceCityPopup,
      sliderDp,
      sliderTenure,
      sliderInt
    } = this.props;
    if ( prevProps.sliderDp.values[0] !== sliderDp.values[0] || prevProps.sliderInt.values[0] !== sliderInt.values[0] || prevProps.sliderTenure.values[0] !== sliderTenure.values[0]) {
      updateSimilarBikesEmi(SimilarBikesEMI, {
        downPayment: sliderDp.values[0],
        tenure: sliderTenure.values[0],
        rateOfInt: sliderInt.values[0]
      });
    }

    const currentCityId = this.getSelectedCityId(this.props);
    if (this.state.shouldscroll) {
      if (this.refs.emiSteps != null) {
        this.refs.emiSteps.scrollCityToView();
      }
      // Open city popup if current city not in fetched city list
      if (FinanceCityPopup != null && FinanceCityPopup.IsFetching == false &&  (FinanceCityPopup.Popular.length > 0 || FinanceCityPopup.Other.length > 0) && !(IsGlobalCityPresent(FinanceCityPopup.Popular, currentCityId) || IsGlobalCityPresent(FinanceCityPopup.Other, currentCityId))) {
        this.handleSelectCityClick();
        this.setState({ shouldscroll: false, isGlobalCityInList: false });
      }
      else if (!this.state.isGlobalCityInList) {
        this.setState({ ...this.state, isGlobalCityInList: true });
      }
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
      closeSelectBikePopup,
      fetchMakeModelList,
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

        {((this.getSelectedBikeId(this.props) === -1) || (this.getSelectedCityId(this.props) === -1)) &&
          <EMISteps ref="emiSteps" onSelectBikeClick={this.handleSelectBikeClick} onSelectCityClick={this.handleSelectCityClick} model={selectBikePopup.Selection} onSelectBikeClose={closeSelectBikePopup} onSelectCityClose={closeSelectCityPopup}/>}
        {((this.getSelectedBikeId(this.props) !== -1) && (this.getSelectedCityId(this.props) !== -1)) &&
          <ModelInfo ref="modelInfoComponent"/>}
        {
          <EMICalculator />
        }
        
        {
          SimilarBikesEMI != null && SimilarBikesEMI.data != null && SimilarBikesEMI.data.length > 0 && (
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
          <SelectCityPopup isActive={FinanceCityPopup.isActive} data={{ ...FinanceCityPopup, isGlobalCityInList: this.state.isGlobalCityInList }} onCloseClick={closeSelectCityPopup} onCityClick={this.handleCityClick} />
        }
      </div>);
  }
}
export default EMITab;
