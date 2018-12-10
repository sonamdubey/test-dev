IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMileageByModel_v16_9_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMileageByModel_v16_9_1]
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
					EXEC [dbo].[GetMileageByModel_v16_9_1] 349
*/

-- Modified By: Akansha on 10.4.2014
-- Description : Added Masking Name Column
-- Modified By: Supriya on 25-09-2014 changed ids'for City mileage and highway mileage.
-- Modified By: Meet Shah on 15-09-2016 removed items table and make model select statements
-- =============================================
create PROCEDURE [dbo].[GetMileageByModel_v16_9_1] 
	-- Add the parameters for the stored procedure here
	@ModelId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT IV.CarVersionId, IV.ItemMasterId, COALESCE(CAST(IV.ItemValue AS VARCHAR), UD.Name) AS Value
    FROM CD.ItemValues IV WITH (NOLOCK)
    LEFT JOIN CD.UserDefinedMaster UD WITH (NOLOCK) ON IV.UserDefinedId = UD.UserDefinedId
    LEFT JOIN CarVersions CV WITH (NOLOCK) ON IV.CarVersionId = CV.ID 
    WHERE IV.ItemMasterId IN (14,26,29,261,262,12)
    AND CV.CarModelId = @ModelId AND CV.New = 1
    ORDER BY IV.CarVersionId ASC, IV.ItemMasterId ASC
		
	SELECT CV.ID,AVG(CR.Mileage) MilAvg,MAX(CR.Mileage) MilMax, MIN(CR.Mileage) MilMin 
	FROM CarVersions CV WITH (NOLOCK)
	INNER JOIN CustomerReviews CR WITH (NOLOCK)  ON CV.ID = CR.VersionId
	WHERE CV.CarModelId = @ModelId AND CV.New = 1 AND CR.Mileage BETWEEN 5 AND 30
	GROUP BY CV.ID
END