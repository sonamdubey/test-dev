IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_UpdateDealerLoanAmounts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_UpdateDealerLoanAmounts]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 31 Oct 2014
-- Description:	Proc to update the EMI for the given dealer id
-- exec BW_UpdateDealerLoanAmounts 4, 24, 12.2, 80

--Modified By : Suresh Prajapati on 02nd Dec 2014, To update Loan Provider's Name
-- =============================================
CREATE PROCEDURE [dbo].[BW_UpdateDealerLoanAmounts]
	-- Add the parameters for the stored procedure here
	@DealerId INT
	,@Tenure TINYINT
	,@RateOfInterest VARCHAR(20)
	,@LTV TINYINT
	,@LoanProvider VARCHAR (100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	UPDATE BW_DealerLoanAmounts
	SET LTV = @LTV
		,RateOfInterest = @RateOfInterest
		,Tenure = @Tenure
		,LoanProvider = @LoanProvider
	WHERE DealerId = @DealerId
END

