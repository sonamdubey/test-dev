IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_IBSaveSubSource]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_IBSaveSubSource]
GO

	
CREATE PROCEDURE [dbo].[CRM_IBSaveSubSource]
@SubSource VARCHAR(100)

AS
BEGIN
	INSERT INTO CRM_IBSubSource (SubSource) VALUES (@SubSource)
END
