import {getCityId} from '../../selectors/LocationSelectors'
import { formatBudgetTooltipValue } from '../utils/Budget'

export const getScreenId = (store,name) => {
	const screenId = (store.newCarFinder.filter.filterScreens.screenOrder) ? store.newCarFinder.filter.filterScreens.screenOrder.findIndex(x => x.name === name) : 1;
	return screenId
}

export const getBudgetFilter = store => {
	const budget = store.newCarFinder.budget.inputBox.value
	if(budget){
		return {
			budget
		}
	}
}

export const getBodyTypeFilter = store => {
    const bodyStyleIds = store.newCarFinder.bodyType.data.filter( x => x.isSelected).map(x => x.id)
	if( bodyStyleIds.length){
		return {
			bodyStyleIds
		}
	}
}

export const getTransmissionTypeFilter  = store => {
    const transmissionTypeIds = store.newCarFinder.transmissionType.data.filter( x => x.isSelected).map(x => x.id)
	if( transmissionTypeIds.length){
		return {
			transmissionTypeIds
		}
	}
}

export const getSeatsFilter  = store => {
    const seats = store.newCarFinder.seats.data.filter( x => x.isSelected).map(x => x.id)
	if( seats.length){
		return {
			seats
		}
	}
}

export const getFuelTypeFilter = store => {
	const fuelTypeIds = store.newCarFinder.fuelType.data.filter( x => x.isSelected)
	.reduce((selected,fuelType) => selected.concat(fuelType.id),[])
	if( fuelTypeIds.length ){
		return {
			fuelTypeIds
		}
	}
}

export const getMakeFilter = (store) => {
	let makeData = store.newCarFinder.make.data.otherMakes
		makeData = makeData ? makeData.concat(store.newCarFinder.make.data.popularMakes)
							: store.newCarFinder.make.data.popularMakes
	if(!makeData)
	{
		makeData = store.newCarFinder.make.data
	}

	let carMakeIds
	if(makeData){
		carMakeIds = makeData.filter(make => make.isSelected)
		.reduce((selected,make) => selected.concat(make.makeId),[])
	}
	if(carMakeIds != undefined && carMakeIds.length){
		return {
			carMakeIds
		}
	}
}
// filterSelector is used in FilterScreen.js to fetch all available filter
export const filterSelectors = {
	"BudgetFilter": getBudgetFilter,
	"BodyTypeFilter": getBodyTypeFilter,
	"FuelTypeFilter": getFuelTypeFilter,
	"MakeFilter": getMakeFilter,
	"TransmissionTypeFilter": getTransmissionTypeFilter,
	"SeatsFilter": getSeatsFilter
}

export const getNCFFilterParams = (store, currentFilters) => {
	if(!currentFilters){
		currentFilters = Object.keys(filterSelectors)
	}
	const filterOptions = currentFilters.reduce((acc,screenName) => {return { ...acc, ...filterSelectors[screenName](store)}}, {})
    return { ...getCityId(store), ...filterOptions}
}

export const reduceBudgetFilter = (store) => {
	const {slider} = store
	if (slider && slider.values[0] >= slider.min){
		const obj = {}
		obj[store.displayName] = (slider.values[0] > slider.min ? '₹ ' : '') + formatBudgetTooltipValue(slider.values[0])
		return obj
	}
}

export const reduceBodyTypeFilter = (store) => {
	if(store){
		const bodyTypes = store.data.filter(x => x.isSelected).map( x => x.name).join(', ')
		if(bodyTypes){
			const obj = {}
			obj[store.displayName] = bodyTypes
			return obj
		}
	}
}

export const reduceTransmissionTypeFilter  = (store) => {
	if(store){
		const transmissionType = store.data.filter(x => x.isSelected).map( x => x.name).join(', ')
		if(transmissionType){
			const obj = {}
			obj[store.displayName] = transmissionType
			return obj
		}
	}
}

export const reduceSeatsFilter  = (store) => {
	if(store){
		const seats = store.data.filter(x => x.isSelected).map( x => x.name).join(', ')
		if(seats){
			const obj = {}
			obj[store.displayName] = seats
			return obj
		}
	}
}

export const reduceFuelTypeFilter = (store) => {
	if(store){
		const fuelTypes = store.data.filter(x => x.isSelected).map( x => x.name).join(', ')
		if(fuelTypes){
			const obj = {}
			obj[store.displayName] = fuelTypes
			return obj
		}
	}
}

export const reduceMakeFilter = (store) => {
	if(store){
		const make = store.data.filter(x => x.isSelected).map( x => x.makeName).join(', ')
		if(make){
			const obj = {}
			obj[store.displayName] = make
			return obj
		}
	}
}

const selectionReducers = {
	"budget": reduceBudgetFilter,
	"bodyType": reduceBodyTypeFilter,
	"fuelType": reduceFuelTypeFilter,
	"make": reduceMakeFilter,
	"transmissionType": reduceTransmissionTypeFilter,
	"seats": reduceSeatsFilter
}

export const reduceToSelectionObject = (store) => {
	return Object.keys(selectionReducers).reduce((acc,key) => {return { ...acc, ...selectionReducers[key](store.newCarFinder[key])}}, {})
}

export const mergeBudgetValues = (budget,store) => {
	const budgetStore = store.newCarFinder.budget
	const value =  budget ? parseInt(budget) : ''
	return {
		budget:{
			...budgetStore,
			slider:{
				...budgetStore.slider,
				values: [value]
			},
			inputBox:{
				value
			}
		}
	}
}

