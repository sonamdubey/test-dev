import Cookies  from 'js-cookie'

let abTestValue

export const setAbtestCookieValue = () => {
    abTestValue = parseInt(Cookies.get('_abtest'))
}
