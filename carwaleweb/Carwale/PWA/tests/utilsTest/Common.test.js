import * as Common from '../../src/utils/Common'
import ncfDefaultFilters from '../../src/NewCarFinder/constants/NCFdefaultfilters'

let queryParams = {
	cityId: '1',
	budget: '800000',
	bodyStyleIds: '6,10',
	fuelTypeIds: '1,3'
}

describe('deserialzeQueryStringToObject', () => {
	it('test deserialize query string to object', () => {
		let qs = '?cityId=1&budget=800000&bodyStyleIds=6%2C10&fuelTypeIds=1%2C3'
		let qsObj = Common.deserialzeQueryStringToObject(qs)
		expect(qsObj).toEqual(queryParams)
	})
})

describe('serialzeObjectToQueryString', () => {
	it('test serialize object to query string', () => {
		let expectedQs = 'cityId=1&budget=800000&bodyStyleIds=6%2C10&fuelTypeIds=1%2C3'
		let qs = Common.serialzeObjectToQueryString(queryParams)
		expect(qs).toEqual(expectedQs)
	})
})


// describe('formatValueWithComma', () => {
// 	it('should remove special letters from input value and give simple number', () => {
// 		let simpleNumber = Common.formatValueWithComma({value: '123,45,000'})
// 		expect(simpleNumber).toBe(12345000)

// 		simpleNumber = Common.formatValueWithComma({value: 'adf123,*-*--*--+++45,00-0'})
// 		expect(simpleNumber).toBe(12345000)
// 	})
// })

describe('Test formatToINR', () => {
	it('should return comma seperated value in INR format', () => {
		let formattedPrice = Common.formatToINR(1)
		expect(formattedPrice).toBe('1')

		formattedPrice = Common.formatToINR('')
		expect(formattedPrice).toBe('')

		formattedPrice = Common.formatToINR(100)
		expect(formattedPrice).toBe('100')

		formattedPrice = Common.formatToINR(1000)
		expect(formattedPrice).toBe('1,000')

		formattedPrice = Common.formatToINR(100000)
		expect(formattedPrice).toBe('1,00,000')

		formattedPrice = Common.formatToINR(10000000)
		expect(formattedPrice).toBe('1,00,00,000')
	})
})

describe('Test Array Equal', () => {
	it('should compare two simple array', () => {
		expect([1, 2, [3, 4]].equals([1, 2, [3, 2]])).toBe(false)

		expect([1, "2,3"].equals([1, 2, 3])).toBe(false)

		expect([1, 2, [3, 4]].equals([1, 2, [3, 4]])).toBe(true)

		expect([1, 2, 1, 2].equals([1, 2, 1, 2])).toBe(true)

		expect([{a:1}].equals([{a:1}])).toBe(false)

		expect([1,2].equals([1])).toBe(false)
	})
})

