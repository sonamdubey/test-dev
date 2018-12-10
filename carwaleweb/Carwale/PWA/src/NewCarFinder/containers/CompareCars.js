import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import {
	removeModelFromCompare
} from '../actionCreators/CompareCars'
import { getCompareUrl, getVersionPageUrl } from '../../utils/UrlFactory';
import { fireInteractiveTracking } from '../../utils/Analytics';
import {CATEGORY_NAME} from '../constants/index';
import { trackCustomData } from '../../utils/cwTrackingPwa';
import { ChangeFilterPosition } from '../components/FilterButton'
/**
 * This class representing the container component
 * for compare cars floating screen
 *
 * @class Compare Cars
 * @extends {React.Component}
 */
class CompareCars extends React.Component {
	constructor(props) {
		super(props)
	}

	componentDidMount(){
		this.setScreen()
	}

	componentDidUpdate(prevProps, prevState){
		if (this.props.active !== prevProps.active) {
			this.setScreen()
		}
	}

	setScreen = () => {
		if (this.props.active) {
			this.refs.compareScreen.style.height = `${this.refs.compareContent.offsetHeight}px`

			this.refs.compareScreen.classList.add('screen--translate')
		}
		else {
			this.refs.compareScreen.classList.remove('screen--translate')
		}
	}

	removeFromComparison = (versionId) => {
		this.props.removeModelFromCompare(versionId)
		fireInteractiveTracking(CATEGORY_NAME,"NCF_Listing","CompareUnSelect_"+ this.props.versionIds.indexOf(versionId))
		trackCustomData(CATEGORY_NAME,"ListingCompareUnSelect","position="+this.props.versionIds.indexOf(versionId),false)
	}

	trackEvent = (label) => {
		fireInteractiveTracking(CATEGORY_NAME,"NCF_Listing",label)
		trackCustomData(CATEGORY_NAME,"Listing"+label,"versions="+this.props.versionIds.join(","),false)
	}

	getCompareList = (cars) => {
		const carList = cars.map(car => {
			return (
				<li className="compare-list__item" key={car.versionId}>
					<a href={getVersionPageUrl(car.makeMaskingName, car.modelMaskingName, car.versionMaskingName)} className="compare-model__box">
						<div className="compare-model__image">
							<img src={car.hostUrl + "110X61" + car.originalImagePath} alt="" />
						</div>
						<h2 className="compare-model__title">
							<p className="compare-title__model">{car.makeName} {car.modelName}</p>
							<span className="compare-title__version">{car.versionName}</span>
							<span className="compare-title__transmission">({car.versionFuelType})</span>
						</h2>
					</a>
					<span onClick={this.removeFromComparison.bind(this, car.versionId)} className="compare__close"></span>
				</li>
			)
		})

		return (
			<ul className="compare__list">
				{carList}
			</ul>
		)
	}

	render() {
		let compareList
		if(this.props.active){
			compareList= this.getCompareList(this.props.cars)
			ChangeFilterPosition(true)
		}
		else{
			ChangeFilterPosition(false)
		}
		return (
			<div ref="compareScreen" className="compare__screen screen--translate">
				<div ref="compareContent" className="compare-screen__content">
					{compareList}
					<a href={getCompareUrl(this.props.cars)} onClick={this.trackEvent.bind(this,"CompareClick")} className="btn-secondary btn-compare-cars">Compare Cars</a>
				</div>
			</div>
		)
	}
}

const mapStateToProps = (state) => {
	const {
		active,
		count,
		cars,
		versionIds
	} = state.compareCars

	return {
		active,
		count,
		cars,
		versionIds
	}
}


const mapDispatchToProps = (dispatch, getState) => {
	return {
		removeModelFromCompare: bindActionCreators(removeModelFromCompare, dispatch)
	}
}

export default connect(mapStateToProps, mapDispatchToProps)(CompareCars)
