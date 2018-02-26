import React from 'react';

class CarouselBrand extends React.Component{
	render() {
		return (
			<div className="container bg-white box-shadow section-bottom-margin carousel-bottom-padding carousel-top-padding">
				<div className="carousel-heading-content">
					<div className="swiper-heading-left-grid inline-block">
						<h2>Popular scooter brands</h2>
					</div>
					<div className="swiper-heading-right-grid inline-block text-right">
						<a href="" title="" className="btn view-all-target-btn">View all</a>
					</div>
				</div>
				<ul className="carousel-wrapper brand-type-carousel">
					<li className="carousel-slide">
						<div className="carousel-card">
							<a href="" title="" className="card-target-block">
								<div className="brand-logo-image">
									<span class="brand-type">
										<span class="brandlogosprite brand-7"></span>
									</span>
								</div>
								<div className="card-details-block">
									<p class="brand-details__title">Honda</p>
									<h3 class="brand-details__subtitle">6 scooters</h3>
								</div>
							</a>
						</div>
					</li>
					<li className="carousel-slide">
						<div className="carousel-card">
							<a href="" title="" className="card-target-block">
								<div className="brand-logo-image">
									<span class="brand-type">
										<span class="brandlogosprite brand-15"></span>
									</span>
								</div>
								<div className="card-details-block">
									<p class="brand-details__title">TVS</p>
									<h3 class="brand-details__subtitle">6 scooters</h3>
								</div>
							</a>
						</div>
					</li>
					<li className="carousel-slide">
						<div className="carousel-card">
							<a href="" title="" className="card-target-block">
								<div className="brand-logo-image">
									<span class="brand-type">
										<span class="brandlogosprite brand-13"></span>
									</span>
								</div>
								<div className="card-details-block">
									<p class="brand-details__title">Yamaha</p>
									<h3 class="brand-details__subtitle">6 scooters</h3>
								</div>
							</a>
						</div>
					</li>
					<li className="carousel-slide">
						<div className="carousel-card">
							<a href="" title="" className="card-target-block">
								<div className="brand-logo-image">
									<span class="brand-type">
										<span class="brandlogosprite brand-6"></span>
									</span>
								</div>
								<div className="card-details-block">
									<p class="brand-details__title">Hero</p>
									<h3 class="brand-details__subtitle">6 scooters</h3>
								</div>
							</a>
						</div>
					</li>
					<li className="carousel-slide">
						<div className="carousel-card">
							<a href="" title="" className="card-target-block">
								<div className="brand-logo-image">
									<span class="brand-type">
										<span class="brandlogosprite brand-1"></span>
									</span>
								</div>
								<div className="card-details-block">
									<p class="brand-details__title">Bajaj</p>
									<h3 class="brand-details__subtitle">6 scooters</h3>
								</div>
							</a>
						</div>
					</li>
				</ul>
			</div>
		)
	}
}

export default CarouselBrand;
