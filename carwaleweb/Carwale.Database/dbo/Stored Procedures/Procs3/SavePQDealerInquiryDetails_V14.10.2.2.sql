IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SavePQDealerInquiryDetails_V14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SavePQDealerInquiryDetails_V14]
GO

	-- =============================================
-- Author:		Ashish Verma
-- Create date: 07/08/2014
-- Description:	To save PQ dealer add inquiry details before calling to api
-- exec [dbo].[SavePQDealerInquiryDetails] 35,1,""
-- Modified by : Vinayak on 15-10-2014 Added columns Name,Email and Mobile
-- Modified by : Vinayak on 28-10-2014 Added parameter @CampaignId. Update counters for column TotalCount,DailyCount and setting isActive flag  (SavePQDealerInquiryDetails_V14.10.2.2)
-- =============================================
 CREATE PROCEDURE [dbo].[SavePQDealerInquiryDetails_V14.10.2.2] 
	-- Add the parameters for the stored procedure here
	@PQId NUMERIC
	,@DealerId INT
	,@LeadClickSource INT
	,@DealerLeadBusinessType INT
	,@ResponseId NUMERIC OUTPUT
	,@Name varchar(100)=NULL
	,@Email varchar(100)=NULL
	,@Mobile varchar(100)=NULL
	,@CampaignId INT =NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @dealerType INT
    -- Insert statements for procedure here
	INSERT INTO PQDealerAdLeads(PQId,DealerId,LeadClickSource,DealerLeadBusinessType,Name,Email,Mobile) 
	                    VALUES (@PQId,@DealerId,@LeadClickSource,@DealerLeadBusinessType,@Name,@Email,@Mobile)

	SET @ResponseId = SCOPE_IDENTITY();

	-- Modified by : Vinayak on 28-10-2014 to show lead wise Ads
	select @dealerType=Type from PQ_DealerSponsored WITH (NOLOCK) where Id=@CampaignId
	IF @dealerType=2
	BEGIN
		UPDATE PQ_DealerSponsored SET DailyCount+=1,TotalCount+=1 where Id=@CampaignId
	END
END
