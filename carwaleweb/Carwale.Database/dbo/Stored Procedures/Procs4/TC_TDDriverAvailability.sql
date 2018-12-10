IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDDriverAvailability]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDDriverAvailability]
GO

	-- =============================================
-- Author:        Binumon George
-- Create date: 27-jul-2012
-- Description:    checking driver avaibility given date and time
-- =============================================
--declare @out int 
--execute TC_TDDriverAvailability '2012-07-03','10:00','14:00' ,10,124,@out out
--select @out
CREATE PROCEDURE [dbo].[TC_TDDriverAvailability]
    @TDDate DATE,
    @TDStartTime TIME(0),
    @TDEndTime TIME(0),
    @TC_TDCalendarId BIGINT,
    @TDDriverId BIGINT,
    @Status TINYINT OUTPUT
AS
BEGIN
    SET @Status=0 
   
    IF EXISTS(SELECT TOP 1 TC_TDCalendarId FROM TC_TDCalendar TDC WHERE  (TDDriverId=@TDDriverId OR TC_UsersId=@TDDriverId)
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
		SET @Status=1 --Driver not available.
    END
    --SET @Status=1-- Consultant available.
END



