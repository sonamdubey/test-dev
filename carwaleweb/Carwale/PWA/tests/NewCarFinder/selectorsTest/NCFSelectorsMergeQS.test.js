import * as NCFSelectors from '../../../src/NewCarFinder/selectors/NCFSelectors'
import ncfDefaultFilters from '../../../src/NewCarFinder/constants/NCFdefaultfilters'
import { ncfFilters, ncfExpectedFilters} from './../NcfFilters'

describe('mergeBudgetValues', () => {
    it('should return updated buget object with value of budget passed', () => {
        let store = ncfDefaultFilters()
        let expectedBudget = {budget: {}}
        expectedBudget.budget = ncfExpectedFilters.newCarFinder.budget

        let updatedBudget = NCFSelectors.mergeBudgetValues('800000', store)
        expect(updatedBudget).toEqual(expectedBudget)
    })
})

describe('mergeBodyTypeFilterValues', () => {
    let store = ncfDefaultFilters()
    it('should return updated bodytype object with isSelected = true for given ids', () => {
        let expectedBodyType = {bodyType: {}}
        expectedBodyType.bodyType = ncfExpectedFilters.newCarFinder.bodyType
        let updatedBodyType = NCFSelectors.mergeBodyTypeFilterValues('6,10', store)

        expect(updatedBodyType).toEqual(expectedBodyType)
    })

    it('should return updated bodytype object with isSelected = false when no ids given', () => {
        let expectedBodyType = {bodyType: {}}
        expectedBodyType.bodyType = store.newCarFinder.bodyType
        let updatedBodyType = NCFSelectors.mergeBodyTypeFilterValues('', store)

        expect(updatedBodyType).toEqual(expectedBodyType)
    })
})

describe('mergeFuelTypeFilterValues', () => {
    let store = ncfDefaultFilters()
    it('should return updated fuel object with isSelected = true for given ids', () => {
        let expectedFuelType = {fuelType: {}}
        expectedFuelType.fuelType = ncfExpectedFilters.newCarFinder.fuelType
        let updatedFuelType = NCFSelectors.mergeFuelTypeFilterValues('1,3', store)

        expect(updatedFuelType).toEqual(expectedFuelType)
    })

    it('should return updated fuel object with isSelected = false when no ids given', () => {
        let expectedFuelType = {fuelType: {}}
        expectedFuelType.fuelType = store.newCarFinder.fuelType
        let updatedFuelType = NCFSelectors.mergeFuelTypeFilterValues('', store)

        expect(updatedFuelType).toEqual(expectedFuelType)
    })
})

describe('mergeLocationValues', () => {
    let store = ncfDefaultFilters()
    it('should return updated bodytype object with isSelected = true for given ids', () => {
        let expectedLocation = ncfExpectedFilters.location
        let updatedLocation = NCFSelectors.mergeLocationValues(1, store)

        expect(updatedLocation).toEqual(expectedLocation)
    })
})

describe('mergeFuelTypeFilterValues', () => {
    let store = ncfDefaultFilters()
    it('should return updated fuel object with isSelected = true for given ids', () => {
        let expectedFuelType = {fuelType: {}}
        expectedFuelType.fuelType = ncfExpectedFilters.newCarFinder.fuelType
        let updatedFuelType = NCFSelectors.mergeFuelTypeFilterValues('1,3', store)

        expect(updatedFuelType).toEqual(expectedFuelType)
    })

    it('should return updated fuel object with isSelected = false when no ids given', () => {
        let expectedFuelType = {fuelType: {}}
        expectedFuelType.fuelType = store.newCarFinder.fuelType
        let updatedFuelType = NCFSelectors.mergeFuelTypeFilterValues('', store)

        expect(updatedFuelType).toEqual(expectedFuelType)
    })
})

describe('mergeLocationValues', () => {
    let store = ncfDefaultFilters()
    it('should return updated bodytype object with isSelected = true for given ids', () => {
        let expectedLocation = ncfExpectedFilters.location
        let updatedLocation = NCFSelectors.mergeLocationValues(1, store)

        expect(updatedLocation).toEqual(expectedLocation)
    })
})

describe('mergeMakeFilterValues', () => {
    let store = ncfDefaultFilters()
    it('should return updated make object with isSelected = true for given ids', () => {
        let expectedMake = {make: {}}
        expectedMake.make = ncfExpectedFilters.newCarFinder.make
        let updatedMake = NCFSelectors.mergeMakeFilterValues('8,10', store)

        expect(updatedMake).toEqual(expectedMake)
    })

    it('should return updated make object with isSelected = false when no ids given', () => {
        let expectedMake = {make: {}}
        expectedMake.make = ncfExpectedFilters.newCarFinder.make
        let updatedMake = NCFSelectors.mergeMakeFilterValues('', store)

        expect(updatedMake).toEqual(expectedMake)
    })
})

describe('mergeLocationValues', () => {
    let store = ncfDefaultFilters()
    it('should return updated bodytype object with isSelected = true for given ids', () => {
        let expectedLocation = ncfExpectedFilters.location
        let updatedLocation = NCFSelectors.mergeLocationValues(1, store)

        expect(updatedLocation).toEqual(expectedLocation)
    })
})
describe ('mergeQStoStore', () => {
    let store = ncfDefaultFilters()
    it('should update state with the parameters given in QueryStrings', () => {
        let qs = {
            cityId: '1',
            budget: '800000',
            bodyStyleIds: '6,10',
            fuelTypeIds: '1,3',
            carMakeIds: '2,8,10'
        }
        let expected = ncfExpectedFilters
        let updated = NCFSelectors.mergeQStoStore(qs, store)

        expect(updated).toEqual(expected)
    })
})
