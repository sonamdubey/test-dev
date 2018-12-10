IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetEmiDetails_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetEmiDetails_15]
GO

	
-- Modified By : Shikhar on 19.01.2015
-- Modified : Added @ShowEmi parameter as a indicator for Showing the EMIs

CREATE    PROCEDURE [dbo].[GetEmiDetails_15.1.1]
	@InquiryId		NUMERIC(18,0),
	@StockId NUMERIC OUTPUT, 
	@EMI INT OUTPUT,
	@LoanAmount INT OUTPUT,
	@InterestRate FLOAT OUTPUT,
	@Tenure SMALLINT OUTPUT,
	@OtherCharges INT OUTPUT,
	@DownPayment INT OUTPUT,
	@LoanToValue INT OUTPUT,
	@ShowEmi BIT OUTPUT, -- Modified by Shikhar on 19.91.2015
	@Price	DECIMAL(18, 0)	OUTPUT

 AS
	BEGIN

	    
		
		SELECT TOP 1 @StockId=TC_StockId,@Price=Price FROM SellInquiries WITH (NOLOCK) WHERE ID = @InquiryId
		
		
			If @StockId IS NOT NULL
			BEGIN

			SELECT   @EMI = tcst.EMI
					,@LoanAmount = tcst.LoanAmount
					,@InterestRate = tcst.InterestRate
					,@Tenure = tcst.Tenure
					,@OtherCharges = tcst.OtherCharges 
					,@LoanToValue = tcst.LoanToValue
					,@DownPayment = (@Price - tcst.LoanAmount)
					,@ShowEmi = tcst.ShowEMIOnCarwale
			FROM TC_Stock tcst WITH(NOLOCK)
			WHERE tcst.ShowEMIOnCarwale = 1
			AND tcst.Id = @StockId

		END
	END



