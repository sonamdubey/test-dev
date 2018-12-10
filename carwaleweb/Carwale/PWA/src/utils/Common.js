export function formatToINR(amount) {
	let inputPrice = amount.toString()
	let formattedPrice = ''
	let breakPoint = 3

	for (let i = inputPrice.length - 1; i >= 0; i--) {
		formattedPrice = inputPrice.charAt(i) + formattedPrice

		if ((inputPrice.length - i) === breakPoint && inputPrice.length > breakPoint) {
			formattedPrice = ',' + formattedPrice
			breakPoint += 2
		}
	}

	return formattedPrice
}

//test case for this function is not written as it depends on input<> and data-val
export function formatValueWithComma(inputField) {
	let fieldValue = inputField.value

	fieldValue = fieldValue.replace(/[^\d]/g, "").replace(/^0+/, "")
	inputField.setAttribute('data-value', fieldValue)

	return Number(fieldValue)
}
/**
 * function expects string or number
 * @param {string|number} price
 * @returns return price round of to 2 digits
 * Rs 120410 to 1.20 and
 * 120410 to 1.20
 */
export function priceInLakhs(price) {
	if(typeof(price) === "string"){
		return Math.round(removeComma(price) / 1000 ) / 100
	}
	if(typeof(price) === "number"){
		return Math.round(price / 1000 ) / 100
	}
	console.warn("TypeErro: expected number or string but found " + typeof(price))
}
export function removeComma(value) {
	return Number(value.replace(/[^\d]/g, "").replace(/^0+/, ""))
}
/**
 * @returns the current month and year example: "April 2018"
 */
export const getMonthYearText = () => {
	let months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    let date = new Date();
    return months[date.getMonth()] + ' ' + date.getFullYear()
}
export function handleCommaDelete(event) {
	let inputField = event.currentTarget
	let fieldValue = inputField.value
	let eventKeyCode = event.keyCode

	if (eventKeyCode === 8 || eventKeyCode === 229) { //backspace
		if (fieldValue[inputField.selectionEnd - 1] === ',') {
			inputField.selectionEnd -= 1
		}
	}
	else if (eventKeyCode === 46) {	//delete
		if (fieldValue[inputField.selectionEnd] === ',') {
			inputField.selectionStart += 1
		}
	}
}

export function handleCursorPosition(newAmount, oldAmount, currentPosition) {
	let newAmountLength = newAmount.toString().length
	let oldAmountLength = oldAmount.toString().length
	let breakPoint

	if (newAmountLength < oldAmountLength) {
		breakPoint = 3

		for (let i = breakPoint; i <= newAmountLength; i += 2) {
			if (i === newAmountLength) {
				currentPosition -= 1
				break
			}
		}
	}
	else if (newAmountLength > oldAmountLength) {
		breakPoint = 4

		for (let i = breakPoint; i <= newAmountLength; i += 2) {
			if (i === newAmountLength) {
				currentPosition += 1
				break
			}
		}
	}

	return currentPosition
}

export const serialzeObjectToQueryString = (data) => {
	return Object.keys(data)
		.reduce((acc, key) => acc.concat([encodeURIComponent(key) + "=" + encodeURIComponent(data[key].toString())]), [])
		.join('&')
}

export const deserialzeQueryStringToObject = (queryString) => {
	if (queryString.indexOf('?') === 0) {
		queryString = queryString.slice(1)
	}
	let pairs = queryString.split('&')
	if (pairs.length > 0) {
		const queryObject = {}
		for (let i = 0; i < pairs.length; i++) {
			let pair = pairs[i]
			let index = pair.indexOf('=')
			if (index > -1) {
				queryObject[decodeURIComponent(pair.slice(0, index))] = decodeURIComponent(pair.slice(index + 1))
			}
		}
		return queryObject
	}
	return {}
}

/**
 * Function to calculate width of container based on its childrens
 *
 * @param: container
 */
export const setContainerWidth = (container) => {
	let elements = container.children
	let width = 0

	for (let i = 0; i < elements.length; i++) {
		width += elements[i].getBoundingClientRect().width
	}

	container.style.width = `${width}px`
}

//https://stackoverflow.com/questions/7837456/how-to-compare-arrays-in-javascript
// Warn if overriding existing method
if (Array.prototype.equals)
	console.warn("Overriding existing Array.prototype.equals. Possible causes: New API defines the method, there's a framework conflict or you've got double inclusions in your code.")
// attach the .equals method to Array's prototype to call it on any array
Array.prototype.equals = function (array) {
	// if the other array is a falsy value, return
	if (!array)
		return false

	// compare lengths - can save a lot of time
	if (this.length != array.length)
		return false

	for (let i = 0, l = this.length; i < l; i++) {
		// Check if we have nested arrays
		if (this[i] instanceof Array && array[i] instanceof Array) {
			// recurse into the nested arrays
			if (!this[i].equals(array[i]))
				return false
		}
		else if (this[i] != array[i]) {
			// Warning - two different object instances will never be equal: {x:20} != {x:20}
			return false
		}
	}
	return true;
}

export const isDesktop = window.innerWidth > 768 ? true : false;
export const trackingCategory = isDesktop ? "Desktop_EMICalculatorPage" : "MSite_EMICalculatorPage";

// Hide method from for-in loops
Object.defineProperty(Array.prototype, "equals", { enumerable: false })
