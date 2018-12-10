IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CWAwards_GetPreviousData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CWAwards_GetPreviousData]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 24-Jan-2013
-- Description:	To get previously filled data about awards as per the Survey Id
-- =============================================
CREATE PROCEDURE [dbo].[CWAwards_GetPreviousData]
	-- Add the parameters for the stored procedure here
	@SurveyId	NUMERIC
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CA.FirstName, CA.Email, CA.PhoneNo, CA.CarMakedId, CA.CarModelId, CA.CarVersionId, CA.FamiliarityOfCar,
		CA.RecommendPoint, CA.Mileage, CA.Economy, CA.Exterior, CA.Comfort, CA.Performance, CA.ValueForMoney,
		CA.Reliability, CA.Braking, CA.Handling, CA.RideQuality, CA.Design, CA.BuildQuality, CA.Interior, CA.EaseofDriving, CA.SafetyFeature
	FROM CWAwards CA
		WHERE SurveyId = @SurveyId
END
