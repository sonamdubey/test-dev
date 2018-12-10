import React from 'react'
import PropTypes from 'prop-types'
import ModelBox from './ModelBox'
import VersionList from '../containers/VersionList'

import { trackCustomData } from '../../utils/cwTrackingPwa';
import {CATEGORY_NAME} from '../constants/index';

import OnVisible, { setDefaultProps } from '../utils/react-on-visible';

setDefaultProps({
    bounce: false,
    visibleClassName: 'appear',
	percent: 50,
	onChange: function (visible){
        if(visible){
			trackCustomData(CATEGORY_NAME,this.children.props.action,this.children.props.label,false)
			if(this.OnVisibleCallback != undefined)
			{
				this.OnVisibleCallback()
			}
        }
	}
})

const propTypes = {
    // Model data
    item: PropTypes.object,
    // model rank
    rank: PropTypes.number,
    // true if model is in shortlisted
    isShortlisted: PropTypes.bool
}
/**
 * This class representing the component
 * for find car model
 *
 * @class ModelCard
 * @extends {React.Component}
 */
class ModelCard extends React.Component {
	constructor(props) {
        super(props)
        this.state = {
            listActive: !this.props.isShortlisted
        }
	}
    componentWillReceiveProps (nextProps) {
        if(!this.props.isShortlisted && nextProps.isShortlisted) {
            this.setState({ listActive: false })
        }
        else if(!nextProps.isShortlisted && !this.state.listActive) {
            this.setState({ listActive: true })
        }
    }

    toggleVersionList = () => {
        if(this.state.listActive) {
            let VersionList = this.refs.modelCard.querySelector('.version-list__container')
            setTimeout(function(){
                VersionList.classList.add('version-list__container--collapse')
            },0)
            setTimeout(this.setState.bind(this,{ listActive: false }),401)
        }
        else {
            this.setState({ listActive: true })
        }
    }

    shouldComponentUpdate(nextProps,nextState){
        if(this.state.listActive !== nextState.listActive){
			return true
        }
        if(this.props.pageName == 'mainListing' && this.props.isShortlisted !== nextProps.isShortlisted){
            return true;
        }
        if(this.props.pageName == 'mainListing' && this.props.shortlistCars.count != nextProps.shortlistCars.count){
            return true;
        }
        return false
    }

    getVersionList = (isShortlisted,item,rank) => {
		if(this.state.listActive){
            return <VersionList
                        data={item}
                        pageName={this.props.pageName}
                        cityName={this.props.cityName}
                        cityId={this.props.cityId}
                        modelRank={rank}
                        isShortlisted={isShortlisted}
                        shortlistCars={this.props.shortlistCars}
                    />
		}
		return null
    }

    componentDidUpdate(prevState) {
        if(this.state.listActive != prevState.listActive && this.state.listActive) {
            let VersionList = this.refs.modelCard.querySelector('.version-list__container')
            setTimeout(function(){
                VersionList.classList.remove('version-list__container--collapse')
            },0)
        }
    }

	render(){
		const {
            item,
            rank,
            isShortlisted
		} = this.props
		return (
            <OnVisible key={item.modelId} >
                <div action={this.props.action} label={this.props.label} ref="modelCard" className="model-card">
                    <ModelBox
                        data={item}
                        modelRank={rank}
                        toggleVersionList={this.toggleVersionList}
                        pageName={this.props.pageName}
                        isShortlisted={this.props.isShortlisted} />
                    {this.getVersionList(isShortlisted,item,rank)}
                </div>
            </OnVisible>
		)
	}
}

ModelCard.propTypes = propTypes

export default ModelCard
