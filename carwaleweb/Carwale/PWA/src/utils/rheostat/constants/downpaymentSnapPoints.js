export const downpaymentArray = [
	{
		step: 0.1,
		start: 10000,
		end: 1000000000
	},
]
const snapPoints = []
for (let i = 0; i < downpaymentArray.length; i++) {
    let downpayment = downpaymentArray[i]

    for (let j = downpayment.start; j <= downpayment.end; j += downpayment.step * 100000) {
        if (!(snapPoints.indexOf(j) > -1)) {
            snapPoints.push(j)
        }
    }
}
export default snapPoints
