import React from 'react'
import { Link } from 'react-router-dom'
import {triggerGA , GetCatForNav} from '../../utils/analyticsUtils'

class Navigation extends React.Component {

    openNavDrawer() {
        document.body.className = 'nav-drawer-active';

        var categ = GetCatForNav()
        if (categ != null) {
            triggerGA(categ, 'Hamburger_Menu_Icon', 'Icon_Click')
        }
    }
    lockPopup() {
        document.body.classList.add('lock-browser-scroll');
        document.getElementsByClassName('blackOut-window')[0].style.display = 'block';

    }
    openGlobalSearchPopUp() {
        try{
            document.getElementById('global-search-popup').style.display = 'block';
            document.getElementById('globalSearch').focus();
            this.lockPopup();

          }
          catch(err) {}
          
    }
    openGlobalCityPopUp() {
        try {
            document.getElementById('globalcity-popup').style.display = 'block';
          
            this.lockPopup();  
              document.getElementById('error-icon').classList.add('hide');
              document.getElementById('bw-blackbg-tooltip').classList.add('hide');
              document.getElementById("globalCityPopUp").classList.remove('border-red');

            
            document.getElementById("loaderGlobalCity").style.display = 'none';

            window.location.hash = "globalCity";
          }
          catch(err) {}

    }

