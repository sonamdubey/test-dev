import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import {
	fetchBodyType,
	selectBodyType,
	setBodyStoredFilters
} from '../actionCreators/BodyType'

import {
	initToast,
	clearToast
} from '../../actionCreators/Toast'

import {
	createRipple
} from '../../utils/Ripple'

import {
	setCurrentScreen
} from '../actionCreators/Filter'

import { formatBudgetTooltipValue } from '../utils/Budget'

import BodyTypeItem from '../components/BodyTypeItem'
import BodyTypeKnowMore from './BodyTypeKnowMore'
import { fireInteractiveTracking } from '../../utils/Analytics';
import {
	getScreenId,
	getBudgetFilter
} from '../selectors/NCFSelectors'

import {
	getCityName
} from '../../selectors/LocationSelectors'

import SpeedometerLoader from '../../components/SpeedometerLoader'
import {CATEGORY_NAME} from '../constants/index';
import { trackCustomData } from '../../utils/cwTrackingPwa';

class BodyType extends React.Component {
	constructor(props) {
		super(props)

		this.bodyTypeClickSource = {
			BODY_TYPE_SCREEN: 1,
			KNOW_MORE_HEADER: 2,
			KNOW_MORE_BODY: 3
		}
	}

	componentDidMount() {
		const {
			fetchBodyType,
			setCurrentScreen,
			screenId
		} = this.props

		setCurrentScreen(screenId)
		fetchBodyType()
		trackCustomData(CATEGORY_NAME, "BodyTypeScreenImpression", "NA", false)
	}

	componentWillUnmount() {
		this.props.clearToast()
	}

	componentDidUpdate(prevProps) {
		if(prevProps.isFetching && !this.props.isFetching)
		{
			this.props.setBodyStoredFilters(this.props.storedBodytype)
		}
	}

	getRecommendedBodyType = () => {
		const {
			bodyTypeData
		} = this.props

		let recommendedBodyTypeData = bodyTypeData.filter(item => item.isRecommended)

		return recommendedBodyTypeData
	}

	getOtherBodyType = () => {
		const {
			bodyTypeData
		} = this.props

		let otherBodyTypeData = bodyTypeData.filter(item => !item.isRecommended)

		return otherBodyTypeData
	}

	getBodyTypeList = (data, listType) => {
		let listItemIconType = ''

		if (listType === 'list-other-body__item') {
			listItemIconType = 'line'
		}

		let list = data.map(item => {
			const itemStatusClass = this.getItemStatus(item)

			return (
				<li
					key={item.id}
					className={"body-type-list__item " + listType + " " + itemStatusClass}
					onClick={this.handleBodyTypeClick.bind(this, item, this.bodyTypeClickSource.BODY_TYPE_SCREEN)}
				>
					<BodyTypeItem
						item={item}
						iconType={listItemIconType}
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

	trackBodyTypeSelection(section,status,name){
		fireInteractiveTracking(CATEGORY_NAME, "NCF_BodyTypeFilter",section+status+'_'+name )
		trackCustomData(CATEGORY_NAME, "BodyTypeFilterClick","section="+section+"|state="+status+"|bodyType="+name, false)
	}

	handleBodyTypeClick = (item, clickSource, event) => {
		createRipple(event)
		if (item.carCount) {
			this.props.selectBodyType(item.id)
		}
		else {
			this.setToast(item, event)
		}
		const bodyTypeClickSource = this.bodyTypeClickSource
		const status = (item.carCount > 0 ? (item.isSelected ? "Deselect" : "Select") : "NotAvailableClick")
		switch (clickSource) {
			case bodyTypeClickSource.BODY_TYPE_SCREEN:
				this.trackBodyTypeSelection((item.isRecommended ? "Recommend" : ""),status,item.name)
				break
			case bodyTypeClickSource.KNOW_MORE_BODY:
				this.trackBodyTypeSelection("KnowMoreBody",status,item.name)
				break
			case bodyTypeClickSource.KNOW_MORE_HEADER:
				this.trackBodyTypeSelection("KnowMoreHeader",status,item.name)
				break
		}
	}

	setToast = (item, event) => {
		this.props.initToast({
			message: `There is no ${item.name} available for ${formatBudgetTooltipValue(this.props.budget)} in ${this.props.cityName}.`,
			event: event
		})
	}

	render() {
		const {
			bodyTypeData,
			isFetching
		} = this.props

		if(isFetching){
			return <SpeedometerLoader />
		}

		let recommendedList,
			otherList

		const isDataAvailable = bodyTypeData && bodyTypeData.length

		if (isDataAvailable) {
			let recommendedBodyTypeData = this.getRecommendedBodyType()
			recommendedList = this.getBodyTypeList(recommendedBodyTypeData, "list-recommended-body__item")

			let otherBodyTypeData = this.getOtherBodyType()
			otherList = this.getBodyTypeList(otherBodyTypeData, "list-other-body__item")
		}

		return (
			<div className="body-type-screen">
				<div className="screen__head">
					<div className="screen-head__image"></div>
					<p className="screen-head__title">Select your preferred body type</p>
					<p className="screen-head__subtitle">Recommended Body Types pre-selected</p>
					<ul className="body-type__list list__recommended-body">
						{recommendedList}
					</ul>
				</div>
				<div className="screen__body">
					<p className="screen-body__title">Other body types for selection</p>
					<ul className="body-type__list list__other-body">
						{otherList}
					</ul>
				</div>

				{
					isDataAvailable
						? <BodyTypeKnowMore
							bodyTypeData={bodyTypeData}
							handleBodyTypeClick={this.handleBodyTypeClick}
							getItemStatus={this.getItemStatus}
							clickSource={this.bodyTypeClickSource}
						/>
						: ''
				}
			</div>
		)
	}
}

const mapStateToProps = (state) => {
	const {
		data: bodyTypeData,
		isFetching
	} = state.newCarFinder.bodyType
	const {
		bodyType: storedBodytype
	} = state.filtersScreen.filters
	const budget = getBudgetFilter(state)
	const cityName = getCityName(state)
	const screenId = getScreenId(state,'BodyTypeFilter')
	return {
		bodyTypeData,
		isFetching,
		screenId,
		...budget,
		...cityName,
		storedBodytype
	}
}

const mapDispatchToProps = (dispatch) => {
	return {
		fetchBodyType: bindActionCreators(fetchBodyType, dispatch),
		selectBodyType: bindActionCreators(selectBodyType, dispatch),
		initToast: bindActionCreators(initToast, dispatch),
		clearToast: bindActionCreators(clearToast, dispatch),
		setCurrentScreen: bindActionCreators(setCurrentScreen, dispatch),
		setBodyStoredFilters: bindActionCreators(setBodyStoredFilters, dispatch)
	}
}

export default connect(mapStateToProps, mapDispatchToProps)(BodyType)
