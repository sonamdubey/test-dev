IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('NCS'))
     name = 'vwRManagers' AND
     DROP VIEW NCS.vwRManagers
GO

	create view [NCS].[vwRManagers]
as
SELECT [Id]
      ,[Name]
      ,[Designation]
      ,[MakeId]
      ,[MakeGroupId]
      ,[ReportToCurrent]
      ,[ReportTo]
      ,[EMail]
      ,[MobileNo]
      ,[LoginId]
      ,[Password]
      ,[NodeCode]
      ,[IsActive]
      ,[UpdatedDate]
      ,[UpdatedBy]
      ,[IsExecutive]
      ,[CityId]
  FROM [Carwale_com].[NCS].[RManagers]