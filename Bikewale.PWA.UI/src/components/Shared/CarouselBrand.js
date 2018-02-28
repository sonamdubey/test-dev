import React from 'react';

class CarouselBrand extends React.Component{
    constructor(props){
        super(props);
        this.renderBrand = this.renderBrand.bind(this);
        this.renderBrandList = this.renderBrandList.bind(this);
    }
    renderBrand(brand) {
        return (
            <li className="carousel-slide">
				<div className="carousel-card">
					<a href={"/m/" + brand.MaskingName +"-scooters/"} title={brand.MakeName + " scooters"} className="card-target-block">
						<div className="brand-logo-image">
							<span class="brand-type">
								<span class={"brandlogosprite brand-".concat(brand.MakeId.toString())}></span>
							</span>
						</div>
						<div className="card-details-block">
							<p class="brand-details__title">{brand.MakeName}</p>
							<h3 class="brand-details__subtitle">{brand.TotalCount} scooters</h3>
						</div>
					</a>
				</div>
			</li>);
    }
    renderBrandList(brandList) {
        return (<ul className="carousel-wrapper brand-type-carousel">
					{brandList.map(function(brand){return this.renderBrand(brand)}.bind(this))}
				</ul>);
    }
    render() {
        var brandWidget = this.props.brandList;
        if(!brandWidget || !brandWidget.MakeList || brandWidget.MakeList.length == 0) {
            return ;
        }
        return (
			<div className="container bg-white box-shadow section-bottom-margin carousel-bottom-padding carousel-top-padding">
				<div className="carousel-heading-content">
					<div className="swiper-heading-left-grid inline-block">
						<h2>{brandWidget.Heading}</h2>
					</div>
					<div className="swiper-heading-right-grid inline-block text-right">
						<a href={brandWidget.CompleteListUrl} title={brandWidget.CompleteListUrlAlternateLabel} className="btn view-all-target-btn">{brandWidget.CompleteListUrlLabel}</a>
					</div>
				</div>
                {this.renderBrandList(brandWidget.MakeList)}
			</div>
		)
	}
}

export default CarouselBrand;
