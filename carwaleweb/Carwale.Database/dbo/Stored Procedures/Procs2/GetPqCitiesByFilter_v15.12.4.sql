IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPqCitiesByFilter_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPqCitiesByFilter_v15]
GO

	-- =============================================
-- Author:		Shalini Nair	
-- Create date: 03/11/2015
-- Description:	Returns the PQ cities based on model, state and make
--				StateOrCity parameter : 1 for returning cities
--									  : 2 for returning states
-- exec GetPqCitiesByFilter '516',null,1,null,1
-- Modified By : Shalini Nair on 28/12/2015 to retrieve statename also and changed left joins to inner join
-- =============================================
CREATE PROCEDURE [dbo].[GetPqCitiesByFilter_v15.12.4]
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
				,st.Name as StateName
			FROM Cities CT with(nolock)
			INNER JOIN NewCarShowroomPrices NCP with(nolock) ON NCP.CityId = ct.ID
			INNER JOIN CarVersions CV with(nolock) ON NCP.CarVersionId = CV.ID
			INNER JOIN States st with(nolock) ON st.ID = CT.StateId
			INNER JOIN carmodels cmo with(nolock) on cmo.ID = cv.CarModelId 
			INNER JOIN CarMakes cm with(nolock) on cm.ID = cmo.CarMakeId
			WHERE CV.New = 1
				AND NCP.IsActive = 1
				AND CV.IsDeleted=0
				AND CM.IsDeleted=0
				AND CMO.IsDeleted=0
				AND st.ID = @StateId
				AND (@VersionId IS NULL OR cv.ID IN (
						SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@VersionId)
						))
				AND (@ModelId IS NULL OR cv.CarModelId IN (
						SELECT ListMember
						FROM fnSplitCSVValuesWithIdentity(@ModelId)
						))
				AND (@MakeId is null OR CMO.CarMakeId =@MakeId)
		
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
			INNER JOIN carmodels cmo with(nolock) on cmo.ID = cv.CarModelId 
			INNER JOIN CarMakes cm with(nolock) on cm.ID = cmo.CarMakeId
			WHERE CV.New = 1
			    AND CV.IsDeleted=0
				AND CM.IsDeleted=0
				AND CMO.IsDeleted=0
				AND NCP.IsActive = 1
				AND (@VersionId IS NULL OR cv.ID IN (
							SELECT ListMember
							FROM fnSplitCSVValuesWithIdentity(@VersionId)
							))
				AND (@ModelId IS NULL OR cv.CarModelId IN (
							SELECT ListMember
							FROM fnSplitCSVValuesWithIdentity(@ModelId)
							))
				AND (@MakeId is null OR CMO.CarMakeId =@MakeId)
		
				ORDER BY st.Name
		END 
	END
END