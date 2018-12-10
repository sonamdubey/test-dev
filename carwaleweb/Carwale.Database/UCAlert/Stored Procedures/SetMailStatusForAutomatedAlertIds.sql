IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[SetMailStatusForAutomatedAlertIds]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[SetMailStatusForAutomatedAlertIds]
GO

	-- =============================================
-- Author:		Shikhar
-- Create date: 17-06-2013
-- Description:	Set Alert Id as ismailed =1 
-- exec [UCAlert].[SetMailStatusForAutomatedAlertIds] '1,2,3,4,5'
---Modified by Manish on 22-07-2013 data type changed for correcting is_mailed update 
-- =============================================
CREATE PROCEDURE [UCAlert].[SetMailStatusForAutomatedAlertIds](@AlertIds VARCHAR(MAX))  --VARCHAR(1000) ---Modified by Manish on 22-07-2013 data type changed for correcting is_mailed update 
 AS
BEGIN
   
    UPDATE U
    SET    U.Is_Mailed=1
    FROM UCAlert.[RecommendUsedCarAlert] as U
    join UCAlert.UserCarAlerts AS UCA  ON U.CustomerId=UCA.CustomerId
    join [dbo].[fnSplitCSV](@AlertIds) as f on  UCA.UsedCarAlert_Id =f.ListMember
    WHERE U.Is_Mailed=0
    AND (UCA.EntryDateTime=CONVERT(DATE,GETDATE()-4) OR UCA.EntryDateTime=CONVERT(DATE,GETDATE()))

END



