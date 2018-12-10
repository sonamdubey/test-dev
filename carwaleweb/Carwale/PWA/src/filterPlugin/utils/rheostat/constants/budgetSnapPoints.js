const budgetArray = [
	{
		step: 0.5,
		start: 200000,
		end: 1300000
	},
	{
		step: 1,
		start: 1300000,
		end: 2800000
	},
	{
		step: 2,
		start: 2800000,
		end: 3400000
	},
	{
		step: 5,
		start: 3500000,
		end: 8000000
	},
	{
		step: 10,
		start: 8000000,
		end: 10000000
	}
]

const snapPoints = []

for (let i = 0; i < budgetArray.length; i++) {
    let budget = budgetArray[i]

    for (let j = budget.start; j <= budget.end; j += budget.step * 100000) {
        if (!(snapPoints.indexOf(j) > -1)) {
            snapPoints.push(j)
        }
    }
}

export default snapPoints
