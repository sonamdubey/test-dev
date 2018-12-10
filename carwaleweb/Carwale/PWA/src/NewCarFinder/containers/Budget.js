import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import SpeedometerLoader from '../../components/SpeedometerLoader'
import Rheostat from '../../components/Rheostat'
import PitComponent from '../components/BudgetPit'
import {
	updateBudgetSlider
} from '../actionCreators/Budget'
import {
	setCurrentScreen
} from '../actionCreators/Filter'
import BudgetApi from '../apis/Budget'
import {
	formatValueWithComma
} from '../../utils/Common'
import {
	formatBudgetTooltipValue
} from '../utils/Budget'
import {
	getScreenId
} from '../selectors/NCFSelectors'
import { getCityId } from '../../selectors/LocationSelectors'
import {
	fireInteractiveTracking
} from '../../utils/Analytics'
import {CATEGORY_NAME} from '../constants/index';
import { trackCustomData } from '../../utils/cwTrackingPwa';
class Budget extends React.Component {
	constructor(props) {
		super(props)
		this.state = {
			isBudgetValid: true,
			budgetErrorText: '',
			oldBudgetAmount: 0,
			isFetching: true
		}
	}
	componentDidMount(){
		const {
			setCurrentScreen,
			screenId,
			cityId,
			updateBudgetSlider,
			slider
		} = this.props
		setCurrentScreen(screenId)
		//fetch suggested budget for city
		if (slider.values[0] <= 0 || !slider.userChange) {
			let {storedBudget} = this.props
			if(storedBudget.length <= 0 || storedBudget[0] <= 0){
				BudgetApi.get(cityId).then(response => {
					Object.keys(response).forEach(key => response[key] = response[key] * 100000)
					const { min, max, suggested } = response
					updateBudgetSlider({ values: [suggested], min, max, userChange: false })
					setTimeout(() => this.setState({ isFetching: false }), 300)
				}).catch(error => {
					updateBudgetSlider({values: [9],min:2,max:100,userChange: false})
					console.log(error)
					this.setState({ isFetching: false })
				})
			}
			else{
				updateBudgetSlider({ values: storedBudget, userChange: false })
				this.setState({ isFetching: false })
			}
		}
		else {
			this.setState({ isFetching: false })
		}
		trackCustomData(CATEGORY_NAME, "BudgetScreenImpression", "NA", false)
	}

	componentWillUnmount() {
		const {
			slider
		} = this.props;
		fireInteractiveTracking(CATEGORY_NAME, "NCF_BudgetFilter", "Final_" + slider.values[0] + "_SliderUsed")
		trackCustomData(CATEGORY_NAME, "BudgetFilterFinalSliderUsed", "value=" + slider.values[0], false)
	}

	componentWillReceiveProps(nextprops){
		const { slider } = nextprops
		this.validateBudget(slider.value)
	}

	handleSliderChange = ({ values }) => {
		const {
			updateBudgetSlider
		} = this.props

		updateBudgetSlider({ values, userChange: true })
	}

	handleSliderDragEnd = () => {
		const{
			slider
		} = this.props;
		fireInteractiveTracking(CATEGORY_NAME,"NCF_BudgetFilter","SliderUsed_"+slider.values[0])
		trackCustomData(CATEGORY_NAME,"BudgetFilterSliderUsed","value="+slider.values[0],false)
	}

	validateBudget = (updatedValue) => {

		const {
			slider
		} = this.props

		if (updatedValue < slider.min) {
			this.setState({
				isBudgetValid: false,
				budgetErrorText: 'Invalid amount'
			})
		}
		else {
			this.setState({
				isBudgetValid: true,
				budgetErrorText: ''
			})
		}
	}

	render() {
		if (this.state.isFetching) {
			return <SpeedometerLoader />
		}
		let {
			slider
		} = this.props

		slider = {
			...slider,
			className: 'budget-rheostat',
			pitComponent: PitComponent,
			pitPoints: [slider.min, slider.max],
			pitPointLabel: formatBudgetTooltipValue,
			snap: true,
			snapOnDragMove: true,
			disableSnapOnClick: true,
			onChange: this.handleSliderChange,
			onSliderDragEnd: this.handleSliderDragEnd,
			handleTooltipLabel: formatBudgetTooltipValue
		}

		return (
			<div className="budget-screen">
				<div className="screen__head">
					<span className="budget-image"></span>
					<h1 className="screen-head__title">{"What's your Budget?"}</h1>
				</div>

				<div className="screen__body">
					<Rheostat
						{...slider}
					/>
				</div>
			</div>
		)
	}
}

const mapStateToProps = (state) => {
	const {
		slider
	} = state.newCarFinder.budget
	const {
		budget: storedBudget
	} = state.filtersScreen.filters
	const screenId = getScreenId(state, 'BudgetFilter')
	const cityId = getCityId(state)
	return {
		slider,
		...cityId,
		screenId,
		storedBudget
	}
}

const mapDispatchToProps = (dispatch, getState) => {
	return {
		updateBudgetSlider: bindActionCreators(updateBudgetSlider, dispatch),
		setCurrentScreen: bindActionCreators(setCurrentScreen, dispatch)
	}
}

export default connect(mapStateToProps, mapDispatchToProps)(Budget)
