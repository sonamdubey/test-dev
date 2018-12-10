
import * as UrlFactory from '../../src/utils/UrlFactory'

describe('getModelPageUrl', () => {
    it('should return modelpage url if makeMaskingName, modelMaskingName given', () => {
        let url = UrlFactory.getModelPageUrl('marutisuzuki', 'dzire')
        expect(url).toBe('/marutisuzuki-cars/dzire/')
    })
    it('should return undefined if any of makeMaskingName or modelMaskingName is null or empty', () => {
        let url = UrlFactory.getModelPageUrl('', 'dzire')
        expect(url).toBeUndefined()

        url = UrlFactory.getModelPageUrl('', '')
        expect(url).toBeUndefined()
    })
})

describe('getVersionPageUrl', () => {
    it('should return varsionPage url if makeMaskingName, modelMaskingName given', () => {
        let url = UrlFactory.getVersionPageUrl('marutisuzuki', 'dzire', 'lxi')
        expect(url).toBe('/m/marutisuzuki-cars/dzire/lxi/')

        url = UrlFactory.getVersionPageUrl('marutisuzuki', 'dzire', 'lxi', false)
        expect(url).toBe('/marutisuzuki-cars/dzire/lxi/')
    })
    it('should return undefined if any of makeMaskingName,modelMaskingName or versionMaskingName is null or empty', () => {
        let url = UrlFactory.getVersionPageUrl('', 'dzire','lxi')
        expect(url).toBeUndefined()

        url = UrlFactory.getVersionPageUrl('marutisuzuki', '','lxi', false)
        expect(url).toBeUndefined()

        url = UrlFactory.getVersionPageUrl('', '', '')
        expect(url).toBeUndefined()
    })
})

describe('getCompareUrl', () => {
    let data = [
        { modelId: 918, makeMaskingName: 'volkswagen', modelMaskingName: 'vento', versionId: 4558 },
        { modelId: 1116, makeMaskingName: 'marutisuzuki', modelMaskingName: 'ciaz', versionId: 5181}
    ]
    it('should return compareUrl', () => {
        let url = UrlFactory.getCompareUrl(data, false)
        expect(url).toMatch('/comparecars/marutisuzuki-ciaz-vs-volkswagen-vento/?c1=5181&c2=4558')

        url = UrlFactory.getCompareUrl(data)
        expect(url).toBe('/m/comparecars/marutisuzuki-ciaz-vs-volkswagen-vento/?c1=5181&c2=4558')

        data = null
        url = UrlFactory.getCompareUrl(data)
        expect(url).toBeUndefined()
    })
})
