export const AreaCities = [1, 2, 10, 12];

export const CheckLocation = location => {
  const { cityId, areaId } = location;
  if (cityId > 0) {
    if (AreaCities.includes(parseInt(cityId))) {
      if (!areaId || areaId < 0) {
        return false;
      }
    }
    return true;
  }
  return false;
};
