import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { toJS } from '../../immutableWrapperContainer'

import ModelInfo from './ModelInfoComponent'
import { openSelectBikePopup, selectBikeVersion } from '../../actionCreators/SelectBikePopup'
import { openSelectCityPopup } from '../../actionCreators/FinanceCityPopup'

const mapStateToProps = (store) => {
  return {
    model: store.getIn(['Finance', 'SelectBikePopup', 'Selection']),
    city: store.getIn(['Finance','FinanceCityPopup', 'Selection'])
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    openSelectBikePopup: bindActionCreators(openSelectBikePopup, dispatch),
    openSelectCityPopup: bindActionCreators(openSelectCityPopup, dispatch),
    selectBikeVersion: bindActionCreators(selectBikeVersion, dispatch)
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(toJS(ModelInfo))
