//import snapPoints from '../constants/tenureSnapPoints'
// const snapPoints = [1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7]

export default {
	getPosition(snapPoints, value, min, max) {
        let percentOfEachInterval = 100/(snapPoints.length-1);
        // find index of snapPoints having value >= param value
        let index = snapPoints.findIndex( x => x >= value );

        // handle base cases
        if( index == 0 ) return 0 // if value <= min
        if( index < 0 || index == snapPoints.length ) return 100 // if value > max or value = snapPoints.length

        let valueExceedingLowerSnap = value - snapPoints[ index - 1 ]

        let snapInterval = snapPoints[ index ] - snapPoints[ index - 1 ]

        let percentWeightInInterval = ( valueExceedingLowerSnap / snapInterval ) * 100

        let pos = ( ( ( index - 1 ) * 100 ) / ( snapPoints.length - 1) ) + (percentWeightInInterval * percentOfEachInterval)/100

        return pos
	},

	getValue(snapPoints, pos, min, max) {
        let percentOfEachInterval = 100/(snapPoints.length-1);
        // handle base cases
		if (pos <= 0) {
			return min
		} else if (pos >= 100) {
			return max
        }

        let lowerSnapIndex = Math.floor( pos / percentOfEachInterval )

        let percentOfScaleExceedingLowerSnap = pos - ( lowerSnapIndex * percentOfEachInterval )

        let value = (
                    snapPoints[ lowerSnapIndex ]
                    +
                    ( percentOfScaleExceedingLowerSnap / percentOfEachInterval )
                    *
                    ( snapPoints[ lowerSnapIndex + 1 ] - snapPoints[ lowerSnapIndex ])
        )

        return ((value * 100) / 100).toFixed(1);
	},
}
