import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import {
	showShortlistPopup,
	resetShortlistCars
} from '../actionCreators/Shortlist'

import {
	initToast
} from '../../actionCreators/Toast'
import { trackCustomData } from '../../utils/cwTrackingPwa'
import {CATEGORY_NAME} from '../constants/index';
import {
	makeCancelable
} from '../../utils/CancelablePromise'
import NewCarSearchApi from '../apis/NewCarSearch'
import { deserialzeQueryStringToObject } from '../../utils/Common'

class ShortlistIcon extends React.Component {
	constructor(props) {
		super(props)
		this.apiRequest = null
		this.updateShortlistCount()
	}

    handleShortlistClick = () => {
		if(this.props.shortlistCount > 0){
			history.pushState('shortlistpopup',null)
			this.props.showShortlistPopup()
        }
        else{
            this.props.initToast({
				message: 'You have no shortlisted cars available.'
			})
        }
		trackCustomData(CATEGORY_NAME,'ShortlistIconClick',"shortlistCount="+this.props.shortlistCount,false)
    }

	updateShortlistCount = () => {
		if(this.props.modelIds.length > 0){
			let cityId = ''
			if(window.location.search.length > 0){
				const searchParams = deserialzeQueryStringToObject(window.location.search)
				cityId = searchParams.cityId
			}
			let filterParams = {
				modelIds: this.props.modelIds,
				cityId: cityId != undefined && cityId != '' ? cityId : this.props.storeCityId
			}
			this.apiRequest && this.apiRequest.cancel()
				let apiRequest = makeCancelable(NewCarSearchApi.getPage(filterParams))
				apiRequest
					.promise
					.then(x => {
						if(x.totalModels != this.props.shortlistCount){
							trackCustomData(CATEGORY_NAME,'ShortlistCountData',"shortlistCount="+this.props.shortlistCount+"fetchedCount="+x.totalModels,false)
							let currentModelIds = x.models.map((item,index) => {
								return item.modelId
							})
							this.props.resetShortlistCars(x.totalModels,currentModelIds)
						}
					})
					.catch(error => {
						if (error.isCanceled) {
							console.log("Request cancelled for: ", filterParams.budget)
						}
						else {
							console.log(error)
							//TODO: handle search api failure
						}
					})
			this.apiRequest = apiRequest
		}
	}

    componentDidMount() {
		trackCustomData(CATEGORY_NAME,'ShortlistIconImpression',"shortlistCount="+this.props.shortlistCount,false)
    }

	render() {
        const shortlistActiveClass = this.props.shortlistCount > 0 ? 'shortlist--active' : ''
		return (
            <span className={"shortlist-icon " + shortlistActiveClass} onClick={this.handleShortlistClick}>
                { (this.props.shortlistCount>0)
                    &&
                        <span className="shortlist__count">
                        {this.props.shortlistCount}
                        </span>
                }
            </span>
		)
	}
}

const mapStateToProps = (state) => {
	const { count, modelIds } = state.newCarFinder.shortlistCars
	const {
		cityId
	} = state.location
	return {
		shortlistCount: count,
		modelIds,
		storeCityId: cityId
	}
}

const mapDispatchToProps = (dispatch) => {
	return {
		showShortlistPopup: bindActionCreators(showShortlistPopup, dispatch),
		resetShortlistCars: bindActionCreators(resetShortlistCars, dispatch),
		initToast: bindActionCreators(initToast, dispatch)
	}
}

export default connect(mapStateToProps, mapDispatchToProps) (ShortlistIcon)
