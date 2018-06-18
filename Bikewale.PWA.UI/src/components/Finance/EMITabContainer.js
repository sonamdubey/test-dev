import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { toJS } from '../../immutableWrapperContainer'
import {
  openSelectBikePopup, closeSelectBikePopup, selectModel, fetchMakeModelList, fetchBikeVersionList, fetchSelectedBikeDetail,
  resetMakeModelListFailure, resetSelectedBikeDetailFailure, resetBikeVersionListFailure, resetVersionSelection
} from '../../actionCreators/SelectBikePopup'
import { fetchCity, openSelectCityPopup, closeSelectCityPopup, selectCity, resetCityFailure } from '../../actionCreators/FinanceCityPopup'
import { fetchSimilarBikes, updateSimilarBikesEmi  } from '../../actionCreators/SimilarBikesEMI'
import { openEmiCalculator } from '../../actionCreators/emiDownPaymentSlider'
import { initToast, clearToast } from '../../actionCreators/toast'
import EMITab from './EMITab'


const mapStateToProps = (store) => {
    return {
      selectBikePopup: store.getIn(['Finance', 'SelectBikePopup']),
      FinanceCityPopup: store.getIn(['Finance', 'FinanceCityPopup']),
      SimilarBikesEMI: store.getIn(['Finance', 'SimilarBikesEMI']),
      sliderDp: store.getIn(['Emi', 'VehicleDownPayment', 'slider']),
      sliderTenure: store.getIn(['Emi', 'VehicleTenure', 'slider']),
      sliderInt: store.getIn(['Emi', 'VehicleInterest', 'slider'])
    }
  }
  
  const mapDispatchToProps = (dispatch) => {
    return {
      openSelectBikePopup: bindActionCreators(openSelectBikePopup, dispatch),
      closeSelectBikePopup: bindActionCreators(closeSelectBikePopup, dispatch),
      fetchMakeModelList: bindActionCreators(fetchMakeModelList, dispatch),
      resetMakeModelListFailure: bindActionCreators(resetMakeModelListFailure, dispatch),
      fetchSelectedBikeDetail: bindActionCreators(fetchSelectedBikeDetail, dispatch),
      resetSelectedBikeDetailFailure: bindActionCreators(resetSelectedBikeDetailFailure, dispatch),
      fetchBikeVersionList: bindActionCreators(fetchBikeVersionList, dispatch),
      resetBikeVersionListFailure: bindActionCreators(resetBikeVersionListFailure, dispatch),
      openSelectCityPopup: bindActionCreators(openSelectCityPopup, dispatch),
      closeSelectCityPopup: bindActionCreators(closeSelectCityPopup, dispatch),
      fetchCity: bindActionCreators(fetchCity, dispatch),
      resetCityFailure: bindActionCreators(resetCityFailure, dispatch),
      selectCity: bindActionCreators(selectCity, dispatch),
      fetchSimilarBikes: bindActionCreators(fetchSimilarBikes, dispatch),
      selectModel: bindActionCreators(selectModel, dispatch),
      updateSimilarBikesEmi: bindActionCreators(updateSimilarBikesEmi, dispatch),
      openEmiCalculator: bindActionCreators(openEmiCalculator, dispatch),
      initToast: bindActionCreators(initToast, dispatch),
      clearToast: bindActionCreators(clearToast, dispatch),
      resetVersionSelection: bindActionCreators(resetVersionSelection, dispatch)
    }
  }
  
 export default connect(mapStateToProps, mapDispatchToProps)(toJS(EMITab));