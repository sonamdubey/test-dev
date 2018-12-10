IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[SetMailStatusForWeeklyAlertIds]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[SetMailStatusForWeeklyAlertIds]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 12-06-2012
-- Description:	Set Alert Id as is mailed =1 
-- exec [UCAlert].[SetMailStatusForWeeklyAlertIds] '1,2,3,4,5'
---Modified by Manish on 22-07-2013 data type changed for correcting is_mailed update 
-- =============================================
CREATE PROCEDURE [UCAlert].[SetMailStatusForWeeklyAlertIds](@AlertIds VARCHAR(MAX)) --VARCHAR(1000)---Modified by Manish on 22-07-2013 data type changed for correcting is_mailed update 
AS
BEGIN
   
    UPDATE UCAlert.WeeklyAlerts
    SET Is_Mailed=1
    FROM UCAlert.WeeklyAlerts as U
    --WHERE U.UsedCarAlert_Id =@AlertId
    join [dbo].[fnSplitCSV](@AlertIds) as f on  U.UsedCarAlert_Id =f.ListMember
    

END
/****** Object:  StoredProcedure [UCAlert].[SetMailStatusForAutomatedAlertIds]    Script Date: 07/22/2013 15:08:06 ******/
SET ANSI_NULLS ON
