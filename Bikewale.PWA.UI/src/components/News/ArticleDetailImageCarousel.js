import React from 'react'

import Slider from 'react-slick'
import Gallery from '../Shared/Gallery'

class ArticleDetailImageCarousel extends React.Component {
	constructor(props) {
		super(props)

		this.state = {
			images: [
				{
					hostUrl: 'https://imgd.aeplcdn.com/',
					originalImagePath: '/bw/ec/32131/Kawasaki-Ninja-400-Action-113949.jpg',
					title: 'Kawasaki Ninja 400 Action'
				},
				{
					hostUrl: 'https://imgd.aeplcdn.com/',
					originalImagePath: '/bw/ec/32131/Kawasaki-Ninja-400-Action-113950.jpg',
					title: 'Kawasaki Ninja 400 Action New'
				},
				{
					hostUrl: 'https://imgd.aeplcdn.com/',
					originalImagePath: '/bw/ec/27155/Aprilia-SR-150-Race-First-Ride-Review-90928.jpg',
					title: 'Aprilia SR 150 Race 1'
				},
				{
					hostUrl: 'https://imgd.aeplcdn.com/',
					originalImagePath: '/bw/ec/27155/Aprilia-SR-150-Race-First-Ride-Review-90943.jpg',
					title: 'Aprilia SR 150 Race 2'
				},
				{
					hostUrl: 'https://imgd.aeplcdn.com/',
					originalImagePath: '/bw/ec/27155/Aprilia-SR-150-Race-First-Ride-Review-90933.jpg',
					title: 'Aprilia SR 150 Race 3'
				}
			],
			isGalleryActive: false,
			slideIndex: 0
		}

		this.handleThumbnailCarouselBeforeChange = this.handleThumbnailCarouselBeforeChange.bind(this);
	}

	getImageSlides() {
		let list = this.state.images.map(function (item) {
			return (
				<div style={{ width: 90 }}>
					<img src={item.hostUrl + '110x61' + item.originalImagePath} />
				</div>
			)
		})

		return list
	}

	handleThumbnailCarouselBeforeChange(oldindex, newindex) {
		if (event.currentTarget.classList.contains('slick-slide')) {
			const currentSlideIndex = newindex

			this.setState({
				isGalleryActive: true,
				slideIndex: currentSlideIndex
			})
		}
	}

	render() {
		const imageSliderSettings = {
			className: 'slider__thumbnail',
			focusOnSelect: true,
			infinite: false,
			slidesToShow: 1,
			slidesToScroll: 1,
			variableWidth: true,
			beforeChange: this.handleThumbnailCarouselBeforeChange
		};

		const gallery = {
			heading: '2018 Triumph Tiger 800 Launch Ride Review',
			images: this.state.images,
			isActive: this.state.isGalleryActive,
			slideIndex: this.state.slideIndex
		}

		return (
			<div className="article-image-content">
				<h3 className="article-image-heading">Images</h3>
				<div ref="imageSliderContainer" className="article-image-slider">
					<Slider ref="imageSlider" {...imageSliderSettings}>
						{this.getImageSlides()}
					</Slider>
				</div>
				{this.state.isGalleryActive ? <Gallery {...gallery} /> : ''}
			</div>
		)
	}
}

export default ArticleDetailImageCarousel
