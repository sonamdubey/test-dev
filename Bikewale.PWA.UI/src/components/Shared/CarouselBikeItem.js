import React from 'react'
import LazyLoad from 'react-lazy-load'

class CarouselBikeItem extends React.Component {
	render() {
		if(!this.props.item) return null;
        var item = this.props.item;
		var priceHtml = null;
        if(item.Price == "") {
            priceHtml = <span className="value-size-16">{item.PriceSuffix}</span>
        }
        else {
            priceHtml = (
                            <div className="value-size-16">
                                <span>&#x20B9;</span>&nbsp;
                                <span>{item.Price} {item.PriceSuffix}</span>
                            </div>
                        )
        }

        return (
            <li key={item.Name} className="carousel-slide">
                <div className="carousel-card">
                    <a href={item.DetailPageUrl} title={item.Name} className="card-target-block">
                        <div className="card-image-block">
                            <LazyLoad offsetVertical={0}>
                                <img src={item.ImgUrl} alt={item.Name} />
                            </LazyLoad>
                        </div>
                        <div className="card-details-block">
                            <h3 className="h3-title-target target-link text-truncate">{item.Name}</h3>
                            <p className="text-truncate key-size-11">{item.PriceDescription}</p>
                            {priceHtml}
                        </div>
                    </a>
                </div>
            </li>
        )
    }
}

module.exports = CarouselBikeItem;
