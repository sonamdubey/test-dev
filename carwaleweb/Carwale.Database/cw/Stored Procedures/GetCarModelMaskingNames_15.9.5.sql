IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetCarModelMaskingNames_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetCarModelMaskingNames_15]
GO

	

-- =============================================
-- Author:		<Author,Prasad Gawde>
-- Create date: <Create Date,,>
-- Description:	<Modified GetCarModelMaskingNames to retrieve CarMakeName in the same SQL>
-- =============================================

CREATE  PROCEDURE [cw].[GetCarModelMaskingNames_15.9.5] 
	-- Add the parameters for the stored procedure here
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select CMO.id ModelId,CMO.MaskingName,CMO.CarMakeId CarMakeId , Mk.Name MakeName
from CarModels CMO with(nolock) 
inner join CarMakes Mk with (nolock) on CMO.CarMakeId = Mk.ID
where CMO.IsDeleted = 0 and Mk.IsDeleted=0;
END