    render() {
   
        
        return (
            <div className="header-navbar">
				<span className="doodle__container">
					<span className="doodle__animation-block">
						<span className="doodle__hexagaon">
							<span className="doodle__left-cloud"></span>
							<span className="doodle__right-cloud"></span>
							<span className="doodle__flag"></span>
							<span className="doodle__star doodle__star-one"></span>
							<span className="doodle__star doodle__star-two"></span>
							<span className="doodle__star doodle__star-three"></span>
							<span className="doodle__star doodle__star-four"></span>
							<span className="doodle__star doodle__star-five"></span>
							<span className="doodle__firework firework-one"></span>
							<span className="doodle__firework firework-two"></span>
							<span className="doodle__firework firework-three"></span>
							<span className="doodle__lantern lantern-one"></span>
							<span className="doodle__lantern lantern-two"></span>
						</span>
					</span>
				</span>
                <div className="leftfloat">
                    <div id="navbar-btn" className="inline-block cur-pointer" onClick={this.openNavDrawer}>
                        <span className="nav-icon"></span>
                    </div>
                    <a href="/m/" title="BikeWale" id="bwheader-logo" className="header-link inline-block">
                      <svg xmlns="http://www.w3.org/2000/svg" width="84" height="24" viewBox="0 0 110 32"><title>BikeWale</title><path fill="#fff" d="M19.5 1.2A2.62 2.62 0 1 1 16.9 3.8 2.62 2.62 0 0 1 19.5 1.2z"/><path fill="#fff" d="M11.5 3.14l5.32-.99c-.25.38-.4.84-.46 1.33v.01l-4.87.34v-.69zm8-3.14c1.06 0 1.96.69 2.28 1.64v.01a3.15 3.15 0 0 0-4.56 0A2.42 2.42 0 0 1 19.5 0zm8.02 3.14l-5.34-1c.25.39.41.85.46 1.33v.02l4.88.34v-.69zM53.25 8.84c.55 0 .85.1.85.69 0 2.49-1.35 2.56-2.3 2.56h-9.2c-.02.44.02.85 0 1.29l7.07.3c1.24 0 .7 1.91-.06 1.91l-7.02.22c-1 2.72 6.84 4.76 13.03 3.3.82-.2 1.18-.1.51.86v.01a3.9 3.9 0 0 1-2.53 1.66c-4.64.94-13.08 1.36-14.96-5.16-.53.03-1.1.1-1.63.12-1.06.04-1.2.14-1.14-1.2.02-.92.54-1.71 1.32-2.12 1.42-.06 1.47.27 1.65-.97.22-1.55.6-3.18 1.93-3.45h12.46zm-45.2 8.5c-2.45-.45-4.45-.27-4.51.37s1.85 1.5 4.28 1.95c2.43.46 4.56.28 4.63-.36s-1.97-1.52-4.4-1.96zm-7.73.53c.24-1.97.02-4.81.23-6.61S2.38 8.2 4.38 7.98c1.72-.18 3.75-.31 5.43.11 1.93.49 3.8 2.07 3.8 3.72-.06 1-.55 1.86-1.29 2.42-.61 1.07-.13 1.08.82 1.66 2.06.7 3.36 1.8 3.36 3.08-.4 3.27-4.44 3.93-7.83 3.8a28.5 28.5 0 0 1-6.05-.67l.2.03c-1.55-.38-2.61-.46-2.8-2.17a4.12 4.12 0 0 1 .31-2.12v.03zm9.69-6.44c-5.79-2.86-8.24 1.48-5.7 2.2.42 0 8.12-.44 5.7-2.22zm7.97-2.32v7.4c0 1.18-.28 3.45-.03 5.23s2.68 1.1 3.03.41.13-3.32.12-5.37l-.04-7.68c0-.54-.1-.82-.43-1.03a5.5 5.5 0 0 0-.85-.06h-.14.01v3.51h-.22v-3.5c-.3 0-.62.02-.87.04a1.03 1.03 0 0 0-.59 1.05zm5.89.64c3.9-5.27 2.35 2.38 2.63 4.5.2.72.73.95 1.61.68l6.74-4.9c1.49-1.23 2.95.95 1.69 1.94l-6.25 4.84c-.45.35-.43.7.22 1.07a238.3 238.3 0 0 0 25.93 9.25l1.9.48c1.64.18 2.23.17 2.84.86.94 1.08.15 3.95-2.04 3.48-.48-.1-1.43-1.24-1.93-1.43-9.09-3.22-21.1-6.76-28.7-10.02-1.2-.34-1.47-.73-1.97.5-.18.45-.34.83-.44 1.17-.27.96-.7 1.43-1.13 1.58-.61.23-1.12-.52-1.6-1.3-.68-1.34.04-3.52.82-5.43 0-1.33.05-4.4-.53-5.6a1.78 1.78 0 0 1 .23-1.66zM106.52 8.84c.54 0 .85.1.85.69 0 2.49-1.36 2.56-2.3 2.56h-9.2c-.03.44.01.85 0 1.29l7.07.3c1.23 0 .7 1.91-.07 1.91l-7.01.22c-1.01 2.72 6.83 4.76 13.03 3.3.81-.2 1.17-.1.5.86v.01a3.9 3.9 0 0 1-2.52 1.66c-4.64.94-13.08 1.36-14.97-5.16-.53.03-1.1.1-1.62.12-1.07.04-1.2.14-1.15-1.2.02-.92.54-1.71 1.32-2.12 1.42-.06 1.47.27 1.65-.97.23-1.55.6-3.18 1.93-3.45h12.46zm-28.24 2.8c-.04-.5-.12-1.02-.18-1.53-.02-.53-.23-1-.56-1.36-1.4-1.99 1.4-3.34 2.6-2.23.74.71.4 2.05.47 2.98l.03.31c.3 3.05.76 6.06 1.3 8.49l.7 1.23c.95 1.74-1.86 3.23-2.73 1.91l-.41-.64c-.26-.4-.25-.4-.61-.17-5.03 3.17-9.99-.26-6.27-6.45 1.02-1.83 2.9-2.8 5.67-2.54zm.6 3.38c-1.07.02-2.36 0-3.43.03-1.19 1.66-1.61 3.28-.45 4.02 1.42-.1 2.86-.89 4.26-1.97l-.38-2.06zm-22.48-3.9c-.77-3.33 3.86-3.45 3.5-.32-.12.94-.27.74-.05 1.8s.44 1.98.64 3.05a15 15 0 0 1 1.9-4.48l-.04.06c.87-.89 3.72-1.24 3.63 1.04-.03.67-.15 1.17-.12 1.53.05.54.4 1.1 1.1 2.6 1.84-3.31 3.72-6.65 5.54-9.95.7-1.27.1-1.31.48-2.27.66-1.7 3.71-1.37 4.26.57.3 1.13-.37 2.03-1.4 2.48s-.6.35-1.18 1.35l-5 8.47c-.92 1.59-.62 1.32-.97 2.87-.66 2.94-4.37 2.64-4.37-1.43a6.16 6.16 0 0 0-.39-1.89l.01.05c-.63-1.35-1.43 1.69-1.85 2.38.5 2.62-1.59 3.72-3.67 2.66-1.54-1.31.3-2.92-.67-5.1-.45-1.03-.85-3.07-1.35-5.46zM84.68 7c-.06 4.13-.49 7.65-.5 11.79.1 2.97 2.79 3.04 3.34.15l.14-11.57c0-2.06.65-2.68.33-6.19-.55-1.6-1.7-1.5-2.38-.09-1.07 2.21-1.77 4.6-.93 5.9z"/></svg>
                    </a>
                </div>
                <div className="rightfloat">
                    <div className="global-search cur-pointer" id="global-search" onClick={this.openGlobalSearchPopUp.bind(this)}>
                        <span className="global-search-icon"></span>
                    </div>
                    <div className="global-location cur-pointer" onClick={this.openGlobalCityPopUp.bind(this)}>
                        <span className="map-loc-icon"></span>
                    </div>
                </div>
                <div className="clear"></div>
            </div>
        );
    }
}

export default Navigation


