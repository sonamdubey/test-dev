import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import SpeedometerLoader from '../../components/SpeedometerLoader'

import {
	fetchFuelType,
	selectFuelType,
	setFuelStoredFilters
} from '../actionCreators/FuelType'

import {
	setCurrentScreen
} from '../actionCreators/Filter'

import {
	initToast,
	clearToast
} from '../../actionCreators/Toast'

import {
	makeCancelable
} from '../../utils/CancelablePromise'

import {
	getScreenId
} from '../selectors/NCFSelectors'

import ListItem from '../../components/ListItem'

import { fireInteractiveTracking } from '../../utils/Analytics';
import {CATEGORY_NAME} from '../constants/index';
import { trackCustomData } from '../../utils/cwTrackingPwa';
class FuelType extends React.Component {
	constructor(props) {
		super(props)
	}

	componentDidMount() {
		const {
			fetchFuelType,
			setCurrentScreen,
			screenId
		} = this.props

		setCurrentScreen(screenId)
		fetchFuelType()
		trackCustomData(CATEGORY_NAME, "FuelTypeScreenImpression", "NA", false)
	}

	componentWillUnmount() {
		this.props.clearToast()
	}

	componentDidUpdate(prevProps) {
		if(prevProps.isFetching && !this.props.isFetching)
		{
			this.props.setFuelStoredFilters(this.props.storedFueltype)
		}
	}

	getFuelTypeList = (data) => {
		let list = data.map(item => {
			const itemStatusClass = this.getItemStatus(item)

			return (
				<li
					key={item.id}
					className={"fuel-type-list__item " + itemStatusClass}
					onClick={this.handleFuelTypeClick.bind(this, item)}
				>
					<ListItem
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
		else if (!item.carCount) {
			status = 'disable'
		}

		return status
	}

	handleFuelTypeClick = (item, event) => {
		if (item.carCount) {
			this.props.selectFuelType(item.id)
		}
		else {
			this.setToast(item, event)
		}
		const status = (item.carCount > 0 ? (item.isSelected ? "Deselect" : "Select") : "NotAvailableClick")
		fireInteractiveTracking(CATEGORY_NAME, "NCF_FuelTypeFilter", status+ "_" + item.name)
		trackCustomData(CATEGORY_NAME,"FuelTypeFilterClick","state="+status+"|fuelType="+item.name,false)
	}

	setToast = (item, event) => {
		this.props.initToast({
			message: `There is no ${item.name} car matching your criteria.`,
			event: event
		})
	}

	render() {
		const {
			fuelTypeData,
			isFetching
		} = this.props

		if(isFetching){
			return <SpeedometerLoader />
		}
		let list

		const isDataAvailable = fuelTypeData && fuelTypeData.length

		if (isDataAvailable) {
			list = this.getFuelTypeList(fuelTypeData)
		}

		return (
			<div className="fuel-type-screen">
				<div className="screen__head">
					<p className="screen-head__title">Select your preferred fuel type</p>
				</div>
				<div className="screen__body">
					<ul className="fuel-type__list">
						{list}
					</ul>
				</div>
			</div>
		)
	}
}

const mapStateToProps = (state) => {
	const {
		data: fuelTypeData,
		isFetching
	} = state.newCarFinder.fuelType
	const {
		fuelType: storedFueltype
	} = state.filtersScreen.filters
	const screenId = getScreenId(state,'FuelTypeFilter')
	return {
		fuelTypeData,
		isFetching,
		screenId,
		storedFueltype
	}
}

const mapDispatchToProps = (dispatch) => {
	return {
		fetchFuelType: bindActionCreators(fetchFuelType, dispatch),
		selectFuelType: bindActionCreators(selectFuelType, dispatch),
		initToast: bindActionCreators(initToast, dispatch),
		clearToast: bindActionCreators(clearToast, dispatch),
		setCurrentScreen: bindActionCreators(setCurrentScreen, dispatch),
		setFuelStoredFilters: bindActionCreators(setFuelStoredFilters, dispatch)
	}
}

export default connect(mapStateToProps, mapDispatchToProps)(FuelType)
