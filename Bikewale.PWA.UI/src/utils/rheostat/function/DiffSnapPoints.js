export const createNewSnapPoints = (algoObj) => {
    const snapPoints = [];

    for(let i=algoObj.startPoint;i<=algoObj.endPoint;i += algoObj.difference) {
        snapPoints.push(i);
    }

    return snapPoints;
}


