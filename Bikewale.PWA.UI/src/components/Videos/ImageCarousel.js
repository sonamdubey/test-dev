import React from 'react'

import ImageCard from '../Shared/ImageCard'

class ImageCarousel extends React.Component {
	constructor(props) {
		super(props)
	}

	renderImageCardList() {
		let list = [];

		for(let i = 0; i < 5; i++) {
			list.push(
				<div className="carousel-slide">
					<a href="" title="" className="block">
						<ImageCard />
					</a>
				</div>
			)
		}

		return list
	}

	render() {
		return (
			<section>
				<div className="container margin-bottom15">
					<h2 className="image-carousel__title">Images</h2>
					<div className="carousel-wrapper carousel__recently-added">
						{this.renderImageCardList()}
					</div>
				</div>
			</section>
		)
	}
}

module.exports = ImageCarousel
