IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_BookingExpDateChange]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_BookingExpDateChange]
GO

	

--Name of SP/Function				: CarWale.dbo.CRM_BookingExpDateChange
--Applications using SP				: CRM 
--Modules using the SP				: \CRM\Forms\Code\(TaskApprovalList.cs,ApprovalList.cs)
--Business department				: CRM
--Technical department				: Database
--Modification history				: 1. Dilip V. 30-Jan-2012 (Validation of dupicate record before insert by select statement)

CREATE PROCEDURE [dbo].[CRM_BookingExpDateChange]		
	@CBDId					Numeric,	
	@BookExpDate			DateTime
				
 AS

BEGIN
	SET NOCOUNT ON
	DECLARE @NumberRecords AS INT
	SELECT CBDID FROM CRM_CarBookingCalendar WHERE CBDID = @CBDId
	SET @NumberRecords = @@ROWCOUNT
	IF(@NumberRecords = 0)
		BEGIN
			INSERT INTO CRM_CarBookingCalendar
			(
				CBDID, BookingExpectedDate				
			)
			VALUES
			(
				@CBDId, @BookExpDate
			)			
		END	
	ELSE
		BEGIN
			UPDATE CRM_CarBookingCalendar
			SET BookingExpectedDate = @BookExpDate
			WHERE CBDID = @CBDId
			
		END
	END


