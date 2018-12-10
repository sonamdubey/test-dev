IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_BindFLCConversation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_BindFLCConversation]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 21-Jan-2014
-- Description:	To get flc conversation log for the lead from NewCRM
-- Modified By : Chetan Navin - 17th Feb 2014 (Added Fields Purchase mode and PurchaseOnName)
-- Modified by : Vaibhav K 6 Mar 2014 Modified NotConnectedReason 
-- =============================================
CREATE PROCEDURE [dbo].[CRM_BindFLCConversation]
	-- Add the parameters for the stored procedure here
	@LeadId		BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT (case VL.NotConnectedReason WHEN 8 THEN 'Fake' WHEN 76 THEN 'Invalid Number' WHEN 2 THEN 'Ringing' ELSE SD.SubDisposition  END) AS NotConnectedReason,
	(case VL.UnavailabilityReason WHEN 1 THEN 'Call Back' ELSE ETD_UR.NAME END) AS UnavailabilityReason,ETD_NIR.NAME AS NotIntReason,
    VL.CallConnected,VL.GoodTimeToTalk,VL.Language,VL.LookingForCar,VL.IsSameMake,VL.BuyingSpan,VL.CallBackRequest,
    VL.IsCarBookedAlready,VW.CAR AS BookedCar,(VL.BookedCarDate) AS BookedCarDate,VL.SpecialComments,VL.BookedCarProblem,VL.IsFuturePlanToBuy,(VL.FuturePurchaseDate) as FutureDate,
    CASE WHEN VL.PurchaseMode = 1 THEN 'Finance' WHEN VL.PurchaseMode = 2 THEN 'Outright' ELSE 'Others' END AS PurchaseMode,
	CASE WHEN VL.PurchaseOnNameType = 2 THEN VL.PurchaseOnName + '-' + VL.PurchaseContact ELSE 'Same' END AS PurchaseOnName
	FROM CRM_VerificationOthersLog VL WITH(NOLOCK)
    LEFT JOIN CRM_SubDisposition SD WITH(NOLOCK) ON SD.ID = VL.NotConnectedReason  
    LEFT JOIN CRM_ETDispositions ETD_UR WITH(NOLOCK) ON ETD_UR.ID =VL.UnavailabilityReason
    LEFT JOIN CRM_ETDispositions ETD_NIR WITH(NOLOCK) ON ETD_NIR.ID =VL.NotIntReason
    LEFT JOIN vwMMV VW ON VW.VersionId = VL.BookedCarVersion
    WHERE VL.LeadId = @LeadId
END
