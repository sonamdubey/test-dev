IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetVersionData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetVersionData]
GO

	-- =============================================
-- Author:		Supriya
-- Create date: 9/7/2014												
-- Description:	Get car details for compare cars & get values for items in compare cars
--Exec CD.GetVersionData 2425
-- Approved by Manish on 11-7-2014 04:30 pm
-- Modified By : Supriya On 18/8/2014 to fetch price & smallPic for versionid passed
-- =============================================
CREATE PROCEDURE [CD].[GetVersionData] 
	-- Add the parameters for the stored procedure here
	@VersionId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CMA.Name Make
		,CM.Name Model
		,CV.Name Version
		,CMA.ID MakeId
		,CM.ID ModelId
		,CV.ID VersionId
		,CM.MaskingName
		,CV.largePic Image
		,CV.DirPath ImgPath
		,CV.HostURL HostURL
		,SP.Price
		,CV.smallPic ImageSmall
	FROM CarVersions CV WITH(NOLOCK)
	INNER JOIN CarModels CM WITH(NOLOCK) ON CV.CarModelId = CM.ID
	INNER JOIN CarMakes CMA WITH(NOLOCK) ON CM.CarMakeId = CMA.ID
	LEFT JOIN NewCarShowroomPrices SP WITH (NOLOCK) ON SP.CarVersionId = CV.ID AND SP.CityId = 10
	WHERE CV.ID = @VersionId

	SELECT IV.ItemMasterId
		,CASE IV.DatatypeId
		WHEN 2 THEN
			CASE IV.ItemValue WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE '' END
			ELSE ISNULL(IV.CustomText,'')+ISNULL(CAST(IV.ItemValue AS VARCHAR(20)),'')+ISNULL(UD.Name,'') END Value
	FROM CD.ItemValues IV WITH(NOLOCK)
	LEFT JOIN CD.UserDefinedMaster UD WITH(NOLOCK) ON IV.UserDefinedId = UD.UserDefinedId
	WHERE IV.CarVersionId = @VersionId

	SELECT Color Name, HexCode Value FROM VersionColors WITH(NOLOCK) WHERE IsActive=1 and CarVersionID = @VersionId Order By HexCode

END


