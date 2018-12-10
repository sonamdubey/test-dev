IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetAvailableTDCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetAvailableTDCars]
GO
	-- ================================================================================
-- Author:		Vinay Kumar Prajapati 
-- Create date: 01 June 2016
-- Description:	This will return all records of available test drive cars for given branchId
-- EXECUTE [TC_GetAvailableTDCars] 5 ,57
-- ================================================================================
CREATE PROCEDURE [dbo].[TC_GetAvailableTDCars](
 @BranchId INT,
 @TDCalendarId INT
 )
AS
BEGIN

	SELECT TOP 1 TCS.Address , CD.ArealId AS AreaId,AR.CityId, CD.AreaName, TDC.TC_TDCarsId AS  CarId , TDC.CarName , CD.TC_UsersId AS   ConsultantId ,TCS.Mobile AS Mobile ,
	CD.TC_SourceId AS SourceId , CD.TC_CustomerId AS  CustomerId , TCS.CustomerName AS CustomerName , CD.TDDriverId AS DriverId, CD.ModifiedDate  AS UpdatedDate,
	'' AS InquiryId,CD.TC_TDCalendarId,CD.TDDate,CD.TDStartTime,CD.TDEndTime,CD.TDStatus,TDC.BranchId
	FROM TC_TDCars AS TDC WITH(NOLOCK) 
	LEFT JOIN TC_TDCalendar AS CD WITH(NOLOCK) ON CD.TC_TDCarsId= TDC.TC_TDCarsId	
	LEFT JOIN TC_CustomerDetails AS TCS WITH(NOLOCK) ON TCS.id = Cd.TC_CustomerId 
	LEFT JOIN Areas AS AR WITH(NOLOCK) ON AR.ID = CD.ArealId AND AR.IsDeleted=0
	WHERE TDC.BranchId = @BranchId AND CD.TC_TDCalendarId = @TDCalendarId AND TDC.IsActive = 1  
	ORDER BY TDC.TC_TDCarsId DESC
END

