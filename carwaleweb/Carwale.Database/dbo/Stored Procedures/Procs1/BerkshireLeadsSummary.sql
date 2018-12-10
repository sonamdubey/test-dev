IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BerkshireLeadsSummary]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BerkshireLeadsSummary]
GO

	-- =============================================
-- Author:		<Prashant Vishe>
-- Create date: <08/08/2012>
-- Description:	<To show Berkshire Leads summary>
-- EXEC [dbo].[BerkshireLeadsSummary] '2012-08-03','2012-08-04'
-- =============================================
CREATE PROCEDURE [dbo].[BerkshireLeadsSummary]
	-- Add the parameters for the stored procedure here
	@frmDt  DateTime,
	@toDt DateTime
	
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @startDate DATETIME
    DECLARE @endDate DATETIME
	SET @StartDate = CONVERT(DATETIME,CONVERT(VARCHAR(10),@frmDt,120)+ ' 00:00:00')	
	SET @endDate = CONVERT(DATETIME,CONVERT(VARCHAR(10),@toDt,120)+ ' 23:59:59');

    -- Insert statements for procedure here
    
	SELECT  Convert(varchar(10), EntryDate, 111)as dt , 
			sum(CASE IsPushedToBerkshire  WHEN 1 THEN 1 WHEN 0 THEN 0  END) AS Pushed,
			sum(CASE IsPushedToBerkshire WHEN 0 THEN 1 WHEN 1 THEN 0 END) AS NonPushed,
			(sum(CASE IsPushedToBerkshire WHEN 0 THEN 1 WHEN 1 THEN 0 END) * 100 /COUNT(BerkshireLeadId)) [Loss] ,
			COUNT(BerkshireLeadId) AS Leads
	FROM dbo.BerkshireinsuranceLeads AS B
	    INNER JOIN BerkshireVehicleInfo AS BV WITH(NOLOCK) ON BV.MAKE_CODE=B.BerkshireMakeId
						AND BV.MODEL_CODE=B.BerkshireModelId AND BV.SUBTYPE_CODE=B.BerkshireVersionId			
		INNER JOIN BerkshireCityInfo Bc ON BC.ID = B.BerkshireCityId
	WHERE EntryDate BETWEEN @StartDate AND @endDate
	GROUP BY Convert(varchar(10), EntryDate, 111)
	ORDER BY Convert(varchar(10), EntryDate, 111)









END
