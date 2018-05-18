
export function EmiCalculation(loanAmount, tenure, rateOfInterest) {
  const monthlyInterestRatio = (rateOfInterest/1200);
  return parseInt((loanAmount * monthlyInterestRatio) / (1 - Math.pow((1 + monthlyInterestRatio), -1 * tenure)));
}

export function InterestPayable(loanAmount, tenure, interest) {
  return parseFloat(EmiCalculation(loanAmount, tenure, interest) * tenure) - loanAmount;
}

export function TotalPrincipalAmt(downPayment, loanAmount, tenure, interest) {
  let intPayable = InterestPayable(loanAmount, tenure, interest)
  return parseInt(downPayment + loanAmount + intPayable);
}