import ncfDefaultFilters from '../../filterPlugin/constants/NCFdefaultfilters';
import React from 'react'
import { priceInLakhs, getMonthYearText } from '../../utils/Common'
import { getModelPageUrl } from '../../utils/UrlFactory';
// note: heading is appended to - CarWale and return as Title
// it is also use for og and twitter tags and schema
// dimension,budgetText are also use across meta tags change can impact all the seo data
export const getOgFormat = (property,content) => {
    if(property && content) {
        return (
            <meta property={property} key={property} content={content} />
        )
    }
    return null
}
export const getTwitterFormat = (property,content) => {
    if(property && content) {
        return (
            <meta name={property} key={property} content={content} />
        )
    }
    return null
}
export const getSocialTags = (heading,description,url,logoUrl) => {
    let socialTags = []
    socialTags.push(getOgFormat("og:title",heading))
    socialTags.push(getOgFormat("og:type","website"))
    socialTags.push(getOgFormat("og:description",description))
    socialTags.push(getOgFormat("og:url",url))
    socialTags.push(getOgFormat("og:site_name","CarWale"))
    socialTags.push(getOgFormat("og:image",logoUrl))
    socialTags.push(getTwitterFormat("twitter:card","Summary"))
    socialTags.push(getTwitterFormat("twitter:site","@CarWale"))
    socialTags.push(getTwitterFormat("twitter:title",heading))
    socialTags.push(getTwitterFormat("twitter:description",description))
    socialTags.push(getTwitterFormat("twitter:creator","@CarWale"))
    socialTags.push(getTwitterFormat("twitter:image",logoUrl))
    return socialTags
}
export const getTitle = (searchParams,dimension) => {
    let title =  getHeading(searchParams,dimension,false)+" - CarWale";
    return title.replace(/  +/g, ' ');
}

// note: firstCarDetails have name and price in object
export const getDescription = (searchParams,dimension,firstCarDetails) => {
    let priceLakhs = priceInLakhs(firstCarDetails.price)
    let description = "The best " + dimension + " cars in India include "+ firstCarDetails.name + " (\u20B9 " + priceLakhs
     + " Lakhs)." + " Check out the latest price in your city, offers, variants, specifications, pictures, mileage and reviews of the best "
    + dimension +" cars " + getBudgetText(searchParams) + ".";
    return description.replace(/  +/g, ' ');
}

export const getHeading = (searchParams,dimension,isH1) => {
    let heading = "Best "+ dimension +" cars "+ getBudgetText(searchParams) + (isH1 ? "" : " in India");
    return heading.replace(/  +/g, ' ');
}
export const getSchema = (modelListData,url,heading,dimension,budgetText) => {
    let schemaObject = {}
    schemaObject = createBasicProperties(heading,dimension,budgetText,url,schemaObject)
    schemaObject["itemListElement"] = getItemListElementsObject (modelListData)
    return JSON.stringify(schemaObject)
}

// todo: seating capacity
export const getDimensions = (searchParams) => {
    return getMakeName(searchParams) + " " +getTransmissionTypeName(searchParams)
                    +" " + getFuelTypeName(searchParams) + " " + getSeatingCapacity(searchParams) + " " + getBodyTypeName(searchParams);
}

export const getBudgetText = (searchParams) => {
    if(typeof(searchParams.budget) != "undefined"){
        return "under \u20B9 "+priceInLakhs(searchParams.budget)+" lakhs"
    }
    return ''
}

const getSeatingCapacity = (searchParams) =>{
    if (typeof (searchParams.seats) != "undefined") {
        let seatIds = searchParams.seats.split(',')
        if (seatIds.length == 1) {
            let selectedSeatId = parseInt(seatIds[0])
            if (!isNaN(selectedSeatId)) {
                let seats = ncfDefaultFilters().newCarFinder.seats.data
                let selected = ''
                seats.forEach(function (element) {
                    if (element["id"] == selectedSeatId) {
                        selected = element["name"]
                    }
                })
                return selected ? (selected + " Seater") : '';
            }
        }
    }
    return ''
}

