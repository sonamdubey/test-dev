IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetNCDMetrics]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetNCDMetrics]
GO

	
-- =============================================
-- Author:		Amit Yadav
-- Create date: 25 Feb 2016
-- Description:	To get the NCD Metrics for NCD users.
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetNCDMetrics]
	
AS
BEGIN
	
	SELECT 
		BM.Id AS Value, 
		BM.MetricName AS Name
	FROM 
		DCRM_ExecScoreBoardMetric BM WITH(NOLOCK)
	WHERE 
		BM.BusinessUnitId=2 --For NCD  
		AND BM.IsActive=1
	ORDER BY
		BM.MetricName 

END
