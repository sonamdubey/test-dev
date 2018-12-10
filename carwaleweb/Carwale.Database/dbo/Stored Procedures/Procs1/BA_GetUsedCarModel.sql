IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_GetUsedCarModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_GetUsedCarModel]
GO

	-- =============================================
-- Author:		Ranjeet
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BA_GetUsedCarModel] 
	@MakeId Int
AS
BEGIN
	SET NOCOUNT ON;

	select CM.ID, CM.Name,CM.SmallPic, CM.LargePic 
	from CarModels AS CM  WITH (NOLOCK) where CM.Used = 1 AND  CM.IsDeleted =0 AND CM.CarMakeId = @MakeId
END
