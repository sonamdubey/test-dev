import React from 'react'
import Autocomplete from '../components/Autocomplete'
import {closeCityAreaSelectionPopup} from '../utils/popUpUtils'

class CityAreaAutocomplete  extends React.Component {
	constructor(props) {
		super(props);
		this.state = {
			type : 'city',
			completeCityAreaList : [],
			displayedCityAreaList : [],
			value : ''
		}
		
		this.filterData = this.filterData.bind(this);
		this.renderMenuItem = this.renderMenuItem.bind(this);
		this.renderMenu = this.renderMenu.bind(this);
		this.onChange = this.onChange.bind(this);
		this.onSelect = this.onSelect.bind(this);
			
	}
	
	componentWillReceiveProps(nextProps) {
		this.setState({
			completeCityAreaList : nextProps.completeCityAreaList,
			displayedCityAreaList : nextProps.completeCityAreaList,
			type: nextProps.type,
			value : ''

		})
	}
	onSelect(state) {
		if(this.props.citySelect !== null && this.props.citySelect !== undefined && typeof this.props.citySelect == 'function') {
			this.props.citySelect(state);
		}
		else if(this.props.areaSelect !== null && this.props.areaSelect !== undefined && typeof this.props.areaSelect == 'function') {
			this.props.areaSelect(state);
		}
		document.getElementById("popup"+(this.state.type == 'area' ? "Area" : "City")+"Input").blur();

	}
	onChange() {
		try{
			var value = document.getElementById("popup"+(this.state.type == 'area' ? "Area" : "City")+"Input").value;
			var filteredList = this.filterData(this.state.completeCityAreaList , value);
			this.setState({
				value: value,
				displayedCityAreaList : filteredList
			})	
		}catch(err){}
		
	}
	filterData(placeList , filterString) {
		var filteredList = [];
		if(filterString && filterString.length > 0) {
			var pattern = new RegExp(filterString , "i");
			filteredList = placeList.filter(function(place) {
				if(pattern.test(place.name)) return place;
			})
		}
		else {
			return placeList;
		}
		return filteredList;

	}
	renderMenuItem(item) {
		return (<li id = {item.id} onClick={function(){this.onSelect(item)}.bind(this)}>
	  				{item.name}	
		  		</li>);
	}
	renderMenu(items, value) {
		if(value == undefined || value == null || items == undefined || items == null)
			return <ul/>;
		return (<ul id={"popup"+(this.state.type == 'area' ? "Area" : "City")+"List"}>
					{
						items.map(function(item){
							return this.renderMenuItem(item)
						}.bind(this))
					}
				</ul>)
	}
	render() {
		if(this.state.type == null || this.state.type == '') 
			return null;
		return (
			<div id={"bw-"+this.state.type+"-popup-box"} className={"bw-"+this.state.type+"-popup-box bwm-city-area-box "+this.state.type+"-list-container form-control-box text-left"}>
				<div className="user-input-box">
					<span className="back-arrow-box" onClick={closeCityAreaSelectionPopup}>
						<span className="bwmsprite back-long-arrow-left"></span>
					</span>
					<Autocomplete
					 	value = {this.state.value}
					  	items = {this.state.displayedCityAreaList}
					  	inputProps = {{
					  		className:"form-control padding-right40 ",
					  		placeholder:"Type to select "+this.state.type,
					  		id : "popup"+(this.state.type == 'area' ? "Area" : "City")+"Input"
					  	}}
					  	onChange = {this.onChange}
					  	renderMenu = {this.renderMenu}
					 	wrapperStyle = {{
							'position' : 'relative'  
						}}
					  
					/>
				</div>
			</div>)
	}
}

module.exports = CityAreaAutocomplete;