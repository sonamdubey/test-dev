IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewOnRoadPrice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewOnRoadPrice]
GO

	-- =============================================
-- Author:		amit verma						EXEC [dbo].[GetNewOnRoadPrice] 6865--,776,13
-- Create date: 14/10/2013
-- Description:	get new on-road price
-- Modified by : Raghu on 23.12.2013 getting only 2 datatable's , commented out pricequote datatable and getting Hosturl and smallpic
-- Modified By : Akansha on 06.03.2014 Added Masking Name Column
-- =============================================
CREATE PROCEDURE [dbo].[GetNewOnRoadPrice] 
	-- Add the parameters for the stored procedure here
	@PQId numeric(18,0)
	--,@CarversionId INT
	--,@CityId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @PQCarversionId INT
	DECLARE @PQCityId INT
	DECLARE @PQCityName VARCHAR(50)

	SELECT @PQCarversionId = NCP.CarVersionId, @PQCityId = NPC.CityId, @PQCityName = npc.City
	FROM NewCarPurchaseInquiries NCP WITH(NOLOCK)
	INNER JOIN NewPurchaseCities NPC WITH(NOLOCK) ON NCP.Id = NPC.InquiryId
	WHERE Id = @PQId


	IF(@PQCarversionId > 0 AND @PQCityId > 0)
	BEGIN
		DECLARE @PQCarModelId INT = (SELECT CarModelId FROM CarVersions WHERE ID = @PQCarversionId)

		SELECT Mk.Id MakeId, Mk.Name MakeName, Mo.Id ModelId, Mo.Name ModelName, Mo.MaskingName, Vs.ID VersionId, Vs.Name VersionName,
		Vs.New, Vs.LargePic,Vs.smallPic, @PQCityName AS Name,@PQCityId AS CityId,Vs.HostURL
		FROM CarVersions Vs WITH(NOLOCK)
		INNER JOIN CarModels Mo WITH(NOLOCK) ON Vs.CarModelId = Mo.ID
		INNER JOIN CarMakes Mk WITH(NOLOCK) ON MO.CarMakeId = MK.ID
		WHERE Vs.ID = @PQCarversionId

		EXEC GetAllVersionsOnRoadPrice @PQCarModelId,@PQCityId
	END
END
