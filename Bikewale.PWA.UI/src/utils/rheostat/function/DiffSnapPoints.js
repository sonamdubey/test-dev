export const createNewSnapPoints = (algoObj) => {
    const snapPoints = [];

    for (let i = algoObj.startPoint; i <= algoObj.endPoint; i += algoObj.difference) {
        snapPoints.push(i);
    }

    return snapPoints;
}
var snapDifferences = [100, 500, 1000, 2500, 5000, 10000, 15000, 20000, 25000, 50000, 100000];
function adaptiveDifference(low, high, divisions) {
    return snapDifferences[snapDifferences.findIndex(x => x >= (high - low) / divisions)];
}

export const createSnapPointsWithBoundaryValues = (algoObj) => {
    const snapPoints = [];
    const difference = adaptiveDifference(algoObj.startPoint, algoObj.endPoint, algoObj.divisions);
    snapPoints.push(algoObj.startPoint);
    let startPoint = Math.round(algoObj.startPoint / difference) * difference;
    if (startPoint < algoObj.startPoint) {
        startPoint += difference;
    }
    for (let i = startPoint; i < algoObj.endPoint; i += difference) {
        snapPoints.push(i);
    }
    snapPoints.push(algoObj.endPoint);
    return snapPoints;
}

