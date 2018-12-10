IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[DCRM].[InsertDealerPlanPrintLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [DCRM].[InsertDealerPlanPrintLog]
GO

	
-- Description	:	Insert Dealer ID, CreatedBy, ProductType, ExpiryDate
-- Author		:	Dilip V. 16-Mar-2012
-- Modifier		:	
CREATE PROCEDURE [DCRM].[InsertDealerPlanPrintLog]
	@DealerID NUMERIC(18,0),
	@CreatedBy NUMERIC(18,0),
	@ProductType NUMERIC(18,0),
	@ExpiryDate DATETIME
AS
BEGIN
	SET NOCOUNT ON			

	INSERT INTO DCRM.DealerPlanPrintLog
			(DealerId,CreatedOn, CreatedBy,ProductType,ExpiryDate)
	VALUES (@DealerID,GETDATE(),@CreatedBy,@ProductType,@ExpiryDate)
	
END

