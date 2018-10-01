import React from 'react'

class Footer extends React.Component {
    render() {

        return (
            <footer className="bwm-footer">
                <div className="text-center padding-bottom15 border-solid-bottom">
                    <div className="grid-4">
                        <a href="/m/" className="bw-footer-icon" title="BikeWale">BikeWale</a>
                    </div>
                    <div className="grid-4">
                        <a href="https://www.carwale.com/m/" target="_blank" rel="noopener" className="cw-footer-icon" title="CarWale">CarWale</a>
                        <p className="cw-logo-label">ask the experts</p>
                    </div>
                    <div className="grid-4">
                        <a href="https://m.cartrade.com/" target="_blank" rel="noopener" className="ct-footer-icon" title="CarTrade">CarTrade</a>
                    </div>
                    <div className="clear"></div>
                </div>
                <div className="text-center padding-top10 padding-bottom20 border-solid-bottom">
                    <ul className="footer-link-list">
                        <li><a href="/m/contactus.aspx" rel="nofollow">Contact Us</a></li>
                        <li><a href="/m/advertisewithus.aspx" rel="nofollow">Advertise with Us</a></li>
                    </ul>
                    <p>Download Mobile App</p>
                    <a href="https://play.google.com/store/apps/details?id=com.bikewale.app&referrer=utm_source%3DBikeWaleMobileWebsite%26utm_medium%3DFooter%26utm_campaign%3DBikeWale%2520MobileWebsite%2520Footer" target="_blank" className="google-play-logo" rel="noopener nofollow" title="BikeWale App on Google Play">BikeWale App on Google Play</a>
                    <br />
                    <a href="/?site=desktop" target="_blank">View Desktop Version</a>
                </div>
                <div className="font11 padding-top10">
                    <div className="grid-4">&copy; BikeWale India</div>
                    <div className="grid-8 text-right">
                        <a href="https://www.bikewale.com/visitoragreement.aspx" rel="nofollow">Visitor Agreement</a>&nbsp;&amp;&nbsp;
                        <a href="https://www.bikewale.com/privacypolicy.aspx" rel="nofollow">Privacy Policy</a>
                    </div>
                    <div className="clear"></div>
                </div>
            </footer>
        )
    }
}

export default Footer

