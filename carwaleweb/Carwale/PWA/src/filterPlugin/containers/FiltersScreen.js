import React from 'react'
import shallowequal from 'shallowequal'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { withRouter } from 'react-router-dom'
import PitComponent from '../components/BudgetPit'
import BudgetCollapsible from '../components/BudgetCollapsible'
import BodyTypeCollapsible from '../components/BodyTypeCollapsible'
import FuelTypeCollapsible from '../components/FuelTypeCollapsible'
import MakeCollapsible from '../components/MakeCollapsible'
import TransmissionTypeCollapsible from '../components/TransmissionTypeCollapsible'
import SeatsCollapsible from '../components/SeatsCollapsible'
import BodyTypeItem from '../components/BodyTypeItem'
import ListItem from '../../components/ListItem'
import MakeItem from '../components/MakeItem'
import {closeFilterScreen, setFilters} from '../actionCreators/FiltersScreen'
import { deserialzeQueryStringToObject } from '../../utils/Common'
import {
	reduceFuelTypeFilter,
	getNCFFilterParams,
	reduceTransmissionTypeFilter,
	reduceSeatsFilter,
	filterSelectors,
	getFilterStoreObject
} from '../selectors/FilterPluginSelectors'
import { trackCustomData } from '../../utils/cwTrackingPwa'
import { serialzeObjectToQueryString } from '../../utils/Common'
import {lockScroll,unlockScroll} from '../../utils/ScrollLock'
import {createRipple} from '../../utils/Ripple'
import { fireInteractiveTracking,fireNonInteractiveTracking } from '../../utils/Analytics'
import {formatBudgetTooltipValue} from '../utils/Budget'
import {filterTypes} from '../constants/FilterTypes'
import BudgetApi from '../../NewCarFinder/apis/Budget'
/**
 * This class representing the container component
 * for filters screen
 *
 * @class FiltersScreen
 * @extends {React.Component}
 **/
class FiltersScreen extends React.Component {
	constructor(props) {
		super(props)
		const { selectionObject, defaultFilters } = this.props
		this.defaultBudget = 900000;
		this.getDefaultBudget(defaultFilters.newCarFinder.budget.slider.values[0]);
		this.state = {
			budget: {
				...defaultFilters.newCarFinder.budget,
				preview: selectionObject['Budget']
			},
			bodyType: {
				...defaultFilters.newCarFinder.bodyType,
				preview: selectionObject['Body Type']
			},
			fuelType: {
				...defaultFilters.newCarFinder.fuelType,
				preview: selectionObject['Fuel Type']
			},
			make: {
				...defaultFilters.newCarFinder.make,
				preview: selectionObject['Make']
			},
			transmissionType: {
				...defaultFilters.newCarFinder.transmissionType,
				preview: selectionObject['Transmission Type']
			},
			seats: {
				...defaultFilters.newCarFinder.seats,
				preview: selectionObject['Seating Capacity']
			},
			collapsibleStatus: {
				'Budget': false,
				'Body Type': false,
				'Fuel Type': false,
				'Make': false,
				'Transmission Type': false,
				'Seating Capacity': false
			}
		}
		this.listingRefreshTimer = null
		this.userSliderChange = false
	}

	getDefaultBudget (budget) {
		if(budget == ""){
			this.setDefaultBudget = true;
			BudgetApi.get(defaultFilters.location.cityId).then(response => {
				Object.keys(response).forEach(key => response[key] = response[key] * 100000)
				this.defaultBudget = response.suggested;
			}).catch(error => {
				console.log(error)
			});
		}
	}

	componentWillReceiveProps(nextProps) {
		if (nextProps.activeAccordion) {
			let ref = nextProps.activeAccordion.replace(' ', '')
			this.setState((prevState) => {
				const key = nextProps.activeAccordion
				const { selectionObject, defaultFilters } = nextProps
				const newStatus = {}
				newStatus[key] = true
				return {
					collapsibleStatus: {
						...prevState.collapsibleStatus,
						...newStatus
					}
				}
			}, () => {
				this.refs[ref] && this.refs[ref].scrollIntoView && this.refs[ref].scrollIntoView()
			})
		}

		// for back handling
		if (this.props.defaultFilters !== nextProps.defaultFilters) {
			this.setState((prevState) => {
				const { selectionObject, defaultFilters } = nextProps
				return {
					budget: {
						...defaultFilters.newCarFinder.budget,
						preview: selectionObject['Budget']
					},
					bodyType: {
						...defaultFilters.newCarFinder.bodyType,
						preview: selectionObject['Body Type']
					},
					transmissionType: {
						...defaultFilters.newCarFinder.transmissionType,
						preview: selectionObject['Transmission Type']
					},
					fuelType: {
						...defaultFilters.newCarFinder.fuelType,
						preview: selectionObject['Fuel Type']
					},
					make: {
						...defaultFilters.newCarFinder.make,
						preview: selectionObject['Make']
					},
					seats: {
						...defaultFilters.newCarFinder.seats,
						preview: selectionObject['Seating Capacity']
					}
				}
			})
		}
	}

