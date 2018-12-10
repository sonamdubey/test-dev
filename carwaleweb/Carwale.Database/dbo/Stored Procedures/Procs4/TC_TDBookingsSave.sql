IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDBookingsSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDBookingsSave]
GO

	-- =============================================  
-- Author:  Tejashree Patil on 7 Dec 2012 at 11 am  
-- Description: Return details report of scheduled testdrive request  
-- DECLARE @Status TINYINT , @TC_TDCalendarId BIGINT  
-- Modified by: Nilesh Utture on 27th Feb,2013 Added condition for driver
-- EXEC TC_TDBookingSave 'Varsha',5,'1524897656','asdertyu',1,'sion',22,1,NULL,1,'BMW 3 Series','2012-10-0211:00:00',  
-- '11:00:00','12:30:00',NULL,1,1,@Status OUTPUT,@TC_TDCalendarId OUTPUT  
-- SELECT @Status, @TC_TDCalendarId   
-- Modified by: Nilesh Utture on 24th April,2013 Used @ModifiedBy instead of @TDConsultant
-- Modified by: Umesh on 3 july 2013 added @IsComplete parameter for past date & time TD book & update it to completed
-- Modified By: Tejashree Patil on 9 July 2013 at 5 pm, Added parameter @IsFromexcel to identify that this TD request from Import inquiry from excel of VW.
-- Modified By: Tejashree Patil on 23 July 2013 TDStatus = 29 instead of TDStatus = 39
-- Modified by: Umesh on 23 july 2013 status variable assigned to fix value accroding to consultant,car & driver
-- Modified BY: Nilesh Utture on 5th September, 2013 Added inquiryId in TDCalendar TABLE to capture rebooking of TD Data
-- Modified By: Manish on 26-09-2013 for capturing every change in TC_CalendarLog table and caturing TD request date and TD status entry date in TC_NewcarInquiries table
-- Modified By: Vivek Singh 17-02-2014 for capturing Test Drive Address In the new Column TDADDRESS in TC_TDCalendar table presently  Test Drive Address is updated in Address Field in TC_CustomerDetails Table
-- Modified By: Manish on 12-03-2014 added keyword WITH (NOLOCK) in select queries
-- =============================================  
CREATE PROCEDURE [dbo].[TC_TDBookingsSave]  
	 @BranchId BIGINT,  
	 @CustId BIGINT,  
	 @InqId BIGINT,  
	 @Address VARCHAR(200),  
	 @Area VARCHAR(100),  
	 @AreaId BIGINT ,  
	 @TC_TDCarsId INT,  
	 @TDCarDetails VARCHAR(100),  
	 @TDDate DATE,  
	 @TDStartTime TIME,  
	 @TDEndTime TIME,  
	 @ModifiedBy BIGINT,  
	 @TDConsultant BIGINT,  
	 @TC_TDDriverId BIGINT,  
	 @Status TINYINT OUTPUT,  
	 @TDStatus SMALLINT,  
	 @SourceId TINYINT,  
	 @TC_TDCalendarId BIGINT = -1 OUTPUT,
	 @IsComplete TINYINT = 0,
     @IsFromExcel BIT = 0,
     @EventCreatedOn DATE = NULL
