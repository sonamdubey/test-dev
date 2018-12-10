IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDStepThreeLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDStepThreeLoad]
GO

	-- =============================================
-- Author:		Binumon George
-- Create date: 21-06-2012
-- Description:	Display data i step 3 page in front end
-- Modified By Vivek Singh 17-02-2014 for Fetching Test Drive Address from the new Column TDADDRESS in TC_TDCalendar table 
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDStepThreeLoad]
	@TC_TDCalenderId INT
AS
BEGIN
	SELECT TCC.CustomerName, TCU.UserName As Consultant,CAl.AreaName,CAl.TDCarDetails,CAl.TC_TDCarsId,CAl.TDDate AS CalDate, CAl.TDAddress AS Address,
	DATENAME(DW,CAl.TDDate)  + ', ' +DATENAME(DD, CAl.TDDate)+ ' ' + DATENAME(MM,CAl.TDDate)+ '  ' + CONVERT(VARCHAR(10),CAL.TDStartTime,100) + ' - ' + CONVERT(VARCHAR(10),CAL.TDEndTime,100) AS TDDate
	FROM TC_TDCalendar CAl
	INNER JOIN TC_CustomerDetails TCC ON CAL.TC_CustomerId=TCC.Id
	INNER JOIN TC_Users TCU ON  CAL.TC_UsersId=TCU.Id
	WHERE TC_TDCalendarId=@TC_TDCalenderId
END



/****** Object:  StoredProcedure [dbo].[TC_TDBookingLoad]    Script Date: 2/13/2014 4:08:39 PM ******/
SET ANSI_NULLS ON
