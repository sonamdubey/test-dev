import React from 'react'
import SelectBikePopup from '../Shared/SelectBikePopup'
import EMISteps from './EMISteps'
import EMICalculator from './EMICalculator'
import SelectCityPopup from '../Shared/SelectCityPopup'
import ModelInfo from './ModelInfoContainer'
import SwiperContainer from '../Shared/SwiperContainer'
import SwiperSimilarBikesEMI from '../Shared/SwiperSimilarBikesEMI'
import { IsGlobalCityPresent, openPopupWithHash, closePopupWithHash } from '../../utils/popUpUtils'
import { triggerGA } from '../../utils/analyticsUtils'
class EMITab extends React.Component {
  constructor(props) {
    super(props);
    this.getSelectedBikeId = this.getSelectedBikeId.bind(this);
    this.getSelectedCityId = this.getSelectedCityId.bind(this);
    this.handleCityClick = this.handleCityClick.bind(this);
    this.handleBikeClick = this.handleBikeClick.bind(this);
    this.handleSimilarEMISwiperCardClick = this.handleSimilarEMISwiperCardClick.bind(this);
    this.handleSelectBikeClick = this.handleSelectBikeClick.bind(this);
    this.handleSelectCityClick = this.handleSelectCityClick.bind(this);
    this.checkNetworkFailure = this.checkNetworkFailure.bind(this);
    this.setCity = this.setCity.bind(this);
    this.setToast = this.setToast.bind(this);
    this.state = {
      shouldscroll: false,
      isGlobalCityInList: true,
      shouldFetchSimilarBikes: true,
      shouldOpenEmiCalculator: true,
      isFetching: false,
      currentSelectedBikeId: -1

    };
  }

  handleSelectBikeClick = () => {
    openPopupWithHash(this.props.openSelectBikePopup, this.props.closeSelectBikePopup, "SelectBike");
    if (this.state.currentSelectedBikeId <= 0) {
      if (typeof gaObj != 'undefined') {
        triggerGA(gaObj.name, 'Select_Bike_Clicked', '');
      }
    }
    else {
      if (typeof gaObj != 'undefined') {
        triggerGA(gaObj.name, 'Model_Change_Initiated', 'Existing Model - ' + this.props.selectBikePopup.Selection.modelName);
      }
    }
  }

  handleBikeClick = (item) => {
    this.props.selectModel(item.modelId);
    this.props.fetchCity(item.modelId);
    this.setState({ ...this.state, shouldscroll: true, currentSelectedBikeId: item.modelId });
    this.props.fetchSelectedBikeDetail(item.modelId);
    const currentCityId = this.props.FinanceCityPopup.currentGlobalCityId;
    if (currentCityId > 0) {
      this.props.fetchBikeVersionList(item.modelId, currentCityId);
      this.props.fetchSimilarBikes({
        modelId: item.modelId,
        cityId: currentCityId,
        downPayment: this.props.sliderDp.values[0],
        tenure: this.props.sliderTenure.values[0],
        rateOfInt: this.props.sliderInt.values[0]
      })
      this.state.isFetching = true
    }
  }

  handleSelectCityClick = () => {
    openPopupWithHash(this.props.openSelectCityPopup, this.props.closeSelectCityPopup, "SelectCity");
  }

  setCity = (item) => {
    let payload = {
      cityId: item.cityId,
      cityName: item.cityName,
      userChange: true
    }
    this.props.selectCity(payload);
  }

  handleCityClick = (item) => {
    this.setCity(item);
    this.setState({ ...this.state, isGlobalCityInList: true, shouldFetchSimilarBikes: true });
    const currentBikeId = this.getSelectedBikeId(this.props);
    this.props.fetchBikeVersionList(currentBikeId, item.cityId);
    this.props.fetchSimilarBikes({
      modelId: currentBikeId,
      cityId: item.cityId,
      downPayment: this.props.sliderDp.values[0],
      tenure: this.props.sliderTenure.values[0],
      rateOfInt: this.props.sliderInt.values[0]
    })
    this.state.isFetching = true
  }

