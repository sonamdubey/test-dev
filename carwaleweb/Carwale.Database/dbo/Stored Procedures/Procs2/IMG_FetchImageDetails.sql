IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IMG_FetchImageDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IMG_FetchImageDetails]
GO

	CREATE PROCEDURE [dbo].[IMG_FetchImageDetails] 
	@Id BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT CategoryId,ItemId,IsMaster,IsMain,AspectRatio,IsWaterMark 
	FROM IMG_Photos WITH(NOLOCK) 
	WHERE Id = @Id
END