IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetCarModelMaskingNames]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetCarModelMaskingNames]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [cw].[GetCarModelMaskingNames]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select CMO.id ModelId,CMO.MaskingName,CarMakeId from CarModels CMO with(nolock) where IsDeleted = 0

END
