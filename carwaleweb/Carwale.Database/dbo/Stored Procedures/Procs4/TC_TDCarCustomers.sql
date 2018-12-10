IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDCarCustomers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDCarCustomers]
GO

	-- =============================================
-- Author:		Upendra Kumar
-- Create date: 16 dec, 2015
-- Description:	Getting CustomerList who inquireis on car using CarId
 -- EXEC TC_TDCarCustomers 103,5
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDCarCustomers]
(
   @TC_TDCarId INT,
   @BranchId INT
   )
  AS
  BEGIN
   SELECT  TDC.TC_TDCarsId AS CarId ,TDC.CarName AS CarName,TCC.Id AS CustomerId,TCC.CustomerName AS CustomerName,TCNI.TC_InquirySourceId AS SourceId,TCNI.TC_NewCarInquiriesId AS InquiriesId
   FROM TC_TDCars TDC WITH(NOLOCK) 
   JOIN TC_NewCarInquiries  TCNI WITH(NOLOCK) ON TDC.VersionId = TCNI.VersionId AND  TCNI.MostInterested = 1
   Join TC_InquiriesLead TCI WITH(NOLOCK) ON TCNI.TC_InquiriesLeadId = TCI.TC_InquiriesLeadId
   JOIN TC_CustomerDetails TCC WITH(NOLOCK) ON TCI.TC_CustomerId = TCC.Id AND TCC.IsActive = 1 AND TCC.Isfake = 0
   WHERE TDC.TC_TDCarsId = @TC_TDCarId AND TDC.BranchId = @BranchId AND TDC.IsActive = 1
   ORDER BY TDC.CarName
 END