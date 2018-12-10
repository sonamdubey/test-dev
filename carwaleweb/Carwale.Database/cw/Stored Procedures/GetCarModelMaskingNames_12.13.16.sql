IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetCarModelMaskingNames_12.13.16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetCarModelMaskingNames_12.13.16]
GO
-- =============================================
-- Author:		<Author,Prasad Gawde>
-- Create date: <Create Date,11/1/2017,>
-- Description:	<Modified GetCarModelMaskingNames to retrieve CarMakeName in the same SQL>
-- Modified by Garule Prabhudas , Fetch RootName.
-- =============================================
CREATE PROCEDURE [cw].[GetCarModelMaskingNames_12.13.16] 
	-- Add the parameters for the stored procedure here
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select CMO.id ModelId,CMO.MaskingName,CMO.CarMakeId CarMakeId , Mk.Name MakeName,CMR.RootName
from CarModels CMO with(nolock) 
inner join CarMakes Mk with (nolock) on CMO.CarMakeId = Mk.ID
inner join CarModelRoots CMR with (nolock) on CMO.RootId = CMR.RootId
where CMO.IsDeleted = 0 and Mk.IsDeleted=0;
END
