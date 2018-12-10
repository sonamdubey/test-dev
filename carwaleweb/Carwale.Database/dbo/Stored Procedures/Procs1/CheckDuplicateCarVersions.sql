IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckDuplicateCarVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckDuplicateCarVersions]
GO

	-- =============================================
-- Author:		Manish
-- Create date: 26-12-2012
-- Description:	Alert if any dealer set to inactive and their car is listed in carwale.
-- =============================================
CREATE PROCEDURE [dbo].[CheckDuplicateCarVersions]	
AS
BEGIN
select  CarModelId AS [CarModelId],CM.Name AS [Car Model],CV.Name as [Duplicate CarVersion]
from CarVersions AS CV WITH (NOLOCK) 
JOIN CarModels   AS CM WITH (NOLOCK) ON CV.CarModelId=CM.ID
where CV.IsDeleted=0 
and carmodelid not in (234,442)
group by CV.Name,CV.CarModelId,CM.Name having COUNT(*)>1
end