	shouldComponentUpdate(nextProps) {
		if(nextProps.active && !this.props.active){
			this.fireFiltersTracking()
		}
		if (this.props.active !== nextProps.active) {
			this.setScreen(nextProps.active)
			return nextProps.activeAccordion !== null && this.props.activeAccordion !== nextProps.activeAccordion
		}
		return true
	}

	componentWillUnmount() {
		window.removeEventListener('popstate', this.onPopState)
		clearTimeout(this.listingRefreshTimer)
	}

	componentDidMount() {
		this.setContentDimesion()
		window.addEventListener('popstate', this.onPopState);
	}

	setScreen = (active) => {
		try {
			if (active) {
				lockScroll()
				this.refs.filtersScreen.classList.add('screen--translate')
				this.refs.blackoutWindow.classList.add('screen--active')
				window.history.pushState('filtersScreen', '', '')
				this.refs.accordionList.scrollTo(0, 0)
			}
			else {
				this.refs.filtersScreen.classList.remove('screen--translate')

				unlockScroll()
				this.refs.blackoutWindow.classList.remove('screen--active')
			}
		}
		catch (e) {
			console.log(e)
		}
	}

	setContentDimesion = () => {
		let screenHeight = (75 * window.innerHeight) / 100
		this.refs.filtersScreen.style.height = `${screenHeight}px`

		// let accordionListHeight = this.refs.filtersScreen.offsetHeight - this.refs.filtersHead.offsetHeight

		// this.refs.accordionList.style.height = `${accordionListHeight}px`
	}

	trackFilterApply(filterParams, oldFilterParams) {
		const allKeys = [...Object.keys(oldFilterParams), ...Object.keys(filterParams)]
		const filterKeys = allKeys.filter((key, index, filterKeysRef ) => { return filterKeysRef.indexOf(key) == index});
		const changedFilters = filterKeys.reduce((acc, key) => {
			if (filterParams[key] && oldFilterParams[key]) {
				if (filterParams[key] instanceof Array) {
					acc = filterParams[key].equals(oldFilterParams[key]) ? acc : [...acc, key]
				}
				else if (filterParams[key] != oldFilterParams[key]) {
					acc = [...acc, key]
				}
			}
			else {
				acc = [...acc, key]
			}
			return acc
		}, [])
		fireInteractiveTracking(this.props.trackingCategory, "FilterApplyButtonClicked", changedFilters.join('_'))
		trackCustomData(this.props.trackingCategory,"FilterApplyButtonClicked","changedFilters="+changedFilters.join('_'),false)
	}
	getFilterStoreObject = (filterParams) => {
		let filters = {
			budget : [],
			bodyType: [],
			fuelType: [],
			make: [],
			transmissionType: [],
			seats: []
		}
		if(filterParams.budget != undefined) {
			filters.budget = [filterParams.budget]
		}
		if(filterParams.bodyStyleIds != undefined) {
			filters.bodyType = filterParams.bodyStyleIds
		}
		if(filterParams.fuelTypeIds != undefined) {
			filters.fuelType = filterParams.fuelTypeIds
		}
		if(filterParams.carMakeIds != undefined) {
			filters.make = filterParams.carMakeIds
		}
		if(filterParams.transmissionTypeIds != undefined) {
			filters.transmissionType = filterParams.transmissionTypeIds
		}
		if(filterParams.seats != undefined) {
			filters.seats = filterParams.seats
		}
		return filters
	}
	handleFilterApply = () => {
		const store = {
			...this.props.defaultFilters,
			newCarFinder: this.state,
			location:{
				cityId: this.props.cityId ? this.props.cityId : -1
			}
		}
		const filterParams = getNCFFilterParams(store)
		const { cityId } = deserialzeQueryStringToObject(this.props.location.search)
		if(cityId){
			filterParams.cityId = cityId
		}
		let filters = this.getFilterStoreObject(filterParams)
		const oldFilterParams = getNCFFilterParams(this.props.defaultFilters)
		const queryString = serialzeObjectToQueryString(filterParams)
		const oldQueryString = serialzeObjectToQueryString(oldFilterParams)
		if(queryString !== oldQueryString || queryString !== this.props.location.search.replace('?','')){
			this.props.closeFilterScreen()
			this.trackFilterApply(filterParams, oldFilterParams)
			this.props.setFilters(filters)
			this.listingRefreshTimer = setTimeout(() => {
				this.props.history.replace(this.props.location.pathname + '?' +
					serialzeObjectToQueryString(filterParams))
					const callbackFunction  = this.props.callbackFunction
					if(callbackFunction && !shallowequal(filterParams,oldFilterParams)) {
						window[callbackFunction](serialzeObjectToQueryString(filterParams))
					}
			}, 300)
		}
		else{
			window.history.back();
		}
	}

