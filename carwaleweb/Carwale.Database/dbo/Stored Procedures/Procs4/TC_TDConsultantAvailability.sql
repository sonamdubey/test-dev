IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDConsultantAvailability]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDConsultantAvailability]
GO

	-- =============================================
-- Author:        Binumon George
-- Create date: 20-06-2012
-- Description:    checking consutant avaibility given date and time
-- =============================================
--declare @out int 
--execute TC_TDConsultantAvailability '2012-07-03','10:00','14:00' ,10,124,@out out
--select @out
CREATE PROCEDURE [dbo].[TC_TDConsultantAvailability]
    @TDDate DATE,
    @TDStartTime TIME(0),
    @TDEndTime TIME(0),
    @TC_TDCalendarId BIGINT,
    @TDConsultant BIGINT,
    @Status TINYINT OUTPUT
AS
BEGIN
    SET @Status=0-- error occured   
   
    IF EXISTS(SELECT TOP 1 TC_TDCalendarId FROM TC_TDCalendar TDC WHERE  (TC_UsersId=@TDConsultant OR TDDriverId=@TDConsultant)
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
		SET @Status=1 --Consultant not available.
    END
    --SET @Status=1-- Consultant available.
END



