IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckDuplicateCityName]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckDuplicateCityName]
GO

	-- =============================================
-- Author:		Manish
-- Create date: 26-12-2012
-- Description:	Check the Duplicate city name in master
-- =============================================
CREATE  PROCEDURE [dbo].[CheckDuplicateCityName]	
AS
BEGIN
select CT.StateId,ST.Name AS [State Name],CT.Name [Dulicate city name in CityMaster] 
FROM Cities  AS CT WITH (NOLOCK) 
join States  AS ST  WITH (NOLOCK) ON ST.ID=CT.StateId
WHERE  CT.IsDeleted=0 
group by CT.Name,CT.StateId,ST.Name having COUNT(*)>1
END