IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FLCRulesDisplay]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FLCRulesDisplay]
GO

	CREATE PROCEDURE [dbo].[CRM_FLCRulesDisplay]
	@queueId		INT
AS
BEGIN

	SELECT CAF.Id, CAQ.Name AS QueueName,MK.Name AS Make, OU.UserName, CAF.UpdatedOn,
		CASE WHEN CAF.ModelId = -1 THEN  'All Models' ELSE MO.Name END AS Model, 
		CASE WHEN CAF.CityId = -1 THEN  'All Cities' ELSE C.Name END AS Cities,
		CASE WHEN CAF.SourceId = -1 THEN  'All Sources' ELSE LA.Organization END AS Organization,
		ISNULL(ST.Name,'All States') AS State,
		CAF.Rank AS Priority
	FROM CRM_ADM_FLCRules CAF 
		INNER JOIN CRM_ADM_FLCGroups CAQ ON CAF.GroupId=CAQ.Id
		INNER JOIN CarMakes MK ON CAF.MakeId= MK.ID
		INNER JOIN OprUsers OU ON OU.Id = CAF.UpdatedBy
		LEFT JOIN CarModels MO ON CAF.ModelId = MO.ID
		LEFT JOIN Cities C ON CAF.CityId=C.ID
		LEFT JOIN States AS ST (NOLOCK) ON ST.ID = C.StateId
		LEFT JOIN LA_Agencies LA ON CAF.SourceId= LA.Id
	WHERE CAF.GroupId = @queueId
END
