IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MicroSite_SelectCarPrice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MicroSite_SelectCarPrice]
GO

	
-- =============================================
-- Author:		Umesh Ojha
-- Create date: 24-Jan-2013
-- Description:	Fetching data for car Price and its engine CC (Displacement) (Calculating Insurance)
-- [dbo].[MicroSite_SelectCarPrice] 2616,1
-- =============================================
CREATE PROCEDURE [dbo].[MicroSite_SelectCarPrice]
	-- Add the parameters for the stored procedure here
	@CarVersionId BIGINT,
	@CityId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT P.Price,S.Displacement,IsNull(I.Discount, 0) AS Discount 
	FROM NewCarShowroomPrices P WITH(NOLOCK)
		INNER JOIN NewCarSpecifications S WITH(NOLOCK) ON S.CarVersionId = P.CarVersionId
		INNER JOIN CarVersions V WITH(NOLOCK) ON V.ID = P.CarVersionId
		LEFT OUTER JOIN Con_InsuranceDiscount I WITH(NOLOCK) ON V.CarModelId =I.ModelId AND P.CityId =I.CityId
	WHERE P.CarVersionId = @CarVersionId 
	   AND P.IsActive = 1
	   AND P.CityId=@CityId
END


