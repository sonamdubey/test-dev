export const getCityId = store => {
    if(store.location)
    {
        return {
            cityId:store.location.cityId
        }
    }
}

export const getCityName = store => {
    if(store.location)
    {
        return {
            cityName:store.location.cityName
        }
    }
}
