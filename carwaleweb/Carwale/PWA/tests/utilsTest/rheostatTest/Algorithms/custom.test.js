import algorithm from '../../../../src/utils/rheostat/algorithms/custom'

describe('Test getPosition', () => {
    it('gives position of slider when budget value is passed', () => {
        let pos = algorithm.getPosition(1000, 200000, 10000000)
        expect(pos).toBe(0)

        pos = algorithm.getPosition(10000000, 200000, 10000000)
        expect(pos).toBe(100)

        pos = algorithm.getPosition(100000000, 200000, 10000000)
        expect(pos).toBe(100)

        pos = algorithm.getPosition(1600000, 200000, 10000000)
        expect(pos).toBe(48.07692307692307)
    })
})

describe('Test getValue', () => {
    it('gives value corresponding to slider position', () => {
        let value = algorithm.getValue(48.07692307692307, 200000, 10000000)
        expect(value).toBe(1600000)

        value = algorithm.getValue(-1, 200000, 10000000)
        expect(value).toBe(200000)

        value = algorithm.getValue(110, 200000, 10000000)
        expect(value).toBe(10000000)
    })
})
