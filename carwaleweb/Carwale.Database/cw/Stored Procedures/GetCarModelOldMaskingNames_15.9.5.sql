IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetCarModelOldMaskingNames_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetCarModelOldMaskingNames_15]
GO

	
-- =============================================
-- Author:		<Author,Prasad Gawde>
-- Create date: <Create Date,,>
-- Description:	<Modified GetCarModelOldMaskingNames to retrieve CarMakeName in the same SQL>
-- =============================================

CREATE PROCEDURE [cw].[GetCarModelOldMaskingNames_15.9.5]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   	select ML.ModelId,ML.MaskingName, CM.CarMakeId, Mk.Name MakeName  from MaskingNameUpdateLog ML with(nolock)
	INNER JOIN CarModels CM WITH(NOLOCK) ON ML.ModelId = CM.ID
	INNER JOIN CarMakes Mk with (nolock) on CM.CarMakeId = Mk.ID 
	WHERE Mk.IsDeleted=0;
END
