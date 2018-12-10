IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[QAScoreWithSubHead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[QAScoreWithSubHead]
GO

	CREATE PROCEDURE [CRM].[QAScoreWithSubHead]
@callDataId		NUMERIC(18,0),		
@roleId		NUMERIC(18,0)
AS
BEGIN
	
	SELECT QH.Id,HeadName, TotalWeight, QSH.Id AS SubheadId, QSH.SubheadName, QSH.Weight, QSH.Type,
	QCS.Score AS QAScore,  QCS.Id AS ScoreSaveId, ISNULL(SUM(QCS.Score)  OVER (PARTITION BY QH.Id),0) AS PresentScore
	FROM CRM.QAHeads QH WITH(NOLOCK) INNER JOIN CRM.QASubhead QSH WITH (NOLOCK) ON QH.Id = QSH.HeadId
	LEFT JOIN CRM.QACallScore QCS WITH(NOLOCK) ON QCS.SubheadId = QSH.Id 
	AND QCS.QACallDataId =@callDataId
	WHERE QH.IsActive=1 AND QSH.IsActive = 1 AND QH.RoleId = @roleId
END

