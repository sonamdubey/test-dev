IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Failed_PQDealerInquiryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Failed_PQDealerInquiryDetails]
GO

	-- =============================================
-- Author:		Vinayak
-- Create date: 07/08/2014
-- Description:	To save Failed PQ dealer add inquiry details before calling to api
-- =============================================
CREATE PROCEDURE [dbo].[Failed_PQDealerInquiryDetails] 
	-- Add the parameters for the stored procedure here
	@PQId INT=0
	,@LeadClickSource INT=0
	,@Name varchar(100)=NULL
	,@Email varchar(100)=NULL
	,@Mobile varchar(100)=NULL
	,@CampaignId INT =NULL
	,@AssignedDealerId INT = NULL
	,@CityId INT =0   
	,@ZoneId INT = 0
	,@VersionId INT=0
	,@PlatformId INT=0
	,@Comment VARCHAR(500)=NULL
	,@LTSRC VARCHAR(100)=NULL
	,@ErrorMsg VARCHAR(500)=NULL
	,@ModelId INT=0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	INSERT INTO Failed_PQDealerAdLeads(PQId,LeadClickSource,Name,Email,Mobile,AssignedDealerID,CityId,ZoneId,VersionId,ModelId,PlatformId,LTSRC,Comment,CampaignId,ErrorMsg,RequestDateTime) 
	                   values (@PQId,@LeadClickSource,@Name,@Email,@Mobile,@AssignedDealerId,@CityID,@ZoneID,@VersionId,@ModelId,@PlatformId,@LTSRC,@Comment,@CampaignId,@ErrorMsg,GETDATE())
END