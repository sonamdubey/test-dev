import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'

import ModelList from '../containers/ModelList'
import { lockScroll } from '../../utils/ScrollLock';
import { trackCustomData } from '../../utils/cwTrackingPwa';
import {CATEGORY_NAME} from '../constants/index';

const propTypes = {
  // popup visibility status
  status: PropTypes.bool,
  // on click back arrow click
  handleBack: PropTypes.func
}

const defaultProps = {
  status: false,
  handleBack: null
}

class ShortlistPopup extends React.Component {
	constructor(props) {
    super(props)
  }
  getModelListContainer() {
    let searchParams = {
      modelIds: this.props.modelIds,
      cityId: this.props.cityId != undefined && this.props.cityId != '' ? this.props.cityId : this.props.storeCityId
    }
    if (this.props.status && searchParams.modelIds.length > 0) {
      trackCustomData(CATEGORY_NAME,'ShortlistPopupLoad',"shortlistCount="+this.props.count,false)
      return  <ModelList
                key="shortListing"
                pagename="shortListing"
                searchParams={searchParams}
              />
    }
    return null
  }
  componentDidMount(){
    window.onpopstate = this.props.handleBack
  }
  shouldComponentUpdate(nextProps){
    if(this.props.status != nextProps.status){
      return true
    }
    if(this.props.cityId != nextProps.cityId){
      return true
    }
    return false
  }
  handleShortlistClose () {
    this.props.handleBack()
    history.back()
	}
  componentDidUpdate(prevProps) {
		if (this.props.status && !prevProps.status) {
			setTimeout(function () {
        window.dispatchEvent(new Event('scroll'))
      },500)
		}
	}
  handleDivScroll(){
    window.dispatchEvent(new Event('scroll'))
  }

  render() {
    let status = (this.props.status) ? 'shortlist-popup--active' : ''
    return (
      <div className={"shortlist-popup " + status} onScroll={this.handleDivScroll}>
      <div className="shortlist-popup__header">
        <span className="shortlist-popup__back-arrow" onClick={this.handleShortlistClose.bind(this)}>
        </span>
        <p className="shortlist-popup__title">
          My Shortlist
        </p>
      </div>
      <div className="shortlist-popup__content">
        {this.getModelListContainer()}
      </div>
    </div>
    )
  }
}

const mapStateToProps = (state) => {
	const {
    modelIds,
    count
  } = state.newCarFinder.shortlistCars
  const {
    cityId
  } = state.location
	return {
    modelIds,
    storeCityId: cityId,
    count
	}
}

ShortlistPopup.propTypes = propTypes
ShortlistPopup.defaultProps = defaultProps

export default connect(mapStateToProps)(ShortlistPopup)
