IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPqCitiesByFilter]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPqCitiesByFilter]
GO

	
-- =============================================
-- Author:		Shalini Nair	
-- Create date: 03/11/2015
-- Description:	Returns the PQ cities based on model, state and make
--				StateOrCity parameter : 1 for returning cities
--									  : 2 for returning states
-- exec GetPqCitiesByFilter '516',null,1,null,1
-- =============================================
CREATE PROCEDURE [dbo].[GetPqCitiesByFilter]
	-- Add the parameters for the stored procedure here
	@ModelId varchar(100) = NULL ,
	@VersionId varchar(100) = NULL,
	@StateId INT = NULL,
	@MakeId INT = NULL,
	@StateOrCity INT

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @StateOrCity = 1 -- resultset cities
    -- Insert statements for procedure here
		BEGIN
			SELECT DISTINCT CT.ID AS Value
				,CT.NAME AS Text
			FROM Cities CT with(nolock)
			INNER JOIN NewCarShowroomPrices NCP with(nolock) ON NCP.CityId = ct.ID
			INNER JOIN CarVersions CV with(nolock) ON NCP.CarVersionId = CV.ID
			INNER JOIN States st with(nolock) ON st.ID = CT.StateId
			LEFT JOIN carmodels cmo with(nolock) on cmo.ID = cv.CarModelId 
			LEFT JOIN CarMakes cm with(nolock) on cm.ID = cmo.CarMakeId
			WHERE CV.New = 1
				AND NCP.IsActive = 1
				AND st.ID = @StateId
				AND (@VersionId IS NULL OR cv.ID IN (
						SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@VersionId)
						))
				AND (@ModelId IS NULL OR cv.CarModelId IN (
						SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@ModelId)
						))
				AND (@MakeId is null OR CMO.CarMakeId IN (@MakeId))
		
				ORDER BY ct.Name
		END
	ELSE
		BEGIN
		IF @StateOrCity = 2
			BEGIN
				SELECT DISTINCT st.ID AS Value
				,st.NAME AS Text
			FROM Cities CT with(nolock)
			INNER JOIN NewCarShowroomPrices NCP with(nolock) ON NCP.CityId = ct.ID
			INNER JOIN CarVersions CV with(nolock) ON NCP.CarVersionId = CV.ID
			INNER JOIN States st with(nolock) ON st.ID = CT.StateId
			LEFT JOIN carmodels cmo with(nolock) on cmo.ID = cv.CarModelId 
			LEFT JOIN CarMakes cm with(nolock) on cm.ID = cmo.CarMakeId
			WHERE CV.New = 1
				AND NCP.IsActive = 1
				AND (@VersionId IS NULL OR cv.ID IN (
							SELECT ListMember
							FROM fnSplitCSVValuesWithIdentity(@VersionId)
							))
				AND (@ModelId IS NULL OR cv.CarModelId IN (
							SELECT ListMember
							FROM fnSplitCSVValuesWithIdentity(@ModelId)
							))
				AND (@MakeId is null OR CMO.CarMakeId IN (@MakeId))
		
				ORDER BY st.Name
		END 
	END
END