const getMakeName = (searchParams) => {
    if(typeof(searchParams.carMakeIds) != "undefined"){
        let makeIds = searchParams.carMakeIds.split(',')
        if(makeIds.length == 1){
            let selectedMakeId = parseInt(makeIds[0])
            if(!isNaN(selectedMakeId)){
                let makes = ncfDefaultFilters().newCarFinder.make.data
                let selected = ''
                makes.forEach(function(element){
                    if(element["makeId"] == selectedMakeId){
                        selected = element["makeName"]
                    }
                })
                return selected ? selected : ''
            }
        }
    }
    return ''
}

const getBodyTypeName = (searchParams) => {
    if(typeof(searchParams.bodyStyleIds) != "undefined"){
        let bodyTypeIds = searchParams.bodyStyleIds.split(',')
        if(bodyTypeIds.length == 1){
            let selectedBodyTypeId = parseInt(bodyTypeIds[0])
            if(!isNaN(selectedBodyTypeId)){
                let bodyTypes = ncfDefaultFilters().newCarFinder.bodyType.data
                let selected = ''
                bodyTypes.forEach(function(element){
                    if(element["id"] == selectedBodyTypeId){
                        selected = element["name"]
                    }
                })
                return selected ? selected : ''
            }
        }
    }
    return ''
}

const getFuelTypeName = (searchParams) => {
    if(typeof(searchParams.fuelTypeIds) != "undefined"){
        let fuelTypeIds = searchParams.fuelTypeIds.split(',')
        if(fuelTypeIds.length == 1){
            let selectedFuelTypeId = parseInt(fuelTypeIds[0])
            if(!isNaN(selectedFuelTypeId)){
                let fuelTypes = ncfDefaultFilters().newCarFinder.fuelType.data
                let selected = ''
                fuelTypes.forEach(function(element){
                    if(element["id"] == selectedFuelTypeId){
                        selected = element["name"]
                    }
                })
                return selected ? selected : ''
            }
        }
    }
    return ''
}

const getTransmissionTypeName = (searchParams) => {
    if(typeof(searchParams.transmissionTypeIds) != "undefined"){
        let transmissionTypeIds = searchParams.transmissionTypeIds.split(',')
        if(transmissionTypeIds.length == 1){
            let selectedTransmissionTypeId = parseInt(transmissionTypeIds[0])
            if(!isNaN(selectedTransmissionTypeId)){
                let transmissionTypes = ncfDefaultFilters().newCarFinder.transmissionType.data
                let selected = ''
                transmissionTypes.forEach(function(element){
                    if(element["id"] == selectedTransmissionTypeId){
                        selected = element["name"]
                    }
                })
                return selected ? selected : ''
            }
        }
    }
    return ''
}

const createBasicProperties = (heading,dimension,budgetText,url,schemaObject) => {
    schemaObject['@context'] = "http://schema.org"
    schemaObject['@type'] = "ItemList"
    schemaObject['url'] = url
    schemaObject['name'] = (heading + " - " + getMonthYearText()).replace(/  +/g, ' ');
    schemaObject['description'] = getSchemaDesc(dimension,budgetText)
    return schemaObject
}

const getItemListElementsObject = (modelListData) => {
    let list = [], modelRank = 1
    modelListData.forEach(function (element){
        let listItem = {
            "@type": "ListItem",
            "position": modelRank,
            "url": "https://"+window.location.hostname+getModelPageUrl(element.makeMaskingName,element.modelMaskingName),
            "name": element.makeName + " " + element.modelName,
            "image": element.hostUrl+element.originalImagePath,
        }
        list.push(listItem)
        modelRank = modelRank + 1
    })
    return list
}

const getSchemaDesc = (dimension,budgetText) => {
    let desc = "CarWale brings the list of best " + dimension + " cars " + budgetText + " in India for " + getMonthYearText() +
    ". Explore the best " + dimension + " cars to buy the best car of your choice.";
    return desc.replace(/  +/g, ' ');
}
