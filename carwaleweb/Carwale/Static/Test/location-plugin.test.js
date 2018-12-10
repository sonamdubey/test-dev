global.ac_Source = {
    allCarCities: "6"
}
global.askingAreaCityId = [1,2,10,12];

global.mockData = {
    "LocationWithCityArea":{cityId : 1, areaId : 1001},
    "LoactionHavingOnlyCity" : {cityId : 10}, 
    "LoactionHavingOnlyArea" : {areaId : 10005}};

require("../Src/jquery-1.7.2.min.js");
require("../Src/autocomplete.js")
const LocationSearch = require('../js/location-plugin');


describe('city validation', () => {
    var location;
    beforeEach(() => {location = new LocationSearch()});

    it('validate city location',() => {
        location.setLocation(mockData.LocationWithCityArea);
        expect(location.validateLocation()).toBe(true);
    });

    it('validate city location',() => {
        location.setLocation(mockData.LoactionHavingOnlyCity);
        expect(location.validateLocation()).toBe(false);
    });

    it('validate city location',() => {
        location.setLocation();
    expect(location.validateLocation()).toBe(false);
    })

    it('validate city location',() => {
        location.setLocation(mockData.LoactionHavingOnlyArea);
    expect(location.validateLocation()).toBe(false);
    })

})

describe('city validation with isAreaOptional flag', () => {
        var location;
    beforeEach(() => {location = new LocationSearch('',{isAreaOptional : true})});

    it('validate city location when area is optional',() => {
        location.setLocation(mockData.LocationWithCityArea);
    expect(location.validateLocation()).toBe(true);
})
    it('validate city location when area is optional',() => {
        location.setLocation(mockData.LoactionHavingOnlyCity);
    expect(location.validateLocation()).toBe(true);
})
    it('validate city location when area is optional',() => {
    location.setLocation();
    expect(location.validateLocation()).toBe(false);
})

    it('validate city location when area is optional',() => {
        location.setLocation(mockData.LoactionHavingOnlyArea);
    expect(location.validateLocation()).toBe(false);
    })
})