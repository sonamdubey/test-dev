IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[SetMailStatusForDailyAlertIds]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[SetMailStatusForDailyAlertIds]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 12-06-2012
-- Description:	Set Alert Id as is mailed =1 
-- exec [UCAlert].[SetMailStatusForWeeklyAlertIds] '1,2,3,4,5'
---Modified by Manish on 22-07-2013 data type changed for correcting is_mailed update 
-- =============================================
CREATE PROCEDURE [UCAlert].[SetMailStatusForDailyAlertIds](@AlertIds VARCHAR(MAX))  --VARCHAR(1000)---Modified by Manish on 22-07-2013 data type changed for correcting is_mailed update 
AS
BEGIN
   
    UPDATE UCAlert.DailyAlerts
    SET Is_Mailed=1
    FROM UCAlert.DailyAlerts as U
    --WHERE U.UsedCarAlert_Id =@AlertId
    join [dbo].[fnSplitCSV](@AlertIds) as f on  U.UsedCarAlert_Id =f.ListMember
    

END

/****** Object:  StoredProcedure [UCAlert].[SetMailStatusForWeeklyAlertIds]    Script Date: 07/22/2013 15:07:45 ******/
SET ANSI_NULLS ON
