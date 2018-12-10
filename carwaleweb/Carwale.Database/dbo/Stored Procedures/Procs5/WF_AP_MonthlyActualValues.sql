IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_AP_MonthlyActualValues]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_AP_MonthlyActualValues]
GO

	CREATE PROCEDURE [dbo].[WF_AP_MonthlyActualValues]
	
 AS

BEGIN

	
	------------------------ UPDATE DATA FROM Individual & Dealer Classified ---------
	--Fake%
	Print 1
	
	------------------------ END UPDATE DATA FROM Individual & Dealer Classified ------

END