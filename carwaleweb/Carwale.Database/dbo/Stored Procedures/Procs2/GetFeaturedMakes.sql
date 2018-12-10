IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetFeaturedMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetFeaturedMakes]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 29 AUG 2013
-- Description:	GET FEATURED MAKES
-- =============================================
CREATE PROCEDURE [dbo].[GetFeaturedMakes] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
     SELECT distinct CM.Id, CM.Name 
	 FROM CarMakes CM WITH (NOLOCK)
	 JOIN FeaturedCarsTrackingCode FCT WITH (NOLOCK) ON CM.Id=FCT.MakeId
END