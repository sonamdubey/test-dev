/**
 * This class representing the container component
 * for find car model version listing
 *
 * @class VersionList
 * @extends {React.Component}
 */

import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import VersionBox from '../components/VersionBox'
import OnRoadPrice from './OnRoadPrice'
import Button from '../components/Button'
import { openPopup as openLocationPopup } from '../../Location/actionCreators'
import OnVisible from '../utils/react-on-visible';

import { fireInteractiveTracking } from '../../utils/Analytics';
import { trackCustomData } from '../../utils/cwTrackingPwa';
import '../utils/closestPolyfill';
import { CATEGORY_NAME } from '../constants/index';
import {
	createRipple
} from '../../utils/Ripple'

import {
	addModelToCompare,
	removeModelFromCompare
} from '../actionCreators/CompareCars'
import {
	addModelToShortlist,
	removeModelFromShortlist,
} from '../actionCreators/Shortlist'
import {
	initToast
} from '../../actionCreators/Toast'

class VersionList extends React.Component {
	constructor(props) {
		super(props)
	}

	trackEvent = (label, bhriguAction, bhriguLabel) => {
		let actionPrefix = this.props.pageName == 'mainListing' ? 'Listing' : 'Shortlist'
		fireInteractiveTracking(CATEGORY_NAME, 'NCF_'+actionPrefix, label)
		trackCustomData(CATEGORY_NAME,actionPrefix+bhriguAction,bhriguLabel,false)
	}
	fireScrollOnListScroll () {
		window.dispatchEvent(new Event('scroll'))
	}
	componentDidMount() {
		this.refs.versionList.addEventListener('scroll',this.fireScrollOnListScroll)
	}
	componentWillUnmount () {
		this.refs.versionList.removeEventListener('scroll',this.fireScrollOnListScroll)
	}

	shouldComponentUpdate(nextProps, nextState) {
		let shouldUpdate = false
		const nextVersionIds = nextProps.data.versions.map(x => x.id)
		const prevVersionIds = this.props.data.versions.map(x => x.id)

		if (!nextVersionIds.equals(prevVersionIds)) {
			shouldUpdate = true
		}
		else if (!nextProps.versionsAddedToCompare.equals(this.props.versionsAddedToCompare)) {
			const hasVersionsInCompare = nextVersionIds.filter(x => {
				if (nextProps.versionsAddedToCompare.indexOf(x) > -1) {
					return x
				}
			})
			const hadVersionsInCompare = nextVersionIds.filter(x => {
				if (this.props.versionsAddedToCompare.indexOf(x) > -1) {
					return x
				}
			})
			shouldUpdate = !hasVersionsInCompare.equals(hadVersionsInCompare)
		}
		if(this.props.pageName == 'mainListing' && this.props.isShortlisted != nextProps.isShortlisted){
			shouldUpdate = true
		}
		if(this.props.pageName == 'mainListing' && this.props.shortlistCars.count !== nextProps.shortlistCars.count){
            return true;
        }
		return shouldUpdate
	}

	getVersionList = (data) => {
		const { makeMaskingName, modelMaskingName, makeName, modelName, modelId } = this.props.data

		let trackingLabel = ""
		let versionCount = data.length
		let list = data.map((item, index) => {
			const isInCompareCars = this.props.versionsAddedToCompare.findIndex(x => x === item.id) > -1
			let versionRank = (index + 1)
			trackingLabel = "versionId=" + item.id + "|versionName=" + item.name + "|versionRank=" + versionRank + "|totalVersions="
										+ versionCount + "|modelName=" + modelName + "|modelRank=" + this.props.modelRank
			let action = this.props.pageName == 'mainListing' ? 'ListingVersionImpression' : 'ShortlistVersionImpression'
			let listItem = (
				<li key={item.id} className="card-wrapper__item version-list__item">
					<div data-version={item.id} className="version-list__item-content">
						<OnVisible
							key={item.id}
							className="version__container">
							<div className="version__card" action={action} label={trackingLabel} >
								<div className="version__content">
									<VersionBox
										data={item}
										makeName={makeName}
										modelName={modelName}
										modelRank={this.props.modelRank}
										versionRank={versionRank}
										versionCount={versionCount}
										makeMaskingName={makeMaskingName}
										modelMaskingName={modelMaskingName}
										trackEvent={this.trackEvent}
										pageName={this.props.pageName} />
									<OnRoadPrice
										pageName = {this.props.pageName}
										data={item}
										cityName={this.props.cityName}
										cityId={this.props.cityId}
										trackEvent={this.trackEvent}
										handlePriceInMyCityClick={this.props.openLocationPopup} />
								</div>
								<div className='version__action-buttons'>
									<Button
										key={item.id+"_shortlist"}
										clickHandle={this.handleShortlistClick.bind(this,modelId,trackingLabel)}
										btnText={ this.props.isShortlisted ? "Shortlisted" : "Shortlist" }
										iconClass={ "shortlist-action-icon" + (this.props.isShortlisted ? " shortlist--active" : "") }
										active={this.props.isShortlisted} />
									<Button
										key={item.id+"_compare"}
										clickHandle={this.handleCompareChange.bind(this,item.id,isInCompareCars)}
										btnText={ isInCompareCars ? "Remove from Compare" : "Add to Compare" }
										iconClass={ "compare-action-icon" + (isInCompareCars ? " compare--active" : "") }
										active={isInCompareCars} />
								</div>
							</div>
						</OnVisible>
					</div>
				</li>
				)
				return (
					listItem
				)
			})
		return (
			list
		)
	}

