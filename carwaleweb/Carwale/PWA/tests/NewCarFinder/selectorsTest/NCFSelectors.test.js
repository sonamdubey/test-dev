import * as NCFSelectors from '../../../src/NewCarFinder/selectors/NCFSelectors'
import ncfDefaultFilters from '../../../src/NewCarFinder/constants/NCFdefaultfilters'
import { ncfFilters } from './../NcfFilters'




describe('getNCFFilterParams with defaul parameters', () => {
    let store
    let objectToSerialized
    let filterParamsObj
    it('check with some filters applied',() => {
        store = getMockInitialStore()
        objectToSerialized = { budget: 300000, cityId: 1}
        filterParamsObj = NCFSelectors.getNCFFilterParams(store)
        expect(filterParamsObj).toEqual(objectToSerialized)
    })

    it('check with all filters applied', () => {
        store = getMockInitialStore();
        objectToSerialized = {budget: 300000, cityId: 1, bodyStyleIds: [6,10], fuelTypeIds: [2]}
        store.newCarFinder.bodyType.data = ncfFilters.newCarFinder.bodyType.data
        store.newCarFinder.fuelType.data = ncfFilters.newCarFinder.fuelType.data
        filterParamsObj = NCFSelectors.getNCFFilterParams(store)
        expect(filterParamsObj).toEqual(objectToSerialized)
    })
})


describe('getNCFFilterParams with given parameter', () => {
    let store = getMockInitialStore()
    let objectToSerialized
    let filterParamsObj
    store.newCarFinder.bodyType.data = ncfFilters.newCarFinder.bodyType.data
    store.newCarFinder.fuelType.data = ncfFilters.newCarFinder.fuelType.data

    it('check when budget and body type filter applied',() => {
        objectToSerialized = {budget: 300000, cityId: 1, bodyStyleIds: [6,10]}
        filterParamsObj = NCFSelectors.getNCFFilterParams(store, ["BudgetFilter", "BodyTypeFilter"])
        expect(filterParamsObj).toEqual(objectToSerialized)
    })

    it('check when fuel and body type filter applied',() => {
        objectToSerialized = {cityId: 1, bodyStyleIds: [6,10], fuelTypeIds: [2]}
        filterParamsObj = NCFSelectors.getNCFFilterParams(store, ["BodyTypeFilter","FuelTypeFilter"])
        expect(filterParamsObj).toEqual(objectToSerialized)
    })

})

                    //-------------test cases for reduceBudgetFilter----------

describe('Test reduceBudgetFilter', () => {
    let store
    it('should return reduced buget object containing formated budget if slider.value > 2,00,000', () => {
        store = getMockInitialStore().newCarFinder.budget
        let expectedOutput = {}
        expectedOutput[store.displayName] =  '₹ 3 lakh'
        let reducedBudget = NCFSelectors.reduceBudgetFilter(store)
        expect(reducedBudget).toEqual(expectedOutput)
    })
    it('should return Upto 2 lakh when slider.value = 2,00,000', () => {
        store = getMockInitialStore().newCarFinder.budget
        store.slider.values[0] = 200000
        let reducedBudget = NCFSelectors.reduceBudgetFilter(store)
        expect(reducedBudget).toEqual({"Budget": "Upto 2 lakh"})
    })
    it('should return undefied(nothing) when slider.value < 2,00,000', () => {
        store = getMockInitialStore().newCarFinder.budget
        store.slider.values[0] = 100000
        let reducedBudget = NCFSelectors.reduceBudgetFilter(store)
        expect(reducedBudget).toBeUndefined()
    })
})

                    //-------------test cases for reduceBodyTypeFilter----------
describe('reduceBodyTypeFilter', () => {
    let store = getMockInitialStore().newCarFinder.bodyType
    let reducedBodyType, expectedOutput = {}

    it('should return reduced bodytype object containing display name and comma seperated seleted bodytypes', () => {
        store.data = ncfFilters.newCarFinder.bodyType.data
        reducedBodyType = NCFSelectors.reduceBodyTypeFilter(store)
        expectedOutput[store.displayName] = 'SUV/MUV, Compact Sedan'
        expect(reducedBodyType).toEqual(expectedOutput)
    })

    it('should return undefined when no bodytype is selected or store is null', () => {
        store.data = [{
            id: 3,
            name: 'Hatchback',
            icon: 'https://imgd.aeplcdn.com//0x0/cw/body/svg/hatchback_clr.svg',
            lineIcon: 'https://imgd.aeplcdn.com//0x0/cw/body/svg/hatchback.svg',
            isSelected: false
        }]
        reducedBodyType = NCFSelectors.reduceBodyTypeFilter(store)
        expect(reducedBodyType).toBeUndefined()

        store = null
        reducedBodyType = NCFSelectors.reduceBodyTypeFilter(store)
        expect(reducedBodyType).toBeUndefined()
    })
})

                    //-------------test cases for reduceFuelTypeFilter----------
describe('Test reduceFuelTypeFilter', () => {
    let reducedFuelType, expectedOutput = {}
    it('should return undefined when no Fueltype is selected or store is null', () => {
        let store = getMockInitialStore().newCarFinder.bodyType
        store.data = [{
            id: 1,
            name: 'Petrol',
            icon: 'https://imgd.aeplcdn.com/0x0/cw/fuel/svg/petrol.svg',
            isSelected: false
        }]
        reducedFuelType = NCFSelectors.reduceFuelTypeFilter(store)
        expect(reducedFuelType).toBeUndefined()

        store = null
        reducedFuelType = NCFSelectors.reduceFuelTypeFilter(store)
        expect(reducedFuelType).toBeUndefined()
    })

    it('should return reduced fuelType object containing display name and comma seperated seleted FuelType', () => {
        let store = getMockInitialStore().newCarFinder.bodyType
        store.data = ncfFilters.newCarFinder.fuelType.data
        reducedFuelType = NCFSelectors.reduceFuelTypeFilter(store)
        expectedOutput[store.displayName] = 'Diesel'
        expect(reducedFuelType).toEqual(expectedOutput)
    })
})
