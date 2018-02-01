function extractPageNoFromURL(url) {
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

module.exports = {
	extractPageNoFromURL,
	mapNewsArticleDataToInitialData
}