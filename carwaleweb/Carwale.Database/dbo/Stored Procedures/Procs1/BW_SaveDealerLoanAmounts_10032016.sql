IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_SaveDealerLoanAmounts_10032016]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_SaveDealerLoanAmounts_10032016]
GO

	-- =============================================
--	Author:		Ashish G. Kamble
--	Create date: 31 Oct 2014
--	Description:	Proc to save the EMI for the given dealer id
--	Modified By : Suresh Prajapati on 02nd Dec 2014
--	Description : Added "LoanProvider" To save Loan Provider's Name.
--	Modified By	:	Sumit Kate on 10 Mar 2016
--	Description	:	If ID is passed then updates existing record else
--					the inserts new row data
--					Down Payment(min-max)
--					Tenure(min-max)
--					Rate of Interest(min-max)
--					Processing Fee
--	Modified by :Sangram Nandkhile on 14 March 2016
-- Description	: Added minLtv and maxLtv
-- =============================================
CREATE PROCEDURE [dbo].[BW_SaveDealerLoanAmounts_10032016]
	-- Add the parameters for the stored procedure here
	@DealerId INT
	--,@Tenure TINYINT
	--,@RateOfInterest VARCHAR(20)
	--,@LTV TINYINT
	,@LoanProvider VARCHAR(100)
	,@UserId INT
	,@minDownPayment FLOAT = NULL
	,@maxDownPayment FLOAT = NULL
	,@minTenure INT = NULL
	,@maxTenure INT = NULL
	,@minRateOfInterest FLOAT = NULL
	,@maxRateOfInterest FLOAT = NULL
	,@minLtv FLOAT = NULL
	,@maxLtv FLOAT = NULL
	,@processingFee FLOAT = NULL
	,@ID INT = NULL	
AS
BEGIN
	IF @ID IS NULL
		BEGIN
			-- Insert statements for procedure here
			INSERT INTO BW_DealerLoanAmounts (
				DealerId
				--,LTV
				--,RateOfInterest
				--,Tenure
				,LoanProvider
				,UserId
				,MinDownPayment
				,MaxDownPayment
				,MinTenure
				,MaxTenure
				,MinRateOfInterest
				,MaxRateOfInterest
				,minLtv
				,maxLtv
				,ProcessingFee
				)
			VALUES (
				@DealerId
				--,@LTV
				--,@RateOfInterest
				--,@Tenure
				,@LoanProvider
				,@UserId
				,@MinDownPayment
				,@MaxDownPayment
				,@MinTenure
				,@MaxTenure
				,@MinRateOfInterest
				,@MaxRateOfInterest
				,@minLtv
				,@maxLtv
				,@ProcessingFee
				)
		END
	ELSE
		BEGIN
			UPDATE BW_DealerLoanAmounts
			SET --LTV = COALESCE(@LTV,LTV)
				--,RateOfInterest = COALESCE(@RateOfInterest,RateOfInterest)
				--,Tenure	= COALESCE(@Tenure,Tenure)
				 minLtv = COALESCE(@minLtv,minLtv)
				,maxLtv = COALESCE(@maxLtv,maxLtv)
				,LoanProvider = COALESCE(@LoanProvider,LoanProvider)
				,MinDownPayment = ISNULL(@MinDownPayment,MinDownPayment)
				,MaxDownPayment	= ISNULL(@MaxDownPayment,MaxDownPayment)
				,MinTenure = ISNULL(@MinTenure,MinTenure)
				,MaxTenure = ISNULL(@MaxTenure,MaxTenure)
				,MinRateOfInterest = ISNULL(@MinRateOfInterest,MinRateOfInterest)
				,MaxRateOfInterest = ISNULL(@MaxRateOfInterest,MaxRateOfInterest)
				,ProcessingFee = ISNULL(@ProcessingFee,ProcessingFee)
			WHERE Id = @ID
		END
END