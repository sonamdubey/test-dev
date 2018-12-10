IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MicroSite_SelectColors]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MicroSite_SelectColors]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 17-Feb-2011
-- Description:	Fetching data for car features
-- =============================================
CREATE PROCEDURE [dbo].[MicroSite_SelectColors]
	-- Add the parameters for the stored procedure here
	@CarVersionId BigInt
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM VersionColors WHERE IsActive=1 AND CarVersionId=@CarVersionId Order By HexCode
END