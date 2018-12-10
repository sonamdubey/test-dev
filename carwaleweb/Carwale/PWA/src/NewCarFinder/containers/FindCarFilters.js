import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import FindCarFilterRouter from '../routes/FindCarFilters'
import FiltersFooter from './FiltersFooter'
import {
	hideFooter
} from '../../actionCreators/Footer'
import {
    hideHeader
} from '../../actionCreators/Header'
import {
    hideShortlistIcon,makeHeaderNormal
} from '../actionCreators/HeaderWrapper'

class FindCarFilters extends React.Component {
	constructor(props) {
		super(props)
	}

	componentWillMount(){
		this.props.hideShortlistIcon()
		this.props.makeHeaderNormal()
	}
	render() {
			return (
			<div>
				<FindCarFilterRouter />
				<FiltersFooter />
			</div>
			)
		}
}

const mapDispatchToProps = (dispatch) => {
    return {
        hideFooter: bindActionCreators(hideFooter, dispatch),
		hideHeader: bindActionCreators(hideHeader, dispatch),
		hideShortlistIcon: bindActionCreators(hideShortlistIcon, dispatch),
		makeHeaderNormal: bindActionCreators(makeHeaderNormal, dispatch)
    }
}


export default connect(null,mapDispatchToProps)(FindCarFilters)