export const mergeBodyTypeFilterValues = (bodyTypeIds,store) => {
	const bodyTypeStore = store.newCarFinder.bodyType
	bodyTypeIds = bodyTypeIds.split(',')
	const data = bodyTypeStore.data.map( x => {
										if(bodyTypeIds.findIndex(y => y==x.id) > -1)
										{
											x.isSelected = true
										}
										return x
								})
	return {
		bodyType:{
			...bodyTypeStore,
			data
		}
	}
}

export const mergeTransmissionFilterValues  = (transmissionTypeIds,store) => {
	const tranmissionTypeStore = store.newCarFinder.transmissionType
	transmissionTypeIds = transmissionTypeIds.split(',')
	const data = tranmissionTypeStore.data.map( x => {
										if(transmissionTypeIds.findIndex(y => y==x.id) > -1)
										{
											x.isSelected = true
										}
										return x
								})
	return {
		transmissionType:{
			...tranmissionTypeStore,
			data
		}
	}
}

export const mergeSeatsFilterValues  = (seats,store) => {
	const seatsStore = store.newCarFinder.seats
	seats = seats.split(',')
	const data = seatsStore.data.map( x => {
										if(seats.findIndex(y => y==x.id) > -1)
										{
											x.isSelected = true
										}
										return x
								})
	return {
		seats:{
			...seatsStore,
			data
		}
	}
}

export const mergeFuelTypeFilterValues = (fuelTypeIds,store) => {
	const fuelTypeStore = store.newCarFinder.fuelType
	fuelTypeIds = fuelTypeIds.split(',')
	const data = fuelTypeStore.data.map( x => {
										if(fuelTypeIds.findIndex(y => y==x.id) > -1)
										{
											x.isSelected = true
										}
										return x
								})
	return {
		fuelType:{
			...fuelTypeStore,
			data
		}
	}
}

export const mergeMakeFilterValues = (makeIds,store) => {
	const makeStore = store.newCarFinder.make
	makeIds = makeIds.split(',')
	const newData = makeStore.data.map( x => {
		if(makeIds.findIndex(y => y==x.makeId) > -1)
		{
			x.isSelected = true
		}
		return x
	})
	return {
		make:{
			...makeStore,
			data:newData
		}
	}
}

export const mergeLocationValues = (cityId,store) => {
	const locationStore = store.location
	return {
			...locationStore,
			cityId
		}
}

const mergeQShandlers = {
	newCarFinder:{
		budget: mergeBudgetValues,
		bodyStyleIds: mergeBodyTypeFilterValues,
		fuelTypeIds: mergeFuelTypeFilterValues,
		carMakeIds: mergeMakeFilterValues,
		transmissionTypeIds: mergeTransmissionFilterValues,
		seats: mergeSeatsFilterValues
	},
	location:{
		cityId: mergeLocationValues
	}
}

export const mergeQStoStore = (qs, store) => {
	const newCarFinderStore = {
			location: Object.keys(mergeQShandlers.location).reduce((acc,key) => {
				return {
				...acc,
				...(mergeQShandlers.location[key](qs[key] ? parseInt(qs[key]) : -1,store))
				}
			}, {}),
			newCarFinder : Object.keys(mergeQShandlers.newCarFinder).reduce((acc,key) => {
				return {
				...acc,
				...(mergeQShandlers.newCarFinder[key](qs[key] ? qs[key]: '',store))
				}
			}, {})
	}

	return {
		location:{
			...store.location,
			...newCarFinderStore.location
		},
		newCarFinder:{
			...store.newCarFinder,
			...newCarFinderStore.newCarFinder
		}
	}
}

export const getFilterStoreObject = (filterParams) => {
	let filters = {
		budget : [],
		bodyType: [],
		fuelType: [],
		make: [],
		transmissionType: [],
		seats: []
	}
	if(filterParams.budget != undefined) {
		filters.budget = [parseInt(filterParams.budget)]
	}
	if(filterParams.bodyStyleIds != undefined) {
		filters.bodyType = filterParams.bodyStyleIds.split(',').map(function(item) {
			return parseInt(item, 10)});
	}
	if(filterParams.fuelTypeIds != undefined) {
		filters.fuelType = filterParams.fuelTypeIds.split(',').map(function(item) {
			return parseInt(item, 10)});
	}
	if(filterParams.carMakeIds != undefined) {
		filters.make = filterParams.carMakeIds.split(',').map(function(item) {
			return parseInt(item, 10)});
	}
	if(filterParams.transmissionTypeIds != undefined) {
		filters.transmissionType = filterParams.transmissionTypeIds.split(',').map(function(item) {
			return parseInt(item, 10)});
	}
	if(filterParams.seats != undefined) {
		filters.seats = filterParams.seats.split(',').map(function(item) {
			return parseInt(item, 10)});
	}
	return filters
}

export const modifySearchParamsWithStoredFilters = (searchParams,storedFilters) => {
	if(storedFilters.budget.length > 0)
	{
		searchParams.budget = storedFilters.budget[0].toString()
	}
	if(storedFilters.bodyType.length > 0)
	{
		searchParams.bodyStyleIds = searchParams.bodyStyleIds +","+ storedFilters.bodyType.join(",")
	}
	if(storedFilters.fuelType.length > 0)
	{
		searchParams.fuelTypeIds = searchParams.fuelTypeIds +","+ storedFilters.fuelType.join(",")
	}
	if(storedFilters.make.length > 0)
	{
		searchParams.carMakeIds = searchParams.carMakeIds +","+ storedFilters.make.join(",")
	}
	if(storedFilters.transmissionType.length > 0)
	{
		searchParams.transmissionTypeIds = searchParams.transmissionTypeIds +","+ storedFilters.transmissionType.join(",")
	}
	if(storedFilters.seats.length > 0)
	{
		searchParams.seats = searchParams.seats +","+ storedFilters.seats.join(",")
	}
	return searchParams
}