AS  
BEGIN 
	DECLARE @InquiriesLeadId BIGINT
	DECLARE @LeadId BIGINT 
	
	SELECT @InquiriesLeadId = TC_InquiriesLeadId FROM TC_NewCarInquiries WITH (NOLOCK) WHERE TC_NewCarInquiriesId = @InqId
    SELECT @LeadId = TC_LeadId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId
   
    SET @Status=0-- error occured     
     
     IF(@TC_TDDriverId = -1 OR @TC_TDDriverId = 0) -- Modified by: Nilesh Utture on 18th Jun,2013 
		SET @TC_TDDriverId = NULL
    			
    IF( @IsFromExcel=0)
    BEGIN
		IF(@TC_TDDriverId IS NULL )-- Modified by: Nilesh Utture on 27th Feb,2013 -- Modified By: Tejashree Patil on 9 July 2013 at 5 pm
		   BEGIN 
		   
		   SELECT TOP 1 @Status = (CASE WHEN  TC_UsersId=@TDConsultant  THEN 2
										WHEN  TC_TDCarsId=@TC_TDCarsId AND TC_UsersId=@TDConsultant THEN 3
										WHEN  TC_TDCarsId=@TC_TDCarsId  THEN 4			                           
										ELSE 0
										 END)
		   
			FROM TC_TDCalendar TDC  WITH (NOLOCK) WHERE  (TC_TDCarsId=@TC_TDCarsId Or TC_UsersId=@TDConsultant)
				AND 
			   TDC.TDDate =@TDDate  AND    
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
		   
		 
			END
		ELSE
			BEGIN
				SELECT TOP 1 @Status = (CASE WHEN  TC_UsersId=@TDConsultant  THEN 2
									WHEN  TC_TDCarsId=@TC_TDCarsId AND TC_UsersId=@TDConsultant THEN 3
									WHEN  TC_TDCarsId=@TC_TDCarsId  THEN 4
									WHEN  TC_TDCarsId=@TC_TDCarsId AND TDDriverId=@TC_TDDriverId THEN 6
									WHEN  TC_UsersId=@TDConsultant AND TDDriverId=@TC_TDDriverId THEN 7		
									WHEN  TC_UsersId=@TDConsultant AND TDDriverId=@TC_TDDriverId AND TC_TDCarsId=@TC_TDCarsId THEN 8		                           
									WHEN  TDDriverId=@TC_TDDriverId  THEN 9
									ELSE 0
									END)			
				FROM TC_TDCalendar TDC  WITH (NOLOCK) WHERE  (TC_TDCarsId=@TC_TDCarsId Or TC_UsersId=@TDConsultant OR TDDriverId=@TC_TDDriverId )
					AND 
				   TDC.TDDate =@TDDate  AND    
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
				
			END 
			
			IF(@Status <> 0)
			BEGIN
				RETURN -1
			END
    END
	 -- Insert record in TC_TDCalendarLog table  
	 -- Modified By: Tejashree Patil on 11 Dec 2012  
	IF (@TC_TDCalendarId<>-1)  
		BEGIN  
			DECLARE @OldTDDate DATE,@OldStartTime TIME, @OldEndTime TIME  
			SELECT @OldTDDate=TDDate ,@OldStartTime=TDStartTime , @OldEndTime=TDEndTime  
			FROM TC_TDCalendar WITH (NOLOCK)  WHERE TC_TDCalendarId=@TC_TDCalendarId  


			----below if condition commented by Manish on 26-09-2013 since for every update we need to insert record in TC_TDCalendarLog table.
			--IF(@OldTDDate<>@TDDate OR @OldStartTime<>@TDStartTime OR @OldEndTime<>@TDEndTime)  
			--BEGIN   
				
				 
			--END      
			-- Modified By: Tejashree Patil on 23 July 2013 TDStatus = 29 instead of TDStatus = 39
			UPDATE TC_TDCalendar SET  TC_TDCarsId=@TC_TDCarsId, 
			                          TDCarDetails=@TDCarDetails, 
									  TDDate=@TDDate, 
									  TDStatus = 29,
									  ArealId = @AreaId, 
									  AreaName = @Area,  
			                          TDStartTime=@TDStartTime, 
									  TDEndTime=@TDEndTime, 
									  ModifiedDate=GETDATE(), 
									  ModifiedBy=@ModifiedBy,  
			                          TC_UsersId=@TDConsultant,
									  TdDriverId=@TC_TDDriverId, -- Modified By: Tejashree Patil on 11 Dec 2012 
									  TDStatusDate=GETDATE(), -- this column added by manish on 26-09-2013
									  TDAddress=@Address 
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
				WHERE TC_TDCalendarId = @TC_TDCalendarId  


			
			UPDATE TC_NewCarInquiries SET TDStatus=29, 
			                              TDDate= CAST (@TDDate AS DATETIME) + CAST(@TDStartTime AS DATETIME),
										  TDStatusEntryDate= GETDATE() -- this column added by manish on 26-09-2013 
										   WHERE TC_TDCalendarId = @TC_TDCalendarId
			
			--UPDATE TC_CustomerDetails SET Address = @Address WHERE Id= @CustId  

			EXEC TC_DispositionLogInsert @ModifiedBy,29,@InqId,5,@LeadId,@EventCreatedOn -- Modified by: Nilesh Utture on 24th April,2013

			SET @Status=1-- sussesfully save        

		END  
	ELSE  
	BEGIN
		
		INSERT INTO TC_TDCalendar(BranchId, TC_CustomerId, AreaName, ArealId,  TC_TDCarsId, TDCarDetails, TDStatus,  
				  TDDate, TDStartTime, TDEndTime, TDStatusDate, ModifiedDate, ModifiedBy, TC_UsersId, TdDriverId, TC_SourceId, TC_NewCarInquiriesId,TDAddress)  
		VALUES(@BranchId,@CustId,@Area,@AreaId, @TC_TDCarsId,@TDCarDetails, 39,@TDDate,@TDStartTime,@TDEndTime,  GETDATE(), GETDATE(),   
			  @ModifiedBy, @TDConsultant,@TC_TDDriverId, @SourceId, @InqId,@Address)  -- Modified BY: Nilesh Utture on 5th September, 2013 Added inquiryId in INSERT 
		        
		SET @TC_TDCalendarId = SCOPE_IDENTITY()    


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
				WHERE TC_TDCalendarId = @TC_TDCalendarId   