	handleFilterClose = () => {
		if(window.history.state == "filtersScreen"){
			window.history.back()
		}
	}

	handleSliderChange = ({ values }) => {
		if(!this.userSliderChange && values[0] === 200000){
			this.userSliderChange = false
			return
		}
		this.userSliderChange = false
		const data = this.state.budget
		data.slider = {
			...data.slider,
			values
		}
		data.inputBox = {
			...data.inputBox,
			value: values[0]
		}
		const preview = (values[0] > data.slider.min ? '₹ ' : '') + formatBudgetTooltipValue(values[0])
		this.setState({
			budget: {
				...this.state.budget,
				...data,
				preview
			}
		})
	}

	handleSliderDragStart = (event) => {
		this.userSliderChange = true
	}

	handleSliderClick = (event) => {
		this.userSliderChange = true
	}

	onPopState = (state) => {
		this.props.closeFilterScreen()
	}

	getItemStatus = (item) => {
		let status = ''

		if (item.isSelected) {
			status = 'active'
		}
		return status
	}

	// body type
	getBodyTypeList = (data, listType) => {
		if (data && data.length) {
			let list = data.map(item => {
				const itemStatusClass = this.getItemStatus(item)

				return (
					<li
						key={item.id}
						className={"filter-body-type__item " + listType + " " + itemStatusClass}
						onClick={this.handleBodyTypeClick.bind(this, item)}
					>
						<BodyTypeItem
							item={item}
							showCount={false}
						/>
					</li>
				)
			})
			return list
		}
	}

	handleBodyTypeClick = (item, event) => {
		createRipple(event)
		const preview = []
		const data = this.state.bodyType.data.map(x => {
			if (x.id === item.id) {
				if (!x.isSelected) preview.push(x.name)
				return {
					...x,
					isSelected: !x.isSelected
				}
			}
			if (x.isSelected) preview.push(x.name)
			return x
		})
		this.setState({
			bodyType: {
				...this.state.bodyType,
				data,
				preview: preview.join(', ')
			}
		})
	}

	// fuel type
	getFuelTypeList = (data) => {
		if (data && data.length) {
			let list = data.map(item => {
				const itemStatusClass = this.getItemStatus(item)

				return (
					<li
						key={item.id}
						className={"filter-fuel-type__item " + itemStatusClass}
						onClick={this.handleFuelTypeClick.bind(this, item)}
					>
						<ListItem
							item={item}
							showDescription={false}
						/>
					</li>
				)
			})
			return list
		}
	}

	handleFuelTypeClick = (item) => {
		const preview = []
		const data = this.state.fuelType.data.map(x => {
			if (x.id === item.id) {
				if (!x.isSelected) preview.push(x.name)
				return {
					...x,
					isSelected: !x.isSelected
				}
			}
			if (x.isSelected) preview.push(x.name)
			return x
		})
		const fuelTypePreview = reduceFuelTypeFilter(this.state.fuelType)
		this.setState((prevState) => {
			return {
				fuelType: {
					...prevState.fuelType,
					data,
					preview: preview.join(', ')
				}
			}
		})
	}
	//transmission type
	getTransissionTypeList = (data, listType) => {
		if (data && data.length) {
			let list = data.map(item => {
				const itemStatusClass = this.getItemStatus(item)
				return (
					<li
						key={item.id}
						className={"filter-transmission-type__item " + listType + " " + itemStatusClass}
						onClick={this.handleTransmissionTypeClick.bind(this, item)}
					>
						<ListItem
							item={item}
							showDescription={false}
						/>
					</li>
				)
			})
			return list
		}
	}
	handleTransmissionTypeClick = (item,event) => {
		createRipple(event)
		const preview = []
		const data = this.state.transmissionType.data.map(x => {
			if (x.id === item.id) {
				if (!x.isSelected) preview.push(x.name)
				return {
					...x,
					isSelected: !x.isSelected
				}
			}
			if (x.isSelected) preview.push(x.name)
			return x
		})
		const transmissionTypePreview = reduceTransmissionTypeFilter(this.state.transmissionType)
		this.setState((prevState) => {
			return {
				transmissionType: {
					...prevState.transmissionType,
					data,
					preview: preview.join(', ')
				}
			}
		})
	}

