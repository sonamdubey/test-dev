IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LogThirdPartyInquiryDetails_V15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LogThirdPartyInquiryDetails_V15]
GO

	
-- =============================================
-- Author:		Vinayak
-- Create date: 16/11/15
-- =============================================
CREATE PROCEDURE [dbo].[LogThirdPartyInquiryDetails_V15.11.7] 
	-- Add the parameters for the stored procedure here
	 @Name varchar(100)=NULL
	,@Email varchar(150)=NULL
	,@Mobile varchar(15)=NULL
	,@CityId INT    --Ashish verma on 18-12-2014 Added 4 new Fields To Sp(CityId,ZoneId,VersionId,PlatformId)
	,@VersionId INT=NULL
	,@ModelId INT=NULL
	,@PlatformId INT=NULL
	,@PartnerSourceId INT=NULL
	,@ResponseId INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
		INSERT INTO CarTradeNewCarLeads(ModelId,VersionId,CityId,PlatformSourceId,PartnerSourceId,Name,Mobile,Email,RequestDateTime)
		values (@ModelId,@VersionId,@CityId,@PlatformId,@PartnerSourceId,@Name,@Mobile,@Email,GETDATE())
		SET @ResponseId = SCOPE_IDENTITY();
END
