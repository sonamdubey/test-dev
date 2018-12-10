IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetDealerAutoInspectionFlag]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetDealerAutoInspectionFlag]
GO

	-- =============================================
-- Author:		Yuga Hatolkar
-- Create date: Aug 17,2015
-- Description:	Get Dealer Auto Inspection Flag.
-- EXEC AbSure_GetDealerAutoInspectionFlag 611547
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetDealerAutoInspectionFlag] 		
		@DealerId BIGINT		
AS
BEGIN
	
	SELECT ISNULL(AutoInspection,0) AutoInspection FROM Dealers WITH(NOLOCK)
	WHERE ID = @DealerId
END