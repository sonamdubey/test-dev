IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDCarAvaibility]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDCarAvaibility]
GO

	



-- =============================================
-- Author:		Binumon George
-- Create date: 20 Jun 2012
-- Description:Checking here avaibilty of TD cars for test drive on basis of date and time.
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDCarAvaibility]
@TDDate DATE,
@TDStartTime TIME(1),
@TDEndTime TIME(1),
@TC_TDCarsId BIGINT,
@BranchId BIGINT ,
@TC_TDCalendarId BIGINT,
@IsAvaialable BIT OUT
AS
BEGIN
	SET @IsAvaialable=1
	
	IF EXISTS(SELECT TOP 1 TC_TDCalendarId FROM TC_TDCalendar TDC WHERE  TC_TDCarsId=@TC_TDCarsId AND BranchId=@BranchId
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
    AND TC_TDCalendarId<>@TC_TDCalendarId AND TDC.TDStatus<>4)
    BEGIN   
		SET @IsAvaialable=0		
    END
END