	handleShortlistClick = (modelId,trackingLabel, event) => {
		createRipple(event)
		let removeShortlistFunc = this.props.removeModelFromShortlist.bind(this,modelId)
		let action = ''
		if(this.props.isShortlisted) {
			action = "ShortlistRemoved"
			if(this.props.pageName ==  "shortListing") {
				let modelCardElement = this.refs.versionList.closest('.model-card')
				modelCardElement.classList.add('model-card--collapse')
				setTimeout(function(){
					removeShortlistFunc()
					window.dispatchEvent(new Event('scroll'))
				},1001)
			}
			else {
				removeShortlistFunc()
			}
		}
		else {
			if(this.props.shortlistCars.count < this.props.shortlistCars.max) {
				action = "Shortlisted"
				let shortlistFunc = this.props.addModelToShortlist.bind(this,modelId)
				setTimeout(function () {
					shortlistFunc()
					window.dispatchEvent(new Event('scroll'))
				},401)
				this.refs.versionList.parentElement.classList.add('version-list__container--collapse')
			}
			else {
				this.props.initToast({
					message: `You are not allowed to shortlist more than ${this.props.shortlistCars.max} cars as of now.`
				})
			}
		}
		if(action) {
			fireInteractiveTracking(CATEGORY_NAME,'NCF_'+action,trackingLabel)
			trackCustomData(CATEGORY_NAME,action,trackingLabel,false)
		}
	}

	handleCompareChange = (id,isInCompare, event) => {
		createRipple(event)
		let message
		const {
			data,
			versionsAddedToCompare,
			maxCarsAllowed
		} = this.props
		const version = data.versions.filter(x => x.id === id)[0]
		if (!isInCompare) {
			if (versionsAddedToCompare.length < maxCarsAllowed) {
				const carData = {
					makeId: data.makeId,
					makeName: data.makeName,
					makeMaskingName: data.makeMaskingName,
					modelId: data.modelId,
					modelName: data.modelName,
					modelMaskingName: data.modelMaskingName,
					hostUrl: data.hostUrl,
					originalImagePath: data.originalImagePath,
					versionId: version.id,
					versionName: version.name,
					versionMaskingName: version.maskingName,
					versionFuelType: version.fuelType
				}
				if (!versionsAddedToCompare.length) {
					message = `${carData.modelName} ${carData.versionName} has been added to compare bucket. Select 1 more car to compare.`
				}
				this.props.addModelToCompare(carData)
				let versionCount = versionsAddedToCompare.length + 1
				this.trackEvent("CompareSelect_" + versionCount, "CompareSelect", "versionCount="+versionCount + "|versionId="+id+"|versionName="+version.name)
			}
			else {
				message = `You are not allowed to compare more than ${maxCarsAllowed} cars as of now. We are working on increasing the number in your consideration set.`
			}
		}
		else {
			this.props.removeModelFromCompare(id)
			message = 'Car has been removed from compare list'
			this.trackEvent("CompareUnCheck_" + (versionsAddedToCompare.length - 1), "CompareUnCheck", "versionCount="+(versionsAddedToCompare.length - 1)+ "|versionId="+id+"|versionName="+version.name)
		}

		if (message) {
			this.props.initToast({
				message: message
			})
		}
	}

	render() {
		const { versions: versionListData } = this.props.data
		let versionList
		let listCount = versionListData.length
		const isDataAvailable = versionListData && listCount

		if (isDataAvailable) {
			versionList = this.getVersionList(versionListData)
		}
		return (
			<div className={"version-list__container " + (this.props.isShortlisted ? 'version-list__container--collapse' : '') } >
				<ul ref="versionList" className="card-wrapper version-list__content" >
					{versionList}
				</ul>
			</div>
		)
	}
}

const mapStateToProps = (state) => {
	const { compareCars } = state
	const versionsAddedToCompare = compareCars.versionIds
	const maxCarsAllowed = compareCars.max

	return {
		versionsAddedToCompare,
		maxCarsAllowed
	}
}

const mapDispatchToProps = (dispatch) => {
	return {
		addModelToCompare: bindActionCreators(addModelToCompare, dispatch),
		initToast: bindActionCreators(initToast, dispatch),
		openLocationPopup: bindActionCreators(openLocationPopup, dispatch),
		addModelToShortlist: bindActionCreators(addModelToShortlist, dispatch),
		removeModelFromShortlist: bindActionCreators(removeModelFromShortlist, dispatch),
		removeModelFromCompare: bindActionCreators(removeModelFromCompare, dispatch)
	}
}

export default connect(mapStateToProps, mapDispatchToProps)(VersionList)
