export function EmiCalculation(loanAmount, tenure, interest) {
    const finalLoanAmout = parseFloat(loanAmount.max - loanAmount.values[0])
    const numberOfMonths = parseFloat(tenure.values) * 12
    const rateOfInterest = (interest.values)
    const monthlyInterestRatio = (rateOfInterest/100)/12
    const top = Math.pow((1+monthlyInterestRatio),numberOfMonths)
    const bottom = top -1;
    const sp = top / bottom;
    const emi = parseInt((finalLoanAmout * monthlyInterestRatio) * sp)
    return emi
}

export function InterestPayable(loanAmount, tenure, interest) {
  let emiValue = EmiCalculation(loanAmount, tenure, interest)
  let numberOfMonths = parseFloat(tenure.values) * 12
  let emiWithmonth = parseFloat(emiValue * numberOfMonths)
  const finalLoanAmout = parseFloat(loanAmount.max - loanAmount.values)
  const totalInterestPayable = parseFloat(emiWithmonth - finalLoanAmout)
  return totalInterestPayable
}

export function TotalPrincipalAmt(loanAmount, tenure, interest) {
    const finalLoanAmout = parseFloat(loanAmount.max - loanAmount.values[0])
    let intPayable = InterestPayable(loanAmount, tenure, interest)
    TotalPrincipalAmt = parseInt(finalLoanAmout + intPayable)
    return TotalPrincipalAmt
}
