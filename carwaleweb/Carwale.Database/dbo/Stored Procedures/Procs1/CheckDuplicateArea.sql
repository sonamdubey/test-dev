IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckDuplicateArea]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckDuplicateArea]
GO

	-- =============================================
-- Author:		Manish
-- Create date: 26-12-2012
-- Description:	Alert if any dealer set to inactive and their car is listed in carwale.
-- =============================================
CREATE PROCEDURE [dbo].[CheckDuplicateArea]	
AS
BEGIN
select CityId, CT.Name AS [City Name],A.Name as [Duplicate Area name] 
from areas AS A WITH (NOLOCK)
JOIN Cities AS CT WITH (NOLOCK) ON A.CityId=CT.ID
where A.IsDeleted=0 group by A.Name,A.CityId,CT.Name,A.PinCode having COUNT(*)>1
end
