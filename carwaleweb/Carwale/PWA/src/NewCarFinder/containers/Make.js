import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import {
	fetchMake,
	selectMake,
	setMakeStoredFilters
} from '../actionCreators/Make'

import {
	initToast,
	clearToast
} from '../../actionCreators/Toast'

import {
	setCurrentScreen
} from '../actionCreators/Filter'

import {
	makeCancelable
} from '../../utils/CancelablePromise'

import {
	getScreenId
} from '../selectors/NCFSelectors'

import MakeItem from '../components/MakeItem'

import { fireInteractiveTracking } from '../../utils/Analytics';

import SpeedometerLoader from '../../components/SpeedometerLoader'

import {
	createRipple
} from '../../utils/Ripple'
import {CATEGORY_NAME} from '../constants/index';
import { trackCustomData } from '../../utils/cwTrackingPwa';
class Make extends React.Component {
	constructor(props) {
		super(props)
	}

	componentDidMount() {
		const {
			fetchMake,
			setCurrentScreen,
			screenId
		} = this.props

		setCurrentScreen(screenId)
		fetchMake()
		trackCustomData(CATEGORY_NAME, "MakeScreenImpression", "NA", false)
	}

	componentWillUnmount() {
		this.props.clearToast()
	}

	componentDidUpdate(prevProps) {
		if(prevProps.isFetching && !this.props.isFetching)
		{
			this.props.setMakeStoredFilters(this.props.storedMakes)
		}
	}

	getPopularMake = (pop) => {
		const {
			makeData
		} = this.props

		let popularMakeData = makeData.popularMakes
		return popularMakeData
	}

	getOtherMake = () => {
		const {
			makeData
		} = this.props

		let otherMakeData = makeData.otherMakes
		return otherMakeData
	}

	getMakeList = (data, listType,section) => {
		let listItemIconType = ''

		if (listType === 'list-other-body__item') {
			listItemIconType = 'line'
		}
		let list = data.map(item => {
			const itemStatusClass = this.getItemStatus(item)
			return (
				<li
					key={item.makeId}
					className={"make-list__item " + listType + " " + itemStatusClass}
					onClick={this.handleMakeClick.bind(this, item,section)}
				>
					<MakeItem
						item={item}
					/>
				</li>
			)
		})

		return list
	}

	getItemStatus = (item) => {
		let status = ''

		if (item.isSelected) {
			status = 'active'
		}
		else if (!item.modelCount) {
			status = 'disable'
		}
		return status
	}

	handleMakeClick = (item,section,event) => {
		createRipple(event)
		if (item.modelCount) {
			this.props.selectMake(item.makeId)
		}
		else {
			this.setToast(item, event)
		}
		const status = (item.modelCount > 0 ? (item.isSelected ? "Deselect" : "Select") : "NotAvailableClick")
		fireInteractiveTracking(CATEGORY_NAME, "NCF_MakeFilter", status + "_" + item.makeName)
		trackCustomData(CATEGORY_NAME,"MakeFilterClick","section="+section+"|state="+status+"|make="+item.makeName,false)
	}

	setToast = (item, event) => {
		this.props.initToast({
			message: `There is no ${item.makeName} car matching your criteria.`,
			event: event
		})
	}

	render() {
		const {
			makeData,
			isFetching
		} = this.props

		if(isFetching){
			return <SpeedometerLoader />
		}
		let list,
		otherMakeList

		const isDataAvailable = (makeData.otherMakes != undefined)

		if (isDataAvailable) {
			let popularMakeData = this.getPopularMake()
			list = this.getMakeList(popularMakeData, "list-popular-make__item","PopularBrands")

			let otherMakeData = this.getOtherMake()
			otherMakeList = this.getMakeList(otherMakeData, "list-other-make__item","OtherBrands")
		}
		return (
			<div className="make-type-screen">
				<div className="screen__head">
					<p className="screen-head__title">Select your preferred Brands</p>
					<p className="screen-head__subtitle">Popular Brands</p>
					<ul className="make__list list__popular-make">
						{list}
					</ul>
				</div>
				<div className="screen__body">
					<p className="screen-body__title">Other Brands</p>
					<ul className="make__list list__other-make">
						{otherMakeList}
					</ul>
				</div>
			</div>
		)
	}
}

const mapStateToProps = (state) => {
	const {
		data: makeData,
		isFetching
	} = state.newCarFinder.make
	const {
		make : storedMakes
	} = state.filtersScreen.filters
	const screenId = getScreenId(state,'MakeFilter')
	return {
		makeData,
		isFetching,
		screenId,
		storedMakes
	}
}

const mapDispatchToProps = (dispatch) => {
	return {
		fetchMake: bindActionCreators(fetchMake, dispatch),
		selectMake: bindActionCreators(selectMake, dispatch),
		initToast: bindActionCreators(initToast, dispatch),
		clearToast: bindActionCreators(clearToast, dispatch),
		setCurrentScreen: bindActionCreators(setCurrentScreen, dispatch),
		setMakeStoredFilters: bindActionCreators(setMakeStoredFilters, dispatch)
	}
}

export default connect(mapStateToProps, mapDispatchToProps)(Make)
