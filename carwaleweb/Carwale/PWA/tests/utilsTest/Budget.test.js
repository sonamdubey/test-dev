import {formatBudgetTooltipValue} from '../../src/NewCarFinder/utils/Budget'

describe('formatBudgetTooltipValue', () => {
    it('should return formatted value in lakh and crore', () => {
        let formattedValue = formatBudgetTooltipValue(200000)
        expect(formattedValue).toMatch('Upto 2 lakh')

        formattedValue = formatBudgetTooltipValue(250000)
        expect(formattedValue).toMatch('2.5 lakh')

        formattedValue = formatBudgetTooltipValue(25000000)
        expect(formattedValue).toMatch('2.5+ Cr')
    })
})
