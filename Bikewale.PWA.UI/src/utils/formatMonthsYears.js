
function monthsToYears(months) {
	let yeartext = ""
	let monthtext = ""
	let yearCal = Math.floor(months/12)    
	let monthCal =  (months%12)
	if(monthCal < 2){
		monthtext = "month"
	}
	else{
		monthtext = "months"
	}
	if(months > 11 && months < 24 ){
		yeartext = "Year"
	}
	else{
		yeartext = "Years"
	}
	if(months > 11){
		return yearCal + " " + yeartext + " and " + monthCal + " " + monthtext
	}
	else{
		return monthCal + " " + monthtext
	}
}

module.exports = {
    monthsToYears
}