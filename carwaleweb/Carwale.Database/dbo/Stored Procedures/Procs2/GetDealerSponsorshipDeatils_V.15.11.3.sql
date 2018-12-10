IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerSponsorshipDeatils_V]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerSponsorshipDeatils_V]
GO

	-- =============================================
-- Author:		Ashish Verma
-- Create date: 07/08/2014
-- Description:	Get Sponsored dealer Details based on dealerId
-- exec [dbo].[PQ_GetDealerSponsorshipV1.1] 35,1,""
--modified by ashish verma on 21/08/2014
--modified by ashish Verma Instead of dealer id we are retrieving dealer info by campaign id on 1/09/2014
--modified by vinayak passing address,makename as output param 2/3/2015
--modified by vinayak changed inner join to left join for carmakes,PQ_DealerCitiesModels 11/3/2015
--modified by rohan sapkal , fetch address with city from Dealer_NewCar instead of Dealers (if available) 11-05-2015
--Modified By : Vikas J on 19/05/15 Added four new output parametes
--Modified By : Vicky Lund, 04/11/15, Removed dealer_newcar dependency
-- =============================================
CREATE PROCEDURE [dbo].[GetDealerSponsorshipDeatils_V.15.11.3]
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
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT @DealerId = ds.DealerId
		,@DealerName = ds.DealerName
		,@DealerMobile = ds.Phone
		,@DealerEmail = ds.DealerEmailId
		,@DealerActualMobile = dl.MobileNo
		,@DealerLeadBusinessType = dl.DealerLeadBusinessType --modified by ashish verma
		,@DealerAddress = isnull(dl.Address1, ' ') + isnull(dl.Address2, ' ') + isnull(' ' + C.NAME, ' ')
		,@EnableUserEmail = ISNULL(ds.EnableUserEmail, 1)
		,@EnableUserSMS = ISNULL(ds.EnableUserSMS, 1)
		,@EnableDealerEmail = ISNULL(ds.EnableDealerEmail, 1)
		,@EnableDealerSMS = ISNULL(ds.EnableDealerSMS, 1)
	FROM PQ_DealerSponsored ds WITH (NOLOCK)
	INNER JOIN Dealers dl WITH (NOLOCK) ON dl.ID = ds.DealerId
	LEFT JOIN Cities C WITH (NOLOCK) ON dl.CityId = C.ID
	WHERE ds.Id = @CampaignId
	    --And dl.Status = 0
		AND ds.IsActive = 1 --modified by ashish Verma Instead of dealer id we are retrieving dealer info by campaign id
END

