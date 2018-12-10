USE [CarWale]
GO
/****** Object:  StoredProcedure [dbo].[GetVersionDetailsForSiteMap]    Script Date: 12/28/2016 3:10:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetVersionDetailsForSiteMap]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetVersionDetailsForSiteMap]
GO
-- =============================================
-- Author:		Meet Shah
-- Create date: 27 Dec 2016
-- Description:	To get version data for car versions sitemap
-- =============================================
CREATE PROCEDURE [dbo].[GetVersionDetailsForSiteMap] 
	-- Add the parameters for the stored procedure here
	@ModelId SMALLINT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ID,Name,HostURL,MaskingName,OriginalImgPath from CarVersions WITH(NOLOCK) WHERE (IsDeleted = 0 AND CarModelId = @ModelId)
END
