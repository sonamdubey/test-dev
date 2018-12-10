IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDStepTwoLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDStepTwoLoad]
GO

	--Modified By:Binu,Date 24-Jul-2012 Description: Added Driver procedure
-- Author:		Binu
-- Create date: 19 Jun 2012
-- Description:	Load to step two test cars page
-- exec [TC_TDStepTwoLoad] 5,159
--Modified By:Tejashree Patil on 16 Aug 2012 Changed in SELECT clause TC_UsersId 
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDStepTwoLoad]
@BranchId BIGINT,
@tdcalenderId BIGINT
AS
BEGIN
	EXEC TC_GetTestDriveCars @BranchId
	EXEC TC_TDConsultant @BranchId
	EXEC TC_TDDriver @BranchId
	
	IF(@tdcalenderId IS NOT NULL)--Edit mode selecting data for display purpose
		BEGIN
			SELECT CD.TC_TDCarsId,TDC.TC_UsersId AS Consutant,TDC.TDDate,CONVERT(VARCHAR(5),TDC.TDStartTime)AS TDStartTime, CONVERT(VARCHAR(5),TDC.TDEndTime) AS TDEndTime,CD.VinNo,TDC.TDDriverId AS DriverId
			FROM TC_TDCalendar TDC
			INNER JOIN TC_TDCars CD ON TDC.TC_TDCarsId=CD.TC_TDCarsId
			INNER JOIN TC_Users TCU ON TDC.TC_UsersId=TCU.Id
			WHERE TDC.TC_TDCalendarId=@tdcalenderId
		END
END


