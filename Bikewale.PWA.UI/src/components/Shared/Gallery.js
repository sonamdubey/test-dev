import React from 'react'

import Slider from 'react-slick'
import {createImageUrl} from '../Widgets/WidgetsCommon'

const defaultProps = {
	heading: '',
	isActive: false,
	slideIndex: -1
}

class Gallery extends React.Component {
	constructor(props) {
		super(props)

		this.state = {
			heading: this.props.heading,
			isActive: this.props.isActive,
			slideIndex: this.props.slideIndex
		}

		this.handleGalleryClose = this.handleGalleryClose.bind(this);
		this.handleMainSliderBeforeChange = this.handleMainSliderBeforeChange.bind(this);
		this.handleThumbnailSliderBeforeChange = this.handleThumbnailSliderBeforeChange.bind(this);
	}

	componentWillReceiveProps(nextProps) {
		this.refs.mainSlider.slickGoTo(nextProps.slideIndex);

		this.setState({
			isActive: nextProps.isActive,
			slideIndex: nextProps.slideIndex
		})
	}

	handleGalleryClose() {
		this.setState({
			isActive: false
		})
	}

	getMainCarouselSlides() {
		let list = this.props.images.map(function (image) {
			return (
				<div>
					<img title={image.ImageName} alt={image.ImageName} src={createImageUrl(image.HostUrl, image.OriginalImgPath, '476x268')} />
				</div>
			)
		})

		return list
	}

	getThumbnailSlides() {
		let list = this.props.images.map(function (image) {
			return (
				<div style={{ width: 90 }}>
					<img title={image.ImageName} alt={image.ImageName} src={createImageUrl(image.HostUrl, image.OriginalImgPath, '110x61')} />
				</div>
			)
		})

		return list
	}

	handleMainSliderBeforeChange(oldindex, newindex) {
		const currentSlideIndex = newindex >= 0 ? newindex : this.state.slideIndex
		const currentSlideTitle = this.props.images[currentSlideIndex].title

		this.refs.thumbnailSlider.slickGoTo(currentSlideIndex)

		this.setState({
			slideIndex: currentSlideIndex,
			slideTitle: currentSlideTitle
		})
	}

	handleThumbnailSliderBeforeChange(oldindex, newindex) {
		if (event) {
			if (event.currentTarget.classList.contains('slick-slide')) {
				const currentSlideIndex = newindex

				// reset click event to trigger 'before change' function loop
				event = undefined

				this.refs.mainSlider.slickGoTo(currentSlideIndex);
			}
		}
	}

	render() {
	    if( !this.props.images && this.props.images.length === 0 ){
	        return false;
	    }
		const mainSliderSettings = {
			className: 'slider__main',
			initialSlide: this.state.slideIndex,
			infinite: false,
			lazyLoad: true,
			slidesToShow: 1,
			slidesToScroll: 1,
			beforeChange: this.handleMainSliderBeforeChange
		};

		const thumbnailSliderSettings = {
			arrows: false,
			className: 'slider__thumbnail',
			focusOnSelect: true,
			initialSlide: this.state.slideIndex,
			infinite: false,
			slidesToShow: 1,
			slidesToScroll: 1,
			variableWidth: true,
			beforeChange: this.handleThumbnailSliderBeforeChange
		};

		const activeClass = this.state.isActive ? 'gallery--active' : '';
		const slideTitle = this.props.images[this.state.slideIndex].ImageName;

		return (
			<div className={"article-gallery-container " + activeClass}>
				<h3 className="gallery__heading">{this.state.heading}</h3>
				<span onClick={this.handleGalleryClose} className="gallery__close"></span>
				<div className="gallery__body">
					<div className="gallery-body__header">
						<span className="gallery-slider__title">{slideTitle}</span>
						<span className="gallery-slider__count">
							{this.state.slideIndex + 1} of {this.props.totalCount}
						</span>
						<div className="clear"></div>
					</div>

					<Slider ref="mainSlider" {...mainSliderSettings}>
						{this.getMainCarouselSlides()}
					</Slider>
					<Slider ref="thumbnailSlider" {...thumbnailSliderSettings}>
						{this.getThumbnailSlides()}
					</Slider>
				</div>
			</div>
		)
	}
}

Gallery.defaultProps = defaultProps;

export default Gallery;
