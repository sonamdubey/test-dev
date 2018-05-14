
function monthsToYears(months) {
	let yearText = ""
	let monthText = ""
	let yearCal = Math.floor(months/12)    
	let monthCal =  (months%12)
	if(monthCal < 2){
		monthText = "month"
	}
	else{
		monthText = "months"
	}
	if(months > 11 && months < 24 ){
		yearText = "Year"
	}
	else{
		yearText = "Years"
	}
	if(months > 11){
		return yearCal + " " + yearText + " and " + monthCal + " " + monthText
	}
	else{
		return monthCal + " " + monthText
	}
}

module.exports = {
    monthsToYears
}