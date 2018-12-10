IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwNCS_RManagers' AND
     DROP VIEW dbo.vwNCS_RManagers
GO

	CREATE VIEW  [dbo].[vwNCS_RManagers]
as
select  [Id]
      ,[Name]
      ,[Designation]
      ,[HierId]
      ,[lvl]
      ,[NodeCode]
      ,[ReportTo]
      ,[MakeId]
      ,[MakeGroupId]
      ,[EMail]
      ,[MobileNo]
      ,[LoginId]     
      ,[IsActive]
      ,[UpdatedDate]
      ,[UpdatedBy]
      ,[IsExecutive]
      ,[CityId]
      ,[Type]
      ,[OprUserId]
  FROM [Carwale_com].[dbo].[NCS_RManagers]
