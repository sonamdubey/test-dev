function extractPageNoFromURL() {
	var url = window.location.pathname;
	//var matches =  url.match(/page\/(\d+)\/$/);
	var regex = /\d+(?=\D*$)/;
	var matches = regex.exec(url);

	
	if(matches) {
  	return matches[0];
	}
	else{
    return 1; // no /page/no in url means first page
  } 
}

function mapNewsArticleDataToInitialData (article) {
	if(!article)
		return null;
	var initialData = {
            ArticleUrl : article.ArticleUrl?article.ArticleUrl:"",
            BasicId: article.BasicId?article.BasicId:"",
            Title : article.Title?article.Title:"",
            AuthorName : article.AuthorName?article.AuthorName:"",
            DisplayDateTime : article.DisplayDateTime?article.DisplayDateTime:"",
            ArticleApi : article.ArticleApi?article.ArticleApi:"",
            LargePicUrl :  article.LargePicUrl?article.LargePicUrl:"",
            HostUrl : article.HostUrl?article.HostUrl:"",
			AuthorMaskingName : article.AuthorMaskingName || ''
        }
       
	return initialData;

}

function extractPageCategoryFromURL() {
	var url = typeof(window) !== 'undefined' ? window.location.pathname : "";
	var regex = /\/m\/([A-Za-z0-9-_.~]+)\/{0,1}/
	var res = url.match(regex);
	if(res != null && res.length > 1 && res[1] == "news")
		return "news";
	else if(res != null && res.length > 1 && res[1] == "expert-reviews")
		return "expert-reviews";
	else 
		return "";
}

function isReactCategory(categoryName) {
	var allowedCategoriesInReact = ["NEWS", "EXPERT REVIEWS", "AutoExpo 2018"];
	if (allowedCategoriesInReact.indexOf(categoryName) !== -1) {
		return true;
	}
	return false;
}

module.exports = {
	extractPageNoFromURL,
	mapNewsArticleDataToInitialData,
	extractPageCategoryFromURL,
	isReactCategory
}