	//transmission type
	getSeatsList = (data, listType) => {
		if (data && data.length) {
			let list = data.map(item => {
				const itemStatusClass = this.getItemStatus(item)
				return (
					<li
						key={item.id}
						className={"filter-seats__item " + listType + " " + itemStatusClass}
						onClick={this.handleSeatsClick.bind(this, item)}
					>
						<ListItem
							item={item}
							showDescription={false}
						/>
					</li>
				)
			})
			return list
		}
	}
	handleSeatsClick = (item,event) => {
		createRipple(event)
		const preview = []
		const data = this.state.seats.data.map(x => {
			if (x.id === item.id) {
				if (!x.isSelected) preview.push(x.name)
				return {
					...x,
					isSelected: !x.isSelected
				}
			}
			if (x.isSelected) preview.push(x.name)
			return x
		})
		const seatsPreview = reduceSeatsFilter(this.state.seats)
		this.setState((prevState) => {
			return {
				seats: {
					...prevState.seats,
					data,
					preview: preview.join(', ')
				}
			}
		})
	}

	// make type
	getMakeList = (data) => {
		if (data!= null) {
			let list = data.map(item => {
				const itemStatusClass = this.getItemStatus(item)
				return (
					<li
						key={item.makeId}
						className={"filter-make__item " + " " + itemStatusClass}
						onClick={this.handleMakeClick.bind(this, item)}
					>
						<MakeItem
							item={item}
						/>
					</li>
				)
			})
			return list
		}
	}

	handleMakeClick = (item, event) => {
		createRipple(event)
		const preview = []
		const makeData = this.state.make.data.map(x => {
			if (x.makeId === item.makeId) {
				if (!x.isSelected) preview.push(x.makeName)
				return {
					...x,
					isSelected: !x.isSelected
				}
			}
			if (x.isSelected) preview.push(x.makeName)
			return x
		})
		this.setState({
			make: {
				...this.state.make,
				data:makeData,
				preview: preview.join(', ')
			}
		})
	}

