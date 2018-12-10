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
-- Modified by : Vinayak on 3-11-2014 Added parameter @AssignedDealerId.To update the column with assigned dealer id for auto assignment.
-- Modified by : Vinayak on 6-11-2014,UNDONE the modificaton done on 28-10-2014: Removed (Update counters for column TotalCount,DailyCount and setting isActive flag  (SavePQDealerInquiryDetails_V14.10.2.2))
-- =============================================
CREATE PROCEDURE [dbo].[SavePQDealerInquiryDetails_V14.11.2.3] 
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
	,@AssignedDealerId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @dealerType INT
    -- Insert statements for procedure here
	INSERT INTO PQDealerAdLeads(PQId,DealerId,LeadClickSource,DealerLeadBusinessType,Name,Email,Mobile,AssignedDealerID) values (@PQId,@DealerId,@LeadClickSource,@DealerLeadBusinessType,@Name,@Email,@Mobile,@AssignedDealerId)

	SET @ResponseId = SCOPE_IDENTITY();
END
