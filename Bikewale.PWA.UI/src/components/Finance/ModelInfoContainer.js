import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { toJS } from '../../immutableWrapperContainer'

import ModelInfo from './ModelInfoComponent'
import { openSelectBikePopup } from '../../actionCreators/SelectBikePopup'
import { openSelectCityPopup } from '../../actionCreators/FinanceCityPopup'

const mapStateToProps = (store) => {
  return {
    model: store.getIn(['Finance', 'SelectBikePopup', 'Selection'])
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    openSelectBikePopup: bindActionCreators(openSelectBikePopup, dispatch),
    openSelectCityPopup: bindActionCreators(openSelectCityPopup, dispatch)
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(toJS(ModelInfo))
