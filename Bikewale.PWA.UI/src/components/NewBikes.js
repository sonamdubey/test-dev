import React from 'react'
import LazyLoad from 'react-lazy-load'
import {setDataforPopularBikesWidget, setPriceQuoteFlag} from '../utils/popUpUtils'
import {triggerGA} from '../utils/analyticsUtils'
class NewBikes extends React.Component {
    constructor(props) {
        super(props);
        this.checkOnRoadLinkClick = this.checkOnRoadLinkClick.bind(this);

    }
    propTypes : {
        newBikesData : React.PropTypes.object
    };
    checkOnRoadLinkClick (item,event) {
        var category = gaObj.name;
        triggerGA(category, "Get_On_Road_Price_Click", item.ModelName);
        event.preventDefault();
        setDataforPopularBikesWidget(event,item);
        setPriceQuoteFlag();

    }
    renderListItem(item,index) {
        var priceHtml = null;
        var button = null;
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
        if(item.IsNew) {
              button = (
                  <div class="card-btn-block">
                      <a data-pagecatid={gaObj.id} data-makename={item.MakeName} data-modelname={item.ModelName} data-modelid={item.ModelId} className="btn btn-card btn-full-width btn-white getquotation" onClick={this.checkOnRoadLinkClick.bind(this,item)}>Check on-road price</a>
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
                            {button}
                    </a>                
                </div>
            </li>
        )
    }
    render() {

        if(!this.props.newBikesData || !this.props.newBikesData.BikesList || this.props.newBikesData.BikesList.length === 0)
		{
			return false;
		}
        var newBikesData = this.props.newBikesData;
		return (
            <div className="container bg-white box-shadow section-bottom-margin carousel-bottom-padding carousel-top-padding">
                <div className="carousel-heading-content">
                    <div className="swiper-heading-left-grid inline-block">
                        <h2>{newBikesData.Heading}</h2>
                    </div>
                    <div className="swiper-heading-right-grid inline-block text-right">
                        <a href={newBikesData.CompleteListUrl} title={newBikesData.CompleteListUrlAlternateLabel} className="btn view-all-target-btn">View all</a>
                    </div>
                </div>
                <ul className="carousel-wrapper">
                    {           
                        newBikesData.BikesList.map(function(item,index) {                                         
                            return this.renderListItem(item,index)
                        }.bind(this))
                    }
                </ul>
            </div>
		)
    }
}

export default NewBikes