  handleSimilarEMISwiperCardClick = (modelObj, event) => {
    let quickLinksTabElement = document.getElementById("quickLinksTab");
    window.scrollTo(0, this.refs.modelInfoComponent.base.getBoundingClientRect().top + (window.pageYOffset || document.documentElement.scrollTop) - quickLinksTabElement.offsetHeight);
    if (typeof gaObj != 'undefined') {
      triggerGA(gaObj.name, 'Similar_EMI_Widget_Clicked', this.props.selectBikePopup.Selection.modelName + '_' + modelObj.modelName);
    }
    this.props.fetchSelectedBikeDetail(modelObj.modelId)
    const currentCityId = this.getSelectedCityId(this.props);
    this.props.fetchBikeVersionList(modelObj.modelId, currentCityId);
    this.props.fetchSimilarBikes({
      modelId: modelObj.modelId,
      cityId: currentCityId,
      downPayment: this.props.sliderDp.values[0],
      tenure: this.props.sliderTenure.values[0],
      rateOfInt: this.props.sliderInt.values[0]
    })
    this.state.shouldFetchSimilarBikes = true;
    this.state.isFetching = true


  }

  checkNetworkFailure() {
    const {
      selectBikePopup,
      FinanceCityPopup,
      resetMakeModelListFailure,
      resetSelectedBikeDetailFailure,
      resetBikeVersionListFailure,
      resetCityFailure
    } = this.props;
    let networkFailureError = false;
    if (selectBikePopup.MakeModelListFetchError) {
      resetMakeModelListFailure();
      networkFailureError = true;
    }
    else if (selectBikePopup.VersionListFetchError) {
      resetBikeVersionListFailure();
      networkFailureError = true;
    }
    else if (selectBikePopup.ModelDetailFetchError) {
      resetSelectedBikeDetailFailure();
      networkFailureError = true;
    }
    else if (FinanceCityPopup.CityFetchError) {
      resetCityFailure();
      networkFailureError = true;
    }
    if (networkFailureError) {
      this.setToast('Something went wrong. Please try again.');
    }
  }

  setToast = (message) => {
    this.props.initToast({
      message: message,
      event: event,
      duration: 5000,
      style: { bottom: '50px', top: 'auto' }
    })
  }

  componentWillReceiveProps(nextProps) {

    const {
      selectBikePopup,
      openEmiCalculator,
      updateSimilarBikesEmi
    } = this.props
    let currentSelectBikePopupVersion = selectBikePopup.Selection.version
    let nextPropsSelectBikePopupVersion = nextProps.selectBikePopup.Selection.version
    if (nextPropsSelectBikePopupVersion.versionList.length > 0 && nextPropsSelectBikePopupVersion.selectedVersionIndex > -1
      && (currentSelectBikePopupVersion.modelId != nextPropsSelectBikePopupVersion.modelId
        || currentSelectBikePopupVersion.selectedVersionIndex != nextPropsSelectBikePopupVersion.selectedVersionIndex
        || currentSelectBikePopupVersion.cityId != nextPropsSelectBikePopupVersion.cityId)) {
      openEmiCalculator(nextPropsSelectBikePopupVersion.versionList[nextPropsSelectBikePopupVersion.selectedVersionIndex].price)
      this.state.isFetching = false
    }

    if (nextProps.SimilarBikesEMI.data.length > 0 && nextProps.sliderDp.values[0] > 0 && (nextProps.sliderDp.values[0] !== nextProps.SimilarBikesEMI.downPayment || nextProps.sliderInt.values[0] !== nextProps.SimilarBikesEMI.rateOfInt || nextProps.sliderTenure.values[0] !== nextProps.SimilarBikesEMI.tenure
      || nextProps.SimilarBikesEMI.modelId != this.props.SimilarBikesEMI.modelId || nextProps.SimilarBikesEMI.cityId != this.props.SimilarBikesEMI.cityId)) {
      updateSimilarBikesEmi(nextProps.SimilarBikesEMI, {
        downPayment: nextProps.sliderDp.values[0],
        tenure: nextProps.sliderTenure.values[0],
        rateOfInt: nextProps.sliderInt.values[0]
      });
    }
    this.checkNetworkFailure();
  }

  

