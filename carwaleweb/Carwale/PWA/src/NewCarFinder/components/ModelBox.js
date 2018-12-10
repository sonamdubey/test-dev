import React from 'react'
import PropTypes from 'prop-types'
import { getModelPageUrl } from '../../utils/UrlFactory';
import { trackCustomData } from '../../utils/cwTrackingPwa';
import {CATEGORY_NAME} from '../constants/index';

const propTypes = {
	// model details object
	data: PropTypes.object
}
function ModelBox(props) {
	let {
		data,
		modelRank,
		pageName,
		isShortlisted
	} = props
	const trackModelClick = () => {
		let action = pageName == 'mainListing' ? 'ListingModelClick' : 'ShortlistModelClick'
		trackCustomData(CATEGORY_NAME,action,"makeName="+data.makeName + "|modelId=" + data.modelId + "|modelName=" + data.modelName + "|modelRank=" + modelRank ,false)
	}
	const handleModelBoxClick = (event) => {
		if(isShortlisted) {
			event.preventDefault()
			event.stopPropagation()
			props.toggleVersionList()
			if(event.currentTarget.classList.contains('left')) {
				event.currentTarget.classList.remove('left')
			}
			else {
				event.currentTarget.classList.add('left')
			}
		}
	}
	const getClosest = (element, tag) => {
		do {
			if (element.classList.contains(tag)) {
				// tag name is found! let's return it. :)
				return element;
			}
		} while (element = element.parentNode);
		// not found :(
		return null;
	}
	return (
		<div className="model-list__item-container">
			<a href={ getModelPageUrl(data.makeMaskingName, data.modelMaskingName)} onClick={trackModelClick} title={data.makeName +" "+data.modelName} className="model__box">
				<div className="model-box__image">
					<img src={data.hostUrl + '144X81' + data.originalImagePath} alt={data.makeName +" "+data.modelName} />
				</div>
				<h2 className="model-box__title">
					<span className="model-box__make-name">{data.makeName}</span>
					<span className="model-box__model-name">{data.modelName}</span>
				</h2>
				<span className="model-box__arrow" onClick={handleModelBoxClick}></span>
			</a>
		</div>
	)
}

export default ModelBox
