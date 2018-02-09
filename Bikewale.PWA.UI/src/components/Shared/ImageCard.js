import React from 'react'

import LazyLoad from 'react-lazy-load'

class ImageCard extends React.Component {
	constructor(props) {
		super(props)
	}

	render() {
		return (
			<div className="model-image__card">
				<ul className="image-grid__list" data-grid="7">
					<li className="image-grid-list__item">
						<LazyLoad>
							<img className="swiper-lazy" src="https://imgd.aeplcdn.com//310x174//bikewaleimg/ec/18879/img/m/Suzuki-Gixxer-SF-Exterior-52489_l.jpg" alt="Suzuki Gixxer SF Images" title="Suzuki Gixxer SF Images" />
						</LazyLoad>
					</li>
					<li className="image-grid-list__item">
						<LazyLoad>
							<img className="swiper-lazy" src="https://imgd.aeplcdn.com//310x174//bikewaleimg/ec/18879/img/l/Suzuki-Gixxer-SF-Exterior-52490.jpg" alt="Suzuki Gixxer SF Images" title="Suzuki Gixxer SF Images" />
						</LazyLoad>
					</li>
					<li className="image-grid-list__item">
						<LazyLoad>
							<img className="swiper-lazy" src="https://imgd.aeplcdn.com//310x174//bikewaleimg/ec/18879/img/l/Suzuki-Gixxer-SF-Exterior-52491.jpg" alt="Suzuki Gixxer SF Images" title="Suzuki Gixxer SF Images" />
						</LazyLoad>
					</li>
					<li className="image-grid-list__item">
						<LazyLoad>
							<img className="swiper-lazy" src="https://imgd.aeplcdn.com//310x174//bikewaleimg/ec/18879/img/l/Suzuki-Gixxer-SF-Exterior-52492.jpg" alt="Suzuki Gixxer SF Images" title="Suzuki Gixxer SF Images" />
						</LazyLoad>
					</li>
					<li className="image-grid-list__item">
						<LazyLoad>
							<img className="swiper-lazy" src="https://imgd.aeplcdn.com//310x174//bikewaleimg/ec/18879/img/l/Suzuki-Gixxer-SF-Exterior-52493.jpg" alt="Suzuki Gixxer SF Images" title="Suzuki Gixxer SF Images" />
						</LazyLoad>
					</li>
					<li className="image-grid-list__item">
						<LazyLoad>
							<img className="swiper-lazy" src="https://imgd.aeplcdn.com//310x174//bikewaleimg/ec/18879/img/l/Suzuki-Gixxer-SF-Exterior-52494.jpg" alt="Suzuki Gixxer SF Images" title="Suzuki Gixxer SF Images" />
						</LazyLoad>
					</li>
					<li className="image-grid-list__item">
						<LazyLoad>
							<img className="swiper-lazy" src="https://imgd.aeplcdn.com//310x174//bikewaleimg/ec/18879/img/l/Suzuki-Gixxer-SF-Exterior-52495.jpg" alt="Suzuki Gixxer SF Images" title="Suzuki Gixxer SF Images" />
						</LazyLoad>
					</li>
				</ul>
				<div className="card-image__details">
					<h3 className="card-details__left-col">
						<span className="card-details__make">Suzuki</span>
						<span className="card-details__model">Gixxer SF</span>
					</h3>
					<div className="card-details__right-col">
						<span className="card-details__image-count">75</span>
					</div>
				</div>
			</div>
		)
	}
}

module.exports = ImageCard
