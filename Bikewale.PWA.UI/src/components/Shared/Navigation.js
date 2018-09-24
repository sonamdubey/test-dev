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
                <div className="leftfloat">
                    <div id="navbar-btn" className="inline-block cur-pointer" onClick={this.openNavDrawer}>
                        <span className="bwmsprite nav-icon"></span>
                    </div>
                    <a href="/m/" id="bwheader-logo" className="header-link inline-block">
                        <img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAFQAAAAZCAYAAACvrQlzAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAA25pVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMDY3IDc5LjE1Nzc0NywgMjAxNS8wMy8zMC0yMzo0MDo0MiAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDoxZjc4MTg1Ni1kYTQ1LTAxNDktOWY5OC1hYjdhZjhiNmVlZWQiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6QjQ5RjQ3QkNGODI0MTFFNkJFQ0VFQzhFMUIxMzY4RUYiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6QjQ5RjQ3QkJGODI0MTFFNkJFQ0VFQzhFMUIxMzY4RUYiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MTBGQTZCMDdGNDQxMTFFNjhDNkRCNkRCNjRGQUFEMDkiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MTBGQTZCMDhGNDQxMTFFNjhDNkRCNkRCNjRGQUFEMDkiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz5AWURMAAAFeklEQVR42uxZX2ibVRRv02R2TDb/FP9Nx5CxlSrE6UDy4KaT4Z+nMFSMQ6kMXH2Y0ilUEAK6lw5FqQ/a6EvJkz5EQZBFFBumsmwP2QppabsNRnX+i38YpIhNmvg75Xfj8fh939KiXYu5cDjf991z7z333HN+59ykrW2JrV6v7wFlQL1tq7xhDzlQelkXrVQqm2nEflCMxpRW4Pc+UALUXRwfX7eKjBnnPqaq8/MblmvRKA1YpAFToHL9r5YGZUElnnZ8JRirVqttpG5pefbZW9o5xrIZVBYSo4qXqnDXLam8OOqn/GXwvqTSMeuzrynXf1kji8qKVw4t28kuXseMMmiGUBU3Bi2yf+i/DO+YoyBjeZ3oXGXuuhVizKiCpSETVXs8jJ78N9YNq4kTYAdAO0DOULMdodA0+j5ob28/YhQW+ZfBv0bfAX6TU74HdGeTm5Y55MAu+ohcAeoE5bDGJA3xCOhtvI9dYvoj3McJyD4vMAQ+y29dl9JNHCMSjjzM1z+C9KtUKx+uiaz5yZZAugmuzJhvKWOMIX4v2jl8jNfNhDXA9756862fY6YsHgpuW8wO8EaXgBLqW9bLQ5VsM62xRoj8fvJx0B04yW2gTbD89Xh/i33PCASoNX8n/5ncbWraqzQB+xK0C3QTPz+uRKZJ3yhy88pz3kzZ6eYNh8NF6HpaQlz1HyZ/F32fMfx1/wYFWZ0+UHavi1If3WbZf6parZ6xG05ZYBaXpyKD6iSiaoz7nlOY1fBYp5jy5IaXE59LLhoCKouY9j6lZ5rvBTV33NSVM1y/38OjBtUaeeuhcllR+JsJKMn+kWPCRu4JCN0Mfi3oRnqdw9Nng3AL3vw9MEdObr0LcbAR0N0UOYTxb/LQ9qt5NzKEZ9V08vwF5EfMMsclUkCbaMBbVN+P5C6inrutp+dR8Dc81F2IksnJySpkruS3hQPA+ycmj+w0RnV6vgP9jvsmJTYB670+NjsXBOKhUIcA96+grTRQDzcsIbLfhZ5s1GO4yK41epz1kDtFvoOG08lFcHMz1/x0vlYbRUJ1cxwibAiEvQe6XY1bT34BxtxCWLJtp3kXXY/ygD0Thgsld9PJMwGUTKjkXNjbkDc1Xd0rXJiYkqSyV8iLlwQV2NTBtTIhxeoZVUkvb8aLjmVX3qmxCcJc0sBcZjH62UxX8DBAHzOeM0CJxhswGBoz19E65417KWCyaMokjSBdNSZnqEvB3ohUfZlSe0nZfKB0HjDrFNShJd0tsdk69C7y7TSQYOUostfpSCQyjOdhfN9KPOyq12pXI8bndMZlWFjDPSCEUDrBeUeknpQOhORBhKSE3nbi4j7IfAUuHnse9AMriasosw369JmsfDLc0XER4+bUt1Hyb8n3ol9C9CEDES/AI18kTIneM0b3p0Gfc8wrqCYOYp5jeD6j9JN2A/Vbi7095RVGdRNSJVOTZo2nFIzHlRhyvR61rIOUXgUTAx5yXs15o75OJgz8lF1pxyxsISitxhc5n4zN+URRN6Os3IR+gw0Pxcs+WFcy725m92sIvOuU10ktloVnufLiFwcrpi49ympgDAp/BC+8D89P0oO7CPq7sOZr4K9zzmHIybqPgbZwTcmmv4G+Ax1zGRXjXuU6Y+MTEx8vFM8TE4cRBSI74+RCodAFeOBu3nYkGRUkMfIGJN77vng3+Et+4ctoepCHJPrfqrzc6Xeee54M+mWp29zpo/YEKRd3dSLlcqb4t7+nDii8rpsfLhIstVptCT9cxFhwe0FNlmEYbVlqkY03GVfieBk3T6yOraZ/AVaS50ZZB+Y9jFukcRMtAy8dFpI+mNvI+i1LLc247s/Bv3lus8V2qwX/t+WuramWRf4H7U8BBgBqDdX3KzD5KwAAAABJRU5ErkJggg==" alt="BikeWale" />
                    </a>
                </div>
                <div className="rightfloat">
                    <div className="global-search cur-pointer" id="global-search" onClick={this.openGlobalSearchPopUp.bind(this)}>
                        <span className="bwmsprite search-bold-icon"></span>
                    </div>
                    <div className="global-location cur-pointer" onClick={this.openGlobalCityPopUp.bind(this)}>
                        <span className="bwmsprite map-loc-icon"></span>
                    </div>
                </div>
                <div className="clear"></div>
            </div>
        );
    }
}

export default Navigation


