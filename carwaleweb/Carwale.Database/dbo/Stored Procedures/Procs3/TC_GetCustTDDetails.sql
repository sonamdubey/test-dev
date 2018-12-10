IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCustTDDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCustTDDetails]
GO
	-- =============================================  
-- Author:  Yuga Hatolkar
-- Created On: 8th Dec, 2015
-- Description: Get Customer Test Drive Details.
-- Modified By : Ruchira Patil on 25th July 2016 (added ISNULL for firstname and lastname)
-- =============================================  
CREATE PROCEDURE [dbo].[TC_GetCustTDDetails]  	 
	 @CustId BIGINT,
	 @TC_TDCalendarId BIGINT=NULL
AS  
BEGIN 
	
	SELECT TCD.TDCarDetails, TCD.TDDate, TCD.TDStartTime, ISNULL(D.FirstName,'') + ' ' + ISNULL(D.LastName,'') AS DealerName FROM TC_TDCalendar TCD WITH(NOLOCK)
	INNER JOIN Dealers D WITH(NOLOCK) ON TCD.BranchId = D.ID
	WHERE TC_CustomerId = @CustId AND TCD.TC_TDCalendarId = @TC_TDCalendarId
	
END
----------------------------------------------------------------------------------
