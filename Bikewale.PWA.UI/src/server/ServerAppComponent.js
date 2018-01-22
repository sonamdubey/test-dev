import React from 'react'
import { StaticRouter, Switch ,  Route } from 'react-router-dom'
import Navigation from '../components/Shared/Navigation'
import NavigationDrawer from '../components/Shared/NavigationDrawer'
import AdUnit320x50 from '../components/AdUnit320x50'
import {AD_DIV_REVIEWS_TOP_320_50 , AD_PATH_REVIEWS_TOP_320_50} from '../utils/constants'
import ArticleListComponent from '../components/News/ArticleListComponent'
import ArticleDetailComponent from '../components/News/ArticleDetailComponent'
import VideoLandingComponent_Server from './Videos/VideoLandingComponent_Server'
import VideosByCategoryComponent from '../components/Videos/VideosByCategoryComponent'
import VideoDetailComponent from '../components/Videos/VideoDetailComponent'


class ServerAppComponent extends React.Component {

	render() {
		return (
				<div>
					<Navigation/>
					<AdUnit320x50 adSlot={AD_PATH_REVIEWS_TOP_320_50} adContainerId={AD_DIV_REVIEWS_TOP_320_50}/>        
					<div className="body-content">
						<Switch>
	                        <Route exact path='/m/news/' component={() => (<ArticleListComponent {...this.props.childComponentProps}/>)}/>
	                        <Route exact path='/m/news/page/:pageNo/' component={() => (<ArticleListComponent {...this.props.childComponentProps}/>)}/>
	                        <Route exact path='/m/news/:basicId(\d+)-:title.html' component={() => (<ArticleDetailComponent {...this.props.childComponentProps}/>)}/>
	                        <Route exact path='/m/bike-videos/' component={() => (<VideoLandingComponent_Server {...this.props.childComponentProps}/>)}/>
						    <Route exact path='/m/bike-videos/category/*-:categoryId(\d+)/' component={() => (<VideosByCategoryComponent {...this.props.childComponentProps}/>)}/>
						    <Route exact path='/m/bike-videos/:title-:basicId(\d+)/' component={() => (<VideoDetailComponent {...this.props.childComponentProps}/>)}/>
	                    </Switch>
					</div>
					<NavigationDrawer/>
					<div id="peripheralComponents">
						
					</div>
					<div className="blackOut-window"></div>
				</div>
				)
	}
}
			
module.exports = ServerAppComponent
