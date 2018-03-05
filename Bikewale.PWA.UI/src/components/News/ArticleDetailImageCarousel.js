import React from 'react'

import Slider from 'react-slick'
import Gallery from '../Shared/Gallery'
import {createImageUrl} from '../Widgets/WidgetsCommon'

class ArticleDetailImageCarousel extends React.Component {
	constructor(props) {
		super(props)

		this.state = {
		    images: this.props.imageGallery? this.props.imageGallery.ModelImages:[],
		    totalCount: this.props.imageGallery? this.props.imageGallery.RecordCount:0,
            title: this.props.title,
			isGalleryActive: false,
			slideIndex: 0
		}

		this.handleThumbnailCarouselBeforeChange = this.handleThumbnailCarouselBeforeChange.bind(this);
	}

	componentDidMount() {
		let imageSlider = this.refs.imageSlider.innerSlider.base
		let imageSliderSlides = imageSlider.querySelectorAll('.slick-slide')

		for (let i = 0; i < imageSliderSlides.length; i++) {
			let self = this;
			imageSliderSlides[i].addEventListener('click', function () {
				self.focusSlider(i);
			});
		}
	}

	getImageSlides() {
		let list = this.state.images.map(function (image) {
			return (
				<div style={{ width: 90 }}>
					<img title={image.ImageName} alt={image.ImageName} src={createImageUrl(image.HostUrl, image.OriginalImgPath, '110x61')} />
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

	focusSlider(slideIndex) {
		this.handleThumbnailCarouselBeforeChange(0, slideIndex);
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
			heading: this.state.title,
			images: this.state.images,
            totalCount: this.state.totalCount,
			isActive: this.state.isGalleryActive,
			slideIndex: this.state.slideIndex
		}
		if (gallery.totalCount == 0) {
		    return false;
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
