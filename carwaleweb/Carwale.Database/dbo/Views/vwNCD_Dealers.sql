IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwNCD_Dealers' AND
     DROP VIEW dbo.vwNCD_Dealers
GO

	CREATE VIEW [dbo].[vwNCD_Dealers]
AS
select DealerId	,
JoiningDate	,
IsActive	,
NCD_Website	,
IsPanelOnly	,
Longitude	,
Lattitude	
 FROM NCD_Dealers
