IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMGetCarVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMGetCarVersions]
GO

	-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date,19th Nov, 2013>
-- Description:	<Description,Gives all new car versions with their targets>
-- Modified By:	Nilesh Utture on 28th Nov, 2013 Added parameter @MonthId & @DistributionMasterId
-- =============================================
CREATE PROCEDURE [dbo].[TC_TMGetCarVersions]
	-- Add the parameters for the stored procedure here
	@ModelId SMALLINT,
	@ModelFlag SMALLINT = NULL,
	@DistributionMasterId SMALLINT = NULL,
	@MonthId SMALLINT = NULL
AS
BEGIN
	IF(@ModelFlag IS NULL)
	BEGIN
		SELECT ID AS Value, Name AS Text
		FROM CarVersions  WITH (NOLOCK)  
		WHERE CarModelId = @ModelId AND
		IsDeleted = 0 
		AND Futuristic = 0 
		AND New = 1 
		ORDER BY Text  
	END

	IF(@ModelFlag = 1)
	BEGIN
			--Changed By Deepak on 4th Dec 2013
			IF @DistributionMasterId = -1
				BEGIN
					SELECT CV.ID AS Value, CV.Name AS Text, SUM(DT.Target) AS Target
					FROM CarVersions CV  WITH (NOLOCK) 
					JOIN TC_TMIntermediateLegacyDetail DT  WITH (NOLOCK)  ON DT.CarVersionId = CV.ID AND DT.Month >= @MonthId
					WHERE Cv.CarModelId = @ModelId AND 
					IsDeleted = 0 
					AND Futuristic = 0 
					AND New = 1 
					GROUP BY CV.ID, CV.Name
					ORDER BY Text  
				END
			ELSE
				BEGIN
					SELECT CV.ID AS Value, CV.Name AS Text, SUM(DT.Target) AS Target
					FROM CarVersions CV  WITH (NOLOCK) 
					JOIN TC_TMTargetScenarioDetail DT  WITH (NOLOCK)  ON DT.CarVersionId = CV.ID  AND DT.TC_TMDistributionPatternMasterId = @DistributionMasterId AND DT.Month >= @MonthId
					WHERE Cv.CarModelId = @ModelId AND 
					IsDeleted = 0 
					AND Futuristic = 0 
					AND New = 1 
					GROUP BY CV.ID, CV.Name
					ORDER BY Text  
				END
	END
END
