export const formatToCurrency = (value, {thousandDecimal = 2, lakhDecimal = 2, croreDecimal = 2, plusSign = true}) => {
	let amountValue

	if (value > 9999999) {
		amountValue = parseFloat((value / 10000000).toFixed(croreDecimal))
		amountValue += plusSign ? '+' : ''
		amountValue += ' Cr'
	}
	else if (value < 100000) {
		amountValue = parseFloat((value / 1000).toFixed(thousandDecimal)) + 'K'
	}
	else {
		amountValue = parseFloat((value / 100000).toFixed(lakhDecimal)) + ' lakh'
	}

	return amountValue
}

export function formatToINR(amount, isSymbol = true) {
	let inputPrice = amount.toString()
	let formattedPrice = ''
	let breakPoint = 3
	let currencySymbol = isSymbol ? '\u20b9 ' : ''

	for (let i = inputPrice.length - 1; i >= 0; i--) {
		formattedPrice = inputPrice.charAt(i) + formattedPrice

		if ((inputPrice.length - i) === breakPoint && inputPrice.length > breakPoint) {
			formattedPrice = ',' + formattedPrice
			breakPoint += 2
		}
	}

		return currencySymbol + formattedPrice
}

//test case for this function is not written as it depends on input<> and data-val
export function formatValueWithComma(inputField) {
	let fieldValue = inputField.value

	fieldValue = fieldValue.replace(/[^\d]/g, "").replace(/^0+/, "")
	inputField.setAttribute('data-value', fieldValue)

	return Number(fieldValue)
}

