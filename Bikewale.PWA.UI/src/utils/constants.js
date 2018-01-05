
var NewsArticlesPerPage = 10;

var apiStatus = {
	'Reset' : 1,
	'IsFetching' : 2,
	'Fetched' : 3,
	'Error' : 4
}

var AD_PATH_REVIEWS_TOP_320_50 = '/1017752/BikeWale_News_Top_320x50';
var AD_PATH_REVIEWS_BOTTOM_320_50 = '/1017752/BikeWale_News_Bottom_320x50';
var AD_PATH_REVIEWS_MIDDLE_320_50 = '/1017752/BikeWale_News_Middle_320x50';

var AD_DIV_REVIEWS_TOP_320_50 = 'div-gpt-ad-1395986297721-0';
var AD_DIV_REVIEWS_BOTTOM_320_50 = 'div-gpt-ad-1395986297721-1';
var AD_DIV_REVIEWS_MIDDLE_320_50 = 'div-gpt-ad-1395986297721-5';

module.exports = {
	NewsArticlesPerPage,
	Status : apiStatus,
	AD_PATH_REVIEWS_TOP_320_50,
	AD_PATH_REVIEWS_BOTTOM_320_50,
	AD_PATH_REVIEWS_MIDDLE_320_50,
	AD_DIV_REVIEWS_TOP_320_50,
	AD_DIV_REVIEWS_BOTTOM_320_50,
	AD_DIV_REVIEWS_MIDDLE_320_50
}