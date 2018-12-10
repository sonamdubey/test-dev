IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPqCitiesByFilter_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPqCitiesByFilter_v16]
GO

	
    

-- =============================================
-- Author:		Shalini Nair	
-- Create date: 03/11/2015
-- Description:	Returns the PQ cities based on model, state and make
--				StateOrCity parameter : 1 for returning cities
--									  : 2 for returning states
-- exec GetPqCitiesByFilter '516',null,1,null,1
-- Modified By : Shalini Nair on 28/12/2015 to retrieve statename also and changed left joins to inner join
-- Modified By : Vicky Lund on 18/05/2016 to convert stateId to multiple stateId (csv)
-- =============================================
CREATE PROCEDURE [dbo].[GetPqCitiesByFilter_v16.5.1]
	-- Add the parameters for the stored procedure here
	@ModelId VARCHAR(100) = NULL
	,@VersionId VARCHAR(100) = NULL
	,@StateId VARCHAR(500) = NULL
	,@MakeId INT = NULL
	,@StateOrCity INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @StateOrCity = 1 -- resultset cities
		-- Insert statements for procedure here
	BEGIN
		SELECT DISTINCT CT.ID AS Value
			,CT.NAME AS [Text]
			,st.NAME AS StateName
		FROM Cities CT WITH (NOLOCK)
		INNER JOIN NewCarShowroomPrices NCP WITH (NOLOCK) ON NCP.CityId = ct.ID
		INNER JOIN CarVersions CV WITH (NOLOCK) ON NCP.CarVersionId = CV.ID
		INNER JOIN States st WITH (NOLOCK) ON st.ID = CT.StateId
		INNER JOIN carmodels cmo WITH (NOLOCK) ON cmo.ID = cv.CarModelId
		INNER JOIN CarMakes cm WITH (NOLOCK) ON cm.ID = cmo.CarMakeId
		WHERE CV.New = 1
			AND NCP.IsActive = 1
			AND CV.IsDeleted = 0
			AND CM.IsDeleted = 0
			AND CMO.IsDeleted = 0
			AND (
				@StateId IS NULL
				OR st.ID IN (
					SELECT ListMember
					FROM fnSplitCSVValuesWithIdentity(@StateId)
					)
				)
			AND (
				@VersionId IS NULL
				OR cv.ID IN (
					SELECT ListMember
					FROM fnSplitCSVValuesWithIdentity(@VersionId)
					)
				)
			AND (
				@ModelId IS NULL
				OR cv.CarModelId IN (
					SELECT ListMember
					FROM fnSplitCSVValuesWithIdentity(@ModelId)
					)
				)
			AND (
				@MakeId IS NULL
				OR CMO.CarMakeId = @MakeId
				)
		ORDER BY ct.NAME
	END
	ELSE
	BEGIN
		IF @StateOrCity = 2
		BEGIN
			SELECT DISTINCT st.ID AS Value
				,st.NAME AS [Text]
			FROM Cities CT WITH (NOLOCK)
			INNER JOIN NewCarShowroomPrices NCP WITH (NOLOCK) ON NCP.CityId = ct.ID
			INNER JOIN CarVersions CV WITH (NOLOCK) ON NCP.CarVersionId = CV.ID
			INNER JOIN States st WITH (NOLOCK) ON st.ID = CT.StateId
			INNER JOIN carmodels cmo WITH (NOLOCK) ON cmo.ID = cv.CarModelId
			INNER JOIN CarMakes cm WITH (NOLOCK) ON cm.ID = cmo.CarMakeId
			WHERE CV.New = 1
				AND CV.IsDeleted = 0
				AND CM.IsDeleted = 0
				AND CMO.IsDeleted = 0
				AND NCP.IsActive = 1
				AND (
					@VersionId IS NULL
					OR cv.ID IN (
						SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@VersionId)
						)
					)
				AND (
					@ModelId IS NULL
					OR cv.CarModelId IN (
						SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@ModelId)
						)
					)
				AND (
					@MakeId IS NULL
					OR CMO.CarMakeId = @MakeId
					)
			ORDER BY st.NAME
		END
	END
END
