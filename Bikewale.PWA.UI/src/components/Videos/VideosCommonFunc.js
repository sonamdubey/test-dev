var  formVideoUrl = function(urlSuffix,basicId) {
	return '/m/bike-videos/'+urlSuffix+'-'+basicId+'/';
}
var formVideoImageUrl = function(videoId,imageSuffix) {
	return 'https://img.youtube.com/vi/' + videoId + '/'+ imageSuffix;
	
}
var formMoreVideosUrl = function(urlSuffix) {
	return "/m/bike-videos/category" + urlSuffix;
}
var pushVideoDetailUrl = function(context,video) {
	var videoUrl = formVideoUrl(video.VideoTitleUrl,video.BasicId);
	context.props.history.push(videoUrl);
	var videoInitialData = mapVideoDataToInitialData(video);
	context.props.fetchVideoDetail(videoInitialData); // TODO call other apis
	context.props.fetchModelSlug(videoInitialData.BasicId)
	

}
var getCategoryIdFromUrl = function(url) {
	var regex = /\/m\/bike-videos\/category\/.*-(\d+)\//;
	var matches = regex.exec(url);
	if(matches) {
		return matches[1];
	}
	return -1;

}

var pushVideosByCategoryUrl = function(context,sectionTitle,moreVideosUrl) {
	var url = formMoreVideosUrl(moreVideosUrl);
	context.props.history.push(url);
	var categoryId  = context.props.match.params.categoryId ? context.props.match.params.categoryId : getCategoryIdFromUrl(url);
	context.props.fetchVideosByCategory(categoryId,sectionTitle);


}
var mapVideoDataToInitialData = function(video) {
	if(!video) {
		return null;
	}
	return {
		VideoTitle : video.VideoTitle,
		VideoUrl : video.VideoUrl,
		BasicId : video.BasicId,
		VideoTitleUrl : video.VideoTitleUrl,
		DisplayDate : video.DisplayDate

	}
}



module.exports = {
	formVideoUrl,
	formVideoImageUrl,
	formMoreVideosUrl,
	pushVideoDetailUrl,
	pushVideosByCategoryUrl,
	mapVideoDataToInitialData
}