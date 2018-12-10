import React from 'react'
import {withRouter } from 'react-router-dom'
import { connect } from 'react-redux'
import FiltersScreen from './FiltersScreen'
import ncfDefaultFilters from '../constants/NCFdefaultfilters'
import { deserialzeQueryStringToObject} from '../../utils/Common'
import { reduceToSelectionObject, mergeQStoStore, modifySearchParamsWithStoredFilters} from '../selectors/FilterPluginSelectors'

class Main extends React.Component {
	constructor(props) {
		super(props)
	}
  render() {
		const ncfDefaultFiltersClone = ncfDefaultFilters()
		let params = this.props.location.search
		let searchParams = {}
		let defaultFilters = {}
		if(this.props.filterPreSelected != "")
		{
			params = params + "&" + this.props.filterPreSelected;
		}

		searchParams = deserialzeQueryStringToObject(params)
		searchParams = modifySearchParamsWithStoredFilters(searchParams, this.props.filters)

		defaultFilters = mergeQStoStore(searchParams, ncfDefaultFiltersClone)

		const selectionObject = reduceToSelectionObject(defaultFilters)
		return (
				<FiltersScreen
					reqFilters={this.props.reqFilters}
					cityId={this.props.cityId}
					trackingCategory={this.props.trackingCategory}
					callbackFunction={this.props.callbackFunction}
					defaultFilters={defaultFilters}
					selectionObject={selectionObject}
					openFilter={this.props.active}
					location={this.props.location}
					history={this.props.history}
					match={this.props.match}
					/>
		)
	}
}
const mapStateToProps = (state) => {
	const {
		active,
		activeAccordion,
		filters
	} = state.filtersScreen
	return {
		active,
		activeAccordion,
		filters
	}
}
export default withRouter(connect(mapStateToProps, null)(Main))
