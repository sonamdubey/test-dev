USE [CarWale]
GO
/****** Object:  StoredProcedure [dbo].[GetModelDetailsForSiteMap]    Script Date: 12/28/2016 3:10:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelDetailsForSiteMap]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelDetailsForSiteMap]
GO
-- =============================================
-- Author:		Meet Shah
-- Create date: 27 Dec 2016
-- Description:	To get data for car models sitemap
-- =============================================
CREATE PROCEDURE [dbo].[GetModelDetailsForSiteMap] 
	-- Add the parameters for the stored procedure here
	@MakeId SMALLINT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ID,Name,HostURL,MaskingName,OriginalImgPath from CarModels WITH(NOLOCK) WHERE (IsDeleted = 0 AND CarMakeId = @MakeId)
END
