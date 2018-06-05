import React from 'react'
import { StaticRouter, Switch ,  Route } from 'react-router-dom'
import Navigation from '../components/Shared/Navigation'
import NavigationDrawer from '../components/Shared/NavigationDrawer'
import AdUnit from '../components/AdUnit'
import { AD_DIV_REVIEWS_TOP_320_50, AD_PATH_REVIEWS_TOP_320_50, AD_DIMENSION_320_50} from '../utils/constants'
import ArticleListComponent from '../components/News/ArticleListComponent'
import ArticleDetailComponent from '../components/News/ArticleDetailComponent'
import VideoLandingComponent_Server from './Videos/VideoLandingComponent_Server'
import VideosByCategoryComponent from '../components/Videos/VideosByCategoryComponent'
import VideoDetailComponent from '../components/Videos/VideoDetailComponent'
import FinanceComponent from '../components/Finance/FinanceComponent'


class ServerAppComponent extends React.Component {

	render() {
		return (
				<div>
					<Navigation/>      
					<div className="body-content">
						<Switch>
	                        <Route exact path='/m/(news|expert-reviews)/' component={() => (<ArticleListComponent {...this.props.childComponentProps}/>)}/>
	                        <Route exact path='/m/(news|expert-reviews)/page/:pageNo/' component={() => (<ArticleListComponent {...this.props.childComponentProps}/>)}/>
	                        <Route exact path='/m/news/:basicId(\d+)-:title.html' component={() => (<ArticleDetailComponent {...this.props.childComponentProps}/>)}/>
	                        <Route exact path='/m/expert-reviews/:title-:basicId(\d+).html' component={() => (<ArticleDetailComponent {...this.props.childComponentProps}/>)}/>
                            <Route exact path='/m/bike-videos/' component={() => (<VideoLandingComponent_Server {...this.props.childComponentProps}/>)}/>
						    <Route exact path='/m/bike-videos/category/*-:categoryId(\d+)/' component={() => (<VideosByCategoryComponent {...this.props.childComponentProps}/>)}/>
						    <Route exact path='/m/bike-videos/:title-:basicId(\d+)/' component={() => (<VideoDetailComponent {...this.props.childComponentProps}/>)}/>
							<Route exact path='/m/bike-loan-emi-calculator/' component={() => (<FinanceComponent {...this.props.childComponentProps}/>)} />
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