	handleCollapsibleToggle = (key) => {
		this.setState((prevState) => {
			const newStatus = {}
			let budget = this.state.budget
			newStatus[key] = !prevState.collapsibleStatus[key]
			if(key == "Budget" && budget.slider.values[0] == ""){
				budget = this.state.budget
				budget.inputBox.value = this.defaultBudget
				budget.slider.values = [this.defaultBudget]
				budget.preview = (budget.slider.values[0] > budget.slider.min ? '₹ ' : '') + formatBudgetTooltipValue(budget.slider.values[0])
				return {
					collapsibleStatus: {
						...prevState.collapsibleStatus,
						...newStatus
					},
					budget: budget
				}
			}
			else{
				return {
					collapsibleStatus: {
						...prevState.collapsibleStatus,
						...newStatus
					}
				}
			}
		})
	}
	fireFiltersTracking = () => {
		let label = Object.keys(filterSelectors).join('_').trim()
		trackCustomData(this.props.trackingCategory,"AvailableFilters","availableFilters="+label)
		fireNonInteractiveTracking(this.props.trackingCategory, "AvailableFilters",label)
	}
	render() {
		let {
			activeAccordion,
		} = this.props
		let slider = this.state.budget.slider
		const collapsibleStatus = this.state.collapsibleStatus
		slider = {
			...slider,
			className: 'filter-budget-rheostat',
			pitComponent: PitComponent,
			pitPoints: [slider.min, slider.max],
			pitPointLabel: formatBudgetTooltipValue,
			snap: true,
			snapOnDragMove: true,
			disableSnapOnClick: true,
			onChange: this.handleSliderChange,
			handleTooltipLabel: formatBudgetTooltipValue,
			onSliderDragStart: this.handleSliderDragStart,
			onClick: this.handleSliderClick
		}

		const bodyTypeData = this.state.bodyType.data
		const fuelTypeData = this.state.fuelType.data
		const makeData = this.state.make.data
		const transmissionTypeData = this.state.transmissionType.data
		const seatsData = this.state.seats.data
		const budget = 'Budget', fuelType = 'Fuel Type', bodyType = 'Body Type', make = 'Make',transmissionType = 'Transmission Type',seats = 'Seating Capacity'
		const budgetCollapsibleProps = {
			title: budget,
			open: collapsibleStatus[budget],
			selectionPreview: this.state.budget.preview,
			onToggle: this.handleCollapsibleToggle.bind(this, budget)
		}

		const bodyTypeCollapsibleProps = {
			title: bodyType,
			open: collapsibleStatus[bodyType],
			selectionPreview: this.state.bodyType.preview,
			onToggle: this.handleCollapsibleToggle.bind(this, bodyType)
		}

		const fuelTypeCollapsibleProps = {
			title: fuelType,
			open: collapsibleStatus[fuelType],
			selectionPreview: this.state.fuelType.preview,
			onToggle: this.handleCollapsibleToggle.bind(this, fuelType)
		}

		const transmissionTypeCollapsibleProps = {
			title: transmissionType,
			open: collapsibleStatus[transmissionType],
			selectionPreview: this.state.transmissionType.preview,
			onToggle: this.handleCollapsibleToggle.bind(this, transmissionType)
		}

		const seatsCollapsibleProps = {
			title: seats,
			open: collapsibleStatus[seats],
			selectionPreview: this.state.seats.preview,
			onToggle: this.handleCollapsibleToggle.bind(this, seats)
		}

		const makeCollapsibleProps = {
			title: make,
			open: collapsibleStatus[make],
			selectionPreview: this.state.make.preview,
			onToggle: this.handleCollapsibleToggle.bind(this, make)
		}
		return (
			<div>
				<div ref="filtersScreen" className="filters__screen">
					<div ref="filtersContent" className="filters__content">
						<div className="filters-head__content">
							<div ref="filtersHead" className="filters-screen__head">
									<div className="filters-head__left-content">
										<span className="filters-head__title">Filter</span>
										{/* <span className="filters-head__count">10 cars found</span> */}
									</div>
								{/* <div className="filter__apply" onClick={this.handleFilterApply}>Apply</div> */}
							</div>
						</div>
						<ul ref="accordionList" className="accordion__list">
							<li className="accordion-list__item" ref={budget} >
								<BudgetCollapsible collapsibleProps={budgetCollapsibleProps} slider={slider} />
							</li>
							<li className="accordion-list__item" ref={bodyType.replace(' ', '')}>
								<BodyTypeCollapsible collapsibleProps={bodyTypeCollapsibleProps} getBodyTypeList={this.getBodyTypeList.bind(this, bodyTypeData, '')} />
							</li>
							<li className="accordion-list__item" ref={fuelType.replace(' ', '')}>
								<FuelTypeCollapsible collapsibleProps={fuelTypeCollapsibleProps} getFuelTypeList={this.getFuelTypeList.bind(this, fuelTypeData)} />
							</li>
							<li className="accordion-list__item" ref={make.replace(' ', '')}>
								<MakeCollapsible collapsibleProps={makeCollapsibleProps} getMakeList={this.getMakeList.bind(this, makeData)} />
							</li>
							<li className="accordion-list__item" ref={transmissionType.replace(' ', '')}>
								<TransmissionTypeCollapsible collapsibleProps={transmissionTypeCollapsibleProps} getTransissionTypeList={this.getTransissionTypeList.bind(this, transmissionTypeData, '')} />
							</li>
							<li className="accordion-list__item" ref={seats.replace(' ', '')}>
								<SeatsCollapsible collapsibleProps={seatsCollapsibleProps} getSeatsList={this.getSeatsList.bind(this, seatsData, '')} />
							</li>
						</ul>
						<div onClick={this.handleFilterApply} id="btnApplyFilter" className="apply-filters">
							<button className="btn-primary btn-full-width">Apply Filters</button>
						</div>
					</div>
				</div>
				<div
					ref="blackoutWindow"
					className="filter__blackout-window"
					onClick={this.handleFilterClose}
				></div>
			</div>
		)
	}
}

const mapStateToProps = (state) => {
	const {
		active,
		activeAccordion
	} = state.filtersScreen

	return {
		active,
		activeAccordion
	}
}

const mapDispatchToProps = (dispatch, getState) => {
	return {
		closeFilterScreen: bindActionCreators(closeFilterScreen, dispatch),
		setFilters: bindActionCreators(setFilters, dispatch)
	}
}

export default connect(mapStateToProps, mapDispatchToProps)(FiltersScreen)
