IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'VW_NCD_Dealers' AND
     DROP VIEW dbo.VW_NCD_Dealers
GO

	CREATE VIEW VW_NCD_Dealers
AS
SELECT 
DealerId,
UserId,
JoiningDate,
IsActive,
NCD_Website,
IsPanelOnly,
Longitude,
Lattitude,
IsPremium
FROM NCD_Dealers WITH (NOLOCK)