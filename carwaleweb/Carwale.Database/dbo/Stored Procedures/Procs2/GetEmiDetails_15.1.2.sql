IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetEmiDetails_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetEmiDetails_15]
GO

	
-- Modified By : Shikhar on 19.01.2015
-- Modified : Added @ShowEmi parameter as a indicator for Showing the EMIs

CREATE    PROCEDURE [dbo].[GetEmiDetails_15.1.2]
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
	@Price	DECIMAL(18, 0)	OUTPUT,
	-- Added by Kirtan Shetty on 27 January 2015
	@AbsureCarScore INT OUTPUT,					
	@AbsureId INT OUTPUT,						
	@AbsureCertificateUrl VARCHAR(100) OUTPUT,			
	@DealerCertificateId INT OUTPUT,					
	@FinalWarrantyType VARCHAR(100) OUTPUT

 AS
	BEGIN    
		
		SELECT TOP 1 @StockId=TC_StockId,@Price=Price FROM SellInquiries WITH (NOLOCK) WHERE ID = @InquiryId
		
		
		IF @StockId IS NOT NULL
		BEGIN
			SELECT   
				@EMI = TCST.EMI
				,@LoanAmount = TCST.LoanAmount
				,@InterestRate = TCST.InterestRate
				,@Tenure = TCST.Tenure
				,@OtherCharges = TCST.OtherCharges 
				,@LoanToValue = TCST.LoanToValue
				,@DownPayment = (@Price - TCST.LoanAmount)
				,@ShowEmi = TCST.ShowEMIOnCarwale
				-- Added by Kirtan Shetty on Jan 27
				,@AbsureCarScore = ABC.CarScore																
				,@AbsureId = ABC.Id																			
				,@AbsureCertificateUrl = 'http://www.autobiz.in/absure/CarCertificate.aspx?carId=' + CONVERT(VARCHAR, ABC.Id)
				,@DealerCertificateId = TCST.CertificationId												
				,@FinalWarrantyType = ABW.Warranty	
			FROM TC_Stock TCST WITH(NOLOCK)
				LEFT JOIN AbSure_CarDetails ABC WITH(NOLOCK)
					ON ABC.StockId = TCST.Id
				LEFT JOIN AbSure_WarrantyTypes ABW WITH (NOLOCK)
					ON ABC.FinalWarrantyTypeId = ABW.AbSure_WarrantyTypesId
			WHERE-- Removed the condition to show EMI value by Shikhar on Feb 2, 2015
				TCST.Id = @StockId
		END
	END



