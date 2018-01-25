import React from 'react'
import LazyLoad from 'react-lazy-load'

class UserReviewSlug extends React.Component {
	constructor(props) {
		super(props);
		this.state = {
			visible : true
		};
		this.closeUserReviewSlug = this.closeUserReviewSlug.bind(this);
	}
	closeUserReviewSlug() {
		bwcache.set('showeditcmsreviewslug', '1', true);
		this.setState({
			visible : false
		});
	}
	render() {

		return (
			<li className={"list-item__slug "+ (this.state.visible ? '':'hide')} id="writereviewslug">
				<span className="bwmsprite list-slug__close" onClick={this.closeUserReviewSlug}></span>
				<div className="list-slug__wrapper">
					<div className="slug-content--top-align">
						<LazyLoad>
							<img className="list-slug__image" src="https://imgd.aeplcdn.com/0x0/bw/static/sprites/m/review-contest-slug-120.png" alt="Bike Review Contest" />
						</LazyLoad>
					</div>
					<div className="list-slug__details slug-content--top-align">
					 <p className="list-slug__heading">Bike review contest</p>
					 <p className="list-slug__subheading">Write a detailed review &amp; stand a chance to win Amazon vouchers worth <span>&#x20B9;</span>&nbsp;1,000</p>
					 <a href="/m/bike-review-contest/?csrc=13" title="Bike Review Contest" className="list-slug__target">Participate now<span className="bwmsprite target-arrow"></span></a>
				  </div>
				</div>
			</li>)

	}
}

module.exports = UserReviewSlug