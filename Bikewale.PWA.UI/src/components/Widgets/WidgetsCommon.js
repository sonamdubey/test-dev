function moreImagesUrl() {
    return "/m/images/";
}

function mapPopularBikesToInitialData(bikesList) {
    try{
        if(!bikesList)
            return null;
        return bikesList.map(function(bikeData){
            return mapBikeToInitialData(bikeData);
        });
    }
    catch(e){
        console.log(e);
        return null;
    }
    
}

function mapBikeToInitialData(bikeData) {
    try{
        if(!bikeData)
            return null;
        var initialData = {
            ModelId : bikeData.ModelId,
            RecordCount : bikeData.RecordCount,
            ModelImagePageUrl : bikeData.ModelImagePageUrl,
            BikeName : bikeData.BikeName,
            ModelName : bikeData.ModelName,
            MakeName : bikeData.MakeName,
            ModelImages : bikeData.ModelImages.map(function(modelImage) { return (!modelImage.HostUrl || !modelImage.OriginalImgPath) ? 'https://imgd.aeplcdn.com/160x89/bikewaleimg/images/noimage.png?q=70':modelImage.HostUrl + '174x98' + modelImage.OriginalImgPath; })
        }
        return initialData;
    }
    catch(e){
        console.log(e);
        return null;
    }
    
}

module.exports = {
    mapPopularBikesToInitialData,
    mapBikeToInitialData,
    moreImagesUrl
}