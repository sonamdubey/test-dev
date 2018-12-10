IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ComputeLLSortScore_17May2014]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ComputeLLSortScore_17May2014]
GO

	
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Modified by Manish on 23-04-2014 added with(nolock) keyword
-- =============================================
CREATE PROCEDURE [dbo].[ComputeLLSortScore_17May2014] 
	-- Add the parameters for the stored procedure here
	@ProfileId VARCHAR(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

	UPDATE ll
	SET ll.SortScore = (

	CASE WHEN PT.Priority in (1,2) THEN 
		CASE WHEN responses > 15 THEN 2 - (CAST(Responses AS FLOAT)/10000) + (CASE WHEN LL.Score < 0 THEN (LL.Score/100) ELSE LL.Score END / 10000)
		ELSE
			(CASE WHEN CONVERT(DATE, lastupdated) = CONVERT(DATE, GETDATE()) THEN  4  ELSE 3 END)
			
			+ ( CASE WHEN LL.Score < 0 THEN (LL.Score/100) ELSE LL.Score END ) - (CAST(Responses AS FLOAT)/10000000000)
			
		END
	ELSE 1 + LL.Score END )
	FROM livelistings LL WITH (NOLOCK)
	LEFT JOIN PackageTypePriority PT WITH (NOLOCK) ON LL.PackageType = PT.PackageType
	WHERE (@ProfileId IS NULL OR @ProfileId = LL.ProfileId)
END

