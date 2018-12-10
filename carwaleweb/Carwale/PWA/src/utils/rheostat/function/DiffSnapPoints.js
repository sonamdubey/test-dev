export const createNewSnapPoints = (algoObj) => {
    const snapPoints = [];

    for(let i=algoObj.startPoint;i<=algoObj.endPoint;i += algoObj.difference) {
        snapPoints.push(i);
    }
    if ((algoObj.endPoint-algoObj.startPoint)%algoObj.difference !== 0){
        snapPoints.push(endPoint);
    }

    return snapPoints;
}

export function getSnapPointsWithInterval(sliderInterval, base = 100000) {
  let snapPoints = [];

  for (let i = 0; i < sliderInterval.length; i++) {
    let interval = sliderInterval[i];

    for (let j = interval.start; j <= interval.end; j += interval.step * base) {
      if (snapPoints.indexOf(j) === -1) {
        snapPoints.push(j);
      }
    }
  }

  return snapPoints;
}
