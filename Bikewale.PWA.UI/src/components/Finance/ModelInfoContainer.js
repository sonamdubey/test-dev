import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { toJS } from '../../immutableWrapperContainer'

import ModelInfo from './ModelInfoComponent'
import { openSelectBikePopup, closeSelectBikePopup, selectBikeVersion } from '../../actionCreators/SelectBikePopup'
import { openSelectCityPopup, closeSelectCityPopup } from '../../actionCreators/FinanceCityPopup'
import { FinanceCityPopup } from '../../reducers/FinanceCityPopup';

const mapStateToProps = (store) => {
  return {
    isLoaderShown: store.getIn(['Finance', 'SelectBikePopup', 'IsFetchingModelDetail']) || store.getIn(['Finance', 'FinanceCityPopup', 'IsFetching']),
    model: store.getIn(['Finance', 'SelectBikePopup', 'Selection']),
    city: store.getIn(['Finance','FinanceCityPopup', 'Selection'])
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    openSelectBikePopup: bindActionCreators(openSelectBikePopup, dispatch),
    openSelectCityPopup: bindActionCreators(openSelectCityPopup, dispatch),
    closeSelectBikePopup: bindActionCreators(closeSelectBikePopup, dispatch),
    closeSelectCityPopup: bindActionCreators(closeSelectCityPopup, dispatch),
    selectBikeVersion: bindActionCreators(selectBikeVersion, dispatch)
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(toJS(ModelInfo))
