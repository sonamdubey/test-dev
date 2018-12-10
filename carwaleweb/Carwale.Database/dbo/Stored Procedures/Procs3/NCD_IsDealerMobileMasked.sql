IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_IsDealerMobileMasked]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_IsDealerMobileMasked]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(19th Aug 2014)
-- Description	:	Check is Dealer mobile number is masked or not
-- Exec [dbo].[NCD_IsDealerMobileMasked] 3797,null,null
-- =============================================
CREATE PROCEDURE [dbo].[NCD_IsDealerMobileMasked] 
	@DealerID	INT,
	@MaskedNumber	VARCHAR(50) OUTPUT,
	@IsFind		SMALLINT OUTPUT
AS
BEGIN
	SET @IsFind = 0
	SET @MaskedNumber = 0
    SELECT MaskingNumber FROM MM_SellerMobileMasking WHERE ConsumerId = @DealerID
	IF @@ROWCOUNT <> 0
		SET @IsFind = 1
END
