IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDScheduleSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDScheduleSave]
GO

	-- =============================================
-- Author:			Nilesh Utture
-- Create date:		12-12-2012
-- Description:     Inserting Test Drive Details through API
-- Modified by: Nilesh Utture on 27th Feb,2013 ADDed CAST (@TDDate AS DATETIME) + CAST(@TDStartTime AS DATETIME)
-- Modified by: Nilesh Utture on 18th Jun,2013  Passed @ModifiedBy as parameter
-- Modified by: Umesh on 3 july 2013 added @IsComplete parameter for past date & time TD book & update it to completed
-- Modified by: Umesh on 23 july 2013 status variable assigned to fix value accroding to consultant,car & driver
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDScheduleSave]
	@CustName VARCHAR(100),
	@BranchId BIGINT,
	@Mobile VARCHAR(15),
	@Address VARCHAR(150), 
	@SourceId TINYINT,
	@Area VARCHAR(100),
	@AreaId BIGINT ,
	@UserId BIGINT,
	@Email VARCHAR(100),
    @TC_TDCarsId INT,
    @TDCarDetails VARCHAR(100),
    @TDDate DATE,
    @TDStartTime TIME,
    @TDEndTime TIME,
    @ModifiedBy BIGINT,
    @TC_TDCalendarId BIGINT = -1 OUTPUT,
    @TDConsultant BIGINT,
    @Status TINYINT OUTPUT,
    @TC_TDDriverId BIGINT,
    @Comments VARCHAR (500)=NULL,
    @TDStatus TINYINT,
    @IsComplete TINYINT = 0
