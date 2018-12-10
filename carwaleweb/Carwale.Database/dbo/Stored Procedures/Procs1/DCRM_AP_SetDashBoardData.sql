IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_AP_SetDashBoardData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_AP_SetDashBoardData]
GO

	
-- =============================================
-- Author:		Sunil M. Yadav
-- Create date: 04 feb 2016
-- Description:	call dashboard sp's.
-- =============================================
CREATE PROCEDURE DCRM_AP_SetDashBoardData 
	
AS
BEGIN
	EXEC DCRM_DB_DumpRevenueData
	EXEC DCRM_DB_GetNewPaidDealers
	EXEC DCRM_DB_SetRenewalsData

END
