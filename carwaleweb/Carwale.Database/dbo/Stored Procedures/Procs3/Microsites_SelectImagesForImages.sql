IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsites_SelectImagesForImages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsites_SelectImagesForImages]
GO

	--===========================================================
-- Modified By : Nilesh Utture on 25th October, 2013
-- Added Join with CarModels and carMakes to retrieve CarName
-- Modified By : Umesh Ojha on 06-11-2013  for adding order by desc 
-- Modified By : Komal Manjare on 7th August 2015 
-- HostURL and OriginalImgPath fetched
--================================================================
CREATE PROCEDURE [dbo].[Microsites_SelectImagesForImages] 
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
	SELECT 
		MSI.Id,
		MSI.ThumbImage,
		MSI.HostURL + MSI.DirectoryPath + MSI.ThumbImage AS URL, 
		MSI.DirectoryPath + MSI.ThumbImage AS RelativeUrl,
		CMA.Name + ' ' +  CMO.Name AS TagName ,
		MSI.HostURL,MSI.OriginalImgPath
	FROM 
		Microsite_Images AS MSI  WITH (NOLOCK) 
	LEFT JOIN -- Modified By: Nilesh Utture on 25th October, 2013
		CarModels		 AS CMO  WITH (NOLOCK) ON CMO.Id = MSI.ModelId
	LEFT JOIN
		CarMakes		 AS CMA  WITH (NOLOCK) ON CMA.Id = CMO.CarMakeId
	WHERE DealerId= @DealerId AND IsActive = 1  and ThumbImage !=''
	ORDER BY MSI.EntryDate DESC

END