AS
BEGIN
	DECLARE @VersionId SMALLINT 
	DECLARE @CityId SMALLINT 
	SELECT @versionId = VersionId FROM TC_TDCars WHERE TC_TDCarsId = @TC_TDCarsId
	SELECT @CityId = cityId FROM Areas WHERE ID = @AreaId
    SET @Status=0-- error occured   
    IF (@TC_TDDriverId =-1 OR @TC_TDDriverId = 0) -- Modified by: Nilesh Utture on 18th Jun,2013 
    BEGIN
		SET @TC_TDDriverId=NULL
    END
   
   IF(@TC_TDDriverId IS NULL)
	   BEGIN   
			/*IF EXISTS(SELECT TOP 1 TC_TDCalendarId FROM TC_TDCalendar TDC  WITH (NOLOCK) WHERE  (TC_TDCarsId=@TC_TDCarsId Or TC_UsersId=@TDConsultant)
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
			AND TC_TDCalendarId<>@TC_TDCalendarId AND TDC.TDStatus<>27)
			BEGIN   
				SET @Status=2
				RETURN -1
			END*/	
			
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
		SELECT TOP 1 @Status =  (CASE WHEN  TC_UsersId=@TDConsultant  THEN 2
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
	
			/*
			IF EXISTS(SELECT TOP 1 TC_TDCalendarId FROM TC_TDCalendar TDC  WITH (NOLOCK) WHERE  (TC_TDCarsId=@TC_TDCarsId Or TC_UsersId=@TDConsultant OR TDDriverId=@TC_TDDriverId )
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
				AND TC_TDCalendarId<>@TC_TDCalendarId AND TDC.TDStatus<>27)
				BEGIN   
					SET @Status=2
					RETURN -1
				END
				*/
		END
		
		IF(@Status <> 0)
		BEGIN
			RETURN -1
		END		
		
DECLARE @InqStatus SMALLINT
DECLARE @TC_InqId BIGINT
DECLARE @TC_LeadDiverterdTo VARCHAR(100)
DECLARE @DateTimeNow DATETIME
SELECT @DateTimeNow = GETDATE()
DECLARE @InquiriesLeadId BIGINT
DECLARE @LeadId BIGINT 

EXEC TC_INQNewCarBuyerSave   
						@CustomerName = @CustName, @CustomerEmail = @Email, @CustomerMobile = @Mobile,
						@VersionId = @VersionId, @CityId = @CityId, @Buytime = NULL, @InquirySource = @SourceId,
						@Eagerness =NULL, @TC_CustomerId = NULL, @AutoVerified = 1, @BranchId = @BranchId,
						@LeadOwnerId = @TDConsultant, @CreatedBy = @TDConsultant, @Status = @InqStatus OUTPUT,
						@PQReqDate = NULL, @TDReqDate = NULL, @ModelId = NULL, @FuelType = NULL,
						@Transmission = NULL, @CW_CustomerId = NULL, @CWInquiryId = NULL, @TC_NewCarInquiryId = @TC_InqId OUTPUT,
						@LeadDivertedTo = @TC_LeadDiverterdTo OUTPUT		
						
SELECT @InquiriesLeadId = NC.TC_InquiriesLeadId,@LeadId = L.TC_LeadId  FROM TC_NewCarInquiries NC 
INNER JOIN TC_InquiriesLead L ON L.TC_InquiriesLeadId = NC.TC_InquiriesLeadId WHERE NC.TC_NewCarInquiriesId = @TC_InqId
				
		IF(@TC_TDCalendarId = -1)
			BEGIN
				DECLARE @CustId BIGINT
				EXEC TC_Customer @BranchId,@Email,@CustName,@Mobile,@Area,NULL,NULL,@UserId,@CustId OUTPUT,@Address
				
				INSERT INTO TC_TDCalendar(BranchId, TC_CustomerId, AreaName, ArealId, TC_SourceId, TC_TDCarsId, TDCarDetails,
										  TDDate, TDStartTime, TDEndTime,TDStatus, TDStatusDate, ModifiedDate, ModifiedBy, TC_UsersId, TdDriverId, Comments)
				VALUES(@BranchId,@CustId,@Area,@AreaId,@SourceId, @TC_TDCarsId,@TDCarDetails,@TDDate,@TDStartTime,@TDEndTime, @TDStatus, GETDATE(), GETDATE(), 
					   @ModifiedBy, @TDConsultant,@TC_TDDriverId,@Comments)
				
				SET @TC_TDCalendarId = SCOPE_IDENTITY()
				
				-- Modified by: Nilesh Utture on 27th Feb,2013 ADDed CAST (@TDDate AS DATETIME) + CAST(@TDStartTime AS DATETIME)
				UPDATE TC_NewCarInquiries SET TDStatus=39, TDDate= CAST (@TDDate AS DATETIME) + CAST(@TDStartTime AS DATETIME), TC_TDCalendarId = @TC_TDCalendarId WHERE TC_NewCarInquiriesId = @TC_InqId
				EXEC TC_DispositionLogInsert @ModifiedBy,39,@TC_InqId,5,@LeadId -- Modified by: Nilesh Utture on 11th Jun,2013 
				SET @Status=1-- sussesfully save  
			END
		-- Modified by: Nilesh Utture on 27th Feb,2013 Commented as this part is no longer used
		--ELSE
		--	BEGIN
		--		DECLARE @OldTDDate DATE,@OldStartTime TIME, @OldEndTime TIME
				
		--		SELECT @OldTDDate=TDDate ,@OldStartTime=TDStartTime , @OldEndTime=TDEndTime
		--		FROM TC_TDCalendar WHERE TC_TDCalendarId=@TC_TDCalendarId
				
		--		IF(@OldTDDate<>@TDDate OR @OldStartTime<>@TDStartTime OR @OldEndTime<>@TDEndTime)
		--		BEGIN	
		--			INSERT INTO TC_TDCalendarLog (TC_TDCalendarId, BranchId, TC_CustomerId,TC_TDCarsId, TDCarDetails, AreaName, ArealId, TC_UsersId, 
		--						TC_SourceId, TDDate, TDStartTime, TDEndTime, TDStatus, EntryDate, TDDriverId,TDStatusDate, Comments, ModifiedDate, ModifiedBy )
		--			SELECT	TC_TDCalendarId, BranchId, TC_CustomerId,TC_TDCarsId, TDCarDetails, AreaName, ArealId, TC_UsersId, 
		--					TC_SourceId, TDDate, TDStartTime, TDEndTime, TDStatus, EntryDate, TDDriverId,TDStatusDate, Comments, ModifiedDate, ModifiedBy
		--			FROM	TC_TDCalendar WITH (NOLOCK)
		--			WHERE	TC_TDCalendarId = @TC_TDCalendarId	
		--		END	   
					
		--		UPDATE TC_TDCalendar SET  TC_TDCarsId=@TC_TDCarsId, TDCarDetails=@TDCarDetails, TDDate=@TDDate,TDStatus = @TDStatus,
		--		TDStartTime=@TDStartTime, TDEndTime=@TDEndTime, ModifiedDate=GETDATE(), ModifiedBy=@ModifiedBy,
		--		TC_UsersId=@TDConsultant,TdDriverId=@TC_TDDriverId, Comments=@Comments -- Modified By: Tejashree Patil on 11 Dec 2012
		--		WHERE TC_TDCalendarId=@TC_TDCalendarId
		--		SET @Status=1-- sussesfully save 
		--		SET @Status = @TC_InqId
		--	END
		
		-- Modified by: Umesh on 3 july 2013 added @IsComplete parameter for past date & time TD book & update it to completed
		IF(@IsComplete = 1)
		BEGIN
			EXEC TC_TDStatusChange @BranchId,@TC_TDCalendarId,28
		END
		
END
