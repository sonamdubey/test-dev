IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetFeaturedModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetFeaturedModels]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 29 AUG 2013
-- Description:	GET FEATURED MODELS
-- =============================================
CREATE PROCEDURE [dbo].[GetFeaturedModels]
	-- Add the parameters for the stored procedure here
	@MakeID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT DISTINCT CM.Id, CM.Name FROM CarModels CM WITH(NOLOCK)
    JOIN FeaturedCarsTrackingCode FCT WITH(NOLOCK) ON CM.ID = FCT.ModelID AND FCT.MakeId = @MakeID
END