IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[RateCallHeadBind]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[RateCallHeadBind]
GO

	CREATE PROCEDURE [CRM].[RateCallHeadBind]
@RoleId NUMERIC (18,0)

AS 
BEGIN
 SELECT Id,HeadName,TotalWeight,CreatedOn FROM CRM.QAHeads WITH(NOLOCK) WHERE RoleId=@RoleId AND IsActive=1
END