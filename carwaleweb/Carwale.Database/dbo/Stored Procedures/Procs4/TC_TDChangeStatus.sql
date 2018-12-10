IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDChangeStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDChangeStatus]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 21/6/2012
-- Description: To change status of Testdrive. TestDrive status are Tentative,Confirmed,Cancelled.
-- TC_TDChangeStatus 5 ,64,3
-- Modified By: Tejashree Patil on 11 Dec 2012 updated TDStatusDate field when status changes
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDChangeStatus]
	-- Add the parameters for the stored procedure here
	@BranchId BIGINT,
	@TC_TDCalendarId BIGINT,
	@TDStatus TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF (@TDStatus=2)	
	BEGIN 
		IF EXISTS(SELECT TOP 1 TC_TDCalendarId FROM TC_TDCalendar 
		WHERE TC_TDCalendarId=@TC_TDCalendarId AND GETDATE()>(cast ((cast(TDDate as varchar) + ' ' + cast (TDStartTime as varchar)) as datetime)))
		BEGIN
			RETURN 2
		END
	END
	
	IF (@TDStatus=3)	
	BEGIN 
		IF EXISTS(SELECT TOP 1 TC_TDCalendarId FROM TC_TDCalendar 
		WHERE TC_TDCalendarId=@TC_TDCalendarId AND GETDATE()<(cast ((cast(TDDate as varchar) + ' ' + cast (TDEndTime as varchar)) as datetime)))
		BEGIN
			RETURN 3
		END
	END	
	
	-- Update TDStatusDate when TDStatus changes
	-- Modified By: Tejashree Patil on 11 Dec 2012
	UPDATE TC_TDCalendar SET TDStatus=@TDStatus, TDStatusDate = GETDATE()
	WHERE TC_TDCalendarId=@TC_TDCalendarId AND BranchId=@BranchId	
   
	RETURN 1
END
