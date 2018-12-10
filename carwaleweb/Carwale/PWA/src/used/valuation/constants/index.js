/**
 * PAGE URL's
 */
export const VALUATION_ENDPOINT = '/used/carvaluation/';

export const BUY_CAR_ID = 1
export const SELL_CAR_ID = 2

// kilometer
export const MIN_KILOMETER_DRIVEN = 1
export const MAX_KILOMETER_DRIVEN = 999999

// api end points
export const MAKE_END_POINT='/webapi/carmakesdata/getcarmakes/'
export const MODEL_END_POINT='/webapi/carmodeldata/GetCarModelsByType/'
export const VERSION_END_POINT='/webapi/carversionsdata/GetCarVersions/'
export const VALUATION_HTML_END_POINT='/m/used/valuation/v1/report/'
export const CITY_MASKING_NAME_END_POINT='/api/city/'
export const VERSION_DETAILS_END_POINT='/webapi/CarVersionsData/GetCarDetailsByVersionId/'
export const MODEL_DETAILS_END_POINT = '/webapi/CarModelData/GetCarDetailsByModelId/'

//manufacturing year
export const DEFAULT_MANUFACTURING_YEAR = 1997
export const MIN_MANUFACTURING_YEAR = 1998
export const MAX_MANUFACTURING_YEAR = new Date().getFullYear()

//Report CTA button text
export const BUY_CAR_BUTTON_TEXT = 'View Similar Cars'
export const SELL_CAR_BUTTON_TEXT = 'Sell Your Car'
