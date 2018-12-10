import React from 'react'
import PropTypes from 'prop-types'

import { getVersionPageUrl } from '../../utils/UrlFactory';
import { trackCustomData } from '../../utils/cwTrackingPwa';
import {CATEGORY_NAME} from '../constants/index';

const propTypes = {
	// model details object
	data: PropTypes.object
}

function VersionBox({ data,makeName, modelName, modelRank, versionRank, versionCount, makeMaskingName, modelMaskingName, trackEvent, pageName }) {

    const trackVersionClick = () => {
		let action = pageName == 'mainListing' ? 'ListingVersionClick' : 'ShortlistVersionClick'
        trackCustomData(CATEGORY_NAME,action,"versionId="+ data.id + "|versionName=" +data.name + "|versionRank="+ versionRank +"|totalVersions="+versionCount+"|modelName="+modelName+"|modelRank="+modelRank)
	}

	return (
		<a href={getVersionPageUrl(makeMaskingName, modelMaskingName, data.maskingName)} onClick={trackVersionClick} title={makeName+" "+modelName+" "+data.name} className="version__box">
			<h3>
				<span className="version__name">{data.name}</span>
			</h3>
			<ul className="version__feature-list">
				{
					data.maxPower
						? <li className="feature-list__item">{data.maxPower}</li>
						: ''
				}
				{
					data.displacement
						? <li className="feature-list__item">{data.displacement}</li>
						: ''
				}
				{
					data.transmission
						? <li className="feature-list__item">{data.transmission}</li>
						: ''
				}
				{
					data.fuelType
						? <li className="feature-list__item">{data.fuelType}</li>
						: ''
				}
			</ul>
		</a>
	)
}

export default VersionBox
