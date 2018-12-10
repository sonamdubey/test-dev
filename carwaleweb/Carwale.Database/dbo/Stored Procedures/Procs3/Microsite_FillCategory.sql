IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_FillCategory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_FillCategory]
GO

	-- =============================================
-- Author:		<Chetan Kane>
-- Create date: <21/02/2012>
-- Description:	<To fetch content catagories for filling dropdown>
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_FillCategory]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id,CatagoryName from Microsite_ContentCatagories WHERE IsActive = 1
END