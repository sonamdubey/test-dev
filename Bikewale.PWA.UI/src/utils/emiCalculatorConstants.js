
const minTenure = 12;
const maxTenure = 48;
const defaultTenure = (maxTenure - minTenure) / 2 + minTenure;
const minROI = 10;
const maxROI = 15;
const defaultROI = (maxROI - minROI) / 2 + minROI;
const minDownPaymentPercentage = 10;
const maxDownPaymentPercentage = 40;
const defaultDownPaymentPercentage = 30;

export default {
    minTenure : minTenure,
    maxTenure : maxTenure,
    defaultTenure : defaultTenure,
    minROI : minROI,
    maxROI : maxROI,
    defaultROI : defaultROI,
    minDownPaymentPercentage : minDownPaymentPercentage,
    maxDownPaymentPercentage : maxDownPaymentPercentage,
    defaultDownPaymentPercentage : defaultDownPaymentPercentage
};