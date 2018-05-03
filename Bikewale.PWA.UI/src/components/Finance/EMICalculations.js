export function EmiCalculation(loanAmount, tenure, interest) {
    const finalLoanAmout = parseFloat(loanAmount.max - loanAmount.values[0])
    const numberOfMonths = parseFloat(tenure.values)
    const rateOfInterest = (interest.values)
    const monthlyInterestRatio = (rateOfInterest/100)/12
    const emiDividend = Math.pow((1+monthlyInterestRatio),numberOfMonths)
    const emiDivisor = emiDividend - 1;
    const emiDivision = emiDividend / emiDivisor
    const emi = parseInt((finalLoanAmout * monthlyInterestRatio) * emiDivision)
    return emi
}

export function InterestPayable(loanAmount, tenure, interest) {
  let emiValue = EmiCalculation(loanAmount, tenure, interest)
  let numberOfMonths = parseFloat(tenure.values)
  let emiWithmonth = parseFloat(emiValue * numberOfMonths)
  const finalLoanAmout = parseFloat(loanAmount.max - loanAmount.values)
  const totalInterestPayable = parseFloat(emiWithmonth - finalLoanAmout)
  return totalInterestPayable
}

export function TotalPrincipalAmt(loanAmount, tenure, interest) {
  const finalLoanAmout = parseFloat(loanAmount.max - loanAmount.values[0])
  let intPayable = InterestPayable(loanAmount, tenure, interest)
  let totalPrincipalAmt = parseInt(finalLoanAmout + intPayable)
  return totalPrincipalAmt
}