
function formatToCurrency(value) {
    let amountValue

	if (value > 9999999) {
		amountValue = parseFloat((value / 10000000).toFixed(2)) + '+ Cr'
	}
	else if (value <= 100000) {
		amountValue = parseFloat((value / 1000).toFixed(2)) + 'K'
	}
	else {
		amountValue = parseFloat((value / 100000).toFixed(2)) + ' lakh'
	}

	return amountValue
}

function formatToINR(amount, isSymbol = true) {
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

function formatValueWithComma(inputField) {
    let fieldValue = inputField.value

	fieldValue = fieldValue.replace(/[^\d]/g, "").replace(/^0+/, "")
	inputField.setAttribute('data-value', fieldValue)

	return Number(fieldValue)
}

function formatToRound(value) {
	return Math.round(value)
}

module.exports = {
    formatToCurrency,
	formatToINR,
	formatValueWithComma,
	formatToRound
}