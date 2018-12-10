IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_GetDealerSponsorship_API_V14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_GetDealerSponsorship_API_V14]
GO

	
-- =============================================
-- Author:		Ashish Verma
-- Create date: 15/07/2014
-- Description:	Get Sponsored dealer
-- exec [dbo].[PQ_GetDealerSponsorshipV1.1] 35,1,""
-- Modified By Vikas : on <11/8/2014>  modified the where clause for cityid and zoneid
--modified by ashish : on 21/08/2014 for showing both autobiz and crm ad on mobile site
--modified by vinayak <31/10/2014> for constrain ads based on count and types
-- =============================================
CREATE PROCEDURE [dbo].[PQ_GetDealerSponsorship_API_V14.10.2.2] 
	-- Add the parameters for the stored procedure here
	@ModelId NUMERIC
	,@CityId INT
	,@ZoneId INT
	-- Output Parameters 
	,@CampaignId INT OUTPUT
	,@DealerName VARCHAR(30) OUTPUT
	,@DealerMobile VARCHAR(50) OUTPUT
	,@DealerEmail VARCHAR(50) OUTPUT
	,@DealerActualMobile VARCHAR(15) OUTPUT
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
	SELECT Top 1 @CampaignId = ds.Id
		,@DealerName = ds.DealerName
		,@DealerMobile = ds.Phone
		,@DealerEmail = ds.DealerEmailId
		,@DealerActualMobile = dl.MobileNo
		,@DealerLeadBusinessType = dl.DealerLeadBusinessType
		FROM PQ_DealerSponsored ds WITH (NOLOCK)
		INNER JOIN PQ_DealerCitiesModels PCM  WITH(NOLOCK) ON PCM.PqId = ds.Id
		INNER JOIN Dealers dl WITH(NOLOCK) on dl.ID = ds.DealerId
		WHERE	( (PCM.CityId=@cityid  AND ISNULL(PCM.ZoneId,0) =ISNULL(@ZoneId,0) --modified by vikas
                          )-- modified by vikas
                     OR PCM.CityId=-1
                   )
	and PCM.ModelId=@ModelId and IsActive = 1 --modified by ashish 
	and ((ds.TotalCount<ds.TotalGoal and ds.DailyCount<ds.DailyGoal) or ds.Type <> 2)--modified by vinayak
	and CONVERT(date,GETDATE()) BETWEEN  CONVERT(date,ds.StartDate) and CONVERT(date,ds.EndDate)-- modified by vikas
	ORDER BY dl.DealerLeadBusinessType, NEWID();
END


