import snapPoints from '../constants/budgetSnapPoints'

let totalSnapPoints = snapPoints.length
let percentOfEachInterval = 100/(totalSnapPoints-1)

export default {
	getPosition(value, min, max) {
        // find index of snapPoints having value >= param value
        let index = snapPoints.findIndex( x => x >= value )

        // handle base cases
        if( index == 0 ) return 0 // if value <= min
        if( index < 0 || index == totalSnapPoints ) return 100 // if value > max or value = totalSnapPoints

        let valueExceedingLowerSnap = value - snapPoints[ index - 1 ]

        let snapInterval = snapPoints[ index ] - snapPoints[ index - 1 ]

        let percentWeightInInterval = ( valueExceedingLowerSnap / snapInterval ) * 100

        let pos = ( ( ( index - 1 ) * 100 ) / ( totalSnapPoints - 1) ) + (percentWeightInInterval * percentOfEachInterval)/100

        return pos
	},

	getValue(pos, min, max) {
        // handle base cases
		if (pos <= 0) {
			return min
		} else if (pos >= 100) {
			return max
        }

        let lowerSnapIndex = Math.floor( pos / percentOfEachInterval )

        let percentOfScaleExceedingLowerSnap = pos - ( lowerSnapIndex * percentOfEachInterval )

        let value = Math.round(
                    snapPoints[ lowerSnapIndex ]
                    +
                    ( percentOfScaleExceedingLowerSnap / percentOfEachInterval )
                    *
                    ( snapPoints[ lowerSnapIndex + 1 ] - snapPoints[ lowerSnapIndex ])
                )

        return value
	}
}
