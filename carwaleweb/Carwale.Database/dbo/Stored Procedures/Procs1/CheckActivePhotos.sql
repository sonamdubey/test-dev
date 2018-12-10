IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckActivePhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckActivePhotos]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 12-12-2012
-- Description:	Alert the Carphotos which are active and not approved
-- =============================================
CREATE PROCEDURE [dbo].[CheckActivePhotos]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	CREATE TABLE #tempCarPhotos(
	CarType varchar(20),
	PhotoCount int	
	)
	
    INSERT INTO #tempCarPhotos(CarType,PhotoCount)
    SELECT case IsDealer when 1 then 'Dealer' else 'Individual' end as PType,COUNT(*) as cnt
	FROM CarPhotos WITH (NOLOCK)
	WHERE IsApproved=0
	AND IsActive=1
	GROUP BY IsDealer
	
	SELECT CarType,PhotoCount
	FROM #tempCarPhotos
	WHERE PhotoCount>0
	
	DROP table #tempCarPhotos
END
