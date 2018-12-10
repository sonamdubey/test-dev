IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[DCRM].[UpdateDealerPlanPrintLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [DCRM].[UpdateDealerPlanPrintLog]
GO

	


-- Description	:	Update CourierNo, CourierDate WHERE ID
-- Author		:	Dilip V. 30-Mar-2012
-- Modifier		:	1.Dilip V. 26-Apr-2012 (Converted Bigint to varchar for courier no)
CREATE PROCEDURE [DCRM].[UpdateDealerPlanPrintLog]
	@Id				INT,
	@CourierNo		VARCHAR(100),
	@CourierDate	DATETIME = NULL
AS
BEGIN
	SET NOCOUNT ON	
	
	UPDATE DCRM.DealerPlanPrintLog 
	SET CourierNo = @CourierNo , CourierDate = @CourierDate
	WHERE Id = @Id
	
END




