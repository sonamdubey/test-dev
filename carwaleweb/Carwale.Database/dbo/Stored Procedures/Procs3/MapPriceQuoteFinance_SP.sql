IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MapPriceQuoteFinance_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MapPriceQuoteFinance_SP]
GO

	

CREATE  Procedure MapPriceQuoteFinance_SP
	@NewCarInquiryId	NUMERIC,
	@LoanId			NUMERIC
AS
	INSERT INTO MapNewCarInqFinance(NewCarInquiryId, LoanId) VALUES(@NewCarInquiryId, @LoanId)


