IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetBmwModelVersionId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetBmwModelVersionId]
GO

	-- =============================================
-- Author:		<Author,,Ashish Verma>
-- Create date: <Create Date,,14/01/2014>
-- Description:	<Description,,For Getting Bmw cars Variant and Model Code base on versionId and modelId>
-- =============================================
CREATE PROCEDURE [dbo].[GetBmwModelVersionId]
	-- Add the parameters for the stored procedure here
	@VersionId INT,
	@ModelId INT,
	@MGUId VARCHAR(100) OUTPUT,
	@VGUId VARCHAR(100) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @MGUId = MGUID,@VGUId =VGUID FROM [BmwModelsvariants] where VersionId = @VersionId AND ModelId = @ModelId
END
