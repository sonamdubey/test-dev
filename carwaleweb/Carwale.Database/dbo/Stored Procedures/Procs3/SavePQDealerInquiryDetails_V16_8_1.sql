IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SavePQDealerInquiryDetails_V16_8_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SavePQDealerInquiryDetails_V16_8_1]
GO

	
-- =============================================
-- Author:		Ashish Verma
-- Create date: 07/08/2014
-- Description:	To save PQ dealer add inquiry details before calling to api
-- exec [dbo].[SavePQDealerInquiryDetails] 35,1,""
-- Modified by : Vinayak on 15-10-2014 Added columns Name,Email and Mobile
-- Modified by : Vinayak on 28-10-2014 Added parameter @CampaignId. Update counters for column TotalCount,DailyCount and setting isActive flag  (SavePQDealerInquiryDetails_V14.10.2.2)
-- Modified by : Vinayak on 3-11-2014 Added parameter @AssignedDealerId.To update the column with assigned dealer id for auto assignment.
-- Modified by : Vinayak on 6-11-2014,UNDONE the modificaton done on 28-10-2014: Removed (Update counters for column TotalCount,DailyCount and setting isActive flag  (SavePQDealerInquiryDetails_V14.10.2.2)
-- Modified by : Rohan Sapkal on 03-04-2015 Added new field LTSRC
-- Modified by : Shalini Nair on 21/09/2015 added update operation and parameter @PQDealerAdLeadId
-- Modified by : Vinayak on 17/02/2016 added parameter @ModelHistory to store user model history
-- Modified by : Vikas J on 17/06/2016 added more columns to update statement
-- Modified by : Vikas J on 15/07/2016 added more columns to insert statement
-- Modified by : Sanjay Soni on 10/08/2016 added dealerId in update statement 
-- =============================================
CREATE PROCEDURE [dbo].[SavePQDealerInquiryDetails_V16_8_1]
	-- Add the parameters for the stored procedure here
	@PQId NUMERIC
	,@DealerId INT
	,@LeadClickSource INT
	,@DealerLeadBusinessType INT
	,@Name VARCHAR(100) = NULL
	,@Email VARCHAR(100) = NULL
	,@Mobile VARCHAR(100) = NULL
	,@CampaignId INT = NULL
	,@AssignedDealerId INT = NULL
	,@CityID INT
	,@ZoneID INT = NULL
	,@VersionId INT
	,@PlatformId INT
	,@Comment VARCHAR(500) = NULL
	,@LTSRC VARCHAR(100) = NULL
	,@UtmaCookie VARCHAR(500) = NULL
	,@UtmzCookie VARCHAR(500) = NULL
	,@ModelHistory VARCHAR(200) = NULL
	,@CWCookieValue VARCHAR(100) = NULL
	,@ClientIP VARCHAR(50) = NULL
	,@Browser VARCHAR(100) = NULL
	,@OperatingSystem VARCHAR(50) = NULL
	,@PQDealerAdLeadId NUMERIC OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @dealerType INT

	-- Insert statements for procedure here
	IF (@PQDealerAdLeadId IS NOT NULL)
	BEGIN
		UPDATE PQDealerAdLeads
		SET Email = @Email
			,DealerLeadBusinessType = @DealerLeadBusinessType
			,NAME = @Name
			,Mobile = @Mobile
			,AssignedDealerID = @AssignedDealerID
			,DealerId = @DealerId
			,CityId = @CityId
			,ZoneId = @ZoneId
			,CampaignId = @CampaignId
		WHERE Id = @PQDealerAdLeadId
			-- AND Mobile = @Mobile
	END
	ELSE
	BEGIN
		INSERT INTO PQDealerAdLeads (
			PQId
			,DealerId
			,LeadClickSource
			,DealerLeadBusinessType
			,NAME
			,Email
			,Mobile
			,AssignedDealerID
			,CityId
			,ZoneId
			,VersionId
			,PlatformId
			,LTSRC
			,Comment
			,UtmaCookie
			,UtmzCookie
			,CampaignId
			,ModelHistory
			,CWCookieValue 
			,ClientIP 
			,Browser 
			,OperatingSystem
			)
		VALUES (
			@PQId
			,@DealerId
			,@LeadClickSource
			,@DealerLeadBusinessType
			,@Name
			,@Email
			,@Mobile
			,@AssignedDealerId
			,@CityID
			,@ZoneID
			,@VersionId
			,@PlatformId
			,@LTSRC
			,@Comment
			,@UtmaCookie
			,@UtmzCookie
			,@CampaignId
			,@ModelHistory
			,@CWCookieValue 
			,@ClientIP 
			,@Browser 
			,@OperatingSystem
			)

		SET @PQDealerAdLeadId = SCOPE_IDENTITY();
	END
END

