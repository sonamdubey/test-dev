IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMilageByModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMilageByModel]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- 
/*
12	Mileage (ARAI)
295	Mileage City
296	Mileage Overall
297	Mileage Highway

26	Fuel Type
14	Displacement
					EXEC [dbo].[GetMilageByModel] 269
*/
-- =============================================
CREATE PROCEDURE [dbo].[GetMilageByModel] 
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
	INSERT INTO @Items (ItemMasterId,Name) VALUES (14,'dis'),(26,'ft'),(29,'tr'),(295,'mlc'),(297,'mlh'),(12,'mla')
	--EXETEREMLY IMPORTANT LINE HERE DO NOT MODIFY ANYTHING HERE
	
	SELECT DISTINCT Make,Model FROM vwMMV WHERE ModelId = @ModelId 
    SELECT * FROM @Items
    -- Insert statements for procedure here 29
    SELECT IV.CarVersionId, IV.ItemMasterId, COALESCE(CAST(IV.ItemValue AS VARCHAR), UD.Name) AS Value
    FROM CD.ItemValues IV WITH (NOLOCK)
    LEFT JOIN CD.UserDefinedMaster UD WITH (NOLOCK) ON IV.UserDefinedId = UD.UserDefinedId
    LEFT JOIN CarVersions CV WITH (NOLOCK) ON IV.CarVersionId = CV.ID 
    WHERE IV.ItemMasterId IN (SELECT ItemMasterId FROM @Items)
    AND CV.CarModelId = @ModelId AND CV.New = 1
    ORDER BY IV.CarVersionId ASC, IV.ItemMasterId ASC
		
	SELECT CV.ID,AVG(CR.Mileage) MilAvg,MAX(CR.Mileage) MilMax, MIN(CR.Mileage) MilMin FROM CarVersions CV  WITH (NOLOCK)
	INNER JOIN CustomerReviews CR  WITH (NOLOCK) ON CV.ID = CR.VersionId
	WHERE CV.CarModelId = @ModelId AND CV.New = 1
	GROUP BY CV.ID
END

/*
								EXEC [dbo].[GetMilageByModel] 269
								
*/
