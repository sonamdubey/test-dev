USE [CarWale]
GO
/****** Object:  StoredProcedure [dbo].[GetCitiesForSiteMap]    Script Date: 12/29/2016 12:02:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCitiesForSiteMap]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCitiesForSiteMap]
GO
-- =============================================
-- Author:		Meet Shah
-- Create date: 27 Dec 2016
-- Description:	To get city data for price in city sitemap
-- =============================================
CREATE PROCEDURE [dbo].[GetCitiesForSiteMap] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT NAME,CityMaskingName from Cities WITH(NOLOCK) WHERE IsDeleted = 0
END
