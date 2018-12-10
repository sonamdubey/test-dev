IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerSponsorshipDeatils]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerSponsorshipDeatils]
GO

	
-- =============================================
-- Author:		Ashish Verma
-- Create date: 07/08/2014
-- Description:	Get Sponsored dealer Details based on dealerId
-- exec [dbo].[PQ_GetDealerSponsorshipV1.1] 35,1,""
--modified by ashish verma on 21/08/2014
--modified by ashish Verma Instead of dealer id we are retrieving dealer info by campaign id on 1/09/2014
-- =============================================
CREATE PROCEDURE [dbo].[GetDealerSponsorshipDeatils] 
	-- Add the parameters for the stored procedure here
	
	
	@CampaignId INT
	,@DealerId INT OUTPUT
	,@DealerName VARCHAR(30) OUTPUT
	,@DealerMobile VARCHAR(50) OUTPUT
	,@DealerEmail VARCHAR(50) OUTPUT
	,@DealerActualMobile VARCHAR(100) OUTPUT
	,@DealerLeadBusinessType INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	--SELECT DealerId,DealerName,Phone AS PhoneNo,DealerEmailId AS DealerEmail
	--FROM PQ_DealerSponsored WITH(NOLOCK)
	--WHERE CityId= @CityId and ModelId=@ModelId and IsActive = 1
	SELECT @DealerId = ds.DealerId, 
	     @DealerName = ds.DealerName
		,@DealerMobile = ds.Phone
		,@DealerEmail = ds.DealerEmailId
		,@DealerActualMobile = dl.MobileNo
		,@DealerLeadBusinessType = dl.DealerLeadBusinessType --modified by ashish verma
		FROM PQ_DealerSponsored ds WITH (NOLOCK)
		INNER JOIN Dealers dl WITH(NOLOCK) on dl.ID = ds.DealerId
		WHERE	ds.Id = @CampaignId and ds.IsActive = 1 --modified by ashish Verma Instead of dealer id we are retrieving dealer info by campaign id
END



/****** Object:  StoredProcedure [dbo].[PQ_GetDealerSponsorshipV1.1]    Script Date: 8/27/2014 8:48:36 AM ******/
-- SET ANSI_NULLS ON





/****** Object:  StoredProcedure [dbo].[Forum_InsertForum]    Script Date: 2/16/2015 5:15:35 PM ******/
-- SET ANSI_NULLS ON
