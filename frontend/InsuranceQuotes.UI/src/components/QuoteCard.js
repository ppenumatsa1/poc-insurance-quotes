import React from 'react';

const QuoteCard = ({ quote }) => {
  return (
    <div className="quote-card">
      <h3>{quote.customerName}</h3>
      <div className="quote-detail">
        <strong>Vehicle:</strong> {quote.vehicleYear} {quote.vehicleMake} {quote.vehicleModel}
      </div>
      <div className="quote-detail">
        <strong>Coverage:</strong> {quote.coverageType}
      </div>
      <div className="quote-detail">
        <strong>Monthly Premium:</strong> ${quote.monthlyPremium.toFixed(2)}
      </div>
      <div className="quote-detail">
        <strong>Deductible:</strong> ${quote.deductible.toFixed(2)}
      </div>
      <div className="quote-detail">
        <strong>Policy Term:</strong> {quote.policyTerm}
      </div>
      <div className="quote-detail">
        <strong>Coverage Limits:</strong>
        <div>Bodily Injury: {quote.coverageLimits.bodilyInjury}</div>
        <div>Property Damage: {quote.coverageLimits.propertyDamage}</div>
      </div>
    </div>
  );
};

export default QuoteCard;