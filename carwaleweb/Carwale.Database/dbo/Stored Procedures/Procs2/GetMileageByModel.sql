IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMileageByModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMileageByModel]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 01-10-2013
-- Description:	Get mileage data by model id
-- 
/*
12	Mileage (ARAI)
295	Mileage City
296	Mileage Overall
297	Mileage Highway

26	Fuel Type
14	Displacement
					EXEC [dbo].[GetMileageByModel] 349
*/

-- Modified By: Akansha on 10.4.2014
-- Description : Added Masking Name Column
-- Modified By: Supriya on 25-09-2014 changed ids'for City mileage and highway mileage.
-- =============================================
CREATE PROCEDURE [dbo].[GetMileageByModel] 
	-- Add the parameters for the stored procedure here
	@ModelId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @Items TABLE
	(
		ItemMasterId INT,
		Name VARCHAR(10)		
	)	
     --   dis - displacement
     --   ft - fuel type
     --   tr - transmission type
     --   mlc - mileage city
     --   mlh - mileage highway
     --   mla - mileage arai
	--EXETEREMLY IMPORTANT LINE HERE DO NOT MODIFY ANYTHING HERE
	INSERT INTO @Items (ItemMasterId,Name) VALUES (14,'dis'),(26,'ft'),(29,'tr'),(261,'mlc'),(262,'mlh'),(12,'mla')
	--EXETEREMLY IMPORTANT LINE HERE DO NOT MODIFY ANYTHING HERE
	
	SELECT DISTINCT Make,Model,MaskingName FROM vwMMV WHERE ModelId = @ModelId 
    SELECT * FROM @Items
    -- Insert statements for procedure here 29
    SELECT IV.CarVersionId, IV.ItemMasterId, COALESCE(CAST(IV.ItemValue AS VARCHAR), UD.Name) AS Value
    FROM CD.ItemValues IV WITH (NOLOCK)
    LEFT JOIN CD.UserDefinedMaster UD WITH (NOLOCK) ON IV.UserDefinedId = UD.UserDefinedId
    LEFT JOIN CarVersions CV WITH (NOLOCK) ON IV.CarVersionId = CV.ID 
    WHERE IV.ItemMasterId IN (SELECT ItemMasterId FROM @Items)
    AND CV.CarModelId = @ModelId AND CV.New = 1
    ORDER BY IV.CarVersionId ASC, IV.ItemMasterId ASC
		
	SELECT CV.ID,AVG(CR.Mileage) MilAvg,MAX(CR.Mileage) MilMax, MIN(CR.Mileage) MilMin FROM CarVersions CV WITH (NOLOCK)
	INNER JOIN CustomerReviews CR WITH (NOLOCK)  ON CV.ID = CR.VersionId
	WHERE CV.CarModelId = @ModelId AND CV.New = 1 AND CR.Mileage BETWEEN 5 AND 30
	GROUP BY CV.ID
END