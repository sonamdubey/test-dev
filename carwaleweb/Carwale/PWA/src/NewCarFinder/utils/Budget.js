export const formatBudgetTooltipValue = (value) => {
	let budgetValue

	if (value > 9999999) {
		budgetValue = parseFloat((value / 10000000).toFixed(2)) + '+ Cr'
	}
	else if (value <= 200000) {
		budgetValue = 'Upto 2 lakh'
	}
	else {
		budgetValue = parseFloat((value / 100000).toFixed(2)) + ' lakh'
	}

	return budgetValue
}
