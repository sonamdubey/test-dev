IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetProactiveCountsByProfileId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetProactiveCountsByProfileId]
GO

	
-- =============================================
-- Author:		Akansha
-- Create date: 19.06.2014
-- Description:	Get Counts for root, bodytype and make for profileId
-- =============================================
CREATE PROCEDURE [dbo].[GetProactiveCountsByProfileId]-- 1,1,1,73456,173456,1
	@MakeId INT
	,@RootId INT
	,@BodyTypeId INT
	,@MinPrice NUMERIC(18, 0)
	,@MaxPrice NUMERIC(18, 0)
	,@CityId INT
AS
BEGIN
	DECLARE @RootCount INT = 0
		,@MakeCount INT = 0
		,@BodyTypeCount INT = 0;

	SELECT @RootCount = count(Inquiryid)
	FROM livelistings with (nolock)
	WHERE Price BETWEEN @MinPrice
			AND @MaxPrice
		AND RootId = @RootId AND CityId = @CityId

	SELECT @MakeCount = count(Inquiryid)
	FROM livelistings with (nolock)
	WHERE Price BETWEEN @MinPrice
			AND @MaxPrice
		AND MakeId = @MakeId AND CityId = @CityId

	SELECT @BodyTypeCount = count(Inquiryid)
	FROM livelistings LL with (nolock)
	INNER JOIN CarVersions CV ON LL.VersionId = CV.ID
	WHERE Price BETWEEN @MinPrice
			AND @MaxPrice
		AND CV.BodyStyleId = @BodyTypeId AND CityId = @CityId

	SELECT @RootCount as RootCount
		,@MakeCount as MakeCount
		,@BodyTypeCount as BodyTypeCount
END

