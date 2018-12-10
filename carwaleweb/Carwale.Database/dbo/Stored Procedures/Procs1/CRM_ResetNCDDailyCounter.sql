IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ResetNCDDailyCounter]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ResetNCDDailyCounter]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 22 NOV 2013
-- Description: To reset the Daily Delivered Leads to 0 for both NCD and NCS dealers
-- ============================================
CREATE PROCEDURE [dbo].[CRM_ResetNCDDailyCounter]
AS
BEGIN
	
		UPDATE NCS_Dealers SET DailyDel = 0 WHERE IsNCDDealer = 1
		UPDATE NCD_Dealers SET DailyDel = 0 
END

