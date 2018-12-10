IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_GetPQCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_GetPQCities]
GO

	-- =============================================
-- Author:		<Chetan Kane>
-- Create date: <2/10/2012>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[NCD_GetPQCities] 
	-- Add the parameters for the stored procedure here
	(
	@DealerId int 
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT Ct.Id AS Value, Ct.Name AS Text
    FROM NCD_DealerCities Dc INNER JOIN Cities Ct ON Ct.ID = Dc.CityID
    INNER JOIN Dealer_NewCar Ds ON Ds.CityId = Dc.RegionId
    Where Ds.Id = @DealerID  ORDER BY Text
END
