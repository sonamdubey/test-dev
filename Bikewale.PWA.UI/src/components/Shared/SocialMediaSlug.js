import React from 'react' 


class SocialMediaSlug extends React.Component {
	propTypes : {
		shareUrl : React.PropTypes.string
	};
    shareLink(platform) {
        var cururl = window.location.href;
        var url;
        switch(platform) {
            case "facebook" :
                url = 'https://www.facebook.com/sharer/sharer.php?u=';
                break;
            case "twitter":
                url = 'https://twitter.com/home?status=';
                break;
            case "googleplus":
                url = 'https://plus.google.com/share?url=';
                break;
            case "whatsapp" :
                var text = document.getElementsByTagName("title")[0].innerHTML;
                var message = encodeURIComponent(text) + " - " + encodeURIComponent(cururl);
                var whatsapp_url = "whatsapp://send?text=" + message;
                url = whatsapp_url;
                window.open(url, '_blank');
                return;
        }
        url += cururl;
        window.open(url,'_blank');
    }
	render() {
      
		return (
			<div className="social-media-divider">
			<p className="margin-bottom10 font14 text-light-grey">Share this story</p>
                    <ul className="social-wrapper">
                        <li className="whatsapp-container rounded-corner2 share-btn" onClick={this.shareLink.bind(this,"whatsapp")}>
                            <span className="social-icons-sprite whatsapp-icon"></span>
                        </li>
                        <li className="fb-container rounded-corner2" onClick={this.shareLink.bind(this,"facebook")}>
                            <span className="social-icons-sprite fb-icon"></span>
                        </li>
                        <li className="tweet-container rounded-corner2" onClick={this.shareLink.bind(this,"twitter")}>
                            <span className="social-icons-sprite tweet-icon"></span>
                        </li>
                        <li className="gplus-container rounded-corner2" onClick={this.shareLink.bind(this,"googleplus")}>
                            <span className="social-icons-sprite gplus-icon"></span>
                        </li>
                    </ul>
                    <div className="clear"></div>
            </div>
		)
	}
}


module.exports = SocialMediaSlug;