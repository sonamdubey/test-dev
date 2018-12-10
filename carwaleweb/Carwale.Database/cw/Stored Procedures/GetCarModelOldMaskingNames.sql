IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetCarModelOldMaskingNames]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetCarModelOldMaskingNames]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [cw].[GetCarModelOldMaskingNames]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	select ML.ModelId,ML.MaskingName, CM.CarMakeId from MaskingNameUpdateLog ML with(nolock)
	INNER JOIN CarModels CM WITH(NOLOCK) ON ML.ModelId = CM.ID
END
