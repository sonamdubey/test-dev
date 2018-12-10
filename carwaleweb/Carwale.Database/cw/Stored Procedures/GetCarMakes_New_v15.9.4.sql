IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetCarMakes_New_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetCarMakes_New_v15]
GO

	-- =============================================
-- Author:		<Ashish Verma>
-- Create date: <01/04/2014>
-- Description:	<for getting new Car Makes for xml site map generation>
-- Modified By : Satish on 27/5/2014 to comment used=1 filter when type is Used
-- Approved by: Manish Chourasiya on 01-07-2014 06:10 pm 
-- Modified by : Rohan Sapkal on 19-08-2015 16:40 Ordered by PriorityOrder in "New"
-- Modified by : Sachin Bharti on 28-09-2015 added query for road-test makes
-- =============================================
CREATE PROCEDURE [cw].[GetCarMakes_New_v15.9.4]
	-- Add the parameters for the stored procedure here
	@MakeCond VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	IF (@MakeCond = 'New')
		SELECT ID AS Value
			,NAME AS TEXT
			,Mk.LogoUrl
			,Mk.OriginalImgPath
			,Mk.HostURL
		FROM CarMakes Mk WITH (NOLOCK)
		WHERE IsDeleted = 0
			AND Futuristic = 0
			AND New = 1
		ORDER BY PriorityOrder
	ELSE
		IF (@MakeCond = 'Used')
			SELECT Id AS Value
				,NAME AS TEXT
				,Mk.LogoUrl
				,Mk.OriginalImgPath
				,Mk.HostURL
			FROM CarMakes Mk WITH (NOLOCK)
			WHERE IsDeleted = 0
				AND Futuristic = 0
				--AND Used = 1
			ORDER BY NAME
		ELSE
			IF (@MakeCond = 'All')
				SELECT Id AS Value
					,NAME AS TEXT
					,Mk.LogoUrl
					,Mk.OriginalImgPath
					,Mk.HostURL
				FROM CarMakes Mk WITH (NOLOCK)
				WHERE IsDeleted = 0
				ORDER BY NAME
		ELSE 
			IF(@MakeCond = 'RoadTest')
				SELECT 
					DISTINCT Mk.Id AS Value
					,NAME AS TEXT
					,Mk.LogoUrl
					,Mk.OriginalImgPath
					,Mk.HostURL
				FROM CarMakes Mk WITH (NOLOCK)
				INNER JOIN Con_EditCms_Cars CC WITH (NOLOCK) ON CC.MakeId = Mk.ID AND CC.IsActive = 1
				INNER JOIN Con_EditCms_Basic CB WITH (NOLOCK) ON CB.Id = CC.BasicId
				WHERE IsDeleted = 0
					AND Mk.New = 1
					AND CB.CategoryId = 8
					AND CB.IsActive = 1
					AND CB.IsPublished = 1
				ORDER BY NAME
END