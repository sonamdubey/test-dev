IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMatchingTyresList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMatchingTyresList]
GO

	
-- =======================================================================================
-- Author:		Akansha
-- Create date: 18/03/2013
-- Description:	SP to Get Matching Tyres List based on version
-- // EXEC GetMatchingTyresList 'Coupe','DB9'
-- ========================================================================================
CREATE PROCEDURE [dbo].[GetMatchingTyresList] @Version NVARCHAR(50),
@Model NVARCHAR(50)
AS
SELECT *
FROM Apollo_TyreGuideData
WHERE Version =@Version and Model=@Model


