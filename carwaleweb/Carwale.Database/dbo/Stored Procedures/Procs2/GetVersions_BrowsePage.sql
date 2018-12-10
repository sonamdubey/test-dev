IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetVersions_BrowsePage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetVersions_BrowsePage]
GO

	-- =============================================
-- Author:                <Reshma Shetty>
-- Create date: <06/03/2013>
-- Description:        <Get the version details to be displayed on the browse car by versions page> EXEC GetVersions_BrowsePage 386
-- Modified by:Reshma Shetty				Date:9/4/2013			Comment:Added a contraint to restrict futuristic versions
-- Modified By : Ashish G. Kamble on 10 July 2013
-- Modified : showing prices from delhi only.
-- =============================================
CREATE PROCEDURE   [dbo].[GetVersions_BrowsePage]
       -- Add the parameters for the stored procedure here
      @ModelId INT,
      @CityId BIGINT = NULL
AS
BEGIN
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

   -- Insert statements for procedure here
       SELECT CV.ID AS ID, CV.Name AS Version, CV.CarModelId AS ModelId,
       CV.SpecsSummary,cmo.MaskingName,
       --AvgPrice AS MinPrice, CV.New, IsNull(CV.ReviewRate, 0) AS ReviewRate,
       CV.New, IsNull(CV.ReviewRate, 0) AS ReviewRate,
       SP.Price AS MinPrice,
       IsNull(CV.ReviewCount, 0) AS ReviewCount
       FROM CarVersions AS CV
	   Inner Join carmodels cmo on cv.CarModelId=cmo.ID
       --LEFT JOIN Con_NewCarNationalPrices NCN ON NCN.VersionId=CV.ID
       LEFT JOIN NewCarShowroomPrices AS SP ON SP.CarVersionId = CV.ID AND SP.CityId = @CityId
       WHERE CV.IsDeleted=0 AND CV.IsSpecsExist=1
       AND CV.Futuristic=0 
       AND CV.CarModelId=@ModelId
       ORDER BY MinPrice ASC
END
