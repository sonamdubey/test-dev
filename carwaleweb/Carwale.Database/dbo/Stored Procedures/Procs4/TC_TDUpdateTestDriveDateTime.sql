IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDUpdateTestDriveDateTime]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDUpdateTestDriveDateTime]
GO

	-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date,27-08-2013>
-- Description:	<Description,Update only date and time for a particular testdrive booked through Android app.>
-- Modified By : Vivek Singh on 17-02-2014 for capturing TDaddress in TC_CalendarLog table
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDUpdateTestDriveDateTime]
	-- Add the parameters for the stored procedure here
	@BranchId INT,
	@TC_UserId INT, -- logged in users Id
	@TC_TDCalendarId INT,
	@TC_InquiryId INT,
	@TDDate DATE,
	@TDStartTime TIME,
	@TDEndTime TIME,
	@IsComplete TINYINT,
	@Status SMALLINT OUTPUT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @OldTDDate DATE, 
			@OldTDStartTime TIME, 
			@OldTDEndTime TIME,
			@LeadId INT,
			@TDConsultant INT,
			@TC_TDCarsId INT,
			@TC_TDDriverId INT
	
	SELECT @OldTDDate		= TDDate, 
		   @OldTDStartTime  = TDStartTime, 
		   @OldTDEndTime	= TDEndTime,
		   @TDConsultant	= TC_UsersId,
		   @TC_TDCarsId		= TC_TDCarsId,
		   @TC_TDDriverId	= TDDriverId
	FROM TC_TDCalendar 
	WHERE TC_TDCalendarId = @TC_TDCalendarId
	
	-- check if the driver, consultant and car are free 
	SELECT TOP 1 @Status = (CASE WHEN  TC_UsersId=@TDConsultant THEN 2 
								 WHEN  TC_TDCarsId=@TC_TDCarsId AND TC_UsersId=@TDConsultant THEN 3
								 WHEN  TC_TDCarsId=@TC_TDCarsId THEN 4
								 WHEN  TC_TDCarsId=@TC_TDCarsId AND TDDriverId=@TC_TDDriverId THEN 6
								 WHEN  TC_UsersId=@TDConsultant AND TDDriverId=@TC_TDDriverId THEN 7		
								 WHEN  TC_UsersId=@TDConsultant AND TDDriverId=@TC_TDDriverId AND TC_TDCarsId=@TC_TDCarsId THEN 8		                           
								 WHEN  TDDriverId=@TC_TDDriverId THEN 9
							ELSE 0 END)			
				FROM TC_TDCalendar TDC  WITH (NOLOCK) WHERE  (TC_TDCarsId=@TC_TDCarsId Or TC_UsersId=@TDConsultant OR TDDriverId=@TC_TDDriverId )
					 AND TDC.TDDate =@TDDate  
					 AND    
						(
							(@TDStartTime >= TDC.TDStartTime AND @TDStartTime <TDC.TDEndTime)
						OR 
							(@TDEndTime > TDC.TDStartTime AND @TDEndTime <= TDC.TDEndTime)
						OR
							(TDC.TDStartTime >= @TDStartTime AND TDC.TDStartTime <@TDEndTime)
						OR
							(TDC.TDEndTime > @TDStartTime AND TDC.TDEndTime <= @TDEndTime)
						)
					 AND TC_TDCalendarId<>@TC_TDCalendarId AND TDC.TDStatus<>27
					 				 
	IF(@Status <> 0)
	BEGIN
		RETURN -1
	END
			
	SELECT @LeadId = L.TC_LeadId FROM TC_NewCarInquiries N 
												 JOIN TC_InquiriesLead L 
												 ON N.TC_InquiriesLeadId = L.TC_InquiriesLeadId
								 WHERE TC_NewCarInquiriesId = @TC_InquiryId
	
			
	--IF(@TDDate <> @OldTDDate OR @TDStartTime <> @OldTDStartTime OR @TDEndTime <> @OldTDEndTime)
	--BEGIN -- As the details are being updated Insert the old data in log table
		
	--END
	
	UPDATE TC_TDCalendar SET TDDate=@TDDate, 
							 TDStartTime=@TDStartTime, 
							 TDEndTime=@TDEndTime, 
							 TDStatus=29,
							 TDStatusDate=GETDATE(),
           					 ModifiedDate=GETDATE(), 
							 ModifiedBy=@TC_UserId 
						 WHERE TC_TDCalendarId=@TC_TDCalendarId 


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
									  TC_NewCarInquiriesId,
									  TDAddress)  
									  SELECT	TC_TDCalendarId, 
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
						  
	UPDATE TC_NewCarInquiries SET TDDate= CAST(@TDDate AS DATETIME) + CAST(@TDStartTime AS DATETIME) ,
	                              TDStatus=29,
								  TDStatusEntryDate=GETDATE()
							  WHERE TC_TDCalendarId = @TC_TDCalendarId
	
	DECLARE @DateTimeNow DATETIME = GETDATE()						  
	EXEC TC_DispositionLogInsert @TC_UserId,29,@TC_InquiryId,5,@LeadId,@DateTimeNow -- Insert record in to log table for filling activity feed
	
	IF(@IsComplete = 1)-- Change status to completed when booking TD for past date time
	BEGIN
		EXEC TC_TDStatusChange @BranchId,@TC_TDCalendarId,28,@DateTimeNow  
	END
	
	SET @Status = 1
END



/****** Object:  StoredProcedure [dbo].[TC_TestDriveBookingDisposition]    Script Date: 2/17/2014 6:04:25 PM ******/
SET ANSI_NULLS ON
