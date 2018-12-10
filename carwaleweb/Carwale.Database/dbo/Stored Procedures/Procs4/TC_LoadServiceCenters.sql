IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LoadServiceCenters]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LoadServiceCenters]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 16-10-2015
-- Description:	It shows all serivce centers of the dealer
-- EXEC TC_LoadServiceCenters 5,0
-- =============================================
CREATE PROCEDURE [dbo].[TC_LoadServiceCenters] @BranchId INT
	,@ServiceCenterId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT SC.TC_ServiceCenterId
		,SC.ServiceCenterName
		,SC.Address
		,C.NAME AS City
		,SC.PhoneNo
		,SC.CityId
		,ISNULL(SC.AreaId, 0) AreaId
		,C.StateId
		,SC.ZipCode
		,SC.EmailId
		,ISNULL(A.Lattitude, 0) AS Lattitude
		,ISNULL(A.Longitude, 0) AS Longitude
		,A.Name AS AreaName
	FROM TC_ServiceCenter SC WITH (NOLOCK)
	JOIN Cities C WITH (NOLOCK) ON SC.CityId = C.ID
	LEFT JOIN Areas A WITH (NOLOCK) ON A.ID = SC.AreaId
	WHERE SC.BranchId = @BranchId
		AND (
			@ServiceCenterId = 0
			OR SC.TC_ServiceCenterId = @ServiceCenterId
			)
		AND SC.IsActive = 1
	ORDER BY CreatedDate DESC
END