------------------------------------------------------------------------------------------------------------------------------------

		IF(@TC_TDCalendarId<>-1)  
		BEGIN  
		UPDATE TC_TDCalendar SET TDAddress = @Address WHERE TC_TDCalendarId=@TC_TDCalendarId
			
			-- Modified by: Nilesh Utture on 27th Feb,2013 ADDed CAST (@TDDate AS DATETIME) + CAST(@TDStartTime AS DATETIME)
			UPDATE TC_NewCarInquiries SET TDStatus=39, 
			                              TDDate= CAST (@TDDate AS DATETIME) + CAST(@TDStartTime AS DATETIME), 
										  TC_TDCalendarId= @TC_TDCalendarId ,
										  TDStatusEntryDate= GETDATE() , -- this column added by manish on 26-09-2013 
										  TDRequestedDate=CAST (@TDDate AS DATETIME) + CAST(@TDStartTime AS DATETIME) ---- this column added by manish on 26-09-2013
			WHERE TC_NewCarInquiriesId = @InqId


			EXEC TC_DispositionLogInsert @ModifiedBy,39,@InqId,5,@LeadId,@EventCreatedOn -- Modified by: Nilesh Utture on 24th April,2013
			-- Modified By: Tejashree Patil on 9 July 2013 at 5 pm

		END  

		--SET @TC_TDCalendarId =@TC_TDCalendarId  --  Commented  by Manish on 27-09-2013

		SET @Status=1-- sussesfully save        
	END  
	
	-- Modified by: Umesh on 3 july 2013 added @IsComplete parameter for past date & time TD book & update it to completed
	-- Modified By: Tejashree Patil on 9 July 2013 passed @EventCreatedOn parameter to TC_TDStatusChange so td complete date is @TDDate for import inquiries.
	
	IF(@IsComplete = 1)
	BEGIN
	---- Event created on commented by manish  on 26-09-2013
		EXEC TC_TDStatusChange @BranchId,@TC_TDCalendarId,28, @TDDate      --@EventCreatedOn-- Modified By: Tejashree Patil on 9 July 2013 at 5 pm
	END
	
END


