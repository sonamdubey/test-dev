import { firePageView } from '../../src/utils/Analytics'

describe('Test firePageView', () => {
    it('should fire pageview only after ga is defined', () => {
        let location = {
            pathname: '/find-car/filters/budget/',
            search: ''
        }

        for (let i = 0; i < 5; i++) {
            firePageView(location)
        }
        global.ga = jest.fn()

        firePageView(location)

        expect(global.ga).toHaveBeenCalledTimes(12) //since two calls per pageview

        global.ga = undefined
    })
})
