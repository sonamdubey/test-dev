IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_AP_LogOutletData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_AP_LogOutletData]
GO

	
-- Created By:    Deepak
-- Create date: 28-10-2015
-- Description:	Log Outlet Data
-- Modified by Manish on 06-01-2016 changed the logic: discarded the migrated record from Dealer_NewCar where outlet field is null
-- Modified by Manish on 07-01-2016 since table has the constraint not null on outlet count field job failed. Handled that case also.
-- Modified by Manish on 13-01-2016 As discussed with Megha as per revised logic only consider the outlet where user entered the value i.e. no need to handle 0 and null value.
--========================================================================
CREATE PROCEDURE [dbo].[DCRM_AP_LogOutletData] 

AS
 BEGIN
	  SET NOCOUNT ON;
	
		INSERT INTO DCRM_OutletCountLog (  DealerId
										  ,DealerType
										  ,OutletCount
										  ,CaptureDate
										  ,ApplicationId)
								SELECT   D.ID AS DealerId
									   , D.TC_DealerTypeId
									   , D.OutletCnt   OutletCount
										, GETDATE() CaptureDate
										, ApplicationId
								FROM Dealers AS D WITH (NOLOCK)
								WHERE D.Status = 0
								AND  D.OutletCnt<>0 
								AND  D.OutletCnt IS NOT NULL ;
   END
