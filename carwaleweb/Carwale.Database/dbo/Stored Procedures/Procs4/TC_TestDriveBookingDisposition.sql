IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TestDriveBookingDisposition]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TestDriveBookingDisposition]
GO

	-- ==============================================================
-- Modified By : Nilesh Utture on 24th April, 2013 Added parameter @TC_UserId 
-- Modified By : Manish on 26-09-2013 for capturing every change in TC_CalendarLog table and caturing TD request date and TD status entry date in TC_NewcarInquiries table
-- Modified By : Vivek Singh on 17-02-2014 for capturing TDaddress in TC_CalendarLog table
-- ==============================================================
-- EXEC TC_changeBookingDisposition 30, 2  
CREATE PROCEDURE [dbo].[TC_TestDriveBookingDisposition]   
(  
@CalId BIGINT,  
@status SMALLINT,
@TC_UserId INT = NULL  -- userId
)  
AS  
BEGIN  

	DECLARE @InquiriesLeadId BIGINT
	DECLARE @InqId BIGINT
	DECLARE @LeadId BIGINT 
	DECLARE @UserId BIGINT -- Lead ownerId
	
	SELECT @InquiriesLeadId = TC_InquiriesLeadId,@InqId = TC_NewCarInquiriesId FROM TC_NewCarInquiries WITH (NOLOCK) WHERE TC_TDCalendarId = @CalId
    SELECT @LeadId = TC_LeadId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId
    
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;   
 DECLARE @TDDate  DATE  
 SELECT @TDDate = TDDate FROM TC_TDCalendar WITH (NOLOCK) WHERE TC_TDCalendarId = @CalId  
   
 IF(@status=2)  
  BEGIN  
   UPDATE TC_TDCalendar set TDStatus=27,
                      TDStatusDate=GETDATE()      --- Added by manish on 27-09-2013
					   where TC_TDCalendarId=@calId  
   
   UPDATE TC_NewCarInquiries set TDStatus=27,
           -- , TDDate = GETDATE()  --- commented by Manish on 27-09-2013 since we have to take tddate similar to TC_TDCalendar table
		   TDStatusEntryDate=GETDATE()
   WHERE TC_TDCalendarId=@calId  
   
   		
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
											  TC_NewCarInquiriesId,  -- This column added by manish on 26-09-2013
											  TDAddress)  -- -- This column added by Vivek Singh on 17-02-2014
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
				WHERE TC_TDCalendarId = @calId   


------------------------------------------------------------------------------------------------------------------------------------
		
   EXEC TC_DispositionLogInsert @TC_UserId,27,@InqId,5,@LeadId -- Modified By: Nilesh Utture on 24th April, 2013
  END   
 ELSE IF(@status=3)  
  BEGIN  
  IF EXISTS(SELECT TOP 1 TC_TDCalendarId FROM TC_TDCalendar 
		WHERE TC_TDCalendarId=@CalId AND GETDATE()<(cast ((cast(TDDate as varchar) + ' ' + cast (TDEndTime as varchar)) as datetime)))
		BEGIN
			RETURN 2
		END
   UPDATE TC_TDCalendar SET TDStatus=28,
                            TDStatusDate=GETDATE() --- Added by manish on 27-09-2013
                  WHERE TC_TDCalendarId=@calId  
   
   UPDATE TC_NewCarInquiries SET TDStatus=28,
                      --, TDDate = GETDATE()   --- commented by Manish on 27-09-2013 since we have to take tddate similar to TC_TDCalendar table
					  TDStatusEntryDate=GETDATE()
	WHERE TC_TDCalendarId=@calId  

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
											  TC_NewCarInquiriesId,    -- This column added by manish on 26-09-2013
											  TDAddress)        -- -- This column added by Vivek Singh on 17-02-2014
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
				WHERE TC_TDCalendarId = @calId   


------------------------------------------------------------------------------------------------------------------------------------


   EXEC TC_DispositionLogInsert @TC_UserId,28,@InqId,5,@LeadId -- Modified By: Nilesh Utture on 24th April, 2013
   RETURN 1  
  END  
   
END


