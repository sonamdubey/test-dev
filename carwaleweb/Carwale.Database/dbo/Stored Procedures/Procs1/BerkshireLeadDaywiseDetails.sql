IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BerkshireLeadDaywiseDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BerkshireLeadDaywiseDetails]
GO

	-- =============================================
-- Author:		<Avishkar>
-- Create date: <29/01/2013>
-- Description:	<to show the berkshireleads inquiries>
-- EXEC [dbo].[BerkshireLeadDaywiseDetails]  '2013/01/01','2013/01/29'
-- =============================================
CREATE PROCEDURE [dbo].[BerkshireLeadDaywiseDetails]
	-- Add the parameters for the stored procedure here
	@FromDate  DATETIME,@ToDate  DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    
	SELECT DISTINCT B.ReturnedBerkshireLeadId as BerkshireLeadId,B.BerkshireLeadId as CarWaleLeadId,
	       B.CustomerName,B.CustomerEmail,B.CustomerMobile,
	--C.Name,C.email,C.Mobile, 
	BC.STATE, Bc.CITY AS City, 
	(BV.MAKE_NAME+' - '+BV.MODEL_NAME+'-  '+BV.SUBTYPE_NAME) AS Car,B.CarMakeYear,B.CarRegistrationDate,
	CASE B.PolicyType WHEN 1 THEN 'New' WHEN 3 THEN 'Renew' END PolicyType ,B.CurrentPolicyExpiryDate ,
	B.PushStatusMessage,B.EntryDate
	FROM BerkshireInsuranceLeads AS B WITH(NOLOCK)
		INNER JOIN BerkshireVehicleInfo AS BV WITH(NOLOCK) ON BV.MAKE_CODE=B.BerkshireMakeId
						AND BV.MODEL_CODE=B.BerkshireModelId AND BV.SUBTYPE_CODE=B.BerkshireVersionId			
		INNER JOIN BerkshireCityInfo Bc ON BC.CITY_CODE = B.BerkshireCityId	
	WHERE  CONVERT(VARCHAR(10),B.EntryDate, 111) between @FromDate and @ToDate
	ORDER BY B.EntryDate,B.CustomerName
END


