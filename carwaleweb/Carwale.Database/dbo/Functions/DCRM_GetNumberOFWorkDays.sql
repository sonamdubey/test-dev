IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetNumberOFWorkDays]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[DCRM_GetNumberOFWorkDays]
GO

	
CREATE FUNCTION [dbo].[DCRM_GetNumberOFWorkDays](@StartDate DATETIME, @EndDate DATETIME)

RETURNS SMALLINT

AS
	BEGIN
		SET @StartDate = DATEADD(DD, DATEDIFF(DD, 0, @StartDate), 0) 
		SET @EndDate = DATEADD(DD, DATEDIFF(DD, 0, @EndDate), 0) 
          
		DECLARE @WORKDAYS INT

		SELECT @WORKDAYS =	(DATEDIFF(DD, @StartDate, @EndDate) + 1)
							-(DATEDIFF(WK, @StartDate, @EndDate) * 2)
   							-(CASE WHEN DATENAME(DW, @StartDate) = 'Sunday' THEN 1 ELSE 0 END)--remove sunday
							-(CASE WHEN @StartDate IN (SELECT Holiday FROM CRM_HolidayList) THEN 1 ELSE 0 END)--remove carwale holiday
							-(CASE WHEN @EndDate IN (SELECT Holiday FROM CRM_HolidayList) THEN 1 ELSE 0 END)--remove carwale holiday
		RETURN @WORKDAYS
	END

