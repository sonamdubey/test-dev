IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SavePQDealerInquiryDetails_V15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SavePQDealerInquiryDetails_V15]
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
-- Modified by : Ashish verma on 18-12-2014 Added 4 new Fields To Sp(CityId,ZoneId,VersionId,PlatformId)
-- Modified by: Rohan Sapkal on 03-04-2015 Added new field LTSRC
-- =============================================
CREATE PROCEDURE [dbo].[SavePQDealerInquiryDetails_V15.4.1] 
	-- Add the parameters for the stored procedure here
	@PQId NUMERIC
	,@DealerId INT
	,@LeadClickSource INT
	,@DealerLeadBusinessType INT
	,@Name varchar(100)=NULL
	,@Email varchar(100)=NULL
	,@Mobile varchar(100)=NULL
	,@CampaignId INT =NULL
	,@AssignedDealerId INT = NULL
	,@CityID INT    --Ashish verma on 18-12-2014 Added 4 new Fields To Sp(CityId,ZoneId,VersionId,PlatformId)
	,@ZoneID INT = NULL
	,@VersionId INT
	,@PlatformId INT
	,@LTSRC VARCHAR(100)=NULL
	,@ResponseId NUMERIC OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @dealerType INT
    -- Insert statements for procedure here
	INSERT INTO PQDealerAdLeads(PQId,DealerId,LeadClickSource,DealerLeadBusinessType,Name,Email,Mobile,AssignedDealerID,CityId,ZoneId,VersionId,PlatformId,LTSRC) values (@PQId,@DealerId,@LeadClickSource,@DealerLeadBusinessType,@Name,@Email,@Mobile,@AssignedDealerId,@CityID,@ZoneID,@VersionId,@PlatformId,@LTSRC)
	--Ashish verma on 18-12-2014 Added 4 new Fields To Sp(CityId,ZoneId,VersionId,PlatformId)
	SET @ResponseId = SCOPE_IDENTITY();
END
