IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDStepTwo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDStepTwo]
GO

	-- =============================================
-- Author:        Binumon George
-- Create date: 20-06-2012
-- Description:    Inserting remaining fileds to TC_TDCalender table through step 2
-- Modification: By : Surendra Date 8th Aug,2012,check @TC_TDDriverId  conditional
-- Modified By: Tejashree Patil on 11 Dec 2012 Added parameter @Comments and Updated Comments field in TC_TDCalendar
-- =============================================
CREATE  Procedure [dbo].[TC_TDStepTwo]
    @TC_TDCarsId INT,
    @TDCarDetails VARCHAR(100),
    @TDDate DATE,
    @TDStartTime TIME,
    @TDEndTime TIME,
    @ModifiedBy BIGINT,
    @TC_TDCalendarId BIGINT,
    @TDConsultant BIGINT,
    @Status TINYINT OUTPUT,
    @TC_TDDriverId BIGINT,
    @Comments VARCHAR (500)=NULL
AS
BEGIN
    SET @Status=0-- error occured   
    IF (@TC_TDDriverId =-1)
    BEGIN
		SET @TC_TDDriverId=NULL
    END
   
   IF(@TC_TDDriverId IS NULL)
	   BEGIN   
			IF EXISTS(SELECT TOP 1 TC_TDCalendarId FROM TC_TDCalendar TDC  WITH (NOLOCK) WHERE  (TC_TDCarsId=@TC_TDCarsId Or TC_UsersId=@TDConsultant)
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
		END
	ELSE
		BEGIN
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
		END
			
	-- Insert record in TC_TDCalendarLog table
	-- Modified By: Tejashree Patil on 11 Dec 2012
	DECLARE @OldTDDate DATE,@OldStartTime TIME, @OldEndTime TIME
	SELECT @OldTDDate=TDDate ,@OldStartTime=TDStartTime , @OldEndTime=TDEndTime
	FROM TC_TDCalendar WHERE TC_TDCalendarId=@TC_TDCalendarId
	
	IF(@OldTDDate<>@TDDate OR @OldStartTime<>@TDStartTime OR @OldEndTime<>@TDEndTime)
	BEGIN	
		INSERT INTO TC_TDCalendarLog (TC_TDCalendarId, BranchId, TC_CustomerId,TC_TDCarsId, TDCarDetails, AreaName, ArealId, TC_UsersId, 
					TC_SourceId, TDDate, TDStartTime, TDEndTime, TDStatus, EntryDate, TDDriverId,TDStatusDate, Comments, ModifiedDate, ModifiedBy )
		SELECT	TC_TDCalendarId, BranchId, TC_CustomerId,TC_TDCarsId, TDCarDetails, AreaName, ArealId, TC_UsersId, 
				TC_SourceId, TDDate, TDStartTime, TDEndTime, TDStatus, EntryDate, TDDriverId,TDStatusDate, Comments, ModifiedDate, ModifiedBy
		FROM	TC_TDCalendar WITH (NOLOCK)
		WHERE	TC_TDCalendarId = @TC_TDCalendarId	
	END	   
		
    UPDATE TC_TDCalendar SET  TC_TDCarsId=@TC_TDCarsId, TDCarDetails=@TDCarDetails, TDDate=@TDDate,
    TDStartTime=@TDStartTime, TDEndTime=@TDEndTime, ModifiedDate=GETDATE(), ModifiedBy=@ModifiedBy,
    TC_UsersId=@TDConsultant,TdDriverId=@TC_TDDriverId, Comments=@Comments -- Modified By: Tejashree Patil on 11 Dec 2012
    WHERE TC_TDCalendarId=@TC_TDCalendarId
   
    SET @Status=1-- sussesfully save      
   
END
