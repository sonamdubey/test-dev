import { shallow, mount, render, configure } from 'enzyme'
import Adapter from 'enzyme-adapter-react-16'
import thunk from 'redux-thunk'
import configureStore from 'redux-mock-store'
import snapPoints from '../src/utils/rheostat/constants/budgetSnapPoints'
import 'raf/polyfill'

configure({ adapter: new Adapter() })

/**
 * Make Enzyme functions available in all test files without importing
 */
global.shallow = shallow;
global.render = render;
global.mount = mount;


const shallowWithStore = (component, store) => {
    const context = {
        store
    }
    return shallow(component, { context })
}

const mountWithStore = (component, store) => {
    const context = {
        store
    }
    return mount(component, { context })
}

const middlewares = [ thunk ]

const mockStore = configureStore(middlewares)

const getMockInitialStore = () => { return {
    newCarFinder:{
        bodyType: {
            displayName: "Body Type",
            isFetching: false,
            data: []
        },
        fuelType: {
            displayName: "Fuel Type",
            isFetching: false,
            data: []
        },
        budget: {
            displayName: "Budget",
            slider:{
                values:[300000],
                min:200000,
                max:10000000,
                userChange:false,
                snapPoints
            },
            inputBox:{
                value: 300000
            },
        },
        make:{
            displayName: "Make",
            isFetching: false,
            data: []
        },
        filter: {
            filterScreens: {
                screenOrder: [
                    {
                        name:"BudgetFilter",
                        path:"/find-car/filters/budget"
                    },
                    {
                        name:"BodyTypeFilter",
                        path:"/find-car/filters/body-type"
                    },
                    {
                        name:"FuelTypeFilter",
                        path:"/find-car/filters/fuel-type"
                    },
                    {
                        name:"MakeFilter",
                        path:"/find-car/filters/make"
                    }
                ]
            }
        }
    },
    toast:{
        isVisible: false,
        message: '',
        duration: 3000,
        toastTimerId: null,
        style: null
    },
    location: {
        cityname: "Mumbai",
        cityId:1,
        userConfirmed: false
    }
}}
global.BHRIGU_HOST_URL = JSON.stringify('/bhrigu/pixel.gif?t=')

global.shallowWithStore = shallowWithStore;
global.mountWithStore = mountWithStore;
global.mockStore = mockStore;
global.getMockInitialStore = getMockInitialStore;
global.dataLayer =[]
jest.mock("../src/utils/Storage")
jest.mock("../src/utils/Ripple")
