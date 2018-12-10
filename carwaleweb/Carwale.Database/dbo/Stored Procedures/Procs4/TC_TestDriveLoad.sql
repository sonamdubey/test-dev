IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TestDriveLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TestDriveLoad]
GO

	-- Author:		Nilesh
-- Create date: 28th Feb, 2013
-- Description:	Load all test drive details on TDCalendar tAB
-- Modified By : Umesh Ojha on 27 june 2013 for fetching inquiry source as per dealer make wise
-- =============================================
CREATE  PROCEDURE [dbo].[TC_TestDriveLoad]
@BranchId BIGINT,
@TC_TDCalendarId BIGINT
AS
BEGIN
	EXEC TC_GetTestDriveCars @BranchId  
	--EXEC TC_GetAreas @BranchId 
	EXEC TC_DealerCitiesView @BranchId,1
	EXEC TC_TDConsultant @BranchId
	EXEC TC_TDDriver @BranchId
	--EXECUTE TC_InquirySourceSelect   
	EXECUTE TC_InquirySourceDealerWise @BranchId
END
