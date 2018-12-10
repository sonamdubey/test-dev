import Cookies from 'js-cookie'

/**
 * Check if the type of storage is available
 * @param {string} type
 * @returns bool
 */
const isStorageAvailable = (type) => {
    try {
        let storage = window[type], x = '__storage_test__'
        storage.setItem(x, x)
        storage.removeItem(x)
        return true
    }
    catch (error) {
        return false
    }
}

const sessionStorageAvailable = isStorageAvailable('sessionStorage')
const localStorageAvailable = isStorageAvailable('localStorage')

/**
 * Set value for key in sessionStorage if available
 * else set value in session Cookie
 * @param { string } key
 * @param { * } value
 */
const setSessionValue = (key, value) => {
    try {
        let serializedValue = JSON.stringify(value)
        if (sessionStorageAvailable) {
            sessionStorage.setItem(key, serializedValue)
        }
        else {
            Cookies.set(key, serializedValue, { domain: COOKIE_DOMAIN })
        }
    } catch (error) {
        console.log("setSessionValue error: ", key, value)
    }
}


/**
 * Set value for key in localStorage if available
 * else set value in Cookie with expiry 180 days
 * @param { string } key
 * @param { * } value
 */
const setValue = (key, value) => {
    try {
        let serializedValue = JSON.stringify(value)
        if (localStorageAvailable) {
            localStorage.setItem(key, serializedValue)
        }
        else {
            Cookies.set(key, serializedValue, { expires: 180, domain: COOKIE_DOMAIN })
        }
    } catch (error) {
        console.log("setValue error: ", key, value)
    }
}

/**
 * Return session value for the key
 * return undefined in case of error
 * @param { string } key
 * @returns *
 */
const getSessionValue = (key) => {
    try {
        let serializedValue = sessionStorageAvailable ? sessionStorage.getItem(key) : Cookies.get(key)
        return JSON.parse(serializedValue)
    } catch (error) {
        return undefined
    }
}

/**
 * Return value for the key,
 * return undefined in case of error
 * @param { string } key
 * @returns *
 */
const getValue = (key) => {
    try {
        let serializedValue = localStorageAvailable ? localStorage.getItem(key) : Cookies.get(key)
        return JSON.parse(serializedValue)
    } catch (error) {
        return undefined
    }
}
/**
 * Clear key in sessionStorage if available
 * else clear in session Cookie
 * @param { string } key
 * @param { * } value
 */
const clearSessionValue = (key) => {
    try {
        if (sessionStorageAvailable) {
            sessionStorage.removeItem(key)
        }
        else {
            Cookies.remove(key);
        }
    } catch (error) {
        console.log("setSessionValue error: ", key)
    }
}
export const storage = {
    setSessionValue,
    getSessionValue,
    setValue,
    getValue,
    clearSessionValue
}