  componentDidUpdate(prevProps) {
    const {
      FinanceCityPopup,
      selectBikePopup
    } = this.props;
    const currentCityId = this.getSelectedCityId(this.props);
    const currentGlobalCityId = FinanceCityPopup.currentGlobalCityId;
    if (this.state.shouldscroll) {
      if(!this.props.FinanceCityPopup.isActive){
        this.handleSelectCityClick();
        if (this.refs.emiSteps != undefined) {
          this.refs.emiSteps.scrollCityToView();
        }
      }
      // Open city popup if current city not in fetched city list
      if (selectBikePopup != undefined && FinanceCityPopup.RelatedModelId == this.state.currentSelectedBikeId && this.state.currentSelectedBikeId > 0) {
        if (FinanceCityPopup.CityFetchError) {
          this.setState({ ...this.state, shouldscroll: false });
          closePopupWithHash(this.props.closeSelectCityPopup);
        }
        else if (FinanceCityPopup.Popular.length > 0 || FinanceCityPopup.Other.length > 0) {
          // Check if current global city in new city list
          if (currentGlobalCityId > 0 && (IsGlobalCityPresent(FinanceCityPopup.Popular, currentGlobalCityId) || IsGlobalCityPresent(FinanceCityPopup.Other, currentGlobalCityId))) {
            this.setState({ ...this.state, shouldscroll: false, isGlobalCityInList: true });
            if (currentCityId != currentGlobalCityId)
              this.setCity({
                cityId: currentGlobalCityId,
                cityName: FinanceCityPopup.currentGlobalCityName,
                userChange: false
              });
            closePopupWithHash(this.props.closeSelectCityPopup);
          }
          else {
            if(currentCityId != -1)
              this.setCity({
                cityId: -1,
                cityName: "",
                userChange: false
              });
            this.props.resetVersionSelection()
            if (FinanceCityPopup.currentGlobalCityName != undefined) {
              this.setToast("Price of this bike is not available in " + FinanceCityPopup.currentGlobalCityName + ". Please choose another city.");
            }
            this.setState({ ...this.state, shouldscroll: false, isGlobalCityInList: false, isFetching: false });
          }

        }
        else {
          this.setToast("Price of this bike is not available. Unfortunately, we won't be able to show EMI for this bike.");
          this.setState({ ...this.state, shouldscroll: false });
        }
      }
    }
  }

  componentWillUnmount() {
    this.props.clearToast();
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
    const isBikeAndCitySelected = ((this.getSelectedBikeId(this.props) !== -1) && (this.getSelectedCityId(this.props) !== -1));
    return (
      <div>
        <div className="emi-calculator__head">
          <h2 className="emi-calculator__title">EMI Calculator</h2>
          <p className="emi-calculator__head-description">
            Know the tentative EMI for bike of your choice in 2 simple steps.
          </p>
        </div>

        <EMISteps ref="emiSteps" onSelectBikeClick={this.handleSelectBikeClick} onSelectCityClick={this.handleSelectCityClick} model={selectBikePopup.Selection} onSelectBikeClose={closeSelectBikePopup} onSelectCityClose={closeSelectCityPopup} isShown={!isBikeAndCitySelected}/>
        { isBikeAndCitySelected == true &&
          <div>
            <ModelInfo ref="modelInfoComponent" />
            <EMICalculator IsFetching={this.state.isFetching} />
          </div>
        }

        {
          SimilarBikesEMI != null && SimilarBikesEMI.data != null && SimilarBikesEMI.data.length > 0 && (
            <SwiperContainer
              type="carousel__similar-emi"
              heading="Other Bikes in similar range"
              data={SimilarBikesEMI}
              carouselCard={SwiperSimilarBikesEMI}
              onCarouselCardClick={this.handleSimilarEMISwiperCardClick}
            />
          )
        }

        {
          selectBikePopup != null &&
          <SelectBikePopup isActive={selectBikePopup.isActive} data={selectBikePopup} onCloseClick={closeSelectBikePopup} onBikeClick={this.handleBikeClick} fetchMakeModelList={fetchMakeModelList} />
        }
        {
          FinanceCityPopup != null &&
          <SelectCityPopup isActive={FinanceCityPopup.isActive} data={{ ...FinanceCityPopup, isGlobalCityInList: this.state.isGlobalCityInList }} onCloseClick={closeSelectCityPopup} onCityClick={this.handleCityClick} />
        }
      </div>);
  }
}
export default EMITab;
