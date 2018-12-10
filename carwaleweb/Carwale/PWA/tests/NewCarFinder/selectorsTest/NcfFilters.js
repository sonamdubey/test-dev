import snapPoints from '../../../src/NewCarFinder/utils/rheostat/constants/budgetSnapPoints'

export const ncfFilters = {
    location: {
        cityId: -1,
        cityName: "",
        userConfirmed: false
    },
    newCarFinder: {
        budget: {
            displayName: "Budget",
            slider: {
                min: 200000,
                max: 10000000,
                values: [
                    1000001
                ],
                userChange: false,
                snapPoints
            },
            inputBox: {
                value: 1000001
            }
        },
        bodyType: {
            displayName: "Body Type",
            data: [
                {
                    id: 6,
                    name: 'SUV/MUV',
                    icon: 'https://imgd.aeplcdn.com//0x0/cw/body/svg/suv_clr.svg',
                    lineIcon: 'https://imgd.aeplcdn.com//0x0/cw/body/svg/suv.svg',
                    isSelected: true
                },
                {
                    id: 3,
                    name: 'Hatchback',
                    icon: 'https://imgd.aeplcdn.com//0x0/cw/body/svg/hatchback_clr.svg',
                    lineIcon: 'https://imgd.aeplcdn.com//0x0/cw/body/svg/hatchback.svg',
                    isSelected: false
                },
                {
                    id: 10,
                    name: 'Compact Sedan',
                    icon: 'https://imgd.aeplcdn.com//0x0/cw/body/svg/compactsedan_clr.svg',
                    lineIcon: 'https://imgd.aeplcdn.com//0x0/cw/body/svg/compactsedan.svg',
                    isSelected: true
                }
            ]
        },
        fuelType: {
            displayName: "Fuel Type",
            data: [
                {
                    id: 1,
                    name: 'Petrol',
                    icon: 'https://imgd.aeplcdn.com/0x0/cw/fuel/svg/petrol.svg',
                    isSelected: false
                },
                {
                    id: 2,
                    name: 'Diesel',
                    icon: 'https://imgd.aeplcdn.com/0x0/cw/fuel/svg/diesel.svg',
                    isSelected: true
                }
            ]
        }
    }
}

