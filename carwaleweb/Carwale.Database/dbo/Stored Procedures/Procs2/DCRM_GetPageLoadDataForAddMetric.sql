IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetPageLoadDataForAddMetric]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetPageLoadDataForAddMetric]
GO

	





-- =============================================
-- Author	:	Sachin Bharti(11th May 2015)
-- Description	:	Used to get page load data for 
-- Modifier	:	Sachin Bharti(25th June 2015)
-- Purpose	:	Added new table to get metric target type
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetPageLoadDataForAddMetric]

AS
BEGIN
	
	--get business units
	SELECT 
		BU.Id, BU.Name 
	FROM 
		DCRM_BusinessUnit BU(NOLOCK) 
	WHERE 
		BU.IsActive = 1
		AND Id <> 4
	ORDER BY Name  
	
	--get target type for metric
	SELECT 
		MTT.ID,MTT.Name
	FROM	
		DCRM_MetricTargetType MTT(NOLOCK)

	--get inquiry point those are not mapped yet
	SELECT 
		IPC.Id , 
		IPC.Name
	FROM 
		InquiryPointCategory IPC(NOLOCK) 
		LEFT JOIN DCRM_ExecScoreBoardMetric BM(NOLOCK) ON IPC.Id = BM.InquiryPointId 
	WHERE
		BM.Id IS NULL
	ORDER BY Name 

	--get mapped product data
	SELECT 
		BM.Id AS MetricId,
		BM.MetricName,
		BM.IsActive,
		BU.Name AS BusUnitName,
		BU.Id AS BusUnitId,
		ISNULL(IPC.Name,'') AS Product,
		ISNULL(IPC.Id,-1) AS InqPointId,
		OU.UserName,
		CONVERT(VARCHAR,BM.AddedOn,106) AS AddedOn,
		ISNULL(BM.TargetType,0) AS TargetTypeId,
		DMT.Name AS TargetTypeName
	FROM 
		DCRM_ExecScoreBoardMetric BM(NOLOCK) 
		INNER JOIN DCRM_BusinessUnit BU(NOLOCK) ON BU.Id = BM.BusinessUnitId
		INNER JOIN DCRM_MetricTargetType DMT(NOLOCK) ON DMT.Id = BM.TargetType 
		LEFT JOIN InquiryPointCategory IPC(NOLOCK) ON IPC.Id = BM.InquiryPointId
		INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = BM.AddedBy
	ORDER BY
		BM.AddedOn DESC , BM.IsActive DESC
END
