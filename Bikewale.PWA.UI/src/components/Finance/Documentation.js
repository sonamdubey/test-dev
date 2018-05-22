import React from 'react';

const Documentation = () => {
  return (
    <div className="finance-documentation-container">
      <div className="finance-documentation__head">
        <h2 className="finance-documentation__title">Documentation for Loans</h2>
      </div>
      <ul className="finance-documentation__list">
        <li className="documentation-list__item photo-identity">
          <div className="documentation-list-item__head documentation-item__photo-identity">
            <span className="documentation-list-head__icon documentation-icon__photo"></span>
            <span className="documentation-list-head__title">Photo Identity</span>
          </div>
          <div className="documentation-list-item__body">
            <p>Passport-size photographs, at least 2 to 6 are recommended to be kept handy for your loan application.</p>
          </div>
        </li>

        <li className="documentation-list__item identity-proof">
          <div className="documentation-list-item__head">
            <span className="documentation-list-head__icon documentation-icon__identity"></span>
            <span className="documentation-list-head__title">Identity Proof</span>
          </div>
          <div className="documentation-list-item__body">
              <span className="documentation-item__list-head">Any one of the following:</span>
              <ul className="documentation-item__list">
                <li>Passport</li>
                <li>Electoral Voter Identity card</li>
                <li>Driving License</li>
                <li>PAN Card</li>
                <li>Aadhar Card</li>
              </ul>
          </div>
        </li>

        <li className="documentation-list__item address-proof">
          <div className="documentation-list-item__head identity-proof">
            <span className="documentation-list-head__icon documentation-icon__address"></span>
            <span className="documentation-list-head__title">Address Proof</span>
          </div>
          <div className="documentation-list-item__body">
              <span className="documentation-item__list-head">Permanent address proof: </span>
              <ul className="documentation-item__list">
                <li>PAN card</li>
                <li>Driving License </li>
              </ul>
              <span className="documentation-item__list-head list-head--padding-top">Temporary address proof: </span>
              <ul className="documentation-item__list">
                <li>Bills against your name</li>
              </ul>
          </div>
        </li>

        <li className="documentation-list__item employement-proof">
          <div className="documentation-list-item__head identity-proof">
            <span className="documentation-list-head__icon documentation-icon__employment"></span>
            <span className="documentation-list-head__title">Employment Proof</span>
          </div>
          <div className="documentation-list-item__body">
              <span className="documentation-item__list-head">For salaried employees:</span>
              <ul className="documentation-item__list">
                <li>Employment or offer letter</li>
                <li>Bank statements</li>
                <li>Salary slips</li>
              </ul>

              <span className="documentation-item__list-head list-head--padding-top">For self-employed individuals:</span>
              <ul className="documentation-item__list">
                <li>Latest Form 16</li>
                <li>Bank statements</li>
                <li>Proof of their source of income</li>
              </ul>
          </div>
        </li>

      </ul>
    </div>
  );
}

module.exports = Documentation;