export const ncfExpectedFilters = {
    location:{
        cityId: 1,
        cityName:"",
        userConfirmed: false
    },
    newCarFinder: {
        budget: {
            displayName: "Budget",
            slider: {
                min: 200000,
                max: 10000000,
                values: [
                    800000
                ],
                userChange: false,
                snapPoints
            },
            inputBox: {
                value: 800000
            }
        },
        bodyType: {
            displayName: "Body Type",
            data: [
                {
                    id: 6,
                    name: 'SUV/MUV',
                    icon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/suv_clr.svg',
                    lineIcon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/suv.svg',
                    isSelected: true
                },
                {
                    id: 3,
                    name: 'Hatchback',
                    icon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/hatchback_clr.svg',
                    lineIcon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/hatchback.svg',
                    isSelected: false
                },
                {
                    id: 1,
                    name: 'Sedan',
                    icon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/sedan_clr.svg',
                    lineIcon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/sedan.svg',
                    isSelected: false
                },
                {
                    id: 5,
                    name: 'Convertible',
                    icon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/convertible_clr.svg',
                    lineIcon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/convertible.svg',
                    isSelected: false
                },
                {
                    id: 2,
                    name: 'Coupe',
                    icon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/coupe_clr.svg',
                    lineIcon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/coupe.svg',
                    isSelected: false
                },
                {
                    id: 10,
                    name: 'Compact Sedan',
                    icon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/compactsedan_clr.svg',
                    lineIcon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/compactsedan.svg',
                    isSelected: true
                },
                {
                    id: 4,
                    name: 'Minivan/Van',
                    icon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/van_clr.svg',
                    lineIcon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/van.svg',
                    isSelected: false
                },
                {
                    id: 8,
                    name: 'Station Wagon',
                    icon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/wagon_clr.svg',
                    lineIcon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/wagon.svg',
                    isSelected: false
                },
                {
                    id: 7,
                    name: 'Truck',
                    icon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/truck_clr.svg',
                    lineIcon: 'https://imgd.aeplcdn.com/0x0/cw/body/svg/truck.svg',
                    isSelected: false
                }
            ]
        },
        fuelType: {
            displayName: "Fuel Type",
            data: [
                {
                    id: 1,
                    name: 'Petrol',
                    icon: 'https://imgd.aeplcdn.com/0x0/cw/fuel/svg/petrol.svg',
                    isSelected: true
                },
                {
                    id: 2,
                    name: 'Diesel',
                    icon: 'https://imgd.aeplcdn.com/0x0/cw/fuel/svg/diesel.svg',
                    isSelected: false
                },
                {
                    id: 3,
                    name: 'CNG',
                    icon: 'https://imgd.aeplcdn.com/0x0/cw/fuel/svg/cng.svg',
                    isSelected: true
                },
                {
                    id: 5,
                    name: 'Electric',
                    icon: 'https://imgd.aeplcdn.com/0x0/cw/fuel/svg/electric.svg',
                    isSelected: false
                }
            ]
        },
        make: {
            displayName: "Make",
            data: [
                {
                    "makeId": 10,
                    "makeName": "Maruti Suzuki",
                    "isSelected": true
                },
                {
                    "makeId": 8,
                    "makeName": "Hyundai",
                    "isSelected": true
                },
                {
                    "makeId": 7,
                    "makeName": "Honda",
                    "isSelected": false
                },
                {
                    "makeId": 17,
                    "makeName": "Toyota",
                    "isSelected": false
                },
                {
                    "makeId": 16,
                    "makeName": "Tata",
                    "isSelected": false
                },
                {
                    "makeId": 9,
                    "makeName": "Mahindra",
                    "isSelected": false
                },
                {
                    "makeId": 5,
                    "makeName": "Ford",
                    "isSelected": false
                },
                {
                    "makeId": 1,
                    "makeName": "BMW",
                    "isSelected": false
                },
                {
                    "makeId": 15,
                    "makeName": "Skoda",
                    "isSelected": false
                },
                {
                    "makeId": 11,
                    "makeName": "Mercedes-Benz",
                    "isSelected": false
                },
                {
                    "makeId": 20,
                    "makeName": "Volkswagen",
                    "isSelected": false
                },
                {
                    "makeId": 18,
                    "makeName": "Audi",
                    "isSelected": false
                },
                {
                    "makeId": 4,
                    "makeName": "Fiat",
                    "isSelected": false
                },
                {
                    "makeId": 12,
                    "makeName": "Mitsubishi",
                    "isSelected": false
                },
                {
                    "makeId": 45,
                    "makeName": "Renault",
                    "isSelected": false
                },
                {
                    "makeId": 21,
                    "makeName": "Nissan",
                    "isSelected": false
                },
                {
                    "makeId": 23,
                    "makeName": "Land Rover",
                    "isSelected": false
                },
                {
                    "makeId": 44,
                    "makeName": "Jaguar",
                    "isSelected": false
                },
                {
                    "makeId": 19,
                    "makeName": "Porsche",
                    "isSelected": false
                },
                {
                    "makeId": 54,
                    "makeName": "Ssangyong",
                    "isSelected": false
                },
                {
                    "makeId": 37,
                    "makeName": "Volvo",
                    "isSelected": false
                },
                {
                    "makeId": 51,
                    "makeName": "Mini",
                    "isSelected": false
                },
                {
                    "makeId": 22,
                    "makeName": "Bentley",
                    "isSelected": false
                },
                {
                    "makeId": 34,
                    "makeName": "Lexus",
                    "isSelected": false
                },
                {
                    "makeId": 50,
                    "makeName": "Force Motors",
                    "isSelected": false
                },
                {
                    "makeId": 33,
                    "makeName": "Ferrari",
                    "isSelected": false
                },
                {
                    "makeId": 43,
                    "makeName": "Jeep",
                    "isSelected": false
                },
                {
                    "makeId": 49,
                    "makeName": "Aston Martin",
                    "isSelected": false
                },
                {
                    "makeId": 47,
                    "makeName": "Bugatti",
                    "isSelected": false
                },
                {
                    "makeId": 56,
                    "makeName": "Datsun",
                    "isSelected": false
                },
                {
                    "makeId": 61,
                    "makeName": "Isuzu",
                    "isSelected": false
                },
                {
                    "makeId": 30,
                    "makeName": "Lamborghini",
                    "isSelected": false
                },
                {
                    "makeId": 36,
                    "makeName": "Maserati",
                    "isSelected": false
                },
                {
                    "makeId": 25,
                    "makeName": "Rolls-Royce",
                    "isSelected": false
                },
                {
                    "makeId": 66,
                    "makeName": "DC",
                    "isSelected": false
                },
                {
                    "makeId": 70,
                    "makeName": "Kia",
                    "isSelected": false
                }
            ]
        }
    }
}
