IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetBusinessUnitMetric]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetBusinessUnitMetric]
GO
	


-- =============================================
-- Author	:	Sachin Bharti(14th May 2015)
-- Description	:	Get opr users based on business unti
-- execute DCRM_GetBusinessUnitMetric 1
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetBusinessUnitMetric]
	@BusinessUnitId	INT
AS
	BEGIN
		SELECT
			BM.Id AS Value,
			BM.MetricName AS Text
		FROM
			DCRM_ExecScoreBoardMetric BM(NOLOCK)
		WHERE
			BM.BusinessUnitId = @BusinessUnitId
			AND BM.IsActive = 1
		ORDER BY
			BM.MetricName 
	END

