jest.mock('jquery', () => {
    return {
        cookie: jest.fn(( key ) => 2)
    }
})

const GalleryAds = require('../js/gallery-ads');

describe('Ad blocker is on', () => {
    it('isRefreshRequired should return false', () => 
    {
        let carousal = "carmodel-image-swipper";
        GalleryAds.isAdblockDetecter = false;
        GalleryAds.abTestVal = 2;
        GalleryAds.swipeCounter = 2;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(false);

        GalleryAds.abTestVal = 26;
        GalleryAds.swipeCounter = 6;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(false);

        GalleryAds.abTestVal = 51;
        GalleryAds.swipeCounter = 10;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(false);

        carousal = "tab";
        GalleryAds.abTestVal = 6;
        GalleryAds.swipeCounter = 2;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(false);
    })
})

describe('Ad blocker is off and carousal is "carmodel-image-swipper"', () => {
    it('isRefreshRequired should return true when swipe count is multiple of 2 for abtest cookie value cookie between 1-50', () => 
    {
        let carousal = "carmodel-image-swipper";
        GalleryAds.isAdblockDetecter = true;
        GalleryAds.abTestVal = 1;
        GalleryAds.swipeCounter = 2;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(true);
        
        GalleryAds.abTestVal = 50;
        GalleryAds.swipeCounter = 6;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(true);
        
        GalleryAds.abTestVal = 7;
        GalleryAds.swipeCounter = 2;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(true);
        
        GalleryAds.abTestVal = 7;
        GalleryAds.swipeCounter = 3;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(false);
        
        GalleryAds.abTestVal = 51;
        GalleryAds.swipeCounter = 4;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(false);
    })
})

describe('Ad blocker is off and carousal is "carmodel-image-swipper"', () => {
    it('isRefreshRequired should return true when swipe count is multiple of 3 for abtest cookie value between 51-100', () => 
    {
        let carousal = "carmodel-image-swipper";
        GalleryAds.isAdblockDetecter = true;
        GalleryAds.abTestVal = 51;
        GalleryAds.swipeCounter = 3;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(true);
        
        GalleryAds.abTestVal = 100;
        GalleryAds.swipeCounter = 6;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(true);
        
        GalleryAds.abTestVal = 60;
        GalleryAds.swipeCounter = 9;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(true);
        
        GalleryAds.abTestVal = 60;
        GalleryAds.swipeCounter = 29;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(false);
        
        GalleryAds.abTestVal = 24;
        GalleryAds.swipeCounter = 33;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(false);
    })
})

describe('Ad blocker is off and carousal is "tab"', () => {
    it('isRefreshRequired should return true irrespective of abtest cookie value', () => 
    {
        let carousal = "tab";
        GalleryAds.isAdblockDetecter = true;
        GalleryAds.abTestVal = 10;
        GalleryAds.swipeCounter = 2;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(true);
        
        GalleryAds.abTestVal = 30;
        GalleryAds.swipeCounter = 6;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(true);
        
        GalleryAds.abTestVal = 60;
        GalleryAds.swipeCounter = 2;
        expect(GalleryAds.responsive.isRefreshRequired(carousal)).toBe(true);
    })
})
