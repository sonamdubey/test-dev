IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDBookingLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDBookingLoad]
GO

	--Modified By:Binu,Date 24-Jul-2012 Description: Added Driver procedure  
-- Author:  Binu  
-- Create date: 19 Jun 2012  
-- Description: Load to step two test cars page  
-- exec [TC_TDBookingLoad] 5,679,313  
-- Modified By:Tejashree Patil on 16 Aug 2012 Changed in SELECT clause TC_UsersId   
-- Modified by: Nilesh Utture on 28th Feb,2013 Bring only those Td cars for the car model on which inquiry is added
-- Modified By: Vivek Singh 17-02-2014 for @CustId Added in Parameters Fetching Test Drive Address from the new Column TDADDRESS in TC_TDCalendar table presently  Test Drive Address is Fetched and stored in Address Field in TC_CustomerDetails Table
-- =============================================  
CREATE PROCEDURE [dbo].[TC_TDBookingLoad]  
@BranchId BIGINT,  
@tdcalenderId BIGINT, 
@InquiryId BIGINT,
@CustId BIGINT=null
  
AS  
BEGIN  
	 -- Modified by: Nilesh Utture on 28th Feb,2013 
	 IF @InquiryId IS NOT NULL
	 BEGIN 
		DECLARE @ModelId INT
		SELECT @ModelId = V.ModelId FROM TC_NewCarInquiries N INNER JOIN vwMMV V ON V.VersionId = N.VersionId WHERE N.TC_NewCarInquiriesId = @InquiryId
		
		SELECT TDC.TC_TDCarsId, TDC.CarName FROM TC_TDCars  TDC INNER JOIN vwMMV V
		ON V.VersionId = TDC.VersionId
		WHERE IsActive=1 AND TDC.BranchId=@BranchId AND V.ModelId = @ModelId
		ORDER BY TDC.CarName ASC
		
		EXEC TC_GetAreas @BranchId  
		EXEC TC_TDConsultant @BranchId
		EXEC TC_TDDriver @BranchId
		
		 IF(@tdcalenderId IS NULL)
		  BEGIN
		   SELECT C.Address AS Address FROM TC_CustomerDetails C WITH (NOLOCK) WHERE C.Id=@CustId
		  END
	 END
	   
	 IF(@tdcalenderId IS NOT NULL)--Edit mode selecting data for display purpose  
	  BEGIN
	  
	     SELECT TDC.TC_TDCarsId,COALESCE(TDC.TDAddress,C.Address)AS Address,TDC.ArealId,TDC.TDDate,CONVERT(VARCHAR(5),TDC.TDStartTime)AS TDStartTime, CONVERT(VARCHAR(5),TDC.TDEndTime) AS TDEndTime, TDC.TC_UsersId, TDC.TDDriverId AS DriverId
	     FROM TC_TDCalendar TDC WITH (NOLOCK) 
	     INNER JOIN TC_CustomerDetails C WITH (NOLOCK)  ON C.Id = TDC.TC_CustomerId  
	     WHERE TDC.TC_TDCalendarId=@tdcalenderId
	    END
	  END  








