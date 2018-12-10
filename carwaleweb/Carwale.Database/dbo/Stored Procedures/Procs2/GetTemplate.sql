IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTemplate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTemplate]
GO

	-- =============================================
-- Author:		Sourav Roy						EXEC GetAllVersionsOnRoadPrice 99,13
-- Create date: 14/10/2015
-- Description:	Get Template from gtemplate Id
-- =============================================
CREATE PROCEDURE [dbo].[GetTemplate]
	-- Add the parameters for the stored procedure here
	@TemplateId INT
AS
BEGIN
	SELECT Template from PQ_SponsoredDealeAd_Templates WITH (NOLOCK) where TemplateId=@TemplateId
END

