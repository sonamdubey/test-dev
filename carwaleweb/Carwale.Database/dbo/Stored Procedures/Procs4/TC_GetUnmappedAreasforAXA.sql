IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetUnmappedAreasforAXA]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetUnmappedAreasforAXA]
GO

	-- =============================================
-- Author:        Ruchira Patil
-- Create date: 10th Aug 2015
-- Description:    Bind unmapped areas
-- =============================================
CREATE PROCEDURE TC_GetUnmappedAreasforAXA
    @CityId INT
AS
BEGIN
    SELECT A.ID AS Value,A.Name AS Text FROM Areas A
    WHERE ID NOT IN (SELECT AreaId FROM TC_UserAreaMapping) AND
    CityId=@CityId AND A.IsDeleted=0
END