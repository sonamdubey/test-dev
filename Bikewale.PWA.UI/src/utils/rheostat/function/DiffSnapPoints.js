export const createNewSnapPoints = (algoObj) => {
    const snapPoints = [];

    for(let i=algoObj.startPoint;i<=algoObj.endPoint;i += algoObj.difference) {
        snapPoints.push(i);
    }
    // if ((algoObj.endPoint-algoObj.startPoint)%algoObj.difference !== 0){
    //     snapPoints.push(endPoint);
    // }

    return snapPoints;
}


