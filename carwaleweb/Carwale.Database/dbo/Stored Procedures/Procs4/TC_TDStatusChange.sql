IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDStatusChange]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDStatusChange]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 21/6/2012
-- Description: To change status of Testdrive. TestDrive status are Tentative,Confirmed,Cancelled.
-- TC_TDChangeStatus 5 ,64,3
-- Modified By: Tejashree Patil on 11 Dec 2012 updated TDStatusDate field when status changes
-- Modified By : Tejashree Patil on 13 Jul 2013, Added @EventCreatedOn to insert actual date of td given in imported excel.
-- Modified By : Manish on 26-09-2013 for capturing every change in TC_CalendarLog table and caturing TD request date and TD status entry date in TC_NewcarInquiries table
-- Modified By : Vivek Singh on 17-02-2014 for capturing TDaddress in TC_CalendarLog table
-- Modified By : Manish on 12-03-2014  added keyword WITH (NOLOCK) in all Select queries
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDStatusChange]
	-- Add the parameters for the stored procedure here
	@BranchId BIGINT,
	@TC_TDCalendarId BIGINT,
	@TDStatus TINYINT,
	@EventCreatedOn DATETIME=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET @EventCreatedOn = ISNULL(@EventCreatedOn,GETDATE())-- Modified By : Tejashree Patil on 13 Jul 2013,
	SET NOCOUNT ON;
	IF (@TDStatus=39)	
	BEGIN 
		IF EXISTS(SELECT TOP 1 TC_TDCalendarId FROM TC_TDCalendar WITH(NOLOCK)
		WHERE TC_TDCalendarId=@TC_TDCalendarId AND GETDATE()>(cast ((cast(TDDate as varchar) + ' ' + cast (TDStartTime as varchar)) as datetime)))
		BEGIN
			RETURN 2
		END
	END
	
	IF (@TDStatus=28)	
	BEGIN 
		IF EXISTS(SELECT TOP 1 TC_TDCalendarId FROM TC_TDCalendar WITH(NOLOCK)
		WHERE TC_TDCalendarId=@TC_TDCalendarId AND GETDATE()<(cast ((cast(TDDate as varchar) + ' ' + cast (TDEndTime as varchar)) as datetime)))
		BEGIN
			RETURN 3
		END
	END	
	
	-- Update TDStatusDate when TDStatus changes
	-- Modified By: Tejashree Patil on 11 Dec 2012
	IF EXISTS(SELECT TOP 1 TC_TDCalendarId  FROM TC_TDCalendar WITH(NOLOCK) WHERE TC_TDCalendarId=@TC_TDCalendarId AND BranchId=@BranchId)
	BEGIN
	
		UPDATE TC_TDCalendar SET TDStatus=@TDStatus, 
		                         TDStatusDate = GETDATE()
		 WHERE TC_TDCalendarId=@TC_TDCalendarId 
		   AND BranchId=@BranchId
		
------------------------------------------------------------------------------------------------------------------------------------
		--below insert statement added  by Manish on 26-09-2013 since for every insert we need to insert record in TC_TDCalendarLog table.
		INSERT INTO TC_TDCalendarLog (TC_TDCalendarId, 
				                              BranchId, 
											  TC_CustomerId,
											  TC_TDCarsId, 
											  TDCarDetails, 
											  AreaName, 
											  ArealId, 
											  TC_UsersId,   
                                              TC_SourceId, 
											  TDDate, 
											  TDStartTime, 
											  TDEndTime, 
											  TDStatus, 
											  EntryDate, 
											  TDDriverId,
											  TDStatusDate, 
											  Comments, 
											  ModifiedDate, 
											  ModifiedBy,
											  TC_NewCarInquiriesId,   -- This column added by manish on 26-09-2013
											  TDAddress)  -- This column added by Vivek Singh on 17-02-2014
				                   SELECT     TC_TDCalendarId, 
								              BranchId, 
											  TC_CustomerId,
											  TC_TDCarsId, 
											  TDCarDetails, 
											  AreaName, 
											  ArealId, 
											  TC_UsersId,   
				                              TC_SourceId, 
											  TDDate, 
											  TDStartTime, 
											  TDEndTime, 
											  TDStatus, 
											  GETDATE(), 
											  TDDriverId,
											  TDStatusDate, 
											  Comments, 
											  ModifiedDate, 
											  ModifiedBy,
											  TC_NewCarInquiriesId,
											  TDAddress
				FROM TC_TDCalendar WITH (NOLOCK)  
				WHERE TC_TDCalendarId = @TC_TDCalendarId   


------------------------------------------------------------------------------------------------------------------------------------
		
 		
		DECLARE @InquiriesLeadId BIGINT
		DECLARE @InqId BIGINT
		DECLARE @LeadId BIGINT 
		DECLARE @UserId BIGINT 
		
		SELECT @InquiriesLeadId = TC_InquiriesLeadId,@InqId = TC_NewCarInquiriesId FROM TC_NewCarInquiries WITH(NOLOCK)  WHERE TC_TDCalendarId = @TC_TDCalendarId
		SELECT @LeadId = TC_LeadId, @UserId = TC_UserId FROM TC_InquiriesLead WITH(NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId

		UPDATE TC_NewCarInquiries set TDStatus=@TDStatus, 
		                              TDDate = @EventCreatedOn ,
									  TDStatusEntryDate=GETDATE()  ---- this column added by manish on 26-09-2013
		where TC_TDCalendarId=@TC_TDCalendarId 
		-- Modified By : Tejashree Patil on 13 Jul 2013,TDDate = @EventCreatedOn instead of GETDATE()	
		
		------------------below if block added by manish on 26-09-2013 since when td completed event created on shoul be equal to system date
		IF @TDStatus =28
		BEGIN 
		SET @EventCreatedOn=GETDATE()
		END 
		------------------------------------------------------------------------------------------------------------------------------------

		EXEC TC_DispositionLogInsert @UserId,@TDStatus,@InqId,5,@LeadId, @EventCreatedOn
	
	END
	
	RETURN 1
END


