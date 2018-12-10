IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbsureTracker]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbsureTracker]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 23-04-2015
-- Description:	Absure Tracker
-- =============================================
CREATE PROCEDURE AbsureTracker
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	CREATE TABLE #TempTracker
	(
	  TrackerType VARCHAR(20),
	  Count INT
	)

    INSERT INTO #TempTracker(TrackerType,Count)
    SELECT 'Today' AS TrackerType,
		convert(int,sum(B.Amount) / 4000) as Warranty_sold
	FROM RVN_DealerPackageFeatures A WITH (NOLOCK)
		Inner JOIN DCRM_PaymentDetails B WITH (NOLOCK) ON A.TransactionId = B.TransactionId
		inner join Dealers C with (NOLOCK) on C.ID=A.DealerId
	WHERE A.PackageId in('72','73','74')
	and CONVERT(date, B.AddedOn) = CONVERT(date, GetDate())
	AND B.IsApproved = 1
	AND A.DealerId NOT IN (
	'4271'
	,'3838'
	)
	and C.Organization not like '%test%'

	INSERT INTO #TempTracker(TrackerType,Count)
    SELECT 'MTD' AS TrackerType,
    		convert(int,sum(B.Amount) / 4000) as Warranty_sold
	FROM RVN_DealerPackageFeatures A WITH (NOLOCK)
		Inner JOIN DCRM_PaymentDetails B WITH (NOLOCK) ON A.TransactionId = B.TransactionId
		inner join Dealers C with (NOLOCK) on C.ID=A.DealerId
	WHERE A.PackageId in('72','73','74')
	--and CONVERT(date, B.AddedOn) = '2015-04-21'
	AND B.IsApproved = 1
	AND A.DealerId NOT IN (
	'4271'
	,'3838'
	)
	and C.Organization not like '%test%'
	
	SELECT TrackerType,isnull(Count,0) Count  FROM #TempTracker

END
