IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetNewsDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetNewsDetails]
GO

	
-- =========================================
-- Author:		Supriya
-- Create date: 10/01/2012
-- Description:	SP for fetching News details
-- =========================================

CREATE PROCEDURE [App].[GetNewsDetails] 
	@Id Integer
AS
BEGIN
	
	SET NOCOUNT ON;
	 SELECT  CB.Title, CB.DisplayDate, CB.AuthorName, CPC.Data, CB.MainImageSet,CEI.HostUrl,CEI.ImagePathLarge,CEI.Caption
		 FROM Con_EditCms_Basic CB 
         LEFT JOIN Con_EditCms_Pages CP ON CP.BasicId = CB.Id 
         LEFT JOIN Con_EditCms_PageContent CPC ON CPC.PageId = CP.Id 
         LEFT JOIN Con_EditCms_Images CEI ON CEI.BasicId = CB.Id And CEI.IsMainImage = 1 And CEI.IsActive = 1
         WHERE CB.Id = @Id AND CP.IsActive = 1
END

