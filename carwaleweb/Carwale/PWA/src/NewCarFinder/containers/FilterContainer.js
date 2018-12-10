/**
 * This class representing the container component
 * for filters buttons
 * @class FilterContainer
 * @extends {React.Component}
*/
import React from 'react';
import {
    connect
} from 'react-redux'
import {
    bindActionCreators
} from 'redux'
import {
    openFilterScreen
} from '../../filterPlugin/actionCreators/FiltersScreen'
import {
    fireInteractiveTracking
} from '../../utils/Analytics';
import {
    trackCustomData
} from '../../utils/cwTrackingPwa';
import {
    CATEGORY_NAME
} from '../constants';
import FilterButton from '../components/FilterButton';
import PropTypes from 'prop-types'

const propTypes = {
    searchParams: PropTypes.object
}
const defaultProps = {
    searchParams: {}
}
class FilterContainer extends React.Component {
     constructor(props){
         super(props);
     }
     handleFilterClick = () => {
         this.props.openFilterScreen()
         trackCustomData(CATEGORY_NAME, "FilterButtonClicked", "page=NcfListing", false)
     }
     getSelections = () => {
        const { searchParams } = this.props
        let count = 0;
		if (searchParams) {
			Object.keys(searchParams).map(selection => {
                if(selection != "removedModelIds" && selection != "cityId"){
                    count += searchParams[selection].split(',').length;
                }
            });
            return count;
        }
	}
     render() {
        let count = this.getSelections()
		return (
			<FilterButton withCompare={this.props.withCompare} count={count} text='Filter' onClick={this.handleFilterClick} />
		)
	}
}

const mapDispatchToProps = (dispatch, getState) => {
    return {
        openFilterScreen: bindActionCreators(openFilterScreen, dispatch)
    }
}
FilterContainer.propTypes = propTypes
FilterContainer.defaultProps = defaultProps
export default connect(null, mapDispatchToProps)(FilterContainer)
