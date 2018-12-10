IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTrackingCodeForFC]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[GetTrackingCodeForFC]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 29 AUG 2013
-- Description:	get tracking code for featured version
-- SELECT [dbo].[GetTrackingCodeForFC] (2588)
-- =============================================
/*
	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       
*/
CREATE FUNCTION [dbo].[GetTrackingCodeForFC]
(
	@VersionID int
)
RETURNS nvarchar(max)
AS
BEGIN
    RETURN (SELECT ISNULL(ImpressionsTracker,'')+','+ISNULL(ImageTracker,'')+','+ISNULL(KeywordTracker,'')+','+ISNULL(OnRoadPriceTracker,'')
    FROM FeaturedCarsTrackingCode WITH (NOLOCK) WHERE VersionId = @VersionId)
END
