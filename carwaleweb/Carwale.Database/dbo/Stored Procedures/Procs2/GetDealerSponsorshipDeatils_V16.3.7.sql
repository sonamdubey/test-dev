IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerSponsorshipDeatils_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerSponsorshipDeatils_V16]
GO

	-- =============================================
-- Author:		Ashish Verma
-- Create date: 07/08/2014
-- Description:	Get Sponsored dealer Details based on dealerId
-- exec [dbo].[PQ_GetDealerSponsorshipV1.1] 35,1,""
-- modified by ashish Verma Instead of dealer id we are retrieving dealer info by campaign id on 1/09/2014
-- modified by vinayak passing address,makename as output param 2/3/2015
-- modified by vinayak changed inner join to left join for carmakes,PQ_DealerCitiesModels 11/3/2015
-- modified by rohan sapkal , fetch address with city from Dealer_NewCar instead of Dealers (if available) 11-05-2015
-- Modified By : Vikas J on 19/05/15 Added four new output parametes
-- Modified By : Vicky Lund, 04/11/15, Removed dealer_newcar dependency
-- Modified By: Shalini Nair on 01/02/2016 to read masking number from mm_sellermobilemasking or Carwaletollfreenumber table
-- Modified: Vicky Lund, 05/04/2016, Used applicationId column of MM_SellerMobileMasking
-- =============================================
CREATE PROCEDURE [dbo].[GetDealerSponsorshipDeatils_V16.3.7]
	-- Add the parameters for the stored procedure here
	@CampaignId INT
	,@DealerId INT OUTPUT
	,@DealerName VARCHAR(30) OUTPUT
	,@DealerMobile VARCHAR(50) OUTPUT
	,@DealerEmail VARCHAR(250) OUTPUT
	,@DealerActualMobile VARCHAR(100) OUTPUT
	,@DealerLeadBusinessType INT OUTPUT
	,@DealerAddress VARCHAR(MAX) OUTPUT
	,@EnableUserEmail BIT OUTPUT --Modified By : Vikas J on 19/05/15 Added four new output parametes
	,@EnableUserSMS BIT OUTPUT
	,@EnableDealerEmail BIT OUTPUT
	,@EnableDealerSMS BIT OUTPUT
	,@ShowEmail BIT OUTPUT
	,@LeadPanel INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT @DealerId = ds.DealerId
		,@DealerName = ds.DealerName
		--,@DealerMobile = ds.Phone
		,@DealerMobile = CASE 
			WHEN DS.IsDefaultNumber != 0
				THEN CTOLL.TollFreeNumber
			ELSE MM.MaskingNumber
			END
		,@DealerEmail = ds.DealerEmailId
		,@DealerActualMobile = dl.MobileNo
		,@DealerLeadBusinessType = dl.DealerLeadBusinessType --modified by ashish verma
		,@DealerAddress = isnull(dl.Address1, ' ') + isnull(dl.Address2, ' ') + isnull(' ' + C.NAME, ' ')
		,@EnableUserEmail = ISNULL(ds.EnableUserEmail, 1)
		,@EnableUserSMS = ISNULL(ds.EnableUserSMS, 1)
		,@EnableDealerEmail = ISNULL(ds.EnableDealerEmail, 1)
		,@EnableDealerSMS = ISNULL(ds.EnableDealerSMS, 1)
		,@ShowEmail = ds.ShowEmail
		,@LeadPanel = ds.LeadPanel
	FROM PQ_DealerSponsored ds WITH (NOLOCK)
	INNER JOIN Dealers dl WITH (NOLOCK) ON dl.ID = ds.DealerId
	LEFT JOIN Cities C WITH (NOLOCK) ON dl.CityId = C.ID
	LEFT JOIN MM_SellerMobileMasking MM WITH (NOLOCK) ON MM.LeadCampaignId = DS.Id
		AND MM.ApplicationId = 1
	LEFT JOIN CarwaleTollFreeNumber CTOLL WITH (NOLOCK) ON DS.IsDefaultNumber = CTOLL.Id
	WHERE ds.Id = @CampaignId
		--And dl.Status = 0
		AND ds.IsActive = 1 --modified by ashish Verma Instead of dealer id we are retrieving dealer info by campaign id
		AND (
			MM.MaskingNumber IS NOT NULL
			OR DS.IsDefaultNumber != 0
			)
END
