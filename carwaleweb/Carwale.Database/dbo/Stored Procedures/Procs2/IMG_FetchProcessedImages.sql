IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IMG_FetchProcessedImages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IMG_FetchProcessedImages]
GO

	
CREATE PROCEDURE [dbo].[IMG_FetchProcessedImages]
	@ImageList VARCHAR(1000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SELECT IP.Id, IP.HostUrl AS HostUrl,IP.OriginalPath,IP.ItemId,IP.ProcessedId as ImageId
	FROM IMG_Photos AS IP WITH (NOLOCK)
	JOIN  dbo.fnSplitCSVValuesWithIdentity(@ImageList) AS FN ON FN.ListMember = IP.Id AND IsProcessed = 1
END