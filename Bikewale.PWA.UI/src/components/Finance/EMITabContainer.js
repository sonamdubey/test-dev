import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { toJS } from '../../immutableWrapperContainer'
import EMITab from './EMITab'
import { openSelectBikePopup, closeSelectBikePopup, selectModel, fetchMakeModelList, fetchBikeVersionList} from '../../actionCreators/SelectBikePopup'
import { fetchCity, openSelectCityPopup, closeSelectCityPopup, selectCity } from '../../actionCreators/FinanceCityPopup'
import { fetchSimilarBikes } from '../../actionCreators/SimilarBikesEMI'

const mapStateToProps = (store) => {
    return {
      selectBikePopup: store.getIn(['Finance', 'SelectBikePopup']),
      FinanceCityPopup: store.getIn(['Finance', 'FinanceCityPopup']),
      SimilarBikesEMI: store.getIn(['Finance', 'SimilarBikesEMI'])
    }
  }
  
  const mapDispatchToProps = (dispatch) => {
    return {
      openSelectBikePopup: bindActionCreators(openSelectBikePopup, dispatch),
      closeSelectBikePopup: bindActionCreators(closeSelectBikePopup, dispatch),
      fetchMakeModelList: bindActionCreators(fetchMakeModelList, dispatch),
      fetchBikeVersionList: bindActionCreators(fetchBikeVersionList, dispatch),
      openSelectCityPopup: bindActionCreators(openSelectCityPopup, dispatch),
      closeSelectCityPopup: bindActionCreators(closeSelectCityPopup, dispatch),
      fetchCity: bindActionCreators(fetchCity, dispatch),
      selectCity: bindActionCreators(selectCity, dispatch),
      fetchSimilarBikes: bindActionCreators(fetchSimilarBikes, dispatch),
      selectModel: bindActionCreators(selectModel, dispatch)
    }
  }
  
 export default connect(mapStateToProps, mapDispatchToProps)(toJS(EMITab));